namespace Nomad.FileSystem.Property.Providers.Wdx
{
    using Nomad.Commons.IO;
    using Nomad.Commons.Resources;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Resources;
    using System.Text;

    public class LngResourceManager : ResourceManager
    {
        private static Dictionary<CultureInfo, string> CultureLanguageMap;
        private Ini FLngIni;
        private string FLngPath;

        public LngResourceManager(string lngPath)
        {
            this.FLngPath = lngPath;
            base.ResourceSets = new Hashtable();
        }

        private string GetLanguage(CultureInfo info)
        {
            string str;
            if (CultureLanguageMap == null)
            {
                CultureLanguageMap = new Dictionary<CultureInfo, string>(0x12);
                CultureLanguageMap.Add(CultureInfo.GetCultureInfo("en"), "ENG");
                CultureLanguageMap.Add(CultureInfo.GetCultureInfo("zh-CHS"), "CHN");
                CultureLanguageMap.Add(CultureInfo.GetCultureInfo("cs"), "CZ");
                CultureLanguageMap.Add(CultureInfo.GetCultureInfo("da"), "DAN");
                CultureLanguageMap.Add(CultureInfo.GetCultureInfo("de"), "DEU");
                CultureLanguageMap.Add(CultureInfo.GetCultureInfo("nl"), "DUT");
                CultureLanguageMap.Add(CultureInfo.GetCultureInfo("es"), "ESP");
                CultureLanguageMap.Add(CultureInfo.GetCultureInfo("fr"), "FRA");
                CultureLanguageMap.Add(CultureInfo.GetCultureInfo("hu"), "HUN");
                CultureLanguageMap.Add(CultureInfo.GetCultureInfo("it"), "ITA");
                CultureLanguageMap.Add(CultureInfo.GetCultureInfo("ko"), "KOR");
                CultureLanguageMap.Add(CultureInfo.GetCultureInfo("no"), "NOR");
                CultureLanguageMap.Add(CultureInfo.GetCultureInfo("pl"), "POL");
                CultureLanguageMap.Add(CultureInfo.GetCultureInfo("ro"), "ROM");
                CultureLanguageMap.Add(CultureInfo.GetCultureInfo("ru"), "RUS");
                CultureLanguageMap.Add(CultureInfo.GetCultureInfo("sk"), "SK");
                CultureLanguageMap.Add(CultureInfo.GetCultureInfo("sl"), "SVN");
                CultureLanguageMap.Add(CultureInfo.GetCultureInfo("sv"), "SWE");
            }
            if (CultureLanguageMap.TryGetValue(info, out str))
            {
                return str;
            }
            return null;
        }

        protected override string GetResourceFileName(CultureInfo culture)
        {
            return this.FLngPath;
        }

        protected override ResourceSet InternalGetResourceSet(CultureInfo culture, bool createIfNotExists, bool tryParents)
        {
            ResourceSet set = (ResourceSet) base.ResourceSets[culture];
            if (set == null)
            {
                if (this.FLngIni == null)
                {
                    this.FLngIni = new Ini();
                    using (TextReader reader = new StreamReader(this.FLngPath, Encoding.Default, true))
                    {
                        this.FLngIni.Read(reader);
                    }
                }
                string language = this.GetLanguage(culture);
                if (!((!string.IsNullOrEmpty(language) && (language != "ENG")) && this.FLngIni.ContainsSection(language)))
                {
                    language = "ENG";
                }
                if (!this.FLngIni.ContainsSection(language) && tryParents)
                {
                    CultureInfo parent = culture.Parent;
                    if (parent != CultureInfo.InvariantCulture)
                    {
                        return this.InternalGetResourceSet(parent, createIfNotExists, tryParents);
                    }
                }
                set = new IniResourceSet(this.FLngIni, language);
                base.ResourceSets.Add(culture, set);
            }
            return set;
        }

        public override void ReleaseAllResources()
        {
            this.FLngIni = null;
            base.ReleaseAllResources();
        }

        public override Type ResourceSetType
        {
            get
            {
                return typeof(IniResourceSet);
            }
        }
    }
}

