using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for EmailDA
/// </summary>
public class EmailDA : IEmailDA
{
    private readonly IGoldDataAccess _goldDataAccess;
    public EmailDA()
    {
        this._goldDataAccess = new GoldDataAccess();
    }

    public DataTable GetEmailData()
    {
        return _goldDataAccess.ReturnDataTable("Gold_EmailAccountshow");
    }

    public bool CheckEmailData(string EmailID)
    {

        bool result = false;
        SqlParameter[] objParameter = new SqlParameter[1];

        objParameter[0] = new SqlParameter("@Email", SqlDbType.NVarChar, 255);
        objParameter[0].Value = EmailID;

        var dt = _goldDataAccess.ReturnDataTableWithParameters("Gold_EmailAccountCheck", objParameter);
        if (dt.Rows.Count > 0)
        {
            result = true;
        }
        return result;
    }


    public DataTable GetEmailById(int EmailID)
    {

      
        SqlParameter[] objParameter = new SqlParameter[1];

        objParameter[0] = new SqlParameter("@EmailAccountId", SqlDbType.NVarChar, 255);
        objParameter[0].Value = EmailID;

        return _goldDataAccess.ReturnDataTableWithParameters("Gold_EmailAccountSelect", objParameter);
       
    }
    public bool SaveEmailData(string EmailId,string DisplayName,string Host,int Port,string UserName, string Password, byte EnableSSL,byte UseDefaultCredentials,byte DefaultEmail,int EmailLimit,int logno,int Createuid)
    {
        bool result = false;
        SqlParameter[] objParameter = new SqlParameter[12];
      
        objParameter[0] = new SqlParameter("@Email", SqlDbType.NVarChar, 255);
        objParameter[0].Value = EmailId;

        objParameter[1] = new SqlParameter("@DisplayName", SqlDbType.NVarChar , 255);
        objParameter[1].Value = DisplayName;

        objParameter[2] = new SqlParameter("@Host", SqlDbType.NVarChar, 255);
        objParameter[2].Value = Host;

        objParameter[3] = new SqlParameter("@Port", SqlDbType.Int);
        objParameter[3].Value = Port;

        objParameter[4] = new SqlParameter("@UserName", SqlDbType.NVarChar, 255);
        objParameter[4].Value = UserName;

        objParameter[5] = new SqlParameter("@Password", SqlDbType.NVarChar , 255);
        objParameter[5].Value = Password;

        objParameter[6] = new SqlParameter("@EnableSSL", SqlDbType.Bit);
        objParameter[6].Value = EnableSSL;

        objParameter[7] = new SqlParameter("@UseDefaultCredentials", SqlDbType.Bit);
        objParameter[7].Value = UseDefaultCredentials;

        objParameter[7] = new SqlParameter("@UseDefaultCredentials", SqlDbType.Bit);
        objParameter[7].Value = UseDefaultCredentials;

        objParameter[8] = new SqlParameter("@DefaultEmail", SqlDbType.Bit);
        objParameter[8].Value = DefaultEmail;

        objParameter[9] = new SqlParameter("@EmailLimit", SqlDbType.Int);
        objParameter[9].Value = EmailLimit;

        objParameter[10] = new SqlParameter("@logno", SqlDbType.Int);
        objParameter[10].Value = logno;

        objParameter[11] = new SqlParameter("@createuid", SqlDbType.Int);
        objParameter[11].Value = Createuid;

        var dt = _goldDataAccess.ReturnDataTableWithParameters("Gold_EmailAccountAdd", objParameter);
        if (dt.Rows.Count > 0)
        {
            result = true;
        }
        return result;
    }


    public bool UpdateEmailData(string EmailId, string DisplayName, string Host, int Port, string UserName, string Password, byte EnableSSL, byte UseDefaultCredentials, byte DefaultEmail, int EmailLimit, int logno, int Createuid,int Id)
    {
        bool result = false;
        SqlParameter[] objParameter = new SqlParameter[13];

        objParameter[0] = new SqlParameter("@Email", SqlDbType.NVarChar, 255);
        objParameter[0].Value = EmailId;

        objParameter[1] = new SqlParameter("@DisplayName", SqlDbType.NVarChar, 255);
        objParameter[1].Value = DisplayName;

        objParameter[2] = new SqlParameter("@Host", SqlDbType.NVarChar, 255);
        objParameter[2].Value = Host;

        objParameter[3] = new SqlParameter("@Port", SqlDbType.Int);
        objParameter[3].Value = Port;

        objParameter[4] = new SqlParameter("@UserName", SqlDbType.NVarChar, 255);
        objParameter[4].Value = UserName;

        objParameter[5] = new SqlParameter("@Password", SqlDbType.NVarChar, 255);
        objParameter[5].Value = Password;

        objParameter[6] = new SqlParameter("@EnableSSL", SqlDbType.Bit);
        objParameter[6].Value = EnableSSL;

        objParameter[7] = new SqlParameter("@UseDefaultCredentials", SqlDbType.Bit);
        objParameter[7].Value = UseDefaultCredentials;

        objParameter[8] = new SqlParameter("@DefaultEmail", SqlDbType.Bit);
        objParameter[8].Value = DefaultEmail;

        objParameter[9] = new SqlParameter("@EmailLimit", SqlDbType.Int);
        objParameter[9].Value = EmailLimit;

        objParameter[10] = new SqlParameter("@logno", SqlDbType.Int);
        objParameter[10].Value = logno;

        objParameter[11] = new SqlParameter("@lmodifyuid", SqlDbType.Int);
        objParameter[11].Value = Createuid;

        objParameter[12] = new SqlParameter("@EmailAccountId", SqlDbType.Int);
        objParameter[12].Value = Id;

        var dt = _goldDataAccess.ReturnDataTableWithParameters("Gold_EmailAccountEdit", objParameter);
        if (dt.Rows.Count > 0)
        {
            result = true;
        }
        return result;
    }
    public DataTable EmailMappingCheckByEmailId(int EmailId)
    {

        SqlParameter[] objParameter = new SqlParameter[1];

        objParameter[0] = new SqlParameter("@EmailAccountId", SqlDbType.Int);
        objParameter[0].Value = EmailId;

        return _goldDataAccess.ReturnDataTableWithParameters("Gold_EmailMappingCheckByEmailId", objParameter);
        
    }
}