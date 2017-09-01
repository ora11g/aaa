using System;
using System.Collections.Generic;
using System.Text;

namespace YBInterface1.Support.C010002
{
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.section9.org/cms/referral/data")]
    public partial class ResponseBody
    {

        private string d503_01Field;

        private ResponseBodyDetails detailsField;

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
        public ResponseBodyDetails Details
        {
            get
            {
                return this.detailsField;
            }
            set
            {
                this.detailsField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.section9.org/cms/referral/data")]
    public partial class ResponseBodyDetails
    {

        private ResponseBodyDetailsD502_03_01 d502_03_01Field;

        private ResponseBodyDetailsItem[] d502_03_02Field;

        private ResponseBodyDetailsItem1[] d502_03_03Field;

        /// <remarks/>
        public ResponseBodyDetailsD502_03_01 D502_03_01
        {
            get
            {
                return this.d502_03_01Field;
            }
            set
            {
                this.d502_03_01Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Item", IsNullable = false)]
        public ResponseBodyDetailsItem[] D502_03_02
        {
            get
            {
                return this.d502_03_02Field;
            }
            set
            {
                this.d502_03_02Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Item", IsNullable = false)]
        public ResponseBodyDetailsItem1[] D502_03_03
        {
            get
            {
                return this.d502_03_03Field;
            }
            set
            {
                this.d502_03_03Field = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.section9.org/cms/referral/data")]
    public partial class ResponseBodyDetailsD502_03_01
    {

        private ResponseBodyDetailsD502_03_01Item[] d502_31_01Field;

        private ResponseBodyDetailsD502_03_01Item1[] d502_31_02Field;

        private ResponseBodyDetailsD502_03_01Item2[] d502_31_03Field;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Item", IsNullable = false)]
        public ResponseBodyDetailsD502_03_01Item[] D502_31_01
        {
            get
            {
                return this.d502_31_01Field;
            }
            set
            {
                this.d502_31_01Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Item", IsNullable = false)]
        public ResponseBodyDetailsD502_03_01Item1[] D502_31_02
        {
            get
            {
                return this.d502_31_02Field;
            }
            set
            {
                this.d502_31_02Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Item", IsNullable = false)]
        public ResponseBodyDetailsD502_03_01Item2[] D502_31_03
        {
            get
            {
                return this.d502_31_03Field;
            }
            set
            {
                this.d502_31_03Field = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.section9.org/cms/referral/data")]
    public partial class ResponseBodyDetailsD502_03_01Item
    {

        private string d502_04Field;

        private string d502_05Field;

        private string d502_07Field;

        private string d502_06Field;

        private string d502_08Field;

        private string d502_09Field;

        private string d502_11Field;

        private string d502_39Field;

        private string d502_40Field;

        private string d502_33Field;

        private string d502_34Field;

        private string d506_93Field;

        private string d502_35Field;

        private string d502_36Field;

        private string d503_66Field;

        /// <remarks/>
        public string D502_04
        {
            get
            {
                return this.d502_04Field;
            }
            set
            {
                this.d502_04Field = value;
            }
        }

        /// <remarks/>
        public string D502_05
        {
            get
            {
                return this.d502_05Field;
            }
            set
            {
                this.d502_05Field = value;
            }
        }

        /// <remarks/>
        public string D502_07
        {
            get
            {
                return this.d502_07Field;
            }
            set
            {
                this.d502_07Field = value;
            }
        }

        /// <remarks/>
        public string D502_06
        {
            get
            {
                return this.d502_06Field;
            }
            set
            {
                this.d502_06Field = value;
            }
        }

        /// <remarks/>
        public string D502_08
        {
            get
            {
                return this.d502_08Field;
            }
            set
            {
                this.d502_08Field = value;
            }
        }

        /// <remarks/>
        public string D502_09
        {
            get
            {
                return this.d502_09Field;
            }
            set
            {
                this.d502_09Field = value;
            }
        }

        /// <remarks/>
        public string D502_11
        {
            get
            {
                return this.d502_11Field;
            }
            set
            {
                this.d502_11Field = value;
            }
        }

        /// <remarks/>
        public string D502_39
        {
            get
            {
                return this.d502_39Field;
            }
            set
            {
                this.d502_39Field = value;
            }
        }

        /// <remarks/>
        public string D502_40
        {
            get
            {
                return this.d502_40Field;
            }
            set
            {
                this.d502_40Field = value;
            }
        }

        /// <remarks/>
        public string D502_33
        {
            get
            {
                return this.d502_33Field;
            }
            set
            {
                this.d502_33Field = value;
            }
        }

        /// <remarks/>
        public string D502_34
        {
            get
            {
                return this.d502_34Field;
            }
            set
            {
                this.d502_34Field = value;
            }
        }

        /// <remarks/>
        public string D506_93
        {
            get
            {
                return this.d506_93Field;
            }
            set
            {
                this.d506_93Field = value;
            }
        }

        /// <remarks/>
        public string D502_35
        {
            get
            {
                return this.d502_35Field;
            }
            set
            {
                this.d502_35Field = value;
            }
        }

        /// <remarks/>
        public string D502_36
        {
            get
            {
                return this.d502_36Field;
            }
            set
            {
                this.d502_36Field = value;
            }
        }

        /// <remarks/>
        public string D503_66
        {
            get
            {
                return this.d503_66Field;
            }
            set
            {
                this.d503_66Field = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.section9.org/cms/referral/data")]
    public partial class ResponseBodyDetailsD502_03_01Item1
    {

        private string d502_04Field;

        private string d502_05Field;

        private string d502_07Field;

        private string d502_06Field;

        private string d502_08Field;

        private string d502_09Field;

        private string d502_11Field;

        private string d502_39Field;

        private string d502_40Field;

        private string d502_33Field;

        private string d502_34Field;

        private string d506_93Field;

        private string d502_35Field;

        private string d502_36Field;

        private string d503_66Field;

        /// <remarks/>
        public string D502_04
        {
            get
            {
                return this.d502_04Field;
            }
            set
            {
                this.d502_04Field = value;
            }
        }

        /// <remarks/>
        public string D502_05
        {
            get
            {
                return this.d502_05Field;
            }
            set
            {
                this.d502_05Field = value;
            }
        }

        /// <remarks/>
        public string D502_07
        {
            get
            {
                return this.d502_07Field;
            }
            set
            {
                this.d502_07Field = value;
            }
        }

        /// <remarks/>
        public string D502_06
        {
            get
            {
                return this.d502_06Field;
            }
            set
            {
                this.d502_06Field = value;
            }
        }

        /// <remarks/>
        public string D502_08
        {
            get
            {
                return this.d502_08Field;
            }
            set
            {
                this.d502_08Field = value;
            }
        }

        /// <remarks/>
        public string D502_09
        {
            get
            {
                return this.d502_09Field;
            }
            set
            {
                this.d502_09Field = value;
            }
        }

        /// <remarks/>
        public string D502_11
        {
            get
            {
                return this.d502_11Field;
            }
            set
            {
                this.d502_11Field = value;
            }
        }

        /// <remarks/>
        public string D502_39
        {
            get
            {
                return this.d502_39Field;
            }
            set
            {
                this.d502_39Field = value;
            }
        }

        /// <remarks/>
        public string D502_40
        {
            get
            {
                return this.d502_40Field;
            }
            set
            {
                this.d502_40Field = value;
            }
        }

        /// <remarks/>
        public string D502_33
        {
            get
            {
                return this.d502_33Field;
            }
            set
            {
                this.d502_33Field = value;
            }
        }

        /// <remarks/>
        public string D502_34
        {
            get
            {
                return this.d502_34Field;
            }
            set
            {
                this.d502_34Field = value;
            }
        }

        /// <remarks/>
        public string D506_93
        {
            get
            {
                return this.d506_93Field;
            }
            set
            {
                this.d506_93Field = value;
            }
        }

        /// <remarks/>
        public string D502_35
        {
            get
            {
                return this.d502_35Field;
            }
            set
            {
                this.d502_35Field = value;
            }
        }

        /// <remarks/>
        public string D502_36
        {
            get
            {
                return this.d502_36Field;
            }
            set
            {
                this.d502_36Field = value;
            }
        }

        /// <remarks/>
        public string D503_66
        {
            get
            {
                return this.d503_66Field;
            }
            set
            {
                this.d503_66Field = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.section9.org/cms/referral/data")]
    public partial class ResponseBodyDetailsD502_03_01Item2
    {

        private string d502_04Field;

        private string d502_05Field;

        private string d502_07Field;

        private string d502_06Field;

        private string d502_08Field;

        private string d502_09Field;

        private string d502_11Field;

        private string d502_39Field;

        private string d502_40Field;

        private string d502_33Field;

        private string d502_34Field;

        private string d506_93Field;

        private string d502_35Field;

        private string d502_36Field;

        private string d503_66Field;

        /// <remarks/>
        public string D502_04
        {
            get
            {
                return this.d502_04Field;
            }
            set
            {
                this.d502_04Field = value;
            }
        }

        /// <remarks/>
        public string D502_05
        {
            get
            {
                return this.d502_05Field;
            }
            set
            {
                this.d502_05Field = value;
            }
        }

        /// <remarks/>
        public string D502_07
        {
            get
            {
                return this.d502_07Field;
            }
            set
            {
                this.d502_07Field = value;
            }
        }

        /// <remarks/>
        public string D502_06
        {
            get
            {
                return this.d502_06Field;
            }
            set
            {
                this.d502_06Field = value;
            }
        }

        /// <remarks/>
        public string D502_08
        {
            get
            {
                return this.d502_08Field;
            }
            set
            {
                this.d502_08Field = value;
            }
        }

        /// <remarks/>
        public string D502_09
        {
            get
            {
                return this.d502_09Field;
            }
            set
            {
                this.d502_09Field = value;
            }
        }

        /// <remarks/>
        public string D502_11
        {
            get
            {
                return this.d502_11Field;
            }
            set
            {
                this.d502_11Field = value;
            }
        }

        /// <remarks/>
        public string D502_39
        {
            get
            {
                return this.d502_39Field;
            }
            set
            {
                this.d502_39Field = value;
            }
        }

        /// <remarks/>
        public string D502_40
        {
            get
            {
                return this.d502_40Field;
            }
            set
            {
                this.d502_40Field = value;
            }
        }

        /// <remarks/>
        public string D502_33
        {
            get
            {
                return this.d502_33Field;
            }
            set
            {
                this.d502_33Field = value;
            }
        }

        /// <remarks/>
        public string D502_34
        {
            get
            {
                return this.d502_34Field;
            }
            set
            {
                this.d502_34Field = value;
            }
        }

        /// <remarks/>
        public string D506_93
        {
            get
            {
                return this.d506_93Field;
            }
            set
            {
                this.d506_93Field = value;
            }
        }

        /// <remarks/>
        public string D502_35
        {
            get
            {
                return this.d502_35Field;
            }
            set
            {
                this.d502_35Field = value;
            }
        }

        /// <remarks/>
        public string D502_36
        {
            get
            {
                return this.d502_36Field;
            }
            set
            {
                this.d502_36Field = value;
            }
        }

        /// <remarks/>
        public string D503_66
        {
            get
            {
                return this.d503_66Field;
            }
            set
            {
                this.d503_66Field = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.section9.org/cms/referral/data")]
    public partial class ResponseBodyDetailsItem
    {

        private string d502_04Field;

        private string d502_05Field;

        private string d502_08Field;

        private string d502_09Field;

        private string d502_11Field;

        private string d502_32Field;

        private string d502_39Field;

        private string d502_40Field;

        private string d502_38Field;

        private string d502_33Field;

        private string d506_93Field;

        private string d502_35Field;

        private string d502_36Field;

        private string d503_66Field;

        /// <remarks/>
        public string D502_04
        {
            get
            {
                return this.d502_04Field;
            }
            set
            {
                this.d502_04Field = value;
            }
        }

        /// <remarks/>
        public string D502_05
        {
            get
            {
                return this.d502_05Field;
            }
            set
            {
                this.d502_05Field = value;
            }
        }

        /// <remarks/>
        public string D502_08
        {
            get
            {
                return this.d502_08Field;
            }
            set
            {
                this.d502_08Field = value;
            }
        }

        /// <remarks/>
        public string D502_09
        {
            get
            {
                return this.d502_09Field;
            }
            set
            {
                this.d502_09Field = value;
            }
        }

        /// <remarks/>
        public string D502_11
        {
            get
            {
                return this.d502_11Field;
            }
            set
            {
                this.d502_11Field = value;
            }
        }

        /// <remarks/>
        public string D502_32
        {
            get
            {
                return this.d502_32Field;
            }
            set
            {
                this.d502_32Field = value;
            }
        }

        /// <remarks/>
        public string D502_39
        {
            get
            {
                return this.d502_39Field;
            }
            set
            {
                this.d502_39Field = value;
            }
        }

        /// <remarks/>
        public string D502_40
        {
            get
            {
                return this.d502_40Field;
            }
            set
            {
                this.d502_40Field = value;
            }
        }

        /// <remarks/>
        public string D502_38
        {
            get
            {
                return this.d502_38Field;
            }
            set
            {
                this.d502_38Field = value;
            }
        }

        /// <remarks/>
        public string D502_33
        {
            get
            {
                return this.d502_33Field;
            }
            set
            {
                this.d502_33Field = value;
            }
        }

        /// <remarks/>
        public string D506_93
        {
            get
            {
                return this.d506_93Field;
            }
            set
            {
                this.d506_93Field = value;
            }
        }

        /// <remarks/>
        public string D502_35
        {
            get
            {
                return this.d502_35Field;
            }
            set
            {
                this.d502_35Field = value;
            }
        }

        /// <remarks/>
        public string D502_36
        {
            get
            {
                return this.d502_36Field;
            }
            set
            {
                this.d502_36Field = value;
            }
        }

        /// <remarks/>
        public string D503_66
        {
            get
            {
                return this.d503_66Field;
            }
            set
            {
                this.d503_66Field = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.section9.org/cms/referral/data")]
    public partial class ResponseBodyDetailsItem1
    {

        private string d502_04Field;

        private string d502_05Field;

        private string d502_08Field;

        private string d502_09Field;

        private string d502_11Field;

        private string d502_39Field;

        private string d502_40Field;

        private string d502_38Field;

        private string d502_06Field;

        private string d502_33Field;

        private string d506_93Field;

        private string d502_35Field;

        private string d502_36Field;

        private string d503_66Field;

        /// <remarks/>
        public string D502_04
        {
            get
            {
                return this.d502_04Field;
            }
            set
            {
                this.d502_04Field = value;
            }
        }

        /// <remarks/>
        public string D502_05
        {
            get
            {
                return this.d502_05Field;
            }
            set
            {
                this.d502_05Field = value;
            }
        }

        /// <remarks/>
        public string D502_08
        {
            get
            {
                return this.d502_08Field;
            }
            set
            {
                this.d502_08Field = value;
            }
        }

        /// <remarks/>
        public string D502_09
        {
            get
            {
                return this.d502_09Field;
            }
            set
            {
                this.d502_09Field = value;
            }
        }

        /// <remarks/>
        public string D502_11
        {
            get
            {
                return this.d502_11Field;
            }
            set
            {
                this.d502_11Field = value;
            }
        }

        /// <remarks/>
        public string D502_39
        {
            get
            {
                return this.d502_39Field;
            }
            set
            {
                this.d502_39Field = value;
            }
        }

        /// <remarks/>
        public string D502_40
        {
            get
            {
                return this.d502_40Field;
            }
            set
            {
                this.d502_40Field = value;
            }
        }

        /// <remarks/>
        public string D502_38
        {
            get
            {
                return this.d502_38Field;
            }
            set
            {
                this.d502_38Field = value;
            }
        }

        /// <remarks/>
        public string D502_06
        {
            get
            {
                return this.d502_06Field;
            }
            set
            {
                this.d502_06Field = value;
            }
        }

        /// <remarks/>
        public string D502_33
        {
            get
            {
                return this.d502_33Field;
            }
            set
            {
                this.d502_33Field = value;
            }
        }

        /// <remarks/>
        public string D506_93
        {
            get
            {
                return this.d506_93Field;
            }
            set
            {
                this.d506_93Field = value;
            }
        }

        /// <remarks/>
        public string D502_35
        {
            get
            {
                return this.d502_35Field;
            }
            set
            {
                this.d502_35Field = value;
            }
        }

        /// <remarks/>
        public string D502_36
        {
            get
            {
                return this.d502_36Field;
            }
            set
            {
                this.d502_36Field = value;
            }
        }

        /// <remarks/>
        public string D503_66
        {
            get
            {
                return this.d503_66Field;
            }
            set
            {
                this.d503_66Field = value;
            }
        }
    }
}
