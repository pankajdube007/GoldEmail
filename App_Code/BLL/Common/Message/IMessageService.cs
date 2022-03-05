using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;

/// <summary>
/// Summary description for IMessageService
/// </summary>
public partial interface IMessageService
{
    /// <summary>
    /// Gets a message template by template identifier
    /// </summary>
    /// <param name="messageTemplateId">Message template identifier</param>
    /// <returns>Message template</returns>
    MessageTemplate GetMessageTemplateById(int messageTemplateId);

    /// <summary>
    /// Gets all message templates
    /// </summary>
    /// <returns>Message template collection</returns>
    List<MessageTemplate> GetAllMessageTemplates();

    /// <summary>
    /// Gets a localized message template by identifier
    /// </summary>
    /// <param name="localizedMessageTemplateId">Localized message template identifier</param>
    /// <returns>Localized message template</returns>
    LocalizedMessageTemplate GetLocalizedMessageTemplateById(int localizedMessageTemplateId);

    /// <summary>
    /// Deletes a localized message template
    /// </summary>
    /// <param name="localizedMessageTemplateId">Message template identifier</param>
    void DeleteLocalizedMessageTemplate(int localizedMessageTemplateId);

    /// <summary>
    /// Gets all localized message templates
    /// </summary>
    /// <param name="messageTemplateName">Message template name</param>
    /// <returns>Localized message template collection</returns>
    List<LocalizedMessageTemplate> GetAllLocalizedMessageTemplates(string messageTemplateName);

    /// <summary>
    /// Inserts a localized message template
    /// </summary>
    /// <param name="localizedMessageTemplate">Localized message template</param>
    void InsertLocalizedMessageTemplate(LocalizedMessageTemplate localizedMessageTemplate);

    /// <summary>
    /// Updates the localized message template
    /// </summary>
    /// <param name="localizedMessageTemplate">Localized message template</param>
    void UpdateLocalizedMessageTemplate(LocalizedMessageTemplate localizedMessageTemplate);

    /// <summary>
    /// Gets a queued email by identifier
    /// </summary>
    /// <param name="queuedEmailId">Email item identifier</param>
    /// <returns>Email item</returns>
    QueuedEmail GetQueuedEmailById(int queuedEmailId);

    /// <summary>
    /// Deletes a queued email
    /// </summary>
    /// <param name="queuedEmailId">Email item identifier</param>
    void DeleteQueuedEmail(int queuedEmailId);

    /// <summary>
    /// Gets all queued emails
    /// </summary>
    /// <param name="queuedEmailCount">Email item count. 0 if you want to get all items</param>
    /// <param name="loadNotSentItemsOnly">A value indicating whether to load only not sent emails</param>
    /// <param name="maxSendTries">Maximum send tries</param>
    /// <returns>Email item collection</returns>
    List<QueuedEmail> GetAllQueuedEmails(int queuedEmailCount,
        bool loadNotSentItemsOnly, int maxSendTries);

    /// <summary>
    /// Gets all queued emails
    /// </summary>
    /// <param name="fromEmail">From Email</param>
    /// <param name="toEmail">To Email</param>
    /// <param name="startTime">The start time</param>
    /// <param name="endTime">The end time</param>
    /// <param name="queuedEmailCount">Email item count. 0 if you want to get all items</param>
    /// <param name="loadNotSentItemsOnly">A value indicating whether to load only not sent emails</param>
    /// <param name="maxSendTries">Maximum send tries</param>
    /// <returns>Email item collection</returns>
    List<QueuedEmail> GetAllQueuedEmails(string fromEmail,
        string toEmail, DateTime? startTime, DateTime? endTime,
        int queuedEmailCount, bool loadNotSentItemsOnly, int maxSendTries);

    /// <summary>
    /// Inserts a queued email
    /// </summary>
    /// <param name="priority">The priority</param>
    /// <param name="from">From</param>
    /// <param name="to">To</param>
    /// <param name="cc">CC</param>
    /// <param name="bcc">BCC</param>
    /// <param name="subject">Subject</param>
    /// <param name="body">Body</param>
    /// <param name="createdOn">The date and time of item creation</param>
    /// <param name="sendTries">The send tries</param>
    /// <param name="sentOn">The sent date and time. Null if email is not sent yet</param>
    /// <param name="emailAccountId">Email account identifer</param>
    /// <returns>Queued email</returns>
    QueuedEmail InsertQueuedEmail(int priority, MailAddress from,
        MailAddress to, string cc, string bcc,
        string subject, string body, DateTime createdOn, int sendTries,
        DateTime? sentOn, int emailAccountId);

    /// <summary>
    /// Inserts a queued email
    /// </summary>
    /// <param name="priority">The priority</param>
    /// <param name="from">From</param>
    /// <param name="fromName">From name</param>
    /// <param name="to">To</param>
    /// <param name="toName">To name</param>
    /// <param name="cc">Cc</param>
    /// <param name="bcc">Bcc</param>
    /// <param name="subject">Subject</param>
    /// <param name="body">Body</param>
    /// <param name="createdOn">The date and time of item creation</param>
    /// <param name="sendTries">The send tries</param>
    /// <param name="sentOn">The sent date and time. Null if email is not sent yet</param>
    /// <param name="emailAccountId">Email account identifer</param>
    /// <returns>Queued email</returns>
    QueuedEmail InsertQueuedEmail(int priority, string from,
        string fromName, string to, string toName, string cc, string bcc,
        string subject, string body, DateTime createdOn, int sendTries,
        DateTime? sentOn, int emailAccountId);

    /// <summary>
    /// Updates a queued email
    /// </summary>
    /// <param name="queuedEmail">Queued email</param>
    void UpdateQueuedEmail(QueuedEmail queuedEmail);

    /// <summary>
    /// Gets a email account by identifier
    /// </summary>
    /// <param name="emailAccountId">The email account identifier</param>
    /// <returns>Email account</returns>
    EmailAccount GetEmailAccountById(int emailAccountId);

    /// <summary>
    /// Deletes the email account
    /// </summary>
    /// <param name="emailAccountId">The email account identifier</param>
    void DeleteEmailAccount(int emailAccountId);

    /// <summary>
    /// Inserts an email account
    /// </summary>
    /// <param name="emailAccount">Email account</param>
    void InsertEmailAccount(EmailAccount emailAccount);

    /// <summary>
    /// Updates an email account
    /// </summary>
    /// <param name="emailAccount">Email account</param>
    void UpdateEmailAccount(EmailAccount emailAccount);

    /// <summary>
    /// Gets all email accounts
    /// </summary>
    /// <returns>Email accounts</returns>
    List<EmailAccount> GetAllEmailAccounts();

    /// <summary>
    /// Sends a private message notification
    /// </summary>
    /// <param name="privateMessage">Private message</param>
    /// <param name="languageId">Message language identifier</param>
    /// <returns>Queued email identifier</returns>
    int SendPrivateMessageNotification(PrivateMessage privateMessage);

    /// <summary>
    /// Gets list of allowed (supported) message tokens
    /// </summary>
    /// <returns></returns>
    string[] GetListOfAllowedTokens();

    /// <summary>
    /// Sends an email
    /// </summary>
    /// <param name="subject">Subject</param>
    /// <param name="body">Body</param>
    /// <param name="from">From</param>
    /// <param name="to">To</param>
    /// <param name="emailAccount">Email account to use</param>
    void SendEmail(string subject, string body, string from, string to, EmailAccount emailAccount);

    /// <summary>
    /// Sends an email
    /// </summary>
    /// <param name="subject">Subject</param>
    /// <param name="body">Body</param>
    /// <param name="from">From</param>
    /// <param name="to">To</param>
    /// <param name="emailAccount">Email account to use</param>
    void SendEmail(string subject, string body, MailAddress from, MailAddress to, EmailAccount emailAccount);

    /// <summary>
    /// Sends an email
    /// </summary>
    /// <param name="subject">Subject</param>
    /// <param name="body">Body</param>
    /// <param name="from">From</param>
    /// <param name="to">To</param>
    /// <param name="bcc">BCC</param>
    /// <param name="cc">CC</param>
    /// <param name="emailAccount">Email account to use</param>
    void SendEmail(string subject, string body, MailAddress from, MailAddress to, List<string> bcc, List<string> cc, EmailAccount emailAccount);

    /// <summary>
    /// Gets or sets a primary store currency
    /// </summary>
    EmailAccount DefaultEmailAccount { get; set; }

    bool UpdateQueuedEmail();

    DataTable GetQueuedActiveData();

    bool UpdateQueuedEmailAfterSend(int EmailId, int Senttries);

    bool DeleteSendQueuedEmail();

    DataTable GetQueuedMessageActiveData();

    DataTable GetQueuedMobileNotificationData(string Sendby);

    DataTable GetQueuedMobileNotificationDataEx(string Sendby);

    bool UpdateQueuedMessageAfterSend(int Id);

    bool UpdateQueuedNotificationAfterSend(int Id,string Sendby);

    DataTable GetEmailAccountForTestMail();

    DataTable getdata(string procedure);

    DataTable getdatabyuser(string procedure, int ID, string parameter);

    bool ValidateEmailId(string Email);

    bool ValidateMobileNo(string Email);

    DataTable return_dt(string s1);
    SqlDataReader return_dr(string s1);

    bool UpdateQueuedEmailTries(int Id, int tries);
}