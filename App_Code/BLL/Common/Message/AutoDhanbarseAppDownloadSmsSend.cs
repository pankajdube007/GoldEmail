using System;
using System.Data;
using System.Net;
using System.Web.Configuration;
using System.Xml;


/// <summary>
/// Summary description for AutoDhanbarseAppDownloadSmsSend
/// </summary>
namespace Goldmedal.Emails.App_Code.BLL.Common.Message
{
    public class AutoDhanbarseAppDownloadSmsSend :ITask
    {
     
        private int _maxTries = 5;
        private readonly IEmailTemplateDA da;
        private readonly IMessageService _messageService;
        private WebClient web = new WebClient();
        private byte[] bufData = null;


        public AutoDhanbarseAppDownloadSmsSend()
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
            if (DateTime.Now.Hour >= 5)
            {
                DataTable dt = da.GetspfireDetails("autoDhanbarseAppDownloadlinksend");
                if (dt.Rows.Count == 0)
                {

                    //  if (DateTime.Compare(DateTime.Now, time1) > 0)

                    DataTable dt1 = _messageService.return_dt("autoDhanbarseAppDownloadlinksend");

                    if (dt1.Rows.Count > 0)
                    {
                        da.CreateSPFireDetails("autoDhanbarseAppDownloadlinksend");
                        string sms = string.Empty;

                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            if (_messageService.ValidateMobileNo(dt1.Rows[i]["MobileNo"].ToString()))
                            {
                                // sms = "Dear " + dt1.Rows[i]["UserName"].ToString() + ", Please download Dhanbarse App on bellow link  https://play.google.com/store/apps/details?id=com.goldmedal.gparivaar&hl=en_IN.  Regards,  Team Goldmedal  *TC Apply";
                                sms = dt1.Rows[i]["msg"].ToString();
                                bufData = web.DownloadData("http://sms6.rmlconnect.net:8080/bulksms/bulksms?username=" + WebConfigurationManager.AppSettings["SmsUser"].ToString() + "&password=" + WebConfigurationManager.AppSettings["SmsPassword"].ToString() + "&type=0&dlr=0&destination=" + dt1.Rows[i]["MobileNo"].ToString() + "&source=GLDMDL&message=" + sms);
                            }
                        }

                       
                    }


                }
            }

        }
    }
  
}