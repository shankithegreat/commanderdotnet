namespace Nomad
{
    using Nomad.Commons;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Threading;

    public sealed class Trace : IDisposable
    {
        private bool AppFirstInstance;
        private System.Threading.Mutex AppMutex;
        private static Nomad.Trace CurrentTrace;
        public static readonly TraceSource Error = new TraceSource("Error");
        private TraceSource ThreadError;

        public Trace()
        {
            Debug.Assert(CurrentTrace == null);
            CurrentTrace = this;
            this.AppMutex = new System.Threading.Mutex(false, @"Local\NOMADNET", out this.AppFirstInstance);
            this.ThreadError = new TraceSource("Thread");
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(this.UnhandledExceptionHandler);
        }

        public void Dispose()
        {
            AppDomain.CurrentDomain.UnhandledException -= new UnhandledExceptionEventHandler(this.UnhandledExceptionHandler);
            this.ThreadError.Close();
            Error.Close();
            this.AppMutex.Close();
        }

        public static void RouteDebugTraceToError()
        {
            foreach (TraceListener listener in Error.Listeners)
            {
                if (!(listener is DefaultTraceListener))
                {
                    Debug.Listeners.Add(listener);
                }
            }
        }

        private void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Exception exceptionObject = e.ExceptionObject as Exception;
            if (exceptionObject != null)
            {
                this.ThreadError.TraceException(TraceEventType.Critical, exceptionObject);
                this.WriteCrushLog(exceptionObject);
            }
            else
            {
                this.ThreadError.TraceEvent(TraceEventType.Critical, 0, e.ExceptionObject.ToString());
            }
            this.ThreadError.Flush();
        }

        public void WriteCrushLog(Exception e)
        {
            try
            {
                if (this.Mutex.WaitOne(0x3e8, false))
                {
                    try
                    {
                        using (TextWriter writer = File.CreateText(CrushLogPath))
                        {
                            writer.Write(ErrorReport.CreateErrorReport(e));
                        }
                    }
                    finally
                    {
                        this.Mutex.ReleaseMutex();
                    }
                }
            }
            catch (Exception exception)
            {
                this.ThreadError.TraceException(TraceEventType.Error, exception);
            }
        }

        public static string CrushLogPath
        {
            get
            {
                return Path.Combine(SettingsManager.SpecialFolders.UserConfig, "crush.log");
            }
        }

        public static Nomad.Trace Current
        {
            get
            {
                return CurrentTrace;
            }
        }

        public bool FirstInstance
        {
            get
            {
                return this.AppFirstInstance;
            }
        }

        public System.Threading.Mutex Mutex
        {
            get
            {
                return this.AppMutex;
            }
        }
    }
}

