using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Linq;
using YBInterface;
using BLL;
using Model;

namespace YBInterface1.Support
{
    public abstract class ApiBase<TRequest, TRequestBody, TResponse, TResponseBody>
        where TRequestBody : class, new()
        where TResponseBody : class, new()
        where TRequest : Request<TRequestBody>, new()
        where TResponse : Response<TResponseBody>, new()
    {
        protected enum XmlFileType
        {
            Request = 0,
            Response = 1,
        }

        protected QZNhCommon QzNh { get; set; }

        protected string YbRegNo { get; set; }

        protected abstract string FunctionNo { get; }

        protected abstract string FunctionName { get; }

        public TResponse Process()
        {
            TResponse response = default(TResponse);

            try
            {
                response = this.ProcessRequest();
            }
            catch (Exception ex)
            {
                response = new TResponse()
                {
                    status = Status.ERROR,
                    head = new ResponseHead 
                    {
                        describe = ex.Message,
                        stateCode = "Unknow",
                    },
                    body = new TResponseBody(),
                };             
            }

            return response;
        }

        protected virtual bool MockResponse(out TResponse response)
        {
            response = null;

            return false;
        }

        private TResponse ProcessRequest()
        {
            TResponse response = new TResponse();

            TRequest request = this.BuildRequest();

            string requestXmlDoc = this.GetXmlFullFileName(request, XmlFileType.Request);
            string responseXmlDoc = this.GetXmlFullFileName(request, XmlFileType.Response);

            if (File.Exists(requestXmlDoc))
                File.Delete(requestXmlDoc);

            XmlSerializer.SaveToXml(requestXmlDoc, request, string.Empty);

            XmlDocument xmlDoc = new XmlDocument();

            if (MockResponse(out response))
            {
                XmlSerializer.SaveToXml(responseXmlDoc, response, "");
                xmlDoc.Load(responseXmlDoc);
            }
            else
            {               
                xmlDoc.Load(requestXmlDoc);
            
                string result = QzNh.FinallySetXmlNs(xmlDoc);
                result = result.Replace("standalone=\"yes\"", "");
                xmlDoc.LoadXml(result);
            }

            if (File.Exists(responseXmlDoc))
                File.Delete(responseXmlDoc);

            this.SaveXmlToFile(xmlDoc, responseXmlDoc);
            if (!File.Exists(responseXmlDoc))
                throw new FileNotFoundException(string.Format("File '{0}' not found", responseXmlDoc));

            response = XmlSerializer.LoadFromXml<TResponse, TResponseBody>(responseXmlDoc);
            if (response != null)
            {
                response.status = (response.head.stateCode == Constants.OK_STATE_CODE) ? Status.OK : Status.ERROR;
            }

            return response;
        }

        protected TRequest BuildRequest()
        {
            TRequest request = new TRequest()
            {
                head = this.BuildRequestHead(),
                body = this.BuildRequestBody()
            };

            return request;
        }

        private RequestHead BuildRequestHead()
        {
            RequestHead requestHead = new RequestHead()
            {
                functionNo = this.FunctionNo,
                functionName = this.FunctionName,
                healthcareprovider = new RequestHead.RequestHeadHealthcareprovider()
                {
                    identity = Constants.IDENTITY,
                    password = Constants.PASSWORD,
                },
                targetOrg = string.Empty,
                version = Constants.VERSION,
            };

            if (string.IsNullOrEmpty(this.YbRegNo) || this.YbRegNo.Length < 6)
                requestHead.targetOrg = Constants.TARGET_ORG;
            else
                requestHead.targetOrg = this.YbRegNo.Substring(0, 6);

            return requestHead;
        }

        protected abstract TRequestBody BuildRequestBody();

        private string GetXmlFullFileName(TRequest request, XmlFileType xmlFileType)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            string fileName = this.GetFileName(request.head.functionNo, request.head.functionName, xmlFileType);
            string appStartupPath = System.Windows.Forms.Application.StartupPath;
            string xmlDir = Path.Combine(appStartupPath, Constants.XML_DIRECTORY_NAME);

            if (!Directory.Exists(xmlDir))
            {
                Directory.CreateDirectory(xmlDir);
            }

            return System.IO.Path.Combine(xmlDir, fileName);
        }

        private string GetFileName(string functionNo, string functionName, XmlFileType xmlFileType)
        {
            // 文件名格式 - 接口名称.接口编号.输出XML类型.xml
            return string.Format("{0}.{1}.{2}.xml", functionName, functionNo, xmlFileType.ToString().ToUpper());
        }

        private void SaveXmlToFile(XmlDocument xmlDoc, string fileName)
        {
            StringBuilder xmlBuilder = new StringBuilder();
            xmlBuilder.Append(xmlDoc.InnerXml);

            Utilities.Document document = new Utilities.Document()
            {
                Content = xmlBuilder
            };

            document.Create(fileName, false);
        }

        /// <summary>
        /// Get compensation number of patient by mzRegId, invoiceNo and status
        /// </summary>
        /// <param name="mzRegId"></param>
        /// <param name="invoiceNo"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public YbPatSeqInfo GetPatientYbSeq(int mzRegId, string invoiceNo, int status)
        {
            if (string.IsNullOrEmpty(invoiceNo))
                throw new ArgumentNullException(invoiceNo);

            CYbPatSeq objYbPatSeq = new CYbPatSeq();

            // Query patient seq by mzRegId and invoice no, maybe exists records which status is 0 or 1. (F4 field) 
            string sqlWhere = string.Format("MzRegId={0} and F4='{1}'", mzRegId, invoiceNo);
            ModelList<YbPatSeqInfo> lstYbpatSeq = objYbPatSeq.GetDynamic(sqlWhere, string.Empty);

            // Seq status: 0 - normal , 1 - cancelled
            return lstYbpatSeq.FirstOrDefault(x => x.F4 == status.ToString());
        }

        public YbPatSeqInfo GetPatientYbSeq(int mzRegId, string invoiceNo)
        {
            return GetPatientYbSeq(mzRegId, invoiceNo, 0);
        }

        /// <summary>
        /// Save compensation number for patient
        /// </summary>
        /// <param name="patientSeq"></param>
        public void SavePatientYbSeq(string patientSeq)
        {
            int mzRegId = this.QzNh.InfoOuHosInfo.ID;
            string invoiceNo = this.QzNh.InfoOuInvoice.InvoNo;

            BLL.CYbPatSeq objYbPatSeqJb = new BLL.CYbPatSeq();
            YbPatSeqInfo patientSeqInfo = GetPatientYbSeq(mzRegId, invoiceNo);

            // create a new seq record in YbPatSeq table for patient
            if (patientSeqInfo == null)
            {
                Model.ModelList<Model.YbPatSeqInfo> lstYbpatSeqJb = new Model.ModelList<Model.YbPatSeqInfo>();

                patientSeqInfo = new YbPatSeqInfo()
                {
                    MzRegId = mzRegId,
                    YbSeq = patientSeq,
                    F1 = this.QzNh.InfoTallyGroup.ID.ToString(),
                    F2 = "0",
                    F3 = "",
                    F4 = invoiceNo,
                };
                lstYbpatSeqJb.Add(patientSeqInfo);
                objYbPatSeqJb.Save(lstYbpatSeqJb, null);
            }
            else
            {
                // modify seq record for patient
                patientSeqInfo.YbSeq = patientSeq;
                patientSeqInfo.F1 = this.QzNh.InfoTallyGroup.ID.ToString();
                patientSeqInfo.F2 = "0";
                patientSeqInfo.F3 = "";
                patientSeqInfo.F4 = invoiceNo;
                objYbPatSeqJb.Create(patientSeqInfo, null);
            }
        }
    }
}
