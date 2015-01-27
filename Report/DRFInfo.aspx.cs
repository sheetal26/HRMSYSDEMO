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

public partial class Report_DRFInfo : System.Web.UI.Page
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
        ddlEmployee.DataSource = BLayer.FillEmp(IntEmpId, ""); //EmpDs;
        ddlEmployee.DataValueField = "EmpId";
        ddlEmployee.DataTextField = "EmpName";
        ddlEmployee.DataBind();
        if (IntEmpId == 0)
        {
            ddlEmployee.Items.Insert(0, new ListItem("--Select Employee--", "0"));
        }
               
        TxtFDrfDate.Text = "";
        TxtTDrfDate.Text = "";

        var PrjDS = from prj in HRMLinq.Project_Masts                   
                   orderby prj.ProjectName
                    select new { PrjId = prj.Id, PrjName = prj.ProjectName };

        DDLPrjName.Items.Clear();
        DDLPrjName.DataSource = PrjDS; //BLayer.FillProject("");        
        DDLPrjName.DataValueField = "PrjId";
        DDLPrjName.DataTextField = "PrjName";
        DDLPrjName.DataBind();
        DDLPrjName.Items.Insert(0, new ListItem("Select Project", "0"));

        DDLPrjModule.Items.Clear();
        DDLPrjModule.Items.Insert(0, new ListItem("--Select Module--", "0"));

        DDlWorkStat.SelectedIndex = 0;

        ddlEmployee.Focus();
    }
    protected void BtnShow_Click(object sender, EventArgs e)
    {
        try
        {
            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("DRFInfoRV.rdlc");

            StrSql = new StringBuilder();
            StrSql.Length = 0;

            StrSql.AppendLine("Select E.EmpName");
            StrSql.AppendLine(",Convert(Varchar(10),H.DRFDate,103) As DRFDate");
            StrSql.AppendLine(",H.InTime,H.OutTime,H.TotTime As TotTimeH,H.Remark");
            StrSql.AppendLine(",D.StartTime,D.EndTime,D.TotTime As TotTimeD");
            StrSql.AppendLine(",P.ProjectName,M.ModuleName");
            StrSql.AppendLine(",D.Work_Desc");

            StrSql.AppendLine(",Case When IsNull(D.WorkStatus,'')='D' Then 'Done'");
            StrSql.AppendLine("      When IsNull(D.WorkStatus,'')='P' Then 'Pending'");
            StrSql.AppendLine("      When IsNull(D.WorkStatus,'')='C' Then 'Cancel' Else '' End As WorkStatus");
            StrSql.AppendLine(",D.Remark");

            StrSql.AppendLine("From DRFH H");
            StrSql.AppendLine("Left Join Emp_Mast E On H.EmpId=E.Id");
            StrSql.AppendLine("Left Join DRFDET D On D.DRFId=H.Id");
            StrSql.AppendLine("Left Join Project_Mast P On P.Id=D.PrjId");
            StrSql.AppendLine("Left Join Project_Module M On M.ProjectId=P.Id And M.Id=D.PrjModId");

            StrSql.AppendLine("Where 1=1");

            if (ddlEmployee.SelectedValue != "0")
            {
                StrSql.AppendLine("And H.EmpId=" + int.Parse(ddlEmployee.SelectedValue.ToString()));
            }
            if (TxtFDrfDate.Text.Trim() != "")
            {
                StrSql.AppendLine("And H.DRFDate >='" + ValueConvert.ConvertDate(TxtFDrfDate.Text.Trim()) + "'");
            }
            if (TxtTDrfDate.Text.Trim() != "")
            {
                StrSql.AppendLine("And H.DRFDate <='" + ValueConvert.ConvertDate(TxtTDrfDate.Text.Trim()) + "'");
            }
            if (DDLPrjName.SelectedValue != "0")
            {
                StrSql.AppendLine("And D.PrjId=" + int.Parse(DDLPrjName.SelectedValue.ToString()));
            }
            if (DDLPrjModule.SelectedValue != "0")
            {
                StrSql.AppendLine("And D.PrjModId=" + int.Parse(DDLPrjModule.SelectedValue.ToString()));
            }
            if (DDlWorkStat.SelectedValue != "0")
            {
                StrSql.AppendLine("And IsNull(D.WorkStatus,'')='" + DDlWorkStat.SelectedValue + "'");
            }

            StrSql.AppendLine("Order By E.EmpName,Convert(Varchar(10),H.DRFDate,103)");

            HRMDataSet dsAssWorkInfo = ComFunc.GetData(StrSql.ToString().Replace("\r\n", " "), "DRFInfo");

            ReportDataSource WorkDataSource = new ReportDataSource("DRFInfo", dsAssWorkInfo.Tables["DRFInfo"]);

            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(WorkDataSource);
        }
        catch
        {
            Response.Redirect("~/ErrorPage.aspx");
        }

    }
    protected void DDLPrjName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            var PMDs = from pm in HRMLinq.Project_Modules
                       where pm.ProjectId == int.Parse(DDLPrjName.SelectedValue)
                        orderby pm.ModuleName
                       select new { ModId = pm.Id, ModName = pm.ModuleName };

            DDLPrjModule.Items.Clear();
            DDLPrjModule.DataSource = PMDs; //BLayer.FillPrjModule(int.Parse(dr.SelectedValue));
            DDLPrjModule.DataValueField = "ModId";
            DDLPrjModule.DataTextField = "ModName";
            DDLPrjModule.DataBind();
            DDLPrjModule.Items.Insert(0, new ListItem("--Select Module--", "0"));
        }
        catch
        {
            Response.Redirect("~/ErrorPage.aspx");
        }
    }
}