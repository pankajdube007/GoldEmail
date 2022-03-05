using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Summary description for TestSms
/// </summary>
public class TestSms
{
    [Required]
    public int ID { get; set; }

    public string ToSMS { get; set; }
}

public class TestSmss
{
    public string result { get; set; }
    public string message { get; set; }
    public string servertime { get; set; }

    public List<DetailsTemplateSMS> data { get; set; }
}

public class DetailsTemplateSMS
{
    public string BodyTEMP { get; set; }
}