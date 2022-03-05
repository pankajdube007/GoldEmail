using System;
using System.Data;
using System.Net;
using System.Web.Configuration;
using System.Xml;

/// <summary>
/// Summary description for SendSMS
/// </summary>
///
namespace Goldmedal.Emails.App_Code.BLL.Common.Message
{
    public partial class SendSMS : ITask
    {
        private int _maxTries = 5;
        private readonly IMessageService _messageService;
        private readonly GoldLogging _gLog;
        private WebClient web = new WebClient();
        private byte[] bufData = null;

        public SendSMS()
        {
            _messageService = new MessageService();
            _gLog = new GoldLogging();
        }

        public void Execute(XmlNode node)
        {
            XmlAttribute attribute1 = node.Attributes["maxTries"];
            if (attribute1 != null && !String.IsNullOrEmpty(attribute1.Value))
            {
                this._maxTries = int.Parse(attribute1.Value);
            }
            DataTable dt = new DataTable();
            dt = _messageService.GetQueuedMessageActiveData();
            //   string path = WebConfigurationManager.AppSettings["SmsUserID"];
            //   string path1 = WebConfigurationManager.AppSettings["SmsPassword"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int tries = Convert.ToInt32(dt.Rows[i]["SendTries"]);

                try
                {
                    bufData = web.DownloadData("http://sms6.rmlconnect.net:8080/bulksms/bulksms?username=" + WebConfigurationManager.AppSettings["SmsUser"].ToString() + "&password=" + WebConfigurationManager.AppSettings["SmsPassword"].ToString() + "&type=0&dlr=1&destination=" + Convert.ToString(dt.Rows[i]["To"]) + "&source=GLDMDL&message=" + Convert.ToString(dt.Rows[i]["Body"])+ "&entityid=1601100000000001629&tempid=" + Convert.ToString(dt.Rows[i]["tritemplateid"]));

                    // bufData = web.DownloadData("http://sms6.routesms.com:8080/bulksms/bulksms?username=goldmedal&password=sd56jjaa&type=0&dlr=0&destination=" + Convert.ToString(dt.Rows[i]["To"]) + "&source=GLDMDL&message=" + Convert.ToString(dt.Rows[i]["Body"]));

                    //   bufData = web.DownloadData("http://sms6.routesms.com:8080/bulksms/bulksms?username=goldmedal&password=sd56jjaa&type=0&dlr=0&destination=" + Convert.ToString(dtsms.Rows[0]["contactno"]) + "&source=GLDMDL&message=Invoice: " + Convert.ToString(dtsms.Rows[0]["name"]) + "(" + Convert.ToString(dtsms.Rows[0]["code"]) + "),Invoice no: " + orderno + " Dt " + orderdate + " of Rs." + orderamount + " have been generated and goods will be dispatched soon from " + dispatchfrom + "  Branch. &url=KKKK%3E");

                    //     tries = tries + 1;
                    // DateTime dta = DateTime.UtcNow;
                    _messageService.UpdateQueuedMessageAfterSend(Convert.ToInt32(dt.Rows[i]["QueuedMessageId"]));
                }
                catch (Exception exc)
                {
                    //queuedEmail.SendTries = queuedEmail.SendTries + 1;
                    //_messageService.UpdateQueuedEmail(queuedEmail);


                    _gLog.SendErrorToText(exc);
                }
            }
        }
    }
}