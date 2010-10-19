namespace Nomad.Commons.Resources
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows.Forms;

    public class FormLocalizer : BasicFormLocalizer
    {
        private ComponentResourceManager ResourceManager;

        protected override void AfterLocalizeRootControl()
        {
            this.ResourceManager.ReleaseAllResources();
            this.ResourceManager = null;
        }

        protected override void ApplyResources(Component component, string componentName, CultureInfo culture)
        {
            this.ResourceManager.ApplyResources(component, componentName, culture);
        }

        protected override void BeforeLocalizeType(System.Type controlType)
        {
            this.ResourceManager = new ComponentResourceManager(controlType);
        }

        public static void LocalizeForm(Form form)
        {
            LocalizeForm(form, null);
        }

        public static void LocalizeForm(Form form, CultureInfo culture)
        {
            new FormLocalizer().Localize(form, culture);
        }
    }
}

