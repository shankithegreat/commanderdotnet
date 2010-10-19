namespace Nomad.Configuration
{
    using Nomad;
    using System;
    using System.CodeDom.Compiler;
    using System.Configuration;
    using System.Diagnostics;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0"), SettingsProvider(typeof(ConfigurableSettingsProvider)), CompilerGenerated]
    internal sealed class FormSettings : ApplicationSettingsBase
    {
        private static FormSettings defaultInstance = ((FormSettings) SettingsBase.Synchronized(new FormSettings()));
        private Form OwnedForm;
        private const string PropertyDialogFont = "DialogFont";

        private void FormDisposed(object sender, EventArgs e)
        {
            Form senderForm = (Form) sender;
            if (senderForm == this.OwnedForm)
            {
                this.OwnedForm = null;
            }
            this.SavePlacement(senderForm);
        }

        public static FormSettings RegisterForm(Form form)
        {
            return RegisterForm(form, ~FormPlacement.None);
        }

        public static FormSettings RegisterForm(Form form, FormPlacement restorePlacement)
        {
            if (form == null)
            {
                throw new ArgumentNullException();
            }
            bool flag = false;
            FormSettings settings = SettingsManager.GetSettings<FormSettings>(form.Name);
            if (settings == null)
            {
                settings = new FormSettings();
                flag = true;
            }
            settings.SettingsKey = form.Name;
            System.Diagnostics.Debug.Assert(settings.OwnedForm == null);
            settings.OwnedForm = form;
            if (restorePlacement != FormPlacement.None)
            {
                if (flag)
                {
                    settings.Reload();
                }
                settings.RestorePlacement(form, restorePlacement);
            }
            form.Disposed += new EventHandler(settings.FormDisposed);
            return settings;
        }

        public void RestorePlacement()
        {
            this.RestorePlacement(this.OwnedForm, ~FormPlacement.None);
        }

        public void RestorePlacement(FormPlacement restorePlacement)
        {
            this.RestorePlacement(this.OwnedForm, restorePlacement);
        }

        private void RestorePlacement(Form senderForm, FormPlacement restore)
        {
            senderForm.SuspendLayout();
            if (((restore & FormPlacement.Location) > FormPlacement.None) && (this["Location"] != null))
            {
                Screen screen = Screen.FromPoint(this.Location);
                if ((screen != null) && screen.Bounds.Contains(this.Location))
                {
                    senderForm.StartPosition = FormStartPosition.Manual;
                    senderForm.Location = this.Location;
                }
            }
            if (((restore & FormPlacement.Size) > FormPlacement.None) && (this["Size"] != null))
            {
                senderForm.Size = this.Size;
            }
            if ((restore & FormPlacement.WindowState) > FormPlacement.None)
            {
                senderForm.WindowState = this.WindowState;
            }
            senderForm.ResumeLayout();
        }

        public void SavePlacement()
        {
            this.SavePlacement(this.OwnedForm);
        }

        private void SavePlacement(Form senderForm)
        {
            this.WindowState = (senderForm.WindowState == FormWindowState.Minimized) ? FormWindowState.Normal : senderForm.WindowState;
            if (senderForm.WindowState == FormWindowState.Normal)
            {
                this.Location = senderForm.Location;
                this.Size = senderForm.Size;
            }
            else
            {
                this.Location = senderForm.RestoreBounds.Location;
                this.Size = senderForm.RestoreBounds.Size;
            }
            SettingsManager.RegisterSettings(this);
        }

        public static FormSettings Default
        {
            get
            {
                return defaultInstance;
            }
        }

        [UserScopedSetting]
        public Font DialogFont
        {
            get
            {
                Font dialogFont = (Font) this["DialogFont"];
                if (dialogFont == null)
                {
                    dialogFont = SystemFonts.DialogFont;
                }
                return dialogFont;
            }
            set
            {
                if ((value != null) || (this["DialogFont"] != null))
                {
                    this["DialogFont"] = value;
                }
            }
        }

        public bool DialogFontEnabled
        {
            get
            {
                return (this["DialogFont"] != null);
            }
        }

        [DebuggerNonUserCode, UserScopedSetting]
        public Point Location
        {
            get
            {
                return (Point) this["Location"];
            }
            set
            {
                this["Location"] = value;
            }
        }

        [UserScopedSetting, DebuggerNonUserCode]
        public System.Drawing.Size Size
        {
            get
            {
                return (System.Drawing.Size) this["Size"];
            }
            set
            {
                this["Size"] = value;
            }
        }

        [UserScopedSetting, DefaultSettingValue("Normal"), DebuggerNonUserCode]
        public FormWindowState WindowState
        {
            get
            {
                return (FormWindowState) this["WindowState"];
            }
            set
            {
                this["WindowState"] = value;
            }
        }
    }
}

