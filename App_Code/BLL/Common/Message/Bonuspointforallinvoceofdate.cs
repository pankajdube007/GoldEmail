using System;
using System.Data;
using System.Xml;

/// <summary>
/// Summary description for Bonuspointforallinvoceofdate
/// </summary>
namespace Goldmedal.Emails.App_Code.BLL.Common.Message
{
    public class Bonuspointforallinvoceofdate : ITask
    {
        private int _maxTries = 5;

        //   WebClient web = new WebClient();
        private readonly IEmailTemplateDA da;

        private readonly IMessageService _messageService;

        public Bonuspointforallinvoceofdate()
        {
            da = new EmailTemplateDA();
            _messageService = new MessageService();
        }

        public void Execute(XmlNode node)
        {
            XmlAttribute attribute1 = node.Attributes["maxTries"];
            if (attribute1 != null && !String.IsNullOrEmpty(attribute1.Value))
            {
                this._maxTries = int.Parse(attribute1.Value);
            }

            if (DateTime.Now.Hour >= 1)
            {
                DataTable dt = da.GetspfireDetails("sp_bonuspointforallinvoceofdate");
                if (dt.Rows.Count < 1)
                {
                    _messageService.return_dt("sp_bonuspointforallinvoceofdate");
                    da.CreateSPFireDetails("sp_bonuspointforallinvoceofdate");
                }
            }
        }
    }
}