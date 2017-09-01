using System;
using System.Collections.Generic;
using System.Text;
using YBInterface;
using YBInterface1.Support;

namespace YBInterface1
{
    public class QzNhApi : AbstractQZNhCommon
    {
        public override Response<Support.B010001.responseBody> OuInvoiceBudget()
        {
            Support.B010001.Api api = new Support.B010001.Api(this);
            
            return api.Process();
        }
        
        //public override B010001.UploadOuInvoiceResponse OuInvoiceBalance()
        //{
        //    B010001 service = new B010001(this);

        //    B010001.UploadOuInvoiceRequest request = new B010001.UploadOuInvoiceRequest()
        //    {
        //        FunctionNo = "B010002",
        //        FunctionName = "门诊结算",
        //    };

        //    return service.CallService(request);
        //}


        //public override B010004.OuInvoiceResponse OuInvoice()
        //{
        //    B010004 service = new B010004(this);

        //    B010004.OuInvoiceRequest request = new B010004.OuInvoiceRequest()
        //    {
        //        FunctionNo = "B010004",
        //        FunctionName = "门诊发票",
        //    };

        //    return service.CallService(request);
        //}
    }
}
