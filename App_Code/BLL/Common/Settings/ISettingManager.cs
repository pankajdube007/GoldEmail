using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ISettingManager
/// </summary>
public partial interface ISettingManager
{
    /// <summary>
    /// Gets a setting
    /// </summary>
    /// <param name="settingId">Setting identifer</param>
    /// <returns>Setting</returns>
    Setting GetSettingById(int settingId);

    /// <summary>
    /// Deletes a setting
    /// </summary>
    /// <param name="settingId">Setting identifer</param>
    void DeleteSetting(int settingId);

    /// <summary>
    /// Gets all settings
    /// </summary>
    /// <returns>Setting collection</returns>
    Dictionary<string, Setting> GetAllSettings();

    /// <summary>
    /// Inserts/updates a param
    /// </summary>
    /// <param name="name">The name</param>
    /// <param name="value">The value</param>
    /// <returns>Setting</returns>
    Setting SetParam(string name, string value);

    /// <summary>
    /// Inserts/updates a param
    /// </summary>
    /// <param name="name">The name</param>
    /// <param name="value">The value</param>
    /// <param name="description">The description</param>
    /// <returns>Setting</returns>
    Setting SetParam(string name, string value, string description);

    /// <summary>
    /// Inserts/updates a param in US locale
    /// </summary>
    /// <param name="name">The name</param>
    /// <param name="value">The value</param>
    /// <returns>Setting</returns>
    Setting SetParamNative(string name, decimal value);

    /// <summary>
    /// Inserts/updates a param in US locale
    /// </summary>
    /// <param name="name">The name</param>
    /// <param name="value">The value</param>
    /// <param name="description">The description</param>
    /// <returns>Setting</returns>
    Setting SetParamNative(string name, decimal value, string description);

    /// <summary>
    /// Adds a setting
    /// </summary>
    /// <param name="name">The name</param>
    /// <param name="value">The value</param>
    /// <param name="description">The description</param>
    /// <returns>Setting</returns>
    Setting AddSetting(string name, string value, string description);

    /// <summary>
    /// Updates a setting
    /// </summary>
    /// <param name="settingId">Setting identifier</param>
    /// <param name="name">The name</param>
    /// <param name="value">The value</param>
    /// <param name="description">The description</param>
    /// <returns>Setting</returns>
    Setting UpdateSetting(int settingId, string name, string value, string description);
    /// <summary>
    /// Gets a boolean value of a setting
    /// </summary>
    /// <param name="name">The setting name</param>
    /// <returns>The setting value</returns>
    bool GetSettingValueBoolean(string name);

    /// <summary>
    /// Gets a boolean value of a setting
    /// </summary>
    /// <param name="name">The setting name</param>
    /// <param name="defaultValue">The default value</param>
    /// <returns>The setting value</returns>
    bool GetSettingValueBoolean(string name, bool defaultValue);

    /// <summary>
    /// Gets an integer value of a setting
    /// </summary>
    /// <param name="name">The setting name</param>
    /// <returns>The setting value</returns>
    int GetSettingValueInteger(string name);

    /// <summary>
    /// Gets an integer value of a setting
    /// </summary>
    /// <param name="name">The setting name</param>
    /// <param name="defaultValue">The default value</param>
    /// <returns>The setting value</returns>
    int GetSettingValueInteger(string name, int defaultValue);

    /// <summary>
    /// Gets a long value of a setting
    /// </summary>
    /// <param name="name">The setting name</param>
    /// <returns>The setting value</returns>
    long GetSettingValueLong(string name);

    /// <summary>
    ///  Gets a long value of a setting
    /// </summary>
    /// <param name="name">The setting name</param>
    /// <param name="defaultValue">The default value</param>
    /// <returns>The setting value</returns>
    long GetSettingValueLong(string name, int defaultValue);

    /// <summary>
    /// Gets a decimal value of a setting in US locale
    /// </summary>
    /// <param name="name">The setting name</param>
    /// <returns>The setting value</returns>
    decimal GetSettingValueDecimalNative(string name);

    /// <summary>
    /// Gets a decimal value of a setting in US locale
    /// </summary>
    /// <param name="name">The setting name</param>
    /// <param name="defaultValue">The default value</param>
    /// <returns>The setting value</returns>
    decimal GetSettingValueDecimalNative(string name, decimal defaultValue);

    /// <summary>
    /// Gets a setting value
    /// </summary>
    /// <param name="name">The setting name</param>
    /// <returns>The setting value</returns>
    string GetSettingValue(string name);

    /// <summary>
    /// Gets a setting value
    /// </summary>
    /// <param name="name">The setting name</param>
    /// <param name="defaultValue">The default value</param>
    /// <returns>The setting value</returns>
    string GetSettingValue(string name, string defaultValue);

    /// <summary>
    /// Get a setting by name
    /// </summary>
    /// <param name="name">The setting name</param>
    /// <returns>Setting instance</returns>
    Setting GetSettingByName(string name);

    /// <summary>
    /// Gets or sets a store name
    /// </summary>
    string StoreName { get; set; }

    /// <summary>
    /// Gets or sets a store URL (ending with "/")
    /// </summary>
    string StoreUrl { get; set; }

    /// <summary>
    /// Get current version
    /// </summary>
    /// <returns></returns>
    string CurrentVersion { get; }
}