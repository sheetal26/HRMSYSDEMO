using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.Services;
using System.Xml;
using System.Xml.Linq;
using HRMSystem;

using System.Configuration;
using Microsoft.Reporting.WebForms;

public partial class Report_AssWrkInfo : System.Web.UI.Page
{
    StringBuilder StrSql = new StringBuilder();
    SqlFunction SqlFunc = new SqlFunction();
    SqlCommand cmd = new SqlCommand();
    BAL BLayer = new BAL();   
    DataTable dtTemp;
    DataSet ds = new DataSet();
    Common ComFunc = new Common();

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
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    protected void Page_Load(object sender, EventArgs e)
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
                ViewState["LoginUserGroup"] = Session["LoginUserGrp"].ToString();
                ViewState["LoginId"] = Session["LoginId"].ToString();
            }

            DDLPrjLed.Enabled = true;
            DDLPrjLed.Items.Clear();
            DDLPrjLed.DataSource = BLayer.FillEmp(0," And IsNull(EmpGroup,'')='Ledger'");
            DDLPrjLed.DataValueField = "EmpId";
            DDLPrjLed.DataTextField = "EmpName";
            DDLPrjLed.DataBind();
            DDLPrjLed.Items.Insert(0, new ListItem("--Select Ledger--", "0"));

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
            ddlEmployee.DataSource = BLayer.FillEmp(IntEmpId, "");
            ddlEmployee.DataValueField = "EmpId";
            ddlEmployee.DataTextField = "EmpName";
            ddlEmployee.DataBind();
            if (IntEmpId == 0)
            {
                ddlEmployee.Items.Insert(0, new ListItem("--Select Employee--", "0"));
            }
            
            DDLPrjName.Items.Clear();
            DDLPrjName.DataSource = BLayer.FillProject("");
            DDLPrjName.DataValueField = "PrjId";
            DDLPrjName.DataTextField = "PrjName";
            DDLPrjName.DataBind();
            DDLPrjName.Items.Insert(0, new ListItem("--Select Project--", "0"));

            DDLPrjModule.Items.Clear();
            DDLPrjModule.Items.Insert(0, new ListItem("--Select Module--", "0"));

            ddlWrkStatus.SelectedIndex = 0;
        }

    }
    protected void BtnShowWS_Click(object sender, EventArgs e)
    {
        try
        {
            HRMServiceReference.WebServiceSoapClient HrmWS = new HRMServiceReference.WebServiceSoapClient();
            ds = new DataSet();

            GridWrokList.DataSource = ComFunc.ToDataSetOrNull(HrmWS.GetWorkList(int.Parse(DDLPrjLed.SelectedValue),int.Parse(ddlEmployee.SelectedValue),int.Parse(DDLPrjName.SelectedValue),int.Parse(DDLPrjModule.SelectedValue),TxtAssignDate.Text,ddlWrkStatus.SelectedValue,TxtDueDate.Text));
            GridWrokList.DataBind();
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }      
    }
    protected void DDLPrjName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DDLPrjModule.Items.Clear();
            DDLPrjModule.DataSource = BLayer.FillPrjModule(int.Parse(DDLPrjName.SelectedValue));
            DDLPrjModule.DataValueField = "ModId";
            DDLPrjModule.DataTextField = "ModName";
            DDLPrjModule.DataBind();
            DDLPrjModule.Items.Insert(0, new ListItem("--Select Module--", "0"));
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }
    protected void BtnShow_Click(object sender, EventArgs e)
    {
        try
        {
            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("AssWrkInfoRV.rdlc");

            StrSql = new StringBuilder();
            StrSql.Length = 0;

            StrSql.AppendLine("Select PL.EmpName As [Project_Ledger]" + Environment.NewLine);
            StrSql.AppendLine(" ,P.ProjectName As Project_Name" + Environment.NewLine);
            StrSql.AppendLine(" ,M.ModuleName As Module_Name" + Environment.NewLine);
            StrSql.AppendLine(" ,CM.ClientName As Client_Name" + Environment.NewLine);
            StrSql.AppendLine(" ,E.EmpName As Employee_Name" + Environment.NewLine);
            StrSql.AppendLine(" ,Convert(Varchar(10),W.AssignDate,103) As [Assign_Date]" + Environment.NewLine);
            StrSql.AppendLine(" ,Convert(Varchar(10),W.AssignTime,108) As [Assign_Time]" + Environment.NewLine);
            StrSql.AppendLine(" ,IsNull(W.DueDays,0) As [Due_Days]" + Environment.NewLine);
            StrSql.AppendLine(" ,Convert(Varchar(10),W.DueDate,103) As [Due_Date]" + Environment.NewLine);
            StrSql.AppendLine(" ,W.WorkDet1 As [Work_1]" + Environment.NewLine);
            StrSql.AppendLine(" ,W.WorkDet2 As [Work_2]" + Environment.NewLine);
            StrSql.AppendLine(" ,W.WorkDet3 As [Work_3]" + Environment.NewLine);
            StrSql.AppendLine(" ,Case When IsNull(W.WorkStatus,'')='D' Then 'Done'" + Environment.NewLine);
            StrSql.AppendLine("	   When IsNull(W.WorkStatus,'')='P' Then 'Pending'" + Environment.NewLine);
            StrSql.AppendLine("	   When IsNull(W.WorkStatus,'')='C' Then 'Cancle'" + Environment.NewLine);
            StrSql.AppendLine("  Else '' End As [Work_Status]" + Environment.NewLine);
            StrSql.AppendLine(" ,W.Remark" + Environment.NewLine);
            StrSql.AppendLine(" ,Case When IsNull(W.CompleteDate,'')<>''" + Environment.NewLine);
            StrSql.AppendLine("       Then Convert(Varchar(10),W.CompleteDate,103) Else '' End  As [Complete_Date]" + Environment.NewLine);
            StrSql.AppendLine(" ,Convert(Varchar(10),W.CompleteTime,108) As [Complete_Time]" + Environment.NewLine);

            StrSql.AppendLine(" From WorkAssDet W" + Environment.NewLine);
            StrSql.AppendLine(" Left Join Emp_Mast PL On PL.Id=W.PrjLedId " + Environment.NewLine);
            StrSql.AppendLine(" Left Join Project_Mast P On P.Id=W.PrjId" + Environment.NewLine);
            StrSql.AppendLine(" Left Join Project_Module M On M.Id=W.PrjModId" + Environment.NewLine);
            StrSql.AppendLine(" Left Join Client_Mast CM On CM.Id=P.ClientId" + Environment.NewLine);
            StrSql.AppendLine(" Left Join Emp_Mast E On E.Id=W.EmpId" + Environment.NewLine);

            StrSql.AppendLine(" Where 1=1 " + Environment.NewLine);

            if (DDLPrjLed.SelectedValue != "0")
            {
                StrSql.AppendLine("And W.PrjLedId=" + int.Parse(DDLPrjLed.SelectedValue.ToString()) + Environment.NewLine);
            }

            if (ddlEmployee.SelectedValue != "0")
            {
                StrSql.AppendLine("And W.EmpId=" + int.Parse(ddlEmployee.SelectedValue.ToString()) + Environment.NewLine);
            }

            if (DDLPrjName.SelectedValue != "0")
            {
                StrSql.AppendLine("And W.PrjId=" + int.Parse(DDLPrjName.SelectedValue.ToString())  + Environment.NewLine);
            }

            if (DDLPrjModule.SelectedValue != "0")
            {
                StrSql.AppendLine("And W.PrjModId=" + int.Parse(DDLPrjModule.SelectedValue.ToString()) + Environment.NewLine);
            }

            if (TxtAssignDate.Text.Trim() != "")
            {
                StrSql.AppendLine("And W.AssignDate ='" + ValueConvert.ConvertDate(TxtAssignDate.Text.Trim()) + "'" + Environment.NewLine);
            }

            if (ddlWrkStatus.SelectedValue != "0")
            {
                StrSql.AppendLine("And W.WorkStatus='" + ddlWrkStatus.SelectedValue.ToString()  + "'" + Environment.NewLine);
            }

            if (TxtDueDate.Text.Trim() != "")
            {
                StrSql.AppendLine("And W.DueDate ='" + ValueConvert.ConvertDate(TxtDueDate.Text.Trim()) + "'" + Environment.NewLine);
            }
            StrSql.AppendLine(" Order By PL.EmpName,P.ProjectName,M.ModuleName,E.EmpName" + Environment.NewLine);

            HRMDataSet dsAssWorkInfo = ComFunc.GetData(StrSql.ToString().Replace("\r\n"," "), "AssWorkInfo");

            ReportDataSource WorkDataSource = new ReportDataSource("AssWorkInfo", dsAssWorkInfo.Tables["AssWorkInfo"]);

            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(WorkDataSource);                 
        }
        catch 
        {
            Response.Redirect("~/ErrorPage.aspx");
        }
    }
}