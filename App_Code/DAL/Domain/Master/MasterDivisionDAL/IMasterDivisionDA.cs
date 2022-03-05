using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for IMasterDivisionDA
/// </summary>
public interface IMasterDivisionDA
{
    bool GetMasterDivisionCheck(long recid, int seq);
    bool GetMasterDivisionCheck(string name, string code, long recid);
    DataTable GetMasterDivisionList();
    bool DelMasterDivision(MasterDivisionModel.MasterDivisionDel mdd);
    DataTable GetMasterDivisionByID(long recid);
    bool CheckDataIntregity(long rcid, Guid guid);
}