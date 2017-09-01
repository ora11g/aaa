using System;
using System.Collections.Generic;
using System.Text;

namespace YBInterface1.Support.B010005
{
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.section9.org/cms/referral/data")]
    public partial class RequestBody
    {

        private string d503_01Field;

        private string d501_02Field;

        private string d503_18Field;

        private string d503_78Field;

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
        public string D503_18
        {
            get
            {
                return this.d503_18Field;
            }
            set
            {
                this.d503_18Field = value;
            }
        }

        /// <remarks/>
        public string D503_78
        {
            get
            {
                return this.d503_78Field;
            }
            set
            {
                this.d503_78Field = value;
            }
        }
    }
}
