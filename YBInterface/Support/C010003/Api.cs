using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using YBInterface;
using Model;
using Model.MZYbInterface;

namespace YBInterface1.Support.C010003
{
    public class Api : ApiBase<Request<RequestBody>, RequestBody, Response<ResponseBody>, ResponseBody>
    {
        const string FUNCTION_NO = "C010003";
        const string FUNCTION_NAME = "门诊补偿公示表"; 
 
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
            // TODO:
            RequestBody requestBody = new RequestBody()
            {
                D503_34 = "", //日期类型
                D503_35 = "", //起始日期
                D503_36 = "", //终止日期
                D503_15 = this.QzNh.GetNhZd("就诊类别", "25"), //就诊类别
                D501_11 = "", //就诊机构代码                
            };

            return requestBody;
        }
    }
}
