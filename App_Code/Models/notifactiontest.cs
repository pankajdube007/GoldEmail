using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for notifactiontest
/// </summary>


public class notifactiontest
{
    public string cin { get; set; }
    public string title { get; set; }
    public string body { get; set; }
    public string imageurl { get; set; }
    public string redirectToActivity { get; set; }
    public string redirecturl { get; set; }
    public string informToServer { get; set; }
    public string pooswooshid { get; set; }
}


public class notifactiontestEx
{
    public int ExId { get; set; }
    public string title { get; set; }
    public string body { get; set; }
    public string imageurl { get; set; }
    public string redirectToActivity { get; set; }
    public string redirecturl { get; set; }
    public string informToServer { get; set; }
    public string pooswooshid { get; set; }
}

public class notifactiontests
{
    public string result { get; set; }
    public string message { get; set; }
    public string servertime { get; set; }
    public List<Detailsnotifactiontest> data { get; set; }
}

public class Detailsnotifactiontest
{
    public string output { get; set; }
}

