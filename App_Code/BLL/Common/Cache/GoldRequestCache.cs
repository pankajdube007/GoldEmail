using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
/// <summary>
/// Summary description for GoldRequestCache
/// </summary>
public partial class GoldRequestCache : ICacheManager
{
    #region Methods

    /// <summary>
    /// Creates a new instance of the NopRequestCache class
    /// </summary>
    protected IDictionary GetItems()
    {
        HttpContext current = HttpContext.Current;
        if (current != null)
        {
            return current.Items;
        }

        return null;
    }

    /// <summary>
    /// Gets or sets the value associated with the specified key.
    /// </summary>
    /// <param name="key">The key of the value to get.</param>
    /// <returns>The value associated with the specified key.</returns>
    public object Get(string key)
    {
        var items = GetItems();
        if (items == null)
            return null;

        return items[key];
    }

    /// <summary>
    /// Adds the specified key and object to the cache.
    /// </summary>
    /// <param name="key">key</param>
    /// <param name="obj">object</param>
    public void Add(string key, object obj)
    {
        var items = GetItems();
        if (items == null)
            return;

        if (IsEnabled && (obj != null))
        {
            items.Add(key, obj);
        }
    }

    /// <summary>
    /// Removes the value with the specified key from the cache
    /// </summary>
    /// <param name="key"></param>
    public void Remove(string key)
    {
        var items = GetItems();
        if (items == null)
            return;

        items.Remove(key);
    }

    /// <summary>
    /// Removes items by pattern
    /// </summary>
    /// <param name="pattern">pattern</param>
    public void RemoveByPattern(string pattern)
    {
        var items = GetItems();
        if (items == null)
            return;

        IDictionaryEnumerator enumerator = items.GetEnumerator();
        Regex regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
        var keysToRemove = new List<String>();
        while (enumerator.MoveNext())
        {
            if (regex.IsMatch(enumerator.Key.ToString()))
            {
                keysToRemove.Add(enumerator.Key.ToString());
            }
        }

        foreach (string key in keysToRemove)
        {
            items.Remove(key);
        }
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets a value indicating whether the cache is enabled
    /// </summary>
    public bool IsEnabled
    {
        get
        {
            return true;
        }
    }

    #endregion
}