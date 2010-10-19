namespace Nomad.Commons.Resources
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows.Forms;

    public class FormStringLocalizer : BasicFormStringLocalizer
    {
        private ComponentResourceManager ResourceManager;

        protected override void AfterLocalizeRootControl()
        {
            this.ResourceManager.ReleaseAllResources();
            this.ResourceManager = null;
        }

        protected override void BeforeLocalizeType(System.Type controlType)
        {
            this.ResourceManager = new ComponentResourceManager(controlType);
        }

        protected override string GetString(string name, CultureInfo culture)
        {
            return this.ResourceManager.GetString(name, culture);
        }

        public static void LocalizeForm(Form form)
        {
            LocalizeForm(form, null);
        }

        public static void LocalizeForm(Form form, CultureInfo culture)
        {
            new FormStringLocalizer().Localize(form, culture);
        }
    }
}

