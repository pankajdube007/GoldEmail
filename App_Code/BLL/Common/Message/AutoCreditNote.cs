using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Xml;

/// <summary>
/// Summary description for AutoCreditNote
/// </summary>
///
namespace Goldmedal.Emails.App_Code.BLL.Common.Message
{
    public class AutoCreditNote : ITask
    {
        private int _maxTries = 5;
        private readonly IMessageService _messageService;
        private readonly GoldLogging _gLog;
        private readonly IEmailTemplateDA da;
        private readonly IGoldMedia _goldMedia;
        private MessageService ms = new MessageService();

        private SqlDataReader rdr = null;

        public AutoCreditNote()
        {
            _messageService = new MessageService();
            _gLog = new GoldLogging();
            da = new EmailTemplateDA();
            _goldMedia = new GoldMedia();
        }

        public void Execute(XmlNode node)
        {
            int DealerTemplateId = 3025;
            int ExTemplateId = 3024;

            XmlAttribute attribute1 = node.Attributes["maxTries"];
            if (attribute1 != null && !String.IsNullOrEmpty(attribute1.Value))
            {
                this._maxTries = int.Parse(attribute1.Value);
            }
            DealerMail(DealerTemplateId);
            PartyAgentMail(ExTemplateId);
        }

        protected void DealerMail(int DealerTemplateId)
        {
            DataTable dttemplate = da.GetEmailForTimeCheckbyID(DealerTemplateId);
            if (dttemplate.Rows.Count > 0)
            {
                DateTime lastsend = DateTime.Now.Date;
                int flag = 0;
                DataTable dtActiveTime = da.GetActiveSchemeTimeID(DealerTemplateId);
                if (dtActiveTime.Rows.Count > 0)
                {
                    lastsend = Convert.ToDateTime(dtActiveTime.Rows[0]["lastSend"]);
                    flag = 0;
                }
                else
                {
                    lastsend = DateTime.Now.Date;
                    flag = 1;
                }
                int days = (DateTime.Now.Date - lastsend).Days;
                int interval = Convert.ToInt32(dttemplate.Rows[0]["interval"]);

                if (days >= interval || flag == 1)
                {
                    DataTable dt = _messageService.return_dt("exec CreditNoteMasterbyDate '" + lastsend + "','" + DateTime.Now.Date + "'");

                    if (dt.Rows.Count > 0)
                    {
                        //   ExcuativeMail(dt);
                        PartyMail(dt, DealerTemplateId, dttemplate);
                    }
                }
            }
        }

        protected void PartyAgentMail(int ExTemplateId)
        {
            DataTable dttemplate = da.GetEmailForTimeCheckbyID(ExTemplateId);
            if (dttemplate.Rows.Count > 0)
            {
                DateTime lastsend = DateTime.Now.Date;
                int flag = 0;
                DataTable dtActiveTime = da.GetActiveSchemeTimeID(ExTemplateId);
                if (dtActiveTime.Rows.Count > 0)
                {
                    lastsend = Convert.ToDateTime(dtActiveTime.Rows[0]["lastSend"]);
                    flag = 0;
                }
                else
                {
                    lastsend = DateTime.Now.Date;
                    flag = 1;
                }
                int days = (DateTime.Now.Date - lastsend).Days;
                int interval = Convert.ToInt32(dttemplate.Rows[0]["interval"]);

                if (days >= interval || flag == 1)
                {
                    DataTable dt = _messageService.return_dt("exec CreditNoteMasterbyDate '" + lastsend + "','" + DateTime.Now.Date + "'");

                    if (dt.Rows.Count > 0)
                    {
                        AgentMail(dt, ExTemplateId, dttemplate);
                    }
                }
            }
        }

        protected void PartyMail(DataTable dt, int DealerTemplateId, DataTable dttemplate)
        {
            string AttachmentName = string.Empty;
            string FileName = string.Empty;
            da.AddActiveScheme(DealerTemplateId);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataTable dtparty = _messageService.return_dt("exec GetPartyCollectionSelect " + dt.Rows[i]["partyid"].ToString() + "," + dt.Rows[i]["partytypeid"].ToString());
                if (dtparty.Rows.Count > 0)
                {
                    if (_messageService.ValidateEmailId(dtparty.Rows[0]["emailid"].ToString()) == true)
                    {
                        DataTable dtcheckhistory = da.GetQueuedEmailHistoryforExandag(dtparty.Rows[0]["emailid"].ToString(), DealerTemplateId);
                        if (dtcheckhistory.Rows.Count <= 0)
                        {
                            CreditNote MailSMS = new CreditNote();
                            MailSMS.PartyName = dtparty.Rows[0]["partyname"].ToString();
                            MailSMS.FullAddress = dtparty.Rows[0]["fulladdress"].ToString();
                            MailSMS.AgentName = "";

                            byte[] report = generatePDF(Convert.ToInt32(dt.Rows[0]["slno"]));
                            Stream stream = new MemoryStream(report);
                            FileName = string.Format(@"{0}", dtparty.Rows[0]["cin"].ToString() + dtparty.Rows[0]["printnm"].ToString());
                            string uniquefoldernm = dtparty.Rows[0]["cin"].ToString() + "CreditNote";

                            var retStr = _goldMedia.GoldMediaUpload(FileName, DealerTemplateId.ToString() + "/" + uniquefoldernm, ".pdf", stream, "application/pdf", false, false, true);

                            SendMail(DealerTemplateId, dtparty.Rows[0]["emailid"].ToString(), dtparty.Rows[0]["partyname"].ToString(), FileName + ".pdf", uniquefoldernm, MailSMS);
                        }
                    }
                }
            }
        }

        protected void AgentMail(DataTable dt, int ExTemplateId, DataTable dttemplate)
        {
            string AttachmentName = string.Empty;
            string FileName = string.Empty;
            da.AddActiveScheme(ExTemplateId);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataTable dtparty = new DataTable();
                dtparty = _messageService.return_dt("exec GetPartyCollectionSelect " + dt.Rows[i]["partyid"] + "," + dt.Rows[i]["partytypeid"]);
                if (dtparty.Rows.Count > 0)
                {
                    if (Convert.ToInt32(dtparty.Rows[0]["agentid"]) > 0)
                    {
                        DataTable dtAgent = _messageService.return_dt("exec agentshow " + dtparty.Rows[0]["agentid"].ToString());
                        if (dtAgent.Rows.Count > 0)
                        {
                            if (_messageService.ValidateEmailId(dtAgent.Rows[0]["email"].ToString()) == true)
                            {
                                DataTable dtcheckhistory = da.GetQueuedEmailHistoryforExandag(dtAgent.Rows[0]["email"].ToString(), ExTemplateId);
                                if (dtcheckhistory.Rows.Count <= 0)
                                {
                                    CreditNote MailSMS = new CreditNote();
                                    MailSMS.PartyName = "";
                                    MailSMS.FullAddress = "";
                                    MailSMS.AgentName = dtAgent.Rows[0]["agentnm"].ToString();

                                    byte[] report = generatePDF(Convert.ToInt32(dt.Rows[0]["slno"]));

                                    FileName = string.Format(@"{0}", dtparty.Rows[0]["cin"].ToString() + dtparty.Rows[0]["printnm"].ToString());
                                    Stream stream = new MemoryStream(report);
                                    string uniquefoldernm = dtparty.Rows[0]["cin"].ToString() + "CreditNote";

                                    var retStr = _goldMedia.GoldMediaUpload(FileName, ExTemplateId.ToString() + "/" + uniquefoldernm, ".pdf", stream, "application/pdf", false, false, true);

                                    SendMail(ExTemplateId, dtAgent.Rows[0]["email"].ToString(), dtAgent.Rows[0]["agentnm"].ToString(), FileName + ".pdf", uniquefoldernm, MailSMS);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void SendMail(int TemplateId, string Email, string Name, string Attachment, string uniquefoldernm, CreditNote MailSMS)
        {
            DataTable dtcheck = da.GetQueuedEmailHistoryforExandag(Email, TemplateId);
            if (dtcheck.Rows.Count <= 0)
            {
                string Attach = string.Empty;
                string Subject = string.Empty;
                string Body = string.Empty;
                if (Attachment != "")
                {
                    Attach = ProperAttachmentUrl(Attachment, TemplateId, uniquefoldernm);
                }
                DataTable dt = new DataTable();
                dt = da.GetEmailTemplateDatabyID(TemplateId);

                if (dt.Rows.Count > 0)
                {
                    Subject = HttpUtility.HtmlDecode(ms.ReplaceTemplateCreditNote(dt.Rows[0]["Subject"].ToString(), dt.Rows[0]["Subject"].ToString(), MailSMS));

                    Body = ms.ReplaceTemplateCreditNote(dt.Rows[0]["Body"].ToString(), dt.Rows[0]["Body"].ToString(), MailSMS);

                    Body = Body + "&nbsp;&nbsp;" + dt.Rows[0]["Signature"].ToString();

                    da.AddEmailQueue(0, Email, Name, "", "", Subject, Body, dt.Rows[0]["EmailTime"].ToString(), dt.Rows[0]["EmailLastDate"].ToString(), 0, 0, TemplateId, Attach, "Credit-Note", 1);
                }
            }
        }

        public string ProperAttachmentUrl(string Attachment, int MailId, string uniquefoldernm)

        {
            string AttachmentUrl = string.Empty;
            foreach (string str1 in Attachment.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                AttachmentUrl = AttachmentUrl + MailId + "\\" + uniquefoldernm + "\\" + str1 + ",";
            }
            return AttachmentUrl;
        }

        protected byte[] generatePDF(int PartyCin)
        {
            string url = WebConfigurationManager.AppSettings["ErpAutoCreditNote"].ToString() + PartyCin;

            var webClient = new WebClient();
            byte[] report = webClient.DownloadData(new Uri(url));
            return report;
        }
    }
}