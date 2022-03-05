/// <summary>
/// Summary description for LocalizedMessageTemplate
/// </summary>
public partial class LocalizedMessageTemplate
{
    private readonly IMessageService _messageService;

    public LocalizedMessageTemplate()
    {
        _messageService = new MessageService();
    }

    #region Properties

    /// <summary>
    /// Gets or sets the localized message template identifier
    /// </summary>
    public int MessageTemplateLocalizedId { get; set; }

    /// <summary>
    /// Gets or sets the message template identifier
    /// </summary>
    public int MessageTemplateId { get; set; }

    /// <summary>
    /// Gets or sets the BCC Email addresses
    /// </summary>
    public string BccEmailAddresses { get; set; }

    /// <summary>
    /// Gets or sets the name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the subject
    /// </summary>
    public string Subject { get; set; }

    /// <summary>
    /// Gets or sets the body
    /// </summary>
    public string Body { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the template is active
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the used email account identifier
    /// </summary>
    public int EmailAccountId { get; set; }

    #endregion Properties

    #region Custom Properties

    /// <summary>
    /// Gets the message template
    /// </summary>
    public MessageTemplate MessageTemplate
    {
        get
        {
            return _messageService.GetMessageTemplateById(this.MessageTemplateId);
        }
    }

    /// <summary>
    /// Gets the used email account
    /// </summary>
    public EmailAccount EmailAccount
    {
        get
        {
            var emailAccount = _messageService.GetEmailAccountById(this.EmailAccountId);
            if (emailAccount == null)
                emailAccount = _messageService.DefaultEmailAccount;
            return emailAccount;
        }
    }

    #endregion Custom Properties

    #region Navigation Properties

    /// <summary>
    /// Gets the message template
    /// </summary>
    public virtual MessageTemplate GoldMessageTemplate { get; set; }

    #endregion Navigation Properties
}