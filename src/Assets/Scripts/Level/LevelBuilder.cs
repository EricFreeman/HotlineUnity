﻿using Assets.Scripts.Util;
using Dungeon.Generator;
using UnityEngine;

namespace Assets.Scripts.Level
{
    public class LevelBuilder : MonoBehaviour
    {
        public GameObject NavigationLayer;
        public GameObject Wall;

        public GameObject Enemy;

        public float TileSize = 1;

        void Start()
        {
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

            UnityEditor.NavMeshBuilder.BuildNavMesh();
        }
    }
}