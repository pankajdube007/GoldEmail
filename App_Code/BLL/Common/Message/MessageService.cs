using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;

/// <summary>
/// Summary description for MessageService
/// </summary>
public partial class MessageService : IMessageService
{
    #region Fields

    private readonly ICacheManager _cacheManager;
    private readonly ISettingManager _settingManager;
    private readonly IMessageDA _messageDA;
    private DataTable dt = new DataTable();
    private EmailTemplateDA da = new EmailTemplateDA();
    private readonly IGoldDataAccess _goldDataAccess;
    private readonly IGoldMedia _goldMedia;

    #endregion Fields

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="context">Object context</param>
    public MessageService()
    {
        this._cacheManager = new GoldRequestCache();
        this._settingManager = new SettingManager();
        this._messageDA = new MessageDA();
        this._goldDataAccess = new GoldDataAccess();
        _goldMedia = new GoldMedia();
    }

    #region Utilities

    private string Replace(string original, string pattern, string replacement)
    {
        //if (_settingManager.GetSettingValueBoolean("MessageTemplates.CaseInvariantReplacement"))
        //{
        int count = 0;
        int position0 = 0;
        int position1 = 0;
        string upperString = original.ToUpper();
        string upperPattern = pattern.ToUpper();
        int inc = (original.Length / pattern.Length) * (replacement.Length - pattern.Length);
        char[] chars = new char[original.Length + Math.Max(0, inc)];
        while ((position1 = upperString.IndexOf(upperPattern, position0)) != -1)
        {
            for (int i = position0; i < position1; ++i)
                chars[count++] = original[i];
            for (int i = 0; i < replacement.Length; ++i)
                chars[count++] = replacement[i];
            position0 = position1 + pattern.Length;
        }
        if (position0 == 0) return original;
        for (int i = position0; i < original.Length; ++i)
            chars[count++] = original[i];
        return new string(chars, 0, count);
        //}
        //else
        //{
        //    return original.Replace(pattern, replacement);
        //}
    }

    /// <summary>
    /// Replaces a message template tokens
    /// </summary>
    /// <param name="privateMessage">Private message</param>
    /// <param name="template">Template</param>
    /// <returns>New template</returns>
    private string ReplaceMessageTemplateTokens(PrivateMessage privateMessage, string template)
    {
        var tokens = new NameValueCollection();
        tokens.Add("Store.Name", _settingManager.StoreName);
        tokens.Add("Store.URL", _settingManager.StoreUrl);
        tokens.Add("Store.Email", this.DefaultEmailAccount.Email);

        tokens.Add("PrivateMessage.Subject", HttpUtility.HtmlEncode(privateMessage.Subject));
        //tokens.Add("PrivateMessage.Text", privateMessage.FormatPrivateMessageText());
        tokens.Add("PrivateMessage.Text", HttpUtility.HtmlEncode(privateMessage.Text));

        foreach (string token in tokens.Keys)
        {
            template = Replace(template, String.Format(@"%{0}%", token), tokens[token]);
        }

        return template;
    }

    public string ReplaceEmailTemplateCreateInvoice(string text, string template, CreateInvoice MailSMS)
    {
        var tokens = new NameValueCollection();

        tokens.Add("Branch.Name", MailSMS.BranchName);
        tokens.Add("Party.Name", MailSMS.PartyName);
        tokens.Add("Party.Type", MailSMS.PartyType);
        tokens.Add("Party.Code", MailSMS.PartyCode.ToString());
        tokens.Add("SaleEx.Name", MailSMS.SalesExName);
        tokens.Add("EmailID", MailSMS.EmailId);
        tokens.Add("MobileNo", MailSMS.Mobile);
        tokens.Add("Cin", MailSMS.cin);
        tokens.Add("State", MailSMS.State);
        tokens.Add("Area", MailSMS.Area);
        tokens.Add("City", MailSMS.City);
        tokens.Add("Vat.No", MailSMS.VatNo);
        tokens.Add("Cst.No", MailSMS.CstNo);
        tokens.Add("Order.No", MailSMS.OrderNo);
        tokens.Add("Order.Amount", MailSMS.OrderAmt);
        tokens.Add("Order.Date", MailSMS.OrderDt);
        tokens.Add("Dispatch.From", MailSMS.dispatchfrom);

        foreach (string token in tokens.Keys)
        {
            template = Replace(template, String.Format(@"%{0}%", token), tokens[token]);
        }

        return template;
    }

    public string ReplaceEmailTemplateBranchWiseActiveScheme(string text, string template, string MailSMS)
    {
        var tokens = new NameValueCollection();

        tokens.Add("Branch.Name", MailSMS);

        foreach (string token in tokens.Keys)
        {
            template = Replace(template, String.Format(@"%{0}%", token), tokens[token]);
        }

        return template;
    }

    public string ReplaceEmailTemplateLrReport(string text, string template, LrReport MailSMS)
    {
        var tokens = new NameValueCollection();

        tokens.Add("Branch.Name", MailSMS.BranchName);
        tokens.Add("Party.Name", MailSMS.PartyName);
        tokens.Add("Party.Type", MailSMS.PartyType);
        tokens.Add("Party.Code", MailSMS.PartyCode.ToString());
        tokens.Add("SaleEx.Name", MailSMS.SalesExName);

        tokens.Add("MobileNo", MailSMS.Mobile);
        tokens.Add("Cin", MailSMS.cin);
        tokens.Add("State", MailSMS.State);
        tokens.Add("Area", MailSMS.Area);
        tokens.Add("City", MailSMS.City);
        tokens.Add("Vat.No", MailSMS.VatNo);
        tokens.Add("Cst.No", MailSMS.CstNo);
        tokens.Add("LR.No", MailSMS.LrNo);
        tokens.Add("Order.Amount", MailSMS.OrderAmt);
        tokens.Add("LR.Date", MailSMS.LrDt);
        tokens.Add("Dispatch.From", MailSMS.dispatchfrom);
        tokens.Add("Traspoter.Name", MailSMS.TraspoterName);

        foreach (string token in tokens.Keys)
        {
            template = Replace(template, String.Format(@"%{0}%", token), tokens[token]);
        }

        return template;
    }

    public string ReplaceEmailTemplateSchemeUpload(string text, string template, SchemeUpload testmail)
    {
        // testmail test = new testmail();
        var tokens = new NameValueCollection();
        tokens.Add("Scheme.ID", testmail.SchemeID.ToString());
        tokens.Add("Scheme.Type", testmail.SchemeType.ToString());
        tokens.Add("Scheme.Name", testmail.SchemeName);
        tokens.Add("From.Date", testmail.FromDate);
        tokens.Add("To.Date", testmail.ToDate);
        tokens.Add("Remarks", testmail.Remark);
        tokens.Add("Branch.Name", testmail.BranchName);
        tokens.Add("Party.Name", testmail.PartyName);
        tokens.Add("Extra.Remarks", testmail.ExtraRemark);
        tokens.Add("Party.Type", testmail.PartyType);
        tokens.Add("SaleEx.Name", testmail.SalesExName);
        tokens.Add("EmailID", testmail.EmailId);
        tokens.Add("MobileNo", testmail.Mobile);
        tokens.Add("Cin", testmail.cin);
        tokens.Add("State", testmail.State);
        tokens.Add("Area", testmail.Area);
        tokens.Add("City", testmail.City);
        tokens.Add("Dealer.Name", testmail.Dealer);
        tokens.Add("Party.Address", testmail.FullAdd);
        tokens.Add("Today.Date", testmail.Date);

        foreach (string token in tokens.Keys)
        {
            template = Replace(template, String.Format(@"%{0}%", token), tokens[token]);
        }

        return template;
    }

    public string ReplaceEmailTemplateSchemeUploadSMS(string text, string template, ScemeUploadSMS sms)
    {
        // testmail test = new testmail();
        var tokens = new NameValueCollection();
        tokens.Add("Scheme.ID", sms.SchemeID.ToString());
        tokens.Add("Scheme.Type", sms.SchemeType.ToString());
        tokens.Add("Scheme.Name", sms.SchemeName);
        tokens.Add("From.Date", sms.FromDate);
        tokens.Add("To.Date", sms.FromDate);
        tokens.Add("Remark", sms.Remark);
        tokens.Add("Branch.Id", sms.BranchId);
        tokens.Add("Branch.Name", sms.BranchName);

        tokens.Add("Party.Name", sms.ToSMSName);

        foreach (string token in tokens.Keys)
        {
            template = Replace(template, String.Format(@"%{0}%", token), tokens[token]);
        }

        return template;
    }

    public string ReplaceEmailTemplatePartyWiseOutstanding(string text, string template, PartywiseOutstanding Data)
    {
        // testmail test = new testmail();
        var tokens = new NameValueCollection();
        tokens.Add("Party.Name", Data.PartyName);
        tokens.Add("Party.Type", Data.PartyType);
        tokens.Add("Branch.Name", Data.HomeBranch);
        tokens.Add("SaleEx.Name", Data.SaleExName);
        tokens.Add("Extra.Remarks", Data.ExtraRemarks);
        tokens.Add("EmailID", Data.Email);
        tokens.Add("MobileNo", Data.Mobile);
        tokens.Add("Cin", Data.cin);
        tokens.Add("State", Data.State);
        tokens.Add("Area", Data.Area);
        tokens.Add("City", Data.City);
        tokens.Add("Outstanding.Details", Data.OutstandingDetails);
        tokens.Add("Party.Code", Data.PartyCode.ToString());
        tokens.Add("Cst.No", Data.CstNo);
        tokens.Add("Vat.No", Data.VatNo);
        tokens.Add("Dealer.Name", Data.Dealer);
        tokens.Add("Party.Address", Data.FullAdd);
        tokens.Add("Today.Date", Data.Date.ToString());

        foreach (string token in tokens.Keys)
        {
            template = Replace(template, String.Format(@"%{0}%", token), tokens[token]);
        }

        return template;
    }

    public string ReplaceEmailTemplatePartyWiseCDInvoice(string text, string template, PartyWiseCDInvoice Data)
    {
        // testmail test = new testmail();
        var tokens = new NameValueCollection();
        tokens.Add("Party.Name", Data.PartyName);
        tokens.Add("Party.Type", Data.PartyType);
        tokens.Add("Branch.Name", Data.HomeBranch);
        tokens.Add("SaleEx.Name", Data.SaleExName);
        tokens.Add("Extra.Remarks", Data.ExtraRemarks);
        tokens.Add("EmailID", Data.Email);
        tokens.Add("MobileNo", Data.Mobile);
        tokens.Add("Cin", Data.cin);
        tokens.Add("State", Data.State);
        tokens.Add("Area", Data.Area);
        tokens.Add("City", Data.City);
        //  tokens.Add("CD.Details", Data.CDDetails.ToString());
        tokens.Add("Party.Code", Data.PartyCode.ToString());
        tokens.Add("Cst.No", Data.CstNo);
        tokens.Add("Vat.No", Data.VatNo);
        tokens.Add("Dealer.Name", Data.Dealer);
        tokens.Add("Party.Address", Data.FullAdd);
        tokens.Add("Today.Date", Data.Date.ToString());

        foreach (string token in tokens.Keys)
        {
            template = Replace(template, String.Format(@"%{0}%", token), tokens[token]);
        }

        template = template.Replace(@"%CD.Details%", Data.CDDetails);

        return template;
    }

    public string ReplaceTemplatePartyWiseLedger(string text, string template, PartywiseLedger Data)
    {
        // testmail test = new testmail();
        var tokens = new NameValueCollection();
        tokens.Add("Party.Name", Data.PartyName);
        tokens.Add("Party.Type", Data.PartyType);
        tokens.Add("Branch.Name", Data.HomeBranch);
        tokens.Add("SaleEx.Name", Data.SaleExName);
        tokens.Add("Extra.Remarks", Data.ExtraRemarks);
        tokens.Add("EmailID", Data.Email);
        tokens.Add("MobileNo", Data.Mobile);
        tokens.Add("Cin", Data.cin);
        tokens.Add("State", Data.State);
        tokens.Add("Area", Data.Area);
        tokens.Add("City", Data.City);
        tokens.Add("Cst.No", Data.CstNo);
        tokens.Add("Vat.No", Data.VatNo);
        tokens.Add("Party.Code", Data.PartyCode.ToString());
        tokens.Add("Dealer.Name", Data.Dealer);
        tokens.Add("Party.Address", Data.FullAdd);
        tokens.Add("Today.Date", Data.Date.ToString());

        foreach (string token in tokens.Keys)
        {
            template = Replace(template, String.Format(@"%{0}%", token), tokens[token]);
        }

        return template;
    }

    public string ReplaceTemplateTicketBookingCancel(string text, string template, TicketBookingCancel Data)
    {
        // testmail test = new testmail();
        var tokens = new NameValueCollection();
        tokens.Add("Refund.Amount", Data.RefundAmount);
        tokens.Add("Cancel.Date", Data.CancelDate);
        tokens.Add("Booking.No", Data.BookingNo);
        tokens.Add("ApprovalPageLink", Data.ApprovalPageLink);
        foreach (string token in tokens.Keys)
        {
            template = Replace(template, String.Format(@"%{0}%", token), tokens[token]);
        }

        return template;
    }

    public string ReplaceTemplateTicketBooking(string text, string template, TicketBooking Data)
    {
        // testmail test = new testmail();
        var tokens = new NameValueCollection();
        tokens.Add("Booking.Amount", Data.BookingAmount);
        tokens.Add("Booking.Type", Data.BookingType);
        tokens.Add("Tour.Operator,", Data.TourOperator);
        tokens.Add("Travel.ToDate", Data.TravelToDate);
        tokens.Add("Travel.FromDate", Data.TravelFromDate);
        tokens.Add("Booking.No", Data.BookingNo);
        tokens.Add("Source.Place", Data.SourcePlace);
        tokens.Add("Destination.Place", Data.DestinationPlace);
        tokens.Add("ApprovalPageLink", Data.ApprovalPageLink);
        tokens.Add("Booking.Date", Data.BookingDate);
        foreach (string token in tokens.Keys)
        {
            template = Replace(template, String.Format(@"%{0}%", token), tokens[token]);
        }

        return template;
    }

    public string ReplaceTemplatePartyWisePendingDetails(string text, string template, PendingDetails Data)
    {
        // testmail test = new testmail();
        var tokens = new NameValueCollection();
        tokens.Add("Party.Name", Data.PartyName);
        tokens.Add("Party.Type", Data.PartyType);
        tokens.Add("Branch.Name", Data.HomeBranch);
        tokens.Add("SaleEx.Name", Data.SaleExName);
        tokens.Add("Extra.Remarks", Data.ExtraRemarks);
        tokens.Add("EmailID", Data.Email);
        tokens.Add("MobileNo", Data.Mobile);
        tokens.Add("Cin", Data.cin);
        tokens.Add("State", Data.State);
        tokens.Add("Area", Data.Area);
        tokens.Add("City", Data.City);
        tokens.Add("Party.Code", Data.PartyCode.ToString());
        tokens.Add("Today.Date", Data.Date.ToString());

        foreach (string token in tokens.Keys)
        {
            template = Replace(template, String.Format(@"%{0}%", token), tokens[token]);
        }

        return template;
    }

    public string ReplaceTemplateChequeReturnApproval(string text, string template, ChequeReturnApproval MailSMS)
    {
        var tokens = new NameValueCollection();
        tokens.Add("Branch.Name", MailSMS.BranchName);
        tokens.Add("Party.Name", MailSMS.PartyName);
        tokens.Add("Party.Type", MailSMS.PartyType);
        tokens.Add("Party.Code", MailSMS.PartyCode.ToString());
        tokens.Add("SaleEx.Name", MailSMS.SalesExName);
        tokens.Add("EmailID", MailSMS.EmailId);
        tokens.Add("MobileNo", MailSMS.Mobile);
        tokens.Add("Cin", MailSMS.cin);
        tokens.Add("State", MailSMS.State);
        tokens.Add("Area", MailSMS.Area);
        tokens.Add("City", MailSMS.City);
        tokens.Add("Vat.No", MailSMS.VatNo);
        tokens.Add("Cst.No", MailSMS.CstNo);
        tokens.Add("Order.Amount", MailSMS.OrderAmt);
        tokens.Add("Order.Date", MailSMS.OrderDt);
        tokens.Add("Cheque.No", MailSMS.ChequeNo);
        tokens.Add("Cheque.Amount", MailSMS.ChequeAmt);
        tokens.Add("Narration", MailSMS.Narration);

        foreach (string token in tokens.Keys)
        {
            template = Replace(template, String.Format(@"%{0}%", token), tokens[token]);
        }

        return template;
    }

    public string ReplaceTemplateCreditNote(string text, string template, CreditNote MailSMS)
    {
        var tokens = new NameValueCollection();

        tokens.Add("Party.Name", MailSMS.PartyName);
        tokens.Add("Agent.Name", MailSMS.AgentName);
        tokens.Add("Party.Address", MailSMS.FullAddress);

        foreach (string token in tokens.Keys)
        {
            template = Replace(template, String.Format(@"%{0}%", token), tokens[token]);
        }

        return template;
    }
    //ToDO Santanu
    public string ReplaceTemplateLoyaltyPoint(string text, string template, LoyaltyPoint MailSMS)
    {
        var tokens = new NameValueCollection();

        tokens.Add("Party.Name", MailSMS.PartyName);
        tokens.Add("Loyalty.Point", MailSMS.LoyaltyPoints);
        tokens.Add("Fin.Year", MailSMS.FYear);
        tokens.Add("AsOn.Date", MailSMS.AsonDate);
       
        foreach (string token in tokens.Keys)
        {
            template = Replace(template, String.Format(@"%{0}%", token), tokens[token]);
        }

        return template;
    }
    //TODO Currently we don't need this method because we set the message template in calling method as const by Santanu on 19-03-2019
    public string ReplaceTemplateStarRewards(string text, string template, StarRewards MailSMS)
    {
        var tokens = new NameValueCollection();

        tokens.Add("Party.Name", MailSMS.PartyName);
        tokens.Add("Wiring.Device.CurrentYearSale", MailSMS.CurrentYearSale);
        tokens.Add("Wiring.Device.LastYearSale", MailSMS.LastYearSale);
        tokens.Add("Wiring.Device.Bonus.1percentage", MailSMS.onepercentage);
        //tokens.Add("Wiring.Device.BonusDifference.2percentage", MailSMS.twopercentage);
        tokens.Add("Wiring.Lights.Pipes.CurrentYearSale", MailSMS.wlpCurrentYearSale);
        tokens.Add("Wiring.Lights.Pipes.LastYearSale", MailSMS.wlpLastYearSale);
        tokens.Add("Wiring.Lights.Pipes.Bonus.5percentage", MailSMS.fivepercentage);
        //tokens.Add("Wiring.Lights.Pipes.BonusDifference1percentage", MailSMS.BonusDifference1percentage);

        foreach (string token in tokens.Keys)
        {
            template = Replace(template, String.Format(@"%{0}%", token), tokens[token]);
        }

        return template;
    }

    public string ReplaceTemplateAutoInvoice(string text, string template, EveryDayInvoice MailSMS)
    {
        var tokens = new NameValueCollection();

        tokens.Add("Party.Name", MailSMS.PartyName);
        tokens.Add("SaleEx.Name", MailSMS.ExName);
        tokens.Add("Cin", MailSMS.cin);
        tokens.Add("Today.Date", DateTime.Now.Date.ToString("dd-MM-yyyy"));

        foreach (string token in tokens.Keys)
        {
            template = Replace(template, String.Format(@"%{0}%", token), tokens[token]);
        }

        return template;
    }

    public string ReplaceTemplateCustomerRecieptApproval(string text, string template, CustomerRecieptApproval MailSMS)
    {
        var tokens = new NameValueCollection();
        tokens.Add("Branch.Name", MailSMS.BranchName);
        tokens.Add("Party.Name", MailSMS.PartyName);
        tokens.Add("Party.Type", MailSMS.PartyType);
        tokens.Add("Party.Code", MailSMS.PartyCode.ToString());
        tokens.Add("SaleEx.Name", MailSMS.SalesExName);
        tokens.Add("EmailID", MailSMS.EmailId);
        tokens.Add("MobileNo", MailSMS.Mobile);
        tokens.Add("Cin", MailSMS.cin);
        tokens.Add("State", MailSMS.State);
        tokens.Add("Area", MailSMS.Area);
        tokens.Add("City", MailSMS.City);
        tokens.Add("Vat.No", MailSMS.VatNo);
        tokens.Add("Cst.No", MailSMS.CstNo);
        tokens.Add("Order.Amount", MailSMS.OrderAmt);
        tokens.Add("Order.Date", MailSMS.OrderDt);
        tokens.Add("Cheque.No", MailSMS.ChequeNo);
        tokens.Add("Cheque.Amount", MailSMS.ChequeAmt);
        tokens.Add("Instrument.Type", MailSMS.instrumenttype);

        foreach (string token in tokens.Keys)
        {
            template = Replace(template, String.Format(@"%{0}%", token), tokens[token]);
        }

        return template;
    }

    public string ReplaceTemplatePOApproval(string text, string template, POApproval MailSMS)
    {
        var tokens = new NameValueCollection();
        tokens.Add("Branch.Name", MailSMS.BranchName);
        tokens.Add("Party.Name", MailSMS.PartyName);
        tokens.Add("Party.Type", MailSMS.PartyType);
        tokens.Add("Party.Code", MailSMS.PartyCode.ToString());
        tokens.Add("SaleEx.Name", MailSMS.SalesExName);
        tokens.Add("EmailID", MailSMS.EmailId);
        tokens.Add("MobileNo", MailSMS.Mobile);
        tokens.Add("Cin", MailSMS.cin);
        tokens.Add("State", MailSMS.State);
        tokens.Add("Area", MailSMS.Area);
        tokens.Add("City", MailSMS.City);
        tokens.Add("Vat.No", MailSMS.VatNo);
        tokens.Add("Cst.No", MailSMS.CstNo);
        tokens.Add("Order.No", MailSMS.OrderNo);
        tokens.Add("Order.Amount", MailSMS.OrderAmt);
        tokens.Add("Order.Date", MailSMS.OrderDt);
        tokens.Add("Dispatch.From", MailSMS.dispatchfrom);

        foreach (string token in tokens.Keys)
        {
            template = Replace(template, String.Format(@"%{0}%", token), tokens[token]);
        }

        return template;
    }

    //public string ReplaceTemplateChequeReturnApprovalParty(string text, string template, ChequeReturnApproval Data)
    //{
    //    var tokens = new NameValueCollection();
    //    tokens.Add("ChequeReturn-PartyChequeNo", Data.ChequeNo);
    //    tokens.Add("ChequeReturn-PartyChequeAmt", Data.ChequeAmt);
    //    tokens.Add("ChequeReturn-PartyChequeNarration", Data.Narration);

    //    foreach (string token in tokens.Keys)
    //    {
    //        template = Replace(template, String.Format(@"%{0}%", token), tokens[token]);
    //    }

    //    return template;
    //}

    //public string ReplaceTemplateChequeReturnApprovalAgent(string text, string template, ChequeReturnApproval Data)
    //{
    //    var tokens = new NameValueCollection();
    //    tokens.Add("ChequeReturn-AgentPartyName", Data.PartyName);
    //   // tokens.Add("ChequeReturn-AgentPartyCode", Data.Code.ToString());
    //    tokens.Add("ChequeReturn-AgentPartyOrderAmt", Data.OrderAmt);
    //    tokens.Add("ChequeReturn-AgentPartyOrderDt", Data.OrderDt);

    //    foreach (string token in tokens.Keys)
    //    {
    //        template = Replace(template, String.Format(@"%{0}%", token), tokens[token]);
    //    }

    //    return template;
    //}

    #endregion Utilities

    #region Methods

    #region Repository methods

    /// <summary>
    /// Gets a message template by template identifier
    /// </summary>
    /// <param name="messageTemplateId">Message template identifier</param>
    /// <returns>Message template</returns>
    public MessageTemplate GetMessageTemplateById(int messageTemplateId)
    {
        if (messageTemplateId == 0)
            return null;

        var message = _messageDA.GetMessageTemplateDataByID(messageTemplateId);
        if (message.Rows.Count > 0)
        {
            return new MessageTemplate()
            {
                MessageTemplateId = Convert.ToInt32(message.Rows[0]["MessageTemplateId"]),
                Name = Convert.ToString(message.Rows[0]["Name"]),
            };
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Gets all message templates
    /// </summary>
    /// <returns>Message template collection</returns>
    public List<MessageTemplate> GetAllMessageTemplates()
    {
        List<MessageTemplate> _listMessageTemplate = new List<MessageTemplate>();

        var message = _messageDA.GetMessageTemplateDataList();
        for (var i = 0; i < message.Rows.Count; i++)
        {
            _listMessageTemplate.Add(new MessageTemplate()
            {
                MessageTemplateId = Convert.ToInt32(message.Rows[i]["MessageTemplateId"]),
                Name = Convert.ToString(message.Rows[i]["Name"]),
            });
        }
        return _listMessageTemplate;
    }

    /// <summary>
    /// Gets a localized message template by identifier
    /// </summary>
    /// <param name="localizedMessageTemplateId">Localized message template identifier</param>
    /// <returns>Localized message template</returns>
    public LocalizedMessageTemplate GetLocalizedMessageTemplateById(int localizedMessageTemplateId)
    {
        if (localizedMessageTemplateId == 0)
            return null;

        var localizedMessageTemplate = _messageDA.GetLocalizedMessageTemplateDataByID(localizedMessageTemplateId);
        if (localizedMessageTemplate.Rows.Count > 0)
        {
            return new LocalizedMessageTemplate()
            {
                MessageTemplateLocalizedId = Convert.ToInt32(localizedMessageTemplate.Rows[0]["MessageTemplateLocalizedId"]),
                MessageTemplateId = Convert.ToInt32(localizedMessageTemplate.Rows[0]["MessageTemplateId"]),
                BccEmailAddresses = Convert.ToString(localizedMessageTemplate.Rows[0]["BccEmailAddresses"]),
                Subject = Convert.ToString(localizedMessageTemplate.Rows[0]["Subject"]),
                Body = Convert.ToString(localizedMessageTemplate.Rows[0]["Body"]),
                IsActive = Convert.ToBoolean(localizedMessageTemplate.Rows[0]["IsActive"]),
                EmailAccountId = Convert.ToInt32(localizedMessageTemplate.Rows[0]["EmailAccountId"]),
                //Name = Convert.ToString(localizedMessageTemplate.Rows[0]["Name"]),
            };
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Deletes a localized message template
    /// </summary>
    /// <param name="localizedMessageTemplateId">Message template identifier</param>
    public void DeleteLocalizedMessageTemplate(int localizedMessageTemplateId)
    {
        var localizedMessageTemplate = GetLocalizedMessageTemplateById(localizedMessageTemplateId);
        if (localizedMessageTemplate == null)
            return;

        //implement update here
    }

    /// <summary>
    /// Gets all localized message templates
    /// </summary>
    /// <param name="messageTemplateName">Message template name</param>
    /// <returns>Localized message template collection</returns>
    public List<LocalizedMessageTemplate> GetAllLocalizedMessageTemplates(string messageTemplateName)
    {
        List<LocalizedMessageTemplate> _listMessageTemplate = new List<LocalizedMessageTemplate>();

        var message = _messageDA.GetLocalizedMessageTemplateDataByName(messageTemplateName);
        for (var i = 0; i < message.Rows.Count; i++)
        {
            _listMessageTemplate.Add(new LocalizedMessageTemplate()
            {
                MessageTemplateLocalizedId = Convert.ToInt32(message.Rows[i]["MessageTemplateLocalizedId"]),
                MessageTemplateId = Convert.ToInt32(message.Rows[i]["MessageTemplateId"]),
                BccEmailAddresses = Convert.ToString(message.Rows[i]["BccEmailAddresses"]),
                Subject = Convert.ToString(message.Rows[i]["Subject"]),
                Body = Convert.ToString(message.Rows[i]["Body"]),
                IsActive = Convert.ToBoolean(message.Rows[i]["IsActive"]),
                EmailAccountId = Convert.ToInt32(message.Rows[i]["EmailAccountId"]),
                Name = Convert.ToString(message.Rows[i]["Name"]),
            });
        }
        return _listMessageTemplate;
    }

    /// <summary>
    /// Inserts a localized message template
    /// </summary>
    /// <param name="localizedMessageTemplate">Localized message template</param>
    public void InsertLocalizedMessageTemplate(LocalizedMessageTemplate localizedMessageTemplate)
    {
        if (localizedMessageTemplate == null)
            throw new ArgumentNullException("localizedMessageTemplate");

        localizedMessageTemplate.BccEmailAddresses = CommonHelper.EnsureNotNull(localizedMessageTemplate.BccEmailAddresses);
        localizedMessageTemplate.BccEmailAddresses = CommonHelper.EnsureMaximumLength(localizedMessageTemplate.BccEmailAddresses, 200);
        localizedMessageTemplate.Subject = CommonHelper.EnsureNotNull(localizedMessageTemplate.Subject);
        localizedMessageTemplate.Subject = CommonHelper.EnsureMaximumLength(localizedMessageTemplate.Subject, 200);
        localizedMessageTemplate.Body = CommonHelper.EnsureNotNull(localizedMessageTemplate.Body);

        //implement update here
    }

    /// <summary>
    /// Updates the localized message template
    /// </summary>
    /// <param name="localizedMessageTemplate">Localized message template</param>
    public void UpdateLocalizedMessageTemplate(LocalizedMessageTemplate localizedMessageTemplate)
    {
        if (localizedMessageTemplate == null)
            throw new ArgumentNullException("localizedMessageTemplate");

        localizedMessageTemplate.BccEmailAddresses = CommonHelper.EnsureNotNull(localizedMessageTemplate.BccEmailAddresses);
        localizedMessageTemplate.BccEmailAddresses = CommonHelper.EnsureMaximumLength(localizedMessageTemplate.BccEmailAddresses, 200);
        localizedMessageTemplate.Subject = CommonHelper.EnsureNotNull(localizedMessageTemplate.Subject);
        localizedMessageTemplate.Subject = CommonHelper.EnsureMaximumLength(localizedMessageTemplate.Subject, 200);
        localizedMessageTemplate.Body = CommonHelper.EnsureNotNull(localizedMessageTemplate.Body);

        //implement update here
    }

    /// <summary>
    /// Gets a queued email by identifier
    /// </summary>
    /// <param name="queuedEmailId">Email item identifier</param>
    /// <returns>Email item</returns>
    public QueuedEmail GetQueuedEmailById(int queuedEmailId)
    {
        if (queuedEmailId == 0)
            return null;

        //implement update here
        return null;
    }

    /// <summary>
    /// Deletes a queued email
    /// </summary>
    /// <param name="queuedEmailId">Email item identifier</param>
    public void DeleteQueuedEmail(int queuedEmailId)
    {
        var queuedEmail = GetQueuedEmailById(queuedEmailId);
        if (queuedEmail == null)
            return;

        //implement update here
    }

    /// <summary>
    /// Gets all queued emails
    /// </summary>
    /// <param name="queuedEmailCount">Email item count. 0 if you want to get all items</param>
    /// <param name="loadNotSentItemsOnly">A value indicating whether to load only not sent emails</param>
    /// <param name="maxSendTries">Maximum send tries</param>
    /// <returns>Email item collection</returns>
    public List<QueuedEmail> GetAllQueuedEmails(int queuedEmailCount, bool loadNotSentItemsOnly, int maxSendTries)
    {
        return GetAllQueuedEmails(string.Empty, string.Empty, null, null, queuedEmailCount, loadNotSentItemsOnly, maxSendTries);
    }

    /// <summary>
    /// Gets all queued emails
    /// </summary>
    /// <param name="fromEmail">From Email</param>
    /// <param name="toEmail">To Email</param>
    /// <param name="startTime">The start time</param>
    /// <param name="endTime">The end time</param>
    /// <param name="queuedEmailCount">Email item count. 0 if you want to get all items</param>
    /// <param name="loadNotSentItemsOnly">A value indicating whether to load only not sent emails</param>
    /// <param name="maxSendTries">Maximum send tries</param>
    /// <returns>Email item collection</returns>
    public List<QueuedEmail> GetAllQueuedEmails(string fromEmail, string toEmail, DateTime? startTime, DateTime? endTime, int queuedEmailCount, bool loadNotSentItemsOnly, int maxSendTries)
    {
        if (fromEmail == null)
            fromEmail = string.Empty;
        fromEmail = fromEmail.Trim();

        if (toEmail == null)
            toEmail = string.Empty;
        toEmail = toEmail.Trim();

        //var query = (IQueryable<QueuedEmail>)_context.QueuedEmails;
        //if (!String.IsNullOrEmpty(fromEmail))
        //    query = query.Where(qe => qe.From.Contains(fromEmail));
        //if (!String.IsNullOrEmpty(toEmail))
        //    query = query.Where(qe => qe.To.Contains(toEmail));
        //if (startTime.HasValue)
        //    query = query.Where(qe => startTime.Value <= qe.CreatedOn);
        //if (endTime.HasValue)
        //    query = query.Where(qe => endTime.Value >= qe.CreatedOn);
        //if (loadNotSentItemsOnly)
        //    query = query.Where(qe => !qe.SentOn.HasValue);
        //query = query.Where(qe => qe.SendTries < maxSendTries);
        //if (queuedEmailCount > 0)
        //    query = query.Take(queuedEmailCount);
        //query = query.OrderByDescending(qe => qe.Priority).ThenBy(qe => qe.CreatedOn);

        //var queuedEmails = query.ToList();

        //return queuedEmails;

        //implement update here
        return null;
    }

    /// <summary>
    /// Inserts a queued email
    /// </summary>
    /// <param name="priority">The priority</param>
    /// <param name="from">From</param>
    /// <param name="to">To</param>
    /// <param name="cc">CC</param>
    /// <param name="bcc">BCC</param>
    /// <param name="subject">Subject</param>
    /// <param name="body">Body</param>
    /// <param name="createdOn">The date and time of item creation</param>
    /// <param name="sendTries">The send tries</param>
    /// <param name="sentOn">The sent date and time. Null if email is not sent yet</param>
    /// <param name="emailAccountId">Email account identifer</param>
    /// <returns>Queued email</returns>
    public QueuedEmail InsertQueuedEmail(int priority, MailAddress from, MailAddress to, string cc, string bcc, string subject, string body, DateTime createdOn, int sendTries, DateTime? sentOn, int emailAccountId)
    {
        return InsertQueuedEmail(priority, from.Address, from.DisplayName, to.Address, to.DisplayName, cc, bcc, subject, body, createdOn, sendTries, sentOn, emailAccountId);
    }

    /// <summary>
    /// Inserts a queued email
    /// </summary>
    /// <param name="priority">The priority</param>
    /// <param name="from">From</param>
    /// <param name="fromName">From name</param>
    /// <param name="to">To</param>
    /// <param name="toName">To name</param>
    /// <param name="cc">Cc</param>
    /// <param name="bcc">Bcc</param>
    /// <param name="subject">Subject</param>
    /// <param name="body">Body</param>
    /// <param name="createdOn">The date and time of item creation</param>
    /// <param name="sendTries">The send tries</param>
    /// <param name="sentOn">The sent date and time. Null if email is not sent yet</param>
    /// <param name="emailAccountId">Email account identifer</param>
    /// <returns>Queued email</returns>
    public QueuedEmail InsertQueuedEmail(int priority, string from, string fromName, string to, string toName, string cc, string bcc, string subject, string body, DateTime createdOn, int sendTries, DateTime? sentOn, int emailAccountId)
    {
        from = CommonHelper.EnsureNotNull(from);
        from = CommonHelper.EnsureMaximumLength(from, 500);
        fromName = CommonHelper.EnsureNotNull(fromName);
        fromName = CommonHelper.EnsureMaximumLength(fromName, 500);
        to = CommonHelper.EnsureNotNull(to);
        to = CommonHelper.EnsureMaximumLength(to, 500);
        toName = CommonHelper.EnsureNotNull(toName);
        toName = CommonHelper.EnsureMaximumLength(toName, 500);
        cc = CommonHelper.EnsureNotNull(cc);
        cc = CommonHelper.EnsureMaximumLength(cc, 500);
        bcc = CommonHelper.EnsureNotNull(bcc);
        bcc = CommonHelper.EnsureMaximumLength(bcc, 500);
        subject = CommonHelper.EnsureNotNull(subject);
        subject = CommonHelper.EnsureMaximumLength(subject, 500);
        body = CommonHelper.EnsureNotNull(body);

        //implement update here
        return null;
    }

    /// <summary>
    /// Updates a queued email
    /// </summary>
    /// <param name="queuedEmail">Queued email</param>
    public void UpdateQueuedEmail(QueuedEmail queuedEmail)
    {
        if (queuedEmail == null)
            throw new ArgumentNullException("queuedEmail");

        queuedEmail.From = CommonHelper.EnsureNotNull(queuedEmail.From);
        queuedEmail.From = CommonHelper.EnsureMaximumLength(queuedEmail.From, 500);
        queuedEmail.FromName = CommonHelper.EnsureNotNull(queuedEmail.FromName);
        queuedEmail.FromName = CommonHelper.EnsureMaximumLength(queuedEmail.FromName, 500);
        queuedEmail.To = CommonHelper.EnsureNotNull(queuedEmail.To);
        queuedEmail.To = CommonHelper.EnsureMaximumLength(queuedEmail.To, 500);
        queuedEmail.ToName = CommonHelper.EnsureNotNull(queuedEmail.ToName);
        queuedEmail.ToName = CommonHelper.EnsureMaximumLength(queuedEmail.ToName, 500);
        queuedEmail.CC = CommonHelper.EnsureNotNull(queuedEmail.CC);
        queuedEmail.CC = CommonHelper.EnsureMaximumLength(queuedEmail.CC, 500);
        queuedEmail.Bcc = CommonHelper.EnsureNotNull(queuedEmail.Bcc);
        queuedEmail.Bcc = CommonHelper.EnsureMaximumLength(queuedEmail.Bcc, 500);
        queuedEmail.Subject = CommonHelper.EnsureNotNull(queuedEmail.Subject);
        queuedEmail.Subject = CommonHelper.EnsureMaximumLength(queuedEmail.Subject, 500);
        queuedEmail.Body = CommonHelper.EnsureNotNull(queuedEmail.Body);

        //implement update here
    }

    /// <summary>
    /// Gets a email account by identifier
    /// </summary>
    /// <param name="emailAccountId">The email account identifier</param>
    /// <returns>Email account</returns>
    public EmailAccount GetEmailAccountById(int emailAccountId)
    {
        if (emailAccountId == 0)
            return null;

        var email = _messageDA.GetEmailAccountDataByID(emailAccountId);
        if (email.Rows.Count > 0)
        {
            return new EmailAccount()
            {
                EmailAccountId = Convert.ToInt32(email.Rows[0]["EmailAccountId"]),
                Email = Convert.ToString(email.Rows[0]["Email"]),
                DisplayName = Convert.ToString(email.Rows[0]["DisplayName"]),
                Host = Convert.ToString(email.Rows[0]["Host"]),
                Port = Convert.ToInt32(email.Rows[0]["Port"]),
                Username = Convert.ToString(email.Rows[0]["Username"]),
                Password = Convert.ToString(email.Rows[0]["Password"]),
                EnableSSL = Convert.ToBoolean(email.Rows[0]["EnableSSL"]),
                UseDefaultCredentials = Convert.ToBoolean(email.Rows[0]["UseDefaultCredentials"]),
            };
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Deletes the email account
    /// </summary>
    /// <param name="emailAccountId">The email account identifier</param>
    public void DeleteEmailAccount(int emailAccountId)
    {
        var emailAccount = GetEmailAccountById(emailAccountId);
        if (emailAccount == null)
            return;

        if (GetAllEmailAccounts().Count == 1)
            throw new GoldException("You cannot delete this email account. At least one account is required.");

        //implement update here
    }

    /// <summary>
    /// Inserts an email account
    /// </summary>
    /// <param name="emailAccount">Email account</param>
    public void InsertEmailAccount(EmailAccount emailAccount)
    {
        if (emailAccount == null)
            throw new ArgumentNullException("emailAccount");

        emailAccount.Email = CommonHelper.EnsureNotNull(emailAccount.Email);
        emailAccount.DisplayName = CommonHelper.EnsureNotNull(emailAccount.DisplayName);
        emailAccount.Host = CommonHelper.EnsureNotNull(emailAccount.Host);
        emailAccount.Username = CommonHelper.EnsureNotNull(emailAccount.Username);
        emailAccount.Password = CommonHelper.EnsureNotNull(emailAccount.Password);

        emailAccount.Email = emailAccount.Email.Trim();
        emailAccount.DisplayName = emailAccount.DisplayName.Trim();
        emailAccount.Host = emailAccount.Host.Trim();
        emailAccount.Username = emailAccount.Username.Trim();
        emailAccount.Password = emailAccount.Password.Trim();

        emailAccount.Email = CommonHelper.EnsureMaximumLength(emailAccount.Email, 255);
        emailAccount.DisplayName = CommonHelper.EnsureMaximumLength(emailAccount.DisplayName, 255);
        emailAccount.Host = CommonHelper.EnsureMaximumLength(emailAccount.Host, 255);
        emailAccount.Username = CommonHelper.EnsureMaximumLength(emailAccount.Username, 255);
        emailAccount.Password = CommonHelper.EnsureMaximumLength(emailAccount.Password, 255);

        //implement updte here
    }

    /// <summary>
    /// Updates an email account
    /// </summary>
    /// <param name="emailAccount">Email account</param>
    public void UpdateEmailAccount(EmailAccount emailAccount)
    {
        if (emailAccount == null)
            throw new ArgumentNullException("emailAccount");

        emailAccount.Email = CommonHelper.EnsureNotNull(emailAccount.Email);
        emailAccount.DisplayName = CommonHelper.EnsureNotNull(emailAccount.DisplayName);
        emailAccount.Host = CommonHelper.EnsureNotNull(emailAccount.Host);
        emailAccount.Username = CommonHelper.EnsureNotNull(emailAccount.Username);
        emailAccount.Password = CommonHelper.EnsureNotNull(emailAccount.Password);

        emailAccount.Email = emailAccount.Email.Trim();
        emailAccount.DisplayName = emailAccount.DisplayName.Trim();
        emailAccount.Host = emailAccount.Host.Trim();
        emailAccount.Username = emailAccount.Username.Trim();
        emailAccount.Password = emailAccount.Password.Trim();

        emailAccount.Email = CommonHelper.EnsureMaximumLength(emailAccount.Email, 255);
        emailAccount.DisplayName = CommonHelper.EnsureMaximumLength(emailAccount.DisplayName, 255);
        emailAccount.Host = CommonHelper.EnsureMaximumLength(emailAccount.Host, 255);
        emailAccount.Username = CommonHelper.EnsureMaximumLength(emailAccount.Username, 255);
        emailAccount.Password = CommonHelper.EnsureMaximumLength(emailAccount.Password, 255);

        //implement updte here
    }

    /// <summary>
    /// Gets all email accounts
    /// </summary>
    /// <returns>Email accounts</returns>
    public List<EmailAccount> GetAllEmailAccounts()
    {
        List<EmailAccount> _listEmail = new List<EmailAccount>();

        var email = _messageDA.GetEmailAccountDataList();
        for (var i = 0; i < email.Rows.Count; i++)
        {
            _listEmail.Add(new EmailAccount()
            {
                EmailAccountId = Convert.ToInt32(email.Rows[i]["EmailAccountId"]),
                Email = Convert.ToString(email.Rows[i]["Email"]),
                DisplayName = Convert.ToString(email.Rows[i]["DisplayName"]),
                Host = Convert.ToString(email.Rows[i]["Host"]),
                Port = Convert.ToInt32(email.Rows[i]["Port"]),
                Username = Convert.ToString(email.Rows[i]["Username"]),
                Password = Convert.ToString(email.Rows[i]["Password"]),
                EnableSSL = Convert.ToBoolean(email.Rows[i]["EnableSSL"]),
                UseDefaultCredentials = Convert.ToBoolean(email.Rows[i]["UseDefaultCredentials"]),
                // Attchment = Convert.ToString(email.Rows[i]["AttachmentUrl"]),
            });
        }
        return _listEmail;
    }

    #endregion Repository methods

    #region Workflow methods

    /// <summary>
    /// Sends a private message notification
    /// </summary>
    /// <param name="privateMessage">Private message</param>
    /// <param name="languageId">Message language identifier</param>
    /// <returns>Queued email identifier</returns>
    public int SendPrivateMessageNotification(PrivateMessage privateMessage)
    {
        if (privateMessage == null)
            throw new ArgumentNullException("privateMessage");

        string templateName = "Customer.NewPM";
        var localizedMessageTemplate = this.GetAllLocalizedMessageTemplates(templateName);
        if (localizedMessageTemplate == null || !localizedMessageTemplate.Select(i => i.IsActive).FirstOrDefault())
            return 0;

        var emailAccount = localizedMessageTemplate.Select(i => i.EmailAccount).FirstOrDefault();

        string subject = ReplaceMessageTemplateTokens(privateMessage, localizedMessageTemplate.Select(i => i.Subject).FirstOrDefault());
        string body = ReplaceMessageTemplateTokens(privateMessage, localizedMessageTemplate.Select(i => i.Body).FirstOrDefault());
        string bcc = localizedMessageTemplate.Select(i => i.BccEmailAddresses).FirstOrDefault();
        var from = new MailAddress(emailAccount.Email, emailAccount.DisplayName);

        //var recipient = privateMessage.ToUser;
        var recipient = privateMessage.Email;
        if (recipient == null)
            return 0;

        //var to = new MailAddress(recipient.Email, recipient.FullName);
        var to = new MailAddress(recipient);
        var queuedEmail = InsertQueuedEmail(5, from, to, string.Empty, bcc, subject, body,
            DateTime.UtcNow, 0, null, emailAccount.EmailAccountId);
        return queuedEmail.QueuedEmailId;
    }

    /// <summary>
    /// Gets list of allowed (supported) message tokens
    /// </summary>
    /// <returns></returns>
    public string[] GetListOfAllowedTokens()
    {
        var allowedTokens = new List<string>();
        dt = da.GetAllAvailableTokanCreation();

        for (int i = 0; i <= dt.Rows.Count - 1; i++)
        {
            allowedTokens.Add("%" + dt.Columns["TokanName"].ToString() + "%");
        }
        //allowedTokens.Add("%Store.Name%");
        //allowedTokens.Add("%Store.URL%");
        //allowedTokens.Add("%Store.Email%");

        //allowedTokens.Add("%PrivateMessage.Subject%");
        //allowedTokens.Add("%PrivateMessage.Text%");

        return allowedTokens.ToArray();
    }

    /// <summary>
    /// Sends an email
    /// </summary>
    /// <param name="subject">Subject</param>
    /// <param name="body">Body</param>
    /// <param name="from">From</param>
    /// <param name="to">To</param>
    /// <param name="emailAccount">Email account to use</param>
    public void SendEmail(string subject, string body, string from, string to, EmailAccount emailAccount)
    {
        SendEmail(subject, body, new MailAddress(from), new MailAddress(to), new List<String>(), new List<String>(), emailAccount);
    }

    /// <summary>
    /// Sends an email
    /// </summary>
    /// <param name="subject">Subject</param>
    /// <param name="body">Body</param>
    /// <param name="from">From</param>
    /// <param name="to">To</param>
    /// <param name="emailAccount">Email account to use</param>
    public void SendEmail(string subject, string body, MailAddress from, MailAddress to, EmailAccount emailAccount)
    {
        //  SendEmail(subject, body, from, to, new List<String>(), new List<String>(), emailAccount);
    }

    /// <summary>
    /// Sends an email
    /// </summary>
    /// <param name="subject">Subject</param>
    /// <param name="body">Body</param>
    /// <param name="from">From</param>
    /// <param name="to">To</param>
    /// <param name="bcc">BCC</param>
    /// <param name="cc">CC</param>
    /// <param name="emailAccount">Email account to use</param>
    public void SendEmail(string subject, string body, MailAddress from, MailAddress to, List<string> bcc, List<string> cc, EmailAccount emailAccount)
    {
        var message = new MailMessage();
        message.From = from;
        message.To.Add(to);
        if (null != bcc)
            foreach (string address in bcc)
            {
                if (address != null)
                {
                    if (!String.IsNullOrEmpty(address.Trim()))
                    {
                        message.Bcc.Add(address.Trim());
                    }
                }
            }
        string abc = string.Empty;
        foreach (string str1 in emailAccount.Attchment.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            var streamdata = _goldMedia.GoldMediaDownload(str1.ToLower());
            if (streamdata != null && streamdata.Length > 0)
            {
                var filename = str1.Split('\\').LastOrDefault().ToLower();
                var extname = filename.Split('.').LastOrDefault().ToLower();
                var contenttype = string.Empty;
                if (extname == "pdf")
                {
                    contenttype = "application/pdf";
                }
                else if (extname == "zip")
                {
                    contenttype = "application/zip";
                }
                else if (extname == "xls")
                {
                    contenttype = "application/xls";
                }
                else if (extname == "jpg")
                {
                    contenttype = "image/jpeg";
                }
                else if (extname == "jpeg")
                {
                    contenttype = "image/jpeg";
                }
                else if (extname == "png")
                {
                    contenttype = "image/png";
                }

                abc = _goldMedia.MapPathToPublicUrl(str1).ToLower();
                //if (File.Exists(abc))
                //{
                message.Attachments.Add(new Attachment(streamdata, filename, contenttype));
                // }
            }
        }

        if (null != cc)
            foreach (string address in cc)
            {
                if (address != null)
                {
                    if (!String.IsNullOrEmpty(address.Trim()))
                    {
                        message.CC.Add(address.Trim());
                    }
                }
            }
        message.Subject = subject;

        message.Body = body;
        //  message.Attachments.Add(Attach);
        message.IsBodyHtml = true;

        var smtpClient = new SmtpClient();
        smtpClient.UseDefaultCredentials = emailAccount.UseDefaultCredentials;
        smtpClient.Host = emailAccount.Host;
        smtpClient.Port = emailAccount.Port;
        smtpClient.EnableSsl = true;
        if (emailAccount.UseDefaultCredentials)
            smtpClient.Credentials = CredentialCache.DefaultNetworkCredentials;
        else
            smtpClient.Credentials = new NetworkCredential(emailAccount.Username, emailAccount.Password);
        smtpClient.Send(message);
    }

    private bool CheckUrlStatus(object text)
    {
        throw new NotImplementedException();
    }

    #endregion Workflow methods

    #endregion Methods

    #region Properties

    /// <summary>
    /// Gets or sets a primary store currency
    /// </summary>
    public EmailAccount DefaultEmailAccount
    {
        get
        {
            int defaultEmailAccountId = _settingManager.GetSettingValueInteger("EmailAccount.DefaultEmailAccountId");
            var emailAccount = GetEmailAccountById(defaultEmailAccountId);
            if (emailAccount == null)
                emailAccount = GetAllEmailAccounts().FirstOrDefault();

            return emailAccount;
        }
        set
        {
            if (value != null)
                _settingManager.SetParam("EmailAccount.DefaultEmailAccountId", value.EmailAccountId.ToString());
        }
    }

    //public void UpdateQueuedEmail()
    //{
    //    _goldDataAccess.ReturnDataTable("Gold_EmailQueueFirstTask");
    //}
    public bool UpdateQueuedEmail()
    {
        bool result = false;
        var dt = _goldDataAccess.ReturnDataTable("Gold_EmailQueueFirstTask");

        if (dt.Rows.Count > 0)
        { result = true; }
        else { result = false; }
        return result;
    }

    public bool DeleteSendQueuedEmail()
    {
        bool result = false;
        var dt = _goldDataAccess.ReturnDataTable("Gold_QueuedEmailAfterEmailSendDelete");
        if (dt.Rows.Count > 0)
        { result = true; }
        else { result = false; }
        return result;
    }

    public DataTable GetQueuedActiveData()
    {
        return _goldDataAccess.ReturnDataTable("Gold_EmailQueueAllActiveData");
    }

    public DataTable GetQueuedMessageActiveData()
    {
        return _goldDataAccess.ReturnDataTable("Gold_MessageQueueAllActiveData");
    }

    public DataTable GetQueuedMobileNotificationData(string Sendby)
    {
        SqlParameter[] objParameter = new SqlParameter[1];

        objParameter[0] = new SqlParameter("@sendby", SqlDbType.VarChar);
        objParameter[0].Value = Sendby;

        return _goldDataAccess.ReturnDataTableWithParameters("Gold_NotificationQueueData",objParameter);
    }

    public DataTable GetQueuedMobileNotificationDataEx(string Sendby)
    {
        SqlParameter[] objParameter = new SqlParameter[1];

        objParameter[0] = new SqlParameter("@sendby", SqlDbType.VarChar);
        objParameter[0].Value = Sendby;

        return _goldDataAccess.ReturnDataTableWithParameters("Gold_NotificationQueueDataEx", objParameter);
    }

    public DataTable GetEmailAccountForTestMail()
    {
        return _goldDataAccess.ReturnDataTable("Gold_EmailAccountforTestMail");
    }

    public bool UpdateQueuedEmailAfterSend(int EmailId, int Senttries)
    {
        bool result = false;
        SqlParameter[] objParameter = new SqlParameter[2];

        objParameter[0] = new SqlParameter("@EmailId", SqlDbType.Int);
        objParameter[0].Value = EmailId;

        objParameter[1] = new SqlParameter("@Senttries", SqlDbType.Int);
        objParameter[1].Value = Senttries;

        return bool.TryParse(_goldDataAccess.ExecuteNonQueryWithParameters("Gold_QueuedEmailUpadteAfterEmailSend", objParameter).ToString(), out result);
    }

    public bool UpdateQueuedMessageAfterSend(int Id)
    {
        bool result = false;
        SqlParameter[] objParameter = new SqlParameter[1];

        objParameter[0] = new SqlParameter("@Queued", SqlDbType.Int);
        objParameter[0].Value = Id;

        return bool.TryParse(_goldDataAccess.ExecuteNonQueryWithParameters("Gold_QueuedMessageUpadteAfterMessageSend", objParameter).ToString(), out result);
    }

    public bool UpdateQueuedNotificationAfterSend(int Id,string Sendby)
    {
        bool result = false;
        SqlParameter[] objParameter = new SqlParameter[2];

        objParameter[0] = new SqlParameter("@Queued", SqlDbType.Int);
        objParameter[0].Value = Id;

        objParameter[1] = new SqlParameter("@sendby", SqlDbType.VarChar);
        objParameter[1].Value = Sendby;

        return bool.TryParse(_goldDataAccess.ExecuteNonQueryWithParameters("Gold_QueuedNotificationUpadteAfterSend", objParameter).ToString(), out result);
    }

    public bool UpdateQueuedEmailTries(int Id, int tries)
    {
        bool result = false;
        SqlParameter[] objParameter = new SqlParameter[2];

        objParameter[0] = new SqlParameter("@Id", SqlDbType.Int);
        objParameter[0].Value = Id;

        objParameter[1] = new SqlParameter("@Tries", SqlDbType.Int);
        objParameter[1].Value = tries;

        return bool.TryParse(_goldDataAccess.ExecuteNonQueryWithParameters("Gold_EmailQueueTriesUpdate", objParameter).ToString(), out result);
    }

    public DataTable getdata(string procedure)
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["myconErp"].ConnectionString);

        conn.Open();
        DataTable dt = new DataTable();
        SqlDataAdapter adapter = new SqlDataAdapter();
        SqlCommand cmd = new SqlCommand(procedure, conn);

        cmd.CommandType = CommandType.StoredProcedure;
        adapter.SelectCommand = cmd;
        adapter.Fill(dt);
        conn.Close();
        return dt;
    }

    public DataTable return_dt(string s1)
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["myconErp"].ConnectionString);
        conn.Open();
        SqlCommand cmd1 = new SqlCommand(s1, conn);
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(cmd1);
        da.Fill(dt);
        conn.Close();
        return dt;
    }


    public SqlDataReader return_dr(string s1)
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["myconErp"].ConnectionString);
        conn.Open();
        try
        {
            SqlDataReader dr;
            using (SqlCommand cmd1 = new SqlCommand(s1, conn))
            {
                dr = cmd1.ExecuteReader();
            }

            return dr;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable getdatabyuser(string procedure, int ID, string parameter)
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["myconErp"].ConnectionString);

        conn.Open();
        DataTable dt = new DataTable();
        SqlDataAdapter adapter = new SqlDataAdapter();
        SqlCommand cmd = new SqlCommand(procedure, conn);
        cmd.Parameters.Add(parameter, SqlDbType.Int, 50).Value = ID;
        cmd.CommandType = CommandType.StoredProcedure;
        adapter.SelectCommand = cmd;
        adapter.Fill(dt);
        conn.Close();
        return dt;
    }

    public bool ValidateEmailId(string Email)
    {
        bool check = false;

        check = Regex.IsMatch(Email.Trim(), @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
        if (Regex.IsMatch(Email.Trim(), "^[0-9]"))
        {
            check = false;
        }

        return check;
    }

    public bool ValidateMobileNo(string Mobile)
    {
        bool check = false;

        check = Regex.IsMatch(Mobile.Trim().TrimStart('0'), @"\d{10}", RegexOptions.IgnoreCase);

        return check;
    }

    #endregion Properties
}