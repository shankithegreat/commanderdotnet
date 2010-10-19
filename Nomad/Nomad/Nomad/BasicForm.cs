namespace Nomad
{
    using Nomad.Commons.Resources;
    using Nomad.Configuration;
    using Nomad.Controls;
    using Nomad.Properties;
    using Nomad.Themes;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Reflection;
    using System.Resources;
    using System.Windows.Forms;

    public class BasicForm : FormEx
    {
        private AdvancedToolTip FInfoTipProvider;
        private ResourceManager FInfoTipResMan;
        private MouseWheelHelper FMouseWheelFix;
        private bool IsFormLocalized;

        public BasicForm()
        {
            base.ShowIcon = false;
            base.BackColor = Theme.Current.ThemeColors.DialogBackground;
        }

        public static void CheckForDuplicateMnemonics(Control container)
        {
            Dictionary<char, Control> dictionary = new Dictionary<char, Control>();
            Stack<Control> stack = new Stack<Control>();
            if (container.Controls.Count > 0)
            {
                stack.Push(container);
            }
            while (stack.Count > 0)
            {
                foreach (Control control in stack.Pop().Controls)
                {
                    Label label = control as Label;
                    bool flag = (label != null) && label.UseMnemonic;
                    Button button = control as Button;
                    if (button != null)
                    {
                        Form form = button.FindForm();
                        flag = ((button.Text.IndexOf('&') >= 0) || (form == null)) || ((form.AcceptButton != button) && (form.CancelButton != button));
                        if (flag && (button.FlatStyle == FlatStyle.System))
                        {
                            button.FlatStyle = FlatStyle.Standard;
                        }
                    }
                    if ((control is CheckBox) || (control is RadioButton))
                    {
                        flag = true;
                    }
                    if (flag)
                    {
                        char mnemonic = GetMnemonic(control.Text);
                        if (mnemonic != '\0')
                        {
                            Control control2;
                            if (dictionary.TryGetValue(mnemonic, out control2))
                            {
                                control2.BackColor = Color.Red;
                                control.BackColor = Color.Red;
                            }
                            else
                            {
                                dictionary.Add(mnemonic, control);
                            }
                        }
                        else
                        {
                            control.BackColor = Color.Olive;
                        }
                    }
                    if (control.Controls.Count > 0)
                    {
                        stack.Push(control);
                    }
                }
            }
        }

        public static void CheckForDuplicateMnemonics(ToolStripItemCollection collection)
        {
            Dictionary<char, ToolStripItem> dictionary = new Dictionary<char, ToolStripItem>();
            foreach (ToolStripItem item in collection)
            {
                if ((item is ToolStripSeparator) || (item.DisplayStyle == ToolStripItemDisplayStyle.Image))
                {
                    continue;
                }
                char mnemonic = GetMnemonic(item.Text);
                if (mnemonic != '\0')
                {
                    ToolStripItem item2;
                    if (dictionary.TryGetValue(mnemonic, out item2))
                    {
                        item2.BackColor = Color.Red;
                        item.BackColor = Color.Red;
                    }
                    else
                    {
                        dictionary.Add(mnemonic, item);
                    }
                }
                else
                {
                    item.BackColor = Color.Olive;
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.FInfoTipProvider != null)
                {
                    this.FInfoTipProvider.Dispose();
                    this.FInfoTipProvider = null;
                }
                if (this.FMouseWheelFix != null)
                {
                    this.FMouseWheelFix.Dispose();
                    this.FMouseWheelFix = null;
                }
            }
            base.Dispose(disposing);
        }

        protected void FixChildrenMouseWheel(Control parent)
        {
            this.FMouseWheelFix.ApplyToChildren(parent, new Predicate<Control>(this.MouseWheelFixNeeded));
        }

        private static char GetMnemonic(string str)
        {
            int index = str.IndexOf('&');
            if (index < 0)
            {
                return '\0';
            }
            return str.Substring(index + 1, 1).ToUpper()[0];
        }

        private void InitializeContainerInfoTips(Control container, ResourceManager manager)
        {
            Stack<Control> stack = new Stack<Control>();
            if (container.Controls.Count > 0)
            {
                stack.Push(container);
            }
            string name = container.GetType().Name;
            while (stack.Count > 0)
            {
                ArrayList list = new ArrayList(stack.Pop().Controls);
                foreach (Control control in list)
                {
                    if (control.Controls.Count > 0)
                    {
                        if (control is UserControl)
                        {
                            this.InitializeContainerInfoTips(control, manager);
                        }
                        else
                        {
                            stack.Push(control);
                        }
                    }
                    else
                    {
                        this.SetControlInfoTip(control, name, manager);
                    }
                }
            }
        }

        protected virtual void InitializeInfoTips(Control container)
        {
            this.InitializeContainerInfoTips(container, this.InfoTipResourceManager);
        }

        protected void LocalizeForm()
        {
            if (!this.IsFormLocalized)
            {
                BasicFormLocalizer argument = SettingsManager.GetArgument<BasicFormLocalizer>(ArgumentKey.FormLocalizer);
                if (argument != null)
                {
                    argument.Localize(this);
                }
                this.IsFormLocalized = true;
            }
        }

        protected virtual bool MouseWheelFixNeeded(Control ctrl)
        {
            if (ctrl is ScrollableControl)
            {
                return false;
            }
            TextBox box = ctrl as TextBox;
            if (!((box == null) || box.Multiline))
            {
                return false;
            }
            return true;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!base.DesignMode)
            {
                if (base.DataBindings.Count == 0)
                {
                    base.DataBindings.Add(new Binding("Font", FormSettings.Default, "DialogFont", false, DataSourceUpdateMode.Never));
                }
                if (!this.ShowIcon)
                {
                    Icon icon = IconSet.GetIcon(base.Name + ".Icon");
                    if (icon != null)
                    {
                        base.Icon = icon;
                        this.ShowIcon = true;
                    }
                }
                this.LocalizeForm();
                this.InitializeInfoTips(this);
                if (this.FMouseWheelFix != null)
                {
                    this.FMouseWheelFix.SetFixMouseWheel(this, true);
                    this.FixChildrenMouseWheel(this);
                }
                if (SettingsManager.GetArgument<bool>(ArgumentKey.Debug))
                {
                    CheckForDuplicateMnemonics(this);
                }
                this.OnThemeChanged(EventArgs.Empty);
            }
            base.OnLoad(e);
        }

        private void SetControlInfoTip(Control control, string parentName, ResourceManager manager)
        {
            string str = manager.GetString(parentName + "." + control.Name);
            while (!string.IsNullOrEmpty(str) && (str[0] == '>'))
            {
                str = manager.GetString(str.Substring(1));
            }
            if (!string.IsNullOrEmpty(str))
            {
                this.InfoTipProvider.SetAdvancedToolTip(control, str);
            }
        }

        [DefaultValue(false)]
        public bool FixMouseWheel
        {
            get
            {
                return (this.FMouseWheelFix != null);
            }
            set
            {
                if (value)
                {
                    if (this.FMouseWheelFix == null)
                    {
                        this.FMouseWheelFix = new MouseWheelHelper();
                    }
                }
                else
                {
                    if (this.FMouseWheelFix != null)
                    {
                        this.FMouseWheelFix.Dispose();
                    }
                    this.FMouseWheelFix = null;
                }
            }
        }

        protected AdvancedToolTip InfoTipProvider
        {
            get
            {
                if (this.FInfoTipProvider == null)
                {
                    this.FInfoTipProvider = new AdvancedToolTip();
                }
                return this.FInfoTipProvider;
            }
        }

        private ResourceManager InfoTipResourceManager
        {
            get
            {
                if (this.FInfoTipResMan == null)
                {
                    this.FInfoTipResMan = new SettingsManager.LocalizedResourceManager("Nomad.Properties.InfoTips", Assembly.GetExecutingAssembly());
                }
                return this.FInfoTipResMan;
            }
        }

        [DefaultValue(false)]
        public bool ShowIcon
        {
            get
            {
                return base.ShowIcon;
            }
            set
            {
                base.ShowIcon = value;
            }
        }
    }
}

