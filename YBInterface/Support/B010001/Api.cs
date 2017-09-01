using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Linq;
using YBInterface;
using Model;
using Model.MZYbInterface;

namespace YBInterface1.Support.B010001
{
    public class Api : ApiBase<Request<requestBody>, requestBody, Response<responseBody>, responseBody>
    {
        private const string FUNCTION_NO = "B010001";
        private const string FUNCTION_NAME = "门诊预结算"; 

        BLL.CBsUnit _objBsUnit = new BLL.CBsUnit();
        BLL.CBsItemYb _objBsItemYb = new BLL.CBsItemYb();
        BLL.CBsItemDrug _objBsItemDrug = new BLL.CBsItemDrug();
        BLL.CBsDrugForm _objBsDrugForm = new BLL.CBsDrugForm();
        BLL.CBsItemYbRpType _objBsItemYbRpType = new BLL.CBsItemYbRpType();

        Model.ModelList<Model.BsUnitInfo> _lstBsUnit = new ModelList<BsUnitInfo>();
        Model.ModelList<Model.BsItemYbInfo> _lstBsItemYb = new ModelList<BsItemYbInfo>();
        Model.ModelList<Model.BsDrugFormInfo> _lstBsDrugForm = new ModelList<BsDrugFormInfo>();

        public Api(QZNhCommon qzNh)
        {
            _lstBsUnit = _objBsUnit.GetAll();
            _lstBsDrugForm = _objBsDrugForm.GetAll();
            this.QzNh = qzNh;
            //this.YbRegNo = this.QzNh.InfoBsPatient.ybRegNo;
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
            DateTime invoiceDate = this.QzNh.InfoOuInvoice.InvoTime;

            requestBody requestBody = new requestBody()
            {
                D501_02 = this.YbRegNo, // 个人编码
                D501_09 = this.QzNh.GetNhZd("就诊类别", "25"), //就诊类别
                D501_11 = this.QzNh.GetNhZd("就医机构", "23"), // 就诊机构代码
                D501_16 = this.QzNh.GetNhZd("疾病", "11"), // 疾病代码
                D501_38 = string.Empty, // 症状名称
                D501_14 = string.Empty, // 经治医生
                D501_13 = string.Empty, // 接诊科室
                D501_15 = string.Empty, // 来院状态
                D501_39 = this.GetOuClincDia(this.QzNh.InfoOuHosInfo.ID), // 门诊诊断, 必填
                D503_31 = invoiceDate.ToString("yyyy-MM-ddTHH-mm-ss"), // 发票时间, 必填(yyyy-mm-ddT24-mi-ss 强制要求位数，需补齐)
                D503_32 = this.QzNh.InfoOuInvoice.InvoNo, // 发票号
                D503_18 = string.Empty, // 经办人
                item = new requestBodyItem()
                {
                    D503_67 = string.Empty, // 类型代码
                    D503_68 = string.Empty, // 类型值
                },
                details = this.BuildRequestBodyDetails()
            };

            return requestBody;
        }

        private string GetOuClincDia(int mzRegId) 
        {
            string result = string.Empty;

            BLL.COuClincDiag objOuClincDiag = new BLL.COuClincDiag();
            ModelList<OuClincDiagInfo> lstClincDiagInfo = objOuClincDiag.OuClincDiag_SelectByMzRegId(mzRegId);
            var list = (from d in lstClincDiagInfo select (GetString(d.IllDesc) + GetString(d.F1))).ToArray();
            
            return string.Join(@"\n", list);
        }

        string GetString(string s) 
        {

            return string.IsNullOrEmpty(s) ? string.Empty : s;
        }

        /// <summary>
        /// 按项目类型填充数据
        /// </summary>
        /// <returns></returns>
        private requestBodyDetails BuildRequestBodyDetails()
        {
            requestBodyDetails requestBodyDetails = new requestBodyDetails();

            if (this.QzNh.LstUspOuInvoiceDtl.Count == 0)
                return requestBodyDetails;

            foreach (var detail in this.QzNh.LstUspOuInvoiceDtl) 
            {                
                _lstBsItemYb = _objBsItemYb.BsItemYb_SelectByItemIdAndTallyGroupId(detail.ItemId, this.QzNh.InfoTallyGroup.ID);
                if (_lstBsItemYb.Count == 0)
                {
                    throw new Exception(string.Format("项目【{0}】【{1}】没有对应医保的病人大类，上传失败，请在项目代码中维护！", detail.Name, detail.Code));                    
                }                    
            }                     

            int i = 0;
            requestBodyDetails.D502_03_01 = new requestBodyDetailsD502_03_01();
            
            //西药
            // var details = this.QzNh.LstUspOuInvoiceDtl.Find2("LsRpType", ((int)EnumRpType.WestMedicine).ToString());
            var details = this.QzNh.LstUspOuInvoiceDtl.Where(x => x.LsRpType == (int)EnumRpType.WestMedicine);
            int rows = details.Count();
            if (rows > 0) 
            {
                requestBodyDetails.D502_03_01.D502_31_01 = new requestBodyDetailsD502_03_01Item[rows];
                i = 0;
                foreach (var detail in details)
                {
                    string drugType = string.Empty;

                    Model.ModelList<Model.BsItemDrugInfo> drugs = _objBsItemDrug.BsItemDrug_SelectByItemId(detail.ItemId);
                    if (drugs.Count > 0)
                    {
                        BsDrugFormInfo drugFormInfo = _lstBsDrugForm.FindByID(drugs[0].FormId);
                        if (drugFormInfo == null)
                            throw new Exception(string.Format("Can not find drugFormInfo by item id {0}", detail.ItemId));
                        drugType = drugFormInfo.Name;
                    }
                    else
                    {
                        drugType = "无";
                    }
                    
                    _lstBsItemYb = _objBsItemYb.BsItemYb_SelectByItemIdAndTallyGroupId(detail.ItemId, this.QzNh.InfoTallyGroup.ID);
                    requestBodyDetails.D502_03_01.D502_31_01[i] = new requestBodyDetailsD502_03_01Item()
                    {
                        D502_02 = this.QzNh.RegNo,
                        D502_04 = detail.Code,
                        D502_05 = detail.Name,
                        D502_06 = detail.Spec,
                        D502_07 = drugType, //剂型
                        D502_08 = Convert.ToDouble(detail.Price).ToString("#0.0000"),
                        D502_09 = detail.Totality.ToString(),
                        D502_11 = Convert.ToDouble(detail.Amount).ToString("#0.0000"),
                        D502_12 = detail.ID.ToString(),
                        D502_39 = _lstBsItemYb[0].YbCode,
                        D502_40 = _lstBsItemYb[0].YbDesc,
                        D502_43 = _lstBsUnit.FindByID(detail.UnitId).Name
                    };
                    i++;
                }

            }

            // 中成药项目
            // details = this.QzNh.LstUspOuInvoiceDtl.Find2("LsRpType", ((int)EnumRpType.ChinesePatentMedicine).ToString());
            details = this.QzNh.LstUspOuInvoiceDtl.Where(x => x.LsRpType == (int)EnumRpType.ChinesePatentMedicine);
            rows = details.Count();
            if (rows > 0)
            {
                requestBodyDetails.D502_03_01.D502_31_02 = new requestBodyDetailsD502_03_01Item1[rows];
                i = 0;
                foreach (var detail in details)
                {
                    string drugType = string.Empty;

                    Model.ModelList<Model.BsItemDrugInfo> drugs = _objBsItemDrug.BsItemDrug_SelectByItemId(detail.ItemId);
                    if (drugs.Count > 0)
                    {
                        BsDrugFormInfo drugFormInfo = _lstBsDrugForm.FindByID(drugs[0].FormId);
                        if (drugFormInfo == null)
                            throw new Exception(string.Format("Can not find drugFormInfo by item id {0}", detail.ItemId));
                        drugType = drugFormInfo.Name;
                    }
                    else
                    {
                        drugType = "无";
                    }
                    
                    _lstBsItemYb = _objBsItemYb.BsItemYb_SelectByItemIdAndTallyGroupId(detail.ItemId, this.QzNh.InfoTallyGroup.ID);

                    requestBodyDetails.D502_03_01.D502_31_02[i] = new requestBodyDetailsD502_03_01Item1()
                    {
                        D502_02 = this.QzNh.RegNo,
                        D502_04 = detail.Code,
                        D502_05 = detail.Name,
                        D502_06 = detail.Spec,
                        D502_07 = drugType, //剂型
                        D502_08 = Convert.ToDouble(detail.Price).ToString("#0.0000"),
                        D502_09 = detail.Totality.ToString(),
                        D502_11 = Convert.ToDouble(detail.Amount).ToString("#0.0000"),
                        D502_12 = detail.ID.ToString(),
                        D502_39 = _lstBsItemYb[0].YbCode,
                        D502_40 = _lstBsItemYb[0].YbDesc,
                        D502_43 = _lstBsUnit.FindByID(detail.UnitId).Name,
                    };
                    i++;
                }
            }            

            // 中草药项目
            //details = this.QzNh.LstUspOuInvoiceDtl.Find2("LsRpType", ((int)EnumRpType.ChineseMedicine).ToString());
            details = this.QzNh.LstUspOuInvoiceDtl.Where(x => x.LsRpType == (int)EnumRpType.ChineseMedicine);
            rows = details.Count();
            if (rows > 0) 
            {
                requestBodyDetails.D502_03_01.D502_31_03 = new requestBodyDetailsD502_03_01Item2[rows];
                i = 0;
                foreach (var detail in details)
                {
                    string drugType = string.Empty;

                    Model.ModelList<Model.BsItemDrugInfo> drugs = _objBsItemDrug.BsItemDrug_SelectByItemId(detail.ItemId);
                    if (drugs.Count > 0)
                    {
                        BsDrugFormInfo drugFormInfo = _lstBsDrugForm.FindByID(drugs[0].FormId);
                        if (drugFormInfo == null)
                            throw new Exception(string.Format("Can not find drugFormInfo by item id {0}", detail.ItemId));
                        drugType = drugFormInfo.Name;
                    }
                    else
                    {
                        drugType = "无";
                    }

                    _lstBsItemYb = _objBsItemYb.BsItemYb_SelectByItemIdAndTallyGroupId(detail.ItemId, this.QzNh.InfoTallyGroup.ID);

                    requestBodyDetails.D502_03_01.D502_31_03[i] = new requestBodyDetailsD502_03_01Item2()
                    {
                        D502_02 = this.QzNh.RegNo,
                        D502_04 = detail.Code,
                        D502_05 = detail.Name,
                        D502_06 = detail.Spec,
                        D502_07 = drugType, //剂型
                        D502_08 = Convert.ToDouble(detail.Price).ToString("#0.0000"),
                        D502_09 = detail.Totality.ToString(),
                        D502_11 = Convert.ToDouble(detail.Amount).ToString("#0.0000"),
                        D502_12 = detail.ID.ToString(),
                        D502_39 = _lstBsItemYb[0].YbCode,
                        D502_40 = _lstBsItemYb[0].YbDesc,
                        D502_43 = _lstBsUnit.FindByID(detail.UnitId).Name,
                        D502_10 = string.Empty //副数                    
                    };
                    i++;
                }
            }
            
            // 材料项目
            //details = this.QzNh.LstUspOuInvoiceDtl.Find2("LsRpType", ((int)EnumRpType.Other).ToString());
            details = this.QzNh.LstUspOuInvoiceDtl.Where(x => x.LsRpType == (int)EnumRpType.Other);
            rows = details.Count();
            if (rows > 0) 
            {
                requestBodyDetails.D502_03_02 = new requestBodyDetailsItem[rows];
                i = 0;
                foreach (var detail in details)
                {
                    //Model.BsItemYbRpTypeInfo infoBsItemYbRpType = _objBsItemYbRpType.GetByID(_lstBsItemYb[0].ItemYbRpTypeId);

                    _lstBsItemYb = _objBsItemYb.BsItemYb_SelectByItemIdAndTallyGroupId(detail.ItemId, this.QzNh.InfoTallyGroup.ID);

                    requestBodyDetails.D502_03_02[i] = new requestBodyDetailsItem()
                    {
                        D502_02 = this.QzNh.RegNo,
                        D502_04 = detail.Code,
                        D502_05 = detail.Name,
                        D502_08 = Convert.ToDouble(detail.Price).ToString("#0.0000"),
                        D502_09 = detail.Totality.ToString(),
                        D502_11 = Convert.ToDouble(detail.Amount).ToString("#0.0000"),
                        D502_12 = detail.ID.ToString(),
                        D502_39 = _lstBsItemYb[0].YbCode,
                        D502_40 = _lstBsItemYb[0].YbDesc,
                        D502_32 = this.QzNh.GetNhZd("财务分类", "27"), //infoBsItemYbRpType.Code.Trim(), //财务分类, 必填
                        D502_38 = _lstBsUnit.FindByID(detail.UnitId).Name // 单位                    
                    };
                    i++;
                }
            }
           

            // 剩余的为诊疗器材项目
            //details = this.QzNh.LstUspOuInvoiceDtl
            //    .FindNotInclude("LsRpType", ((int)EnumRpType.WestMedicine).ToString()) // 西药
            //    .FindNotInclude("LsRpType", ((int)EnumRpType.ChinesePatentMedicine).ToString()) // 中成药
            //    .FindNotInclude("LsRpType", ((int)EnumRpType.ChineseMedicine).ToString()) // 中草药
            //    .FindNotInclude("LsRpType", ((int)EnumRpType.Other).ToString()) //材料
            //    ;
            int[] types = new int[]
            {
                (int)EnumRpType.WestMedicine,
                (int)EnumRpType.ChinesePatentMedicine,
                (int)EnumRpType.ChineseMedicine,
                (int)EnumRpType.Other
            };
            details = this.QzNh.LstUspOuInvoiceDtl.Where(x => !types.Contains(x.LsRpType));
            rows = details.Count();
            if (rows > 0) 
            {
                requestBodyDetails.D502_03_03 = new requestBodyDetailsItem1[rows];
                i = 0;
                foreach (var detail in details)
                {
                    _lstBsItemYb = _objBsItemYb.BsItemYb_SelectByItemIdAndTallyGroupId(detail.ItemId, this.QzNh.InfoTallyGroup.ID);

                    requestBodyDetails.D502_03_03[i] = new requestBodyDetailsItem1()
                    {
                        D502_02 = this.QzNh.RegNo,
                        D502_04 = detail.Code,
                        D502_05 = detail.Name,
                        D502_06 = detail.Spec,
                        D502_08 = Convert.ToDouble(detail.Price).ToString("#0.0000"),
                        D502_09 = detail.Totality.ToString(),
                        D502_11 = Convert.ToDouble(detail.Amount).ToString("#0.0000"),
                        D502_12 = detail.ID.ToString(),
                        D502_39 = _lstBsItemYb[0].YbCode,
                        D502_40 = _lstBsItemYb[0].YbDesc,
                        D502_43 = _lstBsUnit.FindByID(detail.UnitId).Name,
                        D502_38 = _lstBsUnit.FindByID(detail.UnitId).Name // 单位                    
                    };
                    i++;
                }
            }
           
            return requestBodyDetails;
        }  
    }
}