using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using YBInterface;
using Model;
using Model.MZYbInterface;

namespace YBInterface1.Support.B010007
{
    public class Api : ApiBase<Request<RequestBody>, RequestBody, Response<ResponseBody>, ResponseBody>
    {
        const string FUNCTION_NO = "B010007";
        const string FUNCTION_NAME = "门诊统筹总额预算"; 
 
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
            RequestBody requestBody = new RequestBody()
            {
                //就诊机构代码
                D501_11 = this.QzNh.GetNhZd("就医机构", "23")
            };

            return requestBody;
        }
    }
}
