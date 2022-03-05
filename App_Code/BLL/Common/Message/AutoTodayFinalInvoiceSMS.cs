using System;
using System.Data;
using System.Net;
using System.Web.Configuration;
using System.Xml;

/// <summary>
/// Summary description for AutoTodayFinalInvoiceSMS
/// </summary>
namespace Goldmedal.Emails.App_Code.BLL.Common.Message
{
    public class AutoTodayFinalInvoiceSMS : ITask
    {

        private int _maxTries = 5;
        private readonly IEmailTemplateDA da;
        private readonly IMessageService _messageService;
        private WebClient web = new WebClient();
        private byte[] bufData = null;


        public AutoTodayFinalInvoiceSMS()
        {
            da = new EmailTemplateDA();
            _messageService = new MessageService();
        }

        public void Execute(XmlNode node)
        {
            XmlAttribute attribute1 = node.Attributes["maxTries"];
            if (attribute1 != null && !String.IsNullOrEmpty(attribute1.Value))
            {
                this._maxTries = int.Parse(attribute1.Value);
            }



            //  DateTime time1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 19, 0, 0);
            //  DateTime time2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 22, 0, 0);
            if (DateTime.Now.Hour >= 19)
            {
                DataTable dt = da.GetspfireDetails("TodayInvoiceReport");
                if (dt.Rows.Count == 0)
                {

                    //  if (DateTime.Compare(DateTime.Now, time1) > 0)

                    DataTable dt1 = _messageService.return_dt("TodayInvoiceReport");

                    if (dt1.Rows.Count > 0)
                    {
                        string sms = "Today Sale:" + String.Format("{0:N}", dt1.Rows[0]["TodayInvoice"]) + ", Current Month Sale:" + String.Format("{0:N}", dt1.Rows[0]["MonthlyInvoice"]) + ", Current Quarter Sale:" + String.Format("{0:N}", dt1.Rows[0]["QuarterlyInvoice"]) +
                        ", Today Payment:" + String.Format("{0:N}", dt1.Rows[0]["TodayPayment"]) + ", Current Month Payment:" + String.Format("{0:N}", dt1.Rows[0]["MonthlyPayment"]) + ", Current Quarter Payment:" + String.Format("{0:N}", dt1.Rows[0]["QuarterlyPayment"]);


                          bufData = web.DownloadData("http://sms6.rmlconnect.net:8080/bulksms/bulksms?username=" + WebConfigurationManager.AppSettings["SmsUser"].ToString() + "&password=" + WebConfigurationManager.AppSettings["SmsPassword"].ToString() + "&type=0&dlr=0&destination=" + "8981804211,9820012363,9820745251,9820224662,9820187966,9518957760" + "&source=GLDMDL&message=" + sms);
                        //  bufData = web.DownloadData("http://sms6.rmlconnect.net:8080/bulksms/bulksms?username=" + WebConfigurationManager.AppSettings["SmsUser"].ToString() + "&password=" + WebConfigurationManager.AppSettings["SmsPassword"].ToString() + "&type=0&dlr=0&destination=" + "9518957760" + "&source=GLDMDL&message=" + sms);

                        da.CreateSPFireDetails("TodayInvoiceReport");
                    }
                }
            }

            if (DateTime.Now.Hour >= 22)
            {
                DataTable dt = da.GetspfireDetails("TodayInvoiceReport");
                if (dt.Rows.Count < 2)
                {
                    // if (DateTime.Compare(DateTime.Now, time2) > 0)

                    DataTable dt1 = _messageService.return_dt("TodayInvoiceReport");

                    if (dt1.Rows.Count > 0)
                    {
                        string sms = "Today Sale:" + String.Format("{0:N}", dt1.Rows[0]["TodayInvoice"]) + ", Current Month Sale:" + String.Format("{0:N}", dt1.Rows[0]["MonthlyInvoice"]) + ", Current Quarter Sale:" + String.Format("{0:N}", dt1.Rows[0]["QuarterlyInvoice"]) +
                        ", Today Payment:" + String.Format("{0:N}", dt1.Rows[0]["TodayPayment"]) + ", Current Month Payment:" + String.Format("{0:N}", dt1.Rows[0]["MonthlyPayment"]) + ", Current Quarter Payment:" + String.Format("{0:N}", dt1.Rows[0]["QuarterlyPayment"]);

                          bufData = web.DownloadData("http://sms6.rmlconnect.net:8080/bulksms/bulksms?username=" + WebConfigurationManager.AppSettings["SmsUser"].ToString() + "&password=" + WebConfigurationManager.AppSettings["SmsPassword"].ToString() + "&type=0&dlr=0&destination=" + "8981804211,9820012363,9820745251,9820224662,9820187966,9518957760" + "&source=GLDMDL&message=" + sms);
                        // bufData = web.DownloadData("http://sms6.rmlconnect.net:8080/bulksms/bulksms?username=" + WebConfigurationManager.AppSettings["SmsUser"].ToString() + "&password=" + WebConfigurationManager.AppSettings["SmsPassword"].ToString() + "&type=0&dlr=0&destination=" + "9518957760" + "&source=GLDMDL&message=" + sms);

                        da.CreateSPFireDetails("TodayInvoiceReport");
                    }

                }
            }
        }
    }
}