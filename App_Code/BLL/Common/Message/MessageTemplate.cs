using System.Collections.Generic;

/// <summary>
/// Summary description for MessageTemplate
/// </summary>
public partial class MessageTemplate
{
    #region Properties

    /// <summary>
    /// Gets or sets the message template identifier
    /// </summary>
    public int MessageTemplateId { get; set; }

    /// <summary>
    /// Gets or sets the name
    /// </summary>
    public string Name { get; set; }

    #endregion Properties

    #region Navigation Properties

    /// <summary>
    /// Gets the localized message template
    /// </summary>
    public virtual ICollection<LocalizedMessageTemplate> GoldMessageTemplateLocalized { get; set; }

    #endregion Navigation Properties
}