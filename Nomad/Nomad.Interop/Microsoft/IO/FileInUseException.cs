namespace Microsoft.IO
{
    using Microsoft;
    using Microsoft.COM;
    using Microsoft.Win32;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;

    public class FileInUseException : IOException
    {
        public readonly string ApplicationName;
        public readonly string FileName;

        public FileInUseException(string message) : base(message)
        {
            base.HResult = HRESULT.HRESULT_FROM_WIN32(0x20);
        }

        public FileInUseException(string message, Exception innerException) : base(message, innerException)
        {
            base.HResult = HRESULT.HRESULT_FROM_WIN32(0x20);
        }

        public FileInUseException(string message, string fileName) : base(message)
        {
            base.HResult = HRESULT.HRESULT_FROM_WIN32(0x20);
            this.FileName = fileName;
            this.ApplicationName = GetAppNameUsingFile(fileName);
        }

        public FileInUseException(string message, string fileName, Exception innerException) : base(message, innerException)
        {
            base.HResult = HRESULT.HRESULT_FROM_WIN32(0x20);
            this.FileName = fileName;
            this.ApplicationName = GetAppNameUsingFile(fileName);
        }

        public FileInUseException(string message, string fileName, string appPath, Exception innerException) : base(message, innerException)
        {
            base.HResult = HRESULT.HRESULT_FROM_WIN32(0x20);
            this.FileName = fileName;
            this.ApplicationName = appPath;
        }

        public static string GetAppNameUsingFile(string filePath)
        {
            string ppszName = null;
            object runningObject = GetRunningObject(filePath);
            if (runningObject != null)
            {
                try
                {
                    IFileIsInUse use = runningObject as IFileIsInUse;
                    if (use != null)
                    {
                        use.GetAppName(out ppszName);
                    }
                    else
                    {
                        ppszName = GetAppPathUsingFile(runningObject);
                        if (ppszName != null)
                        {
                            ppszName = Path.GetFileName(ppszName);
                        }
                    }
                }
                catch
                {
                    ppszName = null;
                }
                finally
                {
                    Marshal.ReleaseComObject(runningObject);
                }
            }
            if ((ppszName == null) && OS.IsWinVista)
            {
                RM_PROCESS_INFO[] restartManagerUsingFile = GetRestartManagerUsingFile(filePath);
                if ((restartManagerUsingFile != null) && (restartManagerUsingFile.Length > 0))
                {
                    ppszName = restartManagerUsingFile[0].strAppName;
                }
            }
            return ppszName;
        }

        private static string GetAppPathUsingFile(object runningObject)
        {
            IPersist persist = runningObject as IPersist;
            if (persist != null)
            {
                Guid guid;
                persist.GetClassID(out guid);
                string str = null;
                using (RegistryKey key = Registry.ClassesRoot.OpenSubKey(@"CLSID\" + guid.ToString("B") + @"\ProgID"))
                {
                    if (key != null)
                    {
                        str = key.GetValue(null) as string;
                    }
                }
                if (string.IsNullOrEmpty(str))
                {
                    return null;
                }
                using (RegistryKey key2 = Registry.ClassesRoot.OpenSubKey(str + @"\shell\open\command"))
                {
                    if (key2 != null)
                    {
                        string str2 = key2.GetValue(null) as string;
                        if (!(string.IsNullOrEmpty(str2) || (str2[0] != '"')))
                        {
                            str2 = str2.Substring(1, str2.IndexOf('"', 2) - 1);
                        }
                        return str2;
                    }
                }
            }
            return null;
        }

        public static IList<Process> GetProcessesUsingFile(string filePath)
        {
            if (!OS.IsWinVista)
            {
                throw new PlatformNotSupportedException();
            }
            if (filePath == null)
            {
                throw new ArgumentNullException();
            }
            if (filePath == string.Empty)
            {
                throw new ArgumentException();
            }
            RM_PROCESS_INFO[] restartManagerUsingFile = GetRestartManagerUsingFile(filePath);
            List<Process> list = new List<Process>();
            for (int i = 0; i < restartManagerUsingFile.Length; i++)
            {
                try
                {
                    list.Add(Process.GetProcessById(restartManagerUsingFile[i].Process.dwProcessId));
                }
                catch (ArgumentException)
                {
                }
            }
            return list;
        }

        private static RM_PROCESS_INFO[] GetRestartManagerUsingFile(string filePath)
        {
            SafeRestartSessionHandle handle;
            int num = Rstrtmgr.RmStartSession(out handle, 0, Guid.NewGuid().ToString("N"));
            if (num != 0)
            {
                return null;
            }
            RM_PROCESS_INFO[] rgAffectedApps = new RM_PROCESS_INFO[1];
            using (handle)
            {
                string[] rgsFilenames = new string[] { filePath };
                num = Rstrtmgr.RmRegisterResources(handle, rgsFilenames.Length, rgsFilenames, 0, null, 0, null);
                if (num == 0)
                {
                    int num2;
                    int length = rgAffectedApps.Length;
                    RM_REBOOT_REASON rmRebootReasonNone = RM_REBOOT_REASON.RmRebootReasonNone;
                    num = Rstrtmgr.RmGetList(handle, out num2, ref length, rgAffectedApps, ref rmRebootReasonNone);
                    if (num == 0xea)
                    {
                        rgAffectedApps = new RM_PROCESS_INFO[num2];
                        length = rgAffectedApps.Length;
                        num = Rstrtmgr.RmGetList(handle, out num2, ref length, rgAffectedApps, ref rmRebootReasonNone);
                    }
                }
            }
            if (num != 0)
            {
                return null;
            }
            return rgAffectedApps;
        }

        private static object GetRunningObject(string filePath)
        {
            IMoniker moniker;
            object obj3;
            if (HRESULT.FAILED(ActiveX.CreateFileMoniker(filePath, out moniker)))
            {
                return null;
            }
            try
            {
                IBindCtx ctx;
                if (HRESULT.FAILED(ActiveX.CreateBindCtx(0, out ctx)))
                {
                    obj3 = null;
                }
                else
                {
                    try
                    {
                        IRunningObjectTable table;
                        ctx.GetRunningObjectTable(out table);
                        try
                        {
                            object obj2;
                            if (HRESULT.FAILED(table.GetObject(moniker, out obj2)))
                            {
                                return null;
                            }
                            obj3 = obj2;
                        }
                        finally
                        {
                            Marshal.ReleaseComObject(table);
                        }
                    }
                    finally
                    {
                        Marshal.ReleaseComObject(ctx);
                    }
                }
            }
            catch
            {
                obj3 = null;
            }
            finally
            {
                Marshal.ReleaseComObject(moniker);
            }
            return obj3;
        }

        public static bool IsFileInUseError(Exception e)
        {
            if (e is FileInUseException)
            {
                return true;
            }
            Win32Exception exception = e as Win32Exception;
            if ((exception != null) && (exception.NativeErrorCode == 0x20))
            {
                return true;
            }
            Win32IOException exception2 = e as Win32IOException;
            return (((exception2 != null) && (exception2.NativeErrorCode == 0x20)) || (Marshal.GetHRForException(e) == HRESULT.HRESULT_FROM_WIN32(0x20)));
        }
    }
}

