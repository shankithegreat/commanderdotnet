namespace Nomad.FileSystem.Archive.Common
{
    using Nomad;
    using System;
    using System.Configuration;

    public abstract class PersistArchiveFormatInfo : ArchiveFormatInfo, IPersistComponentSettings
    {
        private bool FModified;

        protected PersistArchiveFormatInfo()
        {
        }

        protected override void Changed()
        {
            this.FModified = true;
        }

        public void LoadComponentSettings()
        {
            ArchiveFormatSettings settings = SettingsManager.GetSettings<ArchiveFormatSettings>(this.SettingsKey);
            if (settings == null)
            {
                settings = new ArchiveFormatSettings {
                    SettingsKey = this.SettingsKey
                };
                settings.Reload();
            }
            if (settings.Initialized)
            {
                base.BeginInit();
                try
                {
                    this.OnLoadComponentSettings(settings);
                }
                finally
                {
                    base.EndInit();
                }
            }
            this.FModified = false;
        }

        protected virtual void OnLoadComponentSettings(ArchiveFormatSettings settings)
        {
            base.Disabled = settings.Disabled;
            base.HideFormat = settings.HideFormat;
            string[] extensionList = settings.ExtensionList;
            if (extensionList != null)
            {
                base.Extension = extensionList;
            }
        }

        protected virtual void OnResetComponentSettings()
        {
            base.HideFormat = false;
        }

        protected virtual void OnSaveComponentSettings(ArchiveFormatSettings settings)
        {
            settings.Disabled = base.Disabled;
            settings.HideFormat = base.HideFormat;
            settings.ExtensionList = base.Extension;
        }

        public void ResetComponentSettings()
        {
            base.BeginInit();
            try
            {
                this.OnResetComponentSettings();
            }
            finally
            {
                base.EndInit();
            }
            this.FModified = false;
        }

        public void SaveComponentSettings()
        {
            ArchiveFormatSettings settings = new ArchiveFormatSettings {
                SettingsKey = this.SettingsKey,
                Initialized = true
            };
            this.OnSaveComponentSettings(settings);
            SettingsManager.RegisterSettings(settings);
            this.FModified = false;
        }

        public bool SaveSettings
        {
            get
            {
                return this.FModified;
            }
            set
            {
                this.FModified = true;
            }
        }

        public string SettingsKey
        {
            get
            {
                return this.Name;
            }
            set
            {
            }
        }
    }
}

