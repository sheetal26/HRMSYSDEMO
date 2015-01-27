using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using System.Text;
//using System.Web.Services;
//using System.Xml;
//using System.Xml.Linq;
using HRMSystem;

using System.Configuration;
using Microsoft.Reporting.WebForms;

public partial class Report_EmpInfo : System.Web.UI.Page
{
    StringBuilder StrSql = new StringBuilder();
    //SqlFunction SqlFunc = new SqlFunction();
    //SqlCommand cmd = new SqlCommand();
    //BAL BLayer = new BAL();
    //DataTable dtTemp;
    //DataSet ds = new DataSet();
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
        var EmpDs = from e in HRMLinq.Emp_Masts
                    orderby e.EmpName
                    select new {EmpId=e.Id,e.EmpName};

        ddlEmployee.Enabled = true;
        ddlEmployee.Items.Clear();
        ddlEmployee.DataSource = EmpDs; //BLayer.FillEmp("");
        ddlEmployee.DataValueField = "EmpId";
        ddlEmployee.DataTextField = "EmpName";
        ddlEmployee.DataBind();
        ddlEmployee.Items.Insert(0, new ListItem("--Select Employee--", "0"));

        DDLEmpGroup.SelectedIndex = 0;

        ddlBloodGrp.SelectedIndex = 0;

        var DeptDs = from dept in HRMLinq.Dept_Masts
                     orderby dept.DeptName
                     select new {DeptId=dept.Id,dept.DeptName};
        DDLDeptName.Items.Clear();
        DDLDeptName.DataSource = DeptDs; //BLayer.FillDept();
        DDLDeptName.DataValueField = "DeptId";
        DDLDeptName.DataTextField = "DeptName";
        DDLDeptName.DataBind();
        DDLDeptName.Items.Insert(0, new ListItem("--Select Department--", "0"));

        DDLDesignation.Items.Clear();
        DDLDesignation.Items.Insert(0,new ListItem("---Select Designation---","0"));

        rblGender.SelectedIndex = 0;

        TxtFDOJ.Text = "";
        TxtTDOJ.Text = "";
        TxtFDOB.Text = "";
        TxtTDOB.Text = "";
        TxtFLeftDate.Text = "";
        TxtTLeftDate.Text = "";

        var ContryDs = from c in HRMLinq.Country_Masts
                       orderby c.CountryName
                       select new { CountryId = c.Id, c.CountryName };

        ddlCountry.Items.Clear();
        ddlCountry.DataSource = ContryDs; //BLayer.FillCountry();
        ddlCountry.DataValueField = "CountryId";
        ddlCountry.DataTextField = "CountryName";
        ddlCountry.DataBind();
        ddlCountry.Items.Insert(0, new ListItem("--Select Country--", "0"));

        ddlState.Items.Clear();
        ddlState.Items.Insert(0,new ListItem("---Select State---","0"));

        ddlCity.Items.Clear();
        ddlCity.Items.Insert(0,new ListItem ("---Select City---","0"));

        ddlEmployee.Focus(); 
    }

    protected void DDLDeptName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            var DesigDs = from d in HRMLinq.Desig_Masts
                          where d.Dept_Id == int.Parse(DDLDeptName.SelectedValue) 
                          orderby d.DesigName
                          select new {DesigId=d.Id,d.DesigName};

            DDLDesignation.DataSource = DesigDs; //BLayer.FillDesig(int.Parse(DDLDeptName.SelectedValue));
            DDLDesignation.DataValueField = "DesigId";
            DDLDesignation.DataTextField = "DesigName";
            DDLDesignation.DataBind();
            DDLDesignation.Items.Insert(0, new ListItem("--Select Designation--", "0"));
        }
        catch
        {
            Response.Redirect("~/ErrorPage.aspx");
        }
    }
    protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            var StateDS = from s in HRMLinq.State_Masts
                          where s.CountryId == int.Parse(ddlCountry.SelectedValue)
                          orderby s.StateName
                          select new {StateId=s.Id,s.StateName};

            ddlState.Items.Clear();
            ddlState.DataSource = StateDS; //BLayer.FillState(int.Parse(ddlCountry.SelectedValue));
            ddlState.DataValueField = "StateId";
            ddlState.DataTextField = "StateName";
            ddlState.DataBind();
            ddlState.Items.Insert(0, new ListItem("--Select State--", "0"));

            ddlCity.Items.Clear();
            ddlCity.Items.Insert(0, new ListItem("--Select City--", "0"));
        }
        catch
        {
            Response.Redirect("~/ErrorPage.aspx");
        }
    }
    protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            var CityDs = from c in HRMLinq.City_Masts
                         where c.StateId == int.Parse(ddlState.SelectedValue)
                         orderby c.CityName
                         select new {CityId=c.Id,c.CityName};

            ddlCity.Items.Clear();
            ddlCity.DataSource = CityDs; //BLayer.FillCity(int.Parse(ddlState.SelectedValue));
            ddlCity.DataValueField = "CityId";
            ddlCity.DataTextField = "CityName";
            ddlCity.DataBind();
            ddlCity.Items.Insert(0, new ListItem("--Select City--", "0"));
        }
        catch
        {
            Response.Redirect("~/ErrorPage.aspx");
        }
    }
    protected void BtnShow_Click(object sender, EventArgs e)
    {
        try
        {
            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("EmpInfoRV.rdlc");

            StrSql = new StringBuilder();
            StrSql.Length = 0;

            StrSql.AppendLine("Select E.EmpName,E.EmpGroup");
            StrSql.AppendLine(",D.DeptName,Ds.DesigName ");
            StrSql.AppendLine(",CONVERT(VarChar(10),E.DOJ,103) As DOJ");
            StrSql.AppendLine(",IsNull(E.BasicRate,0) As BasicRate");
            StrSql.AppendLine(",CONVERT(VarChar(10),E.DOB,103) As DOB");
            StrSql.AppendLine(",Case When IsNull(E.Gender,'') ='M' Then 'Male'");
            StrSql.AppendLine("      When IsNull(E.Gender,'') ='F' Then 'Female' Else '' End As Gender");
            StrSql.AppendLine(",E.EMailId");
            StrSql.AppendLine(",C.CountryName,S.StateName,Ci.CityName");
            StrSql.AppendLine(",E.Address1,E.PinCode,E.MobNo,E.Phone,E.BloodGroup");
            StrSql.AppendLine(",Convert(VarChar(10),E.LeftDate,103) As LeftDate");
            StrSql.AppendLine(",E.FJobTime,E.TJobTime,E.TotTime");
            StrSql.AppendLine("From Emp_Mast E");
            StrSql.AppendLine("Left Join Dept_Mast D On D.Id=E.DeptId");
            StrSql.AppendLine("Left Join Desig_Mast Ds On Ds.Id=E.DesigId");
            StrSql.AppendLine("Left Join Country_Mast C On C.Id=E.CountryId");
            StrSql.AppendLine("Left Join State_Mast S On S.Id=E.StateId");
            StrSql.AppendLine("Left Join City_Mast Ci On Ci.Id=E.CityId");
            StrSql.AppendLine("Where 1=1");

            if (ddlEmployee.SelectedValue != "0")
            {
                StrSql.AppendLine("And E.Id=" + int.Parse(ddlEmployee.SelectedValue.ToString()) );
            }
            if (DDLEmpGroup.SelectedValue != "0")
            {
                StrSql.AppendLine("And IsNull(E.EmpGroup,'')='" + DDLEmpGroup.SelectedValue.ToString() + "'");
            }
            if (ddlBloodGrp.SelectedValue != "0")
            {
                StrSql.AppendLine("And IsNull(E.BloodGroup,'')='" + ddlBloodGrp.SelectedValue.ToString() + "'");
            }
            if (DDLDeptName.SelectedValue != "0")
            {
                StrSql.AppendLine("And E.DeptId=" + int.Parse(DDLDeptName.SelectedValue.ToString()));
            }
            if (DDLDesignation.SelectedValue != "0")
            {
                StrSql.AppendLine("And E.DesigId=" + int.Parse(DDLDesignation.SelectedValue.ToString()));
            }

            if (rblGender.SelectedValue != "A")
            {
                StrSql.AppendLine("And IsNull(E.Gender,'')='" + rblGender.SelectedValue + "'");
            }

            if (TxtFDOJ.Text.Trim() != "")
            {
                StrSql.AppendLine("And E.DOJ >='" + ValueConvert.ConvertDate(TxtFDOJ.Text.Trim()) + "'");
            }
            if (TxtTDOJ.Text.Trim() != "")
            {
                StrSql.AppendLine("And E.DOJ <='" + ValueConvert.ConvertDate(TxtTDOJ.Text.Trim()) + "'");
            }

            if (TxtFDOB.Text.Trim() != "")
            {
                StrSql.AppendLine("And E.DOB >='" + ValueConvert.ConvertDate(TxtFDOB.Text.Trim()) + "'");
            }
            if (TxtTDOB.Text.Trim() != "")
            {
                StrSql.AppendLine("And E.DOB <='" + ValueConvert.ConvertDate(TxtTDOB.Text.Trim()) + "'");
            }

            if (TxtFLeftDate.Text.Trim() != "")
            {
                StrSql.AppendLine("And E.LeftDate >='" + ValueConvert.ConvertDate(TxtFLeftDate.Text.Trim()) + "'");
            }
            if (TxtTLeftDate.Text.Trim() != "")
            {
                StrSql.AppendLine("And E.LeftDate <='" + ValueConvert.ConvertDate(TxtTLeftDate.Text.Trim()) + "'");
            }

            if (ddlCountry.SelectedValue != "0")
            {
                StrSql.AppendLine("And E.CountryId=" + int.Parse(ddlCountry.SelectedValue.ToString()));
            }
            if (ddlState.SelectedValue != "0")
            {
                StrSql.AppendLine("And E.StateId=" + int.Parse(ddlState.SelectedValue.ToString()));
            }
            if (ddlCity.SelectedValue != "0")
            {
                StrSql.AppendLine("And E.CityId=" + int.Parse(ddlCity.SelectedValue.ToString()));
            }

            StrSql.AppendLine("Order By E.EmpName ");

            HRMDataSet dsAssWorkInfo = ComFunc.GetData(StrSql.ToString().Replace("\r\n", " "), "EmpInfo");

            ReportDataSource WorkDataSource = new ReportDataSource("EmpInfo", dsAssWorkInfo.Tables["EmpInfo"]);

            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(WorkDataSource);
        }
        catch
        {
            Response.Redirect("~/ErrorPage.aspx");
        }
    }    
}