namespace Nomad.Themes
{
    using Microsoft;
    using Nomad.Controls;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Windows.Forms.VisualStyles;

    public class ThemeDefaultColorTable : ThemeColorTable
    {
        public override Color FromKnownColor(KnownThemeColor knownColor)
        {
            return this.GetKnownColor(knownColor);
        }

        protected override Color GetKnownColor(KnownThemeColor knownColor)
        {
            ToolStripProfessionalRenderer renderer;
            switch (knownColor)
            {
                case KnownThemeColor.MenuBar:
                    return (OS.IsWinXP ? SystemColors.MenuBar : SystemColors.Menu);

                case KnownThemeColor.OptionSectionGradientBegin:
                    switch (CurrentTheme)
                    {
                        case VisualTheme.LunaBlue:
                        case VisualTheme.LunaOlive:
                        case VisualTheme.LunaSilver:
                        case VisualTheme.Other:
                            renderer = ToolStripManager.Renderer as ToolStripProfessionalRenderer;
                            if (renderer != null)
                            {
                                return renderer.ColorTable.MenuStripGradientBegin;
                            }
                            return ProfessionalColors.MenuStripGradientBegin;

                        case VisualTheme.Aero:
                        case VisualTheme.Royal:
                            return Color.LightSkyBlue;
                    }
                    return SystemColors.GradientActiveCaption;

                case KnownThemeColor.OptionSectionLabelText:
                case KnownThemeColor.OptionBlockLabelText:
                    switch (CurrentTheme)
                    {
                        case VisualTheme.None:
                        case VisualTheme.LunaOlive:
                        case VisualTheme.LunaSilver:
                            return Color.DimGray;

                        case VisualTheme.Other:
                            renderer = ToolStripManager.Renderer as ToolStripProfessionalRenderer;
                            if ((renderer != null) && renderer.ColorTable.UseSystemColors)
                            {
                                return Color.Navy;
                            }
                            return Color.DimGray;
                    }
                    break;

                case KnownThemeColor.OptionBlockLabelBackground:
                    switch (CurrentTheme)
                    {
                        case VisualTheme.None:
                            return Color.WhiteSmoke;

                        case VisualTheme.LunaBlue:
                        case VisualTheme.LunaOlive:
                        case VisualTheme.LunaSilver:
                        case VisualTheme.Other:
                            return ((ToolStripProfessionalRenderer) ToolStripManager.Renderer).ColorTable.ToolStripGradientMiddle;

                        case VisualTheme.Aero:
                        case VisualTheme.Royal:
                            goto Label_01AC;
                    }
                    goto Label_01AC;

                case KnownThemeColor.OptionBlockLabelBorder:
                    if (CurrentTheme != VisualTheme.LunaOlive)
                    {
                        return Color.Silver;
                    }
                    return Color.Gray;

                case KnownThemeColor.WindowBackground:
                    renderer = ToolStripManager.Renderer as ToolStripProfessionalRenderer;
                    if (renderer == null)
                    {
                        return SystemColors.ButtonFace;
                    }
                    return renderer.ColorTable.ToolStripGradientMiddle;

                case KnownThemeColor.WindowBorder:
                    return (VisualStyleRenderer.IsSupported ? VisualStyleInformation.TextControlBorder : Color.DarkGray);

                case KnownThemeColor.ActiveBreadcrumbBackground:
                    if (CurrentTheme == VisualTheme.None)
                    {
                        return SystemColors.ActiveCaption;
                    }
                    return Color.FromArgb(0x60, SystemColors.ActiveCaption);

                case KnownThemeColor.ActiveBreadcrumbBorder:
                    if (CurrentTheme == VisualTheme.None)
                    {
                        if (ToolStripManager.Renderer is ToolStripSystemRenderer)
                        {
                            return SystemColors.ControlDarkDark;
                        }
                        return SystemColors.ActiveBorder;
                    }
                    return Color.Empty;

                case KnownThemeColor.ActiveBreadcrumbText:
                    return SystemColors.ActiveCaptionText;

                case KnownThemeColor.InactiveBreadcrumbBackground:
                    if (CurrentTheme == VisualTheme.None)
                    {
                        return SystemColors.InactiveCaption;
                    }
                    return Color.FromArgb(0x60, SystemColors.InactiveCaption);

                case KnownThemeColor.InactiveBreadcrumbBorder:
                    if (CurrentTheme == VisualTheme.None)
                    {
                        if (ToolStripManager.Renderer is ToolStripSystemRenderer)
                        {
                            return SystemColors.ControlDark;
                        }
                        return SystemColors.InactiveBorder;
                    }
                    return Color.Empty;

                case KnownThemeColor.InactiveBreadcrumbText:
                    return SystemColors.InactiveCaptionText;

                case KnownThemeColor.DialogBackground:
                    return SystemColors.ButtonFace;

                case KnownThemeColor.DialogButtonsBackground:
                    return (Application.RenderWithVisualStyles ? Color.Gainsboro : SystemColors.ButtonFace);

                case KnownThemeColor.OptionNavigatorBackground:
                case KnownThemeColor.OptionSectionBackground:
                    return SystemColors.Window;

                case KnownThemeColor.OptionNavigatorText:
                    return SystemColors.WindowText;

                default:
                    throw new InvalidEnumArgumentException();
            }
            return Color.Navy;
        Label_01AC:
            return Color.FromArgb(0xdd, 0xe7, 0xee);
        }

        public static VisualTheme CurrentTheme
        {
            get
            {
                switch (ToolStripManager.RenderMode)
                {
                    case ToolStripManagerRenderMode.Custom:
                    {
                        ToolStripProfessionalRenderer renderer = ToolStripManager.Renderer as ToolStripProfessionalRenderer;
                        if (renderer == null)
                        {
                            break;
                        }
                        if (renderer.ColorTable is LunaBlueColorTable)
                        {
                            return VisualTheme.LunaBlue;
                        }
                        if (renderer.ColorTable is LunaOliveColorTable)
                        {
                            return VisualTheme.LunaOlive;
                        }
                        if (renderer.ColorTable is LunaSilverColorTable)
                        {
                            return VisualTheme.LunaSilver;
                        }
                        if (renderer.ColorTable is RoyaleColorTable)
                        {
                            return VisualTheme.Royal;
                        }
                        if (renderer.ColorTable is VistaColorTable)
                        {
                            return VisualTheme.Aero;
                        }
                        return VisualTheme.Other;
                    }
                    case ToolStripManagerRenderMode.Professional:
                        return VisualThemeInformation.VisualTheme;
                }
                return VisualTheme.None;
            }
        }
    }
}

