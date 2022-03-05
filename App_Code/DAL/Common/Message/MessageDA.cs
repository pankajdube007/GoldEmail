using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for MessageDA
/// </summary>
public class MessageDA : IMessageDA
{
    private readonly IGoldDataAccess _goldDataAccess;
    public MessageDA()
    {
        this._goldDataAccess = new GoldDataAccess();
    }
    public DataTable GetEmailAccountDataList()
    {
        return _goldDataAccess.ReturnDataTable("Get_Gold_EmailAccount_List");
    }
    public DataTable GetEmailAccountDataByID(int recid)
    {
        SqlParameter[] objParameter = new SqlParameter[1];
        objParameter[0] = new SqlParameter("@EmailAccountId", SqlDbType.Int);
        objParameter[0].Value = recid;
        return _goldDataAccess.ReturnDataTableWithParameters("Gold_EmailAccountSelect", objParameter);
    }
    public DataTable GetEmailAccountDataByName(string name)
    {
        SqlParameter[] objParameter = new SqlParameter[1];
        objParameter[0] = new SqlParameter("@name", SqlDbType.VarChar, 200);
        objParameter[0].Value = name;
        return _goldDataAccess.ReturnDataTableWithParameters("Get_Gold_EmailAccount_ByName", objParameter);
    }
    public DataTable GetMessageTemplateDataList()
    {
        return _goldDataAccess.ReturnDataTable("Get_Gold_MessageTemplate_List");
    }
    public DataTable GetMessageTemplateDataByID(int recid)
    {
        SqlParameter[] objParameter = new SqlParameter[1];
        objParameter[0] = new SqlParameter("@recid", SqlDbType.Int);
        objParameter[0].Value = recid;
        return _goldDataAccess.ReturnDataTableWithParameters("Get_Gold_MessageTemplate_ByID", objParameter);
    }
    public DataTable GetMessageTemplateDataByName(string name)
    {
        SqlParameter[] objParameter = new SqlParameter[1];
        objParameter[0] = new SqlParameter("@name", SqlDbType.VarChar, 200);
        objParameter[0].Value = name;
        return _goldDataAccess.ReturnDataTableWithParameters("Get_Gold_MessageTemplate_ByName", objParameter);
    }
    public DataTable GetLocalizedMessageTemplateDataByID(int recid)
    {
        SqlParameter[] objParameter = new SqlParameter[1];
        objParameter[0] = new SqlParameter("@recid", SqlDbType.Int);
        objParameter[0].Value = recid;
        return _goldDataAccess.ReturnDataTableWithParameters("Get_Gold_MessageTemplateLocalized_ByID", objParameter);
    }
    public DataTable GetLocalizedMessageTemplateDataByName(string name)
    {
        SqlParameter[] objParameter = new SqlParameter[1];
        objParameter[0] = new SqlParameter("@name", SqlDbType.VarChar, 200);
        objParameter[0].Value = name;
        return _goldDataAccess.ReturnDataTableWithParameters("Get_Gold_MessageTemplateLocalized_ByName", objParameter);
    }
}





