using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for IUserManagement
/// </summary>
public interface IUserManagement
{
    string AuthUser(string data);
    string GetUserInfoString(UserEnum.UserInfoEnum uifo);
    bool GetUserInfoBool(UserEnum.UserInfoEnum uifo);
    int GetUserInfoInt(UserEnum.UserInfoEnum uifo);
    long GetUserInfoLong(UserEnum.UserInfoEnum uifo);
    bool IsDataExportable(string key);
    void FakeUserStore(int uid);
}