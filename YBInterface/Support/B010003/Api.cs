using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using YBInterface;
using Model;
using Model.MZYbInterface;

namespace YBInterface1.Support.B010003
{
    public class Api : ApiBase<Request<RequestBody>, RequestBody, Response<ResponseBody>, ResponseBody>
    {
        const string FUNCTION_NO = "B010003";
        const string FUNCTION_NAME = "门诊结算单"; 
 
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

            RequestBody requestBody = new RequestBody()
            {
                //门诊补偿流水号
                D503_01 = patientYbSeq.YbSeq
            };

            return requestBody;
        }
    }
}
