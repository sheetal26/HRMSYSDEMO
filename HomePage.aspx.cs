using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HomePage : System.Web.UI.Page
{    
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
        catch(Exception ex)
        {
            Response.Write(ex.ToString());  
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["LoginUserId"] != null)
            {
                TxtLoginName.Text = Session["LoginName"].ToString();
                TxtEMailId.Text = Session["LoginEMailId"].ToString();
                TxtLoginId.Text = Session["LoginUserId"].ToString();
            }
            else
            {
                Response.Redirect("~/Default.aspx");
            }
        }
        catch(Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }
    //protected void BtnLogout_Click(object sender, EventArgs e)
    //{
    //    Session["LoginUser"] = null;
    //    Response.Redirect("Login.aspx");
    //}
}