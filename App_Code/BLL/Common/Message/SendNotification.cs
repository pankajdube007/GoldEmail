using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Xml;

/// <summary>
/// Summary description for SendNotification
/// </summary>
namespace Goldmedal.Emails.App_Code.BLL.Common.Message
{
    public class SendNotification : ITask
    {
        private int _maxTries = 5;
        private readonly IMessageService _messageService;
        private readonly GoldLogging _gLog;
        private WebClient web = new WebClient();
        private byte[] bufData = null;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public SendNotification()
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
            gparivarnotification();
            gstarnotification();
        }

        public void gparivarnotification()
        {

            DataTable dt = new DataTable();
            dt = _messageService.GetQueuedMobileNotificationData("Android");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //    int tries = Convert.ToInt32(dt.Rows[i]["SendTries"]);

                try
                {
                    var redirectToScreen = dt.Rows[i]["redirecttype"];
                    var client = new FCMClient();
                    var notification = new AndroidNotification();
                    var data = new Dictionary<string, string>();
                    var redirectToActivitydata = new Dictionary<string, string>();

                    //notification.Title = dt.Rows[i]["Title"].ToString();
                    //notification.Body = dt.Rows[i]["Message"].ToString();
                    //notification.Icon = dt.Rows[i]["imageurl"].ToString();

                    data.Add("Title", dt.Rows[i]["Title"].ToString());
                    data.Add("Body", dt.Rows[i]["Message"].ToString());
                    data.Add("Icon", dt.Rows[i]["imageurl"].ToString());


                    if (dt.Rows[i]["redirecttype"].ToString() != "")
                    {
                        if (dt.Rows[i]["redirecttype"].ToString() == "web")
                        {
                            redirectToActivitydata.Add("redirectToWeb", dt.Rows[i]["redirecturl"].ToString());
                        }
                        else if (dt.Rows[i]["redirecttype"].ToString() == "screen")
                        {
                            redirectToActivitydata.Add("redirectToScreen", dt.Rows[i]["redirecturl"].ToString());
                        }
                        data.Add("redirectToActivity", JsonConvert.SerializeObject(redirectToActivitydata));
                    }
                    else
                    {
                        data.Add("redirectToActivity", "NULL");
                    }
                    data.Add("informToServer", dt.Rows[i]["logtoserver"].ToString());
                    data.Add("notificationID", dt.Rows[i]["QueuedNotificationID"].ToString());
                    data.Add("imageurl", dt.Rows[i]["imageurl"].ToString());

                    DataTable dt1 = _messageService.return_dt("exec appnotificationDetails " + Convert.ToInt32(dt.Rows[i]["slno"]) + ",'android'");
                    if (dt1.Rows.Count > 0)
                    {
                        List<DataTable> dtuse = SplitTable(dt1, 100);
                        for (int f = 0; f < dtuse.Count; f++)
                        {
                            ICollection<string> dtdeviceid = new List<string>();

                            for (int k = 0; k < dtuse[f].Rows.Count; k++)
                            {
                                dtdeviceid.Add(dtuse[f].Rows[k]["pooswooshid"].ToString());
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
                                async () => await client.SendMessageAsync(message)
                                );

                            var result = task.Result;
                        }


                        bool result1 = _messageService.UpdateQueuedNotificationAfterSend(Convert.ToInt32(dt.Rows[i]["QueuedNotificationID"]), "Android");
                    }
                }
                catch (Exception exc)
                {
                    Logger.Error("An error occurred while Sending Notification G Parivar Android- " + exc.Message);
                }
            }
        }

        public void gstarnotification()
        {

            DataTable dt = new DataTable();
            dt = _messageService.GetQueuedMobileNotificationDataEx("Android");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //    int tries = Convert.ToInt32(dt.Rows[i]["SendTries"]);

                try
                {
                    var redirectToScreen = dt.Rows[i]["redirecttype"];
                    var client = new FCMClient();
                    var notification = new AndroidNotification();
                    var data = new Dictionary<string, string>();
                    var redirectToActivitydata = new Dictionary<string, string>();

                    //notification.Title = dt.Rows[i]["Title"].ToString();
                    //notification.Body = dt.Rows[i]["Message"].ToString();
                    //notification.Icon = dt.Rows[i]["imageurl"].ToString();

                    data.Add("Title", dt.Rows[i]["Title"].ToString());
                    data.Add("Body", dt.Rows[i]["Message"].ToString());
                    data.Add("Icon", dt.Rows[i]["imageurl"].ToString());


                    if (dt.Rows[i]["redirecttype"].ToString() != "")
                    {
                        if (dt.Rows[i]["redirecttype"].ToString() == "web")
                        {
                            redirectToActivitydata.Add("redirectToWeb", dt.Rows[i]["redirecturl"].ToString());
                        }
                        else if (dt.Rows[i]["redirecttype"].ToString() == "screen")
                        {
                            redirectToActivitydata.Add("redirectToScreen", dt.Rows[i]["redirecturl"].ToString());
                        }
                        data.Add("redirectToActivity", JsonConvert.SerializeObject(redirectToActivitydata));
                    }
                    else
                    {
                        data.Add("redirectToActivity", "NULL");
                    }
                    data.Add("informToServer", dt.Rows[i]["logtoserver"].ToString());
                    data.Add("notificationID", dt.Rows[i]["QueuedNotificationID"].ToString());
                    data.Add("imageurl", dt.Rows[i]["imageurl"].ToString());

                    DataTable dt1 = _messageService.return_dt("exec appnotificationDetails " + Convert.ToInt32(dt.Rows[i]["slno"]) + ",'android'");
                    if (dt1.Rows.Count > 0)
                    {
                        List<DataTable> dtuse = SplitTable(dt1, 100);
                        for (int f = 0; f < dtuse.Count; f++)
                        {
                            ICollection<string> dtdeviceid = new List<string>();

                            for (int k = 0; k < dtuse[f].Rows.Count; k++)
                            {
                                dtdeviceid.Add(dtuse[f].Rows[k]["pooswooshid"].ToString());
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
                        }



                        bool result1 = _messageService.UpdateQueuedNotificationAfterSend(Convert.ToInt32(dt.Rows[i]["QueuedNotificationID"]), "Android");
                    }
                }
                catch (Exception exc)
                {
                    Logger.Error("An error occurred while Sending Notification GStar Android - " + exc.Message);
                }
            }
        }

        private List<DataTable> SplitTable(DataTable tableToClone, int countLimit)
        {
            List<DataTable> tables = new List<DataTable>();
            int count = 0;
            DataTable copyTable = null;
            foreach (DataRow dr in tableToClone.Rows)
            {
                if ((count++ % countLimit) == 0)
                {
                    copyTable = new DataTable();
                    copyTable = tableToClone.Clone();
                    copyTable.TableName = "TableCount" + count;
                    tables.Add(copyTable);
                }
                copyTable.ImportRow(dr);
            }
            return tables;
        }
    }
}