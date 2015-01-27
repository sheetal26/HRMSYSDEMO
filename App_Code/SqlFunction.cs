using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;

/// <summary>
/// Summary description for SqlFunction
/// </summary>
public class SqlFunction
{
    #region "Variables"

    //SqlCommand cmd;
    //DataTable tbl;
    //DataSet dsTemp;
    //SqlDataAdapter da=new SqlDataAdapter();
    //StringBuilder StrSql;

    public string gStrConn;
    public SqlConnection gConn = new SqlConnection();
    public static SqlTransaction SqlTran = null;
    public static SqlCommand SqlCmd = new SqlCommand();

    #endregion

    public SqlFunction()
	{ 
        //Get connection string from web.config file
        gStrConn = ConfigurationManager.ConnectionStrings["gConn"].ConnectionString.ToString();

        //create new sqlconnection and connection to database by using connection string from web.config file
        gConn = new SqlConnection(gStrConn);
	}

    public int ExecuteNonQuery(string pSql)
    {
        try
        {
            int result = 0;
            SqlCommand Cmd = new SqlCommand();
            using (SqlConnection SqlConn = new SqlConnection(gStrConn))
            {
                Cmd.Connection = SqlConn;
                Cmd.CommandTimeout = 900;//15 minutes
                Cmd.CommandText = pSql;
                if (SqlConn.State == ConnectionState.Open)
                    SqlConn.Close();
                SqlConn.Open();
                result = Convert.ToInt16(Cmd.ExecuteNonQuery());
                SqlConn.Close();
            }

            return result;
        }
        catch (Exception ex)
        {
            throw ex;
        }        

        //try
        //{
        //    cmd = new SqlCommand();
        //    cmd.Connection = gConn;
        //    cmd.CommandText = sql;
        //    if (gConn.State != ConnectionState.Open)
        //    {
        //        gConn.Open();
        //    }
        //    int x = cmd.ExecuteNonQuery();
        //    gConn.Close();
        //    return x;
        //}
        //catch (SqlException ex)
        //{
        //    //return ex.Number;
        //    throw ex;
        //}
    }

    public DataTable ExecuteDataTable(string pSql)
    {
        try
        {
            DataTable dt = new DataTable();
            SqlCommand Cmd = new SqlCommand();
            using (SqlConnection SqlConn = new SqlConnection(gStrConn))
            {
                Cmd.Connection = SqlConn;
                Cmd.CommandTimeout = 900;//15 minutes
                Cmd.CommandText = pSql;
                if (SqlConn.State == ConnectionState.Open)
                    SqlConn.Close();
                SqlConn.Open();
                SqlDataReader dr = Cmd.ExecuteReader(CommandBehavior.CloseConnection);
                dt.Load(dr);
                dr.Close();
            }
            return dt;
        }
        catch (Exception ex)
        {
            throw ex;
        }

        //try
        //{
        //    cmd = new SqlCommand();
        //    cmd.Connection = gConn;
        //    cmd.CommandText = sql;
        //    if (gConn.State != ConnectionState.Open)
        //    {
        //        gConn.Open();
        //    }
        //    tbl = new DataTable();
        //    tbl.Load(cmd.ExecuteReader());
        //    gConn.Close();
        //    return tbl;
        //}
        //catch(SqlException ex) 
        //{
        //   //return tbl;
        //    throw ex;
        //}
    }

    public DataSet ExecuteDataSet(string pSql)
    {

        try
        {
            DataSet ds = new DataSet();
            SqlCommand Cmd = new SqlCommand();
            using (SqlConnection SqlConn = new SqlConnection(gStrConn))
            {
                Cmd.Connection = SqlConn;
                Cmd.CommandTimeout = 900;//15 minutes
                Cmd.CommandText = pSql;
                if (SqlConn.State == ConnectionState.Open)
                    SqlConn.Close();
                SqlConn.Open();
                SqlDataAdapter dr = new SqlDataAdapter(Cmd);
                dr.Fill(ds);
                SqlConn.Close();
            }

            return ds;
        }
        catch (Exception ex)
        {
            throw ex;
        }

        //try
        //{
        //    SqlDataAdapter da = new SqlDataAdapter(sql, gConn);
        //    dsTemp = new DataSet();
        //    da.Fill(dsTemp);
        //    return dsTemp;
        //}
        //catch(Exception ex)
        //{
        //    //return dsTemp;
        //    throw ex;
        //}
    }      

    #region "ADO.NET Function Parameters used sqlCommand"

    public DataTable ExecuteTable(SqlCommand pSqlCmd)
    {
        try
        {
            DataTable dt = new DataTable();
            using (SqlConnection SqlConn = new SqlConnection(gStrConn))
            {
                pSqlCmd.Connection = SqlConn;
                pSqlCmd.CommandTimeout = 900;//15 minutes
                if (SqlConn.State == ConnectionState.Open)
                    SqlConn.Close();
                SqlConn.Open();
                SqlDataReader dr = pSqlCmd.ExecuteReader(CommandBehavior.CloseConnection);
                dt.Load(dr);
                dr.Close();
            }
            return dt;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataSet ExecuteDataset(SqlCommand pSqlCmd)
    {
        try
        {
            DataSet ds = new DataSet();
            using (SqlConnection SqlConn = new SqlConnection(gStrConn))
            {
                pSqlCmd.Connection = SqlConn;
                pSqlCmd.CommandTimeout = 900;//15 minutes
                if (SqlConn.State == ConnectionState.Open)
                    SqlConn.Close();
                SqlConn.Open();
                SqlDataAdapter dr = new SqlDataAdapter(pSqlCmd);
                dr.Fill(ds);
                SqlConn.Close();
            }
            return ds;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public int ExecuteNonQuery(SqlCommand pSqlCmd)
    {
        try
        {
            int result = 0;
            using (SqlConnection SqlConn = new SqlConnection(gStrConn))
            {
                pSqlCmd.Connection = SqlConn;
                pSqlCmd.CommandTimeout = 900;//15 minutes
                if (SqlConn.State == ConnectionState.Open)
                    SqlConn.Close();
                SqlConn.Open();
                result = Convert.ToInt16(pSqlCmd.ExecuteNonQuery());
                SqlConn.Close();
            }
            return result;
        }
        catch (Exception ex)
        {
            throw ex;
        }

        //try
        //{
        //    pCmd.Connection = gConn;
        //    if (gConn.State != ConnectionState.Open)
        //    {
        //        gConn.Open();
        //    }
        //    int x = pCmd.ExecuteNonQuery();
        //    gConn.Close();
        //    return x;
        //}
        //catch (SqlException ex)
        //{
        //    //return ex.Number;
        //    throw ex;
        //}
    }

    #endregion

    #region "Finction For BeginTrans And CommitTrans"

    public void BeginConnWithTransaction()
    {
        if (gConn.State == ConnectionState.Open)
        {
            gConn.Close();
        }
        gConn.ConnectionString = gStrConn.ToString();
        gConn.Open();
        SqlTran = gConn.BeginTransaction();
        
        SqlCmd.Connection = gConn;
        SqlCmd.CommandTimeout = 300;//5 minutes
        SqlCmd.Transaction = SqlTran;

    }

    public void EndConnWithTransaction()
    {
        if (SqlTran != null)
        {
            if (SqlTran.Connection != null)
            {
                SqlTran.Commit();
                SqlTran.Dispose();
                gConn.Close();
            }
        }
    }

    public int ExecuteTransaction(string sqlquery)
    {
        int result = 0;
        string ErrorLogPath = HttpContext.Current.Server.MapPath("~/Attachments/ErrorLog.txt");
        try
        {
            SqlCmd.CommandText = sqlquery;
            SqlCmd.CommandTimeout = 300;//5 minutes
            result = SqlCmd.ExecuteNonQuery();            
        }
        catch (Exception ex)
        {
            SqlTran.Rollback();
            SqlTran.Dispose();
            gConn.Close();            
            throw ex;
        }
        return result;
    }

    public DataTable ExecuteTransTable(string sqlquery)
    {
        DataTable dtResult = new DataTable();
        try
        {
            SqlCmd.CommandText = sqlquery;
            SqlCmd.CommandTimeout = 300;//5 minutes
            SqlDataReader dr = SqlCmd.ExecuteReader();
            dtResult.Load(dr);
            dr.Close();
        }
        catch
        {
            SqlTran.Rollback();
            SqlTran.Dispose();
            gConn.Close();
        }
        return dtResult;
    }

    public void RollbackTransaction()
    {
        if (SqlTran.Connection != null)
        {
            SqlTran.Rollback();
            SqlTran.Dispose();
            gConn.Close();
        }
    }


    #endregion
}