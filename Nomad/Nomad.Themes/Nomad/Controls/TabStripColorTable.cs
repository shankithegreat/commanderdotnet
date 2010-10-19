namespace Nomad.Controls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class TabStripColorTable
    {
        public TabStripColorTable() : this(new ProfessionalColorTable())
        {
        }

        public TabStripColorTable(ProfessionalColorTable table)
        {
            this.ColorTable = table;
        }

        public virtual Color FromKnownColor(KnownTabColor knownColor)
        {
            Color color = this.GetKnownColor(this.MapKnownColor(knownColor));
            if (color.IsEmpty)
            {
                throw new InvalidEnumArgumentException();
            }
            return color;
        }

        protected virtual Color GetKnownColor(KnownTabColor knownColor)
        {
            switch (knownColor)
            {
                case KnownTabColor.TabActiveGradientBegin:
                    return this.ColorTable.ToolStripGradientBegin;

                case KnownTabColor.TabActiveGradientEnd:
                case KnownTabColor.TabStripBottomBorder:
                    return this.ColorTable.ToolStripGradientEnd;

                case KnownTabColor.TabSelectedGradientBegin:
                    return this.ColorTable.ButtonSelectedGradientBegin;

                case KnownTabColor.TabSelectedGradientEnd:
                    return this.ColorTable.ButtonSelectedGradientEnd;

                case KnownTabColor.TabInactiveGradientBegin:
                case KnownTabColor.TabInactiveGradientEnd:
                    return this.ColorTable.MenuStripGradientEnd;

                case KnownTabColor.TabActiveBorderOuter:
                case KnownTabColor.TabInactiveBorderOuter:
                    return this.ColorTable.SeparatorDark;

                case KnownTabColor.TabBorderInner:
                    return this.ColorTable.SeparatorLight;

                case KnownTabColor.TabActiveText:
                case KnownTabColor.TabInactiveText:
                case KnownTabColor.TabSelectedText:
                    return SystemColors.ControlText;
            }
            return Color.Empty;
        }

        protected virtual KnownTabColor MapKnownColor(KnownTabColor knownColor)
        {
            return knownColor;
        }

        public ProfessionalColorTable ColorTable { get; private set; }

        public virtual Color TabActiveBorderOuter
        {
            get
            {
                return this.FromKnownColor(KnownTabColor.TabActiveBorderOuter);
            }
        }

        public virtual Color TabActiveGradientBegin
        {
            get
            {
                return this.FromKnownColor(KnownTabColor.TabActiveGradientBegin);
            }
        }

        public virtual Color TabActiveGradientEnd
        {
            get
            {
                return this.FromKnownColor(KnownTabColor.TabActiveGradientEnd);
            }
        }

        public virtual Color TabActiveText
        {
            get
            {
                return this.FromKnownColor(KnownTabColor.TabActiveText);
            }
        }

        public virtual Color TabBorderInner
        {
            get
            {
                return this.FromKnownColor(KnownTabColor.TabBorderInner);
            }
        }

        public virtual Color TabInactiveBorderOuter
        {
            get
            {
                return this.FromKnownColor(KnownTabColor.TabInactiveBorderOuter);
            }
        }

        public virtual Color TabInactiveGradientBegin
        {
            get
            {
                return this.FromKnownColor(KnownTabColor.TabInactiveGradientBegin);
            }
        }

        public virtual Color TabInactiveGradientEnd
        {
            get
            {
                return this.FromKnownColor(KnownTabColor.TabInactiveGradientEnd);
            }
        }

        public virtual Color TabInactiveText
        {
            get
            {
                return this.FromKnownColor(KnownTabColor.TabInactiveText);
            }
        }

        public virtual Color TabSelectedGradientBegin
        {
            get
            {
                return this.FromKnownColor(KnownTabColor.TabSelectedGradientBegin);
            }
        }

        public virtual Color TabSelectedGradientEnd
        {
            get
            {
                return this.FromKnownColor(KnownTabColor.TabSelectedGradientEnd);
            }
        }

        public virtual Color TabSelectedText
        {
            get
            {
                return this.FromKnownColor(KnownTabColor.TabSelectedText);
            }
        }

        public virtual Color TabStripBottomBorder
        {
            get
            {
                return this.FromKnownColor(KnownTabColor.TabStripBottomBorder);
            }
        }
    }
}

