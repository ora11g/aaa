using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using YBInterface;
using Model;
using Model.MZYbInterface;
using YBInterface1.Support.B010001;

namespace YBInterface1.Support.B010002
{
    public class Api : ApiBase<Request<requestBody>, requestBody, Response<responseBody>, responseBody>
    {
        const string FUNCTION_NO = "B010002";
        const string FUNCTION_NAME = "门诊结算"; 

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

        protected override requestBody BuildRequestBody()
        {
            throw new NotImplementedException();
        }
    }
}
