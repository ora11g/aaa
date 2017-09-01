using System;
using System.Collections.Generic;
using System.Text;
using YBInterface;
using YBInterface1.Support;

namespace YBInterface1
{
    public abstract class AbstractQZNhCommon : QZNhCommon
    {
        public virtual Response<Support.B010001.responseBody> OuInvoiceBudget() 
        {
            throw new NotImplementedException();
        }

        public virtual Response<Support.B010001.responseBody> OuInvoiceBalance() 
        {
            throw new NotImplementedException();
        }

        //public virtual B010004.OuInvoiceResponse OuInvoice() 
        //{
        //    throw new NotImplementedException();
        //}
    }
}
