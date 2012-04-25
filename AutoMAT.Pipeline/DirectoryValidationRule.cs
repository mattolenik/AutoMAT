using System.Globalization;
using System.IO;
using System.Windows.Controls;

namespace AutoMAT.Pipeline
{
    class DirectoryValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return Directory.Exists(value as string) ? ValidationResult.ValidResult : new ValidationResult(false, "Directory doesn't exist");
        }
    }
}