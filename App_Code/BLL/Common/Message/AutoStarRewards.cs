using System;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

/// <summary>
/// Summary description for AutoStarRewards
/// </summary>
namespace Goldmedal.Emails.App_Code.BLL.Common.Message
{
    public class AutoStarRewards : ITask
    {
        private int _maxTries = 5;
        private readonly IMessageService _messageService;
        private readonly GoldLogging _gLog;
        private readonly IEmailTemplateDA da;
        private readonly IGoldMedia _goldMedia;
        private MessageService ms = new MessageService();

        private SqlDataReader rdr = null;

        private readonly string QUALIFYTEMPLATE = "Dear Channel Partner, " +
"Goldmedal congratulates you for achieving your target under the Rewards Scheme.You are assured of a bonus on last year's " +
"purchase and also on the additional purchase amount in FY 2018-19. But don't celebrate yet! There are still a few days left. " +
"You can earn an even higher bonus if you purchase more.So don't stop. Get the maximum benefit of the scheme." +
"                                                                                                             " +
"For more information please log-in to G-Parivaar app and check the Brand Loyalty Section.";

        private readonly string BOTHSHORTTEMPLATE = "We thank you for your strong support so far in financial year 2018-19." +
"As per Star Rewards scheme, if you achieve more sales of Rs. {0} for Wiring Devices and Rs.{1} in Wires & Other products " +
"in March 2019, you will be eligible for bonus on last year's purchase and also on the additional purchase amount. There's still " +
"time to achieve it! So put in your best efforts and reap the rewards!" +
"                                                                     " +
"Team Goldmedal";

        private readonly string WIRINGSHORTTEMPLATE = "We thank you for your strong support so far in financial year 2018-19." +
"As per Star Rewards scheme, if you achieve more sales of Rs. {0} for Wiring Devices " +
"in March 2019, you will be eligible for bonus on last year's purchase and also on the additional purchase amount. There's still " +
"time to achieve it! So put in your best efforts and reap the rewards!" +
"                                                                     " +
"Team Goldmedal";

        private readonly string OTHERSHORTTEMPLATE = "We thank you for your strong support so far in financial year 2018-19." +
"As per Star Rewards scheme, if you achieve more sales of  Rs. {0} in Wires & Other products " +
"in March 2019, you will be eligible for bonus on last year's purchase and also on the additional purchase amount. There's still " +
"time to achieve it! So put in your best efforts and reap the rewards!" +
"                                                                     " +
"Team Goldmedal";

        public AutoStarRewards()
        {
            _messageService = new MessageService();
            _gLog = new GoldLogging();
            da = new EmailTemplateDA();
            _goldMedia = new GoldMedia();
        }

        public void Execute(XmlNode node)
        {
            int TemplateId = 3018;

            XmlAttribute attribute1 = node.Attributes["maxTries"];
            if (attribute1 != null && !String.IsNullOrEmpty(attribute1.Value))
            {
                this._maxTries = int.Parse(attribute1.Value);
            }
            DealerSMS(TemplateId);
        }

        protected void DealerSMS(int DealerTemplateId)
        {
            DataTable dttemplate = da.GetMeassageTemplateByHeaderId(DealerTemplateId);
            if (dttemplate.Rows.Count > 0)
            {
                DateTime lastsend = DateTime.Now.Date;
                int flag = 0;
                DataTable dtActiveTime = da.GetActiveSchemeTimeID(DealerTemplateId);
                if (dtActiveTime.Rows.Count > 0)
                {
                    lastsend = Convert.ToDateTime(dtActiveTime.Rows[0]["lastSend"]);
                    flag = 0;
                }
                else
                {
                    lastsend = DateTime.Now.Date;
                    flag = 1;
                }
                int days = (DateTime.Now.Date - lastsend).Days;
                int interval = Convert.ToInt32(dttemplate.Rows[0]["interval"]);

                if (days >= interval || flag == 1)
                {
                    DataTable dt = _messageService.return_dt("exec smsStarReward");

                    if (dt.Rows.Count > 0)
                    {
                        //   ExcuativeMail(dt);
                        PartySMS(dt, DealerTemplateId);
                    }
                }
            }
        }

        protected void PartySMS(DataTable dt, int DealerTemplateId)
        {
            da.AddActiveScheme(DealerTemplateId);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (ms.ValidateMobileNo(dt.Rows[i]["mobile"].ToString()))
                {
                    var PartyName = dt.Rows[i]["name"].ToString();
                    var Mobile = dt.Rows[i]["mobile"].ToString();

                    var WDCurrentYearSale = Convert.ToInt32(dt.Rows[i]["WDCurrentYearSale"].ToString());
                    var WDLastYearSale = Convert.ToInt32(dt.Rows[i]["WDLastYearSale"].ToString());
                    //var WDBonus= Convert.ToInt32(dt.Rows[i]["lastyeawirebonus"].ToString());

                    var OWPCurrentYearSale = Convert.ToInt32(dt.Rows[i]["OWPCurrentYearSale"].ToString());
                    var OWPLatYearSale = Convert.ToInt32(dt.Rows[i]["OWPLatYearSale"].ToString());
                    //var OWPBonus = Convert.ToInt32(dt.Rows[i]["lastyeaotherwirebonus"].ToString());

                    FilterTemplate(DealerTemplateId, PartyName, Mobile,
                        WDCurrentYearSale, WDLastYearSale,
                        OWPCurrentYearSale, OWPLatYearSale);
                }
            }
        }

        private void FilterTemplate(int DealerTemplateId, string PartyName, string Mobile,
            int WDCurrentYearSale, int WDLastYearSale,
            int OWPCurrentYearSale, int OWPLatYearSale)
        {
            var isWDBonus = (WDCurrentYearSale - WDLastYearSale) > 0 && WDLastYearSale >= 2500000;
            var isOWPBonus = (OWPCurrentYearSale - OWPLatYearSale) > 0 && OWPLatYearSale >= 2500000;

            var notWDCalculate = WDLastYearSale < 2500000;
            var notOWPCalculate = OWPLatYearSale < 2500000;

            var WDShortSale = WDLastYearSale - WDCurrentYearSale;
            var OWPShortSale = OWPLatYearSale - OWPCurrentYearSale;

            var template = string.Empty;
            if ((isWDBonus && isOWPBonus)
                || (isWDBonus && notOWPCalculate)
                || (isOWPBonus && notWDCalculate))
            {
                template = QUALIFYTEMPLATE;
                SendSMS(DealerTemplateId, template, PartyName, Mobile);
                return;
            }
            else if (WDShortSale > 0 && !isWDBonus && OWPShortSale > 0 && !isOWPBonus && !notOWPCalculate && !notWDCalculate)
            {
                template = string.Format(BOTHSHORTTEMPLATE, WDShortSale, OWPShortSale);
                SendSMS(DealerTemplateId, template, PartyName, Mobile);
                return;
            }
            else if (OWPShortSale > 0 && !notOWPCalculate)
            {
                template = string.Format(OTHERSHORTTEMPLATE, OWPShortSale);
                SendSMS(DealerTemplateId, template, PartyName, Mobile);
                return;
            }
            else if (WDShortSale > 0 && !notWDCalculate)
            {
                template = string.Format(WIRINGSHORTTEMPLATE, WDShortSale);
                SendSMS(DealerTemplateId, template, PartyName, Mobile);
                return;
            }
        }

        private void SendSMS(int DealerTemplateId, string template, string partyName, string mobile)
        {
            var dt = new DataTable();
            dt = da.GetMeassageById(DealerTemplateId);

            if (dt.Rows.Count > 0)
            {
                da.AddMessageQueue(0, mobile, partyName, template, dt.Rows[0]["MessageTime"].ToString(), dt.Rows[0]["LastMessageDate"].ToString(), 0, 0, DealerTemplateId, "Star Reward", 1);
            }
        }
    }
}