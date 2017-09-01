using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using YBInterface;
using Model;
using Model.MZYbInterface;

namespace YBInterface1.Support.B010006
{
    public class Api : ApiBase<Request<RequestBody>, RequestBody, Response<ResponseBody>, ResponseBody>
    {
        const string FUNCTION_NO = "B010006";
        const string FUNCTION_NAME = "门诊连续日判断"; 
 
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
            DateTime invoiceDate = this.QzNh.InfoOuInvoice.InvoTime;

            RequestBody requestBody = new RequestBody()
            {
                D501_09 = this.QzNh.GetNhZd("就诊类别", "25"), //就诊类别
                //D501_02 = request.YbRegNo, //个人编码
                D503_31 = invoiceDate.ToString("yyyy-MM-ddTHH-mm-ss"), //发票时间
                D501_11 = this.QzNh.GetNhZd("就医机构", "23"), //就诊机构代码
            };
            
            return requestBody;
        }
    }
}