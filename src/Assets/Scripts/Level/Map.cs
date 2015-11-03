using System.Xml.Serialization;

namespace Assets.Scripts.Level
{
    [XmlRoot(ElementName = "map")]
    public class Map
    {
        [XmlElement(ElementName = "tileset")]
        public TileSet TileSet;

        [XmlElement(ElementName = "layer")]
        public Layer[] Layer;

        [XmlAttribute(AttributeName = "width")]
        public int Width;

        [XmlAttribute(AttributeName = "height")]
        public int Height;
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
        [XmlElement(ElementName = "image")]
        public Image Image;
    }

    public class Image
    { 
        [XmlAttribute(AttributeName = "source")]
        public string Source;
    }
}