using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Xml;

/// <summary>
/// Summary description for AutoExecutiveSignOut
/// </summary>
namespace Goldmedal.Emails.App_Code.BLL.Common.Message
{
    public class AutoExecutiveSignOut:ITask
    {
        private int _maxTries = 5;
        private readonly IMessageService _messageService;
        private readonly IEmailTemplateDA da;
        private readonly GoldLogging _gLog;
        private WebClient web = new WebClient();
        private byte[] bufData = null;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public AutoExecutiveSignOut()
        {
            _messageService = new MessageService();
            _gLog = new GoldLogging();
            da = new EmailTemplateDA();
        }

        public void Execute(XmlNode node)
        {
            XmlAttribute attribute1 = node.Attributes["maxTries"];
            if (attribute1 != null && !String.IsNullOrEmpty(attribute1.Value))
            {
                this._maxTries = int.Parse(attribute1.Value);
            }
            //DataTable dt = new DataTable();
            //dt = _messageService.GetQueuedMobileNotificationData();

            if (DateTime.Now.Hour >= 19 && DateTime.Now.Minute >= 15)
            {
                DataTable dt = da.GetspfireDetails("aapexecsignoutmissing");
                if (dt.Rows.Count == 0)
                {
                    try
                    {
                        var client = new FCMClient();
                        var notification = new AndroidNotification();
                        var data = new Dictionary<string, string>();
                        var redirectToActivitydata = new Dictionary<string, string>();

                        data.Add("Title", "Today sign out missing");
                        data.Add("Body", "Today Date: " + DateTime.Now.ToString("dd-MM-yyyy") + " punch out missing, Please Sign out...");
                        data.Add("Icon", "");
                        data.Add("redirectToActivity", "My Attendance");
                        data.Add("informToServer", "false");
                        data.Add("notificationID", DateTime.Now.ToString("ddMMyyyy"));
                        data.Add("imageurl", "");

                        DataTable dt1 = _messageService.return_dt("exec aapexecsignoutmissing");
                        if (dt1.Rows.Count > 0)
                        {
                            JArray devid = new JArray();
                            //  devid.Add(pooswooshid);
                            ICollection<string> dtdeviceid = new List<string>();

                            for (int k = 0; k < dt1.Rows.Count; k++)
                            {
                                dtdeviceid.Add(dt1.Rows[k]["pooswooshid"].ToString());
                            }

                            var message = new NotiMessage()
                            {
                                RegistrationIds = dtdeviceid,
                                // To = txtDeviceKey.Text.Trim() ?? null,
                                Notification = notification,
                                Data = data
                            };
                            var task = System.Threading.Tasks.Task.Factory.StartNew
                                (
                                async () => await client.SendMessageAsyncExcutiveApp(message)
                                );

                            var result = task.Result;

                            da.CreateSPFireDetails("aapexecsignoutmissing");
                        }
                    }
                    catch (Exception exc)
                    {
                        Logger.Error("An error occurred while Sending Notification - " + exc.Message);
                    }
                }
            }
        }
    }
}