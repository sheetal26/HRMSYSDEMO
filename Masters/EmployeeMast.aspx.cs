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
using System.Web.Services;

public partial class Masters_EmployeeMast : System.Web.UI.Page
{
    BAL BLayer = new BAL();
    StringBuilder StrSql;
    SqlFunction SqlFunc = new SqlFunction();
    SqlCommand Cmd = new SqlCommand();
    DataTable dtTemp;
    DataSet dtSet = new DataSet();
    Security Sec = new Security();

    public class CountryDetails
    {
        public int CountryId;
        public string CountryName;
    }

    public class State
    {
        public string strStateId;
        public string strState;
    }

    public class City
    {
        public string strCityId;
        public string strCity;
    }

    public class Department
    {
        public string strDeptId;
        public string strDeptName;
    }

    public class Designation
    {
        public string strDesigId;
        public string strDesigName;
    }

    [WebMethod]
    public static Department[] BindDepartment()
    {
        HRMSysLinQDataContext HrmLinq = new HRMSysLinQDataContext();
        List<Department> details = new List<Department>();
        details = (from d in HrmLinq.Dept_Masts
                   orderby d.DeptName
                   select new Department
                   {
                       strDeptId = d.Id.ToString(),
                       strDeptName = d.DeptName
                   }
                 ).ToList();

        return details.ToArray();
    }

    [WebMethod]
    public static Designation[] BindDesignation(int IntDeptId)
    {
        HRMSysLinQDataContext HrmLinq = new HRMSysLinQDataContext();
        List<Designation> details =new List<Designation>();
        details = (from d in HrmLinq.Desig_Masts
                   where d.Dept_Id ==IntDeptId
                   orderby d.DesigName
                   select new Designation
                   {
                       strDesigId = d.Id.ToString(),
                       strDesigName = d.DesigName
                   }
                 ).ToList();

        return details.ToArray();
    }

    [WebMethod]
    public static CountryDetails[] BindCountry()
    {
        DataTable dt = new DataTable();
        List<CountryDetails> details = new List<CountryDetails>();

        SqlFunction SqlFunc = new SqlFunction();
        string str = "";
        str = "Select ID As CountryID,CountryName From Country_Mast Order BY CountryName";
        dt = SqlFunc.ExecuteDataTable(str);
        foreach (DataRow dtrow in dt.Rows)
        {
            CountryDetails country = new CountryDetails();
            country.CountryId = Convert.ToInt32(dtrow["CountryId"].ToString());
            country.CountryName = dtrow["CountryName"].ToString();
            details.Add(country);
        }
        return details.ToArray();
    }

    [WebMethod]
    public static State[] BindState(int IntCountryId)
    {
        HRMSysLinQDataContext HrmLinQ = new HRMSysLinQDataContext();

        List<State> details = new List<State>();
        details = (from s in HrmLinQ.State_Masts
                   where s.CountryId == IntCountryId
                   orderby s.StateName
                   select new State
                   {
                       strStateId = s.Id.ToString(),
                       strState = s.StateName
                   }).ToList();

        return details.ToArray();
    }

    [WebMethod]
    public static City[] BindCity(int IntStateId)
    {
        HRMSysLinQDataContext HrmLinQ = new HRMSysLinQDataContext();

        List<City> details = new List<City>();
        details = (from s in HrmLinQ.City_Masts
                   where s.StateId == IntStateId
                   orderby s.CityName
                   select new City
                   {
                       strCityId = s.Id.ToString(),
                       strCity = s.CityName
                   }).ToList();

        return details.ToArray();
    }

    //[WebMethod]
    //public static DataSet BindGrid()
    //{
    //    SqlFunction SqlFunc=new SqlFunction();
    //    DataSet dsTemp=new DataSet();
        
    //    StringBuilder StrSql;
    //    StrSql = new StringBuilder();
    //    StrSql.Length = 0;
    //    StrSql.AppendLine("Select Id,EmpName,EmpGroup,EMailId,MobNo");
    //    StrSql.AppendLine("From Emp_Mast");
    //    StrSql.AppendLine("Order By EmpName");

    //    dsTemp = new DataSet();
    //    dsTemp = SqlFunc.ExecuteDataSet(StrSql.ToString());

    //    return dsTemp;
    //}

    //[WebMethod]
    //public static int GetTimeWebMth(string pTime,int IntRet)
    //{
    //    int IntTime;
    //    string[] Time = pTime.ToString().Split('.');
    //    if (pTime.IndexOf(".", 0) < 0)
    //    {            
    //        if (IntRet == 1)
    //        {
    //            IntTime = 0;
    //        }
    //        else
    //        {
    //            if (Time[IntRet].ToString() == "")
    //            {
    //                IntTime = 0;
    //            }
    //            else
    //            {
    //                IntTime = int.Parse(Time[IntRet].ToString());
    //            }                
    //        }
    //    }
    //    else
    //    {
    //        if (Time[IntRet].ToString() == "")
    //        {
    //            IntTime = 0;
    //        }
    //        else
    //        {
    //            IntTime = int.Parse(Time[IntRet].ToString());
    //        }          
    //    }   
     
    //   return IntTime;
    //}

    //[WebMethod]
    //public static string GetTotTimeWebMth(string StrFJobTime, string StrTJobTime)
    //{       
    //    // Define two dates.        
    //    DateTime FrmTime = new DateTime(1, 1, 1, GetTimeWebMth(StrFJobTime.ToString(), 0), GetTimeWebMth(StrFJobTime.ToString(), 1), 0);
    //    DateTime ToTime = new DateTime(1, 1, 1, GetTimeWebMth(StrTJobTime.ToString(), 0), GetTimeWebMth(StrTJobTime.ToString(), 1), 0);

    //    // Calculate the interval between the two dates.
    //    TimeSpan interval = ToTime - FrmTime;

    //    String DiffTime = interval.Hours.ToString("00") + "." + interval.Minutes.ToString("00");

    //    //TxtFJobTime.Text = GetTimeWebMth(StrFJobTime.ToString(), 0).ToString("00") + "." + GetTimeWebMth(StrFJobTime.ToString(), 1).ToString("00");
    //    //TxtTJobTime.Text = GetTimeWebMth(StrTJobTime.ToString(), 0).ToString("00") + "." + GetTimeWebMth(StrTJobTime.ToString(), 1).ToString("00");
    //    //TxtTotHours.Text = DiffTime.ToString();

    //    return DiffTime.ToString();
    //}

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

    private void FillGrid()
    {
        try
        {
            StrSql = new StringBuilder();
            StrSql.Length = 0;
            StrSql.AppendLine("Select Id,EmpName,EmpGroup,EMailId,MobNo");
            StrSql.AppendLine("From Emp_Mast");
            StrSql.AppendLine("Order By EmpName");

            dtTemp = new DataTable();
            dtTemp = SqlFunc.ExecuteDataTable(StrSql.ToString());

            GridEmp.DataSource = dtTemp;
            GridEmp.DataBind();
          
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    protected void DDLDeptName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DDLDesignation.DataSource = BLayer.FillDesig(int.Parse(DDLDeptName.SelectedValue));
            DDLDesignation.DataValueField = "DesigId";
            DDLDesignation.DataTextField = "DesigName";
            DDLDesignation.DataBind();
            DDLDesignation.Items.Insert(0, new ListItem("--Select Designation--", "0"));

            DDLDesignation.Focus();
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

    protected void ClearAll()
    {
        try
        {
            LblMsg.Text = "";

            HidFldId.Value = "";
            TxtEmpName.Text = "";

            DDLDeptName.Items.Clear();
            DDLDeptName.DataSource = BLayer.FillDept();
            DDLDeptName.DataValueField = "DeptId";
            DDLDeptName.DataTextField = "DeptName";
            DDLDeptName.DataBind();
            DDLDeptName.Items.Insert(0, new ListItem("--Select Department--", "0"));

            DDLDesignation.Items.Clear();
            DDLDesignation.Items.Add("---Select Designation---");

            DDLEmpGroup.SelectedIndex = 0;

            TxtFJobTime.Text = "";
            TxtTJobTime.Text = "";
            TxtTotHours.Text = "";
            TxtBasicSal.Text = "";
            TxtDOJ.Text = "";
            TxtDOB.Text = "";
            rblGender.SelectedIndex = 0;
            TxtEMailId.Text = "";
            TxtPassword.Text = "";

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

            TxtAddress1.Text = "";
            TxtPinCode.Text = "";
            TxtMobNo.Text = "";
            TxtPhone.Text = "";
            ddlBloodGrp.SelectedIndex = 0;
            TxtLeftDate.Text = "";

        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    protected void DispRecord(string pStrNavigation)
    {
        int IntId=0;
        
        StrSql = new StringBuilder();
        StrSql.Length = 0;
        if (pStrNavigation == "EXAC")        
        {
            IntId = int.Parse(HidFldId.Value);
        }
        else if (pStrNavigation.Trim().ToUpper() == "FIRST")
        {
            StrSql.AppendLine("Select Top 1 Id From Emp_Mast Order By Id");
        }
        else if (pStrNavigation.Trim().ToUpper() == "NEXT")
        {
            if (HidFldId.Value == "")
            {
                LblMsg.Text = "Data not found";
                return;
            }
            StrSql.AppendLine("Select Top 1 Id From Emp_Mast Where Id > " + int.Parse(HidFldId.Value));
        }
        else if (pStrNavigation.Trim().ToUpper() == "PREV")
        {
            if (HidFldId.Value == "")
            {
                LblMsg.Text = "Data not found";
                return;
            }
            StrSql.AppendLine("Select Top 1 Id From Emp_Mast Where Id < " + int.Parse(HidFldId.Value) + " Order By Id Desc");
        }
        else if (pStrNavigation.Trim().ToUpper() == "LAST")
        {
            StrSql.AppendLine("Select Top 1 Id From Emp_Mast Order By Id Desc");
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

        StrSql.AppendLine("Select E.Id,E.EmpName,E.EmpGroup,E.EMailId,E.MobNo,E.Password ");
        StrSql.AppendLine(",E.DeptId,E.DesigId,E.FJobTime,E.TJobTime,E.TotTime");
        StrSql.AppendLine(",E.BasicRate,Convert(Varchar(10),E.DOJ,103) As DOJ,Convert(Varchar(10),E.DOB,103) As DOB,E.Gender");
        StrSql.AppendLine(",E.CountryId,E.StateId,E.CityId");
        StrSql.AppendLine(",E.Address1,E.PinCode");
        StrSql.AppendLine(",E.Phone,E.BloodGroup,Convert(Varchar(10),E.LeftDate,103) As LeftDate");
        StrSql.AppendLine("From Emp_Mast E");
        StrSql.AppendLine("Where E.Id=" + IntId);

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
        TxtEmpName.Text = dtTemp.Rows[0]["EmpName"].ToString();

        //DDLDeptName.SelectedValue = dtTemp.Rows[0]["DeptId"].ToString();
        //DDLDesignation.SelectedValue = dtTemp.Rows[0]["DesigId"].ToString();

        DDLDeptName.Items.Clear();
        DDLDeptName.DataSource = BLayer.FillDept();
        DDLDeptName.DataValueField = "DeptId";
        DDLDeptName.DataTextField = "DeptName";
        DDLDeptName.DataBind();
        DDLDeptName.Items.Insert(0, new ListItem("--Select Department--", "0"));

        if (dtTemp.Rows[0]["DeptId"].ToString() != "")
        {
            DDLDeptName.SelectedValue = dtTemp.Rows[0]["DeptId"].ToString();
        }

        DDLDesignation.Items.Clear();
        DDLDesignation.DataSource = BLayer.FillDesig();
        DDLDesignation.DataValueField = "DesigId";
        DDLDesignation.DataTextField = "DesigName";
        DDLDesignation.DataBind();
        DDLDesignation.Items.Insert(0, new ListItem("--Select Designation--", "0"));

        if (dtTemp.Rows[0]["DesigId"].ToString() != "")
        {
            DDLDesignation.SelectedValue = dtTemp.Rows[0]["DesigId"].ToString();
        }

        DDLEmpGroup.SelectedValue = dtTemp.Rows[0]["EmpGroup"].ToString();

        TxtFJobTime.Text = dtTemp.Rows[0]["FJobTime"].ToString();
        TxtTJobTime.Text = dtTemp.Rows[0]["TJobTime"].ToString();
        TxtTotHours.Text = dtTemp.Rows[0]["TotTime"].ToString();

        double basicsal = ValueConvert.ValDouble(dtTemp.Rows[0]["BasicRate"].ToString());
        TxtBasicSal.Text = basicsal.ToString("00");

        TxtDOJ.Text = dtTemp.Rows[0]["DOJ"].ToString();
        TxtDOB.Text = dtTemp.Rows[0]["DOB"].ToString();

        rblGender.SelectedValue = dtTemp.Rows[0]["Gender"].ToString();

        TxtEMailId.Text = dtTemp.Rows[0]["EMailId"].ToString();

        //ddlCountry.SelectedValue = dtTemp.Rows[0]["CountryId"].ToString();
        //ddlState.SelectedValue = dtTemp.Rows[0]["StateID"].ToString();
        //ddlCity.SelectedValue = dtTemp.Rows[0]["CityId"].ToString();

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

        if (dtTemp.Rows[0]["BloodGroup"].ToString() != "")
        {
            ddlBloodGrp.SelectedValue = dtTemp.Rows[0]["BloodGroup"].ToString();
        }
        else
        {
            ddlBloodGrp.SelectedIndex = 0;
        }

        TxtLeftDate.Text = dtTemp.Rows[0]["LeftDate"].ToString();
        TxtPassword.Text = dtTemp.Rows[0]["Password"].ToString();
    }

    protected void GridEmp_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GridEmp.PageIndex = e.NewPageIndex;
            FillGrid();
            LblMsg.Text = "";
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    protected void btnEmpDelete_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton btndetails = sender as ImageButton;
        GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
        Session["Id"] = GridEmp.DataKeys[gvrow.RowIndex].Value.ToString();
        LblMsg.Text = "Are you sure you want to delete this entry ? ";
    }

    protected void btnYes_Click(object sender, ImageClickEventArgs e)
    {
        try
        {            
            int Empid = Convert.ToInt32(Session["Id"]);
            StrSql = new StringBuilder();
            StrSql.Length = 0;
            StrSql.AppendLine("Delete From Emp_Mast Where Id=@Id");
            Cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);
            Cmd.Parameters.AddWithValue("@Id", Empid);            
            SqlFunc.ExecuteNonQuery(Cmd);

            FillGrid();                   
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    protected void btnEmpSelect_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton btndetails = sender as ImageButton;
            GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
            Session["Id"] = GridEmp.DataKeys[gvrow.RowIndex].Value.ToString();

            HidFldId.Value = Session["Id"].ToString();

            DispRecord("EXAC");
    
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

    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            //if (TxtEmpName.Text.Length == 0)
            //{
            //    //LblMsg.Text = "Employee Name Is Blank, Enter Valid Employee Name....";
            //    TxtEmpName.Focus();
            //    return;
            //}

            //if (DDLEmpGroup.SelectedValue == "0")
            //{
            //    //LblMsg.Text = "Select Employee Group....";
            //    DDLEmpGroup.Focus();
            //    return;
            //}

            //if (TxtFJobTime.Text.Length == 0)
            //{
            //    //LblMsg.Text = "Job From Time Is Blank, Enter Valid Time....";
            //    TxtFJobTime.Focus();
            //    return;
            //}

            //if (TxtTJobTime.Text.Length == 0)
            //{
            //    //LblMsg.Text = "Job To Time Is Blank, Enter Valid Time....";
            //    TxtTJobTime.Focus();
            //    return;
            //}

            //if (TxtBasicSal.Text.Length == 0)
            //{
            //    //LblMsg.Text = "Basic Rate Is Blank, Enter Valid Rate....";
            //    TxtBasicSal.Focus();
            //    return;
            //}

            //if (TxtDOJ.Text.Length == 0)
            //{
            //    //LblMsg.Text = "Date Of Join Is Blank, Enter Valid Join Date....";
            //    TxtDOJ.Focus();
            //    return;
            //}

            //if (TxtDOB.Text.Length == 0)
            //{
            //    //LblMsg.Text = "DOB Is Blank, Enter Valid Brith Date....";
            //    TxtDOB.Focus();
            //    return;
            //}

            //if (TxtEMailId.Text.Length == 0)
            //{
            //    //LblMsg.Text = "EMail Id Is Blank, Enter Valid EMail Id....";
            //    TxtEMailId.Focus();
            //    return;
            //}

            //if (TxtMobNo.Text.Length == 0)
            //{
            //    //LblMsg.Text = "Mob.No Is Blank, Enter Valid Mob.No....";
            //    TxtMobNo.Focus();
            //    return;
            //}
            //if (TxtPassword.Text.Length == 0)
            //{
            //    //LblMsg.Text = "Password Is Blank, Enter Valid Password....";
            //    TxtPassword.Focus();
            //    return;
            //}

            StrSql = new StringBuilder();
            StrSql.Length = 0;


            if (HidFldId.Value.Length == 0)
            {
                StrSql.AppendLine("Insert Into Emp_Mast");
                StrSql.AppendLine("(EmpName,DeptId,DesigId");
                StrSql.AppendLine(",EmpGroup,FJobTime,TJobTime,TotTime,BasicRate");
                StrSql.AppendLine(",DOJ,DOB");
                StrSql.AppendLine(",Gender,EMailId");
                StrSql.AppendLine(",CountryId,StateId,CityId");
                StrSql.AppendLine(",Address1,PinCode");
                StrSql.AppendLine(",MobNo,Phone");
                StrSql.AppendLine(",BloodGroup,LeftDate");
                StrSql.AppendLine(",Entry_Date,Entry_Time");
                StrSql.AppendLine(",Entry_UID,UPDFLAG,Password");                
                StrSql.AppendLine(")");
                
                StrSql.AppendLine("Values(@EmpName,@DeptId,@DesigId");
                StrSql.AppendLine(",@EmpGroup,@FJobTime,@TJobTime,@TotTime,@BasicRate ");
                StrSql.AppendLine(",@DOJ,@DOB");
                StrSql.AppendLine(",@Gender,@EMailId");
                StrSql.AppendLine(",@CountryId,@StateId,@CityId");
                StrSql.AppendLine(",@Address1,@PinCode");
                StrSql.AppendLine(",@MobNo,@Phone");
                StrSql.AppendLine(",@BloodGroup,@LeftDate");
                StrSql.AppendLine(",GetDate(),Convert(VarChar,GetDate(),108)");
                StrSql.AppendLine(",@Entry_UID,0,@Password)");
            }
            else
            {
                StrSql.AppendLine("Update Emp_Mast");
                StrSql.AppendLine("Set EmpName=@EmpName,DeptId=@DeptId,DesigId=@DesigId");
                StrSql.AppendLine(",EmpGroup=@EmpGroup,FJobTime=@FJobTime,TJobTime=@TJobTime,TotTime=@TotTime");
                StrSql.AppendLine(",BasicRate=@BasicRate,DOJ=@DOJ,DOB=@DOB");
                StrSql.AppendLine(",Gender=@Gender,EMailId=@EMailId");
                StrSql.AppendLine(",CountryId=@CountryId,StateId=@StateId,CityId=@CityId");
                StrSql.AppendLine(",Address1=@Address1,PinCode=@PinCode");
                StrSql.AppendLine(",MobNo=@MobNo,Phone=@Phone");
                StrSql.AppendLine(",BloodGroup=@BloodGroup,LeftDate=@LeftDate");                
                StrSql.AppendLine(",MEntry_Date=GetDate(),MEntry_Time=Convert(Varchar,GetDate(),108)");
                StrSql.AppendLine(",MEntry_UID=@Entry_UID,UPDFLAG=IsNull(UPDFlag,0)+1,Password=@Password");
                StrSql.AppendLine("Where Id=@Id");
            }

            Cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);
                        
            Cmd.Parameters.AddWithValue("@EmpName", TxtEmpName.Text.Trim());
            if (DDLDeptName.SelectedValue != "0")
            {
                Cmd.Parameters.AddWithValue("@DeptId", DDLDeptName.SelectedValue);
            }
            else
            {
                Cmd.Parameters.AddWithValue("@DeptId", DBNull.Value);
            }
            if (DDLDesignation.SelectedValue != "0")
            {
                Cmd.Parameters.AddWithValue("@DesigId", DDLDesignation.SelectedValue);
            }
            else
            {
                Cmd.Parameters.AddWithValue("@DesigId", DBNull.Value);
            }
            if (DDLEmpGroup.SelectedValue != "0")
            {
                Cmd.Parameters.AddWithValue("@EmpGroup", DDLEmpGroup.SelectedValue);
            }
            else
            {
                Cmd.Parameters.AddWithValue("@EmpGroup", DBNull.Value);
            }                      

            Cmd.Parameters.AddWithValue("@FJobTime", TxtFJobTime.Text.Trim());
            Cmd.Parameters.AddWithValue("@TJobTime", TxtTJobTime.Text.Trim());
            Cmd.Parameters.AddWithValue("@TotTime", TxtTotHours.Text.Trim());
            Cmd.Parameters.AddWithValue("@BasicRate", TxtBasicSal.Text.Trim());
            Cmd.Parameters.AddWithValue("@DOJ", ValueConvert.ConvertDate(TxtDOJ.Text));             
            Cmd.Parameters.AddWithValue("@DOB", ValueConvert.ConvertDate(TxtDOB.Text));
            Cmd.Parameters.AddWithValue("@Gender", rblGender.SelectedValue);
            Cmd.Parameters.AddWithValue("@EMailId", TxtEMailId.Text.Trim());
            Cmd.Parameters.AddWithValue("@Password", Sec.Encrypt(TxtPassword.Text.Trim()));

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
            if (ddlBloodGrp.SelectedValue != "0")
            {
                Cmd.Parameters.AddWithValue("@BloodGroup", ddlBloodGrp.SelectedValue);
            }
            else
            {
                Cmd.Parameters.AddWithValue("@BloodGroup", DBNull.Value);
            }
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
                LblMsg.Text = "Employee added successfully";
            }
            else
            {
                Cmd.Parameters.AddWithValue("@ID", HidFldId.Value.ToString());

                SqlFunc.ExecuteNonQuery(Cmd);
                LblMsg.Text = "Employee updated successfully";
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
            TxtEmpName.Focus();
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
                string confirmValue = Request.Form["confirm_value"];
                if (confirmValue == "Yes")
                {
                    //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked YES !')", true);

                    StrSql = new StringBuilder();
                    StrSql.Length = 0;

                    StrSql.AppendLine("Delete From User_Mast Where Id=@Id ");
                    Cmd = new SqlCommand(StrSql.ToString(), SqlFunc.gConn);
                    Cmd.Parameters.AddWithValue("@Id", int.Parse(HidFldId.Value));
                    SqlFunc.ExecuteNonQuery(Cmd);

                    FillGrid();
                    LblMsg.Text = "User deleted successfully";

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

    protected int GetTime(string pTime,int IntRet)
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
        //DateTime FrmTime = new DateTime(1, 1, 1, ValueConvert.ValInt16(FTime[0].ToString()), ValueConvert.ValInt16(FTime[1].ToString()), 0);
        DateTime FrmTime = new DateTime(1, 1, 1, GetTime(TxtFJobTime.Text.ToString(), 0), GetTime(TxtFJobTime.Text.ToString(), 1), 0);
        DateTime ToTime = new DateTime(1, 1, 1, GetTime(TxtTJobTime.Text.ToString(), 0), GetTime(TxtTJobTime.Text.ToString(), 1), 0);

        // Calculate the interval between the two dates.
        TimeSpan interval = ToTime-FrmTime;

        String DiffTime = interval.Hours.ToString("00") + "." + interval.Minutes.ToString("00");

        TxtFJobTime.Text = GetTime(TxtFJobTime.Text.ToString(), 0).ToString("00") + "." + GetTime(TxtFJobTime.Text.ToString(), 1).ToString("00");
        TxtTJobTime.Text = GetTime(TxtTJobTime.Text.ToString(), 0).ToString("00") + "." + GetTime(TxtTJobTime.Text.ToString(), 1).ToString("00");
        TxtTotHours.Text = DiffTime.ToString();       
    }
    
    protected void TxtFJobTime_TextChanged(object sender, EventArgs e)
    {
        try
        {
            GetTotTime();
            TxtTJobTime.Focus();
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    protected void TxtTJobTime_TextChanged(object sender, EventArgs e)
    {
        try
        {
            GetTotTime();
            TxtBasicSal.Focus();
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    protected void FindEmp()
    {
        StrSql = new StringBuilder();
        StrSql.Length = 0;
        StrSql.AppendLine("Select Id,EmpName,EmpGroup,EMailId,MobNo ");
        StrSql.AppendLine("From Emp_Mast ");
        StrSql.AppendLine("Where 1=1 ");
        if (TxtFEmpName.Text.Length != 0)
        {
            StrSql.AppendLine(" And EmpName Like '" + TxtFEmpName.Text.ToString() + "%'");
        }
        if (TxtFEmpGrp.Text.Length != 0)
        {
            StrSql.AppendLine(" And EmpGroup Like '" + TxtFEmpGrp.Text.ToString() + "%'");
        }
        if (TxtFEMailId.Text.Length != 0)
        {
            StrSql.AppendLine(" And EMailId Like '" + TxtFEMailId.Text.ToString() + "%'");
        }
        if (TxtFMobNo.Text.Length != 0)
        {
            StrSql.AppendLine(" And MobNo Like '" + TxtFMobNo.Text.ToString() + "%'");
        }

        StrSql.AppendLine(" Order By EmpName");

        dtTemp = new DataTable();
        dtTemp = SqlFunc.ExecuteDataTable(StrSql.ToString());

        GridEmp.DataSource = dtTemp;
        GridEmp.DataBind();
    }

    protected void TxtFEmpName_TextChanged(object sender, EventArgs e)
    {
        try
        {
            FindEmp(); 
        }
        catch
        {
            Response.Redirect("~/ErrorPage.aspx");
        }
    }
    protected void TxtFEmpGrp_TextChanged(object sender, EventArgs e)
    {
        try
        {
            FindEmp();
        }
        catch
        {
            Response.Redirect("~/ErrorPage.aspx");
        }
    }
    protected void TxtFEMailId_TextChanged(object sender, EventArgs e)
    {
        try
        {
            FindEmp();
        }
        catch
        {
            Response.Redirect("~/ErrorPage.aspx");
        }
    }
    protected void TxtFMobNo_TextChanged(object sender, EventArgs e)
    {
        try
        {
            FindEmp();
        }
        catch
        {
            Response.Redirect("~/ErrorPage.aspx");
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
}