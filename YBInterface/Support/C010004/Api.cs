using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using YBInterface;
using Model;
using Model.MZYbInterface;

namespace YBInterface1.Support.C010004
{
    public class Api : ApiBase<Request<RequestBody>, RequestBody, Response<ResponseBody>, ResponseBody>
    {
        const string FUNCTION_NO = "C010004";
        const string FUNCTION_NAME = "门诊统筹总额预算查询"; 
 
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
                D501_11 = this.QzNh.GetNhZd("就医机构", "23"), //必填
                D603_02 = string.Empty //年份, 非必填
            };

            return requestBody;
        }
    }
}
