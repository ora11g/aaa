using System;
using System.Collections.Generic; 
using System.Text;
using System.Xml.Serialization;

namespace YBInterface1.Support
{
    public enum Status
    {
        OK = 0,
        ERROR = 1,
    }

    /// <remarks/>
    //[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.section9.org/cms/referral/data")]
    [System.Xml.Serialization.XmlRootAttribute("response")]
    public partial class Response<TResponseBody> where TResponseBody : class, new()
    {            
        private ResponseHead headField;
        
        public ResponseHead head
        {
            get
            {
                return this.headField;
            }
            set
            {
                this.headField = value;
            }
        }

        private TResponseBody bodyField;
        
        public TResponseBody body
        {
            get
            {
                return this.bodyField;
            }
            set
            {
                this.bodyField = value;
            }
        }
                
        [XmlIgnoreAttribute]
        public Status status { get; set; }
    }

    /// <remarks/>
    //[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.section9.org/cms/referral/data")]
    public partial class ResponseHead
    {

        private string stateCodeField;

        private string describeField;
        
        public string stateCode
        {
            get
            {
                return this.stateCodeField;
            }
            set
            {
                this.stateCodeField = value;
            }
        }
        
        public string describe
        {
            get
            {
                return this.describeField;
            }
            set
            {
                this.describeField = value;
            }
        }
    }
}
