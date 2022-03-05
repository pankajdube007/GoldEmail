using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ICacheManager
/// </summary>
public interface ICacheManager
{
    /// <summary>
    /// Gets or sets the value associated with the specified key.
    /// </summary>
    /// <param name="key">The key of the value to get.</param>
    /// <returns>The value associated with the specified key.</returns>
    object Get(string key);

    /// <summary>
    /// Adds the specified key and object to the cache.
    /// </summary>
    /// <param name="key">key</param>
    /// <param name="obj">object</param>
    void Add(string key, object obj);

    /// <summary>
    /// Removes the value with the specified key from the cache
    /// </summary>
    /// <param name="key"></param>
    void Remove(string key);

    /// <summary>
    /// Removes items by pattern
    /// </summary>
    /// <param name="pattern">pattern</param>
    void RemoveByPattern(string pattern);
}