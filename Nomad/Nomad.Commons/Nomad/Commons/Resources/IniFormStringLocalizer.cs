namespace Nomad.Commons.Resources
{
    using Nomad.Commons.IO;
    using System;
    using System.Globalization;
    using System.Resources;

    public class IniFormStringLocalizer : BasicFormStringLocalizer
    {
        private string FIniPath;
        private Ini FIniSource;
        private System.Resources.ResourceSet ResourceSet;

        public IniFormStringLocalizer(Ini iniSource)
        {
            if (iniSource == null)
            {
                throw new ArgumentNullException();
            }
            this.FIniSource = iniSource;
        }

        public IniFormStringLocalizer(string iniPath)
        {
            if (iniPath == null)
            {
                throw new ArgumentNullException();
            }
            if (iniPath == string.Empty)
            {
                throw new ArgumentException();
            }
            this.FIniPath = iniPath;
        }

        public IniFormStringLocalizer(Ini iniSource, string iniPath)
        {
            if ((iniSource == null) && string.IsNullOrEmpty(iniPath))
            {
                throw new ArgumentException();
            }
            this.FIniSource = iniSource;
            this.FIniPath = iniPath;
        }

        protected override void AfterLocalizeRootControl()
        {
            this.ResourceSet.Close();
            this.ResourceSet = null;
        }

        protected override void BeforeLocalizeType(Type controlType)
        {
            if (this.FIniSource != null)
            {
                this.ResourceSet = new IniResourceSet(this.FIniSource, controlType);
            }
            else if (this.FIniPath != null)
            {
                this.ResourceSet = new IniResourceSet(this.FIniPath, controlType);
            }
        }

        protected override string GetString(string name, CultureInfo culture)
        {
            return this.ResourceSet.GetString(name);
        }

        public string IniPath
        {
            get
            {
                return this.FIniPath;
            }
        }

        public Ini IniSource
        {
            get
            {
                return this.FIniSource;
            }
        }
    }
}

