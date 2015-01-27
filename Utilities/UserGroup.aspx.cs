using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Data.SqlClient; 

public partial class Utilities_UserGroup : System.Web.UI.Page
{
    SqlFunction SqlFunc = new SqlFunction();
    StringBuilder StrSql;    
    DataTable dtTemp = new DataTable();
    BAL Blayer = new BAL();
    SqlCommand Cmd = new SqlCommand();

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
            }
            //FillGrid();
            LblMsg.Text = "";
        }
    }

    protected void BtnSave_Click(object sender, EventArgs e)
    {
        if (TxtGrpName.Text.Length == 0)
        {            
            //LblMsg.Text = "Group Name Is Blank, Enter Valid Group Value....";
            TxtGrpName.Focus();
            return;
        }

        StrSql = new StringBuilder();
        StrSql.Length = 0;
        Blayer.UserGrpName = TxtGrpName.Text.ToString();
  
        StrSql = new StringBuilder();
        StrSql.Length = 0;

        StrSql.AppendLine("Insert Into User_Group (Group_Name,Entry_Date,Entry_Time,Entry_UID,UpdFlag) ");
        StrSql.AppendLine("Values (@UserGrpName");        
        StrSql.AppendLine(",GetDate()");
        StrSql.AppendLine(",Convert(VarChar,GetDate(),108)");
        StrSql.AppendLine(",@UID");
        StrSql.AppendLine(",0)");
        Cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);
        Cmd.Parameters.AddWithValue("@UserGrpName", Blayer.UserGrpName);
        Cmd.Parameters.AddWithValue("@UID", HidFldUID.Value.ToString());
        SqlFunc.ExecuteNonQuery(Cmd);

        FillGrid();

        TxtGrpName.Text = "";
        TxtGrpName.Focus();
        LblMsg.Text = "User group added successfully.....";
    }

    private void FillGrid()
    {
        StrSql = new StringBuilder();
        StrSql.Length = 0;
        StrSql.AppendLine("Select Id,Group_Name As UGrpName From User_Group Order By Group_Name");
        dtTemp = new DataTable();
        dtTemp = SqlFunc.ExecuteDataTable(StrSql.ToString());
        GridUserGrp.DataSource = dtTemp;
        GridUserGrp.DataBind();
    }

    protected void GridUserGrp_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridUserGrp.PageIndex = e.NewPageIndex;
        FillGrid();
        LblMsg.Text = "";
    }

    //protected void GridUserGrp_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    Blayer.UserGrpId = int.Parse(GridUserGrp.DataKeys[e.RowIndex].Value.ToString());

    //    StrSql = new StringBuilder();
    //    StrSql.Length = 0;

    //    StrSql.AppendLine("Delete From User_Group Where Id=@UserGrpId");
    //    Cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);
    //    Cmd.Parameters.AddWithValue("@UserGrpId", Blayer.UserGrpId);

    //    SqlFunc.ExecuteNonQuery(Cmd);
    //    GridUserGrp.EditIndex = -1;
    //    FillGrid();
    //    LblMsg.Text = "User group deleted successfully...";
    //}

    protected void btnYes_Click(object sender, ImageClickEventArgs e)
    {

        Blayer.UserGrpId = Convert.ToInt32(Session["Id"]);

        StrSql = new StringBuilder();
        StrSql.Length = 0;
        StrSql.AppendLine("Delete From User_Group Where Id=@Id ");
        Cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);
        Cmd.Parameters.AddWithValue("@Id", Blayer.UserGrpId);
        SqlFunc.ExecuteNonQuery(Cmd);
        FillGrid();
        LblMsg.Text = "User group deleted successfully....";

    }
    protected void btnDelete_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton btndetails = sender as ImageButton;
        GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
        Session["Id"] = GridUserGrp.DataKeys[gvrow.RowIndex].Value.ToString();
        lblUser.Text = "Are you sure you want to delete Details ? ";
        ModalPopupExtender1.Show();
    }

    protected void GridUserGrp_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridUserGrp.EditIndex = e.NewEditIndex;
        FillGrid();
        LblMsg.Text = "";
    }

    protected void GridUserGrp_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        Label LblId = (Label)GridUserGrp.Rows[e.RowIndex].FindControl("LblId");
        TextBox TxtUGrpName = (TextBox)GridUserGrp.Rows[e.RowIndex].FindControl("TxtUGrpName");

        Blayer.UserGrpId = int.Parse(LblId.Text.ToString());
        Blayer.UserGrpName = TxtUGrpName.Text.ToString();

        StrSql = new StringBuilder();
        StrSql.Length = 0;

        StrSql.AppendLine("Update User_Group Set Group_Name=@UserGrpName");
        StrSql.AppendLine(",UPDFlag=IsNull(UPDFlag,0)+1");
        StrSql.AppendLine(",MEntry_Date=GetDate()");
        StrSql.AppendLine(",MEntry_Time=Convert(Varchar,GetDate(),108)");
        StrSql.AppendLine(",MEntry_UID=@UID");
        StrSql.AppendLine("Where Id=@GrpId");

        Cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);
        Cmd.Parameters.AddWithValue("@UserGrpName", Blayer.UserGrpName);        
        Cmd.Parameters.AddWithValue("@UID", HidFldUID.Value.ToString());
        Cmd.Parameters.AddWithValue("@GrpId", Blayer.UserGrpId);
        SqlFunc.ExecuteNonQuery(Cmd);

        GridUserGrp.EditIndex = -1;
        FillGrid();
        LblMsg.Text = "User group updated successfully....";
    }

    protected void GridUserGrp_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridUserGrp.EditIndex = -1;
        FillGrid();
        LblMsg.Text = "";
    }
    protected void BtnView_Click(object sender, EventArgs e)
    {
        FillGrid();
    }
}