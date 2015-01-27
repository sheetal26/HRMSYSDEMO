using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ErrorPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Exception ex = Server.GetLastError();
        if (ex != null && ex.InnerException != null)
        {
            LabelError.Text = string.Format("An error occured: {0}", ex.InnerException.Message);
        }
    }
}