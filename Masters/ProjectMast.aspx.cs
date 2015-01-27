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
using System.Activities.Statements;
using HRMSystem;

public partial class Masters_ProjectMast : System.Web.UI.Page
{
    BAL BLayer = new BAL();
    StringBuilder StrSql;
    SqlFunction SqlFunc = new SqlFunction();
    SqlCommand Cmd = new SqlCommand();
    DataTable dtTemp;
    DataSet ds = new DataSet();
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
        catch(Exception ex)
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

            ClearAll();
            //FillGrid();

            MyMenu.Items[0].Selected = true;
            MyMenu.Items[0].ImageUrl = "~/Images/ViewEnable.jpg";
            MyMultiView.ActiveViewIndex = 0; 
        }
    }

    protected void ClearAll()
    {
        try
        {
            LblMsg.Text = "";

            HidFldId.Value = "";
            
            DDLClientName.Items.Clear();
            DDLClientName.DataSource = BLayer.FillClient();
            DDLClientName.DataTextField = "ClientName";
            DDLClientName.DataValueField = "ClientId";
            DDLClientName.DataBind();
            DDLClientName.Items.Insert(0, new ListItem("---Select Client---", "0"));

            TxtPrjName.Text = "";
            TxtStartDate.Text = "";
            TxtEndDate.Text = "";

            ddlPrjStatus.SelectedIndex = 0;
            TxtRemark.Text = "";

            GridModule.DataSource = null;
            GridModule.DataBind();
        }
        catch(Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    protected void FillGrid()
    {
        try
        {
            StrSql = new StringBuilder();
            StrSql.Length = 0;
            StrSql.AppendLine("Select P.Id,C.ClientName");
            StrSql.AppendLine(",P.ProjectName");
            StrSql.AppendLine(",Case When IsNull(P.ProjctStatus,'')='D' Then 'Done'");
            StrSql.AppendLine("      When IsNull(P.ProjctStatus,'')='P' Then 'Pending'");
            StrSql.AppendLine("      When IsNull(P.ProjctStatus,'')='C' Then 'Cancle' Else '0' End As ProjctStatus");
            StrSql.AppendLine("From Project_Mast P");
            StrSql.AppendLine("Left Join Client_Mast C On C.Id=P.ClientId");
            StrSql.AppendLine("Order By P.ProjectName");

            dtTemp = new DataTable();
            dtTemp = SqlFunc.ExecuteDataTable(StrSql.ToString());

            GridPrj.DataSource = dtTemp;
            GridPrj.DataBind();
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
            if (DDLClientName.SelectedValue == "0")
            {
                //LblMsg.Text = "Select Client...";
                DDLClientName.Focus(); 
                return;
            }

            if (TxtPrjName.Text.Length == 0)
            {
                //LblMsg.Text = "Enter Project Name...";
                TxtPrjName.Focus();
                return;
            }

            StrSql = new StringBuilder();
            StrSql.Length=0;

            if (HidFldId.Value.Length == 0)
            {
                StrSql.AppendLine("Insert Into Project_Mast");
                StrSql.AppendLine("(ClientId,ProjectName");
                StrSql.AppendLine(",StartDate,EndDate");
                StrSql.AppendLine(",ProjctStatus,Remark");
                StrSql.AppendLine(",Entry_Date,Entry_Time");
                StrSql.AppendLine(",Entry_UID,UPDFLAG)");
                StrSql.AppendLine("Values");
                StrSql.AppendLine("(@ClientId,@ProjectName");
                StrSql.AppendLine(",@StartDate,@EndDate");
                StrSql.AppendLine(",@ProjctStatus,@Remark");
                StrSql.AppendLine(",GetDate(),CONVERT(Varchar,GETDATE(),108)");
                StrSql.AppendLine(",@Entry_UID,0)");
            }
            else
            {
                StrSql.AppendLine("Update Project_Mast");
                StrSql.AppendLine("Set ClientId=@ClientId");
                StrSql.AppendLine(",ProjectName=@ProjectName");
                StrSql.AppendLine(",StartDate=@StartDate");
                StrSql.AppendLine(",EndDate=@EndDate");
                StrSql.AppendLine(",ProjctStatus=@ProjctStatus");
                StrSql.AppendLine(",Remark=@Remark");
                StrSql.AppendLine(",UPDFLAG=ISNULL(UPDFLAG,0)+1");
                StrSql.AppendLine(",MEntry_Date=GETDATE()");
                StrSql.AppendLine(",MEntry_Time=CONVERT(Varchar,GETDATE(),108)");
                StrSql.AppendLine(",MEntry_UID=@Entry_UId");
                StrSql.AppendLine("Where Id=@Id");
            }

            Cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);
            if (DDLClientName.SelectedValue != "0")
            {
                Cmd.Parameters.AddWithValue("@ClientId", DDLClientName.SelectedValue);
            }
            else
            {
                Cmd.Parameters.AddWithValue("@ClientId", DBNull.Value);
            }

            Cmd.Parameters.AddWithValue("@ProjectName", TxtPrjName.Text);

            if (TxtStartDate.Text.Length != 0)
            {
                Cmd.Parameters.AddWithValue("@StartDate", ValueConvert.ConvertDate(TxtStartDate.Text));
            }
            else
            {
                Cmd.Parameters.AddWithValue("@StartDate", DBNull.Value);
            }

            if (TxtEndDate.Text.Length != 0)
            {
                Cmd.Parameters.AddWithValue("@EndDate", ValueConvert.ConvertDate(TxtEndDate.Text));
            }
            else
            {
                Cmd.Parameters.AddWithValue("@EndDate", DBNull.Value);
            }

            if (ddlPrjStatus.SelectedValue != "0")
            {
                Cmd.Parameters.AddWithValue("@ProjctStatus", ddlPrjStatus.SelectedValue);
            }
            else
            {
                Cmd.Parameters.AddWithValue("@ProjctStatus", DBNull.Value);
            }
            Cmd.Parameters.AddWithValue("@Remark", TxtRemark.Text);
            Cmd.Parameters.AddWithValue("@Entry_UID", HidFldUID.Value.ToString());
            if (HidFldId.Value.Length == 0)
            {
                SqlFunc.ExecuteNonQuery(Cmd);
                LblMsg.Text="Entry insert successfully...";
            }
            else
            {
                Cmd.Parameters.AddWithValue("@Id",int.Parse(HidFldId.Value));
                SqlFunc.ExecuteNonQuery(Cmd);
                LblMsg.Text = "Entry update successfully...";
            }

            FillGrid();
            ClearAll();
            MyMenu.Items[0].Selected = true;
            MyMenu.Items[0].ImageUrl = "~/Images/ViewEnable.jpg";
            MyMenu.Items[1].ImageUrl = "~/Images/NewOrEditDisable.jpg";
            MyMultiView.ActiveViewIndex = 0;            
        }
        catch(Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }
    protected void BtnNew_Click(object sender, EventArgs e)
    {
        try
        {
            ClearAll();
            DDLClientName.Focus();
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

    protected void btnPrjDelete_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton btndetails = sender as ImageButton;
        GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
        Session["Id"] = GridPrj.DataKeys[gvrow.RowIndex].Value.ToString();       
        lblUser.Text = "Are you sure you want to delete this entry ? ";
        ModalPopupExtender1.Show();
    }

    protected void btnYes_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //getting userid of particular row
            int prjid = Convert.ToInt32(Session["Id"]);
            StrSql = new StringBuilder();
            StrSql.Length = 0;
            StrSql.AppendLine("Delete From Project_Module Where ProjectId=@Id");
            Cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);
            Cmd.Parameters.AddWithValue("@Id", prjid);
            int returnval = SqlFunc.ExecuteNonQuery(Cmd);            
            //if (returnval == 1)
            //{
                StrSql = new StringBuilder();
                StrSql.Length = 0;
                StrSql.AppendLine("Delete From Project_Mast Where Id=@Id");
                Cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);
                Cmd.Parameters.AddWithValue("@Id", prjid);
                SqlFunc.ExecuteNonQuery(Cmd);

                FillGrid();
            //}            
        }
        catch(Exception ex)
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
                string confirmValue = "";
                confirmValue = Request.Form["confirm_value"];
                if (confirmValue == "Yes")
                {
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked YES !')", true);

                    StrSql = new StringBuilder();
                    StrSql.Length = 0;

                    StrSql.AppendLine("Delete From Project_Module Where ProjectId=@Id");
                    Cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);
                    Cmd.Parameters.AddWithValue("@Id", int.Parse(HidFldId.Value));
                    int returnval = SqlFunc.ExecuteNonQuery(Cmd);
                    //if (returnval == 1)
                    //{
                        StrSql = new StringBuilder();
                        StrSql.Length = 0;
                        StrSql.AppendLine("Delete From Project_Mast Where Id=@Id");
                        Cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);
                        Cmd.Parameters.AddWithValue("@Id", int.Parse(HidFldId.Value));
                        SqlFunc.ExecuteNonQuery(Cmd);

                        FillGrid();
                        LblMsg.Text = "Project name deleted successfully";

                        // Move to View Tab 
                        MyMenu.Items[0].Selected = true;
                        MyMenu.Items[0].ImageUrl = "~/Images/ViewEnable.jpg";
                        MyMultiView.ActiveViewIndex = 0;
                        MyMenu.Items[1].ImageUrl = "~/Images/NewOrEditDisable.jpg";
                    //}
                }
                else
                {
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked NO !')", true);

                }
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    protected void GridPrj_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GridPrj.PageIndex = e.NewPageIndex;
            FillGrid();
            LblMsg.Text = "";
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }
    
    protected void btnPrjSelect_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton btndetails = sender as ImageButton;
            GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
            Session["Id"] = GridPrj.DataKeys[gvrow.RowIndex].Value.ToString();

            HidFldId.Value = Session["Id"].ToString();

            StrSql = new StringBuilder();
            StrSql.Length = 0;

            StrSql.AppendLine("Select P.Id,P.ClientId,P.ProjectName");
            StrSql.AppendLine(",Convert(Varchar(10),P.StartDate,103) As StartDate");
            StrSql.AppendLine(",Convert(Varchar(10),P.EndDate,103) As EndDate");
            StrSql.AppendLine(",P.ProjctStatus");
            StrSql.AppendLine(",P.Remark");
            StrSql.AppendLine("From Project_Mast P");
            StrSql.AppendLine("Where P.Id=" + int.Parse(HidFldId.Value));
            dtTemp = new DataTable();
            dtTemp = SqlFunc.ExecuteDataTable(StrSql.ToString());

            if (dtTemp.Rows.Count == 0)
            {
                LblMsg.Text = "Data not found";
                return;
            }

            DDLClientName.Items.Clear();
            DDLClientName.DataSource = BLayer.FillClient();
            DDLClientName.DataTextField = "ClientName";
            DDLClientName.DataValueField = "ClientId";
            DDLClientName.DataBind();
            DDLClientName.Items.Insert(0, new ListItem("--Select Client--", "0"));
            if (dtTemp.Rows[0]["ClientId"].ToString() != "")
            {
                DDLClientName.SelectedValue = dtTemp.Rows[0]["ClientId"].ToString();
            }

            TxtPrjName.Text = dtTemp.Rows[0]["ProjectName"].ToString();
            TxtStartDate.Text = dtTemp.Rows[0]["StartDate"].ToString();
            TxtEndDate.Text = dtTemp.Rows[0]["EndDate"].ToString();
            if (dtTemp.Rows[0]["ProjctStatus"].ToString() != "")
            {
                ddlPrjStatus.SelectedValue = dtTemp.Rows[0]["ProjctStatus"].ToString();
            }
            else
            {
                ddlPrjStatus.SelectedIndex = 0;
            }
            TxtRemark.Text = dtTemp.Rows[0]["Remark"].ToString();

            FillModuleData();

            //Move to Edit Tab
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

    protected void FillModuleData()
    {
        try
        {
            if (int.Parse(HidFldId.Value) != 0)
            {
                StrSql = new StringBuilder();
                StrSql.Length = 0;

                StrSql.AppendLine("Select Id,ProjectId,ModuleName");
                StrSql.AppendLine(",CONVERT(Varchar(10),StartDate,103) As StartDate ");
                StrSql.AppendLine(",CONVERT(Varchar(10),EndDate,103) As EndDate ");
                StrSql.AppendLine(",Case When IsNull(ModuleStatus,'')='D' Then 'Done'");
                StrSql.AppendLine("      When IsNull(ModuleStatus,'')='P' Then 'Pending'");
                StrSql.AppendLine("      When IsNull(ModuleStatus,'')='C' Then 'Cancle' Else '0' End As ModuleStatus");
                StrSql.AppendLine(",Remark  ");
                StrSql.AppendLine("From Project_Module ");
                StrSql.AppendLine("Where ProjectId=" + int.Parse(HidFldId.Value));
                StrSql.AppendLine("Order By StartDate");

                ds = new DataSet();
                ds=SqlFunc.ExecuteDataSet(StrSql.ToString()); 

                if (ds.Tables[0].Rows.Count != 0)
                {
                    GridModule.DataSource = ds;
                    GridModule.DataBind();
                    ModulePanel.Visible = true; 
                }
                else
                {
                    ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                    GridModule.DataSource = ds;
                    GridModule.DataBind();
                    int columncount = GridModule.Rows[0].Cells.Count;
                    GridModule.Rows[0].Cells.Clear();
                    GridModule.Rows[0].Cells.Add(new TableCell());
                    GridModule.Rows[0].Cells[0].ColumnSpan = columncount;
                    GridModule.Rows[0].Cells[0].Text = "No Records Found";                 
                }
            }
        }
        catch(Exception ex)
        {
            Response.Write(ex.ToString());
        }

    }   
   
    protected void GridModule_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            GridModule.EditIndex = e.NewEditIndex;
            FillModuleData();
        }
        catch(Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    protected void GridModule_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            int Id = Convert.ToInt32(GridModule.DataKeys[e.RowIndex].Value.ToString());
            int Projectid = Convert.ToInt32(GridModule.DataKeys[e.RowIndex].Values["ProjectId"].ToString());

            TextBox TxtModName = (TextBox)GridModule.Rows[e.RowIndex].FindControl("TxtEditModName");
            TextBox TxtModStartDate = (TextBox)GridModule.Rows[e.RowIndex].FindControl("TxtEditStartDate");
            TextBox TxtModEndDate = (TextBox)GridModule.Rows[e.RowIndex].FindControl("TxtEditEndDate");
            DropDownList DDlModStat = (DropDownList)GridModule.Rows[e.RowIndex].FindControl("DDlEditModStat");
            TextBox TxtRemark = (TextBox)GridModule.Rows[e.RowIndex].FindControl("TxtEditRemark");

            StrSql = new StringBuilder();            
            StrSql.Length = 0;
            StrSql.AppendLine("Update Project_Module ");
            StrSql.AppendLine("Set StartDate='" + ValueConvert.ConvertDate(TxtModStartDate.Text) + "'");
            if (TxtModEndDate.Text.Length != 0)
            {
                StrSql.AppendLine(",EndDate='" + ValueConvert.ConvertDate(TxtModEndDate.Text) + "'");
            }
            else
            {
                StrSql.AppendLine(",EndDate=NULL");
            }
            StrSql.AppendLine(",ModuleStatus='"+DDlModStat.Text+"'");
            StrSql.AppendLine(",Remark='"+TxtRemark.Text+"'");
            StrSql.AppendLine(",UPDFLAG=ISNULL(UPDFLAG,0)+1");
            StrSql.AppendLine(",MEntry_Date=GetDate()");
            StrSql.AppendLine(",MEntry_Time=CONVERT(VarChar,GetDate(),108)");
            StrSql.AppendLine(",MEntry_UID='"+HidFldUID.Value.ToString()+"'");
            StrSql.AppendLine("Where Id="+Id);
            StrSql.AppendLine("And ProjectId="+Projectid);

            SqlFunc.ExecuteNonQuery(StrSql.ToString());
                        
            GridModule.EditIndex = -1;
            FillModuleData();
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    protected void GridModule_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("AddNew"))
            {
                TextBox TxtFtrModName = (TextBox)GridModule.FooterRow.FindControl("TxtFtrModName");
                TextBox TxtFtrStartDate = (TextBox)GridModule.FooterRow.FindControl("TxtFtrStartDate");
                TextBox TxtFtrEndDate = (TextBox)GridModule.FooterRow.FindControl("TxtFtrEndDate");
                DropDownList DDlFtrModStat = (DropDownList)GridModule.FooterRow.FindControl("DDlFtrModStat");
                TextBox TxtFtrRemark = (TextBox)GridModule.FooterRow.FindControl("TxtFtrRemark");

                StrSql = new StringBuilder();
                StrSql.Length = 0;
                StrSql.AppendLine("Insert Into Project_Module");
                StrSql.AppendLine("(ProjectId,ModuleName");
                StrSql.AppendLine(",StartDate,EndDate");
                StrSql.AppendLine(",ModuleStatus,Remark");
                StrSql.AppendLine(",Entry_Date,Entry_Time");
                StrSql.AppendLine(",Entry_UID,UPDFLAG)");
                StrSql.AppendLine("Values");
                StrSql.AppendLine("(@ProjectId,@ModuleName");
                StrSql.AppendLine(",@StartDate,@EndDate");
                StrSql.AppendLine(",@ModuleStatus,@Remark");
                StrSql.AppendLine(",GETDATE(),CONVERT(VarChar,GETDATE(),108)");
                StrSql.AppendLine(",@Entry_UID,0)");

                Cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);

                Cmd.Parameters.AddWithValue("@ProjectId", int.Parse(HidFldId.Value));
                Cmd.Parameters.AddWithValue("@ModuleName", TxtFtrModName.Text);
                if (TxtFtrStartDate.Text.Length != 0)
                {
                    Cmd.Parameters.AddWithValue("@StartDate", ValueConvert.ConvertDate(TxtFtrStartDate.Text));
                }
                else
                {
                    Cmd.Parameters.AddWithValue("@StartDate", DBNull.Value);
                }
                if (TxtFtrEndDate.Text.Length != 0)
                {
                    Cmd.Parameters.AddWithValue("@EndDate", ValueConvert.ConvertDate(TxtFtrEndDate.Text));
                }
                else 
                {
                    Cmd.Parameters.AddWithValue("@EndDate", DBNull.Value);
                }
                Cmd.Parameters.AddWithValue("@ModuleStatus", DDlFtrModStat.Text);
                Cmd.Parameters.AddWithValue("@Remark", TxtFtrRemark.Text);
                Cmd.Parameters.AddWithValue("@Entry_UID", HidFldUID.Value);

                int result = SqlFunc.ExecuteNonQuery(Cmd);       
                if (result == 1)
                {
                    FillModuleData();                    
                }
                else
                {
                    LblMsg.ForeColor = Color.Red;
                    LblMsg.Text = "Module details not inserted";
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    protected void GridModule_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            GridModule.EditIndex = -1;
            FillModuleData();
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    protected void GridModule_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int Id = Convert.ToInt32(GridModule.DataKeys[e.RowIndex].Values["Id"].ToString());
            int ProjectId = Convert.ToInt32(GridModule.DataKeys[e.RowIndex].Values["ProjectId"].ToString());

            Cmd = new SqlCommand("Delete From Project_Module Where Id=" + Id + " And ProjectId=" + ProjectId, SqlFunc.gConn);
            int result = SqlFunc.ExecuteNonQuery(Cmd);
            if (result == 1)
            {
                FillModuleData();
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    protected void GridModule_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //getting Module Name from particular row
                string ModuleName = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "ModuleName"));
                //identifying the control in gridview
                ImageButton lnkbtnresult = (ImageButton)e.Row.FindControl("imgbtnDelete");
                //raising javascript confirmationbox whenver employee clicks on link button
                if (lnkbtnresult != null)
                {
                    lnkbtnresult.Attributes.Add("onclick", "javascript:return ConfirmationBox('" + ModuleName + "')");
                }

            }
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
            StrSql.AppendLine("Select Top 1 Id From Project_Mast Order By Id");
        }
        else if (pStrNavigation.Trim().ToUpper() == "NEXT")
        {
            if (HidFldId.Value == "")
            {
                LblMsg.Text = "Data not found";
                return;
            }
            StrSql.AppendLine("Select Top 1 Id From Project_Mast Where Id > " + int.Parse(HidFldId.Value));
        }
        else if (pStrNavigation.Trim().ToUpper() == "PREV")
        {
            if (HidFldId.Value == "")
            {
                LblMsg.Text = "Data not found";
                return;
            }
            StrSql.AppendLine("Select Top 1 Id From Project_Mast Where Id < " + int.Parse(HidFldId.Value) + " Order By Id Desc");
        }
        else if (pStrNavigation.Trim().ToUpper() == "LAST")
        {
            StrSql.AppendLine("Select Top 1 Id From Project_Mast Order By Id Desc");
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

        StrSql.AppendLine("Select P.Id,P.ClientId,P.ProjectName");
        StrSql.AppendLine(",Convert(Varchar(10),P.StartDate,103) As StartDate");
        StrSql.AppendLine(",Convert(Varchar(10),P.EndDate,103) As EndDate");
        StrSql.AppendLine(",P.ProjctStatus");
        StrSql.AppendLine(",P.Remark");
        StrSql.AppendLine("From Project_Mast P");
        StrSql.AppendLine("Where P.Id=" + IntId);

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

        DDLClientName.Items.Clear();
        DDLClientName.DataSource = BLayer.FillClient();
        DDLClientName.DataTextField = "ClientName";
        DDLClientName.DataValueField = "ClientId";
        DDLClientName.DataBind();
        DDLClientName.Items.Insert(0, new ListItem("--Select Client--", "0"));
        if (dtTemp.Rows[0]["ClientId"].ToString() != "")
        {
            DDLClientName.SelectedValue = dtTemp.Rows[0]["ClientId"].ToString();
        }

        TxtPrjName.Text = dtTemp.Rows[0]["ProjectName"].ToString();
        TxtStartDate.Text = dtTemp.Rows[0]["StartDate"].ToString();
        TxtEndDate.Text = dtTemp.Rows[0]["EndDate"].ToString();
        if (dtTemp.Rows[0]["ProjctStatus"].ToString() != "")
        {
            ddlPrjStatus.SelectedValue = dtTemp.Rows[0]["ProjctStatus"].ToString();
        }
        else
        {
            ddlPrjStatus.SelectedIndex = 0;
        }
        TxtRemark.Text = dtTemp.Rows[0]["Remark"].ToString();

        FillModuleData();
    }
}