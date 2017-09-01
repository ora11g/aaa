using System;
using System.Collections.Generic;
using System.Text;

namespace YBInterface1.Support.B010003
{
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.section9.org/cms/referral/data")]
    public partial class RequestBody
    {

        private string d503_01Field;

        /// <remarks/>
        public string D503_01
        {
            get
            {
                return this.d503_01Field;
            }
            set
            {
                this.d503_01Field = value;
            }
        }
    }
}
