using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using Microsoft.Reporting.WebForms;
using System.Text;
using System.Web.Services;
using System.Xml;
using System.Xml.Linq;
using HRMSystem;

public partial class Report_ProjectList : System.Web.UI.Page
{
    SqlFunction SqlFunc = new SqlFunction();
    DataTable dtTemp = new DataTable();
    StringBuilder StrSql = new StringBuilder();
    DataSet ds = new DataSet();
    BAL BLayer = new BAL();
    //ValueConvert VC = new ValueConvert();
    Common ComFunc=new Common();
            
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

                DDLPrjName.Items.Clear();
                DDLPrjName.DataSource = BLayer.FillProject("");
                DDLPrjName.DataValueField = "PrjId";
                DDLPrjName.DataTextField = "PrjName";
                DDLPrjName.DataBind();
                DDLPrjName.Items.Insert(0, new ListItem("--Select Project--", "0"));

                DDLPrjModule.Items.Clear();
                DDLPrjModule.Items.Insert(0, new ListItem("--Select Module--", "0"));

                ddlPrjStatus.SelectedIndex = 0;
            }
        }
        catch(Exception ex)
        {
            Response.Write(ex.ToString());
        }        
    }

    protected void BtnShow_Click(object sender, EventArgs e)
    {
        try
        {
            ReportViewer1.ProcessingMode = ProcessingMode.Local;            
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("ProjectListRV.rdlc");

            StrSql = new StringBuilder();
            StrSql.Length = 0;
            
            StrSql.AppendLine("Select P.ProjectName");
            StrSql.AppendLine(",Convert(Varchar(10),P.StartDate,103) As StartDate");
            StrSql.AppendLine(",Convert(Varchar(10),P.EndDate,103) As EndDate");
            StrSql.AppendLine(",Case When IsNull(P.ProjctStatus,'')='D' Then 'Done'");
            StrSql.AppendLine("      When IsNull(P.ProjctStatus,'')='P' Then 'Pending'");
            StrSql.AppendLine("Else '' End As ProjctStatus");
            StrSql.AppendLine(",P.Remark,C.ClientName");
            StrSql.AppendLine(",M.ModuleName");
            StrSql.AppendLine(",Convert(Varchar(10),M.StartDate,103) As ModStart_Date");
            StrSql.AppendLine(",Convert(Varchar(10),M.EndDate,103) As ModEnd_Date");
            StrSql.AppendLine(",Case When IsNull(M.ModuleStatus,'')='D' Then 'Done'");
            StrSql.AppendLine("      When IsNull(M.ModuleStatus,'')='P' Then 'Pending'");
            StrSql.AppendLine("      When IsNull(M.ModuleStatus,'')='C' Then 'Cancel'");
            StrSql.AppendLine("Else '' End As ModuleStatus");
            StrSql.AppendLine(",M.Remark As ModRemark");
            StrSql.AppendLine("From Project_Mast P");
            StrSql.AppendLine("Left Join Project_Module M On P.Id=M.ProjectId");
            StrSql.AppendLine("Left Join Client_Mast C On P.ClientId=C.Id");
            StrSql.AppendLine("Where 1=1 ");

            if (DDLPrjName.SelectedValue != "0")
            {
                StrSql.AppendLine("And P.Id=" + DDLPrjName.SelectedValue);
            }

            if (ddlPrjStatus.SelectedValue != "0")
            {
                StrSql.AppendLine("And P.ProjctStatus='" + ddlPrjStatus.SelectedValue + "'");
            }

            if (TxtStartDate.Text.Length != 0)
            {
                StrSql.AppendLine("And P.StartDate >='" + ValueConvert.ConvertDate(TxtStartDate.Text) + "'");
            }

            if (TxtEndDate.Text.Length != 0)
            {
                StrSql.AppendLine("And P.EndDate <='" + ValueConvert.ConvertDate(TxtEndDate.Text) + "'");
            }

            if (DDLPrjModule.SelectedValue != "0")
            {
                StrSql.AppendLine("And M.Id=" + DDLPrjModule.SelectedValue);
            }

            if (ddlModStatus.SelectedValue != "0")
            {
                StrSql.AppendLine("And M.ModuleStatus='" + ddlModStatus.SelectedValue + "'");
            }

            if (TxtModStartDate.Text.Length != 0)
            {
                StrSql.AppendLine("And M.StartDate >='" + ValueConvert.ConvertDate(TxtModStartDate.Text) + "'");
            }

            if (TxtModEndDate.Text.Length != 0)
            {
                StrSql.AppendLine("And M.EndDate <='" + ValueConvert.ConvertDate(TxtModEndDate.Text) + "'");
            }

            StrSql.AppendLine(" Order By P.ProjectName");

            HRMDataSet dsProjectInfo = ComFunc.GetData(StrSql.ToString().Replace("\r\n", " "), "ProjectInfo");

            ReportDataSource PrjDataSource = new ReportDataSource("ProjectInfo", dsProjectInfo.Tables[0]);

            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(PrjDataSource);
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
    protected void BtnShowCR_Click(object sender, EventArgs e)
    {

    }

    protected void BtnShowWS_Click(object sender, EventArgs e)
    {
        try
        {
            HRMServiceReference.WebServiceSoapClient HrmWS = new HRMServiceReference.WebServiceSoapClient();
            ds = new DataSet();

            GridPrjList.DataSource = ComFunc.ToDataSetOrNull(HrmWS.GetProjectList(int.Parse(DDLPrjName.SelectedValue), ddlPrjStatus.SelectedValue, TxtStartDate.Text, TxtEndDate.Text, int.Parse(DDLPrjModule.SelectedValue), ddlModStatus.SelectedValue, TxtModStartDate.Text, TxtModEndDate.Text));
            GridPrjList.DataBind();
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }
    protected void BtnShowQE_Click(object sender, EventArgs e)
    {

    }
}