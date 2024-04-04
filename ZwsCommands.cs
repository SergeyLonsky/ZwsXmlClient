using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ZwsXmlClient
{
    [XmlRoot("zulu-server", Namespace = "", IsNullable = false)]
    public class ZuluServerRoot
    {
        [XmlAttribute("service")]
        public string service { get; set; }

        [XmlAttribute("version")]
        public string version { get; set; }

        [XmlElement("Command")]
        public Command command { get; set; }

        public ZuluServerRoot()
        {
            service = "zws";
            version = "1.0.0";
            command = new Command();
        }
    }

    public class Command
    {
        [XmlIgnore()]
        ICommand CommandProperty { get; set; }

        [XmlElement("LayerDeleteElement", typeof(LayerDeleteElementCommand))]
        [XmlElement("LayerAddPolyline", typeof(LayerAddPolylineCommand))]
        public object CurrentCommand
        {
            get => CommandProperty;
            set => CommandProperty = (ICommand)value;
        }
    }

    public interface ICommand
    {
    }

    public class LayerDeleteElementCommand : ICommand
    {

        [XmlElement("Layer")]
        public string Layer { get; set; }

        [XmlElement("ElemID")]
        public string ElemID { get; set; }

        public LayerDeleteElementCommand() { }

        public LayerDeleteElementCommand(string _layer, string _elemid)
        {
            Layer = _layer;
            ElemID = _elemid;
        }
    }

    public class LayerAddPolylineCommand : ICommand
    {

        [XmlElement("Layer")]
        public string Layer { get; set; }

        [XmlElement("TypeID")]
        public string TypeID { get; set; }

        [XmlElement("ModeNum")]
        public string ModeNum { get; set; }

        [XmlElement("CRS")]
        public string CRS { get; set; }

        [XmlElement("coordinates")]
        public string coordinates { get; set; }

        [XmlElement("SnapTo")]
        public string SnapTo { get; set; }

        public LayerAddPolylineCommand() { }

        public LayerAddPolylineCommand(string _layer, string _typeid, string _modenum, string _coordinates, string _crs = "EPSG:4326")
        {
            Layer = _layer;
            TypeID = _typeid;
            ModeNum = _modenum;
            CRS = _crs;
            coordinates = _coordinates;
            SnapTo = string.Empty;
        }
    }
}
