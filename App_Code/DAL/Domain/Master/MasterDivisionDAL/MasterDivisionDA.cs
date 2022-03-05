using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MasterDivisionDA
/// </summary>
public class MasterDivisionDA : IMasterDivisionDA
{
    private readonly IGoldDataAccess _goldDataAccess;
    public MasterDivisionDA()
    {
        this._goldDataAccess = new GoldDataAccess();
    }
    public bool GetMasterDivisionCheck(long recid, int seq)
    {
        bool result = false;
        SqlParameter[] objParameter = new SqlParameter[2];
        objParameter[0] = new SqlParameter("@Recid", SqlDbType.BigInt);
        objParameter[0].Value = recid;
        objParameter[1] = new SqlParameter("@Seqid", SqlDbType.Int);
        objParameter[1].Value = seq;
        var dt = _goldDataAccess.ReturnDataTableWithParameters("Check_MasterDivisionMast_SeqNo", objParameter);
        if (dt.Rows.Count > 0)
        {
            result = true;
        }
        return result;
    }
    public bool GetMasterDivisionCheck(string name,string code,long recid)
    {
        bool result = false;
        SqlParameter[] objParameter = new SqlParameter[3];
        objParameter[0] = new SqlParameter("@divnm1", SqlDbType.VarChar,500);
        objParameter[0].Value = name;
        objParameter[0] = new SqlParameter("@divcode1", SqlDbType.VarChar,500);
        objParameter[0].Value = code;
        objParameter[0] = new SqlParameter("@trackid", SqlDbType.BigInt);
        objParameter[0].Value = recid;
        var dt = _goldDataAccess.ReturnDataTableWithParameters("MasterDivisioncheck", objParameter);
        if (dt.Rows.Count > 0)
        {
            result = true;
        }
        return result;
    }
    public DataTable GetMasterDivisionList()
    {
        return _goldDataAccess.ReturnDataTable("MasterDivisionselect");
    }
    public bool DelMasterDivision(MasterDivisionModel.MasterDivisionDel mdd)
    {
        bool result = false;
        SqlParameter[] objParameter = new SqlParameter[3];
        objParameter[0] = new SqlParameter("@crtuid1", SqlDbType.Int);
        objParameter[0].Value = mdd.Uid;
        objParameter[1] = new SqlParameter("@logon1", SqlDbType.BigInt);
        objParameter[1].Value = mdd.Logno;
        objParameter[2] = new SqlParameter("@recid", SqlDbType.BigInt);
        objParameter[2].Value = mdd.Recid;
        return bool.TryParse(_goldDataAccess.ExecuteNonQueryWithParameters("divisiondel", objParameter).ToString(), out result);
    }
    public DataTable GetMasterDivisionByID(long recid)
    {
        SqlParameter[] objParameter = new SqlParameter[1];
        objParameter[0] = new SqlParameter("@recid", SqlDbType.BigInt);
        objParameter[0].Value = recid;
        return _goldDataAccess.ReturnDataTableWithParameters("Masterdivisionshow", objParameter);
    }
    public bool CheckDataIntregity(long rcid, Guid guid)
    {
        bool result = false;
        SqlParameter[] objParameter = new SqlParameter[2];
        objParameter[0] = new SqlParameter("@recid", SqlDbType.BigInt);
        objParameter[0].Value = rcid;
        objParameter[1] = new SqlParameter("@uniqueid", SqlDbType.UniqueIdentifier);
        objParameter[1].Value = guid;
        var dt = _goldDataAccess.ReturnDataTableWithParameters("Check_MasterDivisionMast_Data", objParameter);
        if (dt.Rows.Count > 0)
        {
            result = true;
        }
        return result;
    }
}