using System;
using System.Collections.Generic;
using System.Text;

namespace YBInterface1.Support.B010004
{
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.section9.org/cms/referral/data")]
    public partial class ResponseBody
    {

        private ResponseBodyBaseInfo baseInfoField;

        private ResponseBodyFeeInfo feeInfoField;

        /// <remarks/>
        public ResponseBodyBaseInfo BaseInfo
        {
            get
            {
                return this.baseInfoField;
            }
            set
            {
                this.baseInfoField = value;
            }
        }

        /// <remarks/>
        public ResponseBodyFeeInfo FeeInfo
        {
            get
            {
                return this.feeInfoField;
            }
            set
            {
                this.feeInfoField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.section9.org/cms/referral/data")]
    public partial class ResponseBodyBaseInfo
    {

        private string d501_02Field;

        private string d501_03Field;

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
        public string D501_03
        {
            get
            {
                return this.d501_03Field;
            }
            set
            {
                this.d501_03Field = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.section9.org/cms/referral/data")]
    public partial class ResponseBodyFeeInfo
    {

        private ResponseBodyFeeInfoComputeTypeFee computeTypeFeeField;

        /// <remarks/>
        public ResponseBodyFeeInfoComputeTypeFee ComputeTypeFee
        {
            get
            {
                return this.computeTypeFeeField;
            }
            set
            {
                this.computeTypeFeeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.section9.org/cms/referral/data")]
    public partial class ResponseBodyFeeInfoComputeTypeFee
    {

        private string d503_54Field;

        private string d503_64Field;

        private string d503_56Field;

        private string d503_02_2Field;

        private string d503_62Field;

        private string d503_09Field;

        private string d503_63Field;

        /// <remarks/>
        public string D503_54
        {
            get
            {
                return this.d503_54Field;
            }
            set
            {
                this.d503_54Field = value;
            }
        }

        /// <remarks/>
        public string D503_64
        {
            get
            {
                return this.d503_64Field;
            }
            set
            {
                this.d503_64Field = value;
            }
        }

        /// <remarks/>
        public string D503_56
        {
            get
            {
                return this.d503_56Field;
            }
            set
            {
                this.d503_56Field = value;
            }
        }

        /// <remarks/>
        public string D503_02_2
        {
            get
            {
                return this.d503_02_2Field;
            }
            set
            {
                this.d503_02_2Field = value;
            }
        }

        /// <remarks/>
        public string D503_62
        {
            get
            {
                return this.d503_62Field;
            }
            set
            {
                this.d503_62Field = value;
            }
        }

        /// <remarks/>
        public string D503_09
        {
            get
            {
                return this.d503_09Field;
            }
            set
            {
                this.d503_09Field = value;
            }
        }

        /// <remarks/>
        public string D503_63
        {
            get
            {
                return this.d503_63Field;
            }
            set
            {
                this.d503_63Field = value;
            }
        }
    }

}
