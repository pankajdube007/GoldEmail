using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for EmailTemplateDA
/// </summary>
public class EmailTemplateDA :IEmailTemplateDA
{
    private readonly IGoldDataAccess _goldDataAccess;
 
    public EmailTemplateDA()
    {
        this._goldDataAccess = new GoldDataAccess();
       
    }

    public bool AddHeader(string HeaderName,byte Email,byte SMS,int UserName, int Logno)
    {
        bool result = false;
        SqlParameter[] objParameter = new SqlParameter[5];

        objParameter[0] = new SqlParameter("@name", SqlDbType.NVarChar);
        objParameter[0].Value = HeaderName;

        objParameter[1] = new SqlParameter("@Email", SqlDbType.Bit);
        objParameter[1].Value = Email;

        objParameter[2] = new SqlParameter("@SMS", SqlDbType.Bit);
        objParameter[2].Value = SMS;

        objParameter[3] = new SqlParameter("@createuid", SqlDbType.Int);
        objParameter[3].Value = UserName;

        objParameter[4] = new SqlParameter("@logon", SqlDbType.Int);
        objParameter[4].Value = Logno;




        var dt = _goldDataAccess.ReturnDataTableWithParameters("Gold_MessageTemplateadd", objParameter);
        if (dt.Rows.Count > 0)
        {
            result = true;
        }
        return result;
    }


    public bool AddAvailableTokanCreation(string TokanName, string SampleValue, int Logno, int UserName )
    {
        bool result = false;
        SqlParameter[] objParameter = new SqlParameter[4];

        objParameter[0] = new SqlParameter("@TokanName", SqlDbType.NVarChar);
        objParameter[0].Value = TokanName;

        objParameter[1] = new SqlParameter("@SampleValue", SqlDbType.NVarChar);
        objParameter[1].Value = SampleValue;

        objParameter[2] = new SqlParameter("@logno", SqlDbType.Int);
        objParameter[2].Value = Logno;

        objParameter[3] = new SqlParameter("@createuid", SqlDbType.Int);
        objParameter[3].Value = UserName;

       




        var dt = _goldDataAccess.ReturnDataTableWithParameters("Gold_AvailableTokanCreationAdd", objParameter);
        if (dt.Rows.Count > 0)
        {
            result = true;
        }
        return result;
    }


    public bool AddHeaderTokanMapping(int HeaderId, string TokanId,string TokanNames, int Logno, int UserName)
    {
        bool result = false;
        SqlParameter[] objParameter = new SqlParameter[5];

        objParameter[0] = new SqlParameter("@HeaderId", SqlDbType.Int);
        objParameter[0].Value = HeaderId;

        objParameter[1] = new SqlParameter("@TokanId", SqlDbType.NVarChar);
        objParameter[1].Value = TokanId;

        objParameter[2] = new SqlParameter("@TokanNames", SqlDbType.NVarChar);
        objParameter[2].Value = TokanNames;

        objParameter[3] = new SqlParameter("@logno", SqlDbType.Int);
        objParameter[3].Value = Logno;

        objParameter[4] = new SqlParameter("@createuid", SqlDbType.Int);
        objParameter[4].Value = UserName;






        var dt = _goldDataAccess.ReturnDataTableWithParameters("Gold_TokanHeaderMappingAdd", objParameter);
        if (dt.Rows.Count > 0)
        {
            result = true;
        }
        return result;
    }


    public bool AddEmailTemplates(int HeaderId, string BCC, string Subject, string Body, byte Active, int EmailId,int logno, int UserName)
    {
        bool result = false;
        SqlParameter[] objParameter = new SqlParameter[8];

        objParameter[0] = new SqlParameter("@headid", SqlDbType.Int);
        objParameter[0].Value = HeaderId;

        objParameter[1] = new SqlParameter("@bccaddress", SqlDbType.NVarChar);
        objParameter[1].Value = BCC;

        objParameter[2] = new SqlParameter("@subject", SqlDbType.NVarChar);
        objParameter[2].Value = Subject;

        objParameter[3] = new SqlParameter("@body", SqlDbType.NVarChar);
        objParameter[3].Value = Body;

        objParameter[4] = new SqlParameter("@isactive", SqlDbType.Bit);
        objParameter[4].Value = Active;

        objParameter[5] = new SqlParameter("@emailaccountid", SqlDbType.NVarChar);
        objParameter[5].Value = EmailId;

        objParameter[6] = new SqlParameter("@logon", SqlDbType.Int);
        objParameter[6].Value = logno;

        objParameter[7] = new SqlParameter("@createuid", SqlDbType.Int);
        objParameter[7].Value = UserName;

        var dt = _goldDataAccess.ReturnDataTableWithParameters("Gold_MessageTemplateLocalizedadd", objParameter);
        if (dt.Rows.Count > 0)
        {
            result = true;
        }
        return result;
    }


    public bool EditEmailTemplates(int HeaderId, string BCC, string Subject, string Body, byte Active,  int logno, int UserName,int ID ,string Tokan, string Time, string Date, string url,string Signature,int interval)
    {
        bool result = false;
        SqlParameter[] objParameter = new SqlParameter[14];

        objParameter[0] = new SqlParameter("@HeaderId", SqlDbType.Int);
        objParameter[0].Value = HeaderId;

        objParameter[1] = new SqlParameter("@bccaddress", SqlDbType.NVarChar);
        objParameter[1].Value = BCC;

        objParameter[2] = new SqlParameter("@subject", SqlDbType.NVarChar);
        objParameter[2].Value = Subject;

        objParameter[3] = new SqlParameter("@body", SqlDbType.NVarChar);
        objParameter[3].Value = Body;

        objParameter[4] = new SqlParameter("@isactive", SqlDbType.Bit);
        objParameter[4].Value = Active;
               
        objParameter[5] = new SqlParameter("@logon", SqlDbType.Int);
        objParameter[5].Value = logno;

        objParameter[6] = new SqlParameter("@lmodifyuid", SqlDbType.Int);
        objParameter[6].Value = UserName;

        objParameter[7] = new SqlParameter("@messageid", SqlDbType.Int);
        objParameter[7].Value = ID;

        objParameter[8] = new SqlParameter("@UsedTokan", SqlDbType.NVarChar);
        objParameter[8].Value = Tokan;

        objParameter[9] = new SqlParameter("@EmailTime", SqlDbType.NVarChar);
        objParameter[9].Value = Time;

        objParameter[10] = new SqlParameter("@EmailLastDate", SqlDbType.NVarChar);
        objParameter[10].Value = Date;

        objParameter[11] = new SqlParameter("@AttachmentUrl", SqlDbType.NVarChar);
        objParameter[11].Value = url;

        objParameter[12] = new SqlParameter("@Signature", SqlDbType.NVarChar);
        objParameter[12].Value = Signature;

        objParameter[13] = new SqlParameter("@Interval", SqlDbType.Int);
        objParameter[13].Value = interval;

        //  var dt = _goldDataAccess.ReturnDataTableWithParameters("Gold_MessageTemplateLocalizedEdit", objParameter);
        return bool.TryParse(_goldDataAccess.ExecuteNonQueryWithParameters("Gold_MessageTemplateLocalizedEdit", objParameter).ToString(), out result);
    }

    public bool EditHeaderTokanMapping(int HeaderId, string TokanId,string TokanNames, int Logno, int UserName,int Id)
    {
        bool result = false;
        SqlParameter[] objParameter = new SqlParameter[6];

        objParameter[0] = new SqlParameter("@HeaderId", SqlDbType.Int);
        objParameter[0].Value = HeaderId;

        objParameter[1] = new SqlParameter("@TokanId", SqlDbType.NVarChar);
        objParameter[1].Value = TokanId;

        objParameter[2] = new SqlParameter("@TokanNames", SqlDbType.NVarChar);
        objParameter[2].Value = TokanNames;

        objParameter[3] = new SqlParameter("@logno", SqlDbType.Int);
        objParameter[3].Value = Logno;

        objParameter[4] = new SqlParameter("@createuid", SqlDbType.Int);
        objParameter[4].Value = UserName;


        objParameter[5] = new SqlParameter("@ID", SqlDbType.Int);
        objParameter[5].Value = Id;




        var dt = _goldDataAccess.ReturnDataTableWithParameters("Gold_TokanHeaderMappingEdit", objParameter);
        if (dt.Rows.Count > 0)
        {
            result = true;
        }
        return result;
    }

    public bool EditHeader(string HeaderName, byte Email, byte SMS, int UserName, int Logno,int ID)
    {
        bool result = false;
        SqlParameter[] objParameter = new SqlParameter[6];

        objParameter[0] = new SqlParameter("@name", SqlDbType.NVarChar);
        objParameter[0].Value = HeaderName;

        objParameter[1] = new SqlParameter("@Email", SqlDbType.Bit);
        objParameter[1].Value = Email;

        objParameter[2] = new SqlParameter("@SMS", SqlDbType.Bit);
        objParameter[2].Value = SMS;

        objParameter[3] = new SqlParameter("@createuid", SqlDbType.Int);
        objParameter[3].Value = UserName;

        objParameter[4] = new SqlParameter("@logon", SqlDbType.Int);
        objParameter[4].Value = Logno;


        objParameter[5] = new SqlParameter("@ID", SqlDbType.Int);
        objParameter[5].Value = ID;

        var dt = _goldDataAccess.ReturnDataTableWithParameters("Gold_MessageTemplateEdit", objParameter);
        if (dt.Rows.Count > 0)
        {
            result = true;
        }
        return result;
    }


    public bool EditAvailableTokanCreation(string TokanName, string SampleValue, int UserName, int Logno, int ID)
    {
        bool result = false;
        SqlParameter[] objParameter = new SqlParameter[5];

        objParameter[0] = new SqlParameter("@TokanName", SqlDbType.NVarChar);
        objParameter[0].Value = TokanName;

        objParameter[1] = new SqlParameter("@SampleValue", SqlDbType.NVarChar);
        objParameter[1].Value = SampleValue;       

        objParameter[2] = new SqlParameter("@lmodifyuid", SqlDbType.Int);
        objParameter[2].Value = UserName;

        objParameter[3] = new SqlParameter("@logno", SqlDbType.Int);
        objParameter[3].Value = Logno;


        objParameter[4] = new SqlParameter("@ID", SqlDbType.Int);
        objParameter[4].Value = ID;

        var dt = _goldDataAccess.ReturnDataTableWithParameters("Gold_AvailableTokanCreationEdit", objParameter);
        if (dt.Rows.Count > 0)
        {
            result = true;
        }
        return result;
    }

    public DataTable GetAllHeader()
    {
        return _goldDataAccess.ReturnDataTable("Gold_MessageTemplateshow");
    }

    public DataTable GetAllHeaderForEmail()
    {
        return _goldDataAccess.ReturnDataTable("Gold_MessageTemplateshowForEmail");
    }


    public DataTable GetAllHeaderForMessage()
    {
        return _goldDataAccess.ReturnDataTable("Gold_MessageTemplateshowForMeassage");
    }

    public DataTable GetQueuedEmail()
    {
        return _goldDataAccess.ReturnDataTable("Gold_QueuedEmailShow");
    }

    public DataTable GetSendEmail( string fromdate, string todate)
    {
        SqlParameter[] objParameter = new SqlParameter[2];

        objParameter[0] = new SqlParameter("@fromdate", SqlDbType.NVarChar);
        objParameter[0].Value = fromdate;

        objParameter[1] = new SqlParameter("@Todate", SqlDbType.NVarChar);
        objParameter[1].Value = todate;

        return _goldDataAccess.ReturnDataTableWithParameters("Gold_SendEmailShow", objParameter);
    
    }

    public DataTable GetQueuedMessage()
    {
        return _goldDataAccess.ReturnDataTable("Gold_QueuedMessageShow");
    }

    public DataTable GetSendMessage(string fromdate, string todate)

    {
        SqlParameter[] objParameter = new SqlParameter[2];

        objParameter[0] = new SqlParameter("@fromdate", SqlDbType.NVarChar);
        objParameter[0].Value = fromdate;

        objParameter[1] = new SqlParameter("@Todate", SqlDbType.NVarChar);
        objParameter[1].Value = todate;

        return _goldDataAccess.ReturnDataTableWithParameters("Gold_SendMessageShow", objParameter);
       
    }

    public bool DeleteQueuedEmailByID(int ID)
    {
        bool result = false;
        SqlParameter[] objParameter = new SqlParameter[1];

        objParameter[0] = new SqlParameter("@ID", SqlDbType.Int);
        objParameter[0].Value = ID;

      

        var dt = _goldDataAccess.ReturnDataTableWithParameters("Gold_QueuedEmailDeleteByID", objParameter);
        if (dt.Rows.Count > 0)
        {
            result = true;
        }
        return result;
    }

    public DataTable GetSendEmailByID(int ID)
    {
       
        SqlParameter[] objParameter = new SqlParameter[1];

        objParameter[0] = new SqlParameter("@EmailID", SqlDbType.Int);
        objParameter[0].Value = ID;



        
        return _goldDataAccess.ReturnDataTableWithParameters("Gold_SendEmailSelect", objParameter);
    }

    public DataTable GetSendMessageByID(int ID)
    {

        SqlParameter[] objParameter = new SqlParameter[1];

        objParameter[0] = new SqlParameter("@ID", SqlDbType.Int);
        objParameter[0].Value = ID;




        return _goldDataAccess.ReturnDataTableWithParameters("Gold_SendMessageSelect", objParameter);
    }


    public DataTable GetQueuedEmailHistoryforExandag(string EmailID, int TempId)
    {

        SqlParameter[] objParameter = new SqlParameter[2];

        objParameter[0] = new SqlParameter("@To", SqlDbType.NVarChar);
        objParameter[0].Value = EmailID;

        objParameter[1] = new SqlParameter("@TempId", SqlDbType.Int);
        objParameter[1].Value = TempId;


        return _goldDataAccess.ReturnDataTableWithParameters("Gold_QueuedEmailcheckforagtandsalesex", objParameter);
    }
    public bool DeleteQueuedMessageByID(int ID)
    {
        bool result = false;
        SqlParameter[] objParameter = new SqlParameter[1];

        objParameter[0] = new SqlParameter("@ID", SqlDbType.Int);
        objParameter[0].Value = ID;



        var dt = _goldDataAccess.ReturnDataTableWithParameters("Gold_QueuedMessageDeleteByID", objParameter);
        if (dt.Rows.Count > 0)
        {
            result = true;
        }
        return result;
    }
    public DataTable GetAllHeaderAfterSelection()
    {
        return _goldDataAccess.ReturnDataTable("Gold_MessageTemplateAfterSelectionShow");
    }

    public DataTable GetAllAvailableTokanCreation()
    {
        return _goldDataAccess.ReturnDataTable("Gold_AvailableTokanCreationShow");
    }

    public DataTable GetAllTokanHeaderMapping()
    {
        return _goldDataAccess.ReturnDataTable("Gold_TokanHeaderMappingShow");
    }

    public DataTable GetAllEmail()
    {
        return _goldDataAccess.ReturnDataTable("Gold_EmailAccountshow");
    }

    public DataTable GetAllEmailTemplates()
    {
        return _goldDataAccess.ReturnDataTable("Gold_MessageTemplateLocalizedshow");
    }

    public DataTable GetAllMessageTemplates()
    {
        return _goldDataAccess.ReturnDataTable("Gold_MessageTemplateLocalizedForMessageShow");
    }
    public DataTable GetAvailableTokanCreationaAfterMapping (string ID)
    {
        SqlParameter[] objParameter = new SqlParameter[1];

        objParameter[0] = new SqlParameter("@ID", SqlDbType.NVarChar);
        objParameter[0].Value = ID;

        return _goldDataAccess.ReturnDataTableWithParameters("Gold_AvailableTokanCreationAfterMappingShow", objParameter);
    }

    public DataTable CheckHeader(string HeaderName)
    {

        SqlParameter[] objParameter = new SqlParameter[1];

        objParameter[0] = new SqlParameter("@name", SqlDbType.NVarChar);
        objParameter[0].Value = HeaderName;

        return _goldDataAccess.ReturnDataTableWithParameters("Gold_MessageTemplateselect", objParameter);
    }

    public DataTable CheckExistTokanInTemplates(string TokanName)
    {

        SqlParameter[] objParameter = new SqlParameter[1];

        objParameter[0] = new SqlParameter("@TokanName", SqlDbType.NVarChar);
        objParameter[0].Value = TokanName;

        return _goldDataAccess.ReturnDataTableWithParameters("Gold_AvailableTokanInTemplatesCheck", objParameter);
    }



    public DataTable CheckExistTokanMapping(string TokanName,int ID)
    {

        SqlParameter[] objParameter = new SqlParameter[2];

        objParameter[0] = new SqlParameter("@TokanName", SqlDbType.NVarChar);
        objParameter[0].Value = TokanName;

        objParameter[1] = new SqlParameter("@ID", SqlDbType.Int);
        objParameter[1].Value = ID;

        return _goldDataAccess.ReturnDataTableWithParameters("Gold_AvailableTokanMappingCheck", objParameter);
    }


    public DataTable CheckEmailMapping(int TemplateId, int EmailId)
    {

        SqlParameter[] objParameter = new SqlParameter[2];

        objParameter[0] = new SqlParameter("@localTemplateId", SqlDbType.Int);
        objParameter[0].Value = TemplateId;

        objParameter[1] = new SqlParameter("@EmailAccountId", SqlDbType.Int);
        objParameter[1].Value = EmailId;

        return _goldDataAccess.ReturnDataTableWithParameters("Gold_EmailMappingCheck", objParameter);
    }

    public bool DeleteEmailMapping(int TemplateId, int EmailId)
    {
        bool result = false;
        SqlParameter[] objParameter = new SqlParameter[2];

        objParameter[0] = new SqlParameter("@LocalTemplateId", SqlDbType.Int);
        objParameter[0].Value = TemplateId;

        objParameter[1] = new SqlParameter("@EmailAccountId", SqlDbType.Int);
        objParameter[1].Value = EmailId;

        var dt = _goldDataAccess.ReturnDataTableWithParameters("Gold_EmailMappingDelete", objParameter);
        if (dt.Rows.Count > 0)
        {
            result = true;
        }
        return result;
    }


    public bool EditEmailMapping(int TemplateId, int EmailId, int SequenceNo, int UserName, int Logno, int ID)
    {
        bool result = false;
        SqlParameter[] objParameter = new SqlParameter[6];

        objParameter[0] = new SqlParameter("@LocalTemplateId", SqlDbType.Int);
        objParameter[0].Value = TemplateId;

        objParameter[1] = new SqlParameter("@EmailAccountId", SqlDbType.Int);
        objParameter[1].Value = EmailId;

        objParameter[2] = new SqlParameter("@SequenceNo", SqlDbType.Int);
        objParameter[2].Value = SequenceNo;

        objParameter[3] = new SqlParameter("@lmodifyuid", SqlDbType.Int);
        objParameter[3].Value = UserName;

        objParameter[4] = new SqlParameter("@logno", SqlDbType.Int);
        objParameter[4].Value = Logno;


        objParameter[5] = new SqlParameter("@SlNo", SqlDbType.Int);
        objParameter[5].Value = ID;

        var dt = _goldDataAccess.ReturnDataTableWithParameters("Gold_EmailMappingEdit", objParameter);
        if (dt.Rows.Count > 0)
        {
            result = true;
        }
        return result;
    }


    public bool ADDEmailMapping(int TemplateId, int EmailId, int SequenceNo, int UserName, int Logno)
    {
        bool result = false;
        SqlParameter[] objParameter = new SqlParameter[5];

        objParameter[0] = new SqlParameter("@LocalTemplateId", SqlDbType.Int);
        objParameter[0].Value = TemplateId;

        objParameter[1] = new SqlParameter("@EmailAccountId", SqlDbType.Int);
        objParameter[1].Value = EmailId;

        objParameter[2] = new SqlParameter("@SequenceNo", SqlDbType.Int);
        objParameter[2].Value = SequenceNo;

        objParameter[3] = new SqlParameter("@Createuid", SqlDbType.Int);
        objParameter[3].Value = UserName;

        objParameter[4] = new SqlParameter("@logno", SqlDbType.Int);
        objParameter[4].Value = Logno;



        var dt = _goldDataAccess.ReturnDataTableWithParameters("Gold_EmailMappingAdd", objParameter);
        if (dt.Rows.Count > 0)
        {
            result = true;
        }
        return result;
    }
    public DataTable CheckAvailableTokanCreationByName(string TokanName)
    {

        SqlParameter[] objParameter = new SqlParameter[1];

        objParameter[0] = new SqlParameter("@TokanName", SqlDbType.NVarChar);
        objParameter[0].Value = TokanName;

        return _goldDataAccess.ReturnDataTableWithParameters("Gold_AvailableTokanCreationByNameCheck", objParameter);
    }


    public DataTable TokanHeaderSelectById(int Id)
    {

        SqlParameter[] objParameter = new SqlParameter[1];

        objParameter[0] = new SqlParameter("@ID", SqlDbType.Int);
        objParameter[0].Value = Id;

        return _goldDataAccess.ReturnDataTableWithParameters("Gold_TokanHeaderMappingselect", objParameter);
    }

    public DataTable GetHaderById(int ID)
    {

        SqlParameter[] objParameter = new SqlParameter[1];

        objParameter[0] = new SqlParameter("@ID", SqlDbType.Int);
        objParameter[0].Value = ID;

        return _goldDataAccess.ReturnDataTableWithParameters("Gold_MessageTemplateSelectByID", objParameter);
    }

    public DataTable GetHaderTokanMappingByHeaderId(int ID)
    {

        SqlParameter[] objParameter = new SqlParameter[1];

        objParameter[0] = new SqlParameter("@ID", SqlDbType.Int);
        objParameter[0].Value = ID;

        return _goldDataAccess.ReturnDataTableWithParameters("Gold_TokanHeaderMappingselectByHeader", objParameter);
    }

    public DataTable GetAvailableTokanCreationById(int ID)
    {

        SqlParameter[] objParameter = new SqlParameter[1];

        objParameter[0] = new SqlParameter("@ID", SqlDbType.Int);
        objParameter[0].Value = ID;

        return _goldDataAccess.ReturnDataTableWithParameters("Gold_AvailableTokanCreationById", objParameter);
    }


    public DataTable GetAvailableTokanCreationByString(string ID)
    {

        SqlParameter[] objParameter = new SqlParameter[1];

        objParameter[0] = new SqlParameter("@ID", SqlDbType.VarChar);
        objParameter[0].Value = ID;

        return _goldDataAccess.ReturnDataTableWithParameters("Gold_AvailableTokanCreationByString", objParameter);
    }
    public DataTable GetEmailAllData(int EmailId)
    {

        SqlParameter[] objParameter = new SqlParameter[1];

        objParameter[0] = new SqlParameter("@ID", SqlDbType.Int);
        objParameter[0].Value = EmailId;

        return _goldDataAccess.ReturnDataTableWithParameters("Gold_MessageTemplateLocalizedselect", objParameter);
    }


    public DataTable GetEmailTemplateDatabyID(int ID)
    {

        SqlParameter[] objParameter = new SqlParameter[1];

        objParameter[0] = new SqlParameter("@ID", SqlDbType.Int);
        objParameter[0].Value = ID;

        return _goldDataAccess.ReturnDataTableWithParameters("Gold_MessageTemplateLocalizedselectByTemplateId", objParameter);
    }


    public DataTable GetEmailForTimeCheckbyID(int ID)
    {

        SqlParameter[] objParameter = new SqlParameter[1];

        objParameter[0] = new SqlParameter("@ID", SqlDbType.Int);
        objParameter[0].Value = ID;

        return _goldDataAccess.ReturnDataTableWithParameters("Gold_CheckforEmailTimeCheck", objParameter);
    }

    public DataTable GetActiveSchemeTimeID(int TemplateId)
    {

        SqlParameter[] objParameter = new SqlParameter[1];

        objParameter[0] = new SqlParameter("@TemplateId", SqlDbType.Int);
        objParameter[0].Value = TemplateId;

        return _goldDataAccess.ReturnDataTableWithParameters("Gold_CheckActiveSchemeTime", objParameter);
    }


    public DataTable GetMessageTemplateDatabyID(int ID)
    {

        SqlParameter[] objParameter = new SqlParameter[1];

        objParameter[0] = new SqlParameter("@ID", SqlDbType.Int);
        objParameter[0].Value = ID;

        return _goldDataAccess.ReturnDataTableWithParameters("Gold_MessageTemplateLocalizedForMessagetByTemplateId", objParameter);
    }

    public DataTable GetEmailMappingByHeader(int TemplateId)
    {

        SqlParameter[] objParameter = new SqlParameter[1];

        objParameter[0] = new SqlParameter("@localTemplateId", SqlDbType.Int);
        objParameter[0].Value = TemplateId;

        return _goldDataAccess.ReturnDataTableWithParameters("Gold_EmailMappingSelectByTemplate", objParameter);
    }
    public bool AddActiveScheme(int TemplateID)
    {
        bool result = false;
        SqlParameter[] objParameter = new SqlParameter[1];

        objParameter[0] = new SqlParameter("@TemplateId", SqlDbType.Int);
        objParameter[0].Value = TemplateID;

       

        var dt = _goldDataAccess.ReturnDataTableWithParameters("Gold_AddActiveScheme", objParameter);
        if (dt.Rows.Count > 0)
        {
            result = true;
        }
        return result;

    }



    public bool AddEmailTemplates(int MessageTemplateID,string  BCCEmailAddresses,string Subject, string Body,byte IsActive,int createuid,int logno, string Tokan,string Time,string Date,string url,string Signature,int interval)
    {
        bool result = false;
        SqlParameter[] objParameter = new SqlParameter[13];

        objParameter[0] = new SqlParameter("@headid", SqlDbType.Int);
        objParameter[0].Value = MessageTemplateID;

        objParameter[1] = new SqlParameter("@bccaddress", SqlDbType.NVarChar);
        objParameter[1].Value = BCCEmailAddresses;

        objParameter[2] = new SqlParameter("@subject", SqlDbType.NVarChar);
        objParameter[2].Value = Subject;

        objParameter[3] = new SqlParameter("@body", SqlDbType.NVarChar);
        objParameter[3].Value = Body;

        objParameter[4] = new SqlParameter("@isactive", SqlDbType.Bit);
        objParameter[4].Value = IsActive;

        objParameter[5] = new SqlParameter("@createuid", SqlDbType.Int);
        objParameter[5].Value = createuid;

        objParameter[6] = new SqlParameter("@logon", SqlDbType.Int);
        objParameter[6].Value = logno;

        objParameter[7] = new SqlParameter("@UsedTokan", SqlDbType.NVarChar);
        objParameter[7].Value = Tokan;

        objParameter[8] = new SqlParameter("@EmailTime", SqlDbType.NVarChar);
        objParameter[8].Value = Time;

        objParameter[9] = new SqlParameter("@EmailLastDate", SqlDbType.NVarChar);
        objParameter[9].Value = Date;

        objParameter[10] = new SqlParameter("@AttachmentUrl", SqlDbType.NVarChar);
        objParameter[10].Value = url;

        objParameter[11] = new SqlParameter("@Signature", SqlDbType.NVarChar);
        objParameter[11].Value = Signature;

        objParameter[12] = new SqlParameter("@Interval", SqlDbType.Int);
        objParameter[12].Value = interval;

        var dt = _goldDataAccess.ReturnDataTableWithParameters("Gold_MessageTemplateLocalizedadd", objParameter);
        if (dt.Rows.Count > 0)
        {
            result = true;
        }
        return result;
    }

    public bool AddMessageTemplates(int MessageTemplateID,  string Body, byte IsActive, int createuid, int logno, string Tokan, string Time, string Date,int interval)
    {
        bool result = false;
        SqlParameter[] objParameter = new SqlParameter[9];

        objParameter[0] = new SqlParameter("@headid", SqlDbType.Int);
        objParameter[0].Value = MessageTemplateID;


        objParameter[1] = new SqlParameter("@body", SqlDbType.NVarChar);
        objParameter[1].Value = Body;

        objParameter[2] = new SqlParameter("@isactive", SqlDbType.Bit);
        objParameter[2].Value = IsActive;

        objParameter[3] = new SqlParameter("@createuid", SqlDbType.Int);
        objParameter[3].Value = createuid;

        objParameter[4] = new SqlParameter("@logon", SqlDbType.Int);
        objParameter[4].Value = logno;

        objParameter[5] = new SqlParameter("@UsedTokan", SqlDbType.NVarChar);
        objParameter[5].Value = Tokan;

        objParameter[6] = new SqlParameter("@MessageTime", SqlDbType.NVarChar);
        objParameter[6].Value = Time;

        objParameter[7] = new SqlParameter("@LastMessageDate", SqlDbType.NVarChar);
        objParameter[7].Value = Date;

        objParameter[8] = new SqlParameter("@Interval", SqlDbType.Int);
        objParameter[8].Value = interval;


        var dt = _goldDataAccess.ReturnDataTableWithParameters("Gold_MessageTemplateLocalizedForMessageadd", objParameter);
        if (dt.Rows.Count > 0)
        {
            result = true;
        }
        return result;
    }


    public DataTable GetMeassageTemplateByHeaderId(int ID)
    {

        SqlParameter[] objParameter = new SqlParameter[1];

        objParameter[0] = new SqlParameter("@ID", SqlDbType.Int);
        objParameter[0].Value = ID;

        return _goldDataAccess.ReturnDataTableWithParameters("Gold_MessageTemplateLocalizedForMessageSelect", objParameter);
    }

    public DataTable GetMeassageById(int ID)
    {

        SqlParameter[] objParameter = new SqlParameter[1];

        objParameter[0] = new SqlParameter("@ID", SqlDbType.Int);
        objParameter[0].Value = ID;

        return _goldDataAccess.ReturnDataTableWithParameters("Gold_MessageTemplateLocalizedForMessageSelectByID", objParameter);
    }

    public bool EditMessageTemplates(int HeaderId, string Body, byte Active, int logno, int UserName, int ID, string Tokan, string Time, string Date,int interval)
    {
        bool result = false;
        SqlParameter[] objParameter = new SqlParameter[10];

        objParameter[0] = new SqlParameter("@HeaderId", SqlDbType.Int);
        objParameter[0].Value = HeaderId;    

        objParameter[1] = new SqlParameter("@body", SqlDbType.NVarChar);
        objParameter[1].Value = Body;

        objParameter[2] = new SqlParameter("@isactive", SqlDbType.Bit);
        objParameter[2].Value = Active;

        objParameter[3] = new SqlParameter("@logon", SqlDbType.Int);
        objParameter[3].Value = logno;

        objParameter[4] = new SqlParameter("@lmodifyuid", SqlDbType.Int);
        objParameter[4].Value = UserName;

        objParameter[5] = new SqlParameter("@messageid", SqlDbType.Int);
        objParameter[5].Value = ID;

        objParameter[6] = new SqlParameter("@UsedTokan", SqlDbType.NVarChar);
        objParameter[6].Value = Tokan;

        objParameter[7] = new SqlParameter("@EmailTime", SqlDbType.NVarChar);
        objParameter[7].Value = Time;

        objParameter[8] = new SqlParameter("@EmailLastDate", SqlDbType.NVarChar);
        objParameter[8].Value = Date;

        objParameter[9] = new SqlParameter("@Interval", SqlDbType.Int);
        objParameter[9].Value = interval;


        //  var dt = _goldDataAccess.ReturnDataTableWithParameters("Gold_MessageTemplateLocalizedEdit", objParameter);
        return bool.TryParse(_goldDataAccess.ExecuteNonQueryWithParameters("Gold_MessageTemplateLocalizedForMessageEdit", objParameter).ToString(), out result);
    }

    public bool AddEmailQueue(int Priority, string To, string ToName, string Cc, string Bcc, string Subject, string Body, string SentOn,string ExpiryDate, int UserId, int Logno, int TemplateId,string AtachmentUrl,string Perpose,byte SendNow)
    {
        bool result = false;
        SqlParameter[] objParameter = new SqlParameter[15];

        objParameter[0] = new SqlParameter("@Priority", SqlDbType.Int);
        objParameter[0].Value = Priority;


        objParameter[1] = new SqlParameter("@To", SqlDbType.NVarChar);
        objParameter[1].Value = To;

        objParameter[2] = new SqlParameter("@ToName", SqlDbType.NVarChar);
        objParameter[2].Value = ToName;

        objParameter[3] = new SqlParameter("@Cc", SqlDbType.NVarChar);
        objParameter[3].Value = Cc;

        objParameter[4] = new SqlParameter("@Bcc", SqlDbType.NVarChar);
        objParameter[4].Value = Bcc;

        objParameter[5] = new SqlParameter("@Subject", SqlDbType.NVarChar);
        objParameter[5].Value = Subject;

        objParameter[6] = new SqlParameter("@Body", SqlDbType.NVarChar);
        objParameter[6].Value = Body;

        objParameter[7] = new SqlParameter("@SentOn", SqlDbType.NVarChar);
        objParameter[7].Value = SentOn;

        objParameter[8] = new SqlParameter("@ExpiryDate", SqlDbType.NVarChar);
        objParameter[8].Value = ExpiryDate;

        objParameter[9] = new SqlParameter("@createuid", SqlDbType.Int);
        objParameter[9].Value = UserId;

        objParameter[10] = new SqlParameter("@logno", SqlDbType.Int);
        objParameter[10].Value = Logno;

        objParameter[11] = new SqlParameter("@TemplateId", SqlDbType.Int);
        objParameter[11].Value = TemplateId;

        objParameter[12] = new SqlParameter("@AttachmentUrl", SqlDbType.NVarChar);
        objParameter[12].Value = AtachmentUrl;

        objParameter[13] = new SqlParameter("@Perpose", SqlDbType.NVarChar);
        objParameter[13].Value = Perpose;

        objParameter[14] = new SqlParameter("@SendNow", SqlDbType.Bit);
        objParameter[14].Value = SendNow;


        var dt = _goldDataAccess.ReturnDataTableWithParameters("Gold_QueuedEmailAdd", objParameter);
        if (dt.Rows.Count > 0)
        {
            result = true;
        }
        return result;
    }

    public bool AddMessageQueue(int Priority, string To, string ToName,  string Body, string SentOn, string ExpiryDate, int UserId, int Logno, int TemplateId,string Perpose,byte SendNow)
    {
        bool result = false;
        SqlParameter[] objParameter = new SqlParameter[11];

        objParameter[0] = new SqlParameter("@Priority", SqlDbType.Int);
        objParameter[0].Value = Priority;


        objParameter[1] = new SqlParameter("@To", SqlDbType.NVarChar);
        objParameter[1].Value = To;

        objParameter[2] = new SqlParameter("@ToName", SqlDbType.NVarChar);
        objParameter[2].Value = ToName;

        objParameter[3] = new SqlParameter("@Body", SqlDbType.NVarChar);
        objParameter[3].Value = Body;

        objParameter[4] = new SqlParameter("@SentOn", SqlDbType.NVarChar);
        objParameter[4].Value = SentOn;

        objParameter[5] = new SqlParameter("@ExpiryDate", SqlDbType.NVarChar);
        objParameter[5].Value = ExpiryDate;

        objParameter[6] = new SqlParameter("@createuid", SqlDbType.Int);
        objParameter[6].Value = UserId;

        objParameter[7] = new SqlParameter("@logno", SqlDbType.Int);
        objParameter[7].Value = Logno;

        objParameter[8] = new SqlParameter("@TemplateId", SqlDbType.Int);
        objParameter[8].Value = TemplateId;

        objParameter[9] = new SqlParameter("@Perpose", SqlDbType.NVarChar);
        objParameter[9].Value = Perpose;

        objParameter[10] = new SqlParameter("@SendNow", SqlDbType.Bit);
        objParameter[10].Value = SendNow;


        var dt = _goldDataAccess.ReturnDataTableWithParameters("Gold_QueuedMessageAdd", objParameter);
        if (dt.Rows.Count > 0)
        {
            result = true;
        }
        return result;
    }


    public bool AddMobileNotification(int Priority, int slno, int createid)
    {
        bool result = false;
        SqlParameter[] objParameter = new SqlParameter[3];

        objParameter[0] = new SqlParameter("@Priority", SqlDbType.Int);
        objParameter[0].Value = Priority;

        objParameter[1] = new SqlParameter("@slno", SqlDbType.Int);
        objParameter[1].Value = slno;

        objParameter[2] = new SqlParameter("@createid", SqlDbType.Int);
        objParameter[2].Value = createid;

        var dt = _goldDataAccess.ReturnDataTableWithParameters("MobileNotificationAdd", objParameter);
        if (dt.Rows.Count > 0)
        {
            result = true;
        }
        return result;
    }

    public bool UpdateQueuedEmail()
    {

        bool result = false;
        //_goldDataAccess.ReturnDataTable("Gold_EmailQueueFirstTask");


        var dt = _goldDataAccess.ReturnDataTable("Gold_EmailQueueFirstTask");
        if (dt.Rows.Count > 0)
        {
            result = true;
        }
        return result;

    }

    public bool DeleteAllQueuedEmail()
    {

        bool result = false;     

        var dt = _goldDataAccess.ReturnDataTable("Gold_DeleteAllQueuedEmail");
        if (dt.Rows.Count > 0)
        {
            result = true;
        }
        return result;

    }

    public bool DeleteAllQueuedMessage()
    {

        bool result = false;

        var dt = _goldDataAccess.ReturnDataTable("Gold_DeleteAllQueuedMessage");
        if (dt.Rows.Count > 0)
        {
            result = true;
        }
        return result;

    }


   
    public DataTable GetspfireDetails(string SP)
    {

        SqlParameter[] objParameter = new SqlParameter[1];

        objParameter[0] = new SqlParameter("@spname", SqlDbType.NVarChar);
        objParameter[0].Value = SP;

        return _goldDataAccess.ReturnDataTableWithParameters("getSPFireDetails", objParameter);
    }

    public bool CreateSPFireDetails(string SP)
    {

       
        bool result = false;

        SqlParameter[] objParameter = new SqlParameter[1];

        objParameter[0] = new SqlParameter("@spname", SqlDbType.NVarChar);
        objParameter[0].Value = SP;
        var dt = _goldDataAccess.ReturnDataTableWithParameters("CreateSPFireDetails", objParameter);

        if (dt.Rows.Count > 0)
        {
            result = true;
        }
        return result;

    }

}