using Assets.Scripts.Level.Models;
using Assets.Scripts.Misc;
using Assets.Scripts.Util;
using Dungeon.Generator;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Level
{
    public class LevelBuilder : MonoBehaviour
    {
        public GameObject NavigationLayer;
        public GameObject Wall;
        public GameObject Floor;

        public GameObject Enemy;

        public float TileSize = 1;

        void Start()
        {
            BuildLevel("TestMap1");
            UnityEditor.NavMeshBuilder.BuildNavMesh();
        }

        private void BuildLevel(string levelName)
        {
            var map = XmlManager<Map>.Load("Assets/Resources/Levels/" + levelName + ".tmx");

            // todo: load in all spritesheets here
            var tileSheet = map.TileSet[0].Image.Source.Substring(3, map.TileSet[0].Image.Source.Length - 7);
            var spriteSheet = GenerateSpriteSheet(tileSheet);

            var width = map.Width;
            var height = map.Height;

            foreach (var layer in map.Layer)
            {
                for (var z = 0; z < height; z++)
                {
                    for (var x = 0; x < width; x++)
                    {
                        var context = new TileContext
                        {
                            Tile = layer.Data[z * width + x],
                            TilePosition = new Vector3(x, 0, -z) * TileSize,
                            SpriteSheet = spriteSheet,
                        };

                        if (layer.Name == "Ground")
                        {
                            CreateGround(context);
                        }
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

        private SpriteSheet GenerateSpriteSheet(string tileSheet)
        {
            var spriteSheet = GetComponent<SpriteSheet>();
            spriteSheet.Sheet = Resources.Load<Texture2D>(tileSheet);
            spriteSheet.TileWidth = 32;
            spriteSheet.TileHeight = 32;
            spriteSheet.GenerateSpriteSheet();

            return spriteSheet;
        }

        private void BuildProceduralDungeon()
        {
            var spriteSheet = GenerateSpriteSheet("Images/Sheets/Sheet1");
            var map = Generator.Generate(MapSize.Small, 0);

            for (var z = 0; z < map.Height; z++)
            {
                for (var x = 0; x < map.Width; x++)
                {
                    var mapTile = map[x, z];
                    var tilePosition = new Vector3(x, 0, z) * TileSize;

                    if (mapTile.MaterialType == MaterialType.Wall)
                    {
                        var wall = Instantiate(Wall);
                        wall.transform.position = tilePosition;
                        wall.GetComponent<Renderer>().material.SetTexture("_MainTex", spriteSheet.GetTexture(0));
                    }

                    if (mapTile.Attributes.HasFlag(AttributeType.Entry))
                    {
                        var player = GameObject.Find("Player");
                        player.transform.position = tilePosition + new Vector3(0, .5f, 0);
                    }

                    var random = Random.Range(0, 100);
                    if (random == 0)
                    {
                        var enemy = Instantiate(Enemy);
                        enemy.transform.position = tilePosition;
                    }
                }
            }
        }
    }
}