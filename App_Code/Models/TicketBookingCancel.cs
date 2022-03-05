using System.Collections.Generic;

/// <summary>
/// Summary description for TicketBookingCancel
/// </summary>
public class TicketBookingCancel
{
    public int ID { get; set; }
    public string To { get; set; }
    public string ToName { get; set; }
    public string BookingNo { get; set; }
    public string CancelDate { get; set; }
    public string RefundAmount { get; set; }
    public string ApprovalPageLink { get; set; }
    public byte SendNow { get; set; }
    public string For { get; set; }
    public int UserID { get; set; }
    public int LogNO { get; set; }
}

public class TicketBookingCancelDetails
{
    public string result { get; set; }
    public string message { get; set; }
    public string servertime { get; set; }
    public List<DetailsTemplateTicketBookingCancel> data { get; set; }
}

public class DetailsTemplateTicketBookingCancel
{
    public string SubjectTEMP { get; set; }
    public string BodyTEMP { get; set; }
}