namespace Nomad.FileSystem.Property.Providers
{
    using Nomad.FileSystem.Property;
    using System;
    using System.Text.RegularExpressions;

    public class CustomExtPropertyProvider : CustomPropertyProvider
    {
        protected static Regex InitializeExtRegex(ExtSection section)
        {
            if (section == null)
            {
                return null;
            }
            string extension = section.Extension;
            if (string.IsNullOrEmpty(extension))
            {
                return null;
            }
            return new Regex(extension, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }
    }
}

