using System;
using System.Collections.Generic;
using System.Text;

namespace YBInterface1.Support.B010007
{
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.section9.org/cms/referral/data")]
    public partial class ResponseBody
    {

        private string infoField;

        /// <remarks/>
        public string Info
        {
            get
            {
                return this.infoField;
            }
            set
            {
                this.infoField = value;
            }
        }
    }
}