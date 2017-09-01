using System;
using System.Collections.Generic;
using System.Text;

namespace YBInterface1.Support.C010004
{    
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.section9.org/cms/referral/data")]
    public partial class RequestBody
    {

        private string d501_11Field;

        private string d603_02Field;

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

        /// <remarks/>
        public string D603_02
        {
            get
            {
                return this.d603_02Field;
            }
            set
            {
                this.d603_02Field = value;
            }
        }
    }
}
