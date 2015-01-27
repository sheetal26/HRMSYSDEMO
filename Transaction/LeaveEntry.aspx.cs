using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Globalization;
using HRMSystem;

public partial class Transaction_LeaveEntry : System.Web.UI.Page
{
   
    BAL BLayer = new BAL();
    StringBuilder StrSql;
    SqlFunction SqlFunc = new SqlFunction();
    SqlCommand Cmd = new SqlCommand();
    DataTable dtTemp;
    DataSet dtSet = new DataSet();
    Security Sec = new Security();

    IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
    HRMSysLinQDataContext HRMLinQ = new HRMSysLinQDataContext();
    
    protected void Page_PreInit(object sender, EventArgs e)
    {
        try
        {
            if (Session["MasterFile"] != null)
            {
                MasterPageFile = Session["MasterFile"].ToString();
            }
            else
            {
                Page.MasterPageFile = "~/MasterHomeBlank.master";
            }
        }
        catch//(Exception ex)
        {
            Response.Redirect("~/ErrorPage.aspx");
        }
    }

    protected void MyMenu_MenuItemClick(object sender, MenuEventArgs e)
    {   
        try
        {
            int index = int.Parse(e.Item.Value);

            MyMultiView.ActiveViewIndex = Int32.Parse(e.Item.Value);

            MyMenu.Items[0].ImageUrl = "~/Images/ViewDisable.jpg";
            MyMenu.Items[1].ImageUrl = "~/images/NewOrEditDisable.jpg";

            switch (index)
            {
                case 0:
                    MyMenu.Items[0].ImageUrl = "~/Images/ViewEnable.jpg";
                    break;
                case 1:
                    MyMenu.Items[1].ImageUrl = "~/Images/NewOrEditEnable.jpg";
                    break;
            }

        }
        catch //(Exception ex)
        {
            Response.Redirect("~/ErrorPage.aspx");
        }

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Session["LoginUserId"] == null)
                {
                    Response.Redirect("~/Default.aspx");
                    return;
                }
                else
                {
                    HidFldUID.Value = Session["LoginUserId"].ToString();
                    ViewState["LoginUserGroup"] = Session["LoginUserGrp"].ToString();
                    ViewState["LoginId"] = Session["LoginId"].ToString();
                }

                if (ViewState["LoginUserGroup"].ToString() == "EMP")
                {
                    BtnRow.Visible = false; //Save,New,Cancle,Delete Button Panel
                    //EmpSearchRow.Visible = false; //Row For View Data Base On Employee Selection
                }
                else
                {
                    //For Only Admin Level User
                    BtnRow.Visible = true;
                    //EmpSearchRow.Visible = true;
                }

                ClearAll();

               // FillGrid();

                MyMenu.Items[0].Selected = true;
                MyMenu.Items[0].ImageUrl = "~/Images/ViewEnable.jpg";
                MyMultiView.ActiveViewIndex = 0;
            }
        }
        catch //(Exception ex)
        {
            Response.Redirect("~/ErrorPage.aspx");
        }
    }

    protected void FillGrid()
    {
        try
        {
            StrSql = new StringBuilder();
            StrSql.Length = 0;

            StrSql.AppendLine("Select l.Id, e.EmpName,Convert(VarChar(10),l.Application_Date,103) As Application_Date");
            StrSql.AppendLine(",Convert(VarChar(10),l.FromDate,103) As FromDate");
            StrSql.AppendLine(",Convert(VarChar(10),l.ToDate,103) As ToDate");
            StrSql.AppendLine(",l.TotalDays");
            StrSql.AppendLine("From Leave_Application l");
            StrSql.AppendLine("Left Join Emp_Mast E On l.EmpId=E.Id");
            StrSql.AppendLine("Where 1=1 ");
            if (ViewState["LoginUserGroup"].ToString() == "EMP")
            {
                StrSql.AppendLine("And l.EmpId=" + int.Parse(ViewState["LoginId"].ToString()));
            }

            dtTemp = new DataTable();
            dtTemp = SqlFunc.ExecuteDataTable(StrSql.ToString());

            GridLeave.DataSource = dtTemp;
            GridLeave.DataBind();
        }
        catch //(Exception ex)
        {
            Response.Redirect("~/ErrorPage.aspx");
        }
    }

    protected void ClearAll()
    {
        try
        {
            int IntEmpId = 0;
            if (ViewState["LoginUserGroup"].ToString() == "EMP")
            {
                IntEmpId = int.Parse(ViewState["LoginId"].ToString());
                ddlEmployee.Enabled = false;              
            }
            else
            {
                ddlEmployee.Enabled = true;                
            }
            ddlEmployee.Items.Clear();
            ddlEmployee.DataSource = BLayer.FillEmp(IntEmpId, ""); //BLayer.FillEmp("");
            ddlEmployee.DataValueField = "EmpId";
            ddlEmployee.DataTextField = "EmpName";
            ddlEmployee.DataBind();
            if (IntEmpId == 0)
            {
                ddlEmployee.Items.Insert(0, new ListItem("--Select Employee--", ""));
            }

            DDLLeaveType.SelectedIndex = 0;
            DDLStatus.SelectedIndex = 0;

            LblMsg.Text = "";

            HidFldId.Value = "";

            TxtDate.Text = "";
            TxtFromDate.Text = "";
            TxtToDate.Text = "";
            TxtTotDays.Text = "";
            TxtReason.Text = "";
            TxtRemark.Text = "";
        }
        catch //(Exception ex)
        {
            Response.Redirect("~/ErrorPage.aspx");
        }
    }

    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlEmployee.SelectedValue == "")
            {
                // ddlEmployee.Focus();
                return;
            }

            if (TxtDate.Text.Length == 0)
            {
                //TxtDate.Focus();
                return;
            }

            if (TxtFromDate.Text.Length == 0)
            {
                // TxtFromDate.Focus();
                return;
            }

            if (TxtToDate.Text.Length == 0)
            {
                //TxtToDate.Focus();
                return;
            }

            if (TxtReason.Text.Length == 0)
            {
                //TxtReason.Focus();
                return;
            }

            if (HidFldId.Value.Length == 0)
                {
                Leave_Application LI = new Leave_Application();

                LI.EmpId = int.Parse(ddlEmployee.SelectedValue);

                LI.Application_Date = ValueConvert.ToDate(TxtDate.Text.ToString());
                LI.FromDate = ValueConvert.ToDate(TxtFromDate.Text.ToString());
                LI.ToDate =ValueConvert.ToDate(TxtToDate.Text.ToString());

                LI.TotalDays = int.Parse(TxtTotDays.Text);
                LI.LeaveType = DDLLeaveType.SelectedValue;
                LI.Reason = TxtReason.Text;
                LI.LeaveStatus = DDLStatus.SelectedValue;
                LI.Remark = TxtRemark.Text;
                LI.UPDFLAG = 0;
                LI.Entry_Date = DateTime.Today.Date;
                LI.Entry_Time = DateTime.Parse(DateTime.Now.TimeOfDay.ToString());
                LI.Entry_UID = HidFldUID.Value.ToString();

                HRMLinQ.Leave_Applications.InsertOnSubmit(LI);
                HRMLinQ.SubmitChanges();

                LblMsg.Text = "Entry insert successfully...";
            }
            else
            {              
                int IntId = int.Parse(HidFldId.Value.ToString());

                //Get Single record which need to update
                Leave_Application LI = HRMLinQ.Leave_Applications.Single(Leave => Leave.Id == IntId);

                LI.EmpId = int.Parse(ddlEmployee.SelectedValue);                               

                LI.Application_Date = ValueConvert.ToDate(TxtDate.Text.ToString());
                LI.FromDate = ValueConvert.ToDate(TxtFromDate.Text.ToString());
                LI.ToDate = ValueConvert.ToDate(TxtToDate.Text.ToString());
                                
                LI.TotalDays = int.Parse(TxtTotDays.Text);
                LI.LeaveType = DDLLeaveType.SelectedValue;
                LI.Reason = TxtReason.Text;
                LI.LeaveStatus = DDLStatus.SelectedValue;
                LI.Remark = TxtRemark.Text;

                int updflg = int.Parse(LI.UPDFLAG.ToString()) + 1;
                LI.UPDFLAG = byte.Parse(updflg.ToString());
                LI.MEntry_Date = DateTime.Today.Date;
                LI.MEntry_Time = DateTime.Parse(DateTime.Today.TimeOfDay.ToString());
                LI.MEntry_UID = HidFldUID.Value.ToString();

                HRMLinQ.SubmitChanges();

                LblMsg.Text = "Entry update successfully...";
            }

            FillGrid();
            ClearAll();

            MyMenu.Items[0].Selected = true;
            MyMenu.Items[0].ImageUrl = "~/Images/ViewEnable.jpg";
            MyMenu.Items[1].ImageUrl = "~/Images/NewOrEditDisable.jpg";
            MyMultiView.ActiveViewIndex = 0;
        }
        catch //(Exception ex)
        {
            Response.Redirect("~/ErrorPage.aspx");
        }
    }
    protected void BtnNew_Click(object sender, EventArgs e)
    {
        try
        {
            ClearAll();
            ddlEmployee.Focus();
        }
        catch //(Exception ex)
        {
            Response.Redirect("~/ErrorPage.aspx");
        }
    }
    protected void BtnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ClearAll();
            // Move to View Tab            
            MyMenu.Items[0].Selected = true;
            MyMenu.Items[0].ImageUrl = "~/Images/ViewEnable.jpg";
            MyMultiView.ActiveViewIndex = 0;
            MyMenu.Items[1].ImageUrl = "~/Images/NewOrEditDisable.jpg";
        }
        catch //(Exception ex)
        {
            Response.Redirect("~/ErrorPage.aspx");
        }
    }
    protected void BtnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (HidFldId.Value.ToString() != null)
            {
                string confirmValue = "";
                confirmValue = Request.Form["confirm_value"];
                if (confirmValue == "Yes")
                {
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked YES !')", true);

                    var leavDet = 
                        (from l in HRMLinQ.Leave_Applications
                        where l.Id == int.Parse(HidFldId.Value)
                        select l).First();

                    if(leavDet != null)
                    {
                        HRMLinQ.Leave_Applications.DeleteOnSubmit(leavDet);
                        HRMLinQ.SubmitChanges();

                        FillGrid();

                        LblMsg.Text = "Entry Deleted Successfully";

                        ClearAll();

                        //Move to View Tab
                        MyMenu.Items[0].Selected = true;
                        MyMenu.Items[0].ImageUrl = "~/Images/ViewEnable.jpg";
                        MyMultiView.ActiveViewIndex = 0;
                        MyMenu.Items[1].ImageUrl = "~/Images/NewOrEditDisable.jpg"; 
                    }
                }
                else
                {
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked NO !')", true);
                }
            }
        }
        catch //(Exception ex)
        {
            Response.Redirect("~/ErrorPage.aspx");
        }
    }

    private void GetTotDays()
    {
        if (TxtFromDate.Text.Length != 0 && TxtToDate.Text.Length != 0)
        {            
            DateTime from_date = DateTime.ParseExact(TxtFromDate.Text, "dd/MM/yyyy", theCultureInfo);
            DateTime to = DateTime.ParseExact(TxtToDate.Text, "dd/MM/yyyy", theCultureInfo);
            TxtTotDays.Text = ((to - from_date).TotalDays + 1).ToString();
        }
    }

    protected void TxtFromDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            GetTotDays();
        }
        catch //(Exception ex)
        {
            Response.Redirect("~/ErrorPage.aspx");
        }        
    }
    protected void TxtToDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            GetTotDays();
        }
        catch //(Exception ex)
        {
            Response.Redirect("~/ErrorPage.aspx");
        }        
    }

    protected void btnLeaveDelete_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton btndetails = sender as ImageButton;
            GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
            Session["Id"] = GridLeave.DataKeys[gvrow.RowIndex].Value.ToString();
            lblUser.Text = "Are you sure you want to delete this entry ? ";
            ModalPopupExtender1.Show();
        }
        catch //(Exception ex)
        {
            Response.Redirect("~/ErrorPage.aspx");
        }        
    }

    protected void btnYes_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            int LeaveId = Convert.ToInt32(Session["Id"]);

            var leavdet =
                (from l in HRMLinQ.Leave_Applications
                where l.Id == LeaveId
                select l).First();

            if (leavdet != null)
            {
                HRMLinQ.Leave_Applications.DeleteOnSubmit(leavdet);
                HRMLinQ.SubmitChanges();
                FillGrid();
            }
        }
        catch //(Exception ex)
        {
            Response.Redirect("~/ErrorPage.aspx");
        }        
    }

    protected void btnLeaveSelect_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton btndetails = sender as ImageButton;
            GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
            Session["Id"] = GridLeave.DataKeys[gvrow.RowIndex].Value.ToString();
            int IntId = int.Parse(Session["Id"].ToString());

            var LeaveDS = (from l in HRMLinQ.Leave_Applications
                           where l.Id == IntId
                          select new { l.EmpId  
                             ,LVAppDate =DateTime.Parse(l.Application_Date.ToString()).ToString("dd/MM/yyyy")
                             ,LVFrmDate = DateTime.Parse(l.FromDate.ToString()).ToString("dd/MM/yyyy")
                             ,LVToDate = DateTime.Parse(l.ToDate.ToString()).ToString("dd/MM/yyyy")
                             ,l.TotalDays
                             ,l.LeaveType,l.Reason,l.LeaveStatus,l.Remark                                                            
                           }).First();
          
            if (LeaveDS == null)
            {
                LblMsg.Text = "Data not found";
                return;
            }

            HidFldId.Value = IntId.ToString();

            var EmpDs = from emp in HRMLinQ.Emp_Masts
                        orderby emp.EmpName
                        select new { EmpId = emp.Id, emp.EmpName };
            ddlEmployee.Items.Clear();
            ddlEmployee.DataSource = EmpDs; //BLayer.FillEmp("");
            ddlEmployee.DataValueField = "EmpId";
            ddlEmployee.DataTextField = "EmpName";
            ddlEmployee.DataBind();
            ddlEmployee.Items.Insert(0, new ListItem("--Select Employee--", ""));

            if (LeaveDS.EmpId.ToString() != null)
            {
                ddlEmployee.SelectedValue = LeaveDS.EmpId.ToString();
            }

            ////Below Code Also Worked For Convert Date In dd/MM/yyyy Format
            //DateTime myDateTime = DateTime.Parse(LeaveDS.Application_Date.ToString());
            //var dateingddmmyy = myDateTime.ToString("dd/MM/yyyy");

            TxtDate.Text = LeaveDS.LVAppDate.ToString();
            TxtFromDate.Text = LeaveDS.LVFrmDate.ToString();
            TxtToDate.Text = LeaveDS.LVToDate.ToString();
            TxtTotDays.Text = LeaveDS.TotalDays.ToString(); 
            DDLLeaveType.SelectedValue =LeaveDS.LeaveType.ToString();
            TxtReason.Text = LeaveDS.Reason.ToString();
            DDLStatus.SelectedValue = LeaveDS.LeaveStatus.ToString();
            TxtRemark.Text = LeaveDS.Remark.ToString();

            ddlEmployee.Enabled = false;
            LblMsg.Text = "";

            // Move to Edit Tab            
            MyMenu.Items[0].ImageUrl = "~/Images/ViewDisable.jpg";
            MyMenu.Items[1].ImageUrl = "~/Images/NewOrEditEnable.jpg";
            MyMenu.Items[1].Selected = true;
            MyMultiView.ActiveViewIndex = 1;
        }
        catch//(Exception ex)
        {
            Response.Redirect("~/ErrorPage.aspx");
        }
    }
  
    protected void GridLeave_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GridLeave.PageIndex=e.NewPageIndex;
            FillGrid();
            LblMsg.Text="";
        }
        catch //(Exception ex)
        {
            Response.Redirect("~/ErrorPage.aspx");
        }        
    }

    protected void BtnView_Click(object sender, EventArgs e)
    {
        FillGrid();
    }

    protected void ImgBtnFirst_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            DispRecord("FIRST");
        }
        catch
        {
            Response.Redirect("~/ErrorPage.aspx");
        }
    }
    protected void ImgBtnNext_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            DispRecord("NEXT");
        }
        catch
        {
            Response.Redirect("~/ErrorPage.aspx");
        }
    }
    protected void ImgBtnPrev_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            DispRecord("PREV");
        }
        catch
        {
            Response.Redirect("~/ErrorPage.aspx");
        }
    }
    protected void ImgBtnLast_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            DispRecord("LAST");
        }
        catch
        {
            Response.Redirect("~/ErrorPage.aspx");
        }
    }

    protected void DispRecord(string pStrNavigation)
    {
        int IntId = 0;

        StrSql = new StringBuilder();
        StrSql.Length = 0;
        if (pStrNavigation == "EXAC")
        {
            IntId = int.Parse(HidFldId.Value);
        }
        else if (pStrNavigation.Trim().ToUpper() == "FIRST")
        {
            StrSql.AppendLine("Select Top 1 Id From Leave_Application");
            if (int.Parse(ddlEmployee.SelectedValue) != 0)
            {
                StrSql.AppendLine("Where EmpId=" + int.Parse(ddlEmployee.SelectedValue));
            }
            StrSql.AppendLine("Order By Id");
        }
        else if (pStrNavigation.Trim().ToUpper() == "NEXT")
        {
            if (HidFldId.Value == "")
            {
                LblMsg.Text = "Data not found";
                return;
            }
            StrSql.AppendLine("Select Top 1 Id From Leave_Application Where Id > " + int.Parse(HidFldId.Value));
            if (int.Parse(ddlEmployee.SelectedValue) != 0)
            {
                StrSql.AppendLine("And EmpId=" + int.Parse(ddlEmployee.SelectedValue));
            }
        }
        else if (pStrNavigation.Trim().ToUpper() == "PREV")
        {
            if (HidFldId.Value == "")
            {
                LblMsg.Text = "Data not found";
                return;
            }
            StrSql.AppendLine("Select Top 1 Id From Leave_Application Where Id < " + int.Parse(HidFldId.Value));
            if (int.Parse(ddlEmployee.SelectedValue) != 0)
            {
                StrSql.AppendLine("And EmpId=" + int.Parse(ddlEmployee.SelectedValue));
            }
            StrSql.AppendLine("Order By Id Desc");
        }
        else if (pStrNavigation.Trim().ToUpper() == "LAST")
        {
            StrSql.AppendLine("Select Top 1 Id From Leave_Application");
            if (int.Parse(ddlEmployee.SelectedValue) != 0)
            {
                StrSql.AppendLine("Where EmpId=" + int.Parse(ddlEmployee.SelectedValue));
            }
            StrSql.AppendLine("Order By Id Desc");
        }

        if (pStrNavigation != "EXAC")
        {
            dtTemp = new DataTable();
            dtTemp = SqlFunc.ExecuteDataTable(StrSql.ToString());
            if (dtTemp.Rows.Count != 0)
            {
                IntId = int.Parse(dtTemp.Rows[0]["Id"].ToString());
            }
        }

        if (IntId != 0)
        { 
            var LeaveDS = (from l in HRMLinQ.Leave_Applications
                           where l.Id == IntId
                          select new { l.EmpId  
                             ,LVAppDate =DateTime.Parse(l.Application_Date.ToString()).ToString("dd/MM/yyyy")
                             ,LVFrmDate = DateTime.Parse(l.FromDate.ToString()).ToString("dd/MM/yyyy")
                             ,LVToDate = DateTime.Parse(l.ToDate.ToString()).ToString("dd/MM/yyyy")
                             ,l.TotalDays
                             ,l.LeaveType,l.Reason,l.LeaveStatus,l.Remark                                                            
                           }).First();

            if (LeaveDS == null)
            {
                ClearAll();
                LblMsg.Text = "Data not found";
                return;
            }

            LblMsg.Text = "";

            HidFldId.Value = IntId.ToString();

            var EmpDs = from emp in HRMLinQ.Emp_Masts
                        orderby emp.EmpName
                        select new { EmpId = emp.Id, emp.EmpName };
            ddlEmployee.Items.Clear();
            ddlEmployee.DataSource = EmpDs;
            ddlEmployee.DataValueField = "EmpId";
            ddlEmployee.DataTextField = "EmpName";
            ddlEmployee.DataBind();
            ddlEmployee.Items.Insert(0, new ListItem("--Select Employee--", ""));

            if (LeaveDS.EmpId.ToString() != null)
            {
                ddlEmployee.SelectedValue = LeaveDS.EmpId.ToString();
            }

            TxtDate.Text = LeaveDS.LVAppDate.ToString();
            TxtFromDate.Text = LeaveDS.LVFrmDate.ToString();
            TxtToDate.Text = LeaveDS.LVToDate.ToString();
            TxtTotDays.Text = LeaveDS.TotalDays.ToString();
            DDLLeaveType.SelectedValue = LeaveDS.LeaveType.ToString();
            TxtReason.Text = LeaveDS.Reason.ToString();
            DDLStatus.SelectedValue = LeaveDS.LeaveStatus.ToString();
            TxtRemark.Text = LeaveDS.Remark.ToString();

            ddlEmployee.Enabled = false;
        }
        else
        {
            ClearAll();
            LblMsg.Text = "Data not found";
            return;
        }
        
    }
}

