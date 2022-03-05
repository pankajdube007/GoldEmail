using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Summary description for PartyWiseInd
/// </summary>

public class PartyWiseInd
{
    [Required]
    public string To { get; set; }

    public string ToName { get; set; }

    public string Subject { get; set; }
    public string Body { get; set; }

    public int UserID { get; set; }
    public int LogNO { get; set; }

    public string CC { get; set; }
    public string SendingTime { get; set; }

    public string For { get; set; }
}

public class PartyWiseIndDetails
{
    public string result { get; set; }
    public string message { get; set; }
    public string servertime { get; set; }
    public List<DetailsTemplatePartyWiseInd> data { get; set; }
}

public class DetailsTemplatePartyWiseInd
{
    public string SubjectTEMP { get; set; }
    public string BodyTEMP { get; set; }
}