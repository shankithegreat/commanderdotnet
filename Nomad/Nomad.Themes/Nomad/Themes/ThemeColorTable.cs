namespace Nomad.Themes
{
    using System;
    using System.ComponentModel;
    using System.Drawing;

    public abstract class ThemeColorTable
    {
        public static readonly ThemeColorTable Default = new ThemeDefaultColorTable();

        protected ThemeColorTable()
        {
        }

        public virtual Color FromKnownColor(KnownThemeColor knownColor)
        {
            Color color = this.GetKnownColor(knownColor);
            if (color.IsEmpty)
            {
                throw new InvalidEnumArgumentException();
            }
            return color;
        }

        protected abstract Color GetKnownColor(KnownThemeColor knownColor);

        public virtual Color ActiveBreadcrumbBackground
        {
            get
            {
                return this.FromKnownColor(KnownThemeColor.ActiveBreadcrumbBackground);
            }
        }

        public virtual Color ActiveBreadcrumbBorder
        {
            get
            {
                return this.FromKnownColor(KnownThemeColor.ActiveBreadcrumbBorder);
            }
        }

        public virtual Color ActiveBreadcrumbText
        {
            get
            {
                return this.FromKnownColor(KnownThemeColor.ActiveBreadcrumbText);
            }
        }

        public virtual Color DialogBackground
        {
            get
            {
                return this.FromKnownColor(KnownThemeColor.DialogBackground);
            }
        }

        public virtual Color DialogButtonsBackground
        {
            get
            {
                return this.FromKnownColor(KnownThemeColor.DialogButtonsBackground);
            }
        }

        public virtual Color InactiveBreadcrumbBackground
        {
            get
            {
                return this.FromKnownColor(KnownThemeColor.InactiveBreadcrumbBackground);
            }
        }

        public virtual Color InactiveBreadcrumbBorder
        {
            get
            {
                return this.FromKnownColor(KnownThemeColor.InactiveBreadcrumbBorder);
            }
        }

        public virtual Color InactiveBreadcrumbText
        {
            get
            {
                return this.FromKnownColor(KnownThemeColor.InactiveBreadcrumbText);
            }
        }

        public virtual Color MenuBar
        {
            get
            {
                return this.FromKnownColor(KnownThemeColor.WindowBorder);
            }
        }

        public virtual Color OptionBlockLabelBackground
        {
            get
            {
                return this.FromKnownColor(KnownThemeColor.OptionBlockLabelBackground);
            }
        }

        public virtual Color OptionBlockLabelBorder
        {
            get
            {
                return this.FromKnownColor(KnownThemeColor.OptionBlockLabelBorder);
            }
        }

        public virtual Color OptionBlockLabelText
        {
            get
            {
                return this.FromKnownColor(KnownThemeColor.OptionBlockLabelText);
            }
        }

        public virtual Color OptionNavigatorBackground
        {
            get
            {
                return this.FromKnownColor(KnownThemeColor.OptionNavigatorBackground);
            }
        }

        public virtual Color OptionNavigatorText
        {
            get
            {
                return this.FromKnownColor(KnownThemeColor.OptionNavigatorText);
            }
        }

        public virtual Color OptionSectionBackground
        {
            get
            {
                return this.FromKnownColor(KnownThemeColor.OptionSectionBackground);
            }
        }

        public virtual Color OptionSectionGradientBegin
        {
            get
            {
                return this.FromKnownColor(KnownThemeColor.OptionSectionGradientBegin);
            }
        }

        public virtual Color OptionSectionLabelText
        {
            get
            {
                return this.FromKnownColor(KnownThemeColor.OptionSectionLabelText);
            }
        }

        public virtual Color WindowBackground
        {
            get
            {
                return this.FromKnownColor(KnownThemeColor.WindowBackground);
            }
        }

        public virtual Color WindowBorder
        {
            get
            {
                return this.FromKnownColor(KnownThemeColor.WindowBorder);
            }
        }
    }
}

