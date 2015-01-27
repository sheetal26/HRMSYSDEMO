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

public partial class Masters_UserMast : System.Web.UI.Page
{
    BAL BLayer = new BAL();
    StringBuilder StrSql;
    SqlFunction SqlFunc = new SqlFunction();
    SqlCommand Cmd = new SqlCommand();
    DataTable dtTemp;
    DataSet dtSet = new DataSet();
    Security Sec = new Security();
    //ValueConvert VC = new ValueConvert();

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

               // FillGrid();

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
            StrSql.AppendLine("Select UM.ID,UM.UId,UM.UserName,UM.LoginName");
            StrSql.AppendLine(",UM.UserGroup,Convert(Varchar(10),UM.DOB,103) As DOB,UM.EMailId");
            StrSql.AppendLine("From User_Mast UM");
            StrSql.AppendLine("Inner Join User_Group UG On UM.UserGroup=UG.Id");
            StrSql.AppendLine("Order By UM.UId");

            dtTemp = new DataTable();
            dtTemp = SqlFunc.ExecuteDataTable(StrSql.ToString());

            GridUser.DataSource = dtTemp;
            GridUser.DataBind();
        }
        catch(Exception ex)
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
    protected void GridUser_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GridUser.PageIndex = e.NewPageIndex;
            FillGrid();
            LblMsg.Text = "";
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }
   
    protected void GridUser_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            BLayer.UserId = int.Parse(GridUser.DataKeys[e.RowIndex].Value.ToString());

            StrSql = new StringBuilder();
            StrSql.Length = 0;

            StrSql.AppendLine("Delete From User_Mast Where Id=@Id ");
            Cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);
            Cmd.Parameters.AddWithValue("@Id", BLayer.UserId);
            SqlFunc.ExecuteNonQuery(Cmd);

            GridUser.EditIndex = -1;
            FillGrid();
            LblMsg.Text = "User deleted successfully";
        }
        catch (Exception ex)
        {
            Response.Write(ex);
        }
    }
   
   
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (TxtUserId.Text.Length == 0)
            {
               // LblMsg.Text = "User Id Is Blank, Enter Valid User Id....";
                TxtUserId.Focus();
                return;
            }

            if (TxtUserName.Text.Length == 0)
            {
               // LblMsg.Text = "User Name Is Blank, Enter Valid User Name....";
                TxtUserName.Focus();
                return;
            }

            if (TxtLoginName.Text.Length == 0)
            {
                //LblMsg.Text = "Login Name Is Blank, Enter Valid Login Name....";
                TxtLoginName.Focus();
                return;
            }

            if (DDLUserGroup.SelectedValue == "0")
            {
                //LblMsg.Text = "Select User Group....";
                DDLUserGroup.Focus();
                return;
            }

            if (TxtDOB.Text.Length == 0)
            {
                //LblMsg.Text = "DOB Is Blank, Enter Valid Brith Date....";
                TxtDOB.Focus();
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

            if (TxtPassword.Text.Length == 0)
            {
                //LblMsg.Text = "Password Is Blank, Enter Valid Password....";
                TxtPassword.Focus();
                return;
            }

            if (TxtQuestion.Text.Length == 0)
            {
                //LblMsg.Text = "Question Is Blank, Enter Valid Question....";
                TxtQuestion.Focus();
                return;
            }

            if (TxtAnswer.Text.Length == 0)
            {
                //LblMsg.Text = "Answer Is Blank, Enter Valid Answer....";
                TxtAnswer.Focus();
                return;
            }
            
            StrSql = new StringBuilder();
            StrSql.Length = 0;

          
            if (HidFldId.Value.Length == 0)
            {
                StrSql.AppendLine("Insert Into User_Mast (UId,UserName,LoginName");
                StrSql.AppendLine(",UserGroup,Password ");
                StrSql.AppendLine(",Gender,EMailId,DOB");
                StrSql.AppendLine(",CountryId,StateId,CityId");
                StrSql.AppendLine(",MobNo,Phone");
                StrSql.AppendLine(",Question,Answer");
                StrSql.AppendLine(",Activation");
                StrSql.AppendLine(",Entry_Date,Entry_Time");
                StrSql.AppendLine(",Entry_UID,UPDFLAG)");

                StrSql.AppendLine("Values(@UserId,@UserName,@LoginName");
                StrSql.AppendLine(",@UserGroup,@Password ");
                StrSql.AppendLine(",@Gender,@EMailId,@DOB");
                StrSql.AppendLine(",@CountryId,@StateId,@CityId");
                StrSql.AppendLine(",@MobNo,@Phone");
                StrSql.AppendLine(",@Question,@Answer");
                StrSql.AppendLine(",@Activation");
                StrSql.AppendLine(",GetDate(),Convert(VarChar,GetDate(),108)");
                StrSql.AppendLine(",@Entry_UID,0)");               
            }
            else
            {
                StrSql.AppendLine("Update User_Mast Set UId=@UserId,UserName=@UserName,LoginName=@LoginName");
                StrSql.AppendLine(",UserGroup=@UserGroup,Password=@Password ");
                StrSql.AppendLine(",Gender=@Gender,EMailId=@EMailId,DOB=@DOB");
                StrSql.AppendLine(",CountryId=@CountryId,StateId=@StateId,CityId=@CityId");
                StrSql.AppendLine(",MobNo=@MobNo,Phone=@Phone");
                StrSql.AppendLine(",Question=@Question,Answer=@Answer");
                StrSql.AppendLine(",Activation=@Activation");
                StrSql.AppendLine(",MEntry_Date=GetDate(),MEntry_Time=Convert(Varchar,GetDate(),108)");
                StrSql.AppendLine(",MEntry_UID=@Entry_UID,UPDFLAG=IsNull(UPDFlag,0)+1");
                StrSql.AppendLine("Where Id=@Id");                
            }

            Cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);
            Cmd.Parameters.AddWithValue("@UserId", TxtUserId.Text.Trim());
            Cmd.Parameters.AddWithValue("@UserName", TxtUserName.Text.Trim());
            Cmd.Parameters.AddWithValue("@LoginName", TxtLoginName.Text.Trim());
            Cmd.Parameters.AddWithValue("@UserGroup", DDLUserGroup.SelectedValue);
            Cmd.Parameters.AddWithValue("@Password",Sec.Encrypt(TxtPassword.Text.Trim()));
            Cmd.Parameters.AddWithValue("@Gender", rblGender.SelectedValue);
            Cmd.Parameters.AddWithValue("@EMailId", TxtEMailId.Text.Trim());

            Cmd.Parameters.AddWithValue("@DOB", ValueConvert.ConvertDate(TxtDOB.Text));

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
            Cmd.Parameters.AddWithValue("@MobNo", TxtMobNo.Text.Trim());
            Cmd.Parameters.AddWithValue("@Phone", TxtPhone.Text.Trim());
            Cmd.Parameters.AddWithValue("@Question", TxtQuestion.Text.Trim());
            Cmd.Parameters.AddWithValue("@Answer", TxtAnswer.Text.Trim());
            Cmd.Parameters.AddWithValue("@Activation", 0);
            Cmd.Parameters.AddWithValue("@Entry_UID", HidFldUID.Value.ToString());

            if (HidFldId.Value.Length == 0)
            {
                SqlFunc.ExecuteNonQuery(Cmd);
                LblMsg.Text = "User added successfully";                
            }
            else
            {
                Cmd.Parameters.AddWithValue("@ID", HidFldId.Value.ToString());

                SqlFunc.ExecuteNonQuery(Cmd);
                LblMsg.Text = "User updated successfully";
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

    protected void GridUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            HidFldId.Value = GridUser.DataKeys[GridUser.SelectedRow.RowIndex].Value.ToString();

            StrSql = new StringBuilder();
            StrSql.Length = 0;

            StrSql.AppendLine("Select UId,UserName,LoginName,UserGroup,Password");
            StrSql.AppendLine(",Gender,EMailId,Convert(varchar(10),DOB,103) As DOB,CountryId,StateId,CityId");
            StrSql.AppendLine(",MobNo,Phone,Question,Answer,Activation");
            StrSql.AppendLine("From User_Mast");
            StrSql.AppendLine("Where Id=" + int.Parse(HidFldId.Value));

            dtTemp = new DataTable();
            dtTemp = SqlFunc.ExecuteDataTable(StrSql.ToString());

            if (dtTemp.Rows.Count == 0)
            {
                LblMsg.Text = "Data not found";
                return;
            }

            TxtUserId.Text = dtTemp.Rows[0]["UID"].ToString();
            TxtUserName.Text = dtTemp.Rows[0]["UserName"].ToString();
            TxtLoginName.Text = dtTemp.Rows[0]["LoginName"].ToString();
       
            DDLUserGroup.ClearSelection();
            DDLUserGroup.SelectedValue =dtTemp.Rows[0]["UserGroup"].ToString();           

            TxtPassword.Text = dtTemp.Rows[0]["Password"].ToString();        
            rblGender.SelectedValue = dtTemp.Rows[0]["Gender"].ToString();
            TxtEMailId.Text = dtTemp.Rows[0]["EMailId"].ToString();
            TxtDOB.Text = dtTemp.Rows[0]["DOB"].ToString();

            ddlCountry.ClearSelection();
            ddlCountry.DataSource = BLayer.FillCountry();
            ddlCountry.DataValueField = "CountryId";
            ddlCountry.DataTextField = "CountryName";
            ddlCountry.DataBind();
            ddlCountry.Items.Insert(0, new ListItem("--Select Country--", "0"));
            if (dtTemp.Rows[0]["CountryId"].ToString() != "")
            {
                ddlCountry.SelectedValue = dtTemp.Rows[0]["CountryId"].ToString();
            }

            ddlState.ClearSelection();
            ddlState.DataSource = BLayer.FillState(int.Parse(ddlCountry.SelectedValue));
            ddlState.DataValueField = "StateId";
            ddlState.DataTextField = "StateName";
            ddlState.DataBind();
            ddlState.Items.Insert(0, new ListItem("--Select State--", "0"));
            if (dtTemp.Rows[0]["StateID"].ToString() != "")
            {
                ddlState.SelectedValue = dtTemp.Rows[0]["StateID"].ToString();
            }

            ddlCity.ClearSelection();
            ddlCity.DataSource = BLayer.FillCity(int.Parse(ddlState.SelectedValue));
            ddlCity.DataValueField = "CityId";
            ddlCity.DataTextField = "CityName";
            ddlCity.DataBind();
            ddlCity.Items.Insert(0, new ListItem("--Select City--", "0"));
            if (dtTemp.Rows[0]["CityId"].ToString() != "")
            {
                ddlCity.SelectedValue = dtTemp.Rows[0]["CityId"].ToString();
            }

            TxtMobNo.Text = dtTemp.Rows[0]["MobNo"].ToString();
            TxtPhone.Text = dtTemp.Rows[0]["Phone"].ToString();
            TxtQuestion.Text = dtTemp.Rows[0]["Question"].ToString();
            TxtAnswer.Text = dtTemp.Rows[0]["Answer"].ToString();

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

    protected void BtnNew_Click(object sender, EventArgs e)
    {
        try
        {
            ClearAll();
            TxtUserId.Focus();
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
                BLayer.UserId = int.Parse(HidFldId.Value);

                StrSql = new StringBuilder();
                StrSql.Length = 0;

                StrSql.AppendLine("Delete From User_Mast Where Id=@Id ");
                Cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);
                Cmd.Parameters.AddWithValue("@Id", BLayer.UserId);
                SqlFunc.ExecuteNonQuery(Cmd);

                FillGrid();
                LblMsg.Text = "User deleted successfully";

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

    protected void ClearAll()
    {
        try
        {
            DDLUserGroup.Items.Clear();
            DDLUserGroup.DataSource = BLayer.FillUserGroup();
            DDLUserGroup.DataValueField = "UGroupId";
            DDLUserGroup.DataTextField = "UGroupName";
            DDLUserGroup.DataBind();
            DDLUserGroup.Items.Insert(0, new ListItem("--Select User Group--", "0"));

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

            //ddlState.Items.Clear();
            //ddlState.DataSource = BLayer.FillState(int.Parse(ddlCountry.SelectedValue));
            //ddlState.DataValueField = "StateId";
            //ddlState.DataTextField = "StateName";
            //ddlState.DataBind();
            //ddlState.Items.Insert(0, new ListItem("--Select State--", "0"));

            //ddlCity.Items.Clear();
            //ddlCity.DataSource = BLayer.FillCity(int.Parse(ddlState.SelectedValue));
            //ddlCity.DataValueField = "CityId";
            //ddlCity.DataTextField = "CityName";
            //ddlCity.DataBind();
            //ddlCity.Items.Insert(0, new ListItem("--Select City--", "0"));
                 
            LblMsg.Text = "";

            HidFldId.Value = "";
            TxtUserId.Text = ""; 
            TxtUserName.Text = "";
            TxtLoginName.Text = "";            
            TxtDOB.Text = "";
            rblGender.SelectedIndex = 0;
            TxtEMailId.Text = "";
            TxtPassword.Text = "";           
            TxtMobNo.Text = "";
            TxtPhone.Text = "";
            TxtQuestion.Text = "";
            TxtAnswer.Text = "";
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
            StrSql.AppendLine("Select Top 1 Id From User_Mast Order By Id");
        }
        else if (pStrNavigation.Trim().ToUpper() == "NEXT")
        {
            if (HidFldId.Value == "")
            {
                LblMsg.Text = "Data not found";
                return;
            }
            StrSql.AppendLine("Select Top 1 Id From User_Mast Where Id > " + int.Parse(HidFldId.Value));
        }
        else if (pStrNavigation.Trim().ToUpper() == "PREV")
        {
            if (HidFldId.Value == "")
            {
                LblMsg.Text = "Data not found";
                return;
            }
            StrSql.AppendLine("Select Top 1 Id From User_Mast Where Id < " + int.Parse(HidFldId.Value) + " Order By Id Desc");
        }
        else if (pStrNavigation.Trim().ToUpper() == "LAST")
        {
            StrSql.AppendLine("Select Top 1 Id From User_Mast Order By Id Desc");
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

        StrSql.AppendLine("Select UId,UserName,LoginName,UserGroup,Password");
        StrSql.AppendLine(",Gender,EMailId,Convert(varchar(10),DOB,103) As DOB,CountryId,StateId,CityId");
        StrSql.AppendLine(",MobNo,Phone,Question,Answer,Activation");
        StrSql.AppendLine("From User_Mast");
        StrSql.AppendLine("Where Id=" + int.Parse(HidFldId.Value));
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

        TxtUserId.Text = dtTemp.Rows[0]["UID"].ToString();
        TxtUserName.Text = dtTemp.Rows[0]["UserName"].ToString();
        TxtLoginName.Text = dtTemp.Rows[0]["LoginName"].ToString();

        DDLUserGroup.ClearSelection();
        DDLUserGroup.SelectedValue = dtTemp.Rows[0]["UserGroup"].ToString();

        TxtPassword.Text = dtTemp.Rows[0]["Password"].ToString();
        rblGender.SelectedValue = dtTemp.Rows[0]["Gender"].ToString();
        TxtEMailId.Text = dtTemp.Rows[0]["EMailId"].ToString();
        TxtDOB.Text = dtTemp.Rows[0]["DOB"].ToString();

        ddlCountry.ClearSelection();
        ddlCountry.DataSource = BLayer.FillCountry();
        ddlCountry.DataValueField = "CountryId";
        ddlCountry.DataTextField = "CountryName";
        ddlCountry.DataBind();
        ddlCountry.Items.Insert(0, new ListItem("--Select Country--", "0"));
        if (dtTemp.Rows[0]["CountryId"].ToString() != "")
        {
            ddlCountry.SelectedValue = dtTemp.Rows[0]["CountryId"].ToString();
        }

        ddlState.ClearSelection();
        ddlState.DataSource = BLayer.FillState(int.Parse(ddlCountry.SelectedValue));
        ddlState.DataValueField = "StateId";
        ddlState.DataTextField = "StateName";
        ddlState.DataBind();
        ddlState.Items.Insert(0, new ListItem("--Select State--", "0"));
        if (dtTemp.Rows[0]["StateID"].ToString() != "")
        {
            ddlState.SelectedValue = dtTemp.Rows[0]["StateID"].ToString();
        }

        ddlCity.ClearSelection();
        ddlCity.DataSource = BLayer.FillCity(int.Parse(ddlState.SelectedValue));
        ddlCity.DataValueField = "CityId";
        ddlCity.DataTextField = "CityName";
        ddlCity.DataBind();
        ddlCity.Items.Insert(0, new ListItem("--Select City--", "0"));
        if (dtTemp.Rows[0]["CityId"].ToString() != "")
        {
            ddlCity.SelectedValue = dtTemp.Rows[0]["CityId"].ToString();
        }

        TxtMobNo.Text = dtTemp.Rows[0]["MobNo"].ToString();
        TxtPhone.Text = dtTemp.Rows[0]["Phone"].ToString();
        TxtQuestion.Text = dtTemp.Rows[0]["Question"].ToString();
        TxtAnswer.Text = dtTemp.Rows[0]["Answer"].ToString();
    }
}