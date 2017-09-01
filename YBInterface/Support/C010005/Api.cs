using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using YBInterface;
using Model;
using Model.MZYbInterface;

namespace YBInterface1.Support.C010005
{
    public class Api : ApiBase<Request<RequestBody>, RequestBody, Response<responseBody>,responseBody>
    {
        private const string FUNCTION_NO = "C010005";
        private const string FUNCTION_NAME = "门诊慢性病患者疾病代码查询"; 
 
        public Api(QZNhCommon qzNh)
        { 
            this.QzNh = qzNh;
            this.YbRegNo = "450702201408148128";
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
                D501_02 = this.YbRegNo
            };

            return requestBody;
        }

        protected override bool MockResponse(out Response<responseBody> response)
        { 
            #region The following code for debugging
            
            response = new Response<responseBody>()
            {
                head = new ResponseHead()
                {
                    stateCode = "01000002",
                    describe = "Error message",
                },
                //Body = new ResponseBody()
                //{
                //    Item = new ResponseBodyItem()
                //    {
                //        D501_02 = "个人编码",
                //        D501_16 = "疾病代码",
                //        D501_46 = "疾病名称",
                //        D501_84 = "慢病代码"
                //    }
                //}
            };
           
            #endregion

            return true;
        }
    }
}
