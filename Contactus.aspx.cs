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
using System.Web.Services;

public partial class Contactus : System.Web.UI.Page
{
    BAL BLayer = new BAL();
    StringBuilder StrSql;
    SqlFunction SqlFunc = new SqlFunction();
    SqlCommand Cmd = new SqlCommand();
    DataTable dtTemp;    

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
                else
                {
                    HidFldUID.Value = Session["LoginUserId"].ToString();
                    ViewState["LoginUserGroup"] = Session["LoginUserGrp"].ToString();
                    ViewState["LoginId"] = Session["LoginId"].ToString();
                }

                ClearAll();
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }           
    }
    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (TxtName.Text.Length == 0)
            {               
                TxtName.Focus();
                return;
            }                       
            if (TxtEMailId.Text.Length == 0)
            {
                //LblMsg.Text = "EMail Id Is Blank, Enter Valid EMail Id....";
                TxtEMailId.Focus();
                return;
            }

            if (TxtMobNo.Text.Length == 0)
            {
                //LblMsg.Text = "Mob.No Is Blank, Enter Valid Mob.No....";
                TxtMobNo.Focus();
                return;
            }
          
            StrSql = new StringBuilder();
            StrSql.Length = 0;


            if (HidFldId.Value.Length == 0)
            {
                StrSql.AppendLine("Insert Into ContactDet");
                StrSql.AppendLine("(Name,EMailId,WebSite,MobNo,Msg,Contact_Status");
                StrSql.AppendLine(",Entry_Date,Entry_Time,UPDFLAG");
                StrSql.AppendLine(")");
                StrSql.AppendLine("Values(@Name,@EMailId,@WebSite");
                StrSql.AppendLine(",@MobNo,@Msg,@Contact_Status");                
                StrSql.AppendLine(",GetDate(),Convert(VarChar,GetDate(),108),0");
                StrSql.AppendLine(")");

                Cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);

                Cmd.Parameters.AddWithValue("@Name", TxtName.Text.Trim());
                Cmd.Parameters.AddWithValue("@EMailId", TxtEMailId.Text.Trim());
                Cmd.Parameters.AddWithValue("@WebSite", TxtWebSite.Text.Trim());
                Cmd.Parameters.AddWithValue("@MobNo", TxtMobNo.Text.Trim());                
                Cmd.Parameters.AddWithValue("@Msg", TxtMsg.Text.Trim());
                Cmd.Parameters.AddWithValue("@Contact_Status", DdlContactStat.SelectedValue.ToString());

                SqlFunc.ExecuteNonQuery(Cmd);
                LblMsg.Text = "Entry added successfully";
            }
            else
            {
                StrSql.AppendLine("Update ContactDet");
                StrSql.AppendLine("Set Contact_Status=@Contact_Status");               
                StrSql.AppendLine(",MEntry_Date=GetDate(),MEntry_Time=Convert(Varchar,GetDate(),108)");
                StrSql.AppendLine(",MEntry_UID=@Entry_UID,UPDFLAG=IsNull(UPDFlag,0)+1");
                StrSql.AppendLine("Where Id=@Id");

                Cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);

                Cmd.Parameters.AddWithValue("@Contact_Status", DdlContactStat.SelectedValue.ToString());
                Cmd.Parameters.AddWithValue("@Entry_UID", HidFldUID.Value.ToString());

                Cmd.Parameters.AddWithValue("@ID", HidFldId.Value.ToString());

                SqlFunc.ExecuteNonQuery(Cmd);
                LblMsg.Text = "Employee updated successfully";
            }
            ClearAll();
        }
        catch (Exception ex)
        {
            Response.Write(ex);
        } 
    }

    protected void ClearAll()
    {
        try
        {
            LblMsg.Text = "";

            HidFldId.Value = "";

            TxtName.Text = "";           
            TxtEMailId.Text = "";
            TxtWebSite.Text = "";
            TxtMobNo.Text = "";
            TxtMsg.Text = "";
            DdlContactStat.SelectedIndex = 0;

            if (ViewState["LoginUserGroup"].ToString() == "EMP")
            {
                trContact.Visible = false;
            }
            else
            {
                trContact.Visible = true;
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }
}