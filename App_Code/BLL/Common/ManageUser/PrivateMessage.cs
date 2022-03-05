using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for PrivateMessage
/// </summary>
public partial class PrivateMessage
{
    /// <summary>
    /// Gets or sets the subject
    /// </summary>
    public string Subject { get; set; }

    /// <summary>
    /// Gets or sets the text
    /// </summary>
    public string Text { get; set; }
    /// <summary>
    /// Gets or sets the user identifier who should receive the message
    /// </summary>
    public string Email { get; set; }
}