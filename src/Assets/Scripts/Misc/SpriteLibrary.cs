using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Level.Models;
using UnityEngine;

namespace Assets.Scripts.Misc
{
    public class SpriteLibrary
    {
        public List<SpriteSheet> SpriteSheets = new List<SpriteSheet>();
        private TileSet[] _tileSets = new TileSet[0];

        public void Generate(TileSet[] tileSets)
        {
            _tileSets = tileSets;

            foreach (var tileSet in tileSets)
            {
                var spriteSheet = new SpriteSheet
                {
                    Sheet = Resources.Load<Texture2D>(tileSet.Image.Source.Substring(3, tileSet.Image.Source.Length - 7)),
                    TileWidth = tileSet.TileWidth,
                    TileHeight = tileSet.TileHeight,
                    FirstGid = tileSet.FirstGid
                };
                spriteSheet.GenerateSpriteSheet();

                SpriteSheets.Add(spriteSheet);
            }
        }

        public TileSet GetTileSet(TileContext context)
        {
            TileSet tileSheet = null;
            foreach (var sheet in _tileSets)
            {
                if (sheet.FirstGid > context.Tile.Gid)
                {
                    break;
                }

                tileSheet = sheet;
            }

            return tileSheet ?? _tileSets.Last();
        }

        public Texture GetTexture(int index)
        {
            SpriteSheet tileSheet = null;
            foreach (var sheet in SpriteSheets)
            {
                if (sheet.FirstGid > index)
                {
                    break;
                }

                tileSheet = sheet;
            }

            tileSheet = tileSheet ?? SpriteSheets.Last();

            return tileSheet.GetTexture(index - tileSheet.FirstGid);
        }
    }
}