using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;

public class ViewPartywisependingDetailsController : ApiController
{
    [HttpPost]
    [ValidateModel]
    [Route("api/ViewPartywisependingDetails")]
    public HttpResponseMessage GetViewTemplate(PendingDetails mails)
    {
        Common cm = new Common();
        MessageService ms = new MessageService();

        try
        {
            string data1;
            string Subject;
            string Body = "";

            EmailTemplateDA da = new EmailTemplateDA();
            PrivateMessage pm = new PrivateMessage();

            DataTable dt = da.GetEmailTemplateDatabyID(mails.ID);
            DataTable dt1 = da.GetMeassageById(mails.ID); ;
            List<PendingDetails> testmail = new List<PendingDetails>();
            List<PendingDetailsDetails> Listtestmail = new List<PendingDetailsDetails>();
            List<DetailsTemplatePendingDetails> Template = new List<DetailsTemplatePendingDetails>();

            if (mails.For == "Email")
            {
                if (dt.Rows.Count > 0)
                {
                    Subject = HttpUtility.HtmlDecode(ms.ReplaceTemplatePartyWisePendingDetails(dt.Rows[0]["Subject"].ToString(), dt.Rows[0]["Subject"].ToString(), mails));
                    Body = ms.ReplaceTemplatePartyWisePendingDetails(dt.Rows[0]["Body"].ToString(), dt.Rows[0]["Body"].ToString(), mails);
                    Body = HttpUtility.HtmlDecode(Body + "&nbsp;&nbsp;" + dt.Rows[0]["Signature"].ToString());

                    Template.Add(new DetailsTemplatePendingDetails
                    {
                        SubjectTEMP = Subject,
                        BodyTEMP = Body,
                    });
                }
            }
            else if (mails.For == "SMS")
            {
                if (dt1.Rows.Count > 0)
                {
                    Body = ms.ReplaceTemplatePartyWisePendingDetails(dt1.Rows[0]["Body"].ToString(), dt1.Rows[0]["Body"].ToString(), mails);
                }
                Template.Add(new DetailsTemplatePendingDetails
                {
                    SubjectTEMP = "",
                    BodyTEMP = Body,
                });
            }

            Listtestmail.Add(new PendingDetailsDetails
            {
                result = "True",
                message = "Test Mail Send",
                servertime = DateTime.Now.ToString(),
                data = Template,
            });

            data1 = JsonConvert.SerializeObject(Listtestmail, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);

            response.Content = new StringContent(data1, Encoding.Unicode);
            return response;
        }
        catch (Exception ex)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(cm.StatusTime(false, "Oops! Something is wrong, try again later!!!!!!!!" + ex.ToString()), Encoding.Unicode);

            return response;
        }
    }
}