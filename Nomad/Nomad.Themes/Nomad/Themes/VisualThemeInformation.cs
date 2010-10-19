namespace Nomad.Themes
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Windows.Forms.VisualStyles;

    public static class VisualThemeInformation
    {
        private const string aeroFileName = "aero.msstyles";
        private const string lunaFileName = "luna.msstyles";
        private const string normalColorScheme = "NormalColor";
        private const string oliveColorScheme = "HomeStead";
        private const string royaleColorScheme = "Royale";
        private const string royaleFileName = "royale.msstyles";
        private const string silverColorScheme = "Metallic";
        private static PropertyInfo ThemeProperty = typeof(VisualStyleInformation).GetProperty("ThemeFilename", BindingFlags.NonPublic | BindingFlags.Static);

        public static string ThemeFilename
        {
            get
            {
                return (string) ThemeProperty.GetValue(null, null);
            }
        }

        public static Nomad.Themes.VisualTheme VisualTheme
        {
            get
            {
                if (!VisualStyleRenderer.IsSupported)
                {
                    return Nomad.Themes.VisualTheme.None;
                }
                string colorScheme = VisualStyleInformation.ColorScheme;
                string fileName = Path.GetFileName(ThemeFilename);
                if (string.Equals("luna.msstyles", fileName, StringComparison.OrdinalIgnoreCase))
                {
                    switch (colorScheme)
                    {
                        case "NormalColor":
                            return Nomad.Themes.VisualTheme.LunaBlue;

                        case "HomeStead":
                            return Nomad.Themes.VisualTheme.LunaOlive;

                        case "Metallic":
                            return Nomad.Themes.VisualTheme.LunaSilver;
                    }
                }
                else
                {
                    if (string.Equals("aero.msstyles", fileName, StringComparison.OrdinalIgnoreCase))
                    {
                        return Nomad.Themes.VisualTheme.Aero;
                    }
                    if (string.Equals("royale.msstyles", fileName, StringComparison.OrdinalIgnoreCase) && ((colorScheme == "NormalColor") || (colorScheme == "Royale")))
                    {
                        return Nomad.Themes.VisualTheme.Royal;
                    }
                }
                return Nomad.Themes.VisualTheme.Other;
            }
        }
    }
}

