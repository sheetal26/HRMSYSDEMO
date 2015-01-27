using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Data.SqlClient; 

public partial class Masters_CountryMast : System.Web.UI.Page
{
    SqlCommand Cmd = new SqlCommand();
    SqlFunction SqlFunc = new SqlFunction();
    StringBuilder StrSql;
    DataTable DtTemp = new DataTable();
    BAL BLayer = new BAL();

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

    void FillGrid()
    {
        StrSql = new StringBuilder();
        StrSql.Length = 0;
        StrSql.AppendLine("Select Id,CountryName From Country_Mast Order By CountryName");
        DtTemp = new DataTable();
        DtTemp = SqlFunc.ExecuteDataTable(StrSql.ToString());
        GridCountry.DataSource = DtTemp;
        GridCountry.DataBind(); 
    }

    protected void BtnSave_Click(object sender, EventArgs e)
    {

        if (TxtCntName.Text.Length == 0)
        {            
            //LblMsg.Text = "Country Name Is Blank, Enter Valid Country Value....";
            TxtCntName.Focus();
            return;
        }

        StrSql = new StringBuilder();
        StrSql.Length = 0;
        BLayer.CountryName = TxtCntName.Text.ToString();

        if (HidFldId.Value == "")
        {
            StrSql.AppendLine("Insert Into Country_Mast (CountryName,Entry_Date,Entry_Time,Entry_UID,UpdFlag) ");
            StrSql.AppendLine("Values ('" + BLayer.CountryName + "'");
            StrSql.AppendLine(",GetDate()");
            StrSql.AppendLine(",Convert(VarChar,GetDate(),108)");
            StrSql.AppendLine(",'" + HidFldUID.Value.ToString() + "'");
            StrSql.AppendLine(",0)");
            SqlFunc.ExecuteNonQuery(StrSql.ToString());
        }             

        FillGrid();

        TxtCntName.Text = "";
        TxtCntName.Focus();
        LblMsg.Text = "Country added successfully....";
    }

    protected void GridCountry_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridCountry.PageIndex = e.NewPageIndex;
        FillGrid();
        LblMsg.Text = "";
    }
    protected void GridCountry_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridCountry.EditIndex = -1;
        FillGrid();
        LblMsg.Text = "";
    }

    //protected void GridCountry_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    BLayer.CountryId = int.Parse(GridCountry.DataKeys[e.RowIndex].Value.ToString());
    //    StrSql = new StringBuilder(); 
    //    StrSql.Length = 0;
    //    StrSql.AppendLine("Delete From Country_Mast Where Id="+BLayer.CountryId);
    //    SqlFunc.ExecuteNonQuery(StrSql.ToString());
    //    GridCountry.EditIndex = -1;
    //    FillGrid();
    //    LblMsg.Text = "Country deleted successfully....";
    //}

    protected void btnYes_Click(object sender, ImageClickEventArgs e)
    {

        BLayer.CountryId = Convert.ToInt32(Session["Id"]);

        StrSql = new StringBuilder();
        StrSql.Length = 0;
        StrSql.AppendLine("Delete From Country_Mast Where Id=@Id ");
        Cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);
        Cmd.Parameters.AddWithValue("@Id", BLayer.CountryId);
        SqlFunc.ExecuteNonQuery(Cmd);
        FillGrid();
        LblMsg.Text = "Country deleted successfully....";

    }
    protected void btnDelete_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton btndetails = sender as ImageButton;
        GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
        Session["Id"] = GridCountry.DataKeys[gvrow.RowIndex].Value.ToString();       
        lblUser.Text = "Are you sure you want to delete Details ? ";
        ModalPopupExtender1.Show();
    }

    protected void GridCountry_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridCountry.EditIndex = e.NewEditIndex;
        FillGrid();
        LblMsg.Text = "";
    }
    protected void GridCountry_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        Label LblId = (Label)GridCountry.Rows[e.RowIndex].FindControl("LblId");
        TextBox TxtCountyName = (TextBox)GridCountry.Rows[e.RowIndex].FindControl("TxtCountryName");

        BLayer.CountryId = int.Parse(LblId.Text);
        BLayer.CountryName = TxtCountyName.Text;

        StrSql = new StringBuilder();
        StrSql.Length = 0;
        StrSql.AppendLine("Update Country_Mast Set CountryName='" + BLayer.CountryName + "'");
        StrSql.AppendLine(",UPDFlag=IsNull(UPDFlag,0)+1");
        StrSql.AppendLine(",MEntry_Date=GetDate()");
        StrSql.AppendLine(",MEntry_Time=Convert(Varchar,GetDate(),108)");
        StrSql.AppendLine(",MEntry_UID='" + HidFldUID.Value.ToString() + "'");
        StrSql.AppendLine("Where Id=" + BLayer.CountryId);

        SqlFunc.ExecuteNonQuery(StrSql.ToString());

        GridCountry.EditIndex = -1;
        FillGrid();
        LblMsg.Text = "Country updated successfully....";
    }
    protected void BtnView_Click(object sender, EventArgs e)
    {
        FillGrid();
    }
}