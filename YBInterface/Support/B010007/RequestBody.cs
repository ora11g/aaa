using System;
using System.Collections.Generic;
using System.Text;

namespace YBInterface1.Support.B010007
{
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.section9.org/cms/referral/data")]
    public partial class RequestBody
    {
        private string d501_11Field;

        /// <remarks/>
        public string D501_11
        {
            get
            {
                return this.d501_11Field;
            }
            set
            {
                this.d501_11Field = value;
            }
        }
    }

}
