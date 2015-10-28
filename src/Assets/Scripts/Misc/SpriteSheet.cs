using UnityEngine;

namespace Assets.Scripts.Misc
{
    public class SpriteSheet : MonoBehaviour
    {
        public Texture2D Sheet;
        public int TileWidth;
        public int TileHeight;

        private Texture2D[] _textures;

        public Texture GetTexture(int index)
        {
            return _textures[index];
        }

        public void GenerateSpriteSheet()
        {
            var width = Sheet.width / TileWidth;
            var height = Sheet.height / TileHeight;

            _textures = new Texture2D[width * height];

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var textureColor = Sheet.GetPixels(x * TileWidth, y * TileHeight, TileWidth, TileHeight);
                    var newTexture = new Texture2D(TileWidth, TileHeight);
                    newTexture.SetPixels(0, 0, TileWidth, TileHeight, textureColor);

                    // Unity reads textures from bottom to top instead of top to bottom
                    _textures[x + (height - 1 - y) * width] = newTexture;
                }
            }
        }
    }
}