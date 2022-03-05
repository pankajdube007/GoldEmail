using System.Xml;

/// <summary>
/// Summary description for ITask
/// </summary>
public partial interface ITask
{
    /// <summary>
    /// Execute task
    /// </summary>
    /// <param name="node">Custom configuration node</param>
    void Execute(XmlNode node);
}