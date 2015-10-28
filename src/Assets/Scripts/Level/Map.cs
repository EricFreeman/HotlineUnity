using Assets.Scripts.Misc;
using UnityEngine;

namespace Assets.Scripts.Level
{
    public class Map : MonoBehaviour
    {
        void Start()
        {
            var spriteSheet = GetComponent<SpriteSheet>();
            spriteSheet.Sheet = Resources.Load<Texture2D>("Images/Sheets/Sheet1");
            spriteSheet.TileWidth = 32;
            spriteSheet.TileHeight = 32;
            spriteSheet.GenerateSpriteSheet();

            GetComponentsInChildren<Transform>()[1].GetComponent<Renderer>().material.SetTexture("_MainTex", spriteSheet.GetTexture(0));
        }
    }
}