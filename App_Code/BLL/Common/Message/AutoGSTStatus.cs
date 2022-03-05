using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Web.Configuration;
using System.Xml;
/// <summary>
/// Summary description for AutoGSTStatus
/// </summary>
/// 
namespace Goldmedal.Emails.App_Code.BLL.Common.Message
{
    public class AutoGSTStatus : ITask
    {
        private int _maxTries = 5;
        private readonly IEmailTemplateDA da;
        private readonly IMessageService _messageService;
        private WebClient web = new WebClient();
        private byte[] bufData = null;


        public AutoGSTStatus()
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
            if (DateTime.Now.Day >= 1)
            {
                DataTable dt = _messageService.return_dt("GetPartyGSTStatus");
                if (dt.Rows.Count > 0)
                {

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        getstatus(dt.Rows[i]["GSTNo"].ToString(), dt.Rows[i]["key1"].ToString(), Convert.ToInt32(dt.Rows[i]["partyid"]), Convert.ToInt32(dt.Rows[i]["typecat"]));
                    }

                }
            }

        }
        public void getstatus(string gstno, string key, int partyid, int typecat)

        {

            var baseurldupicate = "https://gsp.adaequare.com/enriched/commonapi/search?action=TP&gstin=" + gstno;

            System.Net.HttpWebRequest request2 = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(baseurldupicate);

            // for production //
            request2.Method = "GET";
            request2.ContentType = "application/json";
            request2.Headers.Add("Authorization", "Bearer " + key);
            System.Net.HttpWebResponse response2 = (System.Net.HttpWebResponse)request2.GetResponse();
            if (response2.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var _output1 = JsonConvert.DeserializeObject<GstStatus>((new StreamReader(response2.GetResponseStream())).ReadToEnd());
                //  dynamic _output1 = JsonConvert.DeserializeObject((new StreamReader(response2.GetResponseStream())).ReadToEnd());

                if (_output1.success = true && _output1.message == "Search taxpayer is completed successfully")
                {


                    var t1 = string.IsNullOrEmpty(_output1.result.stjCd) ? "" : _output1.result.stjCd.ToString().Replace("'","");
                    var t2 = string.IsNullOrEmpty(_output1.result.lgnm) ? "" : _output1.result.lgnm.ToString().Replace("'", "");
                    var t3 = string.IsNullOrEmpty(_output1.result.stj) ? "" : _output1.result.stj.ToString().Replace("'", "");
                    var t4 = string.IsNullOrEmpty(_output1.result.dty) ? "" : _output1.result.dty.ToString().Replace("'", "");
                    var t5 = string.IsNullOrEmpty(_output1.result.cxdt) ? "" : _output1.result.cxdt.ToString().Replace("'", "");
                    var t6 = string.IsNullOrEmpty(_output1.result.gstin) ? "" : _output1.result.gstin.ToString().Replace("'", "");
                    var t7 = string.IsNullOrEmpty(_output1.result.lstupdt) ? "" : _output1.result.lstupdt.ToString().Replace("'", "");
                    var t8 = string.IsNullOrEmpty(_output1.result.rgdt) ? "" : _output1.result.rgdt.ToString().Replace("'", "");
                    var t9 = string.IsNullOrEmpty(_output1.result.ctb) ? "" : _output1.result.ctb.ToString().Replace("'", "");
                    var t10 = string.IsNullOrEmpty(_output1.result.tradeNam) ? "" : _output1.result.tradeNam.ToString().Replace("'", "");
                    var t11 = string.IsNullOrEmpty(_output1.result.sts) ? "" : _output1.result.sts.ToString().Replace("'", "");
                    var t12 = string.IsNullOrEmpty(_output1.result.ctjCd) ? "" : _output1.result.ctjCd.ToString().Replace("'", "");
                    var t13 = string.IsNullOrEmpty(_output1.result.ctj) ? "" : _output1.result.ctj.ToString().Replace("'", "");


                    var sqlinput = "UpdatePartyGSTStatus " + partyid + "," + typecat + ","
                                               + t1 + ","
                                            + t2 + ","
                                            + t3 + ","
                                            + t4 + ","
                                            + t5 + ","
                                            + t6 + ","
                                            + t7 + ","
                                            + t8 + ","
                                            + t9 + ","
                                            + t10 + ","
                                            + t11 + ","
                                            + t12 + ","
                                            + t13 ;


                    var dr6 = _messageService.return_dt("UpdatePartyGSTStatus " + partyid + "," + typecat + ",'"
                                               + t1 + "','"
                                            + t2 + "','"
                                            + t3 + "','"
                                            + t4 + "','"
                                            + t5 + "','"
                                            + t6 + "','"
                                            + t7 + "','"
                                            + t8 + "','"
                                            + t9 + "','"
                                            + t10 + "','"
                                            + t11 + "','"
                                            + t12 + "','"
                                            + t13 + "','"
                                            +sqlinput +"'"
                                                );
                }
                else
                {

                    var dr6 = _messageService.return_dt("updateInvalidgstno '" + gstno + "',"+typecat);

                }


            }
        }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Addr
    {
        public string bnm { get; set; }
        public string st { get; set; }
        public string loc { get; set; }
        public string bno { get; set; }
        public string dst { get; set; }
        public string stcd { get; set; }
        public string city { get; set; }
        public string flno { get; set; }
        public string lt { get; set; }
        public string pncd { get; set; }
        public string lg { get; set; }
    }

    public class Pradr
    {
        public Addr addr { get; set; }
        public string ntr { get; set; }
    }

    public class Result
    {
        public string stjCd { get; set; }
        public string dty { get; set; }
        public string lgnm { get; set; }
        public string stj { get; set; }
        public List<object> adadr { get; set; }
        public string cxdt { get; set; }
        public string gstin { get; set; }
        public List<string> nba { get; set; }
        public string lstupdt { get; set; }
        public string ctb { get; set; }
        public string rgdt { get; set; }
        public Pradr pradr { get; set; }
        public string ctjCd { get; set; }
        public string tradeNam { get; set; }
        public string sts { get; set; }
        public string ctj { get; set; }
    }

    public class GstStatus
    {
        public bool success { get; set; }
        public string message { get; set; }
        public Result result { get; set; }
        public int status { get; set; }
        public string errorCode { get; set; }
    }



}