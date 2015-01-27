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

public partial class Transaction_DRFEntry : System.Web.UI.Page
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
                ViewState["LoginUserGroup"] = Session["LoginUserGrp"].ToString();
                ViewState["LoginId"] = Session["LoginId"].ToString();
            }

            //if (ViewState["LoginUserGroup"].ToString() == "EMP")
            //{
            //    BtnRow.Visible = false; //Save,New,Cancle,Delete Button Panel               
            //}
            //else
            //{
            //    //For Only Admin Level User
            //    BtnRow.Visible = true;               
            //}

            ClearAll();

            MyMenu.Items[0].Selected = true;
            MyMenu.Items[0].ImageUrl = "~/Images/ViewEnable.jpg";
            MyMultiView.ActiveViewIndex = 0;
        }
    }

    protected void ClearAll()
    {
        try
        {
            int IntEmpId = 0;
            if (ViewState["LoginUserGroup"].ToString() == "EMP")
            {
                IntEmpId = int.Parse(ViewState["LoginId"].ToString());
                DDLEmplName.Enabled = false;
                DDLEmpView.Enabled = false;
            }
            else
            {
                DDLEmplName.Enabled = true;
                DDLEmpView.Enabled = true;
            }

            DDLEmpView.Items.Clear();
            DDLEmpView.DataSource = BLayer.FillEmp(IntEmpId,"");
            DDLEmpView.DataValueField = "EmpId";
            DDLEmpView.DataTextField = "EmpName";
            DDLEmpView.DataBind();
            if (IntEmpId == 0)
            {
                DDLEmpView.Items.Insert(0, new ListItem("--Select Employee--", "0"));
            }            
                  
            DDLEmplName.Items.Clear();
            DDLEmplName.DataSource = BLayer.FillEmp(IntEmpId,"");
            DDLEmplName.DataValueField = "EmpId";
            DDLEmplName.DataTextField = "EmpName";
            DDLEmplName.DataBind();
            if (IntEmpId == 0)
            {
                DDLEmplName.Items.Insert(0, new ListItem("--Select Employee--", "0"));
            } 
                        
            LblMsg.Text = "";
            HidFldId.Value = ""; 

            TxtDRFDate.Text = "";
            TxtInTime.Text = "";
            TxtOutTime.Text = "";
            TxtTotWrkHours.Text = "";
            
            TxtRemark.Text = "";

            GridWork.DataSource = null;
            GridWork.DataBind();
            TxtTotTime.Text = "";
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    protected void FillGrid()
    {
        try
        {
            int IntEmpId = 0;
            if (DDLEmpView.SelectedValue != "0")
            {
                IntEmpId = int.Parse(DDLEmpView.SelectedValue);
            }

            StrSql = new StringBuilder();
            StrSql.Length = 0;

            StrSql.AppendLine("Select D.ID,E.EmpName,Convert(Varchar(10),D.DRFDate,103) As DRFDate,D.InTime,D.OutTime,D.TotTime ");
            StrSql.AppendLine("From DRFH D");
            StrSql.AppendLine("Left Join Emp_Mast E On D.EmpId=E.Id");
            StrSql.AppendLine("Where 1=1 ");
            if (ViewState["LoginUserGroup"].ToString() == "EMP")
            {
                StrSql.AppendLine("And D.EmpId=" + int.Parse(ViewState["LoginId"].ToString()));
            }
            else
            {
                if (IntEmpId != 0)
                {
                    StrSql.AppendLine("And D.EmpId=" + IntEmpId);
                }
            }
            
            StrSql.AppendLine("Order By E.EmpName");

            dtTemp = new DataTable();
            dtTemp = SqlFunc.ExecuteDataTable(StrSql.ToString());

            GridDRF.DataSource = dtTemp;
            GridDRF.DataBind();
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    protected void GridDRF_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GridDRF.PageIndex = e.NewPageIndex;
            FillGrid();
            LblMsg.Text = "";
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    protected void GridWork_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            GridWork.EditIndex = -1;
            FillWorkData();
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }
    protected void GridWork_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("AddNew"))
            {
                TextBox TxtFtrStartTime = (TextBox)GridWork.FooterRow.FindControl("TxtFtrStartTime");
                TextBox TxtFtrEndTime = (TextBox)GridWork.FooterRow.FindControl("TxtFtrEndTime");
                TextBox TxtFtrTotTime = (TextBox)GridWork.FooterRow.FindControl("TxtFtrTotTime");
                DropDownList DDlFtrPrjName = (DropDownList)GridWork.FooterRow.FindControl("DDlFtrPrjName");
                DropDownList DDlFtrModName = (DropDownList)GridWork.FooterRow.FindControl("DDlFtrModName");
                TextBox TxtFtrWorkDesc = (TextBox)GridWork.FooterRow.FindControl("TxtFtrWorkDesc");                
                DropDownList DDlFtrWorkStat = (DropDownList)GridWork.FooterRow.FindControl("DDlFtrWorkStat");
                TextBox TxtFtrRemark = (TextBox)GridWork.FooterRow.FindControl("TxtFtrRemark");

                StrSql = new StringBuilder();
                StrSql.Length = 0;

                StrSql.AppendLine("Insert Into DRFDET");
                StrSql.AppendLine("(DRFId,StartTime");
                StrSql.AppendLine(",EndTime,TotTime");
                StrSql.AppendLine(",Work_Desc");
                StrSql.AppendLine(",PrjId,PrjModId");
                StrSql.AppendLine(",WorkStatus,Remark");
                StrSql.AppendLine(",Entry_Date,Entry_Time");
                StrSql.AppendLine(",Entry_UID,UPDFLAG)");
                StrSql.AppendLine("Values");
                StrSql.AppendLine("(@DRFId,@StartTime");
                StrSql.AppendLine(",@EndTime,@TotTime");
                StrSql.AppendLine(",@Work_Desc");
                StrSql.AppendLine(",@PrjId,@PrjModId");
                StrSql.AppendLine(",@WorkStatus,@Remark");
                StrSql.AppendLine(",GETDATE(),CONVERT(VarChar,GETDATE(),108)");
                StrSql.AppendLine(",@Entry_UID,0)");

                Cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);

                Cmd.Parameters.AddWithValue("@DRFId", int.Parse(HidFldId.Value));

                Cmd.Parameters.AddWithValue("@StartTime", TxtFtrStartTime.Text);
                Cmd.Parameters.AddWithValue("@EndTime", TxtFtrEndTime.Text);
                Cmd.Parameters.AddWithValue("@TotTime", TxtFtrTotTime.Text);
                Cmd.Parameters.AddWithValue("@PrjId", DDlFtrPrjName.Text);                
                Cmd.Parameters.AddWithValue("@PrjModId", DDlFtrModName.Text);
                Cmd.Parameters.AddWithValue("@Work_Desc", TxtFtrWorkDesc.Text);
                Cmd.Parameters.AddWithValue("@WorkStatus", DDlFtrWorkStat.Text);
                Cmd.Parameters.AddWithValue("@Remark", TxtFtrRemark.Text);                
                Cmd.Parameters.AddWithValue("@Entry_UID", HidFldUID.Value);

                int result = SqlFunc.ExecuteNonQuery(Cmd);
                if (result == 1)
                {
                    FillWorkData();
                }
                else
                {
                    LblMsg.ForeColor = Color.Red;
                    LblMsg.Text = "Work details not inserted";
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    protected void GridWork_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int Id = Convert.ToInt32(GridWork.DataKeys[e.RowIndex].Values["Id"].ToString());
            int DrfId = Convert.ToInt32(GridWork.DataKeys[e.RowIndex].Values["DRFID"].ToString());

            Cmd = new SqlCommand("Delete From DRFDET Where Id=" + Id + " And DRFID=" + DrfId, SqlFunc.gConn);
            int result = SqlFunc.ExecuteNonQuery(Cmd);
            if (result == 1)
            {
                FillWorkData();
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    protected void GridWork_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            GridWork.EditIndex = e.NewEditIndex;
            
            FillWorkData();
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }
    protected void GridWork_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            int Id = Convert.ToInt32(GridWork.DataKeys[e.RowIndex].Values["Id"].ToString());
            int DrfId = Convert.ToInt32(GridWork.DataKeys[e.RowIndex].Values["DRFID"].ToString());

            TextBox TxtEditStartTime = (TextBox)GridWork.Rows[e.RowIndex].FindControl("TxtEditStartTime");
            TextBox TxtEditEndTime = (TextBox)GridWork.Rows[e.RowIndex].FindControl("TxtEditEndTime");
            TextBox TxtEditTotTime = (TextBox)GridWork.Rows[e.RowIndex].FindControl("TxtEditTotTime");
            DropDownList DDlEditPrjName = (DropDownList)GridWork.Rows[e.RowIndex].FindControl("DDlEditPrjName");
            DropDownList DDlEditModName = (DropDownList)GridWork.Rows[e.RowIndex].FindControl("DDlEditModName");
            TextBox TxtEditWorkDesc = (TextBox)GridWork.Rows[e.RowIndex].FindControl("TxtEditWorkDesc");
            DropDownList DDlEditWorkStat = (DropDownList)GridWork.Rows[e.RowIndex].FindControl("DDlEditWorkStat");
            TextBox TxtEditRemark = (TextBox)GridWork.Rows[e.RowIndex].FindControl("TxtEditRemark");


            StrSql = new StringBuilder();
            StrSql.Length = 0;

            StrSql.AppendLine("Update DRFDET ");

            StrSql.AppendLine("Set StartTime='" + TxtEditStartTime.Text + "'");
            StrSql.AppendLine(",EndTime='" + TxtEditEndTime.Text + "'");
            StrSql.AppendLine(",TotTime=" + TxtEditTotTime.Text);
            StrSql.AppendLine(",PrjId=" + DDlEditPrjName.SelectedValue);
            StrSql.AppendLine(",PrjModId=" + DDlEditModName.SelectedValue);
            StrSql.AppendLine(",Work_Desc='" + TxtEditWorkDesc.Text + "'");
            StrSql.AppendLine(",WorkStatus='" + DDlEditWorkStat.SelectedValue + "'");
            StrSql.AppendLine(",Remark='" + TxtEditRemark.Text + "'");
            StrSql.AppendLine(",UPDFLAG=ISNULL(UPDFLAG,0)+1");
            StrSql.AppendLine(",MEntry_Date=GetDate()");
            StrSql.AppendLine(",MEntry_Time=CONVERT(VarChar,GetDate(),108)");
            StrSql.AppendLine(",MEntry_UID='" + HidFldUID.Value.ToString() + "'");
            StrSql.AppendLine("Where Id=" + Id);
            StrSql.AppendLine("And DRFId=" + DrfId);

            SqlFunc.ExecuteNonQuery(StrSql.ToString());

            GridWork.EditIndex = -1;
            FillWorkData();
        }
        catch (Exception ex)
        {
            //Response.Write(ex.ToString());
            throw ex;
        }
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            StrSql = new StringBuilder();
            StrSql.Length = 0;

            if (HidFldId.Value.Length == 0)
            {
                StrSql.AppendLine("Insert Into DRFH");
                StrSql.AppendLine("(DRFDate,EmpId");
                StrSql.AppendLine(",InTime,OutTime");
                StrSql.AppendLine(",TotTime,Remark");
                StrSql.AppendLine(",Entry_Date,Entry_Time");
                StrSql.AppendLine(",Entry_UID,UPDFLAG)");
                StrSql.AppendLine("Values");
                StrSql.AppendLine("(@DRFDate,@EmpId");
                StrSql.AppendLine(",@InTime,@OutTime");
                StrSql.AppendLine(",@TotTime,@Remark");
                StrSql.AppendLine(",GetDate(),CONVERT(Varchar,GETDATE(),108)");
                StrSql.AppendLine(",@Entry_UID,0)");
            }
            else
            {
                StrSql.AppendLine("Update DRFH");
                StrSql.AppendLine("Set DRFDate=@DRFDate");
                StrSql.AppendLine(",EmpId=@EmpId");
                StrSql.AppendLine(",InTime=@InTime");
                StrSql.AppendLine(",OutTime=@OutTime");
                StrSql.AppendLine(",TotTime=@TotTime");
                StrSql.AppendLine(",Remark=@Remark");
                StrSql.AppendLine(",UPDFLAG=ISNULL(UPDFLAG,0)+1");
                StrSql.AppendLine(",MEntry_Date=GETDATE()");
                StrSql.AppendLine(",MEntry_Time=CONVERT(Varchar,GETDATE(),108)");
                StrSql.AppendLine(",MEntry_UID=@Entry_UId");
                StrSql.AppendLine("Where Id=@Id");
            }

            Cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);

            Cmd.Parameters.AddWithValue("@DRFDate", ValueConvert.ConvertDate(TxtDRFDate.Text));
            Cmd.Parameters.AddWithValue("@EmpId", DDLEmplName.SelectedValue);
            Cmd.Parameters.AddWithValue("@InTime", TxtInTime.Text);
            Cmd.Parameters.AddWithValue("@OutTime", TxtOutTime.Text);
            Cmd.Parameters.AddWithValue("@TotTime", TxtTotWrkHours.Text);
            Cmd.Parameters.AddWithValue("@Remark", TxtRemark.Text);
            Cmd.Parameters.AddWithValue("@Entry_UID", HidFldUID.Value.ToString());

            if (HidFldId.Value.Length == 0)
            {
                SqlFunc.ExecuteNonQuery(Cmd);
                LblMsg.Text = "Entry insert successfully...";
            }
            else
            {
                Cmd.Parameters.AddWithValue("@Id", int.Parse(HidFldId.Value));
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
            TxtDRFDate.Focus();
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
                string confirmValue = "";
                confirmValue = Request.Form["confirm_value"];
                if (confirmValue == "Yes")
                {
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked YES !')", true);

                    StrSql = new StringBuilder();
                    StrSql.Length = 0;

                    StrSql.AppendLine("Delete From DRFDET Where DRFId=@Id");
                    Cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);
                    Cmd.Parameters.AddWithValue("@Id", int.Parse(HidFldId.Value));
                    int returnval = SqlFunc.ExecuteNonQuery(Cmd);
                   
                    StrSql = new StringBuilder();
                    StrSql.Length = 0;
                    StrSql.AppendLine("Delete From DRFH Where Id=@Id");
                    Cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);
                    Cmd.Parameters.AddWithValue("@Id", int.Parse(HidFldId.Value));
                    SqlFunc.ExecuteNonQuery(Cmd);

                    FillGrid();
                    LblMsg.Text = "DRF Entry deleted successfully";

                    // Move to View Tab 
                    MyMenu.Items[0].Selected = true;
                    MyMenu.Items[0].ImageUrl = "~/Images/ViewEnable.jpg";
                    MyMultiView.ActiveViewIndex = 0;
                    MyMenu.Items[1].ImageUrl = "~/Images/NewOrEditDisable.jpg";
                   
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

    protected void btnDRFDelete_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton btndetails = sender as ImageButton;
        GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
        Session["Id"] = GridDRF.DataKeys[gvrow.RowIndex].Value.ToString();
        lblUser.Text = "Are you sure you want to delete this entry ? ";
        ModalPopupExtender1.Show();
    }

    protected void btnYes_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //getting userid of particular row
            int Drfid = Convert.ToInt32(Session["Id"]);
            StrSql = new StringBuilder();
            StrSql.Length = 0;
            StrSql.AppendLine("Delete From DRFDET Where DRFId=@Id");
            Cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);
            Cmd.Parameters.AddWithValue("@Id", Drfid);
            int returnval = SqlFunc.ExecuteNonQuery(Cmd);
           
            StrSql = new StringBuilder();
            StrSql.Length = 0;
            StrSql.AppendLine("Delete From DRFH Where Id=@Id");
            Cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);
            Cmd.Parameters.AddWithValue("@Id", Drfid);
            SqlFunc.ExecuteNonQuery(Cmd);

            FillGrid();
                
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    protected void btnDRFSelect_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton btndetails = sender as ImageButton;
            GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
            Session["Id"] = GridDRF.DataKeys[gvrow.RowIndex].Value.ToString();

            HidFldId.Value = Session["Id"].ToString();

            StrSql = new StringBuilder();
            StrSql.Length = 0;

            StrSql.AppendLine("Select H.Id,H.EmpId,Convert(Varchar(10),H.DRFDate,103) As DRFDate");
            StrSql.AppendLine(",H.InTime,H.OutTime,H.TotTime,H.Remark ");
            StrSql.AppendLine("From DRFH H");
            StrSql.AppendLine("Where H.Id=" + int.Parse(HidFldId.Value));
            dtTemp = new DataTable();
            dtTemp = SqlFunc.ExecuteDataTable(StrSql.ToString());

            if (dtTemp.Rows.Count == 0)
            {
                LblMsg.Text = "Data not found";
                return;
            }

            int IntEmpId = 0;
            if (ViewState["LoginUserGroup"].ToString() == "EMP")
            {
                IntEmpId = int.Parse(ViewState["LoginId"].ToString());
                DDLEmplName.Enabled = false;
            }
            else
            {                
                if (dtTemp.Rows[0]["EmpId"].ToString() != "")
                {
                    IntEmpId = int.Parse(dtTemp.Rows[0]["EmpId"].ToString());
                }
                DDLEmplName.Enabled = true;
            }

            DDLEmplName.Items.Clear();
            DDLEmplName.DataSource = BLayer.FillEmp(IntEmpId,"");
            DDLEmplName.DataValueField = "EmpId";
            DDLEmplName.DataTextField = "EmpName";
            DDLEmplName.DataBind();
            if (ViewState["LoginUserGroup"].ToString() != "EMP")
            {
                DDLEmplName.Items.Insert(0, new ListItem("--Select Employee--", "0"));
            }
            if (IntEmpId != 0)
            {
                DDLEmplName.SelectedValue = dtTemp.Rows[0]["EmpId"].ToString();
            }
            DDLEmplName.Enabled = false;


            TxtDRFDate.Text = dtTemp.Rows[0]["DRFDate"].ToString();
            TxtInTime.Text = dtTemp.Rows[0]["InTime"].ToString();
            TxtOutTime.Text = dtTemp.Rows[0]["OutTime"].ToString();
            TxtTotWrkHours.Text = dtTemp.Rows[0]["TotTime"].ToString();            
            TxtRemark.Text = dtTemp.Rows[0]["Remark"].ToString();

            FillWorkData();

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

    protected void FillWorkData()
    {
        try
        {
            if (int.Parse(HidFldId.Value) != 0)
            {
                StrSql = new StringBuilder();
                StrSql.Length = 0;

                StrSql.AppendLine("Select D.Id,D.DRFId,D.StartTime,D.EndTime,D.TotTime");
                StrSql.AppendLine(",P.Id As PrjId,P.ProjectName AS PrjName");
                StrSql.AppendLine(",M.Id As ModId,M.ModuleName AS ModName");
                StrSql.AppendLine(",D.Work_Desc AS WorkDesc");                
                StrSql.AppendLine(",Case When IsNull(D.WorkStatus,'')='D' Then 'Done'");
                StrSql.AppendLine("      When IsNull(D.WorkStatus,'')='P' Then 'Pending'");
                StrSql.AppendLine("      When IsNull(D.WorkStatus,'')='C' Then 'Cancle' Else '0' End As WorkStatus");
                StrSql.AppendLine(",D.Remark");
                StrSql.AppendLine("From DRFDET D");
                StrSql.AppendLine("Left Join Project_Mast P On D.PrjId=P.Id ");
                StrSql.AppendLine("Left Join Project_Module M On D.PrjModId=M.Id ");
                StrSql.AppendLine("Where D.DRFId=" + int.Parse(HidFldId.Value));
                StrSql.AppendLine("Order By D.DRFId,D.Id");

                StrSql.AppendLine(";");

                StrSql.AppendLine("Select IsNull(SUM(ISNULL(TotTime,0)),0) As TotTime From DRFDET Where DRFId=" + int.Parse(HidFldId.Value));

                ds = new DataSet();
                ds = SqlFunc.ExecuteDataSet(StrSql.ToString());

                if (ds.Tables[0].Rows.Count != 0)
                {
                    GridWork.DataSource = ds;
                    GridWork.DataBind();
                    
                    if (ds.Tables[1].Rows.Count != 0)
                    {
                        TxtTotTime.Text = ds.Tables[1].Rows[0]["TotTime"].ToString();
                    }
                    else
                    {
                        TxtTotTime.Text = "";
                    }
                    ModulePanel.Visible = true;
                }
                else
                {
                    ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                    GridWork.DataSource = ds;
                    GridWork.DataBind();
                    int columncount = GridWork.Rows[0].Cells.Count;
                    GridWork.Rows[0].Cells.Clear();
                    GridWork.Rows[0].Cells.Add(new TableCell());
                    GridWork.Rows[0].Cells[0].ColumnSpan = columncount;
                    GridWork.Rows[0].Cells[0].Text = "No Records Found";
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }
             
    protected void DDlEditPrjName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
        DropDownList dr = (DropDownList)sender;
        GridViewRow gr = (GridViewRow)dr.NamingContainer;
        DropDownList DDlEditModName = (DropDownList)gr.FindControl("DDlEditModName");

        DDlEditModName.Items.Clear();
        DDlEditModName.DataSource = BLayer.FillPrjModule(int.Parse(dr.SelectedValue));
        DDlEditModName.DataValueField = "ModId";
        DDlEditModName.DataTextField = "ModName";
        DDlEditModName.DataBind();
        DDlEditModName.Items.Insert(0, new ListItem("--Select Module--", "0"));
         }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    protected void DDlFtrPrjName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
        DropDownList dr = (DropDownList)sender;
        GridViewRow gr = (GridViewRow)dr.NamingContainer;
        DropDownList DDlFtrModName = (DropDownList)gr.FindControl("DDlFtrModName");

        DDlFtrModName.Items.Clear();
        DDlFtrModName.DataSource = BLayer.FillPrjModule(int.Parse(dr.SelectedValue));
        DDlFtrModName.DataValueField = "ModId";
        DDlFtrModName.DataTextField = "ModName";
        DDlFtrModName.DataBind();
        DDlFtrModName.Items.Insert(0, new ListItem("--Select Module--", "0"));
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }

    }
    protected void GridWork_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {                                             
                //Check if is in edit mode
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    DropDownList DDlEditPrjName = (DropDownList)e.Row.FindControl("DDlEditPrjName");
                    //Bind data to dropdownlist
                    DDlEditPrjName.Items.Clear();
                    DDlEditPrjName.DataSource = BLayer.FillProject("");
                    DDlEditPrjName.DataTextField = "PrjName";
                    DDlEditPrjName.DataValueField = "PrjId";
                    DDlEditPrjName.DataBind();
                    DDlEditPrjName.Items.Insert(0, new ListItem("Select Project", "0"));
                    DataRowView dr = e.Row.DataItem as DataRowView;
                    DDlEditPrjName.SelectedValue = dr["PrjId"].ToString(); //Grid Column Name

                    DropDownList DDlEditModName = (DropDownList)e.Row.FindControl("DDlEditModName");
                    //Bind data to dropdownlist
                    DDlEditModName.Items.Clear();
                    DDlEditModName.DataSource = BLayer.FillPrjModule(int.Parse(dr["PrjId"].ToString()));
                    DDlEditModName.DataTextField = "ModName";
                    DDlEditModName.DataValueField = "ModId";
                    DDlEditModName.DataBind();
                    DDlEditModName.Items.Insert(0, new ListItem("Select Module", "0"));
                    DataRowView drmod = e.Row.DataItem as DataRowView;
                    DDlEditPrjName.SelectedValue = drmod["ModId"].ToString(); //Grid Column Name
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList DDlFtrPrjName = (DropDownList)e.Row.FindControl("DDlFtrPrjName");

                DDlFtrPrjName.Items.Clear();
                DDlFtrPrjName.DataSource = BLayer.FillProject("");
                DDlFtrPrjName.DataTextField = "PrjName";
                DDlFtrPrjName.DataValueField = "PrjId";
                DDlFtrPrjName.DataBind();
                DDlFtrPrjName.Items.Insert(0, new ListItem("Select Project", "0"));
            }
        }
        catch(Exception ex)
        {
            Response.Write(ex.ToString()); 
        }
    }

    protected int GetTime(string pTime, int IntRet)
    {
        int IntTime;
        string[] Time = pTime.ToString().Split('.');
        if (pTime.IndexOf(".", 0) < 0)
        {
            if (IntRet == 1)
            {
                IntTime = 0;
            }
            else
            {
                if (Time[IntRet].ToString() == "")
                {
                    IntTime = 0;
                }
                else
                {
                    IntTime = int.Parse(Time[IntRet].ToString());
                }    
            }
        }
        else
        {
            if (Time[IntRet].ToString() == "")
            {
                IntTime = 0;
            }
            else
            {
                IntTime = int.Parse(Time[IntRet].ToString());
            }    
        }
        return IntTime;
    }    

    protected void GetTotTime()
    {
        // Define two dates.       
        DateTime FrmTime = new DateTime(1, 1, 1, GetTime(TxtInTime.Text.ToString(), 0), GetTime(TxtInTime.Text.ToString(), 1), 0);
        DateTime ToTime = new DateTime(1, 1, 1, GetTime(TxtOutTime.Text.ToString(), 0), GetTime(TxtOutTime.Text.ToString(), 1), 0);

        // Calculate the interval between the two dates.
        TimeSpan interval = ToTime - FrmTime;

        String DiffTime = interval.Hours.ToString("00") + "." + interval.Minutes.ToString("00");

        TxtInTime.Text = GetTime(TxtInTime.Text.ToString(), 0).ToString("00") + "." + GetTime(TxtInTime.Text.ToString(), 1).ToString("00");
        TxtOutTime.Text = GetTime(TxtOutTime.Text.ToString(), 0).ToString("00") + "." + GetTime(TxtOutTime.Text.ToString(), 1).ToString("00");
        TxtTotWrkHours.Text = DiffTime.ToString();
        TxtRemark.Focus();
    }

    protected void TxtInTime_TextChanged(object sender, EventArgs e)
    {
        try
        {
            GetTotTime();
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }
    protected void TxtOutTime_TextChanged(object sender, EventArgs e)
    {
        try
        {
            GetTotTime();
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }
    protected void BtnView_Click(object sender, EventArgs e)
    {
        try
        {
            FillGrid();
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
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
            StrSql.AppendLine("Select Top 1 Id From DRFH");
            if (int.Parse(DDLEmplName.SelectedValue) != 0)
            {
                StrSql.AppendLine("Where EmpId=" + int.Parse(DDLEmplName.SelectedValue));
            }
            StrSql.AppendLine("Order By Id");
        }
        else if (pStrNavigation.Trim().ToUpper() == "NEXT")
        {
            if (HidFldId.Value == "")
            {
                LblMsg.Text = "Data not found";
                return;
            }
            StrSql.AppendLine("Select Top 1 Id From DRFH Where Id > " + int.Parse(HidFldId.Value));
            if (int.Parse(DDLEmplName.SelectedValue) != 0)
            {
                StrSql.AppendLine("And EmpId=" + int.Parse(DDLEmplName.SelectedValue));
            }
        }
        else if (pStrNavigation.Trim().ToUpper() == "PREV")
        {
            if (HidFldId.Value == "")
            {
                LblMsg.Text = "Data not found";
                return;
            }
            StrSql.AppendLine("Select Top 1 Id From DRFH Where Id < " + int.Parse(HidFldId.Value));
            if (int.Parse(DDLEmplName.SelectedValue) != 0)
            {
                StrSql.AppendLine("And EmpId=" + int.Parse(DDLEmplName.SelectedValue));
            }
            StrSql.AppendLine("Order By Id Desc");
        }
        else if (pStrNavigation.Trim().ToUpper() == "LAST")
        {
            StrSql.AppendLine("Select Top 1 Id From DRFH");
            if (int.Parse(DDLEmplName.SelectedValue) != 0)
            {
                StrSql.AppendLine("Where EmpId=" + int.Parse(DDLEmplName.SelectedValue));
            }
            StrSql.AppendLine("Order By Id Desc");
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
        StrSql.AppendLine("Select H.Id,H.EmpId,Convert(Varchar(10),H.DRFDate,103) As DRFDate");
        StrSql.AppendLine(",H.InTime,H.OutTime,H.TotTime,H.Remark ");
        StrSql.AppendLine("From DRFH H");
        StrSql.AppendLine("Where H.Id=" + IntId);
        dtTemp = new DataTable();
        dtTemp = SqlFunc.ExecuteDataTable(StrSql.ToString());

        if (dtTemp.Rows.Count == 0)
        {
            LblMsg.Text = "Data not found";
            return;
        }

        LblMsg.Text = "";
        HidFldId.Value = IntId.ToString();

        DDLEmplName.SelectedValue = dtTemp.Rows[0]["EmpId"].ToString();
        DDLEmplName.Enabled = false;

        TxtDRFDate.Text = dtTemp.Rows[0]["DRFDate"].ToString();
        TxtInTime.Text = dtTemp.Rows[0]["InTime"].ToString();
        TxtOutTime.Text = dtTemp.Rows[0]["OutTime"].ToString();
        TxtTotWrkHours.Text = dtTemp.Rows[0]["TotTime"].ToString();
        TxtRemark.Text = dtTemp.Rows[0]["Remark"].ToString();

        FillWorkData();
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
