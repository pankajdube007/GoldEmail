using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for IEmailDA
/// </summary>
public interface IEmailDA
{
    DataTable GetEmailData();
    bool SaveEmailData(string EmailId, string DisplayName, string Host, int Port, string UserName, string Password, byte EnableSSL, byte UseDefaultCredentials,byte DefaultEmail,int EmailLimit, int logno, int Createuid);
    bool CheckEmailData(string EmailID);
    DataTable GetEmailById(int EmailID);
    bool UpdateEmailData(string EmailId, string DisplayName, string Host, int Port, string UserName, string Password, byte EnableSSL, byte UseDefaultCredentials, byte DefaultEmail, int EmailLimit, int logno, int Createuid,int Id);
    DataTable EmailMappingCheckByEmailId(int EmailId);
}