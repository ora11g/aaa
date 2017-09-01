using System;
using System.Collections.Generic;
using System.Text;

namespace YBInterface1.Support.B010006
{
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.section9.org/cms/referral/data")]
    public partial class RequestBody
    {

        private string d501_09Field;

        private string d501_02Field;

        private string d503_31Field;

        private string d501_11Field;

        /// <remarks/>
        public string D501_09
        {
            get
            {
                return this.d501_09Field;
            }
            set
            {
                this.d501_09Field = value;
            }
        }

        /// <remarks/>
        public string D501_02
        {
            get
            {
                return this.d501_02Field;
            }
            set
            {
                this.d501_02Field = value;
            }
        }

        /// <remarks/>
        public string D503_31
        {
            get
            {
                return this.d503_31Field;
            }
            set
            {
                this.d503_31Field = value;
            }
        }

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
