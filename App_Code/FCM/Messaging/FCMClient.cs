using NLog;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

public class FCMClient
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    private static Uri FCM_URI = new Uri("https://fcm.googleapis.com/fcm/send");

    private ISerializer _serializer;

    private string _serverKey { get; set; }
    private string _serverKeyDhanbarse { get; set; }
    private string _serverKeyEx { get; set; }
    private string _serverKeyHRM { get; set; }

    //public string ServerKey
    //{
    //    get
    //    {
    //        return WebConfigurationManager.AppSettings["FCM_ServerKey"];
    //    }
    //}

    public FCMClient(ISerializer serializer)
    {
        _serializer = serializer;
    }

    public FCMClient() : this(new JsonNetSerializer())
    {
        _serverKey = WebConfigurationManager.AppSettings["FCM_ServerKey"];
        _serverKeyDhanbarse = WebConfigurationManager.AppSettings["FCMDhanbarse_ServerKey"];
        _serverKeyEx = WebConfigurationManager.AppSettings["FCM_ServerKeyExcutive"];
        _serverKeyHRM = WebConfigurationManager.AppSettings["FCM_ServerKeyHRM"];
    }

    public FCMClient(string serverKey, ISerializer serializer)
    {
        _serverKey = serverKey;
        _serializer = serializer;
    }

    public FCMClient(string serverKey) : this(serverKey, new JsonNetSerializer())
    {

    }

    public async Task<T> SendMessageAsync<T>(NotiMessage message) where T : class, IFCMResponse
    {
        var result = await SendMessageAsync(message);
        return result as T;
    }

    public async Task<IFCMResponse> SendMessageAsyncExcutiveApp(NotiMessage message)
    {
        if (TestMode) { message.DryRun = true; }

        var serializedMessage = _serializer.Serialize(message);

        var request = new HttpRequestMessage(HttpMethod.Post, FCM_URI);
        request.Headers.TryAddWithoutValidation("Authorization", "key=" + _serverKeyEx);
        request.Content = new StringContent(serializedMessage, Encoding.UTF8, "application/json");

        var client = HttpClient;
        var result = await client.SendAsync(request);


        if (result.StatusCode != System.Net.HttpStatusCode.OK)
        {
            var errorMessage = await result.Content.ReadAsStringAsync();
            Logger.Warn(errorMessage);

            if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new FCMUnauthorizedException();
            }
            //TODO: handle retry-timeout for 500 messages

            throw new FCMException(result.StatusCode, errorMessage);
        }

        var content = await result.Content.ReadAsStringAsync();
        Logger.Info(content);
        //if contains a multicast_id field, it's a downstream message
        if (content.Contains("multicast_id"))
        {
            return _serializer.Deserialize<DownstreamMessageResponse>(content);
        }

        //otherwhise it's a topic message
        return _serializer.Deserialize<TopicMessageResponse>(content);

    }

    public async Task<IFCMResponse> SendMessageAsync(NotiMessage message)
    {
        if (TestMode) { message.DryRun = true; }

        var serializedMessage = _serializer.Serialize(message);

        var request = new HttpRequestMessage(HttpMethod.Post, FCM_URI);
        request.Headers.TryAddWithoutValidation("Authorization", "key=" + _serverKey);
        request.Content = new StringContent(serializedMessage, Encoding.UTF8, "application/json");

        var client = HttpClient;
        var result = await client.SendAsync(request);


        if (result.StatusCode != System.Net.HttpStatusCode.OK)
        {
            var errorMessage = await result.Content.ReadAsStringAsync();
            Logger.Warn(errorMessage);

            if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new FCMUnauthorizedException();
            }
            //TODO: handle retry-timeout for 500 messages

            throw new FCMException(result.StatusCode, errorMessage);
        }

        var content = await result.Content.ReadAsStringAsync();
        Logger.Info(content);
        //if contains a multicast_id field, it's a downstream message
        if (content.Contains("multicast_id"))
        {
            return _serializer.Deserialize<DownstreamMessageResponse>(content);
        }

        //otherwhise it's a topic message
        return _serializer.Deserialize<TopicMessageResponse>(content);

    }


    public async Task<IFCMResponse> SendMessageAsyncDhanbarse(NotiMessage message)
    {
        if (TestMode) { message.DryRun = true; }

        var serializedMessage = _serializer.Serialize(message);

        var request = new HttpRequestMessage(HttpMethod.Post, FCM_URI);
        request.Headers.TryAddWithoutValidation("Authorization", "key=" + _serverKeyDhanbarse);
        request.Content = new StringContent(serializedMessage, Encoding.UTF8, "application/json");

        var client = HttpClient;
        var result = await client.SendAsync(request);


        if (result.StatusCode != System.Net.HttpStatusCode.OK)
        {
            var errorMessage = await result.Content.ReadAsStringAsync();
            Logger.Warn(errorMessage);

            if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new FCMUnauthorizedException();
            }
            //TODO: handle retry-timeout for 500 messages

            throw new FCMException(result.StatusCode, errorMessage);
        }

        var content = await result.Content.ReadAsStringAsync();
        Logger.Info(content);
        //if contains a multicast_id field, it's a downstream message
        if (content.Contains("multicast_id"))
        {
            return _serializer.Deserialize<DownstreamMessageResponse>(content);
        }

        //otherwhise it's a topic message
        return _serializer.Deserialize<TopicMessageResponse>(content);

    }

    public async Task<IFCMResponse> SendMessageAsyncHRM(NotiMessage message)
    {
        if (TestMode) { message.DryRun = true; }

        var serializedMessage = _serializer.Serialize(message);

        var request = new HttpRequestMessage(HttpMethod.Post, FCM_URI);
        request.Headers.TryAddWithoutValidation("Authorization", "key=" + _serverKeyHRM);
        request.Content = new StringContent(serializedMessage, Encoding.UTF8, "application/json");

        var client = HttpClient;
        var result = await client.SendAsync(request);


        if (result.StatusCode != System.Net.HttpStatusCode.OK)
        {
            var errorMessage = await result.Content.ReadAsStringAsync();
            Logger.Warn(errorMessage);

            if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new FCMUnauthorizedException();
            }
            //TODO: handle retry-timeout for 500 messages

            throw new FCMException(result.StatusCode, errorMessage);
        }

        var content = await result.Content.ReadAsStringAsync();
        Logger.Info(content);
        //if contains a multicast_id field, it's a downstream message
        if (content.Contains("multicast_id"))
        {
            return _serializer.Deserialize<DownstreamMessageResponse>(content);
        }

        //otherwhise it's a topic message
        return _serializer.Deserialize<TopicMessageResponse>(content);

    }

    /// <summary>
    /// Automatically sets all measages as dry_run
    /// </summary>
    public bool TestMode { get; set; }

    private static readonly HttpClient _httpClient = new HttpClient();
    /// <summary>
    /// Gets or sets the HttpClient used by the FCMClient.
    /// Aid for test purposes.
    /// </summary>
    internal HttpClient HttpClient
    {
        get
        {
            return _httpClient;
        }
    }

}

