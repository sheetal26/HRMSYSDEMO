using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Xml.Linq;

/// <summary>
/// Summary description for Common
/// </summary>
public class Common
{
    SqlFunction SqlFunc = new SqlFunction();

	public Common()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    //You could *consider* making this an extension method
    //For Web Services DataSource
    public DataSet ToDataSetOrNull(XElement source)
    {
        if (source == null)
        {
            return null;
        }
        DataSet result = new DataSet();
        result.ReadXml(source.CreateReader(), XmlReadMode.Auto);
        return result;
    }

    //For Report Viewer Datasource
    public HRMDataSet GetData(string query, string StrSrcTbl)
    {
        HRMDataSet dsRepoInfo = new HRMDataSet();
        //try
        //{
            SqlCommand cmd = new SqlCommand(query);
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = SqlFunc.gConn;

                sda.SelectCommand = cmd;
                using (dsRepoInfo = new HRMDataSet())
                {
                    sda.Fill(dsRepoInfo, StrSrcTbl);
                    return dsRepoInfo;
                }
            }
        //}
        //catch //(Exception ex)
        //{ 
        //    return dsRepoInfo;           
        //}
    }
}