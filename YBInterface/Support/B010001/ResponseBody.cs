using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace YBInterface1.Support.B010001
{
    public partial class responseBody
    {
        public responseBodyBaseInfo baseInfo { get; set; }
        
        public responseBodyDiagnoseInfo diagnoseInfo { get; set; }
        
        public responseBodyFeeInfo feeInfo { get; set; }
    }

    public partial class responseBodyBaseInfo
    {
        public string D503_01 { get; set; }

        public string D503_74 { get; set; }

        public string D501_02 { get; set; }

        public string D501_03 { get; set; }

        public string D501_04 { get; set; }

        public string D501_05 { get; set; }
    }

    
    public partial class responseBodyDiagnoseInfo
    {
        public string D603_02 { get; set; }        
        public string D503_32 { get; set; }
        public string D503_31 { get; set; }
        public string D501_11 { get; set; }
        public string D101_02 { get; set; }
        public string D501_16 { get; set; }
        public string D501_38 { get; set; }
        public string D501_39 { get; set; }
        public string D501_15 { get; set; }
        public string D501_13 { get; set; }
        public string D501_12 { get; set; }
        public string D501_14 { get; set; }
        public string D501_10 { get; set; }
        public string D501_09 { get; set; }
        public string D503_18 { get; set; }
        public string D501_01 { get; set; }
        public string D501_80 { get; set; }
        public string D501_81 { get; set; }
        public string D501_82 { get; set; }
        public string D501_83 { get; set; }
    }

    public partial class responseBodyFeeInfo
    {
        public string D503_02_2 { get; set; }
        public string D503_58 { get; set; }
        public string D503_09 { get; set; }
        public string D503_59 { get; set; }
        public string D503_61 { get; set; }
        public string D503_60 { get; set; }
        public string D503_69 { get; set; }
        public string D503_72 { get; set; }
        public string D503_73 { get; set; }
        public string D503_77 { get; set; }
        public responseBodyFeeInfoAllFeeSubentry allFeeSubentry { get; set; }
        public responseBodyFeeInfoComputeTypeFee computeTypeFee { get; set; }
    }

    public partial class responseBodyFeeInfoAllFeeSubentry
    {
        public string D503_03 { get; set; }
        public string D503_04 { get; set; }
        public string D503_45 { get; set; }
        public string D503_46 { get; set; }
        public string D503_47 { get; set; }
        public string D503_05 { get; set; }
        public string D503_48 { get; set; }
        public string D503_06 { get; set; }
        public string D503_49 { get; set; }
        public string D503_50 { get; set; }
        public string D503_51 { get; set; }
        public string D503_52 { get; set; }
        public string D503_53 { get; set; }
        public string D503_57 { get; set; }
    }

    public partial class responseBodyFeeInfoComputeTypeFee
    {
        public string D503_03_A { get; set; }
        public string D503_04_A { get; set; }
        public string D503_45_A { get; set; }
        public string D503_46_A { get; set; }
        public string D503_47_A { get; set; }
        public string D503_05_A { get; set; }
        public string D503_48_A { get; set; }
        public string D503_06_A { get; set; }
        public string D503_49_A { get; set; }
        public string D503_50_A { get; set; }
        public string D503_51_A { get; set; }
        public string D503_52_A { get; set; }
        public string D503_53_A { get; set; }
        public string D503_57_A { get; set; }
    }

}
