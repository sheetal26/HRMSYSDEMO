using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Globalization;
using HRMSystem;

public partial class Transaction_DailyAttandance : System.Web.UI.Page
{
    BAL BLayer=new BAL();
    StringBuilder StrSql;
    SqlFunction SqlFunc = new SqlFunction();
    SqlCommand Cmd = new SqlCommand();
    DataTable dtTemp;
    DataSet ds = new DataSet();
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
                    ViewState["LoginUserGroup"] = Session["LoginUserGrp"].ToString();
                    ViewState["LoginId"] = Session["LoginId"].ToString();
                }

                if (ViewState["LoginUserGroup"].ToString() == "EMP")
                {
                    BtnRow.Visible = false; //Save,New,Cancle,Delete Button Panel
                    //EmpSearchRow.Visible = false; //Row For View Data Base On Employee Selection
                }
                else
                {
                    //For Only Admin Level User
                    BtnRow.Visible = true;
                    //EmpSearchRow.Visible = true;
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

    private void FillGrid()
    {
        try
        {
            int IntEmpId=0;
            if (DDLEmpView.SelectedValue != "0")
            {
                IntEmpId = int.Parse(DDLEmpView.SelectedValue);
            }

            StrSql = new StringBuilder();
            StrSql.Length = 0;
            StrSql.AppendLine("Select D.Id,E.EmpName,Convert(VarChar(10),D.Working_Date,103) As WorkDate");
            StrSql.AppendLine(",D.InTime,D.OutTime");
            StrSql.AppendLine(",Convert(Numeric(9,2),ISNULL(D.OutTime,0))-Convert(Numeric(9,2),ISNULL(D.InTime,0)) As TotTime");
            StrSql.AppendLine("From DailyAttendance D");
            StrSql.AppendLine("Left Join Emp_Mast E On D.EmpId=E.Id");
            StrSql.AppendLine("Where 1=1 ");
            if (ViewState["LoginUserGroup"].ToString() == "EMP")
            {
                StrSql.AppendLine("And D.EmpId=" + int.Parse(ViewState["LoginId"].ToString()) );
            }
            else
            {
                if (IntEmpId != 0)
                {
                    StrSql.AppendLine("And D.EmpId=" + IntEmpId);
                }
            }

            if (ddlmonth.SelectedValue != "0")
            {
                StrSql.AppendLine("And D.Working_Month='" + ddlmonth.SelectedItem.Text.ToString()  + "'");
            }

            StrSql.AppendLine("Order By E.EmpName,Convert(VarChar(10),D.Working_Date,101)");

            dtTemp = new DataTable();
            dtTemp = SqlFunc.ExecuteDataTable(StrSql.ToString());

            GridInOut.DataSource = dtTemp;
            GridInOut.DataBind();

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
            int IntEmpId = 0;
            if (ViewState["LoginUserGroup"].ToString() == "EMP")
            {
                IntEmpId = int.Parse(ViewState["LoginId"].ToString());
                ddlEmployee.Enabled = false;
                DDLEmpView.Enabled = false;
            }
            else
            {
                ddlEmployee.Enabled = true;
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
                        
            ddlEmployee.Items.Clear();
            ddlEmployee.DataSource = BLayer.FillEmp(IntEmpId,"");
            ddlEmployee.DataValueField = "EmpId";
            ddlEmployee.DataTextField = "EmpName";
            ddlEmployee.DataBind();
            if (IntEmpId == 0)
            {
                ddlEmployee.Items.Insert(0, new ListItem("--Select Employee--", "0"));
            }

            LblMsg.Text = "";
            HidFldId.Value = "";

            TxtOffFTime.Text = "";
            TxtOffTTime.Text = "";
            TxtOffHours.Text = "";
            
            TxtDays.Text = "";
            TxtMonth.Text = "";
            TxtInTime.Text = "";
            TxtOutTime.Text = "";
            TxtTotWrkHours.Text = "";
            TxtOverTime.Text = "";
            TxtLessTime.Text = "";
            TxtReason.Text = "";
            TxtRemark.Text = "";

            TxtDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            GetDayMonth();
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    protected void ddlEmployee_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            TxtOffFTime.Text = "";
            TxtOffTTime.Text = "";
            TxtOffHours.Text = "";

            if (ddlEmployee.SelectedValue != "0")
            {
                StrSql = new StringBuilder();
                StrSql.Length = 0;
                StrSql.AppendLine("Select FJobTime,TJobTime");
                StrSql.AppendLine(",Abs(CAST(REPLACE(FJobTime,':','.') As Numeric(5,2))-");
                StrSql.AppendLine(" CAST(REPLACE(TJobTime,':','.') As Numeric(5,2))) As TotHours");
                StrSql.AppendLine("From Emp_Mast ");
                StrSql.AppendLine("Where Id=" + int.Parse(ddlEmployee.SelectedValue));

                dtTemp = new DataTable();
                dtTemp = SqlFunc.ExecuteDataTable(StrSql.ToString());

                if (dtTemp.Rows.Count != 0)
                {
                    TxtOffFTime.Text = dtTemp.Rows[0]["FJobTime"].ToString();
                    TxtOffTTime.Text = dtTemp.Rows[0]["TJobTIme"].ToString ();
                    TxtOffHours.Text = dtTemp.Rows[0]["TotHours"].ToString();

                    TxtInTime.Text = TxtOffFTime.Text;
                    TxtOutTime.Text = TxtOffTTime.Text;
                    GetTotTime();
                    TxtDate.Focus();
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    protected void GridInOut_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GridInOut.PageIndex = e.NewPageIndex;
            FillGrid();
            LblMsg.Text = "";
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    //protected void GridInOut_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    try
    //    {
    //        StrSql = new StringBuilder();
    //        StrSql.Length = 0;

    //        StrSql.AppendLine("Delete From DailyAttendance Where Id=@Id ");
    //        Cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);
    //        Cmd.Parameters.AddWithValue("@Id", int.Parse(GridInOut.DataKeys[e.RowIndex].Value.ToString()));
    //        SqlFunc.ExecuteNonQuery(Cmd);

    //        GridInOut.EditIndex = -1;
    //        FillGrid();
    //        LblMsg.Text = "Entry deleted successfully";
    //    }
    //    catch (Exception ex)
    //    {
    //        Response.Write(ex);
    //    }
    //}

    protected void btnInOutDelete_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton btndetails = sender as ImageButton;
        GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
        Session["Id"] = GridInOut.DataKeys[gvrow.RowIndex].Value.ToString();
        lblUser.Text = "Are you sure you want to delete this entry ? ";
        ModalPopupExtender1.Show();
    }

    protected void btnYes_Click(object sender, ImageClickEventArgs e)
    {
        try
        {          
            int InOutid = Convert.ToInt32(Session["Id"]);
            
            StrSql = new StringBuilder();
            StrSql.Length = 0;
            StrSql.AppendLine("Delete From DailyAttendance Where Id=@Id");
            Cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);
            Cmd.Parameters.AddWithValue("@Id", InOutid);
            SqlFunc.ExecuteNonQuery(Cmd);

            FillGrid();
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    protected void btnInOutSelect_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton btndetails = sender as ImageButton;
            GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
            Session["Id"] = GridInOut.DataKeys[gvrow.RowIndex].Value.ToString();

            HidFldId.Value = Session["Id"].ToString();

            StrSql = new StringBuilder();
            StrSql.Length = 0;

            StrSql = new StringBuilder();
            StrSql.Length = 0;
            StrSql.AppendLine("Select D.Id,D.EmpId");
            StrSql.AppendLine(",D.OffiFTime,D.OffiTTime,D.OffiHours");
            StrSql.AppendLine(",Convert(Varchar(10),D.Working_Date,103) As Working_Date,D.Working_Days,D.Working_Month");
            StrSql.AppendLine(",D.InTime,D.OutTime,D.TotWork_Hours");
            StrSql.AppendLine(",D.OverTime,D.LessTime");
            StrSql.AppendLine(",D.Reason,D.Remark");
            StrSql.AppendLine("From DailyAttendance D");
            StrSql.AppendLine("Where D.Id=" + int.Parse(HidFldId.Value));

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
                ddlEmployee.Enabled = false;                
            }           
            ddlEmployee.Items.Clear();
            ddlEmployee.DataSource = BLayer.FillEmp(IntEmpId,"");
            ddlEmployee.DataValueField = "EmpId";
            ddlEmployee.DataTextField = "EmpName";
            ddlEmployee.DataBind();
            ddlEmployee.Items.Insert(0, new ListItem("--Select Employee--", "0"));
            

            if (dtTemp.Rows[0]["EmpId"].ToString() != "")
            {
                ddlEmployee.SelectedValue = dtTemp.Rows[0]["EmpId"].ToString();
            }
           
            TxtOffFTime.Text = dtTemp.Rows[0]["OffiFTime"].ToString();
            TxtOffTTime.Text = dtTemp.Rows[0]["OffiTTime"].ToString();
            TxtOffHours.Text = dtTemp.Rows[0]["OffiHours"].ToString();
            TxtDate.Text = dtTemp.Rows[0]["Working_Date"].ToString();
            TxtDays.Text = dtTemp.Rows[0]["Working_Days"].ToString();
            TxtMonth.Text = dtTemp.Rows[0]["Working_Month"].ToString();
            TxtInTime.Text = dtTemp.Rows[0]["InTime"].ToString();
            TxtOutTime.Text = dtTemp.Rows[0]["OutTime"].ToString();
            TxtTotWrkHours.Text = dtTemp.Rows[0]["TotWork_Hours"].ToString();
            TxtOverTime.Text = dtTemp.Rows[0]["OverTime"].ToString();
            TxtLessTime.Text = dtTemp.Rows[0]["LessTime"].ToString();
            TxtReason.Text = dtTemp.Rows[0]["Reason"].ToString();
            TxtRemark.Text = dtTemp.Rows[0]["Remark"].ToString();

            ddlEmployee.Enabled = false;

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

    //protected void GridInOut_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        HidFldId.Value = GridInOut.DataKeys[GridInOut.SelectedRow.RowIndex].Value.ToString();

    //        StrSql = new StringBuilder();
    //        StrSql.Length = 0;

    //        StrSql = new StringBuilder();
    //        StrSql.Length = 0;
    //        StrSql.AppendLine("Select D.Id,D.EmpId");
    //        StrSql.AppendLine(",D.OffiFTime,D.OffiTTime,D.OffiHours");
    //        StrSql.AppendLine(",Convert(Varchar(10),D.Working_Date,103) As Working_Date,D.Working_Days,D.Working_Month");
    //        StrSql.AppendLine(",D.InTime,D.OutTime,D.TotWork_Hours");
    //        StrSql.AppendLine(",D.OverTime,D.LessTime");
    //        StrSql.AppendLine(",D.Reason,D.Remark");
    //        StrSql.AppendLine("From DailyAttendance D");            
    //        StrSql.AppendLine("Where D.Id=" + int.Parse(HidFldId.Value));

    //        dtTemp = new DataTable();
    //        dtTemp = SqlFunc.ExecuteDataTable(StrSql.ToString());

    //        if (dtTemp.Rows.Count == 0)
    //        {
    //            LblMsg.Text = "Data not found";
    //            return;
    //        }

    //        ddlEmployee.Items.Clear();
    //        ddlEmployee.DataSource = BLayer.FillEmp("");
    //        ddlEmployee.DataValueField = "EmpId";
    //        ddlEmployee.DataTextField = "EmpName";
    //        ddlEmployee.DataBind();
    //        ddlEmployee.Items.Insert(0, new ListItem("--Select Employee--", "0"));

    //        if (dtTemp.Rows[0]["EmpId"].ToString() != "")
    //        {
    //            ddlEmployee.SelectedValue = dtTemp.Rows[0]["EmpId"].ToString();
    //        }

    //        TxtOffFTime.Text = dtTemp.Rows[0]["OffiFTime"].ToString();
    //        TxtOffTTime.Text = dtTemp.Rows[0]["OffiTTime"].ToString();
    //        TxtOffHours.Text = dtTemp.Rows[0]["OffiHours"].ToString();
    //        TxtDate.Text = dtTemp.Rows[0]["Working_Date"].ToString();
    //        TxtDays.Text = dtTemp.Rows[0]["Working_Days"].ToString();
    //        TxtMonth.Text = dtTemp.Rows[0]["Working_Month"].ToString();
    //        TxtInTime.Text = dtTemp.Rows[0]["InTime"].ToString();
    //        TxtOutTime.Text = dtTemp.Rows[0]["OutTime"].ToString();
    //        TxtTotWrkHours.Text = dtTemp.Rows[0]["TotWork_Hours"].ToString();
    //        TxtOverTime.Text = dtTemp.Rows[0]["OverTime"].ToString();
    //        TxtLessTime.Text = dtTemp.Rows[0]["LessTime"].ToString();
    //        TxtReason.Text = dtTemp.Rows[0]["Reason"].ToString();
    //        TxtRemark.Text = dtTemp.Rows[0]["Remark"].ToString();

    //        ddlEmployee.Enabled = false;
 
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
            if (ddlEmployee.SelectedValue == "0")
            {
                //LblMsg.Text = "Employee Name Is Blank, Enter Valid Name....";
                ddlEmployee.Focus();
                return;
            }

            if (TxtDate.Text.Length == 0)
            {
                //LblMsg.Text = "Date Is Blank, Enter Valid Date....";
                TxtDate.Focus();
                return;
            }

            if (TxtInTime.Text.Length == 0)
            {
                //LblMsg.Text = "In Time Is Blank, Enter Valid Time....";
                TxtInTime.Focus();
                return;
            }
                      
            string[] TotWrkHours = TxtTotWrkHours.Text.Split('.');
            int WrkH = ValueConvert.ValInt16(TotWrkHours[0].ToString());

            if (WrkH < 5 && TxtReason.Text.Length == 0)
            {
                LblMsg.Text = "Give Valid Reason, Because Working Hours Is Less Then 5.00...";
                TxtReason.Focus();
                return;
            }

            LblMsg.Text = "";

            StrSql = new StringBuilder();
            StrSql.Length = 0;
             
            if (HidFldId.Value.Length == 0)
            {
                 StrSql.AppendLine("Insert Into DailyAttendance");                 
                 StrSql.AppendLine("(EmpId");
                 StrSql.AppendLine(",OffiFTime");
                 StrSql.AppendLine(",OffiTTime");
                 StrSql.AppendLine(",OffiHours");
                 StrSql.AppendLine(",Working_Date");
                 StrSql.AppendLine(",Working_Days");
                 StrSql.AppendLine(",Working_Month");
                 StrSql.AppendLine(",InTime");
                 StrSql.AppendLine(",OutTime");
                 StrSql.AppendLine(",TotWork_Hours");
                 StrSql.AppendLine(",OverTime");
                 StrSql.AppendLine(",LessTime");
                 StrSql.AppendLine(",Reason");
                 StrSql.AppendLine(",Remark");
                 StrSql.AppendLine(",Entry_Date");
                 StrSql.AppendLine(",Entry_Time");
                 StrSql.AppendLine(",Entry_UID");
                 StrSql.AppendLine(",UPDFLAG");
                 StrSql.AppendLine(")");
                 StrSql.AppendLine("Values");                 
                 StrSql.AppendLine("(@EmpId");
                 StrSql.AppendLine(",@OffiFTime");
                 StrSql.AppendLine(",@OffiTTime");
                 StrSql.AppendLine(",@OffiHours");
                 StrSql.AppendLine(",@Working_Date");
                 StrSql.AppendLine(",@Working_Days");
                 StrSql.AppendLine(",@Working_Month");
                 StrSql.AppendLine(",@InTime");
                 StrSql.AppendLine(",@OutTime");
                 StrSql.AppendLine(",@TotWork_Hours");
                 StrSql.AppendLine(",@OverTime");
                 StrSql.AppendLine(",@LessTime");
                 StrSql.AppendLine(",@Reason");
                 StrSql.AppendLine(",@Remark");
                 StrSql.AppendLine(",GetDate()");
                 StrSql.AppendLine(",Convert(VarChar,GetDate(),108)");
                 StrSql.AppendLine(",@Entry_UID");
                 StrSql.AppendLine(",0"); 
                 StrSql.AppendLine(")");
            }
            else
            {
                 StrSql.AppendLine("Update DailyAttendance"); 
                 StrSql.AppendLine("Set OffiFTime=@OffiFTime");
                 StrSql.AppendLine(",OffiTTime=@OffiTTime");
                 StrSql.AppendLine(",OffiHours=@OffiHours");
                 StrSql.AppendLine(",Working_Date=@Working_Date");
                 StrSql.AppendLine(",Working_Days=@Working_Days");
                 StrSql.AppendLine(",Working_Month=@Working_Month");
                 StrSql.AppendLine(",InTime=@InTime");
                 StrSql.AppendLine(",OutTime=@OutTime");
                 StrSql.AppendLine(",TotWork_Hours=@TotWork_Hours");
                 StrSql.AppendLine(",OverTime=@OverTime");
                 StrSql.AppendLine(",LessTime=@LessTime");
                 StrSql.AppendLine(",Reason=@Reason");
                 StrSql.AppendLine(",Remark=@Remark"); 
                 StrSql.AppendLine(",MEntry_Date=GetDate(),MEntry_Time=Convert(Varchar,GetDate(),108)");
                 StrSql.AppendLine(",MEntry_UID=@Entry_UID");
                 StrSql.AppendLine(",UPDFLAG=IsNull(UPDFlag,0)+1"); 
                 StrSql.AppendLine("Where Id=@Id And EmpId=@EmpId");
            }

            Cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);
           
            if (ddlEmployee.SelectedValue != "0")
            {
                Cmd.Parameters.AddWithValue("@EmpId", ddlEmployee.SelectedValue);
            }
            else
            {
                Cmd.Parameters.AddWithValue("@EmpId", DBNull.Value);
            }

            Cmd.Parameters.AddWithValue("@OffiFTime",TxtOffFTime.Text);  
            Cmd.Parameters.AddWithValue("@OffiTTime",TxtOffTTime.Text);
            Cmd.Parameters.AddWithValue("@OffiHours",TxtOffHours.Text);                        

            if (TxtDate.Text.Length != 0)
            {
                Cmd.Parameters.AddWithValue("@Working_Date", ValueConvert.ConvertDate(TxtDate.Text));
            }
            else
            {
                Cmd.Parameters.AddWithValue("@Working_Date", DBNull.Value);
            }

            Cmd.Parameters.AddWithValue("@Working_Days",TxtDays.Text);
            Cmd.Parameters.AddWithValue("@Working_Month",TxtMonth.Text);
            Cmd.Parameters.AddWithValue("@InTime",TxtInTime.Text);
            Cmd.Parameters.AddWithValue("@OutTime",TxtOutTime.Text);
            Cmd.Parameters.AddWithValue("@TotWork_Hours",TxtTotWrkHours.Text);
            Cmd.Parameters.AddWithValue("@OverTime",TxtOverTime.Text);
            Cmd.Parameters.AddWithValue("@LessTime",TxtLessTime.Text);
            Cmd.Parameters.AddWithValue("@Reason",TxtReason.Text);
            Cmd.Parameters.AddWithValue("@Remark",TxtRemark.Text);

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

            //// Move to View Tab 
            //MyMenu.Items[0].Selected = true;
            //MyMenu.Items[0].ImageUrl = "~/Images/ViewEnable.jpg";
            //MyMultiView.ActiveViewIndex = 0;
            //MyMenu.Items[1].ImageUrl = "~/Images/NewOrEditDisable.jpg";

            ddlEmployee.Focus();
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
            ddlEmployee.Focus();
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

                    StrSql.AppendLine("Delete From DailyAttendance Where Id=@Id ");
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
    protected void TxtDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (TxtDate.Text.Length != 0)
            {
                StrSql = new StringBuilder();
                StrSql.Length = 0;
                StrSql.AppendLine("Select D.Working_Date");               
                StrSql.AppendLine("From DailyAttendance D");
                StrSql.AppendLine("Where D.EmpId=" + int.Parse(ddlEmployee.SelectedValue));
                StrSql.AppendLine("And D.Working_Date='" + ValueConvert.ConvertDate(TxtDate.Text.ToString()) + "'");                 

                dtTemp = new DataTable();
                dtTemp = SqlFunc.ExecuteDataTable(StrSql.ToString());
                if (dtTemp.Rows.Count != 0)
                {
                    LblMsg.Text = "Attendance Entry Allready Done For " + TxtDate.Text.ToString() + ".";
                    TxtDate.Text = "";
                    TxtDays.Text = "";
                    TxtMonth.Text = "";
                    TxtDate.Focus();
                    return;
                }

                LblMsg.Text = "";
                GetDayMonth();
                TxtInTime.Focus();                
            }
        }
        catch(Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    protected void GetDayMonth()
    {
        DateTime WrkDate;
        WrkDate = Convert.ToDateTime(TxtDate.Text.ToString());
        DateTime dateValue = new DateTime((int)WrkDate.Year, (int)WrkDate.Month, (int)WrkDate.Day);
        TxtDays.Text = dateValue.ToString("dddd");
        TxtMonth.Text = dateValue.ToString("MMMM");
    }

    //protected void TxtInTime_TextChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        CalcTimeDiff();
    //        TxtInTime.Focus();
    //    }
    //    catch(Exception ex)
    //    {
    //        Response.Write(ex);
    //    }
    //}

    //protected void TxtOutTime_TextChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        CalcTimeDiff();
    //        TxtOutTime.Focus();
    //    }
    //    catch (Exception ex)
    //    {
    //        Response.Write(ex);
    //    }
    //}

    //protected void CalcTimeDiff()
    //{
    //    TxtOverTime.Text = "0";
    //    TxtLessTime.Text = "0";

    //    if (TxtInTime.Text.Length != 0)
    //    {
    //        if (TxtOutTime.Text.Length == 0)
    //        {
    //            TxtOutTime.Text = TxtOffTTime.Text;
    //        }
            
    //        //Your DateTime values
    //        DateTime startDateTime = DateTime.Parse(TxtInTime.Text);
    //        DateTime endDateTime = DateTime.Parse(TxtOutTime.Text);

    //        //Your Timespan Result
    //        TimeSpan difference = endDateTime - startDateTime;

    //        TxtTotWrkHours.Text = difference.Hours.ToString() + '.' + difference.Minutes.ToString();

    //        Double OffiHours,TotWrkHours,HoursDiff;

    //        OffiHours = Double.Parse(TxtOffHours.Text);
    //        TotWrkHours = Double.Parse(TxtTotWrkHours.Text);
    //        HoursDiff =Math.Round(Math.Abs(OffiHours - TotWrkHours),2);

    //        if (TotWrkHours != OffiHours)
    //        {
    //            if (TotWrkHours < OffiHours)
    //            {
    //                TxtLessTime.Text = HoursDiff.ToString(); 
    //            }
    //            else
    //            {
    //                TxtOverTime.Text = HoursDiff.ToString(); 
    //            }
    //        }           
    //    }
    //}

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
        try
        {
            //string[] FTime = TxtInTime.Text.Split('.');
            //string[] TTime = TxtOutTime.Text.Split('.');
            //string[] OffTime = TxtOffHours.Text.Split('.');

            // Define two dates.
            DateTime FrmTime = new DateTime(1, 1, 1, GetTime(TxtInTime.Text.ToString(), 0), GetTime(TxtInTime.Text.ToString(), 1), 0);
            DateTime ToTime = new DateTime(1, 1, 1, GetTime(TxtOutTime.Text.ToString(), 0), GetTime(TxtOutTime.Text.ToString(), 1), 0);            

            //For Tot Working Hours
            TimeSpan TotHours = ToTime - FrmTime;
            String DiffTime = TotHours.Hours.ToString("00") + "." + TotHours.Minutes.ToString("00");
            TxtInTime.Text = GetTime(TxtInTime.Text.ToString(), 0).ToString("00") + "." + GetTime(TxtInTime.Text.ToString(), 1).ToString("00");
            TxtOutTime.Text = GetTime(TxtOutTime.Text.ToString(), 0).ToString("00") + "." + GetTime(TxtOutTime.Text.ToString(), 1).ToString("00");
            TxtTotWrkHours.Text = DiffTime.ToString();

            //For Get Diff Between Off.Hours And Working Hours
            //string[] WrkTime = TxtTotWrkHours.Text.Split('.');
            DateTime OffHours = new DateTime(1, 1, 1, GetTime(TxtOffHours.Text.ToString(), 0), GetTime(TxtOffHours.Text.ToString(), 1), 0);
            DateTime WrkHours = new DateTime(1, 1, 1, GetTime(TxtTotWrkHours.Text.ToString(), 0), GetTime(TxtTotWrkHours.Text.ToString(), 1), 0);

            TimeSpan OverLess = OffHours - WrkHours;
            String OverLessTime = OverLess.Hours.ToString("00") + "." + OverLess.Minutes.ToString("00");
            //string[] StrTime = OverLessTime.ToString().Split('.');
            int IntTimeH = Math.Abs(GetTime(OverLessTime.ToString(), 0));
            int IntTimeM = Math.Abs(GetTime(OverLessTime.ToString(), 1));
            TxtLessTime.Text = "0";
            TxtOverTime.Text = "0";
            if (OffHours < WrkHours)
            {
                TxtOverTime.Text = IntTimeH.ToString("00") + "." + IntTimeM.ToString("00");
                TxtLessTime.Text = "0";
            }

            if (OffHours > WrkHours)
            {
                TxtOverTime.Text = "0";
                TxtLessTime.Text = IntTimeH.ToString("00") + "." + IntTimeM.ToString("00");
            }

        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
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
        catch(Exception ex)
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
            StrSql.AppendLine("Select Top 1 Id From DailyAttendance");
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
            StrSql.AppendLine("Select Top 1 Id From DailyAttendance Where Id > " + int.Parse(HidFldId.Value));
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
            StrSql.AppendLine("Select Top 1 Id From DailyAttendance Where Id < " + int.Parse(HidFldId.Value));
            if (int.Parse(ddlEmployee.SelectedValue) != 0)
            {
                StrSql.AppendLine("And EmpId=" + int.Parse(ddlEmployee.SelectedValue));
            }
            StrSql.AppendLine("Order By Id Desc");
            
        }
        else if (pStrNavigation.Trim().ToUpper() == "LAST")
        {
            StrSql.AppendLine("Select Top 1 Id From DailyAttendance");
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
            else
            {
                LblMsg.Text = "Data not found";
                return;
            }
        }

        StrSql = new StringBuilder();
        StrSql.Length = 0;
        StrSql.AppendLine("Select D.Id,D.EmpId");
        StrSql.AppendLine(",D.OffiFTime,D.OffiTTime,D.OffiHours");
        StrSql.AppendLine(",Convert(Varchar(10),D.Working_Date,103) As Working_Date,D.Working_Days,D.Working_Month");
        StrSql.AppendLine(",D.InTime,D.OutTime,D.TotWork_Hours");
        StrSql.AppendLine(",D.OverTime,D.LessTime");
        StrSql.AppendLine(",D.Reason,D.Remark");
        StrSql.AppendLine("From DailyAttendance D");
        StrSql.AppendLine("Where D.Id=" + IntId);

        dtTemp = new DataTable();
        dtTemp = SqlFunc.ExecuteDataTable(StrSql.ToString());

        if (dtTemp.Rows.Count == 0)
        {
            LblMsg.Text = "Data not found";
            return;
        }

        LblMsg.Text = "";
        HidFldId.Value = IntId.ToString();
        ddlEmployee.SelectedValue = dtTemp.Rows[0]["EmpId"].ToString();
        ddlEmployee.Enabled = false;
        TxtOffFTime.Text = dtTemp.Rows[0]["OffiFTime"].ToString();
        TxtOffTTime.Text = dtTemp.Rows[0]["OffiTTime"].ToString();
        TxtOffHours.Text = dtTemp.Rows[0]["OffiHours"].ToString();
        TxtDate.Text = dtTemp.Rows[0]["Working_Date"].ToString();
        TxtDays.Text = dtTemp.Rows[0]["Working_Days"].ToString();
        TxtMonth.Text = dtTemp.Rows[0]["Working_Month"].ToString();
        TxtInTime.Text = dtTemp.Rows[0]["InTime"].ToString();
        TxtOutTime.Text = dtTemp.Rows[0]["OutTime"].ToString();
        TxtTotWrkHours.Text = dtTemp.Rows[0]["TotWork_Hours"].ToString();
        TxtOverTime.Text = dtTemp.Rows[0]["OverTime"].ToString();
        TxtLessTime.Text = dtTemp.Rows[0]["LessTime"].ToString();
        TxtReason.Text = dtTemp.Rows[0]["Reason"].ToString();
        TxtRemark.Text = dtTemp.Rows[0]["Remark"].ToString();
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