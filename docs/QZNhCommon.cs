using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.Timers;
using Model;


namespace YBInterface
{
    /// <summary>
    /// 钦州农合
    /// </summary>
    public class QZNhCommon : YbProvider
    {

        #region BLL
        BLL.CInHosInfo _objInHosInfo = new BLL.CInHosInfo();
        BLL.CBsUnit _objBsUnit = new BLL.CBsUnit();
        BLL.CBsDoctor _objBsDoctor = new BLL.CBsDoctor();
        BLL.CBsDrugForm _objBsDrugForm = new BLL.CBsDrugForm();
        BLL.CBsItemYb _objBsItemYb = new BLL.CBsItemYb();
        BLL.CBsItemDrug _objBsItemDrug = new BLL.CBsItemDrug();
        BLL.CBsLocation _objBsLocation = new BLL.CBsLocation();
        BLL.CInHosMzIll _objInHosMzIll = new BLL.CInHosMzIll();
        BLL.CBsBed _objBsBed = new BLL.CBsBed();
        #endregion


        //private string registerProperty;
        ///// <summary>
        ///// 登记属性
        ///// </summary>
        // public string RegisterProperty
        //{
        //    get { return registerProperty; }
        //    set { registerProperty = value; }
        //}

        //private string seeCategories;
        ///// <summary>
        ///// 就诊类别
        ///// </summary>
        //public string SeeCategories
        //{
        //    get { return seeCategories; }
        //    set { seeCategories = value; }
        //}


        #region 通用方法
        /// <summary>
        /// 字符拥有特殊的意义
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        private string ConvertEscape(object val)
        {
            string val1 = Convert.ToString(val);

            val1 = val1.Replace("<", "&lt")//小于
                .Replace(">", "&gt")//大于
                .Replace("&", "&amp")//和号
                .Replace("'", "&apos")//单引号
                .Replace("\"", "&quot");//引号

            return val1;
        }

        /// <summary>
        /// 设置节点值
        /// </summary>
        /// <param name="ParentNode">父节点</param>
        /// <param name="nodeName">获取当前赋值节点</param>
        /// <param name="val">值</param>
        public void SetNodeValue(XmlNode ParentNode, string nodeName, object val)
        {
            XmlNode node = ParentNode[nodeName];
            if (node == null)
                return;
            node.InnerText = Convert.ToString(val);
        }

        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="currentNode">父节点</param>
        /// <param name="nodeName">获取当前赋值属性</param>
        /// <param name="val">值</param>
        public void SetAttributeValue(XmlDocument docment, XmlNode currentNode, string nodeName, object val)
        {
            if (currentNode == null) return;
            XmlAttribute attrib = currentNode.Attributes[nodeName];
            if (attrib == null)
            {
                if (docment == null) return;
                attrib = docment.CreateAttribute(nodeName);
                currentNode.Attributes.Append(attrib);
            }
            attrib.InnerText = Convert.ToString(val);
        }

        public XmlElement SetElementValue(System.Xml.XmlDocument document, string eleName, object val)
        {
            XmlElement subEle = document.CreateElement(eleName);//月份
            subEle.InnerText = Convert.ToString(val);
            return subEle;
        }
        #endregion

        //#region 数据交换模型
        ///// <summary>
        ///// XML数据交换模型
        ///// </summary>
        //public System.Xml.XmlDocument DataExchangeModel(string functionNo)
        //{
        //    StringBuilder sBuilder = new StringBuilder();
        //    sBuilder.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        //    sBuilder.AppendLine("<request xmlns=\"http://www.section9.org/cms/referral/data\">");
        //    sBuilder.AppendLine("   <head>");
        //    sBuilder.AppendLine("       <version></version>");//版本号
        //    sBuilder.AppendLine("       <functionNo></functionNo>");//功能编码
        //    sBuilder.AppendLine("       <targetOrg></targetOrg>");//目标机构代码
        //    sBuilder.AppendLine("       <healthcareprovider identity=\"\" password=\"\"/>");//医疗单位身份,密码
        //    sBuilder.AppendLine("   </head>");
        //    sBuilder.AppendLine("   <body>");
        //    sBuilder.AppendLine("   </body>");
        //    sBuilder.AppendLine("</request>");
        //    System.Xml.XmlDocument document = new System.Xml.XmlDocument();
        //    document.LoadXml(sBuilder.ToString());

        //    XmlNode rootNode = document.DocumentElement;
        //    XmlNode headNode = rootNode["head"];
        //    if (headNode != null)
        //    {
        //        this.SetNodeValue(headNode, "version", "2000");//版本号
        //        this.SetNodeValue(headNode, "functionNo", functionNo);//功能编码
        //        if (functionNo.Trim() == "C020005" || functionNo.Trim() == "C020010" || functionNo.Trim() == "C020011" || functionNo.Trim() == "C020013"||
        //            functionNo.Trim() == "C020014" || functionNo.Trim() == "A040001")
        //        {
        //            this.SetNodeValue(headNode, "targetOrg", "450703");
        //        }
        //        else
        //        {
        //            this.SetNodeValue(headNode, "targetOrg", InfoInHosInfo.YbRegNo.Substring(0, 6));//目标机构代码
        //        }
        //        this.SetAttributeValue(document, headNode["healthcareprovider"], "identity", "P04");//医疗单位身份
        //        this.SetAttributeValue(document, headNode["healthcareprovider"], "password", "123456");//密码

        //    }
        //    return document;
        //}
        //#endregion

        #region 数据交换模型
        /// <summary>
        /// XML数据交换模型
        /// </summary>
        public System.Xml.XmlDocument XmlDataExchangeModel(string functionNo, string YbRegNo)
        {
            if (InfoInHosInfo != null)
            {
                YbRegNo = InfoInHosInfo.YbRegNo;
            }
            else
            {
                Console.WriteLine(1);
            }
            StringBuilder sBuilder = new StringBuilder();
            sBuilder.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sBuilder.AppendLine("<request xmlns=\"http://www.section9.org/cms/referral/data\">");
            sBuilder.AppendLine("   <head>");
            sBuilder.AppendLine("       <version></version>");//版本号
            sBuilder.AppendLine("       <functionNo></functionNo>");//功能编码
            sBuilder.AppendLine("       <targetOrg></targetOrg>");//目标机构代码
            sBuilder.AppendLine("       <healthcareprovider identity=\"\" password=\"\"/>");//医疗单位身份,密码
            sBuilder.AppendLine("   </head>");
            if (functionNo != "C020010")
            {
                sBuilder.AppendLine("   <body>");
                sBuilder.AppendLine("   </body>");
            }
            sBuilder.AppendLine("</request>");
            System.Xml.XmlDocument document = new System.Xml.XmlDocument();
            document.LoadXml(sBuilder.ToString());

            XmlNode rootNode = document.DocumentElement;
            XmlNode headNode = rootNode["head"];
            if (headNode != null)
            {
                this.SetNodeValue(headNode, "version", "2005");//版本号 大病的改为2005
                this.SetNodeValue(headNode, "functionNo", functionNo);//功能编码
                if (YbRegNo.Trim() == string.Empty)
                {
                    this.SetNodeValue(headNode, "targetOrg", "450703");
                }
                else
                {
                    this.SetNodeValue(headNode, "targetOrg", YbRegNo.Substring(0, 6));//目标机构代码
                }
                this.SetAttributeValue(document, headNode["healthcareprovider"], "identity", "P04");//医疗单位身份
                this.SetAttributeValue(document, headNode["healthcareprovider"], "password", "123456");//密码

            }
            return document;
        }
        #endregion


        #region 每个上传数据都需要使用此方法
        /// <summary>
        /// 每个上传数据都需要使用此方法
        /// </summary>
        /// <param name="document"></param>
        public string FinallySetXmlNs(System.Xml.XmlDocument document)
        {
            XmlNode rootNode = document.DocumentElement;
            //this.SetAttributeValue(document, rootNode, "xmlns", "\"http://www.section9.org/cms/referral/data\">");
            SrYb.NApiSoapService nApiSoapService = new YBInterface.SrYb.NApiSoapService();
 
            string xmlStr = document.InnerXml.Replace(" xmlns=\"\"", "");
            xmlStr = xmlStr.Replace("\r\n", "");
            //xmlStr = xmlStr.Replace("&gt;", ">");
            //System.Threading.Thread.Sleep(1000000);
            nApiSoapService.Timeout = 900000;//15分钟
            string result = nApiSoapService.nh_pipe(xmlStr);
            result = result.Replace("xmlns=\"http://www.section9.org/cms/referral/data\"", "");
            return result;
        }
        #endregion

        #region 显示头部返回的信息
        /// <summary>
        /// 显示头部返回的信息
        /// </summary>
        /// <param name="responseNode"></param>
        public bool ShowHeadMsg(XmlNode responseNode)
        {
            XmlNode headNode = responseNode.SelectSingleNode("head");
            XmlNode stateCodeNode = headNode.SelectSingleNode("stateCode");
            XmlNode describeNode = headNode.SelectSingleNode("describe");
            if (stateCodeNode.InnerText.Trim() != "0000000")
            {
                Utilities.Information.ShowMsgBox(describeNode.InnerText + stateCodeNode.InnerText);
                return false;
            }

            //Utilities.Information.ShowMsgBox(describeNode.InnerText + stateCodeNode.InnerText);
            return true;
        }
        #endregion



        #region 返回头部信息
        /// <summary>
        /// 返回头部信息
        /// </summary>
        /// <param name="responseNode"></param>
        public string RetHeadMsg(XmlNode responseNode)
        {
            XmlNode headNode = responseNode.SelectSingleNode("head");
            XmlNode stateCodeNode = headNode.SelectSingleNode("stateCode");
            XmlNode describeNode = headNode.SelectSingleNode("describe");
            return describeNode.InnerText + stateCodeNode.InnerText;
        }
        #endregion


        #region 转诊申请单查询
        /// <summary>
        /// 转诊申请单查询C020001
        /// </summary>
        /// <param name="MedicareNo">个人编码</param>
        public XmlDocument SearchReferral(String YbRegNo)
        {

            System.Xml.XmlDocument document = this.XmlDataExchangeModel("C020001", YbRegNo);
            XmlNode rootNode = document.DocumentElement;
            XmlNode bodyNode = rootNode["body"];
            XmlElement subNode = this.SetElementValue(document, "D507_01", "");
            subNode.InnerText = YbRegNo;
            bodyNode.AppendChild(subNode);
            string result = this.FinallySetXmlNs(document);
            document.LoadXml(result);
            XmlNode responseNode = document.SelectSingleNode("response");
            XmlNode bodySubNode = responseNode.SelectSingleNode("body");
            Utilities.Information.ShowMsgBox("转诊申请单查询" + RetHeadMsg(responseNode));
            if (bodySubNode == null)
            {
                Utilities.Information.ShowMsgBox("该患者没有转诊信息!" + responseNode.InnerText);
                return document;
            }
            XmlNode itemNode1 = bodySubNode["item"];
            if (itemNode1 == null)
            {
                Utilities.Information.ShowMsgBox("该患者没有转诊信息");
                return document;
            }
            XmlNode itemNode = bodySubNode.SelectSingleNode("item");

            StringBuilder sBuilder1 = new StringBuilder();
            sBuilder1.Append(result);

            Utilities.Document doc = new Utilities.Document();
            doc.Content = sBuilder1;
            doc.Create("c://转诊单.txt", false);

            return document;


        }
        #endregion
        #region 转诊申请单查询
        /// <summary>
        /// 转诊申请单查询C020001
        /// </summary>
        /// <param name="MedicareNo">个人编码</param>
        public string SearchReferral1(String YbRegNo)
        {
            System.Xml.XmlDocument document = this.XmlDataExchangeModel("C020001", YbRegNo);
            XmlNode rootNode = document.DocumentElement;
            XmlNode bodyNode = rootNode["body"];
            XmlElement subNode = this.SetElementValue(document, "D507_01", "");
            subNode.InnerText = YbRegNo;
            bodyNode.AppendChild(subNode);
            string result = this.FinallySetXmlNs(document);
            document.LoadXml(result);
            XmlNode responseNode = document.SelectSingleNode("response");
            XmlNode bodySubNode = responseNode.SelectSingleNode("body");
            Utilities.Information.ShowMsgBox("转诊申请单查询" + RetHeadMsg(responseNode));
            if (bodySubNode == null)
            {
                return "";
            }
            XmlNode itemNode1 = bodySubNode["item"];
            if (itemNode1 == null)
            {
                return "";
            }
            XmlNode itemNode = bodySubNode.SelectSingleNode("item");

            StringBuilder sBuilder1 = new StringBuilder();
            sBuilder1.Append(result);
            return itemNode.SelectSingleNode("D507_32").InnerText;
        }
        #endregion


        #region 家庭成员查询
        /// <summary>
        /// 家庭成员查询C030001
        /// </summary>
        /// <returns></returns>
        public object SearchFamily(string YbRegNo)
        {
            System.Xml.XmlDocument document = this.XmlDataExchangeModel("C030001", YbRegNo);
            XmlNode rootNode = document.DocumentElement;
            XmlNode bodyNode = rootNode["body"];
            bodyNode.AppendChild(this.SetElementValue(document, "D301_01", YbRegNo));//家庭编号
            bodyNode.AppendChild(this.SetElementValue(document, "D401_10", ""));//医疗证/卡号
            string result = this.FinallySetXmlNs(document);
            document.LoadXml(result);
            XmlNode responseNode = document.SelectSingleNode("response");
            if (!this.ShowHeadMsg(responseNode))
                return "失败";
            else
                return document;
        }
        /// <summary>
        /// 家庭成员查询C030001
        /// </summary>
        /// <param name="MedicareNo">个人编码</param>
        public Model.ModelList<Model.QzNhInterface.FamilyInfo> GetSearchFamilyInfo(String YbRegNo)
        {
            Model.ModelList<Model.QzNhInterface.FamilyInfo> lstReferralBillsInfo = new Model.ModelList<Model.QzNhInterface.FamilyInfo>();
            object document = SearchFamily(YbRegNo);
            if (!Convert.ToString(document).Contains("失败"))
            {
                XmlDocument docmumentNew = (XmlDocument)document;
                XmlNode responseNode = docmumentNew.SelectSingleNode("response");
                XmlNode bodySubNode = responseNode.SelectSingleNode("body");
                Utilities.Information.ShowMsgBox("家庭成员查询" + RetHeadMsg(responseNode));
                if (bodySubNode == null)
                {
                    lstReferralBillsInfo = null;
                    return lstReferralBillsInfo;
                }
                int id = 0;
                foreach (System.Xml.XmlNode itemNode in bodySubNode)
                {
                    Model.QzNhInterface.FamilyInfo familyInfoInfo = new Model.QzNhInterface.FamilyInfo();

                    familyInfoInfo.YbRegNo1 = itemNode.SelectSingleNode("D401_21_A").InnerText;
                    familyInfoInfo.FamilyNo = itemNode.SelectSingleNode("D301_01").InnerText;
                    familyInfoInfo.CHYear = itemNode.SelectSingleNode("D506_55").InnerText;
                    familyInfoInfo.IdCardNo = itemNode.SelectSingleNode("D401_01").InnerText;
                    familyInfoInfo.Name = itemNode.SelectSingleNode("D401_02").InnerText;
                    familyInfoInfo.Sex = itemNode.SelectSingleNode("D401_03").InnerText;
                    familyInfoInfo.Birthday = itemNode.SelectSingleNode("D401_04").InnerText;
                    familyInfoInfo.BirthAdress = itemNode.SelectSingleNode("D401_05").InnerText;
                    familyInfoInfo.National = itemNode.SelectSingleNode("D401_06").InnerText;
                    familyInfoInfo.Marriaged = itemNode.SelectSingleNode("D401_07").InnerText;
                    familyInfoInfo.YBCardID = itemNode.SelectSingleNode("D401_10").InnerText;
                    familyInfoInfo.Company = itemNode.SelectSingleNode("D401_11").InnerText;
                    familyInfoInfo.ZY = itemNode.SelectSingleNode("D401_12").InnerText;
                    familyInfoInfo.Phone = itemNode.SelectSingleNode("D401_15").InnerText;
                    familyInfoInfo.Adress = itemNode.SelectSingleNode("D401_13").InnerText;
                    familyInfoInfo.PostCode = itemNode.SelectSingleNode("D401_14").InnerText;
                    familyInfoInfo.LinkName = itemNode.SelectSingleNode("D401_16").InnerText;
                    familyInfoInfo.LinkPhone = itemNode.SelectSingleNode("D401_17").InnerText;
                    familyInfoInfo.Email = itemNode.SelectSingleNode("D401_18").InnerText;
                    familyInfoInfo.PeopleAttribute = itemNode.SelectSingleNode("D401_50").InnerText;
                    id = id + 1;
                    familyInfoInfo.ID = id;
                    lstReferralBillsInfo.Add(familyInfoInfo);
                }
                id = 0;
                return lstReferralBillsInfo;
            }
            else
            {
                lstReferralBillsInfo = null;
                return lstReferralBillsInfo;
            }
        }
        #endregion

        public Model.QzNhInterface.ReferralBills GetReferralInfo(String YbRegNo)
        {
            Model.QzNhInterface.ReferralBills referralBillsInfo = new Model.QzNhInterface.ReferralBills();
            XmlDocument document = SearchReferral(YbRegNo);
            XmlNode responseNode = document.SelectSingleNode("response");
            XmlNode bodySubNode = responseNode.SelectSingleNode("body");
            if (bodySubNode == null)
            {
                referralBillsInfo = null;
                return referralBillsInfo;
            }
            XmlNode itemNode = bodySubNode.SelectSingleNode("item");
            if (itemNode == null)
            {
                referralBillsInfo = null;
                return referralBillsInfo;
            }
            XmlNode itemSubNode = itemNode.SelectSingleNode("item");

            referralBillsInfo.Referral = itemNode.SelectSingleNode("D507_32").InnerText;
            referralBillsInfo.YbRegNo = itemNode.SelectSingleNode("D507_01").InnerText;
            referralBillsInfo.Name = itemNode.SelectSingleNode("D507_02").InnerText;
            referralBillsInfo.Sex = itemNode.SelectSingleNode("D507_37").InnerText; ;
            referralBillsInfo.IllCode = itemNode.SelectSingleNode("D507_03").InnerText;
            referralBillsInfo.AuthDesc = itemNode.SelectSingleNode("D507_11").InnerText;
            referralBillsInfo.ApplyDate = itemNode.SelectSingleNode("D507_04").InnerText;
            referralBillsInfo.AuthTime = itemNode.SelectSingleNode("D507_10").InnerText;
            referralBillsInfo.HGPhone = itemNode.SelectSingleNode("D507_31").InnerText;
            referralBillsInfo.MedicalOuCode = itemNode.SelectSingleNode("D507_33").InnerText;
            referralBillsInfo.OrgnaizationMedical = itemNode.SelectSingleNode("D507_06").InnerText;
            referralBillsInfo.Phone = itemNode.SelectSingleNode("D507_38").InnerText;
            referralBillsInfo.IdCardNo = itemNode.SelectSingleNode("D507_36").InnerText;
            referralBillsInfo.IllName = itemNode.SelectSingleNode("D507_30").InnerText;
            referralBillsInfo.MedicalOuName = itemNode.SelectSingleNode("D507_07").InnerText;
            referralBillsInfo.ApplyDesc = itemNode.SelectSingleNode("D507_05").InnerText;
            referralBillsInfo.MedicalInName = itemSubNode.SelectSingleNode("D507_08").InnerText;
            referralBillsInfo.MedicalOuCode = itemSubNode.SelectSingleNode("D507_34").InnerText;
            referralBillsInfo.AuthStatus = itemNode.SelectSingleNode("D507_09").InnerText;
            referralBillsInfo.AuthName = itemNode.SelectSingleNode("D507_12").InnerText;
            return referralBillsInfo;
        }


        #region 连续住院可依附结算记录列表C020009
        /// <summary>
        /// 连续住院可依附结算记录列表C020009
        /// </summary>
        /// <returns></returns>
        public override object InHosChargeCheckIn(string isTumor)
        {
            System.Xml.XmlDocument document = this.XmlDataExchangeModel("C020009", InfoInHosInfo.YbRegNo);
            XmlNode rootNode = document.DocumentElement;
            XmlNode bodyNode = rootNode["body"];
            bodyNode.AppendChild(this.SetElementValue(document, "D401_21_A", InfoInHosInfo.YbRegNo));//个人编码
            bodyNode.AppendChild(this.SetElementValue(document, "D504_49", isTumor));//是否恶性肿瘤
            bodyNode.AppendChild(this.SetElementValue(document, "D603_02", ""));//年份
            string result = this.FinallySetXmlNs(document);
            document.LoadXml(result);
            XmlNode responseNode = document.SelectSingleNode("response");
            if (!this.ShowHeadMsg(responseNode))
                return "失败";
            else
                return document;
        }

        public object GetInHosChargeCheckInInfo(string isTumor)
        {

            List<Model.QzNhInterface.GetInHosChargeCheckInInfo> lstGetInHosChargeCheckInInfo = new List<Model.QzNhInterface.GetInHosChargeCheckInInfo>();
            object document = InHosChargeCheckIn(isTumor);
            if (!Convert.ToString(document).Contains("失败"))
            {
                XmlDocument docmumentNew = (XmlDocument)document;
                XmlNode responseNode = docmumentNew.SelectSingleNode("response");
                XmlNode bodySubNode = responseNode.SelectSingleNode("body");
                if (bodySubNode == null)
                {
                    lstGetInHosChargeCheckInInfo = null;
                    return "无连续住院可依附结算记录";
                }
                foreach (System.Xml.XmlNode itemNode in bodySubNode)
                {
                    Model.QzNhInterface.GetInHosChargeCheckInInfo GetInHosChargeCheckInInfo = new Model.QzNhInterface.GetInHosChargeCheckInInfo();
                    GetInHosChargeCheckInInfo.OrgnaizationMedical = itemNode.SelectSingleNode("D504_61").InnerText;
                    GetInHosChargeCheckInInfo.YbRegNo = itemNode.SelectSingleNode("D504_62").InnerText;
                    GetInHosChargeCheckInInfo.CumulativePayLine = itemNode.SelectSingleNode("D506_94").InnerText;
                    GetInHosChargeCheckInInfo.CompensationDate = itemNode.SelectSingleNode("D506_26").InnerText;
                    GetInHosChargeCheckInInfo.IllName = itemNode.SelectSingleNode("D504_76").InnerText;
                    lstGetInHosChargeCheckInInfo.Add(GetInHosChargeCheckInInfo);
                }
                return lstGetInHosChargeCheckInInfo;
            }
            return "失败";
        }
        #endregion


        #region 住院登记或者修改，修改需要存在住院登记流水号(RegNo)不为空

        public string TransferType { set; get; }
        public string IllCodeNh { set; get; }
        public string IllDescNh { set; get; }
        /// <summary>
        /// 住院登记或者修改，修改需要存在住院登记流水号(RegNo)不为空
        /// </summary>
        /// <returns></returns>
        public override string InHosInfoCheckIn(string SeeCategories, string RegisterProperty)
        {
            //Model.QzNhInterface.ReferralBills infoReferralBills = new Model.QzNhInterface.ReferralBills();
            //infoReferralBills=GetReferralInfo(InfoInHosInfo.YbRegNo);
            //if (infoReferralBills!=null)
            //{
            //   MessageBox.Show(string.Format("该病人是转诊病人，转诊单号是{0}!",infoReferralBills.Referral));
            //}
            //string str = this.GetDicticnary("24");
            bool IsModify = !string.IsNullOrEmpty(YbSeq);
            System.Xml.XmlDocument document = this.XmlDataExchangeModel(IsModify ? "B020002" : "B020001", InfoInHosInfo.YbRegNo);
            XmlNode rootNode = document.DocumentElement;
            XmlNode bodyNode = rootNode["body"];

            #region basicInfo
            XmlElement eleBasic = this.SetElementValue(document, "basicInfo", "");
            XmlElement subEle = this.SetElementValue(document, "D504_47", "");//infoReferralBills.Referral);
            eleBasic.AppendChild(subEle);

            int man = 1, female = 2, elseMan = 0;


            if (InfoInHosInfo.Sex == "M" || InfoInHosInfo.Sex == "1")
            {
                InfoInHosInfo.Sex = man.ToString();
            }
            else if (InfoInHosInfo.Sex == "F" || InfoInHosInfo.Sex == "2")
            {
                InfoInHosInfo.Sex = female.ToString();
            }
            else
            {
                InfoInHosInfo.Sex = elseMan.ToString();
            }

            if (IsModify) //住院登记流水号不为空即为登记过，只需要修改即可
                eleBasic.AppendChild(this.SetElementValue(document, "D504_01", YbSeq));// InfoInHosInfo.YbRegNo));

            eleBasic.AppendChild(this.SetElementValue(document, "D504_02", InfoInHosInfo.YbRegNo));//个人编码 
            eleBasic.AppendChild(this.SetElementValue(document, "D504_03", InfoInHosInfo.Name));//患者姓名 
            eleBasic.AppendChild(this.SetElementValue(document, "D504_04", InfoInHosInfo.Sex));//患者性别 1男 2女 0其他
            eleBasic.AppendChild(this.SetElementValue(document, "D504_05", InfoInHosInfo.IdCardNo));//患者身份证号 
            eleBasic.AppendChild(this.SetElementValue(document, "D504_06", InfoInHosInfo.F2));//年龄 
            eleBasic.AppendChild(this.SetElementValue(document, "D504_07", ""));//家庭编号  



            eleBasic.AppendChild(this.SetElementValue(document, "D504_08", InfoInHosInfo.MedicareNo));//医疗证卡号 
            eleBasic.AppendChild(this.SetElementValue(document, "D603_02", BLL.Common.DateTimeHandler.GetServerDateTime().ToString("yyyy")));//基金年份 
            eleBasic.AppendChild(this.SetElementValue(document, "D504_28", BLL.Common.DateTimeHandler.GetServerDateTime().ToString("MM")));//月份 
            bodyNode.AppendChild(eleBasic);
            #endregion


            #region registerInfo
            XmlElement eleRegister = document.CreateElement("registerInfo");



            Model.ModelList<Model.BsBedInfo> lstBsBedYb = _objBsBed.BsBed_SelectById(InfoInHosInfo.BedId);
            //if (IllCode.Length <= 20)
            //{
            //    for (int i = 0; i <= 20-IllCode.Length; i++)
            //    {   
            //        IllCode += " ";
            //    }    
            //}
            BLL.CInHosMzIll objInHosMzIll = new BLL.CInHosMzIll();
            foreach (var model in objInHosMzIll.InHosMzIll_SelectByHospId(this.InfoInHosInfo.ID))
            {
                if (model.IcdId == 0) continue;
                Model.BsIllnessInfo infoBsIllness = new BLL.CBsIllness().GetByID(model.IcdId);

                //this.IllCode = this.InfoPatType.Name.Contains("大病") ? infoBsIllness.F2.Trim() : infoBsIllness.Code.Trim();
                this.IllName = infoBsIllness.Name;
                //eleRegister.AppendChild(this.SetElementValue(document, "D504_80", infoBsIllness.Code.Trim()));//HIS疾病代码
                //eleRegister.AppendChild(this.SetElementValue(document, "D504_81", infoBsIllness.Name));// HIS疾病名称
                break;
            }

            if (!IsModify)
            {
                eleRegister.AppendChild(this.SetElementValue(document, "D504_48", RegisterProperty));//登记属性
                eleRegister.AppendChild(this.SetElementValue(document, "D504_10", SeeCategories));//就诊类型
                eleRegister.AppendChild(this.SetElementValue(document, "D504_21", IllCodeNh));//infoReferralBills.IllCode));//"050219F19.201       "));//IllCode));//疾病代码
                eleRegister.AppendChild(this.SetElementValue(document, "D504_49", "0"));//是否恶性肿瘤
                eleRegister.AppendChild(this.SetElementValue(document, "D504_50", IllDescNh));//infoReferralBills.IllName));//"A.P.C药物瘾"));// IllName));//医院初步诊断
                eleRegister.AppendChild(this.SetElementValue(document, "D504_16", ""));//_objBsLocation.GetByID(InfoInHosInfo.LocIn)));//InfoInHosInfo.LocIn));//入院科室
                eleRegister.AppendChild(this.SetElementValue(document, "D504_17", ""));//_objBsLocation.GetByID(InfoInHosInfo.LocationId)));//InfoInHosInfo));//出院科室
                eleRegister.AppendChild(this.SetElementValue(document, "D504_51", ""));//_objBsBed.GetByID(InfoInHosInfo.BedId)));// InfoInHosInfo.BedId.ToString()));//登记床位
                eleRegister.AppendChild(this.SetElementValue(document, "D504_52", ""));//入院类型
                eleRegister.AppendChild(this.SetElementValue(document, "D504_19", ""));//入院状态
                eleRegister.AppendChild(this.SetElementValue(document, "D504_66", ""));//银行卡号
                eleRegister.AppendChild(this.SetElementValue(document, "D504_56", ""));//担保金额
                eleRegister.AppendChild(this.SetElementValue(document, "D504_54", InfoInHosInfo.LocationId.ToString()));//HIS科室代码
                eleRegister.AppendChild(this.SetElementValue(document, "D504_57", ""));//担保人
                eleRegister.AppendChild(this.SetElementValue(document, "D504_58", ""));//登记人
                eleRegister.AppendChild(this.SetElementValue(document, "D504_13", InfoInHosInfo.NTime.ToString()));//住院次数
                eleRegister.AppendChild(this.SetElementValue(document, "D101_02", "钦州市中医医院（定点）"));//就医机构名称
                //eleRegister.AppendChild(this.SetElementValue(document, "D504_14", "504"));//就医机构代码
                //foreach (YBInterface.NhDictionary item in this.GetNhDictionary("23"))
                //{
                //    if (!string.IsNullOrEmpty(item.D911_02))
                //    {
                //        eleRegister.AppendChild(this.SetElementValue(document, "D504_14", item.D911_02));//就医机构代码  
                //    }
                //}
                eleRegister.AppendChild(this.SetElementValue(document, "D504_14", this.GetNhZd("就医机构", "23")));//就医机构代码  

                eleRegister.AppendChild(this.SetElementValue(document, "D504_15", ""));//就医机构级别
                eleRegister.AppendChild(this.SetElementValue(document, "D504_12", ""));//InfoInHosInfo.OutTime.ToString("yyyy-MM-dd HH-mm-ss").Substring(0, 10) + "T" + InfoInHosInfo.OutTime.ToString("yyyy-MM-dd HH-mm-ss").Substring(11, 8)));//出院时间
                eleRegister.AppendChild(this.SetElementValue(document, "D504_62", ""));//住院补偿流水号
                //eleRegister.AppendChild(this.SetElementValue(document, "D504_63", TransferType));//转外类型
                eleRegister.AppendChild(this.SetElementValue(document, "D504_55", ""));//症状名称
                eleRegister.AppendChild(this.SetElementValue(document, "D504_23", ""));//手术名称代码
                eleRegister.AppendChild(this.SetElementValue(document, "D504_20", ""));//出院
                eleRegister.AppendChild(this.SetElementValue(document, "D504_53", ""));//押金金额
                eleRegister.AppendChild(this.SetElementValue(document, "D504_11", InfoInHosInfo.InTime.ToString("yyyy-MM-ddTHH-mm-ss"))); //入院时间yyyy-MM-dd HH-mm-ss
                eleRegister.AppendChild(this.SetElementValue(document, "D504_09", InfoInHosInfo.InPatNo + InfoInHosInfo.NTime.ToString()));//住院号
                eleRegister.AppendChild(this.SetElementValue(document, "D504_59", "0"));//"0"));//是否商业保险
                eleRegister.AppendChild(this.SetElementValue(document, "D504_18", InfoInHosInfo.DoctorId.ToString()));//经治医生
            }

            if (IsModify)
            {
                eleRegister.AppendChild(this.SetElementValue(document, "D504_48", RegisterProperty));//登记属性
                eleRegister.AppendChild(this.SetElementValue(document, "D504_10", SeeCategories));//就诊类型
                eleRegister.AppendChild(this.SetElementValue(document, "D504_49", "0"));//是否恶性肿瘤
                eleRegister.AppendChild(this.SetElementValue(document, "D504_21", IllCodeNh));//infoReferralBills.IllCode));//"050219F19.201       "));//IllCode));//疾病代码
                eleRegister.AppendChild(this.SetElementValue(document, "D504_50", IllDescNh));//infoReferralBills.IllName));//"A.P.C药物瘾"));// IllName));//医院初步诊断
                eleRegister.AppendChild(this.SetElementValue(document, "D504_16", ""));//_objBsLocation.GetByID(InfoInHosInfo.LocIn)));//InfoInHosInfo.LocIn));//入院科室
                eleRegister.AppendChild(this.SetElementValue(document, "D504_17", ""));//_objBsLocation.GetByID(InfoInHosInfo.LocationId)));//InfoInHosInfo.LocIn));//出院科室
                eleRegister.AppendChild(this.SetElementValue(document, "D504_51", ""));// _objBsBed.GetByID(InfoInHosInfo.BedId)));//登记床位
                eleRegister.AppendChild(this.SetElementValue(document, "D504_52", ""));//入院类型
                eleRegister.AppendChild(this.SetElementValue(document, "D504_53", ""));//押金金额
                eleRegister.AppendChild(this.SetElementValue(document, "D504_11", InfoInHosInfo.InTime.ToString("yyyy-MM-dd HH-mm-ss").Substring(0, 10) + "T" + InfoInHosInfo.InTime.ToString("yyyy-MM-dd HH-mm-ss").Substring(11, 8)));//入院时间yyyy-MM-dd HH-mm-ss
                eleRegister.AppendChild(this.SetElementValue(document, "D504_12", ""));//InfoInHosInfo.OutTime.ToString("yyyy-MM-dd HH-mm-ss").Substring(0, 10) + "T" + InfoInHosInfo.OutTime.ToString("yyyy-MM-dd HH-mm-ss").Substring(11, 8)));//出院时间
                eleRegister.AppendChild(this.SetElementValue(document, "D504_13", InfoInHosInfo.NTime.ToString()));//实际住院天数
                eleRegister.AppendChild(this.SetElementValue(document, "D101_02", "钦州市中医医院（定点）"));//BLL.Common.Utils.GetSystemSetting("HospitalName")));//就医机构名称

                eleRegister.AppendChild(this.SetElementValue(document, "D504_14", this.GetNhZd("就医机构", "23")));//就医机构代码  

                eleRegister.AppendChild(this.SetElementValue(document, "D504_15", ""));//就医机构级别
                eleRegister.AppendChild(this.SetElementValue(document, "D504_18", ""));//InfoInHosInfo.DoctorId.ToString()));//经治医生
                eleRegister.AppendChild(this.SetElementValue(document, "D504_19", ""));//入院状态
                eleRegister.AppendChild(this.SetElementValue(document, "D504_20", ""));//出院
                eleRegister.AppendChild(this.SetElementValue(document, "D504_55", ""));//症状名称
                eleRegister.AppendChild(this.SetElementValue(document, "D504_23", ""));//手术名称代码
                eleRegister.AppendChild(this.SetElementValue(document, "D504_56", ""));//担保金额
                eleRegister.AppendChild(this.SetElementValue(document, "D504_09", InfoInHosInfo.InPatNo + InfoInHosInfo.NTime.ToString()));//住院号
                eleRegister.AppendChild(this.SetElementValue(document, "D504_57", ""));//担保人
                eleRegister.AppendChild(this.SetElementValue(document, "D504_58", ""));//登记人
                eleRegister.AppendChild(this.SetElementValue(document, "D504_59", ""));//"0"));//是否商业保险
                eleRegister.AppendChild(this.SetElementValue(document, "D504_54", InfoInHosInfo.LocationId.ToString()));//HIS科室代码
            }
            eleRegister.AppendChild(this.SetElementValue(document, "D504_60", _objBsLocation.GetByID(InfoInHosInfo.LocationId).Name));//InfoInHosInfo.LocIn.ToString()));//HIS科室名称
            eleRegister.AppendChild(this.SetElementValue(document, "D504_61", ""));//经办机构代码
            if (IsModify)
                eleRegister.AppendChild(this.SetElementValue(document, "D504_62", ""));//住院补偿流水号
            eleRegister.AppendChild(this.SetElementValue(document, "D504_63", TransferType));//.PadLeft(5, '0')));//转外类型
            eleRegister.AppendChild(this.SetElementValue(document, "D504_64", "0"));//是否急诊
            eleRegister.AppendChild(this.SetElementValue(document, "D504_65", ""));//账户名
            if (IsModify)
                eleRegister.AppendChild(this.SetElementValue(document, "D504_66", ""));//银行卡号
            eleRegister.AppendChild(this.SetElementValue(document, "D504_67", ""));//转诊理由
            eleRegister.AppendChild(this.SetElementValue(document, "D504_69", ""));//转出医疗机构名称
            eleRegister.AppendChild(this.SetElementValue(document, "D504_68", ""));//转出医疗机构代码
            eleRegister.AppendChild(this.SetElementValue(document, "D504_70", InfoInHosInfo.OperTime.ToString("yyyy-MM-dd HH-mm-ss").Substring(0, 10)));//登记日期
            eleRegister.AppendChild(this.SetElementValue(document, "D504_22", ""));//并发症
            eleRegister.AppendChild(this.SetElementValue(document, "D504_26", ""));//民政通知书号
            eleRegister.AppendChild(this.SetElementValue(document, "D504_27", ""));//生育证号
            //eleRegister.AppendChild(this.SetElementValue(document, "D504_80", IllCode));//HIS疾病代码
            //eleRegister.AppendChild(this.SetElementValue(document, "D504_81", IllName));// HIS疾病名称
            //BLL.CInHosMzIll objInHosMzIll = new BLL.CInHosMzIll();
            //foreach (var model in objInHosMzIll.InHosMzIll_SelectByHospId(this.InfoInHosInfo.ID))
            //{
            //    if (model.IcdId == 0) continue;
            //    Model.BsIllnessInfo infoBsIllness = new BLL.CBsIllness().GetByID(model.IcdId);
            eleRegister.AppendChild(this.SetElementValue(document, "D504_80", this.IllCode));//HIS疾病代码
            eleRegister.AppendChild(this.SetElementValue(document, "D504_81", this.IllName));// HIS疾病名称
            //    break;
            //}

            eleRegister.AppendChild(this.SetElementValue(document, "D401_15", InfoInHosInfo.Phone));// 电话号码
            bodyNode.AppendChild(eleRegister);

            #endregion
            StringBuilder sBuilderXml = new StringBuilder();
            sBuilderXml.Append(document.InnerXml);
            Utilities.Document docXml = new Utilities.Document();
            docXml.Content = sBuilderXml;
            docXml.Create("c://住院登记修改(输入).xml", false);

            string result = this.FinallySetXmlNs(document);

            document.LoadXml(result);
            //Model.ModelList<YBInterface.NhDictionary> lstNhDictionary = this.GetNhDictionary("11");
            //foreach (YBInterface.NhDictionary item in lstNhDictionary)
            //{
            //    if (!string.IsNullOrEmpty(item.D911_02))
            //    {
            //        eleRegister.AppendChild(this.SetElementValue(document, "D504_14", item.D911_02));//就医机构代码  
            //    }
            //}
            //Model.ModelList<YBInterface.NhDictionary> lst = lstNhDictionary.Find("D911_03", IllName);
            XmlNode responseNode = document.SelectSingleNode("response");
            XmlNode headNode = responseNode.SelectSingleNode("head");
            XmlNode describeNode = headNode.SelectSingleNode("describe");
            if (!this.ShowHeadMsg(responseNode))
            {
                sBuilderXml = new StringBuilder();
                sBuilderXml.Append(document.InnerXml);
                docXml = new Utilities.Document();
                docXml.Content = sBuilderXml;
                docXml.Create("c://住院登记修改(输出).xml", false);
                return "失败";
            }
            Utilities.Information.ShowMsgBox("入院登记/修改" + RetHeadMsg(responseNode));
            //this.ShowHeadMsg(responseNode);
            if (!IsModify)
            {
                XmlNode bodySubNode = responseNode.SelectSingleNode("body");
                if (bodySubNode == null)
                {
                    return "失败";
                }
                XmlNode resultNode = bodySubNode.SelectSingleNode("D504_01");
                if (resultNode == null)
                    return "失败";
                BLL.CYbPatSeq objYbPatSeqInfo = new BLL.CYbPatSeq();
                Model.YbPatSeqInfo infoYbpatSeq = new Model.YbPatSeqInfo();
                //objYbPatSeqInfo.YbPatSeq_DeleteByHospId(_infoInHosInfo.ID, null);
                infoYbpatSeq.HospId = _infoInHosInfo.ID;
                infoYbpatSeq.YbSeq = resultNode.InnerText;//this.RegNo;
                infoYbpatSeq.F1 = this.InfoTallyGroup.ID.ToString();
                objYbPatSeqInfo.Create(infoYbpatSeq, null);
            }
            else
            {
                if (!this.ShowHeadMsg(responseNode))
                {
                    return "失败";
                }
                //else return "成功";
                //XmlNode bodySubNode = responseNode.SelectSingleNode("body");
                //if (bodySubNode == null)
                //{
                //    return describeNode.InnerText;
                //}
                //XmlNode resultNode = bodyNode.SelectSingleNode("D504_01");
                //if (resultNode == null)
                //    return describeNode.InnerText;
                //Model.InHosInfoInfo infoInHosInfo = _objInHosInfo.GetByID(_infoInHosInfo.ID);
                //infoInHosInfo.YbRegNo = this.RegNo = InfoInHosInfo.YbRegNo = resultNode.InnerText;
                //_objInHosInfo.Modify(infoInHosInfo, null);
                //BLL.CYbPatSeq objYbPatSeqInfo = new BLL.CYbPatSeq();
                //Model.YbPatSeqInfo infoYbpatSeq = new Model.YbPatSeqInfo();
                //objYbPatSeqInfo.YbPatSeq_DeleteByHospId(_infoInHosInfo.ID, null);
                //infoYbpatSeq.HospId = _infoInHosInfo.ID;
                //infoYbpatSeq.YbSeq = this.RegNo;
                //infoYbpatSeq.F1 = this.InfoTallyGroup.ID.ToString();
                //objYbPatSeqInfo.Create(infoYbpatSeq, null);
            }
            if (describeNode.InnerText.Contains("已结算") || describeNode.InnerText.Contains("出院"))
            {
                return "失败";
            }
            StringBuilder sBuilder1 = new StringBuilder();
            sBuilder1.Append(result);
            Utilities.Document doc = new Utilities.Document();
            doc.Content = sBuilder1;
            doc.Create("c://住院登记修改(输出).xml", false);
            return "成功";
        }

        #endregion

        #region 删除住院登记
        /// <summary>
        /// 删除住院登记
        /// </summary>
        /// <param name="reason"></param>
        /// <returns></returns>
        public string CancelUpLoadInFee(string YbSeq, string YbRegNo, string InPatNo, int HosId)
        {
            System.Xml.XmlDocument document = this.XmlDataExchangeModel("B020003", YbRegNo);
            XmlNode rootNode = document.DocumentElement;
            // XmlNode bodyNode = rootNode.SelectSingleNode("body");
            XmlNode bodyNode = rootNode["body"];


            //XmlElement eleItem = this.SetElementValue(document, "item", "");
            bodyNode.AppendChild(this.SetElementValue(document, "D504_01", YbSeq));//住院登记流水号"000001"));//
            bodyNode.AppendChild(this.SetElementValue(document, "D504_02", YbRegNo));//个人编码"450703080501000201"));//
            bodyNode.AppendChild(this.SetElementValue(document, "D504_09", InPatNo));//住院号"1315088"));//

            string result = this.FinallySetXmlNs(document);

            document.LoadXml(result);
            XmlNode responseNode = document.SelectSingleNode("response");

            if (!this.ShowHeadMsg(responseNode))
            {
                if (responseNode.InnerText.Contains("E172003住院号不唯一或者无此住院号"))
                    return "失败" + responseNode.InnerText;
                return "失败";
            }
            else
            {
                Utilities.Information.ShowMsgBox("删除住院登记" + RetHeadMsg(responseNode));
                BLL.CYbPatSeq objYbPatSeqInfo = new BLL.CYbPatSeq();
                Model.YbPatSeqInfo infoYbpatSeq = new Model.YbPatSeqInfo();
                objYbPatSeqInfo.YbPatSeq_DeleteByHospId(HosId, null);
                return "成功";
            }
        }
        #endregion


        #region  删除住院记账
        /// <summary>
        /// 删除住院记账
        /// </summary>
        /// <param name="reason"></param>
        /// <returns></returns>
        public override string deleteUpLoadInFee()
        {
            System.Xml.XmlDocument document = this.XmlDataExchangeModel("B020015", "");
            XmlNode rootNode = document.DocumentElement;
            // XmlNode bodyNode = rootNode.SelectSingleNode("body");
            XmlNode bodyNode = rootNode["body"];


            //XmlElement eleItem = this.SetElementValue(document, "item", "");
            bodyNode.AppendChild(this.SetElementValue(document, "D504_01", YbSeq));//住院登记流水号

            string result = this.FinallySetXmlNs(document);

            document.LoadXml(result);
            XmlNode responseNode = document.SelectSingleNode("response");

            if (!this.ShowHeadMsg(responseNode))
            {
                return "失败";
            }
            //Utilities.Information.ShowMsgBox(RetHeadMsg(responseNode));
            return "成功";
        }
        #endregion

        StringBuilder sRoolbackQery = new StringBuilder();
        public void RollbackQuery()
        {
            if (SearchIn().Contains("成功"))
            {
                System.Xml.XmlDocument document = new System.Xml.XmlDocument();
                document.LoadXml(sRoolbackQery.ToString());

                XmlNode responseNode = document.SelectSingleNode("response");
                XmlNode bodySubNode = responseNode.SelectSingleNode("body");
                XmlNode itemNode = bodySubNode.SelectSingleNode("item");
                XmlNode baseInfoNode = itemNode.SelectSingleNode("baseInfo");
                XmlNode D504_01Node = baseInfoNode.SelectSingleNode("D504_01");


                BLL.CYbPatSeq objYbPatSeqInfo = new BLL.CYbPatSeq();
                if (_infoInHosInfo.ID > 0 && objYbPatSeqInfo.YbPatSeq_SelectByHospId(_infoInHosInfo.ID).Count == 0
                    && MessageBox.Show("医保入院已经存在登记，是否保存医保状态？", "系统提示",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Model.YbPatSeqInfo infoYbpatSeq = new Model.YbPatSeqInfo();
                    infoYbpatSeq.HospId = _infoInHosInfo.ID;
                    infoYbpatSeq.YbSeq = D504_01Node.InnerText;//this.RegNo;
                    infoYbpatSeq.F1 = this.InfoTallyGroup.ID.ToString();
                    objYbPatSeqInfo.Create(infoYbpatSeq, null);
                }
            }
        }


        #region 在院情况查询
        /// <summary>
        /// 在院情况查询
        /// </summary>
        /// <param name="reason"></param>
        /// <returns></returns>
        public override string SearchIn()
        {
            System.Xml.XmlDocument document = this.XmlDataExchangeModel("C020003", InfoInHosInfo.YbRegNo);
            XmlNode rootNode = document.DocumentElement;
            // XmlNode bodyNode = rootNode.SelectSingleNode("body");
            XmlNode bodyNode = rootNode["body"];


            //XmlElement eleItem = this.SetElementValue(document, "item", "");
            bodyNode.AppendChild(this.SetElementValue(document, "D504_01", YbSeq));//住院登记流水号
            bodyNode.AppendChild(this.SetElementValue(document, "D504_02", InfoInHosInfo.YbRegNo));//个人编码
            bodyNode.AppendChild(this.SetElementValue(document, "D504_09", InfoInHosInfo.InPatNo + InfoInHosInfo.NTime.ToString()));//住院号
            bodyNode.AppendChild(this.SetElementValue(document, "D504_78", InfoInHosInfo.InTime.ToString("yyyy-MM-ddTHH-mm-ss")));//起始登记日期
            bodyNode.AppendChild(this.SetElementValue(document, "D504_79", BLL.Common.DateTimeHandler.GetServerDateTime().ToString("yyyy-MM-ddTHH-mm-ss")));//终止登记日期
            bodyNode.AppendChild(this.SetElementValue(document, "D504_80", ""));//HIS疾病代码
            bodyNode.AppendChild(this.SetElementValue(document, "D504_81", ""));//HIS疾病名称

            string result = this.FinallySetXmlNs(document);

            document.LoadXml(result);
            XmlNode responseNode = document.SelectSingleNode("response");
            if (!this.ShowHeadMsg(responseNode))
            {
                return "失败";
            }
            XmlNode bodySubNode = responseNode.SelectSingleNode("body");
            if (bodySubNode == null)
            {
                MessageBox.Show("没有此病人记录或该病人已办理出院！");
                return "失败";
            }
            XmlNode itemSubNode = bodySubNode.SelectSingleNode("item");
            if (itemSubNode == null)
            {
                MessageBox.Show("没有此病人记录或该病人已办理出院！");
                return "失败";
            }
            XmlNode SubNode = itemSubNode.SelectSingleNode("baseInfo");
            if (SubNode == null)
            {
                MessageBox.Show("没有此病人记录或该病人已办理出院！");
                return "失败";
            }
            Utilities.Information.ShowMsgBox("在院情况查询" + RetHeadMsg(responseNode));
            StringBuilder sBuilder1 = new StringBuilder();
            sBuilder1.Append(result);
            sRoolbackQery = sBuilder1;
            Utilities.Document doc = new Utilities.Document();
            doc.Content = sBuilder1;
            doc.Create("c://在院情况.txt", false);
            return "成功";
            // MessageBox.Show(sBuilder1.ToString());
            //return SubNode.InnerText;
        }
        #endregion


        #region 住院结算记录查询
        /// <summary>
        /// 住院结算记录查询
        /// </summary>
        /// <param name="reason"></param>
        /// <returns></returns>
        public override string SearchInCharge(string YbRegNo, string InPatNo)
        {
            System.Xml.XmlDocument document = this.XmlDataExchangeModel("C020007", YbRegNo);
            XmlNode rootNode = document.DocumentElement;
            // XmlNode bodyNode = rootNode.SelectSingleNode("body");
            XmlNode bodyNode = rootNode["body"];


            //XmlElement eleItem = this.SetElementValue(document, "item", "");
            bodyNode.AppendChild(this.SetElementValue(document, "D504_02", YbRegNo));//个人编码
            bodyNode.AppendChild(this.SetElementValue(document, "D504_09", InPatNo));//住院号
            bodyNode.AppendChild(this.SetElementValue(document, "D506_97", ""));//InfoInHosInfo.InTime.ToString("yyyy-MM-ddTHH-mm-ss")));//起始登记日期
            bodyNode.AppendChild(this.SetElementValue(document, "D506_98", ""));//InfoInHosInfo.OutTime.ToString("yyyy-MM-ddTHH-mm-ss")));//终止登记日期

            string result = this.FinallySetXmlNs(document);

            document.LoadXml(result);
            XmlNode responseNode = document.SelectSingleNode("response");
            if (!this.ShowHeadMsg(responseNode))
            {
                return "失败";
            }
            XmlNode bodySubNode = responseNode.SelectSingleNode("body");
            if (bodySubNode == null)
            {
                MessageBox.Show("没有此病人住院结算记录！");
                return "失败";
            }
            else
            {
                Utilities.Information.ShowMsgBox("住院结算记录查询" + RetHeadMsg(responseNode));
                XmlNode itemNode = bodySubNode.SelectSingleNode("item");
                string inRegNo = itemNode.SelectSingleNode("itemNode").InnerText;
                StringBuilder sBuilder1 = new StringBuilder();
                sBuilder1.Append(result);

                Utilities.Document doc = new Utilities.Document();
                doc.Content = sBuilder1;
                doc.Create("c://住院记录查询.txt", false);

                return inRegNo;
            }

            //return SubNode.InnerText;
        }
        #endregion


        #region 退出院办理
        /// <summary>
        /// 退出院办理
        /// </summary>
        /// <param name="reason"></param>
        /// <returns></returns>
        public string ReturnOu(string YbRegNo, string InPatNo)
        {

            System.Xml.XmlDocument document = this.XmlDataExchangeModel("B020014", YbRegNo);
            XmlNode rootNode = document.DocumentElement;
            // XmlNode bodyNode = rootNode.SelectSingleNode("body");
            XmlNode bodyNode = rootNode["body"];


            //XmlElement eleItem = this.SetElementValue(document, "item", "");
            bodyNode.AppendChild(this.SetElementValue(document, "D504_02", YbRegNo));//个人编码
            bodyNode.AppendChild(this.SetElementValue(document, "D504_09", InPatNo));//住院号
            bodyNode.AppendChild(this.SetElementValue(document, "D504_97", ""));//退出院原因
            bodyNode.AppendChild(this.SetElementValue(document, "D504_98", ""));//退出院人
            bodyNode.AppendChild(this.SetElementValue(document, "D504_99", ""));//退出院日期

            string result = this.FinallySetXmlNs(document);

            document.LoadXml(result);
            XmlNode responseNode = document.SelectSingleNode("response");
            if (!this.ShowHeadMsg(responseNode))
            {
                return "失败";
            }
            Utilities.Information.ShowMsgBox("退出院办理" + RetHeadMsg(responseNode));
            return "成功";
        }
        #endregion




        #region 获取字典
        /// <summary>
        /// 获取字典
        /// </summary>
        /// <returns></returns>
        public string GetDicticnary(string dicNo)
        {
            System.Xml.XmlDocument document = this.XmlDataExchangeModel("A040001", "");
            XmlNode rootNode = document.DocumentElement;
            XmlNode bodyNode = rootNode["body"];


            XmlElement eleItem = this.SetElementValue(document, "item", "");

            bodyNode.AppendChild(eleItem);


            rootNode = document.DocumentElement;
            bodyNode = rootNode["body"];
            XmlNodeList s = bodyNode.SelectNodes("item");
            for (int i = 0; i < s.Count; i++)
            {
                XmlElement items = this.SetElementValue(document, "D911_01", dicNo);
                s[i].AppendChild(items);
            }
            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append(this.FinallySetXmlNs(document));

            Utilities.Document doc = new Utilities.Document();
            doc.Content = sBuilder;
            doc.Create("c://dic.txt", false);
            MessageBox.Show(sBuilder.ToString());
            return "成功";
        }
        #endregion

        public string GetNodeValue(XmlNode parentNode, string nodeName)
        {
            return (parentNode[nodeName] == null) ? string.Empty :
               parentNode[nodeName].InnerText;
        }
        /// <summary>
        /// 获取字典
        /// </summary>
        /// <param name="dicNo">字典编号</param>
        /// <returns></returns>
        public Model.ModelList<NhDictionary> GetNhDictionary(string dicNo)
        {
            return GetNhDictionary(dicNo, string.Empty);
        }
        /// <summary>
        /// 获取字典
        /// </summary>
        /// <param name="dicNo">字典编号</param>
        /// <param name="YbRegNo">区域码</param>
        /// <returns></returns>
        public Model.ModelList<NhDictionary> GetNhDictionary(string dicNo, string YbRegNo)
        {
            System.Xml.XmlDocument document = this.XmlDataExchangeModel("A040001", YbRegNo);
            XmlNode rootNode = document.DocumentElement;
            XmlNode bodyNode = rootNode["body"];
            XmlElement eleItem = document.CreateElement("item");
            eleItem.AppendChild(this.SetElementValue(document, "D911_01", dicNo.PadLeft(2, '0')));//字典编号
            bodyNode.AppendChild(eleItem);

            string result = this.FinallySetXmlNs(document);
            document.LoadXml(result);
            rootNode = document.DocumentElement;
            XmlNode resBodyNode = rootNode["body"];
            Model.ModelList<NhDictionary> lstNhDictionary = new Model.ModelList<NhDictionary>();
            Model.ModelList<NhDictionary> lstNhDictionary1 = new Model.ModelList<NhDictionary>();
            if (resBodyNode == null) return lstNhDictionary;
            foreach (XmlNode itemNode in resBodyNode.SelectNodes("item"))
            {
                NhDictionary nhDictionary = new NhDictionary();
                nhDictionary.D911_01 = GetNodeValue(itemNode, "D911_01");
                #region baseInfo
                XmlNode baseInfoNode = itemNode.SelectSingleNode("baseInfo");
                nhDictionary.D911_02 = GetNodeValue(baseInfoNode, "D911_02").Trim();//代码 
                nhDictionary.D911_03 = GetNodeValue(baseInfoNode, "D911_03").Trim();//名称 
                nhDictionary.D911_04 = GetNodeValue(baseInfoNode, "D911_04").Trim();//拼音 
                nhDictionary.D911_05 = GetNodeValue(baseInfoNode, "D911_05").Trim();//备注  
                #endregion

                #region otherInfo
                XmlNode otherInfoNode = itemNode.SelectSingleNode("otherInfo");
                nhDictionary.D911_06 = GetNodeValue(otherInfoNode, "D911_06").Trim();//年份 
                nhDictionary.D911_07 = GetNodeValue(otherInfoNode, "D911_07").Trim();//区分目录和项目的标志 
                nhDictionary.D911_08 = GetNodeValue(otherInfoNode, "D911_08").Trim();//区划代码 
                nhDictionary.D911_09 = GetNodeValue(otherInfoNode, "D911_09").Trim();//机构等级 
                nhDictionary.D911_10 = GetNodeValue(otherInfoNode, "D911_10").Trim();//申报定点类型 
                nhDictionary.D911_11 = GetNodeValue(otherInfoNode, "D911_11").Trim();//审批定点类型 
                nhDictionary.D911_12 = GetNodeValue(otherInfoNode, "D911_12").Trim();//机构类别 
                nhDictionary.D911_13 = GetNodeValue(otherInfoNode, "D911_13").Trim();//是否定点 
                nhDictionary.D911_14 = GetNodeValue(otherInfoNode, "D911_14").Trim();//是否系统默认医院 
                nhDictionary.D911_15 = GetNodeValue(otherInfoNode, "D911_15").Trim();//机构状态 
                nhDictionary.D911_16 = GetNodeValue(otherInfoNode, "D911_16").Trim();//机构ID  
                #endregion

                lstNhDictionary.Add(nhDictionary);
            }
            if (dicNo == "25")
            {
                lstNhDictionary1 = lstNhDictionary.Find("D911_06", DateTime.Now.Year.ToString()) + lstNhDictionary.Find("D911_06", (DateTime.Now.Year - 1).ToString()).Find("D911_03", "普通住院");
                if (lstNhDictionary1.Count == 0)
                {
                    lstNhDictionary1 = lstNhDictionary.Find("D911_06", DateTime.Now.AddYears(-1).Year.ToString());
                }
                return lstNhDictionary1;
            }

            return lstNhDictionary;
        }


        /// <summary>
        /// 获取字典
        /// </summary>
        /// <param name="title"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public override string GetNhZd(string title, string code)
        {
            Tools.ToolGridLookupInput frm = new Tools.ToolGridLookupInput();
            frm.lblTitle.Text = title;
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("ID", System.Type.GetType("System.Int32"));
            dt.Columns.Add("Code", System.Type.GetType("System.String"));
            dt.Columns.Add("Value", System.Type.GetType("System.Int32"));
            dt.Columns.Add("Name", System.Type.GetType("System.String"));
            dt.Columns.Add("IconIndex", System.Type.GetType("System.Int32"));
            dt.Columns.Add("PyCode", System.Type.GetType("System.String"));
            dt.Columns.Add("WbCode", System.Type.GetType("System.String"));
            frm.uspLookup.Properties.Items.Clear();
            YBInterface.QZNhCommon qZNhCommon = new YBInterface.QZNhCommon();
            qZNhCommon.InfoInHosInfo = _infoInHosInfo.ConvertTo<Model.uspInHosInfoQry>();
            int index = 1;
            foreach (YBInterface.NhDictionary item in qZNhCommon.GetNhDictionary(code))
            {
                DevExpress.XtraEditors.Controls.ImageComboBoxItem imageBoxItem = new DevExpress.XtraEditors.Controls.ImageComboBoxItem();
                imageBoxItem.Value = index++;
                imageBoxItem.Description = item.D911_03;
                frm.uspLookup.Properties.Items.Add(imageBoxItem);

                System.Data.DataRow dr = dt.NewRow();
                dr["ID"] = imageBoxItem.Value;
                dr["Name"] = imageBoxItem.Description;
                dr["Code"] = item.D911_02;
                dr["IconIndex"] = 0;
                dt.Rows.Add(dr);
            }

            frm.uspLookup.ID = 0;
            frm.uspLookup._dv = dt.DefaultView;
            if (frm.uspLookup.DataSource.Count > 15)
                frm.uspLookup.Properties.DropDownRows = 15;
            if (frm.ShowDialog() == DialogResult.OK && frm.SelectedID > 0) return Convert.ToString(frm.uspLookup.SelectedRow["code"]);
            return string.Empty;
        }

        #region 获取财务分类
        /// <summary>
        /// 获取财务分类
        /// </summary>
        /// <returns></returns>
        public string GetYbStatType(string invItemName)
        {
            string invItemCode = string.Empty;
            switch (invItemName)
            {
                case "挂号费":
                    invItemCode = "A";//挂号费
                    break;
                case "诊查费"://
                    invItemCode = "B";
                    break;
                case "床位费"://
                    invItemCode = "C";//床位费
                    break;

                case "检查费":
                case "CT":
                case "MRI":
                case "X光":
                case "B超":
                case "心脑电图":
                case "MR费":
                    invItemCode = "D";//检查费
                    break;
                case "治疗费":
                case "输血费":
                case "输氧费":
                case "煎药费":
                    invItemCode = "E";//治疗费
                    break;
                case "护理费":
                    invItemCode = "F";//护理费
                    break;
                case "手术费":
                case "麻醉费":
                    invItemCode = "G";//手术费
                    break;
                case "检验费":
                    invItemCode = "H";
                    break;
                case "其它费用":
                    invItemCode = "I";
                    break;
                case "特殊材料费":
                    invItemCode = "J";
                    break;
                case "中医诊疗":
                    invItemCode = "K";
                    break;
                default:
                    invItemCode = "I";
                    break;
            }
            return invItemCode;
        }
        #endregion

        #region 批量住院记账B020013 按农合类别上传
        /// <summary>
        /// 批量住院记账B020013 按农合类别上传
        /// </summary>
        /// <returns></returns>
        public override object UpLoadInFeeNHType()
        {
            System.Xml.XmlDocument document = this.XmlDataExchangeModel("B020013", "");
            XmlNode rootNode = document.DocumentElement;
            XmlNode bodyNode = rootNode["body"];

            Model.ModelList<Model.BsUnitInfo> lstBsUnit = _objBsUnit.GetAll();
            Model.ModelList<Model.BsDoctorInfo> lstBsDoctor = _objBsDoctor.GetAll();
            Model.ModelList<Model.BsDrugFormInfo> lstBsDrugForm = _objBsDrugForm.GetAll();
            BLL.CBsItemYb _objBsItemYb = new BLL.CBsItemYb();

            bodyNode.AppendChild(this.SetElementValue(document, "D504_09", InfoInHosInfo.InPatNo + InfoInHosInfo.NTime.ToString()));//住院号


            XmlElement eleDetails = this.SetElementValue(document, "details", "");

            XmlElement eleD505_03_01 = this.SetElementValue(document, "D505_03_01", "");//药品
            XmlElement eleD505_30_01 = this.SetElementValue(document, "D505_30_01", "");//西药
            XmlElement eleD505_30_02 = this.SetElementValue(document, "D505_30_02", "");//中成药
            XmlElement eleD505_30_03 = this.SetElementValue(document, "D505_30_03", "");//中草药

            XmlElement eleD505_03_02 = this.SetElementValue(document, "D505_03_02", "");// 诊疗
            XmlElement eleD505_03_03 = this.SetElementValue(document, "D505_03_03", "");// 材料  

            bool isWestMedicine = false;//是否有西药项目
            bool isChinesePatentMedicine = false;//是否有中成药项目
            bool isChineseMedicine = false;//是否有中药项目
            bool isClinic = false;//是否有诊疗项目
            bool isMaterial = false;//是否有材料项目


            StringBuilder sBuilderMsg = new StringBuilder();
            foreach (var item in _lstUspInBalanceDtl)
            {
                if (item.Amount == 0)
                {
                    sBuilderMsg.AppendFormat("【{0}】【{1}】价格为零;", item.Code, item.Name);
                    sBuilderMsg.AppendLine();
                }
            }
            if (sBuilderMsg.Length > 0)
            {
                Utilities.Information.ShowMsgBox(sBuilderMsg.ToString() + ",请确认！");
                return "失败";
            }

            //30-01
            #region 30-01
            for (int i = 0; i < _lstUspInBalanceDtl.Count; i++)
            {

                Model.uspInBalanceDtlQry infoUspInBalanceDtl = _lstUspInBalanceDtl[i];
                Model.ModelList<Model.BsItemYbInfo> lstBsItemYb = _objBsItemYb.BsItemYb_SelectByItemIdAndTallyGroupId(infoUspInBalanceDtl.ItemId, this._inPatientInfo.TallyGroupId);
                if (lstBsItemYb.Count == 0) return string.Format("项目【{0}】【{1}】没有对应医保的病人大类，上传失败，请在项目代码中维护！", infoUspInBalanceDtl.Name, infoUspInBalanceDtl.Code);
                Model.BsItemYbInfo infoBsItemYbNh = new BsItemYbInfo();
                foreach (var item in lstBsItemYb)
                {
                    infoBsItemYbNh = item;
                }
                if (infoBsItemYbNh.ItemYbRpTypeId != 2)// || infoUspInBalanceDtl.F5.Trim() == "1")//已经上传的或者不是西药的不执行本次循环
                {
                    continue;
                }

                XmlElement eleItem30_01 = document.CreateElement("item");

                eleItem30_01.AppendChild(this.SetElementValue(document, "D505_04", infoUspInBalanceDtl.Code));//医院项目编码
                eleItem30_01.AppendChild(this.SetElementValue(document, "D505_16", infoUspInBalanceDtl.Name));//医院项目名称
                Model.ModelList<Model.BsItemDrugInfo> lstBsItemDrug = _objBsItemDrug.BsItemDrug_SelectByItemId(infoUspInBalanceDtl.ItemId);
                if (infoUspInBalanceDtl.LsRpType < 4)
                {
                    if (lstBsItemDrug.Count > 0)
                        eleItem30_01.AppendChild(this.SetElementValue(document, "D505_06", lstBsDrugForm.FindByID(lstBsItemDrug[0].FormId).Name));// 剂型
                    else
                        eleItem30_01.AppendChild(this.SetElementValue(document, "D505_06", "无"));// 剂型
                    eleItem30_01.AppendChild(this.SetElementValue(document, "D505_05", infoUspInBalanceDtl.Spec));// 规格
                }
                else
                {
                    eleItem30_01.AppendChild(this.SetElementValue(document, "D505_06", "无"));// 剂型
                }
                eleItem30_01.AppendChild(this.SetElementValue(document, "D505_07", Convert.ToDouble(infoUspInBalanceDtl.PriceIn).ToString("#0.0000")));//单价
                if (infoUspInBalanceDtl.Totality == 0) infoUspInBalanceDtl.Totality = 1.0;
                eleItem30_01.AppendChild(this.SetElementValue(document, "D505_08", infoUspInBalanceDtl.Totality));//数量
                eleItem30_01.AppendChild(this.SetElementValue(document, "D505_10", Convert.ToDouble(infoUspInBalanceDtl.Amount).ToString("#0.0000")));// 金额
                string strRegOperDate = string.Empty;
                if (infoUspInBalanceDtl.RegOperDate == string.Empty)
                    strRegOperDate = BLL.Common.DateTimeHandler.GetServerDateTime().ToString("yyyy-MM-ddTHH-mm-ss");
                else
                    strRegOperDate = Convert.ToDateTime(infoUspInBalanceDtl.RegOperDate).ToString("yyyy-MM-ddT00-00-00");
                eleItem30_01.AppendChild(this.SetElementValue(document, "D505_13", strRegOperDate));// 开单时间
                eleItem30_01.AppendChild(this.SetElementValue(document, "D505_33", ""));// 合作标识
                eleItem30_01.AppendChild(this.SetElementValue(document, "D505_34", ""));// 转外医疗机构
                if (lstBsItemYb.Count == 0 || lstBsItemYb[0].YbCode.Trim() == string.Empty || lstBsItemYb[0].YbDesc.Trim() == string.Empty)
                {
                    infoUspInBalanceDtl.DiscIn = 1;
                    infoUspInBalanceDtl.F3 = "自费";
                    infoUspInBalanceDtl.LsGfType = 2;
                }
                if (!string.IsNullOrEmpty(lstBsItemYb[0].YbCode))
                {
                    eleItem30_01.AppendChild(this.SetElementValue(document, "D505_35", lstBsItemYb[0].YbCode));//医院项目编码

                }
                else
                {
                    eleItem30_01.AppendChild(this.SetElementValue(document, "D505_35", "his+" + infoUspInBalanceDtl.Code));//医院项目编码

                }
                if (!string.IsNullOrEmpty(lstBsItemYb[0].YbDesc) && !string.IsNullOrEmpty(lstBsItemYb[0].YbCode))
                {
                    eleItem30_01.AppendChild(this.SetElementValue(document, "D505_36", lstBsItemYb[0].YbDesc));//医院项目名称
                }
                else
                {
                    eleItem30_01.AppendChild(this.SetElementValue(document, "D505_36", "his+" + infoUspInBalanceDtl.Name));//医院项目名称
                }

                eleItem30_01.AppendChild(this.SetElementValue(document, "D505_31", lstBsUnit.FindByID(infoUspInBalanceDtl.UnitId).Name));// 包装 
                eleItem30_01.AppendChild(this.SetElementValue(document, "D505_12", lstBsDoctor.FindByID(infoUspInBalanceDtl.DoctorId).Name));// 医生姓名
                eleItem30_01.AppendChild(this.SetElementValue(document, "D505_01", RegNo));// 住院处方流水号
                eleItem30_01.AppendChild(this.SetElementValue(document, "D505_14", infoUspInBalanceDtl.ID));// 明细费用流水号
                if (infoBsItemYbNh.ItemYbRpTypeId == 2)
                {
                    isWestMedicine = true;//存在西药项目
                    eleD505_30_01.AppendChild(eleItem30_01);
                    eleD505_03_01.AppendChild(eleD505_30_01);
                }
            }
            #endregion


            //30-02
            #region 30-02
            for (int i = 0; i < _lstUspInBalanceDtl.Count; i++)
            {
                Model.uspInBalanceDtlQry infoUspInBalanceDtl = _lstUspInBalanceDtl[i];
                Model.ModelList<Model.BsItemYbInfo> lstBsItemYb = _objBsItemYb.BsItemYb_SelectByItemIdAndTallyGroupId(infoUspInBalanceDtl.ItemId, this._inPatientInfo.TallyGroupId);
                if (lstBsItemYb.Count == 0) return string.Format("项目【{0}】【{1}】没有对应医保的病人大类，上传失败，请在项目代码中维护！", infoUspInBalanceDtl.Name, infoUspInBalanceDtl.Code);
                Model.BsItemYbInfo infoBsItemYbNh = lstBsItemYb[0];
                if (infoBsItemYbNh.ItemYbRpTypeId != 1)// || infoUspInBalanceDtl.F5.Trim() == "1")//已经上传的或者不是中成药的不执行本次循环
                {
                    continue;
                }
                //Model.ModelList<Model.BsItemYbInfo> lstBsItemYb = _objBsItemYb.BsItemYb_SelectByItemIdAndTallyGroupId(infoUspInBalanceDtl.ItemId, this._inPatientInfo.TallyGroupId);
                XmlElement eleItem30_02 = document.CreateElement("item");

                eleItem30_02.AppendChild(this.SetElementValue(document, "D505_04", infoUspInBalanceDtl.Code));//医院项目编码
                eleItem30_02.AppendChild(this.SetElementValue(document, "D505_16", infoUspInBalanceDtl.Name));//医院项目名称
                Model.ModelList<Model.BsItemDrugInfo> lstBsItemDrug = _objBsItemDrug.BsItemDrug_SelectByItemId(infoUspInBalanceDtl.ItemId);
                if (infoUspInBalanceDtl.LsRpType < 4)
                {
                    if (lstBsItemDrug.Count > 0)
                        eleItem30_02.AppendChild(this.SetElementValue(document, "D505_06", lstBsDrugForm.FindByID(lstBsItemDrug[0].FormId).Name));// 剂型
                    else
                        eleItem30_02.AppendChild(this.SetElementValue(document, "D505_06", "无"));// 剂型
                    eleItem30_02.AppendChild(this.SetElementValue(document, "D505_05", infoUspInBalanceDtl.Spec));// 规格
                }
                else
                {
                    eleItem30_02.AppendChild(this.SetElementValue(document, "D505_06", "无"));// 剂型
                }
                eleItem30_02.AppendChild(this.SetElementValue(document, "D505_07", Convert.ToDouble(infoUspInBalanceDtl.PriceIn).ToString("#0.0000")));//单价
                if (infoUspInBalanceDtl.Totality == 0) infoUspInBalanceDtl.Totality = 1.0;
                eleItem30_02.AppendChild(this.SetElementValue(document, "D505_08", infoUspInBalanceDtl.Totality));//数量
                eleItem30_02.AppendChild(this.SetElementValue(document, "D505_10", Convert.ToDouble(infoUspInBalanceDtl.Amount).ToString("#0.0000")));// 金额 
                string strRegOperDate = string.Empty;
                if (infoUspInBalanceDtl.RegOperDate == string.Empty)
                    strRegOperDate = BLL.Common.DateTimeHandler.GetServerDateTime().ToString("yyyy-MM-ddTHH-mm-ss");
                else
                    strRegOperDate = Convert.ToDateTime(infoUspInBalanceDtl.RegOperDate).ToString("yyyy-MM-ddT00-00-00");
                eleItem30_02.AppendChild(this.SetElementValue(document, "D505_13", strRegOperDate));// 开单时间
                eleItem30_02.AppendChild(this.SetElementValue(document, "D505_33", ""));// 合作标识
                eleItem30_02.AppendChild(this.SetElementValue(document, "D505_34", ""));// 转外医疗机构
                if (lstBsItemYb.Count == 0 || lstBsItemYb[0].YbCode.Trim() == string.Empty || lstBsItemYb[0].YbDesc.Trim() == string.Empty)
                {
                    infoUspInBalanceDtl.DiscIn = 1;
                    infoUspInBalanceDtl.F3 = "自费";
                    infoUspInBalanceDtl.LsGfType = 2;
                }
                if (!string.IsNullOrEmpty(lstBsItemYb[0].YbCode))
                {
                    eleItem30_02.AppendChild(this.SetElementValue(document, "D505_35", lstBsItemYb[0].YbCode));//医院项目编码

                }
                else
                {
                    eleItem30_02.AppendChild(this.SetElementValue(document, "D505_35", "his+" + infoUspInBalanceDtl.Code));//医院项目编码

                }
                if (!string.IsNullOrEmpty(lstBsItemYb[0].YbDesc) && !string.IsNullOrEmpty(lstBsItemYb[0].YbCode))
                {
                    eleItem30_02.AppendChild(this.SetElementValue(document, "D505_36", lstBsItemYb[0].YbDesc));//医院项目名称
                }
                else
                {
                    eleItem30_02.AppendChild(this.SetElementValue(document, "D505_36", "his+" + infoUspInBalanceDtl.Name));//医院项目名称
                }

                eleItem30_02.AppendChild(this.SetElementValue(document, "D505_31", lstBsUnit.FindByID(infoUspInBalanceDtl.UnitId).Name));// 包装 
                eleItem30_02.AppendChild(this.SetElementValue(document, "D505_12", lstBsDoctor.FindByID(infoUspInBalanceDtl.DoctorId).Name));// 医生姓名
                eleItem30_02.AppendChild(this.SetElementValue(document, "D505_01", RegNo));// 住院处方流水号
                eleItem30_02.AppendChild(this.SetElementValue(document, "D505_14", infoUspInBalanceDtl.ID));// 明细费用流水号
                if (infoBsItemYbNh.ItemYbRpTypeId == 1)
                {
                    isChinesePatentMedicine = true;//存在中成药项目
                    eleD505_30_02.AppendChild(eleItem30_02);
                    eleD505_03_01.AppendChild(eleD505_30_02);
                }
            }

            #endregion

            //30-03
            #region 30-03
            for (int i = 0; i < _lstUspInBalanceDtl.Count; i++)
            {
                Model.uspInBalanceDtlQry infoUspInBalanceDtl = _lstUspInBalanceDtl[i];
                Model.ModelList<Model.BsItemYbInfo> lstBsItemYb = _objBsItemYb.BsItemYb_SelectByItemIdAndTallyGroupId(infoUspInBalanceDtl.ItemId, this._inPatientInfo.TallyGroupId);
                if (lstBsItemYb.Count == 0) return string.Format("项目【{0}】【{1}】没有对应医保的病人大类，上传失败，请在项目代码中维护！", infoUspInBalanceDtl.Name, infoUspInBalanceDtl.Code);
                Model.BsItemYbInfo infoBsItemYbNh = lstBsItemYb[0];
                if (infoBsItemYbNh.ItemYbRpTypeId != 3)//|| infoUspInBalanceDtl.F5.Trim() == "1")//已经上传的或者不是中药的不执行本次循环
                {
                    continue;
                }
                //Model.ModelList<Model.BsItemYbInfo> lstBsItemYb = _objBsItemYb.BsItemYb_SelectByItemIdAndTallyGroupId(infoUspInBalanceDtl.ItemId, this._inPatientInfo.TallyGroupId);
                XmlElement eleItem30_03 = document.CreateElement("item");

                eleItem30_03.AppendChild(this.SetElementValue(document, "D505_04", infoUspInBalanceDtl.Code));//医院项目编码
                eleItem30_03.AppendChild(this.SetElementValue(document, "D505_16", infoUspInBalanceDtl.Name));//医院项目名称
                Model.ModelList<Model.BsItemDrugInfo> lstBsItemDrug = _objBsItemDrug.BsItemDrug_SelectByItemId(infoUspInBalanceDtl.ItemId);
                if (infoUspInBalanceDtl.LsRpType < 4)
                {
                    if (lstBsItemDrug.Count > 0)
                        eleItem30_03.AppendChild(this.SetElementValue(document, "D505_06", lstBsDrugForm.FindByID(lstBsItemDrug[0].FormId).Name));// 剂型
                    else
                        eleItem30_03.AppendChild(this.SetElementValue(document, "D505_06", "无"));// 剂型
                    eleItem30_03.AppendChild(this.SetElementValue(document, "D505_05", infoUspInBalanceDtl.Spec));// 规格
                }
                else eleItem30_03.AppendChild(this.SetElementValue(document, "D505_06", "无"));// 剂型

                eleItem30_03.AppendChild(this.SetElementValue(document, "D505_07", Convert.ToDouble(infoUspInBalanceDtl.PriceIn).ToString("#0.0000")));//单价
                if (infoUspInBalanceDtl.Totality == 0) infoUspInBalanceDtl.Totality = 1.0;
                eleItem30_03.AppendChild(this.SetElementValue(document, "D505_08", infoUspInBalanceDtl.Totality));//数量
                eleItem30_03.AppendChild(this.SetElementValue(document, "D505_10", Convert.ToDouble(infoUspInBalanceDtl.Amount).ToString("#0.0000")));// 金额 
                string strRegOperDate = string.Empty;
                if (infoUspInBalanceDtl.RegOperDate == string.Empty)
                    strRegOperDate = BLL.Common.DateTimeHandler.GetServerDateTime().ToString("yyyy-MM-ddTHH-mm-ss");
                else
                    strRegOperDate = Convert.ToDateTime(infoUspInBalanceDtl.RegOperDate).ToString("yyyy-MM-ddT00-00-00");
                eleItem30_03.AppendChild(this.SetElementValue(document, "D505_13", strRegOperDate));// 开单时间
                eleItem30_03.AppendChild(this.SetElementValue(document, "D505_33", ""));// 合作标识
                eleItem30_03.AppendChild(this.SetElementValue(document, "D505_34", ""));// 转外医疗机构
                if (lstBsItemYb.Count == 0 || lstBsItemYb[0].YbCode.Trim() == string.Empty || lstBsItemYb[0].YbDesc.Trim() == string.Empty)
                {
                    infoUspInBalanceDtl.DiscIn = 1;
                    infoUspInBalanceDtl.F3 = "自费";
                    infoUspInBalanceDtl.LsGfType = 2;
                }
                if (!string.IsNullOrEmpty(lstBsItemYb[0].YbCode))
                {
                    eleItem30_03.AppendChild(this.SetElementValue(document, "D505_35", lstBsItemYb[0].YbCode));//医院项目编码

                }
                else
                {
                    eleItem30_03.AppendChild(this.SetElementValue(document, "D505_35", "his+" + infoUspInBalanceDtl.Code));//医院项目编码

                }
                if (!string.IsNullOrEmpty(lstBsItemYb[0].YbDesc) && !string.IsNullOrEmpty(lstBsItemYb[0].YbCode))
                {
                    eleItem30_03.AppendChild(this.SetElementValue(document, "D505_36", lstBsItemYb[0].YbDesc));//医院项目名称
                }
                else
                {
                    eleItem30_03.AppendChild(this.SetElementValue(document, "D505_36", "his+" + infoUspInBalanceDtl.Name));//医院项目名称
                }

                eleItem30_03.AppendChild(this.SetElementValue(document, "D505_31", lstBsUnit.FindByID(infoUspInBalanceDtl.UnitId).Name));// 包装 
                eleItem30_03.AppendChild(this.SetElementValue(document, "D505_09", ""));// 明细费用流水号 
                eleItem30_03.AppendChild(this.SetElementValue(document, "D505_12", lstBsDoctor.FindByID(infoUspInBalanceDtl.DoctorId).Name));// 医生姓名
                eleItem30_03.AppendChild(this.SetElementValue(document, "D505_01", RegNo));// 住院处方流水号
                eleItem30_03.AppendChild(this.SetElementValue(document, "D505_14", infoUspInBalanceDtl.ID));// 明细费用流水号
                if (infoBsItemYbNh.ItemYbRpTypeId == 3)
                {
                    isChineseMedicine = true;//存在中药项目
                    eleD505_30_03.AppendChild(eleItem30_03);
                    eleD505_03_01.AppendChild(eleD505_30_03);
                }
            }
            #endregion


            if (isWestMedicine || isChinesePatentMedicine || isChineseMedicine)
            {
                eleDetails.AppendChild(eleD505_03_01);
            }
            //03-01
            //for (int i = 0; i < _lstUspInBalanceDtl.Count; i++)
            //{

            //    Model.uspInBalanceDtlQry infoUspInBalanceDtl = _lstUspInBalanceDtl[i];
            //    //Model.ModelList<Model.BsItemYbInfo> lstBsItemYb = _objBsItemYb.BsItemYb_SelectByItemIdAndTallyGroupId(infoUspInBalanceDtl.ItemId, this._inPatientInfo.TallyGroupId);
            //    if (((Model.EnumRpType)infoUspInBalanceDtl.LsRpType == Model.EnumRpType.ChineseMedicine ||
            //        (Model.EnumRpType)infoUspInBalanceDtl.LsRpType == Model.EnumRpType.WestMedicine ||
            //        (Model.EnumRpType)infoUspInBalanceDtl.LsRpType == Model.EnumRpType.ChinesePatentMedicine) && i >= _lstUspInBalanceDtl.Count - 1 && infoUspInBalanceDtl.F5.Trim() != "1")
            //    {
            //        eleDetails.AppendChild(eleD505_03_01);
            //    }
            //}

            //03-02
            for (int i = 0; i < _lstUspInBalanceDtl.Count; i++)
            {
                if (_lstUspInBalanceDtl[i].ItemId == 239407)
                {
                }
                Model.uspInBalanceDtlQry infoUspInBalanceDtl = _lstUspInBalanceDtl[i];
                Model.ModelList<Model.BsItemYbInfo> lstBsItemYb = _objBsItemYb.BsItemYb_SelectByItemIdAndTallyGroupId(infoUspInBalanceDtl.ItemId, this._inPatientInfo.TallyGroupId);
                if (lstBsItemYb.Count == 0) return string.Format("项目【{0}】【{1}】没有对应医保的病人大类，上传失败，请在项目代码中维护！", infoUspInBalanceDtl.Name, infoUspInBalanceDtl.Code);
                Model.BsItemYbInfo infoBsItemYbNh = lstBsItemYb[0];
                if ((infoBsItemYbNh.ItemYbRpTypeId != 4 && infoBsItemYbNh.ItemYbRpTypeId != 5 && infoBsItemYbNh.ItemYbRpTypeId != 6 &&
                   infoBsItemYbNh.ItemYbRpTypeId != 7 && infoBsItemYbNh.ItemYbRpTypeId != 8 && infoBsItemYbNh.ItemYbRpTypeId != 9 && infoBsItemYbNh.ItemYbRpTypeId != 10 &&
                   infoBsItemYbNh.ItemYbRpTypeId != 11 && infoBsItemYbNh.ItemYbRpTypeId != 12 && infoBsItemYbNh.ItemYbRpTypeId != 13 && infoBsItemYbNh.ItemYbRpTypeId != 15
                   ))// || infoUspInBalanceDtl.F5.Trim() == "1")//已经上传的或者不是中药的不执行本次循环
                {
                    continue;
                }

                //                if (((Model.EnumRpType)infoUspInBalanceDtl.LsRpType != Model.EnumRpType.Test && infoUspInBalanceDtl.BsInvInItemName != "挂号费" &&
                //(Model.EnumRpType)infoUspInBalanceDtl.LsRpType != Model.EnumRpType.Check && (Model.EnumRpType)infoUspInBalanceDtl.LsRpType != Model.EnumRpType.OPS &&
                //(Model.EnumRpType)infoUspInBalanceDtl.LsRpType != Model.EnumRpType.Cure && (Model.EnumRpType)infoUspInBalanceDtl.LsRpType != Model.EnumRpType.Bed) || infoUspInBalanceDtl.F5.Trim() == "1")//已经上传的或者不是诊疗费的不执行本次循环
                //                {
                //                    continue;
                //                }
                //Model.ModelList<Model.BsItemYbInfo> lstBsItemYb = _objBsItemYb.BsItemYb_SelectByItemIdAndTallyGroupId(infoUspInBalanceDtl.ItemId, this._inPatientInfo.TallyGroupId);
                XmlElement eleItem03_02 = document.CreateElement("item");

                eleItem03_02.AppendChild(this.SetElementValue(document, "D505_04", infoUspInBalanceDtl.Code));//医院项目编码
                eleItem03_02.AppendChild(this.SetElementValue(document, "D505_16", infoUspInBalanceDtl.Name));//医院项目名称
                Model.ModelList<Model.BsItemDrugInfo> lstBsItemDrug = _objBsItemDrug.BsItemDrug_SelectByItemId(infoUspInBalanceDtl.ItemId);
                eleItem03_02.AppendChild(this.SetElementValue(document, "D505_07", Convert.ToDouble(infoUspInBalanceDtl.PriceIn).ToString("#0.0000")));//单价
                if (infoUspInBalanceDtl.Totality == 0) infoUspInBalanceDtl.Totality = 1.0;
                eleItem03_02.AppendChild(this.SetElementValue(document, "D505_08", infoUspInBalanceDtl.Totality));//数量
                eleItem03_02.AppendChild(this.SetElementValue(document, "D505_10", Convert.ToDouble(infoUspInBalanceDtl.Amount).ToString("#0.0000")));// 金额 
                //string aa = GetYbStatType(infoUspInBalanceDtl.BsInvInItemName);
                //eleItem03_02.AppendChild(this.SetElementValue(document, "D505_32", GetYbStatType(infoUspInBalanceDtl.BsInvInItemName)));// 财务分类
                BLL.CBsItemYbRpType objBsItemYbRpType = new BLL.CBsItemYbRpType();
                Model.BsItemYbRpTypeInfo infoBsItemYbRpType = objBsItemYbRpType.GetByID(lstBsItemYb[0].ItemYbRpTypeId);
                eleItem03_02.AppendChild(this.SetElementValue(document, "D505_32", infoBsItemYbRpType.Code.Trim()));// 财务分类
                if (lstBsItemYb.Count == 0 || lstBsItemYb[0].YbCode.Trim() == string.Empty || lstBsItemYb[0].YbDesc.Trim() == string.Empty)
                {
                    infoUspInBalanceDtl.DiscIn = 1;
                    infoUspInBalanceDtl.F3 = "自费";
                    infoUspInBalanceDtl.LsGfType = 2;
                }
                if (!string.IsNullOrEmpty(lstBsItemYb[0].YbCode))
                {
                    eleItem03_02.AppendChild(this.SetElementValue(document, "D505_35", lstBsItemYb[0].YbCode));//医院项目编码

                }
                else
                {
                    //eleItem03_02.AppendChild(this.SetElementValue(document, "D505_35", "his+" + infoUspInBalanceDtl.Code));//医院项目编码
                    eleItem03_02.AppendChild(this.SetElementValue(document, "D505_35", infoBsItemYbRpType.Code.Trim()));//没有匹配上的项目传财务分类
                }
                if (!string.IsNullOrEmpty(lstBsItemYb[0].YbDesc) && !string.IsNullOrEmpty(lstBsItemYb[0].YbCode))
                {
                    eleItem03_02.AppendChild(this.SetElementValue(document, "D505_36", lstBsItemYb[0].YbDesc));//医院项目名称
                }
                else
                {
                    eleItem03_02.AppendChild(this.SetElementValue(document, "D505_36", "his+" + infoUspInBalanceDtl.Name));//医院项目名称
                }
                string strRegOperDate = string.Empty;
                if (infoUspInBalanceDtl.RegOperDate == string.Empty)
                    strRegOperDate = BLL.Common.DateTimeHandler.GetServerDateTime().ToString("yyyy-MM-ddTHH-mm-ss");
                else
                    strRegOperDate = Convert.ToDateTime(infoUspInBalanceDtl.RegOperDate).ToString("yyyy-MM-ddT00-00-00");
                eleItem03_02.AppendChild(this.SetElementValue(document, "D505_13", strRegOperDate));// 开单时间
                eleItem03_02.AppendChild(this.SetElementValue(document, "D505_33", ""));// 合作标识
                eleItem03_02.AppendChild(this.SetElementValue(document, "D505_34", ""));// 转外医疗机构
                eleItem03_02.AppendChild(this.SetElementValue(document, "D505_37", lstBsUnit.FindByID(infoUspInBalanceDtl.UnitId).Name));// 单位
                eleItem03_02.AppendChild(this.SetElementValue(document, "D505_12", lstBsDoctor.FindByID(infoUspInBalanceDtl.DoctorId).Name));// 医生姓名
                eleItem03_02.AppendChild(this.SetElementValue(document, "D505_01", RegNo));// 住院处方流水号
                eleItem03_02.AppendChild(this.SetElementValue(document, "D505_14", infoUspInBalanceDtl.ID));// 明细费用流水号

                if (infoBsItemYbNh.ItemYbRpTypeId == 4 || infoBsItemYbNh.ItemYbRpTypeId == 5 || infoBsItemYbNh.ItemYbRpTypeId == 6 ||
                   infoBsItemYbNh.ItemYbRpTypeId == 7 || infoBsItemYbNh.ItemYbRpTypeId == 8 || infoBsItemYbNh.ItemYbRpTypeId == 9 || infoBsItemYbNh.ItemYbRpTypeId == 10 ||
                   infoBsItemYbNh.ItemYbRpTypeId == 11 || infoBsItemYbNh.ItemYbRpTypeId == 12 || infoBsItemYbNh.ItemYbRpTypeId == 13 || infoBsItemYbNh.ItemYbRpTypeId == 15
                   )
                //if ((Model.EnumRpType)infoUspInBalanceDtl.LsRpType == Model.EnumRpType.Test || infoUspInBalanceDtl.BsInvInItemName == "挂号费" ||
                //    (Model.EnumRpType)infoUspInBalanceDtl.LsRpType == Model.EnumRpType.Check || (Model.EnumRpType)infoUspInBalanceDtl.LsRpType == Model.EnumRpType.OPS ||
                //    (Model.EnumRpType)infoUspInBalanceDtl.LsRpType == Model.EnumRpType.Cure || (Model.EnumRpType)infoUspInBalanceDtl.LsRpType == Model.EnumRpType.Bed)
                {
                    eleD505_03_02.AppendChild(eleItem03_02);
                    eleDetails.AppendChild(eleD505_03_02);
                }
            }


            //03-03
            for (int i = 0; i < _lstUspInBalanceDtl.Count; i++)
            {
                Model.uspInBalanceDtlQry infoUspInBalanceDtl = _lstUspInBalanceDtl[i];
                Model.ModelList<Model.BsItemYbInfo> lstBsItemYb = _objBsItemYb.BsItemYb_SelectByItemIdAndTallyGroupId(infoUspInBalanceDtl.ItemId, this._inPatientInfo.TallyGroupId);
                if (lstBsItemYb.Count == 0) return string.Format("项目【{0}】【{1}】没有对应医保的病人大类，上传失败，请在项目代码中维护！", infoUspInBalanceDtl.Name, infoUspInBalanceDtl.Code);
                Model.BsItemYbInfo infoBsItemYbNh = lstBsItemYb[0];
                if (infoBsItemYbNh.ItemYbRpTypeId != 14)//|| infoUspInBalanceDtl.F5.Trim() == "1")//已经上传的或者不是材料的不执行本次循环
                {
                    continue;
                }
                //Model.ModelList<Model.BsItemYbInfo> lstBsItemYb = _objBsItemYb.BsItemYb_SelectByItemIdAndTallyGroupId(infoUspInBalanceDtl.ItemId, this._inPatientInfo.TallyGroupId);
                XmlElement eleItem03_03 = document.CreateElement("item");

                eleItem03_03.AppendChild(this.SetElementValue(document, "D505_04", infoUspInBalanceDtl.Code));//医院项目编码
                eleItem03_03.AppendChild(this.SetElementValue(document, "D505_16", infoUspInBalanceDtl.Name));//医院项目名称
                Model.ModelList<Model.BsItemDrugInfo> lstBsItemDrug = _objBsItemDrug.BsItemDrug_SelectByItemId(infoUspInBalanceDtl.ItemId);
                eleItem03_03.AppendChild(this.SetElementValue(document, "D505_07", Convert.ToDouble(infoUspInBalanceDtl.PriceIn).ToString("#0.0000")));//单价
                if (infoUspInBalanceDtl.Totality == 0) infoUspInBalanceDtl.Totality = 1.0;
                eleItem03_03.AppendChild(this.SetElementValue(document, "D505_08", infoUspInBalanceDtl.Totality));//数量
                eleItem03_03.AppendChild(this.SetElementValue(document, "D505_10", Convert.ToDouble(infoUspInBalanceDtl.Amount).ToString("#0.0000")));// 金额 
                if (lstBsItemYb.Count == 0 || lstBsItemYb[0].YbCode.Trim() == string.Empty || lstBsItemYb[0].YbDesc.Trim() == string.Empty)
                {
                    infoUspInBalanceDtl.DiscIn = 1;
                    infoUspInBalanceDtl.F3 = "自费";
                    infoUspInBalanceDtl.LsGfType = 2;
                }
                if (!string.IsNullOrEmpty(lstBsItemYb[0].YbCode))
                {
                    eleItem03_03.AppendChild(this.SetElementValue(document, "D505_35", lstBsItemYb[0].YbCode));//医院项目编码

                }
                else
                {
                    eleItem03_03.AppendChild(this.SetElementValue(document, "D505_35", "his+" + infoUspInBalanceDtl.Code));//医院项目编码

                }
                if (!string.IsNullOrEmpty(lstBsItemYb[0].YbDesc) && !string.IsNullOrEmpty(lstBsItemYb[0].YbCode))
                {
                    eleItem03_03.AppendChild(this.SetElementValue(document, "D505_36", lstBsItemYb[0].YbDesc));//医院项目名称
                }
                else
                {
                    eleItem03_03.AppendChild(this.SetElementValue(document, "D505_36", "his+" + infoUspInBalanceDtl.Name));//医院项目名称
                }
                string strRegOperDate = string.Empty;
                if (infoUspInBalanceDtl.RegOperDate == string.Empty)
                    strRegOperDate = BLL.Common.DateTimeHandler.GetServerDateTime().ToString("yyyy-MM-ddTHH-mm-ss");
                else
                    strRegOperDate = Convert.ToDateTime(infoUspInBalanceDtl.RegOperDate).ToString("yyyy-MM-ddT00-00-00");
                eleItem03_03.AppendChild(this.SetElementValue(document, "D505_13", strRegOperDate));// 开单时间
                eleItem03_03.AppendChild(this.SetElementValue(document, "D505_33", ""));// 合作标识
                eleItem03_03.AppendChild(this.SetElementValue(document, "D505_34", ""));// 转外医疗机构
                eleItem03_03.AppendChild(this.SetElementValue(document, "D505_37", lstBsUnit.FindByID(infoUspInBalanceDtl.UnitId).Name));// 单位
                eleItem03_03.AppendChild(this.SetElementValue(document, "D505_05", infoUspInBalanceDtl.Spec));// 规格
                eleItem03_03.AppendChild(this.SetElementValue(document, "D505_31", lstBsUnit.FindByID(infoUspInBalanceDtl.UnitId).Name));// 包装
                eleItem03_03.AppendChild(this.SetElementValue(document, "D505_12", lstBsDoctor.FindByID(infoUspInBalanceDtl.DoctorId).Name));// 医生姓名
                eleItem03_03.AppendChild(this.SetElementValue(document, "D505_01", RegNo));// 住院处方流水号
                eleItem03_03.AppendChild(this.SetElementValue(document, "D505_14", infoUspInBalanceDtl.ID));// 明细费用流水号
                if (infoBsItemYbNh.ItemYbRpTypeId == 14 && infoUspInBalanceDtl.BsInvInItemName != "挂号费")
                {
                    eleD505_03_03.AppendChild(eleItem03_03);
                    eleDetails.AppendChild(eleD505_03_03);
                }
            }




            bodyNode.AppendChild(eleDetails);

            string result = this.FinallySetXmlNs(document);

            StringBuilder sBuilderXml = new StringBuilder();
            sBuilderXml.Append(document.InnerXml);

            Utilities.Document docXml = new Utilities.Document();
            docXml.Content = sBuilderXml;
            docXml.Create("c://批量住院记账(输入).xml", false);

            document.LoadXml(result);
            XmlNode responseNode = document.SelectSingleNode("response");
            //GetUpLoadInFee();
            if (!this.ShowHeadMsg(responseNode))
            {

                sBuilderXml = new StringBuilder();
                sBuilderXml.Append(result);
                docXml = new Utilities.Document();
                docXml.Content = sBuilderXml;
                docXml.Create("c://批量住院记账(输出).xml", false);
                return "失败";
            }
            Utilities.Information.ShowMsgBox("批量住院记账" + RetHeadMsg(responseNode));
            //BLL.CInInvoiceDtl objInInvoiceDtl = new BLL.CInInvoiceDtl();
            //Model.ModelList<Model.InInvoiceDtlInfo> lstInInvoiceDtl = _lstUspInBalanceDtl.ConvertTo<Model.InInvoiceDtlInfo>();
            //Model.ModelList<Model.InInvoiceDtlInfo> lstIninvoicedtl2 = new Model.ModelList<Model.InInvoiceDtlInfo>();
            //if (lstInInvoiceDtl.Count > 0)
            //{
            //    int i = 0;
            //    foreach (var item in lstInInvoiceDtl)
            //    {
            //        if (item.F5.Trim() == "1") continue;
            //        if (_lstUspInBalanceDtl[i].F3.Contains("自费"))
            //        {
            //            item.F3 = "自费";
            //        }
            //        else item.F3 = string.Empty;
            //        item.IsPay = false;
            //        //item.LsGfType = 2;
            //        lstIninvoicedtl2.Add(item);
            //        i++;
            //    }
            //    objInInvoiceDtl.Save(lstIninvoicedtl2);
            //}
            StringBuilder sBuilder1 = new StringBuilder();
            sBuilder1.Append(result);

            Utilities.Document doc = new Utilities.Document();
            doc.Content = sBuilder1;
            doc.Create("c://批量住院记账.txt", false);

            return "成功";
            //if (!this.ShowHeadMsg(responseNode))
            //{
            //    return "失败";
            //}
            //StringBuilder sBuilder1 = new StringBuilder();
            //sBuilder1.Append(result);

            //Utilities.Document doc = new Utilities.Document();
            //doc.Content = sBuilder1;
            //doc.Create("c://批量住院记账.txt", false);
            //if (this.ShowHeadMsg(responseNode))
            //{
            //    result = Convert.ToString(this.GetInChargeResultEnd());
            //    //result = Convert.ToString(this.GetInChargeResult());
            //    if (result.Contains("失败"))
            //    {
            //        return "失败";
            //    }
            //}

            //return "成功";
        }
        #endregion

        ///// <summary>
        ///// 获取住院记账
        ///// </summary>
        ///// <returns></returns>
        //public override object GetUpLoadInFee()
        //{
        //    System.Xml.XmlDocument document = this.XmlDataExchangeModel("C020004", InfoInHosInfo.YbRegNo);
        //    XmlNode rootNode = document.DocumentElement;
        //    XmlNode bodyNode = rootNode["body"];
        //    bodyNode.AppendChild(this.SetElementValue(document, "D504_02", InfoInHosInfo.YbRegNo));//个人编码
        //    bodyNode.AppendChild(this.SetElementValue(document, "D504_09", InfoInHosInfo.InPatNo + InfoInHosInfo.NTime.ToString()));//住院号 
        //    bodyNode.AppendChild(this.SetElementValue(document, "D505_47", InfoInHosInfo.InTime.ToString("yyyy-MM-ddTHH-mm-ss")));//开始记账时间
        //    bodyNode.AppendChild(this.SetElementValue(document, "D505_48",
        //        InfoInHosInfo.OutTime.ToString("yyyy-MM-ddTHH-mm-ss").CompareTo(InfoInHosInfo.InTime.ToString("yyyy-MM-ddTHH-mm-ss")) <= 0 ? BLL.Common.DateTimeHandler.GetServerDateTime().ToString("yyyy-MM-ddTHH-mm-ss") : InfoInHosInfo.OutTime.ToString("yyyy-MM-ddTHH-mm-ss")));//终止记账时间 
        //    string result = this.FinallySetXmlNs(document);
        //    document.LoadXml(result);
        //    XmlNode responseNode = document.SelectSingleNode("response");
        //    if (!this.ShowHeadMsg(responseNode))
        //    {
        //        return "失败";
        //    }
        //    Model.ModelList<Model.QzNhInterface.GetUpLoadInFeeInfo> lstGetUpLoadInFeeDtl = new Model.ModelList<Model.QzNhInterface.GetUpLoadInFeeInfo>();
        //    Model.ModelList<Model.uspInBalanceDtlQry> LstInInvoiceDtlNotUp = new Model.ModelList<Model.uspInBalanceDtlQry>();
        //    XmlNode bodySubNode = responseNode.SelectSingleNode("body");
        //    if (bodyNode == null) return "没有记账项目";
        //    foreach (System.Xml.XmlNode itemNode in bodySubNode)
        //    {
        //        string InPatNo = itemNode.SelectSingleNode("D504_09").InnerText;
        //        string YbRegNo = itemNode.SelectSingleNode("D401_21_A").InnerText;
        //        string RegNo = itemNode.SelectSingleNode("D505_02").InnerText;
        //        XmlNode detailsNode = itemNode.SelectSingleNode("details");
        //        XmlNode d505_03_01Node = detailsNode.SelectSingleNode("D505_03_01");
        //        if (d505_03_01Node != null)
        //        {
        //            XmlNode d505_30_01Node = d505_03_01Node.SelectSingleNode("D505_30_01");
        //            if (d505_30_01Node != null)
        //            {
        //                foreach (System.Xml.XmlNode d505_30_01ItemNode in d505_30_01Node)
        //                {
        //                    Model.QzNhInterface.GetUpLoadInFeeInfo getUpLoadInFeeInfo = new Model.QzNhInterface.GetUpLoadInFeeInfo();
        //                    getUpLoadInFeeInfo.InPatNo = InPatNo;
        //                    getUpLoadInFeeInfo.YbRegNo = YbRegNo;
        //                    getUpLoadInFeeInfo.RegNo = RegNo;
        //                    getUpLoadInFeeInfo.IDDtl = d505_30_01ItemNode.SelectSingleNode("D505_14").InnerText;
        //                    getUpLoadInFeeInfo.Code = d505_30_01ItemNode.SelectSingleNode("D505_04").InnerText;
        //                    getUpLoadInFeeInfo.Name = d505_30_01ItemNode.SelectSingleNode("D505_16").InnerText;
        //                    getUpLoadInFeeInfo.DrugName = d505_30_01ItemNode.SelectSingleNode("D505_06").InnerText;
        //                    getUpLoadInFeeInfo.Spec = d505_30_01ItemNode.SelectSingleNode("D505_05").InnerText;
        //                    getUpLoadInFeeInfo.PriceIn = d505_30_01ItemNode.SelectSingleNode("D505_07").InnerText;
        //                    getUpLoadInFeeInfo.Totality = d505_30_01ItemNode.SelectSingleNode("D505_08").InnerText;
        //                    getUpLoadInFeeInfo.Amount = d505_30_01ItemNode.SelectSingleNode("D505_10").InnerText;
        //                    getUpLoadInFeeInfo.RegOperDate = d505_30_01ItemNode.SelectSingleNode("D505_13").InnerText;
        //                    getUpLoadInFeeInfo.YbCode = d505_30_01ItemNode.SelectSingleNode("D505_35").InnerText;
        //                    getUpLoadInFeeInfo.YbName = d505_30_01ItemNode.SelectSingleNode("D505_36").InnerText;
        //                    getUpLoadInFeeInfo.UnitName = "";
        //                    getUpLoadInFeeInfo.DoctorName = d505_30_01ItemNode.SelectSingleNode("D505_12").InnerText;
        //                    getUpLoadInFeeInfo.BsInvInItemName = "";
        //                    getUpLoadInFeeInfo.Package = d505_30_01ItemNode.SelectSingleNode("D505_31").InnerText;
        //                    lstGetUpLoadInFeeDtl.Add(getUpLoadInFeeInfo);
        //                }
        //            }
        //            XmlNode d505_30_02Node = d505_03_01Node.SelectSingleNode("D505_30_02");
        //            if (d505_30_02Node != null)
        //            {
        //                foreach (System.Xml.XmlNode d505_30_02ItemNode in d505_30_02Node)
        //                {
        //                    Model.QzNhInterface.GetUpLoadInFeeInfo getUpLoadInFeeInfo = new Model.QzNhInterface.GetUpLoadInFeeInfo();
        //                    getUpLoadInFeeInfo.InPatNo = InPatNo;
        //                    getUpLoadInFeeInfo.YbRegNo = YbRegNo;
        //                    getUpLoadInFeeInfo.RegNo = RegNo;
        //                    getUpLoadInFeeInfo.IDDtl = d505_30_02ItemNode.SelectSingleNode("D505_14").InnerText;
        //                    getUpLoadInFeeInfo.Code = d505_30_02ItemNode.SelectSingleNode("D505_04").InnerText;
        //                    getUpLoadInFeeInfo.Name = d505_30_02ItemNode.SelectSingleNode("D505_16").InnerText;
        //                    getUpLoadInFeeInfo.DrugName = d505_30_02ItemNode.SelectSingleNode("D505_06").InnerText;
        //                    getUpLoadInFeeInfo.Spec = d505_30_02ItemNode.SelectSingleNode("D505_05").InnerText;
        //                    getUpLoadInFeeInfo.PriceIn = d505_30_02ItemNode.SelectSingleNode("D505_07").InnerText;
        //                    getUpLoadInFeeInfo.Totality = d505_30_02ItemNode.SelectSingleNode("D505_08").InnerText;
        //                    getUpLoadInFeeInfo.Amount = d505_30_02ItemNode.SelectSingleNode("D505_10").InnerText;
        //                    getUpLoadInFeeInfo.RegOperDate = d505_30_02ItemNode.SelectSingleNode("D505_13").InnerText;
        //                    getUpLoadInFeeInfo.YbCode = d505_30_02ItemNode.SelectSingleNode("D505_35").InnerText;
        //                    getUpLoadInFeeInfo.YbName = d505_30_02ItemNode.SelectSingleNode("D505_36").InnerText;
        //                    getUpLoadInFeeInfo.UnitName = "";
        //                    getUpLoadInFeeInfo.DoctorName = d505_30_02ItemNode.SelectSingleNode("D505_12").InnerText;
        //                    getUpLoadInFeeInfo.BsInvInItemName = "";
        //                    getUpLoadInFeeInfo.Package = d505_30_02ItemNode.SelectSingleNode("D505_31").InnerText;
        //                    lstGetUpLoadInFeeDtl.Add(getUpLoadInFeeInfo);
        //                }
        //            }
        //            XmlNode d505_30_03Node = d505_03_01Node.SelectSingleNode("D505_30_03");
        //            if (d505_30_03Node == null)
        //            {
        //                foreach (System.Xml.XmlNode d505_30_03ItemNode in d505_30_03Node)
        //                {
        //                    Model.QzNhInterface.GetUpLoadInFeeInfo getUpLoadInFeeInfo = new Model.QzNhInterface.GetUpLoadInFeeInfo();
        //                    getUpLoadInFeeInfo.InPatNo = InPatNo;
        //                    getUpLoadInFeeInfo.YbRegNo = YbRegNo;
        //                    getUpLoadInFeeInfo.RegNo = RegNo;
        //                    getUpLoadInFeeInfo.IDDtl = d505_30_03ItemNode.SelectSingleNode("D505_14").InnerText;
        //                    getUpLoadInFeeInfo.Code = d505_30_03ItemNode.SelectSingleNode("D505_04").InnerText;
        //                    getUpLoadInFeeInfo.Name = d505_30_03ItemNode.SelectSingleNode("D505_16").InnerText;
        //                    getUpLoadInFeeInfo.DrugName = d505_30_03ItemNode.SelectSingleNode("D505_06").InnerText;
        //                    getUpLoadInFeeInfo.Spec = d505_30_03ItemNode.SelectSingleNode("D505_05").InnerText;
        //                    getUpLoadInFeeInfo.PriceIn = d505_30_03ItemNode.SelectSingleNode("D505_07").InnerText;
        //                    getUpLoadInFeeInfo.Totality = d505_30_03ItemNode.SelectSingleNode("D505_08").InnerText;
        //                    getUpLoadInFeeInfo.Amount = d505_30_03ItemNode.SelectSingleNode("D505_10").InnerText;
        //                    getUpLoadInFeeInfo.RegOperDate = d505_30_03ItemNode.SelectSingleNode("D505_13").InnerText;
        //                    getUpLoadInFeeInfo.YbCode = d505_30_03ItemNode.SelectSingleNode("D505_35").InnerText;
        //                    getUpLoadInFeeInfo.YbName = d505_30_03ItemNode.SelectSingleNode("D505_36").InnerText;
        //                    getUpLoadInFeeInfo.UnitName = "";
        //                    getUpLoadInFeeInfo.DoctorName = d505_30_03ItemNode.SelectSingleNode("D505_12").InnerText;
        //                    getUpLoadInFeeInfo.BsInvInItemName = "";
        //                    getUpLoadInFeeInfo.Package = d505_30_03ItemNode.SelectSingleNode("D505_31").InnerText;
        //                    lstGetUpLoadInFeeDtl.Add(getUpLoadInFeeInfo);
        //                }
        //            }
        //        }
        //        XmlNode d505_03_02Node = detailsNode.SelectSingleNode("D505_03_02");
        //        if (d505_03_02Node != null)
        //        {
        //            foreach (System.Xml.XmlNode d505_03_02ItemNode in d505_03_02Node)
        //            {
        //                Model.QzNhInterface.GetUpLoadInFeeInfo getUpLoadInFeeInfo = new Model.QzNhInterface.GetUpLoadInFeeInfo();
        //                getUpLoadInFeeInfo.InPatNo = InPatNo;
        //                getUpLoadInFeeInfo.YbRegNo = YbRegNo;
        //                getUpLoadInFeeInfo.RegNo = RegNo;
        //                getUpLoadInFeeInfo.IDDtl = d505_03_02ItemNode.SelectSingleNode("D505_14").InnerText;
        //                getUpLoadInFeeInfo.Code = d505_03_02ItemNode.SelectSingleNode("D505_04").InnerText;
        //                getUpLoadInFeeInfo.Name = d505_03_02ItemNode.SelectSingleNode("D505_16").InnerText;
        //                getUpLoadInFeeInfo.DrugName = "";
        //                getUpLoadInFeeInfo.Spec = "";
        //                getUpLoadInFeeInfo.PriceIn = d505_03_02ItemNode.SelectSingleNode("D505_07").InnerText;
        //                getUpLoadInFeeInfo.Totality = d505_03_02ItemNode.SelectSingleNode("D505_08").InnerText;
        //                getUpLoadInFeeInfo.Amount = d505_03_02ItemNode.SelectSingleNode("D505_10").InnerText;
        //                getUpLoadInFeeInfo.RegOperDate = d505_03_02ItemNode.SelectSingleNode("D505_13").InnerText;
        //                getUpLoadInFeeInfo.YbCode = d505_03_02ItemNode.SelectSingleNode("D505_35").InnerText;
        //                getUpLoadInFeeInfo.YbName = d505_03_02ItemNode.SelectSingleNode("D505_36").InnerText;
        //                getUpLoadInFeeInfo.UnitName = d505_03_02ItemNode.SelectSingleNode("D505_37").InnerText;
        //                getUpLoadInFeeInfo.DoctorName = d505_03_02ItemNode.SelectSingleNode("D505_12").InnerText;
        //                getUpLoadInFeeInfo.BsInvInItemName = d505_03_02ItemNode.SelectSingleNode("D505_32").InnerText;
        //                getUpLoadInFeeInfo.Package = "";
        //                lstGetUpLoadInFeeDtl.Add(getUpLoadInFeeInfo);
        //            }
        //        }
        //        XmlNode d505_03_03Node = detailsNode.SelectSingleNode("D505_03_03");
        //        if (d505_03_03Node != null)
        //        {
        //            foreach (System.Xml.XmlNode d505_03_03ItemNode in d505_03_03Node)
        //            {
        //                Model.QzNhInterface.GetUpLoadInFeeInfo getUpLoadInFeeInfo = new Model.QzNhInterface.GetUpLoadInFeeInfo();
        //                getUpLoadInFeeInfo.InPatNo = InPatNo;
        //                getUpLoadInFeeInfo.YbRegNo = YbRegNo;
        //                getUpLoadInFeeInfo.RegNo = RegNo;
        //                getUpLoadInFeeInfo.IDDtl = d505_03_03ItemNode.SelectSingleNode("D505_14").InnerText;
        //                getUpLoadInFeeInfo.Code = d505_03_03ItemNode.SelectSingleNode("D505_04").InnerText;
        //                getUpLoadInFeeInfo.Name = d505_03_03ItemNode.SelectSingleNode("D505_16").InnerText;
        //                getUpLoadInFeeInfo.DrugName = "";
        //                getUpLoadInFeeInfo.Spec = "";
        //                getUpLoadInFeeInfo.PriceIn = d505_03_03ItemNode.SelectSingleNode("D505_07").InnerText;
        //                getUpLoadInFeeInfo.Totality = d505_03_03ItemNode.SelectSingleNode("D505_08").InnerText;
        //                getUpLoadInFeeInfo.Amount = d505_03_03ItemNode.SelectSingleNode("D505_10").InnerText;
        //                getUpLoadInFeeInfo.RegOperDate = d505_03_03ItemNode.SelectSingleNode("D505_13").InnerText;
        //                getUpLoadInFeeInfo.YbCode = d505_03_03ItemNode.SelectSingleNode("D505_35").InnerText;
        //                getUpLoadInFeeInfo.YbName = d505_03_03ItemNode.SelectSingleNode("D505_36").InnerText;
        //                getUpLoadInFeeInfo.UnitName = d505_03_03ItemNode.SelectSingleNode("D505_37").InnerText;
        //                getUpLoadInFeeInfo.DoctorName = d505_03_03ItemNode.SelectSingleNode("D505_12").InnerText;
        //                getUpLoadInFeeInfo.BsInvInItemName = "";
        //                getUpLoadInFeeInfo.Package = d505_03_03ItemNode.SelectSingleNode("D505_31").InnerText;
        //                lstGetUpLoadInFeeDtl.Add(getUpLoadInFeeInfo);
        //            }
        //        }
        //    }
        //    // XmlNode itemNode = bodySubNode.SelectSingleNode("item");
        //    return lstGetUpLoadInFeeDtl;
        //}



        #region 批量住院记账B020013
        /// <summary>
        /// 批量住院记账B020013
        /// </summary>
        /// <returns></returns>
        public override object UpLoadInFee()
        {
            //int count=_lstUspInBalanceDtl.Count;
            //int resultCount=_lstUspInBalanceDtl.Count/10;
            //int resultCount1=_lstUspInBalanceDtl.Count%10;
            //if(resultCount1>0)
            //    resultCount=resultCount+1;
            //if()

            System.Xml.XmlDocument document = this.XmlDataExchangeModel("B020013", "");
            XmlNode rootNode = document.DocumentElement;
            XmlNode bodyNode = rootNode["body"];

            Model.ModelList<Model.BsUnitInfo> lstBsUnit = _objBsUnit.GetAll();
            Model.ModelList<Model.BsDoctorInfo> lstBsDoctor = _objBsDoctor.GetAll();
            Model.ModelList<Model.BsDrugFormInfo> lstBsDrugForm = _objBsDrugForm.GetAll();

            bodyNode.AppendChild(this.SetElementValue(document, "D504_09", InfoInHosInfo.InPatNo + InfoInHosInfo.NTime.ToString()));//住院号


            XmlElement eleDetails = this.SetElementValue(document, "details", "");

            XmlElement eleD505_03_01 = this.SetElementValue(document, "D505_03_01", "");//药品
            XmlElement eleD505_30_01 = this.SetElementValue(document, "D505_30_01", "");//西药
            XmlElement eleD505_30_02 = this.SetElementValue(document, "D505_30_02", "");//中成药
            XmlElement eleD505_30_03 = this.SetElementValue(document, "D505_30_03", "");//中草药

            XmlElement eleD505_03_02 = this.SetElementValue(document, "D505_03_02", "");// 诊疗
            XmlElement eleD505_03_03 = this.SetElementValue(document, "D505_03_03", "");// 材料  

            bool isWestMedicine = false;//是否有西药项目
            bool isChinesePatentMedicine = false;//是否有中成药项目
            bool isChineseMedicine = false;//是否有中药项目
            bool isClinic = false;//是否有诊疗项目
            bool isMaterial = false;//是否有材料项目
            //30-01
            for (int i = 0; i < _lstUspInBalanceDtl.Count; i++)
            {

                Model.uspInBalanceDtlQry infoUspInBalanceDtl = _lstUspInBalanceDtl[i];
                if ((Model.EnumRpType)infoUspInBalanceDtl.LsRpType != Model.EnumRpType.WestMedicine || infoUspInBalanceDtl.F5.Trim() == "1")//已经上传的或者不是西药的不执行本次循环
                {
                    continue;
                }
                Model.ModelList<Model.BsItemYbInfo> lstBsItemYb = _objBsItemYb.BsItemYb_SelectByItemIdAndTallyGroupId(infoUspInBalanceDtl.ItemId, this._inPatientInfo.TallyGroupId);
                if (lstBsItemYb.Count == 0)
                {
                    return string.Format("项目【{0}】【{1}】没有对应医保的病人大类，上传失败，请在项目代码中维护！", infoUspInBalanceDtl.Name, infoUspInBalanceDtl.Code);
                }
                XmlElement eleItem30_01 = document.CreateElement("item");

                eleItem30_01.AppendChild(this.SetElementValue(document, "D505_04", infoUspInBalanceDtl.Code));//医院项目编码
                eleItem30_01.AppendChild(this.SetElementValue(document, "D505_16", infoUspInBalanceDtl.Name));//医院项目名称
                Model.ModelList<Model.BsItemDrugInfo> lstBsItemDrug = _objBsItemDrug.BsItemDrug_SelectByItemId(infoUspInBalanceDtl.ItemId);
                if (infoUspInBalanceDtl.LsRpType < 4)
                {
                    if (lstBsItemDrug.Count > 0)
                        eleItem30_01.AppendChild(this.SetElementValue(document, "D505_06", lstBsDrugForm.FindByID(lstBsItemDrug[0].FormId).Name));// 剂型
                    else
                        eleItem30_01.AppendChild(this.SetElementValue(document, "D505_06", "无"));// 剂型
                    eleItem30_01.AppendChild(this.SetElementValue(document, "D505_05", infoUspInBalanceDtl.Spec));// 规格
                }
                eleItem30_01.AppendChild(this.SetElementValue(document, "D505_07", Convert.ToDouble(infoUspInBalanceDtl.PriceIn).ToString("#0.0000")));//单价
                if (infoUspInBalanceDtl.Totality == 0) infoUspInBalanceDtl.Totality = 1.0;
                eleItem30_01.AppendChild(this.SetElementValue(document, "D505_08", infoUspInBalanceDtl.Totality));//数量
                eleItem30_01.AppendChild(this.SetElementValue(document, "D505_10", Convert.ToDouble(infoUspInBalanceDtl.Amount).ToString("#0.0000")));// 金额
                if (infoUspInBalanceDtl.RegOperDate == string.Empty)
                    infoUspInBalanceDtl.RegOperDate = BLL.Common.DateTimeHandler.GetServerDateTime().ToString("yyyy-MM-ddTHH-mm-ss");
                else
                    infoUspInBalanceDtl.RegOperDate = infoUspInBalanceDtl.RegOperDate + "T00-00-00";
                eleItem30_01.AppendChild(this.SetElementValue(document, "D505_13", infoUspInBalanceDtl.RegOperDate));// 开单时间
                eleItem30_01.AppendChild(this.SetElementValue(document, "D505_33", ""));// 合作标识
                eleItem30_01.AppendChild(this.SetElementValue(document, "D505_34", ""));// 转外医疗机构
                if (lstBsItemYb.Count == 0 || lstBsItemYb[0].YbCode.Trim() == string.Empty || lstBsItemYb[0].YbDesc.Trim() == string.Empty)
                {
                    infoUspInBalanceDtl.DiscIn = 1;
                    infoUspInBalanceDtl.F3 = "自费";
                    infoUspInBalanceDtl.LsGfType = 2;
                }
                if (lstBsItemYb.Count > 0 && !string.IsNullOrEmpty(lstBsItemYb[0].YbCode))
                {
                    eleItem30_01.AppendChild(this.SetElementValue(document, "D505_35", lstBsItemYb[0].YbCode.Trim()));//医院项目编码

                }
                else
                {
                    eleItem30_01.AppendChild(this.SetElementValue(document, "D505_35", "his+" + infoUspInBalanceDtl.Code.Trim()));//医院项目编码

                }
                if (lstBsItemYb.Count > 0 && !string.IsNullOrEmpty(lstBsItemYb[0].YbDesc) && !string.IsNullOrEmpty(lstBsItemYb[0].YbCode))
                {
                    eleItem30_01.AppendChild(this.SetElementValue(document, "D505_36", lstBsItemYb[0].YbDesc.Trim()));//医院项目名称
                }
                else
                {
                    eleItem30_01.AppendChild(this.SetElementValue(document, "D505_36", "his+" + infoUspInBalanceDtl.Name.Trim()));//医院项目名称
                }

                eleItem30_01.AppendChild(this.SetElementValue(document, "D505_31", lstBsUnit.FindByID(infoUspInBalanceDtl.UnitId).Name));// 包装 
                eleItem30_01.AppendChild(this.SetElementValue(document, "D505_12", lstBsDoctor.FindByID(infoUspInBalanceDtl.DoctorId).Name));// 医生姓名
                eleItem30_01.AppendChild(this.SetElementValue(document, "D505_01", RegNo));// 住院处方流水号
                eleItem30_01.AppendChild(this.SetElementValue(document, "D505_14", infoUspInBalanceDtl.ID));// 明细费用流水号
                if (infoUspInBalanceDtl.ID == 0)
                {

                }
                if ((Model.EnumRpType)infoUspInBalanceDtl.LsRpType == Model.EnumRpType.WestMedicine)
                {
                    isWestMedicine = true;//存在西药项目
                    eleD505_30_01.AppendChild(eleItem30_01);
                    eleD505_03_01.AppendChild(eleD505_30_01);
                }
            }

            //30-02
            for (int i = 0; i < _lstUspInBalanceDtl.Count; i++)
            {
                Model.uspInBalanceDtlQry infoUspInBalanceDtl = _lstUspInBalanceDtl[i];
                if ((Model.EnumRpType)infoUspInBalanceDtl.LsRpType != Model.EnumRpType.ChinesePatentMedicine || infoUspInBalanceDtl.F5.Trim() == "1")//已经上传的或者不是中成药的不执行本次循环
                {
                    continue;
                }
                Model.ModelList<Model.BsItemYbInfo> lstBsItemYb = _objBsItemYb.BsItemYb_SelectByItemIdAndTallyGroupId(infoUspInBalanceDtl.ItemId, this._inPatientInfo.TallyGroupId);
                if (lstBsItemYb.Count == 0)
                {
                    return string.Format("项目【{0}】【{1}】没有对应医保的病人大类，上传失败，请在项目代码中维护！", infoUspInBalanceDtl.Name, infoUspInBalanceDtl.Code);
                }
                XmlElement eleItem30_02 = document.CreateElement("item");

                eleItem30_02.AppendChild(this.SetElementValue(document, "D505_04", infoUspInBalanceDtl.Code));//医院项目编码
                eleItem30_02.AppendChild(this.SetElementValue(document, "D505_16", infoUspInBalanceDtl.Name));//医院项目名称
                Model.ModelList<Model.BsItemDrugInfo> lstBsItemDrug = _objBsItemDrug.BsItemDrug_SelectByItemId(infoUspInBalanceDtl.ItemId);
                if (infoUspInBalanceDtl.LsRpType < 4)
                {
                    if (lstBsItemDrug.Count > 0)
                        eleItem30_02.AppendChild(this.SetElementValue(document, "D505_06", lstBsDrugForm.FindByID(lstBsItemDrug[0].FormId).Name));// 剂型
                    else
                        eleItem30_02.AppendChild(this.SetElementValue(document, "D505_06", "无"));// 剂型
                    eleItem30_02.AppendChild(this.SetElementValue(document, "D505_05", infoUspInBalanceDtl.Spec));// 规格
                }
                eleItem30_02.AppendChild(this.SetElementValue(document, "D505_07", Convert.ToDouble(infoUspInBalanceDtl.PriceIn).ToString("#0.0000")));//单价
                if (infoUspInBalanceDtl.Totality == 0) infoUspInBalanceDtl.Totality = 1.0;
                eleItem30_02.AppendChild(this.SetElementValue(document, "D505_08", infoUspInBalanceDtl.Totality));//数量
                eleItem30_02.AppendChild(this.SetElementValue(document, "D505_10", Convert.ToDouble(infoUspInBalanceDtl.Amount).ToString("#0.0000")));// 金额 
                if (infoUspInBalanceDtl.RegOperDate == string.Empty)
                    infoUspInBalanceDtl.RegOperDate = BLL.Common.DateTimeHandler.GetServerDateTime().ToString("yyyy-MM-ddTHH-mm-ss");
                else
                    infoUspInBalanceDtl.RegOperDate = infoUspInBalanceDtl.RegOperDate + "T00-00-00";
                eleItem30_02.AppendChild(this.SetElementValue(document, "D505_13", infoUspInBalanceDtl.RegOperDate));// 开单时间
                eleItem30_02.AppendChild(this.SetElementValue(document, "D505_33", ""));// 合作标识
                eleItem30_02.AppendChild(this.SetElementValue(document, "D505_34", ""));// 转外医疗机构
                if (lstBsItemYb.Count == 0 || lstBsItemYb[0].YbCode.Trim() == string.Empty || lstBsItemYb[0].YbDesc.Trim() == string.Empty)
                {
                    infoUspInBalanceDtl.DiscIn = 1;
                    infoUspInBalanceDtl.F3 = "自费";
                    infoUspInBalanceDtl.LsGfType = 2;
                }
                if (lstBsItemYb.Count > 0 && !string.IsNullOrEmpty(lstBsItemYb[0].YbCode))
                {
                    eleItem30_02.AppendChild(this.SetElementValue(document, "D505_35", lstBsItemYb[0].YbCode.Trim()));//医院项目编码

                }
                else
                {
                    eleItem30_02.AppendChild(this.SetElementValue(document, "D505_35", "his+" + infoUspInBalanceDtl.Code.Trim()));//医院项目编码

                }
                if (lstBsItemYb.Count > 0 && !string.IsNullOrEmpty(lstBsItemYb[0].YbDesc) && !string.IsNullOrEmpty(lstBsItemYb[0].YbCode))
                {
                    eleItem30_02.AppendChild(this.SetElementValue(document, "D505_36", lstBsItemYb[0].YbDesc.Trim()));//医院项目名称
                }
                else
                {
                    eleItem30_02.AppendChild(this.SetElementValue(document, "D505_36", "his+" + infoUspInBalanceDtl.Name.Trim()));//医院项目名称
                }

                eleItem30_02.AppendChild(this.SetElementValue(document, "D505_31", lstBsUnit.FindByID(infoUspInBalanceDtl.UnitId).Name));// 包装 
                eleItem30_02.AppendChild(this.SetElementValue(document, "D505_12", lstBsDoctor.FindByID(infoUspInBalanceDtl.DoctorId).Name));// 医生姓名
                eleItem30_02.AppendChild(this.SetElementValue(document, "D505_01", RegNo));// 住院处方流水号
                eleItem30_02.AppendChild(this.SetElementValue(document, "D505_14", infoUspInBalanceDtl.ID));// 明细费用流水号
                if (infoUspInBalanceDtl.ID == 0)
                {

                }
                if ((Model.EnumRpType)infoUspInBalanceDtl.LsRpType == Model.EnumRpType.ChinesePatentMedicine)
                {
                    isChinesePatentMedicine = true;//存在中成药项目
                    eleD505_30_02.AppendChild(eleItem30_02);
                    eleD505_03_01.AppendChild(eleD505_30_02);
                }
            }


            //30-03
            for (int i = 0; i < _lstUspInBalanceDtl.Count; i++)
            {
                Model.uspInBalanceDtlQry infoUspInBalanceDtl = _lstUspInBalanceDtl[i];
                if ((Model.EnumRpType)infoUspInBalanceDtl.LsRpType != Model.EnumRpType.ChineseMedicine || infoUspInBalanceDtl.F5.Trim() == "1")//已经上传的或者不是中药的不执行本次循环
                {
                    continue;
                }
                Model.ModelList<Model.BsItemYbInfo> lstBsItemYb = _objBsItemYb.BsItemYb_SelectByItemIdAndTallyGroupId(infoUspInBalanceDtl.ItemId, this._inPatientInfo.TallyGroupId);
                if (lstBsItemYb.Count == 0)
                {
                    return string.Format("项目【{0}】【{1}】没有对应医保的病人大类，上传失败，请在项目代码中维护！", infoUspInBalanceDtl.Name, infoUspInBalanceDtl.Code);
                }
                XmlElement eleItem30_03 = document.CreateElement("item");

                eleItem30_03.AppendChild(this.SetElementValue(document, "D505_04", infoUspInBalanceDtl.Code));//医院项目编码
                eleItem30_03.AppendChild(this.SetElementValue(document, "D505_16", infoUspInBalanceDtl.Name));//医院项目名称
                Model.ModelList<Model.BsItemDrugInfo> lstBsItemDrug = _objBsItemDrug.BsItemDrug_SelectByItemId(infoUspInBalanceDtl.ItemId);
                if (infoUspInBalanceDtl.LsRpType < 4)
                {
                    if (lstBsItemDrug.Count > 0)
                        eleItem30_03.AppendChild(this.SetElementValue(document, "D505_06", lstBsDrugForm.FindByID(lstBsItemDrug[0].FormId).Name));// 剂型
                    else
                        eleItem30_03.AppendChild(this.SetElementValue(document, "D505_06", "无"));// 剂型
                    eleItem30_03.AppendChild(this.SetElementValue(document, "D505_05", infoUspInBalanceDtl.Spec));// 规格
                }
                eleItem30_03.AppendChild(this.SetElementValue(document, "D505_07", Convert.ToDouble(infoUspInBalanceDtl.PriceIn).ToString("#0.0000")));//单价
                if (infoUspInBalanceDtl.Totality == 0) infoUspInBalanceDtl.Totality = 1.0;
                eleItem30_03.AppendChild(this.SetElementValue(document, "D505_08", infoUspInBalanceDtl.Totality));//数量
                eleItem30_03.AppendChild(this.SetElementValue(document, "D505_10", Convert.ToDouble(infoUspInBalanceDtl.Amount).ToString("#0.0000")));// 金额 
                if (infoUspInBalanceDtl.RegOperDate == string.Empty)
                    infoUspInBalanceDtl.RegOperDate = BLL.Common.DateTimeHandler.GetServerDateTime().ToString("yyyy-MM-ddTHH-mm-ss");
                else
                    infoUspInBalanceDtl.RegOperDate = infoUspInBalanceDtl.RegOperDate + "T00-00-00";
                eleItem30_03.AppendChild(this.SetElementValue(document, "D505_13", infoUspInBalanceDtl.RegOperDate));// 开单时间
                eleItem30_03.AppendChild(this.SetElementValue(document, "D505_33", ""));// 合作标识
                eleItem30_03.AppendChild(this.SetElementValue(document, "D505_34", ""));// 转外医疗机构
                if (lstBsItemYb.Count == 0 || lstBsItemYb[0].YbCode.Trim() == string.Empty || lstBsItemYb[0].YbDesc.Trim() == string.Empty)
                {
                    infoUspInBalanceDtl.DiscIn = 1;
                    infoUspInBalanceDtl.F3 = "自费";
                    infoUspInBalanceDtl.LsGfType = 2;
                }
                if (lstBsItemYb.Count > 0 && !string.IsNullOrEmpty(lstBsItemYb[0].YbCode))
                {
                    eleItem30_03.AppendChild(this.SetElementValue(document, "D505_35", lstBsItemYb[0].YbCode.Trim()));//医院项目编码

                }
                else
                {
                    eleItem30_03.AppendChild(this.SetElementValue(document, "D505_35", "his+" + infoUspInBalanceDtl.Code.Trim()));//医院项目编码

                }
                if (lstBsItemYb.Count > 0 && !string.IsNullOrEmpty(lstBsItemYb[0].YbDesc) && !string.IsNullOrEmpty(lstBsItemYb[0].YbCode))
                {
                    eleItem30_03.AppendChild(this.SetElementValue(document, "D505_36", lstBsItemYb[0].YbDesc.Trim()));//医院项目名称
                }
                else
                {
                    eleItem30_03.AppendChild(this.SetElementValue(document, "D505_36", "his+" + infoUspInBalanceDtl.Name.Trim()));//医院项目名称
                }

                eleItem30_03.AppendChild(this.SetElementValue(document, "D505_31", lstBsUnit.FindByID(infoUspInBalanceDtl.UnitId).Name));// 包装 
                eleItem30_03.AppendChild(this.SetElementValue(document, "D505_09", ""));// 明细费用流水号 
                eleItem30_03.AppendChild(this.SetElementValue(document, "D505_12", lstBsDoctor.FindByID(infoUspInBalanceDtl.DoctorId).Name));// 医生姓名
                eleItem30_03.AppendChild(this.SetElementValue(document, "D505_01", RegNo));// 住院处方流水号
                eleItem30_03.AppendChild(this.SetElementValue(document, "D505_14", infoUspInBalanceDtl.ID));// 明细费用流水号

                if (infoUspInBalanceDtl.ID == 0)
                {

                }
                if ((Model.EnumRpType)infoUspInBalanceDtl.LsRpType == Model.EnumRpType.ChineseMedicine)
                {
                    isChineseMedicine = true;//存在中药项目
                    eleD505_30_03.AppendChild(eleItem30_03);
                    eleD505_03_01.AppendChild(eleD505_30_03);
                }
            }


            if (isWestMedicine || isChinesePatentMedicine || isChineseMedicine)
            {
                eleDetails.AppendChild(eleD505_03_01);
            }
            //03-01
            //for (int i = 0; i < _lstUspInBalanceDtl.Count; i++)
            //{

            //    Model.uspInBalanceDtlQry infoUspInBalanceDtl = _lstUspInBalanceDtl[i];
            //    //Model.ModelList<Model.BsItemYbInfo> lstBsItemYb = _objBsItemYb.BsItemYb_SelectByItemIdAndTallyGroupId(infoUspInBalanceDtl.ItemId, this._inPatientInfo.TallyGroupId);
            //    if (((Model.EnumRpType)infoUspInBalanceDtl.LsRpType == Model.EnumRpType.ChineseMedicine ||
            //        (Model.EnumRpType)infoUspInBalanceDtl.LsRpType == Model.EnumRpType.WestMedicine ||
            //        (Model.EnumRpType)infoUspInBalanceDtl.LsRpType == Model.EnumRpType.ChinesePatentMedicine) && i >= _lstUspInBalanceDtl.Count - 1 && infoUspInBalanceDtl.F5.Trim() != "1")
            //    {
            //        eleDetails.AppendChild(eleD505_03_01);
            //    }
            //}

            //03-02
            for (int i = 0; i < _lstUspInBalanceDtl.Count; i++)
            {
                if (_lstUspInBalanceDtl[i].ItemId == 239407)
                {
                }
                Model.uspInBalanceDtlQry infoUspInBalanceDtl = _lstUspInBalanceDtl[i];
                if (((Model.EnumRpType)infoUspInBalanceDtl.LsRpType != Model.EnumRpType.Test && infoUspInBalanceDtl.BsInvInItemName != "挂号费" &&
(Model.EnumRpType)infoUspInBalanceDtl.LsRpType != Model.EnumRpType.Check && (Model.EnumRpType)infoUspInBalanceDtl.LsRpType != Model.EnumRpType.OPS &&
(Model.EnumRpType)infoUspInBalanceDtl.LsRpType != Model.EnumRpType.Cure && (Model.EnumRpType)infoUspInBalanceDtl.LsRpType != Model.EnumRpType.Bed) || infoUspInBalanceDtl.F5.Trim() == "1")//已经上传的或者不是诊疗费的不执行本次循环
                {
                    continue;
                }
                Model.ModelList<Model.BsItemYbInfo> lstBsItemYb = _objBsItemYb.BsItemYb_SelectByItemIdAndTallyGroupId(infoUspInBalanceDtl.ItemId, this._inPatientInfo.TallyGroupId);

                if (lstBsItemYb.Count == 0)
                {
                    return string.Format("项目【{0}】【{1}】没有对应医保的病人大类，上传失败，请在项目代码中维护！", infoUspInBalanceDtl.Name, infoUspInBalanceDtl.Code);
                }
                XmlElement eleItem03_02 = document.CreateElement("item");

                eleItem03_02.AppendChild(this.SetElementValue(document, "D505_04", infoUspInBalanceDtl.Code));//医院项目编码
                eleItem03_02.AppendChild(this.SetElementValue(document, "D505_16", infoUspInBalanceDtl.Name));//医院项目名称
                Model.ModelList<Model.BsItemDrugInfo> lstBsItemDrug = _objBsItemDrug.BsItemDrug_SelectByItemId(infoUspInBalanceDtl.ItemId);
                eleItem03_02.AppendChild(this.SetElementValue(document, "D505_07", Convert.ToDouble(infoUspInBalanceDtl.PriceIn).ToString("#0.0000")));//单价
                if (infoUspInBalanceDtl.Totality == 0) infoUspInBalanceDtl.Totality = 1.0;
                eleItem03_02.AppendChild(this.SetElementValue(document, "D505_08", infoUspInBalanceDtl.Totality));//数量
                eleItem03_02.AppendChild(this.SetElementValue(document, "D505_10", Convert.ToDouble(infoUspInBalanceDtl.Amount).ToString("#0.0000")));// 金额 
                //string aa = GetYbStatType(infoUspInBalanceDtl.BsInvInItemName);
                //eleItem03_02.AppendChild(this.SetElementValue(document, "D505_32", GetYbStatType(infoUspInBalanceDtl.BsInvInItemName)));// 财务分类
                BLL.CBsItemYbRpType objBsItemYbRpType = new BLL.CBsItemYbRpType();
                Model.BsItemYbRpTypeInfo infoBsItemYbRpType = objBsItemYbRpType.GetByID(lstBsItemYb[0].ItemYbRpTypeId);
                eleItem03_02.AppendChild(this.SetElementValue(document, "D505_32", infoBsItemYbRpType.Code.Trim()));// 财务分类
                if (lstBsItemYb.Count == 0 || lstBsItemYb[0].YbCode.Trim() == string.Empty || lstBsItemYb[0].YbDesc.Trim() == string.Empty)
                {
                    infoUspInBalanceDtl.DiscIn = 1;
                    infoUspInBalanceDtl.F3 = "自费";
                    infoUspInBalanceDtl.LsGfType = 2;
                }
                if (lstBsItemYb.Count > 0 && !string.IsNullOrEmpty(lstBsItemYb[0].YbCode))
                {
                    eleItem03_02.AppendChild(this.SetElementValue(document, "D505_35", lstBsItemYb[0].YbCode.Trim()));//医院项目编码

                }
                else
                {
                    eleItem03_02.AppendChild(this.SetElementValue(document, "D505_35", "his+" + infoUspInBalanceDtl.Code.Trim()));//医院项目编码
                    //eleItem03_02.AppendChild(this.SetElementValue(document, "D505_35", infoBsItemYbRpType.Code.Trim()));//没有匹配上的项目传财务分类
                }
                if (lstBsItemYb.Count > 0 && !string.IsNullOrEmpty(lstBsItemYb[0].YbDesc) && !string.IsNullOrEmpty(lstBsItemYb[0].YbCode))
                {
                    eleItem03_02.AppendChild(this.SetElementValue(document, "D505_36", lstBsItemYb[0].YbDesc.Trim()));//医院项目名称
                }
                else
                {
                    eleItem03_02.AppendChild(this.SetElementValue(document, "D505_36", "his+" + infoUspInBalanceDtl.Name.Trim()));//医院项目名称
                }
                if (infoUspInBalanceDtl.RegOperDate == string.Empty)
                    infoUspInBalanceDtl.RegOperDate = BLL.Common.DateTimeHandler.GetServerDateTime().ToString("yyyy-MM-ddTHH-mm-ss");
                else
                    infoUspInBalanceDtl.RegOperDate = infoUspInBalanceDtl.RegOperDate + "T00-00-00";
                eleItem03_02.AppendChild(this.SetElementValue(document, "D505_13", infoUspInBalanceDtl.RegOperDate));// 开单时间
                eleItem03_02.AppendChild(this.SetElementValue(document, "D505_33", ""));// 合作标识
                eleItem03_02.AppendChild(this.SetElementValue(document, "D505_34", ""));// 转外医疗机构
                eleItem03_02.AppendChild(this.SetElementValue(document, "D505_37", lstBsUnit.FindByID(infoUspInBalanceDtl.UnitId).Name));// 单位
                eleItem03_02.AppendChild(this.SetElementValue(document, "D505_12", lstBsDoctor.FindByID(infoUspInBalanceDtl.DoctorId).Name));// 医生姓名
                eleItem03_02.AppendChild(this.SetElementValue(document, "D505_01", RegNo));// 住院处方流水号
                eleItem03_02.AppendChild(this.SetElementValue(document, "D505_14", infoUspInBalanceDtl.ID));// 明细费用流水号
                if ((Model.EnumRpType)infoUspInBalanceDtl.LsRpType == Model.EnumRpType.Test || infoUspInBalanceDtl.BsInvInItemName == "挂号费" ||
                    (Model.EnumRpType)infoUspInBalanceDtl.LsRpType == Model.EnumRpType.Check || (Model.EnumRpType)infoUspInBalanceDtl.LsRpType == Model.EnumRpType.OPS ||
                    (Model.EnumRpType)infoUspInBalanceDtl.LsRpType == Model.EnumRpType.Cure || (Model.EnumRpType)infoUspInBalanceDtl.LsRpType == Model.EnumRpType.Bed)
                {
                    eleD505_03_02.AppendChild(eleItem03_02);
                    eleDetails.AppendChild(eleD505_03_02);
                }
            }


            //03-03
            for (int i = 0; i < _lstUspInBalanceDtl.Count; i++)
            {
                Model.uspInBalanceDtlQry infoUspInBalanceDtl = _lstUspInBalanceDtl[i];
                if ((Model.EnumRpType)infoUspInBalanceDtl.LsRpType != Model.EnumRpType.Other || infoUspInBalanceDtl.F5.Trim() == "1")//已经上传的或者不是材料的不执行本次循环
                {
                    continue;
                }
                Model.ModelList<Model.BsItemYbInfo> lstBsItemYb = _objBsItemYb.BsItemYb_SelectByItemIdAndTallyGroupId(infoUspInBalanceDtl.ItemId, this._inPatientInfo.TallyGroupId);
                if (lstBsItemYb.Count == 0)
                {
                    return string.Format("项目【{0}】【{1}】没有对应医保的病人大类，上传失败，请在项目代码中维护！", infoUspInBalanceDtl.Name, infoUspInBalanceDtl.Code);
                }
                XmlElement eleItem03_03 = document.CreateElement("item");

                eleItem03_03.AppendChild(this.SetElementValue(document, "D505_04", infoUspInBalanceDtl.Code));//医院项目编码
                eleItem03_03.AppendChild(this.SetElementValue(document, "D505_16", infoUspInBalanceDtl.Name));//医院项目名称
                Model.ModelList<Model.BsItemDrugInfo> lstBsItemDrug = _objBsItemDrug.BsItemDrug_SelectByItemId(infoUspInBalanceDtl.ItemId);
                eleItem03_03.AppendChild(this.SetElementValue(document, "D505_07", Convert.ToDouble(infoUspInBalanceDtl.PriceIn).ToString("#0.0000")));//单价
                if (infoUspInBalanceDtl.Totality == 0) infoUspInBalanceDtl.Totality = 1.0;
                eleItem03_03.AppendChild(this.SetElementValue(document, "D505_08", infoUspInBalanceDtl.Totality));//数量
                eleItem03_03.AppendChild(this.SetElementValue(document, "D505_10", Convert.ToDouble(infoUspInBalanceDtl.Amount).ToString("#0.0000")));// 金额 
                if (lstBsItemYb.Count == 0 || lstBsItemYb[0].YbCode.Trim() == string.Empty || lstBsItemYb[0].YbDesc.Trim() == string.Empty)
                {
                    infoUspInBalanceDtl.DiscIn = 1;
                    infoUspInBalanceDtl.F3 = "自费";
                    infoUspInBalanceDtl.LsGfType = 2;
                }

                if (lstBsItemYb.Count > 0 && !string.IsNullOrEmpty(lstBsItemYb[0].YbCode))
                {
                    eleItem03_03.AppendChild(this.SetElementValue(document, "D505_35", lstBsItemYb[0].YbCode.Trim()));//医院项目编码

                }
                else
                {
                    eleItem03_03.AppendChild(this.SetElementValue(document, "D505_35", "his+" + infoUspInBalanceDtl.Code.Trim()));//医院项目编码

                }
                if (lstBsItemYb.Count > 0 && !string.IsNullOrEmpty(lstBsItemYb[0].YbDesc) && !string.IsNullOrEmpty(lstBsItemYb[0].YbCode))
                {
                    eleItem03_03.AppendChild(this.SetElementValue(document, "D505_36", lstBsItemYb[0].YbDesc.Trim()));//医院项目名称
                }
                else
                {
                    eleItem03_03.AppendChild(this.SetElementValue(document, "D505_36", "his+" + infoUspInBalanceDtl.Name.Trim()));//医院项目名称
                }
                if (infoUspInBalanceDtl.RegOperDate == string.Empty)
                    infoUspInBalanceDtl.RegOperDate = BLL.Common.DateTimeHandler.GetServerDateTime().ToString("yyyy-MM-ddTHH-mm-ss");
                else
                    infoUspInBalanceDtl.RegOperDate = infoUspInBalanceDtl.RegOperDate + "T00-00-00";
                eleItem03_03.AppendChild(this.SetElementValue(document, "D505_13", infoUspInBalanceDtl.RegOperDate));// 开单时间
                eleItem03_03.AppendChild(this.SetElementValue(document, "D505_33", ""));// 合作标识
                eleItem03_03.AppendChild(this.SetElementValue(document, "D505_34", ""));// 转外医疗机构
                eleItem03_03.AppendChild(this.SetElementValue(document, "D505_37", lstBsUnit.FindByID(infoUspInBalanceDtl.UnitId).Name));// 单位
                eleItem03_03.AppendChild(this.SetElementValue(document, "D505_05", infoUspInBalanceDtl.Spec));// 规格
                eleItem03_03.AppendChild(this.SetElementValue(document, "D505_31", lstBsUnit.FindByID(infoUspInBalanceDtl.UnitId).Name));// 包装
                eleItem03_03.AppendChild(this.SetElementValue(document, "D505_12", lstBsDoctor.FindByID(infoUspInBalanceDtl.DoctorId).Name));// 医生姓名
                eleItem03_03.AppendChild(this.SetElementValue(document, "D505_01", RegNo));// 住院处方流水号
                eleItem03_03.AppendChild(this.SetElementValue(document, "D505_14", infoUspInBalanceDtl.ID));// 明细费用流水号
                if (infoUspInBalanceDtl.ID == 0)
                {

                }
                if ((Model.EnumRpType)infoUspInBalanceDtl.LsRpType == Model.EnumRpType.Other && infoUspInBalanceDtl.BsInvInItemName != "挂号费")
                {
                    eleD505_03_03.AppendChild(eleItem03_03);
                    eleDetails.AppendChild(eleD505_03_03);
                }
            }




            bodyNode.AppendChild(eleDetails);

            string result = this.FinallySetXmlNs(document);

            StringBuilder sBuilderIn = new StringBuilder();
            sBuilderIn.Append(document.InnerXml.Replace(" xmlns=\"\"", ""));

            Utilities.Document docIn = new Utilities.Document();
            docIn.Content = sBuilderIn;
            docIn.Create("c://批量住院记账(输入).xml", false);

            document.LoadXml(result);
            XmlNode responseNode = document.SelectSingleNode("response");
            //GetUpLoadInFee();

            StringBuilder sBuilder1 = new StringBuilder();
            sBuilder1.Append(result);

            Utilities.Document doc = new Utilities.Document();
            doc.Content = sBuilder1;
            doc.Create("c://批量住院记账(输出).xml", false);

            if (!this.ShowHeadMsg(responseNode))
            {
                return "失败";
            }
            Utilities.Information.ShowMsgBox("批量住院记账" + RetHeadMsg(responseNode));
            //BLL.CInInvoiceDtl objInInvoiceDtl = new BLL.CInInvoiceDtl();
            //Model.ModelList<Model.InInvoiceDtlInfo> lstInInvoiceDtl = _lstUspInBalanceDtl.ConvertTo<Model.InInvoiceDtlInfo>();
            //Model.ModelList<Model.InInvoiceDtlInfo> lstIninvoicedtl2 = new Model.ModelList<Model.InInvoiceDtlInfo>();
            //if (lstInInvoiceDtl.Count > 0)
            //{
            //    int i = 0;
            //    foreach (var item in lstInInvoiceDtl)
            //    {
            //        if (item.F5.Trim() == "1") continue;
            //        if (_lstUspInBalanceDtl[i].F3.Contains("自费"))
            //        {
            //            item.F3 = "自费";
            //        }
            //        else item.F3 = string.Empty;
            //        item.IsPay = false;
            //        //item.LsGfType = 2;
            //        lstIninvoicedtl2.Add(item);
            //        i++;
            //    }
            //    objInInvoiceDtl.Save(lstIninvoicedtl2);
            //}

            return "成功";
            //if (!this.ShowHeadMsg(responseNode))
            //{
            //    return "失败";
            //}
            //StringBuilder sBuilder1 = new StringBuilder();
            //sBuilder1.Append(result);

            //Utilities.Document doc = new Utilities.Document();
            //doc.Content = sBuilder1;
            //doc.Create("c://批量住院记账.txt", false);
            //if (this.ShowHeadMsg(responseNode))
            //{
            //    result = Convert.ToString(this.GetInChargeResultEnd());
            //    //result = Convert.ToString(this.GetInChargeResult());
            //    if (result.Contains("失败"))
            //    {
            //        return "失败";
            //    }
            //}

            //return "成功";
        }

        /// <summary>
        /// 获取住院记账
        /// </summary>
        /// <returns></returns>
        public override object GetUpLoadInFee()
        {
            System.Xml.XmlDocument document = this.XmlDataExchangeModel("C020004", InfoInHosInfo.YbRegNo);
            XmlNode rootNode = document.DocumentElement;
            XmlNode bodyNode = rootNode["body"];
            bodyNode.AppendChild(this.SetElementValue(document, "D504_02", InfoInHosInfo.YbRegNo));//个人编码
            bodyNode.AppendChild(this.SetElementValue(document, "D504_09", InfoInHosInfo.InPatNo + InfoInHosInfo.NTime.ToString()));//住院号 
            bodyNode.AppendChild(this.SetElementValue(document, "D505_47", InfoInHosInfo.InTime.ToString("yyyy-MM-ddTHH-mm-ss")));//开始记账时间
            bodyNode.AppendChild(this.SetElementValue(document, "D505_48",
                InfoInHosInfo.OutTime.ToString("yyyy-MM-ddTHH-mm-ss").CompareTo(InfoInHosInfo.InTime.ToString("yyyy-MM-ddTHH-mm-ss")) <= 0 ? BLL.Common.DateTimeHandler.GetServerDateTime().ToString("yyyy-MM-ddTHH-mm-ss") : InfoInHosInfo.OutTime.ToString("yyyy-MM-ddTHH-mm-ss")));//终止记账时间 
            string result = this.FinallySetXmlNs(document);
            document.LoadXml(result);
            XmlNode responseNode = document.SelectSingleNode("response");
            if (!this.ShowHeadMsg(responseNode))
            {
                return "失败";
            }
            Model.ModelList<Model.QzNhInterface.GetUpLoadInFeeInfo> lstGetUpLoadInFeeDtl = new Model.ModelList<Model.QzNhInterface.GetUpLoadInFeeInfo>();
            Model.ModelList<Model.uspInBalanceDtlQry> LstInInvoiceDtlNotUp = new Model.ModelList<Model.uspInBalanceDtlQry>();
            XmlNode bodySubNode = responseNode.SelectSingleNode("body");
            if (bodyNode == null) return "没有记账项目";
            foreach (System.Xml.XmlNode itemNode in bodySubNode)
            {
                string InPatNo = itemNode.SelectSingleNode("D504_09").InnerText;
                string YbRegNo = itemNode.SelectSingleNode("D401_21_A").InnerText;
                string RegNo = itemNode.SelectSingleNode("D505_02").InnerText;
                XmlNode detailsNode = itemNode.SelectSingleNode("details");
                XmlNode d505_03_01Node = detailsNode.SelectSingleNode("D505_03_01");
                if (d505_03_01Node != null)
                {
                    XmlNode d505_30_01Node = d505_03_01Node.SelectSingleNode("D505_30_01");
                    if (d505_30_01Node != null)
                    {
                        foreach (System.Xml.XmlNode d505_30_01ItemNode in d505_30_01Node)
                        {
                            Model.QzNhInterface.GetUpLoadInFeeInfo getUpLoadInFeeInfo = new Model.QzNhInterface.GetUpLoadInFeeInfo();
                            getUpLoadInFeeInfo.InPatNo = InPatNo;
                            getUpLoadInFeeInfo.YbRegNo = YbRegNo;
                            getUpLoadInFeeInfo.RegNo = RegNo;
                            getUpLoadInFeeInfo.IDDtl = d505_30_01ItemNode.SelectSingleNode("D505_14").InnerText;
                            getUpLoadInFeeInfo.Code = d505_30_01ItemNode.SelectSingleNode("D505_04").InnerText;
                            getUpLoadInFeeInfo.Name = d505_30_01ItemNode.SelectSingleNode("D505_16").InnerText;
                            getUpLoadInFeeInfo.DrugName = d505_30_01ItemNode.SelectSingleNode("D505_06").InnerText;
                            getUpLoadInFeeInfo.Spec = d505_30_01ItemNode.SelectSingleNode("D505_05").InnerText;
                            getUpLoadInFeeInfo.PriceIn = d505_30_01ItemNode.SelectSingleNode("D505_07").InnerText;
                            getUpLoadInFeeInfo.Totality = d505_30_01ItemNode.SelectSingleNode("D505_08").InnerText;
                            getUpLoadInFeeInfo.Amount = d505_30_01ItemNode.SelectSingleNode("D505_10").InnerText;
                            getUpLoadInFeeInfo.RegOperDate = d505_30_01ItemNode.SelectSingleNode("D505_13").InnerText;
                            getUpLoadInFeeInfo.YbCode = d505_30_01ItemNode.SelectSingleNode("D505_35").InnerText;
                            getUpLoadInFeeInfo.YbName = d505_30_01ItemNode.SelectSingleNode("D505_36").InnerText;
                            getUpLoadInFeeInfo.UnitName = "";
                            getUpLoadInFeeInfo.DoctorName = d505_30_01ItemNode.SelectSingleNode("D505_12").InnerText;
                            getUpLoadInFeeInfo.BsInvInItemName = "";
                            getUpLoadInFeeInfo.Package = d505_30_01ItemNode.SelectSingleNode("D505_31").InnerText;
                            lstGetUpLoadInFeeDtl.Add(getUpLoadInFeeInfo);
                        }
                    }
                    XmlNode d505_30_02Node = d505_03_01Node.SelectSingleNode("D505_30_02");
                    if (d505_30_02Node != null)
                    {
                        foreach (System.Xml.XmlNode d505_30_02ItemNode in d505_30_02Node)
                        {
                            Model.QzNhInterface.GetUpLoadInFeeInfo getUpLoadInFeeInfo = new Model.QzNhInterface.GetUpLoadInFeeInfo();
                            getUpLoadInFeeInfo.InPatNo = InPatNo;
                            getUpLoadInFeeInfo.YbRegNo = YbRegNo;
                            getUpLoadInFeeInfo.RegNo = RegNo;
                            getUpLoadInFeeInfo.IDDtl = d505_30_02ItemNode.SelectSingleNode("D505_14").InnerText;
                            getUpLoadInFeeInfo.Code = d505_30_02ItemNode.SelectSingleNode("D505_04").InnerText;
                            getUpLoadInFeeInfo.Name = d505_30_02ItemNode.SelectSingleNode("D505_16").InnerText;
                            getUpLoadInFeeInfo.DrugName = d505_30_02ItemNode.SelectSingleNode("D505_06").InnerText;
                            getUpLoadInFeeInfo.Spec = d505_30_02ItemNode.SelectSingleNode("D505_05").InnerText;
                            getUpLoadInFeeInfo.PriceIn = d505_30_02ItemNode.SelectSingleNode("D505_07").InnerText;
                            getUpLoadInFeeInfo.Totality = d505_30_02ItemNode.SelectSingleNode("D505_08").InnerText;
                            getUpLoadInFeeInfo.Amount = d505_30_02ItemNode.SelectSingleNode("D505_10").InnerText;
                            getUpLoadInFeeInfo.RegOperDate = d505_30_02ItemNode.SelectSingleNode("D505_13").InnerText;
                            getUpLoadInFeeInfo.YbCode = d505_30_02ItemNode.SelectSingleNode("D505_35").InnerText;
                            getUpLoadInFeeInfo.YbName = d505_30_02ItemNode.SelectSingleNode("D505_36").InnerText;
                            getUpLoadInFeeInfo.UnitName = "";
                            getUpLoadInFeeInfo.DoctorName = d505_30_02ItemNode.SelectSingleNode("D505_12").InnerText;
                            getUpLoadInFeeInfo.BsInvInItemName = "";
                            getUpLoadInFeeInfo.Package = d505_30_02ItemNode.SelectSingleNode("D505_31").InnerText;
                            lstGetUpLoadInFeeDtl.Add(getUpLoadInFeeInfo);
                        }
                    }
                    XmlNode d505_30_03Node = d505_03_01Node.SelectSingleNode("D505_30_03");
                    if (d505_30_03Node == null)
                    {
                        foreach (System.Xml.XmlNode d505_30_03ItemNode in d505_30_03Node)
                        {
                            Model.QzNhInterface.GetUpLoadInFeeInfo getUpLoadInFeeInfo = new Model.QzNhInterface.GetUpLoadInFeeInfo();
                            getUpLoadInFeeInfo.InPatNo = InPatNo;
                            getUpLoadInFeeInfo.YbRegNo = YbRegNo;
                            getUpLoadInFeeInfo.RegNo = RegNo;
                            getUpLoadInFeeInfo.IDDtl = d505_30_03ItemNode.SelectSingleNode("D505_14").InnerText;
                            getUpLoadInFeeInfo.Code = d505_30_03ItemNode.SelectSingleNode("D505_04").InnerText;
                            getUpLoadInFeeInfo.Name = d505_30_03ItemNode.SelectSingleNode("D505_16").InnerText;
                            getUpLoadInFeeInfo.DrugName = d505_30_03ItemNode.SelectSingleNode("D505_06").InnerText;
                            getUpLoadInFeeInfo.Spec = d505_30_03ItemNode.SelectSingleNode("D505_05").InnerText;
                            getUpLoadInFeeInfo.PriceIn = d505_30_03ItemNode.SelectSingleNode("D505_07").InnerText;
                            getUpLoadInFeeInfo.Totality = d505_30_03ItemNode.SelectSingleNode("D505_08").InnerText;
                            getUpLoadInFeeInfo.Amount = d505_30_03ItemNode.SelectSingleNode("D505_10").InnerText;
                            getUpLoadInFeeInfo.RegOperDate = d505_30_03ItemNode.SelectSingleNode("D505_13").InnerText;
                            getUpLoadInFeeInfo.YbCode = d505_30_03ItemNode.SelectSingleNode("D505_35").InnerText;
                            getUpLoadInFeeInfo.YbName = d505_30_03ItemNode.SelectSingleNode("D505_36").InnerText;
                            getUpLoadInFeeInfo.UnitName = "";
                            getUpLoadInFeeInfo.DoctorName = d505_30_03ItemNode.SelectSingleNode("D505_12").InnerText;
                            getUpLoadInFeeInfo.BsInvInItemName = "";
                            getUpLoadInFeeInfo.Package = d505_30_03ItemNode.SelectSingleNode("D505_31").InnerText;
                            lstGetUpLoadInFeeDtl.Add(getUpLoadInFeeInfo);
                        }
                    }
                }
                XmlNode d505_03_02Node = detailsNode.SelectSingleNode("D505_03_02");
                if (d505_03_02Node != null)
                {
                    foreach (System.Xml.XmlNode d505_03_02ItemNode in d505_03_02Node)
                    {
                        Model.QzNhInterface.GetUpLoadInFeeInfo getUpLoadInFeeInfo = new Model.QzNhInterface.GetUpLoadInFeeInfo();
                        getUpLoadInFeeInfo.InPatNo = InPatNo;
                        getUpLoadInFeeInfo.YbRegNo = YbRegNo;
                        getUpLoadInFeeInfo.RegNo = RegNo;
                        getUpLoadInFeeInfo.IDDtl = d505_03_02ItemNode.SelectSingleNode("D505_14").InnerText;
                        getUpLoadInFeeInfo.Code = d505_03_02ItemNode.SelectSingleNode("D505_04").InnerText;
                        getUpLoadInFeeInfo.Name = d505_03_02ItemNode.SelectSingleNode("D505_16").InnerText;
                        getUpLoadInFeeInfo.DrugName = "";
                        getUpLoadInFeeInfo.Spec = "";
                        getUpLoadInFeeInfo.PriceIn = d505_03_02ItemNode.SelectSingleNode("D505_07").InnerText;
                        getUpLoadInFeeInfo.Totality = d505_03_02ItemNode.SelectSingleNode("D505_08").InnerText;
                        getUpLoadInFeeInfo.Amount = d505_03_02ItemNode.SelectSingleNode("D505_10").InnerText;
                        getUpLoadInFeeInfo.RegOperDate = d505_03_02ItemNode.SelectSingleNode("D505_13").InnerText;
                        getUpLoadInFeeInfo.YbCode = d505_03_02ItemNode.SelectSingleNode("D505_35").InnerText;
                        getUpLoadInFeeInfo.YbName = d505_03_02ItemNode.SelectSingleNode("D505_36").InnerText;
                        getUpLoadInFeeInfo.UnitName = d505_03_02ItemNode.SelectSingleNode("D505_37").InnerText;
                        getUpLoadInFeeInfo.DoctorName = d505_03_02ItemNode.SelectSingleNode("D505_12").InnerText;
                        getUpLoadInFeeInfo.BsInvInItemName = d505_03_02ItemNode.SelectSingleNode("D505_32").InnerText;
                        getUpLoadInFeeInfo.Package = "";
                        lstGetUpLoadInFeeDtl.Add(getUpLoadInFeeInfo);
                    }
                }
                XmlNode d505_03_03Node = detailsNode.SelectSingleNode("D505_03_03");
                if (d505_03_03Node != null)
                {
                    foreach (System.Xml.XmlNode d505_03_03ItemNode in d505_03_03Node)
                    {
                        Model.QzNhInterface.GetUpLoadInFeeInfo getUpLoadInFeeInfo = new Model.QzNhInterface.GetUpLoadInFeeInfo();
                        getUpLoadInFeeInfo.InPatNo = InPatNo;
                        getUpLoadInFeeInfo.YbRegNo = YbRegNo;
                        getUpLoadInFeeInfo.RegNo = RegNo;
                        getUpLoadInFeeInfo.IDDtl = d505_03_03ItemNode.SelectSingleNode("D505_14").InnerText;
                        getUpLoadInFeeInfo.Code = d505_03_03ItemNode.SelectSingleNode("D505_04").InnerText;
                        getUpLoadInFeeInfo.Name = d505_03_03ItemNode.SelectSingleNode("D505_16").InnerText;
                        getUpLoadInFeeInfo.DrugName = "";
                        getUpLoadInFeeInfo.Spec = "";
                        getUpLoadInFeeInfo.PriceIn = d505_03_03ItemNode.SelectSingleNode("D505_07").InnerText;
                        getUpLoadInFeeInfo.Totality = d505_03_03ItemNode.SelectSingleNode("D505_08").InnerText;
                        getUpLoadInFeeInfo.Amount = d505_03_03ItemNode.SelectSingleNode("D505_10").InnerText;
                        getUpLoadInFeeInfo.RegOperDate = d505_03_03ItemNode.SelectSingleNode("D505_13").InnerText;
                        getUpLoadInFeeInfo.YbCode = d505_03_03ItemNode.SelectSingleNode("D505_35").InnerText;
                        getUpLoadInFeeInfo.YbName = d505_03_03ItemNode.SelectSingleNode("D505_36").InnerText;
                        getUpLoadInFeeInfo.UnitName = d505_03_03ItemNode.SelectSingleNode("D505_37").InnerText;
                        getUpLoadInFeeInfo.DoctorName = d505_03_03ItemNode.SelectSingleNode("D505_12").InnerText;
                        getUpLoadInFeeInfo.BsInvInItemName = "";
                        getUpLoadInFeeInfo.Package = d505_03_03ItemNode.SelectSingleNode("D505_31").InnerText;
                        lstGetUpLoadInFeeDtl.Add(getUpLoadInFeeInfo);
                    }
                }
            }
            // XmlNode itemNode = bodySubNode.SelectSingleNode("item");
            return lstGetUpLoadInFeeDtl;
        }

        #endregion

        #region 出院办理B020005
        /// <summary>
        ///出院办理B020005
        ///</summary>
        /// <returns></returns>
        public string CancelInHosInfoCheckIn()
        {

            System.Xml.XmlDocument document = this.XmlDataExchangeModel("B020005", InfoInHosInfo.YbRegNo);
            XmlNode rootNode = document.DocumentElement;
            //XmlNode bodyNode = rootNode.SelectSingleNode("body");
            XmlNode bodyNode = rootNode["body"];
            bodyNode.AppendChild(this.SetElementValue(document, "D504_02", InfoInHosInfo.YbRegNo));//个人编码
            bodyNode.AppendChild(this.SetElementValue(document, "D504_09", InfoInHosInfo.InPatNo + InfoInHosInfo.NTime.ToString()));//住院号 
            string result = this.FinallySetXmlNs(document);
            document.LoadXml(result);
            XmlNode responseNode = document.SelectSingleNode("response");
            if (!this.ShowHeadMsg(responseNode))
            {
                return "失败";
            }
            Utilities.Information.ShowMsgBox("出院办理" + RetHeadMsg(responseNode));
            return "成功！";
        }
        #endregion

        # region 住院预结算B020011
        /// <summary>
        /// 获得住院结果(4.11住院预结算B020011)
        /// </summary>
        /// <returns></returns>
        string resultYbRegNo = string.Empty;
        public string LocationCode { set; get; }
        public string OutStateCode { set; get; }
        public override object GetInChargeResult()
        {
            //this.GetNhDictionary("9");
            System.Xml.XmlDocument document = this.XmlDataExchangeModel("B020011", InfoInHosInfo.YbRegNo);
            XmlNode rootNode = document.DocumentElement;
            //XmlNode bodyNode = rootNode.SelectSingleNode("body");
            XmlNode bodyNode = rootNode["body"];
            bodyNode.AppendChild(this.SetElementValue(document, "D504_09", InfoInHosInfo.InPatNo + InfoInHosInfo.NTime.ToString())); //住院号
            bodyNode.AppendChild(this.SetElementValue(document, "D504_02", InfoInHosInfo.YbRegNo)); //个人编码
            bodyNode.AppendChild(this.SetElementValue(document, "D506_52", InInvoiceInfo.InvoNo)); //发票号
            string InvoTime;
            if (string.IsNullOrEmpty(InInvoiceInfo.OperTime.ToString()) || InInvoiceInfo.OperTime.ToString().Trim() == "0001/1/1 0:00:00")
            {
                InvoTime = BLL.Common.DateTimeHandler.GetServerDateTime().ToString("yyyy-MM-ddTHH-mm-ss");
            }
            else
            {
                InvoTime = BLL.Common.DateTimeHandler.GetServerDateTime().ToString("yyyy-MM-ddTHH-mm-ss");
            }
            bodyNode.AppendChild(this.SetElementValue(document, "D506_53", InvoTime)); //发票时间
            bodyNode.AppendChild(this.SetElementValue(document, "D504_20", OutStateCode)); //出院状态
            bodyNode.AppendChild(this.SetElementValue(document, "D504_17", LocationCode));//.PadRight(4, ' ')));//.PadLeft(4,'0'))); //出院科室 
            Model.BsLocationInfo infoBsLocation = BLL.Common.Utils.GetBaseTableRowInfo<Model.BsLocationInfo>("BsLocation", InfoInHosInfo.LocationId);

            bodyNode.AppendChild(this.SetElementValue(document, "D504_54", infoBsLocation.Code)); //HIS科室代码
            bodyNode.AppendChild(this.SetElementValue(document, "D504_60", infoBsLocation.Name)); //HIS科室名称
            string outTime;
            if (string.IsNullOrEmpty(_inPatientInfo.OutTime.ToString()) || _inPatientInfo.OutTime.ToString().Trim() == "0001/1/1 0:00:00")
            {
                outTime = BLL.Common.DateTimeHandler.GetServerDateTime().ToString("yyyy-MM-ddTHH-mm-ss");
            }
            else
            {
                outTime = _inPatientInfo.OutTime.ToString("yyyy-MM-ddTHH-mm-ss");
            }
            bodyNode.AppendChild(this.SetElementValue(document, "D504_12", outTime)); //出院时间
            bodyNode.AppendChild(this.SetElementValue(document, "D504_03", _inPatientInfo.PatientName)); //患者姓名
            XmlElement subEle = document.CreateElement("item");// 
            ///大病
            string strD506_101 = string.Empty;
            string strD506_102 = string.Empty;
            BLL.CBsPatType objPatType = new BLL.CBsPatType();
            Model.BsPatTypeInfo infoPatType = objPatType.GetByID(InfoInHosInfo.PatTypeId);
            if (infoPatType.Name.Contains("大病"))
            {
                strD506_101 = "0200002";
                strD506_102 = GetDBID();
            }

            subEle.AppendChild(this.SetElementValue(document, "D506_101", strD506_101)); //类型代码  020002 大病
            //lstDictionary[0].
            subEle.AppendChild(this.SetElementValue(document, "D506_102", strD506_102)); // 类型值


            bodyNode.AppendChild(subEle);
            bodyNode.AppendChild(this.SetElementValue(document, "D506_54", IllName)); //出院诊断 
            bodyNode.AppendChild(this.SetElementValue(document, "D504_100", "")); //第三方保险 
            Utilities.Document doc1 = new Utilities.Document();
            doc1.Content = new StringBuilder(document.InnerXml);
            doc1.Create("c://住院预结算B020011(入).xml", false);
            string result = this.FinallySetXmlNs(document);
            document.LoadXml(result);
            //this.GetNhDictionary("15");
            StringBuilder sBuilder1 = new StringBuilder();
            sBuilder1.Append(result);

            Utilities.Document doc = new Utilities.Document();
            doc.Content = sBuilder1;
            doc.Create("c://住院预结算B020011(出).xml", false);
            //MessageBox.Show(sBuilder1.ToString());

            XmlNode responseNode = document.SelectSingleNode("response");
            if (!this.ShowHeadMsg(responseNode))
            {
                return "失败";
            }
            else
            {
                Utilities.Information.ShowMsgBox("住院预结算" + RetHeadMsg(responseNode));
                XmlNode bodySubNode = responseNode.SelectSingleNode("body");
                if (bodyNode == null)
                {
                    return "失败";
                }
                XmlNode baseInfoNode = bodySubNode.SelectSingleNode("baseInfo");
                if (baseInfoNode == null)
                {
                    return "失败";
                }
                XmlNode diagnoseInfoNode = bodySubNode.SelectSingleNode("diagnoseInfo");
                if (diagnoseInfoNode == null)
                {
                    return "失败";
                }
                XmlNode feeInfoNode = bodySubNode.SelectSingleNode("feeInfo");
                if (feeInfoNode == null)
                {
                    return "失败";
                }
                XmlNode allFeeSubentryNode = feeInfoNode.SelectSingleNode("allFeeSubentry");
                if (allFeeSubentryNode == null)
                {
                    return "失败";
                }
                XmlNode computeTypeFeeNode = feeInfoNode.SelectSingleNode("computeTypeFee");
                if (computeTypeFeeNode == null)
                {
                    return "失败";
                }



                double resultAmount = Convert.ToDouble(feeInfoNode.SelectSingleNode("D506_03").InnerText);
                //实际补偿金额(元)

                double resultYbAmount = 0;

                if (this.InfoPatType.Name.Contains("大病"))
                    resultYbAmount = Convert.ToDouble(feeInfoNode.SelectSingleNode("D506_24").InnerText)
                        + Convert.ToDouble(feeInfoNode.SelectSingleNode("D506_128").InnerText);
                else
                    resultYbAmount = Convert.ToDouble(feeInfoNode.SelectSingleNode("D506_24").InnerText);


                //<D506_111>常规比例</D506_111>
                //<D506_112>特殊病实付起付线</D506_112>
                //<D506_113>特殊病补偿比例</D506_113>
                //<D506_114>第三方保险实付起付线</D506_114>
                //<D506_115>第三方保险补偿比例</D506_115>
                //<D506_116>实付起付线</D506_116>

                StringBuilder sBuilderQFX = new StringBuilder();
                sBuilderQFX.AppendFormat("特殊病实付起付线：{0}", feeInfoNode.SelectSingleNode("D506_112").InnerText);
                sBuilderQFX.AppendLine();
                sBuilderQFX.AppendFormat("第三方保险实付起付线：{0}", feeInfoNode.SelectSingleNode("D506_114").InnerText);
                sBuilderQFX.AppendLine();
                sBuilderQFX.AppendFormat("实付起付线：{0}", feeInfoNode.SelectSingleNode("D506_116").InnerText);
                sBuilderQFX.AppendLine();
                sBuilderQFX.AppendFormat("实际补偿金额(元)：{0}", feeInfoNode.SelectSingleNode("D506_24").InnerText);
                sBuilderQFX.AppendLine();
                sBuilderQFX.AppendFormat("大病保险补偿金额（元）：{0}", feeInfoNode.SelectSingleNode("D506_128").InnerText);
                sBuilderQFX.AppendLine();
                Utilities.Information.ShowMsgBox(sBuilderQFX.ToString());

                resultYbRegNo = baseInfoNode.SelectSingleNode("D506_01").InnerText;
                string resultYbBcID = baseInfoNode.SelectSingleNode("D506_125").InnerText;
                Model.YbPatSeqInfo infoYbpatSeq = new Model.YbPatSeqInfo();
                BLL.CYbPatSeq objYbPatSeqInfo = new BLL.CYbPatSeq();
                Model.ModelList<Model.YbPatSeqInfo> lstYbpatSeq = objYbPatSeqInfo.YbPatSeq_SelectByHospId(InfoInHosInfo.ID);

                infoYbpatSeq = lstYbpatSeq[0];
                if (lstYbpatSeq.Count > 0)
                {
                    lstYbpatSeq[0].F2 = resultYbRegNo;
                    lstYbpatSeq[0].F3 = resultYbBcID;
                }
                else
                {
                    //Model.YbPatSeqInfo infoYbpatSeq = new Model.YbPatSeqInfo();
                    //infoYbpatSeq.MzRegId = this.InfoOuHosInfo.ID;
                    infoYbpatSeq.HospId = _infoInHosInfo.ID;
                    infoYbpatSeq.YbSeq = this.RegNo;
                    lstYbpatSeq.Add(infoYbpatSeq);
                }

                objYbPatSeqInfo.Modify(infoYbpatSeq, null);
                //double Amount = 0;
                //for (int i = 0; i < LstUspInBalanceDtl.Count; i++)
                //{
                //    if (!LstUspInBalanceDtl[i].IsPay)
                //    {
                //        Amount += LstUspInBalanceDtl[i].Amount;
                //    }
                //}
                double Amount = LstUspInBalanceDtl.GetSum("Amount");
                if (Math.Abs(resultAmount - Amount) > 0.1)
                {
                    MessageBox.Show("您上传的总费用跟社保接口结算的总费用不一致，请重新上传！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return "失败";
                }
                ReturnF2 = HospNo;
                ReturnF3 = resultYbAmount;
                DivideDisc(resultYbAmount);
                return "成功！";
            }

        }
        #endregion


        private string GetDBID()
        {
            Tools.ToolGridLookupInput frm = new Tools.ToolGridLookupInput();
            frm.lblTitle.Text = "大病类别";
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("ID", System.Type.GetType("System.Int32"));
            dt.Columns.Add("Code", System.Type.GetType("System.String"));
            dt.Columns.Add("Value", System.Type.GetType("System.Int32"));
            dt.Columns.Add("Name", System.Type.GetType("System.String"));
            dt.Columns.Add("IconIndex", System.Type.GetType("System.Int32"));
            dt.Columns.Add("PyCode", System.Type.GetType("System.String"));
            dt.Columns.Add("WbCode", System.Type.GetType("System.String"));
            frm.uspLookup.Properties.Items.Clear();

            int index = 1;
            foreach (YBInterface.NhDictionary item in GetNhDictionary("30"))
            {
                DevExpress.XtraEditors.Controls.ImageComboBoxItem imageBoxItem = new DevExpress.XtraEditors.Controls.ImageComboBoxItem();
                imageBoxItem.Value = index++;
                imageBoxItem.Description = string.Format("{0}({1})", item.D911_03, item.D911_06);
                frm.uspLookup.Properties.Items.Add(imageBoxItem);

                System.Data.DataRow dr = dt.NewRow();
                dr["ID"] = imageBoxItem.Value;
                dr["Name"] = imageBoxItem.Description;
                dr["Code"] = item.D911_02;
                dr["IconIndex"] = 0;
                dt.Rows.Add(dr);
            }

            frm.uspLookup.ID = 0;
            frm.uspLookup._dv = dt.DefaultView;
            if (frm.uspLookup.DataSource.Count > 15)
                frm.uspLookup.Properties.DropDownRows = 15;
            if (frm.ShowDialog() == DialogResult.OK && frm.SelectedID > 0) return Convert.ToString(frm.uspLookup.SelectedRow["code"]);
            return string.Empty;
        }


        #region 住院结算B020012
        /// <summary>
        /// 获得住院结果(4.11住院结算B020012)
        /// </summary>
        /// <returns></returns>
        string resultYbRegNoEnd = string.Empty;
        public override object GetInChargeResultEnd(Model.InHosInfoInfo infoInHosInfo)
        {
            CancelInHosInfoCheckIn();//办理出院登记
            System.Xml.XmlDocument document = this.XmlDataExchangeModel("B020012", InfoInHosInfo.YbRegNo);
            XmlNode rootNode = document.DocumentElement;
            //XmlNode bodyNode = rootNode.SelectSingleNode("body");
            XmlNode bodyNode = rootNode["body"];
            bodyNode.AppendChild(this.SetElementValue(document, "D504_09", InfoInHosInfo.InPatNo + InfoInHosInfo.NTime.ToString())); //住院号
            bodyNode.AppendChild(this.SetElementValue(document, "D504_02", InfoInHosInfo.YbRegNo)); //个人编码
            bodyNode.AppendChild(this.SetElementValue(document, "D506_52", "")); //发票号InInvoiceInfo.InvoNo
            string InvoTime;
            if (string.IsNullOrEmpty(InInvoiceInfo.OperTime.ToString()) || InInvoiceInfo.OperTime.ToString().Trim() == "0001/1/1 0:00:00")
            {
                InvoTime = BLL.Common.DateTimeHandler.GetServerDateTime().ToString("yyyy-MM-ddTHH-mm-ss");
            }
            else
            {
                InvoTime = BLL.Common.DateTimeHandler.GetServerDateTime().ToString("yyyy-MM-ddTHH-mm-ss");
            }
            bodyNode.AppendChild(this.SetElementValue(document, "D506_53", InvoTime)); //发票时间
            bodyNode.AppendChild(this.SetElementValue(document, "D504_20", "1")); //出院状态
            bodyNode.AppendChild(this.SetElementValue(document, "D504_17", "")); //出院科室
            bodyNode.AppendChild(this.SetElementValue(document, "D504_54", InfoInHosInfo.LocationId.ToString())); //HIS科室代码
            bodyNode.AppendChild(this.SetElementValue(document, "D504_60", new BLL.CBsLocation().GetByID(InfoInHosInfo.LocationId).Name.ToString())); //HIS科室名称
            string outTime;
            if (string.IsNullOrEmpty(_inPatientInfo.OutTime.ToString()) || _inPatientInfo.OutTime.ToString().Trim() == "0001/1/1 0:00:00")
            {
                outTime = BLL.Common.DateTimeHandler.GetServerDateTime().ToString("yyyy-MM-ddTHH-mm-ss");
            }
            else
            {
                outTime = _inPatientInfo.OutTime.ToString("yyyy-MM-ddTHH-mm-ss");
            }
            bodyNode.AppendChild(this.SetElementValue(document, "D504_12", outTime)); //出院时间
            bodyNode.AppendChild(this.SetElementValue(document, "D504_03", _inPatientInfo.PatientName)); //患者姓名
            XmlElement subEle = document.CreateElement("item");// 
            ///大病
            string strD506_101 = string.Empty;
            string strD506_102 = string.Empty;
            BLL.CBsPatType objPatType = new BLL.CBsPatType();
            Model.BsPatTypeInfo infoPatType = objPatType.GetByID(InfoInHosInfo.PatTypeId);
            if (infoPatType.Name.Contains("大病"))
            {
                strD506_101 = "0200002";
                strD506_102 = GetDBID();
            }

            subEle.AppendChild(this.SetElementValue(document, "D506_101", strD506_101)); //类型代码  020002 大病
            //lstDictionary[0].
            subEle.AppendChild(this.SetElementValue(document, "D506_102", strD506_102)); // 类型值
            //subEle.AppendChild(this.SetElementValue(document, "D506_101", "")); //类型代码
            //subEle.AppendChild(this.SetElementValue(document, "D506_102", "")); // 类型值 大病补偿类别ID
            bodyNode.AppendChild(subEle);
            bodyNode.AppendChild(this.SetElementValue(document, "D506_54", IllName)); //出院诊断 
            bodyNode.AppendChild(this.SetElementValue(document, "D504_100", "")); //
            string result = this.FinallySetXmlNs(document);
            document.LoadXml(result);

            StringBuilder sBuilder1 = new StringBuilder();
            sBuilder1.Append(result);

            Utilities.Document doc = new Utilities.Document();
            doc.Content = sBuilder1;
            doc.Create("c://住院结算B020012.txt", false);
            //MessageBox.Show(sBuilder1.ToString());

            XmlNode responseNode = document.SelectSingleNode("response");
            if (!this.ShowHeadMsg(responseNode))
            {
                ReturnOu(infoInHosInfo.YbRegNo, infoInHosInfo.InPatNo + infoInHosInfo.NTime.ToString());
                return "失败";
            }
            else
            {
                Utilities.Information.ShowMsgBox(RetHeadMsg(responseNode));
                XmlNode bodySubNode = responseNode.SelectSingleNode("body");
                if (bodyNode == null)
                {
                    ReturnOu(infoInHosInfo.YbRegNo, infoInHosInfo.InPatNo + infoInHosInfo.NTime.ToString());
                    return "失败";
                }
                XmlNode baseInfoNode = bodySubNode.SelectSingleNode("baseInfo");
                if (baseInfoNode == null)
                {
                    ReturnOu(infoInHosInfo.YbRegNo, infoInHosInfo.InPatNo + infoInHosInfo.NTime.ToString());
                    return "失败";
                }
                XmlNode diagnoseInfoNode = bodySubNode.SelectSingleNode("diagnoseInfo");
                if (diagnoseInfoNode == null)
                {
                    ReturnOu(infoInHosInfo.YbRegNo, infoInHosInfo.InPatNo + infoInHosInfo.NTime.ToString());
                    return "失败";
                }
                XmlNode feeInfoNode = bodySubNode.SelectSingleNode("feeInfo");
                if (feeInfoNode == null)
                {
                    ReturnOu(infoInHosInfo.YbRegNo, infoInHosInfo.InPatNo + infoInHosInfo.NTime.ToString());
                    return "失败";
                }
                XmlNode allFeeSubentryNode = feeInfoNode.SelectSingleNode("allFeeSubentry");
                if (allFeeSubentryNode == null)
                {
                    ReturnOu(infoInHosInfo.YbRegNo, infoInHosInfo.InPatNo + infoInHosInfo.NTime.ToString());
                    return "失败";
                }
                XmlNode computeTypeFeeNode = feeInfoNode.SelectSingleNode("computeTypeFee");
                if (computeTypeFeeNode == null)
                {
                    ReturnOu(infoInHosInfo.YbRegNo, infoInHosInfo.InPatNo + infoInHosInfo.NTime.ToString());
                    return "失败";
                }



                double resultAmount = Convert.ToDouble(feeInfoNode.SelectSingleNode("D506_03").InnerText);
                double resultYbAmount = Convert.ToDouble(feeInfoNode.SelectSingleNode("D506_24").InnerText);
                string resultQFX = feeInfoNode.SelectSingleNode("D506_116").InnerText;//起付线
                StringBuilder sBuilderQFX = new StringBuilder();
                sBuilderQFX.AppendFormat("特殊病实付起付线：{0}", feeInfoNode.SelectSingleNode("D506_112").InnerText);
                sBuilderQFX.AppendLine();
                sBuilderQFX.AppendFormat("第三方保险实付起付线：{0}", feeInfoNode.SelectSingleNode("D506_114").InnerText);
                sBuilderQFX.AppendLine();
                sBuilderQFX.AppendFormat("实付起付线：{0}", feeInfoNode.SelectSingleNode("D506_116").InnerText);
                sBuilderQFX.AppendLine();
                Utilities.Information.ShowMsgBox(sBuilderQFX.ToString());
                resultYbRegNo = baseInfoNode.SelectSingleNode("D506_01").InnerText;
                string resultYbBcID = baseInfoNode.SelectSingleNode("D506_125").InnerText;
                Model.YbPatSeqInfo infoYbpatSeq = new Model.YbPatSeqInfo();
                BLL.CYbPatSeq objYbPatSeqInfo = new BLL.CYbPatSeq();
                Model.ModelList<Model.YbPatSeqInfo> lstYbpatSeq = objYbPatSeqInfo.YbPatSeq_SelectByHospId(InfoInHosInfo.ID);

                infoYbpatSeq = lstYbpatSeq[0];
                if (lstYbpatSeq.Count > 0)
                {
                    lstYbpatSeq[0].F2 = resultYbRegNo;
                    lstYbpatSeq[0].F3 = resultYbBcID;//大病医保补偿ID
                }
                else
                {
                    //Model.YbPatSeqInfo infoYbpatSeq = new Model.YbPatSeqInfo();
                    //infoYbpatSeq.MzRegId = this.InfoOuHosInfo.ID;
                    infoYbpatSeq.HospId = _infoInHosInfo.ID;
                    infoYbpatSeq.YbSeq = this.RegNo;
                    lstYbpatSeq.Add(infoYbpatSeq);
                }

                objYbPatSeqInfo.Modify(infoYbpatSeq, null);

                double Amount = LstUspInBalanceDtl.GetSum("Amount");
                if (Math.Abs(resultAmount - Amount) > 0.1)
                {
                    MessageBox.Show("您上传的总费用跟社保接口结算的总费用不一致，请重新上传！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return "失败";
                }
                ReturnF2 = HospNo;
                ReturnF3 = resultYbAmount;
                ReturnQFX = resultQFX;
                DivideDisc(resultYbAmount);
                return "成功！";
            }
        }
        #endregion


        #region 住院发票B020006 医保不用发票
        /// <summary>
        /// 住院发票B020006 医保不用发票
        /// </summary>
        /// <returns></returns>
        //public override string UpLoadSaver()
        //{
        //    System.Xml.XmlDocument document = this.DataExchangeModel("B020006");
        //    XmlNode rootNode = document.DocumentElement;
        //    XmlNode bodyNode = rootNode["body"];
        //    //XmlNode bodyNode = rootNode.SelectSingleNode("body");
        //    bodyNode.AppendChild(this.SetElementValue(document, "D506_01", resultYbRegNo));//住院补偿流水号 
        //    string result = this.FinallySetXmlNs(document);
        //    document.LoadXml(result);

        //    StringBuilder sBuilder1 = new StringBuilder();
        //    sBuilder1.Append(result);

        //    Utilities.Document doc = new Utilities.Document();
        //    doc.Content = sBuilder1;
        //    doc.Create("c://住院发票B020006.txt", false);
        //    MessageBox.Show(sBuilder1.ToString());


        //    XmlNode responseNode = document.SelectSingleNode("response");
        //    if (!this.ShowHeadMsg(responseNode))
        //    {
        //        return "失败";
        //    }
        //    return "成功";
        //}
        #endregion

        #region 大病结算单C020015
        /// <summary>
        /// 大病结算单C020015
        /// </summary>
        /// <returns></returns>
        public override XmlDocument LoadInChargeDB()
        {
            System.Xml.XmlDocument document = this.XmlDataExchangeModel("C020015", "");
            XmlNode rootNode = document.DocumentElement;
            XmlNode bodyNode = rootNode["body"];
            string resultYbBcID = "";
            if (string.IsNullOrEmpty(resultYbRegNo))
            {
                BLL.CYbPatSeq objYbPatSeqInfo = new BLL.CYbPatSeq();
                Model.ModelList<Model.YbPatSeqInfo> lstYbpatSeq = objYbPatSeqInfo.YbPatSeq_SelectByHospId(InfoInHosInfo.ID);
                if (lstYbpatSeq.Count == 0)
                {
                    MessageBox.Show("该病人不是新农合医保病人");
                    return document;
                }
                resultYbRegNo = lstYbpatSeq[0].F2;
                resultYbBcID = lstYbpatSeq[0].F3;
            }
            //XmlNode bodyNode = rootNode.SelectSingleNode("body");
            bodyNode.AppendChild(this.SetElementValue(document, "D504_02", InfoInHosInfo.YbRegNo));//住院补偿流水号 个人编码    
            bodyNode.AppendChild(this.SetElementValue(document, "D504_09", string.Empty));//D504_09	住院号
            bodyNode.AppendChild(this.SetElementValue(document, "D506_97", InInvoiceInfo.OperTime.AddHours(-1).ToString("yyyy-MM-ddTHH-mm-ss")));//D506_97	起始结算日期	String(19)
            bodyNode.AppendChild(this.SetElementValue(document, "D506_98", InInvoiceInfo.OperTime.AddHours(1).ToString("yyyy-MM-ddTHH-mm-ss")));//D506_98终止结算日期
            bodyNode.AppendChild(this.SetElementValue(document, "D506_125", string.IsNullOrEmpty(resultYbBcID) ? "0" : resultYbBcID));//D506_125	大病保险补偿ID


            Utilities.Document doc1 = new Utilities.Document();
            doc1.Content = new StringBuilder();
            doc1.Content.Append(document.InnerXml);
            doc1.Create("c://大病结算单C020015.xml", false);
            string result = this.FinallySetXmlNs(document);
            document.LoadXml(result);

            StringBuilder sBuilder1 = new StringBuilder();
            sBuilder1.Append(result);

            Utilities.Document doc = new Utilities.Document();
            doc.Content = sBuilder1;
            doc.Create("c://大病结算单C020015(结果).xml", false);
            // MessageBox.Show(sBuilder1.ToString());

            XmlNode responseNode = document.SelectSingleNode("response");
            if (!this.ShowHeadMsg(responseNode))
            {
                return document;
            }
            else
            {
                XmlNode nodeResponse = document.SelectSingleNode("response");
                XmlNode nodeBody = nodeResponse.SelectSingleNode("body");
                XmlNode nodeItem = nodeBody.SelectSingleNode("item");

                foreach (XmlNode item in nodeItem.ChildNodes)
                {
                    switch (item.Name)
                    {
                        case "D504_04"://患者性别 
                            item.InnerText = item.InnerText.Trim() == "1" ? "男" : "女";
                            break;
                        case "D504_11"://入院时间 
                        case "D504_12"://出院时间
                        case "D506_129"://大病结算时间（大病补偿日期）
                        case "D506_26"://农合结算时间（农合补偿日期）
                        case "506_53"://发票时间
                            item.InnerText = item.InnerText.Replace("T", " ");
                            break;
                    }
                    if (System.Text.RegularExpressions.Regex.IsMatch(item.InnerText, @"^[0-9]+(.[0-9]{1,4})?$"))
                    {
                        item.InnerText = BLL.Common.Utils.Round(Convert.ToDouble(item.InnerText), 2).ToString("0.00");
                    }

                }

                //         <D504_04></D504_04>
                //         <D504_75>联系电话</D504_75>
                //         <D504_02>医保卡号（参合号）</D504_02>
                //         <D501_87>被保险人类型</D501_87>
                //         <D505_15>人员类别（忽略）</D505_15>
                //         <D504_10>就医类型（特殊疾病大额门诊/住院）</D504_10>
                //         <D506_16>补偿类别（就诊类别）</D506_16>
                //         <D504_101>就医模式（本地/异地）</D504_101>
                //         <D101_02>医疗机构（就医机构）</D101_02>
                //         <D504_15>医院等级（无等级）</D504_15>
                //         <D504_50>诊断疾病</D504_50>
                //         <D504_11>入院时间</D504_11>
                //         <D504_12>出院时间</D504_12>
                //         <D504_13>实际住院天数</D504_13>
                //         <D506_129>大病结算时间（大病补偿日期）</D506_129>
                //         <D506_26>农合结算时间（农合补偿日期）</D506_26>
                //         <D506_52>发票号</D506_52>
                //         <D506_53>发票时间</D506_53>
                //         <D506_27>农合经办人</D506_27>
                //         <D506_127>大病经办人</D506_127>
                //         <D503_20>结算标识</D503_20>
                //         <D506_03>医疗费用发生金额（总费用）</D506_03>
                //         <D506_19>可报总费用(农合补偿范围内费用)</D506_19>
                //         <D506_24>基本医疗支付费用（农合补偿金额）</D506_24>
                //         <D506_130>个人支出（个人自付+个人自负）</D506_130>
                //         <D506_131>大病可报销费用（大病保险合规金额）</D506_131>
                //         <D506_128>大病赔付费用（大病保险补偿金额）</D506_128>
                //         <D506_132>个人自付（大病保险个人自付）</D506_132>
                //         <D506_32>个人自负</D506_32>
                //         <D506_133>自理费用</D506_133>
                //         <D506_77>自费费用（补偿范围外费用）</D506_77>
                //         <D506_134>剔除金额</D506_134>
                //         <D506_135>转外自理金额</D506_135>
                //         <D506_136>第三方补助费用</D506_136>
                //         <D506_137>其他报销金额</D506_137>
                //<D506_116>实付起付线</D506_116>
                //<D506_142>不予支付金额</D506_142>
                //<D506_143>大病核算机构</D506_143>
                //<D506_144>承保机构</D506_144>
                //<D506_145>社保机构</D506_145>
                //<D504_63>转外类型</D504_63>
                //<D504_69>转出医疗机构名称</D504_69>
                //<D504_29>转诊登记流水号</D504_29>
                //<D504_09>HIS住院号</D504_09>
                //<D506_146>本年度历次大病已补偿金额</D506_146>
                //<D506_147>本年度历次大病合规金额</D506_147>
                //<D506_148>本次大病扣除起付线</D506_148>

            }
            return document;
        }


        /// <summary>
        /// 大病结算单C020015
        /// </summary>
        /// <returns></returns>
        public XmlDocument LoadInChargeDB(string YbRegNo, string InPatNo, DateTime BeginTime, DateTime EndTime,
            string resultYbBcID)
        {
            System.Xml.XmlDocument document = this.XmlDataExchangeModel("C020015", YbRegNo);
            XmlNode rootNode = document.DocumentElement;
            XmlNode bodyNode = rootNode["body"];
            bodyNode.AppendChild(this.SetElementValue(document, "D504_02", YbRegNo.Length == 6 ? string.Empty : YbRegNo));//住院补偿流水号 个人编码    
            bodyNode.AppendChild(this.SetElementValue(document, "D504_09", InPatNo));//D504_09	住院号
            bodyNode.AppendChild(this.SetElementValue(document, "D506_97", BeginTime.ToString("yyyy-MM-ddTHH-mm-ss")));//D506_97	起始结算日期	String(19)
            bodyNode.AppendChild(this.SetElementValue(document, "D506_98", EndTime.ToString("yyyy-MM-ddTHH-mm-ss")));//D506_98终止结算日期
            bodyNode.AppendChild(this.SetElementValue(document, "D506_125", string.IsNullOrEmpty(resultYbBcID) ? "0" : resultYbBcID));//D506_125	大病保险补偿ID


            Utilities.Document doc1 = new Utilities.Document();
            doc1.Content = new StringBuilder();
            doc1.Content.Append(document.InnerXml.Replace(" xmlns=\"\"", ""));
            doc1.Create("c://大病结算单C020015.xml", false);
            string result = this.FinallySetXmlNs(document);
            document.LoadXml(result);

            StringBuilder sBuilder1 = new StringBuilder();
            sBuilder1.Append(result);

            Utilities.Document doc = new Utilities.Document();
            doc.Content = sBuilder1;
            doc.Create("c://大病结算单C020015(结果).xml", false);

            XmlNode responseNode = document.SelectSingleNode("response");
            if (!this.ShowHeadMsg(responseNode))
            {
                return document;
            }
            else
            {
                XmlNode nodeResponse = document.SelectSingleNode("response");
                XmlNode nodeBody = nodeResponse.SelectSingleNode("body");
                XmlNode nodeItem = nodeBody.SelectSingleNode("item");

                foreach (XmlNode item in nodeItem.ChildNodes)
                {
                    switch (item.Name)
                    {
                        case "D504_04"://患者性别 
                            item.InnerText = item.InnerText.Trim() == "1" ? "男" : "女";
                            break;
                        case "D504_11"://入院时间 
                        case "D504_12"://出院时间
                        case "D506_129"://大病结算时间（大病补偿日期）
                        case "D506_26"://农合结算时间（农合补偿日期）
                        case "506_53"://发票时间
                            item.InnerText = item.InnerText.Replace("T", " ");
                            break;
                    }
                    if (System.Text.RegularExpressions.Regex.IsMatch(item.InnerText, @"^[0-9]+(.[0-9]{1,4})?$"))
                    {
                        item.InnerText = BLL.Common.Utils.Round(Convert.ToDouble(item.InnerText), 2).ToString("0.00");
                    }

                }

            }
            return document;
        }
        #endregion

        #region 住院结算单B020007
        /// <summary>
        /// 住院结算单B020007
        /// </summary>
        /// <returns></returns>
        public XmlDocument LoadInCharge()
        {
            System.Xml.XmlDocument document = this.XmlDataExchangeModel("B020007", "");
            XmlNode rootNode = document.DocumentElement;
            XmlNode bodyNode = rootNode["body"];
            if (string.IsNullOrEmpty(resultYbRegNo))
            {
                BLL.CYbPatSeq objYbPatSeqInfo = new BLL.CYbPatSeq();
                Model.ModelList<Model.YbPatSeqInfo> lstYbpatSeq = objYbPatSeqInfo.YbPatSeq_SelectByHospId(InfoInHosInfo.ID);
                if (lstYbpatSeq.Count == 0)
                {
                    MessageBox.Show("该病人不是新农合医保病人");
                    return document;
                }
                resultYbRegNo = lstYbpatSeq[0].F2;
            }
            //XmlNode bodyNode = rootNode.SelectSingleNode("body");
            bodyNode.AppendChild(this.SetElementValue(document, "D506_01", resultYbRegNo));//住院补偿流水号 
            string result = this.FinallySetXmlNs(document);
            document.LoadXml(result);

            StringBuilder sBuilder1 = new StringBuilder();
            sBuilder1.Append(result);

            Utilities.Document doc = new Utilities.Document();
            doc.Content = sBuilder1;
            doc.Create("c://住院结算单B020007.txt", false);
            // MessageBox.Show(sBuilder1.ToString());

            XmlNode responseNode = document.SelectSingleNode("response");
            if (!this.ShowHeadMsg(responseNode))
            {
                return document;
            }
            return document;
        }
        #endregion

        #region 住院补偿公示表C020005
        /// <summary>
        /// 住院补偿公示表C020005
        /// </summary>
        /// <returns></returns>
        public List<Model.CompensationPublicityBill> GetCompensationPublicityBill(string dateType, string beginTime,
            string endTime, string SeeCategories, string YbRegNo)
        {

            List<Model.CompensationPublicityBill> lstCompensationPublicityBillF = new List<Model.CompensationPublicityBill>();
            //Model.CompensationPublicityBill compensationPublicityBillInfo1= new Model.CompensationPublicityBill();
            List<Model.CompensationPublicityBill> lstCompensationPublicityBill = new List<Model.CompensationPublicityBill>();
            //Model.ModelList<Model.QzNhInterface.CompensationPublicityBill> lstCompensationPublicityBill = new Model.ModelList<Model.QzNhInterface.CompensationPublicityBill>();// (InfoInHosInfo.ID);
            System.Xml.XmlDocument document = this.XmlDataExchangeModel("C020005", YbRegNo);
            XmlNode rootNode = document.DocumentElement;
            XmlNode bodyNode = rootNode["body"];
            //XmlNode bodyNode = rootNode.SelectSingleNode("body");
            bodyNode.AppendChild(this.SetElementValue(document, "D506_99", dateType));//日期类型 
            bodyNode.AppendChild(this.SetElementValue(document, "D506_97", beginTime));//起始结算日期 
            bodyNode.AppendChild(this.SetElementValue(document, "D506_98", endTime));//终止结算日期 
            bodyNode.AppendChild(this.SetElementValue(document, "D504_10", SeeCategories));//日期类型 
            string result = this.FinallySetXmlNs(document);
            document.LoadXml(result);

            StringBuilder sBuilder1 = new StringBuilder();
            sBuilder1.Append(result);

            Utilities.Document doc = new Utilities.Document();
            doc.Content = sBuilder1;
            doc.Create("c://住院补偿公示表C020005.txt", false);
            // MessageBox.Show(sBuilder1.ToString());

            XmlNode responseNode = document.SelectSingleNode("response");
            XmlNode bodySubNode = responseNode.SelectSingleNode("body");
            if (bodySubNode == null)
            {
                MessageBox.Show("该时间段内没有住院补偿公示记录！");
            }
            else
            {
                foreach (System.Xml.XmlNode itemNode in bodySubNode)
                {
                    Model.CompensationPublicityBill compensationPublicityBillInfo = new Model.CompensationPublicityBill();
                    compensationPublicityBillInfo.Name = itemNode.SelectSingleNode("D504_03").InnerText;
                    if (itemNode.SelectSingleNode("D504_04").InnerText.Trim() == "1")
                    {
                        compensationPublicityBillInfo.Sex = "男";
                    }
                    else if (itemNode.SelectSingleNode("D504_04").InnerText.Trim() == "2")
                    {
                        compensationPublicityBillInfo.Sex = "女";
                    }
                    else
                    {
                        compensationPublicityBillInfo.Sex = "其他";
                    }
                    if (!string.IsNullOrEmpty(itemNode.SelectSingleNode("D504_74").InnerText))
                    {
                        compensationPublicityBillInfo.Age = (BLL.Common.DateTimeHandler.GetServerDateTime().Year - Convert.ToDateTime(itemNode.SelectSingleNode("D504_74").InnerText.Trim()).Year).ToString();
                    }
                    else
                    {
                        compensationPublicityBillInfo.Age = "";
                    }
                    compensationPublicityBillInfo.Address = itemNode.SelectSingleNode("D301_09").InnerText;
                    if (dateType.Trim() == "1")
                    {
                        compensationPublicityBillInfo.CompensationType = "补偿日期";
                    }
                    else
                    {
                        compensationPublicityBillInfo.CompensationType = "发票日期";
                    }
                    compensationPublicityBillInfo.InTime = itemNode.SelectSingleNode("D504_11").InnerText.Trim().Substring(0, 10);
                    compensationPublicityBillInfo.OutTime = itemNode.SelectSingleNode("D504_12").InnerText.Trim().Substring(0, 10);
                    if (!string.IsNullOrEmpty(itemNode.SelectSingleNode("D504_11").InnerText.Trim()) && !string.IsNullOrEmpty(itemNode.SelectSingleNode("D504_12").InnerText.Trim()))
                    {
                        DateTime intime = Convert.ToDateTime(itemNode.SelectSingleNode("D504_11").InnerText.Trim().Substring(0, 10));
                        DateTime outtime = Convert.ToDateTime(itemNode.SelectSingleNode("D504_12").InnerText.Trim().Substring(0, 10));//.ToString("yyyyMMdd HH:mm:ss");
                        compensationPublicityBillInfo.SkyNumber = (outtime - intime).Days.ToString();
                    }
                    else
                    {
                        compensationPublicityBillInfo.SkyNumber = "0";
                    }

                    compensationPublicityBillInfo.OriginalAmount = itemNode.SelectSingleNode("D506_03").InnerText;
                    compensationPublicityBillInfo.CompensationAmount = itemNode.SelectSingleNode("D506_24").InnerText;
                    compensationPublicityBillInfo.CompensationDate = itemNode.SelectSingleNode("D506_26").InnerText.Trim().Substring(0, 10);

                    //处理冲对数据PJJ 2015-3-23
                    if (!string.IsNullOrEmpty(itemNode.SelectSingleNode("D503_20").InnerText))
                    {
                        if (itemNode.SelectSingleNode("D503_20").InnerText.ToString() == "-1")
                        {
                            lstCompensationPublicityBillF.Add(compensationPublicityBillInfo);
                            continue;
                        }
                    }

                    lstCompensationPublicityBill.Add(compensationPublicityBillInfo);
                }
                cancelLst(lstCompensationPublicityBill, lstCompensationPublicityBillF);
                lstCompensationPublicityBill.AddRange(lstCompensationPublicityBillF);
                return lstCompensationPublicityBill;
            }

            return lstCompensationPublicityBill = null;
        }
        #endregion
        //处理冲对数据PJJ 2015-3-23
        private void cancelLst(List<Model.CompensationPublicityBill> lstBill, List<Model.CompensationPublicityBill> lstBillF)
        {
            foreach (Model.CompensationPublicityBill itemF in lstBillF)
            {
                foreach (Model.CompensationPublicityBill item in lstBill)
                {
                    if (item.Address == itemF.Address && item.Age == itemF.Age
                    && item.CompensationType == itemF.CompensationType && item.InTime == itemF.InTime && item.Name == itemF.Name && Math.Abs(Convert.ToDouble(item.OriginalAmount)).ToString() == Math.Abs(Convert.ToDouble(itemF.OriginalAmount)).ToString()
                    && item.OutTime == itemF.OutTime && item.Sex == itemF.Sex && item.SkyNumber == itemF.SkyNumber)
                    {
                        if (Convert.ToDouble(item.CompensationAmount) + Convert.ToDouble(itemF.CompensationAmount) == 0)
                        {
                            lstBill.Remove(item);
                            lstBillF.Remove(itemF);
                            cancelLst(lstBill, lstBillF);
                            break;
                        }
                    }
                }
                break;
            }
        }

        #region 出院补偿办理汇总表C020013
        /// <summary>
        /// 出院补偿办理汇总表C020013
        /// </summary>
        /// <returns></returns>
        public List<Model.YbHospitalOuBill> GetHospitalOuBill(string areaCode, string dateType, string beginTime, string endTime, string SeeCategories)
        {

            // Model.YbHospitalOuBill YbHospitalOuBillInfo1 = new Model.YbHospitalOuBill();
            List<Model.YbHospitalOuBill> lstYbHospitalOuBill = new List<Model.YbHospitalOuBill>();
            System.Xml.XmlDocument document = this.XmlDataExchangeModel("C020013", areaCode);
            XmlNode rootNode = document.DocumentElement;
            XmlNode bodyNode = rootNode["body"];
            bodyNode.AppendChild(this.SetElementValue(document, "D506_99", dateType));//日期类型 
            bodyNode.AppendChild(this.SetElementValue(document, "D506_97", beginTime));//起始结算日期 
            bodyNode.AppendChild(this.SetElementValue(document, "D506_98", endTime));//终止结算日期 
            bodyNode.AppendChild(this.SetElementValue(document, "D504_10", SeeCategories));//日期类型 


            StringBuilder sBuilder1 = new StringBuilder();
            sBuilder1.Append(document.InnerXml);

            Utilities.Document doc = new Utilities.Document();
            doc.Content = sBuilder1;
            doc.Create("c://出院补偿办理汇总表(入).xml", false);

            string result = this.FinallySetXmlNs(document);
            document.LoadXml(result);

            sBuilder1 = new StringBuilder();
            sBuilder1.Append(result);
            doc = new Utilities.Document();
            doc.Content = sBuilder1;
            doc.Create("c://出院补偿办理汇总表(出).xml", false);

            XmlNode responseNode = document.SelectSingleNode("response");
            XmlNode bodySubNode = responseNode.SelectSingleNode("body");
            if (bodySubNode == null)
            {
                MessageBox.Show("该时间段内没有出院补偿办理记录！");
            }
            else
            {
                Model.ModelList<NhDictionary> lstDictionary = GetNhDictionary("25", areaCode);
                foreach (System.Xml.XmlNode itemNode in bodySubNode)
                {
                    Model.YbHospitalOuBill YbHospitalOuBillInfo = new Model.YbHospitalOuBill();
                    YbHospitalOuBillInfo.Name = itemNode.SelectSingleNode("D504_03").InnerText;
                    if (itemNode.SelectSingleNode("D504_04").InnerText.Trim() == "1")
                    {
                        YbHospitalOuBillInfo.Sex = "男";
                    }
                    else if (itemNode.SelectSingleNode("D504_04").InnerText.Trim() == "2")
                    {
                        YbHospitalOuBillInfo.Sex = "女";
                    }
                    else
                    {
                        YbHospitalOuBillInfo.Sex = "其他";
                    }
                    if (!string.IsNullOrEmpty(itemNode.SelectSingleNode("D504_74").InnerText))
                    {
                        YbHospitalOuBillInfo.Age = (BLL.Common.DateTimeHandler.GetServerDateTime().Year - Convert.ToDateTime(itemNode.SelectSingleNode("D504_74").InnerText.Trim().Substring(0, 10)).Year).ToString();
                    }
                    else
                    {
                        YbHospitalOuBillInfo.Age = "";
                    }
                    YbHospitalOuBillInfo.Address = itemNode.SelectSingleNode("D301_09").InnerText;
                    YbHospitalOuBillInfo.IdCardNo = itemNode.SelectSingleNode("D504_05").InnerText;
                    YbHospitalOuBillInfo.MedicalNo = itemNode.SelectSingleNode("D504_02").InnerText.Trim().Substring(6, 12);
                    YbHospitalOuBillInfo.InTime = itemNode.SelectSingleNode("D504_11").InnerText.Trim().Substring(0, 10);
                    YbHospitalOuBillInfo.OutTime = itemNode.SelectSingleNode("D504_12").InnerText.Trim().Substring(0, 10);
                    YbHospitalOuBillInfo.IllName = itemNode.SelectSingleNode("D504_29").InnerText;
                    YbHospitalOuBillInfo.OriginalAmount = itemNode.SelectSingleNode("D506_03").InnerText;
                    NhDictionary infoNhDictionary = new NhDictionary();
                    string strD504_10 = itemNode.SelectSingleNode("D504_10").InnerText;
                    foreach (NhDictionary info in lstDictionary)
                    {
                        if (info.D911_02.Trim() == strD504_10.Trim())
                        {
                            YbHospitalOuBillInfo.CompensationType = info.D911_03;

                        }
                    }
                    //YbHospitalOuBillInfo.CompensationType = lstDictionary.Find("D911_02", itemNode.SelectSingleNode("D504_10").InnerText)[0].D911_03;
                    //if (dateType.Trim() == "1")
                    //{
                    //    YbHospitalOuBillInfo.CompensationType = "补偿日期";
                    //}
                    //else
                    //{
                    //    YbHospitalOuBillInfo.CompensationType = "发票日期";
                    //}
                    YbHospitalOuBillInfo.CompensationAmount = itemNode.SelectSingleNode("D506_24").InnerText;

                    lstYbHospitalOuBill.Add(YbHospitalOuBillInfo);
                }
                return lstYbHospitalOuBill;
            }
            return lstYbHospitalOuBill = null;
        }
        #endregion


        #region 拨付垫付补偿资金报表C020014
        /// <summary>
        /// 拨付垫付补偿资金报表C020014
        /// </summary>
        /// <returns></returns>
        public Model.YbCompensationMoneyBill GetCompensationMoneyBill(string areaCode, string dateType, string beginTime, string endTime, string SeeCategories)
        {
            Model.YbCompensationMoneyBill YbCompensationMoneyBillInfo = new Model.YbCompensationMoneyBill();
            System.Xml.XmlDocument document = this.XmlDataExchangeModel("C020014", areaCode);
            XmlNode rootNode = document.DocumentElement;
            XmlNode bodyNode = rootNode["body"];
            bodyNode.AppendChild(this.SetElementValue(document, "D506_99", dateType));//日期类型 
            bodyNode.AppendChild(this.SetElementValue(document, "D506_97", beginTime));//起始结算日期 
            bodyNode.AppendChild(this.SetElementValue(document, "D506_98", endTime));//终止结算日期 
            bodyNode.AppendChild(this.SetElementValue(document, "D504_10", SeeCategories));//日期类型 


            StringBuilder sBuilder1 = new StringBuilder();
            sBuilder1.Append(document.InnerXml);

            Utilities.Document doc = new Utilities.Document();
            doc.Content = sBuilder1;
            doc.Create("c://拨付垫付补偿资金报表(入).xml", false);

            string result = this.FinallySetXmlNs(document);
            document.LoadXml(result);

            sBuilder1 = new StringBuilder();
            sBuilder1.Append(result);

            doc = new Utilities.Document();
            doc.Content = sBuilder1;
            doc.Create("c://拨付垫付补偿资金报表(出).xml", false);

            XmlNode responseNode = document.SelectSingleNode("response");
            XmlNode bodySubNode = responseNode.SelectSingleNode("body");
            if (bodySubNode == null)
            {
                MessageBox.Show("该时间段内没有拨付垫付补偿资金！");
            }
            else
            {
                YbCompensationMoneyBillInfo.NameSum = bodySubNode.SelectSingleNode("D910_07").InnerText;
                YbCompensationMoneyBillInfo.InOriginalAmount = bodySubNode.SelectSingleNode("D910_09").InnerText;
                YbCompensationMoneyBillInfo.MedicineAmount = bodySubNode.SelectSingleNode("D506_66").InnerText;
                YbCompensationMoneyBillInfo.DirectoryAmount = bodySubNode.SelectSingleNode("D604_44").InnerText;
                YbCompensationMoneyBillInfo.MedicineRate = bodySubNode.SelectSingleNode("D102_12").InnerText;
                YbCompensationMoneyBillInfo.DiagnosisAmount = bodySubNode.SelectSingleNode("D506_09").InnerText;
                YbCompensationMoneyBillInfo.CompensationDiagnosisAmount = bodySubNode.SelectSingleNode("D506_09_A").InnerText;
                YbCompensationMoneyBillInfo.DiagnosisRate = bodySubNode.SelectSingleNode("D102_13").InnerText;
                YbCompensationMoneyBillInfo.CompensationAmount = bodySubNode.SelectSingleNode("D506_109").InnerText;
                YbCompensationMoneyBillInfo.CompensationRate = bodySubNode.SelectSingleNode("D102_14").InnerText;
                YbCompensationMoneyBillInfo.ApplyCompensationAmount = bodySubNode.SelectSingleNode("D506_117").InnerText; ;
                YbCompensationMoneyBillInfo.ActuallyCompensationRate = bodySubNode.SelectSingleNode("D102_16").InnerText;

                return YbCompensationMoneyBillInfo;
            }
            return YbCompensationMoneyBillInfo = null;
        }
        #endregion

        #region 退票
        /// <summary>
        /// 退票
        /// </summary>
        /// <returns></returns>
        public override string CancelInChargeResult(int HospId)
        {
            BLL.CBsPatType objPatType = new BLL.CBsPatType();
            Model.BsPatTypeInfo infoPatType = objPatType.GetByID(InfoInHosInfo.PatTypeId);
            if (infoPatType.Name.Contains("大病"))
                CancelDB(HospId);
            System.Xml.XmlDocument document = this.XmlDataExchangeModel("B020008", InfoInHosInfo.YbRegNo);
            XmlNode rootNode = document.DocumentElement;
            XmlNode bodyNode = rootNode["body"];
            //XmlNode bodyNode = rootNode.SelectSingleNode("body");
            Model.YbPatSeqInfo infoYbpatSeq = new Model.YbPatSeqInfo();
            BLL.CYbPatSeq objYbPatSeqInfo = new BLL.CYbPatSeq();
            Model.ModelList<Model.YbPatSeqInfo> lstYbpatSeq = objYbPatSeqInfo.YbPatSeq_SelectByHospId(HospId);

            if (lstYbpatSeq.Count > 0)
                resultYbRegNo = lstYbpatSeq[0].F2;
            bodyNode.AppendChild(this.SetElementValue(document, "D506_01", resultYbRegNo));//住院补偿流水号 
            bodyNode.AppendChild(this.SetElementValue(document, "D504_02", InfoInHosInfo.YbRegNo));//Model.Configuration.UserProfiles.UserCode));//个人编码
            bodyNode.AppendChild(this.SetElementValue(document, "D506_27", Model.Configuration.UserProfiles.UserName));//经办人 
            //if (infoPatType.Name.Contains("大病"))
            //{
            //    bodyNode.AppendChild(this.SetElementValue(document, "D506_96", "2"));//退票类型 String（1）	非必填 默认1仅农合退票 2 仅大病保险退票
            //}
            //else
            bodyNode.AppendChild(this.SetElementValue(document, "D506_96", "1"));//退票类型 String（1）	非必填 默认1仅农合退票 2 仅大病保险退票
            StringBuilder sBuilderXml = new StringBuilder();
            sBuilderXml.Append(document.InnerXml);
            Utilities.Document docXml = new Utilities.Document();
            docXml.Content = sBuilderXml.Replace(" xmlns=\"\"", ""); ;
            docXml.Create("c://退票(输入).xml", false);
            string result = this.FinallySetXmlNs(document);
            document.LoadXml(result);

            StringBuilder sBuilder1 = new StringBuilder();
            sBuilder1.Append(result);
            Utilities.Document doc = new Utilities.Document();
            doc.Content = sBuilder1;
            doc.Create("c://退票(结果).xml", false);
            //MessageBox.Show(sBuilder1.ToString());


            XmlNode responseNode = document.SelectSingleNode("response");
            if (!this.ShowHeadMsg(responseNode))
            {
                return "失败";
            }
            Utilities.Information.ShowMsgBox(RetHeadMsg(responseNode));
            return "成功";
        }
        #endregion

        public bool CancelDB(int HospId)
        {
            System.Xml.XmlDocument document = this.XmlDataExchangeModel("B020008", InfoInHosInfo.YbRegNo);
            XmlNode rootNode = document.DocumentElement;
            XmlNode bodyNode = rootNode["body"];
            //XmlNode bodyNode = rootNode.SelectSingleNode("body");
            Model.YbPatSeqInfo infoYbpatSeq = new Model.YbPatSeqInfo();
            BLL.CYbPatSeq objYbPatSeqInfo = new BLL.CYbPatSeq();
            Model.ModelList<Model.YbPatSeqInfo> lstYbpatSeq = objYbPatSeqInfo.YbPatSeq_SelectByHospId(HospId);

            if (lstYbpatSeq.Count > 0)
                resultYbRegNo = lstYbpatSeq[0].F2;
            bodyNode.AppendChild(this.SetElementValue(document, "D506_01", resultYbRegNo));//住院补偿流水号 
            bodyNode.AppendChild(this.SetElementValue(document, "D504_02", InfoInHosInfo.YbRegNo));//Model.Configuration.UserProfiles.UserCode));//个人编码
            bodyNode.AppendChild(this.SetElementValue(document, "D506_27", Model.Configuration.UserProfiles.UserName));//经办人 
            BLL.CBsPatType objPatType = new BLL.CBsPatType();
            Model.BsPatTypeInfo infoPatType = objPatType.GetByID(InfoInHosInfo.PatTypeId);
            bodyNode.AppendChild(this.SetElementValue(document, "D506_96", "2"));//退票类型 String（1）	非必填 默认1仅农合退票 2 仅大病保险退票

            string result = this.FinallySetXmlNs(document);
            document.LoadXml(result);

            StringBuilder sBuilder1 = new StringBuilder();
            sBuilder1.Append(result);
            Utilities.Document doc = new Utilities.Document();
            doc.Content = sBuilder1;
            doc.Create("c://退票.txt", false);
            //MessageBox.Show(sBuilder1.ToString());


            XmlNode responseNode = document.SelectSingleNode("response");
            if (!this.ShowHeadMsg(responseNode))
            {
                return false;
            }
            Utilities.Information.ShowMsgBox(RetHeadMsg(responseNode));
            return true;
        }

        public bool CancelCY(int HospId)
        {
            System.Xml.XmlDocument document = this.XmlDataExchangeModel("B020014", InfoInHosInfo.YbRegNo);
            XmlNode rootNode = document.DocumentElement;
            XmlNode bodyNode = rootNode["body"];
            //XmlNode bodyNode = rootNode.SelectSingleNode("body");

            bodyNode.AppendChild(this.SetElementValue(document, "D504_02", InfoInHosInfo.YbRegNo));//个人编码
            bodyNode.AppendChild(this.SetElementValue(document, "D504_09", InfoInHosInfo.InPatNo + InfoInHosInfo.NTime.ToString()));
            bodyNode.AppendChild(this.SetElementValue(document, "D504_97", InInvoiceInfo.CancelMemo));
            bodyNode.AppendChild(this.SetElementValue(document, "D504_98", Model.Configuration.UserProfiles.UserName));
            bodyNode.AppendChild(this.SetElementValue(document, "D504_99", BLL.Common.DateTimeHandler.GetServerDateTime().ToString("yyyy-mm-ddT24-mi-ss")));

            string result = this.FinallySetXmlNs(document);
            document.LoadXml(result);

            //StringBuilder sBuilder1 = new StringBuilder();
            //sBuilder1.Append(result);
            //Utilities.Document doc = new Utilities.Document();
            //doc.Content = sBuilder1;
            //doc.Create("c://退出院.txt", false);
            //MessageBox.Show(sBuilder1.ToString());


            XmlNode responseNode = document.SelectSingleNode("response");
            if (!this.ShowHeadMsg(responseNode))
            {
                return false;
            }
            Utilities.Information.ShowMsgBox(RetHeadMsg(responseNode));
            return true;
        }

        #region 打印
        /// <summary>
        /// 打印
        /// </summary>
        /// <returns></returns>
        public override void PrintTotal(bool view)
        {
            try
            {
                BLL.CBsPatType objPatType = new BLL.CBsPatType();
                Model.BsPatTypeInfo infoPatType = objPatType.GetByID(InfoInHosInfo.PatTypeId);
                if (view && infoPatType.Name.Contains("大病"))
                {
                    //GetPrintDBJSD();
                    //if (ReturnF2 == null) return;

                    //Model.uspGetPrintDBJSDQry DBJSDResult = ReturnF2 as Model.uspGetPrintDBJSDQry;
                    //Model.ModelList<Model.uspGetPrintDBJSDQry> lstPrint = new ModelList<uspGetPrintDBJSDQry>();
                    //lstPrint.Add(DBJSDResult);
                    //string rptName = "PrintDBJSD";
                    //CrystalDecisions.CrystalReports.Engine.ReportDocument Printing = Tools.Utils.GetRptFileByName(rptName);
                    //Printing.SetDataSource(lstPrint);
                    //PrintReport.FrmPreview.ShowReport1(Printing, rptName);
                    ////Tools.Utils.PrintToSetPrinter(Printing, rptName, 0, 0);
                    //Printing.Dispose();

                    System.Xml.XmlDocument document = LoadInChargeDB();
                    if (this.ShowHeadMsg(document.DocumentElement))
                    {
                        XmlNode nodeResponse = document.SelectSingleNode("response");
                        XmlNode nodeBody = nodeResponse.SelectSingleNode("body");
                        XmlNode nodeItem = nodeBody.SelectSingleNode("item");
                        string rptName = "DBNhDocumentsDtl";
                        CrystalDecisions.CrystalReports.Engine.ReportDocument Printing = Tools.Utils.GetRptFileByName(rptName);
                        foreach (XmlNode item in nodeItem.ChildNodes)
                        {
                            Tools.Utils.ReportDefineText(Printing, item.Name, item.InnerText);
                            //if ("D506_32_1".Contains(item.Name))
                            //{
                            //    Tools.Utils.ReportDefineText(Printing, "D506_32_1", item.InnerText);
                            //}
                        }
                        Tools.Utils.ReportDefineText(Printing, "OperName", Model.Configuration.UserProfiles.UserName);
                        Tools.Utils.ReportDefineText(Printing, "OperTime", BLL.Common.DateTimeHandler.GetServerDateTime().ToString("yyyy年MM月dd日"));
                        if (PrintReport.FrmPreview.ShowReportResult(Printing, rptName) == DialogResult.Yes)
                        {
                            try
                            {
                                Tools.Utils.PrintToSetPrinter(Printing, rptName, 0, 0);
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }
                else
                {
                    GetHospitalizationcompensationvalue();
                    if (ReturnF2 == null) return;
                    //Model.QzNhInterface.CompensationResults nhResult = ReturnF1 as Model.QzNhInterface.CompensationResults;
                    Model.QzNhInterface.CountHospitalizationCompensation nhCompensation = ReturnF2 as Model.QzNhInterface.CountHospitalizationCompensation;

                    CrystalDecisions.CrystalReports.Engine.ReportDocument printNhDocumentsDtl = Tools.Utils.GetRptFileByName("NhDocumentsDtl1");
                    #region printNhDocumentsDtl
                    //Model.Configuration.Global info = BLL.ConfigFilesManager<Model.Configuration.Global>.GetFromXml(Utilities.ConfigTypes.GLOBAL_CONFIG);


                    //Tools.Utils.ReportDefineText(printNhDocumentsDtl, "lblHospital", info.HospitalName);
                    //Tools.Utils.ReportDefineText(printNhDocumentsDtl, "Name", this.InPatientInfo.PatientName);
                    //Tools.Utils.ReportDefineText(printNhDocumentsDtl, "Sex", this.InPatientInfo.Sex);
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)printNhDocumentsDtl.ReportDefinition.ReportObjects["InPatArea"]).Text = nhCompensation.InPatArea.ToString();
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)printNhDocumentsDtl.ReportDefinition.ReportObjects["AuthNo"]).Text = nhCompensation.InPatRegNo.ToString();
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)printNhDocumentsDtl.ReportDefinition.ReportObjects["InPatNo"]).Text = nhCompensation.InPatNo;
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)printNhDocumentsDtl.ReportDefinition.ReportObjects["Phone"]).Text = nhCompensation.Phone;
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)printNhDocumentsDtl.ReportDefinition.ReportObjects["Name"]).Text = nhCompensation.Name;
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)printNhDocumentsDtl.ReportDefinition.ReportObjects["Sex"]).Text = nhCompensation.Sex;
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)printNhDocumentsDtl.ReportDefinition.ReportObjects["Age"]).Text = nhCompensation.Age;
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)printNhDocumentsDtl.ReportDefinition.ReportObjects["IdCardNo"]).Text = nhCompensation.IdCardNo;
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)printNhDocumentsDtl.ReportDefinition.ReportObjects["MedicareNo"]).Text = nhCompensation.MedicareNo;
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)printNhDocumentsDtl.ReportDefinition.ReportObjects["PeopleProprity"]).Text = nhCompensation.PeopleProprity;
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)printNhDocumentsDtl.ReportDefinition.ReportObjects["Diagnose"]).Text = nhCompensation.Diagnose;
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)printNhDocumentsDtl.ReportDefinition.ReportObjects["SurgicalOperationName"]).Text = nhCompensation.SurgicalOperationName;
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)printNhDocumentsDtl.ReportDefinition.ReportObjects["InTime"]).Text = nhCompensation.InTime;
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)printNhDocumentsDtl.ReportDefinition.ReportObjects["OutTime"]).Text = nhCompensation.OutTime;
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)printNhDocumentsDtl.ReportDefinition.ReportObjects["SkyNumber"]).Text = nhCompensation.SkyNumber;
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)printNhDocumentsDtl.ReportDefinition.ReportObjects["ReferralName"]).Text = nhCompensation.ReferralName;
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)printNhDocumentsDtl.ReportDefinition.ReportObjects["OriginalA1"]).Text = nhCompensation.OriginalA1.ToString();
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)printNhDocumentsDtl.ReportDefinition.ReportObjects["OriginalA2"]).Text = nhCompensation.OriginalA2.ToString();
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)printNhDocumentsDtl.ReportDefinition.ReportObjects["OriginalA3"]).Text = nhCompensation.OriginalA3.ToString();
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)printNhDocumentsDtl.ReportDefinition.ReportObjects["OriginalA4"]).Text = nhCompensation.OriginalA4.ToString();
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)printNhDocumentsDtl.ReportDefinition.ReportObjects["OriginalA5"]).Text = nhCompensation.OriginalA5.ToString();
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)printNhDocumentsDtl.ReportDefinition.ReportObjects["OriginalA6"]).Text = nhCompensation.OriginalA6.ToString();
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)printNhDocumentsDtl.ReportDefinition.ReportObjects["OriginalA7"]).Text = nhCompensation.OriginalA7.ToString();
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)printNhDocumentsDtl.ReportDefinition.ReportObjects["OriginalA8"]).Text = nhCompensation.OriginalA8.ToString();
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)printNhDocumentsDtl.ReportDefinition.ReportObjects["OriginalA9"]).Text = nhCompensation.OriginalA9.ToString();
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)printNhDocumentsDtl.ReportDefinition.ReportObjects["OriginalAmount"]).Text = nhCompensation.OriginalAmount.ToString();

                    ((CrystalDecisions.CrystalReports.Engine.TextObject)printNhDocumentsDtl.ReportDefinition.ReportObjects["CompensationA1"]).Text = nhCompensation.CompensationA1.ToString();
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)printNhDocumentsDtl.ReportDefinition.ReportObjects["CompensationA2"]).Text = nhCompensation.CompensationA2.ToString();
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)printNhDocumentsDtl.ReportDefinition.ReportObjects["CompensationA3"]).Text = nhCompensation.CompensationA3.ToString();
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)printNhDocumentsDtl.ReportDefinition.ReportObjects["CompensationA4"]).Text = nhCompensation.CompensationA4.ToString();
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)printNhDocumentsDtl.ReportDefinition.ReportObjects["CompensationA5"]).Text = nhCompensation.CompensationA5.ToString();
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)printNhDocumentsDtl.ReportDefinition.ReportObjects["CompensationA6"]).Text = nhCompensation.CompensationA6.ToString();
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)printNhDocumentsDtl.ReportDefinition.ReportObjects["CompensationA7"]).Text = nhCompensation.CompensationA7.ToString();
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)printNhDocumentsDtl.ReportDefinition.ReportObjects["CompensationA8"]).Text = nhCompensation.CompensationA8.ToString();
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)printNhDocumentsDtl.ReportDefinition.ReportObjects["CompensationA9"]).Text = nhCompensation.CompensationA9.ToString();
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)printNhDocumentsDtl.ReportDefinition.ReportObjects["CompensationAmount"]).Text = nhCompensation.CompensationAmount.ToString();

                    ((CrystalDecisions.CrystalReports.Engine.TextObject)printNhDocumentsDtl.ReportDefinition.ReportObjects["CompensationChineseMedicineA"]).Text = nhCompensation.CompensationChineseMedicineA.ToString();
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)printNhDocumentsDtl.ReportDefinition.ReportObjects["Rate1"]).Text = nhCompensation.Rate1.ToString();
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)printNhDocumentsDtl.ReportDefinition.ReportObjects["GeneralHospitalStartingLine"]).Text = nhCompensation.GeneralHospitalStartingLine.ToString();
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)printNhDocumentsDtl.ReportDefinition.ReportObjects["GeneralHospitalStartingLineRate"]).Text = nhCompensation.GeneralHospitalStartingLineRate.ToString();
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)printNhDocumentsDtl.ReportDefinition.ReportObjects["CompensationNationalA"]).Text = nhCompensation.CompensationNationalA.ToString();
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)printNhDocumentsDtl.ReportDefinition.ReportObjects["Rate2"]).Text = nhCompensation.Rate2.ToString();
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)printNhDocumentsDtl.ReportDefinition.ReportObjects["MajorDiseasesStartingLine"]).Text = nhCompensation.MajorDiseasesStartingLine.ToString();
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)printNhDocumentsDtl.ReportDefinition.ReportObjects["MajorDiseasesStartingLineRate"]).Text = nhCompensation.MajorDiseasesStartingLineRate.ToString();
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)printNhDocumentsDtl.ReportDefinition.ReportObjects["MedicalAmount"]).Text = nhCompensation.MedicalAmount.ToString();
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)printNhDocumentsDtl.ReportDefinition.ReportObjects["ThisCompensationAmount"]).Text = nhCompensation.ThisCompensationAmount.ToString();
                    ((CrystalDecisions.CrystalReports.Engine.TextObject)printNhDocumentsDtl.ReportDefinition.ReportObjects["SelfAmount"]).Text = nhCompensation.SelfAmount.ToString();
                    if (InfoBsPatient != null && InfoBsPatient.BirthDate != DateTime.MinValue)
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)printNhDocumentsDtl.ReportDefinition.ReportObjects["Age"]).Text = (BLL.Common.DateTimeHandler.GetServerDateTime().Year - InfoBsPatient.BirthDate.Year) + "";

                    printNhDocumentsDtl.PrintOptions.PrinterName = BLL.Common.Utils.GetPrinterName("NhDocumentsDtl1");
                    printNhDocumentsDtl.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.DefaultPaperSize;
                    #endregion

                    PrintReport.FrmPreview.ShowReport1(printNhDocumentsDtl, "NhDocumentsDtl1");
                }
            }
            catch (Exception ex)
            {
                throw ex;
                //new Tools.FormErrorMessage(ex.Message, ex.InnerException == null ? ex.Message : string.Format("{0}\r\n{1}", ex.InnerException.Message, ex.Message));
            }

        }
        #endregion

        #region 申请结算月份查询
        /// <summary>
        /// 申请结算月份查询
        /// </summary>
        /// <returns></returns>
        public string ApplyChargeMonthSerach()
        {
            //申请结算月份查询
            System.Xml.XmlDocument document = this.XmlDataExchangeModel("C020010", "");
            string result = this.FinallySetXmlNs(document);
            document.LoadXml(result);
            XmlNode responseNode = document.SelectSingleNode("response");
            XmlNode bodySubNode = responseNode.SelectSingleNode("body");
            string MaxMonth = bodySubNode.SelectSingleNode("D910_03").InnerText.Trim();
            return MaxMonth;

        }
        #endregion

        #region 申请月结算
        /// <summary>
        /// 申请月结算
        /// </summary>
        /// <returns></returns>
        public void ApplyChargeMonth(String Month)
        {
            if (Convert.ToInt32(Month) < 10)
            {
                Month = "0" + Month;
            }

            //申请月结算B020009
            System.Xml.XmlDocument document = this.XmlDataExchangeModel("B020009", "");
            XmlNode rootNode = document.DocumentElement;
            XmlNode bodyNode = rootNode["body"];
            bodyNode.AppendChild(this.SetElementValue(document, "D910_03", Month));//结算月份
            string result = this.FinallySetXmlNs(document);
            document.LoadXml(result);
            XmlNode responseNode = document.SelectSingleNode("response");
            XmlNode bodySubNode = responseNode.SelectSingleNode("body");
            ApplyChargeSearch(Month, false);

        }
        #endregion

        #region 申请结算查询C020011
        /// <summary>
        /// 申请结算查询C020011
        /// </summary>
        /// <returns></returns>
        public void ApplyChargeSearch(String Month, bool isCancel)
        {
            if (Convert.ToInt32(Month) < 10 && Month.Length == 1)
            {
                Month = "0" + Month;
            }
            System.Xml.XmlDocument document = this.XmlDataExchangeModel("C020011", "");
            XmlNode rootNode = document.DocumentElement;
            XmlNode bodyNode = rootNode["body"];
            bodyNode.AppendChild(this.SetElementValue(document, "D910_03", Month));//住院补偿流水号 
            string result = this.FinallySetXmlNs(document);
            document.LoadXml(result);
            XmlNode responseNode = document.SelectSingleNode("response");
            XmlNode bodySubNode = responseNode.SelectSingleNode("body");
            if (bodySubNode == null && !isCancel)
            {
                MessageBox.Show(string.Format("[{0}]月份申报结算失败！请重新结算本月份！", Month));
                return;
            }
            else if (bodySubNode != null && !isCancel)
            {
                MessageBox.Show(string.Format("[{0}]月份申报结算成功。", Month));
            }
            else if (bodySubNode == null && isCancel)
            {
                MessageBox.Show(string.Format("[{0}]月份没有结算，您不能对该月份退结算！", Month));
                return;
            }
            else if (bodySubNode != null && isCancel)
            {
                if (MessageBox.Show(string.Format("[{0}]月份已结算，您是否对该月份退结算？", Month), "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel) return;
            }
        }
        #endregion

        #region 退申请月结算B020010
        /// <summary>
        /// 退申请月结算B020010
        /// </summary>
        /// <returns></returns>
        public void CancelChargeMonth(String Month)
        {
            if (Convert.ToInt32(Month) < 10)
            {
                Month = "0" + Month;
            }

            //申请月结算B020009
            System.Xml.XmlDocument document = this.XmlDataExchangeModel("B020010", "");
            XmlNode rootNode = document.DocumentElement;
            XmlNode bodyNode = rootNode["body"];
            bodyNode.AppendChild(this.SetElementValue(document, "D910_03", Month));//住院补偿流水号 
            string result = this.FinallySetXmlNs(document);
            document.LoadXml(result);
            MessageBox.Show(string.Format("{0}月退申请结算成功。", Month));

        }
        #endregion

        private Model.ModelList<Model.uspInBalanceDtlQry> GetSelfDtl(Model.ListView<Model.uspInBalanceDtlQry> lstFee)
        {
            Model.ModelList<Model.uspInBalanceDtlQry> lstUspOuInvoiceDtl = new Model.ModelList<Model.uspInBalanceDtlQry>();
            foreach (Model.uspInBalanceDtlQry info in lstFee)
            {
                //if (info.F3 != "自费" && info.ID != 0)

                lstUspOuInvoiceDtl.Add(info);
                //else
                //{
                //    info.AmountTally = 0;
                //    info.AmountPay = info.Amount;
                //    info.AmountFact  = info.Amount;
                //    info.AmountSelf  = info.Amount; 
                //}
            }
            return lstUspOuInvoiceDtl;
        }

        private void DivideDisc(double amountYb)
        {
            Model.ListView<Model.uspInBalanceDtlQry> lstFee = this.LstUspInBalanceDtl.DefaultView;
            lstFee.Filter = "ID <> 0";
            Model.ModelList<Model.uspInBalanceDtlQry> lstUspOuInvoiceDtl = GetSelfDtl(lstFee);
            double amountSum = BLL.Common.Utils.Round(lstUspOuInvoiceDtl.GetSum("Amount"), 2);
            double amountYbInit = amountYb;
            lstUspOuInvoiceDtl.Sort("Amount");
            for (int i = 0; i < lstUspOuInvoiceDtl.Count; i++)
            {
                Model.uspInBalanceDtlQry infoInInvoiceDtl = lstUspOuInvoiceDtl[i];
                double curAmountYb = BLL.Common.Utils.Round(infoInInvoiceDtl.Amount / amountSum * amountYbInit, 2);
                if (amountYb > curAmountYb && i < lstUspOuInvoiceDtl.Count)
                    infoInInvoiceDtl.AmountFact = infoInInvoiceDtl.Amount - curAmountYb;
                else
                    infoInInvoiceDtl.AmountFact = infoInInvoiceDtl.Amount - amountYb;
                amountYb -= curAmountYb;
                //if (amountYb != 0 && i == lstUspOuInvoiceDtl.Count - 1)
                //{
                //    infoInInvoiceDtl.AmountFact = infoInInvoiceDtl.AmountFact -(amountYbInit-( lstUspOuInvoiceDtl.GetSum("Amount") - lstUspOuInvoiceDtl.GetSum("AmountFact")) );
                //    int t = 0;
                //}
                if (i == lstUspOuInvoiceDtl.Count - 1 && this.LstUspInBalanceDtl.GetSum("Amount") - this.LstUspInBalanceDtl.GetSum("AmountFact") != amountYbInit)
                {
                    double sp = amountYbInit - (this.LstUspInBalanceDtl.GetSum("Amount") - this.LstUspInBalanceDtl.GetSum("AmountFact"));
                    infoInInvoiceDtl.AmountFact -= sp;
                }
                infoInInvoiceDtl.AmountPay = infoInInvoiceDtl.AmountSelf = infoInInvoiceDtl.AmountFact;
                infoInInvoiceDtl.AmountTally = infoInInvoiceDtl.Amount - infoInInvoiceDtl.AmountPay;
                infoInInvoiceDtl.DiscIn = infoInInvoiceDtl.AmountFact / infoInInvoiceDtl.Amount;

            }
        }

        public override bool CheckInFeeUpLoaded()
        {
            throw new NotImplementedException();
        }

        public override object UpLoadOuFee()
        {
            throw new NotImplementedException();
        }

        public override bool CheckOuFeeUpLoaded()
        {
            throw new NotImplementedException();
        }

        public override void PrintClass(bool value)
        {
            throw new NotImplementedException();
        }
        #region 大病结算单

        public object GetPrintDBJSD()
        {
            BLL.Common.CalBirthday objCalBirthday = new BLL.Common.CalBirthday(InfoInHosInfo.BirthDate);
            System.Xml.XmlDocument document = LoadInChargeDB();
            //System.Xml.XmlDocument document = SearchInCharge();
            XmlNode responseNode = document.SelectSingleNode("response");
            if (responseNode == null)
            {
                return "失败";
            }
            XmlNode bodySubNode = responseNode.SelectSingleNode("body");
            if (bodySubNode == null)
            {
                MessageBox.Show("没有此病人记录");
                return document;
            }
            XmlNode itemNode = bodySubNode.SelectSingleNode("item");
            #region 获取住院结算结果
            Model.uspGetPrintDBJSDQry DBJSDResult = new uspGetPrintDBJSDQry();

            //YBInterface.QzNhInterface.DBJSDResult DBJSDResult = new YBInterface.QzNhInterface.DBJSDResult();
            if (InfoInHosInfo.YbRegNo.Substring(0, 6) == "450703")
            {
                DBJSDResult.InPatArea = "钦北区";
            }
            else if (InfoInHosInfo.YbRegNo.Substring(0, 6) == "450702")
            {
                DBJSDResult.InPatArea = "钦南区";
            }
            else if (InfoInHosInfo.YbRegNo.Substring(0, 6) == "450722")
            {
                DBJSDResult.InPatArea = "浦北县";
            }
            else if (InfoInHosInfo.YbRegNo.Substring(0, 6) == "450721")
            {
                DBJSDResult.InPatArea = "灵山县";
            }
            else if (InfoInHosInfo.YbRegNo.Substring(0, 6) == "450706")
            {
                DBJSDResult.InPatArea = "钦州港";
            }
            DBJSDResult.StrD506125 = itemNode.SelectSingleNode("D506_125").InnerText;
            DBJSDResult.StrD50601 = itemNode.SelectSingleNode("D506_01").InnerText;
            DBJSDResult.StrD50403 = itemNode.SelectSingleNode("D504_03").InnerText;
            DBJSDResult.StrD50405 = itemNode.SelectSingleNode("D504_05").InnerText;
            //DBJSDResult.strD504_04 = itemNode.SelectSingleNode("D504_04").InnerText;
            DBJSDResult.StrD50475 = itemNode.SelectSingleNode("D504_75").InnerText;
            DBJSDResult.StrD50402 = itemNode.SelectSingleNode("D504_02").InnerText;
            DBJSDResult.StrD50187 = itemNode.SelectSingleNode("D501_87").InnerText;
            DBJSDResult.StrD50515 = itemNode.SelectSingleNode("D505_15").InnerText;
            DBJSDResult.StrD50410 = itemNode.SelectSingleNode("D504_10").InnerText;
            DBJSDResult.StrD50616 = itemNode.SelectSingleNode("D506_16").InnerText;
            DBJSDResult.StrD504101 = itemNode.SelectSingleNode("D504_101").InnerText;
            DBJSDResult.StrD10102 = itemNode.SelectSingleNode("D101_02").InnerText;
            DBJSDResult.StrD50415 = itemNode.SelectSingleNode("D504_15").InnerText;
            DBJSDResult.StrD50450 = itemNode.SelectSingleNode("D504_50").InnerText;
            DBJSDResult.StrD50411 = itemNode.SelectSingleNode("D504_11").InnerText;
            DBJSDResult.StrD50412 = itemNode.SelectSingleNode("D504_12").InnerText;
            DBJSDResult.StrD50413 = itemNode.SelectSingleNode("D504_13").InnerText;
            DBJSDResult.StrD506129 = itemNode.SelectSingleNode("D506_129").InnerText;
            DBJSDResult.StrD50626 = itemNode.SelectSingleNode("D506_26").InnerText;
            DBJSDResult.StrD50652 = itemNode.SelectSingleNode("D506_52").InnerText;
            DBJSDResult.StrD50653 = itemNode.SelectSingleNode("D506_53").InnerText;
            DBJSDResult.StrD50627 = itemNode.SelectSingleNode("D506_27").InnerText;
            DBJSDResult.StrD506127 = itemNode.SelectSingleNode("D506_127").InnerText;
            DBJSDResult.StrD50320 = itemNode.SelectSingleNode("D503_20").InnerText;
            DBJSDResult.StrD50603 = itemNode.SelectSingleNode("D506_03").InnerText;
            DBJSDResult.StrD50619 = itemNode.SelectSingleNode("D506_19").InnerText;
            DBJSDResult.StrD50624 = itemNode.SelectSingleNode("D506_24").InnerText;
            DBJSDResult.StrD506130 = itemNode.SelectSingleNode("D506_130").InnerText;
            DBJSDResult.StrD506131 = itemNode.SelectSingleNode("D506_131").InnerText;
            DBJSDResult.StrD506128 = itemNode.SelectSingleNode("D506_128").InnerText;
            DBJSDResult.StrD506132 = itemNode.SelectSingleNode("D506_132").InnerText;
            DBJSDResult.StrD50632 = itemNode.SelectSingleNode("D506_32").InnerText;
            DBJSDResult.StrD506133 = itemNode.SelectSingleNode("D506_133").InnerText;
            DBJSDResult.StrD50677 = itemNode.SelectSingleNode("D506_77").InnerText;
            DBJSDResult.StrD506134 = itemNode.SelectSingleNode("D506_134").InnerText;
            DBJSDResult.StrD506135 = itemNode.SelectSingleNode("D506_135").InnerText;
            DBJSDResult.StrD506136 = itemNode.SelectSingleNode("D506_136").InnerText;
            DBJSDResult.StrD506137 = itemNode.SelectSingleNode("D506_137").InnerText;
            DBJSDResult.StrD506116 = itemNode.SelectSingleNode("D506_116").InnerText;
            DBJSDResult.StrD506142 = itemNode.SelectSingleNode("D506_142").InnerText;
            DBJSDResult.StrD506143 = itemNode.SelectSingleNode("D506_143").InnerText;
            DBJSDResult.StrD506144 = itemNode.SelectSingleNode("D506_144").InnerText;
            DBJSDResult.StrD506145 = itemNode.SelectSingleNode("D506_145").InnerText;
            DBJSDResult.StrD50463 = itemNode.SelectSingleNode("D504_63").InnerText;
            DBJSDResult.StrD50469 = itemNode.SelectSingleNode("D504_69").InnerText;
            DBJSDResult.StrD50429 = itemNode.SelectSingleNode("D504_29").InnerText;
            DBJSDResult.StrD50409 = itemNode.SelectSingleNode("D504_09").InnerText;
            DBJSDResult.StrD506146 = itemNode.SelectSingleNode("D506_146").InnerText;
            DBJSDResult.StrD506147 = itemNode.SelectSingleNode("D506_147").InnerText;
            DBJSDResult.StrD506148 = itemNode.SelectSingleNode("D506_148").InnerText;

            string Sex;
            if (itemNode.SelectSingleNode("D504_04").InnerText == "1")
            {
                Sex = "男";
            }
            else if (itemNode.SelectSingleNode("D504_04").InnerText == "2")
            {
                Sex = "女";
            }
            else
            {
                Sex = "其他";
            }
            DBJSDResult.StrD50404 = Sex;
            ReturnF2 = DBJSDResult;
            return ReturnF2;
            #endregion
        }

        #endregion

        #region 获取新型农村合作医疗参合人员住院补偿审核表内容

        public override object GetHospitalizationcompensationvalue()
        {
            BLL.Common.CalBirthday objCalBirthday = new BLL.Common.CalBirthday(InfoInHosInfo.BirthDate);
            System.Xml.XmlDocument document = LoadInCharge();
            //System.Xml.XmlDocument document = SearchInCharge();
            XmlNode responseNode = document.SelectSingleNode("response");
            if (responseNode == null)
            {
                return "失败";
            }
            XmlNode bodySubNode = responseNode.SelectSingleNode("body");
            if (bodySubNode == null)
            {
                MessageBox.Show("没有此病人记录");
                return document;
            }

            XmlNode baseInfoNode = bodySubNode.SelectSingleNode("baseInfo");
            XmlNode feeInfoNode = bodySubNode.SelectSingleNode("feeInfo");
            XmlNode otherInfoNode = bodySubNode.SelectSingleNode("otherInfo");

            XmlNode allFeeSubentryNode = feeInfoNode.SelectSingleNode("allFeeSubentry");
            XmlNode computeTypeFeeNode = feeInfoNode.SelectSingleNode("computeTypeFee");

            #region 获取住院结算结果
            Model.QzNhInterface.CountHospitalizationCompensation nhCompensation = new Model.QzNhInterface.CountHospitalizationCompensation();

            //钦北区	450703
            //钦南区	450702
            //浦北县	450722
            //灵山县	450721
            //钦州港	450706
            if (InfoInHosInfo.YbRegNo.Substring(0, 6) == "450703")
            {
                nhCompensation.InPatArea = "钦北区";
            }
            else if (InfoInHosInfo.YbRegNo.Substring(0, 6) == "450702")
            {
                nhCompensation.InPatArea = "钦南区";
            }
            else if (InfoInHosInfo.YbRegNo.Substring(0, 6) == "450722")
            {
                nhCompensation.InPatArea = "浦北县";
            }
            else if (InfoInHosInfo.YbRegNo.Substring(0, 6) == "450721")
            {
                nhCompensation.InPatArea = "灵山县";
            }
            else if (InfoInHosInfo.YbRegNo.Substring(0, 6) == "450706")
            {
                nhCompensation.InPatArea = "钦州港";
            }
            nhCompensation.InPatRegNo = baseInfoNode.SelectSingleNode("D506_01").InnerText;
            nhCompensation.InPatNo = baseInfoNode.SelectSingleNode("D504_09").InnerText;
            nhCompensation.Phone = InfoInHosInfo.Mobile;// baseInfoNode.SelectSingleNode("D504_75").InnerText;
            nhCompensation.Name = baseInfoNode.SelectSingleNode("D504_03").InnerText;
            string Sex;
            if (baseInfoNode.SelectSingleNode("D504_04").InnerText == "1")
            {
                Sex = "男";
            }
            else if (baseInfoNode.SelectSingleNode("D504_04").InnerText == "2")
            {
                Sex = "女";
            }
            else
            {
                Sex = "其他";
            }
            nhCompensation.Sex = Sex;
            nhCompensation.Age = objCalBirthday.Age.ToString();
            //(Convert.ToInt32(BLL.Common.DateTimeHandler.GetServerDateTime().ToString("yyyy"))- Convert.ToInt32(InfoInHosInfo.BirthDate.ToString("yyyy"))).ToString();
            //InfoInHosInfo.AgeDay.ToString(); //diagnoseInfoNode.SelectSingleNode("D504_09").InnerText;
            nhCompensation.IdCardNo = baseInfoNode.SelectSingleNode("D504_05").InnerText;
            nhCompensation.MedicareNo = InfoInHosInfo.YbRegNo;
            nhCompensation.PeopleProprity = baseInfoNode.SelectSingleNode("D505_15").InnerText;
            nhCompensation.Diagnose = baseInfoNode.SelectSingleNode("D506_54").InnerText;
            nhCompensation.SurgicalOperationName = baseInfoNode.SelectSingleNode("D505_17").InnerText;
            nhCompensation.InTime = baseInfoNode.SelectSingleNode("D504_11").InnerText;
            nhCompensation.OutTime = baseInfoNode.SelectSingleNode("D504_12").InnerText;
            System.TimeSpan diff = InfoInHosInfo.OutTime.Subtract(InfoInHosInfo.InTime);
            nhCompensation.SkyNumber = diff.Days.ToString();
            nhCompensation.ReferralName = otherInfoNode.SelectSingleNode("D504_47").InnerText;//转诊申请单号
            nhCompensation.OriginalA1 = allFeeSubentryNode.SelectSingleNode("D506_04").InnerText;
            nhCompensation.OriginalA2 = allFeeSubentryNode.SelectSingleNode("D506_05").InnerText;
            nhCompensation.OriginalA3 = allFeeSubentryNode.SelectSingleNode("D506_06").InnerText;
            nhCompensation.OriginalA4 = allFeeSubentryNode.SelectSingleNode("D506_07").InnerText;
            nhCompensation.OriginalA5 = allFeeSubentryNode.SelectSingleNode("D506_08").InnerText;
            nhCompensation.OriginalA6 = allFeeSubentryNode.SelectSingleNode("D506_09").InnerText;
            nhCompensation.OriginalA7 = allFeeSubentryNode.SelectSingleNode("D506_10").InnerText;
            nhCompensation.OriginalA8 = allFeeSubentryNode.SelectSingleNode("D506_11").InnerText;
            nhCompensation.OriginalA9 = allFeeSubentryNode.SelectSingleNode("D506_12").InnerText;
            nhCompensation.OriginalAmount = allFeeSubentryNode.SelectSingleNode("D506_03").InnerText;
            //(Convert.ToDouble(nhCompensation.OriginalA1) + Convert.ToDouble(nhCompensation.OriginalA2) + Convert.ToDouble(nhCompensation.OriginalA3)
            //+ Convert.ToDouble(nhCompensation.OriginalA4) + Convert.ToDouble(nhCompensation.OriginalA5) + Convert.ToDouble(nhCompensation.OriginalA6) +
            //Convert.ToDouble(nhCompensation.OriginalA7) + Convert.ToDouble(nhCompensation.OriginalA8) + Convert.ToDouble(nhCompensation.OriginalA9)).ToString();

            nhCompensation.CompensationA1 = computeTypeFeeNode.SelectSingleNode("D506_04_A").InnerText;
            nhCompensation.CompensationA2 = computeTypeFeeNode.SelectSingleNode("D506_05_A").InnerText;
            nhCompensation.CompensationA3 = computeTypeFeeNode.SelectSingleNode("D506_06_A").InnerText;
            nhCompensation.CompensationA4 = computeTypeFeeNode.SelectSingleNode("D506_07_A").InnerText;
            nhCompensation.CompensationA5 = computeTypeFeeNode.SelectSingleNode("D506_08_A").InnerText;
            nhCompensation.CompensationA6 = computeTypeFeeNode.SelectSingleNode("D506_09_A").InnerText;
            nhCompensation.CompensationA7 = computeTypeFeeNode.SelectSingleNode("D506_10_A").InnerText;
            nhCompensation.CompensationA8 = computeTypeFeeNode.SelectSingleNode("D506_11_A").InnerText;
            nhCompensation.CompensationA9 = computeTypeFeeNode.SelectSingleNode("D506_12_A").InnerText;
            nhCompensation.CompensationAmount = computeTypeFeeNode.SelectSingleNode("D506_76").InnerText;
            //(Convert.ToDouble(nhCompensation.CompensationA1) + Convert.ToDouble(nhCompensation.CompensationA2) + Convert.ToDouble(nhCompensation.CompensationA3) +
            //Convert.ToDouble(nhCompensation.CompensationA4) + Convert.ToDouble(nhCompensation.CompensationA5) + Convert.ToDouble(nhCompensation.CompensationA6) +
            //Convert.ToDouble(nhCompensation.CompensationA7) + Convert.ToDouble(nhCompensation.CompensationA8) + Convert.ToDouble(nhCompensation.CompensationA9)).ToString();

            nhCompensation.CompensationChineseMedicineA = "";
            nhCompensation.Rate1 = "";
            nhCompensation.GeneralHospitalStartingLine = otherInfoNode.SelectSingleNode("D506_92").InnerText;
            nhCompensation.GeneralHospitalStartingLineRate = otherInfoNode.SelectSingleNode("D506_93").InnerText;
            nhCompensation.CompensationNationalA = "";
            nhCompensation.Rate2 = "";
            nhCompensation.MajorDiseasesStartingLine = "";// otherInfoNode.SelectSingleNode("D506_92").InnerText;
            nhCompensation.MajorDiseasesStartingLineRate = "";// otherInfoNode.SelectSingleNode("D506_93").InnerText;
            nhCompensation.MedicalAmount = allFeeSubentryNode.SelectSingleNode("D506_03").InnerText;
            nhCompensation.ThisCompensationAmount = computeTypeFeeNode.SelectSingleNode("D506_24").InnerText;
            nhCompensation.SelfAmount = (Convert.ToDouble(nhCompensation.MedicalAmount) - Convert.ToDouble(nhCompensation.ThisCompensationAmount)).ToString();
            this.ReturnF2 = nhCompensation;
            return this.ReturnF2 = nhCompensation;
            #endregion

        }
        #endregion

        public override Model.MZYbInterface.GetUpLoadOuFeeInfo UpLoadOuFeeJb(string budget)
        {
            
            throw new NotImplementedException();
        }

        public override string CancelUpLoadOuFee()
        {
            throw new NotImplementedException();
        }

        public override object GetOuChargeResult()
        {
            throw new NotImplementedException();
        }

        public override string CancelUpLoadOuFeeJb(Model.YbPatSeqInfo info)
        {
            throw new NotImplementedException();
        }

        public override string UpLoadSaver()
        {
            throw new NotImplementedException();
        }

        public override string CancelInHosInfoCheckIn(string reason)
        {
            throw new NotImplementedException();
        }

        public override void PrintTotalBD(bool value)
        {
            throw new NotImplementedException();
        }

        public override string CancelYbUpDtl()
        {
            throw new NotImplementedException();
        }

        public override string GetBalanceInvoiceYbJ()
        {
            //throw new NotImplementedException();
            return string.Empty;
        }
        public override string PrintYB()
        {
            throw new NotImplementedException();
        }

        public override string InitBegin(int isJm)
        {
            throw new NotImplementedException();
        }

        public override string YbouIn()
        {
            throw new NotImplementedException();
        }
        public override string FeeDtlSearch(string RegNoYb)
        {
            throw new NotImplementedException();
        }

        public override string InitEnd(int isJm)
        {
            throw new NotImplementedException();
        }

        public override string CancelInChargeDtl(int LsOut, string OutRegNo)
        {
            throw new NotImplementedException();
        }

        public override string InHosInfoCheckInOut()
        {
            throw new NotImplementedException();
        }

        public override object CancelRegister()
        {
            throw new NotImplementedException();
        }
        public override string YbItemLoad(string loadType)
        {
            throw new NotImplementedException();
        }
        public override string DLoadItem(Model.ListView<Model.BsItemInfo> lstItem)
        {
            throw new NotImplementedException();
        }
        public override string DLoadItemCancel(Model.ListView<Model.BsItemInfo> lstItem)
        {
            throw new NotImplementedException();
        }
        public override object GetInChargeResultBegin()
        {
            throw new NotImplementedException();
        }

        public override string GetInInvoiceDtl()
        {
            throw new NotImplementedException();
        }
        public override void GetInvoiceDtlYbJ(int isOut)
        {
            throw new NotImplementedException();
        }

        public override string CancelUpLoadInFee()
        {
            throw new NotImplementedException();
        }
        public override bool IsYBCheck()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 其它字典获取A040001
    /// </summary>
    public class NhDictionary : absModel
    {
        public override int ID
        {
            get;
            set;
        }
        public override void Fill(System.Data.IDataReader datareader)
        { }
        /// <summary>
        /// 字典编号
        /// </summary>
        public string D911_01 { set; get; }
        /// <summary>
        /// 代码
        /// </summary>
        public string D911_02 { set; get; }
        /// <summary>
        /// 名称
        /// </summary>
        public string D911_03 { set; get; }
        /// <summary>
        /// 拼音
        /// </summary>
        public string D911_04 { set; get; }
        /// <summary>
        /// 备注
        /// </summary>
        public string D911_05 { set; get; }
        /// <summary>
        /// 年份
        /// </summary>
        public string D911_06 { set; get; }
        /// <summary>
        /// 区分目录和项目的标志
        /// </summary>
        public string D911_07 { set; get; }
        /// <summary>
        /// 区划代码
        /// </summary> 
        public string D911_08 { set; get; }
        /// <summary>
        /// 机构等级
        /// </summary>
        public string D911_09 { set; get; }
        /// <summary>
        /// 申报定点类型
        /// </summary>
        public string D911_10 { set; get; }
        /// <summary>
        /// 审批定点类型
        /// </summary> 
        public string D911_11 { set; get; }
        /// <summary>
        /// 机构类别
        /// </summary> 
        public string D911_12 { set; get; }
        /// <summary>
        /// 是否定点
        /// </summary> 
        public string D911_13 { set; get; }
        /// <summary>
        /// 是否系统默认医院
        /// </summary> 
        public string D911_14 { set; get; }
        /// <summary>
        /// 机构状态
        /// </summary> 
        public string D911_15 { set; get; }
        /// <summary>
        /// 机构ID 
        /// </summary>
        public string D911_16 { set; get; }

    }
}
