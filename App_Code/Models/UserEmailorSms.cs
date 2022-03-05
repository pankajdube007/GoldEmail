/// <summary>
/// Summary description for UserEmailorSms
/// </summary>
public class UserEmailorSms
{
    public int Priority { get; set; }
    public string To { get; set; }
    public string ToName { get; set; }
    public string Cc { get; set; }
    public string Bcc { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public string SendOn { get; set; }
    public int UserId { get; set; }
    public int Logno { get; set; }
    public string AtachmentUrl { get; set; }
    public string Perpose { get; set; }
    public byte SendNow { get; set; }
}

public class UserEmailorSmsDetails
{
    public string result { get; set; }
    public string message { get; set; }
    public string servertime { get; set; }
    public string data { get; set; }
}