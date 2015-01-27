using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using System.Text;
using HRMSystem;

using System.Configuration;
using Microsoft.Reporting.WebForms;

public partial class Report_LeaveInfo : System.Web.UI.Page
{
    BAL BLayer = new BAL();
    StringBuilder StrSql = new StringBuilder();
    Common ComFunc = new Common();

    HRMSysLinQDataContext HRMLinq = new HRMSysLinQDataContext();

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
                    ViewState["LoginUserGroup"] = Session["LoginUserGrp"].ToString();
                    ViewState["LoginId"] = Session["LoginId"].ToString();
                }

                ClearAll();
            }
        }
        catch
        {
            Response.Redirect("~/ErrorPage.aspx");
        }
    }

    protected void ClearAll()
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

        //var EmpDs = from e in HRMLinq.Emp_Masts
        //            orderby e.EmpName
        //            select new { EmpId = e.Id, e.EmpName };

        ddlEmployee.Items.Clear();
        ddlEmployee.DataSource = BLayer.FillEmp(IntEmpId, ""); ///EmpDs; 
        ddlEmployee.DataValueField = "EmpId";
        ddlEmployee.DataTextField = "EmpName";
        ddlEmployee.DataBind();
        if (IntEmpId == 0)
        {
            ddlEmployee.Items.Insert(0, new ListItem("--Select Employee--", "0"));
        }

        DDLLeaveType.SelectedIndex = 0;
        DDLStatus.SelectedIndex = 0;

        TxtAppFDate.Text = "";
        TxtAppTDate.Text = "";
 
        ddlEmployee.Focus();
    }


    protected void BtnShow_Click(object sender, EventArgs e)
    {
        try
        {
            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("LeaveInfoRV.rdlc");

            StrSql = new StringBuilder();
            StrSql.Length = 0;

            StrSql.AppendLine("Select E.EmpName");
            StrSql.AppendLine(",Convert(Varchar(10),L.Application_Date,103) As Application_Date ");
            StrSql.AppendLine(",Convert(Varchar(10),L.FromDate,103) As From_Date");
            StrSql.AppendLine(",Convert(Varchar(10),L.ToDate,103) As To_Date");
            StrSql.AppendLine(",L.TotalDays As Total_Days");
            StrSql.AppendLine(",Case When IsNull(L.LeaveStatus,'')='A' Then 'Approve'");
            StrSql.AppendLine("      When IsNull(L.LeaveStatus,'')='U' Then 'UnApproved' ");
            StrSql.AppendLine("      When IsNull(L.LeaveStatus,'')='C' Then 'Cancel'");
            StrSql.AppendLine("      Else '' End As Leave_Status");
            StrSql.AppendLine(",Case When IsNull(L.LeaveType,'')='H' Then 'Half Day'");
            StrSql.AppendLine("	     When IsNull(L.LeaveType,'')='F' Then 'Full Day'");
            StrSql.AppendLine("	     Else '' End As Leave_Type");
            StrSql.AppendLine(",L.Reason,L.Remark ");

            StrSql.AppendLine("From Leave_Application L");
            StrSql.AppendLine("Left Join Emp_Mast E On E.Id=L.EmpId");

            StrSql.AppendLine("Where 1=1");

            if (ddlEmployee.SelectedValue != "0")
            {
                StrSql.AppendLine("And L.EmpId=" + int.Parse(ddlEmployee.SelectedValue.ToString()));
            }

            if (DDLLeaveType.SelectedValue != "0")
            {
                StrSql.AppendLine("And IsNull(L.LeaveType,'')='" + DDLLeaveType.SelectedValue.ToString() + "'");
            }

            if (DDLStatus.SelectedValue != "0")
            {
                StrSql.AppendLine("And IsNull(L.LeaveStatus,'')='" + DDLStatus.SelectedValue.ToString()  + "'");
            }

            if (TxtAppFDate.Text.Trim() != "")
            {
                StrSql.AppendLine("And L.Application_Date >='" + ValueConvert.ConvertDate(TxtAppFDate.Text.Trim()) + "'");
            }

            if (TxtAppTDate.Text.Trim() != "")
            {
                StrSql.AppendLine("And L.Application_Date <='" + ValueConvert.ConvertDate(TxtAppTDate.Text.Trim()) + "'");
            }

            StrSql.AppendLine("Order By E.EmpName,Convert(Varchar(10),L.Application_Date,103)");


            HRMDataSet dsAssWorkInfo = ComFunc.GetData(StrSql.ToString().Replace("\r\n", " "), "LeaveInfo");

            ReportDataSource WorkDataSource = new ReportDataSource("LeaveInfo", dsAssWorkInfo.Tables["LeaveInfo"]);

            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(WorkDataSource);
        }
        catch
        {
            Response.Redirect("~/ErrorPage.aspx");
        }

    }
}