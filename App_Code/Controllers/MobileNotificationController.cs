using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

public class MobileNotificationController : ApiController
{
    [HttpPost]
    [ValidateModel]
    [Route("api/MobileNotification")]
    public HttpResponseMessage GetMobileNotification(mobileNoti noti)
    {
        Common cm = new Common();
        EmailTemplateDA da = new EmailTemplateDA();

        try
        {
            string data1;
            List<mobileNotis> mobileNotis = new List<mobileNotis>();
            bool result = da.AddMobileNotification(noti.Priority, noti.NotifSlno, noti.createuid);

            if (result == true)
            {
                mobileNotis.Add(new mobileNotis
                {
                    result = "True",
                    message = "",
                    servertime = DateTime.Now.ToString(),
                    // data = null
                });

                data1 = JsonConvert.SerializeObject(mobileNotis, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);

                response.Content = new StringContent(data1, Encoding.Unicode);
                return response;
            }
            else
            {
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(cm.StatusTime(false, "Oops! Something is wrong, try again later!!!!!!!!"), Encoding.Unicode);

                return response;
            }
        }
        catch (Exception ex)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(cm.StatusTime(false, "Oops! Something is wrong, try again later!!!!!!!!" + ex.ToString()), Encoding.Unicode);

            return response;
        }
    }
}