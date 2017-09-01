using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using YBInterface;
using Model;
using Model.MZYbInterface;

namespace YBInterface1.Support.C010001
{
    public class Api : ApiBase<Request<RequestBody>, RequestBody, Response<ResponseBody>, ResponseBody>
    {
        const string FUNCTION_NO = "C010001";
        const string FUNCTION_NAME = "门诊结算记录查询"; 
 
        public Api(QZNhCommon qzNh)
        { 
            this.QzNh = qzNh;                
        }

        protected override string FunctionName
        {
            get { return FUNCTION_NAME; }
        }

        protected override string FunctionNo
        {
            get { return FUNCTION_NO; }
        }

        protected override RequestBody BuildRequestBody()
        {
            RequestBody requestBody = new RequestBody()
            {
            };

            return requestBody;
        }
    }
}
