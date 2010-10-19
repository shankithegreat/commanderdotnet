namespace Nomad
{
    using Microsoft.IO.Pipes;
    using Nomad.Commons.Plugin;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.Remoting;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;

    public class ElevatedProcess : IPluginProcess, IPluginActivator
    {
        private TimeSpan _InactivityTimeout;
        private bool _KeepAlive;
        private Process _Process;

        public ElevatedProcess()
        {
        }

        public ElevatedProcess(bool keepAlive)
        {
            this._KeepAlive = keepAlive;
        }

        public ElevatedProcess(TimeSpan inactivityTimeout)
        {
            this._InactivityTimeout = inactivityTimeout;
        }

        public ElevatedProcess(bool keepAlive, TimeSpan inactivityTimeout)
        {
            this._KeepAlive = keepAlive;
            this._InactivityTimeout = inactivityTimeout;
        }

        public T Create<T>(string objectUri)
        {
            if (!this.IsAlive)
            {
                throw new InvalidOperationException();
            }
            return (T) Activator.GetObject(typeof(T), string.Format("ipc://nomad_plugin_proxy_{0}/{1}", this._Process.Id, objectUri));
        }

        public bool Shutdown()
        {
            if (!this.IsAlive)
            {
                return false;
            }
            try
            {
                this.Controller.Shutdown();
            }
            catch (RemotingException)
            {
            }
            if (!this._Process.WaitForExit(100))
            {
                this._Process.Kill();
            }
            return this._Process.HasExited;
        }

        public bool Start()
        {
            if (this._Process != null)
            {
                return false;
            }
            StringBuilder builder = new StringBuilder(" ");
            if (this._KeepAlive)
            {
                builder.Append("-keepalive ");
            }
            if (this._InactivityTimeout.Ticks > 0L)
            {
                builder.AppendFormat("-timeout {0} ", this._InactivityTimeout.Ticks / 0x2710L);
            }
            builder.Length--;
            ProcessStartInfo startInfo = new ProcessStartInfo(Path.Combine(Application.StartupPath, "cmdproxy.exe"), builder.ToString());
            if (!SettingsManager.GetArgument<bool>(ArgumentKey.Debug))
            {
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            }
            startInfo.Verb = "runas";
            try
            {
                this._Process = Process.Start(startInfo);
                int num = 0;
                while (!NamedPipeStream.Check(string.Format(@"\\.\pipe\nomad_plugin_proxy_{0}", this._Process.Id), FileAccess.ReadWrite))
                {
                    Thread.Sleep(100);
                    if (this._Process.HasExited || (++num > 10))
                    {
                        return false;
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected IPluginController Controller
        {
            get
            {
                return (IPluginController) Activator.GetObject(typeof(IPluginController), string.Format("ipc://nomad_plugin_proxy_{0}/controller", this._Process.Id));
            }
        }

        public bool IsAlive
        {
            get
            {
                return ((this._Process != null) && !this._Process.HasExited);
            }
        }

        public bool KeepAlive
        {
            get
            {
                if (this.IsAlive)
                {
                    bool keepAlive;
                    try
                    {
                        keepAlive = this.Controller.KeepAlive;
                    }
                    catch (RemotingException)
                    {
                    }
                    return keepAlive;
                }
                return this._KeepAlive;
            }
            set
            {
                if (this.IsAlive)
                {
                    try
                    {
                        this.Controller.KeepAlive = value;
                    }
                    catch (RemotingException)
                    {
                    }
                }
                this._KeepAlive = value;
            }
        }
    }
}

