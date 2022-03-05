using System.Collections.Generic;

/// <summary>
/// Summary description for TicketBooking
/// </summary>
public class TicketBooking
{
    public int ID { get; set; }
    public string To { get; set; }
    public string ToName { get; set; }
    public string TourOperator { get; set; }
    public string BookingType { get; set; }
    public string BookingNo { get; set; }
    public string BookingDate { get; set; }
    public string BookingAmount { get; set; }
    public string TravelFromDate { get; set; }
    public string TravelToDate { get; set; }
    public string SourcePlace { get; set; }
    public string DestinationPlace { get; set; }
    public string ApprovalPageLink { get; set; }
    public byte SendNow { get; set; }
    public string For { get; set; }
    public int UserID { get; set; }
    public int LogNO { get; set; }
}

public class TicketBookingDetails
{
    public string result { get; set; }
    public string message { get; set; }
    public string servertime { get; set; }
    public List<DetailsTemplateTicketBooking> data { get; set; }
}

public class DetailsTemplateTicketBooking
{
    public string SubjectTEMP { get; set; }
    public string BodyTEMP { get; set; }
}