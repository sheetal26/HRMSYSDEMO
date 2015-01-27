using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text; 

public partial class Masters_Designation : System.Web.UI.Page
{   
    BAL BLayer = new BAL();
    StringBuilder StrSql;    
    SqlFunction SqlFunc = new SqlFunction();
    SqlCommand cmd =new SqlCommand();
    DataTable dtTemp;
    DataSet dtSet =new DataSet();

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

            ddlDept.DataSource= BLayer.FillDept();
            ddlDept.DataValueField = "DeptId";
            ddlDept.DataTextField = "DeptName";
            ddlDept.DataBind();

            //FillGrid();
            LblMsg.Text = "";
        }
    }

    private void FillGrid()
    {
        try
        {
            StrSql=new StringBuilder();
            StrSql.Length = 0;
            StrSql.AppendLine("Select Desig_Mast.Id,Desig_Mast.DesigName");
            StrSql.AppendLine(",Desig_Mast.Dept_Id As DeptId,Dept_Mast.DeptName");
            StrSql.AppendLine("From Desig_Mast");
            StrSql.AppendLine("Inner Join Dept_Mast On Desig_Mast.Dept_Id=Dept_Mast.Id");
            StrSql.AppendLine("Order By Desig_Mast.DesigName");

            dtTemp=new DataTable();
            dtTemp=SqlFunc.ExecuteDataTable(StrSql.ToString());

            GridDesig.DataSource =dtTemp;
            GridDesig.DataBind();
        }
        catch(Exception ex)
        {
            Response.Write(ex.ToString());
        }        
    }

    protected void GridDesig_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GridDesig.PageIndex = e.NewPageIndex;
            FillGrid();
            LblMsg.Text = "";
        }
        catch(Exception ex)
        {
            Response.Write(ex);
        }        
    }

    protected void GridDesig_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            GridDesig.EditIndex = -1;
            FillGrid();
            LblMsg.Text = "";
        }
        catch (Exception ex)
        {
            Response.Write(ex);
        }
    }

    //protected void GridDesig_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    try
    //    {
    //        BLayer.DesigId = int.Parse(GridDesig.DataKeys[e.RowIndex].Value.ToString());

    //        StrSql = new StringBuilder();
    //        StrSql.Length = 0;
            
    //        StrSql.AppendLine("Delete From Desig_Mast Where Id=@Id");
    //        cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);
    //        cmd.Parameters.AddWithValue("@Id", BLayer.DesigId);
    //        SqlFunc.ExecuteNonQuery(cmd);

    //        GridDesig.EditIndex = -1;
    //        FillGrid();
    //        LblMsg.Text = "Designation deleted successfully....";
    //    }
    //    catch (Exception ex)
    //    {
    //        Response.Write(ex);
    //    }        
    //}

    protected void btnYes_Click(object sender, ImageClickEventArgs e)
    {
        //getting userid of particular row
        int Desigid = Convert.ToInt32(Session["Id"]);

        StrSql = new StringBuilder();
        StrSql.Length = 0;
        StrSql.AppendLine("Delete From Desig_Mast Where Id=@Id ");
        cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);
        cmd.Parameters.AddWithValue("@Id", Desigid);
        SqlFunc.ExecuteNonQuery(cmd);

        FillGrid();

    }
    protected void btnDelete_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton btndetails = sender as ImageButton;
        GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
        Session["Id"] = GridDesig.DataKeys[gvrow.RowIndex].Value.ToString();
      
        lblUser.Text = "Are you sure you want to delete Details ? ";
        ModalPopupExtender1.Show();
    }

    protected void GridDesig_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            GridDesig.EditIndex = e.NewEditIndex;
            FillGrid();
            LblMsg.Text = "";

            DropDownList ddlGrdDept = (DropDownList)GridDesig.Rows[e.NewEditIndex].FindControl("ddlGrdDept");
            ddlGrdDept.DataSource = BLayer.FillDept();
            ddlGrdDept.DataValueField = "DeptId";
            ddlGrdDept.DataTextField = "DeptName";
            ddlGrdDept.DataBind();
        }
        catch(Exception ex)
        {
            Response.Write(ex.ToString());
        }        
    }

    protected void GridDesig_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            Label LblId = (Label)GridDesig.Rows[e.RowIndex].FindControl("LblId");
            TextBox TxtDesig = (TextBox)GridDesig.Rows[e.RowIndex].FindControl("TxtDesig");
            DropDownList ddlGrdDept = (DropDownList)GridDesig.Rows[e.RowIndex].FindControl("ddlGrdDept");

            BLayer.DesigId = int.Parse(LblId.Text);
            BLayer.DesigName = TxtDesig.Text;
            BLayer.DeptId =int.Parse(ddlGrdDept.SelectedValue);

            StrSql = new StringBuilder();
            StrSql.Length = 0;            

            StrSql.AppendLine("Update Desig_Mast Set DesigName=@DesigName");
            StrSql.AppendLine(",Dept_Id=@DeptId");
            StrSql.AppendLine(",UPDFlag=IsNull(UPDFlag,0)+1");
            StrSql.AppendLine(",MEntry_Date=GetDate()");
            StrSql.AppendLine(",MEntry_Time=Convert(Varchar,GetDate(),108)");
            StrSql.AppendLine(",MEntry_UID=@UID");
            StrSql.AppendLine("Where Id=@DesigId");

            cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);
            cmd.Parameters.AddWithValue("@DesigName", BLayer.DesigName);
            cmd.Parameters.AddWithValue("@DeptId", BLayer.DeptId);
            cmd.Parameters.AddWithValue("@UID", HidFldUID.Value.ToString());
            cmd.Parameters.AddWithValue("@DesigId", BLayer.DesigId);
            SqlFunc.ExecuteNonQuery(cmd);

            GridDesig.EditIndex = -1;
            FillGrid();
            LblMsg.Text = "Designation updated successfully...."; 
        }
        catch(Exception ex)
        {
            Response.Write(ex.ToString()); 
        }        
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlDept.SelectedValue == null)
            {                
                //LblMsg.Text = "Select Department Code....";
                ddlDept.Focus();
                return;
            }

            if (TxtDesigName.Text.Length == 0)
            {                
                //LblMsg.Text = "Designation Name Is Blank, Enter Valid Designation Value....";
                TxtDesigName.Focus();
                return;
            }

            BLayer.DesigName = TxtDesigName.Text;
            BLayer.DeptId =int.Parse(ddlDept.SelectedValue);  

            StrSql = new StringBuilder();
            StrSql.Length = 0;

            StrSql.AppendLine("Insert Into Desig_Mast (DesigName,Dept_Id,Entry_Date,Entry_Time,Entry_UID,UpdFlag) ");
            StrSql.AppendLine("Values (@DesigName");
            StrSql.AppendLine(",@DeptId");
            StrSql.AppendLine(",GetDate()");
            StrSql.AppendLine(",Convert(VarChar,GetDate(),108)");
            StrSql.AppendLine(",@UID");
            StrSql.AppendLine(",0)");
            cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);
            cmd.Parameters.AddWithValue("@DesigName", BLayer.DesigName);
            cmd.Parameters.AddWithValue("@DeptId", BLayer.DeptId);
            cmd.Parameters.AddWithValue("@UID",HidFldUID.Value.ToString());
            SqlFunc.ExecuteNonQuery(cmd);

            FillGrid();

            ddlDept.SelectedIndex = -1;
            TxtDesigName.Text = "";
            ddlDept.Focus();
                        
            LblMsg.Text = "Entry Insert Successfully...";
        }
        catch(Exception ex)
        {
            Response.Write(ex);
        }        
    }
    protected void BtnView_Click(object sender, EventArgs e)
    {
        FillGrid();
    }
}