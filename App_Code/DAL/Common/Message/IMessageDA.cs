using System.Data;

/// <summary>
/// Summary description for IMessageDA
/// </summary>
public interface IMessageDA
{
    DataTable GetEmailAccountDataList();
    DataTable GetEmailAccountDataByID(int recid);
    DataTable GetEmailAccountDataByName(string name);
    DataTable GetMessageTemplateDataList();
    DataTable GetMessageTemplateDataByID(int recid);
    DataTable GetMessageTemplateDataByName(string name);
    DataTable GetLocalizedMessageTemplateDataByID(int recid);
    DataTable GetLocalizedMessageTemplateDataByName(string name);
}