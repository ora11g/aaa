using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace YBInterface1.Support.C010005
{
    public partial class responseBody
    {
        [XmlElement(ElementName = "item")]
        public responseItem[] item { get; set; }
    }

    public partial class responseItem
    {
        public string D501_02 { get; set; }

        public string D501_84 { get; set; }

        public string D501_16 { get; set; }

        public string D501_46 { get; set; }
    }

}
