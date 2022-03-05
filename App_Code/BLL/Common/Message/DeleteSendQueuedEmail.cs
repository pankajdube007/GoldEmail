using System;
using System.Xml;

/// <summary>
/// Summary description for DeleteSendQueuedEmail
/// </summary>
///
namespace Goldmedal.Emails.App_Code.BLL.Common.Message
{
    public class DeleteSendQueuedEmail : ITask
    {
        private int _maxTries = 5;
        private readonly IMessageService _messageService;
        private readonly GoldLogging _gLog;
        private readonly IEmailTemplateDA da;

        public DeleteSendQueuedEmail()
        {
            _messageService = new MessageService();
            _gLog = new GoldLogging();
        }

        /// <summary>
        /// Executes a task
        /// </summary>
        /// <param name="node">Xml node that represents a task description</param>
        public void Execute(XmlNode node)
        {
            XmlAttribute attribute1 = node.Attributes["maxTries"];
            if (attribute1 != null && !String.IsNullOrEmpty(attribute1.Value))
            {
                this._maxTries = int.Parse(attribute1.Value);
            }

            _messageService.DeleteSendQueuedEmail();
        }
    }
}