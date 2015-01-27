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

public partial class Transaction_MthAttSalGen : System.Web.UI.Page
{
    BAL BLayer = new BAL();
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
                }
                else
                {
                    //For Only Admin Level User
                    BtnRow.Visible = true;
                }

                ClearAll();
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
            LblMsg.Text = "";

            HidFldId.Value = "";

            FillYear();
            ddlmonth.SelectedIndex = 0;

            TxtFDate.Text = "";
            TxtTDate.Text = "";
            TxtRemark.Text = "";

            ddlEmployee.Enabled = true;
            ddlEmployee.Items.Clear();
            ddlEmployee.DataSource = BLayer.FillEmp(0,"");
            ddlEmployee.DataValueField = "EmpId";
            ddlEmployee.DataTextField = "EmpName";
            ddlEmployee.DataBind();
            ddlEmployee.Items.Insert(0, new ListItem("--Select Employee--", "0"));

            GridSal.DataSource = null;
            GridSal.DataBind();
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
            StrSql.AppendLine("Select M.TransactionId,M.EmpId,E.EmpName As EmpName");
            StrSql.AppendLine(",M.Present,M.Absent,M.WorkHours,M.ExtraOrCut");
            StrSql.AppendLine(",M.BasicRate,M.PerDaySal,M.PerHourSal");
            StrSql.AppendLine(",M.CutSal,M.GiveSal,M.WorkSal,M.ExtraOrCutSal");
            StrSql.AppendLine("From MTHSALLARY M");
            StrSql.AppendLine("Left Join Emp_Mast E On M.EmpId=E.Id");
            StrSql.AppendLine("Where M.MMonth='" + ddlmonth.SelectedItem.Text.ToString() + "'");
            StrSql.AppendLine("And M.MYear=" + int.Parse(ddlyear.SelectedValue.ToString()));

            if (ddlEmployee.SelectedValue.ToString() != "0")
            {
                StrSql.AppendLine("And M.EmpId=" + int.Parse(ddlEmployee.SelectedValue.ToString()));   
            }         
            StrSql.AppendLine("Order By E.EmpName");
            dtTemp = new DataTable();

            dtTemp = SqlFunc.ExecuteDataTable(StrSql.ToString().Replace("\r\n"," "));
            GridSal.DataSource = dtTemp;
            GridSal.DataBind();
            if (dtTemp.Rows.Count != 0)
            {
                LblMsg.Text = "";                
            }
            else
            {              
                LblMsg.Text = "Data Not Available !!!";
                ddlyear.Focus();                
            }

        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    protected void btnYes_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //getting userid of particular row
            int Salid = Convert.ToInt32(Session["Id"]);

            StrSql = new StringBuilder();
            StrSql.Length = 0;
            StrSql.AppendLine("Delete From MTHSALLARY Where TransactionId=@Id");
            Cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);
            Cmd.Parameters.AddWithValue("@Id", Salid);
            SqlFunc.ExecuteNonQuery(Cmd);

            FillGrid();
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }
     
    protected void btnSalDelete_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton btndetails = sender as ImageButton;
        GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
        Session["Id"] = GridSal.DataKeys[gvrow.RowIndex].Value.ToString();
        lblUser.Text = "Are you sure you want to delete this entry ? ";
        ModalPopupExtender1.Show();
    }

    protected void btnSalSelect_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
           //
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
            if (ddlyear.SelectedValue.ToString() == "0")
            {
                LblMsg.Text = "Select Year !!!";
                ddlyear.Focus();
                return;
            }

            if (ddlmonth.SelectedValue.ToString() == "0")
            {
                LblMsg.Text = "Select Month !!!";
                ddlmonth.Focus();
                return;
            }

            FillGrid();
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    protected void BtnGenSal_Click(object sender, EventArgs e)
    {
        try
        {
            //if (ddlmonth.SelectedValue.ToString() == "0")
            //{
            //    LblMsg.Text = "Select Month !!!";
            //    ddlmonth.Focus();
            //    return;
            //}

            //if (ddlyear.SelectedValue.ToString()  == "0")
            //{
            //    LblMsg.Text = "Select Year !!!";
            //    ddlyear.Focus();
            //    return;
            //}

            string FDate, TDate;

            FDate = ValueConvert.ConvertDate(TxtFDate.Text.ToString());
            TDate = ValueConvert.ConvertDate(TxtTDate.Text.ToString());

            StrSql = new StringBuilder();
            StrSql.Length = 0;

            StrSql.AppendLine("Select D.EmpId,E.EmpName,D.Working_Month,E.BasicRate");
            StrSql.AppendLine(",(ISNULL(E.BasicRate,0) / 30) As PerDay_Salary");
            StrSql.AppendLine(",((ISNULL(E.BasicRate,0) / 30) / ISNULL(E.TotTime,0)) As PerHours_Salary");
            StrSql.AppendLine("");
            StrSql.AppendLine(",DateDiff(dd,DateAdd(dd, 1-Day('" + FDate + "'),'" + FDate + "'),dateadd(m,1,dateadd(dd, 1-day('" + FDate + "'),'" + FDate + "'))) Month_Days");
            StrSql.AppendLine(",DATEDIFF(wk, '" + FDate + "', '" + TDate + "') As Sunday");
            StrSql.AppendLine(",Count(1) As Present");
            StrSql.AppendLine(",DateDiff(dd,DateAdd(dd, 1-Day('" + FDate + "'),'" + FDate + "'),dateadd(m,1,dateadd(dd, 1-day('" + FDate + "'),'" + FDate + "'))) -");
            StrSql.AppendLine(" (DATEDIFF(wk, '" + FDate + "', '" + TDate + "')+(Count(1))) As Absent");
            StrSql.AppendLine(",(Count(1)) * (Round((ISNULL(E.BasicRate,0) / 30),0)) As Give_Salary");
            StrSql.AppendLine(",((DateDiff(dd,DateAdd(dd, 1-Day('" + FDate + "'),'" + FDate + "'),dateadd(m,1,dateadd(dd, 1-day('" + FDate + "'),'" + FDate + "')))) -");
            StrSql.AppendLine(" (DATEDIFF(wk, '" + FDate + "', '" + TDate + "')+(Count(1)))) * Round((ISNULL(E.BasicRate,0) / 30),0) As Cut_Salary");
            StrSql.AppendLine("");
            StrSql.AppendLine(",E.TotTime As JobTime");
            StrSql.AppendLine(",((DateDiff(dd,DateAdd(dd, 1-Day('" + FDate + "'),'" + FDate + "'),dateadd(m,1,dateadd(dd, 1-day('" + FDate + "'),'" + FDate + "'))) -");
            StrSql.AppendLine(" DATEDIFF(wk, '" + FDate + "', '" + TDate + "')) * IsNull(D.OffiHours,0)) As TotHours");
            StrSql.AppendLine(",SUM(IsNull(D.TotWork_Hours,0)) As WorkHours");
            StrSql.AppendLine(",SUM(IsNull(D.OverTime,0)) As OverTime");
            StrSql.AppendLine(",SUM(IsNull(D.LessTime,0)) As LessTime");
            StrSql.AppendLine(",SUM(IsNull(D.OverTime,0))-SUM(IsNull(D.LessTime,0)) As ExtraOrCut");
            StrSql.AppendLine("");
            StrSql.AppendLine(",(ISNULL(E.BasicRate,0) / 30) / ISNULL(E.TotTime,0) * ");
            StrSql.AppendLine(" SUM(IsNull(D.TotWork_Hours,0)) As WorkSalary");
            StrSql.AppendLine(",(ISNULL(E.BasicRate,0) / 30) / ISNULL(E.TotTime,0) * ");
            StrSql.AppendLine(" (SUM(IsNull(D.OverTime,0))-SUM(IsNull(D.LessTime,0))) As ExtraOrCutSalary");
            StrSql.AppendLine("");
            StrSql.AppendLine("From DailyAttendance D");
            StrSql.AppendLine("Left Join Emp_Mast E On D.EmpId=E.Id");
            StrSql.AppendLine("");            
            StrSql.AppendLine("Where D.Working_Month='" + ddlmonth.SelectedItem.Text.ToString() + "'");
            StrSql.AppendLine("And DATEPART(YY,D.Working_Date)='" + int.Parse(ddlyear.SelectedValue) +"'");
            StrSql.AppendLine("And Cast(D.Working_Month As VarChar(25))+' '+Cast(DATEPART(YY,D.Working_Date) As VarChar(25))+' '+Cast(D.EmpId As VarChar(10))");
            StrSql.AppendLine("    Not In (Select Cast(MMonth As VarChar(25))+' '+Cast(MYear As VarChar(25))+' '+Cast(EmpId As VarChar(10)) From MthSallary)");
            if (ddlEmployee.SelectedValue.ToString() != "0")
            {
                StrSql.AppendLine("And D.EmpId In (" + int.Parse(ddlEmployee.SelectedValue) + ")");
            }           
            StrSql.AppendLine("");
            StrSql.AppendLine("Group By D.EmpId,E.EmpName,E.BasicRate,E.TotTime,D.Working_Month");
            StrSql.AppendLine(",IsNull(D.OffiHours,0)");

            dtTemp = new DataTable();
            dtTemp = SqlFunc.ExecuteDataTable(StrSql.ToString().Replace("\r\n"," "));

            if (dtTemp.Rows.Count != 0)
            {
                SqlFunc.BeginConnWithTransaction();

                int i;
                for (i = 0; i <= dtTemp.Rows.Count - 1; i++)
                {
                    int IntNewTransId=0;

                    StrSql =new StringBuilder();
                    StrSql.Length = 0;
                    StrSql.AppendLine("Select IsNull(Max(IsNull(TransactionId,0)),0) + 1 As TransId From MthSallary ");
                    DataTable dtTrnId = SqlFunc.ExecuteTransTable(StrSql.ToString().Replace("\r\n", " "));
                    if (dtTrnId.Rows.Count != 0)
                    {
                        IntNewTransId = Convert.ToInt16(dtTrnId.Rows[0]["TransId"].ToString());
                    }
                    
                    StrSql = new StringBuilder();
                    StrSql.Length = 0;

                    StrSql.AppendLine("Insert Into MthSallary(");
                    StrSql.AppendLine("TransactionId");
                    StrSql.AppendLine(",EmpId");
                    StrSql.AppendLine(",MMonth");
                    StrSql.AppendLine(",MYear");
                    StrSql.AppendLine(",FromDate");
                    StrSql.AppendLine(",ToDate");
                    StrSql.AppendLine(",MthDays");
                    StrSql.AppendLine(",WOffDays");
                    StrSql.AppendLine(",Present");
                    StrSql.AppendLine(",Absent");
                    StrSql.AppendLine(",JobTime");
                    StrSql.AppendLine(",TotHours");
                    StrSql.AppendLine(",WorkHours");
                    StrSql.AppendLine(",OverTime");
                    StrSql.AppendLine(",LessTime");
                    StrSql.AppendLine(",ExtraOrCut");
                    StrSql.AppendLine(",BasicRate");
                    StrSql.AppendLine(",PerDaySal");
                    StrSql.AppendLine(",PerHourSal");
                    StrSql.AppendLine(",CutSal");
                    StrSql.AppendLine(",GiveSal");
                    StrSql.AppendLine(",WorkSal");
                    StrSql.AppendLine(",ExtraOrCutSal");
                    StrSql.AppendLine(",Entry_Date,Entry_Time,Entry_UID,UPDFLAG");
                    StrSql.AppendLine(")");
                    StrSql.AppendLine("Values(");
                    StrSql.AppendLine("@TransactionId");
                    StrSql.AppendLine(",@EmpId");
                    StrSql.AppendLine(",@MMonth");
                    StrSql.AppendLine(",@MYear");
                    StrSql.AppendLine(",@FromDate");
                    StrSql.AppendLine(",@ToDate");
                    StrSql.AppendLine(",@MthDays");
                    StrSql.AppendLine(",@WOffDays");
                    StrSql.AppendLine(",@Present");
                    StrSql.AppendLine(",@Absent");
                    StrSql.AppendLine(",@JobTime");
                    StrSql.AppendLine(",@TotHours");
                    StrSql.AppendLine(",@WorkHours");
                    StrSql.AppendLine(",@OverTime");
                    StrSql.AppendLine(",@LessTime");
                    StrSql.AppendLine(",@ExtraOrCut");
                    StrSql.AppendLine(",@BasicRate");
                    StrSql.AppendLine(",@PerDaySal");
                    StrSql.AppendLine(",@PerHourSal");
                    StrSql.AppendLine(",@CutSal");
                    StrSql.AppendLine(",@GiveSal");
                    StrSql.AppendLine(",@WorkSal");
                    StrSql.AppendLine(",@ExtraOrCutSal");
                    StrSql.AppendLine(",GetDate(),Convert(VarChar,GetDate(),108),@Entry_UID,0");
                    StrSql.AppendLine(")");

                    Cmd = new SqlCommand(StrSql.ToString().Replace("\r\n", " "), SqlFunc.gConn);

                    Cmd.Parameters.AddWithValue("@TransactionId",IntNewTransId);
                    Cmd.Parameters.AddWithValue("@EmpId", int.Parse(dtTemp.Rows[i]["EmpId"].ToString()));
                    Cmd.Parameters.AddWithValue("@MMonth", ddlmonth.SelectedItem.Text.ToString());
                    Cmd.Parameters.AddWithValue("@MYear",int.Parse(ddlyear.SelectedValue));
                    Cmd.Parameters.AddWithValue("@FromDate",ValueConvert.ConvertDate(TxtFDate.Text.ToString()));
                    Cmd.Parameters.AddWithValue("@ToDate",ValueConvert.ConvertDate(TxtTDate.Text.ToString()));
                    Cmd.Parameters.AddWithValue("@MthDays",int.Parse(dtTemp.Rows[i]["Month_Days"].ToString()));
                    Cmd.Parameters.AddWithValue("@WOffDays",int.Parse(dtTemp.Rows[i]["Sunday"].ToString()));
                    Cmd.Parameters.AddWithValue("@Present",double.Parse(dtTemp.Rows[i]["Present"].ToString()));
                    Cmd.Parameters.AddWithValue("@Absent",double.Parse(dtTemp.Rows[i]["Absent"].ToString()));
                    Cmd.Parameters.AddWithValue("@JobTime",dtTemp.Rows[i]["JobTime"].ToString());
                    Cmd.Parameters.AddWithValue("@TotHours",double.Parse(dtTemp.Rows[i]["TotHours"].ToString()));
                    Cmd.Parameters.AddWithValue("@WorkHours",double.Parse(dtTemp.Rows[i]["WorkHours"].ToString()));
                    Cmd.Parameters.AddWithValue("@OverTime",double.Parse(dtTemp.Rows[i]["OverTime"].ToString()));
                    Cmd.Parameters.AddWithValue("@LessTime",double.Parse(dtTemp.Rows[i]["LessTime"].ToString()));
                    Cmd.Parameters.AddWithValue("@ExtraOrCut",double.Parse(dtTemp.Rows[i]["ExtraOrCut"].ToString()));
                    Cmd.Parameters.AddWithValue("@BasicRate",double.Parse(dtTemp.Rows[i]["BasicRate"].ToString()));
                    Cmd.Parameters.AddWithValue("@PerDaySal",double.Parse(dtTemp.Rows[i]["PerDay_Salary"].ToString()));
                    Cmd.Parameters.AddWithValue("@PerHourSal",double.Parse(dtTemp.Rows[i]["PerHours_Salary"].ToString()));
                    Cmd.Parameters.AddWithValue("@CutSal",double.Parse(dtTemp.Rows[i]["Cut_Salary"].ToString()));
                    Cmd.Parameters.AddWithValue("@GiveSal",double.Parse(dtTemp.Rows[i]["Give_Salary"].ToString()));
                    Cmd.Parameters.AddWithValue("@WorkSal",double.Parse(dtTemp.Rows[i]["WorkSalary"].ToString()));
                    Cmd.Parameters.AddWithValue("@ExtraOrCutSal",double.Parse(dtTemp.Rows[i]["ExtraOrCutSalary"].ToString()));                    
                    Cmd.Parameters.AddWithValue("@Entry_UID",HidFldUID.Value.ToString());

                    SqlFunc.ExecuteNonQuery(Cmd);                                        
                }
                FillGrid();
                LblMsg.Text = "Salary Generate Successfully !!!";
            }
            else
            {
                GridSal.DataSource = null;
                GridSal.DataBind();
                LblMsg.Text = "Data Not Found For Generate Salary !!!";
                return;
            }            
        }
        catch (Exception ex)
        {
            SqlFunc.RollbackTransaction();
            Response.Write(ex.ToString());
        }
        finally
        {
            SqlFunc.EndConnWithTransaction();            
        }
    }

    protected void BtnNew_Click(object sender, EventArgs e)
    {
        try
        {
            ClearAll();
            ddlyear.Focus();
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
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    protected void GridSal_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GridSal.PageIndex = e.NewPageIndex;
            FillGrid();
            LblMsg.Text = "";
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    protected void FillYear()
    {       
        int i,IntYear;

        IntYear = DateTime.Today.Year;

        ddlyear.Items.Clear();
        ddlyear.Items.Insert(0, new ListItem("--Select Year--", "0"));
        ddlyear.Items.Insert(1, new ListItem(IntYear.ToString(), IntYear.ToString()));
        for (i =2; i <= 100; i++)
        {             
            IntYear = IntYear - 1;
            ddlyear.Items.Insert(i, new ListItem(IntYear.ToString(), IntYear.ToString()));
        }
    }

    protected void GetFrmToDate()
    {
        if (ddlyear.SelectedValue == "0" || ddlmonth.SelectedValue == "0")
        {
            TxtFDate.Text = "";
            TxtTDate.Text = "";
            return;
        }
        int days = DateTime.DaysInMonth(int.Parse(ddlyear.SelectedValue), int.Parse(ddlmonth.SelectedValue));
        string FrmDate, ToDate;

        FrmDate = "01/" + ddlmonth.SelectedValue + "/" + ddlyear.SelectedValue;
        ToDate = days + "/" + ddlmonth.SelectedValue + "/" + ddlyear.SelectedValue;

        TxtFDate.Text = FrmDate;
        TxtTDate.Text = ToDate;
    }

    protected void ddlmonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GetFrmToDate();
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    protected void ddlyear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GetFrmToDate();
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }
}