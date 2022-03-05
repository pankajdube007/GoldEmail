using System;
using System.Data;
using System.Net;
using System.Web.Configuration;
using System.Xml;

/// <summary>
/// Summary description for AutoWorldCup3
/// </summary>
namespace Goldmedal.Emails.App_Code.BLL.Common.Message
{
    public class AutoWorldCup3 : ITask
    {
        private int _maxTries = 5;
        private readonly IEmailTemplateDA da;
        private readonly IMessageService _messageService;
        private WebClient web = new WebClient();
        private byte[] bufData = null;


        public AutoWorldCup3()
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
            if (DateTime.Now.Hour >= 10)
            {
                DataTable dt = da.GetspfireDetails("autosmswordcup3");
                if (dt.Rows.Count == 0)
                {

                    //  if (DateTime.Compare(DateTime.Now, time1) > 0)

                    DataTable dt1 = _messageService.return_dt("autosmswordcup3");

                    if (dt1.Rows.Count > 0)
                    {
                        string sms = string.Empty;

                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            if (_messageService.ValidateMobileNo(dt1.Rows[i]["contactno"].ToString()))
                            {
                                sms = "Dear " + dt1.Rows[i]["name"].ToString() + ", Last few days to predict the winners of the matches to be played in the World Cup till the finals. We have checked from our records that you have partially filled the prediction i.e Finals still need to be filled from your end. So do hurry and get the maximum benefit by completing the prediction.There will be no selection allowed post 29th May 2019. Please ignore if you have filled the selection of the winners list.  Regards,  Team Goldmedal  *TC Apply";
                                bufData = web.DownloadData("http://sms6.rmlconnect.net:8080/bulksms/bulksms?username=" + WebConfigurationManager.AppSettings["SmsUser"].ToString() + "&password=" + WebConfigurationManager.AppSettings["SmsPassword"].ToString() + "&type=0&dlr=0&destination=" + dt1.Rows[i]["contactno"].ToString() + "&source=GLDMDL&message=" + sms);
                            }
                        }

                        da.CreateSPFireDetails("autosmswordcup3");
                    }


                }
            }

            if (DateTime.Now.Hour >= 20)
            {
                DataTable dt = da.GetspfireDetails("autosmswordcup3");
                if (dt.Rows.Count < 2)
                {

                    DataTable dt1 = _messageService.return_dt("autosmswordcup3");
                    if (dt1.Rows.Count > 0)
                    {
                        string sms = string.Empty;
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            if (_messageService.ValidateMobileNo(dt1.Rows[i]["contactno"].ToString()))
                            {
                                sms = "Dear " + dt1.Rows[i]["name"].ToString() + ", Last few days to predict the winners of the matches to be played in the World Cup till the finals. We have checked from our records that you have partially filled the prediction i.e Finals still need to be filled from your end. So do hurry and get the maximum benefit by completing the prediction.There will be no selection allowed post 29th May 2019. Please ignore if you have filled the selection of the winners list.  Regards,  Team Goldmedal  *TC Apply";
                                bufData = web.DownloadData("http://sms6.rmlconnect.net:8080/bulksms/bulksms?username=" + WebConfigurationManager.AppSettings["SmsUser"].ToString() + "&password=" + WebConfigurationManager.AppSettings["SmsPassword"].ToString() + "&type=0&dlr=0&destination=" + dt1.Rows[i]["contactno"].ToString() + "&source=GLDMDL&message=" + sms);
                              //  bufData = web.DownloadData("http://sms6.rmlconnect.net:8080/bulksms/bulksms?username=" + WebConfigurationManager.AppSettings["SmsUser"].ToString() + "&password=" + WebConfigurationManager.AppSettings["SmsPassword"].ToString() + "&type=0&dlr=0&destination=" + "9518957760" + "&source=GLDMDL&message=" + sms);
                            }
                        }
                        da.CreateSPFireDetails("autosmswordcup3");
                    }

                }

            }
        }
    }
}