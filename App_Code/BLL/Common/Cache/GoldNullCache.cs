using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for GoldNullCache
/// </summary>
public partial class GoldNullCache : ICacheManager
{
    #region Methods

    /// <summary>
    /// Gets or sets the value associated with the specified key.
    /// </summary>
    /// <param name="key">The key of the value to get.</param>
    /// <returns>The value associated with the specified key.</returns>
    public object Get(string key)
    {
        return null;
    }

    /// <summary>
    /// Adds the specified key and object to the cache.
    /// </summary>
    /// <param name="key">key</param>
    /// <param name="obj">object</param>
    public void Add(string key, object obj)
    {
    }

    /// <summary>
    /// Removes the value with the specified key from the cache
    /// </summary>
    /// <param name="key"></param>
    public void Remove(string key)
    {
    }

    /// <summary>
    /// Removes items by pattern
    /// </summary>
    /// <param name="pattern">pattern</param>
    public void RemoveByPattern(string pattern)
    {
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