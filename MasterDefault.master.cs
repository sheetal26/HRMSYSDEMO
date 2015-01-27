using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text; 

public partial class MasterDefault : System.Web.UI.MasterPage
{
    Security SC = new Security();
    SqlFunction SqlFunc = new SqlFunction();
    DataTable dtTemp;
    StringBuilder StrSql=new StringBuilder();
    BAL BLayer = new BAL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.Cookies["UserId"] != null && Request.Cookies["Password"] != null)
            {
                TxtUserId.Value = Request.Cookies["UserId"].Value;
                TxtPassword.Attributes["value"] = Request.Cookies["Password"].Value;
            }

            DdlUserGrp.Items.Clear();
            DdlUserGrp.DataSource = BLayer.FillUserGroup();
            DdlUserGrp.DataTextField ="UGroupName";
            DdlUserGrp.DataValueField = "UGroupID";
            DdlUserGrp.DataBind();
            DdlUserGrp.Items.Insert(0,new ListItem("---Select User Group---", "0"));
            DdlUserGrp.Items.Insert(1, new ListItem("Employee", "EMP")); 
        }
    }

    protected void BtnSubmit_Click(object sender, EventArgs e)
    {   

        StrSql = new StringBuilder();
        StrSql.Length = 0;
        if (DdlUserGrp.Value == "EMP")
        {
            StrSql.AppendLine("Select EM.EmpName,EM.EMailId,EM.ID,DM.DesigName");
            StrSql.AppendLine("From Emp_Mast EM ");
            StrSql.AppendLine("Left Join Desig_Mast DM On EM.DesigId=DM.Id");
            StrSql.AppendLine("Where ISDATE(EM.LeftDate)=0 And EM.EMailId ='" + TxtUserId.Value + "'");
            StrSql.AppendLine("And EM.Password='" + SC.Encrypt(TxtPassword.Value) + "'");
        }
        else
        {
            StrSql.AppendLine("Select UM.LoginName,UM.EMailId,UM.ID,UM.UID,UG.Group_Name");
            StrSql.AppendLine("From User_Mast UM ");
            StrSql.AppendLine("Left Join User_Group UG On UM.UserGroup=UG.Id");
            StrSql.AppendLine("Where (UM.EMailId ='" + TxtUserId.Value + "' Or UM.MobNo='" + TxtUserId.Value + "')");
            StrSql.AppendLine("And UM.Password='" + SC.Encrypt(TxtPassword.Value) + "'");
            StrSql.AppendLine("And UM.UserGroup=" + int.Parse(DdlUserGrp.Value));
        }        

        dtTemp = new DataTable();
        dtTemp = SqlFunc.ExecuteDataTable(StrSql.ToString());

        if (dtTemp.Rows.Count > 0)
        {
            if (ChkRememberMe.Checked)
            {
                Response.Cookies["UserId"].Expires = DateTime.Now.AddDays(15);
                Response.Cookies["Password"].Expires = DateTime.Now.AddDays(15);
            }
            else
            {
                Response.Cookies["UserId"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies["Password"].Expires = DateTime.Now.AddDays(-1);
            }
            Response.Cookies["UserId"].Value = TxtUserId.Value.Trim();
            Response.Cookies["Password"].Value = TxtPassword.Value.Trim();

            if (DdlUserGrp.Value == "EMP")
            {
                Session["LoginName"] = dtTemp.Rows[0]["EmpName"].ToString();
                Session["LoginEMailId"] = dtTemp.Rows[0]["EMailId"].ToString();
                Session["LoginId"] = dtTemp.Rows[0]["ID"].ToString();
                Session["LoginEmpDesig"] = dtTemp.Rows[0]["DesigName"].ToString();
              
                Session["LoginUser"] = dtTemp.Rows[0]["ID"].ToString();
                Session["LoginUserId"] = dtTemp.Rows[0]["ID"].ToString();
                Session["LoginUserGrp"] = "EMP"; //dtTemp.Rows[0]["EmpName"].ToString();

                //Response.Redirect("EmpHome.aspx");
                //Response.Redirect("HomePage.aspx");
            }
            else
            {
                Session["LoginName"] = dtTemp.Rows[0]["LoginName"].ToString();
                Session["LoginEMailId"] = dtTemp.Rows[0]["EMailId"].ToString();
                Session["LoginId"] = dtTemp.Rows[0]["ID"].ToString();
                Session["LoginEmpDesig"] = dtTemp.Rows[0]["Group_Name"].ToString();

                Session["LoginUser"] = dtTemp.Rows[0]["UID"].ToString();
                Session["LoginUserId"] = dtTemp.Rows[0]["ID"].ToString() + "-" + dtTemp.Rows[0]["UID"].ToString();
                Session["LoginUserGrp"] = dtTemp.Rows[0]["Group_Name"].ToString();

                //Response.Redirect("HomePage.aspx");
            }

            if (Session["LoginUserGrp"].ToString().ToUpper() == "ADMIN")
            {
                Session["MasterFile"] = "/MasterHome.master";
            }
            else if (Session["LoginUserGrp"].ToString().ToUpper() == "USER")
            {
                Session["MasterFile"] = "/MasterHomeUser.master";
            }
            else if (Session["LoginUserGrp"].ToString().ToUpper() == "REPORT")
            {
                Session["MasterFile"] = "/MasterHomeReport.master";
            }
            else if (Session["LoginUserGrp"].ToString().ToUpper() == "EMP")
            {
                Session["MasterFile"] = "/MasterHomeEmp.master";
            }
            else
            {
                Session["MasterFile"] = "/MasterHomeBlank.master";
            }

            Response.Redirect("~/HomePage.aspx");
        }
        else
        {
            LblMsg.Text = "UserName/Passwrod incorrect.";
            TxtUserId.Focus();
        }
    }
}

