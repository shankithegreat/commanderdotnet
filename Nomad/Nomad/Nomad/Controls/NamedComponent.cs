namespace Nomad.Controls
{
    using System;
    using System.ComponentModel;

    [ToolboxItem(false), Designer(typeof(NamedComponentDesigner)), DesignerCategory("Code")]
    public class NamedComponent : Component
    {
        private string FName = string.Empty;

        [DefaultValue(""), Browsable(false)]
        public string Name
        {
            get
            {
                return this.FName;
            }
            set
            {
                this.FName = (value == null) ? string.Empty : value;
            }
        }
    }
}

