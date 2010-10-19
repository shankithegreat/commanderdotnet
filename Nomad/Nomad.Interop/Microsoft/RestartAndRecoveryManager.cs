namespace Microsoft
{
    using Microsoft.Win32;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public static class RestartAndRecoveryManager
    {
        public const int DefaultRecoveryPingInterval = 0x1388;
        private static string FCommandLine;
        private static ApplicationRestartOptions FOptions;
        private static int FPingInterval;
        private static bool FRecoveryEnabled;
        private static bool FRestartEnabled;
        private static ApplicationRecoveryCallback RecoveryCallbackHandler;

        private static  event EventHandler<RecoveryEventArgs> FRecovery;

        public static  event EventHandler<RecoveryEventArgs> Recovery
        {
            add
            {
                FRecovery = (EventHandler<RecoveryEventArgs>) Delegate.Combine(FRecovery, value);
            }
            remove
            {
                FRecovery = (EventHandler<RecoveryEventArgs>) Delegate.Remove(FRecovery, value);
                if ((FRecovery == null) && RecoveryEnabled)
                {
                    UnregisterRecovery();
                }
            }
        }

        private static int RecoveryCallback(IntPtr param)
        {
            RecoveryEventArgs e = new RecoveryEventArgs();
            if (FRecovery != null)
            {
                FRecovery(null, e);
            }
            Windows.ApplicationRecoveryFinished(e.Finished);
            return 0;
        }

        public static bool RegisterRecovery()
        {
            if (!IsSupported)
            {
                throw new PlatformNotSupportedException();
            }
            if (FRecoveryEnabled)
            {
                throw new InvalidOperationException();
            }
            if (FRecovery == null)
            {
                throw new ArgumentException();
            }
            if (RecoveryCallbackHandler == null)
            {
                RecoveryCallbackHandler = new ApplicationRecoveryCallback(RestartAndRecoveryManager.RecoveryCallback);
            }
            int num = Windows.RegisterApplicationRecoveryCallback(RecoveryCallbackHandler, IntPtr.Zero, (uint) FPingInterval, 0);
            if (num == -2147024809)
            {
                throw new ArgumentException();
            }
            FRecoveryEnabled = num == 0;
            return FRecoveryEnabled;
        }

        public static bool RegisterRestart()
        {
            if (!IsSupported)
            {
                throw new PlatformNotSupportedException();
            }
            int num = Windows.RegisterApplicationRestart(RestartCommandLine, (RESTART) RestartOptions);
            if (num == -2147024809)
            {
                throw new ArgumentException();
            }
            FRestartEnabled = num == 0;
            return FRestartEnabled;
        }

        public static bool RegisterRestart(string commandLine)
        {
            return RegisterRestart(commandLine, 0);
        }

        public static bool RegisterRestart(string commandLine, ApplicationRestartOptions options)
        {
            if (commandLine == null)
            {
                throw new ArgumentNullException();
            }
            if (commandLine == string.Empty)
            {
                throw new ArgumentException();
            }
            if (!((options == ((ApplicationRestartOptions) 0)) || Enum.IsDefined(typeof(ApplicationRestartOptions), options)))
            {
                throw new InvalidEnumArgumentException();
            }
            FCommandLine = commandLine;
            FOptions = options;
            return RegisterRestart();
        }

        public static bool UnregisterRecovery()
        {
            if (!IsSupported)
            {
                throw new PlatformNotSupportedException();
            }
            if (Windows.UnregisterApplicationRecoveryCallback() == 0)
            {
                FRecoveryEnabled = false;
                return true;
            }
            return false;
        }

        public static bool UnregisterRestart()
        {
            if (!IsSupported)
            {
                throw new PlatformNotSupportedException();
            }
            if (Windows.UnregisterApplicationRestart() == 0)
            {
                FRestartEnabled = false;
                return true;
            }
            return false;
        }

        public static bool CanRestart
        {
            get
            {
                TimeSpan span = (TimeSpan) (DateTime.Now - Process.GetCurrentProcess().StartTime);
                return (span.TotalMinutes >= 1.0);
            }
        }

        public static bool IsSupported
        {
            get
            {
                return OS.IsWinVista;
            }
        }

        public static bool RecoveryEnabled
        {
            get
            {
                return FRecoveryEnabled;
            }
            set
            {
                if (value != FRecoveryEnabled)
                {
                    if (value)
                    {
                        RegisterRecovery();
                    }
                    else
                    {
                        UnregisterRecovery();
                    }
                }
            }
        }

        public static int RecoveryPingInterval
        {
            get
            {
                return ((FPingInterval == 0) ? 0x1388 : FPingInterval);
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                if (FRecoveryEnabled)
                {
                    throw new InvalidOperationException();
                }
                FPingInterval = value;
            }
        }

        public static string RestartCommandLine
        {
            get
            {
                return FCommandLine;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }
                if (value == string.Empty)
                {
                    throw new ArgumentException();
                }
                FCommandLine = value;
                if (RestartEnabled)
                {
                    RegisterRestart();
                }
            }
        }

        public static bool RestartEnabled
        {
            get
            {
                return FRestartEnabled;
            }
            set
            {
                if (value != FRestartEnabled)
                {
                    if (value)
                    {
                        RegisterRestart();
                    }
                    else
                    {
                        UnregisterRestart();
                    }
                }
            }
        }

        public static ApplicationRestartOptions RestartOptions
        {
            get
            {
                return FOptions;
            }
            set
            {
                if (!Enum.IsDefined(typeof(ApplicationRestartOptions), value))
                {
                    throw new InvalidEnumArgumentException();
                }
                FOptions = value;
                if (RestartEnabled)
                {
                    RegisterRestart();
                }
            }
        }
    }
}

