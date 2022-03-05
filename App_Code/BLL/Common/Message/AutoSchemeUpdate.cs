using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Xml;

/// <summary>
/// Summary description for AutoSchemeUpdate
/// </summary>
namespace Goldmedal.Emails.App_Code.BLL.Common.Message
{
    public class AutoSchemeUpdate : ITask
    {
        private int _maxTries = 5;
        private readonly IMessageService _messageService;
        private readonly GoldLogging _gLog;
        private readonly IEmailTemplateDA da;
        private readonly IGoldMedia _goldMedia;
        private MessageService ms = new MessageService();

        private SqlDataReader rdr = null;

        public AutoSchemeUpdate()
        {
            _messageService = new MessageService();
            _gLog = new GoldLogging();
            da = new EmailTemplateDA();
            _goldMedia = new GoldMedia();
        }

        public void Execute(XmlNode node)
        {
            //   int DealerTemplateId = 3025;
            //   int ExTemplateId = 3024;

            XmlAttribute attribute1 = node.Attributes["maxTries"];
            if (attribute1 != null && !String.IsNullOrEmpty(attribute1.Value))
            {
                this._maxTries = int.Parse(attribute1.Value);
            }
            PartyMail();
            //  PartyAgentMail(ExTemplateId);
        }








        protected void PartyMail()
        {
            string AttachmentName = string.Empty;
            string FileName = string.Empty;
            DataTable dt = _messageService.return_dt("exec getschemedata");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                try
                {
                    byte[] report = generatePDF(Convert.ToString(dt.Rows[i]["cin"]));
                    Stream stream = new MemoryStream(report);
                    FileName = string.Format(@"{0}", dt.Rows[i]["cin"].ToString() + "dec2021");
                    //  string uniquefoldernm = dtparty.Rows[0]["cin"].ToString() + "CreditNote";

                    var retStr = _goldMedia.GoldMediaUpload(FileName, "creditnote", ".pdf", stream, "application/pdf", false, false, true);

                    DataTable dt1 = _messageService.return_dt("exec updatescheme '" + dt.Rows[i]["partyid"].ToString() + "','" + dt.Rows[i]["typecat"].ToString() + "','" + FileName + ".pdf" + "'");
                }
                catch (Exception ex)
                {
                }
            }
        }




        protected byte[] generatePDF(string PartyCin)
        {
            string url = WebConfigurationManager.AppSettings["ErpBranchActiveSchemeReport"].ToString() + PartyCin;

            var webClient = new WebClient();
            byte[] report = webClient.DownloadData(new Uri(url));
            return report;
        }
        

    }
}
