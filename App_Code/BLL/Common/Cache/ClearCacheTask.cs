using System;
using System.Xml;

/// <summary>
/// Summary description for ClearCacheTask
/// </summary>
public partial class ClearCacheTask : ITask
{
    /// <summary>
    /// Executes the clear cache task
    /// </summary>
    /// <param name="node">XML node that represents a task description</param>
    public void Execute(XmlNode node)
    {
        try
        {
            new GoldStaticCache().Clear();
        }
        catch (Exception ex)
        {
            GoldLogging gLog = new GoldLogging();
            gLog.SendErrorToText(ex);

        }
    }
}