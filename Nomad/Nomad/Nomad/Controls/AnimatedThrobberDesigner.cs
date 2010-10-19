namespace Nomad.Controls
{
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    public class AnimatedThrobberDesigner : ControlDesigner
    {
        public override System.Windows.Forms.Design.SelectionRules SelectionRules
        {
            get
            {
                return (base.SelectionRules & ~(((Control) base.Component).AutoSize ? 15 : 0));
            }
        }
    }
}

