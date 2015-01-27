using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;

/// <summary>
/// Summary description for BAL
/// </summary>
public class BAL
{
    SqlFunction SqlFunc=new SqlFunction();
    DataSet dsTemp;
    StringBuilder StrSql;

	public BAL()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public int DeptId { get; set; }
    public string DeptName {get; set;}

    public int DesigId { get; set; }
    public string DesigName { get; set; }

    public int CountryId { get; set; }
    public string CountryName { get; set; }

    public int StateId { get; set; }
    public string StateName { get; set; }

    public int CityId { get; set; }
    public string CityName { get; set; }

    public int UserGrpId { get; set; }
    public string UserGrpName { get; set; }

    public int UserId { get; set; }

    public DataSet FillUserGroup()
    {
        StrSql = new StringBuilder();
        StrSql.Length = 0;
        StrSql.AppendLine("Select Id As UGroupId,Group_Name As UGroupName From User_Group Order By Group_Name");

        dsTemp = new DataSet();
        dsTemp = SqlFunc.ExecuteDataSet(StrSql.ToString()); 

        return dsTemp;
    }

    public DataSet FillEmp(int pIntEmpId, string pStrCriteria)//string pStrCriteria
    {
        StrSql = new StringBuilder();
        StrSql.Length = 0;
        StrSql.AppendLine("Select Id As EmpId,EmpName ");
        StrSql.AppendLine("From Emp_Mast");
        StrSql.AppendLine("Where IsDate(LeftDate)=0");
        if (pStrCriteria.Length != 0)
        {
            StrSql.AppendLine(pStrCriteria);
        }
        if (pIntEmpId != 0)
        {
            StrSql.AppendLine(" And Id=" + pIntEmpId); 
        }
        StrSql.AppendLine("Order By EmpName");

        dsTemp = new DataSet();
        dsTemp = SqlFunc.ExecuteDataSet(StrSql.ToString()); 

        return dsTemp;
    }

    public DataSet FillProject(string pStrCriteria)
    {
        StrSql = new StringBuilder();
        StrSql.Length = 0;
        StrSql.AppendLine("Select Id As PrjId,ProjectName As PrjName ");
        StrSql.AppendLine("From Project_Mast");
        StrSql.AppendLine("Order By ProjectName");        

        dsTemp = new DataSet();
        dsTemp = SqlFunc.ExecuteDataSet(StrSql.ToString());

        return dsTemp;
    }

    public DataSet FillPrjModule(int PrjId = 0)
    {
        StrSql = new StringBuilder();
        StrSql.Length = 0;
        StrSql.AppendLine("Select Id As ModId,ModuleName As ModName");
        StrSql.AppendLine("From Project_Module");
        if (PrjId != 0)
        {
            StrSql.AppendLine("Where ProjectId=" + PrjId);
        }
        StrSql.AppendLine("Order By ModuleName");

        dsTemp = new DataSet();
        dsTemp = SqlFunc.ExecuteDataSet(StrSql.ToString());

        return dsTemp;
    } 

    public DataSet FillDept()
    {
        StrSql = new StringBuilder();
        StrSql.Length = 0;
        StrSql.AppendLine("Select Id As DeptId,DeptName From Dept_Mast Order By DeptName");
        
        dsTemp = new DataSet();
        dsTemp = SqlFunc.ExecuteDataSet(StrSql.ToString()); 

        return dsTemp;
    }

    public DataSet FillDesig(int DeptId=0)
    {
        StrSql = new StringBuilder();
        StrSql.Length = 0;
        StrSql.AppendLine("Select Id As DesigId,DesigName From Desig_Mast ");
        if (DeptId != 0)
        {
            StrSql.AppendLine("Where Dept_Id=" + DeptId);
        }
        StrSql.AppendLine("Order By DesigName");

        dsTemp = new DataSet();
        dsTemp = SqlFunc.ExecuteDataSet(StrSql.ToString());

        return dsTemp;
    }

    public DataSet FillCity(int StateId = 0)
    {
        StrSql = new StringBuilder();
        StrSql.Length = 0;
        StrSql.AppendLine("Select Id As CityId,CityName");
        StrSql.AppendLine("From City_Mast");
        if (StateId != 0)
        {
            StrSql.AppendLine("Where StateId=" + StateId);
        }
        StrSql.AppendLine("Order By CityName");

        dsTemp = new DataSet();
        dsTemp = SqlFunc.ExecuteDataSet(StrSql.ToString());

        return dsTemp; 
    }

    public DataSet FillState(int CountryId=0)
    {
        StrSql = new StringBuilder();
        StrSql.Length = 0;        
        StrSql.AppendLine("Select Id As StateId,StateName");
        StrSql.AppendLine("From State_Mast");
        if (CountryId != 0)
        {
            StrSql.AppendLine("Where CountryId=" + CountryId);
        }        
        StrSql.AppendLine("Order By StateName");

        dsTemp = new DataSet();
        dsTemp = SqlFunc.ExecuteDataSet(StrSql.ToString());

        return dsTemp; 
    }

    public DataSet FillCountry()
    {
        StrSql = new StringBuilder();
        StrSql.Length = 0;
        StrSql.AppendLine("Select Id As CountryId,CountryName");
        StrSql.AppendLine("From Country_Mast");        
        StrSql.AppendLine("Order By CountryName");

        dsTemp = new DataSet();
        dsTemp = SqlFunc.ExecuteDataSet(StrSql.ToString());

        return dsTemp; 
    }

    public DataSet FillClient()
    {
        StrSql = new StringBuilder();
        StrSql.Length = 0;
        StrSql.AppendLine("Select Id As ClientId,ClientName");
        StrSql.AppendLine("From Client_Mast");
        StrSql.AppendLine("Order By ClientName");

        dsTemp = new DataSet();
        dsTemp = SqlFunc.ExecuteDataSet(StrSql.ToString());

        return dsTemp;
    }
}