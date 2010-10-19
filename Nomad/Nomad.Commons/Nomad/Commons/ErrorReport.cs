namespace Nomad.Commons
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Threading;

    public static class ErrorReport
    {
        private static IList<WeakReference> ThreadList = new List<WeakReference>();

        public static string AssembliesToString(AppDomain domain)
        {
            StringBuilder builder = new StringBuilder();
            AssembliesToString(builder, domain);
            return builder.ToString();
        }

        private static void AssembliesToString(StringBuilder builder, AppDomain domain)
        {
            foreach (Assembly assembly in domain.GetAssemblies())
            {
                AssemblyName name = assembly.GetName();
                builder.Append(name.Name);
                builder.Append(" (Version=");
                builder.Append(name.Version);
                builder.AppendLine(")");
                try
                {
                    Module module = assembly.GetModule(Path.GetFileName(assembly.Location));
                    if (module != null)
                    {
                        PortableExecutableKinds kinds;
                        ImageFileMachine machine;
                        builder.Append("  PE: ");
                        FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(module.FullyQualifiedName);
                        if (!string.IsNullOrEmpty(versionInfo.FileVersion))
                        {
                            builder.Append("Version=");
                            builder.Append(versionInfo.FileVersion);
                            builder.Append(", ");
                        }
                        module.GetPEKind(out kinds, out machine);
                        builder.Append("Kind=");
                        builder.Append(kinds);
                        builder.AppendLine();
                    }
                }
                catch (Exception exception)
                {
                    builder.Append(exception.Message);
                }
                builder.Append("  CodeBase: ");
                builder.AppendLine(name.EscapedCodeBase);
            }
            builder.Length -= Environment.NewLine.Length;
        }

        public static string CreateErrorReport(Exception e)
        {
            return CreateErrorReport(e, ErrorReportDetails.All);
        }

        public static string CreateErrorReport(Exception e, ErrorReportDetails details)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("--- Exception Stack Trace ---");
            ExceptionToString(builder, e);
            List<DictionaryEntry> list = null;
            if ((details & ErrorReportDetails.Environment) > 0)
            {
                list = new List<DictionaryEntry> {
                    new DictionaryEntry("OS Version", Environment.OSVersion.VersionString),
                    new DictionaryEntry("CLR Version", Environment.Version),
                    new DictionaryEntry("Processor Count", Environment.ProcessorCount),
                    new DictionaryEntry("Working Set", Environment.WorkingSet.ToString("G", CultureInfo.InvariantCulture))
                };
            }
            if ((details & ErrorReportDetails.Data) > 0)
            {
                if (list == null)
                {
                    list = new List<DictionaryEntry>();
                }
                for (Exception exception = e; exception != null; exception = exception.InnerException)
                {
                    foreach (DictionaryEntry entry in exception.Data)
                    {
                        list.Add(entry);
                    }
                }
            }
            if ((list != null) && (list.Count > 0))
            {
                builder.AppendLine();
                builder.AppendLine();
                builder.Append("--- Additional Data ---");
                foreach (DictionaryEntry entry in list)
                {
                    builder.AppendLine();
                    builder.Append(entry.Key);
                    builder.Append('=');
                    builder.Append(entry.Value);
                }
            }
            if ((details & ErrorReportDetails.Assemblies) > 0)
            {
                builder.AppendLine();
                builder.AppendLine();
                builder.AppendLine("--- Loaded Assemblies ---");
                AssembliesToString(builder, AppDomain.CurrentDomain);
            }
            return builder.ToString();
        }

        public static string DumpThreads()
        {
            StringBuilder builder = new StringBuilder();
            List<Thread> list = new List<Thread>();
            lock (ThreadList)
            {
                for (int i = ThreadList.Count - 1; i >= 0; i--)
                {
                    Thread thread;
                    if (!((ThreadList[i].IsAlive && ((thread = (Thread) ThreadList[i].Target) != null)) && thread.IsAlive))
                    {
                        ThreadList.RemoveAt(i);
                    }
                    else
                    {
                        list.Add(thread);
                    }
                }
            }
            foreach (Thread thread in list)
            {
                Exception exception;
                if (builder.Length > 0)
                {
                    builder.AppendLine();
                }
                try
                {
                    builder.Append("Thread ");
                    builder.Append(thread.ManagedThreadId);
                    if (!string.IsNullOrEmpty(thread.Name))
                    {
                        builder.Append(", Name=");
                        builder.Append(thread.Name);
                    }
                    builder.Append(", ThreadState=");
                    builder.Append(thread.ThreadState);
                    if (thread.IsAlive)
                    {
                        builder.Append(", Priority=");
                        builder.Append(thread.Priority);
                        builder.Append(", ApartmentState=");
                        builder.Append(thread.GetApartmentState());
                        if (thread.IsBackground)
                        {
                            builder.Append(", Background");
                        }
                        if (thread.IsThreadPoolThread)
                        {
                            builder.Append(", ThreadPool");
                        }
                    }
                    try
                    {
                        StackTrace trace = null;
                        int managedThreadId = Thread.CurrentThread.ManagedThreadId;
                        if ((thread.ThreadState & System.Threading.ThreadState.Suspended) > System.Threading.ThreadState.Running)
                        {
                            trace = new StackTrace(thread, true);
                        }
                        else if ((managedThreadId != thread.ManagedThreadId) && ((thread.ThreadState == System.Threading.ThreadState.Running) || ((thread.ThreadState & System.Threading.ThreadState.WaitSleepJoin) > System.Threading.ThreadState.Running)))
                        {
                            thread.Suspend();
                            try
                            {
                                trace = new StackTrace(thread, true);
                            }
                            finally
                            {
                                thread.Resume();
                            }
                        }
                        if ((trace != null) && (trace.FrameCount > 0))
                        {
                            builder.AppendLine();
                            StackTraceToString(builder, trace);
                        }
                    }
                    catch (Exception exception1)
                    {
                        exception = exception1;
                        builder.AppendLine();
                        builder.Append(exception.Message);
                    }
                }
                catch (Exception exception2)
                {
                    exception = exception2;
                    builder.Append(exception.Message);
                }
            }
            return builder.ToString();
        }

        public static string ExceptionToString(Exception e)
        {
            StringBuilder builder = new StringBuilder();
            ExceptionToString(builder, e);
            return builder.ToString();
        }

        private static void ExceptionToString(StringBuilder builder, Exception e)
        {
            builder.Append(e.GetType().FullName);
            builder.Append(": ");
            builder.Append(e.Message);
            Exception innerException = e.InnerException;
            if (innerException != null)
            {
                builder.Append(" ---> ");
                ExceptionToString(builder, innerException);
                builder.AppendLine();
                builder.Append("--- End of inner exception stack trace ---");
            }
            StackTrace trace = new StackTrace(e, 0, true);
            if ((trace != null) && (trace.FrameCount > 0))
            {
                builder.AppendLine();
                StackTraceToString(builder, trace);
            }
        }

        private static string GetFullNameForStackTrace(MethodBase method)
        {
            StringBuilder builder = new StringBuilder();
            ParameterInfo[] parameters = method.GetParameters();
            for (int i = 0; i < parameters.Length; i++)
            {
                if (i > 0)
                {
                    builder.Append(", ");
                }
                string str = (parameters[i].Name == null) ? string.Empty : (" " + parameters[i].Name);
                Type parameterType = parameters[i].ParameterType;
                if (parameterType.IsClass && (parameterType.Namespace != string.Empty))
                {
                    builder.AppendFormat("{0}.{1}{2}", parameterType.Namespace, parameterType.Name, str);
                }
                else
                {
                    builder.Append(parameterType.Name);
                    builder.Append(str);
                }
            }
            StringBuilder builder2 = new StringBuilder();
            if (method.IsGenericMethod)
            {
                Type[] genericArguments = method.GetGenericArguments();
                builder2.Append('[');
                for (int j = 0; j < genericArguments.Length; j++)
                {
                    if (j > 0)
                    {
                        builder2.Append(',');
                    }
                    builder2.Append(genericArguments[j].Name);
                }
                builder2.Append(']');
            }
            return string.Concat(new object[] { method.DeclaringType.ToString(), ".", method.Name, builder2, "(", builder, ")" });
        }

        public static void RegisterThread()
        {
            RegisterThread(Thread.CurrentThread);
        }

        public static void RegisterThread(Thread thread)
        {
            lock (ThreadList)
            {
                bool flag = false;
                for (int i = ThreadList.Count - 1; i >= 0; i--)
                {
                    Thread thread2;
                    if (!((ThreadList[i].IsAlive && ((thread2 = (Thread) ThreadList[i].Target) != null)) && thread2.IsAlive))
                    {
                        ThreadList.RemoveAt(i);
                    }
                    else
                    {
                        flag = thread2.ManagedThreadId == thread.ManagedThreadId;
                    }
                }
                if (!flag)
                {
                    ThreadList.Add(new WeakReference(thread));
                }
            }
        }

        public static string StackTraceToString(StackTrace trace)
        {
            StringBuilder builder = new StringBuilder();
            StackTraceToString(builder, trace);
            return builder.ToString();
        }

        public static void StackTraceToString(StringBuilder builder, StackTrace trace)
        {
            for (int i = 0; i < trace.FrameCount; i++)
            {
                StackFrame frame = trace.GetFrame(i);
                if (i != 0)
                {
                    builder.AppendLine();
                }
                builder.Append("  at ");
                MethodBase method = frame.GetMethod();
                if (method == null)
                {
                    builder.AppendFormat("<unknown method> <0x{0:x5}>", frame.GetNativeOffset());
                }
                else
                {
                    builder.Append(GetFullNameForStackTrace(method));
                    string fileName = frame.GetFileName();
                    if (fileName != null)
                    {
                        builder.AppendFormat(" in {0}:{1}", fileName, frame.GetFileLineNumber());
                    }
                    else
                    {
                        builder.AppendFormat(" {{{0:N}:0x{1:x}{2}", method.Module.ModuleVersionId, method.MetadataToken, '}');
                        int iLOffset = frame.GetILOffset();
                        if (iLOffset == -1)
                        {
                            builder.AppendFormat(" <0x{0:x}>", frame.GetNativeOffset());
                        }
                        else
                        {
                            builder.AppendFormat(" [0x{0:x}]", iLOffset);
                        }
                    }
                }
            }
        }
    }
}

