using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using HRMSystem;

/// <summary>
/// Summary description for WebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class WebService : System.Web.Services.WebService {

    SqlFunction SqlFunc=new SqlFunction();
    //ValueConvert VC =new ValueConvert();
    StringBuilder StrSql=new StringBuilder();

    DataSet ds = new DataSet();
    SqlCommand cmd = new SqlCommand();
    
    public WebService () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld() {
        return "Hello World";
    }

    [WebMethod]
    public XmlElement GetProjectList(int pIntPrjId, string pStrPrjStatus, string pStrPrjStartDate, string pStrPrjEndDate
        , int pIntModId, string pStrModStatus, string pStrModStartDate, string pStrModEndDate)
    {
        try
        {
            StrSql = new StringBuilder();
            StrSql.Length = 0;
            // P.Id,M.Id As ModuleId,P.ClientId
            StrSql.AppendLine("Select P.ProjectName");
            StrSql.AppendLine(",Convert(Varchar(10),P.StartDate,103) As StartDate");
            StrSql.AppendLine(",Convert(Varchar(10),P.EndDate,103) As EndDate");
            StrSql.AppendLine(",Case When IsNull(P.ProjctStatus,'')='D' Then 'Done'");
            StrSql.AppendLine("      When IsNull(P.ProjctStatus,'')='P' Then 'Pending'");
            StrSql.AppendLine("Else '' End As ProjctStatus");
            StrSql.AppendLine(",P.Remark,C.ClientName");
            StrSql.AppendLine(",M.ModuleName");
            StrSql.AppendLine(",Convert(Varchar(10),M.StartDate,103) As ModStart_Date");
            StrSql.AppendLine(",Convert(Varchar(10),M.EndDate,103) As ModEnd_Date");
            StrSql.AppendLine(",Case When IsNull(M.ModuleStatus,'')='D' Then 'Done'");
            StrSql.AppendLine("      When IsNull(M.ModuleStatus,'')='P' Then 'Pending'");
            StrSql.AppendLine("      When IsNull(M.ModuleStatus,'')='C' Then 'Cancel'");
            StrSql.AppendLine("Else '' End As ModuleStatus");
            StrSql.AppendLine(",M.Remark As ModRemark");
            StrSql.AppendLine("From Project_Mast P");
            StrSql.AppendLine("Left Join Project_Module M On P.Id=M.ProjectId");
            StrSql.AppendLine("Left Join Client_Mast C On P.ClientId=C.Id");
            StrSql.AppendLine("Where 1=1 ");
            if (pIntPrjId != 0)
            {
                StrSql.AppendLine("And P.Id=" + pIntPrjId);
            }
            if (pStrPrjStatus != "0")
            {
                StrSql.AppendLine("And P.ProjctStatus='" + pStrPrjStatus + "'");
            }
            if (pStrPrjStartDate != "")
            {
                StrSql.AppendLine("And P.StartDate >='" + ValueConvert.ConvertDate(pStrPrjStartDate) + "'");
            }
            if (pStrPrjEndDate != "")
            {
                StrSql.AppendLine("And P.EndDate <='" + ValueConvert.ConvertDate(pStrPrjEndDate) + "'");
            }
            if (pIntModId != 0)
            {
                StrSql.AppendLine("And M.Id=" + pIntModId);
            }
            if (pStrModStatus != "0")
            {
                StrSql.AppendLine("And M.ModuleStatus='" + pStrModStatus + "'");
            }
            if (pStrModStartDate != "")
            {
                StrSql.AppendLine("And M.StartDate >='" + ValueConvert.ConvertDate(pStrModStartDate) + "'");
            }
            if (pStrModEndDate != "")
            {
                StrSql.AppendLine("And M.EndDate <='" + ValueConvert.ConvertDate(pStrModEndDate) + "'");
            }

            ds = new DataSet();
            ds = SqlFunc.ExecuteDataSet(StrSql.ToString().Replace("\r\n", " "));

            //Return the DataSet as an XmlElement.
            XmlDataDocument xmldata = new XmlDataDocument(ds);
            XmlElement xmlElement = xmldata.DocumentElement;
            return xmlElement;
        }
        catch (Exception ex)
        {
            throw ex;
        }       
    }

    [WebMethod]
    public XmlElement GetWorkList(int pIntPrjLedId,int pIntEmpId,int pIntPrjId,int pIntPrjModId
        ,string pStrAssignDate,string pStrWorkStatus,string pStrDueDate)
    {
        try
        {
            StrSql = new StringBuilder();
            StrSql.Length = 0;

            StrSql.AppendLine("Select PL.EmpName As [Project Ledger]" + Environment.NewLine);
            StrSql.AppendLine(" ,P.ProjectName As Project" + Environment.NewLine);
            StrSql.AppendLine(" ,M.ModuleName As Module" + Environment.NewLine);
            StrSql.AppendLine(" ,CM.ClientName As Client" + Environment.NewLine);
            StrSql.AppendLine(" ,E.EmpName As Employee" + Environment.NewLine);
            StrSql.AppendLine(" ,Convert(Varchar(10),W.AssignDate,103) As [Assign Date]" + Environment.NewLine);
            StrSql.AppendLine(" ,Convert(Varchar(10),W.AssignTime,108) As [Assign Time]" + Environment.NewLine);
            StrSql.AppendLine(" ,IsNull(W.DueDays,0) As [Due Days]" + Environment.NewLine);
            StrSql.AppendLine(" ,Convert(Varchar(10),W.DueDate,103) As [Due Date]" + Environment.NewLine);
            StrSql.AppendLine(" ,W.WorkDet1 As [Work 1]" + Environment.NewLine);
            StrSql.AppendLine(" ,W.WorkDet2 As [Work 2]" + Environment.NewLine);
            StrSql.AppendLine(" ,W.WorkDet3 As [Work 3]" + Environment.NewLine);
            StrSql.AppendLine(" ,Case When IsNull(W.WorkStatus,'')='D' Then 'Done'" + Environment.NewLine);
            StrSql.AppendLine("	   When IsNull(W.WorkStatus,'')='P' Then 'Pending'" + Environment.NewLine);
            StrSql.AppendLine("	   When IsNull(W.WorkStatus,'')='C' Then 'Cancle'" + Environment.NewLine);
            StrSql.AppendLine("  Else '' End As [Work Status]" + Environment.NewLine);
            StrSql.AppendLine(" ,W.Remark" + Environment.NewLine);
            StrSql.AppendLine(" ,Case When IsNull(W.CompleteDate,'')<>''" + Environment.NewLine);
            StrSql.AppendLine("       Then Convert(Varchar(10),W.CompleteDate,103) Else '' End  As [Complete Date]" + Environment.NewLine);
            StrSql.AppendLine(" ,Convert(Varchar(10),W.CompleteTime,108) As [Complete Time]" + Environment.NewLine);

            StrSql.AppendLine(" From WorkAssDet W" + Environment.NewLine);
            StrSql.AppendLine(" Left Join Emp_Mast PL On PL.Id=W.PrjLedId " + Environment.NewLine);
            StrSql.AppendLine(" Left Join Project_Mast P On P.Id=W.PrjId" + Environment.NewLine);
            StrSql.AppendLine(" Left Join Project_Module M On M.Id=W.PrjModId" + Environment.NewLine);
            StrSql.AppendLine(" Left Join Client_Mast CM On CM.Id=P.ClientId" + Environment.NewLine);
            StrSql.AppendLine(" Left Join Emp_Mast E On E.Id=W.EmpId" + Environment.NewLine);            

            StrSql.AppendLine(" Where 1=1 " + Environment.NewLine);
            
            if (pIntPrjLedId != 0)
            {
                StrSql.AppendLine("And W.PrjLedId=" + pIntPrjLedId + Environment.NewLine);
            }                       
            
            if (pIntEmpId != 0)
            {
                StrSql.AppendLine("And W.EmpId=" + pIntEmpId + Environment.NewLine);
            }

            if (pIntPrjId != 0)
            {
                StrSql.AppendLine("And W.PrjId=" + pIntPrjId + Environment.NewLine);
            }

            if (pIntPrjModId != 0)
            {
                StrSql.AppendLine("And W.PrjModId=" + pIntPrjModId + Environment.NewLine);
            }

            if (pStrAssignDate != "")
            {
                StrSql.AppendLine("And W.AssignDate ='" + ValueConvert.ConvertDate(pStrAssignDate) + "'" + Environment.NewLine);
            }

            if (pStrWorkStatus != "0")
            {
                StrSql.AppendLine("And W.WorkStatus='" + pStrWorkStatus + "'" + Environment.NewLine);
            }
            
            if (pStrDueDate != "")
            {
                StrSql.AppendLine("And W.DueDate ='" + ValueConvert.ConvertDate(pStrDueDate) + "'" + Environment.NewLine);
            }
            StrSql.AppendLine(" Order By PL.EmpName,P.ProjectName,M.ModuleName,E.EmpName" + Environment.NewLine);
            ds = new DataSet();
            ds = SqlFunc.ExecuteDataSet(StrSql.ToString().Replace("\r\n", " "));

            //return the dataset as an xmlelement.
            XmlDataDocument xmldata=new XmlDataDocument(ds);
            XmlElement xmlElement=xmldata.DocumentElement;
            return xmlElement;
        }
        catch(Exception ex)
        {
            throw ex;
        }
    }
}
