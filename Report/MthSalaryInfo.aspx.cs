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

public partial class Report_MthSalaryInfo : System.Web.UI.Page
{
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
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
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

                ClearAll();
            }
        }     
        catch
        {           
            Response.Redirect("~/ErrorPage.aspx");
        }      
    }

    protected void FillYear()
    {
        int i, IntYear;

        IntYear = DateTime.Today.Year;

        ddlyear.Items.Clear();
        ddlyear.Items.Insert(0, new ListItem("--Select Year--", "0"));
        ddlyear.Items.Insert(1, new ListItem(IntYear.ToString(), IntYear.ToString()));
        for (i = 2; i <= 100; i++)
        {
            IntYear = IntYear - 1;
            ddlyear.Items.Insert(i, new ListItem(IntYear.ToString(), IntYear.ToString()));
        }
    }

    protected void ClearAll()
    {
        var EmpDs = from e in HRMLinq.Emp_Masts
                    orderby e.EmpName
                    select new { EmpId = e.Id, e.EmpName };

        ddlEmployee.Items.Clear();
        ddlEmployee.DataSource = EmpDs;//BLayer.FillEmp("");
        ddlEmployee.DataValueField = "EmpId";
        ddlEmployee.DataTextField = "EmpName";
        ddlEmployee.DataBind();
        ddlEmployee.Items.Insert(0, new ListItem("--Select Employee--", "0"));

        FillYear();
        
        ddlyear.SelectedValue = DateTime.Today.Year.ToString();
        ddlmonth.SelectedIndex = int.Parse(DateTime.Today.Month.ToString());

        LblMsg.Text = "";
    }

    protected void BtnShow_Click(object sender, EventArgs e)
    {
        try
        {
            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("MthSalaryInfoRV.rdlc");

            StrSql = new StringBuilder();
            StrSql.Length = 0;

            StrSql.AppendLine("Select M.MYear As [Year],M.MMonth As [Month],E.EmpName As Emp_Name");
            StrSql.AppendLine(",M.MthDays As Month_Days,M.WOffDays As WOff_Days");
            StrSql.AppendLine(",M.JobTime As Job_Time,M.TotHours As Tot_Hours");
            StrSql.AppendLine(",M.PerDaySal As Per_Day_Sal");
            StrSql.AppendLine(",M.PerHourSal As Per_Hour_Sal");
            StrSql.AppendLine(",M.Present,M.Absent");
            StrSql.AppendLine(",M.OverTime As Over_Time,M.LessTime As Less_Time,M.ExtraOrCut As Extra_Or_Cut");
            StrSql.AppendLine(",M.WorkHours As Work_Hours");
            StrSql.AppendLine(",M.BasicRate As Basic_Rate,M.CutSal As Cut_Sal");
            StrSql.AppendLine(",M.WorkSal,M.ExtraOrCutSal,M.GiveSal");
            StrSql.AppendLine("From MTHSALLARY M");
            StrSql.AppendLine("Left Join Emp_Mast E On E.Id=M.EmpId");
            StrSql.AppendLine("Where 1=1");

            if (ddlEmployee.SelectedValue != "0")
            {
                StrSql.AppendLine("And M.EmpId=" + int.Parse(ddlEmployee.SelectedValue.ToString()));
            }

            if (DDLEmpGroup.SelectedValue != "0")
            {
                StrSql.AppendLine("And E.EmpGroup=" + int.Parse(DDLEmpGroup.SelectedValue.ToString()));
            }

            if (ddlyear.SelectedValue != "0")
            {
                StrSql.AppendLine("And M.MYear=" + int.Parse(ddlyear.SelectedValue));
            }

            if (ddlmonth.SelectedValue != "0")
            {
                StrSql.AppendLine("And M.MMonth='" + ddlmonth.SelectedItem.Text.ToString() + "'");
            }

            StrSql.AppendLine("Order By M.MYear,M.MMonth,E.EmpName");

            HRMDataSet dsAssWorkInfo = ComFunc.GetData(StrSql.ToString().Replace("\r\n", " "), "MthSalaryInfo");

            ReportDataSource WorkDataSource = new ReportDataSource("MthSalaryInfo", dsAssWorkInfo.Tables["MthSalaryInfo"]);

            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(WorkDataSource);
        }
        catch
        {
            Response.Redirect("~/ErrorPage.aspx");
        }
    }
}