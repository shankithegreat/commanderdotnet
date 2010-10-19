namespace Nomad.Themes
{
    using System.ComponentModel;
    using System.Drawing;

    public class ListViewDefaultColorTable : ListViewColorTable
    {
        public override Color FromKnownColor(KnownListViewColor knownColor)
        {
            switch (knownColor)
            {
                case KnownListViewColor.Back:
                    return SystemColors.Window;

                case KnownListViewColor.Text:
                    return SystemColors.WindowText;

                case KnownListViewColor.ActiveBack:
                case KnownListViewColor.OddLineBack:
                case KnownListViewColor.FocusedText:
                    return Color.Empty;

                case KnownListViewColor.FocusedBack:
                    return Color.Silver;

                case KnownListViewColor.SelectedText:
                    return Color.Red;
            }
            throw new InvalidEnumArgumentException();
        }
    }
}

