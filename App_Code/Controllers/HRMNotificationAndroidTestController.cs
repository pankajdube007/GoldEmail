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

/// <summary>
/// Summary description for HRMNotificationAndroidTest
/// </summary>
public class HRMNotificationAndroidTestController : ApiController
{
    [HttpPost]
    [ValidateModel]
    [Route("api/HRMNotificationAndroidTest")]
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
            var notification = new AndroidNotification();
            var data = new Dictionary<string, string>();
            var redirectToActivitydata = new Dictionary<string, string>();

            if (mailsms.redirectToActivity != "")
            {
                if (mailsms.redirectToActivity == "Web")
                {
                    // data.Add("redirectToWeb", mailsms.redirecturl);
                    redirectToActivitydata.Add("redirectToWeb", mailsms.redirecturl);
                }
                else if (mailsms.redirectToActivity == "Screen")
                {
                    // data.Add("redirectToScreen", mailsms.redirecturl);
                    redirectToActivitydata.Add("redirectToScreen", mailsms.redirecturl);
                }
                data.Add("redirectToActivity", JsonConvert.SerializeObject(redirectToActivitydata));
                // data.Add("redirectToActivity", mailsms.redirectToActivity);
            }
            else
            {
                data.Add("redirectToActivity", "NULL");
            }

            data.Add("informToServer", mailsms.informToServer);
            data.Add("notificationID", "1");//it will come from DB
            data.Add("imageurl", mailsms.imageurl);//it will come from DB




            //  DataTable dt1 = ms.return_dt("exec appnotificationDetailstest '" + mailsms.cin+"'");
            if (mailsms.pooswooshid != "")
            {
                JArray devid = new JArray();
                devid.Add(mailsms.pooswooshid);
                ICollection<string> dtdeviceid = new List<string>();
                dtdeviceid.Add(mailsms.pooswooshid);
                //for (int k = 0; k < dt1.Rows.Count; k++)
                //{
                //    dtdeviceid.Add(dt1.Rows[k]["pooswooshid"].ToString());
                //}

                data.Add("Title", mailsms.title);
                data.Add("Body", mailsms.body);
                data.Add("Icon", mailsms.imageurl);


                //  notification.Title = mailsms.title;
                //  notification.Body = mailsms.body;
                //  notification.Icon = mailsms.imageurl;

                var message = new NotiMessage()
                {
                    RegistrationIds = dtdeviceid,
                    // To = txtDeviceKey.Text.Trim() ?? null,
                    Notification = notification,
                    Data = data
                };
                var task = System.Threading.Tasks.Task.Factory.StartNew
                    (
                    async () => await client.SendMessageAsyncHRM(message)
                    );

                var result = task.Result;

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
}