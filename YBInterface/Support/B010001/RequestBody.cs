using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace YBInterface1.Support.B010001
{
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.section9.org/cms/referral/data")]
    public partial class requestBody
    {

        private string d501_02Field;

        private string d501_09Field;

        private string d501_11Field;

        private string d501_16Field;

        private string d501_38Field;

        private string d501_14Field;

        private string d501_13Field;

        private string d501_15Field;

        private string d501_39Field;

        private string d503_31Field;

        private string d503_32Field;

        private string d503_18Field;

        private requestBodyItem itemField;

        private requestBodyDetails detailsField;

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
        public string D501_16
        {
            get
            {
                return this.d501_16Field;
            }
            set
            {
                this.d501_16Field = value;
            }
        }

        /// <remarks/>
        public string D501_38
        {
            get
            {
                return this.d501_38Field;
            }
            set
            {
                this.d501_38Field = value;
            }
        }

        /// <remarks/>
        public string D501_14
        {
            get
            {
                return this.d501_14Field;
            }
            set
            {
                this.d501_14Field = value;
            }
        }

        /// <remarks/>
        public string D501_13
        {
            get
            {
                return this.d501_13Field;
            }
            set
            {
                this.d501_13Field = value;
            }
        }

        /// <remarks/>
        public string D501_15
        {
            get
            {
                return this.d501_15Field;
            }
            set
            {
                this.d501_15Field = value;
            }
        }

        /// <remarks/>
        public string D501_39
        {
            get
            {
                return this.d501_39Field;
            }
            set
            {
                this.d501_39Field = value;
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
        public string D503_32
        {
            get
            {
                return this.d503_32Field;
            }
            set
            {
                this.d503_32Field = value;
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
        public requestBodyItem item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        /// <remarks/>
        public requestBodyDetails details
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
    public partial class requestBodyItem
    {

        private string d503_67Field;

        private string d503_68Field;

        /// <remarks/>
        public string D503_67
        {
            get
            {
                return this.d503_67Field;
            }
            set
            {
                this.d503_67Field = value;
            }
        }

        /// <remarks/>
        public string D503_68
        {
            get
            {
                return this.d503_68Field;
            }
            set
            {
                this.d503_68Field = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.section9.org/cms/referral/data")]
    public partial class requestBodyDetails
    {

        private requestBodyDetailsD502_03_01 d502_03_01Field;

        private requestBodyDetailsItem[] d502_03_02Field;

        private requestBodyDetailsItem1[] d502_03_03Field;

        /// <remarks/>
        public requestBodyDetailsD502_03_01 D502_03_01
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
        [System.Xml.Serialization.XmlArrayItemAttribute("item", IsNullable = false)]
        public requestBodyDetailsItem[] D502_03_02
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
        [System.Xml.Serialization.XmlArrayItemAttribute("item", IsNullable = false)]
        public requestBodyDetailsItem1[] D502_03_03
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
    public partial class requestBodyDetailsD502_03_01
    {

        private requestBodyDetailsD502_03_01Item[] d502_31_01Field;

        private requestBodyDetailsD502_03_01Item1[] d502_31_02Field;

        private requestBodyDetailsD502_03_01Item2[] d502_31_03Field;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("item", IsNullable = false)]
        public requestBodyDetailsD502_03_01Item[] D502_31_01
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
        [System.Xml.Serialization.XmlArrayItemAttribute("item", IsNullable = false)]
        public requestBodyDetailsD502_03_01Item1[] D502_31_02
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
        [System.Xml.Serialization.XmlArrayItemAttribute("item", IsNullable = false)]
        public requestBodyDetailsD502_03_01Item2[] D502_31_03
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
    public partial class requestBodyDetailsD502_03_01Item
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

        private string d502_43Field;

        private string d502_02Field;

        private string d502_12Field;

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
        public string D502_43
        {
            get
            {
                return this.d502_43Field;
            }
            set
            {
                this.d502_43Field = value;
            }
        }

        /// <remarks/>
        public string D502_02
        {
            get
            {
                return this.d502_02Field;
            }
            set
            {
                this.d502_02Field = value;
            }
        }

        /// <remarks/>
        public string D502_12
        {
            get
            {
                return this.d502_12Field;
            }
            set
            {
                this.d502_12Field = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.section9.org/cms/referral/data")]
    public partial class requestBodyDetailsD502_03_01Item1
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

        private string d502_43Field;

        private string d502_02Field;

        private string d502_12Field;

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
        public string D502_43
        {
            get
            {
                return this.d502_43Field;
            }
            set
            {
                this.d502_43Field = value;
            }
        }

        /// <remarks/>
        public string D502_02
        {
            get
            {
                return this.d502_02Field;
            }
            set
            {
                this.d502_02Field = value;
            }
        }

        /// <remarks/>
        public string D502_12
        {
            get
            {
                return this.d502_12Field;
            }
            set
            {
                this.d502_12Field = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.section9.org/cms/referral/data")]
    public partial class requestBodyDetailsD502_03_01Item2
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

        private string d502_43Field;

        private string d502_10Field;

        private string d502_02Field;

        private string d502_12Field;

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
        public string D502_43
        {
            get
            {
                return this.d502_43Field;
            }
            set
            {
                this.d502_43Field = value;
            }
        }

        /// <remarks/>
        public string D502_10
        {
            get
            {
                return this.d502_10Field;
            }
            set
            {
                this.d502_10Field = value;
            }
        }

        /// <remarks/>
        public string D502_02
        {
            get
            {
                return this.d502_02Field;
            }
            set
            {
                this.d502_02Field = value;
            }
        }

        /// <remarks/>
        public string D502_12
        {
            get
            {
                return this.d502_12Field;
            }
            set
            {
                this.d502_12Field = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.section9.org/cms/referral/data")]
    public partial class requestBodyDetailsItem
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

        private string d502_02Field;

        private string d502_12Field;

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
        public string D502_02
        {
            get
            {
                return this.d502_02Field;
            }
            set
            {
                this.d502_02Field = value;
            }
        }

        /// <remarks/>
        public string D502_12
        {
            get
            {
                return this.d502_12Field;
            }
            set
            {
                this.d502_12Field = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.section9.org/cms/referral/data")]
    public partial class requestBodyDetailsItem1
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

        private string d502_43Field;

        private string d502_02Field;

        private string d502_12Field;

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
        public string D502_43
        {
            get
            {
                return this.d502_43Field;
            }
            set
            {
                this.d502_43Field = value;
            }
        }

        /// <remarks/>
        public string D502_02
        {
            get
            {
                return this.d502_02Field;
            }
            set
            {
                this.d502_02Field = value;
            }
        }

        /// <remarks/>
        public string D502_12
        {
            get
            {
                return this.d502_12Field;
            }
            set
            {
                this.d502_12Field = value;
            }
        }
    }
}
