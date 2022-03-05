using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
/// <summary>
/// Summary description for ClientDetails
/// </summary>
public static  class ClientDetails
{
   public static List<ClientSource> GetClientDetails()
    {
        List<ClientSource> clientSource = new List<ClientSource>();
        if (HttpContext.Current != null || HttpContext.Current.Request != null)
        {
            HttpBrowserCapabilities browserCapabilities = HttpContext.Current.Request.Browser;
            var context = HttpContext.Current;
            string ip = String.Empty;
            string name = context.Request.UserHostName;
            if (context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                ip = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }               
            else if (!String.IsNullOrWhiteSpace(context.Request.UserHostAddress))
            {
                ip = context.Request.UserHostAddress;
            }
            if (ip == "::1")
            {
                ip = "127.0.0.1";
                name = "localhost";
            }
            string machineName = string.Empty;
            try
            {
                machineName=Dns.GetHostEntry(context.Request.ServerVariables["remote_addr"]).HostName;
            }
            catch (Exception)
            {
                //throw;
            }
            clientSource.Add(new ClientSource
            {
                UserHostAddress = ip,
                UserHostName = name,
                MachineName= machineName,
                Platform = browserCapabilities.Platform,
                Browser = browserCapabilities.Browser,
                Version = browserCapabilities.Version,
                Cookies = browserCapabilities.Cookies,
                IsMobileDevice = browserCapabilities.IsMobileDevice,
            });
            //string operatingSystem = GetOperatinSystemDetails(HttpContext.Current.Request.UserAgent);          
        }
        return clientSource;
    }
    public static string GetOperatinSystemDetails(string browserDetails)
    {
        try
        {
            switch (browserDetails.Substring(browserDetails.LastIndexOf("Windows NT") + 11, 3).Trim())
            {
                case "6.2":
                    return "Windows 8";
                case "6.1":
                    return "Windows 7";
                case "6.0":
                    return "Windows Vista";
                case "5.2":
                    return "Windows XP 64-Bit Edition";
                case "5.1":
                    return "Windows XP";
                case "5.0":
                    return "Windows 2000";
                default:
                    return browserDetails.Substring(browserDetails.LastIndexOf("Windows NT"), 14);
            }
        }
        catch
        {
            if (browserDetails.Length > 149)
                return browserDetails.Substring(0, 149);
            else
                return browserDetails;
        }
    }  


   
}
public class ClientSource
{
    public string UserHostAddress { get; set; }
    public string UserHostName { get; set; }
    public string MachineName { get; set; }
    public string Platform { get; set; }
    public string Browser { get; set; }
    public string Version { get; set; }
    public bool Cookies { get; set; }
    public bool IsMobileDevice { get; set; }
}