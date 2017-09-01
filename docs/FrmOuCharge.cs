using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.Odbc;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using CrystalDecisions.CrystalReports.Engine;
using DevExpress.XtraGrid.Views.BandedGrid;
using DotNetSpeech;
using System.Threading;
using System.IO;
using System.Collections;
namespace XYHIS
{
    public partial class FrmOuCharge : Tools.FormToolbarBase
    {
        private BLL.Common.RecipeHelper<Model.uspOuInvoiceDtlQry> _uspOuInvoiceDtlRecipeHelper = new BLL.Common.RecipeHelper<Model.uspOuInvoiceDtlQry>();
        string[] _columnNamesForOuInvoiceDtl ={ "Code","Spec", "Name", "LsGfType", "LimitTotalMz", "TypeGFXEId", "ItemId", "InvItemId", "UnitId", "Price","DiscDiag",
"LimitGroupId","LimitFee","ExecLocId","InvItemId","FeeId","InvMzItemName","FeeHsId","UnitDiagName","FeeName","FeeHsMzName","LsRpType"};

        private Model.ModelList<Model.uspOuInvoiceDtlQry> _lstUspOuInvoiceDtl = new Model.ModelList<Model.uspOuInvoiceDtlQry>();
        private Model.uspOuInvoiceDtlQry _infoUspOuInvoiceDtl;
        private Model.ModelList<Model.uspOuInvoiceDtlPatientTodayQry> _lstUspOuInvoiceDtlPatientToday = new Model.ModelList<Model.uspOuInvoiceDtlPatientTodayQry>();
        private BLL.Finder<Model.uspOuInvoiceDtlQry> _uspOuInvoiceDtlFinder = new BLL.Finder<Model.uspOuInvoiceDtlQry>();
        private bool _isNew;//用于标识_infoUspOuInvoiceDtl是新项目（即从BsItemControl获取）还是从网格里获得
        private Model.ModelList<Model.uspOuInvoiceFeetyGoupSumQry> _lstUspOuInvoiceFeetyGoupSumQry = new Model.ModelList<Model.uspOuInvoiceFeetyGoupSumQry>();
        private Model.ModelList<Model.uspOuInvoiceInvItemGoupSumQry> _lstUspOuInvoiceInvItemGoupSumQry = new Model.ModelList<Model.uspOuInvoiceInvItemGoupSumQry>();
        private Model.ListView<Model.BsLimitGroupInfo> _lstvBsLimitGroup = new Model.ListView<Model.BsLimitGroupInfo>();
        private BLL.COuInvoice _objOuInvoice = new BLL.COuInvoice();
        private BLL.COuInvoiceDtl _objOuInvoiceDtl = new BLL.COuInvoiceDtl();
        private BLL.CBsPatient _objBsPatient = new BLL.CBsPatient();
        private Model.OuInvoiceInfo _infoOuInvoice;
        private BLL.Finder<Model.uspOuRecipeDtlForOuInvoiceDtlQry> _uspOuRecipeDtlForOuInvoiceDtFinder =
            new BLL.Finder<Model.uspOuRecipeDtlForOuInvoiceDtlQry>();
        private Model.ModelList<Model.uspOuRecipeDtlForOuInvoiceDtlQry> _lstUspOuRecipeDtlForOuInvoiceDtl = new Model.ModelList<Model.uspOuRecipeDtlForOuInvoiceDtlQry>();
        private Model.ModelList<Model.uspOuRecipeDtlForOuInvoiceDtlQry> _lstUspOuRecipeDtlForOuInvoiceDtlRemove = new Model.ModelList<Model.uspOuRecipeDtlForOuInvoiceDtlQry>();
        private BLL.Finder<Model.uspOuInvoiceFeetyGoupSumQry> _objUspOuInvoiceFeetyGoupSum = new BLL.Finder<Model.uspOuInvoiceFeetyGoupSumQry>();
        private BLL.Finder<Model.uspOuInvoiceInvItemGoupSumQry> _objUspOuInvoiceInvItemGroupSum = new BLL.Finder<Model.uspOuInvoiceInvItemGoupSumQry>();
        Model.ModelList<Model.OuRecipeDtlInfo> _lstChineseRecipeDtl;
        Model.ModelList<Model.OuRecipeDtlInfo> _lstCheckRecipeDtl;
        Model.ModelList<Model.OuRecipeDtlInfo> _lstWestRecipeDtl;
        BLL.COuRecipe _objOuRecipe = new BLL.COuRecipe();
        BLL.COuRecipeDtl _objOuRecipeDtl = new BLL.COuRecipeDtl();

        Model.BsPatTypeYbIllInfo InfoBsPatTypeYbIll = new Model.BsPatTypeYbIllInfo();//医保特殊病种
        private Model.ModelList<Model.uspOuInvoiceDtlQry> _lstUspOuInvoiceDtlFee = new Model.ModelList<Model.uspOuInvoiceDtlQry>();
        private Model.OuRecipeInfo _infoOuChineseRecipe;
        private Model.ModelList<Model.OuRecipeInfo> _lstChineseRecipe;
        private Model.ModelList<Model.uspOuRecipeDtlQry> _lstUspChineseRecipeDtl;
        private Model.ModelList<Model.uspBsItemMiniQry> _lstBsItemMini;
        private Model.ModelList<Model.BsOuMergeFeeInfo> _lstBsOuMergeFee;
        string IntegralItemId = BLL.Common.Utils.GetSystemSetting("IntegralItemId");
        BLL.COuHosInfo _objOuHosInfo = new BLL.COuHosInfo();
        BLL.CBsOuMergeFee _objBsOuMergeFee = new BLL.CBsOuMergeFee();
        private string _currentBalanceNo;
        private bool _isGf;
        private bool _isYb;
        private XYHIS.FrmCurrInvoInfo _frmCurrInvo = new XYHIS.FrmCurrInvoInfo();
        private bool _isOut;
        private FrmOuInvoicePay _frmOuInvoicePay = new FrmOuInvoicePay();
        public FrmOuCharge()
        {
            InitializeComponent();
            _frmCurrInvo.InvoiceType = Model.EnumGblInvType.OuChargeInvoice;
            this.devGrid1.advBandedGridViewMain.DoubleClick += new EventHandler(advBandedGridViewMain_DoubleClick);
            this.devGrid1.advBandedGridViewMain.CellValueChanging += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(advBandedGridViewMain_CellValueChanging);
            this.devGrid1.advBandedGridViewMain.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(advBandedGridViewMain_FocusedRowChanged);
            this.devGrid1.advBandedGridViewMain.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(advBandedGridViewMain_CellValueChanged);
            InitControl();
            _uspOuInvoiceDtlRecipeHelper.LsInOut = 1;
            _uspOuInvoiceDtlRecipeHelper.RecipeType = Model.EnumRecipeType.OuInvoiceDtl;
            this.utxtInvoNo.Properties.ReadOnly = false;
            this.KeyEnterToTab = false;
            this.KeyLeftToTab = false;

            _lock = new BLL.Common.LockKey(Model.EnumLockType.MzRegId);
            _lstBsItemMini = this.uicItemId.LstBsItemMini.ConvertTo<Model.uspBsItemMiniQry>();
            _lstBsOuMergeFee = _objBsOuMergeFee.GetAll();
            this.devGrid1.advBandedGridViewMain.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.devGrid1.advBandedGridViewMain.Appearance.Empty.BackColor = System.Drawing.Color.FromArgb(231, 242, 251);
            this.uicItemId.ThisForm = this;
        }
        Model.GblKeyLockSettingInfo infoKeyLock = new Model.GblKeyLockSettingInfo();
        void timer1_Tick(object sender, System.EventArgs e)
        {
            if (this.hisOuHosInfo1.Value == null || this.hisOuHosInfo1.Value.ID == 0) return;
            if (this.hisOuHosInfo1.Value.ID == 0 || infoKeyLock.ID > 0) return;
            if (this.FormStatus == Model.Configuration.ToolbarStatus.View) return;

            if (_lock.CheckLocked(this.hisOuHosInfo1.Value.ID) == "别人锁")
            {
                infoKeyLock = _lock.GetGblKeyLockSetting();
                if (infoKeyLock != null && infoKeyLock.WorkStationName != string.Empty)
                {
                    MessageBox.Show(this, string.Format("您己经被{0}的{1}用户踢出，系统将自动退出该病人!", infoKeyLock.WorkStationName, BLL.Common.Utils.GetBaseTableRowField<Model.BsUserInfo>("BsUser", "Name", Convert.ToInt32(infoKeyLock.UserId))), "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetAfterCancel();
                }
            }
            infoKeyLock = new Model.GblKeyLockSettingInfo();
        }
        void txtBsItem_GotFocus(object sender, System.EventArgs e)
        {
            //if (Model.Configuration.UserProfiles.CurrentInputLanguage != null)
            //    System.Windows.Forms.InputLanguage.CurrentInputLanguage = Model.Configuration.UserProfiles.CurrentInputLanguage;
            //else
            //    System.Windows.Forms.InputLanguage.CurrentInputLanguage = Tools.Utils.ReadInputLaguage();
            foreach (System.Windows.Forms.InputLanguage il in System.Windows.Forms.InputLanguage.InstalledInputLanguages)
            {
                if (il.LayoutName.Contains("英") || il.LayoutName.Contains("美"))
                    System.Windows.Forms.InputLanguage.CurrentInputLanguage = il;
            }
        }

        void FrmOuCharge_Load(object sender, System.EventArgs e)
        {
            InitControlStyle(this.tableLayoutPanel1, this.devGrid1.FontSize, "Label");
            this.uicItemId.txtBsItem.Font = new System.Drawing.Font("宋体", this.devGrid1.FontSize, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        }

        CrystalDecisions.CrystalReports.Engine.ReportDocument OuInvoicePrinting = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        private void OpenReportDocument(Model.ModelList<Model.uspInInvoice2PrintQry> lstUspOuInvoicePrintNew)
        {
            OuInvoicePrinting = Tools.Utils.GetRptFileByName("InInvoice2Printing");
            OuInvoicePrinting.SetDataSource(lstUspOuInvoicePrintNew);
        }
        protected override void FormToolbarBase_KeyDown(object sender, KeyEventArgs e)
        {
            base.FormToolbarBase_KeyDown(sender, e);
            if (e.KeyCode == Keys.Add || e.KeyCode == Keys.Oemplus)
            {
                this.ubsExecLocId.Focus();
                return;
            }
        }
        void FrmOuCharge_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.PageUp)
            //{
            //    SearchIninvoice(-1);
            //}
            //if (e.KeyCode == Keys.PageDown)
            //{
            //    SearchIninvoice(1);
            //}
        }
        /// <summary>
        /// 当前发票号
        /// </summary>
        public string CurrentBalanceNo
        {
            get
            {
                BLL.CInInvoice objInInvoice = new BLL.CInInvoice();
                if (System.Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsInOuSameCurrInvo")) && objInInvoice.InInvoice_SelectByInvoNo(_currentBalanceNo).Count > 0)
                    _currentBalanceNo = _frmCurrInvo.CalculateNextInvoNo();

                if (hisOuHosInfo1.Value.PatTypeId == 171 && !_currentBalanceNo.StartsWith("X"))
                {
                    _currentBalanceNo = "X" + _currentBalanceNo;
                }

                return _currentBalanceNo;
            }
            set
            {
                _currentBalanceNo = value;
                this.utxtInvoNo.Text = _currentBalanceNo;
            }
        }

        void ExecLocId_DoubleClick(object sender, EventArgs e)
        {
            int rowIndex = this.devGrid1.advBandedGridViewMain.FocusedRowHandle;
            if (rowIndex < 0 || (this._infoOuInvoice != null && _infoOuInvoice.ID > 0)) return;
            foreach (System.Windows.Forms.InputLanguage il in System.Windows.Forms.InputLanguage.InstalledInputLanguages)
            {
                if (il.LayoutName.Contains("英") || il.LayoutName.Contains("美"))
                    System.Windows.Forms.InputLanguage.CurrentInputLanguage = il;
            }
            if (this.FormStatus == Model.Configuration.ToolbarStatus.Add || this.FormStatus == Model.Configuration.ToolbarStatus.Edit || e == null)
            {
                FrmGridLookupInput frm = new FrmGridLookupInput();
                frm.lblTitle.Text = "执行科室";
                frm.BaseType = "BsLocation";
                frm.ShowDialog(this);

                if (frm.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    if (frm.chkBatch.Checked)
                    {
                        Model.ListView<Model.uspOuInvoiceDtlQry> lsvUspOuInvoiceDtlQry = _lstUspOuInvoiceDtl.DefaultView;
                        lsvUspOuInvoiceDtlQry.Filter = string.Format("FeeId={0}", _lstUspOuInvoiceDtl[rowIndex].FeeId);
                        foreach (Model.uspOuInvoiceDtlQry info in lsvUspOuInvoiceDtlQry)
                        {
                            info.ExecLocId = frm.SelectedID;
                            GetDrugSendMemo(info);
                            if (e == null)
                                SaveExecLocId(info);
                        }
                    }
                    else
                    {
                        _lstUspOuInvoiceDtl[rowIndex].ExecLocId = frm.SelectedID;
                        if (e == null)
                            SaveExecLocId(_lstUspOuInvoiceDtl[rowIndex]);
                    }
                    this.devGrid1.advBandedGridViewMain.RefreshData();
                    this.ubsExecLocId.ID = frm.SelectedID;
                }
            }
        }
        void SaveExecLocId(Model.uspOuInvoiceDtlQry info)
        {
            BLL.COuRecipeDtl objOuRecipeDtl = new BLL.COuRecipeDtl();
            Model.OuRecipeDtlInfo infoRecipeDtl = objOuRecipeDtl.GetByID(info.RecipeItemId);
            if (!infoRecipeDtl.IsIssue)
            {
                Model.OuInvoiceDtlInfo infoOuInvoiceDtl = _objOuInvoiceDtl.GetByID(info.ID);
                infoOuInvoiceDtl.ExecLocId = info.ExecLocId;
                infoOuInvoiceDtl.Memo = info.Memo;
                _objOuInvoiceDtl.Modify(infoOuInvoiceDtl, null);
            }
        }
        void advBandedGridViewMain_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            int rowIndex = e.FocusedRowHandle;
            if (rowIndex < 0 || _lstUspOuInvoiceDtl.Count <= rowIndex) return;
            if (_lstUspOuInvoiceDtl.Count == 1)//因为如果只有一条记录的时候可以修改医生的数量
            {
                if (_lstUspOuInvoiceDtl[rowIndex].LsAdviceType != 2 && _lstUspOuInvoiceDtl[rowIndex].IsDoctorInput)
                {
                    this.devGrid1.advBandedGridViewMain.Columns["Totality"].OptionsColumn.AllowEdit = false;
                    this.devGrid1.advBandedGridViewMain.OptionsBehavior.Editable = false;
                }
            }
            if (this.FormStatus == Model.Configuration.ToolbarStatus.View) return;

            if (CheckDrugItemIssued(_lstUspOuInvoiceDtl[e.FocusedRowHandle].RecipeItemId))
            {
                this.ShowInformationInMainForm("您不能修改已发药药品，请确认！");
                this.devGrid1.advBandedGridViewMain.OptionsBehavior.Editable = false;
            }
            else if (_lstUspOuRecipeDtlForOuInvoiceDtl != null && _lstUspOuInvoiceDtl[e.FocusedRowHandle].LsAdviceType != 2 &&
                _lstUspOuRecipeDtlForOuInvoiceDtl.Find("RecipeItemId", _lstUspOuInvoiceDtl[e.FocusedRowHandle].RecipeItemId.ToString()).Find("IsDoctorInput", "True").Count > 0)
            {
                this.ShowInformationInMainForm("您不能修改医生开的处方，请通知医生本人进行修改！");
                this.devGrid1.advBandedGridViewMain.OptionsBehavior.Editable = false;
            }
            else if (_lstUspChineseRecipeDtl != null && _lstUspChineseRecipeDtl.Find("ItemId", _lstUspOuInvoiceDtl[e.FocusedRowHandle].ItemId.ToString()).Count > 0)
            {
                this.ShowInformationInMainForm("您不能这网格直接修改中药处方，请按“中药”按钮进行修改，请确认！");
                this.devGrid1.advBandedGridViewMain.OptionsBehavior.Editable = false;
            }
            else
            {
                this.devGrid1.advBandedGridViewMain.OptionsBehavior.Editable = true;
                this.devGrid1.advBandedGridViewMain.Columns["Totality"].OptionsColumn.AllowEdit = true;
            }

            this.devGrid1.advBandedGridViewMain.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.devGrid1.advBandedGridViewMain.Appearance.Empty.BackColor = System.Drawing.Color.Gainsboro;
        }

        void advBandedGridViewMain_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            this.devGrid1.advBandedGridViewMain.RefreshData();
        }
        protected override void AcceptData()
        {
            //base.AcceptData();
            this.devGrid1.advBandedGridViewMain.MoveFirst();
            this.devGrid1.advBandedGridViewMain.FocusedColumn = this.devGrid1.advBandedGridViewMain.Columns[3];
        }
        /// <summary>
        /// 双击后可以修改数量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void advBandedGridViewMain_DoubleClick(object sender, EventArgs e)
        {
            if (this.devGrid1.advBandedGridViewMain.FocusedRowHandle < 0 || this.devGrid1.advBandedGridViewMain.FocusedRowHandle > this.devGrid1.advBandedGridViewMain.RowCount - 1) return;
            Model.uspOuInvoiceDtlQry infoUspOuInvoiceDtl = _lstUspOuInvoiceDtl[this.devGrid1.advBandedGridViewMain.FocusedRowHandle];
            if (infoUspOuInvoiceDtl.RecipeItemId > 0) //如果是医生开出的处方，则收费员不能修改项目和数量
            {
                this.InformationInMainForm = "该项目是从医生的电子处方导入，收费员不能修改，如果要修改请与医生联系！";
                _infoUspOuInvoiceDtl = new Model.uspOuInvoiceDtlQry();
                _isNew = true;
            }
            else
            {
                _infoUspOuInvoiceDtl = infoUspOuInvoiceDtl;
                _isNew = false;  //表示项目不是要新增（即项目是已有项目）
            }
            if (infoUspOuInvoiceDtl.F4 != string.Empty || this.FormStatus == Model.Configuration.ToolbarStatus.View)
                ExecLocId_DoubleClick(null, null);
            else
                BindBsItemData(_infoUspOuInvoiceDtl, false);
        }

        void advBandedGridViewMain_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //if (!Utilities.Information.IsNumeric(e.Value)) return;
            if (e.Value.ToString() == string.Empty) return;
            if (!Utilities.Information.IsNumeric(e.Value)) return;
            if (e.Column.FieldName == "DiscDiag")
            {
                _lstUspOuInvoiceDtl[e.RowHandle].DiscDiag = Convert.ToDouble(e.Value) / 100;
                ReCalculateDetailAmount(Convert.ToDouble(e.Value) / 100, _lstUspOuInvoiceDtl[e.RowHandle].DiscSelf, _lstUspOuInvoiceDtl[e.RowHandle]);
            }
            else if (e.Column.FieldName == "DiscSelf")
            {
                _lstUspOuInvoiceDtl[e.RowHandle].DiscSelf = Convert.ToDouble(e.Value) / 100;
                ReCalculateDetailAmount(_lstUspOuInvoiceDtl[e.RowHandle].DiscDiag, Convert.ToDouble(e.Value) / 100, _lstUspOuInvoiceDtl[e.RowHandle]);
            }
            else if (e.Column.FieldName == "LimitGroupId")//限额组改变时获取新住院限额
            {
                BLL.CBsLimitGroup objBsLimitGroup = new BLL.CBsLimitGroup();
                Model.BsLimitGroupInfo infoBsLimitGroup = objBsLimitGroup.GetByID(Convert.ToInt32(e.Value));
                _lstUspOuInvoiceDtl[e.RowHandle].LimitGroupId = infoBsLimitGroup.ID;
                _lstUspOuInvoiceDtl[e.RowHandle].LimitFee = infoBsLimitGroup.LimitFeeMz;
            }
            //更改限额时将同一限额组的限额同步更改
            else if (e.Column.FieldName == "LimitFee")
            {
                Model.ListView<Model.uspOuInvoiceDtlQry> lstv = _lstUspOuInvoiceDtl.DefaultView;
                lstv.Filter = string.Format("LimitGroupId = {0}", _lstUspOuInvoiceDtl[e.RowHandle].LimitGroupId.ToString());
                lstv.Fill("LimitFee", e.Value);
            }
            else if (e.Column.FieldName == "ExecLocId")
            {
                _lstUspOuInvoiceDtl[e.RowHandle].ExecLocId = Convert.ToInt32(e.Value);
                GetDrugSendMemo(_lstUspOuInvoiceDtl[e.RowHandle]);
            }
            else if (e.Column.FieldName == "Totality")
            {
                _lstUspOuInvoiceDtl[e.RowHandle].Totality = Convert.ToDouble(e.Value);

                BLL.Common.RecipeHelper<Model.uspOuInvoiceDtlQry> _recipeHelper = new BLL.Common.RecipeHelper<Model.uspOuInvoiceDtlQry>();
                if (_lstUspOuInvoiceDtl[e.RowHandle].F3 == "RoomDrug" && !_recipeHelper.CheckInStockNumIsEnough(_lstUspOuInvoiceDtl[e.RowHandle].Totality, 0, (int)Model.EnumRoomInOut.ForOutPatient, _lstUspOuInvoiceDtl[e.RowHandle].ItemId, _lstUspOuInvoiceDtl[e.RowHandle].Name))
                {
                    this.InformationInMainForm = "药房库存不足够！";
                    CreateInfoToolTip("数量不能大于药房库存", this.devGrid1.advBandedGridViewMain, e.RowHandle, e.Column.ColumnHandle);
                    return;
                }

                ReCalculateDetailAmount(_lstUspOuInvoiceDtl[e.RowHandle].DiscDiag, _lstUspOuInvoiceDtl[e.RowHandle].DiscSelf, _lstUspOuInvoiceDtl[e.RowHandle]);
            }

            CalcuateAmountTally();
            InitData();
        }
        /// <summary>
        /// 设置工具栏的状态
        /// </summary>
        public void SetToolbar()
        {
            this.Key = "Toolbar.OuCharge";
            this.LoadToolbar();
            this.FormStatus = Model.Configuration.ToolbarStatus.View;

        }

        /// <summary>
        /// 设置基本表下拉控件的内容
        /// </summary>
        public void SetBsItemControl()
        {
            this.uicItemId.LocationID = Model.Configuration.UserProfiles.LocationID;
            //this.uicItemId.lsRpType = "1,2,3,4,5,6,7,8,9";
            this.uicItemId.LsInOut = 1;
            this.uicItemId.IsStore = false;
            this.uicItemId.KeyDownEvent += new EventHandler(uicInvItemId_KeyDownEvent);
        }
        /// <summary>
        /// 如果该病人今天已经开过这个项目，再收一次就记录下来，保存时不与处方明细同步
        /// </summary>
        /// <param name="itemId"></param>
        private void MarkMutiInputInSamePatient(int itemId)
        {
            int hasChargeSameItemBefore = Convert.ToInt32(BLL.absBusiness<Model.absModel>.ExecuteScalar("uspOuHasDoublePatientChargeItem", this.hisOuHosInfo1.Value.ID.ToString(), itemId.ToString()));
            if (hasChargeSameItemBefore > 0 && _infoUspOuInvoiceDtl != null)
            {
                _infoUspOuInvoiceDtl.F2 = "重复补";
            }
        }

        private void GetDrugSendMemo(Model.uspOuInvoiceDtlQry infoOuInvoiceDtl)
        {
            if (infoOuInvoiceDtl.Memo.Contains("药房免发")) return;
            BLL.CRmUnderLine objRmUnderLine = new BLL.CRmUnderLine();
            BLL.CBsRoom objBsRoom = new BLL.CBsRoom();
            //药房药品、材料设置由，但执行科室不是药房的
            if (objRmUnderLine.RmUnderLine_SelectByItemId(infoOuInvoiceDtl.ItemId).Count > 0 && objBsRoom.BsRoom_SelectByLocationId(infoOuInvoiceDtl.ExecLocId).Find("F3", "1").Count == 0)
            {
                infoOuInvoiceDtl.Memo = Tools.Utils.AddMemoString(infoOuInvoiceDtl.Memo, "药房免发");
            }
        }
        void txtBsItem_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            //if (!this.uicItemId.IsXdRpUsing && this.uicItemId.txtBsItem.Text == string.Empty && this.uicItemId.ItemID == 0 && e.KeyCode == Keys.Enter && Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOuChargeEnterBeginBalance")))
            //    BeginBalance(false);
            this.uicItemId.IsXdRpUsing = false;
        }
        BLL.CBsItemAttach objBsItemAttach = new BLL.CBsItemAttach();
        private void uicInvItemId_KeyDownEvent(object sender, EventArgs e)
        {
            if (!CheckDiagLocationAndDoctor()) return;
            _infoDoctor = objBsDoctor.GetByID(this.hisOuHosInfo1.Value.DoctorId);
            Model.uspBsItemSelectQry info = this.uicItemId.SelectBsItemInfo;
            if (info == null) return;
            if (!CheckIfCanAddNewItem(info.LsRpType, info.ID)) return;
            if (sender != null && !CheckHasDoctorRecipe(info.ID)) return;

            if ((info.LsRpType == 1 || info.LsRpType == 2 || info.LsRpType == 3) && this.hisOuHosInfo1.Value.DoctorId > 0 && _infoDoctor.F2 != "1")
            {
                Utilities.Information.ShowMsgBox("该医生没有处方权，不能输入药品！");
                return;
            }
            if (_lstSamePatientName.Contains("ItemId", info.ID.ToString()))
            {
                if (DialogResult.No == MessageBox.Show("该病人同名同一天同一个医生已经收过诊金或挂号费，是否继续？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
                {
                    return;
                }
            }
            BLL.CRmUnderLine objRmUnderLine = new BLL.CRmUnderLine();
            _infoUspOuInvoiceDtl = new Model.uspOuInvoiceDtlQry();
            _uspOuInvoiceDtlRecipeHelper.SetBsItemObjectInfo(info, _columnNamesForOuInvoiceDtl, _infoUspOuInvoiceDtl);

            if (System.Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOuChargeOnePatMutiInvoice")) && _lstUspOuInvoiceDtl.Count > 0 && _lstUspOuInvoiceDtl[_lstUspOuInvoiceDtl.Count - 1].InvItemId == info.InvMzItemId)
                _infoUspOuInvoiceDtl.ExecLocId = this.ubsExecLocId.ID = info.ExecLocId;//_lstUspOuInvoiceDtl[_lstUspOuInvoiceDtl.Count - 1].ExecLocId;
            else if (info.LocationId > 0)
                _infoUspOuInvoiceDtl.ExecLocId = this.ubsExecLocId.ID = info.LocationId;
            //else if (info.LsRpType < 4)
            //    this.ubsExecLocId.ID = _infoUspOuInvoiceDtl.ExecLocId = GetDefeultRoomLocationId(1);
            else if (info.LsRpType != 3)
                this.ubsExecLocId.ID = _infoUspOuInvoiceDtl.ExecLocId = GetDefeultRoomLocationId(1, info.ID);
            else if (info.LsRpType == 3)
                this.ubsExecLocId.ID = _infoUspOuInvoiceDtl.ExecLocId = GetDefeultRoomLocationId(2, info.ID);
            if (_infoUspOuInvoiceDtl.ExecLocId == 0)
                _infoUspOuInvoiceDtl.ExecLocId = this.ubsExecLocId.ID = this.hisOuHosInfo1.Value.DiagnDept;
            _infoUspOuInvoiceDtl.FeeHsId = BLL.InsertAccount.GetExecLocId(_infoUspOuInvoiceDtl.ExecLocId, info.ID, true);
            _infoUspOuInvoiceDtl.DoctorId = this.hisOuHosInfo1.Value.DoctorId;
            _infoUspOuInvoiceDtl.Memo += BLL.Common.Utils.GetYbDesc(info.ID, this.hisOuHosInfo1.Value.TallyGroupId, string.Empty);
            _infoUspOuInvoiceDtl.ItemId = info.ID;
            if (info.RoomIdOut > 0) _infoUspOuInvoiceDtl.F3 = "RoomDrug";
            MarkMutiInputInSamePatient(_infoUspOuInvoiceDtl.ItemId);

            _isNew = true;//表示项目是新项目（即要新增项目）
            if (!this.uicItemId.IsManualItem)  //如果不是录入而是调用则省去界面实例化
            {
                BindBsItemData(_infoUspOuInvoiceDtl, true);
                this.uicItemId.txtBsItem.EditValue = _infoUspOuInvoiceDtl.Name;
                if (BLL.Common.Utils.CheckSettingContain("OuChargeInputPriceItemIds", info.ID, false))
                    this.utxtTotality.TextBoxType = Model.EnumTextBoxType.String;
                else
                {
                    this.utxtTotality.TextBoxType = Model.EnumTextBoxType.NumAmount;
                    if (Convert.ToBoolean(BLL.Common.Utils.ReadLocalRegitFile("IsOuChargeDefaultNum")))
                        this.utxtTotality.EditValue = 1;
                    else
                        this.utxtTotality.EditValue = 0;
                }
                this.utxtTotality.Text = "1";
                this.utxtTotality.Focus();
                this.utxtTotality.SelectAll();
            }
        }
        private int GetIssueRoomId(int lsRoomType)
        {
            int roomId = 0;
            Model.BsLocationInfo infoMyLocation = BLL.Common.Utils.GetBaseTableRowInfo<Model.BsLocationInfo>("BsLocation", this.hisOuHosInfo1.Value.DiagnDept);
            if (infoMyLocation.OuRoomId > 0)
            {
                roomId = infoMyLocation.OuRoomId;
            }
            else
            {
                foreach (Model.uspOuInvoiceDtlQry info in _lstUspOuInvoiceDtl)
                {
                    Model.BsRoomInfo infoBsRoom = BLL.Common.Utils.GetBaseTableRowInfo<Model.BsRoomInfo>("BsRoom", info.RoomId);
                    if (infoBsRoom.LsRoomType == lsRoomType && infoBsRoom.F3 == "1")
                        return infoBsRoom.ID;
                }
            }
            return roomId;
        }
        private int GetDefeultRoomLocationId(int lsRoomType, int itemId)
        {
            BLL.CRmUnderLine objRmUnderLine = new BLL.CRmUnderLine();
            BLL.CBsRoom objRoom = new BLL.CBsRoom();
            BLL.CBsLocation objBsLocation = new BLL.CBsLocation();
            if (this.ubsExecLocId.ID > 0 && objRoom.BsRoom_SelectByLocationId(this.ubsExecLocId.ID).Find("LsRoomType", lsRoomType.ToString()).Count > 0)
                return this.ubsExecLocId.ID;  //如果用户已经选择了药房就不要变
            Model.ModelList<Model.BsRoomInfo> lstRoom = objRoom.GetDynamic("LsInOut in (1,3) and IsActive = 1", null);
            foreach (Model.BsRoomInfo infoRoom in lstRoom)
            {
                if (infoRoom.F3 == "1" && infoRoom.LsRoomType == lsRoomType && objBsLocation.GetByID(infoRoom.LocationId).HospitalId == Model.Configuration.UserProfiles.HospitalID
                    && objRmUnderLine.RmUnderLine_SelectByItemIdAndRoomId(itemId, infoRoom.ID).Count > 0)
                    return infoRoom.LocationId;
            }
            return 0;
        }
        /// <summary>
        /// 根据当前病人的类别，实例化限额组的下拉信息
        /// </summary>
        private void GetPatientLimitGroup()
        {
            BLL.Common.RecipeHelper<Model.BsLimitGroupInfo> recipeHelper = new BLL.Common.RecipeHelper<Model.BsLimitGroupInfo>();
            _lstvBsLimitGroup = recipeHelper.FilterLookUpEditDataSource<Model.BsLimitGroupInfo>(this.devGrid1.advBandedGridViewMain, "LimitGroupId", string.Format("PatTypeId = {0}", this.hisOuHosInfo1.Value.PatTypeId.ToString()));
        }
        private void SetStyleOfGfOrYb()
        {
            _isGf = this.hisOuHosInfo1.Value.IsGf;
            _isYb = this.hisOuHosInfo1.Value.IsYb;
            if (_isYb || _isGf)
                SetStyleOfGfFee();
            else
                SetStyleOfNotGfOrYb();
        }
        /// <summary>
        /// 设置公费的显示风格
        /// </summary>
        private void SetStyleOfGfFee()
        {
            DevExpress.XtraGrid.StyleFormatCondition cn = new DevExpress.XtraGrid.StyleFormatCondition(DevExpress.XtraGrid.FormatConditionEnum.NotEqual, devGrid1.advBandedGridViewMain.Columns["LsGfType"], 2, 2);
            cn.ApplyToRow = true;
            cn.Appearance.Font = new Font("宋体", this.devGrid1.FontSize, FontStyle.Bold);
            this.devGrid1.advBandedGridViewMain.FormatConditions.Add(cn);
            this.devGrid1.advBandedGridViewMain.Bands["gridBand3"].Visible = true;
            this.devGrid1.advBandedGridViewMain.Columns["LsGfType"].Visible = true;
        }
        /// <summary>
        /// 设置优惠的显示风格（优惠时将比例项目隐藏）
        /// </summary>
        private void SetStyleOfNotGfOrYb()
        {
            this.devGrid1.advBandedGridViewMain.FormatConditions.Clear();
            this.devGrid1.advBandedGridViewMain.Columns["LsGfType"].Visible = false;
            this.devGrid1.advBandedGridViewMain.Bands["gridBand3"].Visible = false;
        }
        /// <summary>
        /// 显示主表信息
        /// </summary>
        private void ShowMaster()
        {
            //CalculateInvoiceAmount();
            InitData();
            WriteUnbindData();
        }

        private void SetOuInvoiceRoomType()
        {
            //如果要分发票发药，则中药、西药必须分发票
            BLL.CRmUnderLine objUnderLine = new BLL.CRmUnderLine();
            foreach (Model.uspOuInvoiceDtlQry infoOuInvoiceDtl in _lstUspOuInvoiceDtl)
            {
                if (objUnderLine.RmUnderLine_SelectByItemIdAndRoomId(infoOuInvoiceDtl.ItemId, Convert.ToInt32(BLL.Common.Utils.GetSystemSetting("OuChinRoomId"))).Count > 0)
                {
                    _infoOuInvoice.F4 = "2";
                    return;
                }
                else if (objUnderLine.RmUnderLine_SelectByItemIdAndRoomId(infoOuInvoiceDtl.ItemId, Convert.ToInt32(OuWestRoomId)).Count > 0)
                {
                    _infoOuInvoice.F4 = "1";
                    return;
                }
            }
        }

        bool IsNotPrintInvoice = false;

        /// <summary>
        /// 保存结算后信息
        /// </summary>
        BLL.COuHosInfo objOuHosInfo = new BLL.COuHosInfo();
        bool _isDoubleCharged = false;
        bool IsOuChargePrintMZCure = Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOuChargePrintMZCure"));
        bool IsOuChargePrintMzDrop = Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOuChargePrintMzDrop"));
        bool IsOuChargePrintMzReject = Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOuChargePrintMzReject"));
        bool IsOuMzDropPrintDays = Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOuMzDropPrintForDays"));
        string OuWestRoomId = BLL.Common.Utils.GetSystemSetting("OuWestRoomId");
        BLL.CRmSending objRmSending = new BLL.CRmSending();
        double Insurance1 = 0;
        private bool SaveOuBalance(bool isHidePayForm)
        {
            //插入凑整费
            //if()
            InsertTintFee();

            Model.ModelList<Model.OuInvoicePayInfo> lstOuInvoicePay = new Model.ModelList<Model.OuInvoicePayInfo>();
            _frmOuInvoicePay.usgInvItemType.DataSource = _lstUspOuInvoiceInvItemGoupSumQry;

            if (this.hisOuHosInfo1.Value.PatTypeName.IndexOf("在职医保") > 0)
                _frmOuInvoicePay.IsYb = this.hisOuHosInfo1.Value.IsYb;
            _frmOuInvoicePay.PatId = this.hisOuHosInfo1.Value.PatId;

            _frmOuInvoicePay.Beprice = BLL.Common.Utils.Round(_infoOuInvoice.Beprice + _infoOuInvoice.AddFee, 2);
            if (this.hisOuHosInfo1.Value.TallyGroupId == 21 && this.hisOuHosInfo1.Value.TallyTypeId == 83)
            {
                _frmOuInvoicePay.TallyGroupId = this.hisOuHosInfo1.Value.TallyGroupId;
                _frmOuInvoicePay.Insurance1 = Insurance1;
                _frmOuInvoicePay.IsYb = this.hisOuHosInfo1.Value.IsYb;
                _frmOuInvoicePay.PatTypeId = this.hisOuHosInfo1.Value.PatTypeId;
                _frmOuInvoicePay.Insurance = 0;
                _frmOuInvoicePay.Amount = BLL.Common.Utils.Round(_infoOuInvoice.Beprice + _infoOuInvoice.AddFee, 2);
                _frmOuInvoicePay.SetColumnIsEdit(false);
                Insurance1 = 0;
            }
            else
            {
                //_frmOuInvoicePay.SetColumnIsEdit(false);
                _frmOuInvoicePay.Insurance = _infoOuInvoice.Insurance;
                _frmOuInvoicePay.Amount = BLL.Common.Utils.Round(_infoOuInvoice.AmountPay, 2);
            }

            if (!this.CurrentBalanceNo.StartsWith("X"))
                _frmOuInvoicePay.InvoNo = this.CurrentBalanceNo;
            led.ShowLed(_frmOuInvoicePay.Amount, "01", this.hisOuHosInfo1.Value.PatientName);
            if (isHidePayForm || _frmOuInvoicePay.ShowDialog() == DialogResult.OK)
            //if (_frmOuInvoicePay.ShowDialog() == DialogResult.OK)
            {
                lstOuInvoicePay = _frmOuInvoicePay.LstOuInvoicePay;
            }
            else
            {
                BLL.Common.Utils.EndBeginCharge(this.hisOuHosInfo1.Value.ID.ToString());
                if (Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsUpLoadYbFee")) && this.hisOuHosInfo1.Value.TallyGroupId == 21 && this.hisOuHosInfo1.Value.TallyTypeId == 83)//金保医保退费
                {
                    //bool aa=Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsUpLoadYbFee"));
                    CancelYBBalance();
                }
                _frmOuInvoicePay.IsYb = false;
                return false;
            }
            _frmOuInvoicePay.IsYb = false;
            if (Convert.ToDouble(_frmOuInvoicePay.txtChargeBack.Text) > 0)
                led.ShowLed(_frmOuInvoicePay.ChargeBack, "04", this.hisOuHosInfo1.Value.PatientName);
            //if (BLL.Common.Utils.Round(lstOuInvoicePay.GetSum("Amount"), 2) != BLL.Common.Utils.Round(_infoOuInvoice.AmountPay, 2) || BLL.Common.Utils.Round(_lstUspOuInvoiceDtl.GetSum("Amount"), 2) != BLL.Common.Utils.Round(_infoOuInvoice.Beprice + _infoOuInvoice.AddFee, 2))
            //{
            //    BLL.Common.Utils.EndBeginCharge(this.hisOuHosInfo1.Value.ID.ToString());
            //    this.InformationInMainForm = "病人交付总额与应交金额不一致！不能继续结算！请检查";
            //    return false;
            //}
            //查找当前的发票分类的金额是否等于明细的金额
            if (BLL.Common.Utils.Round(_lstUspOuInvoiceInvItemGoupSumQry.GetSum("Amount") + _infoOuInvoice.AddFee, 2) != BLL.Common.Utils.Round(_lstUspOuInvoiceDtl.GetSum("Amount"), 2) || _frmOuInvoicePay.Beprice != BLL.Common.Utils.Round(_lstUspOuInvoiceDtl.GetSum("Amount"), 2))
            {
                BLL.Common.Utils.EndBeginCharge(this.hisOuHosInfo1.Value.ID.ToString());
                this.InformationInMainForm = "发票总金额跟发票分类的总金额不一致！不能继续结算！请检查";
                return false;
            }
            if (this.utxtTallyNo.Text.Trim() != string.Empty && BLL.Common.Utils.GetSystemSetting("OuTallyIntegralPatTypeIds").Contains(string.Format("[{0}]", this.hisOuHosInfo1.Value.PatTypeId)) && !Utilities.Information.IsInteger(_infoOuInvoice.Beprice))
            {
                BLL.Common.Utils.EndBeginCharge(this.hisOuHosInfo1.Value.ID.ToString());
                this.InformationInMainForm = string.Format("该病人记账金额[{0}]需要凑整！请检查", _infoOuInvoice.Beprice);
                return false;
            }
            //收费的时候把病人保存到RmSending，如果有收费不让踢出
            if (!_lock.Lock(this.hisOuHosInfo1.Value.ID)) return false;

            _infoOuInvoice.InvoNo = this.CurrentBalanceNo;
            if (Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOuDrugIssueByInvoice")))
                SetOuInvoiceRoomType();
            BLL.COuInvoiceDtl objOuInvoiceDtl = new BLL.COuInvoiceDtl();

            Model.ModelList<Model.OuInvoiceFeetySumInfo> lstFeetyItemSum = _lstUspOuInvoiceFeetyGoupSumQry.ConvertTo<Model.OuInvoiceFeetySumInfo>();
            BLL.COuInvoiceFeetySum objInvoiceFeetySum = new BLL.COuInvoiceFeetySum();

            Model.ModelList<Model.OuInvoiceInvItemSumInfo> lstInvItemSum = _lstUspOuInvoiceInvItemGoupSumQry.ConvertTo<Model.OuInvoiceInvItemSumInfo>();
            BLL.COuInvoiceInvItemSum objInvoiceInvItemSum = new BLL.COuInvoiceInvItemSum();
            BLL.COuInvoicePay objOuInvoicePay = new BLL.COuInvoicePay();
            BLL.COuRecipeDtl objOuRecipeDtl = new BLL.COuRecipeDtl();
            Model.ModelList<Model.OuRecipeDtlInfo> lstOuRecipeDtl = new Model.ModelList<Model.OuRecipeDtlInfo>();

            _isDoubleCharged = false;
            DAL.SqlUtil db = new DAL.SqlUtil();
            System.Data.Common.DbTransaction trn = db.GetSqlTransaction();
            try
            {
                Model.OuHosInfoInfo info = new Model.OuHosInfoInfo();
                Model.ModelList<Model.OulInvoiceRegInfo> lstOulInvoiceReg = objOulInvoiceReg.OulInvoiceReg_SelectByMzRegId(this.hisOuHosInfo1.Value.ID);
                info = objOuHosInfo.GetByID(this.hisOuHosInfo1.Value.ID);
                info.DoctorId = this.hisOuHosInfo1.Value.DoctorId;
                //if (info.DiagnDept == 0 && Convert.ToInt32(BLL.Common.Utils.GetSystemSetting("OuHosInfoDefultRegLocId")) == this.hisOuHosInfo1.Value.DiagnDept)
                if (info.DiagnDept == 0 && BLL.Common.Utils.GetSystemSetting("OuHosInfoDefultRegLocId").Contains(string.Format("[{0}]", this.hisOuHosInfo1.Value.DiagnDept)))
                    info.DiagnDept = objBsDoctor.GetByID(info.DoctorId).LocationId;
                else
                    info.DiagnDept = this.hisOuHosInfo1.Value.DiagnDept;
                if (lstOulInvoiceReg.Count > 0 && lstOulInvoiceReg[0].DoctorId == 0)
                {
                    lstOulInvoiceReg[0].DoctorId = info.DoctorId;
                    if (lstOulInvoiceReg[0].LocationId == Convert.ToInt32(BLL.Common.Utils.GetSystemSetting("CommenOuDiagLocId")))
                        lstOulInvoiceReg[0].LocationId = info.DiagnDept;
                    objOulInvoiceReg.Modify(lstOulInvoiceReg[0], trn);
                }
                info.F5 = string.Empty;
                objOuHosInfo.Modify(info, null);
                FrmOuHosInfo.ModiInvoiceDoctorId(info, trn);
                //保存结算发票
                _infoOuInvoice.InvoTime = BLL.Common.DateTimeHandler.GetServerDateTime();
                _infoOuInvoice.PatId = this.hisOuHosInfo1.Value.PatId;
                _infoOuInvoice.OperTime = BLL.Common.DateTimeHandler.GetServerDateTime();
                _infoOuInvoice.OperId = Model.Configuration.UserProfiles.UserID;
                _infoOuInvoice.HospitalId = Model.Configuration.UserProfiles.HospitalID;
                _infoOuInvoice.LocationId = this.hisOuHosInfo1.Value.DiagnDept;
                _infoOuInvoice.MzRegId = this.hisOuHosInfo1.Value.ID;
                _infoOuInvoice.PatTypeId = this.hisOuHosInfo1.Value.PatTypeId;
                if (Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsUpLoadYbFee")) && this.hisOuHosInfo1.Value.TallyGroupId == 21 && this.hisOuHosInfo1.Value.TallyTypeId == 83)//金保医保退费
                {
                    _infoOuInvoice.F7 = ybInterface != null ? (ybInterface.ReturnF3 != null ? ybInterface.ReturnF3.ToString() : string.Empty) : string.Empty;//将发票流水号保存到F7里面
                }
                if (_mzRegLastId == _infoOuInvoice.MzRegId && _invoLastId > 0) //如果该病人作废过发票
                {
                    _infoOuInvoice.InvoLastId = _invoLastId;
                    _invoLastId = 0;
                }
                int masterId = _infoOuInvoice.ID = _objOuInvoice.Create(_infoOuInvoice, trn);

                _lstUspOuInvoiceDtl.Fill("DoctorId", this.hisOuHosInfo1.Value.DoctorId);
                //更改发票对应的处方项目的收费标志
                SaveReicipeDtl(true, trn);
                if (_isDoubleCharged)   //过程异常
                {
                    trn.Rollback();
                    trn.Dispose();
                    BLL.Common.Utils.EndBeginCharge(this.hisOuHosInfo1.Value.ID.ToString());
                    this.InformationInMainForm = "该病人重复收费！您不能继续本次结算！请检查";
                    return false;
                }

                //保存发票明细
                Model.ModelList<Model.OuInvoiceDtlInfo> lstOuInvoiceDtl = _lstUspOuInvoiceDtl.ConvertTo<Model.OuInvoiceDtlInfo>();
                lstOuInvoiceDtl.Remove("ItemId", "0");
                lstOuInvoiceDtl.Fill("InvoId", masterId);
                lstOuInvoiceDtl.Fill("LsPerform", 1);//收费
                objOuInvoiceDtl.Save(lstOuInvoiceDtl, trn);

                //保存基本类别项目（分类的结算项目）
                lstFeetyItemSum.Fill("InvoId", masterId);
                objInvoiceFeetySum.Save(lstFeetyItemSum, trn);

                //保存发票分类项目
                lstInvItemSum.Fill("InvoId", masterId);
                objInvoiceInvItemSum.Save(lstInvItemSum, trn);                

                //保存收费记录
                lstOuInvoicePay.Fill("InvoId", masterId);
                objOuInvoicePay.Save(lstOuInvoicePay, trn);

                //对于非医生开处方的，补录入费用的费用明细对应的处方明细ID
                //WriteOuInvoiceDtlRecipeItemId(trn);

                //收费后更新处方的IsCharge标志
                WriteRecipeIsCharged(_infoOuInvoice.InvoNo, trn);
                if (ybInterface != null && ybInterface.InfoOuInvoice != null)
                {
                    ybInterface.InfoOuInvoice.ID = masterId;
                }
                if (this.hisOuHosInfo1.Value.IsYb && this.hisOuHosInfo1.Value.TallyGroupId == 28)
                {
                    string success = ybInterface.GetOuChargeResult().ToString();
                    if (!success.Contains("成功"))
                    {
                        MessageBox.Show(this, "您可能遇到医保网记账失败的错误,请联系管理员!");
                        trn.Rollback();
                        return false;
                    }
                }
                if (this.hisOuHosInfo1.Value.TallyGroupId == 21 && IsCancelMzBalance)
                {
                    ybMZCommon.MzBalance(infoOuHos, false, _lstUspOuInvoiceDtl);
                }
                if (this.hisOuHosInfo1.Value.TallyGroupId == 21 && ybMZCommon.RetMessages.Contains("-"))
                {
                    MessageBox.Show(string.Format("医保结算失败，原因【{0}】，请联系管理员或重新结算！", ybMZCommon.RetMessages), "提示");
                    trn.Rollback();
                    return false;
                }
                trn.Commit();
                //开始打印发票、处方
                if (Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOuChargeSavePrintRecie")))
                    GetPrinterRoom();
                if (!IsNotPrintInvoice && !PrintReport(masterId, GetPayWay(lstOuInvoicePay)))
                {
                    this.InformationInMainForm = "保存成功但打印发票失败！请重新打印发票";
                }
                led.ShowLed(_frmOuInvoicePay.ChargeBack, "08", "祝您健康");
                if (BLL.Common.Utils.CheckSettingContain("OuPrintTallySheetPatTypeIds", this.hisOuHosInfo1.Value.PatTypeId, false))
                {
                    PrintInsuranceTable(masterId);
                }
                if (!IsNotPrintInvoice && !this.CurrentBalanceNo.StartsWith("X"))
                {
                    _frmCurrInvo.CalculateNextInvoNo();
                    _frmCurrInvo.SaveInvoNo();
                }
                _frmOuInvoicePay.btnMerge.Visible = true;
                _frmOuInvoicePay.MzRegId = _infoOuInvoice.MzRegId;
                _frmOuInvoicePay.SetColumnIsEdit(true);
                PrintOuDrugIssue("All");  //即时打印医生电子处方（到药房）
                if (Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOuChargeSavePrintTestLabel")))
                {
                    PrintLab();
                    //objLoadThread = new System.Threading.Thread(new System.Threading.ThreadStart(this.PrintLab));
                    //objLoadThread.Start();
                }
                //PrintMZCrueDropReject(IsOuChargePrintMZCure, IsOuChargePrintMzDrop, IsOuChargePrintMzReject, true);
                PrintExecLocation();
                //if (!System.Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOuChargeOnePatMutiInvoice")) && _frmOuInvoicePay.txtChargeBack.Text != "0.00")
                //{
                //    _frmOuInvoicePay.ShowDialog();
                //    this.InformationInMainForm = string.Format("找回金额：{0}", _frmOuInvoicePay.txtChargeBack.Text);
                //}
                _frmOuInvoicePay.ShowDialog();
                //this.InformationInMainForm = string.Format("找回金额：{0}", _frmOuInvoicePay.txtChargeBack.Text);

                //是否收费后生成门诊护士执行单
                if (Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsChargeOuExecute")))
                {
                    GenOuExecute(this.hisOuHosInfo1.Value);
                }
                ResetAfterCancel();
                this.FormStatus = Model.Configuration.ToolbarStatus.View;
                _notReChargeDrugIssuedMzRegNo = string.Empty;
                //System.Threading.Thread.Sleep(10000);
                //_frmOuInvoicePay.Hide();
                //}
                //catch (Exception e)
                //{
                //trn.Rollback();
                //trn.Dispose();
                //    throw (e);
                //}
                _lock.UnLock();
                OperateOneOtherNotPayed(info.ID);
                if (!IsNotPrintInvoice)
                    this.CurrentBalanceNo = _frmCurrInvo.GetInvoiceNoFromConfigFile();
                return true;
            }
            catch (Exception e)
            {
                BLL.Common.Utils.EndBeginCharge(this.hisOuHosInfo1.Value.ID.ToString());
                Utilities.Information.ShowErrBox(e.ToString());
                return true;
            }
        }
        private void PrintExecLocation()
        {
            if (!Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOuChargeSavePrintExecLocation"))) return;

            BLL.Finder<Model.uspOuExecLocationQry> bllFinder = new BLL.Finder<Model.uspOuExecLocationQry>();
            Model.ModelList<Model.uspOuExecLocationQry> lstExecLocation;
            string rptName = "OuExecLocation.rpt";
            CrystalDecisions.CrystalReports.Engine.ReportDocument printExecLocation = Tools.Utils.GetRptFileByName(rptName);
            bllFinder.AddParameter("MzRegID", this.hisOuHosInfo1.Value.ID);
            bllFinder.AddParameter("IsCharged", true);
            lstExecLocation = bllFinder.Find("uspOuExecLocation");
            if (lstExecLocation.Count <= 0)
                return;
            BLL.Common.Utils.FillPatInfo<Model.uspOuExecLocationQry>(lstExecLocation);
            BLL.Common.Utils.FillIllDesc<Model.uspOuExecLocationQry>(lstExecLocation);
            printExecLocation.SetDataSource(lstExecLocation);
            Tools.Utils.SetHospitalTitle(printExecLocation);
            ((CrystalDecisions.CrystalReports.Engine.TextObject)printExecLocation.ReportDefinition.ReportObjects["TxtAmount"]).Text = string.Format("{0:N2}", lstExecLocation.GetSum("Amount"));
            printExecLocation.PrintOptions.PrinterName = BLL.Common.Utils.GetPrinterName(rptName);
            printExecLocation.PrintToPrinter(1, false, 0, 0);
            printExecLocation.Dispose();
        }
        Model.ModelList<Model.uspInExecuteBillPrintQry> lstMzCurePrint = new Model.ModelList<Model.uspInExecuteBillPrintQry>();
        Model.ModelList<Model.uspInExecuteBillPrintQry> lstMzDropPrint = new Model.ModelList<Model.uspInExecuteBillPrintQry>();
        Model.ModelList<Model.uspInExecuteBillPrintQry> lstMzRejectPrint = new Model.ModelList<Model.uspInExecuteBillPrintQry>();
        private void PrintMZCrueDropReject(bool IsMzCure, bool IsMzDrop, bool IsMzReject, bool firstPrint)
        {
            if (!IsMzCure && !IsMzDrop && !IsMzReject) return;
            lstMzCurePrint = new Model.ModelList<Model.uspInExecuteBillPrintQry>();
            lstMzDropPrint = new Model.ModelList<Model.uspInExecuteBillPrintQry>();
            lstMzRejectPrint = new Model.ModelList<Model.uspInExecuteBillPrintQry>();
            BLL.CBsUsage objBsUsage = new BLL.CBsUsage();
            Model.BsUsageInfo infoBsUsage = new Model.BsUsageInfo();
            Model.OuRecipeDtlInfo infoOuRecipeDtl = new Model.OuRecipeDtlInfo();
            BLL.COuRecipeDtl objOuRecipeDtl = new BLL.COuRecipeDtl();
            BLL.CBsUnitRatio objRatio = new BLL.CBsUnitRatio();
            BLL.CBsUnit objUnit = new BLL.CBsUnit();
            Model.uspInExecuteBillPrintQry infoExecuteBill = new Model.uspInExecuteBillPrintQry();
            Model.ModelList<Model.uspInExecuteBillPrintQry> lstExecuteBill = new Model.ModelList<Model.uspInExecuteBillPrintQry>();
            BLL.CBsFrequency objBsFrequency = new BLL.CBsFrequency();
            int GroupNum = 9000;
            foreach (Model.uspOuInvoiceDtlQry infoOuInvoiceDt in _lstUspOuInvoiceDtl)
            {
                if (!infoOuInvoiceDt.IsDoctorInput) continue;
                infoOuRecipeDtl = objOuRecipeDtl.GetByID(infoOuInvoiceDt.RecipeItemId);
                infoBsUsage = objBsUsage.GetByID(infoOuRecipeDtl.UsageId);
                infoExecuteBill = new Model.uspInExecuteBillPrintQry();
                infoExecuteBill.Age = this.hisOuHosInfo1.Value.Age;
                BLL.Common.Utils.FillPatAge(infoExecuteBill, this.hisOuHosInfo1.Value.PatId);
                infoExecuteBill.SexName = this.hisOuHosInfo1.Value.Sex;
                if (infoOuRecipeDtl.GroupNum > 0)
                    infoExecuteBill.GroupNum = infoOuRecipeDtl.GroupNum;
                else
                {
                    infoExecuteBill.GroupNum = GroupNum;
                    GroupNum++;
                }
                infoExecuteBill.PatientName = this.hisOuHosInfo1.Value.PatientName;
                if (Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOuExecutePrintSpec")))
                    infoExecuteBill.ItemName = string.Format("{0}({1})", infoOuInvoiceDt.Name, infoOuInvoiceDt.Spec);
                else
                    infoExecuteBill.ItemName = string.Format("{0}", infoOuInvoiceDt.Name);
                infoExecuteBill.Code = this.hisOuHosInfo1.Value.DoctorName;
                Model.ModelList<Model.BsUnitRatioInfo> lstUnitRatio = objRatio.BsUnitRatio_SelectByItemId(infoOuRecipeDtl.ItemId).Find("F1", "1");
                if (lstUnitRatio.Count > 0 && lstUnitRatio[0].UnitId1 == infoOuRecipeDtl.UnitTakeId)
                {
                    infoExecuteBill.Dosage = System.Math.Ceiling(infoOuRecipeDtl.Dosage * lstUnitRatio[0].DrugRatio).ToString();
                    infoExecuteBill.UnitTakeName = objUnit.GetByID(lstUnitRatio[0].UnitId2).PrintName;
                }
                else
                {
                    infoExecuteBill.Dosage = Utilities.Information.ConvertToDotString(infoOuRecipeDtl.Dosage);
                    infoExecuteBill.UnitTakeName = objUnit.GetByID(infoOuRecipeDtl.UnitTakeId).Name;
                }
                infoExecuteBill.FrequencyName = objBsFrequency.GetByID(infoOuRecipeDtl.FrequencyId).Name;
                infoExecuteBill.FrequencyId = infoOuRecipeDtl.FrequencyId;
                infoExecuteBill.Times = Convert.ToDouble(infoOuRecipeDtl.Days);
                infoExecuteBill.UsageName = infoBsUsage.Name;
                infoExecuteBill.OperTime = _infoOuInvoice.InvoTime;
                infoExecuteBill.DoctorInputTime = infoOuRecipeDtl.PrepareTime;
                infoExecuteBill.Memo = infoOuRecipeDtl.Memo;
                if (infoBsUsage.IsMzCure && IsMzCure) lstMzCurePrint.Add(infoExecuteBill);
                if (infoBsUsage.IsMzDrop && IsMzDrop && infoOuRecipeDtl.IsAttach) lstMzDropPrint.Add(infoExecuteBill);//infoOuRecipeDtl.IsAttach 在本院执行的就打注射单
                if (infoBsUsage.IsMzReject && IsMzReject) lstMzRejectPrint.Add(infoExecuteBill);
            }
            try
            {
                PrintClass(lstMzDropPrint, false, IsMzDrop, false, firstPrint);
                PrintClass(lstMzRejectPrint, false, false, IsMzReject, firstPrint);
                PrintClass(lstMzCurePrint, IsMzCure, false, false, firstPrint);
            }
            catch
            { }
        }

        private void PrintClass(Model.ModelList<Model.uspInExecuteBillPrintQry> lstInExecuteBillPrintQry, bool IsMzCure, bool IsMzDrop, bool IsMzReject, bool firstPrint)
        {
            if (lstInExecuteBillPrintQry.Count == 0) return;
            string rptName = string.Empty;
            CrystalDecisions.CrystalReports.Engine.ReportDocument print;
            if (IsMzCure) rptName = "OuTransfusionMZCrueA5";
            if (IsMzDrop || IsMzReject) rptName = "OuTransfusionBillA5";
            if (System.Convert.ToBoolean(BLL.Common.Utils.ReadLocalRegitFile("IsRemoteConnect")))
                Utilities.Information.ShowMsgBox(string.Format("报表名称：{0}", rptName));
            print = Tools.Utils.GetRptFileByName(rptName);
            Tools.Utils.SetHospitalTitle(print);
            if (!firstPrint)
                Tools.Utils.ReportDefineText(print, "TxtRePrint", "重打");
            Tools.Utils.ReportDefineText(print, "MzRegNo1", this.hisOuHosInfo1.Value.MzRegNo);
            Tools.Utils.ReportDefineText(print, "CardNo1", this.hisOuHosInfo1.Value.CardNo);
            if (IsMzDrop && IsOuMzDropPrintDays)
            {
                Model.ModelList<Model.ComputeSummary> lstComputeType = lstInExecuteBillPrintQry.GroupBy("Times", "Times", Model.ComputeType.Max);
                int Days = Convert.ToInt16(lstComputeType.GetDoubleMax("Result"));
                int day = 0;
                if (Days > 1 && !firstPrint)
                {
                    day = Tools.Utils.InputInt(string.Format("该病人的门诊注射卡一共有{0}天请输入从第几天开始打印", Days), "请输天数", day.ToString());
                    //if (day == 0) day = 1;
                }
                for (int i = day; i < Days; i++)
                {
                    DateTime dt = lstInExecuteBillPrintQry[0].OperTime;
                    Tools.Utils.ReportDefineText(print, "OperDay", dt.Date.AddDays(i).ToShortDateString());
                    Model.ModelList<Model.uspInExecuteBillPrintQry> lstPrintQry = lstInExecuteBillPrintQry.ConvertTo<Model.uspInExecuteBillPrintQry>();
                    FindDayLarge(lstPrintQry, i);
                    lstPrintQry.Sort("DoctorInputTime");
                    print.SetDataSource(lstPrintQry);
                    string PrinterName = BLL.Common.Utils.GetPrinterName(rptName);
                    if (PrinterName == string.Empty) PrinterName = string.Format("{0}_处方单", _recipePrinterName);
                    print.PrintOptions.PrinterName = PrinterName;
                    print.PrintToPrinter(1, false, 0, 0);
                }
                print.Dispose();
            }
            else
            {
                print.SetDataSource(lstInExecuteBillPrintQry);
                Tools.Utils.ReportDefineText(print, "OperDay", lstInExecuteBillPrintQry[0].OperTime.ToString());
                print.PrintOptions.PrinterName = BLL.Common.Utils.GetPrinterName(rptName);
                string PrinterName = BLL.Common.Utils.GetPrinterName(rptName);
                if (PrinterName == string.Empty) PrinterName = string.Format("{0}_处方单", _recipePrinterName);
                print.PrintOptions.PrinterName = PrinterName;
                print.PrintToPrinter(1, false, 0, 0);
                print.Dispose();
            }
        }
        private void FindDayLarge(Model.ModelList<Model.uspInExecuteBillPrintQry> TempInExecuteBill, int Days)
        {
            for (int i = 0; i < TempInExecuteBill.Count; i++)
            {
                if (Convert.ToInt32(TempInExecuteBill[i].Times) < Days)
                {
                    TempInExecuteBill.Remove(TempInExecuteBill[i]);
                    i--;
                }
            }
        }
        private System.Threading.Thread objLoadThread;
        private void PrintLab()
        {
            PrintLab(_infoOuInvoice.MzRegId);
        }
        public void PrintLab(int mzRegId)
        {
            CkForm.FrmSampleRecord frmSampleRecord = new CkForm.FrmSampleRecord();
            BLL.CCkLabDtl objCkLabDtl = new BLL.CCkLabDtl();
            BLL.CCkLab objCkLab = new BLL.CCkLab();
            frmSampleRecord.xtraTabControl1.SelectedTabPageIndex = 0;
            frmSampleRecord.hisOuHosInfo1.SearchManual = true;
            frmSampleRecord.hisOuHosInfo1.uoupCardNo.FindByHospID(mzRegId);
            frmSampleRecord.SaveModify();
        }
        void PrintInsuranceTable(int invoId)
        {
            if (this.hisOuHosInfo1.Value.IsYb && MessageBox.Show("该病人是医保病人，是否打印记帐单？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.OK)
            {
                XYHIS.Print.FrmInvoicePrint frmInvoice = new XYHIS.Print.FrmInvoicePrint();
                frmInvoice.Text = "打印发票";
                BLL.Finder<Model.uspOuInvoicePrintQry> objFindOuInvoicePrint = new BLL.Finder<Model.uspOuInvoicePrintQry>();
                objFindOuInvoicePrint.AddParameter("InvoId", invoId);
                Model.ModelList<Model.uspOuInvoicePrintQry> lstUspOuInvoicePrint = objFindOuInvoicePrint.Find("uspOuInvoicePrintTable");
                if (lstUspOuInvoicePrint.Count != 1) return;
                lstUspOuInvoicePrint[0].SequenceNum = new BLL.CBsPatient().GetByID(lstUspOuInvoicePrint[0].ID).IdCardNo;
                //PrintReport.Fee.OuInsuranceTable OuInsuranceTable = new PrintReport.Fee.OuInsuranceTable();
                string rptName = "OuInsuranceTable";
                CrystalDecisions.CrystalReports.Engine.ReportDocument OuInsuranceTable = Tools.Utils.GetRptFileByName(rptName);
                OuInsuranceTable.SetDataSource(lstUspOuInvoicePrint);
                if (Convert.ToBoolean(BLL.Common.Utils.ReadLocalRegitFile("IsPrintPreview").ToLower()))
                {
                    frmInvoice.ReportViewer.ReportSource = OuInsuranceTable;
                    frmInvoice.WindowState = System.Windows.Forms.FormWindowState.Normal;
                    if (frmInvoice.ShowDialog() != DialogResult.OK) return;
                }
                Tools.Utils.PrintToSetPrinter(OuInsuranceTable, rptName, 0, 0);
                OuInsuranceTable.Dispose();
            }
        }
        HuRmModule.FrmOuDrugIssue frmOuDrugIssue = null;
        private void PrintOuDrugIssue(string PrintType)
        {
            bool isRePrint = false;
            if (PrintType != "All")
                isRePrint = true;
            if (frmOuDrugIssue == null)
                frmOuDrugIssue = new HuRmModule.FrmOuDrugIssue();
            if (Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOuSendAndDosageSameMan")))
                frmOuDrugIssue.Text = "门诊配药窗口";
            frmOuDrugIssue.tmRefresh.Enabled = false;
            if (PrintType == "OuWestRecipe" || PrintType == "All")
                PrintOuDrugIssueToRoom(frmOuDrugIssue, "OuWestRoomId", _recipePrinterName, isRePrint);
            //string chinRoomWindowName = BLL.Common.Utils.GetSystemSetting("OuChargeChinRecipePrinter");
            //if (BLL.Common.Utils.ReadLocalRegitFile("RecipePrinterName") != string.Empty)
            //    chinRoomWindowName = BLL.Common.Utils.ReadLocalRegitFile("RecipePrinterName");
            //if (BLL.Common.Utils.GetSystemSetting("OuRecipePrintOnePrinterPatTypeIds").Contains(string.Format("[{0}]", this.hisOuHosInfo1.Value.PatTypeId)))
            //    chinRoomWindowName = BLL.Common.Utils.GetSystemSetting("OuRecipePrintOnePrinter");
            //PrintOuDrugIssueToRoom(frmOuDrugIssue, "OuChinRoomId", chinRoomWindowName);
            BLL.COuRecipe objOuRecipe = new BLL.COuRecipe();
            if (PrintType == "OuChinRecipe" || PrintType == "All")
            {
                PrintOuDrugIssueToRoom(frmOuDrugIssue, "OuChinRoomId", _recipeChinPrinterName, isRePrint);
                foreach (Model.OuRecipeInfo infoRecipe in _lstChineseRecipe)      //打印中药
                {
                    int TempId = 0;
                    string chinRoomWindowName = string.Empty;
                    if (_lstUspChineseRecipeDtl.Find("RecipeId", infoRecipe.ID.ToString()).Count == 0) continue;
                    try
                    {
                        if (PrintType == "All")
                        {
                            int roomId = GetIssueRoomId(2);
                            if (roomId == 0)
                                roomId = Convert.ToInt32(BLL.Common.Utils.GetSystemSetting("OuChinRoomId"));
                            frmOuDrugIssue.RoomId = roomId;
                            Model.ModelList<Model.OuRecipeInfo> lstOuRecipe = objOuRecipe.OuRecipe_SelectByRecipeNum(infoRecipe.RecipeNum);
                            if (lstOuRecipe.Count == 0) continue;
                            BLL.COuRecipeTemp objOuRecipeTemp = new BLL.COuRecipeTemp();
                            Model.OuRecipeTempInfo infoOuRecipeTemp = infoRecipe.ConvertTo<Model.OuRecipeTempInfo>();
                            infoOuRecipeTemp.Age = hisOuHosInfo1.Value.Age;
                            infoOuRecipeTemp.BabyMonth = hisOuHosInfo1.Value.BabyMonth;
                            if (Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOuDrugIssueByInvoice")))  //门诊发药按照发票号来新增
                                infoOuRecipeTemp.CardNo = this.CurrentBalanceNo;
                            else
                                infoOuRecipeTemp.CardNo = hisOuHosInfo1.Value.CardNo;
                            infoOuRecipeTemp.DoctorName = hisOuHosInfo1.Value.DoctorName;
                            if (Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOuChinDrugSentByCenterHospital")))
                                infoOuRecipeTemp.HospitalId = Convert.ToInt32(BLL.Common.Utils.GetSystemSetting("CenterHospitalId"));
                            else
                                infoOuRecipeTemp.HospitalId = Model.Configuration.UserProfiles.HospitalID;
                            infoOuRecipeTemp.ID = 0;
                            infoOuRecipeTemp.LocationName = hisOuHosInfo1.Value.LocationName;
                            infoOuRecipeTemp.MzRegNo = hisOuHosInfo1.Value.MzRegNo;
                            infoOuRecipeTemp.Name = hisOuHosInfo1.Value.PatientName;
                            infoOuRecipeTemp.RecipeId = lstOuRecipe[0].ID;
                            infoOuRecipeTemp.RegTime = hisOuHosInfo1.Value.RegTime;
                            infoOuRecipeTemp.RoomId = roomId;
                            infoOuRecipeTemp.RoomWindowName = _recipeChinPrinterName;
                            infoOuRecipeTemp.Sex = hisOuHosInfo1.Value.Sex;
                            infoOuRecipeTemp.F1 = hisOuHosInfo1.Value.PatTypeName;
                            infoOuRecipeTemp.F2 = "1";
                            infoOuRecipeTemp.F3 = string.Empty;
                            infoOuRecipeTemp.F4 = hisOuHosInfo1.Value.PatId.ToString();
                            objOuRecipeTemp.Create(infoOuRecipeTemp, null);
                        }
                        if (Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOuRecipeSavePrintRecie"))) continue;
                        if (!PatTypeIdNeedDoctorRecipePrint.Contains(string.Format("[{0}]", this.hisOuHosInfo1.Value.PatTypeId.ToString())))
                        {
                            if (!Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOuChargeSavePrintRecie"))) continue;
                            if (Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOuRoomAutoPrint"))) continue;
                        }
                        if (System.Convert.ToBoolean(BLL.Common.Utils.ReadLocalRegitFile("IsRemoteConnect")))
                            Utilities.Information.ShowMsgBox(_recipeChinPrinterName);
                        this.InformationInMainForm = string.Format("打印{0}病人的处方，位置{1}", this.hisOuHosInfo1.Value.PatientName, chinRoomWindowName);
                        frmOuDrugIssue.PrintChinRecipe(infoRecipe.RecipeNum, _recipeChinPrinterName, TempId, false, isRePrint);
                        if (BLL.Common.Utils.GetSystemSetting("OuRoomPrintRecipeOrLable") == "Label")//中药配药单
                            frmOuDrugIssue.PrintChinLabel(string.Format("{0}_Label", _recipeChinPrinterName), infoRecipe.ID);
                        //frmOuDrugIssue.PrintRoomIssueSheet();
                        Tools.Utils.TraceFunctionOperate(206);
                    }
                    catch (Exception ex)
                    {
                        //this.InformationInMainForm = string.Format("中药房{0}打印失败！原因：{1}", BLL.Common.Utils.GetSystemSetting("OuChargeChinRecipePrinter"), ex.Message.ToString());
                        this.InformationInMainForm = string.Format("中药房{0}打印失败！原因：{1}", _recipeChinPrinterName, ex.Message.ToString());
                        new Utilities.Document().SaveLog(string.Format(" 病人{3} 中药房{0} 处方打印失败：原因\r\n{1}{2}", BLL.Common.Utils.GetSystemSetting("OuChargeChinRecipePrinter"), ex.Message, ex.Source, this.hisOuHosInfo1.Value.PatientName), "Log.log");
                        Utilities.Document log = new Utilities.Document();
                        log.SaveLog(this.InformationInMainForm, "Log.log");
                    }
                }
            }
        }
        string IsOuDrugIssueByInvoice = Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOuDrugIssueByInvoice")) ? "1" : "0";
        string PatTypeIdNeedDoctorRecipePrint = BLL.Common.Utils.GetSystemSetting("PatTypeIdNeedDoctorRecipePrint");
        private void PrintOuDrugIssueToRoom(HuRmModule.FrmOuDrugIssue frmOuDrugIssue, string roomKey, string printer, bool isRePrint)
        {
            if ((_lstWestRecipeDtl == null || _lstWestRecipeDtl.Count == 0) && (_lstCheckRecipeDtl == null || _lstCheckRecipeDtl.Count == 0) && (_lstChineseRecipeDtl == null || _lstChineseRecipeDtl.Count == 0)) return;
            int roomId = 0;
            //if (roomKey != "OuChinRecipe")   //中药房应该用GetSystemSetting(roomKey))
            if (roomKey != "OuChinRoomId")   //中药房应该用GetSystemSetting(roomKey))
                roomId = GetIssueRoomId(1);
            if (roomId == 0)
                roomId = Convert.ToInt32(BLL.Common.Utils.GetSystemSetting(roomKey));

            frmOuDrugIssue.RoomId = roomId;
            frmOuDrugIssue.LstWestRecipeDtl = _lstWestRecipeDtl + _lstCheckRecipeDtl + _lstChineseRecipeDtl;
            BLL.COuClincDiag objOuClincDiag = new BLL.COuClincDiag();
            BLL.COuRecipeTemp objOuRecipeTemp = new BLL.COuRecipeTemp();
            Model.ModelList<Model.uspOuDrugIssueQry> lstOuDrugIssue = frmOuDrugIssue.FindOuDrugIssue(this.hisOuHosInfo1.Value.ID);
            if (System.Convert.ToBoolean(BLL.Common.Utils.ReadLocalRegitFile("IsRemoteConnect")))
                Utilities.Information.ShowMsgBox(string.Format("{0}条记录", lstOuDrugIssue.Count));
            if (lstOuDrugIssue.Count > 0)    //打印到药房
            {
                int TempId = 0;
                string illDesc = Utilities.Information.FilterArrayToString(objOuClincDiag.OuClincDiag_SelectByMzRegId(this.hisOuHosInfo1.Value.ID).CopyTo("IllDesc"));
                //Model.ModelList<Model.OuRecipeTempInfo> lstOuRecipeTemp = objOuRecipeTemp.OuRecipeTemp_SelectByRecipeId(frmOuDrugIssue.LstWestRecipeDtl[0].RecipeId);
                for (int i = 0; i < lstOuDrugIssue.Count; i++)
                {
                    if (frmOuDrugIssue.LstWestRecipeDtl.Find("RecipeId", lstOuDrugIssue[i].ID.ToString()).Count == 0) continue;
                    Model.ModelList<Model.OuRecipeTempInfo> lstOuRecipeTemp = objOuRecipeTemp.OuRecipeTemp_SelectByRecipeId(lstOuDrugIssue[i].ID);
                    if (Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOuDrugIssueByInvoice")) && lstOuRecipeTemp.Find("CardNo", this.CurrentBalanceNo).Count == 0)  //门诊发药按照发票号来新增
                    {
                        //Model.OuRecipeTempInfo infoOuRecipeTemp = lstOuDrugIssue[0].ConvertTo<Model.OuRecipeTempInfo>();
                        //infoOuRecipeTemp.RecipeId = lstOuDrugIssue[0].ID;
                        Model.OuRecipeTempInfo infoOuRecipeTemp = lstOuDrugIssue[i].ConvertTo<Model.OuRecipeTempInfo>();
                        infoOuRecipeTemp.RecipeId = lstOuDrugIssue[i].ID;
                        infoOuRecipeTemp.CardNo = this.CurrentBalanceNo;
                        infoOuRecipeTemp.RoomWindowName = printer;
                        infoOuRecipeTemp.RoomId = roomId;
                        infoOuRecipeTemp.IllDesc = illDesc;
                        infoOuRecipeTemp.F1 = hisOuHosInfo1.Value.PatTypeName;
                        infoOuRecipeTemp.F2 = IsOuDrugIssueByInvoice;
                        infoOuRecipeTemp.F3 = string.Empty;
                        infoOuRecipeTemp.F4 = hisOuHosInfo1.Value.PatId.ToString();
                        objOuRecipeTemp.Create(infoOuRecipeTemp, null);
                    }
                    else
                    {
                        lstOuRecipeTemp = lstOuRecipeTemp.Find("RoomId", roomId.ToString());
                        if (lstOuRecipeTemp.Find("IsIssue", "True").Count > 0)  //已发的变成未发状态，不新增
                        {
                            lstOuRecipeTemp[0].IsIssue = false;
                            lstOuRecipeTemp[0].Memo += "[部分发]";
                            lstOuRecipeTemp[0].F2 = IsOuDrugIssueByInvoice;
                            lstOuRecipeTemp[0].F3 = string.Empty;
                            lstOuRecipeTemp[0].F4 = hisOuHosInfo1.Value.PatId.ToString();
                            lstOuRecipeTemp[0].RecipeTime = BLL.Common.DateTimeHandler.GetServerDateTime();
                            objOuRecipeTemp.Modify(lstOuRecipeTemp[0], null);
                        }
                        else if (lstOuRecipeTemp.Find("IsIssue", "False").Count == 0)  //没有待发药记录，则新增
                        {
                            //Model.OuRecipeTempInfo infoOuRecipeTemp = lstOuDrugIssue[0].ConvertTo<Model.OuRecipeTempInfo>();
                            //infoOuRecipeTemp.RecipeId = lstOuDrugIssue[0].ID;
                            Model.OuRecipeTempInfo infoOuRecipeTemp = lstOuDrugIssue[i].ConvertTo<Model.OuRecipeTempInfo>();
                            if (Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOuDrugIssueByInvoice")))
                            {
                                infoOuRecipeTemp.CardNo = this.CurrentBalanceNo;
                            }
                            infoOuRecipeTemp.RecipeId = lstOuDrugIssue[i].ID;
                            infoOuRecipeTemp.RoomWindowName = printer;
                            infoOuRecipeTemp.RoomId = roomId;
                            infoOuRecipeTemp.IllDesc = illDesc;
                            infoOuRecipeTemp.F1 = hisOuHosInfo1.Value.PatTypeName;
                            infoOuRecipeTemp.F2 = IsOuDrugIssueByInvoice;
                            infoOuRecipeTemp.F3 = string.Empty;
                            infoOuRecipeTemp.F4 = hisOuHosInfo1.Value.PatId.ToString();
                            objOuRecipeTemp.Create(infoOuRecipeTemp, null);
                        }
                        else if (lstOuRecipeTemp.Count > 0 && (lstOuRecipeTemp[0].F3 == "1" || lstOuRecipeTemp[0].IsBack) && localRecipePrinterName == string.Empty)
                        {
                            if (lstOuRecipeTemp[0].IsIssue)
                                lstOuRecipeTemp[0].Memo += "[部分发]";
                            lstOuRecipeTemp[0].IsIssue = false;
                            lstOuRecipeTemp[0].F2 = IsOuDrugIssueByInvoice;
                            lstOuRecipeTemp[0].F3 = string.Empty;
                            lstOuRecipeTemp[0].F4 = hisOuHosInfo1.Value.PatId.ToString();
                            lstOuRecipeTemp[0].RecipeTime = BLL.Common.DateTimeHandler.GetServerDateTime();
                            objOuRecipeTemp.Modify(lstOuRecipeTemp[0], null);
                        }
                        Model.ModelList<Model.OuRecipeTempInfo> TempOther = objOuRecipeTemp.OuRecipeTemp_SelectByRecipeId(lstOuDrugIssue[i].ID).Find("RoomId", roomId.ToString());
                        //Model.ModelList<Model.OuRecipeTempInfo> TempOther = objOuRecipeTemp.OuRecipeTemp_SelectByRecipeId(lstOuDrugIssue[0].ID).Find("RoomId", roomId.ToString());
                        if (TempOther.Count == 0)  //不成功再创建
                        {
                            //Model.OuRecipeTempInfo infoOuRecipeTemp = lstOuDrugIssue[0].ConvertTo<Model.OuRecipeTempInfo>();
                            //infoOuRecipeTemp.RecipeId = lstOuDrugIssue[0].ID;
                            Model.OuRecipeTempInfo infoOuRecipeTemp = lstOuDrugIssue[i].ConvertTo<Model.OuRecipeTempInfo>();
                            infoOuRecipeTemp.RecipeId = lstOuDrugIssue[i].ID;
                            infoOuRecipeTemp.RoomWindowName = printer;
                            infoOuRecipeTemp.RoomId = roomId;
                            infoOuRecipeTemp.IllDesc = illDesc;
                            infoOuRecipeTemp.F1 = hisOuHosInfo1.Value.PatTypeName;
                            infoOuRecipeTemp.F2 = "0";
                            infoOuRecipeTemp.F3 = string.Empty;
                            infoOuRecipeTemp.F4 = hisOuHosInfo1.Value.PatId.ToString();
                            objOuRecipeTemp.Create(infoOuRecipeTemp, null);
                        }
                    }
                }
                if (Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOuRecipeSavePrintRecie"))) return;
                if (!PatTypeIdNeedDoctorRecipePrint.Contains(string.Format("[{0}]", this.hisOuHosInfo1.Value.PatTypeId.ToString())))
                {
                    if (!Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOuChargeSavePrintRecie"))) return;
                    if (Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOuDrugIssueByInvoice"))) return;
                    if (Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOuRoomAutoPrint"))) return;
                }
                frmOuDrugIssue.BandDataToGridDtl(lstOuDrugIssue[0]);
                try
                {
                    if (System.Convert.ToBoolean(BLL.Common.Utils.ReadLocalRegitFile("IsRemoteConnect")))
                        Utilities.Information.ShowMsgBox(string.Format("{0}_处方单", printer));
                    //this.InformationInMainForm = string.Format("准备完毕，打印{0}病人的处方，窗口{1}", this.hisOuHosInfo1.Value.PatientName, printer);
                    frmOuDrugIssue.PrintRecipe(string.Format("{0}_处方单", printer), TempId, false, isRePrint);
                    if (roomKey == "OuWestRoomId" && !IsOuChargePrintMzDrop && !IsOuChargePrintMzReject)
                        PrintNurseReject(lstOuDrugIssue[0].ID, string.Format("{0}_处方单", printer));
                    if (System.Convert.ToBoolean(BLL.Common.Utils.ReadLocalRegitFile("IsRemoteConnect")))
                        Utilities.Information.ShowMsgBox(string.Format("{0}_Label", printer));
                    if (BLL.Common.Utils.GetSystemSetting("OuRoomPrintRecipeOrLable") == "Label")
                        frmOuDrugIssue.PrintLabel(string.Format("{0}_Label", printer));
                    //if (Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsChargePrintRoomIssueSheet")))
                    //    frmOuDrugIssue.PrintRoomIssueSheet(string.Format("{0}_配药单", printer));
                    Tools.Utils.TraceFunctionOperate(206);
                }
                catch (Exception ex)
                {
                    this.InformationInMainForm = string.Format("准备完毕，药房{0}打印机未连接！原因：{1}", printer, ex.Message.ToString());
                    new Utilities.Document().SaveLog(string.Format("病人 {3} 药房{0} 处方打印失败：原因\r\n{1}{2}", printer, ex.Message, ex.Source, this.hisOuHosInfo1.Value.PatientName), "Log.log");
                    Utilities.Document log = new Utilities.Document();
                    log.SaveLog(this.InformationInMainForm, "Log.log");
                }
            }
            else if (frmOuDrugIssue.LstWestRecipeDtl.Count > 0 && roomKey == "OuWestRoomId")
            {
                //this.InformationInMainForm = string.Format("没有生成药房申请，病人ID：{0}", this.hisOuHosInfo1.Value.ID);
                //药房发部分然后收费处又退费之后
                Model.ModelList<Model.ComputeSummary> lstComputeSummary = frmOuDrugIssue.LstWestRecipeDtl.GroupBy("RecipeId", "RecipeId", Model.ComputeType.Max);
                foreach (Model.ComputeSummary info in lstComputeSummary)
                {
                    BLL.Finder<Model.OuRecipeDtlInfo> finder = new BLL.Finder<Model.OuRecipeDtlInfo>();
                    finder.AddParameter("@RoomId", roomId);
                    finder.AddParameter("@RecipeId", Convert.ToInt32(info.GroupBy));
                    Model.ModelList<Model.OuRecipeDtlInfo> lstOuRecipeDtlIssue = finder.Find("UspOuRecipeDtlIsIssue");
                    if (lstOuRecipeDtlIssue.Count == 0)
                    {
                        Model.ModelList<Model.OuRecipeTempInfo> lstOuRecipeTemp = objOuRecipeTemp.OuRecipeTemp_SelectByRecipeId(Convert.ToInt32(info.GroupBy));
                        lstOuRecipeTemp.Fill("IsIssue", "True");
                        objOuRecipeTemp.Save(lstOuRecipeTemp, null);
                    }
                }
                //BLL.CTrace bllTrace = new BLL.CTrace();
                //Model.TraceInfo tInfo = new Model.TraceInfo();
                //tInfo.UserID = Model.Configuration.UserProfiles.UserID;
                //tInfo.CreateDate = BLL.Common.DateTimeHandler.GetServerDateTime();
                //tInfo.Messages = string.Format("处方项目[{0}]没有生成OuRecipeTemp，病人ID：{1}", Utilities.Information.FilterArrayToString(_lstWestRecipeDtl.CopyTo("ItemId")), this.hisOuHosInfo1.Value.ID);
                //tInfo.OperateType = "Insert";
                //tInfo.Type = "OuRecipeTemp";
                //bllTrace.Create(tInfo, null);
            }
        }
        private void PrintNurseReject(int recipeId, string printer)
        {
            if (!System.Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsRoomPrintNurseReject"))) return;
            //XYHIS.FrmOuExecute frm = new FrmOuExecute();
            //frm.PrintNurseReject(true, true, recipeId, this.hisOuHosInfo1.Value, "注射卡", printer,null);
            bool IsPrintDays = Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsPrintNurseRejectDays"));
            Tools.Print.PrintNurseReject(true, true, recipeId, this.hisOuHosInfo1.Value, "注射卡", printer, null, IsPrintDays, _lstWestRecipeDtl);
        }
        private void PrintRoomCancel(string cancelMemo)
        {
            if (!Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOuChargeSavePrintRecie"))) return;
            GetPrinterRoom();
            //PrintReport.Diagnose.OuChargeCancelRoom printRecipe = new PrintReport.Diagnose.OuChargeCancelRoom();
            string rptName = "OuChargeCancelRoom";
            CrystalDecisions.CrystalReports.Engine.ReportDocument printRecipe = Tools.Utils.GetRptFileByName(rptName);
            string content = string.Format("{2}请注意：门诊号[{0}]姓名[{1}]的病人已经退费！", this.hisOuHosInfo1.Value.MzRegNo, this.hisOuHosInfo1.Value.PatientName, _roomWindowName);

            if (CheckHasRoomIssueDrug(Convert.ToInt32(OuWestRoomId)))
            {
                Tools.Utils.ReportDefineText(printRecipe, "txtContent", content);
                if (System.Convert.ToBoolean(BLL.Common.Utils.ReadLocalRegitFile("IsRemoteConnect")))
                    Utilities.Information.ShowMsgBox(string.Format("{0}_处方单", _recipePrinterName));
                printRecipe.PrintOptions.PrinterName = string.Format("{0}_处方单", _recipePrinterName);
                printRecipe.PrintToPrinter(1, false, 0, 0);
            }
            if (CheckHasRoomIssueDrug(Convert.ToInt32(BLL.Common.Utils.GetSystemSetting("OuChinRoomId"))))
            {
                printRecipe.PrintOptions.PrinterName = BLL.Common.Utils.GetSystemSetting("OuChargeChinRecipePrinter");
                printRecipe.PrintToPrinter(1, false, 0, 0);
            }
            printRecipe.Dispose();
        }

        //private void CheckWindowWorking()
        //{
        //    int Start = _recipePrinterName.IndexOf("\\\\");
        //    if (Start < 0) return;
        //    string strRight = _recipePrinterName.Substring(Start+2);
        //    int End = strRight.IndexOf("\\");
        //    string IP = strRight.Substring(0, End - Start);
        //    if(!Tools.Utils.CheckIpExists(IP))
        //    {
        //        _recipePrinterName = _recipeInitPrinterName;
        //        _roomWindowName = _roomInitWindowName;
        //    }++
        //}

        private string CheckWindowWorking(string ToPrintName)
        {
            int Start = ToPrintName.IndexOf("\\\\");
            if (Start < 0) return ToPrintName;
            string RetrunPrint = ToPrintName;
            string strRight = ToPrintName.Substring(Start + 2);
            int End = strRight.IndexOf("\\");
            string IP = strRight.Substring(0, End - Start);
            if (!Tools.Utils.CheckIpExists(IP))
            {
                RetrunPrint = string.Empty;
            }
            return RetrunPrint;
        }
        string _recipePrinterName = string.Empty;
        string _recipeChinPrinterName = string.Empty;
        string _roomWindowName = "药房";
        string _recipeInitPrinterName = BLL.Common.Utils.GetSystemSetting("OuChargeWestRecipePrinter");//默认西药打印机
        string _recipeInitChinPrinterName = BLL.Common.Utils.GetSystemSetting("OuChargeChinRecipePrinter");//默认中药打印机
        string _roomInitWindowName = BLL.Common.Utils.GetSystemSetting("OuChargeWestRecipeRoomWindowName");
        string localRecipePrinterName = BLL.Common.Utils.ReadLocalRegitFile("RecipePrinterName");
        private void GetPrinterRoom()
        {
            string activeWindowName = string.Empty;
            Model.ModelList<Model.BsLocationRoomWindowInfo> lstLocationRoomWindow = new Model.ModelList<Model.BsLocationRoomWindowInfo>();
            DayOfWeek currentDayOfWeek = BLL.Common.DateTimeHandler.GetServerDateTime().DayOfWeek;
            double currentHour = BLL.Common.DateTimeHandler.GetServerDateTime().TimeOfDay.TotalHours;
            BLL.CBsLocationRoomWindow objLocationRoomWindow = new BLL.CBsLocationRoomWindow();
            lstLocationRoomWindow = objLocationRoomWindow.BsLocationRoomWindow_SelectByLocationId(this.hisOuHosInfo1.Value.DiagnDept);
            if ((!Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOuRoomWindowLimitWorkTime")) || (currentDayOfWeek != DayOfWeek.Sunday && currentDayOfWeek != DayOfWeek.Saturday && currentHour > Convert.ToDouble(BLL.Common.Utils.GetSystemSetting("WorkBeginMorning")) && currentHour < Convert.ToDouble(BLL.Common.Utils.GetSystemSetting("WorkEndMorning")))
                || (currentDayOfWeek != DayOfWeek.Sunday && currentDayOfWeek != DayOfWeek.Saturday && currentHour > Convert.ToDouble(BLL.Common.Utils.GetSystemSetting("WorkBeginAfternoon")) && currentHour < Convert.ToDouble(BLL.Common.Utils.GetSystemSetting("WorkEndAfternoon")))
                || (currentDayOfWeek == DayOfWeek.Saturday && Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOuChargeSavePrintRecie")) && currentHour > Convert.ToDouble(BLL.Common.Utils.GetSystemSetting("WorkBeginMorning")) && currentHour < Convert.ToDouble(BLL.Common.Utils.GetSystemSetting("WorkEndMorning"))))
                && (lstLocationRoomWindow.Count > 0 && lstLocationRoomWindow[0].PrinterName.Trim() != string.Empty && lstLocationRoomWindow[0].RoomWindowName.Trim() != string.Empty))
            {
                _recipePrinterName = lstLocationRoomWindow[0].PrinterName;
                _recipeChinPrinterName = _recipeInitChinPrinterName;
                _roomWindowName = lstLocationRoomWindow[0].RoomWindowName;
            }
            else
            {
                if (Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsAutoSetRoomWindowName")))
                {

                    BLL.CBsRoomWindow objBsRoomWindow = new BLL.CBsRoomWindow();
                    Model.ModelList<Model.BsRoomWindowInfo> lstBsRoomWindow = objBsRoomWindow.GetAllActive();
                    BLL.COuRecipeTemp objTemp = new BLL.COuRecipeTemp();

                    Model.ModelList<Model.OuRecipeTempInfo> lstTmp = objTemp.GetDynamic(string.Format(" (DATEDIFF(d, RecipeTime, GETDATE()) = 0) and CardNo ='{0}' and  RoomId={1} ", this.hisOuHosInfo1.Value.CardNo, OuWestRoomId), "ID");
                    if (lstTmp.Count > 0 && lstBsRoomWindow.Find("Code", lstTmp[0].RoomWindowName).Count > 0)
                    {
                        _recipePrinterName = lstTmp[0].RoomWindowName;
                        _roomWindowName = lstBsRoomWindow.Find("Code", _recipePrinterName)[0].Name;
                    }
                    else
                    {
                        foreach (Model.BsRoomWindowInfo infoBsRoomWindow in lstBsRoomWindow)
                        {
                            BLL.Common.LockKey lockKey = new BLL.Common.LockKey(Model.EnumLockType.RoomWindow);
                            if (lockKey.CheckLocked(infoBsRoomWindow.ID) == "别人锁")
                                activeWindowName += string.Format(",'{0}'", infoBsRoomWindow.Code);
                        }
                        if (activeWindowName != string.Empty)
                        {
                            activeWindowName = string.Format("RoomWindowName in ({0}) and ID>{1}", activeWindowName.Substring(1), lstBsRoomWindow.GetIntMax("F1"));
                            object objInfo = BLL.absBusiness<Model.absModel>.ExecuteScalar("uspMaxNumRoomWindowName", activeWindowName, null);
                            if (objInfo != null)
                                _recipePrinterName = objInfo.ToString();
                            if (_recipePrinterName == string.Empty && lstBsRoomWindow.Find("RoomId", OuWestRoomId).Count > 0)
                            {
                                _recipePrinterName = lstBsRoomWindow.Find("RoomId", OuWestRoomId)[0].Code;
                            }
                            if (lstBsRoomWindow.Find("Code", _recipePrinterName).Count > 0)
                            {
                                _roomWindowName = lstBsRoomWindow.Find("Code", _recipePrinterName)[0].Name;
                            }
                        }
                    }
                }
            }
            string OuRecipePrintOnePrinterPatTypeIds = BLL.Common.Utils.GetSystemSetting("OuRecipePrintOnePrinterPatTypeIds");
            string OuRecipePrintModeWhenNotSpecPatType = BLL.Common.Utils.GetSystemSetting("OuRecipePrintModeWhenNotSpecPatType");
            string OuRecipePrintOnePrinter = BLL.Common.Utils.GetSystemSetting("OuRecipePrintOnePrinter");
            if (OuRecipePrintOnePrinterPatTypeIds.Contains(string.Format("[{0}]", this.hisOuHosInfo1.Value.PatTypeId)) &&
                (!Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOuChargeNoTallyNoAsSelfPatType")) || this.utxtTallyNo.Text != string.Empty))
            {
                _recipeChinPrinterName = _recipePrinterName = OuRecipePrintOnePrinter;
                _roomInitWindowName = _roomWindowName = "药房";
            }
            else if (("1,2").Contains(OuRecipePrintModeWhenNotSpecPatType))
            {
                if (OuRecipePrintModeWhenNotSpecPatType == "1")
                {
                    if (localRecipePrinterName != string.Empty)
                    {
                        _recipeChinPrinterName = _recipePrinterName = localRecipePrinterName;

                    }
                    else
                    {
                        if (_recipePrinterName == string.Empty)
                            _recipePrinterName = _recipeInitPrinterName;
                        _recipeChinPrinterName = _recipeInitChinPrinterName;
                    }
                    // _roomInitWindowName = _roomWindowName = "药房";
                }
            }
            else
            {
                _recipePrinterName = _recipeInitPrinterName;
                _recipeChinPrinterName = _recipeInitChinPrinterName;
                _roomWindowName = _roomInitWindowName;
            }
            //_recipePrinterName = CheckWindowWorking(_recipePrinterName);
            //_recipeChinPrinterName = CheckWindowWorking(_recipeChinPrinterName);
            if (_recipePrinterName == string.Empty) _roomInitWindowName = _roomWindowName = "药房";
            if (System.Convert.ToBoolean(BLL.Common.Utils.ReadLocalRegitFile("IsRemoteConnect")))
                Utilities.Information.ShowMsgBox(string.Format("条件判断：\n\r1、GblSetting的对应病人类别={0}\n\r2、GblSetting的OuRecipePrintModeWhenNotSpecPatType={1}\n\r3、LocalRegitFile的RecipePrinterName=“{2}”\n\r4、{3}科室设置药房对应“{4}”\n\r5、GblSetting的西药房默认打印机={5}\n\r6、GblSetting的中药房默认打印机={6}\n\r7、西药房目标打印机[{9}_处方单]状态：{7}\n\r8、西药房目标打印机[{10}_处方单]状态：{8}",
                    OuRecipePrintOnePrinterPatTypeIds, OuRecipePrintModeWhenNotSpecPatType, localRecipePrinterName, this.hisOuHosInfo1.Value.LocationName, lstLocationRoomWindow.Count > 0 ? lstLocationRoomWindow[0].RoomWindowName : string.Empty, _recipeInitPrinterName, _recipeInitChinPrinterName, _recipePrinterName == string.Empty ? "不通" : "通", _recipeChinPrinterName == string.Empty ? "不通" : "通", _recipePrinterName, _recipeChinPrinterName));
        }
        //private void GetPrinterRoom()
        //{
        //    if (BLL.Common.Utils.ReadLocalRegitFile("RecipePrinterName") != string.Empty)
        //    {
        //        _recipeInitPrinterName = _recipePrinterName = BLL.Common.Utils.ReadLocalRegitFile("RecipePrinterName");
        //        _roomInitWindowName = _roomWindowName = "药房";
        //    }
        //    DayOfWeek currentDayOfWeek = BLL.Common.DateTimeHandler.GetServerDateTime().DayOfWeek;
        //    double currentHour = BLL.Common.DateTimeHandler.GetServerDateTime().TimeOfDay.TotalHours;
        //    BLL.CBsLocationRoomWindow objLocationRoomWindow = new BLL.CBsLocationRoomWindow();
        //    Model.ModelList<Model.BsLocationRoomWindowInfo> lstLocationRoomWindow = objLocationRoomWindow.BsLocationRoomWindow_SelectByLocationId(this.hisOuHosInfo1.Value.DiagnDept);
        //    if ((!Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOuRoomWindowLimitWorkTime")) || (currentDayOfWeek != DayOfWeek.Sunday && currentDayOfWeek != DayOfWeek.Saturday && currentHour > Convert.ToDouble(BLL.Common.Utils.GetSystemSetting("WorkBeginMorning")) && currentHour < Convert.ToDouble(BLL.Common.Utils.GetSystemSetting("WorkEndMorning")))
        //        || (currentDayOfWeek != DayOfWeek.Sunday && currentDayOfWeek != DayOfWeek.Saturday && currentHour > Convert.ToDouble(BLL.Common.Utils.GetSystemSetting("WorkBeginAfternoon")) && currentHour < Convert.ToDouble(BLL.Common.Utils.GetSystemSetting("WorkEndAfternoon")))
        //        || (currentDayOfWeek == DayOfWeek.Saturday && Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOuChargeSavePrintRecie")) && currentHour > Convert.ToDouble(BLL.Common.Utils.GetSystemSetting("WorkBeginMorning")) && currentHour < Convert.ToDouble(BLL.Common.Utils.GetSystemSetting("WorkEndMorning"))))
        //        && (lstLocationRoomWindow.Count > 0 && lstLocationRoomWindow[0].PrinterName.Trim() != string.Empty && lstLocationRoomWindow[0].RoomWindowName.Trim() != string.Empty))
        //    {
        //        _recipePrinterName = lstLocationRoomWindow[0].PrinterName;
        //        _roomWindowName = lstLocationRoomWindow[0].RoomWindowName;
        //    }
        //    else
        //    {
        //        _recipePrinterName = _recipeInitPrinterName;
        //        _roomWindowName = _roomInitWindowName;
        //    }
        //    CheckWindowWorking();
        //}
        private void WriteOuInvoiceDtlRecipeItemId(System.Data.Common.DbTransaction trn)
        {
            DAL.SqlUtil db = new DAL.SqlUtil();
            db.Transaction = trn;
            //db.AddParameter("MzRegId", this.hisOuHosInfo1.Value.ID);
            //db.ExecuteScalar("uspWriteOuInvoiceDtlRecipeItemId");
            db.AddParameter("MzRegId", this.hisOuHosInfo1.Value.ID);
            db.ExecuteScalar("uspWriteRecipeTotalityFromOuInvoiceDtl");
        }
        private void WriteRecipeIsCharged(string invoNo, System.Data.Common.DbTransaction trn)
        {
            if (invoNo == string.Empty)
            {
                this.InformationInMainForm = "发票收费标志更新失败！可能会引起药房不能接收到该处方，请通知系统管理员处理！";
                return;
            }
            DAL.SqlUtil db = new DAL.SqlUtil();
            db.Transaction = trn;
            db.AddParameter("InvoNo", invoNo);
            db.ExecuteScalar("uspWriteRecipeIsChargedFromOuInvoiceDtl");

            int lastPsApplyId = 0;
            foreach (Model.uspOuRecipeDtlForOuInvoiceDtlQry info in _lstUspOuRecipeDtlForOuInvoiceDtl)
            {
                BLL.CPsApplyReport objPsApply = new BLL.CPsApplyReport();
                Model.PsApplyReportInfo infoPsApply;
                if (Utilities.Information.IsNumeric(info.F3) && info.F3 != "0" && lastPsApplyId != Convert.ToInt32(info.F3))
                {
                    infoPsApply = objPsApply.GetByID(Convert.ToInt32(info.F3));
                    infoPsApply.IsCharged = true;
                    objPsApply.Modify(infoPsApply, trn);
                    lastPsApplyId = Convert.ToInt32(info.F3);
                }
            }
        }

        private void OperateOneOtherNotPayed(int patId)
        {
            string where = string.Format("DiagnDept > 0 and OperTime > '{0}' and OperTime < '{1}' and patId={2}", BLL.Common.DateTimeHandler.GetServerDateTime().Date, BLL.Common.DateTimeHandler.GetServerDateTime(), patId);
            Model.ModelList<Model.OuHosInfoInfo> lstOtherNotPayOuHosInfo = new BLL.COuHosInfo().GetDynamic(where, null);
            if (lstOtherNotPayOuHosInfo.Count > 0)
            {
                foreach (Model.OuHosInfoInfo infoOuHosInfo in lstOtherNotPayOuHosInfo)
                {
                    if (_objOuRecipe.OuRecipe_SelectByMzRegId(infoOuHosInfo.ID).Count == 0)
                        continue;
                    if (_objOuInvoice.OuInvoice_SelectByMzRegId(infoOuHosInfo.ID).Count > 0)
                        continue;
                    if (MessageBox.Show("该病人有已就诊但未结算的项目，是否提取并结算？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        this.hisOuHosInfo1.SearchPatient(infoOuHosInfo.ID);
                        if (!_lock.Lock(infoOuHosInfo.ID)) return;

                        OpenRecord();
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 从收费明细中生成处方
        /// </summary>
        private void GenerateRecipeFromInvoiceDtl()
        {
            _lstChineseRecipeDtl = _lstUspChineseRecipeDtl.ConvertTo<Model.OuRecipeDtlInfo>();
            _lstCheckRecipeDtl = new Model.ModelList<Model.OuRecipeDtlInfo>();
            _lstWestRecipeDtl = new Model.ModelList<Model.OuRecipeDtlInfo>();
            BLL.COuRecipeDtl objOuRecipeDtl = new BLL.COuRecipeDtl();
            Model.ModelList<Model.uspOuInvoiceDtlQry> lst = _lstUspOuInvoiceDtl.FilterInclude("ItemId", IntegralItemId, true);
            foreach (Model.uspOuInvoiceDtlQry info in lst)
            {
                if (BLL.Common.Utils.CheckSettingContain("OuChargeInputPriceItemIds", info.ItemId, false))
                    info.Totality = info.Price;
                if (info.RecipeItemId == 0)    //表示用户手工新增的
                {
                    Model.OuRecipeDtlInfo newInfo = info.ConvertTo<Model.OuRecipeDtlInfo>();
                    newInfo.UnitDiagId = info.UnitId;
                    //if (info.LsRpType == 3)
                    //    _lstChineseRecipeDtl.Add(newInfo);

                    //else 
                    if (info.LsRpType == 1 || info.LsRpType == 2)
                        _lstWestRecipeDtl.Add(newInfo);
                    else if (info.LsRpType != 3)
                        _lstCheckRecipeDtl.Add(newInfo);
                    Tools.Utils.TraceFunctionOperate(268);
                }
                else    //表示从处方表导入的
                {
                    //if (info.LsRpType == 3)
                    //_lstChineseRecipeDtl.Add(objOuRecipeDtl.GetByID(info.RecipeItemId));
                    Model.OuRecipeDtlInfo infoOuRecipeDtl = new Model.OuRecipeDtlInfo();
                    if (info.LsRpType == 1 || info.LsRpType == 2)
                    {
                        infoOuRecipeDtl = objOuRecipeDtl.GetByID(info.RecipeItemId);
                        infoOuRecipeDtl.Totality = info.Totality;
                        _lstWestRecipeDtl.Add(infoOuRecipeDtl);
                    }
                    else if (info.LsRpType != 3)
                    {
                        infoOuRecipeDtl = objOuRecipeDtl.GetByID(info.RecipeItemId);
                        infoOuRecipeDtl.Totality = info.Totality;
                        _lstCheckRecipeDtl.Add(infoOuRecipeDtl);
                    }
                    if (infoOuRecipeDtl.IsCharged)   //重复收费（可能同时收取同一个病人），异常
                    {
                        _isDoubleCharged = true;
                        return;
                    }
                }
            }
        }
        private void SaveReicipeData(Model.OuRecipeInfo infoOuRecipe, Model.ModelList<Model.OuRecipeDtlInfo> lstRecipeDtl, System.Data.Common.DbTransaction trn)
        {
            int intSuccess = infoOuRecipe.ID, intSuccessDetail = 0;
            infoOuRecipe.PatId = this.hisOuHosInfo1.Value.PatId;
            //infoOuRecipe.F1 = Model.Configuration.UserProfiles.HospitalID.ToString();
            if (infoOuRecipe.ID == 0)
                intSuccess = infoOuRecipe.ID = _objOuRecipe.Create(infoOuRecipe, trn);
            else
                _objOuRecipe.Modify(infoOuRecipe, trn);
            BLL.CBsXdRpDtl objBsXdRpDtl = new BLL.CBsXdRpDtl();
            foreach (Model.OuRecipeDtlInfo infoRecipeDtl in lstRecipeDtl)
            {
                infoRecipeDtl.RecipeId = intSuccess;
                infoRecipeDtl.F5 = _infoOuInvoice.InvoNo;
                if (infoRecipeDtl.ID == 0)
                {
                    if (!infoRecipeDtl.IsDoctorInput && infoOuRecipe.LsRepType == 1)
                    {
                        Model.ModelList<Model.BsXdRpDtlInfo> lstBsXdRpDtl = objBsXdRpDtl.BsXdRpDtl_SelectByItemIdAndXdRpId(infoRecipeDtl.ItemId, infoRecipeDtl.XDRpId);
                        if (infoRecipeDtl.XDRpId > 0 && lstBsXdRpDtl.Count > 0)
                        {
                            infoRecipeDtl.UnitTakeId = lstBsXdRpDtl[0].UnitTakeId;
                            infoRecipeDtl.UsageId = lstBsXdRpDtl[0].UsageId;
                            infoRecipeDtl.FrequencyId = lstBsXdRpDtl[0].FrequencyId;
                            infoRecipeDtl.Dosage = lstBsXdRpDtl[0].Dosage;
                        }
                        else
                        {
                            Model.BsItemInfo infoItem = BLL.Common.Utils.GetBaseTableRowInfo<Model.BsItemInfo>("BsItem", infoRecipeDtl.ItemId);
                            infoRecipeDtl.UnitTakeId = infoItem.UnitTakeId;
                            infoRecipeDtl.UsageId = infoItem.UsageId;
                            infoRecipeDtl.FrequencyId = infoItem.FrequencyId;
                            infoRecipeDtl.Dosage = infoItem.Dosage;
                        }
                        infoRecipeDtl.Memo = "套餐带出";
                    }
                    intSuccessDetail = _objOuRecipeDtl.Create(infoRecipeDtl, trn);
                    infoRecipeDtl.ID = intSuccessDetail;
                }
                else
                {
                    Model.ModelList<Model.OuRecipeBackInfo> lstOuRecipeBack = bllRecipeBack.OuRecipeBack_SelectByReqDtlId(infoRecipeDtl.ID);
                    lstOuRecipeBack.Fill("IsCharged", "True");
                    bllRecipeBack.Save(lstOuRecipeBack, trn);
                    _objOuRecipeDtl.Modify(infoRecipeDtl, trn);
                }
                AccordRecipeItemId(infoOuRecipe, infoRecipeDtl);
            }
        }
        private void AccordRecipeItemId(Model.OuRecipeInfo infoOuRecipe, Model.OuRecipeDtlInfo infoRecipeDtl)
        {
            foreach (Model.uspOuInvoiceDtlQry infoOuInvoiceDtl in _lstUspOuInvoiceDtl)
            {
                if (infoOuInvoiceDtl.RecipeItemId == 0)
                {
                    if (infoOuInvoiceDtl.F4 != string.Empty && infoOuInvoiceDtl.F4 == infoOuRecipe.RecipeNum && infoOuInvoiceDtl.ItemId == infoRecipeDtl.ItemId)
                    {
                        infoOuInvoiceDtl.RecipeItemId = infoRecipeDtl.ID;
                        return;
                    }
                    else if (infoOuInvoiceDtl.ItemId == infoRecipeDtl.ItemId && infoOuInvoiceDtl.Totality == infoRecipeDtl.Totality)
                    {
                        infoOuInvoiceDtl.RecipeItemId = infoRecipeDtl.ID;
                        return;
                    }
                }
            }
        }
        /// <summary>
        /// 保存处方
        /// </summary>
        /// <param name="isCharge">true表示收费保存,false表示挂起保存</param>
        /// <param name="trn1">当收费保存时，必须传人一个事务</param>
        private void SaveReicipeDtl(bool isCharge, System.Data.Common.DbTransaction trn1)
        {
            BLL.COuRecipeDtl objOuRecipeDtl = new BLL.COuRecipeDtl();
            Model.ModelList<Model.OuRecipeInfo> lstOuRecipe = _objOuRecipe.OuRecipe_SelectByMzRegId(this.hisOuHosInfo1.Value.ID);
            Model.ListView<Model.OuRecipeInfo> lstvOuRecipe = lstOuRecipe.DefaultView;

            GenerateRecipeFromInvoiceDtl();

            System.Data.Common.DbTransaction trn;
            if (isCharge)  //当收费保存时，同一个事务
                trn = trn1;
            else
            {
                DAL.SqlUtil db = new DAL.SqlUtil();
                trn = db.GetSqlTransaction();
            }
            try
            {
                if (_lstCheckRecipeDtl.Count > 0)
                {
                    lstvOuRecipe.Filter = "LsRepType=3";
                    if (isCharge)  //当收费保存时，改变收费标志
                        _lstCheckRecipeDtl.Fill("IsCharged", true);
                    Model.OuRecipeInfo infoOuRecipe = null;
                    if (lstvOuRecipe.Count == 0)
                    {
                        infoOuRecipe = new Model.OuRecipeInfo();
                        infoOuRecipe.DoctorId = this.hisOuHosInfo1.Value.DoctorId;
                        infoOuRecipe.LocationId = this.hisOuHosInfo1.Value.DiagnDept;
                        infoOuRecipe.LsRepType = (int)Model.EnumRecipePrintType.Checkup;
                        infoOuRecipe.MzRegId = this.hisOuHosInfo1.Value.ID;
                        infoOuRecipe.HowMany = 1;
                        infoOuRecipe.RecipeNum = BLL.Common.SequenceNumHandler.GetSequenceNum(Model.EnumSequenceNumType.MzRecipe).ToString();
                    }
                    else
                    {
                        infoOuRecipe = lstvOuRecipe[0];
                        _lstCheckRecipeDtl.Fill("RecipeId", lstvOuRecipe[0].ID);
                        //objOuRecipeDtl.Save(_lstCheckRecipeDtl, trn);
                    }
                    if (!isCharge)//挂起
                    {
                        infoOuRecipe.IsPend = true;
                        _lstCheckRecipeDtl.Remove("F3", "自动收");
                    }
                    infoOuRecipe.RecipeTime = BLL.Common.DateTimeHandler.GetServerDateTime();
                    //_objOuRecipe.SaveChild<Model.OuRecipeDtlInfo, BLL.COuRecipeDtl>(infoOuRecipe, _lstCheckRecipeDtl, "RecipeId", trn);
                    SaveReicipeData(infoOuRecipe, _lstCheckRecipeDtl, trn);
                }
                if (_lstWestRecipeDtl.Count > 0)
                {
                    lstvOuRecipe.Filter = "LsRepType=1";
                    if (isCharge)
                        _lstWestRecipeDtl.Fill("IsCharged", true);
                    Model.OuRecipeInfo infoOuRecipe = null;
                    if (lstvOuRecipe.Count == 0)
                    {
                        infoOuRecipe = new Model.OuRecipeInfo();
                        infoOuRecipe.DoctorId = this.hisOuHosInfo1.Value.DoctorId;
                        infoOuRecipe.LocationId = this.hisOuHosInfo1.Value.DiagnDept;
                        infoOuRecipe.LsRepType = (int)Model.EnumRecipePrintType.WesternMedicine;
                        infoOuRecipe.MzRegId = this.hisOuHosInfo1.Value.ID;
                        infoOuRecipe.HowMany = 1;
                        infoOuRecipe.RecipeNum = BLL.Common.SequenceNumHandler.GetSequenceNum(Model.EnumSequenceNumType.MzRecipe).ToString();
                        infoOuRecipe.RecipeTime = BLL.Common.DateTimeHandler.GetServerDateTime();
                    }
                    else
                    {
                        infoOuRecipe = lstvOuRecipe[0];
                        _lstWestRecipeDtl.Fill("RecipeId", lstvOuRecipe[0].ID);
                        if (PatTypeIdNeedDoctorRecipePrint.Contains(string.Format("[{0}]", this.hisOuHosInfo1.Value.PatTypeId.ToString())))
                            _lstWestRecipeDtl.Fill("IsAttach", false);
                    }
                    if (!isCharge)//挂起
                    {
                        infoOuRecipe.IsPend = true;
                        _lstWestRecipeDtl.Remove("F3", "自动收");
                    }
                    infoOuRecipe.RecipeTime = BLL.Common.DateTimeHandler.GetServerDateTime();
                    //_objOuRecipe.SaveChild<Model.OuRecipeDtlInfo, BLL.COuRecipeDtl>(infoOuRecipe, _lstWestRecipeDtl, "RecipeId", trn);
                    SaveReicipeData(infoOuRecipe, _lstWestRecipeDtl, trn);
                }
                if (_lstChineseRecipeDtl.Count > 0)
                {
                    lstvOuRecipe.Filter = "LsRepType=2";     //中药
                    if (lstvOuRecipe.Count > 0)    //如果不是在收费处登记的中药处方
                    {
                        foreach (Model.OuRecipeDtlInfo info in _lstChineseRecipeDtl)    //把每个处方明细归属到主表中
                        {
                            Model.ModelList<Model.OuRecipeInfo> lstRecipe = _lstChineseRecipe.Find("RecipeNum", info.F4);
                            if (lstRecipe.Count > 0)
                            {
                                int recipeId = lstRecipe[0].ID;
                                info.RecipeId = recipeId;
                            }
                        }
                    }
                    if (!isCharge)//挂起
                        _lstChineseRecipe.Fill("IsPend", true);
                    //_infoOuChineseRecipe.RecipeNum = BLL.Common.SequenceNumHandler.GetSequenceNum(Model.EnumSequenceNumType.MzRecipe).ToString();
                    //_infoOuChineseRecipe.RecipeTime = BLL.Common.DateTimeHandler.GetServerDateTime();
                    foreach (Model.OuRecipeInfo infoRecipe in _lstChineseRecipe)
                    {
                        Model.ModelList<Model.OuRecipeDtlInfo> lstUspChineseRecipeDtl = new Model.ModelList<Model.OuRecipeDtlInfo>();
                        foreach (Model.OuRecipeDtlInfo infoChinDtl in _lstChineseRecipeDtl)
                        {
                            if (infoChinDtl.F4 == infoRecipe.RecipeNum && _lstWestRecipeDtl.Find("ItemId", infoChinDtl.ItemId.ToString()).Count == 0 && !infoChinDtl.IsOtherFee)
                                lstUspChineseRecipeDtl.Add(infoChinDtl);
                        }
                        if (lstUspChineseRecipeDtl.Count == 0)
                        {
                            Utilities.Information.ShowMsgBox("对不起，中药处方收费失败，请删除中药处方重新录入或与系统管理员联系。");
                            trn.Rollback();
                            trn.Dispose();
                        }
                        if (isCharge)
                            lstUspChineseRecipeDtl.Fill("IsCharged", true);
                        infoRecipe.RecipeTime = BLL.Common.DateTimeHandler.GetServerDateTime();
                        string newRecipeNum = BLL.Common.SequenceNumHandler.GetSequenceNum(Model.EnumSequenceNumType.MzRecipe).ToString();
                        foreach (Model.uspOuInvoiceDtlQry infoOuInvoiceDtl in _lstUspOuInvoiceDtl)
                        {
                            if (infoOuInvoiceDtl.F4 == infoRecipe.RecipeNum)   //因为处方号改变了，重新对应发票明细和中药处方明细的关系
                            {
                                infoOuInvoiceDtl.F4 = newRecipeNum;
                            }
                        }
                        infoRecipe.RecipeNum = newRecipeNum;
                        lstUspChineseRecipeDtl.Fill("F4", newRecipeNum);
                        SaveReicipeData(infoRecipe, lstUspChineseRecipeDtl, trn);
                    }
                }
                foreach (Model.uspOuInvoiceDtlQry infoHasAttach in _lstHasAttach)
                {
                    Model.OuRecipeDtlInfo infoRecipeDtl = _objOuRecipeDtl.GetByID(infoHasAttach.RecipeItemId);
                    infoRecipeDtl.IsCharged = isCharge;
                    _objOuRecipeDtl.Modify(infoRecipeDtl, trn);
                }

                if (!isCharge)
                {
                    trn.Commit();
                    this.InformationInMainForm = "成功挂起处方";
                    Tools.Utils.TraceFunctionOperate(208);
                }
            }
            catch (Exception e)
            {
                trn.Rollback();
                trn.Dispose();
                throw (e);
            }
        }

        /// <summary>
        /// 获取结算方式（用字符串将所有的结算方式及每种方式的结算金额读出，以供打印时使用）
        /// </summary>
        /// <param name="lst">结算列表</param>
        /// <returns></returns>
        private string GetPayWay(Model.ModelList<Model.OuInvoicePayInfo> lst)
        {
            StringBuilder payWay = new StringBuilder();
            BLL.CBsPayWay objPayWay = new BLL.CBsPayWay();
            if (lst == null || lst.Count == 0) return string.Empty;
            Model.BsPatientInfo infoPatientInfo = new Model.BsPatientInfo();
            infoPatientInfo = objBsPatient.GetByID(this.hisOuHosInfo1.Value.PatId);
            bool Company = false;
            foreach (Model.OuInvoicePayInfo info in lst)
            {
                if (info.Amount < 0) continue;
                Model.BsPayWayInfo infoBsPayWay = objPayWay.GetByID(info.PaywayId);
                if (Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsInvoicePayWayMoney")) || infoBsPayWay.F3 == "1")
                    payWay.Append(string.Format("{0}:{1} ,", infoBsPayWay.Name, BLL.Common.Utils.Round(info.Amount, 2)));
                else
                    payWay.Append(infoBsPayWay.Name);
                if (infoBsPayWay.F4 == "D 合同欠费")
                    Company = true;
            }
            if (payWay.ToString().EndsWith(","))
                payWay = payWay.Remove(payWay.Length - 1, 1);
            if (Company)
            {
                payWay = new StringBuilder();
                payWay.Append(infoPatientInfo.Company);
            }
            if (lst.Count > 1)
                Tools.Utils.TraceFunctionOperate(205);
            return payWay.ToString();
        }
        public override bool CheckValidate()
        {
            //if (_isGf && !CheckTallyNoForGfPatient()) return false;
            if (_isYb && !CheckTallyNoForGfPatient()) return false;
            if (!CheckDiagLocationAndDoctor()) return false;

            if (!_frmCurrInvo.CheckInvoNoEfectable())
            {
                this.Setup();
                return false;
            }

            BLL.CBsLocation objLocation = new BLL.CBsLocation();
            if (Model.Configuration.UserProfiles.HospitalID > 0 && Model.Configuration.UserProfiles.HospitalID != objLocation.GetByID(this.hisOuHosInfo1.Value.DiagnDept).HospitalId)
            {
                //this.InformationInMainForm = "系统要求病人就诊的科室分院必须与本机所在的分院一致，请修改病人的科室！";
                //return false;
                if (DialogResult.No == MessageBox.Show("系统要求病人就诊的科室分院必须与本机所在的分院一致,是否继续？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
                {
                    return false;
                }
            }

            if (_lstUspOuInvoiceDtl == null || _lstUspOuInvoiceDtl.Count == 0)
            {
                CreateInfoToolTip("不能保存空项目", this.uicItemId);
                return false;
            }

            foreach (Model.uspOuInvoiceDtlQry info in _lstUspOuInvoiceDtl)
            {
                if (info.Totality <= 0)
                {
                    this.InformationInMainForm = string.Format("项目\"{0}\"的数量不能小于或等于零！", info.Name);
                    return false;
                }
                if (info.ExecLocId == 0)
                {
                    this.InformationInMainForm = string.Format("项目\"{0}\"的执行科室不能为空！", info.Name);
                    return false;
                }
                Model.BsItemInfo infoItem = BLL.Common.Utils.GetBaseTableRowInfo<Model.BsItemInfo>("BsItem", info.ItemId);
                if (!infoItem.IsActive || infoItem.IsRpForbid)
                {
                    this.InformationInMainForm = string.Format("项目\"{0}\"的已经被禁用，请检查！", info.Name);
                    return false;
                }
            }
            return base.CheckValidate();
        }

        private bool CheckIfCanAddNewItem(int lsRpType, int itemId)
        {
            //if ((lsRpType != (int)Model.EnumRpType.ChineseMedicine) && _lstUspOuInvoiceDtl.Find("ItemId", itemId.ToString()).Count > 0)
            //{
            //    this.InformationInMainForm = "对不起，在同一张发票不能输入两条相同的项目，请在网格直接修改数量或分开发票！";
            //    return false;
            // }
            return true;
        }

        private bool CheckHasDoctorRecipe(int itemId)
        {
            if (!Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOuChargeAddDortorRecie"))) return true;
            BLL.CRmUnderLine objRmUnderLine = new BLL.CRmUnderLine();
            if (_lstUspOuRecipeDtlForOuInvoiceDtl != null && _lstUspOuRecipeDtlForOuInvoiceDtl.Find("IsDoctorInput", "True").Count > 0 &&
                objRmUnderLine.RmUnderLine_SelectByItemId(itemId).Count > 0)
            {
                this.ShowInformationInMainForm("医生已经开出的电子处方，您不能在收费处单独输入药品，请通知医生本人进行修改！");
                //if (_lstUspOuRecipeDtlForOuInvoiceDtl.Find("IsDoctorInput", "True").Count > 0)
                //   Utilities.Information.ShowMsgBox(_lstUspOuRecipeDtlForOuInvoiceDtl[0].Name);
                return false;
            }
            Tools.Utils.TraceFunctionOperate(195);
            return true;
        }

        private bool CheckDiagLocationAndDoctor()
        {
            if (this.hisOuHosInfo1.Value == null || this.hisOuHosInfo1.Value.RegDept == 0)
            {
                CreateInfoToolTip("就诊科室不能为空，请输入科室", this.hisOuHosInfo1.ubsLocationId);
                return false;
            }
            if (this.hisOuHosInfo1.Value == null || this.hisOuHosInfo1.Value.DoctorId == 0)
            {
                CreateInfoToolTip("医生不能为空，请输入医生", this.hisOuHosInfo1.ubsDoctorId);
                return false;
            }
            return true;
        }
        private void CheckNotChargeRecipeLast()
        {
            BLL.COuRecipeDtl objOuRecipeDtl = new BLL.COuRecipeDtl();
            Model.ModelList<Model.OuRecipeInfo> lstOuRecipe = _objOuRecipe.OuRecipe_SelectByPatId(this.hisOuHosInfo1.Value.PatId).Filter("MzRegId", this.hisOuHosInfo1.Value.ID.ToString());
            Model.ModelList<Model.OuRecipeDtlInfo> lstHasRecipeDtl = _objOuRecipe.GetMutiChild<Model.OuRecipeDtlInfo, BLL.COuRecipeDtl>(lstOuRecipe.ConvertToBase(), "RecipeId");
            if (lstHasRecipeDtl.Find("IsCharged", "False").Find("IsCancel", "False").Count > 0)
            {
                Utilities.Information.ShowMsgBox(string.Format("请注意：该病人还有以往费用没有结算，日期：（{0}）", Utilities.Information.FilterArrayToString(lstOuRecipe.CopyTo("RecipeTime"))));

                //lstOuRecipe.Fill("MzRegId", this.hisOuHosInfo1.Value.ID);
                //_objOuRecipe.Save(lstOuRecipe, null);
            }
        }

        bool isNewOuhospInfo = true;
        Model.BsDoctorInfo _infoDoctor;
        BLL.CBsDoctor objBsDoctor = new BLL.CBsDoctor();
        /// <summary>
        /// 初始病人信息
        /// </summary>
        bool IsDoctorInput = false;
        private void InitPatientData()
        {
            IsDoctorInput = false;
            _lstUspOuRecipeDtlTest = new Model.ModelList<Model.uspOuRecipeDtlForOuInvoiceDtlQry>();
            _lstUspOuRecipeDtlCheck = new Model.ModelList<Model.uspOuRecipeDtlForOuInvoiceDtlQry>();
            _lstUspOuRecipeDtlCure = new Model.ModelList<Model.uspOuRecipeDtlForOuInvoiceDtlQry>();
            _lstUspOuRecipeDtlOPS = new Model.ModelList<Model.uspOuRecipeDtlForOuInvoiceDtlQry>();
            _infoOuInvoice = new Model.OuInvoiceInfo();//从新生成基本发票信息
            _infoDoctor = objBsDoctor.GetByID(this.hisOuHosInfo1.Value.DoctorId);
            if (_objOuRecipe.OuRecipe_SelectByMzRegId(this.hisOuHosInfo1.Value.ID).Count > 0)
            {
                CheckNotChargeRecipeLast();
                _uspOuRecipeDtlForOuInvoiceDtFinder.AddParameter("MzRegId", this.hisOuHosInfo1.Value.ID);
                _uspOuRecipeDtlForOuInvoiceDtFinder.AddParameter("PatTypeId", this.hisOuHosInfo1.Value.PatTypeId);
                _uspOuRecipeDtlForOuInvoiceDtFinder.AddParameter("LocationId", this.hisOuHosInfo1.Value.DiagnDept);
                _lstUspOuRecipeDtlForOuInvoiceDtl = _uspOuRecipeDtlForOuInvoiceDtFinder.Find("uspOuRecipeDtlForOuInvoiceDtl");
                RemoveSelectRecipe();
                _lstUspOuRecipeDtlForOuInvoiceDtl.Sort("RecipeId,GroupNum,PrepareTime");
                //TestMemo();
                _lstUspOuInvoiceDtl = _lstUspOuRecipeDtlForOuInvoiceDtl.ConvertTo<Model.uspOuInvoiceDtlQry>();
                HandleBackDrug();
                SetExecLocId();
                _lstUspOuRecipeDtlForOuInvoiceDtl = _lstUspOuRecipeDtlForOuInvoiceDtl.Find("IsCancel", "False");

                RemoveOtherFeeSecondTime();
                _lstUspOuInvoiceDtl.Fill("DiscSelf", 1.00);
                isNewOuhospInfo = false;
                if (_lstUspOuRecipeDtlForOuInvoiceDtl.Find("IsDoctorInput", "True").Count == 0)
                {
                    this.hisOuHosInfo1.uoupName.txtHospitalNo.ForeColor = System.Drawing.Color.Red;
                    IsDoctorInput = false;
                }
                else
                {
                    IsDoctorInput = true;
                }



            }
            else
            {
                isNewOuhospInfo = true;
                ClearToInit();
            }
            GetHasRecipeDtl();//得到医生开的处方
            GetDiagSamePatientName();
            GetRegFee();
            GetPackgeCharge();
            for (double i = 0; i < 10; i++)
            {
                GetUsageItem(i);
            }
            _lstUspOuInvoiceDtl.AddRange(_lstUspOuRecipeDtlCheck.ConvertTo<Model.uspOuInvoiceDtlQry>());
            _lstUspOuInvoiceDtl.AddRange(_lstUspOuRecipeDtlCure.ConvertTo<Model.uspOuInvoiceDtlQry>());
            _lstUspOuInvoiceDtl.AddRange(_lstUspOuRecipeDtlOPS.ConvertTo<Model.uspOuInvoiceDtlQry>());
            _lstUspOuInvoiceDtl.AddRange(_lstUspOuRecipeDtlTest.ConvertTo<Model.uspOuInvoiceDtlQry>());
            GetItemAttach(false);
            GetLabSourceItem();

            if (this.hisOuHosInfo1.Value.DoctorId == Convert.ToInt32(BLL.Common.Utils.GetSystemSetting("TjDoctorId")))
                GetCheckItemCompany();
            else
                GetCheckItem();
            GetChineseRecipe(this.hisOuHosInfo1.Value.ID, "False");
            GetCookCharge();
            RemoveSelfDrug();
            //_lstUspOuInvoiceDtl.Sort("InvItemId,Memo,F1");
            this.devGrid1.DataSource = _lstUspOuInvoiceDtl;
            this.devGrid1.Refresh();
            //GetPatientLimitGroup();    //如果没有病人类别对应的限额则不需要此语句
            this.ubsExecLocId.ID = 0;
            this.uicItemId.Focus();
        }
        private void RemoveSelectRecipe()
        {
            Model.ModelList<Model.SelectItem> lstSelectRecipe = _lstSelectRecipe.Find("IsSelect", "True");

            _lstUspOuRecipeDtlForOuInvoiceDtlRemove = _lstUspOuRecipeDtlForOuInvoiceDtl.ConvertTo<Model.uspOuRecipeDtlForOuInvoiceDtlQry>();
            if (lstSelectRecipe.Count == 0) return;


            for (int i = 0; i < _lstUspOuRecipeDtlForOuInvoiceDtl.Count; i++)
            {

                if (lstSelectRecipe.Count > 0 && _lstUspOuRecipeDtlForOuInvoiceDtl[i].LsRepType == 1 && lstSelectRecipe.Find("Name", "西药处方").Find("Code", _lstUspOuRecipeDtlForOuInvoiceDtl[i].ListNum.ToString()).Count == 0)
                {
                    _lstUspOuRecipeDtlForOuInvoiceDtl.Remove(_lstUspOuRecipeDtlForOuInvoiceDtl[i]);
                    i--;
                    continue;
                }
                if (lstSelectRecipe.Count > 0 && _lstUspOuRecipeDtlForOuInvoiceDtl[i].LsRepType == 3 && lstSelectRecipe.Find("Name", "检查检验").Find("Code", _lstUspOuRecipeDtlForOuInvoiceDtl[i].ListNum.ToString()).Count == 0)
                {
                    _lstUspOuRecipeDtlForOuInvoiceDtl.Remove(_lstUspOuRecipeDtlForOuInvoiceDtl[i]);
                    i--;
                    continue;
                }
                if (lstSelectRecipe.Count > 0 && _lstUspOuRecipeDtlForOuInvoiceDtl[i].LsRepType == 2 && lstSelectRecipe.Find("Name", "中药处方").Find("F4", _lstUspOuRecipeDtlForOuInvoiceDtl[i].RecipeId.ToString()).Count == 0)
                {
                    _lstUspOuRecipeDtlForOuInvoiceDtl.Remove(_lstUspOuRecipeDtlForOuInvoiceDtl[i]);
                    i--;
                    continue;
                }
            }




        }
        private void RemoveOtherFeeSecondTime()
        {
            BLL.COuInvoiceDtl objOuInvoiceDtl = new BLL.COuInvoiceDtl();
            string OtherFee = string.Empty;
            for (int i = 0; i < _lstUspOuRecipeDtlForOuInvoiceDtl.Count; i++)
            {
                if (!_lstUspOuRecipeDtlForOuInvoiceDtl[i].IsOtherFee) continue;
                Model.ModelList<Model.OuInvoiceDtlInfo> lstOuInvoiceDtl = objOuInvoiceDtl.GetDynamic(string.Format("(F4 like '%{0}%' or recipegroupid={1}) and lsperform=1", _lstUspOuRecipeDtlForOuInvoiceDtl[i].RecipeItemId + ",", _lstUspOuRecipeDtlForOuInvoiceDtl[i].RecipeItemId), null);
                if (lstOuInvoiceDtl.Count > 0)
                {
                    string SameGroupNum = string.Format("[{0}]", "Group" + _lstUspOuRecipeDtlForOuInvoiceDtl[i].GroupNum + "Frequency" + _lstUspOuRecipeDtlForOuInvoiceDtl[i].FrequencyId.ToString() + "Days" + _lstUspOuRecipeDtlForOuInvoiceDtl[i].Days.ToString());
                    if (!OtherFee.Contains(SameGroupNum))
                        OtherFee += SameGroupNum;
                    _lstUspOuRecipeDtlForOuInvoiceDtl.Remove(_lstUspOuRecipeDtlForOuInvoiceDtl[i]);
                    i--;
                }
            }
            if (OtherFee == string.Empty) return;
            for (int i = 0; i < _lstUspOuRecipeDtlForOuInvoiceDtl.Count; i++)
            {
                if (!_lstUspOuRecipeDtlForOuInvoiceDtl[i].IsOtherFee) continue;
                string SameGroupNum = string.Format("[{0}]", "Group" + _lstUspOuRecipeDtlForOuInvoiceDtl[i].GroupNum + "Frequency" + _lstUspOuRecipeDtlForOuInvoiceDtl[i].FrequencyId.ToString() + "Days" + _lstUspOuRecipeDtlForOuInvoiceDtl[i].Days.ToString());
                if (OtherFee.Contains(SameGroupNum))
                {
                    _lstUspOuRecipeDtlForOuInvoiceDtl.Remove(_lstUspOuRecipeDtlForOuInvoiceDtl[i]);
                    i--;
                }
            }
        }
        private void GetPackgeCharge()
        {
            if (!Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOuChargeDrugBag"))) return;
            int PackgeChargeID = Convert.ToInt32(BLL.Common.Utils.GetSystemSetting("DrugBagItemId"));
            string PackItemIds = BLL.Common.Utils.GetSystemSetting("PackItemIds");
            int OuChargeBottleFeeItemId = Convert.ToInt32(BLL.Common.Utils.GetSystemSetting("OuChargeBottleFeeItemId"));
            BLL.CBsItemDrug objBsitemDrug = new BLL.CBsItemDrug();
            BLL.CBsDrugForm objDrugForm = new BLL.CBsDrugForm();
            Model.ModelList<Model.uspOuRecipeDtlForOuInvoiceDtlQry> lstPackgeCharge = new Model.ModelList<Model.uspOuRecipeDtlForOuInvoiceDtlQry>();
            Model.ModelList<Model.uspOuRecipeDtlForOuInvoiceDtlQry> lstBottleCharge = new Model.ModelList<Model.uspOuRecipeDtlForOuInvoiceDtlQry>();
            foreach (Model.uspOuRecipeDtlForOuInvoiceDtlQry info in _lstUspOuRecipeDtlForOuInvoiceDtl)
            {
                if (!info.IsDoctorInput) continue;
                if (!("1,2").Contains(info.LsRpType.ToString())) continue;
                Model.ModelList<Model.BsItemDrugInfo> lstItemDrug = objBsitemDrug.BsItemDrug_SelectByItemId(info.ItemId);
                if (BLL.Common.Utils.GetSystemSetting("OuChargeHasBottleItemId").Contains(string.Format("[{0}]", info.ItemId)) && info.Totality < BLL.HuRmCommon.GetUnitRatio(info.ItemId, lstItemDrug[0].UnitKc, info.UnitId))
                    lstBottleCharge.Add(info);
                else if (lstItemDrug.Count > 0 && (objDrugForm.GetByID(lstItemDrug[0].FormId).F1 == "1") || PackItemIds.Contains(string.Format("[{0}]", info.ItemId)))
                {
                    lstPackgeCharge.Add(info);
                }
            }
            if (lstPackgeCharge.Count == 0 && lstBottleCharge.Count == 0) return;
            Model.ModelList<Model.ComputeSummary> lstComputeSummary = lstPackgeCharge.GroupBy("ItemId", "ItemId", Model.ComputeType.Sum);
            if (lstComputeSummary.Count > 0)
            {
                AddOneCustFee(PackgeChargeID, BLL.Common.Utils.GetBaseTableRowInfo<Model.BsItemInfo>("BsItem", PackgeChargeID).PriceDiag, lstComputeSummary.Count, "西药袋费用", 0, string.Empty, false, 0);
            }
            Model.ModelList<Model.ComputeSummary> lstBottleChargeSummary = lstBottleCharge.GroupBy("ItemId", "ItemId", Model.ComputeType.Sum);
            if (lstBottleChargeSummary.Count > 0)
            {
                AddOneCustFee(OuChargeBottleFeeItemId, BLL.Common.Utils.GetBaseTableRowInfo<Model.BsItemInfo>("BsItem", OuChargeBottleFeeItemId).PriceDiag, lstBottleChargeSummary.Count, "药水瓶费用", 0, string.Empty, false, 0);
            }
        }
        private void RemoveSelfDrug()
        {
            for (int i = 0; i < _lstUspOuInvoiceDtl.Count; i++)
            {
                _lstUspOuInvoiceDtl[i].Memo = _lstUspOuInvoiceDtl[i].Memo.Replace("，，", "，");
                if (_lstUspOuInvoiceDtl[i].Memo.Trim().EndsWith("，"))
                    _lstUspOuInvoiceDtl[i].Memo = _lstUspOuInvoiceDtl[i].Memo.Substring(0, _lstUspOuInvoiceDtl[i].Memo.LastIndexOf("，"));

                if (_lstUspOuInvoiceDtl[i].IsOtherFee)
                {
                    _lstUspOuInvoiceDtl.Remove(_lstUspOuInvoiceDtl[i]);
                    i--;
                }
            }
        }
        private string GetExecuteMemo(string memo)
        {
            char[] split = { ',', '，' };
            string[] strMemo = memo.Split(split);
            for (int i = 0; i < strMemo.Length; i++)
            {
                if (strMemo[i].Trim().Contains("执行"))
                    return strMemo[i].Trim();
            }
            return string.Empty;
        }
        Model.ModelList<Model.BsLabSourceItemInfo> _lstSourceItem = new Model.ModelList<Model.BsLabSourceItemInfo>();
        string OneChargeOnceItemId = BLL.Common.Utils.GetSystemSetting("OneChargeOnceItemId");
        private void GetLabSourceItem()
        {
            BLL.CBsXdRp objBsXdRp = new BLL.CBsXdRp();
            BLL.CCkItem objCkItem = new BLL.CCkItem();
            BLL.CBsLabSourceItem objBsLabSourceItem = new BLL.CBsLabSourceItem();
            Model.ModelList<Model.BsLabSourceItemInfo> lstBsLabSourceItem = objBsLabSourceItem.GetAll();
            Model.ModelList<Model.uspOuInvoiceDtlQry> lstOuInvoiceDtl = _lstUspOuInvoiceDtl.Find("LsRpType", "4");
            string sourceIds = string.Empty;
            string testTypeIds = string.Empty;
            string tubeTypeIds = string.Empty;
            int lastXdRpId = 0;
            string lastMemo = string.Empty;
            foreach (Model.uspOuInvoiceDtlQry infoRecipeDtl in lstOuInvoiceDtl)
            {
                //if (infoRecipeDtl.XDRpId > 0 && infoRecipeDtl.XDRpId == lastXdRpId)
                //    continue;
                bool isSpecExecute = false;
                if (infoRecipeDtl.Memo.Contains("执行") && lastMemo == GetExecuteMemo(infoRecipeDtl.Memo))
                    isSpecExecute = true;
                else
                    isSpecExecute = false;
                lastMemo = GetExecuteMemo(infoRecipeDtl.Memo);

                int testId;
                //if (infoRecipeDtl.XDRpId > 0 && infoRecipeDtl.F3 == "自动收")
                //{
                //    testId = objBsItem.GetByID(infoRecipeDtl.XDRpId).LISCode;
                //    lastXdRpId = infoRecipeDtl.XDRpId;
                //}
                //else if (infoRecipeDtl.XDRpId > 0 && infoRecipeDtl.F3 != "自动收")
                //{
                //    testId = objBsXdRp.GetByID(infoRecipeDtl.XDRpId).TestId;
                //    lastXdRpId = infoRecipeDtl.XDRpId;
                //}
                //else
                testId = BLL.Common.Utils.GetBaseTableRowInfo<Model.BsItemInfo>("BsItem", infoRecipeDtl.ItemId).LISCode;
                Model.CkItemInfo infoCkItem = objCkItem.GetByID(testId);
                string sourceId = string.Format("[{0}]", infoCkItem.SourceId);
                string testTypeId = string.Format("[{0}]", infoCkItem.TestTypeId);
                string tubeTypeId = string.Format("[{0}]", infoCkItem.TubeTypeId);
                Model.ModelList<Model.BsLabSourceItemInfo> lstSourceItem = lstBsLabSourceItem.Find("SourceId", infoCkItem.SourceId.ToString());
                foreach (Model.BsLabSourceItemInfo infoAttach in lstSourceItem)
                {
                    if (infoAttach.LsUseArea == 2) continue;
                    if (infoAttach.TubeTypeId > 0 && infoAttach.TubeTypeId != infoCkItem.TubeTypeId) continue;
                    if (!infoRecipeDtl.Memo.Contains("执行") && sourceIds.Contains(sourceId) && testTypeIds.Contains(testTypeId) && tubeTypeIds.Contains(tubeTypeId) && _lstSourceItem.Find("ItemId", infoAttach.ItemId.ToString()).Count > 0)
                        continue;
                    if (!infoRecipeDtl.Memo.Contains("执行") && infoAttach.F4 == "收一次" && _lstUspOuInvoiceDtl.Find("ItemId", infoAttach.ItemId.ToString()).Count > 0)
                        continue;
                    if (OneChargeOnceItemId.Contains(infoAttach.ItemId.ToString())) continue;

                    _lstSourceItem.Add(infoAttach);
                    AddOneCustFee(infoAttach.ItemId, 0, infoAttach.Totality * infoRecipeDtl.Totality, infoRecipeDtl.Name, infoRecipeDtl.RecipeItemId, string.Empty, false, 0);
                    Tools.Utils.TraceFunctionOperate(199);
                }
                sourceIds += sourceId;
                testTypeIds += testTypeId;
                tubeTypeIds += tubeTypeId;
            }
        }
        private Model.ModelList<Model.uspOuInvoiceDtlQry> _lstHasAttach;
        private void GetItemAttach(bool isManual)
        {
            _lstHasAttach = new Model.ModelList<Model.uspOuInvoiceDtlQry>();
            for (int i = 0; i < _lstUspOuInvoiceDtl.Count; i++)
            {
                if (!_lstUspOuInvoiceDtl[i].IsDoctorInput) continue;
                if (_lstSelectItem.Find("IsSelect", "True").Find("ID", _lstUspOuInvoiceDtl[i].RecipeItemId.ToString()).Count > 0)
                {
                    foreach (Model.SelectItem infoSelectItem in _lstSelectItem)
                    {
                        //if (!infoSelectItem.IsSelect) continue;
                        if (infoSelectItem.ID != _lstUspOuInvoiceDtl[i].RecipeItemId) continue;
                        infoSelectItem.F2 = "1";
                    }
                    _lstUspOuInvoiceDtl.Remove(_lstUspOuInvoiceDtl[i]);
                    i--;
                    continue;
                }

                if (GetItemAttachFee(_lstUspOuInvoiceDtl[i], _lstUspOuInvoiceDtl[i].ItemId, isManual))
                {
                    _lstHasAttach.Add(_lstUspOuInvoiceDtl[i]);
                    _lstUspOuInvoiceDtl.Remove(_lstUspOuInvoiceDtl[i]);
                    i--;
                }
            }
            CalcuateAmountTally();
            InitData();
        }
        int lastAttachItemId = 0;
        private bool GetItemAttachFee(Model.uspOuInvoiceDtlQry infoOuInvoiceDtl, int itemId, bool isManual)
        {
            BLL.COuNotAttach objOuNotAttach = new BLL.COuNotAttach();
            Model.ModelList<Model.OuNotAttachInfo> lstNotSelectAttach = objOuNotAttach.OuNotAttach_SelectByMzRegId(this.hisOuHosInfo1.Value.ID);
            BLL.CBsItemAttach objAttach = new BLL.CBsItemAttach();
            Model.ModelList<Model.BsItemAttachInfo> lstAttach = objAttach.BsItemAttach_SelectByItemId1(itemId);
            lstAttach.Sort("F1");
            string memo = string.Empty;
            if (isManual)
                memo = "手工录入";
            bool hasAttach = false;
            double priceTotal = BLL.Common.Utils.GetBaseTableRowInfo<Model.BsItemInfo>("BsItem", itemId).PriceDiag;
            if (priceTotal == 0)  //没有设置好组项目的价格就按照子项目的原价进行
                priceTotal = -1;
            foreach (Model.BsItemAttachInfo infoAttach in lstAttach)
            {
                if (infoAttach.LsUseArea == 2) continue;
                if (infoAttach.LocationId > 0 && infoAttach.LocationId != this.hisOuHosInfo1.Value.DiagnDept) continue;
                if (infoAttach.ItemId1 == infoAttach.ItemId2) continue;
                if (lstNotSelectAttach.Find("ItemId1", itemId.ToString()).Find("ItemId2", infoAttach.ItemId2.ToString()).Count > 0) continue;
                double times = infoAttach.Totality * infoOuInvoiceDtl.Totality;
                if (infoAttach.F4 == "一天收一次")
                {
                    if (_lstUspOuInvoiceDtlPatientToday.Find("ItemId", infoAttach.ItemId2.ToString()).Count > 0)
                    {
                        if (infoAttach.F3 != "1" && Utilities.Information.IsNumeric(infoAttach.F3))
                            itemId = Convert.ToInt32(infoAttach.F3);
                        else
                            continue;
                    }
                    else
                        times = 1;
                }
                else if (infoAttach.F4 == "一次申请收一次")
                {
                    if (_lstUspOuInvoiceDtl.Find("ItemId", infoAttach.ItemId2.ToString()).Count > 0)
                    {
                        if (infoAttach.F3 != "1" && Utilities.Information.IsNumeric(infoAttach.F3))
                            itemId = Convert.ToInt32(infoAttach.F3);
                        else
                            continue;
                    }
                    times = 1;
                }
                else if (infoAttach.F4 == "第2次开始收一次" && _lstUspOuInvoiceDtl.Find("ItemId", infoAttach.ItemId2.ToString()).Count > 0)
                {
                    times = 1;
                }

                Model.ModelList<Model.BsItemAttachInfo> lstAttach2 = objAttach.BsItemAttach_SelectByItemId1(infoAttach.ItemId2);
                lstAttach2.Sort("F1");
                foreach (Model.BsItemAttachInfo infoAttach2 in lstAttach2)
                {
                    GetItemAttachFee(infoOuInvoiceDtl, infoAttach2.ItemId2, isManual);
                    lastAttachItemId = infoAttach2.ItemId2;
                }
                Model.BsItemInfo infoBsItem = BLL.Common.Utils.GetBaseTableRowInfo<Model.BsItemInfo>("BsItem", infoAttach.ItemId2);
                if (priceTotal > 0 && priceTotal < infoBsItem.PriceDiag)
                    AddOneCustFee(infoAttach.ItemId2, priceTotal, times, Tools.Utils.AddComer(infoOuInvoiceDtl.Name, infoOuInvoiceDtl.Memo), infoOuInvoiceDtl.RecipeItemId, memo == string.Empty ? "折扣" : memo + "折扣", false, itemId);
                else
                    AddOneCustFee(infoAttach.ItemId2, 0, times, Tools.Utils.AddComer(infoOuInvoiceDtl.Name, infoOuInvoiceDtl.Memo), infoOuInvoiceDtl.RecipeItemId, memo, priceTotal <= 0, itemId);
                if (priceTotal > 0)
                    priceTotal -= infoBsItem.PriceDiag;
                hasAttach = true;
                Tools.Utils.TraceFunctionOperate(200);
            }
            return hasAttach;
        }
        private void GetCheckItem()
        {
            if (!Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOuChergeGetPersonCheck"))) return;
            BLL.Finder<Model.uspOuCkeckItemQry> bllFinder = new BLL.Finder<Model.uspOuCkeckItemQry>();
            bllFinder.AddParameter("PatId", this.hisOuHosInfo1.Value.PatId);
            Model.ModelList<Model.uspOuCkeckItemQry> lstOuCkeckItem = bllFinder.Find("uspOuCkeckItem");
            foreach (Model.uspOuCkeckItemQry infoCkeckItem in lstOuCkeckItem)
            {
                AddOneCustFee(infoCkeckItem.ItemId, infoCkeckItem.PriceDiag, 1, "个人体检", 0, string.Empty, false, 0);
            }
            if (lstOuCkeckItem.Count > 0)
                Tools.Utils.TraceFunctionOperate(191);
        }
        private void GetCheckItemCompany()
        {
            if (this.hisOuHosInfo1.Value.MedicareNo == string.Empty) return;
            BLL.Finder<Model.uspOuCkeckItemQry> bllFinder = new BLL.Finder<Model.uspOuCkeckItemQry>();
            bllFinder.AddParameter("CkeckId", Convert.ToInt32(this.hisOuHosInfo1.Value.MedicareNo));
            Model.ModelList<Model.uspOuCkeckItemQry> lstOuCkeckItem = bllFinder.Find("uspOuCkeckItemCompany");
            foreach (Model.uspOuCkeckItemQry infoCkeckItem in lstOuCkeckItem)
            {
                AddOneCustFee(infoCkeckItem.ItemId, infoCkeckItem.PriceDiag, infoCkeckItem.Totality, "单位体检", 0, string.Empty, false, 0);
            }
            if (lstOuCkeckItem.Count > 0)
                Tools.Utils.TraceFunctionOperate(191);
        }
        int GroupTemp = 5000;
        private Model.ModelList<Model.uspOuRecipeDtlForOuInvoiceDtlQry> GetMemoSortedList(double day)
        {
            Model.ModelList<Model.uspOuRecipeDtlForOuInvoiceDtlQry> lstSortedAdvice = new Model.ModelList<Model.uspOuRecipeDtlForOuInvoiceDtlQry>();
            Model.ModelList<Model.uspOuRecipeDtlForOuInvoiceDtlQry> lstOuRecipeDtlDay = _lstUspOuRecipeDtlForOuInvoiceDtl.FindLarge("Days", day);
            Model.ModelList<Model.ComputeSummary> lstGroupType = lstOuRecipeDtlDay.GroupBy("LsGroupType", "LsGroupType", Model.ComputeType.Max);
            Model.ModelList<Model.uspOuRecipeDtlForOuInvoiceDtlQry> lstAdviceGroup = new Model.ModelList<Model.uspOuRecipeDtlForOuInvoiceDtlQry>();
            foreach (Model.ComputeSummary infoGroupType in lstGroupType)
            {
                Model.ModelList<Model.uspOuRecipeDtlForOuInvoiceDtlQry> lstOuRecipeGroupType = lstOuRecipeDtlDay.Find("LsGroupType", infoGroupType.GroupBy);
                Model.ModelList<Model.ComputeSummary> lstGroup = lstOuRecipeGroupType.GroupBy("GroupNum", "GroupNum", Model.ComputeType.Max);
                foreach (Model.ComputeSummary infoGroup in lstGroup)
                {
                    Model.ModelList<Model.uspOuRecipeDtlForOuInvoiceDtlQry> lstTempGroup = lstOuRecipeGroupType.Find("GroupNum", infoGroup.GroupBy.ToString());
                    lstTempGroup.Sort("RecipeDtlMemo");
                    lstAdviceGroup.Add(lstTempGroup[0]);
                }
            }
            lstAdviceGroup.Sort("LsGroupType,UsageId,GroupNum,RecipeDtlMemo");
            foreach (Model.uspOuRecipeDtlForOuInvoiceDtlQry infoGroupAdvice in lstAdviceGroup)
            {
                Model.ModelList<Model.uspOuRecipeDtlForOuInvoiceDtlQry> lstAdvice = lstOuRecipeDtlDay.Find("LsGroupType", infoGroupAdvice.LsGroupType.ToString()).Find("GroupNum", infoGroupAdvice.GroupNum.ToString());
                lstAdvice.Sort("FrequencyId,Days,RecipeDtlMemo");
                Model.uspOuRecipeDtlForOuInvoiceDtlQry TempOuInvoiceDtlQry;
                foreach (Model.uspOuRecipeDtlForOuInvoiceDtlQry infoAdvice in lstAdvice)
                {
                    TempOuInvoiceDtlQry = new Model.uspOuRecipeDtlForOuInvoiceDtlQry();
                    // if (infoAdvice.Memo.Contains("皮试"))
                    if (XYHIS.Common.Helper.IsSkin(infoAdvice.Memo, 1, infoAdvice.UsageId))
                    {
                        if (day < 1)
                            InsertSkinDrug(infoAdvice);
                    }
                    if (GetAloneTotality(infoAdvice) > 1)
                    {
                        TempOuInvoiceDtlQry = infoAdvice.ConvertTo<Model.uspOuRecipeDtlForOuInvoiceDtlQry>();
                        TempOuInvoiceDtlQry.GroupNum = GroupTemp;
                        GroupTemp++;
                        TempOuInvoiceDtlQry.Memo = Tools.Utils.AddMemoString(TempOuInvoiceDtlQry.Memo, "药房免发");
                        TempOuInvoiceDtlQry.RecipeDtlMemo = Tools.Utils.AddMemoString(TempOuInvoiceDtlQry.RecipeDtlMemo, "药房免发");
                        if (TempOuInvoiceDtlQry.Memo.Trim().Length > 0)
                            TempOuInvoiceDtlQry.Memo += "，接瓶";
                        else TempOuInvoiceDtlQry.Memo = "接瓶";
                        if (TempOuInvoiceDtlQry.RecipeDtlMemo.Trim().Length > 0)
                            TempOuInvoiceDtlQry.RecipeDtlMemo += "，接瓶";
                        else TempOuInvoiceDtlQry.RecipeDtlMemo = "接瓶";
                    }
                    if ((infoAdvice.Memo == string.Empty || infoAdvice.Memo.Contains("静脉")) && _lstHasRecipeDtl.Find("GroupNum", infoAdvice.GroupNum.ToString()).Find("RecipeId", infoAdvice.RecipeId.ToString()).Find("IsCharged", "True").Count > 0)
                        continue;
                    lstSortedAdvice.Add(infoAdvice);
                    if (TempOuInvoiceDtlQry.RecipeItemId > 0)
                        lstSortedAdvice.Add(TempOuInvoiceDtlQry);
                }
            }
            //Model.ModelList<Model.uspOuRecipeDtlForOuInvoiceDtlQry> lstDrop = lstSortedAdvice.Find("UsageId", "4");
            //if (lstDrop.Count > 0 && lstDrop.FindInclude("Memo", "接瓶加药").Count == lstDrop.Count)
            //{
            //    lstDrop.Sort("FrequencyId");
            //    lstDrop.Reverse();
            //    int groupNum = lstDrop[0].GroupNum;
            //    foreach (Model.uspOuRecipeDtlForOuInvoiceDtlQry infoDrop in lstSortedAdvice)
            //    {
            //        if (infoDrop.GroupNum == groupNum)
            //        {
            //            //infoDrop.Memo = infoDrop.Memo.Replace("，接瓶加药", string.Empty);
            //            //infoDrop.Memo = infoDrop.Memo.Replace("接瓶加药，", string.Empty);
            //            //infoDrop.Memo = infoDrop.Memo.Replace("接瓶加药", string.Empty);

            //            infoDrop.Memo = infoDrop.Memo.Replace("，接瓶", string.Empty);
            //            infoDrop.Memo = infoDrop.Memo.Replace("接瓶，", string.Empty);
            //            infoDrop.Memo = infoDrop.Memo.Replace("接瓶", string.Empty);
            //            infoDrop.Memo = infoDrop.Memo.Replace("，接瓶加药", string.Empty);
            //            infoDrop.Memo = infoDrop.Memo.Replace("接瓶加药，", string.Empty);
            //            infoDrop.Memo = infoDrop.Memo.Replace("接瓶加药", string.Empty);

            //            infoDrop.RecipeDtlMemo = infoDrop.RecipeDtlMemo.Replace("，接瓶", string.Empty);
            //            infoDrop.RecipeDtlMemo = infoDrop.RecipeDtlMemo.Replace("接瓶，", string.Empty);
            //            infoDrop.RecipeDtlMemo = infoDrop.RecipeDtlMemo.Replace("接瓶", string.Empty);
            //            infoDrop.RecipeDtlMemo = infoDrop.RecipeDtlMemo.Replace("，接瓶加药", string.Empty);
            //            infoDrop.RecipeDtlMemo = infoDrop.RecipeDtlMemo.Replace("接瓶加药，", string.Empty);
            //            infoDrop.RecipeDtlMemo = infoDrop.RecipeDtlMemo.Replace("接瓶加药", string.Empty);
            //        }
            //    }
            //}
            return lstSortedAdvice;
        }
        string DropUsageId = BLL.Common.Utils.GetSystemSetting("DropUsageId");
        string AloneTotalityItemId = BLL.Common.Utils.GetSystemSetting("AloneTotalityItemId");
        string UsageIdNoForFrequencyDay = BLL.Common.Utils.GetSystemSetting("UsageIdNoForFrequencyDay");
        private void GetUsageItem(double day)
        {
            BLL.CBsFrequency objFrequency = new BLL.CBsFrequency();
            _lstSourceItem.Clear();
            _lastGroupUsageId = 0;
            int lastGroupNum = 0;
            int lastUsageId = 0;
            int lastFrequencyId = 0;
            int lastDays = 0;
            string lastMemo = string.Empty;
            string lastRecipeDtlMemo = string.Empty;
            double lastTimes = 0;
            double times = 1;
            int LsGroupType = 0;
            Model.ModelList<Model.uspOuRecipeDtlForOuInvoiceDtlQry> lstAddContinue = new Model.ModelList<Model.uspOuRecipeDtlForOuInvoiceDtlQry>();
            Model.ModelList<Model.uspOuRecipeDtlForOuInvoiceDtlQry> lstSortedAdvice = GetMemoSortedList(day);
            foreach (Model.uspOuRecipeDtlForOuInvoiceDtlQry infoRecipeDtl in lstSortedAdvice)
            {
                if (infoRecipeDtl.GroupNum == 3)
                {

                }
                if (DropUsageId.Contains(string.Format("[{0}]", infoRecipeDtl.UsageId)))
                {
                    lstAddContinue.Add(infoRecipeDtl);
                }
                if (!(infoRecipeDtl.IsDoctorInput && infoRecipeDtl.IsAttach)) continue;
                if (infoRecipeDtl.LsGroupType == LsGroupType && lastGroupNum == infoRecipeDtl.GroupNum && infoRecipeDtl.UsageId == lastUsageId && infoRecipeDtl.FrequencyId == lastFrequencyId
                    && infoRecipeDtl.Days == lastDays //&& lastRecipeDtlMemo == infoRecipeDtl.RecipeDtlMemo.Trim()
                    || (UsageIdNoForFrequencyDay.Contains(string.Format("[{0}]", infoRecipeDtl.UsageId)) && day > 0))
                    continue;
                Model.BsFrequencyInfo infoFrequency = objFrequency.GetByID(infoRecipeDtl.FrequencyId);
                double thisTimes = infoFrequency.Times;
                if (!infoRecipeDtl.Memo.Trim().Contains("接瓶") && !infoRecipeDtl.Memo.Trim().Contains("分时执行") && lastGroupNum != infoRecipeDtl.GroupNum && DropUsageId.Contains(string.Format("[{0}]", infoRecipeDtl.UsageId)) && infoFrequency.F2.Trim() == "1")  //可以用频率约定是否静滴第一组以后都收接瓶的费用
                {
                    double groupTimes = GetDropGroupNumMaxTime(infoRecipeDtl, lstSortedAdvice, thisTimes);
                    if (groupTimes > 1)
                        InsertFeeDropContinue(infoRecipeDtl, groupTimes - 1);
                    times = 1;
                    lastTimes = thisTimes;
                }
                else
                    times = lastTimes = thisTimes;
                if (times == 0) times = 1;
                if (!(infoRecipeDtl.Memo.Trim().Contains("接瓶加药") && DropUsageId.Contains(string.Format("[{0}]", infoRecipeDtl.UsageId))))
                    lastMemo = InsertFeeByUsage(infoRecipeDtl, times);
                if (lastMemo != string.Empty) lastTimes = 0;
                lastRecipeDtlMemo = infoRecipeDtl.RecipeDtlMemo.Trim();
                lastGroupNum = infoRecipeDtl.GroupNum;
                lastUsageId = infoRecipeDtl.UsageId;
                lastFrequencyId = infoRecipeDtl.FrequencyId;
                lastDays = infoRecipeDtl.Days;
                LsGroupType = infoRecipeDtl.LsGroupType;
            }
            AddContinue(lstAddContinue);
            if (lstSortedAdvice.Count > 0)
                Tools.Utils.TraceFunctionOperate(198);
        }
        public bool BeginEndTime()
        {
            DateTime dt = BLL.Common.DateTimeHandler.GetServerDateTime();
            double currentHour = dt.TimeOfDay.TotalHours;
            if (Model.Configuration.UserProfiles.HospitalID == Convert.ToInt32(BLL.Common.Utils.GetSystemSetting("CenterHospitalId")) && currentHour > Convert.ToDouble(BLL.Common.Utils.GetSystemSetting("BeginTime")) && currentHour < Convert.ToDouble(BLL.Common.Utils.GetSystemSetting("EndTime")))
                return true;
            else
                return false;
        }
        private double GetAloneTotality(Model.uspOuRecipeDtlForOuInvoiceDtlQry infoRecipeDtl)
        {
            double DoubleTimes = 1;
            if (DropUsageId.Contains(string.Format("[{0}]", infoRecipeDtl.UsageId)) && AloneTotalityItemId.Contains(string.Format("[{0}]", infoRecipeDtl.ItemId)))
            {
                Model.BsItemInfo infoBsItem = BLL.Common.Utils.GetBaseTableRowInfo<Model.BsItemInfo>("BsItem", infoRecipeDtl.ItemId);
                Model.OuRecipeDtlInfo OuRecipeDtlTemp = _bllRecipeDtl.GetByID(infoRecipeDtl.RecipeItemId);
                DoubleTimes = Math.Ceiling(OuRecipeDtlTemp.Dosage / infoBsItem.LimitTotalZy);
            }
            return DoubleTimes > 1 ? DoubleTimes : 1;
        }
        Model.ModelList<Model.BsUsageItemInfo> lstToFeeFromMemo = new Model.ModelList<Model.BsUsageItemInfo>();
        private void AddContinue(Model.ModelList<Model.uspOuRecipeDtlForOuInvoiceDtlQry> lstInitAdvice)
        {
            BLL.CBsFrequency objBsFrequency = new BLL.CBsFrequency();
            lstToFeeFromMemo = new Model.ModelList<Model.BsUsageItemInfo>();
            Model.ModelList<Model.ComputeSummary> lstGroup = lstInitAdvice.GroupBy("GroupNum", "GroupNum", "RecipeItemId", Model.ComputeType.Max);
            Model.ModelList<Model.uspOuRecipeDtlForOuInvoiceDtlQry> lstAdviceGroup = new Model.ModelList<Model.uspOuRecipeDtlForOuInvoiceDtlQry>();
            BLL.CBsUsageItem objBsUsageItem = new BLL.CBsUsageItem();
            foreach (Model.ComputeSummary infoGroup in lstGroup)
            {
                Model.ModelList<Model.uspOuRecipeDtlForOuInvoiceDtlQry> lstTempGroup = lstInitAdvice.Find("GroupNum", infoGroup.GroupBy.ToString());
                double times = objBsFrequency.GetByID(lstTempGroup[0].FrequencyId).Times;
                if (!(lstTempGroup[0].IsDoctorInput && lstTempGroup[0].IsAttach)) continue;
                //收除了备注为接瓶加药还有别外的收费项目，如接瓶加药，避光
                InsertOtherDropFee(lstTempGroup, times);

                bool isFirsrDropGroup = CheckFirsrDropGroup(lstTempGroup);

                string strMemo = XYHIS.Common.Helper.GetDropUsageIdMemo<Model.uspOuRecipeDtlForOuInvoiceDtlQry>(lstTempGroup);

                Model.ModelList<Model.BsUsageItemInfo> lstBsUsageItem = objBsUsageItem.BsUsageItem_SelectByUsageId(lstTempGroup[0].UsageId).Find("Memo", "接瓶加药");
                if (times > 0 && !isFirsrDropGroup)   //备注为接瓶加药则必加收接滴费
                {
                    foreach (Model.BsUsageItemInfo infoUsageItem in lstBsUsageItem)
                    {
                        if (infoUsageItem.LsUseArea == 2) continue;//过滤门诊
                        if (IsToFeeFromMemo(infoUsageItem, lstToFeeFromMemo, strMemo)) continue;
                        AddOneCustFee(infoUsageItem.ItemId, 0, times, "接滴加收", Convert.ToInt32(infoGroup.Description), string.Empty, false, 0);
                    }
                    foreach (Model.BsUsageItemInfo info in lstToFeeFromMemo)
                        AddOneCustFee(info.ItemId, 0, times, "接滴加收", Convert.ToInt32(infoGroup.Description), string.Empty, false, 0);
                }
            }
        }
        private void InsertOtherDropFee(Model.ModelList<Model.uspOuRecipeDtlForOuInvoiceDtlQry> lstTempGroup, double times)
        {
            string Memo = string.Empty;
            char[] split = { ',', '，' };
            BLL.CBsUsageItem objBsUsageItem = new BLL.CBsUsageItem();
            Model.ModelList<Model.BsUsageItemInfo> lstBsUsageItem = new Model.ModelList<Model.BsUsageItemInfo>();
            Memo = XYHIS.Common.Helper.GetDropUsageIdMemo(lstTempGroup);
            if (Memo.Trim().Length == 0) return;
            string[] strMemo = Memo.Split(split);
            for (int i = 0; i < strMemo.Length; i++)
            {
                if (strMemo[i].Trim().Length == 0) continue;
                if (strMemo[i].Trim().Contains("皮试")) continue;
                lstBsUsageItem = objBsUsageItem.BsUsageItem_SelectByUsageId(lstTempGroup[0].UsageId).Find("Memo", strMemo[i]);
                foreach (Model.BsUsageItemInfo infoUsageItem in lstBsUsageItem)
                {
                    if (infoUsageItem.LsUseArea == 2) continue;//过滤门诊
                    AddOneCustFee(infoUsageItem.ItemId, 0, times * infoUsageItem.Totality, strMemo[i], lstTempGroup[0].RecipeItemId, string.Empty, false, 0);
                }
            }
        }
        private bool CheckFirsrDropGroup(Model.ModelList<Model.uspOuRecipeDtlForOuInvoiceDtlQry> lstTempGroup)
        {
            if (lstTempGroup.Count == 0) return false;
            foreach (Model.uspOuRecipeDtlForOuInvoiceDtlQry infoAdvice in lstTempGroup)
            {
                if (!DropUsageId.Contains(string.Format("[{0}]", infoAdvice.UsageId))) return false;
                if (infoAdvice.Memo.Contains("接瓶加药"))
                    return false;
            }
            return true;
        }
        private double GetDropGroupNumMaxTime(Model.uspOuRecipeDtlForOuInvoiceDtlQry infoRecipeDtl, Model.ModelList<Model.uspOuRecipeDtlForOuInvoiceDtlQry> lstSortedAdvice, double thisTimes)
        {
            Model.ModelList<Model.uspOuRecipeDtlForOuInvoiceDtlQry> lstAdviceTodayNormal = lstSortedAdvice.Find("GroupNum", infoRecipeDtl.GroupNum.ToString());
            double maxTime = 0;
            BLL.CBsFrequency objBsFrequency = new BLL.CBsFrequency();
            foreach (Model.uspOuRecipeDtlForOuInvoiceDtlQry info in lstAdviceTodayNormal)
            {
                double time;
                if (infoRecipeDtl.ID == info.ID)
                    time = thisTimes;
                else
                {
                    int frequecyId = info.FrequencyId;
                    time = objBsFrequency.GetByID(frequecyId).Times;
                    if (time == 0) continue;
                }
                if (time > maxTime)
                    maxTime = time;
            }
            return maxTime;
        }
        private void InsertFeeDropContinue(Model.uspOuRecipeDtlForOuInvoiceDtlQry infoRecipeDtl, double continueTimes)
        {
            BLL.CBsUsageItem objUsageItem = new BLL.CBsUsageItem();
            lstToFeeFromMemo = new Model.ModelList<Model.BsUsageItemInfo>();
            Model.ModelList<Model.uspOuRecipeDtlForOuInvoiceDtlQry> lstOuRecipeDtlForOuInvoiceDtl = _lstUspOuRecipeDtlForOuInvoiceDtl.Find("GroupNum", infoRecipeDtl.GroupNum.ToString()).Find("UsageId", infoRecipeDtl.UsageId.ToString());
            string strMemo = XYHIS.Common.Helper.GetDropUsageIdMemo(lstOuRecipeDtlForOuInvoiceDtl);
            Model.ModelList<Model.BsUsageItemInfo> lstUsageItem = objUsageItem.BsUsageItem_SelectByUsageId(infoRecipeDtl.UsageId).Find("Memo", "接瓶加药");
            foreach (Model.BsUsageItemInfo infoUsageItem in lstUsageItem)
            {
                if (infoUsageItem.LsUseArea == 2) continue;
                if (infoUsageItem.LocationId > 0 && infoUsageItem.LocationId != this.hisOuHosInfo1.Value.DiagnDept) continue;
                if (infoUsageItem.F1.Contains(this.hisOuHosInfo1.Value.DiagnDept.ToString())) continue;
                if (infoUsageItem.IsBaby && this.hisOuHosInfo1.Value.Age > 10) continue;
                if (infoUsageItem.IsOlder && this.hisOuHosInfo1.Value.Age < 60) continue;
                if (infoUsageItem.IsWomen && this.hisOuHosInfo1.Value.Sex == "男") continue;
                if (infoUsageItem.IsMen && this.hisOuHosInfo1.Value.Sex == "女") continue;
                if (IsToFeeFromMemo(infoUsageItem, lstToFeeFromMemo, strMemo)) continue;
                AddOneCustFee(infoUsageItem.ItemId, 0, continueTimes, "接瓶加药", infoRecipeDtl.RecipeItemId, string.Empty, false, 0);
            }
            foreach (Model.BsUsageItemInfo info in lstToFeeFromMemo)
                AddOneCustFee(info.ItemId, 0, continueTimes, "接瓶加药", infoRecipeDtl.RecipeItemId, string.Empty, false, 0);
        }
        string SkinDrugItemId = BLL.Common.Utils.GetSystemSetting("SkinDrugItemId");
        private void InsertSkinDrug(Model.uspOuRecipeDtlForOuInvoiceDtlQry infoRecipeDtl)
        {
            BLL.CBsUsageItem objUsageItem = new BLL.CBsUsageItem();
            Model.ModelList<Model.BsUsageItemInfo> lstUsageItem = objUsageItem.BsUsageItem_SelectByUsageId(infoRecipeDtl.UsageId);
            foreach (Model.BsUsageItemInfo infoUsageItem in lstUsageItem)
            {
                if (!infoUsageItem.Memo.Contains("皮试")) continue;
                if (!infoRecipeDtl.Memo.Contains(infoUsageItem.Memo)) continue;
                if (infoUsageItem.LsUseArea == 2) continue;
                if (infoUsageItem.ItemId.ToString() == SkinDrugItemId && _lstUspOuInvoiceDtl.Find("ItemId", SkinDrugItemId).FilterInclude("Memo", "皮试", false).Count > 0) continue;
                AddOneCustFee(infoUsageItem.ItemId, 0, infoUsageItem.Totality > 1 ? infoUsageItem.Totality : 1, infoUsageItem.Memo, infoRecipeDtl.RecipeItemId, string.Empty, false, 0);
            }
        }
        int _lastGroupUsageId = 0;
        int DropUsageIdOneDrugItemId = Convert.ToInt32(BLL.Common.Utils.GetSystemSetting("OneDrugDropNotFeeItemId"));
        string ToFeeFromMemoItemIds = BLL.Common.Utils.GetSystemSetting("ToFeeFromMemoItemIds");
        private string InsertFeeByUsage(Model.uspOuRecipeDtlForOuInvoiceDtlQry infoRecipeDtl, double times)
        {
            bool IsDropUsageIdOneDrug = false;
            BLL.CBsUsageItem objUsageItem = new BLL.CBsUsageItem();
            BLL.CBsUsage objUsage = new BLL.CBsUsage();
            Model.ModelList<Model.BsUsageItemInfo> lstUsageItem = objUsageItem.BsUsageItem_SelectByUsageId(infoRecipeDtl.UsageId).Find("Memo", infoRecipeDtl.RecipeDtlMemo);
            if (lstUsageItem.Count == 0) return "";
            lstToFeeFromMemo = new Model.ModelList<Model.BsUsageItemInfo>();
            Model.ModelList<Model.uspOuRecipeDtlForOuInvoiceDtlQry> lstOuRecipeDtlForOuInvoiceDtl = _lstUspOuRecipeDtlForOuInvoiceDtl.Find("GroupNum", infoRecipeDtl.GroupNum.ToString()).Find("UsageId", infoRecipeDtl.UsageId.ToString());
            string strMemo = XYHIS.Common.Helper.GetDropUsageIdMemo(lstOuRecipeDtlForOuInvoiceDtl);
            string special = string.Empty;
            string memoSpecial = string.Empty;
            string memoTemp = string.Format("，{0}，", infoRecipeDtl.RecipeDtlMemo);
            memoTemp = memoTemp.Replace(",", "，");
            memoTemp = memoTemp.Replace("接瓶加药", string.Empty);
            foreach (Model.BsUsageItemInfo infoSearch in lstUsageItem)
            {
                string UsageMemo = string.Format("，{0}，", infoSearch.Memo.Trim());
                if (infoSearch.Memo.Trim() != string.Empty && memoTemp.Contains(UsageMemo) && !memoSpecial.Contains(UsageMemo))
                    special = string.Format("{0}，{1}", special, infoSearch.Memo.Trim()); //本处方是要求特殊情况
                memoSpecial = special;
                if (!memoSpecial.StartsWith("，")) memoSpecial = string.Format("，{0}", memoSpecial);
                if (!memoSpecial.EndsWith("，")) memoSpecial = string.Format("{0}，", memoSpecial);
            }
            if (special.StartsWith("，"))
                special = special.Substring(1);
            string AloneExecUsageId = BLL.Common.Utils.GetSystemSetting("AloneExecUsageId");
            if (!infoRecipeDtl.Memo.Trim().Contains("接瓶") && DropUsageId.Contains(string.Format("[{0}]", infoRecipeDtl.UsageId)) && _lstUspOuRecipeDtlForOuInvoiceDtl.Find("LsGroupType", infoRecipeDtl.LsGroupType.ToString()).Find("GroupNum", infoRecipeDtl.GroupNum.ToString()).Count == 1)
            {
                IsDropUsageIdOneDrug = true;
            }
            foreach (Model.BsUsageItemInfo infoUsageItem in lstUsageItem)
            {
                string UsageMemo = string.Format("，{0}，", infoUsageItem.Memo.Trim());
                if (infoUsageItem.LsUseArea == 2) continue;
                if (infoUsageItem.LocationId > 0 && infoUsageItem.LocationId != this.hisOuHosInfo1.Value.DiagnDept) continue;
                if (infoUsageItem.F1.Contains(this.hisOuHosInfo1.Value.DiagnDept.ToString())) continue;
                if (infoUsageItem.IsBaby && this.hisOuHosInfo1.Value.Age > 10) continue;
                if (infoUsageItem.IsOlder && this.hisOuHosInfo1.Value.Age < 60) continue;
                if (infoUsageItem.IsWomen && this.hisOuHosInfo1.Value.Sex == "男") continue;
                if (infoUsageItem.IsMen && this.hisOuHosInfo1.Value.Sex == "女") continue;
                if (special == string.Empty && infoUsageItem.Memo.Trim() != string.Empty) continue;
                if (special != string.Empty && infoUsageItem.Memo.Trim() != string.Empty && !memoSpecial.Contains(UsageMemo)) continue;
                if (infoUsageItem.Memo.Trim() == string.Empty && infoRecipeDtl.UsageId == _lastGroupUsageId && !AloneExecUsageId.Contains(string.Format("[{0}]", infoRecipeDtl.UsageId)) && !memoTemp.Contains("单独执行")) continue;
                if (special != string.Empty && UsageMemo != string.Empty && UsageMemo != "，，" && !UsageMemo.Contains(special)) continue;
                //if (infoUsageItem.Memo.Trim().Contains("皮试")) continue;
                if (UsageIdNoForFrequencyDay.Contains(string.Format("[{0}]", infoRecipeDtl.UsageId))) times = 1;
                if (DropUsageId.Contains(string.Format("[{0}]", infoRecipeDtl.UsageId)) && infoUsageItem.ItemId == DropUsageIdOneDrugItemId && _lstUspOuRecipeDtlForOuInvoiceDtl.Find("GroupNum", infoRecipeDtl.GroupNum.ToString()).Find("RecipeId", infoRecipeDtl.RecipeId.ToString()).Count == 1 && !infoRecipeDtl.Memo.Contains("接瓶"))
                    continue;
                double feeTimes = infoUsageItem.Totality * times;
                if (infoUsageItem.F2 == "1")
                    feeTimes = infoRecipeDtl.Totality;
                if (IsToFeeFromMemo(infoUsageItem, lstToFeeFromMemo, strMemo)) continue;
                AddOneCustFee(infoUsageItem.ItemId, 0, feeTimes, string.Format("{0}，{1}", objUsage.GetByID(infoRecipeDtl.UsageId).Name, special), infoRecipeDtl.RecipeItemId, string.Empty, false, 0);
            }
            AddFeeFromMemo(lstToFeeFromMemo, infoRecipeDtl, times, special);
            _lastGroupUsageId = infoRecipeDtl.UsageId;
            return special;
        }
        private bool IsToFeeFromMemo(Model.BsUsageItemInfo infoUsageItem, Model.ModelList<Model.BsUsageItemInfo> lstToFeeFromMemo, string Memo)
        {
            if (ToFeeFromMemoItemIds.Contains(string.Format("[{0}]", infoUsageItem.ItemId)))
            {
                if (infoUsageItem.F3.Trim().Length == 0 && lstToFeeFromMemo.Count == 0)
                {
                    lstToFeeFromMemo.Add(infoUsageItem);
                }
                if (infoUsageItem.F3.Trim().Length > 0 && Memo.Contains(infoUsageItem.F3.Trim()))
                {
                    for (int i = 0; i < lstToFeeFromMemo.Count; i++)
                    {
                        if (lstToFeeFromMemo[i].F3.Trim().Length == 0 || lstToFeeFromMemo[i].F3.Trim().Length > 0 && !Memo.Contains(lstToFeeFromMemo[i].F3.Trim()))
                        {
                            lstToFeeFromMemo.Remove(lstToFeeFromMemo[i]);
                            i--;
                        }
                    }
                    lstToFeeFromMemo.Add(infoUsageItem);
                }
                return true;
            }
            else return false;
        }
        private void AddFeeFromMemo(Model.ModelList<Model.BsUsageItemInfo> lstToFeeFromMemo, Model.uspOuRecipeDtlForOuInvoiceDtlQry infoRecipeDtl, double times, string special)
        {
            BLL.CBsUsage objUsage = new BLL.CBsUsage();
            foreach (Model.BsUsageItemInfo infoUsageItem in lstToFeeFromMemo)
            {
                if (infoUsageItem.LsUseArea == 2) continue;
                if (infoUsageItem.LocationId > 0 && infoUsageItem.LocationId != this.hisOuHosInfo1.Value.DiagnDept) continue;
                if (infoUsageItem.F1.Contains(this.hisOuHosInfo1.Value.DiagnDept.ToString())) continue;
                if (infoUsageItem.IsBaby && this.hisOuHosInfo1.Value.Age > 10) continue;
                if (infoUsageItem.IsOlder && this.hisOuHosInfo1.Value.Age < 60) continue;
                if (infoUsageItem.IsWomen && this.hisOuHosInfo1.Value.Sex == "男") continue;
                if (infoUsageItem.IsMen && this.hisOuHosInfo1.Value.Sex == "女") continue;
                if (infoUsageItem.Memo.Trim().Contains("皮试")) continue;
                double feeTimes = infoUsageItem.Totality * times;
                if (infoUsageItem.F2 == "1")
                    feeTimes = infoRecipeDtl.Totality;
                AddOneCustFee(infoUsageItem.ItemId, 0, feeTimes, string.Format("{0}，{1}", objUsage.GetByID(infoRecipeDtl.UsageId).Name, special), infoRecipeDtl.RecipeItemId, string.Empty, false, 0);
            }
        }
        private void SetExecLocId()
        {
            BLL.COuInvoice objOuInvoice = new BLL.COuInvoice();
            BLL.COuInvoiceDtl objOuInvoiceDtl = new BLL.COuInvoiceDtl();
            Model.ModelList<Model.OuInvoiceInfo> lstOuInvoice = objOuInvoice.OuInvoice_SelectByMzRegId(this.hisOuHosInfo1.Value.ID).Find("IsCancel", "False");
            Model.ModelList<Model.OuInvoiceDtlInfo> lstOuInvoiceDtl = objOuInvoice.GetMutiChild<Model.OuInvoiceDtlInfo, BLL.COuInvoiceDtl>(lstOuInvoice.ConvertToBase(), "InvoId");//.Find("LsPerform", "1");
            foreach (Model.uspOuRecipeDtlForOuInvoiceDtlQry infoOuRecipeDtl in _lstUspOuRecipeDtlForOuInvoiceDtl)
            {
                BLL.CBsXdRpDtl objBsXdRpDtl = new BLL.CBsXdRpDtl();
                Model.ModelList<Model.BsXdRpDtlInfo> lstXdRpDtl = objBsXdRpDtl.BsXdRpDtl_SelectByXdRpId(infoOuRecipeDtl.XDRpId);
                foreach (Model.uspOuInvoiceDtlQry infoOuInvoiceDtl in _lstUspOuInvoiceDtl)
                {
                    if (infoOuRecipeDtl.RecipeItemId == infoOuInvoiceDtl.RecipeItemId)
                    {
                        if (infoOuRecipeDtl.XDRpId > 0 && lstXdRpDtl.Find("ItemId", infoOuInvoiceDtl.ItemId.ToString()).Count > 0)
                        {
                            Model.BsXdRpDtlInfo infoXdRpDtl = lstXdRpDtl.Find("ItemId", infoOuInvoiceDtl.ItemId.ToString())[0];
                            infoOuInvoiceDtl.ExecLocId = infoXdRpDtl.ExecLocId;
                        }
                        if (infoOuInvoiceDtl.LsRpType != 3)
                            infoOuInvoiceDtl.ExecLocId = GetDefeultRoomLocationId(1, infoOuInvoiceDtl.ItemId);
                        else if (infoOuInvoiceDtl.LsRpType == 3)
                            infoOuInvoiceDtl.ExecLocId = GetDefeultRoomLocationId(2, infoOuInvoiceDtl.ItemId);
                        if (infoOuInvoiceDtl.ExecLocId == 0)
                            infoOuInvoiceDtl.ExecLocId = infoOuRecipeDtl.LocationId > 0 ? infoOuRecipeDtl.LocationId : this.hisOuHosInfo1.Value.DiagnDept;

                        Model.ModelList<Model.OuInvoiceDtlInfo> lstExecLoc = lstOuInvoiceDtl.Find("ItemId", infoOuInvoiceDtl.ItemId.ToString());
                        lstExecLoc.Sort("ID");
                        lstExecLoc.Reverse();
                        if (lstExecLoc.Count > 0)
                            infoOuInvoiceDtl.ExecLocId = lstExecLoc[0].ExecLocId;
                    }
                }
            }
        }
        /// <summary>
        /// 退药处理
        /// </summary>
        private void HandleBackDrug()
        {
            double executedDrugNum;
            double Totality;
            //对护士已经执行的（部分退药），则补收已经执行的数量，其他情况，不再收取病人费用
            foreach (Model.uspOuInvoiceDtlQry infoUspOuInvoiceDtl in _lstUspOuInvoiceDtl)
            {
                if (infoUspOuInvoiceDtl.IsBack)
                {
                    object drugNum = BLL.absBusiness<Model.absModel>.ExecuteScalar("uspCalOuExecutedDrugNum", infoUspOuInvoiceDtl.RecipeItemId.ToString(), string.Empty);
                    if (drugNum is System.DBNull)
                        drugNum = 0;
                    executedDrugNum = Convert.ToDouble(drugNum);
                    if (executedDrugNum != 0)
                    {
                        infoUspOuInvoiceDtl.Totality = executedDrugNum;   //如果不是部分执行的，则应该是零
                        infoUspOuInvoiceDtl.Amount = BLL.Common.Utils.Round(infoUspOuInvoiceDtl.Totality * infoUspOuInvoiceDtl.Price, 2);
                    }
                }
                if (infoUspOuInvoiceDtl.IsBack)
                {
                    executedDrugNum = Convert.ToDouble(BLL.absBusiness<Model.absModel>.ExecuteScalar("uspCalOuRecipeDrugNum", infoUspOuInvoiceDtl.RecipeItemId.ToString(), string.Empty));
                    //infoUspOuInvoiceDtl.Totality -= executedDrugNum;   //如果不是部分执行的，则应该是零
                    Totality = Convert.ToDouble(BLL.absBusiness<Model.absModel>.ExecuteScalar("uspCalOuRecipeF1", infoUspOuInvoiceDtl.RecipeItemId.ToString(), string.Empty));
                    infoUspOuInvoiceDtl.Totality = Totality - executedDrugNum;
                    infoUspOuInvoiceDtl.Amount = BLL.Common.Utils.Round(infoUspOuInvoiceDtl.Totality * infoUspOuInvoiceDtl.Price, 2);
                }
                infoUspOuInvoiceDtl.Price = BLL.Common.Utils.GetLocationPrice(infoUspOuInvoiceDtl.ItemId, this.hisOuHosInfo1.Value.DiagnDept, 1);
                infoUspOuInvoiceDtl.Amount = BLL.Common.Utils.Round(infoUspOuInvoiceDtl.Totality * infoUspOuInvoiceDtl.Price, 2);
                if (BLL.Common.Utils.CheckSettingContain("OuChargeInputPriceItemIds", infoUspOuInvoiceDtl.ItemId, false))
                {
                    infoUspOuInvoiceDtl.Price = infoUspOuInvoiceDtl.Totality;
                    infoUspOuInvoiceDtl.Totality = 1;
                    infoUspOuInvoiceDtl.Amount = BLL.Common.Utils.Round(infoUspOuInvoiceDtl.Totality * infoUspOuInvoiceDtl.Price, 2);
                }
            }
            _lstUspOuInvoiceDtl.Remove("Totality", "0");   //删除全部退药的
        }

        ///
        /// 默认收代煎费用
        /// 
        private void GetCookCharge()
        {
            int diagCookChargeId = Convert.ToInt32(BLL.Common.Utils.GetSystemSetting("CoctByHospitalFeeId"));
            Model.BsItemInfo infoItem = BLL.Common.Utils.GetBaseTableRowInfo<Model.BsItemInfo>("BsItem", diagCookChargeId);
            if (infoItem.ID == 0) return;
            double PriceDiag = infoItem.PriceDiag;
            BLL.COuRecipeChin objOuRecipeChin = new BLL.COuRecipeChin();
            foreach (Model.OuRecipeInfo infoChineseRecip in _lstChineseRecipe)
            {
                Model.ModelList<Model.OuRecipeChinInfo> lstOuRecipeChin = objOuRecipeChin.OuRecipeChin_SelectByRecipeId(infoChineseRecip.ID);
                if (lstOuRecipeChin.Count > 0 && lstOuRecipeChin[0].LsCookSelf == (int)Model.EnumCookSelf.CoctByHospital)
                    AddOneCustFee(diagCookChargeId, PriceDiag, Convert.ToDouble(lstOuRecipeChin[0].DecoctNum), "中药代煎费用", infoChineseRecip.ID, string.Empty, false, 0);
            }
        }
        Model.ModelList<Model.uspOuRecipeDtlForOuInvoiceDtlQry> _lstUspOuRecipeDtlTest;
        Model.ModelList<Model.uspOuRecipeDtlForOuInvoiceDtlQry> _lstUspOuRecipeDtlCheck;
        Model.ModelList<Model.uspOuRecipeDtlForOuInvoiceDtlQry> _lstUspOuRecipeDtlCure;
        Model.ModelList<Model.uspOuRecipeDtlForOuInvoiceDtlQry> _lstUspOuRecipeDtlOPS;
        private void TestMemo()
        {
            for (int i = 0; i < _lstUspOuRecipeDtlForOuInvoiceDtl.Count; i++)
            {
                if (_lstUspOuRecipeDtlForOuInvoiceDtl[i].LsRpType == 4)
                {
                    _lstUspOuRecipeDtlTest.Add(_lstUspOuRecipeDtlForOuInvoiceDtl[i]);
                    _lstUspOuRecipeDtlForOuInvoiceDtl.Remove(_lstUspOuRecipeDtlForOuInvoiceDtl[i]);
                    i--;
                }
                else if (_lstUspOuRecipeDtlForOuInvoiceDtl[i].LsRpType == 5)
                {
                    _lstUspOuRecipeDtlCheck.Add(_lstUspOuRecipeDtlForOuInvoiceDtl[i]);
                    _lstUspOuRecipeDtlForOuInvoiceDtl.Remove(_lstUspOuRecipeDtlForOuInvoiceDtl[i]);
                    i--;
                }
                else if (_lstUspOuRecipeDtlForOuInvoiceDtl[i].LsRpType == 6)
                {
                    _lstUspOuRecipeDtlOPS.Add(_lstUspOuRecipeDtlForOuInvoiceDtl[i]);
                    _lstUspOuRecipeDtlForOuInvoiceDtl.Remove(_lstUspOuRecipeDtlForOuInvoiceDtl[i]);
                    i--;
                }
                else if (_lstUspOuRecipeDtlForOuInvoiceDtl[i].LsRpType == 7)
                {
                    _lstUspOuRecipeDtlCure.Add(_lstUspOuRecipeDtlForOuInvoiceDtl[i]);
                    _lstUspOuRecipeDtlForOuInvoiceDtl.Remove(_lstUspOuRecipeDtlForOuInvoiceDtl[i]);
                    i--;
                }
            }
            BLL.CBsXdRp objCBsXdRp = new BLL.CBsXdRp();
            foreach (Model.uspOuRecipeDtlForOuInvoiceDtlQry info in _lstUspOuRecipeDtlForOuInvoiceDtl)
            {
                //如果是检查单，则显示检查单的部位
                BLL.CPsApplyReport objPsApply = new BLL.CPsApplyReport();
                Model.ModelList<Model.PsApplyReportInfo> lstPsApply = new Model.ModelList<Model.PsApplyReportInfo>();
                if (Utilities.Information.IsNumeric(info.F3))
                    lstPsApply = objPsApply.GetDynamic(string.Format("ID={0}", info.F3), "ID");
                if (lstPsApply.Count > 0 && info.Memo == string.Empty)
                {
                    info.Memo = lstPsApply[0].CheckBody;
                    continue;
                }
                if (info.XDRpId > 0)
                {
                    if (info.LsRpType == (int)Model.EnumRpType.Test || info.LsRpType == (int)Model.EnumRpType.Check)
                    {
                        string xDRpIdName = objCBsXdRp.GetByID(info.XDRpId).Name;
                        foreach (Model.uspOuRecipeDtlForOuInvoiceDtlQry infoGroup in _lstUspOuRecipeDtlForOuInvoiceDtl)
                        {
                            if (infoGroup.XDRpId == info.XDRpId)
                                infoGroup.Memo = xDRpIdName;
                        }
                        continue;
                    }

                }
                if (info.Memo.Trim().Contains("接瓶加药") && _lstUspOuRecipeDtlForOuInvoiceDtl.Find("GroupNum", info.GroupNum.ToString()).Count == 1)
                {
                    info.Memo = info.Memo.Replace("接瓶加药", "接瓶");
                    info.RecipeDtlMemo = info.RecipeDtlMemo.Replace("接瓶加药", "接瓶");
                }
            }
        }        /// <summary>
        /// 根据选择的病人选择病人收费的信息
        /// </summary>
        /// <param name="hisInPatient">病人信息</param>
        private void HisOuHosInfo_RecordFound(Model.uspHisOuPatientQry uspHisOuPatient)
        {
            if (uspHisOuPatient == null || uspHisOuPatient.ID == 0)
            {
                Register();
                return;
            }
            if (IsCanAccessThisModule(Model.Configuration.FuctionAccess.Add) && !_lock.Lock(uspHisOuPatient.ID))
            {
                this.hisOuHosInfo1.Clear();
                return;
            }
            if (uspHisOuPatient == null || uspHisOuPatient.ID == 0) return;
            //if (_notReChargeDrugIssuedMzRegNo != string.Empty && _notReChargeDrugIssuedMzRegNo != uspHisOuPatient.MzRegNo)
            //{
            //    if (MessageBox.Show(this, string.Format("您作废了一张已经发药的处方，但没有补收！现在是否补收？\r\n病人流水号：{0}。或通知系统管理员。", _notReChargeDrugIssuedMzRegNo), "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            //    {
            //        this.hisOuHosInfo1.uoupCardNo.FindByHospID(_lstNotReChargeDrugIssued[0].ID);
            //        return;
            //    }
            //}
            if (uspHisOuPatient.DiagnDept == 0)
            {
                //if (MessageBox.Show("该病人还没有医生接诊，是否继续收费?", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel) return;
                this.hisOuHosInfo1.Value.DiagnDept = this.hisOuHosInfo1.Value.RegDept;
                this.hisOuHosInfo1.RefreshData();
            }
            _lstSelectRecipe.Clear();
            _lstSelectItem.Clear();
            this.FormStatus = Model.Configuration.ToolbarStatus.View;
            OpenRecord();
            this.uicItemId.txtBsItem.Enabled = true;
            this.uicItemId.PatID = uspHisOuPatient.PatId;
            if (this.hisOuHosInfo1.Value.DoctorId <= 0)
            {
                Register();
            }
        }
        int _lastMzRegId = 0;
        public override void OpenRecord()
        {
            //ResetAfterCancel();
            base.OpenRecord();
            if (_lastMzRegId != this.hisOuHosInfo1.Value.ID)
            {
                SameItemId = string.Empty;
                _lstSelectItem.Clear();
            }
            this.uicItemId.PatTypeID = this.hisOuHosInfo1.Value.PatTypeId;
            this.uicItemId.LocationID = this.hisOuHosInfo1.Value.DiagnDept;
            if (this.hisOuHosInfo1.Value.PatTypeId == Convert.ToInt32(BLL.Common.Utils.GetSystemSetting("IsNotPrintInvoicePatTypeId")))
                IsNotPrintInvoice = true;
            else
                IsNotPrintInvoice = false;
            InitPatientData();
            if (!isNewOuhospInfo)
                GetPatientOuInvoiceDtlTodayPayed(this.hisOuHosInfo1.Value.PatId);

            ShowMaster();
            SetStyleOfGfOrYb();
            GetFixCharge();
            CalcuateAmountTally();
            InitData();
            //AddNewOuChineseRecipe();
            ShowFeetyGroup();
            ShowInInvoiceGroup();
            this.CurrentBalanceNo = _frmCurrInvo.GetInvoiceNoFromConfigFile();
            this.FormStatus = Model.Configuration.ToolbarStatus.Add;
            SetToolbarItemStatus("InvoiceCancel", false);

            Lock(false);
            this.devGrid1.advBandedGridViewMain.MoveLast();
            this.uicItemId.Focus();
            _lastMzRegId = this.hisOuHosInfo1.Value.ID;
            IsForCharge = false;
        }
        private Model.ModelList<Model.uspOuInvoiceDtlQry> SumFeeItemId(Model.ModelList<Model.uspOuInvoiceDtlQry> lstFind)
        {
            Model.ModelList<Model.ComputeSummary> lstComputeSummary = lstFind.GroupBy("ItemId", "Totality", Model.ComputeType.Sum);
            Model.uspOuInvoiceDtlQry ReturnItemId = new Model.uspOuInvoiceDtlQry();
            Model.ModelList<Model.uspOuInvoiceDtlQry> lstReturnItemId = new Model.ModelList<Model.uspOuInvoiceDtlQry>();
            foreach (Model.ComputeSummary info in lstComputeSummary)
            {
                ReturnItemId = new Model.uspOuInvoiceDtlQry();
                Model.ModelList<Model.uspOuInvoiceDtlQry> lstReturnItemAdd = lstFind.Find("ItemId", info.GroupBy);
                ReturnItemId = lstReturnItemAdd[0];
                ReturnItemId.Totality = Convert.ToDouble(info.Result);
                ReturnItemId.Amount = ReturnItemId.AmountTally = BLL.Common.Utils.Round(ReturnItemId.Price * ReturnItemId.Totality, 2);

                ReturnItemId.AmountSelf = BLL.Common.Utils.Round(ReturnItemId.AmountTally * ReturnItemId.DiscDiag, 2);
                ReturnItemId.AmountFact = ReturnItemId.AmountSelf + ReturnItemId.Amount - ReturnItemId.AmountTally;
                ReturnItemId.AmountPay = BLL.Common.Utils.Round(ReturnItemId.AmountFact * ReturnItemId.DiscSelf, 2);

                ReturnItemId.Memo = Utilities.Information.FilterArrayToString(lstReturnItemAdd.CopyTo("Memo"));
                lstReturnItemId.Add(ReturnItemId);
            }
            return lstReturnItemId;
        }
        int TallyIntegralItemId = Convert.ToInt32(BLL.Common.Utils.GetSystemSetting("TallyIntegralItemId"));
        private void AddTallyIntegral()
        {
            if (!Utilities.Information.IsNumeric(this.utxtTallyNo.Text)) return;
            if (BLL.Common.Utils.GetSystemSetting("OuTallyIntegralPatTypeIds").Contains(string.Format("[{0}]", this.hisOuHosInfo1.Value.PatTypeId)) &&
              _lstUspOuInvoiceDtl.Find("ItemId", TallyIntegralItemId.ToString()).Count == 0)
            {
                double tinyFeeYb = BLL.Common.Utils.CalculateTint(_lstUspOuInvoiceDtl.GetSum("Amount"), Convert.ToInt32(BLL.Common.Utils.GetSystemSetting("InTintNumber")), Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("InTintType")));
                AddOneCustFee(TallyIntegralItemId, tinyFeeYb, 1, "记帐凑整", 0, string.Empty, false, 0);
            }
        }
        private void AddNewOuChineseRecipe()
        {
            _infoOuChineseRecipe = new Model.OuRecipeInfo();
            _lstUspChineseRecipeDtl = new Model.ModelList<Model.uspOuRecipeDtlQry>();
        }
        /// <summary>
        ///　插入凑整费
        /// </summary>
        private void InsertTintFee()
        {
            BLL.Finder<Model.uspBsItemSelectQry> finder = new BLL.Finder<Model.uspBsItemSelectQry>();
            string[] paras = { "PatTypeID", "ID" };
            string[] values = { this.hisOuHosInfo1.Value.PatTypeId.ToString(), IntegralItemId };
            if (_lstUspOuInvoiceDtl.Find("ItemId", IntegralItemId).Count == 0)//如果还没插入凑整费项目
            {
                Model.ModelList<Model.uspBsItemSelectQry> lstTintFee = finder.Find("uspBsItem", paras, values);
                if (lstTintFee.Count == 0)
                {
                    MessageBox.Show("系统没有设置凑整费项目，请与系统管理员联系", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                if (BLL.Common.Utils.Round(System.Math.Abs(_infoOuInvoice.AddFee), 2) < 0.01) return;
                Model.uspOuInvoiceDtlQry newUspInBalanceDtl = new Model.uspOuInvoiceDtlQry();
                _uspOuInvoiceDtlRecipeHelper.SetBsItemObjectInfo(lstTintFee[0], _columnNamesForOuInvoiceDtl, newUspInBalanceDtl);
                newUspInBalanceDtl.ItemId = lstTintFee[0].ID;
                newUspInBalanceDtl.DiscSelf = 1;
                newUspInBalanceDtl.Totality = 1;
                newUspInBalanceDtl.DoctorId = this.hisOuHosInfo1.Value.DoctorId;
                newUspInBalanceDtl.ExecLocId = this.hisOuHosInfo1.Value.DiagnDept;
                newUspInBalanceDtl.Amount = newUspInBalanceDtl.AmountTally = newUspInBalanceDtl.AmountFact = newUspInBalanceDtl.AmountPay = _infoOuInvoice.AddFee;//凑整费
                _lstUspOuInvoiceDtl.Add(newUspInBalanceDtl);
            }
            else
            {
                if (BLL.Common.Utils.Round(System.Math.Abs(_infoOuInvoice.AddFee), 2) < 0.01)
                {
                    _lstUspOuInvoiceDtl.Remove("ItemId", IntegralItemId);
                    return;
                }
                for (int i = 0; i < _lstUspOuInvoiceDtl.Count; i++)
                {
                    if (_lstUspOuInvoiceDtl[i].ItemId == int.Parse(IntegralItemId))
                    {
                        _lstUspOuInvoiceDtl[i].Amount = _lstUspOuInvoiceDtl[i].AmountTally = _lstUspOuInvoiceDtl[i].AmountFact = _lstUspOuInvoiceDtl[i].AmountPay = _infoOuInvoice.AddFee;//凑整费
                        return;
                    }
                }
            }
            Tools.Utils.TraceFunctionOperate(207);
        }
        double _limitTop = 0.00;
        void utxtTallyNo_EditValueChanged(object sender, System.EventArgs e)
        {
            if (_lstUspOuInvoiceDtl.Count == 0 || this.FormStatus == Model.Configuration.ToolbarStatus.View) return;
            if (Utilities.Information.IsNumeric(this.utxtTallyNo.Text))
            {
                _limitTop = Convert.ToDouble(this.utxtTallyNo.Text);
                if (_limitTop == 0 && Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOuChargeNoTallyNoAsSelfPatType"))
                    && MessageBox.Show("您没有输入记账金额，系统将把该病人变成自费病人并重新打开。是否继续？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Toolbar_Click("HangRecipe");
                    OpenRecord();
                }
                else
                {
                    _lstUspOuInvoiceDtl.Remove("ItemId", IntegralItemId);
                    CalcuateAmountTally();
                    this.devGrid1.advBandedGridViewMain.RefreshData();
                }
            }
        }
        /// <summary>
        /// 计算费用总和
        /// </summary>
        private void CalcuateAmountTally()
        {

            if (_infoOuInvoice == null) return;
            _infoOuInvoice.Beprice = 0;
            _infoOuInvoice.PaySelf = 0;
            _infoOuInvoice.FactGet = 0;
            _infoOuInvoice.Insurance = 0;
            _infoOuInvoice.AmountPay = 0;
            _lstUspOuInvoiceInvItemGoupSumQry.Clear();
            _lstUspOuInvoiceFeetyGoupSumQry.Clear();
            if (Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOuLimitTopLocal")) &&
                        Utilities.Information.IsNumeric(this.utxtTallyNo.Text))
                _limitTop = Convert.ToDouble(this.utxtTallyNo.Text);
            if (_limitTop > 0 && _lstUspOuInvoiceDtl.FindLittle("DiscDiag", 1).Count == 0)
                _lstUspOuInvoiceDtl.Fill("DiscDiag", "0");
            else if (this.hisOuHosInfo1.Value.PatTypeId == Convert.ToInt32(BLL.Common.Utils.GetSystemSetting("SelfPatTypeId")))
                _lstUspOuInvoiceDtl.Fill("DiscDiag", "1");
            //else
            //    _lstUspOuInvoiceDtl.Fill("DiscDiag", "1");

            _lstUspOuInvoiceDtl.DefaultView = null;
            Model.ListView<Model.uspOuInvoiceDtlQry> lstvBalance = _lstUspOuInvoiceDtl.DefaultView;
            lstvBalance.Sort("DiscDiag,InvItemId");// 先按自付比例从高到底排序
            if (Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("OrderByDiscInAscendInBalance")))
                lstvBalance.Reverse();//反序
            //Model.ListView<Model.uspOuInvoiceDtlQry> lstvBalance = lstv;// _lstUspOuInvoiceDtl.DefaultView;
            Model.ModelList<Model.ComputeSummary> lstTally = lstvBalance.GroupBy("LimitGroupId", "LimitFee", Model.ComputeType.Max);
            double remainAmountTally = 0;
            Model.ListView<Model.uspOuInvoiceDtlPatientTodayQry> lstvLimitGroupPatientToday = _lstUspOuInvoiceDtlPatientToday.DefaultView;
            //根据不同的限额，计算可记帐等各种费用的值
            for (int i = 0; i < lstTally.Count; i++)
            {
                //获取该限额的项目
                lstvBalance.Filter = string.Format("LimitGroupId={0}", lstTally[i].GroupBy);
                if (lstTally[i].GroupBy == "" || lstTally[i].GroupBy == "0" || lstTally[i].Result == 0)//如果限额组为空，都可记帐
                    remainAmountTally = double.MaxValue;
                //计算每种限额的可记帐总额
                else　　　//如果不为空就取当前记帐限额组的限额
                {
                    remainAmountTally = lstTally[i].Result;
                    //计算今天剩下还可以记帐的余额
                    lstvLimitGroupPatientToday.Filter = string.Format("LimitGroupId={0}", lstTally[i].GroupBy);
                    remainAmountTally -= lstvLimitGroupPatientToday.GetSum("AmountTally");
                }
                //分摊项目的可记帐部分（直到分配完为止）（按先到先分配的原则）
                for (int j = 0; j < lstvBalance.Count; j++)
                {
                    if (lstvBalance[j].ItemId.ToString() == IntegralItemId) continue;       //过滤凑整费
                    if (remainAmountTally > 0)
                        if (remainAmountTally > 0)
                        {
                            if (remainAmountTally > lstvBalance[j].Amount)
                            {
                                lstvBalance[j].AmountTally = BLL.Common.Utils.Round(lstvBalance[j].Amount, 2);
                                remainAmountTally -= lstvBalance[j].Amount;
                            }
                            else
                            {
                                lstvBalance[j].AmountTally = remainAmountTally;
                                remainAmountTally = 0;
                            }
                        }
                        else
                        {
                            lstvBalance[j].AmountTally = 0;
                        }
                    //计算每个收费项目的其他收费信息
                    lstvBalance[j].AmountSelf = BLL.Common.Utils.Round(lstvBalance[j].AmountTally * lstvBalance[j].DiscDiag, 2);
                    lstvBalance[j].AmountFact = lstvBalance[j].AmountSelf + lstvBalance[j].Amount - lstvBalance[j].AmountTally;
                    lstvBalance[j].AmountPay = BLL.Common.Utils.Round(lstvBalance[j].AmountFact * lstvBalance[j].DiscSelf, 2);
                    if (Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOuLimitTopLocal")) &&
                        Utilities.Information.IsNumeric(this.utxtTallyNo.Text) && _limitTop > 0)
                    {
                        double nowInsurance = _infoOuInvoice.Insurance + lstvBalance[j].Amount - lstvBalance[j].AmountFact;
                        if (nowInsurance >= _limitTop)
                        {
                            remainAmountTally = _limitTop - _infoOuInvoice.Insurance;
                            double factTally = BLL.Common.Utils.Round(remainAmountTally / (1 - lstvBalance[j].DiscDiag), 2);
                            double factSelf = BLL.Common.Utils.Round(factTally * lstvBalance[j].DiscDiag, 2);
                            lstvBalance[j].AmountTally = factTally;
                            lstvBalance[j].AmountSelf = factSelf;
                            lstvBalance[j].AmountFact = factSelf + lstvBalance[j].Amount - factTally;
                            lstvBalance[j].AmountPay = BLL.Common.Utils.Round(lstvBalance[j].AmountFact * lstvBalance[j].DiscSelf, 2);
                        }
                    }
                    lstvBalance[j].AmountInsurance = lstvBalance[j].Amount - lstvBalance[j].AmountFact;
                    //计算所有的项目的总金额
                    _infoOuInvoice.Beprice += lstvBalance[j].Amount;
                    _infoOuInvoice.PaySelf += lstvBalance[j].AmountSelf;
                    _infoOuInvoice.FactGet += lstvBalance[j].AmountFact;
                    _infoOuInvoice.AmountPay += lstvBalance[j].AmountPay;
                    _infoOuInvoice.Insurance = _infoOuInvoice.Beprice - _infoOuInvoice.FactGet;
                    int k = _lstUspOuInvoiceFeetyGoupSumQry.IndexOf("FeeId", lstvBalance[j].FeeId.ToString());
                    //如果已经有了同类，直接累加
                    if (k >= 0)
                    {
                        //计算基本分类项目的每个项目的值
                        _lstUspOuInvoiceFeetyGoupSumQry[k].Amount += lstvBalance[j].Amount;
                        _lstUspOuInvoiceFeetyGoupSumQry[k].AmountFact += lstvBalance[j].AmountFact;
                        _lstUspOuInvoiceFeetyGoupSumQry[k].AmountSelf += lstvBalance[j].AmountSelf;
                        _lstUspOuInvoiceFeetyGoupSumQry[k].AmountJZ += lstvBalance[j].Amount - lstvBalance[j].AmountFact;
                        _lstUspOuInvoiceFeetyGoupSumQry[k].AmountTally += lstvBalance[j].AmountTally;
                        _lstUspOuInvoiceFeetyGoupSumQry[k].AmountZF += lstvBalance[j].AmountFact - lstvBalance[j].AmountSelf;
                        _lstUspOuInvoiceFeetyGoupSumQry[k].AmountPay += lstvBalance[j].AmountPay;
                    }
                    //否则新增一个新类
                    else
                    {
                        //计算基本分类项目的每个项目的值
                        Model.uspOuInvoiceFeetyGoupSumQry newUspOuInvoiceFeetyGroupSumQry = new Model.uspOuInvoiceFeetyGoupSumQry();
                        newUspOuInvoiceFeetyGroupSumQry.Amount = lstvBalance[j].Amount;
                        newUspOuInvoiceFeetyGroupSumQry.AmountFact = lstvBalance[j].AmountFact;
                        newUspOuInvoiceFeetyGroupSumQry.AmountSelf = lstvBalance[j].AmountSelf;
                        newUspOuInvoiceFeetyGroupSumQry.AmountJZ = lstvBalance[j].Amount - lstvBalance[j].AmountFact;
                        newUspOuInvoiceFeetyGroupSumQry.AmountTally = lstvBalance[j].AmountTally;
                        newUspOuInvoiceFeetyGroupSumQry.AmountZF = lstvBalance[j].AmountFact - lstvBalance[j].AmountSelf;
                        newUspOuInvoiceFeetyGroupSumQry.FeeId = lstvBalance[j].FeeId;
                        newUspOuInvoiceFeetyGroupSumQry.Name = lstvBalance[j].FeeName;
                        newUspOuInvoiceFeetyGroupSumQry.AmountPay = lstvBalance[j].AmountPay;
                        _lstUspOuInvoiceFeetyGoupSumQry.Add(newUspOuInvoiceFeetyGroupSumQry);
                    }

                    int l = _lstUspOuInvoiceInvItemGoupSumQry.IndexOf("InvItemId", lstvBalance[j].InvItemId.ToString());
                    //计算发票分类的项目值（原理同上）
                    if (l >= 0)
                    {
                        _lstUspOuInvoiceInvItemGoupSumQry[l].Amount += lstvBalance[j].Amount;
                        _lstUspOuInvoiceInvItemGoupSumQry[l].AmountFact += lstvBalance[j].AmountFact;
                        _lstUspOuInvoiceInvItemGoupSumQry[l].AmountSelf += lstvBalance[j].AmountSelf;
                        _lstUspOuInvoiceInvItemGoupSumQry[l].AmountJZ += lstvBalance[j].Amount - lstvBalance[j].AmountFact;
                        _lstUspOuInvoiceInvItemGoupSumQry[l].AmountTally += lstvBalance[j].AmountTally;
                        _lstUspOuInvoiceInvItemGoupSumQry[l].AmountZF += lstvBalance[j].AmountFact - lstvBalance[j].AmountSelf;
                        _lstUspOuInvoiceInvItemGoupSumQry[l].AmountPay += lstvBalance[j].AmountPay;
                    }
                    else
                    {
                        Model.uspOuInvoiceInvItemGoupSumQry newUspOuInvInItemGroupSumQry = new Model.uspOuInvoiceInvItemGoupSumQry();
                        newUspOuInvInItemGroupSumQry.Amount = lstvBalance[j].Amount;
                        newUspOuInvInItemGroupSumQry.AmountFact = lstvBalance[j].AmountFact;
                        newUspOuInvInItemGroupSumQry.AmountSelf = lstvBalance[j].AmountSelf;
                        newUspOuInvInItemGroupSumQry.AmountJZ = lstvBalance[j].Amount - lstvBalance[j].AmountFact;
                        newUspOuInvInItemGroupSumQry.AmountTally = lstvBalance[j].AmountTally; ;
                        newUspOuInvInItemGroupSumQry.AmountZF = lstvBalance[j].AmountFact - lstvBalance[j].AmountSelf;
                        newUspOuInvInItemGroupSumQry.InvItemId = lstvBalance[j].InvItemId;
                        newUspOuInvInItemGroupSumQry.Name = lstvBalance[j].InvMzItemName;
                        newUspOuInvInItemGroupSumQry.AmountPay = lstvBalance[j].AmountPay;
                        _lstUspOuInvoiceInvItemGoupSumQry.Add(newUspOuInvInItemGroupSumQry);
                    }
                }
            }
            //针对门诊收费处没有维护自付比例的情况，不按预设的自付比例结算，而是按最高记帐去结算，最高金额多少就记帐多少
            if (this.hisOuHosInfo1.Value.IsYb && Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOuBalaceLocal")))// && BLL.Common.Utils.CheckSettingContain("YBMTPatTypeId",this.hisOuHosInfo1.Value.PatTypeId))
            {
                double TopInSurance = 0;
                if (Utilities.Information.IsNumeric(this.utxtTallyNo.Text))
                    TopInSurance = Convert.ToDouble(this.utxtTallyNo.Text);
                Model.BsPatTypeInfo infoPatType = objPatType.GetByID(this.hisOuHosInfo1.Value.PatTypeId);
                double ybTally = _infoOuInvoice.Beprice - _infoOuInvoice.PaySelf;
                //_infoOuInvoice.Insurance = BLL.Common.Utils.Round(ybTally * (1 - infoPatType.DiscDiag), 2);
                _infoOuInvoice.Insurance = BLL.Common.Utils.Round(ybTally * infoPatType.DiscDiag, 2);
                if ((_infoOuInvoice.Insurance > TopInSurance || Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOuChargeDiscDiagNotZeroForYB"))) && TopInSurance > 0)
                {
                    _infoOuInvoice.Insurance = TopInSurance;
                    _infoOuInvoice.AmountPay = _infoOuInvoice.Beprice - _infoOuInvoice.Insurance;// - _infoOuInvoice.PaySelf;
                }
                else
                    _infoOuInvoice.AmountPay = BLL.Common.Utils.Round(ybTally * (1 - infoPatType.DiscDiag), 2) + _infoOuInvoice.PaySelf;
            }

            _infoOuInvoice.AddFee = BLL.Common.Utils.CalculateTint(_infoOuInvoice.AmountPay, Convert.ToInt32(BLL.Common.Utils.GetSystemSetting("InTintNumber")), Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("InTintType")));
            if (BLL.Common.Utils.Round(System.Math.Abs(_infoOuInvoice.AddFee), 2) >= 0.01)
            {
                _infoOuInvoice.AddFee = BLL.Common.Utils.Round(_infoOuInvoice.AddFee, 2);
                _infoOuInvoice.AmountPay += _infoOuInvoice.AddFee;
            }
            //ShowLED();
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        public override void InitData()
        {
            //base.InitData();
            BindData(utxtBeprice, _infoOuInvoice);
            BindData(utxtFactGet, _infoOuInvoice);
            BindData(utxtInsurance, _infoOuInvoice);
            BindData(utxtAmountPay, _infoOuInvoice);
            BindData(utxtTallyNo, _infoOuInvoice);
            BindData(utxtInvoNo, _infoOuInvoice);

        }
        private void BindBsItemData(Model.uspOuInvoiceDtlQry info, bool lockKeyDownEven)
        {
            BindData(this.utxtSpec, info);
            BindData(this.utxtName, info);
            BindData(this.utxtUnitDiagName, info);
            BindData(this.utxtPrice, info);
            //BindData(this.utxtTotality, info);
            //if (!lockKeyDownEven) 
            //    BindData(this.uicItemId, info);
        }

        /// <summary>
        /// 初始化控件
        /// </summary>
        public override void InitControl()
        {
            this.devGrid1.Key = "DataGrid.OuChargeRecipe";
            this.usgInvItemType.Key = "DataGrid.OuChargeInvItemType";
            this.usgFeetyType.Key = "DataGrid.OuChargeFeeType";
            this.ubsExecLocId.BaseType = "BsLocation";
            this.ubsExecLocId.BindComboxBox();

            this.hisOuHosInfo1.RecordFound += new Tools.KeyDownEventDelegate(HisOuHosInfo_RecordFound);
            this.devGrid1.advBandedGridViewMain.Columns["ExecLocId"].ColumnEdit.DoubleClick += new EventHandler(ExecLocId_DoubleClick);
            //this.hisOuHosInfo1.LocationID = Model.Configuration.UserProfiles.LocationID;
            this.hisOuHosInfo1.IsToday = true;
            this.uicItemId.FontSize = this.devGrid1.FontSize;
            this.uicItemId.IsAdvice = true;
            this.uicItemId.LsInOut = 1;
            this.uicItemId.IsManualTotality = true;
            this.uicItemId.getXdRpItemEventHandle = GetXdRpItemEventHandle;
            if (System.Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsCtrlSToDetailFee")))
            {
                this.uicItemId.IsShowCtrF = false;
                this.uicItemId.IsXdRpDtl = true;
            }
            SetToolbar();
            SetBsItemControl();

            SetCancelAndBackFeeColorForBalanceDtl();
            InitData();
            this.utxtInvoNo.Properties.ReadOnly = false;
            _frmOuInvoicePay.usgInvItemType.Key = "DataGrid.OuChargeInvItemTypePay";
            if (Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOuLimitTopLocal")))
            {
                this.utxtTallyNo.EditValueChanged += new EventHandler(utxtTallyNo_EditValueChanged);
                this.utxtTallyNo.TextBoxType = Model.EnumTextBoxType.NumAmount;
                this.label4.Text = "最高记账";
                Tools.Utils.TraceFunctionOperate(204);
            }
        }
        private void GetXdRpItemEventHandle(Model.ModelList<Model.tempXdRpDtlItemQry> uspBsXdRpDtailList)
        {
            if (!CheckDiagLocationAndDoctor()) return;
            bool noInsert;
            foreach (Model.tempXdRpDtlItemQry info in uspBsXdRpDtailList)
            {
                if (!CheckIfCanAddNewItem(info.LsRpType, info.ItemId)) return;
                if (!CheckHasDoctorRecipe(info.ItemId)) return;

                if ((info.LsRpType == 1 || info.LsRpType == 2 || info.LsRpType == 3) && this.hisOuHosInfo1.Value.DoctorId > 0 && _infoDoctor.F2 != "1")
                {
                    Utilities.Information.ShowMsgBox("该医生没有处方权，不能输入药品！");
                    return;
                }
                if (_lstSamePatientName.Contains("ItemId", info.ID.ToString()))
                {
                    if (DialogResult.No == MessageBox.Show("该病人同名同一天同一个医生已经收过诊金或挂号费，是否继续？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
                        return;
                }
                noInsert = false;
                foreach (Model.uspOuInvoiceDtlQry infoOuInvoiceDtl in _lstUspOuInvoiceDtl)
                {
                    if (info.LsRpType == 4 && Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsHasLIS"))) continue;
                    if (infoOuInvoiceDtl.ItemId == info.ItemId && infoOuInvoiceDtl.XDRpId == info.XdRpId && infoOuInvoiceDtl.Price == info.PriceDiag)
                    {
                        infoOuInvoiceDtl.Totality += info.FeeTotality;
                        infoOuInvoiceDtl.Amount = BLL.Common.Utils.Round(infoOuInvoiceDtl.Totality * infoOuInvoiceDtl.Price, 2);
                        noInsert = true;
                        continue;
                    }
                }
                if (noInsert) continue;
                Model.uspOuInvoiceDtlQry newInfo = new Model.uspOuInvoiceDtlQry();
                BLL.CRmUnderLine objRmUnderLine = new BLL.CRmUnderLine();
                _uspOuInvoiceDtlRecipeHelper.SetObjectXdRpItemInfo(info, _columnNamesForOuInvoiceDtl, newInfo);
                newInfo.DiscSelf = 1;
                newInfo.DoctorId = this.hisOuHosInfo1.Value.DoctorId;
                if (_lstUspOuInvoiceDtl.Count > 0 && _lstUspOuInvoiceDtl[_lstUspOuInvoiceDtl.Count - 1].InvItemId == info.InvMzItemId)   //如果录入了单项，再录套餐，延续上一个执行
                    newInfo.ExecLocId = this.ubsExecLocId.ID = _lstUspOuInvoiceDtl[_lstUspOuInvoiceDtl.Count - 1].ExecLocId;
                else if (this.ubsExecLocId.ID > 0)  //也可以先输入执行科室后，才能开始录入套餐
                    newInfo.ExecLocId = this.ubsExecLocId.ID;
                else if (info.ExecLocId > 0)   //如果套餐有执行科室
                    newInfo.ExecLocId = this.ubsExecLocId.ID = info.ExecLocId;
                else if (info.LocationId > 0)
                    newInfo.ExecLocId = this.ubsExecLocId.ID = info.LocationId;
                //else if (info.LsRpType < 4)
                //    newInfo.ExecLocId = GetDefeultRoomLocationId(1);
                else if (info.LsRpType != 3)
                    newInfo.ExecLocId = GetDefeultRoomLocationId(1, info.ItemId);
                else if (info.LsRpType == 3)
                    newInfo.ExecLocId = GetDefeultRoomLocationId(2, info.ItemId);
                if (newInfo.ExecLocId == 0)
                    newInfo.ExecLocId = this.ubsExecLocId.ID = this.hisOuHosInfo1.Value.DiagnDept;

                newInfo.Memo += BLL.Common.Utils.GetYbDesc(info.ItemId, this.hisOuHosInfo1.Value.TallyGroupId, string.Empty);
                newInfo.ItemId = info.ItemId;
                newInfo.FeeHsId = BLL.InsertAccount.GetExecLocId(newInfo.ExecLocId, newInfo.ItemId, true);
                newInfo.Totality = info.FeeTotality;
                newInfo.XDRpId = info.XdRpId;
                newInfo.Amount = BLL.Common.Utils.Round(newInfo.Totality * newInfo.Price, 2);
                newInfo.Memo = info.XdRpMemo;
                GetDrugSendMemo(newInfo);
                MarkMutiInputInSamePatient(newInfo.ItemId);

                _lstUspOuInvoiceDtl.Add(newInfo);
            }
            this.devGrid1.advBandedGridViewMain.RefreshData();

            CalcuateAmountTally();
            InitData();
            this.devGrid1.advBandedGridViewMain.MoveLast();
            this.uicItemId.ItemID = 0;
        }
        /// <summary>
        /// 设置已退费的项目和退费项目的显示状态（在费用明细中）
        /// </summary>
        private void SetCancelAndBackFeeColorForBalanceDtl()
        {
            DevExpress.XtraGrid.StyleFormatCondition cn = new DevExpress.XtraGrid.StyleFormatCondition(DevExpress.XtraGrid.FormatConditionEnum.Equal, devGrid1.advBandedGridViewMain.Columns["LsPerform"], 6, 2);
            cn.ApplyToRow = true;
            cn.Appearance.Font = new Font(FontFamily.GenericSerif, devGrid1.FontSize, FontStyle.Strikeout);
            this.devGrid1.advBandedGridViewMain.FormatConditions.Add(cn);

            cn = new DevExpress.XtraGrid.StyleFormatCondition(DevExpress.XtraGrid.FormatConditionEnum.Equal, devGrid1.advBandedGridViewMain.Columns["IsIssue"], 6, true);
            cn.ApplyToRow = true;
            cn.Appearance.ForeColor = Color.DarkRed;
            this.devGrid1.advBandedGridViewMain.FormatConditions.Add(cn);

            this.devGrid1.advBandedGridViewMain.OptionsView.AllowCellMerge = true;
            devGrid1.advBandedGridViewMain.Columns["Memo"].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
        }
        protected override void Toolbar_Click(string key)
        {
            base.Toolbar_Click(key);
            if (!base.isContinue) return;
            //base.Toolbar_Click(key);
            switch (key)
            {
                case "ReprintInvoice"://重打收据
                    ReprintInvoice();
                    break;
                case "ReprintFormerInvoice":
                    ReprintInvoice();
                    break;
                case "NullifyReprint":  //作废重打
                    PrintNewInvoice();
                    break;
                case "DeleteRowAll"://作废
                    DeleteRow(true);
                    break;
                case "DeleteRow"://作废
                    DeleteRow(false);
                    break;
                case "CancelInvoice"://作废
                    Cancel();
                    FrmInBalance_Activated(null, null);
                    this.uicItemId.Focus();
                    break;
                case "CancelPat"://作废
                    CancelPat();
                    FrmInBalance_Activated(null, null);
                    break;
                case "GoToIn"://作废
                    ToInFee();
                    break;
                case "SelectRecipeItem"://设置
                    SelectRecipeItem();
                    break;
                case "LoadOut"://导出医保
                    break;
                case "Balance"://开始收费
                    BeginBalance(false);
                    break;
                case "SetLimite"://限额设置
                    break;
                case "Read"://读取结果
                    break;
                case "Start"://起始发票
                    Start();
                    break;
                case "BatchDiscIn":
                    BatchDiscIn();
                    break;
                case "ManualRecipe":
                    ManualRecipe();
                    break;
                case "BatchDiscSelf":
                    BatchDiscSelf();
                    break;
                case "CstmDiscIn":
                    CstmDisc();
                    break;
                case "CstmSelf":
                    CstmSelf();
                    break;
                case "FixDiscIn":
                    FixDiscIn();
                    break;
                case "HangRecipe"://挂起处方
                    if (_lstUspOuRecipeDtlForOuInvoiceDtl.Find("IsDoctorInput", "True").Count > 0)
                    {
                        this.InformationInMainForm = "医生电子处方不必挂起！请记住病人流水号，下次重新打开即可";
                        return;
                    }
                    SaveReicipeDtl(false, null);
                    ResetAfterCancel();
                    break;
                case "CancelBalance"://撤消操作
                    this.FormStatus = Model.Configuration.ToolbarStatus.View;
                    ResetAfterCancel();
                    this.utxtInvoNo.Properties.ReadOnly = false;
                    break;
                case "Register"://登记病人
                    Register();
                    break;
                case "CancelYBUp"://取消医保结算
                    CancelYBUpSeqNo();
                    break;
                case "ChinDrug"://登记中药处方
                    InputChinDrug();
                    break;
                case "MergeInvoice"://医保收费
                    MergeInvoice();
                    break;
                case "AutoRegPrint"://打印自助缴费发票
                    AutoRegPrint();
                    break;
                case "PrintInsuranceTable"://记帐单
                    if (_infoOuInvoice != null && _infoOuInvoice.ID > 0)
                        PrintInsuranceTable(_infoOuInvoice.ID);
                    break;
                case "PrintMZDrop"://打印静滴
                    GetPrinterRoom();
                    PrintMZCrueDropReject(false, true, false, false);
                    break;
                case "PrintExecLocation"://打印执行通知单
                    PrintExecLocation();
                    break;
                case "PrintMZReject"://打印静滴
                    GetPrinterRoom();
                    PrintMZCrueDropReject(false, false, true, false);
                    break;
                case "PrintMZCrue"://打印肌注
                    GetPrinterRoom();
                    PrintMZCrueDropReject(true, false, false, false);
                    break;
                case "PrintChineseRecipe"://打印处方
                    GetPrinterRoom();
                    PrintOuDrugIssue("OuChinRecipe");
                    break;
                case "PrintWestRecipe"://打印处方
                    GetPrinterRoom();
                    PrintOuDrugIssue("OuWestRecipe");
                    break;
                case "SerchYbPatient":
                    SerchYbPatient();
                    break;
                case "UpYBOuHos"://医保登记
                    if (!UpYBOuHos())
                        MessageBox.Show("" + strYBMessage, "提示", MessageBoxButtons.OK);
                    break;
                case "UpYBFee"://医保费用上传
                    if (!UpYBFee())
                        MessageBox.Show("" + strYBMessage, "提示", MessageBoxButtons.OK);
                    break;
                case "UpYBOuHosBAInfo"://上传门诊医保备案信息
                    if (!UpYBOuHosBAInfo())
                        MessageBox.Show("" + strYBMessage, "提示", MessageBoxButtons.OK);
                    break;
                case "CancelYBOuHos"://取消医保结算
                    CancelYBOuHos();
                    break;
                case "UrgenUpload"://紧急数据上传
                    UrgenUpload();
                    break;
                case "CancelYBBalance"://医保退费
                    CancelYBBalance();
                    break;
                case "BudgetBalance":
                    BudgetBalance("BUDGET");
                    break;
                case "OneCardSolution":
                    OneCardSolution();
                    break;
            }
        }
        private void OneCardSolution()
        {
            try
            {
                YBInterface.CardApplicationDriverInterface objCaDi = new YBInterface.CardApplicationDriverInterface();
                Model.BsPatientInfo infoBsPatient = objCaDi.GetBsPaitent();
                if (infoBsPatient == null) { return; }
                bool isexists = false;
                if (infoBsPatient.CardNo.Length > 0)
                {
                    this.hisOuHosInfo1.uoupCardNo.txtHospitalNo.Text = infoBsPatient.CardNo;
                    this.hisOuHosInfo1.uoupCardNo.SearchField = Model.EnumHisOuPatientSearchField.CardNo;
                    this.hisOuHosInfo1.uoupCardNo.txtHospitalNo.SendKey(new KeyEventArgs(Keys.Enter));
                    isexists = true;
                }
                isexists = !(this.hisOuHosInfo1.Value == null || this.hisOuHosInfo1.Value.ID == 0);
                if ((this.hisOuHosInfo1.Value == null || this.hisOuHosInfo1.Value.ID == 0) && infoBsPatient.Name.Length > 0)
                {
                    this.hisOuHosInfo1.uoupName.txtHospitalNo.Text = infoBsPatient.Name;
                    this.hisOuHosInfo1.uoupName.SearchField = Model.EnumHisOuPatientSearchField.PatientName;
                    this.hisOuHosInfo1.uoupName.txtHospitalNo.SendKey(new KeyEventArgs(Keys.Enter));
                }
                isexists = !(this.hisOuHosInfo1.Value == null || this.hisOuHosInfo1.Value.ID == 0);
                if (isexists)
                {
                    Utilities.Information.ShowMsgBox("未找到病人信息");
                }
            }
            catch (Exception ex)
            {
                Utilities.Information.ShowErrBox(string.Format("读卡错误：{0}", ex.Message));
            }
        }

        private void BudgetBalance(string budget)
        {
            Model.MZYbInterface.GetUpLoadOuFeeInfo infoGetUpLoadOuFee = new Model.MZYbInterface.GetUpLoadOuFeeInfo();
            if (this.hisOuHosInfo1.Value == null) return;
            ybInterface = YBInterface.YbFactory.Create(Convert.ToInt32(this.hisOuHosInfo1.Value.TallyTypeId), 1);
            if (ybInterface == null) return;
            XYHIS.FrmOuChargeHosInfo frm = new FrmOuChargeHosInfo();
            infoPersonInfo = frm.ReadYBCard(ybInterface);
            if (infoPersonInfo == null) return;


            initYbData();

            Model.OuInvoiceInfo budgetInfoOuinvoice = new Model.OuInvoiceInfo();
            Model.ModelList<Model.uspOuInvoiceDtlQry> _infoBudgetUspOuInvoiceDtl = new Model.ModelList<Model.uspOuInvoiceDtlQry>();
            budgetInfoOuinvoice = _infoOuInvoice;
            _infoBudgetUspOuInvoiceDtl = _lstUspOuInvoiceDtl;
            ybInterface.InfoOuInvoice = budgetInfoOuinvoice;
            ybInterface.LstUspOuInvoiceDtl = _lstUspOuInvoiceDtl;
            double inSurance = 0;
            Tools.ProgressForm progressForm = new Tools.ProgressForm(this);
            progressForm.Show();
            progressForm.SetProgressValue(1);
            progressForm.Tip = "请等待，正在连接医保系统……";
            progressForm.Tip = "请等待，正在上传费用……";
            progressForm.SetProgressValue(40);
            infoGetUpLoadOuFee = ybInterface.UpLoadOuFeeJb(budget);
            progressForm.Tip = "上传费用结束……";
            progressForm.SetProgressValue(50);
            Tools.Utils.TraceFunctionOperate(265);
            bool result = infoGetUpLoadOuFee != null ? true : false;
            Model.OuInvoiceInfo aasdad = _infoOuInvoice;
            if (result)
            {
                progressForm.Tip = "医保系统正在预结算，请等待……";
                MessageBox.Show("医保上传成功!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //inSurance = Convert.ToDouble(ybInterface.ReturnF3);
                progressForm.SetProgressValue(95);
                progressForm.Tip = "医保系统预结算结束！！！请核对各项金额情况！！";
                MessageBox.Show(string.Format("病人姓名：{0}\n个人编号：{1}\n参保待遇：{2}\n 总费用：{3}\n统筹支付：{4}\n现金支付：{5}\n原个账余额：{6}\n现账户余额：{7}",
                    this.hisOuHosInfo1.Value.PatientName, infoPersonInfo.PersNo, infoPersonInfo.PTrea, infoGetUpLoadOuFee.BudMoneySum, infoGetUpLoadOuFee.PayMoney, infoGetUpLoadOuFee.BudMoneySum - infoGetUpLoadOuFee.PayMoney, infoGetUpLoadOuFee.PMoneyBef, infoGetUpLoadOuFee.PMoneyAft));
                progressForm.Close();
                //OpenRecord();
            }
            else
            {
                progressForm.SetProgressValue(100);
                progressForm.Close();
                _lock.UnLock();
                return;
            }
        }


        public string CancelYBBalance()
        {
            string result = string.Empty;
            if (this.hisOuHosInfo1.Value == null) return "失败";
            //ybInterface = YBInterface.YbFactory.Create(Convert.ToInt32(this.hisOuHosInfo1.Value.TallyTypeId), 1);
            if (ybInterface == null) ybInterface = YBInterface.YbFactory.Create(Convert.ToInt32(this.hisOuHosInfo1.Value.TallyTypeId), 1);
            XYHIS.FrmOuChargeHosInfo frm = new FrmOuChargeHosInfo();
            infoPersonInfo = frm.ReadYBCard(ybInterface);
            if (infoPersonInfo == null) return "失败";
            //  GetYBRegNo();
            initYbData();
            //if (DialogResult.No == MessageBox.Show(this, "是否要继续取消该病人在HIS系统未有结算信息的记录?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
            //    return;
            infoOuHos = objOuHos.GetByID(this.hisOuHosInfo1.Value.ID);
            //Model.ModelList<Model.YbPatSeqInfo> lstYbPatSeq = bllYBPatSeq.GetDynamic(string.Format("f3='1640116'", this.hisOuHosInfo1.Value.ID), null);

            Model.ModelList<Model.YbPatSeqInfo> lstYbPatSeq = bllYBPatSeq.GetDynamic(string.Format("MzRegId={0} and F4='{1}' and F2='0' ", this.hisOuHosInfo1.Value.ID, _infoOuInvoice.InvoNo), null);//this.hisOuHosInfo1.Value.ID
            foreach (Model.YbPatSeqInfo info in lstYbPatSeq)
            {
                result = ybInterface.CancelUpLoadOuFeeJb(info);
                break;
                //ybMZCommon.CancelMzBalance(infoOuHos, info.YbSeq);
            }
            if (result == string.Empty || result.Contains("费用上传失败"))
            {
                MessageBox.Show("医保退费失败！");
                return "失败";
            }
            else
            {
                MessageBox.Show("医保退费成功！");
                return "成功";
            }
        }

        private void UrgenUpload()
        {
            FrmUrgenUpload frmFrmUrgenUpload = new FrmUrgenUpload();
            frmFrmUrgenUpload.ShowDialog(this);
        }
        /// <summary>
        /// 上传门诊医保备案信息
        /// </summary>
        private bool UpYBOuHosBAInfo()
        {
            if (this.hisOuHosInfo1.Value != null && !this.hisOuHosInfo1.Value.IsYb)
            {
                MessageBox.Show("该病人不属于医保病人！");
                return true;
            }


            XYHIS.YBPTMZCommon ybMZCommon = new XYHIS.YBPTMZCommon();
            infoOuHos = _objOuHosInfo.GetByID(this.hisOuHosInfo1.Value.ID);
            string strMZQuanlify = ybMZCommon.UploadBackInfo(infoOuHos);
            if (ybMZCommon.GetColumnesValue(strMZQuanlify)[0].ToString() == string.Empty)
                MessageBox.Show("成功上传门诊备案信息！", "提示");
            else
            {
                MessageBox.Show("" + ybMZCommon.GetColumnesValue(strMZQuanlify)[0].ToString(), "提示", MessageBoxButtons.OK);
                return false;
            }
            return true;
        }
        string InsuranceYbk = string.Empty;
        private bool UpYBFee()
        {
            if (this.hisOuHosInfo1.Value != null && !this.hisOuHosInfo1.Value.IsYb)
            {
                MessageBox.Show("该病人不属于医保病人！");
                return true;
            }
            IsCancelMzBalance = false;
            InsuranceYbk = string.Empty;
            Model.OuHosInfoInfo infoOuHos = new Model.OuHosInfoInfo();
            BLL.COuHosInfo objOuHos = new BLL.COuHosInfo();
            infoOuHos = objOuHos.GetByID(this.hisOuHosInfo1.Value.ID);
            if (infoOuHos.F5.Trim().Length > 10 && _lstUspOuInvoiceDtl.Count > 0)
            {
                if (!IsCanAccessThisModule(Model.Configuration.FuctionAccess.Add))
                    return false;
                if (!_lock.Lock(this.hisOuHosInfo1.Value.ID)) return true;
                AcceptData();
                if (!CheckValidate()) return true;
                _lstUspOuInvoiceDtl.Remove("ItemId", IntegralItemId);
                this.FormStatus = Model.Configuration.ToolbarStatus.Edit;
                CalcuateAmountTally();

                this.CurrentBalanceNo = _frmCurrInvo.GetInvoiceNoFromConfigFile();
                if (_infoOuInvoice.AmountPay == 0 && !Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsPrintInvoiceNoAmountPay")))
                    this.CurrentBalanceNo = "X" + this.CurrentBalanceNo;
                _infoOuInvoice.InvoNo = this.CurrentBalanceNo;
                Tools.ProgressForm progressForm = new Tools.ProgressForm(this);
                progressForm.Show();
                progressForm.SetProgressValue(1);
                progressForm.Tip = "请等待，正在连接医保系统……";
                _lstUspOuInvoiceDtl.Remove("ItemId", IntegralItemId);
                progressForm.Tip = "请等待，正在上传费用……";
                ybMZCommon.UpLoadOuDtl(_lstUspOuInvoiceDtl, infoOuHos);
                progressForm.SetProgressValue(50);
                progressForm.Tip = "上传费用结束……";
                // 
                string strMzBalance = string.Empty;
                if (ybMZCommon.RetMessages.Contains("成功"))
                {
                    progressForm.SetProgressValue(60);
                    progressForm.Tip = "医保系统正在开始结算，请等待……";
                    //ybMZCommon.MzBalance(infoOuHos, _lstUspOuInvoiceDtl);
                    ybMZCommon.MzBalance(infoOuHos, true, _lstUspOuInvoiceDtl);
                    progressForm.SetProgressValue(95);
                    progressForm.Tip = "医保系统结算结束！！！请核对各项金额情况！！";
                }
                else
                {
                    progressForm.SetProgressValue(100);
                    progressForm.Close();
                    MessageBox.Show(ybMZCommon.RetMessages, "提示");
                    return false;
                }
                progressForm.SetProgressValue(100);
                progressForm.Close();

                if (!ybMZCommon.RetMessages.Contains("-"))
                {

                    _infoOuInvoice.F4 = infoOuHos.F5.ToString();
                    //if (!BLL.Common.Utils.GetSystemSetting("MZJGPatTypeId").Contains(this.hisOuHosInfo1.Value.PatTypeId.ToString()))
                    //{

                    //  double YBLimitAmount = GetYBLimitGroup();
                    _infoOuInvoice.Insurance = ybMZCommon.Insurance;
                    double AmountPay = _infoOuInvoice.Beprice - _infoOuInvoice.Insurance;
                    //if (AmountPay > YBLimitAmount)
                    //{
                    //    AmountPay -= YBLimitAmount;
                    //    _infoOuInvoice.F8 = YBLimitAmount.ToString("0.00");
                    //    _infoOuInvoice.Insurance += YBLimitAmount;
                    //}
                    //else _infoOuInvoice.F8 = string.Empty;
                    _infoOuInvoice.FactGet = AmountPay;
                    _infoOuInvoice.AmountPay = AmountPay;
                    IsCancelMzBalance = true;
                    //插凑整费用


                    //_infoOuInvoice.AddFee = BLL.Common.Utils.CalculateTint(_infoOuInvoice.AmountPay, Convert.ToInt32(BLL.Common.Utils.GetSystemSetting("InTintNumber")), Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("InTintType")), 0.1);
                    //_infoOuInvoice.AddFee = BLL.Common.Utils.CalculateTint(_infoOuInvoice.AmountPay, _infoOuInvoice.AmountPay - OuChargeNotAddFee > 0 ? Convert.ToInt32(BLL.Common.Utils.GetSystemSetting("InTintNumber")) : 1, Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("InTintType")), 0.1);

                    //if (BLL.Common.Utils.Round(System.Math.Abs(_infoOuInvoice.AddFee), 2) >= 0.01)
                    //{
                    //    _infoOuInvoice.AddFee = BLL.Common.Utils.Round(_infoOuInvoice.AddFee, 2);
                    //    _infoOuInvoice.AmountPay += _infoOuInvoice.AddFee;
                    //}

                    //}
                    //else
                    //{
                    //    InsuranceYbk = ybMZCommon.Insurance.ToString();
                    //    if (BLL.Common.Utils.Round(_infoOuInvoice.Beprice - Convert.ToDouble(InsuranceYbk), 2) > 0.01)
                    //    {
                    //        _infoOuInvoice.AddFee = BLL.Common.Utils.CalculateTint(_infoOuInvoice.Beprice - Convert.ToDouble(InsuranceYbk), Convert.ToInt32(BLL.Common.Utils.GetSystemSetting("InTintNumber")), Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("InTintType")),0.1);
                    //        if (BLL.Common.Utils.Round(System.Math.Abs(_infoOuInvoice.AddFee), 2) >= 0.01)
                    //        {
                    //            _infoOuInvoice.AddFee = BLL.Common.Utils.Round(_infoOuInvoice.AddFee, 2);
                    //            _infoOuInvoice.AmountPay += _infoOuInvoice.AddFee;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        _infoOuInvoice.AddFee = 0;
                    //        _infoOuInvoice.AmountPay = _infoOuInvoice.Beprice;
                    //    }
                    //}
                    InitData();
                    //导出医保数据
                    if (SaveOuBalance(false))
                        Lock(true);
                    this.utxtInvoNo.Properties.ReadOnly = false;
                    this.hisOuHosInfo1.Focus();
                }
                else
                {
                    MessageBox.Show(ybMZCommon.RetMessages, "提示");
                    return false;
                }
                _lock.UnLock();
            }
            else
            {
                MessageBox.Show("该确认该病人是否做过医保登记或是否有明细！");
                return false;
            }
            return true;
        }
        private void SerchYbPatient()
        {
            if (this.hisOuHosInfo1.Value == null) return;
            ybInterface = YBInterface.YbFactory.Create(Convert.ToInt32(this.hisOuHosInfo1.Value.TallyTypeId), 2);
            ybInterface.GetInvoiceDtlYbJ(1);
        }
        private void CancelYBOuHos()
        {
            if (DialogResult.No == MessageBox.Show(this, "是否要继续取消该病人在HIS系统未有结算信息的记录?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                return;
            infoOuHos = objOuHos.GetByID(this.hisOuHosInfo1.Value.ID);
            Model.ModelList<Model.YbPatSeqInfo> lstYbPatSeq = bllYBPatSeq.GetDynamic(string.Format("MzRegId={0} and (ybseq not in (select f4 from ouinvoice where ouinvoice.mzregid=YBPATSEQ.Mzregid) or ybseq in (select f4 from ouinvoice where ouinvoice.mzregid=YBPATSEQ.Mzregid and IsCancel=1))", this.hisOuHosInfo1.Value.ID), null);
            foreach (Model.YbPatSeqInfo info in lstYbPatSeq)
            {
                ybMZCommon.CancelMzBalance(infoOuHos, info.YbSeq);
            }
        }
        string strYBMessage = "    错误说明：\r\n   -a	程序出错    \r\n   -b	传入参数非法(参数长度不等、或必填字段为空)    \r\n    -c	该参保号不在居民参保    \r\n    -d	日期格式有误    \r\n    -e	年度已备案普通门诊    \r\n    -f	不享受普通门诊资格    \r\n    -g	存在有效备案信息    \r\n    -h	该医院编号不没有普通门诊登记权限    \r\n    -i	参保类型有误    \r\n    -j	该参保号不在职工参保    \r\n    -k	不存在有效普通门诊备案信息    \r\n    -l	不存在就诊登记信息    \r\n    -m	存在结算信息    \r\n    -n	结算信息已审核    \r\n    -o	缓存明细失败    \r\n    -p	不存在自费明细     \r\n    -q	结算信息不存在    \r\n    -r	不存在查找记录";
        Model.OuHosInfoInfo infoOuHos = new Model.OuHosInfoInfo();
        BLL.COuHosInfo objOuHos = new BLL.COuHosInfo();
        BLL.CYbPatSeq bllYBPatSeq = new BLL.CYbPatSeq();
        BLL.CBsPatType objPatType = new BLL.CBsPatType();
        bool IsCancelMzBalance = false;
        YBMZCommon ybMZCommon = new YBMZCommon();

        /// <summary>
        /// 上传门诊医保登记
        /// </summary>
        private bool UpYBOuHos()
        {
            if (this.hisOuHosInfo1.Value != null && !this.hisOuHosInfo1.Value.IsYb)
            {
                MessageBox.Show("该病人不属于医保病人！");
                return true;
            }

            infoOuHos = objOuHos.GetByID(this.hisOuHosInfo1.Value.ID);
            if (infoOuHos.F5.Trim().Length > 10 && DialogResult.No == MessageBox.Show(string.Format("该病人已经上传过登记信息，医保流水号是：{0}，是否重新上传登记？", infoOuHos.F5), "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)) return true;
            if (bllYBPatSeq.YbPatSeq_SelectByMzRegId(this.hisOuHosInfo1.Value.ID).Filter("F2", string.Empty).Count > 0)
            {

            }
            XYHIS.YBMZCommon ybMZCommon = new XYHIS.YBMZCommon();
            ybMZCommon.GetYBMZInfo(infoOuHos);
            if (ybMZCommon.RetMessages.ToString() == string.Empty)
            {

                string OrganizationCode = ybMZCommon.OrganizationCode;
                string PatTypeName = ybMZCommon.PatTypeName;
                infoOuHos.F5 = ybMZCommon.YbSeq;
                infoOuHos.F6 = GetPatTypeId(PatTypeName);
                infoOuHos.F7 = OrganizationCode;
                ybMZCommon.UpLoadOuHosInfo(infoOuHos);
                if (ybMZCommon.RetMessages.ToString() == string.Empty || ("1,2").Contains(ybMZCommon.RetMessages.ToString()))
                {
                    Model.YbPatSeqInfo infoYbPatSeq = new Model.YbPatSeqInfo();

                    objOuHos.Modify(infoOuHos, null);

                    infoYbPatSeq.MzRegId = infoOuHos.ID;
                    infoYbPatSeq.YbSeq = infoOuHos.F5;
                    infoYbPatSeq.F1 = BLL.Common.DateTimeHandler.GetServerDateTime().ToString();
                    infoYbPatSeq.F3 = OrganizationCode;
                    infoYbPatSeq.F2 = PatTypeName;
                    bllYBPatSeq.Create(infoYbPatSeq, null);
                    MessageBox.Show("成功上传门诊医保登记信息！", "提示");
                }
                else
                {
                    MessageBox.Show("" + ybMZCommon.RetMessages.ToString(), "提示", MessageBoxButtons.OK);
                    return false;
                }
            }
            else
            {
                MessageBox.Show("" + ybMZCommon.RetMessages.ToString(), "提示", MessageBoxButtons.OK);
                return false;
            }
            return true;
        }
        private string GetPatTypeId(string PatTypeName)
        {
            string strPatType = string.Empty;
            string PatTypeIdForMTTNB = BLL.Common.Utils.GetSystemSetting("PatTypeIdForMTTNB");
            string PatTypeIdForMTGXY = BLL.Common.Utils.GetSystemSetting("PatTypeIdForMTGXY");
            if (PatTypeName.Contains("糖尿病"))
                strPatType += PatTypeIdForMTTNB + ",";
            if (PatTypeName.Contains("高血压"))
                strPatType += PatTypeIdForMTGXY + ",";
            return strPatType;
        }
        public void AutoRegPrint()
        {
            ReadyToEdit = false;

            this.CurrentBalanceNo = _frmCurrInvo.GetInvoiceNoFromConfigFile();
            _infoOuInvoice.InvoNo = this.CurrentBalanceNo;
            _objOuInvoice.Modify(_infoOuInvoice, null);
            _frmCurrInvo.CalculateNextInvoNo();
            _frmCurrInvo.SaveInvoNo();

            ReprintInvoice();
        }
        Model.ModelList<Model.OuInvoiceInfo> lstUserOuIninvoice = new Model.ModelList<Model.OuInvoiceInfo>();
        int ViewIndex = 0;
        protected override void Previous()
        {
            SearchIninvoice(1);
        }
        protected override void Next()
        {
            SearchIninvoice(-1);
        }
        /// <summary>
        /// 作废原来发票，打印一个新发票号
        /// </summary>
        public void PrintNewInvoice()
        {
            Model.uspHisOuPatientQry h = this.hisOuHosInfo1.Value;
            ReadyToEdit = false;
            CancelInvoice("");
            ResetAfterCancel();
            hisOuHosInfo1.SearchPatient(h.ID);
            OpenRecord();
            BeginBalance(true);
            ReprintInvoice();

        }
        //根据当前收费员查找最后一张发票的发票号
        public void SearchIninvoice(int num)
        {
            string returnInvoNo = string.Empty;
            lstUserOuIninvoice = new BLL.COuInvoice().GetDynamic(string.Format(" OperId= {0} and OperTime > '{1}' ", Model.Configuration.UserProfiles.UserID, BLL.Common.DateTimeHandler.GetServerDateTime().AddDays(-1).Date), "InvoNo DESC");
            if (lstUserOuIninvoice.Count == 0) return;
            if (num < 0)
            {
                ViewIndex--;
                if (ViewIndex <= 0) ViewIndex = 0;
            }
            else
            {
                ViewIndex++;
                if (ViewIndex >= lstUserOuIninvoice.Count) ViewIndex = lstUserOuIninvoice.Count - 1;
            }
            returnInvoNo = lstUserOuIninvoice[ViewIndex].InvoNo;
            if (returnInvoNo == "" || returnInvoNo == string.Empty) return;
            FindInvoiceByInvoNo(returnInvoNo);
        }

        private void MergeInvoice()
        {
            //if (this.hisOuHosInfo1.Value == null) return;
            FrmMergeInvoice frmGridInput = new FrmMergeInvoice();
            //frmGridInput.MzRegId = this.hisOuHosInfo1.Value.ID;
            frmGridInput.ShowDialog();
            Tools.Utils.TraceFunctionOperate(263);
        }
        private void FixDiscIn()
        {
            double discAmount = Tools.Utils.InputDouble("请输入要一次性补助的金额", "补助金额", string.Empty);
            if (discAmount == 0) return;
            _infoOuInvoice.Remark = string.Format("补助金额：{0}", discAmount);
            bool isBanlaceFixDiscIn = Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsBanlaceFixDiscIn"));
            for (int i = 0; i < _lstUspOuInvoiceDtl.Count; i++)
            {
                if (discAmount <= 0 || _lstUspOuInvoiceDtl[i].LsPerform == 2)
                    continue;
                if (discAmount > _lstUspOuInvoiceDtl[i].Amount)
                {
                    if (isBanlaceFixDiscIn)
                        _lstUspOuInvoiceDtl[i].DiscDiag = 0;
                    else
                        _lstUspOuInvoiceDtl[i].DiscSelf = 0;
                    ReCalculateDetailAmount(_lstUspOuInvoiceDtl[i].DiscDiag, _lstUspOuInvoiceDtl[i].DiscSelf, _lstUspOuInvoiceDtl[i]);
                    discAmount -= _lstUspOuInvoiceDtl[i].Amount;
                }
                else
                {
                    if (isBanlaceFixDiscIn)
                        _lstUspOuInvoiceDtl[i].DiscDiag = (_lstUspOuInvoiceDtl[i].Amount - discAmount) / _lstUspOuInvoiceDtl[i].Amount;
                    else
                        _lstUspOuInvoiceDtl[i].DiscSelf = (_lstUspOuInvoiceDtl[i].Amount - discAmount) / _lstUspOuInvoiceDtl[i].Amount;
                    ReCalculateDetailAmount(_lstUspOuInvoiceDtl[i].DiscDiag, _lstUspOuInvoiceDtl[i].DiscSelf, _lstUspOuInvoiceDtl[i]);
                    discAmount = 0;
                }
                _lstUspOuInvoiceDtl[i].LimitGroupId = 0;
                _lstUspOuInvoiceDtl[i].LimitFee = 0;
            }
            this.devGrid1.advBandedGridViewMain.RefreshData();
            Tools.Utils.TraceFunctionOperate(203);
        }
        /// <summary>
        /// 自定义优惠比例
        /// </summary>
        private void CstmSelf()
        {
            Model.ModelList<Model.ComputeSummary> lstTally = _lstUspOuInvoiceDtl.GroupBy("FeeId", "DiscSelf", "FeeName", Model.ComputeType.Min);
            SetFeetyNameOrder(lstTally);
            Tools.FrmGridInput frmGridInput = new Tools.FrmGridInput();
            frmGridInput.Text = "请输入优惠比例";
            frmGridInput.usgInvItemType.Key = "DataGrid.FeetyDisc";
            frmGridInput.usgInvItemType.DataSource = lstTally;
            if (frmGridInput.ShowDialog() != DialogResult.OK) return;
            for (int i = 0; i < lstTally.Count; i++)
            {
                for (int j = 0; j < _lstUspOuInvoiceDtl.Count; j++)
                {
                    if (_lstUspOuInvoiceDtl[j].FeeId == Convert.ToInt32(lstTally[i].GroupBy))
                    {
                        _lstUspOuInvoiceDtl[j].DiscSelf = lstTally[i].Result;
                        ReCalculateDetailAmount(_lstUspOuInvoiceDtl[j].DiscDiag, _lstUspOuInvoiceDtl[j].DiscSelf, _lstUspOuInvoiceDtl[j]);
                    }
                }
            }
            CalcuateAmountTally();
            InitData();
            this.devGrid1.advBandedGridViewMain.RefreshData();
            Tools.Utils.TraceFunctionOperate(202);
        }
        /// <summary>
        /// 自定义自付比例
        /// </summary>
        private void CstmDisc()
        {
            if (!this.hisOuHosInfo1.Value.IsGf && !this.hisOuHosInfo1.Value.IsYb)
            {
                this.InformationInMainForm = "该病人不是公费或医保病人，不能修改其自付比例。您可以修改其优惠比例！";
                return;
            }
            Model.ModelList<Model.ComputeSummary> lstTally = _lstUspOuInvoiceDtl.GroupBy("FeeId", "DiscDiag", "FeeName", Model.ComputeType.Min);
            SetFeetyNameOrder(lstTally);
            Tools.FrmGridInput frmGridInput = new Tools.FrmGridInput();
            frmGridInput.Text = "请输入自付比例";
            frmGridInput.usgInvItemType.Key = "DataGrid.FeetyDisc";
            frmGridInput.usgInvItemType.DataSource = lstTally;
            if (frmGridInput.ShowDialog() != DialogResult.OK) return;
            for (int i = 0; i < lstTally.Count; i++)
            {
                for (int j = 0; j < _lstUspOuInvoiceDtl.Count; j++)
                {
                    if (_lstUspOuInvoiceDtl[j].FeeId == Convert.ToInt32(lstTally[i].GroupBy))
                    {
                        _lstUspOuInvoiceDtl[j].DiscDiag = lstTally[i].Result;
                        ReCalculateDetailAmount(_lstUspOuInvoiceDtl[j].DiscDiag, _lstUspOuInvoiceDtl[j].DiscSelf, _lstUspOuInvoiceDtl[j]);
                    }
                }
            }
            CalcuateAmountTally();
            InitData();
            this.devGrid1.advBandedGridViewMain.RefreshData();
        }
        FrmOuRecipe frmOuRecipe;
        bool IsForCharge = false;
        private void ManualRecipe()
        {
            if (this.hisOuHosInfo1.Value == null || this.hisOuHosInfo1.Value.DoctorId == 0) return;

            if (this.hisOuHosInfo1.Value != null && this.hisOuHosInfo1.Value.CardNo != string.Empty)
            {
                if (this.hisOuHosInfo1.Value.DoctorId == 0)
                {
                    this.InformationInMainForm = "病人还没有输入医生";
                    Register();
                }
                if (frmOuRecipe == null)
                {
                    frmOuRecipe = new FrmOuRecipe();
                    frmOuRecipe.IsForCharge = true;
                    frmOuRecipe.MenuId = 24;
                }
                frmOuRecipe.hisOuHosInfo1.uoupCardNo.FindByHospID(this.hisOuHosInfo1.Value.ID);
                int initLocationID = Model.Configuration.UserProfiles.LocationID;
                Model.Configuration.UserProfiles.LocationID = this.hisOuHosInfo1.Value.DiagnDept;
                Model.Configuration.UserProfiles.DoctorID = this.hisOuHosInfo1.Value.DoctorId;
                frmOuRecipe.IsSendKeys = false;
                IsForCharge = true;
                if (frmOuRecipe.ShowDialog() == DialogResult.OK)
                {
                    Model.Configuration.UserProfiles.LocationID = initLocationID;
                    Model.Configuration.UserProfiles.DoctorID = 0;
                    OpenRecord();
                }
                Model.Configuration.UserProfiles.LocationID = initLocationID;
                Model.Configuration.UserProfiles.DoctorID = 0;
                this.uicItemId.Focus();
            }
        }
        /// <summary>
        /// 批量自付比例
        /// </summary>
        private void BatchDiscIn()
        {
            if (!this.hisOuHosInfo1.Value.IsGf && !this.hisOuHosInfo1.Value.IsYb)
            {
                this.InformationInMainForm = "该病人不是公费或医保病人，不能修改其自付比例。您可以修改其优惠比例！";
                return;
            }
            bool rightInput = false;
            double discDiag = 1;
            do
            {
                string temp = Microsoft.VisualBasic.Interaction.InputBox("请输入要批量处理的自付部分的打折比例，例如：0，0.1，0.2，1 ...", "批量优惠比例", string.Empty, 200, 200);
                if (temp == string.Empty) return;
                if (Utilities.Information.IsNumeric(temp))
                {
                    discDiag = Convert.ToDouble(temp);
                    rightInput = true;
                }
            }
            while (!rightInput);
            _lstUspOuInvoiceDtl.Fill("DiscDiag", discDiag);
            for (int i = 0; i < _lstUspOuInvoiceDtl.Count; i++)
            {
                ReCalculateDetailAmount(discDiag, _lstUspOuInvoiceDtl[i].DiscSelf, _lstUspOuInvoiceDtl[i]);
            }
            CalcuateAmountTally();
            InitData();
            this.devGrid1.advBandedGridViewMain.RefreshData();
        }
        /// <summary>
        /// 批量优惠比例
        /// </summary>
        private void BatchDiscSelf()
        {
            double discSelf = 1;
            bool rightInput = false;
            do
            {
                string temp = Microsoft.VisualBasic.Interaction.InputBox("请输入要批量处理的优惠部分的打折比例，例如：0，0.1，0.2，1 ...", "批量优惠比例", string.Empty, 200, 200);
                if (temp == string.Empty) return;
                if (Utilities.Information.IsNumeric(temp))
                {
                    discSelf = Convert.ToDouble(temp);
                    rightInput = true;
                }
            }
            while (!rightInput);
            _lstUspOuInvoiceDtl.Fill("DiscSelf", discSelf);
            for (int i = 0; i < _lstUspOuInvoiceDtl.Count; i++)
            {
                ReCalculateDetailAmount(_lstUspOuInvoiceDtl[i].DiscDiag, discSelf, _lstUspOuInvoiceDtl[i]);
            }
            CalcuateAmountTally();
            InitData();
            this.devGrid1.advBandedGridViewMain.RefreshData();
        }
        private void SetFeetyNameOrder(Model.ModelList<Model.ComputeSummary> lstTally)
        {
            BLL.CBsMzFeety obj = new BLL.CBsMzFeety();
            Model.ModelList<Model.BsMzFeetyInfo> lstBsMzFeety = obj.GetAll();
            lstBsMzFeety.Sort("OrderBy");
            for (int i = 0; i < lstBsMzFeety.Count; i++)
            {
                for (int j = 0; j < lstTally.Count; j++)
                {
                    if (lstBsMzFeety[i].Name == lstTally[j].Description)
                        lstTally[j].ID = lstBsMzFeety[i].OrderBy;
                }
            }
            lstTally.Sort("ID");
        }
        private void WriteNewChinRecipe()
        {
            _infoOuChineseRecipe.LocationId = this.hisOuHosInfo1.Value.DiagnDept;
            _infoOuChineseRecipe.DoctorId = this.hisOuHosInfo1.Value.DoctorId;
            _infoOuChineseRecipe.HowMany = 1;
            _infoOuChineseRecipe.LsRepType = (int)Model.EnumRecipePrintType.ChinMedicine;
            _infoOuChineseRecipe.RecipeTime = BLL.Common.DateTimeHandler.GetServerDateTime();
            _infoOuChineseRecipe.MzRegId = this.hisOuHosInfo1.Value.ID;
            _infoOuChineseRecipe.RecipeNum = BLL.Common.SequenceNumHandler.GetSequenceNum(Model.EnumSequenceNumType.MzRecipe).ToString();
        }
        private void InputChinDrug()
        {
            Model.ModelList<Model.uspOuRecipeDtlQry> lstUspChineseRecipeDtl = new Model.ModelList<Model.uspOuRecipeDtlQry>();
            if (_lstChineseRecipe == null) _lstChineseRecipe = new Model.ModelList<Model.OuRecipeInfo>();
            if (_lstChineseRecipe.Count > 0)
            {
                FrmList frmChinRicipeList = new FrmList();
                frmChinRicipeList.LstOuRecipe = _lstChineseRecipe;
                frmChinRicipeList.ShowDialog();
                if (frmChinRicipeList.DialogResult == DialogResult.Cancel) return;
                if (frmChinRicipeList.DialogResult == DialogResult.Yes)
                {
                    _infoOuChineseRecipe = new Model.OuRecipeInfo();
                    WriteNewChinRecipe();
                }
                else
                {
                    _infoOuChineseRecipe = frmChinRicipeList.InfoOuRecipe;
                    lstUspChineseRecipeDtl = _lstUspChineseRecipeDtl.Find("F4", _infoOuChineseRecipe.RecipeNum);
                }
            }

            if (_infoOuChineseRecipe.MzRegId == 0)
            {
                WriteNewChinRecipe();
            }
            FrmOuChinRicipe frm = new FrmOuChinRicipe();
            frm.InfoOuRecipe = _infoOuChineseRecipe;
            frm.LstOuRecipe = _lstChineseRecipe;
            frm.LstUspChineseRecipeDtl = lstUspChineseRecipeDtl;
            if (Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsChinDrugHasNBF")) && _infoOuChineseRecipe.IsPriority)
                frm.NbfItemChangeToNormal();
            frm.InitData();
            frm.ShowDialog();
            if (frm.DialogResult == DialogResult.OK)
            {
                if (!CheckDiagLocationAndDoctor()) return;
                lstUspChineseRecipeDtl.Fill("F4", _infoOuChineseRecipe.RecipeNum);      //关联当前中药处方和明细
                _lstUspChineseRecipeDtl.Remove("F4", _infoOuChineseRecipe.RecipeNum);   //把上次的处方明细_lstUspChineseRecipeDtl移除
                _lstUspOuInvoiceDtl.Remove("F4", _infoOuChineseRecipe.RecipeNum);       //从费用明细中把上次的处方明细移除
                if (_lstChineseRecipe.Find("RecipeNum", _infoOuChineseRecipe.RecipeNum).Count == 0)   //如果是新处方则增加一条新中药处方
                    _lstChineseRecipe.Add(_infoOuChineseRecipe);
                bool noInsert;
                this.uicItemId.LsRpType = string.Empty;
                if (_infoOuChineseRecipe.IsPriority)
                {
                    _infoOuChineseRecipe.IsNB = true;
                }
                foreach (Model.uspOuRecipeDtlQry info in lstUspChineseRecipeDtl)       //把当前处方明细插入到费用明细和处方明细_lstUspChineseRecipeDtl
                {
                    info.F1 = System.DateTime.Now.TimeOfDay.ToString();
                    _lstUspChineseRecipeDtl.Add((Model.uspOuRecipeDtlQry)info.Clone());
                    noInsert = false;
                    foreach (Model.uspOuInvoiceDtlQry infoInvoiceDtl in _lstUspOuInvoiceDtl)
                    {
                        if (info.ItemId == infoInvoiceDtl.ItemId && info.F4 == infoInvoiceDtl.F4 && lstUspChineseRecipeDtl.Count > 0)   //如果收费明细没有才增加,有则只是同步数量
                        {
                            infoInvoiceDtl.Totality = info.Dosage * _infoOuChineseRecipe.HowMany;
                            infoInvoiceDtl.Amount = BLL.Common.Utils.Round(infoInvoiceDtl.Totality * infoInvoiceDtl.Price, 2);
                            noInsert = true;
                        }
                    }
                    if (noInsert) continue;
                    this.uicItemId.LsRpType = "3";
                    this.uicItemId.ItemID = info.ItemId;
                    uicInvItemId_KeyDownEvent(null, null);
                    if (_infoUspOuInvoiceDtl == null) continue;
                    if (_infoUspOuInvoiceDtl.ItemId == 0) continue;
                    _infoUspOuInvoiceDtl.Totality = info.Dosage * _infoOuChineseRecipe.HowMany;
                    _infoUspOuInvoiceDtl.F4 = _infoOuChineseRecipe.RecipeNum;   //把费用关联处方
                    AddNewItemData();
                    _infoUspOuInvoiceDtl = new Model.uspOuInvoiceDtlQry();
                }
                CalcuateAmountTally();
                InitData();
                this.devGrid1.advBandedGridViewMain.RefreshData();
                this.uicItemId.ReturnInitRpType();
                this.uicItemId.LstBsItemMini = _lstBsItemMini;
                this.uicItemId.txtBsItem.Text = string.Empty;
                this.uicItemId.txtBsItem.Enabled = true;
                this.uicItemId.ItemID = 0;
                this.uicItemId.txtBsItem.Focus();
                Tools.Utils.TraceFunctionOperate(192);
            }
        }
        private void GetChinRecipeItemTotality(int itemId)
        {
        }

        private void Register()
        {
            //Model.Configuration.CardPatientSM infoCardPatient;
            //BLL.Common.CardReadSM objCardRead = new BLL.Common.CardReadSM();
            //infoCardPatient = objCardRead.GetPatientInfo();

            FrmOuChargeHosInfo frm = new FrmOuChargeHosInfo();
            if (hisOuHosInfo1.Value != null && hisOuHosInfo1.Value.ID != 0)    //如果是打开病人以后再按“登记病人”
            {
                if (_infoOuInvoice != null && _infoOuInvoice.IsCancel) return;
                //if (hisOuHosInfo1.Value.PatId == 0 || hisOuHosInfo1.Value.ID == 0)    //如果用户按撤消以后
                //    frm.InfoCardPatient = infoCardPatient;
                //else
                //{
                if (this.hisOuHosInfo1.Value.DoctorId > 0)
                {
                    if (DialogResult.No == MessageBox.Show(string.Format("您现在打开着病人【{0}】的信息，收费是否对该病人的信息进行修改？", hisOuHosInfo1.Value.PatientName), "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                        return;
                }
                frm.PatId = hisOuHosInfo1.Value.PatId;
                frm.OuHosId = hisOuHosInfo1.Value.ID;
                //    }
            }
            else　  //如果是新病人则要检查权限，要求经过挂号处登记
            {
                if (!IsCanAccessThisModule(Model.Configuration.FuctionAccess.Public))
                {
                    this.InformationInMainForm = "您没有新登记挂号病人的权限。请确认！";
                    return;
                }
            }
            frm.ShowPatientInfo();
            frm.IsDoctorInput = IsDoctorInput;
            frm.ShowDialog();
            if (frm.DialogResult == DialogResult.OK)
            {
                InitHosinfo(frm.InfoOuHos.ID, frm.IsModiPatientType);

                this.IsYbReadCardNoEnSure = true;
                this.OHIllnCode = infoPersonInfo.OHIllnCode;
                this.OHIllnName = infoPersonInfo.OHIllnName;
            }


            frm.Dispose();
            Tools.Utils.TraceFunctionOperate(190);
        }
        /// <summary>
        /// 清空医保读卡信息
        /// </summary>
        private void ClearReadCardInfo()
        {
            this.IsYbReadCardNoEnSure = false;
            this.OHIllnCode = string.Empty;
            this.OHIllnName = string.Empty;
        }
        /// <summary>
        /// 是否读取医保卡
        /// </summary>
        public bool IsYbReadCardNoEnSure = false;
        /// <summary>
        /// 病种编码
        /// </summary>
        public string OHIllnCode { set; get; }
        /// <summary>
        /// 病种名称
        /// </summary>
        public string OHIllnName { set; get; }

        void hisOuHosInfo1_PatTypeUpdate(object sender, System.EventArgs e)
        {
            if (this.hisOuHosInfo1.Value != null && this.hisOuHosInfo1.Value.ID > 0)
            {
                Model.OuHosInfoInfo ouhosInfo = _objOuHosInfo.GetByID(this.hisOuHosInfo1.Value.ID);
                Tools.BaseControl ubsPatType = (sender as Tools.BaseControl);
                if (ubsPatType.ID > 0)
                {
                    ouhosInfo.PatTypeId = ubsPatType.ID;
                    _objOuHosInfo.Modify(ouhosInfo, null);
                }

                InitHosinfo(this.hisOuHosInfo1.Value.ID, true);
                this.InformationInMainForm = "病人类别修改";
            }
        }
        private void SetPatType()
        {
            BLL.CBsItemPatType objBsItemPatType = new BLL.CBsItemPatType();
            Model.ModelList<Model.BsItemPatTypeInfo> lstBsItemPatType = new Model.ModelList<Model.BsItemPatTypeInfo>();
            foreach (Model.uspOuInvoiceDtlQry info in _lstUspOuInvoiceDtl)
            {
                lstBsItemPatType = objBsItemPatType.BsItemPatType_SelectByPatTypeIdAndItemId(this.hisOuHosInfo1.Value.PatTypeId, info.ItemId);
                if (lstBsItemPatType.Count > 0)
                    info.DiscDiag = lstBsItemPatType[0].DiscDiag;
            }
        }
        public void InitHosinfo(int OuHosID, bool IsModiPatientType)
        {
            this.hisOuHosInfo1.SearchPatient(OuHosID);
            if (IsModiPatientType && _lstUspOuRecipeDtlForOuInvoiceDtl.Find("IsDoctorInput", "True").Count == 0)
                _lstUspOuInvoiceDtl.Clear();
            else
                SetPatType();
            if (_lstUspOuInvoiceDtl.Count == 0)
                HisOuHosInfo_RecordFound(hisOuHosInfo1.Value);

            if (hisOuHosInfo1.Value == null) return;
            if (this.hisOuHosInfo1.Value.DiagnDept == 0)
                this.hisOuHosInfo1.Value.DiagnDept = this.hisOuHosInfo1.Value.RegDept;
            Model.Configuration.Global.CardNo = this.hisOuHosInfo1.Value.CardNo;
            SetStyleOfGfOrYb();
            this.uicItemId.Focus();
        }
        public void DeleteRow(bool DelteAll)
        {
            int rowIndex = this.devGrid1.advBandedGridViewMain.FocusedRowHandle;
            if (rowIndex < 0 || rowIndex > this._lstUspOuInvoiceDtl.Count - 1) return;
            if (_lstUspOuInvoiceDtl[rowIndex].Name == string.Empty) return;
            if (CheckDrugItemIssued(_lstUspOuInvoiceDtl[rowIndex].RecipeItemId))
            {
                this.InformationInMainForm = "您不能删除已发药药品退费。请确认！";
            }
            else if (_lstUspChineseRecipeDtl != null && _lstUspChineseRecipeDtl.Find("ItemId", _lstUspOuInvoiceDtl[rowIndex].ItemId.ToString()).Count > 0)
            {
                this.InformationInMainForm = "您不能这网格直接删除中药处方，请按“中药”按钮进行删除。请确认！";
                this.devGrid1.advBandedGridViewMain.OptionsBehavior.Editable = false;
            }
            else if (_lstUspOuInvoiceDtl[rowIndex].LsAdviceType != 2 &&   //医生输入的附加收费（通常是套餐带出的），可以由收费员删除
                !Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOuChargeDeleteDoctorRecie")) && _lstUspOuInvoiceDtl[rowIndex].IsDoctorInput && !IsCanAccessThisModule(Model.Configuration.FuctionAccess.Check))
            {
                this.InformationInMainForm = "您不能删除医生开出的处方项目，请通知医生本人进行修改。请确认！";
                this.devGrid1.advBandedGridViewMain.OptionsBehavior.Editable = false;
            }
            else
            {
                //DAL.SqlUtil db = new DAL.SqlUtil();
                //System.Data.Common.DbTransaction trn = db.GetSqlTransaction();
                //BLL.COuRecipeDtl objOuRecipeDtl = new BLL.COuRecipeDtl();
                //try
                //{
                //    if (_lstUspOuInvoiceDtl[rowIndex].ID > 0)
                //    {
                //        _objOuInvoiceDtl.Remove(_lstUspOuInvoiceDtl[rowIndex].ID, trn);
                //    }
                //    //_objOuInvoiceDtl.OuInvoiceDtl_DeleteByRecipeItemId(_lstUspOuInvoiceDtl[rowIndex].RecipeItemId, trn);
                //    if (!objOuRecipeDtl.GetByID(_lstUspOuInvoiceDtl[rowIndex].RecipeItemId).IsDoctorInput || _lstUspOuInvoiceDtl[rowIndex].LsAdviceType == 2)
                //    {
                //        if (_lstUspOuInvoiceDtl[rowIndex].LsAdviceType != 2 && _lstUspOuInvoiceDtl[rowIndex].XDRpId > 0 && DelteAll)
                //        {
                //            foreach (Model.uspOuInvoiceDtlQry info in _lstUspOuInvoiceDtl.Find("XDRpId", _lstUspOuInvoiceDtl[rowIndex].XDRpId.ToString()))
                //            {
                //                objOuRecipeDtl.Remove(info.RecipeItemId, trn);
                //            }
                //        }
                //        else
                //            objOuRecipeDtl.Remove(_lstUspOuInvoiceDtl[rowIndex].RecipeItemId, trn);
                //    }
                //    trn.Commit();
                //    Tools.Utils.TraceFunctionOperate(193);
                //}
                //catch (Exception e)
                //{
                //    trn.Rollback();
                //    trn.Dispose();
                //    throw (e);
                //}

                //int xDRpId = _lstUspOuInvoiceDtl[rowIndex].XDRpId;
                //string memo = _lstUspOuInvoiceDtl[rowIndex].Memo;
                //_lstUspChineseRecipeDtl.Remove("ItemId", _lstUspOuInvoiceDtl[rowIndex].ItemId.ToString());
                //if (_lstUspOuInvoiceDtl[rowIndex].LsAdviceType != 2 && xDRpId > 0 && DelteAll)
                //    RemoveXDRpItem(xDRpId);
                //else if (_lstUspOuInvoiceDtl[rowIndex].RecipeGroupId > 0 && _lstUspOuInvoiceDtl[rowIndex].F3 == "自动收" && _lstHasAttach.Find("RecipeItemId", _lstUspOuInvoiceDtl[rowIndex].RecipeGroupId.ToString()).Count > 0)
                //{
                //    if (SelectDoctorCheckItem(_lstUspOuInvoiceDtl[rowIndex]))
                //        OpenRecord();
                //    else return;
                //}
                //else
                //    _lstUspOuInvoiceDtl.RemoveAt(rowIndex);

                //if (DelteAll)
                //    RemoveAttachItem(memo);

                //CalcuateAmountTally();
                //InitData();



                _lstUspChineseRecipeDtl.Remove("ItemId", _lstUspOuInvoiceDtl[rowIndex].ItemId.ToString());
                if (_lstUspOuInvoiceDtl[rowIndex].XDRpId > 0)
                    _lstUspOuInvoiceDtl.Remove("XDRpId", _lstUspOuInvoiceDtl[rowIndex].XDRpId.ToString());
                else
                    _lstUspOuInvoiceDtl.RemoveAt(rowIndex);

                //string memo = _lstUspOuInvoiceDtl[rowIndex].Memo;
                //RemoveAttachItem(memo);

                CalcuateAmountTally();
                InitData();
            }
            //this.FormStatus = Model.Configuration.ToolbarStatus.Add;
            //Lock(false);
        }
        Model.ModelList<Model.SelectItem> _lstSelectRecipe = new Model.ModelList<Model.SelectItem>();
        int lastListNum = 0;
        int lastLsRepType = 0;
        //private void SelectRecipeItem()
        //{
        //    _lstSelectRecipe.Clear();
        //    Model.ModelList<Model.uspOuRecipeDtlForOuInvoiceDtlQry> lstUspOuRecipeDtlForOuInvoiceDtl = _lstUspOuRecipeDtlForOuInvoiceDtl.Find("IsCancel", "False");
        //    lstUspOuRecipeDtlForOuInvoiceDtl.Sort("LsRepType,ListNum");
        //    foreach (Model.uspOuRecipeDtlForOuInvoiceDtlQry info in lstUspOuRecipeDtlForOuInvoiceDtl)
        //    {
        //        if (info.LsRepType == 3)
        //            info.ListNum = info.ItemId;
        //        if (lastListNum != info.ListNum || lastLsRepType != info.LsRepType)
        //        {
        //            Model.SelectItem infoSelectItem = new Model.SelectItem();
        //            infoSelectItem.IsSelect = true;
        //            infoSelectItem.Code = info.ListNum.ToString();
        //            if (info.LsRepType == 1)
        //                infoSelectItem.Name = "西药处方";
        //            else if (info.LsRepType == 2)
        //                infoSelectItem.Name = "中药处方";
        //            else if (info.LsRepType == 3)
        //                infoSelectItem.Name = "检查检验";
        //            infoSelectItem.F1 = info.LsRepType.ToString();
        //            infoSelectItem.F2 = info.Amount.ToString();
        //            infoSelectItem.F3 = Utilities.Information.FilterArrayToString(lstUspOuRecipeDtlForOuInvoiceDtl.Find("LsRepType", info.LsRepType.ToString()).Find("ListNum", info.ListNum.ToString()).CopyTo("Name"));
        //            _lstSelectRecipe.Add(infoSelectItem);
        //        }
        //        lastListNum = info.ListNum;
        //        lastLsRepType = info.LsRepType;
        //    }

        //    Tools.FrmGridInput frm = new Tools.FrmGridInput();
        //    frm.Text = "请选择您要收费的处方";
        //    frm.usgInvItemType.Key = "DataGrid.SelectRecipeItem";
        //    frm.usgInvItemType.DataSource = _lstSelectRecipe;
        //    frm.ShowDialog();
        //    if (frm.DialogResult == DialogResult.OK)
        //        OpenRecord();
        //}
        int lastRecipe = 0;
        bool isManulSelect = false;
        private void SelectRecipeItem()
        {
            _lstSelectRecipe.Clear();

            Model.ModelList<Model.uspOuRecipeDtlForOuInvoiceDtlQry> lstUspOuRecipeDtlForOuInvoiceDtl = _lstUspOuRecipeDtlForOuInvoiceDtlRemove.Find("IsCancel", "False");

            lstUspOuRecipeDtlForOuInvoiceDtl.Sort("LsRepType,RecipeId,ListNum");
            foreach (Model.uspOuRecipeDtlForOuInvoiceDtlQry info in lstUspOuRecipeDtlForOuInvoiceDtl)
            {
                //if (info.LsRepType == 3)
                //    info.ListNum = info.ItemId;
                if (lastRecipe != info.RecipeId || lastListNum != info.ListNum || lastLsRepType != info.LsRepType)
                //if (lastLsRepType != info.LsRepType)
                {
                    Model.SelectItem infoSelectItem = new Model.SelectItem();
                    infoSelectItem.IsSelect = true;
                    infoSelectItem.Code = info.ListNum.ToString();
                    if (info.LsRepType == 1)
                        infoSelectItem.Name = "西药处方";
                    else if (info.LsRepType == 2)
                        infoSelectItem.Name = "中药处方";
                    else if (info.LsRepType == 3)
                        infoSelectItem.Name = "检查检验";
                    infoSelectItem.F1 = info.LsRepType.ToString();

                    double AmountPay = 0.00;
                    if (info.LsRepType == 2)
                    {
                        for (int i = 0; i < lstUspOuRecipeDtlForOuInvoiceDtl.Count; i++)
                        {
                            if (info.LsRepType == 2 && info.RecipeId == lstUspOuRecipeDtlForOuInvoiceDtl[i].RecipeId)
                            {

                                infoSelectItem.F3 += lstUspOuRecipeDtlForOuInvoiceDtl[i].Name;
                                AmountPay = AmountPay + lstUspOuRecipeDtlForOuInvoiceDtl[i].Amount;
                                infoSelectItem.F2 = AmountPay.ToString();
                            }
                        }
                    }
                    else
                    {
                        infoSelectItem.F2 = info.Amount.ToString();
                        infoSelectItem.F3 = Utilities.Information.FilterArrayToString(lstUspOuRecipeDtlForOuInvoiceDtl.Find("LsRepType", info.LsRepType.ToString()).Find("ListNum", info.ListNum.ToString()).CopyTo("Name"));
                    }

                    infoSelectItem.F4 = info.RecipeId.ToString();
                    _lstSelectRecipe.Add(infoSelectItem);
                }
                lastRecipe = info.RecipeId;
                lastListNum = info.ListNum;
                lastLsRepType = info.LsRepType;
            }

            Tools.FrmGridInput frm = new Tools.FrmGridInput(true);
            frm.IsSelectCancel = true;
            frm.Text = "请选择您要收费的处方";
            frm.usgInvItemType.Key = "DataGrid.SelectRecipeItem";
            frm.usgInvItemType.DataSource = _lstSelectRecipe;

            //if (_lstSelectRecipe.Count >= 2 || isManulSelect)
            //{
            frm.ShowDialog();
            isManulSelect = false;

            //_lstSelectRecipe = null;
            //}
            //_lstUspOuRecipeDtlForOuInvoiceDtlRemove.Clear();
            lastRecipe = 0;
            if (frm.DialogResult == DialogResult.OK)
            {
                OpenRecord();
                // _lstUspOuRecipeDtlForOuInvoiceDtlRemove.Clear();
            }
        }

        Model.ModelList<Model.SelectItem> _lstSelectItem = new Model.ModelList<Model.SelectItem>();
        private bool SelectDoctorCheckItem(Model.uspOuInvoiceDtlQry deleteFee)
        {
            //_lstSelectItem.Clear();
            RemoveF2();
            if (_lstHasAttach.Count == 0) return true;
            foreach (Model.uspOuInvoiceDtlQry infoCheck in _lstHasAttach)
            {
                if (deleteFee.RecipeGroupIds.Contains(infoCheck.RecipeItemId.ToString()) || infoCheck.RecipeItemId == deleteFee.RecipeGroupId)
                {
                    Model.SelectItem infoSelectItem = new Model.SelectItem();
                    infoSelectItem.ID = infoCheck.RecipeItemId;
                    infoSelectItem.Name = infoCheck.Name;
                    infoSelectItem.F1 = infoCheck.ItemId.ToString();
                    _lstSelectItem.Add(infoSelectItem);
                }
            }

            if (_lstSelectItem.Count == 0)
                return false;
            Tools.FrmGridInput frm = new Tools.FrmGridInput();
            frm.Text = "请选择您要删除的医生医嘱";
            frm.usgInvItemType.Key = "DataGrid.SelectItem";
            frm.usgInvItemType.DataSource = _lstSelectItem;
            frm.ShowDialog();
            if (frm.DialogResult == DialogResult.OK)
                return true;
            else
            {
                RemoveF2();
                return false;
            }
        }
        private void RemoveF2()
        {
            for (int i = 0; i < _lstSelectItem.Count; i++)
            {
                if (_lstSelectItem[i].F2 != "1")
                {
                    _lstSelectItem.Remove(_lstSelectItem[i]);
                    i--;
                }
            }
        }
        private void RemoveAttachItem(string memo)
        {
            for (int i = 0; i < _lstUspOuInvoiceDtl.Count; i++)
            {
                Model.uspOuInvoiceDtlQry info = _lstUspOuInvoiceDtl[i];
                if (memo.Contains(_lstUspOuInvoiceDtl[i].Memo) && _lstUspOuInvoiceDtl[i].F3.Trim() == "自动收" &&
                    MessageBox.Show(string.Format("您是否要把附加项目{0}一起删除？", _lstUspOuInvoiceDtl[i].Name), "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _lstUspOuInvoiceDtl.Remove(info);
                    i--;
                }
            }
        }

        private void RemoveXDRpItem(int XDRpId)
        {
            for (int i = 0; i < _lstUspOuInvoiceDtl.Count; i++)
            {
                Model.uspOuInvoiceDtlQry info = _lstUspOuInvoiceDtl[i];
                if (_lstUspOuInvoiceDtl[i].XDRpId == XDRpId && _lstNotReChargeExecuted.Find("ID", _lstUspOuInvoiceDtl[i].RecipeItemId.ToString()).Count == 0)
                {
                    _lstUspOuInvoiceDtl.Remove(info);
                    i--;
                }
            }
        }
        /// <summary>
        /// 根据病人类别和科室获取固定收费项目
        /// </summary>
        private void GetFixCharge()
        {
            if (!CheckDiagLocationAndDoctor()) return;
            BLL.CBsFixCharge objBsFixCHarge = new BLL.CBsFixCharge();
            Model.ModelList<Model.BsFixChargeInfo> lstBsFixCharge = objBsFixCHarge.GetAll();
            BLL.Finder<Model.uspOuGetFixChargeBeenChargedQry> finder = null;

            foreach (Model.BsFixChargeInfo info in lstBsFixCharge)
            {
                if (_lstUspOuInvoiceDtl.Contains("ItemId", info.ItemId.ToString())) continue;
                finder = new BLL.Finder<Model.uspOuGetFixChargeBeenChargedQry>();
                finder.AddParameter("MzRegId", this.hisOuHosInfo1.Value.ID);
                finder.AddParameter("ItemId", info.ItemId);
                if (finder.Find("uspOuGetFixChargeBeenCharged").Count > 0) continue;
                if ((info.PatTypeId == 0 || info.PatTypeId == this.hisOuHosInfo1.Value.PatTypeId) && (info.LocationId == 0 || info.LocationId == this.hisOuHosInfo1.Value.DiagnDept))
                {
                    this.uicItemId.ItemID = info.ItemId;
                    uicInvItemId_KeyDownEvent(null, null);
                    if (_infoUspOuInvoiceDtl == null) continue;
                    this._infoUspOuInvoiceDtl.Totality = info.Totality;
                    AddNewInvoiceDtl();
                }
            }
        }
        Model.ModelList<Model.OuRecipeDtlInfo> _lstHasRecipeDtl = new Model.ModelList<Model.OuRecipeDtlInfo>();
        Model.ModelList<Model.OuRecipeDtlInfo> _lstSamePatientName = new Model.ModelList<Model.OuRecipeDtlInfo>();
        private void GetHasRecipeDtl()
        {
            BLL.COuRecipe objOuRecipe = new BLL.COuRecipe();
            BLL.COuRecipeDtl objOuRecipeDtl = new BLL.COuRecipeDtl();
            Model.ModelList<Model.OuRecipeInfo> lstOuRecipe = objOuRecipe.OuRecipe_SelectByMzRegId(this.hisOuHosInfo1.Value.ID);
            _lstHasRecipeDtl = objOuRecipe.GetMutiChild<Model.OuRecipeDtlInfo, BLL.COuRecipeDtl>(lstOuRecipe.ConvertToBase(), "RecipeId");
        }
        private void GetDiagSamePatientName()
        {
            _lstSamePatientName = new Model.ModelList<Model.OuRecipeDtlInfo>();
            DateTime dt = BLL.Common.DateTimeHandler.GetServerDateTime();
            BLL.Finder<Model.OuRecipeDtlInfo> finder = new BLL.Finder<Model.OuRecipeDtlInfo>();
            finder.AddParameter("@PatientName", this.hisOuHosInfo1.Value.PatientName);
            finder.AddParameter("@DateTime", dt);
            finder.AddParameter("@DoctorId", this.hisOuHosInfo1.Value.DoctorId);
            _lstSamePatientName = finder.Find("UspOuRecipeDtlByPatientName");
        }
        /// <summary>
        /// 如果设置了在门诊收费收取挂号费、诊金，则挂号后自动收取（一个挂号只收一次）
        /// </summary>
        /// 
        int diagFeeItemId = Convert.ToInt32(BLL.Common.Utils.GetSystemSetting("DiagFeeItemId"));
        int regFeeItemId = 0;
        BLL.COulInvoiceReg objOulInvoiceReg = new BLL.COulInvoiceReg();
        bool IsOuChargeRegFee = BLL.Common.Utils.GetSystemSetting("OuRegFeeMode") == "0";//挂号是否在挂号处收
        bool IsOuChargeDiagFee = BLL.Common.Utils.GetSystemSetting("OuDiagFeeMode") == "0";//诊金是否在挂号处收
        private void GetRegFee()
        {
            Model.OuHosInfoInfo infoOuHosInfo = _objOuHosInfo.GetByID(this.hisOuHosInfo1.Value.ID);
            Model.ModelList<Model.OulInvoiceRegInfo> lstOulInvoiceReg = objOulInvoiceReg.OulInvoiceReg_SelectByMzRegId(this.hisOuHosInfo1.Value.ID).Find("IsCancel", "False");
            lstOulInvoiceReg.Reverse();
            BLL.CBsRegPatAmount objBsRegPatAmount = new BLL.CBsRegPatAmount();
            bool isRegFreeFee = this.hisOuHosInfo1.Value.IsInPatient;
            bool isManalOuHosInfo = infoOuHosInfo.F1 != "1" && Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsManalOuHosInfoGetRegFee"));
            Model.BsRegPatAmountInfo infoBsRegPatAmount = CalBsRegPatAmount(lstOulInvoiceReg, isManalOuHosInfo);
            if (infoBsRegPatAmount == null)
            {
                //Utilities.Information.ShowMsgBox("挂号费没有设置，请通知系统管理员！");
                return;
            }
            if ((infoBsRegPatAmount.DiagnoFee > 0 || infoBsRegPatAmount.RegFee > 0) && Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsRegFeeAdjustInOucharge")) && lstOulInvoiceReg.Count > 0 && this.hisOuHosInfo1.Value.RegTypeId != lstOulInvoiceReg[0].RegTypeId)
            {
                isManalOuHosInfo = true;
                isRegFreeFee = true;
            }

            //如果诊金和挂号费都在挂号处收的话，判断是否医生挂号手工登记病人，如果医生挂号手工登记病人则收费处默认要收取挂号和诊金
            if (BLL.Common.Utils.GetSystemSetting("OuRegFeeMode") == "3" && BLL.Common.Utils.GetSystemSetting("OuDiagFeeMode") == "3" ||
                IsOuChargeRegFee && IsOuChargeDiagFee && !isManalOuHosInfo && lstOulInvoiceReg.Count > 0 && lstOulInvoiceReg[0].F4 != "1") return;
            if (!CheckDiagLocationAndDoctor()) return;
            BLL.CBsDoctor objBsDoctor = new BLL.CBsDoctor();
            if (objBsDoctor.GetByID(this.hisOuHosInfo1.Value.DoctorId).LocationId == Convert.ToInt32(BLL.Common.Utils.GetSystemSetting("TJLocationId")))
                return;
            BLL.CBsRegType objBsRegType = new BLL.CBsRegType();
            if (lstOulInvoiceReg.Count == 0 && !isManalOuHosInfo)
            {
                this.ShowInformationInMainForm("该病人流水号没有经过门诊挂号处挂号，系统作手工处方处理，不予自动生成挂号费和诊金！");
                return;
            }
            if (!isRegFreeFee && !isManalOuHosInfo)   //医生挂号或自助挂号的IsInPatient=1
            {
                this.ShowInformationInMainForm("该病人经过门诊挂号处并且已经收取挂号费，系统不予自动生成挂号费和诊金！");
                return;
            }
            int diagFeeItemId = Convert.ToInt32(BLL.Common.Utils.GetSystemSetting("DiagFeeItemId"));
            int regFeeItemId = Convert.ToInt32(BLL.Common.Utils.GetSystemSetting("RegFeeItemId")); ;
            string specDiagFeeItemId = objBsRegType.GetByID(this.hisOuHosInfo1.Value.RegTypeId).F3;  //就诊医生
            if (Utilities.Information.IsNumeric(specDiagFeeItemId.Trim()))
                diagFeeItemId = Convert.ToInt32(specDiagFeeItemId);
            if (infoBsRegPatAmount.F1 == "补收")
            {
                diagFeeItemId = Convert.ToInt32(BLL.Common.Utils.GetSystemSetting("ReGetDiagFeeItemId"));
            }

            BLL.COuInvoice objOuInvoice = new BLL.COuInvoice();
            BLL.CBsDocLevel objBsDocLevel = new BLL.CBsDocLevel();
            Model.ModelList<Model.BsDoctorInfo> lstDoctor = new Model.ModelList<Model.BsDoctorInfo>();
            BLL.CBsDoctor objDoctor = new BLL.CBsDoctor();
            BLL.COuInvoiceDtl objOuInvoiceDtl = new BLL.COuInvoiceDtl();
            Model.ModelList<Model.OuInvoiceInfo> lstOuInvoice = objOuInvoice.OuInvoice_SelectByMzRegId(this.hisOuHosInfo1.Value.ID).Find("IsCancel", "False");
            Model.ModelList<Model.OuInvoiceDtlInfo> lstOuInvoiceDtl = objOuInvoice.GetMutiChild<Model.OuInvoiceDtlInfo, BLL.COuInvoiceDtl>(lstOuInvoice.ConvertToBase(), "InvoId").Find("LsPerform", "1");
            double diagnoFee = infoBsRegPatAmount.DiagnoFee;  //门诊就诊诊金
            double regFee = infoBsRegPatAmount.RegFee;
            if (infoOuHosInfo.F1 == "1")
            {
                double ouDiagnoFee = objBsRegPatAmount.BsRegPatAmount_SelectByRegTypeIDAndPatTypeID(infoOuHosInfo.RegTypeId, infoOuHosInfo.PatTypeId)[0].DiagnoFee;  //挂号诊金
                if (diagnoFee != ouDiagnoFee && ouDiagnoFee > diagnoFee)
                {
                    diagnoFee = ouDiagnoFee - diagnoFee;
                }

                if (diagnoFee == 3)
                {
                    diagFeeItemId = 732357;
                }
                if (diagnoFee == 5)
                {
                    diagFeeItemId = 732358;
                }
                //regFee = 0;
            }

            if (diagnoFee > 0 && !lstOuInvoiceDtl.Contains("ItemId", diagFeeItemId.ToString()) && !_lstHasRecipeDtl.Contains("ItemId", diagFeeItemId.ToString()) && !_lstSamePatientName.Contains("ItemId", diagFeeItemId.ToString()) && !infoOuHosInfo.IsFreeDiag && (!IsOuChargeDiagFee || isRegFreeFee))
                AddOneCustFee(diagFeeItemId, diagnoFee, 1, string.Empty, 0, string.Empty, false, 0);

            if (regFee > 0 && !lstOuInvoiceDtl.Contains("ItemId", regFeeItemId.ToString()) && !_lstHasRecipeDtl.Contains("ItemId", regFeeItemId.ToString()) && !_lstSamePatientName.Contains("ItemId", regFeeItemId.ToString()) && (!IsOuChargeRegFee || isManalOuHosInfo || lstOulInvoiceReg.Count > 0 && lstOulInvoiceReg[0].F4 == "1") && !infoOuHosInfo.IsFreeReg)
                AddOneCustFee(regFeeItemId, regFee, 1, string.Empty, 0, string.Empty, false, 0);

            foreach (Model.uspOuInvoiceDtlQry infoOuInvoiceDtl in _lstUspOuInvoiceDtl)
            {
                if (infoOuInvoiceDtl.ItemId == diagFeeItemId)
                    infoOuInvoiceDtl.Amount = infoOuInvoiceDtl.Price = diagnoFee;
                else if (infoOuInvoiceDtl.ItemId == regFeeItemId)
                    infoOuInvoiceDtl.Amount = infoOuInvoiceDtl.Price = regFee;

            }


            if (_lstUspOuInvoiceDtl.Find("ItemId", diagFeeItemId.ToString()).Count > 0)
            {
                int doctoreId = _lstUspOuInvoiceDtl.Find("ItemId", diagFeeItemId.ToString())[0].DoctorId;
                int levelId = objBsDoctor.GetByID(doctoreId).LevelId;
                string levelName = objBsDocLevel.GetByID(levelId).Name;
                if (levelName.Contains("主任"))
                    _lstUspOuInvoiceDtl.Find("ItemId", diagFeeItemId.ToString()).Fill("Name", levelName);
            }
            if (lstOuInvoiceDtl.Contains("ItemId", diagFeeItemId.ToString()) || lstOuInvoiceDtl.Contains("ItemId", regFeeItemId.ToString()))
            {
                this.ShowInformationInMainForm("该病人流水号已经在门诊收费处收过挂号费或诊金，系统不再自动生成！");
                return;
            }
            Tools.Utils.TraceFunctionOperate(196);
        }
        private Model.BsRegPatAmountInfo CalBsRegPatAmount(Model.ModelList<Model.OulInvoiceRegInfo> lstOulInvoiceReg, bool isManalOuHosInfo)
        {
            Model.BsRegPatAmountInfo infoBsRegPatAmount = new Model.BsRegPatAmountInfo();
            if (Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOuRegConfirmRegFee")) && !isManalOuHosInfo)
            {
                infoBsRegPatAmount.DiagnoFee = lstOulInvoiceReg[0].DiagnoFee;
                infoBsRegPatAmount.RegFee = lstOulInvoiceReg[0].RegFee;
            }
            else
            {
                BLL.CBsRegPatAmount objBsRegPatAmount = new BLL.CBsRegPatAmount();
                Model.ModelList<Model.BsRegPatAmountInfo> lstBsRegPatAmount = objBsRegPatAmount.BsRegPatAmount_SelectByRegTypeIDAndPatTypeID(this.hisOuHosInfo1.Value.RegTypeId, this.hisOuHosInfo1.Value.PatTypeId);
                if (lstBsRegPatAmount.Count == 0) return null;
                infoBsRegPatAmount = lstBsRegPatAmount[0];

                if (lstOulInvoiceReg.Count > 0 && this.hisOuHosInfo1.Value.RegTypeId != lstOulInvoiceReg[0].RegTypeId && Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsRegFeeAdjustInOucharge")))
                {
                    infoBsRegPatAmount.DiagnoFee -= lstOulInvoiceReg[0].DiagnoFee;
                    infoBsRegPatAmount.RegFee -= lstOulInvoiceReg[0].RegFee;
                    infoBsRegPatAmount.F1 = "补收";
                }
            }
            return infoBsRegPatAmount;
        }
        private int ChangeMergeFeeItemId(int itemId)
        {
            foreach (Model.BsOuMergeFeeInfo info in _lstBsOuMergeFee)
            {
                if (info.ItemId1 == itemId && (info.PatTypeId == this.hisOuHosInfo1.Value.PatTypeId || info.PatTypeId == 0))
                    return info.ItemId2;
            }
            return itemId;
        }
        string SameItemId = string.Empty;
        private void AddOneCustFee(int itemId, double price, double totality, string memo, int recipeGroupId, string inputer, bool isZeroPrice, int xdRpId)
        {
            if ((price > 0 && price < 0.009999) && !isZeroPrice) return;
            SameItemId = string.Empty;
            foreach (Model.uspOuInvoiceDtlQry infoOuInvoiceDtl in _lstUspOuInvoiceDtl)
            {
                if (infoOuInvoiceDtl.ItemId == itemId && infoOuInvoiceDtl.Price == price)
                {
                    //if (infoOuInvoiceDtl.F3 == "自动收" && !isDropContinue) return;
                    if (!SameItemId.Contains(recipeGroupId.ToString()))
                        SameItemId += recipeGroupId.ToString() + ",";
                    if (!SameItemId.Contains(infoOuInvoiceDtl.RecipeGroupId.ToString()))
                        SameItemId += infoOuInvoiceDtl.RecipeGroupId.ToString() + ",";
                    infoOuInvoiceDtl.Totality += totality;
                    if (price > 0)
                        infoOuInvoiceDtl.Amount = BLL.Common.Utils.Round(price * infoOuInvoiceDtl.Totality, 2);
                    else
                        infoOuInvoiceDtl.Amount = BLL.Common.Utils.Round(infoOuInvoiceDtl.Price * infoOuInvoiceDtl.Totality, 2);
                    if (infoOuInvoiceDtl.Memo != string.Empty && memo != string.Empty && !infoOuInvoiceDtl.Memo.Contains(memo))
                    {
                        if (memo.Contains(infoOuInvoiceDtl.Memo))
                            infoOuInvoiceDtl.Memo = memo;
                        else
                            infoOuInvoiceDtl.Memo = string.Format("{0}，{1}", infoOuInvoiceDtl.Memo, memo);
                    }
                    infoOuInvoiceDtl.F4 = infoOuInvoiceDtl.RecipeGroupIds = SameItemId;
                    return;
                }
            }
            this.uicItemId.PatTypeID = this.hisOuHosInfo1.Value.PatTypeId;
            this.uicItemId.ItemID = itemId;
            uicInvItemId_KeyDownEvent(null, null);
            if (_infoUspOuInvoiceDtl == null) return;
            if (_infoUspOuInvoiceDtl.Name == string.Empty) return;
            _infoUspOuInvoiceDtl.Totality = totality;
            if (price != 0 || isZeroPrice)
                _infoUspOuInvoiceDtl.Price = price;
            _infoUspOuInvoiceDtl.Amount = BLL.Common.Utils.Round(_infoUspOuInvoiceDtl.Price * _infoUspOuInvoiceDtl.Totality, 2);
            _infoUspOuInvoiceDtl.Memo = memo;
            _infoUspOuInvoiceDtl.RecipeGroupId = recipeGroupId;
            _infoUspOuInvoiceDtl.XDRpId = xdRpId;
            _infoUspOuInvoiceDtl.F3 = "自动收";
            _infoUspOuInvoiceDtl.ExecLocId = this.hisOuHosInfo1.Value.DiagnDept;
            if (memo == "记帐凑整" || _infoUspOuInvoiceDtl.ItemId == TallyIntegralItemId)
                _infoUspOuInvoiceDtl.DiscDiag = 0;
            AddNewInvoiceDtl();
        }
        /// <summary>
        ///  刷新时重新加载病人的门诊项目
        /// </summary>
        public override void Refresh()
        {
            base.Refresh();
            HisOuHosInfo_RecordFound(this.hisOuHosInfo1.Value);
        }
        public override void Search()
        {
            this.FormStatus = Model.Configuration.ToolbarStatus.View;
            this.utxtInvoNo.Properties.ReadOnly = false;

            Tools.FormSearchBase frmSearch = new Tools.FormSearchBase();
            frmSearch.MenuId = 37;
            frmSearch.SeachKey = "SEARCH.OuInvoice";
            frmSearch.getSelectedEventHandle = GetSelectedFromSearch;
            frmSearch.ShowDialog();
        }
        private bool GetSelectedFromSearch(DataRowView dataSelectRow)
        {
            if (!_lock.Lock(dataSelectRow["MzRegId"]))
            {
                return false;
            }
            int InvoId = Convert.ToInt32(dataSelectRow["ID"]);
            FindInvoiceByInvoNo(_objOuInvoice.GetByID(InvoId));
            this.FormStatus = Model.Configuration.ToolbarStatus.View;
            this.barManager1.Items["ManualRecipe"].Enabled = true;
            SetEnabledbar();
            SetCancelAndBackFeeColorForBalanceDtl();
            return true;
        }
        private void Start()
        {
            _frmCurrInvo.ShowDialog();

        }
        /// <summary>
        ///  打印费用清单
        /// </summary>
        private void PrintFee()
        {

        }
        /// <summary>
        ///  打印清单
        /// </summary>
        private void PrintItem()
        {

        }

        BLL.COuRecipeDtl _bllRecipeDtl = new BLL.COuRecipeDtl();
        Model.OuRecipeDtlInfo _infoRecipeDtl;
        private bool CheckDrugIssued()
        {
            int i;
            string str = string.Empty;
            for (i = 0; i < _lstUspOuInvoiceDtl.Count; i++)
            {
                if (CheckDrugItemIssued(_lstUspOuInvoiceDtl[i].RecipeItemId))
                {
                    str += string.Format("\n{0}", _lstUspOuInvoiceDtl[i].Name);
                }
            }
            if (str != string.Empty)
            {
                if (Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOuDrugIssueByInvoice")))
                {
                    Utilities.Information.ShowMsgBox("这个发票的药品己发药但药房未退，您现在不能退费。请通知药房退药后才能退费");
                    return false;
                }
                else //if (MessageBox.Show(this, string.Format("以下药品己发药但药房未退，是否确认退费？{0}", str), "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                {
                    MessageBox.Show(string.Format("以下药品己发药但药房未退，不能退费？{0}", str));
                    return false;
                }

            }
            return true;
        }
        private bool CheckHasCkLab()
        {
            if (Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOtherLISInterface")))
            {
                BLL.COuRecipe objOuRecipe = new BLL.COuRecipe();
                BLL.Cout_t_jczxxx objOtherLIS = new BLL.Cout_t_jczxxx();
                Model.ModelList<Model.OuRecipeInfo> lstOuRecipe = objOuRecipe.OuRecipe_SelectByMzRegId(_infoOuInvoice.MzRegId).Find("LsRepType", "2");
                if (lstOuRecipe.Count == 0) return false;
                for (int i = 0; i < _lstUspOuInvoiceDtl.Count; i++)
                {
                    string lisNo = string.Format("{0}{1}", lstOuRecipe[0].ID, _lstUspOuInvoiceDtl[i].Code);
                    if (objOtherLIS.out_t_jczxxx_SelectByJYJCDHAndEVENTID(lisNo, 2).Count > 0 && objOtherLIS.out_t_jczxxx_SelectByJYJCDHAndEVENTID(lisNo, 3).Count == 0)
                    {
                        this.InformationInMainForm = string.Format("检验科已经确认接收标本{0}，您现在不能取消，请通知检验科取消执行！", _lstUspOuInvoiceDtl[i].Name);
                        return true;
                    }
                }
            }
            else
            {
                BLL.CCkLab objLIS = new BLL.CCkLab();
                BLL.CCkLabDtl objLISDtl = new BLL.CCkLabDtl();
                BLL.COuRecipeDtl bllOuRecipeDtl = new BLL.COuRecipeDtl();
                Model.BsItemInfo infoBsItem = new Model.BsItemInfo();
                BLL.CBsItem objBsItem = new BLL.CBsItem();
                string lastXDRpId = string.Empty;
                string reStr = string.Empty;
                //Model.ModelList<Model.CkLabInfo> lst = objCKLab.CkLab_SelectByMzRegId(_infoOuInvoice.MzRegId);
                //if (lst.Count > 0 && lst.Find("IsCancel", "True").Count == 0 &&
                //    MessageBox.Show(this, string.Format("该病人的检验单已经发送到检验科，是否确认退费？"), "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                //    return true;
                foreach (Model.uspOuInvoiceDtlQry infoUspOuInvoiceDtl in _lstUspOuInvoiceDtl)
                {
                    Model.OuRecipeDtlInfo infoOuRecipeDtl = bllOuRecipeDtl.GetByID(infoUspOuInvoiceDtl.RecipeItemId);
                    if (infoOuRecipeDtl.ID == 0)
                        continue;
                    if (infoOuRecipeDtl.XDRpId > 0 && lastXDRpId.Contains(string.Format(string.Format("[{0}]", infoOuRecipeDtl.XDRpId.ToString())))) continue;
                    if (infoOuRecipeDtl.XDRpId > 0) infoBsItem = objBsItem.GetByID(infoOuRecipeDtl.XDRpId);
                    Model.CkLabInfo infoCkLab = new Model.CkLabInfo();
                    Model.ModelList<Model.CkLabDtlInfo> lstLabDtl = objLISDtl.GetDynamic(string.Format("(F2='{0}'or F2='{1}') AND F3='{2}'", infoOuRecipeDtl.ID, infoOuRecipeDtl.XDRpId, "门诊"), null);
                    if (lstLabDtl.Count > 0) infoCkLab = objLIS.GetByID(lstLabDtl[0].LabId);
                    if (Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsUseLisSystem")) && lstLabDtl.Count > 0 && infoCkLab.RecieveTime != DateTime.MinValue) //.LsStatus > 1)
                    {
                        //Utilities.Information.ShowMsgBox(string.Format("检验科已经确认接收标本{0}，您现在不能取消，请通知检验科取消接收！", infoUspOuInvoiceDtl.Name));
                        reStr += string.Format("项目{0}\n\r", infoBsItem.ID > 0 ? infoBsItem.Name : infoUspOuInvoiceDtl.Name);
                    }
                    lastXDRpId += string.Format(string.Format("[{0}]", infoOuRecipeDtl.XDRpId.ToString()));
                }
                if (reStr.Trim().Length > 1)
                {
                    if (MessageBox.Show("系统提示", string.Format("检验科已经确认接收以下标本项目\n\r{0}您现在是否要作废该发票?", reStr), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        return true;
                }
            }
            return false;
        }
        private bool CheckHasPsApplyReport()
        {
            BLL.CPsApplyReport objPsApplyReport = new BLL.CPsApplyReport();
            BLL.CPsExecute objPsExecute = new BLL.CPsExecute();

            Model.ModelList<Model.PsApplyReportInfo> lst = objPsApplyReport.PsApplyReport_SelectByMzRegId(_infoOuInvoice.MzRegId);
            foreach (Model.PsApplyReportInfo info in lst)
            {
                if (lst.Count > 0 && objPsExecute.PsExecute_SelectByApplyId(info.ID).Count > 0 &&
                    MessageBox.Show(this, string.Format("该病人的申请单已经发送到影像科，是否确认退费？"), "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                    return true;
            }
            return false;
        }

        private bool CheckDrugItemIssued(int recipeItemId)
        {
            _infoRecipeDtl = _bllRecipeDtl.GetByID(recipeItemId);
            double executedDrugNum = Convert.ToDouble(BLL.absBusiness<Model.absModel>.ExecuteScalar("uspCalOuRecipeDrugNum", recipeItemId.ToString(), string.Empty));
            double Totality = Convert.ToDouble(BLL.absBusiness<Model.absModel>.ExecuteScalar("uspCalOuRecipeF1", recipeItemId.ToString(), string.Empty));
            if ((_infoRecipeDtl.IsIssue && !_infoRecipeDtl.IsBack) || (_infoRecipeDtl.IsBack && Totality > executedDrugNum))
                return true;
            else
                Tools.Utils.TraceFunctionOperate(214);

            if (_infoRecipeDtl.IsIssue && !_infoRecipeDtl.IsBack)
                return true;
            return false;
        }
        private void ExecInFeeUsp()
        {
            DAL.SqlUtil db = new DAL.SqlUtil();
            db.AddParameter("InvoId", _infoOuInvoice.ID);
            db.ExecuteScalar("uspOuInvoiceDtlToInFee");
            this.InformationInMainForm = "病人门诊费用转入住院成功！";
        }
        /// <summary>
        /// 把当前发票退费注销并把其费用转入到该病人的住院费
        /// </summary>
        private void ToInFee()
        {
            if (!IsCanAccessThisModule(Model.Configuration.FuctionAccess.Modify))
                return;
            if (_infoOuInvoice == null || _infoOuInvoice.ID <= 0)
            {
                this.InformationInMainForm = "您还没有打开一张有效的发票！请在“发票号”输入发票号，或请按“查找”";
                return;
            }

            if (_infoOuInvoice.IsCancel)
            {
                this.InformationInMainForm = "该发票己经注销，不能退费！";
                return;
            }
            if (_infoOuInvoice.F2.Trim() != string.Empty)
            {
                this.InformationInMainForm = "该发票己生成交款报表，不能退费！";
                return;
            }
            BLL.CInHosInfo objInHosInfo = new BLL.CInHosInfo();
            Model.ModelList<Model.InHosInfoInfo> lstInHosInfo = objInHosInfo.InHosInfo_SelectByMzRegId(_infoOuInvoice.MzRegId);
            if (lstInHosInfo.Count == 0)
                lstInHosInfo = objInHosInfo.GetDynamic(string.Format("CardNo='{0}' and LsInStatus in (1,2,5)", this.hisOuHosInfo1.Value.CardNo), null);
            if (lstInHosInfo.Count == 0)
            {
                this.InformationInMainForm = "该病人还没有登记入院，不能转入住院费！";
                return;
            }

            CancelInvoice("转入住院费");
            ExecInFeeUsp();
            ResetAfterCancel();
            Tools.Utils.TraceFunctionOperate(264);
        }

        /// <summary>
        /// 退费注销
        /// </summary>
        /// 
        private void CancelPat()
        {
            string cancelMemo = string.Empty;
            Model.ModelList<Model.OuInvoiceInfo> lstOuInvoice = _objOuInvoice.GetDynamic(string.Format("IsCancel=0 and MzRegId = {0}", this.hisOuHosInfo1.Value.ID), null);
            foreach (Model.OuInvoiceInfo infoOuInvoice in lstOuInvoice)
            {
                if (!FindInvoiceByInvoNo(infoOuInvoice))
                    continue;
                if (MessageBox.Show(this, string.Format("您是否确认要作废发票[{0}]并退费？", infoOuInvoice.InvoNo), "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    cancelMemo = CancelOne(cancelMemo);
            }
            Tools.Utils.TraceFunctionOperate(262);
        }
        BLL.COuDiagCall _objDiag = new BLL.COuDiagCall();
        Model.ModelList<Model.OuDiagCallInfo> _lstDiagCall = new Model.ModelList<Model.OuDiagCallInfo>();
        public override void Cancel()
        {
            CancelOne(string.Empty);
            ClearReadCardInfo();
        }
        private string CancelOne(string cancelMemo)
        {
            if (!IsCanAccessThisModule(Model.Configuration.FuctionAccess.Modify))
                return string.Empty;
            if (_infoOuInvoice == null || _infoOuInvoice.ID <= 0)
            {
                this.InformationInMainForm = "您还没有打开一张有效的发票！请在“发票号”输入发票号，或请按“查找”";
                return string.Empty;
            }
            if (_infoOuInvoice.OperId != Model.Configuration.UserProfiles.UserID && !IsCanAccessThisModule(Model.Configuration.FuctionAccess.Location))
            {
                this.InformationInMainForm = "您不能注销其他收费员的发票！请通知原收费员注销或向系统管理员申请权限";
                return string.Empty;
            }
            if (_infoOuInvoice.IsCancel)
            {
                this.InformationInMainForm = "该发票己经注销，不能退费！";
                return string.Empty;
            }
            if (CheckHasCkLab())
            {
                this.InformationInMainForm = "该病人的检验单已经发送到检验科，不能退费！请通知检验人员退检才能退费";
                Tools.Utils.TraceFunctionOperate(213);
                return string.Empty;
            }
            if (CheckHasPsApplyReport())
            {
                this.InformationInMainForm = "该病人的申请单已经发送到影像科并已审核，不能退费！请通知影像取消确认审核才能退费";
                Tools.Utils.TraceFunctionOperate(216);
                return string.Empty;
            }
            if (!CheckDrugIssued())
            {
                this.InformationInMainForm = "该处方己发药，不能退费！";
                Tools.Utils.TraceFunctionOperate(213);
                return string.Empty;
            }

            if (_infoOuInvoice.F2.Trim() != string.Empty && System.Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOuChargeTallyBack")))
            {
                this.InformationInMainForm = "该发票己生成交款报表，不能退费！";
                return string.Empty;
            }

            if (!CancelInvoice(cancelMemo))
                return string.Empty;
            if (CheckHasRoomIssueDrug(0) && Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOuChargeSavePrintRecie")) && Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOuRoomAutoPrint")) && (Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOuCancelInvoiceAlertRoom")) ||
                MessageBox.Show(this, string.Format("是否需要通知药房人员病人已退费，以免药房配药并等待病人？"), "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
            {
                PrintRoomCancel(cancelMemo);
            }
            _lstDiagCall = _objDiag.OuDiagCall_SelectByMzRegId(this.hisOuHosInfo1.Value.ID).Find("F3", "0");
            if (_lstDiagCall.Count > 0)
            {
                _objDiag.OuDiagCall_DeleteByMzRegId(this.hisOuHosInfo1.Value.ID, null);
            }
            ResetAfterCancel();
            this.utxtInvoNo.Properties.ReadOnly = false;
            Tools.Utils.TraceFunctionOperate(216);
            return cancelMemo;
        }
        private bool CheckHasRoomIssueDrug(int roomId)
        {
            BLL.COuRecipeDtl objOuRecipeDtl = new BLL.COuRecipeDtl();
            BLL.CBsUsage objUsage = new BLL.CBsUsage();
            BLL.CRmUnderLine objUnderLine = new BLL.CRmUnderLine();
            bool roomDrug = true;
            foreach (Model.uspOuInvoiceDtlQry infoOuInvoiceDtl in _lstUspOuInvoiceDtl)
            {
                if (infoOuInvoiceDtl.RecipeItemId == 0) continue;
                Model.OuRecipeDtlInfo infoOuRecipeDtl = objOuRecipeDtl.GetByID(infoOuInvoiceDtl.RecipeItemId);
                Model.BsUsageInfo infoUsage = objUsage.GetByID(infoOuRecipeDtl.UsageId);
                if (roomId > 0)
                    roomDrug = objUnderLine.RmUnderLine_SelectByItemIdAndRoomId(infoOuRecipeDtl.ItemId, roomId).Count > 0;
                if (roomDrug && !infoOuRecipeDtl.IsBack &&
                    (roomId == Convert.ToInt32(BLL.Common.Utils.GetSystemSetting("OuChinRoomId")) ||   //西药要检查用法不是注射室的
                    (infoOuRecipeDtl.UsageId > 0 && (!infoOuRecipeDtl.IsAttach || (!infoUsage.IsMzCure && !infoUsage.IsMzDrop && !infoUsage.IsMzReject)))))
                    return true;
            }
            return false;
        }
        /// <summary>
        /// 作废发票
        /// </summary>
        int _invoLastId;
        int _mzRegLastId;
        BLL.COuRecipeBack bllRecipeBack = new BLL.COuRecipeBack();
        public bool CancelInvoice(string cancelMemo)
        {
            if (_infoOuInvoice.PatTypeId != this.hisOuHosInfo1.Value.PatTypeId)
            {
                MessageBox.Show(this, string.Format("该发票的病人类别与现在病人信息的不一致，请修改后再作废！"), "系统提示");
                return false;
            }
            DAL.SqlUtil db = new DAL.SqlUtil();
            BLL.COuRecipeDtl bllOuRecipeDtl = new BLL.COuRecipeDtl();
            Model.ModelList<Model.OuRecipeDtlInfo> lstRecipeDtl = new Model.ModelList<Model.OuRecipeDtlInfo>();
            Model.OuRecipeDtlInfo infoOuRecipeDtl;
            BLL.COuRecipeBack bllRecipeBack = new BLL.COuRecipeBack();
            BLL.COuInvoiceDtl bllOuInvoiceDtl = new BLL.COuInvoiceDtl();
            Model.ModelList<Model.OuInvoiceDtlInfo> lstOuInvoiceDtl = new Model.ModelList<Model.OuInvoiceDtlInfo>();
            BLL.COuRecipeTemp objOuRecipeTemp = new BLL.COuRecipeTemp();
            BLL.COuExecuteDtl objOuExecuteDtl = new BLL.COuExecuteDtl();

            System.Data.Common.DbTransaction trn = db.GetSqlTransaction();
            BLL.COuInvoicePay objOuInvoicePay = new BLL.COuInvoicePay();
            Model.ModelList<Model.OuInvoicePayInfo> lst = objOuInvoicePay.OuInvoicePay_SelectByInvoId(_infoOuInvoice.ID);
            _frmOuInvoicePay.LstOuInvoicePay = lst;
            _frmOuInvoicePay.Beprice = BLL.Common.Utils.Round(_infoOuInvoice.Beprice + _infoOuInvoice.AddFee, 2);
            _frmOuInvoicePay.Insurance = _infoOuInvoice.Insurance;
            _frmOuInvoicePay.txtAmount.Text = string.Format("{0:N2}", BLL.Common.Utils.Round(_infoOuInvoice.AmountPay, 2));
            _frmOuInvoicePay.devSimpleGrid1.advBandedGridViewMain.Columns["Amount"].OptionsColumn.AllowEdit = false;
            _frmOuInvoicePay.Text = "请输入退费的支付方式";
            if (_frmOuInvoicePay.ShowDialog() != DialogResult.OK)
                return false;
            _frmOuInvoicePay.devSimpleGrid1.advBandedGridViewMain.Columns["Amount"].OptionsColumn.AllowEdit = true;
            if (lst.Count > 0)
            {
                foreach (Model.OuInvoicePayInfo infoOuInvoicePay in lst)
                {
                    infoOuInvoicePay.F2 = infoOuInvoicePay.PaywayId.ToString();
                    infoOuInvoicePay.InvoId = _infoOuInvoice.ID;
                }
                objOuInvoicePay.Save(lst, trn);
            }
            if (cancelMemo == string.Empty)
            {
                if (System.Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsChargeBackPassword")))
                {
                    cancelMemo = Common.Helper.ConfirmPassWord();
                    if (cancelMemo == null)
                    {
                        trn.Rollback();
                        trn.Dispose();
                        return false;
                    }
                }
                else if (MessageBox.Show(this, string.Format("您是否确认要作废发票并退费？"), "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    trn.Rollback();
                    trn.Dispose();
                    return false;
                }
            }

            //修改处方收费标志,执行类别；
            foreach (Model.uspOuInvoiceDtlQry infoUspOuInvoiceDtl in _lstUspOuInvoiceDtl)
            {
                lstOuInvoiceDtl.Add(bllOuInvoiceDtl.GetByID(infoUspOuInvoiceDtl.ID));
                lstOuInvoiceDtl[lstOuInvoiceDtl.Count - 1].LsPerform = (int)Model.EnumFeeWorkPerform.Refundment;

                infoOuRecipeDtl = bllOuRecipeDtl.GetByID(infoUspOuInvoiceDtl.RecipeItemId);
                if (infoOuRecipeDtl.ID == 0)
                    continue;
                if (infoOuRecipeDtl != null && infoOuRecipeDtl.ID != 0)
                {
                    if (infoOuRecipeDtl.F3 == "自动收" && !infoOuRecipeDtl.IsIssue && objOuExecuteDtl.OuExecuteDtl_SelectByRecipeDtlId(infoUspOuInvoiceDtl.RecipeGroupId).FindInclude("F4", "阳性").Count == 0)
                    {
                        if (infoUspOuInvoiceDtl.RecipeGroupId > 0 && lstRecipeDtl.Find("ID", infoUspOuInvoiceDtl.RecipeGroupId.ToString()).Count == 0)
                        {
                            if (bllOuRecipeDtl.GetByID(infoUspOuInvoiceDtl.RecipeGroupId).ID > 0)
                                lstRecipeDtl.Add(bllOuRecipeDtl.GetByID(infoUspOuInvoiceDtl.RecipeGroupId));
                            //if (bllOuRecipeDtl.GetByID(infoUspOuInvoiceDtl.RecipeGroupId).ID == 0)
                            //    return;
                        }
                        AddRecipeGroupIdDtl(lstRecipeDtl, infoUspOuInvoiceDtl);
                        bllRecipeBack.OuRecipeBack_DeleteByReqDtlId(infoOuRecipeDtl.ID, trn);
                        bllOuRecipeDtl.Remove(infoOuRecipeDtl.ID, trn);
                    }
                    if (infoUspOuInvoiceDtl.LsRpType != (int)Model.EnumRpType.ChineseMedicine)
                        infoOuRecipeDtl.Totality = infoUspOuInvoiceDtl.Totality;
                    lstRecipeDtl.Add(infoOuRecipeDtl);
                }

                BLL.CCkLab objLIS = new BLL.CCkLab();
                BLL.CCkLabDtl objLISDtl = new BLL.CCkLabDtl();
                Model.ModelList<Model.CkLabDtlInfo> lstLabDtl = objLISDtl.GetDynamic(string.Format("(F2='{0}'or F2='{1}') AND F3='{2}'", infoOuRecipeDtl.ID, infoOuRecipeDtl.XDRpId, "门诊"), null);
                Model.CkLabInfo infoCkLab = new Model.CkLabInfo();
                if (lstLabDtl.Count > 0) infoCkLab = objLIS.GetByID(lstLabDtl[0].LabId);
                if (infoCkLab.RecieveTime == DateTime.MinValue)
                {
                    foreach (Model.CkLabDtlInfo infoCkLabDtl in lstLabDtl)
                    {
                        objLISDtl.Remove(infoCkLabDtl.ID, trn);
                    }
                    if (lstLabDtl.Count > 0)
                        objLIS.Remove(lstLabDtl[0].LabId, trn);
                }
                Model.ModelList<Model.OuRecipeBackInfo> lstOuRecipeBack = bllRecipeBack.OuRecipeBack_SelectByReqDtlId(infoOuRecipeDtl.ID);
                lstOuRecipeBack.Fill("IsCharged", "False");
                bllRecipeBack.Save(lstOuRecipeBack, trn);
            }
            try
            {
                //发票
                _invoLastId = _infoOuInvoice.ID;
                _mzRegLastId = _infoOuInvoice.MzRegId;
                _infoOuInvoice.IsCancel = true;
                _infoOuInvoice.CancelOperId = Model.Configuration.UserProfiles.UserID;
                _infoOuInvoice.CancelOperTime = BLL.Common.DateTimeHandler.GetServerDateTime();
                _infoOuInvoice.CancelMemo = cancelMemo;

                _objOuInvoice.SaveChild<Model.OuInvoiceDtlInfo, BLL.COuInvoiceDtl>(_infoOuInvoice, lstOuInvoiceDtl, "InvoId", trn);

                if (cancelMemo != "转入住院费")
                    lstRecipeDtl.Fill("IsCharged", false);
                bllOuRecipeDtl.Save(lstRecipeDtl, trn);


                if (!CancelYBUp())//新医保退费
                {

                    trn.Rollback();
                    trn.Dispose();
                    return false;
                }
                if (this.hisOuHosInfo1.Value.TallyGroupId == 28)
                {
                    ybMZCommon.CancelMzBalance(infoOuHos, _infoOuInvoice.F7);//旧医保退费
                }
                if (Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsUpLoadYbFee")) && this.hisOuHosInfo1.Value.TallyGroupId == 21 && this.hisOuHosInfo1.Value.TallyTypeId == 83)//金保医保退费
                {
                    string result = CancelYBBalance();
                    if (result.Contains("失败"))
                    {
                        trn.Rollback();
                        trn.Dispose();
                        return false;
                    }
                }
                trn.Commit();

                Model.ModelList<Model.OuRecipeTempInfo> lstOuRecipeTemp = new Model.ModelList<Model.OuRecipeTempInfo>();
                Model.ModelList<Model.OuRecipeDtlInfo> lstOuRecipeDtl = new Model.ModelList<Model.OuRecipeDtlInfo>();

                if (Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOuDrugIssueByInvoice")))
                {
                    lstOuRecipeTemp = objOuRecipeTemp.GetDynamic(string.Format("MzRegId={0} and CardNo='{1}'", _infoOuInvoice.MzRegId, _infoOuInvoice.InvoNo), "ID");
                    for (int i = 0; i < lstOuRecipeTemp.Count; i++)
                    {

                        BLL.Finder<Model.OuRecipeDtlInfo> finder = new BLL.Finder<Model.OuRecipeDtlInfo>();
                        finder.AddParameter("@RoomId", lstOuRecipeTemp[i].RoomId);
                        finder.AddParameter("@RecipeId", lstOuRecipeTemp[i].RecipeId);
                        //finder.AddParameter("@LsReportType", lstOuRecipeTemp[i].LsReportType);
                        Model.ModelList<Model.OuRecipeDtlInfo> lstOuRecipeDtlIssue = finder.Find("UspRecipeDtlInvoNoIssue");
                        if (lstOuRecipeDtlIssue.Count == 0)
                            objOuRecipeTemp.Remove(lstOuRecipeTemp[i].ID, null);
                    }
                }
                else
                {
                    lstOuRecipeTemp = objOuRecipeTemp.OuRecipeTemp_SelectByMzRegId(_infoOuInvoice.MzRegId).Find("IsIssue", "False");
                    for (int i = 0; i < lstOuRecipeTemp.Count; i++)
                    {
                        lstOuRecipeDtl = bllOuRecipeDtl.GetDynamic(string.Format("RecipeId={0} and IsCharged=1", lstOuRecipeTemp[i].RecipeId), null);
                        if (lstOuRecipeDtl.Count == 0)
                        {
                            objOuRecipeTemp.Remove(lstOuRecipeTemp[i].ID, null);
                        }
                        //防止医生又拿昨天已经收费发药的门诊号来继续开处方然后又退费
                        BLL.Finder<Model.OuRecipeDtlInfo> finder = new BLL.Finder<Model.OuRecipeDtlInfo>();
                        finder.AddParameter("@RoomId", lstOuRecipeTemp[i].RoomId);
                        finder.AddParameter("@RecipeId", lstOuRecipeTemp[i].RecipeId);
                        Model.ModelList<Model.OuRecipeDtlInfo> lstOuRecipeDtlIssue = finder.Find("UspOuRecipeDtlIsIssue");
                        if (lstOuRecipeDtlIssue.Count == 0)
                        {
                            lstOuRecipeTemp[i].IsIssue = true;
                            objOuRecipeTemp.Modify(lstOuRecipeTemp[i], null);
                        }
                    }
                }

                Tools.Utils.TraceFunctionOperate(211);
            }
            catch (Exception e)
            {
                trn.Rollback();
                trn.Dispose();
                throw (e);
            }
            return true;
        }
        private bool CancelYBUp()
        {
            infoOuHos = objOuHos.GetByID(this.hisOuHosInfo1.Value.ID);
            infoOuHos.F5 = _infoOuInvoice.F4;
            BLL.CYbPatSeq objYbPatSeq = new BLL.CYbPatSeq();
            string returnString = string.Empty;
            try
            {
                if (this.hisOuHosInfo1.Value.IsYb && this.hisOuHosInfo1.Value.TallyTypeName.Contains("职工"))
                {
                    ybInterface = YBInterface.YbFactory.Create(this.hisOuHosInfo1.Value.TallyTypeId, 1);//1门诊2住院
                    if (ybInterface == null) return false;
                    //  Model.ModelList<Model.YbPatSeqInfo> lstYbPatSeq = objYbPatSeq.GetDynamic(string.Format("{0}",_infoOuInvoice.ID),"ID");
                    //  ybInterface.YbPatSeqInfo = lstYbPatSeq[0];
                    ybInterface.InfoOuInvoice = _infoOuInvoice;

                    if (MessageBox.Show(string.Format("该发票是进行过医保联网结算的，是否继续退费？"), "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                        return false;
                    if (this.hisOuHosInfo1.Value.TallyGroupName == "东软医保" && this.hisOuHosInfo1.Value.TallyGroupId == 28)
                    {
                        returnString = ybInterface.CancelUpLoadOuFee();
                    }
                    if (!returnString.Contains("成功"))
                    {
                        MessageBox.Show(string.Format("医保结算取消失败：{0}", returnString), "提示");
                        return false;
                    }
                }
                else return true;
            }
            catch (Exception e)
            {
                infoOuHos = objOuHos.GetByID(this.hisOuHosInfo1.Value.ID);
                MessageBox.Show(string.Format("医保结算取消失败，发生异常：{0}", e.Message), "提示");
                return false;
            }
            return true;
        }
        private void AddRecipeGroupIdDtl(Model.ModelList<Model.OuRecipeDtlInfo> lstRecipeDtl, Model.uspOuInvoiceDtlQry infoUspOuInvoiceDtl)
        {
            char[] split = { ',', '，' };
            string[] strSplit = infoUspOuInvoiceDtl.F4.Split(split);
            BLL.COuRecipeDtl objOuRecipeDtl = new BLL.COuRecipeDtl();
            for (int i = 0; i < strSplit.Length; i++)
            {
                if (!Utilities.Information.IsNumeric(strSplit[i]))
                    return;
                int RecipeGroupId = Convert.ToInt32(strSplit[i]);
                if (RecipeGroupId > 0 && lstRecipeDtl.Find("ID", RecipeGroupId.ToString()).Count == 0)
                {
                    Model.OuRecipeDtlInfo InfoRecipeDtl = objOuRecipeDtl.GetByID(RecipeGroupId);
                    if (InfoRecipeDtl.ID > 0)
                        lstRecipeDtl.Add(InfoRecipeDtl);
                }
            }
        }
        /// <summary>
        /// 撤消时恢复到界面刚打开的状态
        /// </summary>
        private void ResetAfterCancel()
        {
            BLL.Common.Utils.RefleshBsItem();
            ClearToInit();
            _lastMzRegId = 0;
            this.FormStatus = Model.Configuration.ToolbarStatus.View;
            this.hisOuHosInfo1.Clear();//将病人信息设为空
            this.hisOuHosInfo1.EnabledControl(true);
            this.hisOuHosInfo1.Focus();
            this.hisOuHosInfo1.uoupCardNo.Focus();

            this.uicItemId.ReturnInitRpType();
            this.uicItemId.LstBsItemMini = _lstBsItemMini;
            _frmOuInvoicePay.LstOuInvoicePay.Clear();
            _frmOuInvoicePay.btnMerge.Visible = false;
            this.utxtInvoNo.Properties.ReadOnly = false;
            this.IsDoctorInput = false;
            _lock.UnLock();
            if (System.Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsUseMiniCard")))
                this.hisOuHosInfo1.SetFocusControl(Model.EnumHisOuPatientSearchField.CardNo);
            else
            {
                //this.hisOuHosInfo1.SetFocusControl(Model.EnumHisOuPatientSearchField.MzRegNo);
                this.hisOuHosInfo1.SetFocusControl(Model.EnumHisOuPatientSearchField.CardNo);
            }
        }
        /// <summary>
        /// 清空所有信息
        /// </summary>
        private void ClearToInit()
        { ///清除所有的信息
            _infoOuInvoice = new Model.OuInvoiceInfo();
            _infoUspOuInvoiceDtl = new Model.uspOuInvoiceDtlQry();
            _lstUspOuRecipeDtlForOuInvoiceDtl = new Model.ModelList<Model.uspOuRecipeDtlForOuInvoiceDtlQry>();
            _lstSourceItem.Clear();
            _lstUspOuInvoiceDtl.Clear();
            _lstHasRecipeDtl.Clear();
            _lstUspOuInvoiceInvItemGoupSumQry.Clear();
            _lstUspOuInvoiceFeetyGoupSumQry.Clear();
            InitData();
            BindBsItemData(new Model.uspOuInvoiceDtlQry(), false);
            this.uicItemId.txtBsItem.Text = string.Empty;
            SetToolbarItemStatus("InvoiceCancel", false);

            this.devGrid1.DataSource = _lstUspOuInvoiceDtl;
            this.usgFeetyType.DataSource = _lstUspOuInvoiceFeetyGoupSumQry;
            this.usgInvItemType.DataSource = _lstUspOuInvoiceInvItemGoupSumQry;
            this.utxtName.Text = string.Empty;
            _frmOuInvoicePay.ChargeBack = 0;
            _limitTop = 0.00;
            ReadyToEdit = false;
            isNewOuhospInfo = true;
            this.ubsExecLocId.ID = 0;
            SameItemId = string.Empty;
            _lstSelectItem.Clear();
            _lstSelectRecipe.Clear();
            this.IsDoctorInput = false;
        }
        Tools.SoundLed led = new Tools.SoundLed();

        ///// <summary>
        ///// 打印发票报表
        ///// </summary>
        ///// <param name="invoId"></param>
        //private bool PrintReport(int invoId, string payWay)
        //{
        //    try
        //    {
        //        if (_infoOuInvoice.AmountPay == 0) return true;

        //        XYHIS.Print.FrmInvoicePrint frmInvoice = new XYHIS.Print.FrmInvoicePrint();
        //        frmInvoice.Text = "打印发票";
        //        if (Convert.ToInt32(BLL.Common.Utils.GetSystemSetting("DefaltProvinceId")) == 20)  //自定义发票
        //        {
        //            BLL.Finder<Model.uspOuInvoicePrintQryBYQZ> objFindOuInvoicePrint = new BLL.Finder<Model.uspOuInvoicePrintQryBYQZ>();
        //            objFindOuInvoicePrint.AddParameter("InvoId", invoId);
        //            Model.ModelList<Model.uspOuInvoicePrintQryBYQZ> lstUspOuInvoicePrint = objFindOuInvoicePrint.Find("uspOuInvoicePrintQZ");
        //            //if (lstUspOuInvoicePrint.Count != 1) return true;
        //            lstUspOuInvoicePrint[0].Pay_way = payWay;
        //            if (Convert.ToBoolean(BLL.Common.Utils.ReadLocalRegitFile("IsPrintPreview").ToLower()))
        //            {
        //                PrintReport.Fee.OuInvoiceQZ OuInvoicePreview = new PrintReport.Fee.OuInvoiceQZ();
        //                OuInvoicePreview.SetDataSource(lstUspOuInvoicePrint);

        //                frmInvoice.ReportViewer.ReportSource = OuInvoicePreview;
        //                frmInvoice.WindowState = System.Windows.Forms.FormWindowState.Normal;
        //                if (frmInvoice.ShowDialog() != DialogResult.OK) return true;
        //            }
        //            PrintReport.Fee.OuInvoiceQZPrinting OuInvoicePrinting = new PrintReport.Fee.OuInvoiceQZPrinting();
        //            OuInvoicePrinting.SetDataSource(lstUspOuInvoicePrint);
        //            //OuInvoicePrinting.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperLetter;
        //            //OuInvoicePrinting.PrintOptions.PrinterName = BLL.Common.Utils.GetPrinterName(OuInvoicePrinting.ResourceName);
        //            OuInvoicePrinting.PrintToPrinter(1, false, 0, 0);
        //            OuInvoicePrinting.Dispose();
        //            return true;
        //        }
        //        else
        //        {
        //            BLL.Finder<Model.uspOuInvoicePrintQry> objFindOuInvoicePrint = new BLL.Finder<Model.uspOuInvoicePrintQry>();
        //            objFindOuInvoicePrint.AddParameter("InvoId", invoId);
        //            Model.ModelList<Model.uspOuInvoicePrintQry> lstUspOuInvoicePrint = objFindOuInvoicePrint.Find("uspOuInvoicePrint");
        //            if (lstUspOuInvoicePrint.Count != 1) return true;
        //            lstUspOuInvoicePrint[0].Pay_way = payWay;
        //            if (Convert.ToBoolean(BLL.Common.Utils.ReadLocalRegitFile("IsPrintPreview").ToLower()))
        //            {
        //                PrintReport.Fee.OuInvoice OuInvoicePreview = new PrintReport.Fee.OuInvoice();

        //                OuInvoicePreview.SetDataSource(lstUspOuInvoicePrint);
        //                if (lstUspOuInvoicePrint[0].A16 <= 0)
        //                    OuInvoicePreview.DetailSection1.ReportObjects["TxtBed"].Width = 0;
        //                frmInvoice.ReportViewer.ReportSource = OuInvoicePreview;
        //                frmInvoice.WindowState = System.Windows.Forms.FormWindowState.Normal;
        //                if (frmInvoice.ShowDialog() != DialogResult.OK) return true;
        //            }
        //            PrintReport.Fee.OuInvoicePrinting OuInvoicePrinting = new PrintReport.Fee.OuInvoicePrinting();
        //            if (lstUspOuInvoicePrint[0].A16 <= 0)
        //                OuInvoicePrinting.DetailSection1.ReportObjects["TxtBed"].Width = 0;
        //            if (_infoOuInvoice.InvoLastId > 0)
        //                ((CrystalDecisions.CrystalReports.Engine.TextObject)OuInvoicePrinting.ReportDefinition.ReportObjects["txtRePrint"]).Text = "重打";

        //            if (Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOunvoicePrintTips")))
        //                SetTxtRoom(OuInvoicePrinting);
        //            if (System.Math.Abs(lstUspOuInvoicePrint[0].AddFee)!=0 && Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsInvoicPrintAddFee")))
        //                ((CrystalDecisions.CrystalReports.Engine.TextObject)OuInvoicePrinting.ReportDefinition.ReportObjects["TxtAddFee"]).Text = string.Format("凑整费：￥{0}", System.Math.Abs(lstUspOuInvoicePrint[0].AddFee));

        //            OuInvoicePrinting.SetDataSource(lstUspOuInvoicePrint);
        //            Tools.Utils.PrintToSetPrinter(OuInvoicePrinting, string.Empty);
        //            OuInvoicePrinting.Dispose();
        //            return true;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        MessageBox.Show(string.Format("打印发票失败！原因：{0}。", e.Message.ToString()), "系统提示", MessageBoxButtons.OK);
        //        return false;
        //    }
        //    finally
        //    {
        //        ShowLED();
        //    }
        //}
        /// <summary>
        /// 打印发票报表
        /// </summary>
        /// <param name="invoId"></param>
        private bool PrintReport(int invoId, string payWay)
        {
            //try
            //{
            if (_infoOuInvoice.AmountPay == 0 && !Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsPrintInvoiceNoAmountPay")))
                return true;

            if (Convert.ToInt32(BLL.Common.Utils.GetSystemSetting("DefaltProvinceId")) == 20)  //自定义发票
            {
                PrintQz(invoId, payWay);
                if (this.hisOuHosInfo1.Value.TallyGroupId == 21 && this.hisOuHosInfo1.Value.TallyTypeId == 83 && Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsPrintYbInvoNo"))
                    && MessageBox.Show("该病人属于医保病人。您是否要打印小票？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    PrintYbInvoNo(_infoOuInvoice.MzRegId, _infoOuInvoice.InvoNo);
                }
                return true;
            }
            else if (Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsInvoicePrintFree")))// || Convert.ToBoolean(BLL.Common.Utils.ReadLocalRegitFile("IsNurseManageAreaPatient")))  //自定义发票
            {
                PrintGdNew(invoId, payWay);
                return true;
            }
            else
            {
                PrintGdOld(invoId, payWay);
                return true;
            }

            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show(string.Format("打印发票失败！原因：{0}。", e.Message.ToString()), "系统提示", MessageBoxButtons.OK);
            //    return false;
            //}
        }
        private void PrintQz(int invoId, string payWay)
        {
            XYHIS.Print.FrmInvoicePrint frmInvoice = new XYHIS.Print.FrmInvoicePrint();
            frmInvoice.Text = "打印发票";
            BLL.Finder<Model.uspOuInvoicePrintQryBYQZ> objFindOuInvoicePrint = new BLL.Finder<Model.uspOuInvoicePrintQryBYQZ>();
            objFindOuInvoicePrint.AddParameter("InvoId", invoId);
            Model.ModelList<Model.uspOuInvoicePrintQryBYQZ> lstUspOuInvoicePrint = objFindOuInvoicePrint.Find("uspOuInvoicePrintQZ");

            BLL.COuInvoiceYb objOuInvoiceYb = new BLL.COuInvoiceYb();
            Model.ModelList<Model.OuInvoiceYbInfo> lstOuInvoiceYb = new Model.ModelList<Model.OuInvoiceYbInfo>();
            Model.ModelList<Model.OuInvoiceYbInfo> lstOuInvoiceYbMb = new Model.ModelList<Model.OuInvoiceYbInfo>();
            Model.ModelList<Model.OuInvoiceYbInfo> lstOuInvoiceYbPt = new Model.ModelList<Model.OuInvoiceYbInfo>();
            lstOuInvoiceYb = objOuInvoiceYb.OuInvoiceYb_SelectByMzRegIdAndInvoNo(this.hisOuHosInfo1.Value.ID, lstUspOuInvoicePrint[0].Invo_Num);
            lstOuInvoiceYbMb = lstOuInvoiceYb.Find("F1", "2");//筛选慢保病人的发票信息
            lstOuInvoiceYbPt = lstOuInvoiceYb.Find("F1", "1");
            for (int i = 0; i < lstUspOuInvoicePrint.Count; i++)
            {
                if (lstOuInvoiceYbMb.Count > 0)
                    lstUspOuInvoicePrint[i].MbTCAmount = lstOuInvoiceYbMb[0].PubPayMoney;
                if (lstOuInvoiceYbPt.Count > 0)
                    lstUspOuInvoicePrint[i].PtTCAmount = lstOuInvoiceYbPt[0].PayMoney;
                lstUspOuInvoicePrint[i].Sum_Invo = lstUspOuInvoicePrint[i].Sum_Invo - lstUspOuInvoicePrint[i].PtTCAmount - lstUspOuInvoicePrint[i].MbTCAmount;
            }
            //if (lstUspOuInvoicePrint.Count != 1) return true;
            //lstUspOuInvoicePrint[0].Sum_Invo = 
            lstUspOuInvoicePrint[0].Pay_way = payWay;
            if (Convert.ToBoolean(BLL.Common.Utils.ReadLocalRegitFile("IsPrintPreview").ToLower()) && Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsPrintPreviewOuCharge")))
            {
                //PrintReport.Fee.OuInvoiceQZ OuInvoicePreview = new PrintReport.Fee.OuInvoiceQZ();
                string rptName = "OuInvoiceQZPrinting";
                CrystalDecisions.CrystalReports.Engine.ReportDocument OuInvoicePreview = Tools.Utils.GetRptFileByName(rptName);

                OuInvoicePreview.SetDataSource(lstUspOuInvoicePrint);

                frmInvoice.ReportViewer.ReportSource = OuInvoicePreview;
                frmInvoice.WindowState = System.Windows.Forms.FormWindowState.Normal;
                if (frmInvoice.ShowDialog() != DialogResult.OK) return;
            }
            //PrintReport.Fee.OuInvoiceQZPrinting OuInvoicePrinting = new PrintReport.Fee.OuInvoiceQZPrinting();
            string rptName2 = "OuInvoiceQZPrinting";
            CrystalDecisions.CrystalReports.Engine.ReportDocument OuInvoicePrinting = Tools.Utils.GetRptFileByName(rptName2);
            OuInvoicePrinting.SetDataSource(lstUspOuInvoicePrint);
            // OuInvoicePrinting.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperLetter;
            //OuInvoicePrinting.PrintOptions.PrinterName = BLL.Common.Utils.GetPrinterName(OuInvoicePrinting.ResourceName);
            //OuInvoicePrinting.PrintToPrinter(1, false, 0, 0);


            if (!BLL.Common.Utils.GetSystemSetting("LsNotPrintInvoPatType").Contains(string.Format("[{0}]", new BLL.COuInvoice().GetByID(invoId).PatTypeId.ToString())))
            {
                //BLL.CBsPatient objpat = new BLL.CBsPatient();
                //Model.BsPatientInfo patInfo = objpat.GetByID(this.hisOuHosInfo1.Value.PatId);
                //Tools.Utils.ReportDefineText(OuInvoicePrinting, "utxtMedicareNo", patInfo.MedicareNo);//patInfo.MedicareNo);


                //CrystalDecisions.CrystalReports.Engine.ReportDocument OuInvoicePreview = Tools.Utils.GetRptFileByName(rptName2);

                //OuInvoicePreview.SetDataSource(lstUspOuInvoicePrint);

                //frmInvoice.ReportViewer.ReportSource = OuInvoicePreview;
                //frmInvoice.WindowState = System.Windows.Forms.FormWindowState.Normal;
                //if (frmInvoice.ShowDialog() != DialogResult.OK) return;


                Tools.Utils.PrintToSetPrinter(OuInvoicePrinting, "OuInvoiceQZPrinting", 0, 0);
            }
            OuInvoicePrinting.Dispose();
        }
        private void PrintGdOld(int invoId, string payWay)
        {
            XYHIS.Print.FrmInvoicePrint frmInvoice = new XYHIS.Print.FrmInvoicePrint();
            frmInvoice.Text = "打印发票";
            BLL.Finder<Model.uspOuInvoicePrintQry> objFindOuInvoicePrint = new BLL.Finder<Model.uspOuInvoicePrintQry>();
            objFindOuInvoicePrint.AddParameter("InvoId", invoId);
            Model.ModelList<Model.uspOuInvoicePrintQry> lstUspOuInvoicePrint = objFindOuInvoicePrint.Find("uspOuInvoicePrint");
            if (lstUspOuInvoicePrint.Count != 1) return;
            lstUspOuInvoicePrint[0].Pay_way = payWay;
            if (Convert.ToBoolean(BLL.Common.Utils.ReadLocalRegitFile("IsPrintPreview").ToLower()))
            {
                // PrintReport.Fee.OuInvoice OuInvoicePreview = new PrintReport.Fee.OuInvoice();
                string rptName = "OuInvoice";
                CrystalDecisions.CrystalReports.Engine.ReportDocument OuInvoicePreview = Tools.Utils.GetRptFileByName(rptName);

                OuInvoicePreview.SetDataSource(lstUspOuInvoicePrint);
                if (lstUspOuInvoicePrint[0].A16 <= 0)
                    OuInvoicePreview.ReportDefinition.Sections["DetailSection1"].ReportObjects["TxtBed"].Width = 0;//OuInvoicePreview.DetailSection1.ReportObjects["TxtBed"].Width = 0;
                frmInvoice.ReportViewer.ReportSource = OuInvoicePreview;
                frmInvoice.WindowState = System.Windows.Forms.FormWindowState.Normal;
                if (frmInvoice.ShowDialog() != DialogResult.OK) return;
            }


            CrystalDecisions.CrystalReports.Engine.ReportDocument OuInvoicePrinting = Tools.Utils.GetRptFileByName("OuInvoicePrinting");

            if (lstUspOuInvoicePrint[0].A16 <= 0)
            {
                OuInvoicePrinting.ReportDefinition.Sections["DetailSection1"].ReportObjects["TxtBed"].Width = 0;
                // OuInvoicePrinting.DetailSection1.ReportObjects["TxtBed"].Width = 0;
            }
            if (_infoOuInvoice.InvoLastId > 0)
                Tools.Utils.ReportDefineText(OuInvoicePrinting, "txtRePrint", "重打");

            //string roomName = GetRoomName();
            if (Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOunvoicePrintTips")))
                SetTxtRoom(OuInvoicePrinting);

            OuInvoicePrinting.SetDataSource(lstUspOuInvoicePrint);
            if (!BLL.Common.Utils.GetSystemSetting("LsNotPrintInvoPatType").Contains(string.Format("[{0}]", new BLL.COuInvoice().GetByID(invoId).PatTypeId.ToString())))
            {
                Tools.Utils.PrintToSetPrinter(OuInvoicePrinting, "OuInvoicePrinting", 0, 0);
            }
            OuInvoicePrinting.Dispose();
        }
        private void PrintGdNew(int invoId, string payWay)
        {
            XYHIS.Print.FrmInvoicePrint frmInvoice = new XYHIS.Print.FrmInvoicePrint();
            frmInvoice.Text = "打印发票";
            BLL.Finder<Model.uspOuInvoicePrintQry> objFindOuInvoicePrint = new BLL.Finder<Model.uspOuInvoicePrintQry>();
            objFindOuInvoicePrint.AddParameter("InvoId", invoId);
            Model.ModelList<Model.uspOuInvoicePrintQry> lstUspOuInvoicePrint = objFindOuInvoicePrint.Find("uspOuInvoicePrint");
            if (lstUspOuInvoicePrint.Count != 1) return;
            if (lstUspOuInvoicePrint[0].Invo_Num.StartsWith("X"))
            {
                lstUspOuInvoicePrint.Fill("Invo_Num", _infoOuInvoice.InvoNo);
            }
            Model.ModelList<Model.uspInInvoice2PrintQry> lstUspOuInvoicePrintNew = ConvertToNew(lstUspOuInvoicePrint, payWay);
            OpenReportDocument(lstUspOuInvoicePrintNew);

            if (Convert.ToBoolean(BLL.Common.Utils.ReadLocalRegitFile("IsPrintPreview").ToLower()))
            {
                //PrintReport.Fee.InInvoice2 OuInvoicePreview = new PrintReport.Fee.InInvoice2();
                string rptName = "InInvoice2";
                CrystalDecisions.CrystalReports.Engine.ReportDocument OuInvoicePreview = Tools.Utils.GetRptFileByName(rptName);

                OuInvoicePreview.SetDataSource(lstUspOuInvoicePrintNew);
                frmInvoice.ReportViewer.ReportSource = OuInvoicePreview;
                frmInvoice.WindowState = System.Windows.Forms.FormWindowState.Normal;
                if (frmInvoice.ShowDialog() != DialogResult.OK)
                    return;
            }
            if (this.hisOuHosInfo1.Value.Sex == "男")
                Tools.Utils.ReportDefineText(OuInvoicePrinting, "txtBoy", "√");
            else
                Tools.Utils.ReportDefineText(OuInvoicePrinting, "txtGirl", "√");
            if (_infoOuInvoice.InvoLastId > 0)
                Tools.Utils.ReportDefineText(OuInvoicePrinting, "txtRePrint", "重打");
            if (Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOunvoicePrintTips")))
                SetTxtRoom(OuInvoicePrinting);
            Tools.Utils.SetHospitalTitle(OuInvoicePrinting);
            Tools.Utils.ReportDefineText(OuInvoicePrinting, "lblPayWayDesc", payWay);
            Tools.Utils.ReportDefineText(OuInvoicePrinting, "CardNo", "卡号：" + this.hisOuHosInfo1.Value.CardNo);

            Tools.Utils.PrintToSetPrinter(OuInvoicePrinting, "InInvoice2Printing", 0, 0);
            OuInvoicePrinting.Dispose();
            Tools.Utils.TraceFunctionOperate(209);
        }
        private Model.ModelList<Model.uspInInvoice2PrintQry> ConvertToNew(Model.ModelList<Model.uspOuInvoicePrintQry> lstUspOuInvoicePrint, string payWay)
        {
            Model.ModelList<Model.uspInInvoice2PrintQry> lstUspOuInvoicePrintNew = new Model.ModelList<Model.uspInInvoice2PrintQry>();
            Model.uspInInvoice2PrintQry infoNew = lstUspOuInvoicePrint[0].ConvertTo<Model.uspInInvoice2PrintQry>();
            infoNew.Reg_Time = lstUspOuInvoicePrint[0].Invo_Date;
            infoNew.LOCATION = lstUspOuInvoicePrint[0].LocationName;
            infoNew.Hosp_NO = lstUspOuInvoicePrint[0].SequenceNum;
            infoNew.Pay_Self = lstUspOuInvoicePrint[0].Sum_Invo;
            infoNew.Fact_Get = lstUspOuInvoicePrint[0].Sum_Fact;
            infoNew.Bill_ID = lstUspOuInvoicePrint[0].Invo_Num;
            infoNew.BZ = lstUspOuInvoicePrint[0].type_desc;   //病人类别
            if (BLL.Common.Utils.GetSystemSetting("OuInvoicePrintPatTypeNamePatTypeIds").ToString().Contains(string.Format("{0}", this.hisOuHosInfo1.Value.PatTypeId.ToString())))
                infoNew.Pay_WayDesc = lstUspOuInvoicePrint[0].type_desc;    //结算方式
            else
                infoNew.Pay_WayDesc = payWay;

            infoNew.IsOu = "√";
            infoNew.Assessor = lstUspOuInvoicePrint[0].Operator;
            //infoNew.Assessor = Model.Configuration.UserProfiles.UserName;
            if (_infoDoctor == null)
            {
                BLL.CBsDoctor objBsDoctor = new BLL.CBsDoctor();
                _infoDoctor = objBsDoctor.GetByID(this.hisOuHosInfo1.Value.DoctorId);
            }
            infoNew.Sum_Cap = _infoDoctor.Name;
            lstUspOuInvoicePrintNew.Add(infoNew);
            BLL.COuInvoiceInvItemSum objInvItemSum = new BLL.COuInvoiceInvItemSum();
            Model.ModelList<Model.OuInvoiceInvItemSumInfo> lstInvItemSum = objInvItemSum.OuInvoiceInvItemSum_SelectByInvoId(infoNew.ID);
            XYHIS.Common.Helper.SetInvItemOu(infoNew, lstInvItemSum, lstUspOuInvoicePrint[0].AddFee, !this.hisOuHosInfo1.Value.IsYb);
            return lstUspOuInvoicePrintNew;
        }
        //private void SetTxtRoom(PrintReport.Fee.OuInvoicePrinting OuInvoicePrinting)
        //{
        //    BLL.CBsUsage objBsUsage = new BLL.CBsUsage();
        //    BLL.CBsXdRp objXdRp = new BLL.CBsXdRp();
        //    BLL.CBsItem objItem = new BLL.CBsItem();
        //    BLL.CCkItem objCkItem = new BLL.CCkItem();
        //    string mzInjectRoomText = string.Empty;
        //    foreach (Model.OuRecipeDtlInfo infoRecipeDtl in _lstWestRecipeDtl)
        //    {
        //        BLL.CRmUnderLine objRmUnderLine = new BLL.CRmUnderLine();
        //        //查询药房药品设置，如果是中药房发的西药处方（例如本院制剂、某些成药），到中药房，否则就默认到西药房
        //        if (objRmUnderLine.RmUnderLine_SelectByItemIdAndRoomId(infoRecipeDtl.ItemId, Convert.ToInt32(BLL.Common.Utils.GetSystemSetting("OuChinRoomId"))).Count > 0)
        //            ((CrystalDecisions.CrystalReports.Engine.TextObject)OuInvoicePrinting.ReportDefinition.ReportObjects["TxtRoom2"]).Text = "请到门诊中药房取中药";
        //        else
        //        {
        //            if (infoRecipeDtl.UsageId > 0)
        //            {
        //                Model.BsUsageInfo infoBsUsage = objBsUsage.GetByID(infoRecipeDtl.UsageId);
        //                if (infoRecipeDtl.IsAttach && (infoBsUsage.IsMzCure || infoBsUsage.IsMzDrop || infoBsUsage.IsMzReject && Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsInjectRoomSendDrug"))))
        //                    mzInjectRoomText = "请到门诊注射注射治疗";
        //                else
        //                    ((CrystalDecisions.CrystalReports.Engine.TextObject)OuInvoicePrinting.ReportDefinition.ReportObjects["TxtRoom1"]).Text = string.Format("请到{0}取药", _roomWindowName);
        //            }
        //            else
        //                ((CrystalDecisions.CrystalReports.Engine.TextObject)OuInvoicePrinting.ReportDefinition.ReportObjects["TxtRoom1"]).Text = string.Format("请到{0}取药", _roomWindowName);
        //        }
        //    }
        private void SetTxtRoom(CrystalDecisions.CrystalReports.Engine.ReportDocument OuInvoicePrinting)
        {
            BLL.CBsUsage objBsUsage = new BLL.CBsUsage();
            BLL.CBsXdRp objXdRp = new BLL.CBsXdRp();
            BLL.CCkItem objCkItem = new BLL.CCkItem();
            BLL.CBsRoom objRoom = new BLL.CBsRoom();
            BLL.CBsLocation objBsLocation = new BLL.CBsLocation();
            string mzInjectRoomText = string.Empty;
            bool DropReject = false;
            bool IsPriority = _lstUspOuInvoiceDtl.Find("IsPriority", "True").Count > 0;
            foreach (Model.OuRecipeDtlInfo infoRecipeDtl in _lstWestRecipeDtl)
            {
                BLL.CRmUnderLine objRmUnderLine = new BLL.CRmUnderLine();
                //查询药房药品设置，如果是中药房发的西药处方（例如本院制剂、某些成药），到中药房，否则就默认到西药房
                int ouChinRoomId = Convert.ToInt32(BLL.Common.Utils.GetSystemSetting("OuChinRoomId"));
                if (objBsLocation.GetByID(objRoom.GetByID(ouChinRoomId).LocationId).HospitalId == Model.Configuration.UserProfiles.HospitalID &&
                    objRmUnderLine.RmUnderLine_SelectByItemIdAndRoomId(infoRecipeDtl.ItemId, ouChinRoomId).Count > 0)
                    Tools.Utils.ReportDefineText(OuInvoicePrinting, "TxtRoom2", "请到门诊中药房取中药");
                else
                {
                    if (infoRecipeDtl.UsageId > 0)
                    {
                        Model.BsUsageInfo infoBsUsage = objBsUsage.GetByID(infoRecipeDtl.UsageId);
                        if (infoRecipeDtl.IsAttach && (infoBsUsage.IsMzCure || infoBsUsage.IsMzDrop || infoBsUsage.IsMzReject))
                        {
                            DropReject = true;
                        }
                        if (!IsPriority && infoRecipeDtl.IsAttach && (infoBsUsage.IsMzCure || infoBsUsage.IsMzDrop || infoBsUsage.IsMzReject && Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsInjectRoomSendDrug"))))
                            mzInjectRoomText = "请到门诊注射室注射治疗";
                        else
                            Tools.Utils.ReportDefineText(OuInvoicePrinting, "TxtRoom1", string.Format("请到{0}取药", _roomWindowName));
                    }
                    else
                        Tools.Utils.ReportDefineText(OuInvoicePrinting, "TxtRoom1", string.Format("请到{0}取药", _roomWindowName));
                }
            }
            //LIS检验抽血
            int testId = 0;
            foreach (Model.OuRecipeDtlInfo infoRecipeDtl in _lstCheckRecipeDtl)
            {
                if (infoRecipeDtl.XDRpId > 0)
                    testId = objXdRp.GetByID(infoRecipeDtl.XDRpId).TestId;
                Model.BsItemInfo infoItem = BLL.Common.Utils.GetBaseTableRowInfo<Model.BsItemInfo>("BsItem", infoRecipeDtl.ItemId);
                if (infoItem.LsRpType == 4 && infoItem.LISCode > 0 && !mzInjectRoomText.Contains("抽血"))
                    testId = infoItem.LISCode;

                if (testId == 0) continue;
                Model.CkItemInfo infoCkItem = objCkItem.GetByID(testId);
                if ((infoCkItem.PrintHigh != string.Empty && !mzInjectRoomText.Contains(infoCkItem.PrintHigh)) || mzInjectRoomText == string.Empty)
                    mzInjectRoomText += string.Format("，请到{0}进行检验", infoCkItem.PrintHigh == string.Empty ? "检验科" : infoCkItem.PrintHigh);
                else if (infoCkItem.PrintHigh == string.Empty && !mzInjectRoomText.Contains("抽血"))
                    mzInjectRoomText += string.Format("，请到{0}进行抽血检验", BLL.Common.Utils.GetSystemSetting("OuChargePrintLisRegPoint"));
            }
            try
            {
                Tools.Utils.ReportDefineText(OuInvoicePrinting, "TxtPsApply", GetExceLocName());
                Tools.Utils.ReportDefineText(OuInvoicePrinting, "TxtRoom3", mzInjectRoomText.StartsWith("，") ? mzInjectRoomText.Substring(1) : mzInjectRoomText);
            }
            catch (Exception es)
            {
                Utilities.Document document = new Utilities.Document();
                document.SaveLog(es.Message, "Log.log");
            }

            if (_lstChineseRecipeDtl.Count > 0)
                Tools.Utils.ReportDefineText(OuInvoicePrinting, "TxtRoom2", "请到门诊中药房取中药");

            if (DropReject && IsPriority)
            {
                Tools.Utils.ReportDefineText(OuInvoicePrinting, "DropReject", "请到急诊注射室注射治疗");
            }
            Tools.Utils.TraceFunctionOperate(213);
        }
        private string GetExceLocName()
        {
            BLL.CBsLocation objBsLocation = new BLL.CBsLocation();
            string exceLocName = string.Empty;
            string tipExceLocId = BLL.Common.Utils.GetSystemSetting("OuInvoiceTipExceLocId");
            foreach (Model.OuRecipeDtlInfo infoRecipeDtl in _lstCheckRecipeDtl)
            {
                int locationId = BLL.Common.Utils.GetBaseTableRowInfo<Model.BsItemInfo>("BsItem", infoRecipeDtl.ItemId).LocationId;
                string locationName = objBsLocation.GetByID(locationId).F3;
                if (tipExceLocId.Contains(string.Format("[{0}]", locationId.ToString())) && !exceLocName.Contains(string.Format("，{0}", locationName)))
                    exceLocName += "，" + locationName;
            }
            if (exceLocName.StartsWith("，"))
                exceLocName = exceLocName.Substring(1);
            if (exceLocName == string.Empty)
                return string.Empty;
            else
                return string.Format("请到{0}进行检查", exceLocName);
        }
        /// <summary>
        /// 获得发药药房
        /// </summary>
        private string GetRoomName()
        {
            string roomName = string.Empty;
            BLL.Finder<Model.uspItemRoomQry> _finderRoom = new BLL.Finder<Model.uspItemRoomQry>();
            foreach (Model.uspOuInvoiceDtlQry info in _lstUspOuInvoiceDtl)
            {
                _finderRoom.AddParameter("HospitalId", Model.Configuration.UserProfiles.HospitalID);
                _finderRoom.AddParameter("ItemId", info.ItemId);
                Model.ModelList<Model.uspItemRoomQry> lst = _finderRoom.Find("uspItemRoom");
                foreach (Model.uspItemRoomQry infoItemRoom in lst)  //查找药房药品设置的药房，如果是门诊药房才显示
                {
                    if ((infoItemRoom.LsInOut == (int)Model.EnumRoomInOut.ForOutPatient || infoItemRoom.LsInOut == (int)Model.EnumRoomInOut.Other) && roomName.IndexOf(infoItemRoom.Name) < 0)
                        roomName = string.Format("{0}，{1}", roomName, infoItemRoom.Name);
                }
            }
            if (roomName.StartsWith("，"))
                roomName = roomName.Substring(1);
            Tools.Utils.TraceFunctionOperate(216);
            return roomName;
        }

        /// <summary>
        ///重打发票
        /// </summary>
        private void ReprintInvoice()
        {
            if (_infoOuInvoice.ID == 0) return;
            BLL.COuInvoicePay objOuInvoicePay = new BLL.COuInvoicePay();
            Model.ModelList<Model.OuInvoicePayInfo> lstOuInvoicePay = objOuInvoicePay.OuInvoicePay_SelectByInvoId(_infoOuInvoice.ID);
            UpdateInvoNo();
            if (PrintReport(_infoOuInvoice.ID, GetPayWay(lstOuInvoicePay)) && IsUpdateInvoNo)
            {
                _infoOuInvoice.F3 = Model.Configuration.UserProfiles.UserID.ToString();
                _objOuInvoice.Modify(_infoOuInvoice, null);
                _frmCurrInvo.CalculateNextInvoNo();
                _frmCurrInvo.SaveInvoNo();
            }
            if (this.hisOuHosInfo1.Value.TallyGroupId == 21 && this.hisOuHosInfo1.Value.TallyTypeId == 83 && Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsPrintYbInvoNo")))
            {
                Model.ModelList<Model.OuInvoiceYbInfo> lstOuInvoiceYb = new Model.ModelList<Model.OuInvoiceYbInfo>();
                BLL.COuInvoiceYb objOuInvoiceYb = new BLL.COuInvoiceYb();
                lstOuInvoiceYb = objOuInvoiceYb.GetDynamic(string.Format(" MzRegId={0} and InvoNo='{1}' ", _infoOuInvoice.MzRegId, _infoOuInvoice.InvoNo), null);
                if (lstOuInvoiceYb.Count != 1) return;
                if (MessageBox.Show(string.Format("该病人属于医保病人。\n病人结算前账户余额：{0}，结算后账户余额：{1}。\n您是否要打印小票？", lstOuInvoiceYb[0].PMoneyBef, lstOuInvoiceYb[0].PMoneyAft), "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    PrintYbInvoNo(_infoOuInvoice.MzRegId, _infoOuInvoice.InvoNo);
                }
            }
            //if (this.hisOuHosInfo1.Value.TallyGroupId == 21 && this.hisOuHosInfo1.Value.TallyTypeId == 83 && Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsPrintYbInvoNo"))
            //    && MessageBox.Show("该病人属于医保病人。您是否要打印小票？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            //{
            //    PrintYbInvoNo(_infoOuInvoice.MzRegId, _infoOuInvoice.InvoNo);
            //}
            IsUpdateInvoNo = false;
            Tools.Utils.TraceFunctionOperate(210);
        }


        private void PrintYbInvoNo(int mzRegId, string InvoNo)
        {
            Model.ModelList<Model.OuInvoiceYbInfo> lstOuInvoiceYb = new Model.ModelList<Model.OuInvoiceYbInfo>();
            BLL.COuInvoiceYb objOuInvoiceYb = new BLL.COuInvoiceYb();
            lstOuInvoiceYb = objOuInvoiceYb.GetDynamic(string.Format(" MzRegId={0} and InvoNo='{1}' ", mzRegId, InvoNo), null);
            if (lstOuInvoiceYb.Count != 1) return;
            CrystalDecisions.CrystalReports.Engine.ReportDocument OuInvoiceYbPrinting = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            OuInvoiceYbPrinting = Tools.Utils.GetRptFileByName("OuInvoiceYbPrintingQz");
            OuInvoiceYbPrinting.SetDataSource(lstOuInvoiceYb);
            Tools.Utils.PrintToSetPrinter(OuInvoiceYbPrinting, "OuInvoiceYbPrintingQz", 0, 0);
            OuInvoicePrinting.Dispose();
            Tools.Utils.TraceFunctionOperate(209);
        }

        bool IsUpdateInvoNo = false;
        private void UpdateInvoNo()
        {
            if (_infoOuInvoice.InvoNo.StartsWith("20"))
            {
                _infoOuInvoice.InvoNo = this.utxtInvoNo.Text = _frmCurrInvo.GetInvoiceNoFromConfigFile();
                IsUpdateInvoNo = true;
            }
            else if (_infoOuInvoice.InvoNo.StartsWith("X"))
            {
                _infoOuInvoice.InvoNo = this.utxtInvoNo.Text = _frmCurrInvo.GetInvoiceNoFromConfigFile();
                IsUpdateInvoNo = true;
            }
        }
        /// <summary>
        /// 开始收费
        /// </summary>
        BLL.CBsTallyGroup objBsTallyGroup = new BLL.CBsTallyGroup();
        private void BeginBalance(bool isHidePayForm)
        {
            string MzRegId = this.hisOuHosInfo1.Value.ID.ToString();
            //先删除再加
            BLL.Common.Utils.EndBeginCharge(MzRegId);
            InfoBsPatTypeYbIll = new Model.BsPatTypeYbIllInfo();
            _frmOuInvoicePay.lblMessage.Text = string.Empty;
            if (!IsCanAccessThisModule(Model.Configuration.FuctionAccess.Add))
                return;
            if (!_lock.Lock(this.hisOuHosInfo1.Value.ID)) return;
            BLL.Common.Utils.HoldBeginCharge(MzRegId);
            AcceptData();
            if (!CheckValidate())
            {
                BLL.Common.Utils.EndBeginCharge(MzRegId);
                return;
            }
            if (_isYb)
            {
                Model.BsPatTypeInfo infoPatType = objPatType.GetByID(this.hisOuHosInfo1.Value.PatTypeId);
                if (this.utxtTallyNo.EditValue.ToString() == "0")
                    this.utxtTallyNo.EditValue = infoPatType.LimitFee;
            }
            else this.utxtTallyNo.EditValue = "";

            this.FormStatus = Model.Configuration.ToolbarStatus.Edit;
            AddTallyIntegral();
            CalcuateAmountTally();
            //if (_infoOuInvoice.AmountPay == 0 && _infoOuInvoice.Beprice > 0)
            //{
            //    MessageBox.Show("应交金额为0，不能收费结算！");
            //    return;
            //}
            if (Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsUsePatCardFee")) &&
                Convert.ToInt32(BLL.Common.Utils.GetSystemSetting("PatCardFeeLimitType")) == 1 &&
               !BLL.InsertAccount.CheckPatCardFee(this.hisOuHosInfo1.Value.PatId, _infoOuInvoice.Beprice - _infoOuInvoice.AmountPay))
            {
                BLL.Common.Utils.EndBeginCharge(MzRegId);
                return;
            }
            if (!IsNotPrintInvoice)
                CurrentBalanceNo = _frmCurrInvo.GetInvoiceNoFromConfigFile();
            else
                CurrentBalanceNo = BLL.Common.SequenceNumHandler.GetSequenceNum(Model.EnumSequenceNumType.AutoCharge).ToString();

            if (_infoOuInvoice.AmountPay == 0 && !Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsPrintInvoiceNoAmountPay")))
                this.CurrentBalanceNo = "X" + this.CurrentBalanceNo;
            _infoOuInvoice.InvoNo = this.CurrentBalanceNo;
            //&&(!Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOuBalaceLocal")) && this.hisOuHosInfo1.Value.PatTypeId != Convert.ToInt32(BLL.Common.Utils.GetSystemSetting("IsOuChargeNotLoadYBPatTypeId"))
            if (Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsUpLoadYbFee")) && objBsTallyGroup.GetByID(this.hisOuHosInfo1.Value.TallyGroupId).F3 != string.Empty && this.hisOuHosInfo1.Value.TallyTypeId == 83)
            {
                if (MessageBox.Show("该病人属于医保病人。您是否要上传费用？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    if (!UploadYb())
                    {
                        BLL.Common.Utils.EndBeginCharge(MzRegId);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("该病人属于医保病人。不能按自费病人方式结算，若要继续结算，请将病人类别改为自费！");
                    return;
                }
            }
            if (_isYb && Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOuBalaceLocal")))
            {
                if (_tallyAmount > 0 && _infoOuInvoice.Insurance > _tallyAmount)
                {
                    _infoOuInvoice.Insurance = _tallyAmount;
                    _infoOuInvoice.FactGet = _infoOuInvoice.Beprice - _infoOuInvoice.Insurance;
                    _infoOuInvoice.AmountPay = _infoOuInvoice.Beprice - _infoOuInvoice.Insurance;
                }
                //_infoOuInvoice.F4 = _tallyAmount.ToString();
                InitData();
                if (this.hisOuHosInfo1.Value.TallyGroupId == 21 && this.hisOuHosInfo1.Value.TallyTypeId == 83)
                {
                    utxtInsurance.Text = "";
                }
            }
            //if (this.hisOuHosInfo1.Value.TallyGroupId == 21 && this.hisOuHosInfo1.Value.TallyTypeId == 83)
            //{
            //    utxtInsurance.Text = "";
            //}
            //导出医保数据

            if (SaveOuBalance(isHidePayForm))
                Lock(true);


            this.utxtInvoNo.Properties.ReadOnly = false;
            this.hisOuHosInfo1.Focus();
            lstUserOuIninvoice.Add(_infoOuInvoice);
            BLL.Common.Utils.EndBeginCharge(MzRegId);
            //this.hisOuHosInfo1.SetFocusControl(Model.EnumHisOuPatientSearchField.MzRegNo);
            this.hisOuHosInfo1.SetFocusControl(Model.EnumHisOuPatientSearchField.CardNo);
            this.hisOuHosInfo1.Focus();
        }
        private void GetYBRegNo()
        {
            if (this.hisOuHosInfo1.Value != null && !this.hisOuHosInfo1.Value.IsYb)
                return;
            Model.OuHosInfoInfo infoHosInfoInfo = new Model.OuHosInfoInfo();
            infoHosInfoInfo = objOuHosInfo.GetByID(this.hisOuHosInfo1.Value.ID);
            infoHosInfoInfo.F4 = BLL.Common.SequenceNumHandler.GetSequenceNum(Model.EnumSequenceNumType.YBRegNo).ToString();
            objOuHosInfo.Modify(infoHosInfoInfo, null);
            Tools.Utils.InputString("该病人取医保门诊号成功请“复制”以备用", "医保门诊号", infoHosInfoInfo.F4, false);
        }

        YBInterface.YbProvider ybInterface = null;
        Model.MZYbInterface.PersonInfo infoPersonInfo = new Model.MZYbInterface.PersonInfo();
        private bool UploadYb()
        {
            Model.MZYbInterface.GetUpLoadOuFeeInfo infoGetUpLoadOuFee = new Model.MZYbInterface.GetUpLoadOuFeeInfo();
            if (this.hisOuHosInfo1.Value == null) return false;
            ybInterface = YBInterface.YbFactory.Create(Convert.ToInt32(this.hisOuHosInfo1.Value.TallyTypeId), 1);
            if (ybInterface == null) return false;
            XYHIS.FrmOuChargeHosInfo frm = new FrmOuChargeHosInfo();
            infoPersonInfo = frm.ReadYBCard(ybInterface);
            if (infoPersonInfo == null) return false;
            if (infoPersonInfo.CardNo.Trim() != this.hisOuHosInfo1.Value.MedicareNo.Trim())
            {
                MessageBox.Show(string.Format("您所上传的医保卡号【{0}】与HIS系统登记的医保卡号【{1}】不一致，不能结算，请检查！", infoPersonInfo.CardNo, this.hisOuHosInfo1.Value.MedicareNo));
                return false;
            }
            if (infoPersonInfo.PersName.Trim() != this.hisOuHosInfo1.Value.PatientName.Trim())
            {
                MessageBox.Show(string.Format("您所上传的医保卡病人姓名【{0}】与HIS系统登记的【{1}】不一致，不能结算，请检查！", infoPersonInfo.PersName, this.hisOuHosInfo1.Value.PatientName));
                return false;
            }
            //  GetYBRegNo();
            initYbData();
            ybInterface.LstUspOuInvoiceDtl = _lstUspOuInvoiceDtl;
            ybInterface.InfoOuInvoice = _infoOuInvoice;
            double inSurance = 0;
            Tools.ProgressForm progressForm = new Tools.ProgressForm(this);
            progressForm.Show();
            progressForm.SetProgressValue(1);
            progressForm.Tip = "请等待，正在连接医保系统……";
            progressForm.Tip = "请等待，正在上传费用……";
            progressForm.SetProgressValue(40);
            infoGetUpLoadOuFee = ybInterface.UpLoadOuFeeJb("FOOT");
            progressForm.Tip = "上传费用结束……";
            progressForm.SetProgressValue(50);
            Tools.Utils.TraceFunctionOperate(265);
            bool result = infoGetUpLoadOuFee != null ? true : false;
            if (result)
            {
                progressForm.Tip = "医保系统正在开始结算，请等待……";
                MessageBox.Show("医保上传成功!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                inSurance = Convert.ToDouble(ybInterface.ReturnF3);
                progressForm.SetProgressValue(95);
                progressForm.Tip = "医保系统结算结束！！！请核对各项金额情况！！";


            }
            else
            {
                progressForm.SetProgressValue(100);
                progressForm.Close();
                _lock.UnLock();
                return false;
            }
            Insurance1 = inSurance;
            progressForm.SetProgressValue(100);
            progressForm.Close();
            if (this.hisOuHosInfo1.Value.TallyGroupId != 21)
                _infoOuInvoice.Insurance = inSurance;//记帐金额
            _infoOuInvoice.FactGet = _infoOuInvoice.Beprice - _infoOuInvoice.Insurance;
            _infoOuInvoice.AmountPay = _infoOuInvoice.Beprice - _infoOuInvoice.Insurance;
            //插凑整费用
            if (this.hisOuHosInfo1.Value.TallyGroupId == 21 && this.hisOuHosInfo1.Value.TallyTypeId == 83)
            {
                _infoOuInvoice.AddFee = BLL.Common.Utils.CalculateTint(_infoOuInvoice.Beprice, Convert.ToInt32(BLL.Common.Utils.GetSystemSetting("InTintNumber")), Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("InTintType")));
            }
            else
            {
                _infoOuInvoice.AddFee = BLL.Common.Utils.CalculateTint(_infoOuInvoice.AmountPay, Convert.ToInt32(BLL.Common.Utils.GetSystemSetting("InTintNumber")), Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("InTintType")));
            }
            if (BLL.Common.Utils.Round(System.Math.Abs(_infoOuInvoice.AddFee), 2) >= 0.01)
            {
                _infoOuInvoice.AddFee = BLL.Common.Utils.Round(_infoOuInvoice.AddFee, 2);
                _infoOuInvoice.AmountPay += _infoOuInvoice.AddFee;
            }
            return true;
        }

        BLL.CBsPatient objBsPatient = new BLL.CBsPatient();
        BLL.CBsTallyType objBsTallyType = new BLL.CBsTallyType();
        BLL.CYbPatSeq objYbPatSeq = new BLL.CYbPatSeq();
        private void initYbData()
        {
            _lstUspOuInvoiceDtl.Remove("ItemId", IntegralItemId);
            BLL.COuRecipe objOuRecipe = new BLL.COuRecipe();
            Model.OuHosInfoInfo infoHosInfo = new Model.OuHosInfoInfo();
            Model.BsPatientInfo infoPatientInfo = new Model.BsPatientInfo();
            Model.BsPatTypeInfo infoPatTypeInfo = new Model.BsPatTypeInfo();
            Model.BsTallyTypeInfo infoTallyType = new Model.BsTallyTypeInfo();
            Model.BsTallyGroupInfo infoTallyGroup = new Model.BsTallyGroupInfo();
            Model.OuRecipeInfo infoOuRecipe = new Model.OuRecipeInfo();
            //  Model.YbPatSeqInfo infoYbPatSeq = new Model.YbPatSeqInfo();
            infoHosInfo = objOuHosInfo.GetByID(this.hisOuHosInfo1.Value.ID);
            //if (infoHosInfo.MedicareNo.Trim().Length < 5)
            //    MessageBox.Show("该医保病人没取得医保门诊号将会以自费结算，请注意！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            infoPatientInfo = objBsPatient.GetByID(infoHosInfo.PatId);
            infoPatTypeInfo = objPatType.GetByID(infoHosInfo.PatTypeId);
            infoTallyType = objBsTallyType.GetByID(infoPatTypeInfo.TallyTypeId);
            infoTallyGroup = objBsTallyGroup.GetByID(infoTallyType.TallyGroupId);
            int recipeId = 0;
            foreach (Model.uspOuInvoiceDtlQry info in _lstUspOuInvoiceDtl)
            {
                if (info.RecipeId != 0)
                {
                    recipeId = info.RecipeId;
                    break;
                }
            }
            infoOuRecipe = objOuRecipe.GetByID(recipeId);
            //infoYbPatSeq=objYbPatSeq.
            ybInterface.InfoTallyType = infoTallyType;
            ybInterface.InfoBsPatient = infoPatientInfo;
            ybInterface.InfoPatType = infoPatTypeInfo;
            ybInterface.HosInfoInfo = infoHosInfo;
            //ybInterface.LstUspOuInvoiceDtl = _lstUspOuInvoiceDtl;
            //ybInterface.InfoOuInvoice = _infoOuInvoice;
            ybInterface.InfoPersonInfo = infoPersonInfo;
            ybInterface.InfoTallyGroup = infoTallyGroup;
            ybInterface.InfoOuRecipe = infoOuRecipe;
        }
        private void Setup()
        {
            _frmCurrInvo.ShowDialog();

        }
        /// <summary>
        /// 判断公费病人的记费单是否有值
        /// </summary>
        /// <returns>true/fase(空）</returns>
        double _tallyAmount = 0;
        private bool CheckTallyNoForGfPatient()
        {
            if (_isGf && (this.utxtTallyNo.EditValue == null || this.utxtTallyNo.EditValue.ToString() == string.Empty))
            {
                this.InformationInMainForm = "当前病人为医保病人，请输入记账单号码！";
                CreateInfoToolTip("当前病人为医保病人，请输入记账单号码", this.utxtTallyNo);
                this.utxtTallyNo.Focus();
                return false;
            }
            return true;
        }
        /// <summary>
        /// 输入发票号时显示该发票的明细
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void utxtInvoNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.FormStatus == Model.Configuration.ToolbarStatus.View && e.KeyCode == Keys.Enter)
            {
                if (this.utxtInvoNo.EditValue == null || this.utxtInvoNo.EditValue.ToString() == string.Empty) return;
                if (!FindInvoiceByInvoNo(this.utxtInvoNo.EditValue.ToString()))
                {
                    this.utxtInvoNo.Undo();
                    return;
                }
                SetToolbarItemStatus("InvoiceCancel", true);
                //this.barManager1.Items["InvoiceCancel"].Enabled = true;
                this.utxtInvoNo.Properties.ReadOnly = false;
            }
        }
        /// <summary>
        /// 根据发票实例化发票信息
        /// </summary>
        /// <param name="invoNo"></param>
        /// <returns></returns>
        private bool FindInvoiceByInvoNo(string invoNo)
        {
            Model.ModelList<Model.OuInvoiceInfo> lstOuInvoice = _objOuInvoice.OuInvoice_SelectByInvoNo(invoNo);
            if (lstOuInvoice == null || lstOuInvoice.Count == 0)
            {
                CreateInfoToolTip("找不到此发票号，请核对再输入", this.utxtInvoNo);
                return false;
            }
            return FindInvoiceByInvoNo(lstOuInvoice[0]);
        }
        private bool FindInvoiceByInvoNo(Model.OuInvoiceInfo invoice)
        {
            _infoOuInvoice = invoice;
            //if (_infoOuInvoice.IsCancel)
            //{
            //    this.InformationInMainForm = "该发票已经作废不能再打开！";
            //    CreateInfoToolTip("该发票已经作废不能再打开", this.utxtInvoNo);

            //    return false;
            //}

            if (!_lock.Lock(invoice.MzRegId))
                return false;
            GetOuInvoiceInfomation(invoice);

            InitData();
            //WriteUnbindData();
            ////////ShowInDeposit(_infoInInvoice.ID);//选中时的值为当前发票号的Id
            this.devGrid1.DataSource = _lstUspOuInvoiceDtl;
            ShowFeetyGroup();
            ShowInInvoiceGroup();
            GetChineseRecipe(invoice.MzRegId, "True");
            GetOuRecipeDtl();
            Lock(true);
            this.utxtInvoNo.Properties.ReadOnly = false;
            this.devGrid1.advBandedGridViewMain.OptionsBehavior.Editable = false;
            ReadyToEdit = true;
            return true;

        }
        public void SetEnabledbar()
        {
            this.barManager1.Items["InvoiceCancel"].Enabled = !_infoOuInvoice.IsCancel;
            this.barManager1.Items["Print"].Enabled = !_infoOuInvoice.IsCancel;
            this.barManager1.Items["ManualRecipe"].Enabled = !_infoOuInvoice.IsCancel;

        }
        /// <summary>
        /// 获取发票的信息
        /// </summary>
        /// <param name="infoInvoice">发票</param>
        public void GetOuInvoiceInfomation(Model.OuInvoiceInfo infoInvoice)
        {
            //_infoOuInvoice = infoInvoice;
            //获取病人的基本信息
            this.hisOuHosInfo1.SearchPatient(infoInvoice.MzRegId);
            _infoOuInvoice = infoInvoice;
            //获取发票对应的项目
            _uspOuInvoiceDtlFinder.AddParameter("InvoId", infoInvoice.ID);
            _lstUspOuInvoiceDtl = _uspOuInvoiceDtlFinder.Find("uspOuInvoiceDtl");
            foreach (Model.uspOuInvoiceDtlQry infoInvoiceDtl in _lstUspOuInvoiceDtl)
            {
                Model.OuRecipeDtlInfo infoOuRecipeDtl = _objOuRecipeDtl.GetByID(infoInvoiceDtl.RecipeItemId);
                infoInvoiceDtl.IsIssue = infoOuRecipeDtl.IsIssue;
                Model.ModelList<Model.OuRecipeBackInfo> lstOuRecipeBack = bllRecipeBack.OuRecipeBack_SelectByReqDtlId(infoOuRecipeDtl.ID);
                Model.OuRecipeBackInfo BackOuRecipe = new Model.OuRecipeBackInfo();
                if (lstOuRecipeBack.Count == 0) continue;
                BackOuRecipe = lstOuRecipeBack[0];
                if (!infoInvoiceDtl.IsIssue && Utilities.Information.IsNumeric(BackOuRecipe.F1) && Convert.ToDouble(BackOuRecipe.F1) > lstOuRecipeBack.GetSum("Totality"))
                    infoInvoiceDtl.IsIssue = true;
            }
            _lstUspOuInvoiceDtl.Sort("ID");

            _objUspOuInvoiceFeetyGoupSum.AddParameter("InvoId", infoInvoice.ID);
            _lstUspOuInvoiceFeetyGoupSumQry = _objUspOuInvoiceFeetyGoupSum.Find("uspOuInvoiceFeetyGoupSum");

            _objUspOuInvoiceInvItemGroupSum.AddParameter("InvoId", infoInvoice.ID);
            _lstUspOuInvoiceInvItemGoupSumQry = _objUspOuInvoiceInvItemGroupSum.Find("uspOuInvoiceInvItemGoupSum");

        }
        /// <summary>
        /// 获取中药处方的信息
        /// </summary>
        private void GetChineseRecipe(int mzRegId, string IsCharge)
        {
            int ChinDrugBagItemId = Convert.ToInt32(BLL.Common.Utils.GetSystemSetting("ChinDrugBagItemId"));
            _lstUspChineseRecipeDtl = new Model.ModelList<Model.uspOuRecipeDtlQry>();
            _lstChineseRecipe = _objOuRecipe.OuRecipe_SelectByMzRegId(mzRegId).Find("LsRepType", "2");
            bool hasRecipeDtl = false;
            foreach (Model.OuRecipeInfo infoRecipe in _lstChineseRecipe)
            {
                hasRecipeDtl = false;
                BLL.Finder<Model.uspOuRecipeDtlQry> _finder = new BLL.Finder<Model.uspOuRecipeDtlQry>();
                _finder.AddParameter("RecipeId", infoRecipe.ID);
                Model.ModelList<Model.uspOuRecipeDtlQry> lst = _finder.Find("uspOuRecipeDtl").Find("IsCharged", IsCharge);
                for (int j = 0; j < lst.Count; j++) //加载收费项目才能加载中药处方明细，也就是未收费的中药处方明细允许用户修改
                {
                    if (_lstUspOuInvoiceDtl.Find("RecipeItemId", lst[j].ID.ToString()).Count > 0)
                    {
                        _lstUspChineseRecipeDtl.Add(lst[j]);
                        hasRecipeDtl = true;
                    }
                }
                if (!hasRecipeDtl)  //如果中药处方已经收费（没有可以修改的明细），主表也不要
                    infoRecipe.F3 = "DEL";
                if (_infoOuInvoice.ID == 0)
                    AddOneCustFee(ChinDrugBagItemId, BLL.Common.Utils.GetBaseTableRowInfo<Model.BsItemInfo>("BsItem", ChinDrugBagItemId).PriceDiag, infoRecipe.HowMany, "中草药药袋费用", 0, string.Empty, false, 0);
            }
            _lstChineseRecipe.Remove("F3", "DEL");
            if (_lstUspChineseRecipeDtl == null || _lstUspChineseRecipeDtl.Count == 0)
                AddNewOuChineseRecipe();
        }

        /// <summary>
        /// 获取发票处方的信息
        /// </summary>
        private void GetOuRecipeDtl()
        {
            BLL.COuRecipeDtl objOuRecipeDtl = new BLL.COuRecipeDtl();
            Model.uspOuInvoiceDtlQry info;
            _lstChineseRecipeDtl = new Model.ModelList<Model.OuRecipeDtlInfo>();
            _lstCheckRecipeDtl = new Model.ModelList<Model.OuRecipeDtlInfo>();
            _lstWestRecipeDtl = new Model.ModelList<Model.OuRecipeDtlInfo>();
            for (int i = 0; i < _lstUspOuInvoiceDtl.Count; i++)
            {
                info = _lstUspOuInvoiceDtl[i];
                //if (info.LsRpType == 3)
                //    //_lstChineseRecipeDtl.Add(objOuRecipeDtl.GetByID(info.RecipeItemId));
                if (info.LsRpType == 1 || info.LsRpType == 2)
                    _lstWestRecipeDtl.Add(objOuRecipeDtl.GetByID(info.RecipeItemId));
                else if (info.LsRpType != 3)
                    _lstCheckRecipeDtl.Add(objOuRecipeDtl.GetByID(info.RecipeItemId));
            }
        }

        /// <summary>
        /// 显示发票类别
        /// </summary>
        private void ShowInInvoiceGroup()
        {
            if (this.usgInvItemType == null) return;
            this.usgInvItemType.DataSource = _lstUspOuInvoiceInvItemGoupSumQry;
        }
        /// <summary>
        /// 显示基础分类
        /// </summary>
        private void ShowFeetyGroup()
        {
            if (this.usgFeetyType == null) return;
            this.usgFeetyType.DataSource = _lstUspOuInvoiceFeetyGoupSumQry;
        }
        string _notReChargeDrugIssuedMzRegNo = string.Empty;
        Model.ModelList<Model.uspOuNotReChargeDrugIssuedQry> _lstNotReChargeExecuted;
        Model.ModelList<Model.uspOuNotReChargeDrugIssuedQry> _lstNotReChargeDrugIssued;
        private void FrmInBalance_Activated(object sender, EventArgs e)
        {
            BLL.Finder<Model.uspOuNotReChargeDrugIssuedQry> objNotReChargeDrugIssued = new BLL.Finder<Model.uspOuNotReChargeDrugIssuedQry>();
            objNotReChargeDrugIssued.AddParameter("CancelOperId", Model.Configuration.UserProfiles.UserID);
            _lstNotReChargeDrugIssued = objNotReChargeDrugIssued.Find("uspOuNotReChargeDrugIssued");
            if (_lstNotReChargeDrugIssued.Count > 0)
            {
                _lstNotReChargeDrugIssued.Sort("MzRegNo");
                _lstNotReChargeDrugIssued.Reverse();
                //if (MessageBox.Show(string.Format("您作废了一张已经发药的处方，病人流水号：{0}。是否现在补收？", _notReChargeDrugIssuedMzRegNo), "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                //{
                //    _notReChargeDrugIssuedMzRegNo = _lstNotReChargeDrugIssued[0].MzRegNo;
                //    this.hisOuHosInfo1.uoupCardNo.FindByHospID(_lstNotReChargeDrugIssued[0].ID);
                //    return;
                //}
                //else
                _notReChargeDrugIssuedMzRegNo = string.Empty;
            }

            objNotReChargeDrugIssued = new BLL.Finder<Model.uspOuNotReChargeDrugIssuedQry>();
            objNotReChargeDrugIssued.AddParameter("CancelOperId", Model.Configuration.UserProfiles.UserID);
            _lstNotReChargeExecuted = objNotReChargeDrugIssued.Find("uspOuNotReChargeExecuted");
            ////if (_lstNotReChargeExecuted.Count > 0)
            ////{
            ////    _lstNotReChargeExecuted.Sort("MzRegNo");
            ////    _lstNotReChargeExecuted.Reverse();
            ////    _notReChargeDrugIssuedMzRegNo = _lstNotReChargeExecuted[0].MzRegNo;
            ////    if (MessageBox.Show(string.Format("您作废了一张辅助功能科室已经执行的处方，病人流水号：{0}。是否现在补收？", _notReChargeDrugIssuedMzRegNo), "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            ////        this.hisOuHosInfo1.uoupCardNo.FindByHospID(_lstNotReChargeExecuted[0].ID);
            ////}
            if (Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsOtherLISInterface")))
            {
                objNotReChargeDrugIssued = new BLL.Finder<Model.uspOuNotReChargeDrugIssuedQry>();
                objNotReChargeDrugIssued.AddParameter("CancelOperId", Model.Configuration.UserProfiles.UserID);
                _lstNotReChargeExecuted = objNotReChargeDrugIssued.Find("uspOuNotOtherLISExecuted");
                if (_lstNotReChargeExecuted.Count > 0)
                {
                    _lstNotReChargeExecuted.Sort("MzRegNo");
                    _lstNotReChargeExecuted.Reverse();
                    _notReChargeDrugIssuedMzRegNo = _lstNotReChargeExecuted[0].MzRegNo;
                    if (MessageBox.Show(string.Format("您作废了一张检验科已经执行的处方，病人流水号：{0}。是否现在补收？", _notReChargeDrugIssuedMzRegNo), "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                        this.hisOuHosInfo1.uoupCardNo.FindByHospID(_lstNotReChargeExecuted[0].ID);
                }
            }
            if (System.Convert.ToBoolean(BLL.Common.Utils.GetSystemSetting("IsUseMiniCard")))
                this.hisOuHosInfo1.SetFocusControl(Model.EnumHisOuPatientSearchField.CardNo);
            else
            {
                //this.hisOuHosInfo1.SetFocusControl(Model.EnumHisOuPatientSearchField.MzRegNo);
                this.hisOuHosInfo1.SetFocusControl(Model.EnumHisOuPatientSearchField.CardNo);
            }
            //this.CurrentBalanceNo = _frmCurrInvo.GetInvoiceNoFromConfigFile();
        }
        /// <summary>
        /// 输入数量并按Enter后新增收费明细
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void utxtTotality_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {

            this.uicItemId.txtBsItem.Enabled = true;
            if (e != null && e.Modifiers == Keys.Control && e.KeyCode == Keys.D)
            {
                int Totality = Tools.Utils.InputInt(string.Format("请输入药品[{0}]大单位[{1}]的数量！", _infoUspOuInvoiceDtl.Name, this.uicItemId.SelectBsItemInfo.UnitKcName), "系统提示", "1");
                utxtTotality.EditValue = BLL.HuRmCommon.GetUnitRatio(this.uicItemId.ItemID, this.uicItemId.SelectBsItemInfo.UnitKc, this.uicItemId.SelectBsItemInfo.UnitDiagId) * Totality;
                utxtTotality_KeyDown(null, null);
            }
            if (e != null && e.KeyCode == Keys.Left)
            {
                this.uicItemId.Focus();
                this.uicItemId.txtBsItem.SelectAll();
                return;
            }
            if (e != null && e.KeyCode == Keys.Enter || sender == null && e == null)
            {
                if (this.ubsExecLocId.ID == 0) return;
                //if(_infoUspOuInvoiceDtl
                if (!Utilities.Information.IsNumeric(utxtTotality.EditValue))
                {
                    this.InformationInMainForm = "请输入阿拉伯数字！";
                    return;
                }
                if (_infoUspOuInvoiceDtl == null) _infoUspOuInvoiceDtl = new Model.uspOuInvoiceDtlQry();
                if (_infoUspOuInvoiceDtl.Name == string.Empty)
                {
                    this.InformationInMainForm = "请先输入项目！";
                    this.uicItemId.Focus();
                    return;
                }
                else if (Convert.ToDouble(utxtTotality.EditValue) <= 0)
                {
                    this.InformationInMainForm = "数量不能小于或等于０！";
                    CreateInfoToolTip("数量不能小于或等于０", this.utxtTotality);
                    this.utxtTotality.Focus();
                    return;
                }
                _infoUspOuInvoiceDtl.Totality = Convert.ToDouble(utxtTotality.EditValue);
                _infoUspOuInvoiceDtl.ExecLocId = this.ubsExecLocId.ID;

                BLL.Common.RecipeHelper<Model.uspOuInvoiceDtlQry> _recipeHelper = new BLL.Common.RecipeHelper<Model.uspOuInvoiceDtlQry>();
                if (_infoUspOuInvoiceDtl.F3 == "RoomDrug" && !_recipeHelper.CheckInStockNumIsEnough(_infoUspOuInvoiceDtl.Totality, 0, (int)Model.EnumRoomInOut.ForOutPatient, _infoUspOuInvoiceDtl.ItemId, _infoUspOuInvoiceDtl.Name))
                {
                    this.InformationInMainForm = "药房库存不足够！";
                    CreateInfoToolTip("数量不能大于药房库存", this.utxtTotality);
                    this.utxtTotality.Focus();
                    return;
                }
                if (objBsItemAttach.BsItemAttach_SelectByItemId1(_infoUspOuInvoiceDtl.ItemId).Count > 0)
                {
                    _infoUspOuInvoiceDtl.IsDoctorInput = true;
                    AddNewInvoiceDtl();
                    GetItemAttach(true);
                }
                else
                    AddNewInvoiceDtl();

                this.uicItemId.txtBsItem.Enabled = true;
                this.uicItemId.Focus();
            }
        }
        void utxtTotality_GotFocus(object sender, System.EventArgs e)
        {
            if (this.uicItemId.txtBsItem.Text != string.Empty)
            {
                this.uicItemId.txtBsItem.Enabled = false;
            }
        }

        private void AddNewItemData()
        {
            if (BLL.Common.Utils.CheckSettingContain("OuChargeInputPriceItemIds", _infoUspOuInvoiceDtl.ItemId, false))
            {
                _infoUspOuInvoiceDtl.Price = _infoUspOuInvoiceDtl.Totality;
                _infoUspOuInvoiceDtl.Totality = 1;
            }
            _infoUspOuInvoiceDtl.Amount = BLL.Common.Utils.Round(_infoUspOuInvoiceDtl.Totality * _infoUspOuInvoiceDtl.Price, 2);
            _infoUspOuInvoiceDtl.DiscSelf = 1;
            _infoUspOuInvoiceDtl.F1 = System.DateTime.Now.TimeOfDay.ToString();
            if (_isNew)  //如果是新增项目，将其加入网格
                _lstUspOuInvoiceDtl.Add(_infoUspOuInvoiceDtl);
        }
        /// <summary>
        /// 新增收费明细
        /// </summary>
        private void AddNewInvoiceDtl()
        {
            AddNewItemData();
            this.devGrid1.advBandedGridViewMain.RefreshData();
            this.devGrid1.advBandedGridViewMain.MoveLast();
            GetDrugSendMemo(_infoUspOuInvoiceDtl);
            CalcuateAmountTally();
            InitData();
            _infoUspOuInvoiceDtl = new Model.uspOuInvoiceDtlQry();
            BindBsItemData(_infoUspOuInvoiceDtl, false);
            this.uicItemId.txtBsItem.Text = string.Empty;
            this.utxtTotality.Text = string.Empty;
            this.uicItemId.ItemID = 0;
        }
        private void Clear()
        {
            this.utxtName.EditValue = string.Empty;
            this.utxtPrice.EditValue = string.Empty;
            this.utxtSpec.EditValue = string.Empty;
            this.utxtTotality.EditValue = string.Empty;
            this.utxtUnitDiagName.EditValue = string.Empty;
            this.uicItemId.txtBsItem.EditValue = string.Empty;

        }
        /// <summary>
        /// 获取病人今天已结算的项目
        /// </summary>
        /// <param name="patId"></param>
        private void GetPatientOuInvoiceDtlTodayPayed(int patId)
        {
            BLL.Finder<Model.uspOuInvoiceDtlPatientTodayQry> finder = new BLL.Finder<Model.uspOuInvoiceDtlPatientTodayQry>();
            finder.AddParameter("PatId", patId);

            _lstUspOuInvoiceDtlPatientToday = finder.Find("uspOuInvoiceDtlPatientToday");
        }
        /// <summary>
        /// 计算各种费用的总和
        /// </summary>
        /// <param name="DiscDiag">自付比例</param>
        /// <param name="discSelf">优惠比例</param>
        /// <param name="rowHandle">记录项</param>
        private void ReCalculateDetailAmount(double discDiag, double discSelf, Model.uspOuInvoiceDtlQry info)
        {
            info.Amount = BLL.Common.Utils.Round(info.Totality * info.Price, 2);
            info.AmountFact = info.AmountSelf = BLL.Common.Utils.Round(info.Amount * discDiag, 2);
            info.AmountTally = info.Amount - info.AmountFact;
            info.AmountPay = info.AmountFact * discSelf;
        }

        private bool CancelYBUpSeqNo()
        {
            infoOuHos = objOuHos.GetByID(this.hisOuHosInfo1.Value.ID);

            infoOuHos.F5 = Microsoft.VisualBasic.Interaction.InputBox("请输入要退费的流水号", "流水号", string.Empty, 200, 200);

            objOuHos.Modify(infoOuHos, null);

            try
            {
                if (SetYbInterface(false))//初始化医保接口
                {
                    string returnString;
                    if (MessageBox.Show(string.Format("该发票是进行过医保联网结算的，是否继续退费？"), "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                        return false;
                    returnString = ybInterface.UpLoadSaver();
                    if (!returnString.Contains("成功"))
                    {
                        MessageBox.Show(string.Format("医保结算取消失败：{0}", returnString), "提示");
                        return false;
                    }
                }
                else return true;
            }
            catch (Exception e)
            {
                infoOuHos = objOuHos.GetByID(this.hisOuHosInfo1.Value.ID);
                MessageBox.Show(string.Format("医保结算取消失败，发生异常：{0}", e.Message), "提示");
                return false;
            }
            infoOuHos = objOuHos.GetByID(this.hisOuHosInfo1.Value.ID);

            return true;
        }

        /// <summary>
        /// 初始化医保接口
        /// </summary>
        /// <param name="IsBalance"></param>
        /// <returns></returns>
        private bool SetYbInterface(bool IsBalance)
        {
            if (this.hisOuHosInfo1.Value == null) return false;
            ybInterface = YBInterface.YbFactory.Create(this.hisOuHosInfo1.Value.TallyTypeId, 1);//1门诊2住院
            if (ybInterface == null) return false;

            BLL.CBsDoctor objBsDoctor = new BLL.CBsDoctor();
            BLL.CBsTallyType objBsTallyType = new BLL.CBsTallyType();
            BLL.CYbPatSeq objYbPatSeq = new BLL.CYbPatSeq();

            ybInterface.RegNo = string.Empty;
            ybInterface.InfoBsPatient = _objBsPatient.GetByID(this.hisOuHosInfo1.Value.PatId);
            ybInterface.InfoTallyType = objBsTallyType.GetByID(this.hisOuHosInfo1.Value.TallyTypeId);
            ybInterface.InfoPatType = objPatType.GetByID(this.hisOuHosInfo1.Value.PatTypeId);
            ybInterface.HosInfoInfo = objOuHosInfo.GetByID(this.hisOuHosInfo1.Value.ID);
            _lstUspOuInvoiceDtl.Remove("ItemId", IntegralItemId);
            ybInterface.LstUspOuInvoiceDtl = _lstUspOuInvoiceDtl;
            ybInterface.ReturnF3 = null;
            _infoOuInvoice.OperTime = BLL.Common.DateTimeHandler.GetServerDateTime();
            ybInterface.InfoOuInvoice = _infoOuInvoice;
            ybInterface.InfoOuHosInfo = this.hisOuHosInfo1.Value;


            BLL.COuClincDiag objOuClincDiag = new BLL.COuClincDiag();
            _lstUspOuInvoiceDtl.Remove("ItemId", IntegralItemId);
            BLL.COuHosInfo objOuHos = new BLL.COuHosInfo();

            Model.ModelList<Model.OuClincDiagInfo> lstOuClincDiag = objOuClincDiag.OuClincDiag_SelectByMzRegId(hisOuHosInfo1.Value.ID);

            if (lstOuClincDiag != null && lstOuClincDiag.Count > 0 && lstOuClincDiag[0].IcdId > 0)
            {
                BLL.CBsIllness objIllness = new BLL.CBsIllness();

                ybInterface.IllCode = objIllness.GetByID(lstOuClincDiag[0].IcdId).Code;
                ybInterface.IllName = lstOuClincDiag[0].IllDesc;
            }

            BLL.COuHosInfo objhosinfo = new BLL.COuHosInfo();
            BLL.CBsPatTypeYbIll objBsPatTypeYbIll = new BLL.CBsPatTypeYbIll();

            //Model.ModelList<Model.YbPatSeqInfo> infoYbPatSeq = objYbPatSeq.GetDynamic("MZREGID="+this.hisOuHosInfo1.Value.ID+ " and " + " F4="+_infoOuInvoice.ID,"ID");
            Model.ModelList<Model.YbPatSeqInfo> infoYbPatSeq = objYbPatSeq.GetDynamic(string.Format("MZREGID={0} and F4='{1}'", this.hisOuHosInfo1.Value.ID, _infoOuInvoice.ID), "ID");
            if (infoYbPatSeq != null && infoYbPatSeq.Count > 0)
            {
                ybInterface.YbPatSeqInfo = infoYbPatSeq[0];
                ybInterface.RegNo = infoYbPatSeq[0].YbSeq;
                if (infoYbPatSeq[0].F2.Trim() != string.Empty && Utilities.Information.IsNumeric(infoYbPatSeq[0].F2.Trim()))
                    ybInterface.InfoBsPatTypeYbIll = objBsPatTypeYbIll.GetByID(int.Parse(infoYbPatSeq[0].F2.Trim()));
            }
            else if (this._infoOuInvoice.F4.Trim() != string.Empty)
            {
                ybInterface.RegNo = _infoOuInvoice.F4.Trim();
            }
            else
            {
                ybInterface.RegNo = objOuHosInfo.GetByID(this.hisOuHosInfo1.Value.ID).F5.Trim();
            }
            Model.ModelList<Model.BsPatTypeYbIllInfo> lstBsPatTypeYbIll;
            try
            {
                lstBsPatTypeYbIll = objBsPatTypeYbIll.GetDynamic(string.Format("TallyGroupId={0} and (F2 ='1' or F2 = '3')", this.hisOuHosInfo1.Value.TallyGroupId), "ID");
            }
            catch (Exception e)
            {

                MessageBox.Show(this, e.Message.ToString());
                return false;
            }

            if (IsBalance && lstBsPatTypeYbIll != null && lstBsPatTypeYbIll.Count > 0 && InfoBsPatTypeYbIll.ID == 0)
            {
                FrmShowControl showPatTypeYb = new FrmShowControl("BsPatTypeYbIllOu");//BsPatTypeYbIll
                showPatTypeYb.ubsObject.Filter = string.Format("TallyGroupId={0}", this.hisOuHosInfo1.Value.TallyGroupId);

                showPatTypeYb.Text = "请选择特殊病种";
                showPatTypeYb.ShowDialog();
                if (!showPatTypeYb.IsOk)
                {
                    return false;
                }
                if (showPatTypeYb.IsOk && showPatTypeYb.Value > 0)
                {
                    Model.BsPatTypeYbIllInfo infoPatTypeYbIll = objBsPatTypeYbIll.GetByID(showPatTypeYb.Value);
                    InfoBsPatTypeYbIll = ybInterface.InfoBsPatTypeYbIll = infoPatTypeYbIll;
                }
            }
            else if (InfoBsPatTypeYbIll != null && InfoBsPatTypeYbIll.ID > 0 && ybInterface.InfoBsPatTypeYbIll == null)
            {
                ybInterface.InfoBsPatTypeYbIll = InfoBsPatTypeYbIll;
            }

            return true;
        }

        public string AutoRegOuHosInfo(string mzRegNo, int locationId, int doctorId)
        {
            int mzRegId = _objOuHosInfo.OuHosInfo_SelectByMzRegNo(mzRegNo)[0].ID;
            this.hisOuHosInfo1.uoupCardNo.FindByHospID(mzRegId);
            if (this.hisOuHosInfo1.Value.PatTypeId != Convert.ToInt32(BLL.Common.Utils.GetSystemSetting("AutoRegOuHosInfoPatTypeId")))
                return "结算失败，病人不是自助挂号的病人！";
            OpenRecord();

            if (BLL.InsertAccount.CheckPatCardFee(this.hisOuHosInfo1.Value.PatId, _infoOuInvoice.Beprice))
                return "结算失败，病人余额不足！";
            Toolbar_Click("Balance");
            return "成功";
        }



        Model.ListView<Model.uspOuExecutePrintQry> _lstOuExecute = new Model.ListView<Model.uspOuExecutePrintQry>();
        string _newItemId = string.Empty;
        /// <summary>
        /// 重新生成门诊护士执行单
        /// </summary>
        private void ReGenOuExecute()
        {
            if (hisOuHosInfo1.Value == null) return;
            BLL.COuExecute objOuexecute = new BLL.COuExecute();
            BLL.COuExecuteDtl objOuexecuteDtl = new BLL.COuExecuteDtl();
            BLL.CBsUnitRatio objRatio = new BLL.CBsUnitRatio();

            Model.ModelList<Model.uspOuRecipeForOuexecuteQry> lstOuRecipeForOuexecute = new Model.ModelList<Model.uspOuRecipeForOuexecuteQry>();
            BLL.Finder<Model.uspOuRecipeForOuexecuteQry> bllFinder = new BLL.Finder<Model.uspOuRecipeForOuexecuteQry>();
            bllFinder.AddParameter("MzRegID", hisOuHosInfo1.Value.ID);
            lstOuRecipeForOuexecute = bllFinder.Find("uspOuRecipeForOuexecute");
            if (lstOuRecipeForOuexecute.Count == 0) return;

            DateTime dt = BLL.Common.DateTimeHandler.GetServerDateTime();
            DAL.SqlUtil db = new DAL.SqlUtil();
            System.Data.Common.DbTransaction trn = db.GetSqlTransaction();
            try
            {
                //Model.ModelList<Model.uspOuExecutePrintQry> lstDelete = _lstOuExecute.SourceList.Find("IsExecuted", "False");
                //for (int i = 0; i < lstDelete.Count; i++)
                //{
                //    objOuexecuteDtl.OuExecuteDtl_DeleteByExecuteId(lstDelete[i].ExecuteId, trn);
                //    objOuexecute.Remove(lstDelete[i].ExecuteId, trn);
                //}

                ///分组
                //生成顺序：组，天，频率
                Model.ModelList<Model.ComputeSummary> lstGroupNum = lstOuRecipeForOuexecute.GroupBy("GroupNum", "GroupNum", Model.ComputeType.Sum);
                foreach (Model.ComputeSummary infoGroupNumDtl in lstGroupNum)
                {
                    Model.ModelList<Model.uspOuRecipeForOuexecuteQry> lst = lstOuRecipeForOuexecute.Find("GroupNum", infoGroupNumDtl.GroupBy);
                    int days = Convert.ToInt32(lst.GetDoubleMax("Days"));
                    BLL.CBsFrequency objBsFrequency = new BLL.CBsFrequency();
                    int frequencyDays = objBsFrequency.GetByID(lst[0].FrequencyId).Days;
                    for (int nDay = 0; nDay < days; nDay++)
                    {
                        ///按频率次数插入OuExecute  
                        BLL.CBsFrequencyTime objBsFrequencyTime = new BLL.CBsFrequencyTime();
                        int count = objBsFrequencyTime.BsFrequencyTime_SelectByFrequencyId(lst[0].FrequencyId).Count;
                        for (int nTime = 0; nTime < count; nTime++)
                        {
                            Model.OuExecuteInfo infoOuExecute = new Model.OuExecuteInfo();
                            if (frequencyDays > 1 && nDay > 0)
                            {
                                int mDays = 0;
                                mDays = nDay * frequencyDays;
                                infoOuExecute.NDay = mDays + 1;
                            }
                            else
                            {
                                infoOuExecute.NDay = nDay + 1;
                            }
                            Model.ModelList<Model.uspOuExecutePrintQry> lstGroupDay = _lstOuExecute.SourceList.Find("GroupNum", infoGroupNumDtl.GroupBy).Find("NDay", Convert.ToString(infoOuExecute.NDay)).Find("NTime", Convert.ToString(nTime + 1));
                            if (lstGroupDay.Count > 0)   //如果已经生成，则不插入新记录，只是同步处方信息和执行标志
                            {
                                infoOuExecute.ID = lstGroupDay[0].ExecuteId;
                            }
                            infoOuExecute.MzRegId = hisOuHosInfo1.Value.ID;
                            infoOuExecute.GroupNum = Convert.ToInt32(infoGroupNumDtl.GroupBy);
                            //if (frequencyDays > 1 && nDay > 0)
                            //{
                            //    int mDays = 0;
                            //    mDays = nDay * frequencyDays;
                            //    infoOuExecute.NDay = mDays + 1;
                            //}
                            //else
                            //{
                            //    infoOuExecute.NDay = nDay + 1;
                            //}
                            infoOuExecute.NTime = nTime + 1;
                            infoOuExecute.F1 = dt.AddDays(nDay).Date.ToString();
                            infoOuExecute.RecipeId = lst[0].RecipeId;
                            infoOuExecute.UsageId = lst[0].UsageId;
                            infoOuExecute.IsExecuted = false;
                            infoOuExecute.OperId = Model.Configuration.UserProfiles.UserID;
                            infoOuExecute.OperTime = dt;

                            if (lst[0].IsMzDrop || lst[0].IsMzReject)    //根据用法生成执行单
                                infoOuExecute.LsRepType = (int)Model.EnumRecipePrintType.WesternMedicine;
                            else if (lst[0].IsMzCure)
                                infoOuExecute.LsRepType = (int)Model.EnumRecipePrintType.Checkup;

                            Model.ModelList<Model.OuExecuteDtlInfo> lstOuExecuteDtl = new Model.ModelList<Model.OuExecuteDtlInfo>();
                            Model.BsItemInfo infoItem = new Model.BsItemInfo();
                            foreach (Model.uspOuRecipeForOuexecuteQry info in lst)
                            {
                                Model.ModelList<Model.BsUnitRatioInfo> lstUnitRatio = objRatio.BsUnitRatio_SelectByItemId(info.ItemId).Find("F1", "1");
                                Model.ModelList<Model.uspOuExecutePrintQry> lstHasGen = _lstOuExecute.SourceList.Find("GroupNum", infoGroupNumDtl.GroupBy).Find("ItemId", info.ItemId.ToString());
                                if (lstHasGen.Count == 0 && info.Days > nDay)
                                {
                                    Model.OuExecuteDtlInfo infoOuExecuteDtl = new Model.OuExecuteDtlInfo();
                                    infoOuExecuteDtl.Days = info.Days;
                                    infoOuExecuteDtl.FrequencyId = info.FrequencyId;
                                    infoOuExecuteDtl.ItemId = info.ItemId;
                                    infoOuExecuteDtl.ListNum = info.ListNum;
                                    infoOuExecuteDtl.OperId = Model.Configuration.UserProfiles.UserID;
                                    infoOuExecuteDtl.IsExecuted = false;
                                    //infoOuExecuteDtl.ExecutedTime = lstGroupDay[0].ExecutedTime;
                                    infoOuExecuteDtl.OperId = Model.Configuration.UserProfiles.UserID;
                                    infoOuExecuteDtl.OperTime = dt;
                                    if (lstUnitRatio.Count > 0 && lstUnitRatio[0].UnitId1 == info.UnitTakeId)
                                    {
                                        infoOuExecuteDtl.Dosage = info.Dosage * lstUnitRatio[0].DrugRatio;
                                        infoOuExecuteDtl.UnitId = lstUnitRatio[0].UnitId2;
                                    }
                                    else
                                    {
                                        infoOuExecuteDtl.Dosage = info.Dosage;
                                        infoOuExecuteDtl.UnitId = info.UnitTakeId;
                                    }
                                    infoOuExecuteDtl.UsageId = info.UsageId;
                                    infoOuExecuteDtl.F4 = info.Memo;
                                    infoItem = BLL.Common.Utils.GetBaseTableRowInfo<Model.BsItemInfo>("BsItem", info.ItemId);
                                    double limitTotalZy = infoItem.LimitTotalZy;
                                    if (limitTotalZy > 0)
                                        infoOuExecuteDtl.Totality = limitTotalZy / XYHIS.Common.Helper.GetUnitRatio(info.ItemId, infoItem.UnitDiagId, info.UnitTakeId) * System.Math.Ceiling(info.Dosage / limitTotalZy);
                                    else
                                        infoOuExecuteDtl.Totality = info.Dosage / XYHIS.Common.Helper.GetUnitRatio(info.ItemId, infoItem.UnitDiagId, info.UnitTakeId);
                                    infoOuExecuteDtl.UnitDiagId = infoItem.UnitDiagId;
                                    if (info.GroupNum == 0)   //如果是治疗套餐则写套餐名称，不写RecipeDtlId
                                        infoOuExecuteDtl.F2 = info.UsageName.Trim();
                                    else
                                        infoOuExecuteDtl.RecipeDtlId = info.ID;
                                    infoOuExecuteDtl.F3 = info.PrepareTime.ToString();

                                    lstOuExecuteDtl.Add(infoOuExecuteDtl);
                                }
                                else if (lstHasGen.Count == 1)//当执行有记录的时候，修改备注
                                {
                                    Model.OuExecuteDtlInfo infoOuExecuteDtlOne = new Model.OuExecuteDtlInfo();
                                    infoOuExecuteDtlOne = objOuexecuteDtl.GetByID(lstHasGen[0].ID);
                                    infoOuExecuteDtlOne.F4 = info.Memo;
                                    lstOuExecuteDtl.Add(infoOuExecuteDtlOne);
                                }
                            }
                            objOuexecute.SaveChild<Model.OuExecuteDtlInfo, BLL.COuExecuteDtl>(infoOuExecute, lstOuExecuteDtl, "ExecuteId", trn);
                        }
                    }
                }
                trn.Commit();
            }
            catch (Exception ex)
            {
                trn.Rollback();
                trn.Dispose();
                new Tools.FormErrorMessage(ex.Message, ex.InnerException == null ? ex.Message : string.Format("{0}\r\n{1}", ex.InnerException.Message, ex.Message));

            }
            SetDataSource();
        }

        //生成护士执行单
        private void GenOuExecute(Model.uspHisOuPatientQry hisOuPatient)
        {
            BLL.CBsUsage objBsUsage = new BLL.CBsUsage();
            BLL.COuRecipe objOuRecipe = new BLL.COuRecipe();
            Model.ModelList<Model.OuRecipeInfo> lstOuRecipe = objOuRecipe.OuRecipe_SelectByMzRegId(hisOuPatient.ID);
            Model.ModelList<Model.OuRecipeDtlInfo> lstRecipeDtl = objOuRecipe.GetMutiChild<Model.OuRecipeDtlInfo, BLL.COuRecipeDtl>(lstOuRecipe.Find("LsRepType", "1").ConvertToBase(), "RecipeId");
            //自备药
            Model.ModelList<Model.OuRecipeDtlInfo> lstSelf = lstRecipeDtl.Find("IsOtherFee", "True").Find("IsCancel", "False");
            lstRecipeDtl = lstRecipeDtl.Find("IsCharged", "True").Find("IsCancel", "False");
            lstRecipeDtl.AddRange(lstSelf);
            _newItemId = string.Empty;

            SetDataSource();
            foreach (Model.OuRecipeDtlInfo infoRecipeDtl in lstRecipeDtl)
            {
                Model.BsUsageInfo infoUsage = objBsUsage.GetByID(infoRecipeDtl.UsageId);
                if (!infoUsage.IsMzCure && !infoUsage.IsMzDrop && !infoUsage.IsMzReject) continue;

                if (_lstOuExecute.SourceList.Find("RecipeDtlId", infoRecipeDtl.ID.ToString()).Count == 0)
                    _newItemId += string.Format("{0},", infoRecipeDtl.ItemId);
                if (_lstOuExecute.SourceList.Find("ItemId", infoRecipeDtl.ItemId.ToString()).Count == 0 && !_newItemId.Contains(infoRecipeDtl.ItemId.ToString()))
                    _newItemId += string.Format("{0},", infoRecipeDtl.ItemId);
            }
            //if (_lstOuExecute.Count == 0 || (_newItemId != string.Empty && MessageBox.Show("医生已经修改了处方，是否重新生成执行单?", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK))
            //{
            ReGenOuExecute();
            //}
            //this.OpenRecord();
            //Tools.Utils.TraceFunctionOperate(292);
        }

        private void SetDataSource()
        {
            if (hisOuHosInfo1.Value == null) return;
            BLL.Finder<Model.uspOuExecutePrintQry> bllFinder = new BLL.Finder<Model.uspOuExecutePrintQry>();
            bllFinder.AddParameter("MzRegID", hisOuHosInfo1.Value.ID);
            _lstOuExecute = bllFinder.Find("uspOuExecutePrint").DefaultView;
            _lstOuExecute.Sort("GroupNumNDay,DoctorInputTime");
            _lstOuExecute.Fill("IsSelect", true.ToString());
            foreach (Model.uspOuExecutePrintQry infoOuExecute in _lstOuExecute)
            {
                if (XYHIS.Common.Helper.IsSkin(infoOuExecute.Memo, 1, infoOuExecute.UsageId))
                    infoOuExecute.IsSkin = true;
                else
                    infoOuExecute.IsSkin = false;
                if (_newItemId.Contains(infoOuExecute.ItemId.ToString()))
                    infoOuExecute.IsExecuted = false;
            }
            _newItemId = string.Empty;
        }
    }
}

