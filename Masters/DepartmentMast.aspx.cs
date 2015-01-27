using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Data.SqlClient; 

public partial class Masters_DepartmentMast : System.Web.UI.Page
{
    SqlCommand Cmd = new SqlCommand();
    SqlFunction SqlFunc = new SqlFunction();
    StringBuilder StrSql;    
    DataTable dtTemp = new DataTable();
    BAL Blayer = new BAL();

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
        if (TxtDeptName.Text.Length == 0)
        {
            
            //LblMsg.Text = "Department Name Is Blank, Enter Valid Department Value....";
            TxtDeptName.Focus();
            return;
        }

        StrSql =new StringBuilder();
        StrSql.Length = 0;
        Blayer.DeptName = TxtDeptName.Text.ToString();

        if (HidFldId.Value == "")
        {
            StrSql.AppendLine("Insert Into Dept_Mast (DeptName,Entry_Date,Entry_Time,Entry_UID,UpdFlag) ");
            StrSql.AppendLine("Values ('" + Blayer.DeptName + "'");
            StrSql.AppendLine(",GetDate()");
            StrSql.AppendLine(",Convert(VarChar,GetDate(),108)");
            StrSql.AppendLine(",'" + HidFldUID.Value.ToString() + "'");
            StrSql.AppendLine(",0)");

            SqlFunc.ExecuteNonQuery(StrSql.ToString());       
        }
       
        FillGrid();

        TxtDeptName.Text = "";
        TxtDeptName.Focus();
        LblMsg.Text = "Department added successfully....";
    }


    private void FillGrid()
    {
        StrSql = new StringBuilder();
        StrSql.Length = 0;
        StrSql.AppendLine("Select Id,DeptName From Dept_Mast Order By DeptName");
        dtTemp = new DataTable();
        dtTemp = SqlFunc.ExecuteDataTable(StrSql.ToString());
        GridDept.DataSource = dtTemp;
        GridDept.DataBind();
    }
    protected void GridDept_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridDept.PageIndex = e.NewPageIndex;
        FillGrid();
        LblMsg.Text = "";
    }
    //protected void GridDept_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    Blayer.DeptId = int.Parse(GridDept.DataKeys[e.RowIndex].Value.ToString());
    //    StrSql = new StringBuilder();
    //    StrSql.Length = 0;
    //    StrSql.AppendLine("Delete From Dept_Mast Where Id=" + Blayer.DeptId);
    //    SqlFunc.ExecuteNonQuery(StrSql.ToString());
    //    GridDept.EditIndex = -1;
    //    FillGrid();
    //    LblMsg.Text = "Department deleted successfully....";
    //}

    protected void btnYes_Click(object sender, ImageClickEventArgs e)
    {

        Blayer.DeptId = Convert.ToInt32(Session["Id"]);

        StrSql = new StringBuilder();
        StrSql.Length = 0;
        StrSql.AppendLine("Delete From Dept_Mast Where Id=@Id ");
        Cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);
        Cmd.Parameters.AddWithValue("@Id", Blayer.DeptId);
        SqlFunc.ExecuteNonQuery(Cmd);
        FillGrid();
        LblMsg.Text = "Department deleted successfully....";

    }
    protected void btnDelete_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton btndetails = sender as ImageButton;
        GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
        Session["Id"] = GridDept.DataKeys[gvrow.RowIndex].Value.ToString();
        lblUser.Text = "Are you sure you want to delete Details ? ";
        ModalPopupExtender1.Show();
    }

    protected void GridDept_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridDept.EditIndex = e.NewEditIndex;
        FillGrid();
        LblMsg.Text = "";
    }
    protected void GridDept_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        Label LblId = (Label)GridDept.Rows[e.RowIndex].FindControl("LblId");
        TextBox TxtDeptName = (TextBox)GridDept.Rows[e.RowIndex].FindControl("TxtDeptName");

        Blayer.DeptId =int.Parse(LblId.Text.ToString());
        Blayer.DeptName = TxtDeptName.Text.ToString();

        StrSql=new StringBuilder();
        StrSql.Length=0;        
        StrSql.AppendLine("Update Dept_Mast Set DeptName='" + Blayer.DeptName + "'");
        StrSql.AppendLine(",UPDFlag=IsNull(UPDFlag,0)+1");
        StrSql.AppendLine(",MEntry_Date=GetDate()");
        StrSql.AppendLine(",MEntry_Time=Convert(Varchar,GetDate(),108)");
        StrSql.AppendLine(",MEntry_UID='" + HidFldUID.Value.ToString() + "'");
        StrSql.AppendLine("Where Id=" + Blayer.DeptId);

        SqlFunc.ExecuteNonQuery(StrSql.ToString());

        GridDept.EditIndex = -1;
        FillGrid();
        LblMsg.Text = "Deparment updated successfully....";
    }
    protected void GridDept_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridDept.EditIndex = -1;
        FillGrid();
        LblMsg.Text = "";
    }
    protected void BtnView_Click(object sender, EventArgs e)
    {
        FillGrid();
    }
}