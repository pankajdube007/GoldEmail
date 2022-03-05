using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ISettingsDA
/// </summary>
public interface ISettingsDA
{
    DataTable GetSettingDataList();
    DataTable GetSettingDataByID(int recid);
    DataTable GetSettingDataByName(string name);
}