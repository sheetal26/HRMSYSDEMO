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

public partial class Masters_ClientMast : System.Web.UI.Page
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

    }

    protected void Page_PreInit(object sender, EventArgs e)
    {
        try
        {
            if (Session["MasterFile"] != null)
            {
                this.MasterPageFile = Session["MasterFile"].ToString();
            }
            else
            {
                this.MasterPageFile = "~/MasterHomeBlank.master";
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

                //FillGrid();
                ClearAll();
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

    private void FillGrid()
    {
        try
        {
            StrSql = new StringBuilder();
            StrSql.Length = 0;
            StrSql.AppendLine("Select Id,ClientName,FirmName,EMailId,MobNo");
            StrSql.AppendLine("From Client_Mast");
            StrSql.AppendLine("Order By ClientName");

            dtTemp = new DataTable();
            dtTemp = SqlFunc.ExecuteDataTable(StrSql.ToString());

            GridClient.DataSource = dtTemp;
            GridClient.DataBind();

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
            LblMsg.Text = "";

            HidFldId.Value = "";
            
            TxtClientName.Text = "";
            TxtFirmName.Text = "";
            TxtDOJ.Text = "";
            TxtEMailId.Text = "";

            ddlCountry.Items.Clear();
            ddlCountry.DataSource = BLayer.FillCountry();
            ddlCountry.DataValueField = "CountryId";
            ddlCountry.DataTextField = "CountryName";
            ddlCountry.DataBind();
            ddlCountry.Items.Insert(0, new ListItem("--Select Country--", "0"));

            ddlState.Items.Clear();
            ddlState.Items.Insert(0, new ListItem("---Select State---","0"));

            ddlCity.Items.Clear();
            ddlCity.Items.Insert(0, new ListItem("---Select City---","0"));

            TxtAddress1.Text = "";
            TxtPinCode.Text = "";
            TxtMobNo.Text = "";
            TxtPhone.Text = "";

            TxtLeftDate.Text = "";

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
    protected void GridClient_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GridClient.PageIndex = e.NewPageIndex;
            FillGrid();
            LblMsg.Text = "";
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    //protected void GridClient_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    try
    //    {
    //        StrSql = new StringBuilder();
    //        StrSql.Length = 0;

    //        StrSql.AppendLine("Delete From Client_Mast Where Id=@Id ");
    //        Cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);
    //        Cmd.Parameters.AddWithValue("@Id", int.Parse(GridClient.DataKeys[e.RowIndex].Value.ToString()));
    //        SqlFunc.ExecuteNonQuery(Cmd);

    //        GridClient.EditIndex = -1;
    //        FillGrid();
    //        LblMsg.Text = "Client deleted successfully";
    //    }
    //    catch (Exception ex)
    //    {
    //        Response.Write(ex);
    //    }
    //}

    protected void btnYes_Click(object sender, ImageClickEventArgs e)
    {
        //getting userid of particular row
        int ClientId = Convert.ToInt32(Session["Id"]);

        StrSql = new StringBuilder();
        StrSql.Length = 0;
        StrSql.AppendLine("Delete From Client_Mast Where Id=@Id ");
        Cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);
        Cmd.Parameters.AddWithValue("@Id", ClientId);
        SqlFunc.ExecuteNonQuery(Cmd);
               
        FillGrid();
    }
    protected void btnClientDelete_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton btndetails = sender as ImageButton;
        GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
        Session["Id"] = GridClient.DataKeys[gvrow.RowIndex].Value.ToString();        
        lblUser.Text = "Are you sure you want to delete Details ? ";
        ModalPopupExtender1.Show();
    }

    protected void btnClientSelect_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton btndetails = sender as ImageButton;
            GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
            Session["Id"] = GridClient.DataKeys[gvrow.RowIndex].Value.ToString();

            HidFldId.Value = Session["Id"].ToString();

            StrSql = new StringBuilder();
            StrSql.Length = 0;

            StrSql.AppendLine("Select C.Id,C.ClientName,C.FirmName,C.EMailId,C.MobNo ");
            StrSql.AppendLine(",Convert(Varchar(10),C.DOJ,103) As DOJ");
            StrSql.AppendLine(",C.CountryId,C.StateId,C.CityId");
            StrSql.AppendLine(",C.Address1,C.PinCode");
            StrSql.AppendLine(",C.Phone,Convert(Varchar(10),C.LeftDate,103) As LeftDate");
            StrSql.AppendLine("From Client_Mast C");
            StrSql.AppendLine("Where C.Id=" + int.Parse(HidFldId.Value));

            dtTemp = new DataTable();
            dtTemp = SqlFunc.ExecuteDataTable(StrSql.ToString());

            if (dtTemp.Rows.Count == 0)
            {
                LblMsg.Text = "Data not found";
                return;
            }

            TxtClientName.Text = dtTemp.Rows[0]["ClientName"].ToString();
            TxtFirmName.Text = dtTemp.Rows[0]["FirmName"].ToString();
            TxtDOJ.Text = dtTemp.Rows[0]["DOJ"].ToString();
            TxtEMailId.Text = dtTemp.Rows[0]["EMailId"].ToString();

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
            TxtLeftDate.Text = dtTemp.Rows[0]["LeftDate"].ToString();

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

    //protected void GridClient_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        HidFldId.Value = GridClient.DataKeys[GridClient.SelectedRow.RowIndex].Value.ToString();

    //        StrSql = new StringBuilder();
    //        StrSql.Length = 0;

    //        StrSql.AppendLine("Select C.Id,C.ClientName,C.FirmName,C.EMailId,C.MobNo ");
    //        StrSql.AppendLine(",Convert(Varchar(10),C.DOJ,103) As DOJ");
    //        StrSql.AppendLine(",C.CountryId,C.StateId,C.CityId");
    //        StrSql.AppendLine(",C.Address1,C.PinCode");
    //        StrSql.AppendLine(",C.Phone,Convert(Varchar(10),C.LeftDate,103) As LeftDate");
    //        StrSql.AppendLine("From Client_Mast C");
    //        StrSql.AppendLine("Where C.Id=" + int.Parse(HidFldId.Value));

    //        dtTemp = new DataTable();
    //        dtTemp = SqlFunc.ExecuteDataTable(StrSql.ToString());

    //        if (dtTemp.Rows.Count == 0)
    //        {
    //            LblMsg.Text = "Data not found";
    //            return;
    //        }

    //        TxtClientName.Text = dtTemp.Rows[0]["ClientName"].ToString();
    //        TxtFirmName.Text = dtTemp.Rows[0]["FirmName"].ToString();
    //        TxtDOJ.Text = dtTemp.Rows[0]["DOJ"].ToString();
    //        TxtEMailId.Text = dtTemp.Rows[0]["EMailId"].ToString();

    //        ddlCountry.Items.Clear();
    //        ddlCountry.DataSource = BLayer.FillCountry();
    //        ddlCountry.DataValueField = "CountryId";
    //        ddlCountry.DataTextField = "CountryName";
    //        ddlCountry.DataBind();
    //        ddlCountry.Items.Insert(0, new ListItem("--Select Country--", "0"));
    //        if (dtTemp.Rows[0]["CountryId"].ToString() != "")
    //        {
    //            ddlCountry.SelectedValue = dtTemp.Rows[0]["CountryId"].ToString();
    //        }

    //        ddlState.Items.Clear();
    //        ddlState.DataSource = BLayer.FillState(int.Parse(ddlCountry.SelectedValue));
    //        ddlState.DataValueField = "StateId";
    //        ddlState.DataTextField = "StateName";
    //        ddlState.DataBind();
    //        ddlState.Items.Insert(0, new ListItem("--Select State--", "0"));
    //        if (dtTemp.Rows[0]["StateID"].ToString() != "")
    //        {
    //            ddlState.SelectedValue = dtTemp.Rows[0]["StateID"].ToString();
    //        }

    //        ddlCity.Items.Clear();
    //        ddlCity.DataSource = BLayer.FillCity(int.Parse(ddlState.SelectedValue));
    //        ddlCity.DataValueField = "CityId";
    //        ddlCity.DataTextField = "CityName";
    //        ddlCity.DataBind();
    //        ddlCity.Items.Insert(0, new ListItem("--Select City--", "0"));
    //        if (dtTemp.Rows[0]["CityId"].ToString() != "")
    //        {
    //            ddlCity.SelectedValue = dtTemp.Rows[0]["CityId"].ToString();
    //        }

    //        TxtAddress1.Text = dtTemp.Rows[0]["Address1"].ToString();
    //        TxtPinCode.Text = dtTemp.Rows[0]["PinCode"].ToString();
    //        TxtMobNo.Text = dtTemp.Rows[0]["MobNo"].ToString();
    //        TxtPhone.Text = dtTemp.Rows[0]["Phone"].ToString();
    //        TxtLeftDate.Text = dtTemp.Rows[0]["LeftDate"].ToString();

    //        // Move to Edit Tab            

    //        MyMenu.Items[0].ImageUrl = "~/Images/ViewDisable.jpg";
    //        MyMenu.Items[1].ImageUrl = "~/Images/NewOrEditEnable.jpg";
    //        MyMenu.Items[1].Selected = true;
    //        MyMultiView.ActiveViewIndex = 1; 
    //    }
    //    catch (Exception ex)
    //    {
    //        Response.Write(ex.ToString());
    //    }
    //}

    protected void BtnNew_Click(object sender, EventArgs e)
    {
        try
        {
            ClearAll();
            TxtClientName.Focus();
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
                String confirmValue = Request.Form["confirm_value"];
                if (confirmValue == "Yes")
                {
                    //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked YES !')", true);

                    StrSql = new StringBuilder();
                    StrSql.Length = 0;

                    StrSql.AppendLine("Delete From Client_Mast Where Id=@Id ");
                    Cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);
                    Cmd.Parameters.AddWithValue("@Id", int.Parse(HidFldId.Value));
                    SqlFunc.ExecuteNonQuery(Cmd);

                    FillGrid();
                    LblMsg.Text = "Client deleted successfully";

                    // Move to View Tab 
                    MyMenu.Items[0].Selected = true;
                    MyMenu.Items[0].ImageUrl = "~/Images/ViewEnable.jpg";
                    MyMultiView.ActiveViewIndex = 0;
                    MyMenu.Items[1].ImageUrl = "~/Images/NewOrEditDisable.jpg";
                }
                else
                {
                    //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked NO !')", true);
                }                
            }
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
            if (TxtClientName.Text.Length == 0)
            {
                //LblMsg.Text = "Client Name Is Blank, Enter Valid Name....";
                TxtClientName.Focus();
                return;
            }

            if (TxtFirmName.Text.Length == 0)
            {
                //LblMsg.Text = "Firm Name Is Blank, Enter Valid Name....";
                TxtFirmName.Focus();
                return;
            }

            if (TxtDOJ.Text.Length == 0)
            {
                //LblMsg.Text = "Date Of Join Is Blank, Enter Valid Join Date....";
                TxtDOJ.Focus();
                return;
            }

            if (TxtEMailId.Text.Length == 0)
            {
                //LblMsg.Text = "EMail Id Is Blank, Enter Valid EMail Id....";
                TxtEMailId.Focus();
                return;
            }

            if (TxtMobNo.Text.Length == 0)
            {
                //LblMsg.Text = "Mob.No Is Blank, Enter Valid Mob.No....";
                TxtMobNo.Focus();
                return;
            }

            StrSql = new StringBuilder();
            StrSql.Length = 0;


            if (HidFldId.Value.Length == 0)
            {
                StrSql.AppendLine("Insert Into Client_Mast");
                StrSql.AppendLine("(ClientName,FirmName");
                StrSql.AppendLine(",DOJ");
                StrSql.AppendLine(",EMailId");
                StrSql.AppendLine(",CountryId,StateId,CityId");
                StrSql.AppendLine(",Address1,PinCode");
                StrSql.AppendLine(",MobNo,Phone");
                StrSql.AppendLine(",LeftDate");
                StrSql.AppendLine(",Entry_Date,Entry_Time");
                StrSql.AppendLine(",Entry_UID,UPDFLAG");
                StrSql.AppendLine(")");

                StrSql.AppendLine("Values(@ClientName,@FirmName");
                StrSql.AppendLine(",@DOJ");
                StrSql.AppendLine(",@EMailId");
                StrSql.AppendLine(",@CountryId,@StateId,@CityId");
                StrSql.AppendLine(",@Address1,@PinCode");
                StrSql.AppendLine(",@MobNo,@Phone");
                StrSql.AppendLine(",@LeftDate");
                StrSql.AppendLine(",GetDate(),Convert(VarChar,GetDate(),108)");
                StrSql.AppendLine(",@Entry_UID,0)");
            }
            else
            {
                StrSql.AppendLine("Update Client_Mast");
                StrSql.AppendLine("Set ClientName=@ClientName,FirmName=@FirmName");
                StrSql.AppendLine(",DOJ=@DOJ");
                StrSql.AppendLine(",EMailId=@EMailId");
                StrSql.AppendLine(",CountryId=@CountryId,StateId=@StateId,CityId=@CityId");
                StrSql.AppendLine(",Address1=@Address1,PinCode=@PinCode");
                StrSql.AppendLine(",MobNo=@MobNo,Phone=@Phone");
                StrSql.AppendLine(",LeftDate=@LeftDate");
                StrSql.AppendLine(",MEntry_Date=GetDate(),MEntry_Time=Convert(Varchar,GetDate(),108)");
                StrSql.AppendLine(",MEntry_UID=@Entry_UID,UPDFLAG=IsNull(UPDFlag,0)+1");
                StrSql.AppendLine("Where Id=@Id");
            }

            Cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);

            Cmd.Parameters.AddWithValue("@ClientName", TxtClientName.Text.Trim());
            Cmd.Parameters.AddWithValue("@FirmName", TxtFirmName.Text.Trim());

            Cmd.Parameters.AddWithValue("@DOJ", ValueConvert.ConvertDate(TxtDOJ.Text));
            Cmd.Parameters.AddWithValue("@EMailId", TxtEMailId.Text.Trim());

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
            if (TxtLeftDate.Text.Length != 0)
            {
                Cmd.Parameters.AddWithValue("@LeftDate", ValueConvert.ConvertDate(TxtLeftDate.Text));
            }
            else
            {
                Cmd.Parameters.AddWithValue("@LeftDate", DBNull.Value);
            }

            Cmd.Parameters.AddWithValue("@Entry_UID", HidFldUID.Value.ToString());

            if (HidFldId.Value.Length == 0)
            {
                SqlFunc.ExecuteNonQuery(Cmd);
                LblMsg.Text = "Client added successfully";
            }
            else
            {
                Cmd.Parameters.AddWithValue("@ID", HidFldId.Value.ToString());

                SqlFunc.ExecuteNonQuery(Cmd);
                LblMsg.Text = "Client updated successfully";
            }
            
            FillGrid();

            ClearAll();

            // Move to View Tab 
            MyMenu.Items[0].Selected = true;
            MyMenu.Items[0].ImageUrl = "~/Images/ViewEnable.jpg";
            MyMultiView.ActiveViewIndex = 0;
            MyMenu.Items[1].ImageUrl = "~/Images/NewOrEditDisable.jpg";
        }
        catch (Exception ex)
        {
            throw(ex);
        }
    }

    protected void BtnView_Click(object sender, EventArgs e)
    {
        FillGrid();
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
            StrSql.AppendLine("Select Top 1 Id From Client_Mast Order By Id");
        }
        else if (pStrNavigation.Trim().ToUpper() == "NEXT")
        {
            if (HidFldId.Value == "")
            {
                LblMsg.Text = "Data not found";
                return;
            }
            StrSql.AppendLine("Select Top 1 Id From Client_Mast Where Id > " + int.Parse(HidFldId.Value));
        }
        else if (pStrNavigation.Trim().ToUpper() == "PREV")
        {
            if (HidFldId.Value == "")
            {
                LblMsg.Text = "Data not found";
                return;
            }
            StrSql.AppendLine("Select Top 1 Id From Client_Mast Where Id < " + int.Parse(HidFldId.Value) + " Order By Id Desc");
        }
        else if (pStrNavigation.Trim().ToUpper() == "LAST")
        {
            StrSql.AppendLine("Select Top 1 Id From Client_Mast Order By Id Desc");
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

        StrSql.AppendLine("Select C.Id,C.ClientName,C.FirmName,C.EMailId,C.MobNo ");
        StrSql.AppendLine(",Convert(Varchar(10),C.DOJ,103) As DOJ");
        StrSql.AppendLine(",C.CountryId,C.StateId,C.CityId");
        StrSql.AppendLine(",C.Address1,C.PinCode");
        StrSql.AppendLine(",C.Phone,Convert(Varchar(10),C.LeftDate,103) As LeftDate");
        StrSql.AppendLine("From Client_Mast C");
        StrSql.AppendLine("Where C.Id=" + IntId);

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

        TxtClientName.Text = dtTemp.Rows[0]["ClientName"].ToString();
        TxtFirmName.Text = dtTemp.Rows[0]["FirmName"].ToString();
        TxtDOJ.Text = dtTemp.Rows[0]["DOJ"].ToString();
        TxtEMailId.Text = dtTemp.Rows[0]["EMailId"].ToString();

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
        TxtLeftDate.Text = dtTemp.Rows[0]["LeftDate"].ToString();

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

}