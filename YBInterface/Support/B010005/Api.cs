using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using YBInterface;
using Model;
using Model.MZYbInterface;

namespace YBInterface1.Support.B010005
{
    public class Api : ApiBase<Request<RequestBody>, RequestBody, Response<ResponseBody>, ResponseBody>
    {
        const string FUNCTION_NO = "B010005";
        const string FUNCTION_NAME = "门诊退票"; 
 
        public Api(QZNhCommon qzNh)
        { 
            this.QzNh = qzNh;                
        }

        protected override string FunctionNo
        {
            get { return FUNCTION_NO; }
        }

        protected override string FunctionName
        {
            get { return FUNCTION_NAME; }
        }

        protected override RequestBody BuildRequestBody()
        {
            var patientYbSeq = GetPatientYbSeq(this.QzNh.InfoOuHosInfo.ID, this.QzNh.InfoOuInvoice.InvoNo);

            if (patientYbSeq == null)
                throw new Exception("该病人不是新农合医保病人");

            // TODO:
            RequestBody requestBody = new RequestBody()
            {
                D503_01 = patientYbSeq.YbSeq, //门诊补偿流水号
                D501_02 = this.YbRegNo, //个人编码
                D503_18 = string.Empty, //经办人
                D503_78 = string.Empty //退票类型
            };

            return requestBody;
        }
    }
}
