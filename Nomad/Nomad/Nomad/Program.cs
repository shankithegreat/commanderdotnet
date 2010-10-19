namespace Nomad
{
    using Microsoft;
    using Microsoft.Shell;
    using Microsoft.Win32;
    using Nomad.Commons;
    using Nomad.Commons.IO;
    using Nomad.Configuration;
    using Nomad.Dialogs;
    using Nomad.FileSystem.Archive;
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Property.Providers;
    using Nomad.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;

    internal static class Program
    {
        private static bool CheckFirstInstance()
        {
            if (Nomad.Trace.Current.FirstInstance)
            {
                return true;
            }
            MessageDialogResult none = MessageDialogResult.None;
            if (!((none != MessageDialogResult.None) ? true : !SettingsManager.GetArgument<bool>(ArgumentKey.NewInstance)))
            {
                none = MessageDialogResult.Yes;
            }
            if (!((none != MessageDialogResult.None) ? true : !SettingsManager.GetArgument<bool>(ArgumentKey.OldInstance)))
            {
                none = MessageDialogResult.No;
            }
            if (none == MessageDialogResult.None)
            {
                none = ConfirmationSettings.Default.AnotherInstance;
            }
            if (none == MessageDialogResult.None)
            {
                bool checkBoxChecked = false;
                none = MessageDialog.Show(null, Resources.sAskStartAnotherInstance, "Nomad.NET", Resources.sRememberQuestionAnswer, ref checkBoxChecked, MessageDialog.ButtonsYesNo, MessageBoxIcon.Question);
                if (checkBoxChecked)
                {
                    switch (none)
                    {
                        case MessageDialogResult.Yes:
                        case MessageDialogResult.No:
                            ConfirmationSettings.Default.AnotherInstance = none;
                            using (ResourceGuard.Create(Nomad.Trace.Current.Mutex))
                            {
                                ConfirmationSettings.Default.Save();
                            }
                            break;
                    }
                }
            }
            if (none == MessageDialogResult.Yes)
            {
                return true;
            }
            try
            {
                Controller controller = Controller.GetController();
                if (controller != null)
                {
                    IDictionary<ArgumentKey, object> folderArguments = SettingsManager.GetFolderArguments();
                    if (folderArguments.Count > 0)
                    {
                        controller.ReparseCommandLine(folderArguments);
                    }
                    controller.Activate();
                }
                else
                {
                    Process[] processesByName = Process.GetProcessesByName("Nomad");
                    if (processesByName.Length > 0)
                    {
                        Windows.SetForegroundWindow(processesByName[0].MainWindowHandle);
                    }
                }
            }
            catch (Exception exception)
            {
                Nomad.Trace.Error.TraceException(TraceEventType.Warning, exception);
            }
            return false;
        }

        [STAThread]
        private static void Main(string[] args)
        {
            bool flag = false;
            using (new Nomad.Trace())
            {
                Exception exception;
                Thread.CurrentThread.Name = "Main";
                ErrorReport.RegisterThread();
                Environment.SetEnvironmentVariable("nomaddir", Application.StartupPath);
                Environment.SetEnvironmentVariable("nomadver", Assembly.GetExecutingAssembly().GetName().Version.ToString());
                Environment.SetEnvironmentVariable("nomadcfgdir", SettingsManager.SpecialFolders.UserConfig);
                if (OS.IsWin7)
                {
                    Microsoft.Shell.Shell32.SetCurrentProcessExplicitAppUserModelID(AppId);
                }
                SettingsManager.ParseArguments(args);
                int argument = SettingsManager.GetArgument<int>(ArgumentKey.Restart);
                if (argument > 0)
                {
                    try
                    {
                        Process.GetProcessById(argument).WaitForExit(0x3e8);
                    }
                    catch (ArgumentException)
                    {
                    }
                }
                try
                {
                    if (SettingsManager.CheckSafeMode(SafeMode.SkipConfig))
                    {
                        ConfigurableSettingsProvider.SkipUserConfig = true;
                    }
                    SettingsManager.Initialize();
                    if (!SettingsManager.CheckSafeMode(SafeMode.SkipUICulture))
                    {
                        CultureInfo uICulture = SettingsManager.GetArgument<CultureInfo>(ArgumentKey.Culture);
                        if (uICulture != null)
                        {
                            Settings.Default.UICulture = uICulture;
                        }
                        else
                        {
                            uICulture = Settings.Default.UICulture;
                        }
                        if (!((uICulture == null) || string.IsNullOrEmpty(uICulture.Name)))
                        {
                            Thread.CurrentThread.CurrentUICulture = uICulture;
                        }
                    }
                }
                catch (ConfigurationException)
                {
                    MessageBox.Show("Configuration system failed to initialize. Deleting user configuration and restart.", "Nomad.NET", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    using (ResourceGuard.Create(Nomad.Trace.Current.Mutex))
                    {
                        File.Delete(ConfigurableSettingsProvider.UserConfigPath);
                    }
                    flag = true;
                }
                if (SettingsManager.GetArgument<bool>(ArgumentKey.Dump) && !Nomad.Trace.Current.FirstInstance)
                {
                    try
                    {
                        Controller controller = Controller.GetController();
                        if (controller != null)
                        {
                            string path = Path.Combine(Environment.ExpandEnvironmentVariables("%temp%"), StringHelper.GuidToCompactString(Guid.NewGuid()) + ".txt");
                            File.WriteAllText(path, controller.DumpThreads());
                            Process.Start(path);
                        }
                    }
                    catch (Exception exception3)
                    {
                        exception = exception3;
                        Nomad.Trace.Error.TraceException(TraceEventType.Error, exception);
                    }
                    return;
                }
                if (!flag)
                {
                    SettingsManager.InitializeGlobalization();
                    Application.EnableVisualStyles();
                    Application.VisualStyleState = Settings.Default.VisualStyleState;
                    Application.SetCompatibleTextRenderingDefault(false);
                    if (SettingsManager.GetArgument<bool>(ArgumentKey.Debug))
                    {
                        Nomad.Trace.RouteDebugTraceToError();
                    }
                    bool flag2 = SettingsManager.GetArgument<bool>(ArgumentKey.Init);
                    if (flag2 || Settings.Default.PerformInitialInitialize)
                    {
                        try
                        {
                            if (flag2 || !SettingsManager.CheckSafeMode(SafeMode.SkipConfig))
                            {
                                CursorExtension.FixHandCursor();
                                using (SetupAndUpgradeDialog dialog = new SetupAndUpgradeDialog())
                                {
                                    dialog.ShowDialog();
                                }
                            }
                        }
                        catch (Exception exception4)
                        {
                            exception = exception4;
                            Nomad.Trace.Error.TraceException(TraceEventType.Error, exception);
                        }
                        finally
                        {
                            Settings.Default.PerformInitialInitialize = false;
                        }
                    }
                    if (CheckFirstInstance())
                    {
                        DefaultProperty.Initialize();
                        ArchiveProperty.Initialize();
                        PropertyProviderManager.Intitialize(StringHelper.SplitString(Settings.Default.DisabledPropertyProviders, new char[] { ',' }));
                        TypeDescriptor.AddAttributes(typeof(Keys), new Attribute[] { new TypeConverterAttribute(typeof(KeysConverter2)) });
                        CursorExtension.FixHandCursor();
                        if (SettingsManager.GetArgument<bool>(ArgumentKey.Help))
                        {
                            MessageBox.Show(Resources.sCommandLineHelp, "Nomad.NET", MessageBoxButtons.OK);
                        }
                        SetupApplicationExceptionHandler();
                        MainForm.Instance = new MainForm();
                        Application.Run(MainForm.Instance);
                        SafeSaveSettings();
                        SafeCleanup();
                    }
                }
            }
            if (flag)
            {
                Application.Restart();
            }
        }

        public static void Restart(bool preserveOriginalCommandLineArgs, params string[] commandLineArgs)
        {
            Process currentProcess = Process.GetCurrentProcess();
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("-restart {0}", currentProcess.Id);
            foreach (string str in Environment.GetCommandLineArgs().Skip<string>(1).Concat<string>(commandLineArgs))
            {
                builder.Append(' ');
                if (str.IndexOf(' ') < 0)
                {
                    builder.Append(str);
                }
                else
                {
                    builder.Append('"');
                    builder.Append(str);
                    builder.Append('"');
                }
            }
            ProcessStartInfo startInfo = Process.GetCurrentProcess().StartInfo;
            startInfo.FileName = Application.ExecutablePath;
            startInfo.Arguments = commandLineArgs.ToString();
            Application.Exit();
            Process.Start(startInfo);
        }

        private static void SafeCleanup()
        {
            try
            {
                CleanupManager.Cleanup();
            }
            catch (Exception exception)
            {
                Nomad.Trace.Error.TraceException(TraceEventType.Critical, exception);
            }
        }

        public static void SafeSaveSettings()
        {
            try
            {
                if (Nomad.Trace.Current.Mutex.WaitOne(0x7d0, false))
                {
                    try
                    {
                        SettingsManager.SaveSettings();
                    }
                    finally
                    {
                        Nomad.Trace.Current.Mutex.ReleaseMutex();
                    }
                }
            }
            catch (AbandonedMutexException exception)
            {
                Nomad.Trace.Error.TraceException(TraceEventType.Critical, exception);
            }
        }

        public static void SetupApplicationExceptionHandler()
        {
            if (!(!Settings.Default.ImprovedUnhandledExceptionProcessing ? true : SettingsManager.GetArgument<bool>(ArgumentKey.Debug)))
            {
                Application.ThreadException += new ThreadExceptionEventHandler(Program.ThreadExceptionHandler);
            }
        }

        private static void ThreadExceptionHandler(object sender, ThreadExceptionEventArgs e)
        {
            DialogResult none = DialogResult.None;
            try
            {
                Nomad.Trace.Error.TraceException(TraceEventType.Critical, e.Exception);
                using (ExceptionDialog dialog = new ExceptionDialog())
                {
                    dialog.BringToFront();
                    none = dialog.ShowException(e.Exception);
                }
            }
            catch
            {
                none = DialogResult.Retry;
            }
            switch (none)
            {
                case DialogResult.Retry:
                    none = DialogResult.Abort;
                    try
                    {
                        MessageBox.Show(ErrorReport.ExceptionToString(e.Exception));
                    }
                    catch
                    {
                    }
                    break;

                case DialogResult.Abort:
                    none = DialogResult.None;
                    try
                    {
                        CancelEventArgs args = new CancelEventArgs();
                        Application.Exit(args);
                        if (args.Cancel)
                        {
                            none = DialogResult.Abort;
                        }
                    }
                    catch
                    {
                        none = DialogResult.Abort;
                    }
                    break;
            }
            if (none == DialogResult.Abort)
            {
                Environment.FailFast(e.Exception.Message);
            }
        }

        public static string AppId
        {
            get
            {
                return string.Format("BMG.{0}.{1}", "Nomad.NET", Assembly.GetExecutingAssembly().GetName().Version);
            }
        }
    }
}

