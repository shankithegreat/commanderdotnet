namespace Nomad.Themes
{
    using Nomad.Controls;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;

    public abstract class Theme
    {
        private static Theme FCurrent;
        private static Theme FDefault;

        protected Theme()
        {
        }

        public static Theme CreateTheme(string themeKey)
        {
            ThemeConfigurationSection section = ConfigurationManager.GetSection(themeKey) as ThemeConfigurationSection;
            if ((section == null) && string.Equals(Path.GetExtension(themeKey), ".theme", StringComparison.OrdinalIgnoreCase))
            {
                section = ThemeConfigurationSection.Load(Path.Combine(ThemesDirectory, themeKey));
            }
            if (section != null)
            {
                System.Type type = System.Type.GetType(section.ThemeType);
                if (type != null)
                {
                    if ((type == typeof(ConfigurableTheme)) || type.IsSubclassOf(typeof(ConfigurableTheme)))
                    {
                        return (ConfigurableTheme) Activator.CreateInstance(type, new object[] { section });
                    }
                    return (Theme) Activator.CreateInstance(type);
                }
            }
            return null;
        }

        public static IEnumerable<ThemeConfigurationSection> GetAllThemes()
        {
            return new <GetAllThemes>d__0(-2);
        }

        private static Assembly ResolveThemeAssembly(object sender, ResolveEventArgs e)
        {
            AssemblyName name;
            return Assembly.Load(new AssemblyName(e.Name) { CodeBase = Path.Combine(ThemesDirectory, name.Name) + ".dll" });
        }

        public static void SetTheme(string themeKey)
        {
            Current = CreateTheme(themeKey);
        }

        public static Theme Current
        {
            get
            {
                if (FCurrent == null)
                {
                    Current = Default;
                }
                return FCurrent;
            }
            set
            {
                if ((value != null) && (FCurrent != value))
                {
                    FCurrent = value;
                    if (FCurrent.ToolStripRenderer.GetType() == typeof(ToolStripSystemRenderer))
                    {
                        ToolStripManager.RenderMode = ToolStripManagerRenderMode.System;
                    }
                    else
                    {
                        ToolStripManager.Renderer = FCurrent.ToolStripRenderer;
                    }
                }
            }
        }

        public static Theme Default
        {
            get
            {
                if (FDefault == null)
                {
                    ToolStripProfessionalRenderer renderer;
                    FDefault = new SimpleTheme(new ToolStripProfessionalRenderer { RoundedEdges = false }, new TabStripProfessionalRenderer(new TabStripColorTable(renderer.ColorTable)), ThemeColorTable.Default);
                }
                return FDefault;
            }
        }

        public virtual ListViewColorTable ListViewColors
        {
            get
            {
                return ListViewColorTable.Default;
            }
        }

        public virtual bool SelectedTabBoldFont
        {
            get
            {
                return false;
            }
        }

        public virtual TabStripColorTable TabStripColors
        {
            get
            {
                TabStripProfessionalRenderer tabStripRenderer = this.TabStripRenderer as TabStripProfessionalRenderer;
                return ((tabStripRenderer != null) ? tabStripRenderer.TabColorTable : null);
            }
        }

        public virtual FontFamily TabStripFontFamily
        {
            get
            {
                return null;
            }
        }

        public abstract System.Windows.Forms.ToolStripRenderer TabStripRenderer { get; }

        public abstract ThemeColorTable ThemeColors { get; }

        public static string ThemesDirectory
        {
            get
            {
                return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Themes");
            }
        }

        public virtual ProfessionalColorTable ToolStripColors
        {
            get
            {
                ToolStripProfessionalRenderer toolStripRenderer = this.ToolStripRenderer as ToolStripProfessionalRenderer;
                if (toolStripRenderer == null)
                {
                    ToolStripWrapperRenderer renderer2 = this.ToolStripRenderer as ToolStripWrapperRenderer;
                    if (renderer2 != null)
                    {
                        toolStripRenderer = renderer2.BaseRenderer as ToolStripProfessionalRenderer;
                    }
                }
                return ((toolStripRenderer != null) ? toolStripRenderer.ColorTable : null);
            }
        }

        public virtual FontFamily ToolStripFontFamily
        {
            get
            {
                return null;
            }
        }

        public abstract System.Windows.Forms.ToolStripRenderer ToolStripRenderer { get; }

        [CompilerGenerated]
        private sealed class <GetAllThemes>d__0 : IEnumerable<ThemeConfigurationSection>, IEnumerable, IEnumerator<ThemeConfigurationSection>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private ThemeConfigurationSection <>2__current;
            public IEnumerator <>7__wrap7;
            public IDisposable <>7__wrap8;
            public string[] <>7__wrapb;
            public int <>7__wrapc;
            private int <>l__initialThreadId;
            public System.Configuration.Configuration <Config>5__1;
            public ConfigurationSection <NextSection>5__3;
            public ThemeConfigurationSection <NextTheme>5__4;
            public string <NextThemeFile>5__5;
            public ThemeConfigurationSection <Section>5__6;
            public ConfigurationSectionGroup <SectionGroup>5__2;

            [DebuggerHidden]
            public <GetAllThemes>d__0(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally9()
            {
                this.<>1__state = -1;
                this.<>7__wrap8 = this.<>7__wrap7 as IDisposable;
                if (this.<>7__wrap8 != null)
                {
                    this.<>7__wrap8.Dispose();
                }
            }

            private void <>m__Finallya()
            {
                this.<>1__state = -1;
            }

            private bool MoveNext()
            {
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            this.<>1__state = -1;
                            this.<Config>5__1 = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
                            this.<SectionGroup>5__2 = this.<Config>5__1.GetSectionGroup("themes");
                            if (this.<SectionGroup>5__2 != null)
                            {
                                this.<>7__wrap7 = this.<SectionGroup>5__2.Sections.GetEnumerator();
                                this.<>1__state = 1;
                                while (this.<>7__wrap7.MoveNext())
                                {
                                    this.<NextSection>5__3 = (ConfigurationSection) this.<>7__wrap7.Current;
                                    this.<NextTheme>5__4 = this.<NextSection>5__3 as ThemeConfigurationSection;
                                    if (this.<NextTheme>5__4 == null)
                                    {
                                        goto Label_00E5;
                                    }
                                    this.<>2__current = this.<NextTheme>5__4;
                                    this.<>1__state = 2;
                                    return true;
                                Label_00DE:
                                    this.<>1__state = 1;
                                Label_00E5:;
                                }
                                this.<>m__Finally9();
                            }
                            this.<>1__state = 3;
                            this.<>7__wrapb = Directory.GetFiles(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Themes"), "*.theme");
                            this.<>7__wrapc = 0;
                            while (this.<>7__wrapc < this.<>7__wrapb.Length)
                            {
                                this.<NextThemeFile>5__5 = this.<>7__wrapb[this.<>7__wrapc];
                                this.<Section>5__6 = ThemeConfigurationSection.Load(this.<NextThemeFile>5__5);
                                this.<>2__current = this.<Section>5__6;
                                this.<>1__state = 4;
                                return true;
                            Label_0172:
                                this.<>1__state = 3;
                                this.<>7__wrapc++;
                            }
                            this.<>m__Finallya();
                            break;

                        case 2:
                            goto Label_00DE;

                        case 4:
                            goto Label_0172;
                    }
                    return false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
            }

            [DebuggerHidden]
            IEnumerator<ThemeConfigurationSection> IEnumerable<ThemeConfigurationSection>.GetEnumerator()
            {
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    return this;
                }
                return new Theme.<GetAllThemes>d__0(0);
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<Nomad.Themes.ThemeConfigurationSection>.GetEnumerator();
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
                switch (this.<>1__state)
                {
                    case 1:
                    case 2:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finally9();
                        }
                        break;

                    case 3:
                    case 4:
                        this.<>m__Finallya();
                        break;
                }
            }

            ThemeConfigurationSection IEnumerator<ThemeConfigurationSection>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }
        }
    }
}

