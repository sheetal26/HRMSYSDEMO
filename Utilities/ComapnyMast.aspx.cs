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

public partial class Masters_ComapnyMast : System.Web.UI.Page
{
    BAL BLayer = new BAL();
    StringBuilder StrSql;
    SqlFunction SqlFunc = new SqlFunction();
    SqlCommand Cmd = new SqlCommand();
    DataTable dtTemp;
    DataSet dtSet = new DataSet();
    Security Sec = new Security();
    //ValueConvert VC = new ValueConvert();

    protected void MyMenu_MenuItemClick(object sender, MenuEventArgs e)
    {
        try
        {
            int index = int.Parse(e.Item.Value);

            MyMultiView.ActiveViewIndex = Int32.Parse(e.Item.Value);

            MyMenu.Items[0].ImageUrl = "~/Images/ViewDisable.jpg";
            MyMenu.Items[1].ImageUrl = "~/images/NewOrEditDisable.jpg";

            switch (index)
            {
                case 0:
                    MyMenu.Items[0].ImageUrl = "~/Images/ViewEnable.jpg";
                    break;
                case 1:
                    MyMenu.Items[1].ImageUrl = "~/Images/NewOrEditEnable.jpg";
                    break;
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

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
                }

                ClearAll();

                //FillGrid();

                MyMenu.Items[0].Selected = true;
                MyMenu.Items[0].ImageUrl = "~/Images/ViewEnable.jpg";
                MyMultiView.ActiveViewIndex = 0; 
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    protected void ClearAll()
    {
        try
        {
            ddlCountry.Items.Clear();
            ddlCountry.DataSource = BLayer.FillCountry();
            ddlCountry.DataValueField = "CountryId";
            ddlCountry.DataTextField = "CountryName";
            ddlCountry.DataBind();
            ddlCountry.Items.Insert(0, new ListItem("--Select Country--", "0"));

            ddlState.Items.Clear();
            ddlState.Items.Add("---Select State---");

            ddlCity.Items.Clear();
            ddlCity.Items.Add("---Select City---");

            LblMsg.Text = "";

            HidFldId.Value = "";
            TxtCompName.Text = "";
            TxtStartDate.Text = "";
            TxtAddress1.Text = "";
            TxtPinCode.Text = "";
            TxtMobNo.Text = "";
            TxtPhone.Text = "";
            TxtEMailId.Text = "";
            TxtWebSite.Text = "";
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    private void FillGrid()
    {
        try
        {
            StrSql = new StringBuilder();
            StrSql.Length = 0;
            StrSql.AppendLine("Select CM.ID,CM.CompName");
            StrSql.AppendLine(",Convert(Varchar(10),CM.StartDate,103) As StartDate");
            StrSql.AppendLine(",CM.WebSite,CM.MobNo,S.StateName,C.CityName");
            StrSql.AppendLine("From Com_Mast CM");
            StrSql.AppendLine("Left Join City_Mast C On CM.CityId=C.Id");
            StrSql.AppendLine("Left Join State_Mast S On CM.StateId=S.Id");   
            StrSql.AppendLine("Order By CM.Id");

            dtTemp = new DataTable();
            dtTemp = SqlFunc.ExecuteDataTable(StrSql.ToString());

            GridComp.DataSource = dtTemp;
            GridComp.DataBind();
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlCity.Items.Clear();
            ddlCity.DataSource = BLayer.FillCity(int.Parse(ddlState.SelectedValue));
            ddlCity.DataValueField = "CityId";
            ddlCity.DataTextField = "CityName";
            ddlCity.DataBind();
            ddlCity.Items.Insert(0, new ListItem("--Select City--", "0"));

            ddlCity.Focus();
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }
    protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlState.Items.Clear();
            ddlState.DataSource = BLayer.FillState(int.Parse(ddlCountry.SelectedValue));
            ddlState.DataValueField = "StateId";
            ddlState.DataTextField = "StateName";
            ddlState.DataBind();
            ddlState.Items.Insert(0, new ListItem("--Select State--", "0"));

            ddlCity.Items.Clear();
            ddlCity.Items.Insert(0, new ListItem("--Select City--", "0"));

            ddlState.Focus();
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
            if (TxtCompName.Text.Length == 0)
            {
                //LblMsg.Text = "Company Name Is Blank, Enter Valid Company Name....";
                TxtCompName.Focus();
                return;
            }
            
            if (TxtStartDate.Text.Length == 0)
            {
                //LblMsg.Text = "Start Date Is Blank, Enter Valid Start Date....";
                TxtStartDate.Focus();
                return;
            }
           
            if (TxtMobNo.Text.Length == 0)
            {
                //LblMsg.Text = "Mob.No Is Blank, Enter Valid Mob.No....";
                TxtMobNo.Focus();
                return;
            }

            if (TxtEMailId.Text.Length == 0)
            {
               // LblMsg.Text = "Email Id Is Blank, Enter Valid Email Id....";
                TxtEMailId.Focus();
                return;
            }

            StrSql = new StringBuilder();
            StrSql.Length = 0;


            if (HidFldId.Value.Length == 0)
            {
                StrSql.AppendLine("Insert Into Com_Mast (CompName,StartDate");
                StrSql.AppendLine(",CountryId,StateId ");
                StrSql.AppendLine(",CityId,Address1,PinCode");
                StrSql.AppendLine(",MobNo,Phone,EMailId");
                StrSql.AppendLine(",WebSite");
                StrSql.AppendLine(",Entry_Date,Entry_Time");
                StrSql.AppendLine(",Entry_UID,UPDFLAG)");

                StrSql.AppendLine("Values(@CompName,@StartDate");
                StrSql.AppendLine(",@CountryId,@StateId ");
                StrSql.AppendLine(",@CityId,@Address1,@PinCode");
                StrSql.AppendLine(",@MobNo,@Phone,@EMailId");
                StrSql.AppendLine(",@WebSite");
                StrSql.AppendLine(",GetDate(),Convert(VarChar,GetDate(),108)");
                StrSql.AppendLine(",@Entry_UID,0)");
            }
            else
            {
                StrSql.AppendLine("Update Com_Mast Set CompName=@CompName,StartDate=@StartDate");
                StrSql.AppendLine(",CountryId=@CountryId,StateId=@StateId ");
                StrSql.AppendLine(",CityId=@CityId,Address1=@Address1,PinCode=@PinCode");
                StrSql.AppendLine(",MobNo=@MobNo,Phone=@Phone,EMailId=@EMailId");
                StrSql.AppendLine(",WebSite=@WebSite");
                StrSql.AppendLine(",MEntry_Date=GetDate(),MEntry_Time=Convert(Varchar,GetDate(),108)");
                StrSql.AppendLine(",MEntry_UID=@Entry_UID,UPDFLAG=IsNull(UPDFlag,0)+1");
                StrSql.AppendLine("Where Id=@Id");
            }

            Cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);            
            Cmd.Parameters.AddWithValue("@CompName", TxtCompName.Text.Trim());
            Cmd.Parameters.AddWithValue("@StartDate", ValueConvert.ConvertDate(TxtStartDate.Text));

            if (ddlCountry.SelectedValue != "0")
            {
                Cmd.Parameters.AddWithValue("@CountryId", ddlCountry.SelectedValue);
            }
            else
            {
                Cmd.Parameters.AddWithValue("@CountryId", DBNull.Value);
            }
            if (ddlState.SelectedValue != "0")
            {
                Cmd.Parameters.AddWithValue("@StateId", ddlState.SelectedValue);
            }
            else
            {
                Cmd.Parameters.AddWithValue("@StateId", DBNull.Value);
            }
            if (ddlCity.SelectedValue != "0")
            {
                Cmd.Parameters.AddWithValue("@CityId", ddlCity.SelectedValue);
            }
            else
            {
                Cmd.Parameters.AddWithValue("@CityId", DBNull.Value);
            }
            Cmd.Parameters.AddWithValue("@Address1", TxtAddress1.Text.Trim());
            Cmd.Parameters.AddWithValue("@PinCode", TxtPinCode.Text.Trim());
            Cmd.Parameters.AddWithValue("@MobNo", TxtMobNo.Text.Trim());
            Cmd.Parameters.AddWithValue("@Phone", TxtPhone.Text.Trim());
            Cmd.Parameters.AddWithValue("@WebSite", TxtWebSite.Text.Trim());          
            Cmd.Parameters.AddWithValue("@EMailId", TxtEMailId.Text.Trim());          
            Cmd.Parameters.AddWithValue("@Entry_UID", HidFldUID.Value.ToString());

            if (HidFldId.Value.Length == 0)
            {
                SqlFunc.ExecuteNonQuery(Cmd);
                LblMsg.Text = "Company added successfully";
            }
            else
            {
                Cmd.Parameters.AddWithValue("@ID", HidFldId.Value.ToString());

                SqlFunc.ExecuteNonQuery(Cmd);
                LblMsg.Text = "Company updated successfully";
            }

            FillGrid();

            ClearAll();

            MyMenu.Items[0].Selected = true;
            MyMenu.Items[0].ImageUrl = "~/Images/ViewEnable.jpg";
            MyMultiView.ActiveViewIndex = 0;
            MyMenu.Items[1].ImageUrl = "~/Images/NewOrEditDisable.jpg";
        }
        catch (Exception ex)
        {
            Response.Write(ex);
        } 
    }

    protected void BtnNew_Click(object sender, EventArgs e)
    {
        try
        {
            ClearAll();
            TxtCompName.Focus();
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    protected void BtnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ClearAll();
            // Move to View Tab 
            MyMenu.Items[0].Selected = true;
            MyMenu.Items[0].ImageUrl = "~/Images/ViewEnable.jpg";
            MyMultiView.ActiveViewIndex = 0;
            MyMenu.Items[1].ImageUrl = "~/Images/NewOrEditDisable.jpg";
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    protected void BtnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (HidFldId.Value != null)
            {
                StrSql = new StringBuilder();
                StrSql.Length = 0;

                StrSql.AppendLine("Delete From Com_Mast Where Id=@Id ");
                Cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);
                Cmd.Parameters.AddWithValue("@Id", int.Parse(HidFldId.Value));
                SqlFunc.ExecuteNonQuery(Cmd);

                FillGrid();
                LblMsg.Text = "Company deleted successfully";

                // Move to View Tab 
                MyMenu.Items[0].Selected = true;
                MyMenu.Items[0].ImageUrl = "~/Images/ViewEnable.jpg";
                MyMultiView.ActiveViewIndex = 0;
                MyMenu.Items[1].ImageUrl = "~/Images/NewOrEditDisable.jpg";
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    protected void GridComp_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            HidFldId.Value = GridComp.DataKeys[GridComp.SelectedRow.RowIndex].Value.ToString();

            StrSql = new StringBuilder();
            StrSql.Length = 0;

            StrSql.AppendLine("Select CompName");
            StrSql.AppendLine(",Convert(varchar(10),StartDate,103) As StartDate");
            StrSql.AppendLine(",CountryId,StateId,CityId");
            StrSql.AppendLine(",Address1,PinCode,MobNo,Phone,EMailId,WebSite");
            StrSql.AppendLine("From Com_Mast");
            StrSql.AppendLine("Where Id=" + int.Parse(HidFldId.Value));

            dtTemp = new DataTable();
            dtTemp = SqlFunc.ExecuteDataTable(StrSql.ToString());

            if (dtTemp.Rows.Count == 0)
            {
                LblMsg.Text = "Data not found";
                return;
            }
                        
            TxtCompName.Text = dtTemp.Rows[0]["CompName"].ToString();
            TxtStartDate.Text = dtTemp.Rows[0]["StartDate"].ToString();
          

            ddlCountry.Items.Clear();
            ddlCountry.DataSource = BLayer.FillCountry();
            ddlCountry.DataValueField = "CountryId";
            ddlCountry.DataTextField = "CountryName";
            ddlCountry.DataBind();
            ddlCountry.Items.Insert(0, new ListItem("--Select Country--", "0"));
            if (dtTemp.Rows[0]["CountryId"].ToString() != "")
            {
                ddlCountry.SelectedValue = dtTemp.Rows[0]["CountryId"].ToString();
            }

            ddlState.Items.Clear();
            ddlState.DataSource = BLayer.FillState(int.Parse(ddlCountry.SelectedValue));
            ddlState.DataValueField = "StateId";
            ddlState.DataTextField = "StateName";
            ddlState.DataBind();
            ddlState.Items.Insert(0, new ListItem("--Select State--", "0"));
            if (dtTemp.Rows[0]["StateID"].ToString() != "")
            {
                ddlState.SelectedValue = dtTemp.Rows[0]["StateID"].ToString();
            }

            ddlCity.Items.Clear();
            ddlCity.DataSource = BLayer.FillCity(int.Parse(ddlState.SelectedValue));
            ddlCity.DataValueField = "CityId";
            ddlCity.DataTextField = "CityName";
            ddlCity.DataBind();
            ddlCity.Items.Insert(0, new ListItem("--Select City--", "0"));
            if (dtTemp.Rows[0]["CityId"].ToString() != "")
            {
                ddlCity.SelectedValue = dtTemp.Rows[0]["CityId"].ToString();
            }

            TxtAddress1.Text = dtTemp.Rows[0]["Address1"].ToString();
            TxtPinCode.Text = dtTemp.Rows[0]["PinCode"].ToString();
            TxtMobNo.Text = dtTemp.Rows[0]["MobNo"].ToString();
            TxtPhone.Text = dtTemp.Rows[0]["Phone"].ToString();
            TxtEMailId.Text = dtTemp.Rows[0]["EMailId"].ToString();
            TxtWebSite.Text = dtTemp.Rows[0]["WebSite"].ToString();

            // Move to Edit Tab             
            MyMenu.Items[0].ImageUrl = "~/Images/ViewDisable.jpg";
            MyMenu.Items[1].ImageUrl = "~/Images/NewOrEditEnable.jpg";
            MyMenu.Items[1].Selected = true;
            MyMultiView.ActiveViewIndex = 1; 
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    protected void GridComp_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            BLayer.UserId = int.Parse(GridComp.DataKeys[e.RowIndex].Value.ToString());

            StrSql = new StringBuilder();
            StrSql.Length = 0;

            StrSql.AppendLine("Delete From Com_Mast Where Id=@Id ");
            Cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);
            Cmd.Parameters.AddWithValue("@Id", BLayer.UserId);
            SqlFunc.ExecuteNonQuery(Cmd);

            GridComp.EditIndex = -1;
            FillGrid();
            LblMsg.Text = "Company deleted successfully";
        }
        catch (Exception ex)
        {
            Response.Write(ex);
        }
    }

    protected void GridComp_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GridComp.PageIndex = e.NewPageIndex;
            FillGrid();
            LblMsg.Text = "";
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

    protected void ImgBtnFirst_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            DispRecord("FIRST");
        }
        catch
        {
            Response.Redirect("~/ErrorPage.aspx");
        }
    }
    protected void ImgBtnNext_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            DispRecord("NEXT");
        }
        catch
        {
            Response.Redirect("~/ErrorPage.aspx");
        }
    }
    protected void ImgBtnPrev_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            DispRecord("PREV");
        }
        catch
        {
            Response.Redirect("~/ErrorPage.aspx");
        }
    }
    protected void ImgBtnLast_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            DispRecord("LAST");
        }
        catch
        {
            Response.Redirect("~/ErrorPage.aspx");
        }
    }

    protected void DispRecord(string pStrNavigation)
    {
        int IntId = 0;

        StrSql = new StringBuilder();
        StrSql.Length = 0;
        if (pStrNavigation == "EXAC")
        {
            IntId = int.Parse(HidFldId.Value);
        }
        else if (pStrNavigation.Trim().ToUpper() == "FIRST")
        {
            StrSql.AppendLine("Select Top 1 Id From Com_Mast Order By Id");
        }
        else if (pStrNavigation.Trim().ToUpper() == "NEXT")
        {
            if (HidFldId.Value == "")
            {
                LblMsg.Text = "Data not found";
                return;
            }
            StrSql.AppendLine("Select Top 1 Id From Com_Mast Where Id > " + int.Parse(HidFldId.Value));
        }
        else if (pStrNavigation.Trim().ToUpper() == "PREV")
        {
            if (HidFldId.Value == "")
            {
                LblMsg.Text = "Data not found";
                return;
            }
            StrSql.AppendLine("Select Top 1 Id From Com_Mast Where Id < " + int.Parse(HidFldId.Value) + " Order By Id Desc");
        }
        else if (pStrNavigation.Trim().ToUpper() == "LAST")
        {
            StrSql.AppendLine("Select Top 1 Id From Com_Mast Order By Id Desc");
        }

        if (pStrNavigation != "EXAC")
        {
            dtTemp = new DataTable();
            dtTemp = SqlFunc.ExecuteDataTable(StrSql.ToString());
            if (dtTemp.Rows.Count != 0)
            {
                IntId = int.Parse(dtTemp.Rows[0]["Id"].ToString());
            }
        }

        StrSql = new StringBuilder();
        StrSql.Length = 0;

        StrSql.AppendLine("Select CompName");
        StrSql.AppendLine(",Convert(varchar(10),StartDate,103) As StartDate");
        StrSql.AppendLine(",CountryId,StateId,CityId");
        StrSql.AppendLine(",Address1,PinCode,MobNo,Phone,EMailId,WebSite");
        StrSql.AppendLine("From Com_Mast");
        StrSql.AppendLine("Where Id=" + IntId);

        dtTemp = new DataTable();
        dtTemp = SqlFunc.ExecuteDataTable(StrSql.ToString());

        if (dtTemp.Rows.Count == 0)
        {
            ClearAll();
            LblMsg.Text = "Data not found";
            return;
        }

        LblMsg.Text = "";

        HidFldId.Value = IntId.ToString();
        TxtCompName.Text = dtTemp.Rows[0]["CompName"].ToString();
        TxtStartDate.Text = dtTemp.Rows[0]["StartDate"].ToString();
        
        ddlCountry.Items.Clear();
        ddlCountry.DataSource = BLayer.FillCountry();
        ddlCountry.DataValueField = "CountryId";
        ddlCountry.DataTextField = "CountryName";
        ddlCountry.DataBind();
        ddlCountry.Items.Insert(0, new ListItem("--Select Country--", "0"));
        if (dtTemp.Rows[0]["CountryId"].ToString() != "")
        {
            ddlCountry.SelectedValue = dtTemp.Rows[0]["CountryId"].ToString();
        }

        ddlState.Items.Clear();
        ddlState.DataSource = BLayer.FillState(int.Parse(ddlCountry.SelectedValue));
        ddlState.DataValueField = "StateId";
        ddlState.DataTextField = "StateName";
        ddlState.DataBind();
        ddlState.Items.Insert(0, new ListItem("--Select State--", "0"));
        if (dtTemp.Rows[0]["StateID"].ToString() != "")
        {
            ddlState.SelectedValue = dtTemp.Rows[0]["StateID"].ToString();
        }

        ddlCity.Items.Clear();
        ddlCity.DataSource = BLayer.FillCity(int.Parse(ddlState.SelectedValue));
        ddlCity.DataValueField = "CityId";
        ddlCity.DataTextField = "CityName";
        ddlCity.DataBind();
        ddlCity.Items.Insert(0, new ListItem("--Select City--", "0"));
        if (dtTemp.Rows[0]["CityId"].ToString() != "")
        {
            ddlCity.SelectedValue = dtTemp.Rows[0]["CityId"].ToString();
        }

        TxtAddress1.Text = dtTemp.Rows[0]["Address1"].ToString();
        TxtPinCode.Text = dtTemp.Rows[0]["PinCode"].ToString();
        TxtMobNo.Text = dtTemp.Rows[0]["MobNo"].ToString();
        TxtPhone.Text = dtTemp.Rows[0]["Phone"].ToString();
        TxtEMailId.Text = dtTemp.Rows[0]["EMailId"].ToString();
        TxtWebSite.Text = dtTemp.Rows[0]["WebSite"].ToString();

    }
}
