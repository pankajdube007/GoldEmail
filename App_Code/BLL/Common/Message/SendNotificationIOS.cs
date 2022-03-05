using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Xml;
using NLog;
using System.Web.Http;
using PushSharp.Apple;

/// <summary>
/// Summary description for SendNotificationIOS
/// </summary>
namespace Goldmedal.Emails.App_Code.BLL.Common.Message
{
    public class SendNotificationIOS : ITask
    {
        private int _maxTries = 5;
        private readonly IMessageService _messageService;
        private readonly GoldLogging _gLog;
        private WebClient web = new WebClient();
        private byte[] bufData = null;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public SendNotificationIOS()
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
            dt = _messageService.GetQueuedMobileNotificationData("IOS");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //    int tries = Convert.ToInt32(dt.Rows[i]["SendTries"]);

                try
                {
                  //  var ttt = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/UserFiles/APN_Development.p12");

                    //Get Certificate
                    var appleCert = System.IO.File.ReadAllBytes(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/UserFiles/APNs_Prod.p12"));

                    // Configuration (NOTE: .pfx can also be used here)
                    var config = new ApnsConfiguration(ApnsConfiguration.ApnsServerEnvironment.Sandbox, appleCert, "mancity");

                    // Create a new broker
                    var apnsBroker = new ApnsServiceBroker(config);

                    // Wire up events
                    apnsBroker.OnNotificationFailed += (notification, aggregateEx) =>
                    {

                        aggregateEx.Handle(ex =>
                        {

                            // See what kind of exception it was to further diagnose
                            if (ex is ApnsNotificationException)
                            {
                                var notificationException = (ApnsNotificationException)ex;

                                // Deal with the failed notification
                                var apnsNotification = notificationException.Notification;
                                var statusCode = notificationException.ErrorStatusCode;
                                string desc = $"Apple Notification Failed: ID={apnsNotification.Identifier}, Code={statusCode}";
                                Console.WriteLine(desc);
                                //  Label1.Text = desc;
                            }
                            else
                            {
                                string desc = $"Apple Notification Failed for some unknown reason : {ex.InnerException}";
                                // Inner exception might hold more useful information like an ApnsConnectionException           
                                Console.WriteLine(desc);
                                //  Label1.Text = desc;
                            }

                            // Mark it as handled
                            return true;
                        });
                    };

                    apnsBroker.OnNotificationSucceeded += (notification) =>
                    {
                        //  Label1.Text = "Apple Notification Sent successfully!";
                    };




                    var fbs = new FeedbackService(config);
                    fbs.FeedbackReceived += (string devicToken, DateTime timestamp) =>
                    {
                        // Remove the deviceToken from your database
                        // timestamp is the time the token was reported as expired
                    };

                    // Start Proccess 
                    apnsBroker.Start();

                    var head = new Dictionary<string, object>();
                    var payload = new Dictionary<string, object>();
                    var aps = new Dictionary<string, object>();

                    head.Add("title", dt.Rows[i]["Title"].ToString());
                    head.Add("body", dt.Rows[i]["Message"].ToString());
                    aps.Add("alert", head);
                    aps.Add("badge", 1);
                    aps.Add("sound", "chime.aiff");
                    payload.Add("aps", aps);


                    if (!string.IsNullOrEmpty(dt.Rows[i]["redirecttype"].ToString() ))
                    {
                        payload.Add("redirectToActivity", dt.Rows[i]["redirecttype"].ToString());
                        if (dt.Rows[i]["redirecttype"].ToString() == "Web")
                        {
                            payload.Add("redirecturl", dt.Rows[i]["redirecturl"].ToString());

                        }
                        else if (dt.Rows[i]["redirecttype"].ToString() == "Screen")
                        {

                            payload.Add("redirecturl", dt.Rows[i]["redirecturl"].ToString());
                        }
                        else
                        {
                            payload.Add("redirecturl", dt.Rows[i]["redirecturl"].ToString());
                        }


                    }
                    else
                    {
                        payload.Add("redirectToActivity", "NULL");
                    }

                    payload.Add("icon", dt.Rows[i]["imageurl"].ToString());
                    payload.Add("cin", dt.Rows[i]["cin"].ToString());

                    payload.Add("informToServer", dt.Rows[i]["logtoserver"].ToString());
                    payload.Add("notificationID", dt.Rows[i]["slno"].ToString());//it will come from DB
                    payload.Add("attachment-url", dt.Rows[i]["imageurl"].ToString());//it will come from DB
                    payload.Add("media_type", "image");//it will come from DB


                    var jsonx = Newtonsoft.Json.JsonConvert.SerializeObject(payload);

                    DataTable dt1 = _messageService.return_dt("exec appnotificationDetails " + Convert.ToInt32(dt.Rows[i]["slno"]) + ",'ios'");

                    if(dt1.Rows.Count>0)

                    {
                        for (int k = 0; k < dt1.Rows.Count; k++)
                        {
                            if (dt1.Rows[k]["pooswooshid"].ToString() != "")
                            {
                                apnsBroker.QueueNotification(new ApnsNotification
                                {
                                    DeviceToken = dt1.Rows[k]["pooswooshid"].ToString(),
                                    Payload = JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(payload))
                                });
                            }

                          

                        }

                        
                    }
                    apnsBroker.Stop();
                    bool result1 = _messageService.UpdateQueuedNotificationAfterSend(Convert.ToInt32(dt.Rows[i]["QueuedNotificationID"]), "IOS");

                 
                }
                catch (Exception exc)
                {
                    Logger.Error("Notification("+DateTime.Now.ToString()+") An error occurred while Sending Notification - " + exc.Message);
                }
            }
        }
    }
}