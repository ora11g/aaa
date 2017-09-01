using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace YBInterface1.Support
{
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.section9.org/cms/referral/data")]
    [XmlRootAttribute("request",  Namespace = "http://www.section9.org/cms/referral/data")]
    public class Request<TRequestBody> where TRequestBody : class, new()
    {
        private RequestHead headField;
        
        public RequestHead head
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

        private TRequestBody bodyField;
              
        public TRequestBody body
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

        /// <summary>
        /// 个人编码
        /// </summary>
        [XmlIgnoreAttribute]
        public string ybRegNo { get; set; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.section9.org/cms/referral/data")]
    public partial class RequestHead
    {

        private string versionField;

        private string functionNoField;

        private string targetOrgField;
        
        private RequestHeadHealthcareprovider healthcareproviderField;
           
        public string version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }
            
        public string functionNo
        {
            get
            {
                return this.functionNoField;
            }
            set
            {
                this.functionNoField = value;
            }
        }

        [XmlIgnoreAttribute]
        public string functionName { get; set; }
        
        public string targetOrg
        {
            get
            {
                return this.targetOrgField;
            }
            set
            {
                this.targetOrgField = value;
            }
        }
        
        public RequestHeadHealthcareprovider healthcareprovider
        {
            get
            {
                return this.healthcareproviderField;
            }
            set
            {
                this.healthcareproviderField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.section9.org/cms/referral/data")]
        public partial class RequestHeadHealthcareprovider
        {
            private string identityField;

            private string passwordField;
            
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string identity
            {
                get
                {
                    return this.identityField;
                }
                set
                {
                    this.identityField = value;
                }
            }
            
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string password
            {
                get
                {
                    return this.passwordField;
                }
                set
                {
                    this.passwordField = value;
                }
            }
        }
    }
}
