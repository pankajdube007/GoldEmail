﻿using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;

/// <summary>
/// Summary description for GoldStaticCache
/// </summary>
public partial class GoldStaticCache : ICacheManager
{
    #region Fields

    private readonly Cache _cache;

    #endregion

    #region Ctor

    /// <summary>
    /// Creates a new instance of the GoldStaticCache class
    /// </summary>
    public GoldStaticCache()
    {
        HttpContext current = HttpContext.Current;
        if (current != null)
        {
            _cache = current.Cache;
        }
        else
        {
            _cache = HttpRuntime.Cache;
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Removes all keys and values from the cache
    /// </summary>
    public void Clear()
    {
        IDictionaryEnumerator enumerator = _cache.GetEnumerator();
        while (enumerator.MoveNext())
        {
            _cache.Remove(enumerator.Key.ToString());
        }
    }

    /// <summary>
    /// Gets or sets the value associated with the specified key.
    /// </summary>
    /// <param name="key">The key of the value to get.</param>
    /// <returns>The value associated with the specified key.</returns>
    public object Get(string key)
    {
        return _cache[key];
    }

    /// <summary>
    /// Adds the specified key and object to the cache.
    /// </summary>
    /// <param name="key">key</param>
    /// <param name="obj">object</param>
    public void Add(string key, object obj)
    {
        Add(key, obj, null);
    }

    /// <summary>
    /// Adds the specified key and object to the cache.
    /// </summary>
    /// <param name="key">key</param>
    /// <param name="obj">object</param>
    /// <param name="dep">cache dependency</param>
    public void Add(string key, object obj, CacheDependency dep)
    {
        if (IsEnabled && (obj != null))
        {
            _cache.Insert(key, obj, dep, DateTime.MaxValue, TimeSpan.Zero, CacheItemPriority.AboveNormal, null);
        }
    }

    /// <summary>
    /// Removes the value with the specified key from the cache
    /// </summary>
    /// <param name="key"></param>
    public void Remove(string key)
    {
        _cache.Remove(key);
    }

    /// <summary>
    /// Removes items by pattern
    /// </summary>
    /// <param name="pattern">pattern</param>
    public void RemoveByPattern(string pattern)
    {
        IDictionaryEnumerator enumerator = _cache.GetEnumerator();
        Regex regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
        while (enumerator.MoveNext())
        {
            if (regex.IsMatch(enumerator.Key.ToString()))
            {
                _cache.Remove(enumerator.Key.ToString());
            }
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