using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MasterDivisionModel
/// </summary>
public partial class MasterDivisionModel
{
    public MasterDivisionModel()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public class MasterDivisionCkeck
    {
        public string TblName { get; set;}
        public string Recid { get; set; }
        public string Seqid { get; set; }
    }
    public class MasterDivisionDel
    {
        public int Uid { get; set; }
        public long Logno { get; set; }
        public long Recid { get; set; }
    }
}