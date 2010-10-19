namespace Nomad.Dialogs
{
    using Nomad;
    using Nomad.Commons.Controls;
    using Nomad.Themes;
    using System;
    using System.Diagnostics;
    using System.Windows.Forms;

    public class BasicDialog : BasicForm
    {
        protected override void OnThemeChanged(EventArgs e)
        {
            Panel panel = base.Controls["tlpButtons"] as Panel;
            if (panel != null)
            {
                panel.BackColor = Theme.Current.ThemeColors.DialogButtonsBackground;
            }
            else
            {
                Debug.WriteLine("tlpButtons not found in BasicDialog");
            }
            Bevel control = base.Controls["bvlButtons"] as Bevel;
            if (control != null)
            {
                UpdateBevel(control);
            }
            else
            {
                Debug.WriteLine("bvlButtons not found in BasicDialog");
            }
            base.OnThemeChanged(e);
        }

        protected static void UpdateBevel(Bevel control)
        {
            control.Style = Application.RenderWithVisualStyles ? Border3DStyle.Flat : Border3DStyle.Etched;
            control.Visible = (control.Style != Border3DStyle.Flat) || (control.ForeColor.A != 0);
        }
    }
}

