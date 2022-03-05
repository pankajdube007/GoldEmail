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
using System.Web.Http;
using PushSharp.Apple;


public class IOSNotificationtest : ApiController
{
    [HttpPost]
    [ValidateModel]
    [Route("api/NotificationTestIOS1111111111111")]
    public HttpResponseMessage GetNotificationtest(notifactiontest mailsms)
    {
        Common cm = new Common();
        MessageService ms = new MessageService();
        try
        {
            string data1;
            string Body = string.Empty;
            List<notifactiontests> notificationresult = new List<notifactiontests>();
            List<Detailsnotifactiontest> notificationdata = new List<Detailsnotifactiontest>();

            var redirectToScreen = mailsms.redirectToActivity;
            var client = new FCMClient();
            var notification = new IOSNotification();
            var data = new Dictionary<string, string>();
            var redirectToActivitydata = new Dictionary<string, string>();

            if (mailsms.redirectToActivity != "")
            {
                data.Add("redirectToActivity", mailsms.redirectToActivity);
                if (mailsms.redirectToActivity == "Web")
                {
                    data.Add("redirecturl", mailsms.redirecturl);
                  
                }
                else if (mailsms.redirectToActivity == "Screen")
                {

                    data.Add("redirecturl", mailsms.redirecturl);
                }
               
              
            }
            else
            {
                data.Add("redirectToActivity", "NULL");
            }

            data.Add("cin", mailsms.cin);
            data.Add("informToServer", mailsms.informToServer);
            data.Add("notificationID", "1");//it will come from DB
            data.Add("attachment-url", mailsms.imageurl);//it will come from DB
            data.Add("media_type", "image");//it will come from DB




            //  DataTable dt1 = ms.return_dt("exec appnotificationDetailstest '" + mailsms.cin+"'");
            if (mailsms.pooswooshid != "")
            {
                JArray devid = new JArray();
                devid.Add(mailsms.pooswooshid);
                ICollection<string> dtdeviceid = new List<string>();
                dtdeviceid.Add(mailsms.pooswooshid);
             
                 notification.Title = mailsms.title;
                 notification.Body = mailsms.body;
                 notification.Sound = "default";
                 notification.ClickAction = "CustomNotification";

                var message = new NotiMessage()
                {
                    mutablecontent=true,
                    RegistrationIds = dtdeviceid,
                    // To = txtDeviceKey.Text.Trim() ?? null,
                    Notification = notification,
                    Data = data
                };
                var task = System.Threading.Tasks.Task.Factory.StartNew
                    (
                    async () => await client.SendMessageAsync(message)
                    );

                var result = task.Result;

                SendPushNotification(mailsms.pooswooshid, mailsms.body);

                //   bool result1 = _messageService.UpdateQueuedNotificationAfterSend(Convert.ToInt32(dt.Rows[i]["QueuedNotificationID"]));
            }

            notificationdata.Add(new Detailsnotifactiontest
            {
                output = "send",
            });

            notificationresult.Add(new notifactiontests
            {
                result = "True",
                message = "",
                servertime = DateTime.Now.ToString(),
                data = notificationdata,
            });

            data1 = JsonConvert.SerializeObject(notificationresult, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);

            response.Content = new StringContent(data1, Encoding.UTF8, "application/json");
            return response;

        }
        catch (Exception ex)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(cm.StatusTime(false, "Oops! Something is wrong, try again later!!!!!!!!"), Encoding.UTF8, "application/json");

            return response;
        }
    }

    private void SendPushNotification(string deviceToken, string message)
    {
        try
        {

            //Get Certificate
            var appleCert = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/App_Data/UserFiles/APN_Development.p12"));

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

            var payload = new Dictionary<string, object>();
            var aps = new Dictionary<string, object>();
            aps.Add("alert", "This is a sample notification!");
            aps.Add("badge", 1);
            aps.Add("sound", "chime.aiff");
            payload.Add("aps", aps);

            payload.Add("confId", "20");
            payload.Add("pageFormat", "Webs");
            payload.Add("pageTitle", "Evalu");
            payload.Add("webviewURL", "https:/UploadedImages/MobileApp/icons/Datalist-Defg");
            payload.Add("notificationBlastID", "");
            payload.Add("pushtype", "");

            payload.Add("content-available","" );


            var jsonx = Newtonsoft.Json.JsonConvert.SerializeObject(payload);

            if (deviceToken != "")
            {
                apnsBroker.QueueNotification(new ApnsNotification
                {
                    DeviceToken = deviceToken,
                    Payload = JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(payload))
                });
            }

            apnsBroker.Stop();

        }
        catch (Exception)
        {

            throw;
        }
    }
}
