using System;
using System.Data;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Xml;

/// <summary>
/// Summary description for AutoDVSMailSend
/// </summary>
namespace Goldmedal.Emails.App_Code.BLL.Common.Message
{
    public class AutoDVSMailSend : ITask
    {
        private int _maxTries = 5;
        private readonly IEmailTemplateDA da;
        private readonly IMessageService _messageService;
        private WebClient web = new WebClient();
        private byte[] bufData = null;


        public AutoDVSMailSend()
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
          //  if (DateTime.Now.Hour >= 7)
               if (DateTime.Now.Hour >= 7 && DateTime.Now.Day >= 20 && DateTime.Now.Month >= 4 && DateTime.Now.Year >= 2022)
                {
                DataTable dt = da.GetspfireDetails("GetCustomerTDSTCSApplicabilityTask");
                if (dt.Rows.Count == 0)
                {

                    //  if (DateTime.Compare(DateTime.Now, time1) > 0)

                    DataTable dt1 = _messageService.return_dt("GetCustomerTDSTCSApplicabilityTask");

                    if (dt1.Rows.Count > 0)
                    {
                        string sms = string.Empty;

                        for (int i = 0; i < dt1.Rows.Count; i++)
                            {
                        //    if (_messageService.ValidateEmailId(dt1.Rows[i]["EmailID"].ToString()))
                        //    {
                        //        //  sendmail(dt1.Rows[i]["UniqueKey"].ToString(), dt1.Rows[i]["CustomerType"].ToString(), dt1.Rows[i]["CustType"].ToString(), dt1.Rows[i]["EmailID"].ToString());
                        //        sendmailEx(dt1.Rows[i]["EmailID"].ToString());
                        //    }


                        if (_messageService.ValidateEmailId(dt1.Rows[i]["EmailID"].ToString()) && !string.IsNullOrEmpty(dt1.Rows[i]["UniqueKey"].ToString()))
                        {
                            sendmail(dt1.Rows[i]["UniqueKey"].ToString(), dt1.Rows[i]["CustomerType"].ToString(), dt1.Rows[i]["CustType"].ToString(), dt1.Rows[i]["EmailID"].ToString());

                        }


                        // For SMS

                        //if (_messageService.ValidateMobileNo(dt1.Rows[i]["mobile"].ToString()) && !string.IsNullOrEmpty(dt1.Rows[i]["UniqueKey"].ToString()))
                        //{
                        //    sendsms(dt1.Rows[i]["UniqueKey"].ToString(), dt1.Rows[i]["CustomerType"].ToString(), dt1.Rows[i]["CustType"].ToString(), dt1.Rows[i]["mobile"].ToString());
                        //}
                    }

                        da.CreateSPFireDetails("GetCustomerTDSTCSApplicabilityTask");
                    }


                }
            }

        }

        public void sendmail(string UniqueKey,string CustomerType ,string CustType,string EmailID)
        {
           // UniqueKey = dtshow.Rows[0]["UniqueKey"].ToString();
          //  email = dtshow.Rows[0]["email"].ToString();
            string Link = "https://erp.goldmedalindia.in/TDSTCSapplicability.aspx?UniqueKey=" + CustomerType + UniqueKey + "";
            string Subject = "New TDS Provisions for "+ CustType;
            string MailBody = @"<table style='font-weight: 400;'>
<tbody>
<tr>
<td>
<p>Dear Business Partner,</p>
</td>
</tr>
<tr>
<td>
<p>As you are aware, Finance Act 2020 had introduced&nbsp;<strong>Section 206C(1H) w.e.f 01.10.2020</strong>&nbsp;- Requirement to collect TCS on sale of goods at 0.1% on sales consideration exceeding Rs.50 Lakhs in a financial year. Further to this Finance Act 2021 had introduced following two new sections.</p>
</td>
</tr>
<tr>
<td>
<p>Finance Act 2021 has introduced a new&nbsp;<strong>Section 194Q</strong>&nbsp;with effect from 01.07.2021, where Buyer is required to deduct TDS on purchase of goods at the rate of 0.1% on the aggregate value exceeding Rs. 50 lakhs in a financial year, if Buyer has turnover above Rs. 10 Crore during the preceding financial year i.e.2021-22.&nbsp;<strong>IF SECTION 194Q IS APPLICABLE THAN SECTION 206C(1H) WILL NOT BE APPLICABLE.</strong>.</p>
</td>
</tr>
<tr>
<td>
<p>Finance Act 2021 has also introduced&nbsp;<strong>Section 206CCA which requires</strong>&nbsp;the seller to collect higher rate of TCS (Higher the twice of applicable rate or 5%) in case buyer has not filed Income tax returns for preceding two previous years. However this provision is not applicable if the aggregate of Tax deducted , collected at source is less than Rs.50,000 in each of the two previous years.</p>
</td>
</tr>
<tr>
<td>
<p>1.<strong>If dealers/customers turnover for FY 2021-22 is greater than 10 Crores,</strong>&nbsp;Kindly deduct TDS at the applicable rate of 0.1% on the aggregate value of purchase exceeding Rs.50 Lakhs in the current financial year. Goldmedal Electricals Pvt. Ltd. has duly filed Income tax returns for FY 2019-20 and FY 2020-21 and henceTDS to be deducted at normal rates and not higher rates.</p>
<p>2.&nbsp;<strong>If dealers/customers turnover for FY 2021-22 is less than or equal to 10 Crores,</strong>&nbsp;Goldmedal will collect TCS at the applicable rate of 0.1.% as practice followed currently.</p>
<p>We request you to kindly fill the attached turnover declaration form&nbsp;<strong>(Link Provided)</strong>&nbsp;at the earliest by 25.04.2022.</p>
<p>'<a href='"+Link+ @"'>" + Link + @"</a>'</p>
</td>
</tr>
<tr>
<td>
<p>In case of any clarification / or any correspondence (if required), please reach out to us at the following coordinate</p>
<p>Aruna Gupta &ndash;<a href='mailto:aruna.gupta@goldmedalindia.com'>aruna.gupta@goldmedalindia.com</a>&nbsp;( 022-42023000 Extn:3071 )</p>
</td>
</tr>
</tbody>
</table>
<p>Thanks &amp; Regards,</p>
<p>Aruna Gupta</p>
<p>✆ 022-42023000 Extn:3071</p>";

            da.AddEmailQueue(0, EmailID, "DVS","", "", Subject, MailBody, DateTime.Now.ToString(), "9999-12-01", 1, 1, 2017, "", "DVS Mail", 1);
        }



        public void sendmailEx( string EmailID)
        {
            // UniqueKey = dtshow.Rows[0]["UniqueKey"].ToString();
            //  email = dtshow.Rows[0]["email"].ToString();
         //   string Link = "https://erp.goldmedalindia.in/TDSTCSapplicability.aspx?UniqueKey=" + CustomerType + UniqueKey + "";
            string Subject = "TDS/TCS provisions wef 01.07.2021";
            string MailBody = @"<p><span style='font-weight: 400;'>Dear Team,</span></p>
<p style='font-weight: 400;'>&nbsp;</p>
<p style='font-weight: 400;'>We have sent the following mail&nbsp;<span style='color: #0000ff;'><strong>(written in blue font)</strong></span>&nbsp;to all our dealers on Mail (ID as per Address report ERP). Once they click on a link their turnover&nbsp;slab data gets registered with us.</p>
<p style='font-weight: 400;'><strong>&nbsp;</strong></p>
<p style='font-weight: 400;'>Our IT team has also sent messages to mobile numbers as per address report. Request you to follow up with executives&nbsp; and ask all dealers to fill in the data in the link mailed / messaged to them.</p>
<p style='font-weight: 400;'>In case they have not received&nbsp;the mail/ message , Kindly edit the mobile number/ email id in&nbsp;<strong>Transaction- Master-Customer details correction</strong>. Click on mobile no/ email id. Edit and save.</p>
<p style='font-weight: 400;'>&nbsp;</p>
<p style='font-weight: 400;'>After saving you will have to mail them the link through Reports- Master reports- Address report--SEND FILE in the last column</p>
<p style='font-weight: 400;'><strong>&nbsp;</strong></p>
<p style='font-weight: 400;'><strong>Please note all dealers have to fill this irrespective&nbsp;of their turnover and transactions with us. Any hard copies of forms filled will not be considered.&nbsp;</strong></p>
<p style='font-weight: 400;'><strong>&nbsp;</strong></p>
<p style='font-weight: 400;'><strong>THE ENTIRE PROCESS IS ATTACHED IN PPT FOR YOUR UNDERSTANDING</strong></p>
<p style='font-weight: 400;'>&nbsp;</p>
<p style='font-weight: 400;'>&nbsp;</p>
<p><span style='color: #0000ff;'>Dear Business Partner,</span></p>
<p>&nbsp;</p>
<p><span style='color: #0000ff;'>As&nbsp;you are&nbsp;aware, The Finance Act 2020 had introduced<strong>&nbsp;Section 206C(1H) w.e.f 01.10.2020</strong>&nbsp;-Requirement to collect TCS on sale of goods at 0.1% on sales consideration exceeding Rs.50 Lakhs in a financial year. Further to this Finance Act 2021 had introduced the following two new sections.</span></p>
<p><span style='color: #0000ff;'><strong>Section 194Q&nbsp;</strong>with effect from 01.07.2021, where Buyer is required to deduct TDS on purchase of goods at the rate of 0.1% on the aggregate value exceeding Rs. 50 lakhs in a financial year, if Buyer has turnover above Rs. 10 Crore during the preceding financial year i.e.2020-21.&nbsp;<strong>IF SECTION 194Q IS APPLICABLE THAN SECTION 206C(1H) WILL NOT BE APPLICABLE.</strong></span></p>
<p>&nbsp;</p>
<p><span style='color: #0000ff;'><strong>Section 206CCA which requires</strong>&nbsp;the seller to collect a higher rate of TCS (Higher the twice of applicable rate or 5%) in case buyer has not filed Income tax returns for preceding two previous years. However, this provision is not applicable if the aggregate of Tax deducted , collected at source is less than Rs.50,000 in each of the two previous years.</span></p>
<p>&nbsp;</p>
<p><span style='color: #0000ff;'>1)<strong>&nbsp;If dealers/ customers turnover for FY 2020-21 is greater than 10 Crores</strong>, Kindly deduct TDS at the applicable rate of&nbsp; 0.1% on the aggregate value of purchase exceeding Rs.50 Lakhs in the current financial year.</span></p>
<p><span style='color: #0000ff;'>Goldmedal Electricals Pvt. Ltd. has duly filed Income tax returns for&nbsp;FY 2018-19 and FY 2019-20 and hence&nbsp;<strong>TDS to be deducted at normal rates and not higher rates</strong>.&nbsp;</span></p>
<p><span style='color: #0000ff;'>2)&nbsp;<strong>If dealers/customers turnover&nbsp;</strong><strong>for FY 2020-21 is less than or equal to 10 Crores</strong>,&nbsp;&nbsp;Goldmedal will collect TCS at the applicable rate of 0.1.%&nbsp; as practice&nbsp;followed&nbsp;currently.</span></p>
<p>&nbsp;</p>
<p><span style='color: #0000ff;'>We request you to kindly fill the attached turnover declaration form<strong>&nbsp;(Unique Link Provided to each dealer )</strong>&nbsp;at the earliest by 27.06.2021.</span></p>
<p><span style='color: #0000ff;'><u>&nbsp;</u></span></p>
<p><span style='color: #0000ff;'>In case of any clarification / or any correspondence (if required), please reach out to us at the following coordinate</span></p>
<p><span style='color: #0000ff;'>Aruna Gupta &ndash;&nbsp;<a style='color: #0000ff;' href='mailto:aruna.gupta@goldmedalindia.com'>aruna.gupta@goldmedalindia.com</a>&nbsp;( 022-42023000 Extn:3071 )&nbsp;</span> &nbsp;&nbsp;&nbsp; &nbsp;</p>
";

            da.AddEmailQueue(1, EmailID, "DVS", "", "", Subject, MailBody, DateTime.Now.ToString(), "9999-12-01", 1, 1, 2017, "", "DVS Ex Mail", 1);
        }

        public void sendsms(string UniqueKey, string CustomerType, string CustType, string mobileno)
        {
          //  mobileno = "9518957760";
            string Link =  CustomerType;
           // string Link = "www.goggle.com";
            string smsBody = HttpUtility.UrlEncode("Dear Business Partner, As you are aware, Finance Act 2020 had introduced Section 206C(1H) w.e.f 01.10.2020 - Requirement to collect TCS on sale of goods at 0.1% on sales consideration exceeding Rs.50 Lakhs in a financial year. We request you to kindly fill the form (Link Provided) at the earliest. https://erp.goldmedalindia.in/TDSTCSapplicability.aspx?UniqueKey="+Link+" In case of any clarification (if required), please reach out to us at Aruna Gupta –aruna.gupta@goldmedalindia.com ( 022-42023000 Extn:3071 )-Team Goldmedal");

            string url = "http://sms6.rmlconnect.net:8080/bulksms/bulksms?username=" + WebConfigurationManager.AppSettings["SmsUser"].ToString() + "&password=" + WebConfigurationManager.AppSettings["SmsPassword"].ToString() + "&type=0&dlr=1&destination=" + mobileno + "&source=GLDMDL&message=" + smsBody + "&entityid=1601100000000001629&tempid=1007756728420877008";
            bufData = web.DownloadData(url);
         //   WebRequest request = HttpWebRequest.Create(url);
           // WebResponse response = request.GetResponse();


        }
    }
}