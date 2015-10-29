using System.Xml.Serialization;

namespace Assets.Scripts.Level
{
    [XmlRoot(ElementName = "map")]
    public class Map
    {
        [XmlElement(ElementName = "tileset")]
        public TileSet TileSet;

        [XmlElement(ElementName = "layer")]
        public Layer Layer;

        [XmlAttribute(AttributeName = "version")]
        public string Version;
    }

    public class Layer
    {
    }

    public class TileSet
    {
    }
}