using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterHome : System.Web.UI.MasterPage
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["LoginUserId"] == null)
        {
            LblUserName.Text = "";
            LblUserDesig.Text = "";
            Response.Redirect("~/Default.aspx");
            return;
        }
        else
        {
            LblUserName.Text = Session["LoginEMailId"].ToString();
            LblUserDesig.Text = Session["LoginEmpDesig"].ToString();
        }
    }
}
