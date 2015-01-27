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

public partial class Report_AttendanceInfo : System.Web.UI.Page
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
        ddlEmployee.Items.Clear();
        ddlEmployee.DataSource = BLayer.FillEmp(IntEmpId, "");
        ddlEmployee.DataValueField = "EmpId";
        ddlEmployee.DataTextField = "EmpName";
        ddlEmployee.DataBind();
        if (IntEmpId == 0)
        {
            ddlEmployee.Items.Insert(0, new ListItem("--Select Employee--", "0"));
        }

        FillYear();
        ddlmonth.SelectedIndex = 0;

        TxtFDate.Text = "";
        TxtTDate.Text = "";
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

    protected void GetFrmToDate()
    {
        if (ddlyear.SelectedValue == "0" || ddlmonth.SelectedValue == "0")
        {
            TxtFDate.Text = "";
            TxtTDate.Text = "";
            return;
        }
        int days = DateTime.DaysInMonth(int.Parse(ddlyear.SelectedValue), int.Parse(ddlmonth.SelectedValue));
        string FrmDate, ToDate;

        FrmDate = "01/" + ddlmonth.SelectedValue + "/" + ddlyear.SelectedValue;
        ToDate = days + "/" + ddlmonth.SelectedValue + "/" + ddlyear.SelectedValue;

        TxtFDate.Text = FrmDate;
        TxtTDate.Text = ToDate;
    }

    protected void ddlmonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GetFrmToDate();
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    protected void ddlyear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GetFrmToDate();
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
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("AttendanceInfoRV.rdlc");

            StrSql = new StringBuilder();
            StrSql.Length = 0;

            StrSql.AppendLine("Select E.EmpName");
            StrSql.AppendLine(",DATEPART(Year,Working_Date) As Working_Year,D.Working_Month ");
            StrSql.AppendLine(",D.Working_Days,Convert(Varchar(10),D.Working_Date,103) As Working_Date ");
            StrSql.AppendLine(",D.InTime,D.OutTime,D.TotWork_Hours ");
            StrSql.AppendLine(",D.OverTime,D.LessTime ");
            StrSql.AppendLine(",D.Reason,D.Remark ");
            StrSql.AppendLine("From DailyAttendance D");
            StrSql.AppendLine("Left Join Emp_Mast E On E.Id=D.EmpId");
          
            StrSql.AppendLine("Where 1=1");

            if (ddlEmployee.SelectedValue != "0")
            {
                StrSql.AppendLine("And D.EmpId=" + int.Parse(ddlEmployee.SelectedValue.ToString()));
            }

            if (ddlyear.SelectedValue != "0")
            { 
                StrSql.AppendLine("And DATEPART(Year,D.Working_Date)=" + int.Parse(ddlyear.SelectedValue));
            }

            if (ddlmonth.SelectedValue != "0")
            {
                StrSql.AppendLine("And D.Working_Month='" + ddlmonth.Text.ToString() + "'");
            }

            if (TxtFDate.Text.Trim() != "")
            {
                StrSql.AppendLine("And D.Working_Date >='" + ValueConvert.ConvertDate(TxtFDate.Text.Trim()) + "'");
            }
            if (TxtTDate.Text.Trim() != "")
            {
                StrSql.AppendLine("And D.Working_Date <='" + ValueConvert.ConvertDate(TxtTDate.Text.Trim()) + "'");
            }

            StrSql.AppendLine("Order By E.EmpName,DATEPART(Year,D.Working_Date),DATEPART(MONTH,D.Working_Date),Convert(Varchar(10),D.Working_Date,103)");

            HRMDataSet dsAssWorkInfo = ComFunc.GetData(StrSql.ToString().Replace("\r\n", " "), "AttendanceInfo");

            ReportDataSource WorkDataSource = new ReportDataSource("AttendanceInfo", dsAssWorkInfo.Tables["AttendanceInfo"]);

            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(WorkDataSource);
        }
        catch
        {
            Response.Redirect("~/ErrorPage.aspx");
        }

    }
}