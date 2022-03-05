using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

internal class Common
{
    private DataConection g1 = new DataConection();

    internal bool check = false;

    /// <summary>
    ///
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    [Valid]
    public bool Validate(string key)
    {
        DataConection g1 = new DataConection();
        Authen auth = new Authen();
        try
        {
            byte[] uniq = Convert.FromBase64String(key);
            if (uniq.Length == 96)
            {
                var recovered = auth.DecryptString(auth.EncryptionKey, uniq);
                Guid guidOutput;
                bool isValid = Guid.TryParse(recovered, out guidOutput);

                if (isValid)
                {
                    var dt1 = g1.return_dt("exec dcrlogindetlcheck '" + guidOutput + "'");
                    if (dt1.Rows.Count > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    /// <summary>

    public string StatusTime(bool issucess, string text)
    {
        string data1;
        string issucess1 = Convert.ToString(issucess);
        List<SuccessTime> logstatus = new List<SuccessTime>();
        logstatus.Add(new SuccessTime
        {
            result = issucess1,
            message = text,
            servertime = DateTime.Now.ToString(),
            data = new Succes { },
        });
        // data1 = JsonConvert.SerializeObject(logstatus);
        data1 = JsonConvert.SerializeObject(logstatus, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        return data1;
    }

    public class SuccessTime
    {
        public string result { get; set; }
        public string message { get; set; }
        public string servertime { get; set; }
        public Succes data { get; set; }
    }

    public class Succes
    {
    }

    [Valid]
    public string GetValidKey(string key)
    {
        DataConection g1 = new DataConection();
        Authen auth = new Authen();
        try
        {
            byte[] uniq = Convert.FromBase64String(key);
            if (uniq.Length == 96)
            {
                var recovered = auth.DecryptString(auth.EncryptionKey, uniq);
                Guid guidOutput;
                bool isValid = Guid.TryParse(recovered, out guidOutput);

                if (isValid)
                {
                    return guidOutput.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }
        catch (Exception ex)
        {
            return string.Empty;
        }
    }
}