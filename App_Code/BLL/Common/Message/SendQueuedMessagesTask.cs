using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Mail;
using System.Xml;

namespace Goldmedal.Emails.App_Code.BLL.Common.Message
{
    /// <summary>
    /// Summary description for SendQueuedMessagesTask
    /// </summary>
    public partial class SendQueuedMessagesTask : ITask
    {
        private int _maxTries = 5;
        private readonly IMessageService _messageService;
        private readonly GoldLogging _gLog;
        private readonly IEmailTemplateDA da = null;

        public SendQueuedMessagesTask()
        {
            _messageService = new MessageService();
            _gLog = new GoldLogging();
            da = new EmailTemplateDA();
            //userManagement = new UserManagement();
            //UserLogID = userManagement.GetUserInfoInt(UserEnum.UserInfoEnum.UserLogID);
            //LogNo = userManagement.GetUserInfoLong(UserEnum.UserInfoEnum.LogNo);
        }

        /// <summary>
        /// Executes a task
        /// </summary>
        /// <param name="node">Xml node that represents a task description</param>
        public void Execute(XmlNode node)
        {
            XmlAttribute attribute1 = node.Attributes["maxTries"];
            if (attribute1 != null && !String.IsNullOrEmpty(attribute1.Value))
            {
                this._maxTries = int.Parse(attribute1.Value);
            }

            //  var queuedEmails = _messageService.GetAllQueuedEmails(10000, true, _maxTries);
            //  foreach (QueuedEmail queuedEmail in queuedEmails)
            //{
            //    List<string> bcc = new List<string>();
            //    foreach (string str1 in queuedEmail.Bcc.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
            //    {
            //        bcc.Add(str1);
            //    }
            //    List<string> cc = new List<string>();
            //    foreach (string str1 in queuedEmail.CC.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
            //    {
            //        cc.Add(str1);
            //    }

            //    try
            //    {
            //        _messageService.SendEmail(queuedEmail.Subject, queuedEmail.Body,
            //           new MailAddress(queuedEmail.From, queuedEmail.FromName),
            //           new MailAddress(queuedEmail.To, queuedEmail.ToName), bcc, cc, queuedEmail.EmailAccount);

            //        queuedEmail.SendTries = queuedEmail.SendTries + 1;
            //        queuedEmail.SentOn = DateTime.UtcNow;
            //        _messageService.UpdateQueuedEmail(queuedEmail);
            //    }
            //    catch (Exception exc)
            //    {
            //        queuedEmail.SendTries = queuedEmail.SendTries + 1;
            //        _messageService.UpdateQueuedEmail(queuedEmail);

            //        _gLog.SendErrorToText(exc);
            //    }
            //}

            DataTable dt = _messageService.GetQueuedActiveData();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (_messageService.ValidateEmailId(dt.Rows[i]["To"].ToString()) == true)
                    {
                        int tries = 0;
                        List<string> bcc = new List<string>();
                        foreach (string str1 in dt.Rows[i]["Bcc"].ToString().Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            bcc.Add(str1);
                        }

                        List<string> cc = new List<string>();
                        foreach (string str1 in dt.Rows[i]["Cc"].ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            cc.Add(str1);
                        }
                        //List<string> Attach = new List<string>();
                        //foreach (string str1 in dt.Rows[i]["AttachmentUrl"].ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                        //{
                        //    Attach.Add(str1);
                        //}

                        EmailAccount ea = new EmailAccount();
                        ea = _messageService.GetEmailAccountById(Convert.ToInt32(dt.Rows[i]["EmailAccountId"].ToString()));
                        ea.Attchment = dt.Rows[i]["AttachmentUrl"].ToString();

                        ea.TemplateId = dt.Rows[i]["MessageTemplateLocalizedID"].ToString();
                        //   ea.TemplateId = ProperAttachmentUrl(dt.Rows[i]["MessageTemplateLocalizedID"].ToString(), Convert.ToInt32(ea.TemplateId));
                        try
                        {
                            //  ea.EmailAccountId =Convert.ToInt32(row["EmailAccountId"].ToString());
                            _messageService.SendEmail(dt.Rows[i]["Subject"].ToString(), dt.Rows[i]["Body"].ToString(),
                               new MailAddress(ea.Email, ea.DisplayName),
                               new MailAddress(dt.Rows[i]["To"].ToString(), dt.Rows[i]["ToName"].ToString()), bcc, cc, ea);

                            tries = tries + 1;
                            // DateTime dta = DateTime.UtcNow;
                            _messageService.UpdateQueuedEmailAfterSend(Convert.ToInt32(dt.Rows[i]["QueuedEmailId"].ToString()), tries);
                        }
                        catch (Exception exc)
                        {
                            int queuedEmail = 0;
                            queuedEmail = Convert.ToInt32(dt.Rows[i]["SendTries"]) + 1;
                            _messageService.UpdateQueuedEmailTries(Convert.ToInt32(Convert.ToInt32(dt.Rows[i]["QueuedEmailId"])), queuedEmail);

                            _gLog.SendErrorToText(exc);
                        }
                        //   ((IDisposable)ea).Dispose();
                    }
                    else
                    {
                        da.DeleteQueuedEmailByID(Convert.ToInt32(dt.Rows[i]["QueuedEmailID"]));
                    }
                }
            }
        }

        public string ProperAttachmentUrl(string Attachment, int MailId)

        {
            string AttachmentUrl = string.Empty;
            foreach (string str1 in Attachment.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                AttachmentUrl = AttachmentUrl + "Upload\\ErpAttachment\\" + MailId + "\\" + str1 + ",";
            }
            return AttachmentUrl;
        }
    }
}