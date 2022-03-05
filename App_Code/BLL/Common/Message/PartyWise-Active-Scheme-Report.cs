using NLog;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Xml;


/// <summary>
/// Summary description for PartyWise_Active_Scheme_Report
/// </summary>
namespace Goldmedal.Emails.App_Code.BLL.Common.Message
{
    public class PartyWise_Active_Scheme_Report : ITask
    {
        private int _maxTries = 5;
        private readonly IMessageService _messageService;
        private readonly GoldLogging _gLog;
        private readonly IEmailTemplateDA da;
        private readonly IGoldMedia _goldMedia;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private SqlDataReader rdr = null;

        public PartyWise_Active_Scheme_Report()
        {
            _messageService = new MessageService();
            _gLog = new GoldLogging();
            da = new EmailTemplateDA();
            _goldMedia = new GoldMedia();
        }

        public void Execute(XmlNode node)
        {
            XmlAttribute attribute1 = node.Attributes["maxTries"];
            if (attribute1 != null && !String.IsNullOrEmpty(attribute1.Value))
            {
                this._maxTries = int.Parse(attribute1.Value);
            }

            DataTable dt = new DataTable();

            dt = _messageService.getdata("GetallPartyForActiveScheme");
            if (dt.Rows.Count > 0)
            {
                //   ExcuativeMail(dt);
                PartyandExcutiveMail(dt);
            }
        }

        protected void PartyandExcutiveMail(DataTable dt)
        {
            string AttachmentName = string.Empty;
            string FileName = string.Empty;
            int TemplateId = 3021;

            DataTable dtcheck = da.GetEmailForTimeCheckbyID(TemplateId);
            if (dtcheck.Rows.Count > 0)
            {
                int flag = 0;
                DataTable dtActiveTime = da.GetActiveSchemeTimeID(TemplateId);
                if (dtActiveTime.Rows.Count <= 0)
                {
                    flag = 1;
                }
                else
                {
                    DateTime lastsend = Convert.ToDateTime(dtActiveTime.Rows[0]["lastSend"]);
                    int days = (DateTime.Now.Date - lastsend).Days;
                    int interval = Convert.ToInt32(dtcheck.Rows[0]["interval"]);
                    if (days >= interval)
                    {
                        flag = 1;
                    }
                    else
                    {
                        flag = 0;
                    }
                }

                if (flag == 1)
                {
                    da.AddActiveScheme(TemplateId);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        try
                        {
                            string cc = string.Empty;
                            if (_messageService.ValidateEmailId(dt.Rows[i]["emailid"].ToString()) == true)
                            {
                                //DataTable dtcheckhistory = da.GetQueuedEmailHistoryforExandag(dt.Rows[i]["emailid"].ToString(), TemplateId);
                                //if (dtcheckhistory.Rows.Count <= 0)
                                //{
                                if (dt.Rows[i]["salesexid"].ToString() != "" && dt.Rows[i]["salesexid"].ToString() != null)
                                {
                                    DataTable dtexcutive = new DataTable();
                                    dtexcutive = _messageService.return_dt("salesexselectByID " + Convert.ToInt32(dt.Rows[i]["salesexid"]));

                                    if (dtexcutive.Rows.Count > 0)
                                    {
                                        if (_messageService.ValidateEmailId(dtexcutive.Rows[0]["email"].ToString()) == true)
                                        {
                                            cc = dtexcutive.Rows[0]["email"].ToString();
                                        }
                                    }
                                }

                                byte[] report = generatePDF(Convert.ToInt32(dt.Rows[i]["cin"]));
                                Stream stream = new MemoryStream(report);
                                FileName = string.Format(@"{0}", dt.Rows[i]["cin"].ToString() + dt.Rows[i]["printnm"].ToString());
                                string uniquefoldernm = dt.Rows[i]["cin"].ToString() + "ActiveScheme";                               
                                var retStr = _goldMedia.GoldMediaUpload(FileName, TemplateId.ToString() + "/" + uniquefoldernm, ".pdf", stream, "application/pdf", false, false, true);
                                SendMail(TemplateId, dt.Rows[i]["emailid"].ToString(), dt.Rows[i]["name"].ToString(), FileName + ".pdf", uniquefoldernm, cc);
                                //  }
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.Error(ex);
                        }
                    }
                }
            }
        }

        private void SendMail(int TemplateId, string Email, string Name, string Attachment, string uniquefoldernm, string cc)
        {
            //DataTable dtcheck = da.GetQueuedEmailHistoryforExandag(Email, TemplateId);
            //if (dtcheck.Rows.Count <= 0)
            //{
            string Attach = string.Empty;
            string Subject = string.Empty;
            string Body = string.Empty;
            Attach = TemplateId + "/" + uniquefoldernm + "/" + Attachment;
            //if (Attachment != "")
            //{
            //    Attach = ProperAttachmentUrl(Attachment, TemplateId, uniquefoldernm);
            //}
            DataTable dt = new DataTable();
            dt = da.GetEmailTemplateDatabyID(TemplateId);

            if (dt.Rows.Count > 0)
            {
                Subject = HttpUtility.HtmlDecode(dt.Rows[0]["Subject"].ToString());

                Body = dt.Rows[0]["Body"].ToString();

                Body = Body + "&nbsp;&nbsp;" + dt.Rows[0]["Signature"].ToString();

                da.AddEmailQueue(0, Email, Name, cc, "", Subject, Body, dt.Rows[0]["EmailTime"].ToString(), dt.Rows[0]["EmailLastDate"].ToString(), 0, 0, TemplateId, Attach, "Active-Scheme-Report", 1);
            }
            //  }
        }



        protected byte[] generatePDF(int PartyCin)
        {
            byte[] report = null;
            try
            {
                string url = WebConfigurationManager.AppSettings["ErpActiveSchemeReport"].ToString() + PartyCin;              
                var webClient = new WebClient();
                report = webClient.DownloadData(new Uri(url));
            }
            catch (Exception ex)
            {
               
            }
            return report;
        }
    }
}