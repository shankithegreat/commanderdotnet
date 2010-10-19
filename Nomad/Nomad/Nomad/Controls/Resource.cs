namespace Nomad.Controls
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Resources;
    using System.Runtime.CompilerServices;

    [DebuggerNonUserCode, GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0"), CompilerGenerated]
    internal class Resource
    {
        private static CultureInfo resourceCulture;
        private static System.Resources.ResourceManager resourceMan;

        internal Resource()
        {
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static CultureInfo Culture
        {
            get
            {
                return resourceCulture;
            }
            set
            {
                resourceCulture = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    System.Resources.ResourceManager manager = new System.Resources.ResourceManager("Nomad.Controls.Resource", typeof(Resource).Assembly);
                    resourceMan = manager;
                }
                return resourceMan;
            }
        }

        internal static string sAdvancedColors
        {
            get
            {
                return ResourceManager.GetString("sAdvancedColors", resourceCulture);
            }
        }

        internal static string sColorDefault
        {
            get
            {
                return ResourceManager.GetString("sColorDefault", resourceCulture);
            }
        }

        internal static string sColorEmpty
        {
            get
            {
                return ResourceManager.GetString("sColorEmpty", resourceCulture);
            }
        }

        internal static string sMoreColors
        {
            get
            {
                return ResourceManager.GetString("sMoreColors", resourceCulture);
            }
        }

        internal static string sRecentColors
        {
            get
            {
                return ResourceManager.GetString("sRecentColors", resourceCulture);
            }
        }

        internal static string sStandardColors
        {
            get
            {
                return ResourceManager.GetString("sStandardColors", resourceCulture);
            }
        }
    }
}

