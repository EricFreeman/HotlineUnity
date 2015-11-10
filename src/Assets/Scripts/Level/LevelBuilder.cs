using System;
using System.Collections.Generic;
using System.Linq;
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

        public Dictionary<string, Action<TileContext>> BuilderDefinitions;

        void Start()
        {
            BuilderDefinitions = new Dictionary<string, Action<TileContext>>
            {
                { "Ground", CreateGround },
                { "Walls", CreateWalls }
            };

            BuildLevel("TestMap1");
            UnityEditor.NavMeshBuilder.BuildNavMesh();
        }

        private void BuildLevel(string levelName)
        {
            var map = XmlManager<Map>.Load("Assets/Resources/Levels/" + levelName + ".tmx");

            // todo: load in all spritesheets here
            var tileSheet = map.TileSets[0].Image.Source.Substring(3, map.TileSets[0].Image.Source.Length - 7);
            var spriteSheet = GenerateSpriteSheet(tileSheet);

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
                            TilePosition = new Vector3(x, 0, -z),
                            SpriteSheet = spriteSheet,
                            TileSheets = map.TileSets
                        };

                        if (BuilderDefinitions.ContainsKey(layer.Name))
                        {
                            BuilderDefinitions[layer.Name](tileContext);
                        }
                    }
                }
            }

            AddWallCaps(map);
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
                    if (x % width > 0)
                    {
                        //var leftTile = wallLayer.Data[currentIndex - 1];
                    }
                }
            }
        }

        private void CreateGround(TileContext context)
        {
            var floor = Instantiate(Floor);
            floor.transform.position = context.TilePosition;
            floor.GetComponent<Renderer>().material.SetTexture("_MainTex", context.SpriteSheet.GetTexture(context.Tile.Gid - 1));
        }

        private bool _isMovingLeft;
        private bool _isMovingDown;

        private void CreateWalls(TileContext context)
        {
            TileSet wallSheet = null;
            foreach (var sheet in context.TileSheets)
            {
                if (sheet.FirstGid <= context.Tile.Gid)
                {
                    break;
                }

                wallSheet = sheet;
            }

            if (wallSheet == null)
            {
                wallSheet = context.TileSheets.Last();
            }

            var correctedGid = context.Tile.Gid - wallSheet.FirstGid;
            var correctedWall = wallSheet.Tile.FirstOrDefault(x => x.Id == correctedGid);
            if (correctedWall != null)
            {
                var direction = correctedWall.Properties.FirstOrDefault(x => x.Name == "WallDirection");
                if (direction != null)
                {
                    GameObject wall = null;
                    GameObject cap = null;

                    switch (direction.Value)
                    {
                        case WallDirection.Left:
                            wall = Instantiate(LeftWall);
                            if (_isMovingLeft)
                            {
                                cap = Instantiate(WallCap);
                                cap.transform.position = context.TilePosition + new Vector3(-.45f, 0, .45f);
                            }

                            _isMovingLeft = false;
                            _isMovingDown = true;
                            break;
                        case WallDirection.Top:
                            wall = Instantiate(TopWall);

                            if (_isMovingDown)
                            {
                                cap = Instantiate(WallCap);
                                cap.transform.position = context.TilePosition + new Vector3(-.45f, 0, .45f);
                            }

                            _isMovingLeft = true;
                            _isMovingDown = false;
                            break;
                        case WallDirection.Corner:
                            wall = Instantiate(CornerWall);

                            cap = Instantiate(WallCap);
                            cap.transform.position = context.TilePosition + new Vector3(-.45f, 0, .45f);

                            _isMovingLeft = false;
                            _isMovingDown = false;
                            break;
                    }

                    if (wall != null)
                    {
                        wall.transform.position = context.TilePosition;
                    }
                }
            }
        }

        private SpriteSheet GenerateSpriteSheet(string tileSheet)
        {
            var spriteSheet = GetComponent<SpriteSheet>();
            spriteSheet.Sheet = Resources.Load<Texture2D>(tileSheet);
            spriteSheet.TileWidth = 32;
            spriteSheet.TileHeight = 32;
            spriteSheet.GenerateSpriteSheet();

            return spriteSheet;
        }
    }
}