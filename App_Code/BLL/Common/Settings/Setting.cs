using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Setting
/// </summary>
public partial class Setting 
{
    /// <summary>
    /// Gets or sets the setting identifier
    /// </summary>
    public int SettingId { get; set; }

    /// <summary>
    /// Gets or sets the name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the value
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// Gets or sets the description
    /// </summary>
    public string Description { get; set; }
}