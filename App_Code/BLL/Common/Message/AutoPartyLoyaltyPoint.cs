using System;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

/// <summary>
/// Summary description for AutoPartyLoyaltyPoint
/// </summary>
namespace Goldmedal.Emails.App_Code.BLL.Common.Message
{
    public class AutoPartyLoyaltyPoint : ITask
    {
        private int _maxTries = 5;
        private readonly IMessageService _messageService;
        private readonly GoldLogging _gLog;
        private readonly IEmailTemplateDA da;
        private readonly IGoldMedia _goldMedia;
        private MessageService ms = new MessageService();

        private SqlDataReader rdr = null;

        public AutoPartyLoyaltyPoint()
        {
            _messageService = new MessageService();
            _gLog = new GoldLogging();
            da = new EmailTemplateDA();
            _goldMedia = new GoldMedia();
        }

        public void Execute(XmlNode node)
        {
            int TemplateId = 3017;

            XmlAttribute attribute1 = node.Attributes["maxTries"];
            if (attribute1 != null && !String.IsNullOrEmpty(attribute1.Value))
            {
                this._maxTries = int.Parse(attribute1.Value);
            }
            DealerSMS(TemplateId);
        }

        protected void DealerSMS(int DealerTemplateId)
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
                    DataTable dt = _messageService.return_dt("exec smsloyaltypoint");

                    if (dt.Rows.Count > 0)
                    {
                        //   ExcuativeMail(dt);
                        PartySMS(dt, DealerTemplateId);
                    }
                }
            }
        }

        protected void PartySMS(DataTable dt, int DealerTemplateId)
        {
            string AttachmentName = string.Empty;
            string FileName = string.Empty;
            da.AddActiveScheme(DealerTemplateId);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (ms.ValidateMobileNo(dt.Rows[i]["mobile"].ToString()))
                {
                    LoyaltyPoint mailsms = new LoyaltyPoint();
                    mailsms.PartyName = dt.Rows[i]["name"].ToString();
                    mailsms.LoyaltyPoints = dt.Rows[i]["point"].ToString();
                    mailsms.FYear = dt.Rows[i]["fyear"].ToString();
                    mailsms.AsonDate = dt.Rows[i]["pointdt"].ToString();

                    SendSMS(DealerTemplateId, dt.Rows[i]["mobile"].ToString(), dt.Rows[i]["name"].ToString(), mailsms);
                }
            }
        }

        private void SendSMS(int TemplateId, string MobileNo, string Name, LoyaltyPoint MailSMS)
        {
            string Body = string.Empty;

            DataTable dt = new DataTable();
            dt = da.GetMeassageById(TemplateId);

            if (dt.Rows.Count > 0)
            {
                Body = ms.ReplaceTemplateLoyaltyPoint(dt.Rows[0]["Body"].ToString(), dt.Rows[0]["Body"].ToString(), MailSMS);

                da.AddMessageQueue(0, MobileNo, Name, Body, dt.Rows[0]["MessageTime"].ToString(), dt.Rows[0]["LastMessageDate"].ToString(), 0, 0, TemplateId, "Loyalty Point", 1);
            }
        }
    }
}