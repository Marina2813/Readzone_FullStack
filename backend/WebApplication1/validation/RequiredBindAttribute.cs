using System.ComponentModel.DataAnnotations;

namespace WebApplication1.validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class RequiredBindAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null) return false;

            if (value is string str && string.IsNullOrWhiteSpace(str))
                return false;

            return true;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} is required and must be provided in the request.";
        }
    }
}
