using System.Net;

public class FCMUnauthorizedException : FCMException
{
    public FCMUnauthorizedException() : base(HttpStatusCode.Unauthorized, "Unauthorized")
    {
    }
}

