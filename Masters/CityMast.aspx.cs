using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Drawing; 

public partial class Masters_CityMast : System.Web.UI.Page
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
            ddlCountry.DataTextField = "CountryName";
            ddlCountry.DataBind();            
            ddlCountry.Items.Insert(0, new ListItem("--Select Country--", "0"));
           
            ddlState.Items.Insert(0, new ListItem("---Select State---","0"));
            LblMsg.Text = "";
            //FillGrid();
        }        
    }

    private void FillGrid()
    {
        try
        {
            StrSql = new StringBuilder();
            StrSql.Length = 0;
            StrSql.AppendLine("Select CM.Id,CM.CityName,CM.StateId,SM.StateName");
            StrSql.AppendLine(",SM.CountryId,CntM.CountryName");
            StrSql.AppendLine("From City_Mast CM");
            StrSql.AppendLine("Inner Join State_Mast SM On CM.StateId=SM.Id");
            StrSql.AppendLine("Inner Join Country_Mast CntM On CM.CountryId=CntM.Id");
            StrSql.AppendLine("Order By CM.CityName,SM.StateName,CntM.CountryName");

            dtTemp = new DataTable();
            dtTemp = SqlFunc.ExecuteDataTable(StrSql.ToString());

            GridCity.DataSource = dtTemp;
            GridCity.DataBind();
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {            
            if (ddlState.SelectedValue == "0")
            {
                //LblMsg.Text = "Select State Code....";
                ddlState.Focus();
                return;
            }
            
            if (ddlCountry.SelectedValue == "0")
            {
                //LblMsg.Text = "Select Country Code....";
                ddlCountry.Focus();
                return;
            }

            if (TxtCityName.Text.Length == 0)
            {
               // LblMsg.Text = "City Name Is Blank, Enter Valid State Value....";
                TxtCityName.Focus();
                return;
            }

            BLayer.CityName = TxtCityName.Text;
            BLayer.StateId = int.Parse(ddlState.SelectedValue);
            BLayer.CountryId = int.Parse(ddlCountry.SelectedValue);

            StrSql = new StringBuilder();
            StrSql.Length = 0;

            StrSql.AppendLine("Insert Into City_Mast (CityName,StateId,CountryId,Entry_Date,Entry_Time,Entry_UID,UpdFlag) ");
            StrSql.AppendLine("Values (@CityName");
            StrSql.AppendLine(",@StateId");
            StrSql.AppendLine(",@CountryId");
            StrSql.AppendLine(",GetDate()");
            StrSql.AppendLine(",Convert(VarChar,GetDate(),108)");
            StrSql.AppendLine(",@UID");
            StrSql.AppendLine(",0)");
            Cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);
            Cmd.Parameters.AddWithValue("@CityName", BLayer.CityName);
            Cmd.Parameters.AddWithValue("@StateId", BLayer.StateId);
            Cmd.Parameters.AddWithValue("@CountryId", BLayer.CountryId);
            Cmd.Parameters.AddWithValue("@UID", HidFldUID.Value.ToString());
            SqlFunc.ExecuteNonQuery(Cmd);

            FillGrid();

            TxtCityName.Text = "";
            TxtCityName.Focus();

            LblMsg.Text = "City added successfully";
        }
        catch (Exception ex)
        {
            Response.Write(ex);
        } 
    }
    protected void GridCity_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GridCity.PageIndex = e.NewPageIndex;
            FillGrid();
            LblMsg.Text = "";
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }
    protected void GridCity_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            GridCity.EditIndex = -1;
            FillGrid();
            LblMsg.Text = "";
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

   
    protected void GridCity_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            Label LblId = (Label)GridCity.Rows[e.RowIndex].FindControl("LblId");
            TextBox TxtCity = (TextBox)GridCity.Rows[e.RowIndex].FindControl("TxtCity");
            DropDownList ddlGrdState = (DropDownList)GridCity.Rows[e.RowIndex].FindControl("ddlGrdState");
            DropDownList ddlGrdCountry = (DropDownList)GridCity.Rows[e.RowIndex].FindControl("ddlGrdCountry");

            BLayer.CityId = int.Parse(LblId.Text);
            BLayer.CityName = TxtCity.Text;
            BLayer.StateId = int.Parse(ddlGrdState.SelectedValue);
            BLayer.CountryId = int.Parse(ddlGrdCountry.SelectedValue);

            StrSql = new StringBuilder();
            StrSql.Length = 0;

            StrSql.AppendLine("Update City_Mast Set CityName=@CityName");
            StrSql.AppendLine(",StateId=@StateId");
            StrSql.AppendLine(",CountryId=@CountryId");
            StrSql.AppendLine(",UPDFlag=IsNull(UPDFlag,0)+1");
            StrSql.AppendLine(",MEntry_Date=GetDate()");
            StrSql.AppendLine(",MEntry_Time=Convert(Varchar,GetDate(),108)");
            StrSql.AppendLine(",MEntry_UID=@UID");
            StrSql.AppendLine("Where Id=@CityId");

            Cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);
            Cmd.Parameters.AddWithValue("@CityName", BLayer.CityName);
            Cmd.Parameters.AddWithValue("@StateId", BLayer.StateId);
            Cmd.Parameters.AddWithValue("@CountryId", BLayer.CountryId);
            Cmd.Parameters.AddWithValue("@UID", HidFldUID.Value.ToString());
            Cmd.Parameters.AddWithValue("@CityId", BLayer.CityId);
            SqlFunc.ExecuteNonQuery(Cmd);

            GridCity.EditIndex = -1;
            FillGrid();
            LblMsg.Text = "City updated successfully";
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }            
    }
    protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlState.Items.Clear();  
        ddlState.DataSource = BLayer.FillState(int.Parse(ddlCountry.SelectedValue));
        ddlState.DataValueField = "StateId";
        ddlState.DataTextField = "StateName";
        ddlState.DataBind();
        ddlState.Items.Insert(0, new ListItem("--Select State--", "0"));

        ddlState.Focus();
    }

      
    //protected void GridCity_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    //if (e.Row.RowType == DataControlRowType.DataRow)
    //    //{
    //    //    //DropDownList ddlGrdCountry = (DropDownList)GridCity.Rows[e.Row.RowIndex].FindControl("ddlGrdCountry");
    //    //    //ddlState.DataSource = BLayer.FillState(int.Parse(ddlGrdCountry.SelectedValue));
    //    //    //ddlState.DataValueField = "StateId";
    //    //    //ddlState.DataTextField = "StateName";
    //    //    //ddlState.DataBind();
    //    //    //ddlState.Items.Insert(0, new ListItem("--Select--", "0"));
    //    //}       
    //}
    //protected void GridCity_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    ////Add CSS class on header row.
    //    //if (e.Row.RowType == DataControlRowType.Header)
    //    //    e.Row.CssClass = "header";

    //    ////Add CSS class on normal row.
    //    //if (e.Row.RowType == DataControlRowType.DataRow &&
    //    //          e.Row.RowState == DataControlRowState.Normal)
    //    //    e.Row.CssClass = "normal";

    //    ////Add CSS class on alternate row.
    //    //if (e.Row.RowType == DataControlRowType.DataRow &&
    //    //          e.Row.RowState == DataControlRowState.Alternate)
    //    //    e.Row.CssClass = "alternate";
    //}



    //protected void GridCity_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    try
    //    {
    //        BLayer.CityId = int.Parse(GridCity.DataKeys[e.RowIndex].Value.ToString());            

    //        StrSql = new StringBuilder();
    //        StrSql.Length = 0;

    //        StrSql.AppendLine("Delete From City_Mast Where Id=@Id ");
    //        Cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);
    //        Cmd.Parameters.AddWithValue("@Id", BLayer.CityId);            
    //        SqlFunc.ExecuteNonQuery(Cmd);

    //        GridCity.EditIndex = -1;
    //        FillGrid();
    //        LblMsg.Text = "City deleted successfully";
    //    }
    //    catch (Exception ex)
    //    {
    //        Response.Write(ex);
    //    }
    //}

    protected void btnYes_Click(object sender, ImageClickEventArgs e)
    {
        //getting userid of particular row
        int cityid = Convert.ToInt32(Session["Id"]);

        StrSql = new StringBuilder();
        StrSql.Length = 0;
        StrSql.AppendLine("Delete From City_Mast Where Id=@Id ");
        Cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);
        Cmd.Parameters.AddWithValue("@Id", cityid);
        SqlFunc.ExecuteNonQuery(Cmd);
        
        //lblresult.Text = Session["CityName"] + " Details deleted successfully";
        //lblresult.ForeColor = Color.Green;
        FillGrid();
       
    }
    protected void btnDelete_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton btndetails = sender as ImageButton;
        GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
        Session["Id"] = GridCity.DataKeys[gvrow.RowIndex].Value.ToString();
       // Session["CityName"] = gvrow.Cells[0].Text;
        //lblUser.Text = "Are you sure you want to delete " + Session["CityName"] + " Details?";
        lblUser.Text = "Are you sure you want to delete Details ? ";
        ModalPopupExtender1.Show();
    }

    protected void GridCity_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            GridCity.EditIndex = e.NewEditIndex;
            FillGrid();
            LblMsg.Text = "";

            DropDownList ddlGrdCountry = (DropDownList)GridCity.Rows[e.NewEditIndex].FindControl("ddlGrdCountry");
            ddlGrdCountry.DataSource = BLayer.FillCountry();
            ddlGrdCountry.DataValueField = "CountryId";
            ddlGrdCountry.DataTextField = "CountryName";
            ddlGrdCountry.DataBind();

            //DropDownList ddlGrdState = (DropDownList)GridCity.Rows[e.NewEditIndex].FindControl("ddlGrdState");
            //ddlGrdState.DataSource = BLayer.FillState();
            //ddlGrdState.DataValueField = "StateId";
            //ddlGrdState.DataTextField = "StateName";
            //ddlGrdState.DataBind();            
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    protected void ddlGrdCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList dr = (DropDownList)sender;
        GridViewRow gr = (GridViewRow)dr.NamingContainer;
        DropDownList ddlstate1 = (DropDownList)gr.FindControl("ddlGrdState");

        //DropDownList ddlGrdState = (DropDownList)sender;
        ddlstate1.Items.Clear();
        ddlstate1.DataSource = BLayer.FillState(int.Parse(dr.SelectedValue));
        ddlstate1.DataValueField = "StateId";
        ddlstate1.DataTextField = "StateName";
        ddlstate1.DataBind();
        ddlstate1.Items.Insert(0, new ListItem("--Select State--", "0"));
    }
    protected void BtnView_Click(object sender, EventArgs e)
    {
        FillGrid();
    }
}