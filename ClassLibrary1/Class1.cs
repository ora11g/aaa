using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.section9.org/cms/referral/data")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.section9.org/cms/referral/data", IsNullable = false)]
    public partial class response
    {

        private responseBody bodyField;

        /// <remarks/>
        public responseBody body
        {
            get
            {
                return this.bodyField;
            }
            set
            {
                this.bodyField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.section9.org/cms/referral/data")]
    public partial class responseBody
    {

        private responseBodyBaseInfo baseInfoField;

        private responseBodyDiagnoseInfo diagnoseInfoField;

        private responseBodyFeeInfo feeInfoField;

        /// <remarks/>
        public responseBodyBaseInfo baseInfo
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
        public responseBodyDiagnoseInfo diagnoseInfo
        {
            get
            {
                return this.diagnoseInfoField;
            }
            set
            {
                this.diagnoseInfoField = value;
            }
        }

        /// <remarks/>
        public responseBodyFeeInfo feeInfo
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
    public partial class responseBodyBaseInfo
    {

        private string d503_01Field;

        private string d503_74Field;

        private string d501_02Field;

        private string d501_03Field;

        private string d501_04Field;

        private string d501_05Field;

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
        public string D503_74
        {
            get
            {
                return this.d503_74Field;
            }
            set
            {
                this.d503_74Field = value;
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

        /// <remarks/>
        public string D501_04
        {
            get
            {
                return this.d501_04Field;
            }
            set
            {
                this.d501_04Field = value;
            }
        }

        /// <remarks/>
        public string D501_05
        {
            get
            {
                return this.d501_05Field;
            }
            set
            {
                this.d501_05Field = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.section9.org/cms/referral/data")]
    public partial class responseBodyDiagnoseInfo
    {

        private string d603_02Field;

        private string d503_32Field;

        private string d503_31Field;

        private string d501_11Field;

        private string d101_02Field;

        private string d501_16Field;

        private string d501_38Field;

        private string d501_39Field;

        private string d501_15Field;

        private string d501_13Field;

        private string d501_12Field;

        private string d501_14Field;

        private string d501_10Field;

        private string d501_09Field;

        private string d503_18Field;

        private string d501_01Field;

        private string d501_80Field;

        private string d501_81Field;

        private string d501_82Field;

        private string d501_83Field;

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

        /// <remarks/>
        public string D101_02
        {
            get
            {
                return this.d101_02Field;
            }
            set
            {
                this.d101_02Field = value;
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
        public string D501_12
        {
            get
            {
                return this.d501_12Field;
            }
            set
            {
                this.d501_12Field = value;
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
        public string D501_10
        {
            get
            {
                return this.d501_10Field;
            }
            set
            {
                this.d501_10Field = value;
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
        public string D501_01
        {
            get
            {
                return this.d501_01Field;
            }
            set
            {
                this.d501_01Field = value;
            }
        }

        /// <remarks/>
        public string D501_80
        {
            get
            {
                return this.d501_80Field;
            }
            set
            {
                this.d501_80Field = value;
            }
        }

        /// <remarks/>
        public string D501_81
        {
            get
            {
                return this.d501_81Field;
            }
            set
            {
                this.d501_81Field = value;
            }
        }

        /// <remarks/>
        public string D501_82
        {
            get
            {
                return this.d501_82Field;
            }
            set
            {
                this.d501_82Field = value;
            }
        }

        /// <remarks/>
        public string D501_83
        {
            get
            {
                return this.d501_83Field;
            }
            set
            {
                this.d501_83Field = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.section9.org/cms/referral/data")]
    public partial class responseBodyFeeInfo
    {

        private string d503_02_2Field;

        private string d503_58Field;

        private string d503_09Field;

        private string d503_59Field;

        private string d503_61Field;

        private string d503_60Field;

        private string d503_69Field;

        private string d503_72Field;

        private string d503_73Field;

        private string d503_77Field;

        private responseBodyFeeInfoAllFeeSubentry allFeeSubentryField;

        private responseBodyFeeInfoComputeTypeFee computeTypeFeeField;

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
        public string D503_58
        {
            get
            {
                return this.d503_58Field;
            }
            set
            {
                this.d503_58Field = value;
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
        public string D503_59
        {
            get
            {
                return this.d503_59Field;
            }
            set
            {
                this.d503_59Field = value;
            }
        }

        /// <remarks/>
        public string D503_61
        {
            get
            {
                return this.d503_61Field;
            }
            set
            {
                this.d503_61Field = value;
            }
        }

        /// <remarks/>
        public string D503_60
        {
            get
            {
                return this.d503_60Field;
            }
            set
            {
                this.d503_60Field = value;
            }
        }

        /// <remarks/>
        public string D503_69
        {
            get
            {
                return this.d503_69Field;
            }
            set
            {
                this.d503_69Field = value;
            }
        }

        /// <remarks/>
        public string D503_72
        {
            get
            {
                return this.d503_72Field;
            }
            set
            {
                this.d503_72Field = value;
            }
        }

        /// <remarks/>
        public string D503_73
        {
            get
            {
                return this.d503_73Field;
            }
            set
            {
                this.d503_73Field = value;
            }
        }

        /// <remarks/>
        public string D503_77
        {
            get
            {
                return this.d503_77Field;
            }
            set
            {
                this.d503_77Field = value;
            }
        }

        /// <remarks/>
        public responseBodyFeeInfoAllFeeSubentry allFeeSubentry
        {
            get
            {
                return this.allFeeSubentryField;
            }
            set
            {
                this.allFeeSubentryField = value;
            }
        }

        /// <remarks/>
        public responseBodyFeeInfoComputeTypeFee computeTypeFee
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
    public partial class responseBodyFeeInfoAllFeeSubentry
    {

        private string d503_03Field;

        private string d503_04Field;

        private string d503_45Field;

        private string d503_46Field;

        private string d503_47Field;

        private string d503_05Field;

        private string d503_48Field;

        private string d503_06Field;

        private string d503_49Field;

        private string d503_50Field;

        private string d503_51Field;

        private string d503_52Field;

        private string d503_53Field;

        private string d503_57Field;

        /// <remarks/>
        public string D503_03
        {
            get
            {
                return this.d503_03Field;
            }
            set
            {
                this.d503_03Field = value;
            }
        }

        /// <remarks/>
        public string D503_04
        {
            get
            {
                return this.d503_04Field;
            }
            set
            {
                this.d503_04Field = value;
            }
        }

        /// <remarks/>
        public string D503_45
        {
            get
            {
                return this.d503_45Field;
            }
            set
            {
                this.d503_45Field = value;
            }
        }

        /// <remarks/>
        public string D503_46
        {
            get
            {
                return this.d503_46Field;
            }
            set
            {
                this.d503_46Field = value;
            }
        }

        /// <remarks/>
        public string D503_47
        {
            get
            {
                return this.d503_47Field;
            }
            set
            {
                this.d503_47Field = value;
            }
        }

        /// <remarks/>
        public string D503_05
        {
            get
            {
                return this.d503_05Field;
            }
            set
            {
                this.d503_05Field = value;
            }
        }

        /// <remarks/>
        public string D503_48
        {
            get
            {
                return this.d503_48Field;
            }
            set
            {
                this.d503_48Field = value;
            }
        }

        /// <remarks/>
        public string D503_06
        {
            get
            {
                return this.d503_06Field;
            }
            set
            {
                this.d503_06Field = value;
            }
        }

        /// <remarks/>
        public string D503_49
        {
            get
            {
                return this.d503_49Field;
            }
            set
            {
                this.d503_49Field = value;
            }
        }

        /// <remarks/>
        public string D503_50
        {
            get
            {
                return this.d503_50Field;
            }
            set
            {
                this.d503_50Field = value;
            }
        }

        /// <remarks/>
        public string D503_51
        {
            get
            {
                return this.d503_51Field;
            }
            set
            {
                this.d503_51Field = value;
            }
        }

        /// <remarks/>
        public string D503_52
        {
            get
            {
                return this.d503_52Field;
            }
            set
            {
                this.d503_52Field = value;
            }
        }

        /// <remarks/>
        public string D503_53
        {
            get
            {
                return this.d503_53Field;
            }
            set
            {
                this.d503_53Field = value;
            }
        }

        /// <remarks/>
        public string D503_57
        {
            get
            {
                return this.d503_57Field;
            }
            set
            {
                this.d503_57Field = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.section9.org/cms/referral/data")]
    public partial class responseBodyFeeInfoComputeTypeFee
    {

        private string d503_03_AField;

        private string d503_04_AField;

        private string d503_45_AField;

        private string d503_46_AField;

        private string d503_47_AField;

        private string d503_05_AField;

        private string d503_48_AField;

        private string d503_06_AField;

        private string d503_49_AField;

        private string d503_50_AField;

        private string d503_51_AField;

        private string d503_52_AField;

        private string d503_53_AField;

        private string d503_57_AField;

        /// <remarks/>
        public string D503_03_A
        {
            get
            {
                return this.d503_03_AField;
            }
            set
            {
                this.d503_03_AField = value;
            }
        }

        /// <remarks/>
        public string D503_04_A
        {
            get
            {
                return this.d503_04_AField;
            }
            set
            {
                this.d503_04_AField = value;
            }
        }

        /// <remarks/>
        public string D503_45_A
        {
            get
            {
                return this.d503_45_AField;
            }
            set
            {
                this.d503_45_AField = value;
            }
        }

        /// <remarks/>
        public string D503_46_A
        {
            get
            {
                return this.d503_46_AField;
            }
            set
            {
                this.d503_46_AField = value;
            }
        }

        /// <remarks/>
        public string D503_47_A
        {
            get
            {
                return this.d503_47_AField;
            }
            set
            {
                this.d503_47_AField = value;
            }
        }

        /// <remarks/>
        public string D503_05_A
        {
            get
            {
                return this.d503_05_AField;
            }
            set
            {
                this.d503_05_AField = value;
            }
        }

        /// <remarks/>
        public string D503_48_A
        {
            get
            {
                return this.d503_48_AField;
            }
            set
            {
                this.d503_48_AField = value;
            }
        }

        /// <remarks/>
        public string D503_06_A
        {
            get
            {
                return this.d503_06_AField;
            }
            set
            {
                this.d503_06_AField = value;
            }
        }

        /// <remarks/>
        public string D503_49_A
        {
            get
            {
                return this.d503_49_AField;
            }
            set
            {
                this.d503_49_AField = value;
            }
        }

        /// <remarks/>
        public string D503_50_A
        {
            get
            {
                return this.d503_50_AField;
            }
            set
            {
                this.d503_50_AField = value;
            }
        }

        /// <remarks/>
        public string D503_51_A
        {
            get
            {
                return this.d503_51_AField;
            }
            set
            {
                this.d503_51_AField = value;
            }
        }

        /// <remarks/>
        public string D503_52_A
        {
            get
            {
                return this.d503_52_AField;
            }
            set
            {
                this.d503_52_AField = value;
            }
        }

        /// <remarks/>
        public string D503_53_A
        {
            get
            {
                return this.d503_53_AField;
            }
            set
            {
                this.d503_53_AField = value;
            }
        }

        /// <remarks/>
        public string D503_57_A
        {
            get
            {
                return this.d503_57_AField;
            }
            set
            {
                this.d503_57_AField = value;
            }
        }
    }


 }
