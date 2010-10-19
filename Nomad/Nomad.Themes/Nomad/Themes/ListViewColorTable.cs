namespace Nomad.Themes
{
    using System;
    using System.Drawing;

    public abstract class ListViewColorTable
    {
        public static readonly ListViewColorTable Default = new ListViewDefaultColorTable();

        protected ListViewColorTable()
        {
        }

        public abstract Color FromKnownColor(KnownListViewColor knownColor);

        public virtual Color ActiveBack
        {
            get
            {
                return this.FromKnownColor(KnownListViewColor.ActiveBack);
            }
        }

        public virtual Color Back
        {
            get
            {
                return this.FromKnownColor(KnownListViewColor.Back);
            }
        }

        public virtual Color FocusedBack
        {
            get
            {
                return this.FromKnownColor(KnownListViewColor.FocusedBack);
            }
        }

        public virtual Color FocusedText
        {
            get
            {
                return this.FromKnownColor(KnownListViewColor.FocusedText);
            }
        }

        public virtual Color OddLineBack
        {
            get
            {
                return this.FromKnownColor(KnownListViewColor.OddLineBack);
            }
        }

        public virtual Color SelectedText
        {
            get
            {
                return this.FromKnownColor(KnownListViewColor.SelectedText);
            }
        }

        public virtual Color Text
        {
            get
            {
                return this.FromKnownColor(KnownListViewColor.Text);
            }
        }
    }
}

