using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Xml;

/// <summary>
/// Summary description for AutoInvoiceJson
/// </summary>
namespace Goldmedal.Emails.App_Code.BLL.Common.Message
{
    public class AutoInvoiceJson : ITask
    {
      

        private int _maxTries = 5;
        private readonly IMessageService _messageService;
        private readonly GoldLogging _gLog;
        private readonly IEmailTemplateDA da;
        private readonly IGoldMedia _goldMedia;
        private MessageService ms = new MessageService();

        private SqlDataReader rdr = null;

        public AutoInvoiceJson()
        {
            _messageService = new MessageService();
            _gLog = new GoldLogging();
            da = new EmailTemplateDA();
            _goldMedia = new GoldMedia();
        }

        public void Execute(XmlNode node)
        {
            int DealerTemplateId = 3025;
            int ExTemplateId = 3024;

            XmlAttribute attribute1 = node.Attributes["maxTries"];
            if (attribute1 != null && !String.IsNullOrEmpty(attribute1.Value))
            {
                this._maxTries = int.Parse(attribute1.Value);
            }
            createjson();
        }


        public void createjson()
        {
            string data;

            var dr = _messageService.return_dr("exec getdataforjson ");

            System.Collections.Generic.List<lst> quot = new System.Collections.Generic.List<lst>();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    quot.Add(new lst
                    {
                        InvoiceNo = Convert.ToString(dr["invoiceno"].ToString()),
                        InvoiceType = Convert.ToString(dr["invoicetype"].ToString()),
                        Invoicedate = Convert.ToString(dr["invoicedate"].ToString()),
                        PartyCategory = Convert.ToString(dr["PartyCategory"].ToString()),
                        PartyName = Convert.ToString(dr["PartyName"].ToString()),
                        SalesExecutive = Convert.ToString(dr["salesexname"].ToString()),
                        StateName = Convert.ToString(dr["statenm"].ToString()),
                        DivisionName = Convert.ToString(dr["divisionnm"].ToString()),
                        CategoryName = Convert.ToString(dr["categorynm"].ToString()),
                        SubCategory = Convert.ToString(dr["rangenm"].ToString()),
                        ItemCode = Convert.ToString(dr["ProductCode1"].ToString()),
                        HSNCode = Convert.ToString(dr["hsn"].ToString()),
                        ItemDescription = Convert.ToString(dr["ItemDescription"].ToString()),
                        TotalQuantity = Convert.ToString(dr["TotalQty"].ToString()),
                        FreeQuantity = Convert.ToString(dr["FreeQty"].ToString()),
                        PromoDisc = Convert.ToString(dr["PromoDiscper"].ToString()),
                        NetPrice = Convert.ToString(dr["NetPrice"].ToString()),
                        BasicAmount = Convert.ToString(dr["BasicAmount"].ToString()),
                        Discountper = Convert.ToString(dr["Discountper"].ToString()),
                        Discount = Convert.ToString(dr["Discount"].ToString()),
                        Taxper = Convert.ToString(dr["Taxper"].ToString()),
                        TaxAmount = Convert.ToString(dr["TaxAmount"].ToString()),
                        FinalAmount = Convert.ToString(dr["FinalAmount"].ToString()),
                        NewFinalAmount = Convert.ToString(dr["NewFinalAmount"].ToString()),
                        Warehouse = Convert.ToString(dr["warehouse"].ToString()),
                        City = Convert.ToString(dr["city"].ToString()),
                        PinNo = Convert.ToString(dr["topin"].ToString()),
                        ItemName = Convert.ToString(dr["ItemName"].ToString()),
                    });
                }


            }

            var output = JsonConvert.SerializeObject(quot, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(output));

            string FileName = "zoohinvoice";
            string uniquefoldernm = "zoho";
            // var retStr = _goldMedia.GoldMediaUpload(FileName, uniquefoldernm, ".json", stream, "application/json", false, false, true);
            var retStr = _goldMedia.GoldMediaUpload(FileName, uniquefoldernm, ".json", stream, "application/json", false, false, true);

        }





    }


    public class lst
    {

        public string InvoiceNo { get; set; }
        public string InvoiceType { get; set; }
        public string Invoicedate { get; set; }
        public string PartyCategory { get; set; }
        public string PartyName { get; set; }
        public string SalesExecutive { get; set; }
        public string StateName { get; set; }
        public string DivisionName { get; set; }
        public string CategoryName { get; set; }
        public string SubCategory { get; set; }
        public string ItemCode { get; set; }
        public string HSNCode { get; set; }
        public string ItemDescription { get; set; }
        public string TotalQuantity { get; set; }
        public string FreeQuantity { get; set; }
        public string PromoDisc { get; set; }
        public string NetPrice { get; set; }
        public string BasicAmount { get; set; }
        public string Discountper { get; set; }
        public string Discount { get; set; }
        public string Taxper { get; set; }
        public string TaxAmount { get; set; }
        public string FinalAmount { get; set; }
        public string NewFinalAmount { get; set; }
        public string Warehouse { get; set; }
        public string City { get; set; }
        public string ItemName { get; set; }
        public string PinNo { get; set; }

    }
}