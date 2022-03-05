using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
/// <summary>
/// Summary description for GoldDataAccess
/// </summary>
public  class GoldDataAccess: IGoldDataAccess
{
    private readonly string _connectionString = string.Empty;
    public SqlCommand objCommand;
    public SqlConnection objSqlConnection = null;
    GoldLogging log = null;
    public GoldDataAccess()
    {
        this._connectionString = ConfigurationManager.ConnectionStrings["mycon"].ConnectionString;
    }
    /// <summary>
    /// Open_Connection method will open the current instance of the connection object if the object is in closed state  
    /// </summary>
    /// 
    public void Open_Connection()
    {
        try
        {

            if (objSqlConnection.State == ConnectionState.Closed)
            {
                objSqlConnection.Open();
            }
        }
        catch (Exception e)
        {
            log = new GoldLogging();
            log.SendErrorToText(e);
            throw new GoldException("Data Connection open Error: {0}",e.InnerException);
        }
    }
    /// <summary>
    /// Close_Connection method will close and dispose the connection object that was created earlier.
    /// </summary>
    public void Close_Connection()
    {
        try
        {
            if (objSqlConnection.State != ConnectionState.Closed)
            {
                this.objSqlConnection.Close();
                this.objSqlConnection.Dispose();
            }
        }
        catch (Exception e)
        {
            log = new GoldLogging();
            log.SendErrorToText(e);
            throw new GoldException("Data Connection Closed Error: {0}", e.InnerException);
        }
    }

    /// <summary>
    ///Executes the query, and returns the first column of the first row in the result set returned by the query. Additional columns or rows are ignored.
    /// </summary>
    /// <param name="procedureName">Store procedure Name</param>
    /// <returns></returns>
    public object ExecuteScalar(string procedureName)
    {
        objSqlConnection = new SqlConnection(_connectionString);
        SqlCommand command = new SqlCommand(procedureName, objSqlConnection);
        command.CommandType = CommandType.StoredProcedure;
        object ReturnValue;
        Open_Connection();
        using (SqlTransaction Transaction = objSqlConnection.BeginTransaction())
        {
            try
            {
                command.Transaction = Transaction;
                ReturnValue = command.ExecuteScalar();
                Transaction.Commit();
            }
            catch(Exception e)
            {
                Transaction.Rollback();
                log = new GoldLogging();
                log.SendErrorToText(e);
                throw new GoldException("Data Execution(Scaler) Error: {0}", e.InnerException);
            }
            finally
            {
                Close_Connection();
                command.Dispose();
            }
        }
        return ReturnValue;
    }
   

    /// <summary>
    /// Executes the query, and returns the first column of the first row in the result set returned by the query. Additional columns or rows are ignored.
    /// </summary>
    /// <param name="procedureName">Store Procedure Name</param>
    /// <param name="parameters">Parameters List</param>
    /// <returns></returns>
    public object ExecuteScalarWithParameters(string procedureName, SqlParameter[] parameters)
    {
        objSqlConnection = new SqlConnection(_connectionString);
        SqlCommand command = new SqlCommand(procedureName, objSqlConnection);
        command.CommandType = CommandType.StoredProcedure;
        object ReturnValue;
        Open_Connection();
        using (SqlTransaction Transaction = objSqlConnection.BeginTransaction())
        {
            try
            {
                if (parameters != null)
                    command.Parameters.AddRange(parameters);

                command.Transaction = Transaction;
                ReturnValue = command.ExecuteScalar();
                Transaction.Commit();
            }
            catch(Exception e)
            {
                Transaction.Rollback();
                log = new GoldLogging();
                log.SendErrorToText(e);
                throw new GoldException("Data Execution(Scaler with param) Error: {0}", e.InnerException);
            }
            finally
            {
                Close_Connection();
                command.Dispose();
            }
        }
        return ReturnValue;
    }

    /// <summary>
    /// Executes a Transact-SQL statement against the connection and returns the number of rows affected.
    /// </summary>
    /// <param name="procedureName">Store Procedure Name</param>
    /// <returns></returns>
    public object ExecuteNonQuery(string procedureName)
    {
        objSqlConnection = new SqlConnection(_connectionString);
        SqlCommand command = new SqlCommand(procedureName, objSqlConnection);
        command.CommandType = CommandType.StoredProcedure;
        object ReturnValue;
        Open_Connection();
        using (SqlTransaction Transaction = objSqlConnection.BeginTransaction())
        {
            try
            {
                command.Transaction = Transaction;
                ReturnValue = command.ExecuteNonQuery();
                Transaction.Commit();
            }
            catch(Exception e)
            {
                Transaction.Rollback();
                log = new GoldLogging();
                log.SendErrorToText(e);
                throw new GoldException("Data Execution(Non Query) Error: {0}", e.InnerException);
            }
            finally
            {
                Close_Connection();
                command.Dispose();
            }
        }
        return ReturnValue;
    }

    /// <summary>
    /// Executes a Transact-SQL statement against the connection and returns the number of rows affected.
    /// </summary>
    /// <param name="procedureName">Store Procedure Name</param>
    /// <param name="parameters">Parameters</param>
    /// <returns></returns>
    public object ExecuteNonQueryWithParameters(string procedureName, SqlParameter[] parameters)
    {
        objSqlConnection = new SqlConnection(_connectionString);
        SqlCommand command = new SqlCommand(procedureName, objSqlConnection);
        command.CommandType = CommandType.StoredProcedure;
        object ReturnValue;
        Open_Connection();
        using (SqlTransaction Transaction = objSqlConnection.BeginTransaction())
        {
            try
            {
                if (parameters != null)
                    command.Parameters.AddRange(parameters);

                command.Transaction = Transaction;
                ReturnValue = command.ExecuteNonQuery();
                Transaction.Commit();
            }
            catch(Exception e)
            {
                Transaction.Rollback();
                log = new GoldLogging();
                log.SendErrorToText(e);
                throw new GoldException("Data Execution(Non Query param) Error: {0}", e.InnerException);
            }
            finally
            {
                Close_Connection();
                command.Dispose();
            }
        }
        return ReturnValue;
    }

    /// <summary>
    /// Executes a Transact-SQL statement against the connection and returns the result of output parameter
    /// </summary>
    /// <param name="procedureName">Store Procedure Name</param>
    /// <param name="parameters">Parameters</param>
    /// <returns></returns>
    public object ExecuteNonQueryWithOutputParameters(string procedureName, SqlParameter[] parameters)
    {
        objSqlConnection = new SqlConnection(_connectionString);
        object ReturnValue;
        SqlCommand command = new SqlCommand(procedureName, objSqlConnection);
        command.CommandType = CommandType.StoredProcedure;
        Open_Connection();
        using (SqlTransaction Transaction = objSqlConnection.BeginTransaction())
        {
            try
            {
                if (parameters != null)
                    command.Parameters.AddRange(parameters);

                command.Transaction = Transaction;
                command.ExecuteNonQuery();
                ReturnValue = (object)command.Parameters[parameters.Length - 1].Value;
                Transaction.Commit();
            }
            //catch
            //{
            //    Transaction.Rollback();
            //   return 10;
            //    throw;
            //}
            catch (Exception e)
            {
                Transaction.Rollback();
                log = new GoldLogging();
                log.SendErrorToText(e);
                throw new GoldException("Data Execution(Non Query param) Error: {0}", e.InnerException);
            }
            finally
            {
                Close_Connection();
                command.Dispose();
            }
        }
        return ReturnValue;
    }
  
    /// <summary>
    /// Executes a Transact-SQL statement against the connection and returns the result in table format
    /// Used only when we have to return records in multiple table format
    /// </summary>
    /// <param name="procedureName">Store Procedure Name</param>
    /// <returns></returns>
    public DataSet ReturnDataset(string procedureName)
    {
        objSqlConnection = new SqlConnection(_connectionString);
        DataSet dataSet = new DataSet();
        SqlCommand command = new SqlCommand(procedureName, objSqlConnection);
        command.CommandType = CommandType.StoredProcedure;
        SqlDataAdapter DataAdapter = new SqlDataAdapter();
        DataAdapter.SelectCommand = command;
        Open_Connection();
        using (SqlTransaction Transaction = objSqlConnection.BeginTransaction())
        {
            try
            {
                DataAdapter.SelectCommand.Transaction = Transaction;
                DataAdapter.Fill(dataSet);
                Transaction.Commit();
            }
            catch(Exception e)
            {
                Transaction.Rollback();
                log = new GoldLogging();
                log.SendErrorToText(e);
                throw new GoldException("Data Execution(Non Query param) Error: {0}", e.InnerException);
            }
            finally
            {
                Close_Connection();
                command.Dispose();
            }
        }
        return dataSet;
    }
    /// <summary>
    /// Executes a Transact-SQL statement against the connection and returns the result in table format
    /// Used only when we have to return records in multiple table format
    /// </summary>
    /// <param name="procedureName">Store Procedure Name</param>
    /// <param name="parameters">Parameters</param>
    /// <returns></returns>
    public DataSet ReturnDatasetWithParameters(string procedureName, SqlParameter[] parameters)
    {
        objSqlConnection = new SqlConnection(_connectionString);
        DataSet dataSet = new DataSet();
        SqlCommand command = new SqlCommand(procedureName, objSqlConnection);
        command.CommandType = CommandType.StoredProcedure;

        if (parameters != null)
            command.Parameters.AddRange(parameters);

        SqlDataAdapter DataAdapter = new SqlDataAdapter();
        DataAdapter.SelectCommand = command;
        Open_Connection();

        using (SqlTransaction oTransaction = objSqlConnection.BeginTransaction())
        {
            try
            {
                DataAdapter.SelectCommand.Transaction = oTransaction;
                DataAdapter.Fill(dataSet);
                oTransaction.Commit();
            }
            catch(Exception e)
            {
                oTransaction.Rollback();
                log = new GoldLogging();
                log.SendErrorToText(e);
                throw new GoldException("Data Execution(Non Query param) Error: {0}", e.InnerException);
            }
            finally
            {
                Close_Connection();
                command.Dispose();
            }
        }
        return dataSet;
    }
    /// <summary>
    /// Executes a Transact-SQL statement against the connection and returns the result in table format
    /// Used only when we have to return records in single table format
    /// </summary>
    /// <param name="procedureName">Store Procedure Name</param>
    /// <returns></returns>
    public DataTable ReturnDataTable(string procedureName)
    {
        objSqlConnection = new SqlConnection(_connectionString);
        DataTable dataTable = new DataTable();
        SqlCommand command = new SqlCommand(procedureName, objSqlConnection);
        command.CommandType = CommandType.StoredProcedure;
        SqlDataAdapter DataAdapter = new SqlDataAdapter();
        DataAdapter.SelectCommand = command;
        Open_Connection();
        using (SqlTransaction Transaction = objSqlConnection.BeginTransaction())
        {
            try
            {
                DataAdapter.SelectCommand.Transaction = Transaction;
                DataAdapter.Fill(dataTable);
                Transaction.Commit();
            }
            catch(Exception e)
            {
                Transaction.Rollback();
                log = new GoldLogging();
                log.SendErrorToText(e);
                    throw new GoldException("Data Execution(Non Query param) Error: {0}", e.InnerException);
            }
            finally
            {
                Close_Connection();
                command.Dispose();

            }
        }
        return dataTable;
    }
    /// <summary>
    /// Executes a Transact-SQL statement against the connection and returns the result in table format
    /// Used only when we have to return records in single table format
    /// </summary>
    /// <param name="procedureName">Store Procedure Name</param>
    /// <param name="parameters">Parameters</param>
    /// <returns></returns>
    public DataTable ReturnDataTableWithParameters(string procedureName, SqlParameter[] parameters)
    {
        objSqlConnection = new SqlConnection(_connectionString);
        DataTable dataTable = new DataTable();
        SqlCommand command = new SqlCommand(procedureName, objSqlConnection);
        command.CommandType = CommandType.StoredProcedure;

        if (parameters != null)
            command.Parameters.AddRange(parameters);

        SqlDataAdapter DataAdapter = new SqlDataAdapter();
        DataAdapter.SelectCommand = command;
        Open_Connection();

        using (SqlTransaction Transaction = objSqlConnection.BeginTransaction())
        {
            try
            {
                DataAdapter.SelectCommand.Transaction = Transaction;
                DataAdapter.Fill(dataTable);
                Transaction.Commit();
            }
            catch (Exception e)
            {
                Transaction.Rollback();
                log = new GoldLogging();
                log.SendErrorToText(e);
                throw new GoldException("Data Execution(Non Query param) Error: {0}", e.InnerException);
            }
            finally
            {
                Close_Connection();
                command.Dispose();
            }
        }
        return dataTable;
    }
    public string getip()
    {
        string VisitorsIPAddr = string.Empty;

        if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
        {
            VisitorsIPAddr = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
        }
        else if (HttpContext.Current.Request.UserHostAddress.Length != 0)
        {
            VisitorsIPAddr = HttpContext.Current.Request.UserHostAddress;
        }
        return VisitorsIPAddr;

    }
    public System.Data.SqlClient.SqlDataAdapter Return_Record(string s1)
    {

        SqlCommand cmd = new SqlCommand(s1, objSqlConnection);
        SqlDataAdapter da = new SqlDataAdapter(cmd);

        return da;

    }
    public SqlDataAdapter return_da(string s1)
    {
        Open_Connection();
        SqlCommand cmd1 = new SqlCommand(s1, objSqlConnection);
        cmd1.CommandType = CommandType.Text;
        SqlDataAdapter da = new SqlDataAdapter(cmd1);
        Close_Connection();
        return da;

    }
    public int delete_val(String strSql)
    {
        int delval = 0;

        objCommand = new SqlCommand();

        Open_Connection();
        objCommand.Connection = objSqlConnection;
        objCommand.CommandType = CommandType.Text;
        objCommand.CommandText = strSql;
        //try
        //{
        delval = objCommand.ExecuteNonQuery();
        Close_Connection();
        //}
        //catch (Exception e)
        //{
        //    Console.WriteLine("Insertion Problem" + e);
        //}
        //finally{
        //    objCmd.Dispose();
        //        }
        if (delval > 0)
        {
            return 1;
        }
        else
        {
            return 0;
        }

    }
    public DataTable return_dt(string s1)
    {

        Open_Connection();
        SqlCommand cmd1 = new SqlCommand(s1, objSqlConnection);
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(cmd1);
        da.Fill(dt);
        Close_Connection();
        return dt;

    }
    public int ExecDB(String strSql)
    {
        int retVal = 0;

        objCommand = new SqlCommand();
        Open_Connection();
        objCommand.Connection = objSqlConnection;
        objCommand.CommandType = CommandType.Text;
        objCommand.CommandText = strSql;
        //try
        //{
        retVal = objCommand.ExecuteNonQuery();
        Close_Connection();
        //}
        //catch (Exception e)
        //{
        //    Console.WriteLine("Insertion Problem" + e);
        //}
        //finally{
        //    objCmd.Dispose();
        //        }
        if (retVal > 0)
        {
            return 1;
        }
        else
        {
            return 0;
        }

    }
}