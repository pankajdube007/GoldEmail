using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Summary description for EveryInvoice
/// </summary>
public class EveryInvoice
{
    [Required]
    public int ID { get; set; }

    public string To { get; set; }
    public string ToName { get; set; }
    public int UserID { get; set; }
    public int LogNO { get; set; }
    public string Attachment { get; set; }

    public string For { get; set; }
}

public class EveryInvoiceDetails
{
    public string result { get; set; }
    public string message { get; set; }
    public string servertime { get; set; }
    public List<DetailsEveryInvoice> data { get; set; }
}

public class DetailsEveryInvoice
{
    public string SubjectTEMP { get; set; }
    public string BodyTEMP { get; set; }
}

public class EveryDayInvoice
{
    public string PartyName { get; set; }
    public string ExName { get; set; }
    public string cin { get; set; }
}