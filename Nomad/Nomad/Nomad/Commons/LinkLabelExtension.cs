namespace Nomad.Commons
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public static class LinkLabelExtension
    {
        public static void ParseLinks(this LinkLabel label)
        {
            string text = label.Text;
            for (int i = text.IndexOf('^'); i >= 0; i = text.IndexOf('^'))
            {
                text = text.Remove(i, 1);
                int index = text.IndexOf('^', i);
                if (index >= 0)
                {
                    text = text.Remove(index, 1);
                    label.Links.Add(new LinkLabel.Link(i, index - i));
                }
            }
            label.Text = text;
        }
    }
}

