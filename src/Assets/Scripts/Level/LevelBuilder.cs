using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Assets.Scripts.Level.Models;
using Assets.Scripts.Misc;
using Assets.Scripts.Util;
using UnityEngine;

namespace Assets.Scripts.Level
{
    public class LevelBuilder : MonoBehaviour
    {
        public GameObject NavigationLayer;
        public GameObject LeftWall;
        public GameObject TopWall;
        public GameObject CornerWall;
        public GameObject WallCap;
        public GameObject Floor;
        public GameObject Enemy;
        public GameObject SimpleObject;

        public GameObject Player;

        public Dictionary<string, Action<TileContext>> BuilderDefinitions;

        private SpriteLibrary _spriteLibrary = new SpriteLibrary();

        void Start()
        {
            BuilderDefinitions = new Dictionary<string, Action<TileContext>>
            {
                { "Ground", CreateGround },
                { "Walls", CreateWalls },
                { "Spawns", CreateSpawns }
            };

            Player = GameObject.Find("Player");

            BuildLevel("TestMap1");
            UnityEditor.NavMeshBuilder.BuildNavMesh();
        }

        private void BuildLevel(string levelName)
        {
            var map = XmlManager<Map>.Load("Assets/Resources/Levels/" + levelName + ".tmx");
            _spriteLibrary.Generate(map.TileSets);

            var width = map.Width;
            var height = map.Height;

            foreach (var layer in map.Layer)
            {
                for (var z = 0; z < height; z++)
                {
                    for (var x = 0; x < width; x++)
                    {
                        var tileContext = new TileContext
                        {
                            Tile = layer.Data[z * height + x],
                            TilePosition = new Vector3(x, 0, -z)
                        };

                        if (BuilderDefinitions.ContainsKey(layer.Name))
                        {
                            BuilderDefinitions[layer.Name](tileContext);
                        }
                    }
                }
            }

            for (var i = 0; i < map.ObjectGroup.Length; i++)
            {
                BuildObjectGroup(map.ObjectGroup[i], i);
            }

            AddWallCaps(map);
        }

        private void BuildObjectGroup(ObjectGroup objectGroup, int layer)
        {
            if(objectGroup.Object == null) return;

            foreach (var obj in objectGroup.Object)
            {
                var gameObj = Instantiate(SimpleObject);
                gameObj.transform.position = new Vector3(obj.X / 32, .1f, -obj.Y / 32);
                var spriteRenderer = gameObj.GetComponentInChildren<SpriteRenderer>();
                var texture = _spriteLibrary.GetTexture(obj.Gid);
                var sprite = Sprite.Create(texture as Texture2D, new Rect(0, 0, texture.width, texture.height), new Vector2(.0f, 1f));
                spriteRenderer.sprite = sprite;

                var scale = Math.Max(texture.width, texture.height)/16f;
                spriteRenderer.transform.localScale = new Vector3(3, 3, 3);

                gameObj.transform.Translate(-.5f, 0, .5f);
                gameObj.transform.RotateAround(gameObj.transform.position, Vector3.up, obj.Rotation);

                spriteRenderer.gameObject.AddComponent<BoxCollider>();
            }
        }

        private void AddWallCaps(Map map)
        {
            var wallLayer = map.Layer.FirstOrDefault(x => x.Name == "Walls");
            if (wallLayer == null)
            {
                return;
            }

            var width = map.Width;
            var height = map.Height;

            var shit = wallLayer.Data.Select(x => x.Gid).Distinct().Where(x => x != 0).OrderBy(x => x).ToList();

            for (var z = 0; z < height; z++)
            {
                for (var x = 0; x < width; x++)
                {
                    var currentIndex = z * height + x;
                    var currentTile = wallLayer.Data[currentIndex];
                    if (currentTile.Gid > 0)
                    {
                        var currentWall = (WallDirection)shit.IndexOf(currentTile.Gid);

                        switch (currentWall)
                        {
                            case WallDirection.Left:
                                if (z > 0)
                                {
                                    var top = wallLayer.Data[currentIndex - width];
                                    if ((int)currentWall != shit.IndexOf(top.Gid) && shit.IndexOf(top.Gid) != 2)
                                    {
                                        var cap = Instantiate(WallCap);
                                        cap.transform.position = new Vector3(x - .45f, 0, -z + .45f);
                                    }
                                }
                                if (z < height - 1)
                                {
                                    var bottom = wallLayer.Data[currentIndex + width];
                                    if ((int)currentWall != shit.IndexOf(bottom.Gid) && shit.IndexOf(bottom.Gid) != 2)
                                    {
                                        var cap = Instantiate(WallCap);
                                        cap.transform.position = new Vector3(x - .45f, 0, -z - .55f);
                                    }
                                }
                                break;
                            case WallDirection.Top:
                                if (x > 0)
                                {
                                    var left = wallLayer.Data[currentIndex - 1];
                                    if ((int)currentWall != shit.IndexOf(left.Gid) && shit.IndexOf(left.Gid) != 2)
                                    {
                                        var cap = Instantiate(WallCap);
                                        cap.transform.position = new Vector3(x - .45f, 0, -z + .45f);
                                    }
                                }
                                if (x < width)
                                {
                                    var right = wallLayer.Data[currentIndex + 1];
                                    if ((int)currentWall != shit.IndexOf(right.Gid) && shit.IndexOf(right.Gid) != 2)
                                    {
                                        var cap = Instantiate(WallCap);
                                        cap.transform.position = new Vector3(x + .55f, 0, -z + .45f);
                                    }
                                }
                                break;
                            case WallDirection.Corner:
                                var defaultCap = Instantiate(WallCap);
                                defaultCap.transform.position = new Vector3(x - .45f, 0, -z + .45f);

                                if (x < width)
                                {
                                    var right = wallLayer.Data[currentIndex + 1];
                                    if (shit.IndexOf(right.Gid) < 0)
                                    {
                                        var cap = Instantiate(WallCap);
                                        cap.transform.position = new Vector3(x + .55f, 0, -z + .45f);
                                    }
                                }
                                if (z < height - 1)
                                {
                                    var bottom = wallLayer.Data[currentIndex + width];
                                    if (shit.IndexOf(bottom.Gid) < 0)
                                    {
                                        var cap = Instantiate(WallCap);
                                        cap.transform.position = new Vector3(x - .45f, 0, -z - .55f);
                                    }
                                }
                                break;
                        }
                    }
                }
            }
        }

        private void CreateGround(TileContext context)
        {
            if (context.Tile.Gid <= 0) return;

            var floor = Instantiate(Floor);
            floor.transform.position = context.TilePosition;
            floor.GetComponentInChildren<Renderer>().material.SetTexture("_MainTex", _spriteLibrary.GetTexture(context.Tile.Gid));
        }

        private void CreateWalls(TileContext context)
        {
            if (context.Tile.Gid == 0) { 
                return;
            }

            TileSet wallSheet = _spriteLibrary.GetTileSet(context);

            var correctedGid = context.Tile.Gid - wallSheet.FirstGid;
            var correctedWall = wallSheet.Tile.FirstOrDefault(x => x.Id == correctedGid);
            if (correctedWall != null)
            {
                var direction = correctedWall.Properties.FirstOrDefault(x => x.Name == "WallDirection");
                if (direction != null)
                { 
                    GameObject wall = null;

                    switch (direction.Value)
                    {
                        case WallDirection.Left:
                            wall = Instantiate(LeftWall);
                            break;
                        case WallDirection.Top:
                            wall = Instantiate(TopWall);
                            break;
                        case WallDirection.Corner:
                            wall = Instantiate(CornerWall);
                            break;
                    }

                    if (wall != null)
                    {
                        wall.transform.position = context.TilePosition;
                    }
                }
            }
        }

        private void CreateSpawns(TileContext context)
        {
            if (context.Tile.Gid == 0)
            {
                return;
            }

            TileSet spawnSheet = _spriteLibrary.GetTileSet(context);
            var actualId = context.Tile.Gid - spawnSheet.FirstGid;

            switch (actualId)
            {
                case 0:
                    var enemy = Instantiate(Enemy);
                    enemy.transform.position = context.TilePosition;
                    break;
                case 1:
                    Player.transform.position = context.TilePosition;
                    break;
            }
        } 
    }
}