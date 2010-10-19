namespace Nomad
{
    using Nomad.Commons;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.Remoting;
    using System.Runtime.Remoting.Channels;
    using System.Runtime.Remoting.Channels.Ipc;
    using System.Runtime.Serialization.Formatters;

    public class Controller : MarshalByRefObject
    {
        private static IpcServerChannel ServerChannel;

        public void Activate()
        {
            MainForm instance = MainForm.Instance;
            if (instance != null)
            {
                if (instance.InvokeRequired)
                {
                    instance.Invoke(new Func<bool>(instance.actBringToFront.Execute));
                }
                else
                {
                    instance.actBringToFront.Execute();
                }
            }
        }

        public string DumpThreads()
        {
            return ErrorReport.DumpThreads();
        }

        public static Controller GetController()
        {
            return (Controller) Activator.GetObject(typeof(Controller), string.Format("ipc://{0}/controller", Program.AppId));
        }

        public void ReparseCommandLine(IDictionary<ArgumentKey, object> arguments)
        {
            if (arguments != null)
            {
                MainForm instance = MainForm.Instance;
                if (instance != null)
                {
                    foreach (KeyValuePair<ArgumentKey, object> pair in arguments)
                    {
                        SettingsManager.SetArgument(pair.Key, pair.Value);
                    }
                    if (instance.InvokeRequired)
                    {
                        instance.Invoke(new MethodInvoker(instance.ReinitializeTabs));
                    }
                    else
                    {
                        instance.ReinitializeTabs();
                    }
                }
            }
        }

        public static void StartServer()
        {
            try
            {
                RemotingConfiguration.RegisterWellKnownServiceType(typeof(Controller), "controller", WellKnownObjectMode.SingleCall);
                BinaryServerFormatterSinkProvider sinkProvider = new BinaryServerFormatterSinkProvider {
                    TypeFilterLevel = TypeFilterLevel.Full
                };
                IDictionary properties = new Hashtable();
                properties.Add("portName", Program.AppId);
                ServerChannel = new IpcServerChannel(properties, sinkProvider);
                ChannelServices.RegisterChannel(ServerChannel, false);
                ControllerType = Nomad.ControllerType.Server;
            }
            catch (RemotingException)
            {
                ControllerType = Nomad.ControllerType.Client;
            }
        }

        public static void StopServer()
        {
            if (ServerChannel != null)
            {
                ServerChannel.StopListening(null);
            }
        }

        public static Nomad.ControllerType ControllerType
        {
            [CompilerGenerated]
            get
            {
                return <ControllerType>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                <ControllerType>k__BackingField = value;
            }
        }
    }
}

