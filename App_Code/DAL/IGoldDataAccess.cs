﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for IGoldDataAccess
/// </summary>
public interface IGoldDataAccess
{
    object ExecuteScalar(string procedureName);
    object ExecuteScalarWithParameters(string procedureName, SqlParameter[] parameters);
    object ExecuteNonQuery(string procedureName);
    object ExecuteNonQueryWithParameters(string procedureName, SqlParameter[] parameters);
    object ExecuteNonQueryWithOutputParameters(string procedureName, SqlParameter[] parameters);
    DataSet ReturnDataset(string procedureName);
    DataSet ReturnDatasetWithParameters(string procedureName, SqlParameter[] parameters);
    DataTable ReturnDataTable(string procedureName);
    DataTable ReturnDataTableWithParameters(string procedureName, SqlParameter[] parameters);

}