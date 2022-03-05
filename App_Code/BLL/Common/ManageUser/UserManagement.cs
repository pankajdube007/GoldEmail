using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
/// <summary>
/// Summary description for UserManagement
/// </summary>
public class UserManagement : IUserManagement
{
    private List<UserInfo> _uinfo = null;
    private bool IsExportable = false;
    public UserManagement()
    {
        _uinfo = new List<UserInfo>();
    }
    public string AuthUser(string responseString)
    {
        string _returnString = string.Empty;
        dynamic _uData = JsonConvert.DeserializeObject(responseString);
        string resultstatus = _uData[0]["result"];
        string message = _uData[0]["message"];
        if (resultstatus.Trim() == "False")
        {
            CommonHelper.SetCookie("logged", "no", TimeSpan.FromHours(12));
            _returnString = Convert.ToString(_uData[0]["message"]);
        }
        else
        {
            bool isblock = _uData[0].data["isblock"];
            bool issuccess = _uData[0].data["issuccess"];
            if (isblock)
            {
                CommonHelper.SetCookie("logged", "no", TimeSpan.FromHours(12));
                _returnString = "Oops!! Your A/C has been blocked";
            }
            else
            {
                var result = StoreUserData(responseString);
                if (!string.IsNullOrEmpty(result))
                {
                    _returnString = result;
                }
            }
        }
        return _returnString;
    }
    private string StoreUserData(string info)
    {
        string _returnString = string.Empty;
        dynamic _uDatas = JsonConvert.DeserializeObject(info);
        _uinfo.Add(new UserInfo
        {
            UserLogID = Convert.ToString(_uDatas[0].data["userlogid"].ToString()),
            UserLogName = Convert.ToString(_uDatas[0].data["userlognm"].ToString()),
            UserName = Convert.ToString(_uDatas[0].data["usernm"].ToString()),
            StateID = Convert.ToString(_uDatas[0].data["stateid"].ToString()),
            Status = Convert.ToString(_uDatas[0].data["status"].ToString()),
            IsSuccess = Convert.ToString(_uDatas[0].data["issuccess"].ToString()),
            IsBlock = Convert.ToString(_uDatas[0].data["isblock"].ToString()),
            UniqueKey = Convert.ToString(_uDatas[0].data["uniquekey"].ToString()),
            LogNo = "12457",
            Usercat = "Admin",
        });



        const string logged = "yes";
        try
        {
            CommonHelper.SetCookie("logged", logged, TimeSpan.FromHours(12));
            CommonHelper.SetCookie("FacGoldPage", JsonConvert.SerializeObject(_uinfo), TimeSpan.FromHours(12));
        }
        catch (Exception)
        {
            _returnString = "Unable To Store Data! Try Again";
        }
        return _returnString;
    }
    private string GetUserString(UserEnum.UserInfoEnum uifo)
    {
        string _info = CommonHelper.GetCookieString("FacGoldPage", true);
        dynamic _infoData = JsonConvert.DeserializeObject(_info);
        dynamic str11 = _infoData[0][uifo.ToString()];
        return Convert.ToString(str11.Value);
    }
    public string GetUserInfoString(UserEnum.UserInfoEnum uifo)
    {
        return GetUserString(uifo);
    }
    public bool GetUserInfoBool(UserEnum.UserInfoEnum uifo)
    {
        var str1= GetUserInfoString(uifo).ToUpperInvariant();
        return (str1 == "TRUE" || str1 == "YES" || str1 == "1");      
    }
    public int GetUserInfoInt(UserEnum.UserInfoEnum uifo)
    {
        var str1 = CommonHelper.EnsureNumericOnly(GetUserInfoString(uifo));
        if (!String.IsNullOrEmpty(str1))
            return Convert.ToInt32(str1);
        else
            return 0;
    }
    public long GetUserInfoLong(UserEnum.UserInfoEnum uifo)
    {
        var str1 = CommonHelper.EnsureNumericOnly(GetUserInfoString(uifo));
        if (!String.IsNullOrEmpty(str1))
            return Convert.ToInt64(str1);
        else
            return 0;
    }
    public bool IsDataExportable(string key)
    {
        return bool.TryParse(ConfigurationManager.AppSettings[key], out IsExportable);
    }
    public void FakeUserStore(int uid)
    {
        //implement it here
    }
}
