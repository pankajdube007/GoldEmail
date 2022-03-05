using System;

/// <summary>
/// Summary description for QueuedEmail
/// </summary>
public partial class QueuedEmail
{
    private readonly IMessageService _messageService;

    public QueuedEmail()
    {
        _messageService = new MessageService();
    }

    #region Properties

    /// <summary>
    /// Gets or sets the ueued email identifier
    /// </summary>
    public int QueuedEmailId { get; set; }

    /// <summary>
    /// Gets or sets the priority
    /// </summary>
    public int Priority { get; set; }

    /// <summary>
    /// Gets or sets the From property
    /// </summary>
    public string From { get; set; }

    /// <summary>
    /// Gets or sets the FromName property
    /// </summary>
    public string FromName { get; set; }

    /// <summary>
    /// Gets or sets the To property
    /// </summary>
    public string To { get; set; }

    /// <summary>
    /// Gets or sets the ToName property
    /// </summary>
    public string ToName { get; set; }

    /// <summary>
    /// Gets or sets the CC
    /// </summary>
    public string CC { get; set; }

    /// <summary>
    /// Gets or sets the Bcc
    /// </summary>
    public string Bcc { get; set; }

    /// <summary>
    /// Gets or sets the subject
    /// </summary>
    public string Subject { get; set; }

    /// <summary>
    /// Gets or sets the body
    /// </summary>
    public string Body { get; set; }

    /// <summary>
    /// Gets or sets the date and time of item creation
    /// </summary>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// Gets or sets the send tries
    /// </summary>
    public int SendTries { get; set; }

    /// <summary>
    /// Gets or sets the sent date and time
    /// </summary>
    public DateTime? SentOn { get; set; }

    /// <summary>
    /// Gets or sets the used email account identifier
    /// </summary>
    public int EmailAccountId { get; set; }

    #endregion Properties

    #region Custom Properties

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
}