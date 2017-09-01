using System;
using System.Collections.Generic;
using System.Text;

namespace YBInterface1.Support.C010005
{
    public partial class RequestBody
    {
        private string d501_02Field;         

        /// <remarks/>
        public string D501_02
        {
            get
            {
                return this.d501_02Field;
            }
            set
            {
                this.d501_02Field = value;
            }
        }                    
    }
}
