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
using HRMSystem;

public partial class Transaction_WrokAssign : System.Web.UI.Page
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

            if (ViewState["LoginUserGroup"].ToString() == "EMP")
            {
                BtnRow.Visible = false;
            }
            else
            {
                BtnRow.Visible = true;
            }

            ClearAll();
           // FillGrid();

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
           
            DDLPrjLed.Enabled = true;
            DDLPrjLed.Items.Clear();
            DDLPrjLed.DataSource = BLayer.FillEmp(0," And IsNull(EmpGroup,'')='Ledger'");
            DDLPrjLed.DataValueField = "EmpId";
            DDLPrjLed.DataTextField = "EmpName";
            DDLPrjLed.DataBind();
            DDLPrjLed.Items.Insert(0, new ListItem("--Select Ledger--", "0"));
                       
            DDLPrjName.Items.Clear();
            DDLPrjName.DataSource = BLayer.FillProject("");
            DDLPrjName.DataValueField = "PrjId";
            DDLPrjName.DataTextField = "PrjName";
            DDLPrjName.DataBind();
            DDLPrjName.Items.Insert(0, new ListItem("--Select Project--", "0"));

            DDLPrjModule.Items.Clear();
            DDLPrjModule.Items.Add("---Select Module---");

            int IntEmpId = 0;
            if (ViewState["LoginUserGroup"].ToString() == "EMP")
            {
                IntEmpId = int.Parse(ViewState["LoginId"].ToString());
                ddlEmployee.Enabled = false;               
            }
            else
            {
                ddlEmployee.Enabled = true;               
            }            
            ddlEmployee.Items.Clear();
            ddlEmployee.DataSource = BLayer.FillEmp(IntEmpId, "");
            ddlEmployee.DataValueField = "EmpId";
            ddlEmployee.DataTextField = "EmpName";
            ddlEmployee.DataBind();
            if (IntEmpId == 0)
            {
                ddlEmployee.Items.Insert(0, new ListItem("--Select Employee--", "0"));
            }

            TxtClientName.Text = "";
            TxtAssignDate.Text = "";
            TxtAssignTime.Text = "";
            TxtDueDays.Text = "";
            TxtDueDate.Text = "";
            TxtWrkDetails1.Text = "";
            TxtWrkDetails2.Text = "";
            TxtWrkDetails3.Text = "";

            ddlWrkStatus.SelectedIndex = 0;

            TxtCompleteDate.Text = "";
            TxtCompleteTime.Text = "";
            TxtRemark.Text = ""; 
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
            StrSql = new StringBuilder();
            StrSql.Length = 0;

            StrSql.AppendLine("Select W.Id,W.PrjLedId,W.PrjId,W.PrjModId,W.EmpId");
            StrSql.AppendLine(",PL.EmpName As ProjectLed,P.ProjectName,M.ModuleName,E.EmpName");
            StrSql.AppendLine(",Case When IsNull(CompleteDate,'')<>''");
            StrSql.AppendLine("      Then Convert(Varchar(10),CompleteDate,103) Else '' End  As CompleteDate");
            StrSql.AppendLine("From WorkAssDet W");
            StrSql.AppendLine("Left Join Emp_Mast PL On PL.Id=W.PrjLedId");
            StrSql.AppendLine("Left Join Emp_Mast E On E.Id=W.EmpId");
            StrSql.AppendLine("Left Join Project_Mast P On P.Id=W.PrjId");
            StrSql.AppendLine("Left Join Project_Module M On M.Id=W.PrjModId");
            if (ViewState["LoginUserGroup"].ToString() == "EMP")
            {
                StrSql.AppendLine("Where W.EmpId=" + int.Parse(ViewState["LoginId"].ToString()));
            }
        
            StrSql.AppendLine("Order By Pl.EmpName");

            dtTemp = new DataTable();
            dtTemp = SqlFunc.ExecuteDataTable(StrSql.ToString());

            GridWork.DataSource = dtTemp;
            GridWork.DataBind();
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    protected void GridWork_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GridWork.PageIndex = e.NewPageIndex;
            FillGrid();
            LblMsg.Text = "";
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }
    //protected void GridWork_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    try
    //    {
    //        StrSql = new StringBuilder();
    //        StrSql.Length = 0;

    //        StrSql.AppendLine("Delete From WorkAssDet Where Id=@Id ");
    //        Cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);
    //        Cmd.Parameters.AddWithValue("@Id", int.Parse(GridWork.DataKeys[e.RowIndex].Value.ToString()));
    //        SqlFunc.ExecuteNonQuery(Cmd);

    //        GridWork.EditIndex = -1;
    //        ClearAll(); 
    //        FillGrid();
    //        LblMsg.Text = "Entry deleted successfully";
    //    }
    //    catch (Exception ex)
    //    {
    //        Response.Write(ex);
    //    }
    //}

    protected void btnWorkDelete_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton btndetails = sender as ImageButton;
        GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
        Session["Id"] = GridWork.DataKeys[gvrow.RowIndex].Value.ToString();
        lblUser.Text = "Are you sure you want to delete this entry ? ";
        ModalPopupExtender1.Show();
    }

    protected void btnYes_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            int Workid = Convert.ToInt32(Session["Id"]);

            StrSql = new StringBuilder();
            StrSql.Length = 0;
            StrSql.AppendLine("Delete From WorkAssDet Where Id=@Id");
            Cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);
            Cmd.Parameters.AddWithValue("@Id", Workid);
            SqlFunc.ExecuteNonQuery(Cmd);

            GridWork.EditIndex = -1;
            ClearAll();
            FillGrid();
            LblMsg.Text = "Entry deleted successfully";
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    protected void btnWorkSelect_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton btndetails = sender as ImageButton;
            GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
            Session["Id"] = GridWork.DataKeys[gvrow.RowIndex].Value.ToString();

            HidFldId.Value = Session["Id"].ToString();
            
            StrSql = new StringBuilder();
            StrSql.Length = 0;

            StrSql.AppendLine("Select W.Id,W.PrjLedId,W.PrjId,W.PrjModId,CM.ClientName,W.EmpId");
            StrSql.AppendLine(",Convert(Varchar(10),W.AssignDate,103) As AssignDate,W.AssignTime");
            StrSql.AppendLine(",W.DueDays,Convert(Varchar(10),W.DueDate,103) As DueDate");
            StrSql.AppendLine(",W.WorkDet1,W.WorkDet2,W.WorkDet3,W.WorkStatus");
            StrSql.AppendLine(",Case When IsNull(W.CompleteDate,'')<>''");
            StrSql.AppendLine("      Then Convert(Varchar(10),W.CompleteDate,103) Else '' End  As CompleteDate");
            StrSql.AppendLine(",W.CompleteTime,W.Remark");
            StrSql.AppendLine("From WorkAssDet W");
            StrSql.AppendLine("Left Join Emp_Mast PL On PL.Id=W.PrjLedId");
            StrSql.AppendLine("Left Join Emp_Mast E On E.Id=W.EmpId");
            StrSql.AppendLine("Left Join Project_Mast P On P.Id=W.PrjId");
            StrSql.AppendLine("Left Join Client_Mast CM On CM.Id=P.ClientId");
            StrSql.AppendLine("Left Join Project_Module M On M.Id=W.PrjModId");
            StrSql.AppendLine("Where W.Id=" + int.Parse(HidFldId.Value));

            dtTemp = new DataTable();
            dtTemp = SqlFunc.ExecuteDataTable(StrSql.ToString());

            if (dtTemp.Rows.Count == 0)
            {
                LblMsg.Text = "Data not found";
                return;
            }

            DDLPrjLed.Items.Clear();
            DDLPrjLed.DataSource = BLayer.FillEmp(0," And IsNull(EmpGroup,'')='Ledger'");
            DDLPrjLed.DataValueField = "EmpId";
            DDLPrjLed.DataTextField = "EmpName";
            DDLPrjLed.DataBind();
            DDLPrjLed.Items.Insert(0, new ListItem("--Select Ledger--", "0"));

            DDLPrjName.Items.Clear();
            DDLPrjName.DataSource = BLayer.FillProject("");
            DDLPrjName.DataValueField = "PrjId";
            DDLPrjName.DataTextField = "PrjName";
            DDLPrjName.DataBind();
            DDLPrjName.Items.Insert(0, new ListItem("--Select Project--", "0"));

            DDLPrjModule.Items.Clear();
            DDLPrjModule.DataSource = BLayer.FillPrjModule(int.Parse(DDLPrjName.SelectedValue));
            DDLPrjModule.DataValueField = "ModId";
            DDLPrjModule.DataTextField = "ModName";
            DDLPrjModule.DataBind();
            DDLPrjModule.Items.Insert(0, new ListItem("--Select Module--", "0"));

            int IntEmpId = 0;
            if (ViewState["LoginUserGroup"].ToString() == "EMP")
            {
                IntEmpId = int.Parse(ViewState["LoginId"].ToString());
                ddlEmployee.Enabled = false;
            }           
            ddlEmployee.Items.Clear();
            ddlEmployee.DataSource = BLayer.FillEmp(0,"");
            ddlEmployee.DataValueField = "EmpId";
            ddlEmployee.DataTextField = "EmpName";
            ddlEmployee.DataBind();
            ddlEmployee.Items.Insert(0, new ListItem("--Select Employee--", "0"));

            if (dtTemp.Rows[0]["PrjLedId"].ToString() != "")
            {
                DDLPrjLed.SelectedValue = dtTemp.Rows[0]["PrjLedId"].ToString();
            }
            if (dtTemp.Rows[0]["PrjId"].ToString() != "")
            {
                DDLPrjName.SelectedValue = dtTemp.Rows[0]["PrjId"].ToString();
            }
            if (dtTemp.Rows[0]["PrjModId"].ToString() != "")
            {
                DDLPrjModule.SelectedValue = dtTemp.Rows[0]["PrjModId"].ToString();
            }
            if (dtTemp.Rows[0]["EmpId"].ToString() != "")
            {
                ddlEmployee.SelectedValue = dtTemp.Rows[0]["EmpId"].ToString();
            }

            TxtClientName.Text = dtTemp.Rows[0]["ClientName"].ToString();
            TxtAssignDate.Text = dtTemp.Rows[0]["AssignDate"].ToString();
            TxtAssignTime.Text = dtTemp.Rows[0]["AssignTime"].ToString();
            TxtDueDays.Text = dtTemp.Rows[0]["DueDays"].ToString();
            TxtDueDate.Text = dtTemp.Rows[0]["DueDate"].ToString();
            TxtWrkDetails1.Text = dtTemp.Rows[0]["WorkDet1"].ToString();
            TxtWrkDetails2.Text = dtTemp.Rows[0]["WorkDet2"].ToString();
            TxtWrkDetails3.Text = dtTemp.Rows[0]["WorkDet3"].ToString();

            //ddlWrkStatus.Text = dtTemp.Rows[0]["WorkStatus"].ToString();
            if (dtTemp.Rows[0]["WorkStatus"].ToString() != "")
            {
                ddlWrkStatus.SelectedValue = dtTemp.Rows[0]["WorkStatus"].ToString();
            }

            TxtCompleteDate.Text = dtTemp.Rows[0]["CompleteDate"].ToString();
            TxtCompleteTime.Text = dtTemp.Rows[0]["CompleteTime"].ToString();
            TxtRemark.Text = dtTemp.Rows[0]["Remark"].ToString();

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


    //protected void GridWork_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        HidFldId.Value = GridWork.DataKeys[GridWork.SelectedRow.RowIndex].Value.ToString();
                   
    //        StrSql = new StringBuilder();
    //        StrSql.Length = 0;

    //        StrSql.AppendLine("Select W.Id,W.PrjLedId,W.PrjId,W.PrjModId,CM.ClientName,W.EmpId");
    //        StrSql.AppendLine(",Convert(Varchar(10),W.AssignDate,103) As AssignDate,W.AssignTime");
    //        StrSql.AppendLine(",W.DueDays,Convert(Varchar(10),W.DueDate,103) As DueDate");
    //        StrSql.AppendLine(",W.WorkDet1,W.WorkDet2,W.WorkDet3,W.WorkStatus");
    //        StrSql.AppendLine(",Case When IsNull(W.CompleteDate,'')<>''");
    //        StrSql.AppendLine("      Then Convert(Varchar(10),W.CompleteDate,103) Else '' End  As CompleteDate");
    //        StrSql.AppendLine(",W.CompleteTime,W.Remark");
    //        StrSql.AppendLine("From WorkAssDet W");
    //        StrSql.AppendLine("Left Join Emp_Mast PL On PL.Id=W.PrjLedId");
    //        StrSql.AppendLine("Left Join Emp_Mast E On E.Id=W.EmpId");
    //        StrSql.AppendLine("Left Join Project_Mast P On P.Id=W.PrjId");
    //        StrSql.AppendLine("Left Join Client_Mast CM On CM.Id=P.ClientId");
    //        StrSql.AppendLine("Left Join Project_Module M On M.Id=W.PrjModId");
    //        StrSql.AppendLine("Where W.Id=" + int.Parse(HidFldId.Value));            

    //        dtTemp = new DataTable();
    //        dtTemp = SqlFunc.ExecuteDataTable(StrSql.ToString());

    //        if (dtTemp.Rows.Count == 0)
    //        {
    //            LblMsg.Text = "Data not found";
    //            return;
    //        }

    //        DDLPrjLed.Items.Clear();
    //        DDLPrjLed.DataSource = BLayer.FillEmp(" And IsNull(EmpGroup,'')='Ledger'");
    //        DDLPrjLed.DataValueField = "EmpId";
    //        DDLPrjLed.DataTextField = "EmpName";
    //        DDLPrjLed.DataBind();
    //        DDLPrjLed.Items.Insert(0, new ListItem("--Select Ledger--", "0"));

    //        DDLPrjName.Items.Clear();
    //        DDLPrjName.DataSource = BLayer.FillProject("");
    //        DDLPrjName.DataValueField = "PrjId";
    //        DDLPrjName.DataTextField = "PrjName";
    //        DDLPrjName.DataBind();
    //        DDLPrjName.Items.Insert(0, new ListItem("--Select Project--", "0"));

    //        DDLPrjModule.Items.Clear();
    //        DDLPrjModule.DataSource = BLayer.FillPrjModule(int.Parse(DDLPrjName.SelectedValue));
    //        DDLPrjModule.DataValueField = "ModId";
    //        DDLPrjModule.DataTextField = "ModName";
    //        DDLPrjModule.DataBind();
    //        DDLPrjModule.Items.Insert(0, new ListItem("--Select Module--", "0"));

    //        ddlEmployee.Items.Clear();
    //        ddlEmployee.DataSource = BLayer.FillEmp("");
    //        ddlEmployee.DataValueField = "EmpId";
    //        ddlEmployee.DataTextField = "EmpName";
    //        ddlEmployee.DataBind();
    //        ddlEmployee.Items.Insert(0, new ListItem("--Select Employee--", "0"));
                        
    //        if (dtTemp.Rows[0]["PrjLedId"].ToString() != "")
    //        {
    //            DDLPrjLed.SelectedValue = dtTemp.Rows[0]["PrjLedId"].ToString();
    //        }
    //        if (dtTemp.Rows[0]["PrjId"].ToString() != "")
    //        {
    //            DDLPrjName.SelectedValue = dtTemp.Rows[0]["PrjId"].ToString();
    //        }
    //        if (dtTemp.Rows[0]["PrjModId"].ToString() != "")
    //        {
    //            DDLPrjModule.SelectedValue = dtTemp.Rows[0]["PrjModId"].ToString();
    //        }
    //        if (dtTemp.Rows[0]["EmpId"].ToString() != "")
    //        {
    //            ddlEmployee.SelectedValue = dtTemp.Rows[0]["EmpId"].ToString();
    //        }

    //        TxtClientName.Text = dtTemp.Rows[0]["ClientName"].ToString();
    //        TxtAssignDate.Text = dtTemp.Rows[0]["AssignDate"].ToString();
    //        TxtAssignTime.Text = dtTemp.Rows[0]["AssignTime"].ToString();
    //        TxtDueDays.Text = dtTemp.Rows[0]["DueDays"].ToString();
    //        TxtDueDate.Text = dtTemp.Rows[0]["DueDate"].ToString();
    //        TxtWrkDetails1.Text = dtTemp.Rows[0]["WorkDet1"].ToString();
    //        TxtWrkDetails2.Text = dtTemp.Rows[0]["WorkDet2"].ToString();
    //        TxtWrkDetails3.Text = dtTemp.Rows[0]["WorkDet3"].ToString();

    //        //ddlWrkStatus.Text = dtTemp.Rows[0]["WorkStatus"].ToString();
    //        if (dtTemp.Rows[0]["WorkStatus"].ToString() != "")
    //        {
    //            ddlWrkStatus.SelectedValue = dtTemp.Rows[0]["WorkStatus"].ToString();
    //        }

    //        TxtCompleteDate.Text = dtTemp.Rows[0]["CompleteDate"].ToString();
    //        TxtCompleteTime.Text = dtTemp.Rows[0]["CompleteTime"].ToString();
    //        TxtRemark.Text = dtTemp.Rows[0]["Remark"].ToString();                        

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

    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            //if (DDLPrjLed.SelectedValue == "0")
            //{
            //    LblMsg.Text = "Select Project Ledger....";
            //    DDLPrjLed.Focus();
            //    return;
            //}
            //if (DDLPrjName.SelectedValue == "0")
            //{
            //    LblMsg.Text = "Select Project Name....";
            //    DDLPrjName.Focus();
            //    return;
            //}
            //if (DDLPrjModule.SelectedValue == "0")
            //{
            //    LblMsg.Text = "Select Project Module....";
            //    DDLPrjModule.Focus();
            //    return;
            //}
            //if (ddlEmployee.SelectedValue == "0")
            //{
            //    LblMsg.Text = "Select Employee Name....";
            //    ddlEmployee.Focus();
            //    return;
            //}

            //if (TxtAssignDate.Text.Length == 0)
            //{
            //    LblMsg.Text = "Assign Date Is Blank, Enter Valid Date....";
            //    TxtAssignDate.Focus();
            //    return;
            //}

            //if (int.Parse(TxtDueDays.Text) == 0)
            //{
            //    LblMsg.Text = "Due Days Is Zero, Enter Valid Days....";
            //    TxtDueDays.Focus();
            //    return;
            //}

            //if (TxtWrkDetails1.Text.Length == 0)
            //{
            //    LblMsg.Text = "Enter Working Details....";
            //    TxtWrkDetails1.Focus();
            //    return;
            //}
            //if (ddlWrkStatus.SelectedValue == "0")
            //{
            //    LblMsg.Text = "Select Work Status....";
            //    ddlWrkStatus.Focus();
            //    return;
            //}

            StrSql = new StringBuilder();
            StrSql.Length = 0;

            if (HidFldId.Value.Length == 0)
            {
                StrSql.AppendLine("Insert Into WorkAssDet(PrjLedId,PrjId,PrjModId,ClientName,EmpId");
                StrSql.AppendLine(",AssignDate,AssignTime,DueDate,DueDays");
                StrSql.AppendLine(",WorkDet1,WorkDet2,WorkDet3,WorkStatus");
                StrSql.AppendLine(",CompleteDate,CompleteTime,Remark");
                StrSql.AppendLine(",Entry_Date,Entry_Time,Entry_UID,UPDFLAG)");
                StrSql.AppendLine("Values(@PrjLedId,@PrjId,@PrjModId,@ClientName,@EmpId");
                StrSql.AppendLine(",@AssignDate,@AssignTime,@DueDate,@DueDays");
                StrSql.AppendLine(",@WorkDet1,@WorkDet2,@WorkDet3,@WorkStatus");
                StrSql.AppendLine(",@CompleteDate,@CompleteTime,@Remark");
                StrSql.AppendLine(",GETDATE(),CONVERT(VarChar,GetDate(),108),@Entry_UID,0)");
            }
            else
            {
                StrSql.AppendLine("Update WorkAssDet");
                StrSql.AppendLine("Set PrjLedId=@PrjLedId");
                StrSql.AppendLine(",PrjId=@PrjId");
                StrSql.AppendLine(",PrjModId=@PrjModId");
                StrSql.AppendLine(",ClientName=@ClientName");
                StrSql.AppendLine(",EmpId=@EmpId");
                StrSql.AppendLine(",AssignDate=@AssignDate");
                StrSql.AppendLine(",AssignTime=@AssignTime");
                StrSql.AppendLine(",DueDate=@DueDate");
                StrSql.AppendLine(",DueDays=@DueDays");
                StrSql.AppendLine(",WorkDet1=@WorkDet1");
                StrSql.AppendLine(",WorkDet2=@WorkDet2");
                StrSql.AppendLine(",WorkDet3=@WorkDet3");
                StrSql.AppendLine(",WorkStatus=@WorkStatus");
                StrSql.AppendLine(",CompleteDate=@CompleteDate");
                StrSql.AppendLine(",CompleteTime=@CompleteTime");
                StrSql.AppendLine(",Remark=@Remark");              
                StrSql.AppendLine(",MEntry_Date=GetDate(),MEntry_Time=Convert(Varchar,GetDate(),108)");
                StrSql.AppendLine(",MEntry_UID=@Entry_UID");
                StrSql.AppendLine(",UPDFLAG=IsNull(UPDFlag,0)+1");
                StrSql.AppendLine("Where Id=@Id");
            }

            Cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);

            Cmd.Parameters.AddWithValue("@PrjLedId", DDLPrjLed.SelectedValue);
            Cmd.Parameters.AddWithValue("@PrjId", DDLPrjName.SelectedValue);
            Cmd.Parameters.AddWithValue("@PrjModId", DDLPrjModule.SelectedValue);
            Cmd.Parameters.AddWithValue("@ClientName", TxtClientName.Text);
            Cmd.Parameters.AddWithValue("@EmpId", ddlEmployee.SelectedValue);

            Cmd.Parameters.AddWithValue("@AssignDate", ValueConvert.ConvertDate(TxtAssignDate.Text));
            Cmd.Parameters.AddWithValue("@AssignTime", TxtAssignTime.Text);
            Cmd.Parameters.AddWithValue("@DueDate", ValueConvert.ConvertDate(TxtDueDate.Text));
            Cmd.Parameters.AddWithValue("@DueDays", TxtDueDays.Text);

            Cmd.Parameters.AddWithValue("@WorkDet1", TxtWrkDetails1.Text);
            Cmd.Parameters.AddWithValue("@WorkDet2", TxtWrkDetails2.Text);
            Cmd.Parameters.AddWithValue("@WorkDet3", TxtWrkDetails3.Text);

            Cmd.Parameters.AddWithValue("@WorkStatus", ddlWrkStatus.SelectedValue);

            if (TxtCompleteDate.Text.Length != 0)
            {
                Cmd.Parameters.AddWithValue("@CompleteDate", ValueConvert.ConvertDate(TxtCompleteDate.Text));
            }
            else
            {
                Cmd.Parameters.AddWithValue("@CompleteDate", DBNull.Value);
            }
            
            Cmd.Parameters.AddWithValue("@CompleteTime", TxtCompleteTime.Text);
            Cmd.Parameters.AddWithValue("@Remark", TxtRemark.Text);

            Cmd.Parameters.AddWithValue("@Entry_UID", HidFldUID.Value.ToString());

            if (HidFldId.Value.Length == 0)
            {
                SqlFunc.ExecuteNonQuery(Cmd);
                LblMsg.Text = "Entry added successfully";
            }
            else
            {
                Cmd.Parameters.AddWithValue("@ID", HidFldId.Value.ToString());

                SqlFunc.ExecuteNonQuery(Cmd);
                LblMsg.Text = "Entry updated successfully";
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
            Response.Write(ex);
        }
    }
    protected void BtnNew_Click(object sender, EventArgs e)
    {
        try
        {
            ClearAll();
            DDLPrjLed.Focus();
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

                    StrSql.AppendLine("Delete From WorkAssDet Where Id=@Id ");
                    Cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);
                    Cmd.Parameters.AddWithValue("@Id", int.Parse(HidFldId.Value));
                    SqlFunc.ExecuteNonQuery(Cmd);

                    FillGrid();

                    LblMsg.Text = "Entry deleted successfully";

                    ClearAll();

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
    protected void DDLPrjName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {            
            try
            {
                DDLPrjModule.Items.Clear();
                DDLPrjModule.DataSource = BLayer.FillPrjModule(int.Parse(DDLPrjName.SelectedValue));
                DDLPrjModule.DataValueField = "ModId";
                DDLPrjModule.DataTextField = "ModName";
                DDLPrjModule.DataBind();
                DDLPrjModule.Items.Insert(0, new ListItem("--Select Module--", "0"));

                TxtClientName.Text = "";

                StrSql = new StringBuilder();
                StrSql.Length = 0;
                StrSql.AppendLine("Select ClientId From Project_Mast Where Id=" + int.Parse(DDLPrjName.SelectedValue));
                dtTemp = SqlFunc.ExecuteDataTable(StrSql.ToString());
                if (dtTemp.Rows.Count != 0)
                {
                    if (int.Parse(dtTemp.Rows[0]["ClientId"].ToString()) != 0)
                    {
                        StrSql = new StringBuilder();
                        StrSql.Length = 0;
                        StrSql.AppendLine("Select ClientName From Client_Mast Where Id=" + int.Parse(dtTemp.Rows[0]["ClientId"].ToString()));
                        dtTemp = SqlFunc.ExecuteDataTable(StrSql.ToString());
                        if (dtTemp.Rows.Count != 0)
                        {
                            TxtClientName.Text = dtTemp.Rows[0]["ClientName"].ToString();
                        }
                    }
                }

                DDLPrjModule.Focus();
            }
            catch(Exception ex)
            {
                Response.Write(ex.ToString()); 
            }

        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    protected void BtnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            if (TxtCompleteDate.Text.Length != 0)
            {
                LblMsg.Text = "Complete Date Is Blank !!!";
                TxtCompleteDate.Focus();
                return;
            }

            if (TxtCompleteTime.Text.Length != 0)
            {
                LblMsg.Text = "Complete Time Is Blank !!!";
                TxtCompleteTime.Focus();
                return;
            }

            if (ddlWrkStatus.SelectedValue == "0")
            {
                LblMsg.Text = "Select Work Status....";
                ddlWrkStatus.Focus();
                return;
            }

            if (HidFldId.Value.Length == 0)
            {
                StrSql=new StringBuilder();
                StrSql.Length=0;
                StrSql.AppendLine("Update WorkAssDet");
                StrSql.AppendLine("Set CompleteDate=@CompleteDate");
                StrSql.AppendLine(",CompleteTime=@CompleteTime");
                StrSql.AppendLine(",WorkStatus=@WorkStatus");
                StrSql.AppendLine("Where Id=@ID");

                Cmd.Parameters.AddWithValue("@CompleteDate", ValueConvert.ConvertDate(TxtCompleteDate.Text));                
                Cmd.Parameters.AddWithValue("@CompleteTime", TxtCompleteTime.Text);
                Cmd.Parameters.AddWithValue("@WorkStatus", ddlWrkStatus.SelectedValue);
                Cmd.Parameters.AddWithValue("@ID", HidFldId.Value.ToString());

                SqlFunc.ExecuteNonQuery(Cmd);
                LblMsg.Text = "Entry updated successfully";
            }
            else
            {
                LblMsg.Text = "Id Is Blank !!!";
            }
        }
        catch(Exception ex)
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
            StrSql.AppendLine("Select Top 1 Id From WorkAssDet");
            if (int.Parse(ddlEmployee.SelectedValue) != 0)
            {
                StrSql.AppendLine("Where EmpId=" + int.Parse(ddlEmployee.SelectedValue));
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
            StrSql.AppendLine("Select Top 1 Id From WorkAssDet Where Id > " + int.Parse(HidFldId.Value));
            if (int.Parse(ddlEmployee.SelectedValue) != 0)
            {
                StrSql.AppendLine("And EmpId=" + int.Parse(ddlEmployee.SelectedValue));
            }
            
        }
        else if (pStrNavigation.Trim().ToUpper() == "PREV")
        {
            if (HidFldId.Value == "")
            {
                LblMsg.Text = "Data not found";
                return;
            }
            StrSql.AppendLine("Select Top 1 Id From WorkAssDet Where Id < " + int.Parse(HidFldId.Value));
            if (int.Parse(ddlEmployee.SelectedValue) != 0)
            {
                StrSql.AppendLine("And EmpId=" + int.Parse(ddlEmployee.SelectedValue));
            }
            StrSql.AppendLine("Order By Id Desc");
        }
        else if (pStrNavigation.Trim().ToUpper() == "LAST")
        {
            StrSql.AppendLine("Select Top 1 Id From WorkAssDet ");
            if (int.Parse(ddlEmployee.SelectedValue) != 0)
            {
                StrSql.AppendLine("Where EmpId=" + int.Parse(ddlEmployee.SelectedValue));
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

        StrSql.AppendLine("Select W.Id,W.PrjLedId,W.PrjId,W.PrjModId,CM.ClientName,W.EmpId");
        StrSql.AppendLine(",Convert(Varchar(10),W.AssignDate,103) As AssignDate,W.AssignTime");
        StrSql.AppendLine(",W.DueDays,Convert(Varchar(10),W.DueDate,103) As DueDate");
        StrSql.AppendLine(",W.WorkDet1,W.WorkDet2,W.WorkDet3,W.WorkStatus");
        StrSql.AppendLine(",Case When IsNull(W.CompleteDate,'')<>''");
        StrSql.AppendLine("      Then Convert(Varchar(10),W.CompleteDate,103) Else '' End  As CompleteDate");
        StrSql.AppendLine(",W.CompleteTime,W.Remark");
        StrSql.AppendLine("From WorkAssDet W");
        StrSql.AppendLine("Left Join Emp_Mast PL On PL.Id=W.PrjLedId");
        StrSql.AppendLine("Left Join Emp_Mast E On E.Id=W.EmpId");
        StrSql.AppendLine("Left Join Project_Mast P On P.Id=W.PrjId");
        StrSql.AppendLine("Left Join Client_Mast CM On CM.Id=P.ClientId");
        StrSql.AppendLine("Left Join Project_Module M On M.Id=W.PrjModId");
        StrSql.AppendLine("Where W.Id=" + IntId);

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

        DDLPrjLed.Items.Clear();
        DDLPrjLed.DataSource = BLayer.FillEmp(0, " And IsNull(EmpGroup,'')='Ledger'");
        DDLPrjLed.DataValueField = "EmpId";
        DDLPrjLed.DataTextField = "EmpName";
        DDLPrjLed.DataBind();
        DDLPrjLed.Items.Insert(0, new ListItem("--Select Ledger--", "0"));

        DDLPrjName.Items.Clear();
        DDLPrjName.DataSource = BLayer.FillProject("");
        DDLPrjName.DataValueField = "PrjId";
        DDLPrjName.DataTextField = "PrjName";
        DDLPrjName.DataBind();
        DDLPrjName.Items.Insert(0, new ListItem("--Select Project--", "0"));

        DDLPrjModule.Items.Clear();
        DDLPrjModule.DataSource = BLayer.FillPrjModule(int.Parse(DDLPrjName.SelectedValue));
        DDLPrjModule.DataValueField = "ModId";
        DDLPrjModule.DataTextField = "ModName";
        DDLPrjModule.DataBind();
        DDLPrjModule.Items.Insert(0, new ListItem("--Select Module--", "0"));

        int IntEmpId = 0;
        if (ViewState["LoginUserGroup"].ToString() == "EMP")
        {
            IntEmpId = int.Parse(ViewState["LoginId"].ToString());
            ddlEmployee.Enabled = false;
        }
        ddlEmployee.Items.Clear();
        ddlEmployee.DataSource = BLayer.FillEmp(0, "");
        ddlEmployee.DataValueField = "EmpId";
        ddlEmployee.DataTextField = "EmpName";
        ddlEmployee.DataBind();
        ddlEmployee.Items.Insert(0, new ListItem("--Select Employee--", "0"));

        if (dtTemp.Rows[0]["PrjLedId"].ToString() != "")
        {
            DDLPrjLed.SelectedValue = dtTemp.Rows[0]["PrjLedId"].ToString();
        }
        if (dtTemp.Rows[0]["PrjId"].ToString() != "")
        {
            DDLPrjName.SelectedValue = dtTemp.Rows[0]["PrjId"].ToString();
        }
        if (dtTemp.Rows[0]["PrjModId"].ToString() != "")
        {
            DDLPrjModule.SelectedValue = dtTemp.Rows[0]["PrjModId"].ToString();
        }
        if (dtTemp.Rows[0]["EmpId"].ToString() != "")
        {
            ddlEmployee.SelectedValue = dtTemp.Rows[0]["EmpId"].ToString();
        }
        ddlEmployee.Enabled = false;

        TxtClientName.Text = dtTemp.Rows[0]["ClientName"].ToString();
        TxtAssignDate.Text = dtTemp.Rows[0]["AssignDate"].ToString();
        TxtAssignTime.Text = dtTemp.Rows[0]["AssignTime"].ToString();
        TxtDueDays.Text = dtTemp.Rows[0]["DueDays"].ToString();
        TxtDueDate.Text = dtTemp.Rows[0]["DueDate"].ToString();
        TxtWrkDetails1.Text = dtTemp.Rows[0]["WorkDet1"].ToString();
        TxtWrkDetails2.Text = dtTemp.Rows[0]["WorkDet2"].ToString();
        TxtWrkDetails3.Text = dtTemp.Rows[0]["WorkDet3"].ToString();

        //ddlWrkStatus.Text = dtTemp.Rows[0]["WorkStatus"].ToString();
        if (dtTemp.Rows[0]["WorkStatus"].ToString() != "")
        {
            ddlWrkStatus.SelectedValue = dtTemp.Rows[0]["WorkStatus"].ToString();
        }

        TxtCompleteDate.Text = dtTemp.Rows[0]["CompleteDate"].ToString();
        TxtCompleteTime.Text = dtTemp.Rows[0]["CompleteTime"].ToString();
        TxtRemark.Text = dtTemp.Rows[0]["Remark"].ToString();
    }
}