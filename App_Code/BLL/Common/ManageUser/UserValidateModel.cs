using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for UserValidateModel
/// </summary>
public class UserValidateModel
{
    public class UserInputs
    {
        private string name;
        public string usernm
        {
            get
            {
                return this.name ?? "";
            }
            set
            {
                this.name = value;
            }
        }
        private string password;
        public string pwd
        {
            get
            {
                return this.password ?? "";
            }
            set
            {
                this.password = value;
            }
        }
        private string track;
        public string ip
        {
            get
            {
                return this.track ?? "";
            }
            set
            {
                this.track = value;
            }
        }
    }
}