using System;
using System.Collections.Generic;
using System.Text;

namespace YBInterface1.Support.B010005
{
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.section9.org/cms/referral/data")]
    public partial class ResponseBody
    {

        private string d503_41Field;

        /// <remarks/>
        public string D503_41
        {
            get
            {
                return this.d503_41Field;
            }
            set
            {
                this.d503_41Field = value;
            }
        }
    }
}