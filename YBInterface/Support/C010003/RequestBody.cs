using System;
using System.Collections.Generic;
using System.Text;

namespace YBInterface1.Support.C010003
{
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.section9.org/cms/referral/data")]
    public partial class RequestBody
    {

        private string d503_34Field;

        private string d503_35Field;

        private string d503_36Field;

        private string d503_15Field;

        private string d501_11Field;

        /// <remarks/>
        public string D503_34
        {
            get
            {
                return this.d503_34Field;
            }
            set
            {
                this.d503_34Field = value;
            }
        }

        /// <remarks/>
        public string D503_35
        {
            get
            {
                return this.d503_35Field;
            }
            set
            {
                this.d503_35Field = value;
            }
        }

        /// <remarks/>
        public string D503_36
        {
            get
            {
                return this.d503_36Field;
            }
            set
            {
                this.d503_36Field = value;
            }
        }

        /// <remarks/>
        public string D503_15
        {
            get
            {
                return this.d503_15Field;
            }
            set
            {
                this.d503_15Field = value;
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
