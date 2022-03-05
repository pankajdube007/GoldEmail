using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SettingsDA
/// </summary>
public class SettingsDA : ISettingsDA
{
    private readonly IGoldDataAccess _goldDataAccess;
    public SettingsDA()
    {
        this._goldDataAccess = new GoldDataAccess();
    }
    public DataTable GetSettingDataList()
    {
        return _goldDataAccess.ReturnDataTable("Get_Gold_Setting_List");
    }
    public DataTable GetSettingDataByID(int recid)
    {
        SqlParameter[] objParameter = new SqlParameter[1];
        objParameter[0] = new SqlParameter("@recid", SqlDbType.Int);
        objParameter[0].Value = recid;
        return _goldDataAccess.ReturnDataTableWithParameters("Get_Gold_Setting_ByID", objParameter);
    }
    public DataTable GetSettingDataByName(string name)
    {
        SqlParameter[] objParameter = new SqlParameter[1];
        objParameter[0] = new SqlParameter("@name", SqlDbType.VarChar,200);
        objParameter[0].Value = name;
        return _goldDataAccess.ReturnDataTableWithParameters("Get_Gold_Setting_ByName", objParameter);
    }
}