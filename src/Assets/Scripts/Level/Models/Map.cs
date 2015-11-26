using System.Xml.Serialization;

namespace Assets.Scripts.Level.Models
{
    [XmlRoot(ElementName = "map")]
    public class Map
    {
        [XmlElement(ElementName = "tileset")]
        public TileSet[] TileSets;

        [XmlElement(ElementName = "layer")]
        public Layer[] Layer;

        [XmlElement(ElementName = "objectgroup")]
        public ObjectGroup[] ObjectGroup;

        [XmlAttribute(AttributeName = "width")]
        public int Width;

        [XmlAttribute(AttributeName = "height")]
        public int Height;
    }

    public class ObjectGroup
    {
        [XmlElement(ElementName = "object")]
        public MapObject[] Object;
    }

    public class MapObject
    {
        [XmlAttribute(AttributeName = "id")]
        public int Id;

        [XmlAttribute(AttributeName = "gid")]
        public int Gid;

        [XmlAttribute(AttributeName = "x")]
        public float X;

        [XmlAttribute(AttributeName = "y")]
        public float Y;

        [XmlAttribute(AttributeName = "width")]
        public int Width;

        [XmlAttribute(AttributeName = "height")]
        public int Height;

        [XmlAttribute(AttributeName = "rotation")]
        public float Rotation;
    }

    public class Layer
    {
        [XmlArray(ElementName = "data")]
        [XmlArrayItem(ElementName = "tile")]
        public Tile[] Data;

        [XmlAttribute(AttributeName = "name")]
        public string Name;

        [XmlAttribute(AttributeName = "width")]
        public int Width;

        [XmlAttribute(AttributeName = "height")]
        public int Height;
    }

    public class Tile
    {
        [XmlAttribute(AttributeName = "gid")]
        public int Gid;
    }

    public class TileSet
    {
        [XmlAttribute(AttributeName = "firstgid")]
        public int FirstGid;

        [XmlAttribute(AttributeName = "tilewidth")]
        public int TileWidth;

        [XmlAttribute(AttributeName = "tileheight")]
        public int TileHeight;

        [XmlElement(ElementName = "image")]
        public Image Image;

        [XmlElement(ElementName = "tile")]
        public TileDefinition[] Tile;
    }

    public class TileDefinition
    {
        [XmlAttribute(AttributeName = "id")]
        public int Id;

        [XmlArray(ElementName = "properties")]
        [XmlArrayItem(ElementName = "property")]
        public Property[] Properties;
    }

    public class Property
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name;

        [XmlAttribute(AttributeName = "value")]
        public WallDirection Value;
    }

    public enum WallDirection
    {
        Left,
        Top,
        Corner
    }

    public class Image
    { 
        [XmlAttribute(AttributeName = "source")]
        public string Source;
    }
}