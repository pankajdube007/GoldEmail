using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for IEmailTemplateDA
/// </summary>
public interface IEmailTemplateDA
{
    bool AddHeader(string HeaderName, byte Email, byte SMS, int UserName, int Logno);
    DataTable GetAllHeader();
    DataTable CheckHeader(string HeaderName);
    DataTable GetAllEmail();
    DataTable GetEmailAllData(int EmailId);
    DataTable GetHaderById(int ID);
    bool EditHeader(string HeaderName, byte Email, byte SMS, int UserName, int Logno, int ID);
    DataTable GetAllAvailableTokanCreation();
    DataTable CheckAvailableTokanCreationByName(string TokanName);
    bool AddAvailableTokanCreation(string TokanName, string SampleValue, int UserName, int Logno);
    DataTable GetAvailableTokanCreationById(int ID);
    DataTable GetAvailableTokanCreationByString(string ID);
    bool EditAvailableTokanCreation(string TokanName, string SampleValue, int UserName, int Logno, int ID);
    bool AddHeaderTokanMapping(int HeaderId, string TokanId, string TokanNames, int Logno, int UserName);
    DataTable GetAllHeaderAfterSelection();
    DataTable GetAllTokanHeaderMapping();
    DataTable TokanHeaderSelectById(int Id);
      
    DataTable GetAvailableTokanCreationaAfterMapping(string ID);

    bool EditHeaderTokanMapping(int HeaderId, string TokanId, string TokanNames, int Logno, int UserName, int Id);
    bool AddEmailTemplates(int HeaderId, string BCC, string Subject, string Body, byte Active,  int logno, int UserName, string Tokan,string Time, string Date, string url,string Signature, int interval);
    bool EditEmailTemplates(int HeaderId, string BCC, string Subject, string Body, byte Active,  int logno, int UserName, int ID, string Tokan, string Time, string Date, string url, string Signature,int interval);
    DataTable GetAllEmailTemplates();
    DataTable GetHaderTokanMappingByHeaderId(int ID);
    DataTable CheckExistTokanMapping(string TokanName, int ID);
    DataTable CheckExistTokanInTemplates(string TokanName);
    DataTable CheckEmailMapping(int TemplateId, int EmailId);
    bool EditEmailMapping(int TemplateId, int EmailId, int SequenceNo, int UserName, int Logno, int ID);
    bool ADDEmailMapping(int TemplateId, int EmailId, int SequenceNo, int UserName, int Logno);
    DataTable GetEmailMappingByHeader(int TemplateId);
    bool DeleteEmailMapping(int TemplateId, int EmailId);
    DataTable GetAllHeaderForEmail();
    DataTable GetAllHeaderForMessage();
    bool AddMessageTemplates(int MessageTemplateID, string Body, byte IsActive, int createuid, int logno, string Tokan, string Time, string Date,int interval);
    DataTable GetMeassageTemplateByHeaderId(int ID);
  
    bool EditMessageTemplates(int HeaderId, string Body, byte Active, int logno, int UserName, int ID, string Tokan, string Time, string Date,int interval);
    bool AddEmailQueue(int Priority, string To, string ToName, string Cc, string Bcc, string Subject, string Body, string SentOn, string ExpiryDate, int UserId, int Logno, int TemplateId,string AttachmentUrl, string Perpose,byte SendNow);
    DataTable GetEmailTemplateDatabyID(int ID);
    DataTable GetEmailForTimeCheckbyID(int ID);
    DataTable GetActiveSchemeTimeID(int TemplateId);
    DataTable GetMessageTemplateDatabyID(int ID);
    bool AddMessageQueue(int Priority, string To, string ToName, string Body, string SentOn, string ExpiryDate, int UserId, int Logno, int TemplateId, string Perpose,byte SendNow);
    bool AddMobileNotification(int Priority, int slno, int createid);
    bool UpdateQueuedEmail();
    DataTable GetAllMessageTemplates();
    DataTable GetMeassageById(int ID);
    DataTable GetQueuedEmail();
    bool DeleteQueuedEmailByID(int ID);
    DataTable GetQueuedMessage();
    bool DeleteQueuedMessageByID(int ID);
    DataTable GetSendEmail(string fromdate, string todate);
    DataTable GetSendEmailByID(int ID);
    DataTable GetSendMessage(string fromdate, string todate);
    DataTable GetSendMessageByID(int ID);
    bool AddActiveScheme(int TemplateID);
    DataTable GetQueuedEmailHistoryforExandag(string EmailID,int TempId);
    bool DeleteAllQueuedEmail();
    bool DeleteAllQueuedMessage();
 

    DataTable GetspfireDetails(string SP);
    bool CreateSPFireDetails(string SP);


}