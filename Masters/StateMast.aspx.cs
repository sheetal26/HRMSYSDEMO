using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text; 

public partial class Masters_StateMast : System.Web.UI.Page
{
    BAL BLayer = new BAL();
    StringBuilder StrSql;
    SqlFunction SqlFunc = new SqlFunction();
    SqlCommand Cmd = new SqlCommand();
    DataTable dtTemp;
    DataSet dtSet = new DataSet();

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

            ddlCountry.DataSource = BLayer.FillCountry();
            ddlCountry.DataValueField = "CountryId";
            ddlCountry.DataTextField="CountryName";
            ddlCountry.DataBind();

           // FillGrid();
            LblMsg.Text = "";
        }
    }

    private void FillGrid()
    {
        try
        {
            StrSql = new StringBuilder();
            StrSql.Length = 0;
            StrSql.AppendLine("Select SM.Id,SM.StateName");
            StrSql.AppendLine(",SM.CountryId As CountryId,CntM.CountryName");
            StrSql.AppendLine("From State_Mast SM");
            StrSql.AppendLine("Inner Join Country_Mast CntM On SM.CountryId=CntM.Id");
            StrSql.AppendLine("Order By SM.StateName,CntM.CountryName");

            dtTemp = new DataTable();
            dtTemp = SqlFunc.ExecuteDataTable(StrSql.ToString());

            GridState.DataSource = dtTemp;
            GridState.DataBind();
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
            if (ddlCountry.SelectedValue == null)
            {
                //LblMsg.Text = "Select Country Code....";
                ddlCountry.Focus();
                return;
            }

            if (TxtStateName.Text.Length == 0)
            {
                //LblMsg.Text = "State Name Is Blank, Enter Valid State Value....";
                TxtStateName.Focus();
                return;
            }

            BLayer.StateName = TxtStateName.Text;
            BLayer.CountryId = int.Parse(ddlCountry.SelectedValue);

            StrSql = new StringBuilder();
            StrSql.Length = 0;

            StrSql.AppendLine("Insert Into State_Mast (StateName,CountryId,Entry_Date,Entry_Time,Entry_UID,UpdFlag) ");
            StrSql.AppendLine("Values (@StateName");
            StrSql.AppendLine(",@CountryId");
            StrSql.AppendLine(",GetDate()");
            StrSql.AppendLine(",Convert(VarChar,GetDate(),108)");
            StrSql.AppendLine(",@UID");
            StrSql.AppendLine(",0)");
            Cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);
            Cmd.Parameters.AddWithValue("@StateName", BLayer.StateName);
            Cmd.Parameters.AddWithValue("@CountryId", BLayer.CountryId);
            Cmd.Parameters.AddWithValue("@UID", HidFldUID.Value.ToString());
            SqlFunc.ExecuteNonQuery(Cmd);

            FillGrid();

            ddlCountry.SelectedIndex = -1;
            TxtStateName.Text = "";
            ddlCountry.Focus();

            LblMsg.Text = "Entry Insert Successfully...";           
        }
        catch (Exception ex)
        {
            Response.Write(ex);
        }        
    }

    protected void btnYes_Click(object sender, ImageClickEventArgs e)
    {
        //getting userid of particular row
        int Stateid = Convert.ToInt32(Session["Id"]);

        StrSql = new StringBuilder();
        StrSql.Length = 0;
        StrSql.AppendLine("Delete From State_Mast Where Id=@Id ");
        Cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);
        Cmd.Parameters.AddWithValue("@Id", Stateid);
        SqlFunc.ExecuteNonQuery(Cmd);

        FillGrid();
    }
    protected void btnDelete_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton btndetails = sender as ImageButton;
        GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
        Session["Id"] = GridState.DataKeys[gvrow.RowIndex].Value.ToString();        
        lblUser.Text = "Are you sure you want to delete Details ? ";
        ModalPopupExtender1.Show();
    }

    protected void GridState_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GridState.PageIndex = e.NewPageIndex;
            FillGrid();
            LblMsg.Text = "";
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    protected void GridState_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            GridState.EditIndex = -1;
            FillGrid();
            LblMsg.Text = "";
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    //protected void GridState_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    try
    //    {
    //        BLayer.StateId = int.Parse(GridState.DataKeys[e.RowIndex].Value.ToString());
           
    //        StrSql = new StringBuilder();
    //        StrSql.Length = 0;

    //        StrSql.AppendLine("Delete From State_Mast Where Id=@Id ");
    //        Cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);
    //        Cmd.Parameters.AddWithValue("@Id", BLayer.StateId);
            
    //        SqlFunc.ExecuteNonQuery(Cmd);

    //        GridState.EditIndex = -1;
    //        FillGrid();
    //        LblMsg.Text = "State deleted successfully....";
    //    }
    //    catch (Exception ex)
    //    {
    //        Response.Write(ex);
    //    }
    //}

    protected void GridState_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            GridState.EditIndex = e.NewEditIndex;
            FillGrid();
            LblMsg.Text = "";

            DropDownList ddlGrdCountry = (DropDownList)GridState.Rows[e.NewEditIndex].FindControl("ddlGrdCountry");
            ddlGrdCountry.DataSource = BLayer.FillCountry();
            ddlGrdCountry.DataValueField = "CountryId";
            ddlGrdCountry.DataTextField = "CountryName";
            ddlGrdCountry.DataBind();
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }        
    }

    protected void GridState_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            Label LblId = (Label)GridState.Rows[e.RowIndex].FindControl("LblId");
            TextBox TxtState = (TextBox)GridState.Rows[e.RowIndex].FindControl("TxtState");
            DropDownList ddlGrdCountry = (DropDownList)GridState.Rows[e.RowIndex].FindControl("ddlGrdCountry");

            BLayer.StateId = int.Parse(LblId.Text);
            BLayer.StateName = TxtState.Text;
            BLayer.CountryId = int.Parse(ddlGrdCountry.SelectedValue);

            StrSql = new StringBuilder();
            StrSql.Length = 0;
            
            StrSql.AppendLine("Update State_Mast Set StateName=@StateName");
            StrSql.AppendLine(",CountryId=@CountryId");
            StrSql.AppendLine(",UPDFlag=IsNull(UPDFlag,0)+1");
            StrSql.AppendLine(",MEntry_Date=GetDate()");
            StrSql.AppendLine(",MEntry_Time=Convert(Varchar,GetDate(),108)");
            StrSql.AppendLine(",MEntry_UID=@UID");
            StrSql.AppendLine("Where Id=@StateId");

            Cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);
            Cmd.Parameters.AddWithValue("@StateName", BLayer.StateName);
            Cmd.Parameters.AddWithValue("@CountryId", BLayer.CountryId);
            Cmd.Parameters.AddWithValue("@UID", HidFldUID.Value.ToString());
            Cmd.Parameters.AddWithValue("@StateId", BLayer.StateId);
            SqlFunc.ExecuteNonQuery(Cmd);

            GridState.EditIndex = -1;
            FillGrid();
            LblMsg.Text = "State updated successfully...";
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }
    protected void BtnView_Click(object sender, EventArgs e)
    {
        FillGrid();
    }
}