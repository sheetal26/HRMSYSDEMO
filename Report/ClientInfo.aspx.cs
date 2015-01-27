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

public partial class Report_ClientInfo : System.Web.UI.Page
{
    #region "General Declaration"

    StringBuilder StrSql = new StringBuilder();
    Common ComFunc = new Common();

    HRMSysLinQDataContext HRMLinq = new HRMSysLinQDataContext();

    #endregion

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
        TxtClientName.Text = "";
        TxtFirmName.Text = "";
        TxtEMailId.Text = "";
 
        TxtFDOJ.Text = "";
        TxtTDOJ.Text = "";
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
        ddlState.Items.Insert(0, new ListItem("---Select State---", "0"));

        ddlCity.Items.Clear();
        ddlCity.Items.Insert(0, new ListItem("---Select City---", "0"));

        //ReportViewer1.ProcessingMode = ProcessingMode.Local;
        //ReportViewer1.LocalReport.ReportPath = Server.MapPath("ClientInfo.rdlc");
        ReportViewer1.Reset();

        TxtClientName.Focus();
    }

    protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            var StateDS = from s in HRMLinq.State_Masts
                          where s.CountryId == int.Parse(ddlCountry.SelectedValue)
                          orderby s.StateName
                          select new { StateId = s.Id, s.StateName };

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
                         select new { CityId = c.Id, c.CityName };

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
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("ClientInfoRV.rdlc");

            StrSql = new StringBuilder();
            StrSql.Length = 0;

            StrSql.AppendLine("Select C.ClientName,C.FirmName");
            StrSql.AppendLine(",CONVERT(VarChar(10),DOJ,103) As DOJ");
            StrSql.AppendLine(",C.EMailId");
            StrSql.AppendLine(",CM.CountryName");
            StrSql.AppendLine(",SM.StateName");
            StrSql.AppendLine(",Ci.CityName");
            StrSql.AppendLine(",C.Address1,C.PinCode,C.MobNo,C.Phone");
            StrSql.AppendLine(",CONVERT(VarChar(10),LeftDate,103) As LeftDate");
            StrSql.AppendLine("From Client_Mast C");
            StrSql.AppendLine("Left Join Country_Mast CM On CM.Id=C.CountryId");
            StrSql.AppendLine("Left Join State_Mast SM On SM.Id=C.StateId");
            StrSql.AppendLine("Left Join City_Mast Ci On Ci.Id=C.CityId");
            StrSql.AppendLine("Where 1=1");
            if (TxtClientName.Text.Length != 0)
            {
                StrSql.AppendLine("And C.ClientName='" + TxtClientName.Text.Trim() + "'");
            }
            if (TxtFirmName.Text.Length != 0)
            {
                StrSql.AppendLine("And C.FirmName='" + TxtFirmName.Text.Trim() + "'");
            }
            if (TxtFDOJ.Text.Trim() != "")
            {
                StrSql.AppendLine("And C.DOJ >= '" + ValueConvert.ConvertDate(TxtFDOJ.Text.Trim()) + "'");
            }
            if (TxtTDOJ.Text.Trim() != "")
            {
                StrSql.AppendLine("And C.DOJ <= '" + ValueConvert.ConvertDate(TxtTDOJ.Text.Trim()) + "'");
            }
            if (TxtEMailId.Text.Length != 0)
            {
                StrSql.AppendLine("And C.EMailId='" + TxtEMailId.Text.Trim() + "'");
            }
            if (ddlCountry.SelectedValue != "0")
            {
                StrSql.AppendLine("And C.CountryId=" + int.Parse(ddlCountry.SelectedValue.ToString()));
            }
            if (ddlState.SelectedValue != "0")
            {
                StrSql.AppendLine("And C.StateId=" + int.Parse(ddlState.SelectedValue.ToString()));
            }
            if (ddlCity.SelectedValue != "0")
            {
                StrSql.AppendLine("And C.CityId=" + int.Parse(ddlCity.SelectedValue.ToString()));
            }
            if (TxtFLeftDate.Text.Trim() != "")
            {
                StrSql.AppendLine("And C.LeftDate >='" + ValueConvert.ConvertDate(TxtFLeftDate.Text.Trim()) + "'");
            }
            if (TxtTLeftDate.Text.Trim() != "")
            {
                StrSql.AppendLine("And C.LeftDate <='" + ValueConvert.ConvertDate(TxtTLeftDate.Text.Trim()) + "'");
            }            
            StrSql.AppendLine("Order By C.ClientName");  

            HRMDataSet dsAssWorkInfo = ComFunc.GetData(StrSql.ToString().Replace("\r\n", " "), "ClientInfo");

            ReportDataSource WorkDataSource = new ReportDataSource("ClientInfo", dsAssWorkInfo.Tables["ClientInfo"]);

            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(WorkDataSource);
        }
        catch
        {
            Response.Redirect("~/ErrorPage.aspx");
        }
    }
    protected void BtnClear_Click(object sender, EventArgs e)
    {
        try
        {
            ClearAll();
        }
        catch
        {
            Response.Redirect("~/ErrorPage.aspx");
        }
    }
}