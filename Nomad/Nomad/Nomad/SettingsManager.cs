namespace Nomad
{
    using Microsoft;
    using Microsoft.COM;
    using Microsoft.IO;
    using Microsoft.Shell;
    using Microsoft.Win32;
    using Nomad.Commons;
    using Nomad.Commons.Collections;
    using Nomad.Commons.IO;
    using Nomad.Commons.Resources;
    using Nomad.Configuration;
    using Nomad.Controls;
    using Nomad.FileSystem.Archive.Common;
    using Nomad.FileSystem.Archive.SevenZip;
    using Nomad.FileSystem.Ftp;
    using Nomad.FileSystem.LocalFile;
    using Nomad.FileSystem.Properties;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using Nomad.FileSystem.Virtual.Filter;
    using Nomad.Globalization;
    using Nomad.Properties;
    using Nomad.Workers;
    using Nomad.Workers.Configuration;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Resources;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;

    internal static class SettingsManager
    {
        private static Dictionary<ArgumentKey, object> Arguments;
        private const string CategorySettings = "Settings";
        private const string DefaultCulture = "en";
        public static string IconCacheName = "icon.cache";
        private const string LicenseTxt = "license.txt";
        private static Dictionary<string, ApplicationSettingsBase> SettingsMap = new Dictionary<string, ApplicationSettingsBase>();

        public static bool CheckSafeMode(SafeMode value)
        {
            return ((((SafeMode) GetArgument<SafeMode>(ArgumentKey.Safe)) & value) > 0);
        }

        private static bool CompressFiles()
        {
            bool flag = false;
            if ((VolumeCache.FromPath(Application.ExecutablePath).Capabilities & VolumeCapabilities.FileCompression) > ((VolumeCapabilities) 0))
            {
                foreach (string str in System.IO.Directory.GetFiles(Application.StartupPath, "*.pdb"))
                {
                    Microsoft.IO.File.SetCompressedState(str, CompressionFormat.Default);
                    flag = true;
                }
            }
            return flag;
        }

        private static void CreateDesktopShortcut()
        {
            ShellLink link = new ShellLink {
                Path = Application.ExecutablePath
            };
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "Nomad.NET") + ".lnk";
            int num = 2;
            while (System.IO.File.Exists(path))
            {
                path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), string.Format(Nomad.Properties.Settings.Default.AnotherLinkPattern, "Nomad.NET", ".lnk", num++));
            }
            link.Save(path);
        }

        private static string CreateSettingsKey(System.Type settingsType, string settingsKey)
        {
            if (string.IsNullOrEmpty(settingsKey))
            {
                return settingsType.FullName;
            }
            return (settingsType.FullName + '.' + settingsKey);
        }

        public static T GetArgument<T>(ArgumentKey key)
        {
            object obj2;
            if ((Arguments != null) && Arguments.TryGetValue(key, out obj2))
            {
                return (T) obj2;
            }
            return default(T);
        }

        public static IDictionary<ArgumentKey, object> GetFolderArguments()
        {
            Dictionary<ArgumentKey, object> dictionary = new Dictionary<ArgumentKey, object>();
            if (Arguments != null)
            {
                foreach (KeyValuePair<ArgumentKey, object> pair in Arguments)
                {
                    switch (pair.Key)
                    {
                        case ArgumentKey.Tab:
                        case ArgumentKey.LeftFolder:
                        case ArgumentKey.RightFolder:
                        case ArgumentKey.CurrentFolder:
                        case ArgumentKey.FarFolder:
                        {
                            dictionary.Add(pair.Key, pair.Value);
                            continue;
                        }
                    }
                }
            }
            return dictionary;
        }

        public static IEnumerable<CultureInfo> GetInstalledCultures()
        {
            return new <GetInstalledCultures>d__4(-2);
        }

        public static IList<PreviousVersionConfig> GetPreviousVersions()
        {
            List<PreviousVersionConfig> list = new List<PreviousVersionConfig>();
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            Stack<string> stack = new Stack<string>();
            stack.Push(Path.GetDirectoryName(Path.GetDirectoryName(ConfigurableSettingsProvider.GetDefaultExeConfigPath(ConfigurationUserLevel.PerUserRoaming))));
            stack.Push(Path.GetDirectoryName(Path.GetDirectoryName(ConfigurableSettingsProvider.GetDefaultExeConfigPath(ConfigurationUserLevel.PerUserRoamingAndLocal))));
            stack.Push(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"BMG\Nomad.exe_StrongName_55me5n3ht4bso0ikkq3xzzhdtq254fxm"));
            while (stack.Count > 0)
            {
                string path = stack.Pop();
                if (System.IO.Directory.Exists(path))
                {
                    foreach (string str2 in System.IO.Directory.GetDirectories(path))
                    {
                        string str3 = Path.Combine(str2, "user.config");
                        if (System.IO.File.Exists(str3))
                        {
                            try
                            {
                                Version version2 = new Version(Path.GetFileName(str2));
                                if (version2 != version)
                                {
                                    list.Add(new PreviousVersionConfig(version2, str3));
                                }
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }
            list.Sort(delegate (PreviousVersionConfig x, PreviousVersionConfig y) {
                return -x.Version.CompareTo(y.Version);
            });
            return list;
        }

        public static T GetSettings<T>(string settingKey) where T: ApplicationSettingsBase
        {
            ApplicationSettingsBase base2;
            if (SettingsMap.TryGetValue(CreateSettingsKey(typeof(T), settingKey), out base2))
            {
                return (T) base2;
            }
            return default(T);
        }

        private static void ImageProviderCreated(object sender, EventArgs e)
        {
            if (!((!(sender is CustomImageProvider) || !Nomad.Properties.Settings.Default.PersistentIconCache) || CheckSafeMode(SafeMode.SkipIconCache)))
            {
                LoadIconCache();
            }
        }

        public static void Initialize()
        {
            ImageProvider.DefaultImageProviderCreated = (EventHandler) Delegate.Combine(ImageProvider.DefaultImageProviderCreated, new EventHandler(SettingsManager.ImageProviderCreated));
            RegisterSettings(VirtualFilePanelSettings.Default);
            RegisterSettings(ToolbarSettings.Default);
            RegisterSettings(FtpSettings.Default);
            RegisterSettings(CopySettings.Default);
            RegisterSettings(HistorySettings.Default);
            RegisterSettings(ConfirmationSettings.Default);
            RegisterSettings(FormSettings.Default);
            RegisterSettings(ArchiveFormatSettings.Default);
            RegisterSettings(Nomad.FileSystem.Properties.Settings.Default);
            RegisterSettings(Nomad.Properties.Settings.Default);
        }

        public static void InitializeGlobalization()
        {
            if (GetArgument<IniFormStringLocalizer>(ArgumentKey.FormLocalizer) != null)
            {
                System.Type type = typeof(Resources);
                type.GetField("resourceMan", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, new LocalizedResourceManager("Nomad.Properties.Resources", type.Assembly));
                type = typeof(Enums);
                type.GetField("resourceMan", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, new LocalizedResourceManager("Nomad.Properties.Enums", type.Assembly));
                type = typeof(Resource);
                type.GetField("resourceMan", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, new LocalizedResourceManager("Nomad.Controls.Resource", type.Assembly));
            }
            PluralInfo.Current = new PluralInfo(Resources.PluralForms);
            TypeConverterAttribute attribute = new TypeConverterAttribute(typeof(LocalizedEnumConverter));
            TypeDescriptor.AddAttributes(typeof(InitTask), new Attribute[] { attribute });
            TypeDescriptor.AddAttributes(typeof(InitTaskResult), new Attribute[] { attribute });
            TypeDescriptor.AddAttributes(typeof(FormWindowState), new Attribute[] { attribute });
            TypeDescriptor.AddAttributes(typeof(CharacterCasing), new Attribute[] { attribute });
            TypeDescriptor.AddAttributes(typeof(AutoCompleteMode), new Attribute[] { attribute });
            TypeDescriptor.AddAttributes(typeof(PanelView), new Attribute[] { attribute });
            TypeDescriptor.AddAttributes(typeof(LightSource), new Attribute[] { attribute });
            TypeDescriptor.AddAttributes(typeof(VolumeType), new Attribute[] { attribute });
            TypeDescriptor.AddAttributes(typeof(AutoRefreshMode), new Attribute[] { attribute });
            TypeDescriptor.AddAttributes(typeof(CompressionLevel), new Attribute[] { attribute });
            TypeDescriptor.AddAttributes(typeof(EncryptionMethod), new Attribute[] { attribute });
            TypeDescriptor.AddAttributes(typeof(ArchiveUpdateMode), new Attribute[] { attribute });
            TypeDescriptor.AddAttributes(typeof(PackStage), new Attribute[] { attribute });
            TypeDescriptor.AddAttributes(typeof(ReleaseType), new Attribute[] { attribute });
            TypeDescriptor.AddAttributes(typeof(DoubleClickAction), new Attribute[] { attribute });
            TypeDescriptor.AddAttributes(typeof(SizeFormat), new Attribute[] { attribute });
            TypeDescriptor.AddAttributes(typeof(CopyMode), new Attribute[] { attribute });
            VirtualProperty.ResourceManager = new LocalizedResourceManager("Nomad.Properties.VirtualProperties", Assembly.GetExecutingAssembly());
            DefaultProperty.ResourceManager = VirtualProperty.ResourceManager;
        }

        public static void LoadIconCache()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Loading icon cache", "Settings");
                string path = Path.Combine(SpecialFolders.IconCache, IconCacheName);
                if (System.IO.File.Exists(path))
                {
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    CustomImageProvider.LoadIconCache(path);
                    stopwatch.Stop();
                    System.Diagnostics.Debug.WriteLine(string.Format("Load icon cache in {0} ms", stopwatch.ElapsedMilliseconds), "Settings");
                }
            }
            catch (Exception exception)
            {
                Nomad.Trace.Error.TraceException(TraceEventType.Error, exception);
            }
        }

        private static void MakeArchivesHighlighter()
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            StringBuilder builder = new StringBuilder();
            foreach (ArchiveFormatInfo info in ArchiveFormatManager.GetFormats())
            {
                if (!info.Disabled)
                {
                    string[] extension = info.Extension;
                    if (extension != null)
                    {
                        foreach (string str in extension)
                        {
                            if (!string.IsNullOrEmpty(str) && !dictionary.ContainsKey(str))
                            {
                                dictionary.Add(str, 0);
                                if (builder.Length > 0)
                                {
                                    builder.Append(';');
                                }
                                builder.Append('*');
                                if (!str.StartsWith(".", StringComparison.OrdinalIgnoreCase))
                                {
                                    builder.Append('.');
                                }
                                builder.Append(str);
                            }
                        }
                    }
                }
            }
            ListViewHighlighter item = new ListViewHighlighter("Archive", new AggregatedVirtualItemFilter(new VirtualItemAttributeFilter(0, FileAttributes.Directory), new VirtualItemNameFilter(builder.ToString()))) {
                ForeColor = System.Drawing.Color.DarkGreen
            };
            List<ListViewHighlighter> list = new List<ListViewHighlighter>(Nomad.Properties.Settings.Default.Highlighters);
            list.RemoveAll(delegate (ListViewHighlighter highlighter) {
                return highlighter.Name == "Archive";
            });
            list.Add(item);
            Nomad.Properties.Settings.Default.Highlighters = list.ToArray();
        }

        private static bool NGen()
        {
            string path = Path.Combine(Application.StartupPath, "compile.bat");
            if (!System.IO.File.Exists(path))
            {
                return false;
            }
            ProcessStartInfo startInfo = new ProcessStartInfo {
                WorkingDirectory = Application.StartupPath,
                FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "cmd.exe"),
                Arguments = string.Format("/D /S /C \"\"{0}\" /FrmwrkPath {1} /SourcePath \"{2}\"\"", path, PathHelper.ExcludeTrailingDirectorySeparator(RuntimeEnvironment.GetRuntimeDirectory()), Application.StartupPath),
                CreateNoWindow = true,
                ErrorDialog = false,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = true
            };
            if (OS.IsWinVista)
            {
                startInfo.Verb = "runas";
            }
            Process process = Process.Start(startInfo);
            process.WaitForExit();
            return (process.ExitCode == 0);
        }

        public static void ParseArguments(string[] args)
        {
            if ((args != null) && (args.Length != 0))
            {
                Arguments = new Dictionary<ArgumentKey, object>();
                int index = 0;
                while (index < args.Length)
                {
                    string str = args[index++];
                    if (!str.StartsWith('-'))
                    {
                        if (".tab".Equals(Path.GetExtension(str), StringComparison.OrdinalIgnoreCase))
                        {
                            Arguments[ArgumentKey.Tab] = str;
                        }
                        else if (Arguments.ContainsKey(ArgumentKey.CurrentFolder))
                        {
                            Arguments[ArgumentKey.FarFolder] = str;
                        }
                        else
                        {
                            Arguments[ArgumentKey.CurrentFolder] = str;
                        }
                    }
                    else
                    {
                        try
                        {
                            ArgumentKey none = ArgumentKey.None;
                            switch (str)
                            {
                                case "-h":
                                case "-help":
                                case "-?":
                                    Arguments[ArgumentKey.Help] = true;
                                    goto Label_04B8;

                                case "-i":
                                case "-init":
                                    Arguments[ArgumentKey.Init] = true;
                                    goto Label_04B8;

                                case "-n":
                                case "-new":
                                    Arguments[ArgumentKey.NewInstance] = true;
                                    goto Label_04B8;

                                case "-o":
                                case "-old":
                                    Arguments[ArgumentKey.OldInstance] = true;
                                    goto Label_04B8;

                                case "-s":
                                case "-safe":
                                    if ((index >= args.Length) || args[index].StartsWith('-'))
                                    {
                                        break;
                                    }
                                    Arguments[ArgumentKey.Safe] = (SafeMode) int.Parse(args[index++]);
                                    goto Label_04B8;

                                case "-culture":
                                    if (!((index >= args.Length) || args[index].StartsWith('-')))
                                    {
                                        Arguments[ArgumentKey.Culture] = CultureInfo.GetCultureInfo(args[index++]);
                                    }
                                    goto Label_04B8;

                                case "-debug":
                                    Arguments[ArgumentKey.Debug] = true;
                                    goto Label_04B8;

                                case "-dump":
                                    Arguments[ArgumentKey.Dump] = true;
                                    goto Label_04B8;

                                case "-lang":
                                case "-language":
                                    if ((index < args.Length) && !args[index].StartsWith('-'))
                                    {
                                        try
                                        {
                                            string path = args[index++];
                                            if (System.IO.File.Exists(path))
                                            {
                                                Ini iniSource = new Ini();
                                                using (TextReader reader = new StreamReader(path))
                                                {
                                                    iniSource.Read(reader);
                                                }
                                                BasicFormLocalizer localizer = new IniFormStringLocalizer(iniSource, path);
                                                Arguments[ArgumentKey.FormLocalizer] = localizer;
                                            }
                                        }
                                        catch
                                        {
                                        }
                                    }
                                    goto Label_04B8;

                                case "-recovery":
                                    none = ArgumentKey.RecoveryFolder;
                                    goto Label_04B8;

                                case "-restart":
                                    int num2;
                                    if (((index < args.Length) && !args[index].StartsWith('-')) && int.TryParse(args[index++], out num2))
                                    {
                                        Arguments[ArgumentKey.Restart] = num2;
                                    }
                                    goto Label_04B8;

                                case "-t":
                                case "-tab":
                                    none = ArgumentKey.Tab;
                                    goto Label_04B8;

                                case "-l":
                                case "-left":
                                    none = ArgumentKey.LeftFolder;
                                    goto Label_04B8;

                                case "-r":
                                case "-right":
                                    none = ArgumentKey.RightFolder;
                                    goto Label_04B8;

                                case "-c":
                                case "-current":
                                    none = ArgumentKey.CurrentFolder;
                                    goto Label_04B8;

                                case "-f":
                                case "-far":
                                    none = ArgumentKey.FarFolder;
                                    goto Label_04B8;

                                default:
                                    goto Label_04B8;
                            }
                            Arguments[ArgumentKey.Safe] = SafeMode.DefaultSkip;
                        Label_04B8:
                            if (!(((none == ArgumentKey.None) || (index >= args.Length)) || args[index].StartsWith('-')))
                            {
                                Arguments[none] = args[index++];
                            }
                        }
                        catch (Exception exception)
                        {
                            Nomad.Trace.Error.TraceException(TraceEventType.Error, exception);
                        }
                    }
                }
            }
        }

        public static InitTaskResult PerformInitTask(InitTask task)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine(task.ToString() + "- started", "Settings");
                switch (task)
                {
                    case InitTask.SetupAppearance:
                    {
                        if (!OS.IsWinVista)
                        {
                            break;
                        }
                        Nomad.Properties.Settings.Default.Theme = "themes/vista";
                        VirtualFilePanelSettings settings1 = VirtualFilePanelSettings.Default;
                        settings1.BreadcrumbOptions |= PathView.ShowFolderIcon | PathView.VistaLikeBreadcrumb;
                        return InitTaskResult.Successed;
                    }
                    case InitTask.SetupEditor:
                    {
                        if (!string.IsNullOrEmpty(Nomad.Properties.Settings.Default.EditorPath))
                        {
                            break;
                        }
                        string path = Path.Combine(Application.StartupPath, "editor.exe");
                        if (!System.IO.File.Exists(path))
                        {
                            break;
                        }
                        Nomad.Properties.Settings.Default.EditorPath = path;
                        return InitTaskResult.Successed;
                    }
                    case InitTask.SetupViewer:
                    {
                        if (!string.IsNullOrEmpty(Nomad.Properties.Settings.Default.ViewerPath))
                        {
                            break;
                        }
                        string str2 = Path.Combine(Application.StartupPath, "viewer.exe");
                        if (!System.IO.File.Exists(str2))
                        {
                            break;
                        }
                        Nomad.Properties.Settings.Default.ViewerPath = str2;
                        return InitTaskResult.Successed;
                    }
                    case InitTask.SetupExternalTools:
                        if (!SetupExternalTools())
                        {
                            break;
                        }
                        return InitTaskResult.Successed;

                    case InitTask.MakeArchivesHighligter:
                        MakeArchivesHighlighter();
                        return InitTaskResult.Successed;

                    case InitTask.ExcludeFromWer:
                        if (!OS.IsWinVista)
                        {
                            break;
                        }
                        Wer.WerAddExcludedApplication(Application.ExecutablePath, true);
                        return InitTaskResult.Successed;

                    case InitTask.RegisterJumpListTasks:
                        if (!OS.IsWin7)
                        {
                            break;
                        }
                        RegisterJumpListTasks();
                        return InitTaskResult.Successed;

                    case InitTask.CreateDesktopShortcut:
                        CreateDesktopShortcut();
                        return InitTaskResult.Successed;

                    case InitTask.CompressFiles:
                        if (!CompressFiles())
                        {
                            break;
                        }
                        return InitTaskResult.Successed;

                    case InitTask.NGen:
                        if (!NGen())
                        {
                            return InitTaskResult.Failed;
                        }
                        return InitTaskResult.Successed;
                }
                System.Diagnostics.Debug.WriteLine(task.ToString() + "- succeeded", "Settings");
                return InitTaskResult.Skipped;
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine(task.ToString() + "- failed", "Settings");
                Nomad.Trace.Error.TraceException(TraceEventType.Error, exception);
                return InitTaskResult.Failed;
            }
        }

        private static void RegisterJumpListTasks()
        {
            ShellLink link = new ShellLink {
                Path = Application.ExecutablePath,
                Arguments = "-safe"
            };
            link.SetIconLocation(@"%SystemRoot%\System32\Shell32.dll", 0x90);
            link.Title = Resources.sJumpTaskSafeMode;
            ShellLink link2 = new ShellLink {
                Path = Application.ExecutablePath
            };
            link2.SetFlags(SHELL_LINK_DATA_FLAGS.SLDF_RUNAS_USER, true);
            link2.SetIconLocation(@"%SystemRoot%\System32\User32.dll", 6);
            link2.Title = Resources.sJumpTaskRunAsAdmin;
            IObjectCollection o = (IObjectCollection) new CoObjectCollection();
            try
            {
                o.AddObject(link2.NativeObject);
                o.AddObject(link.NativeObject);
                ICustomDestinationList list = (ICustomDestinationList) new CoDestinationList();
                try
                {
                    uint num;
                    object obj2;
                    list.SetAppID(Program.AppId);
                    list.BeginList(out num, typeof(IObjectArray).GUID, out obj2);
                    list.AddUserTasks((IObjectArray) o);
                    list.CommitList();
                    if (obj2 != null)
                    {
                        Marshal.ReleaseComObject(obj2);
                    }
                }
                finally
                {
                    Marshal.ReleaseComObject(list);
                }
            }
            finally
            {
                Marshal.ReleaseComObject(o);
            }
        }

        public static void RegisterSettings(ApplicationSettingsBase settings)
        {
            SettingsMap[CreateSettingsKey(settings.GetType(), settings.SettingsKey)] = settings;
        }

        public static void SaveIconCache()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Saving icon cache", "Settings");
                CustomImageProvider.SaveIconCache(Path.Combine(SpecialFolders.IconCache, IconCacheName));
            }
            catch (Exception exception)
            {
                Nomad.Trace.Error.TraceException(TraceEventType.Error, exception);
            }
        }

        public static void SaveSettings()
        {
            if (Nomad.Properties.Settings.Default.PersistentIconCache && (ImageProvider.Default is CustomImageProvider))
            {
                SaveIconCache();
            }
            try
            {
                System.Diagnostics.Debug.WriteLine("Saving settings", "Settings");
                foreach (ApplicationSettingsBase base2 in SettingsMap.Values)
                {
                    base2.Save();
                }
            }
            catch (Exception exception)
            {
                Nomad.Trace.Error.TraceException(TraceEventType.Error, exception);
            }
        }

        public static void SetArgument(ArgumentKey key, object value)
        {
            if (Arguments == null)
            {
                Arguments = new Dictionary<ArgumentKey, object>();
            }
            Arguments[key] = value;
        }

        private static bool SetupExternalTools()
        {
            DirectoryInfo info = new DirectoryInfo(SpecialFolders.Tools);
            if (!(info.Exists && (info.GetFiles("*.lnk").Length != 0)))
            {
                ShellLink link = new ShellLink {
                    Path = OS.IsWinNT ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "cmd.exe") : Path.Combine(OS.WindowDirectory, "command.com"),
                    WorkingDirectory = EnvironmentVariables.GetVarName("curdir"),
                    Hotkey = Keys.Control | Keys.Shift | Keys.P
                };
                info.Create();
                link.Save(Path.Combine(info.FullName, "Command Prompt.lnk"));
                return true;
            }
            return false;
        }

        public static void ShowLicenseInformation()
        {
            string path = Path.Combine(Path.Combine(Application.StartupPath, CultureInfo.CurrentUICulture.Name), "license.txt");
            if (!System.IO.File.Exists(path))
            {
                path = Path.Combine(Path.Combine(Application.StartupPath, "doc"), "license.txt");
            }
            if (System.IO.File.Exists(path))
            {
                Process.Start(path);
            }
        }

        public static void UpgradeSettings()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Upgrade settings", "Settings");
                foreach (ApplicationSettingsBase base2 in SettingsMap.Values)
                {
                    base2.Upgrade();
                }
            }
            catch (Exception exception)
            {
                Nomad.Trace.Error.TraceException(TraceEventType.Error, exception);
            }
        }

        [CompilerGenerated]
        private sealed class <GetInstalledCultures>d__4 : IEnumerable<CultureInfo>, IEnumerable, IEnumerator<CultureInfo>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private CultureInfo <>2__current;
            public CultureInfo[] <>7__wrapa;
            public int <>7__wrapb;
            private int <>l__initialThreadId;
            public Assembly <AppAssembly>5__6;
            public HashSet<string> <DirMap>5__5;
            public CultureInfo <NextCulture>5__7;
            public bool <ValidCulture>5__8;

            [DebuggerHidden]
            public <GetInstalledCultures>d__4(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally9()
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
                            this.<>2__current = new CultureInfo("en");
                            this.<>1__state = 1;
                            return true;

                        case 1:
                            this.<>1__state = -1;
                            this.<DirMap>5__5 = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                            foreach (string str in System.IO.Directory.GetDirectories(Application.StartupPath))
                            {
                                this.<DirMap>5__5.Add(Path.GetFileName(str));
                            }
                            this.<AppAssembly>5__6 = Assembly.GetEntryAssembly();
                            this.<>1__state = 3;
                            this.<>7__wrapa = CultureInfo.GetCultures(CultureTypes.SpecificCultures | CultureTypes.NeutralCultures);
                            this.<>7__wrapb = 0;
                            while (this.<>7__wrapb < this.<>7__wrapa.Length)
                            {
                                this.<NextCulture>5__7 = this.<>7__wrapa[this.<>7__wrapb];
                                if ((!(this.<NextCulture>5__7.Name != "en") || !(this.<NextCulture>5__7.Name != string.Empty)) || !this.<DirMap>5__5.Contains(this.<NextCulture>5__7.Name))
                                {
                                    goto Label_0188;
                                }
                                this.<ValidCulture>5__8 = false;
                                try
                                {
                                    this.<AppAssembly>5__6.GetSatelliteAssembly(this.<NextCulture>5__7);
                                    this.<ValidCulture>5__8 = true;
                                }
                                catch (FileNotFoundException)
                                {
                                }
                                if (!this.<ValidCulture>5__8)
                                {
                                    goto Label_0188;
                                }
                                this.<>2__current = this.<NextCulture>5__7;
                                this.<>1__state = 5;
                                return true;
                            Label_0180:
                                this.<>1__state = 3;
                            Label_0188:
                                this.<>7__wrapb++;
                            }
                            this.<>m__Finally9();
                            break;

                        case 5:
                            goto Label_0180;
                    }
                    return false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
            }

            [DebuggerHidden]
            IEnumerator<CultureInfo> IEnumerable<CultureInfo>.GetEnumerator()
            {
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    return this;
                }
                return new SettingsManager.<GetInstalledCultures>d__4(0);
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<System.Globalization.CultureInfo>.GetEnumerator();
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
                    case 3:
                    case 5:
                        this.<>m__Finally9();
                        break;
                }
            }

            CultureInfo IEnumerator<CultureInfo>.Current
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

        public static class EnvironmentVariables
        {
            public const string CurrentDirectory = "curdir";
            public const string CurrentItemName = "curitemname";
            public const string CurrentItemPath = "curitempath";
            public const string CurrentSelectionNameList = "curselname";
            public const string CurrentSelectionPathList = "curselpath";
            public const string FarDirectory = "fardir";
            public const string FarItemName = "faritemname";
            public const string FarItemPath = "faritempath";
            public const string FarSelectionNameList = "farselname";
            public const string FarSelectionPathList = "farselpath";
            public const string NomadConfigDirectory = "nomadcfgdir";
            public const string NomadDirectory = "nomaddir";
            public const string NomadVersion = "nomadver";
            public const string UserParameter = "user";

            public static string GetVarName(string variable)
            {
                return ('%' + variable + '%');
            }
        }

        public class LocalizedEnumConverter : ResourceEnumConverter
        {
            public LocalizedEnumConverter(System.Type type) : base(type, Enums.ResourceManager)
            {
            }
        }

        public class LocalizedResourceManager : ResourceManager
        {
            private ResourceSet LocalizedSet;

            public LocalizedResourceManager(System.Type resourceSource) : base(resourceSource)
            {
                IniFormStringLocalizer argument = SettingsManager.GetArgument<IniFormStringLocalizer>(ArgumentKey.FormLocalizer);
                if (argument != null)
                {
                    this.LocalizedSet = new IniResourceSet(argument.IniSource, resourceSource);
                }
            }

            public LocalizedResourceManager(string baseName, Assembly assembly) : base(baseName, assembly)
            {
                IniFormStringLocalizer argument = SettingsManager.GetArgument<IniFormStringLocalizer>(ArgumentKey.FormLocalizer);
                if (argument != null)
                {
                    this.LocalizedSet = new IniResourceSet(argument.IniSource, baseName);
                }
            }

            public override string GetString(string name, CultureInfo culture)
            {
                string str = null;
                if ((culture == null) && (this.LocalizedSet != null))
                {
                    str = this.LocalizedSet.GetString(name);
                }
                if (str == null)
                {
                    str = base.GetString(name, culture);
                }
                return str;
            }
        }

        public class PreviousVersionConfig
        {
            public readonly string BookmarksDir;
            public readonly string ToolsDir;
            public readonly string UserConfigDir;
            public readonly string UserConfigPath;
            public readonly System.Version Version;

            public PreviousVersionConfig(System.Version version, string userConfigPath)
            {
                string directoryName;
                this.Version = version;
                this.UserConfigPath = userConfigPath;
                this.UserConfigDir = Path.GetDirectoryName(userConfigPath);
                if ((version.Major == 2) && (version.Minor < 6))
                {
                    directoryName = Path.Combine(Path.GetDirectoryName(Application.UserAppDataPath), version.ToString());
                }
                else
                {
                    directoryName = Path.GetDirectoryName(this.UserConfigDir);
                }
                this.BookmarksDir = Path.Combine(directoryName, "Bookmarks");
                if (!System.IO.Directory.Exists(this.BookmarksDir))
                {
                    this.BookmarksDir = Path.Combine(this.UserConfigDir, "Bookmarks");
                }
                if (!System.IO.Directory.Exists(this.BookmarksDir))
                {
                    this.BookmarksDir = null;
                }
                this.ToolsDir = Path.Combine(directoryName, "Tools");
                if (!System.IO.Directory.Exists(this.ToolsDir))
                {
                    this.ToolsDir = Path.Combine(this.UserConfigDir, "Tools");
                }
                if (!System.IO.Directory.Exists(this.ToolsDir))
                {
                    this.ToolsDir = null;
                }
            }
        }

        public static class SpecialFolders
        {
            public const string FolderKeyBookmarks = "Bookmarks";
            public const string FolderKeyDesktopIni = "DesktopIni";
            public const string FolderKeyIconCache = "IconCache";
            public const string FolderKeyTabs = "Tabs";
            public const string FolderKeyTools = "Tools";

            public static string GetSpecialFolder(string key)
            {
                string specialFolder = ConfigurableSettingsProvider.GetSpecialFolder(key);
                if (!string.IsNullOrEmpty(specialFolder))
                {
                    return specialFolder;
                }
                return Path.Combine(UserConfig, key);
            }

            public static string Bookmarks
            {
                get
                {
                    return GetSpecialFolder("Bookmarks");
                }
            }

            public static string DesktopIni
            {
                get
                {
                    return GetSpecialFolder("DesktopIni");
                }
            }

            public static string IconCache
            {
                get
                {
                    string specialFolder = ConfigurableSettingsProvider.GetSpecialFolder("IconCache");
                    if (!string.IsNullOrEmpty(specialFolder))
                    {
                        return specialFolder;
                    }
                    return UserConfig;
                }
            }

            public static string Plugins
            {
                get
                {
                    return Path.Combine(Application.StartupPath, "Plugins");
                }
            }

            public static string Tabs
            {
                get
                {
                    return GetSpecialFolder("Tabs");
                }
            }

            public static string Tools
            {
                get
                {
                    return GetSpecialFolder("Tools");
                }
            }

            public static string UserConfig
            {
                get
                {
                    return Path.GetDirectoryName(ConfigurableSettingsProvider.UserConfigPath);
                }
            }
        }
    }
}

