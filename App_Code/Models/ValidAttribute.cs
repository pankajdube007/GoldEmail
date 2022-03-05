using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
internal class ValidAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if ((string.IsNullOrEmpty(value.ToString())))
        {
            //return ValidationResult.Success;
            throw new ValidationException("Key is valid for Base64 only.");
        }
        else
        {
            if (!(IsBase64String(value.ToString())))
            {
                return new ValidationResult(this.FormatErrorMessage(validationContext.MemberName));
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }

    public bool IsBase64String(string s)
    {
        string st = s.Trim().Replace(' ', '+');//.Replace("-","+").Replace("_", "/");
        if (st.Length % 4 > 0)
        {
            st = s.PadRight(s.Length + 4 - s.Length % 4, '=');
        }
        // s = s.Trim();
        return (st.Length % 4 == 0) && Regex.IsMatch(st, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
    }
}