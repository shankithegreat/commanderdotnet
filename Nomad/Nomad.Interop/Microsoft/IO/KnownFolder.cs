namespace Microsoft.IO
{
    using Microsoft;
    using Microsoft.Shell;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;

    public static class KnownFolder
    {
        private static IDictionary<string, CSIDL> _KnownFolderMap;

        public static CSIDL FolderNameToCSIDL(string knownName)
        {
            CSIDL csidl;
            if (OS.IsWinVista)
            {
                IKnownFolderManager o = (IKnownFolderManager) new CoKnownFolderManager();
                try
                {
                    IKnownFolder folder;
                    o.GetFolderByName(knownName, out folder);
                    try
                    {
                        o.FolderIdToCsidl(folder.GetId(), out csidl);
                        return csidl;
                    }
                    finally
                    {
                        Marshal.FinalReleaseComObject(folder);
                    }
                }
                finally
                {
                    Marshal.FinalReleaseComObject(o);
                }
            }
            if (!KnownFolderMap.TryGetValue(knownName, out csidl))
            {
                throw new ArgumentException(string.Format("'{0}' is not valid known shell folder.", knownName));
            }
            return csidl;
        }

        public static IEnumerable<string> GetNames()
        {
            return new <GetNames>d__0(-2);
        }

        private static IDictionary<string, CSIDL> KnownFolderMap
        {
            get
            {
                if (_KnownFolderMap == null)
                {
                    _KnownFolderMap = new Dictionary<string, CSIDL>(0x31, StringComparer.OrdinalIgnoreCase);
                    _KnownFolderMap.Add("Administrative Tools", CSIDL.CSIDL_ADMINTOOLS);
                    _KnownFolderMap.Add("AppData", CSIDL.CSIDL_APPDATA);
                    _KnownFolderMap.Add("Cache", CSIDL.CSIDL_INTERNET_CACHE);
                    _KnownFolderMap.Add("CD Burning", CSIDL.CSIDL_CDBURN_AREA);
                    _KnownFolderMap.Add("Common Administrative Tools", CSIDL.CSIDL_COMMON_ADMINTOOLS);
                    _KnownFolderMap.Add("Common AppData", CSIDL.CSIDL_COMMON_APPDATA);
                    _KnownFolderMap.Add("Common Desktop", CSIDL.CSIDL_COMMON_DESKTOPDIRECTORY);
                    _KnownFolderMap.Add("Common Documents", CSIDL.CSIDL_COMMON_DOCUMENTS);
                    _KnownFolderMap.Add("Common Programs", CSIDL.CSIDL_COMMON_PROGRAMS);
                    _KnownFolderMap.Add("Common Start Menu", CSIDL.CSIDL_COMMON_STARTMENU);
                    _KnownFolderMap.Add("Common Startup", CSIDL.CSIDL_COMMON_STARTUP);
                    _KnownFolderMap.Add("Common Templates", CSIDL.CSIDL_COMMON_TEMPLATES);
                    _KnownFolderMap.Add("CommonMusic", CSIDL.CSIDL_COMMON_MUSIC);
                    _KnownFolderMap.Add("CommonPictures", CSIDL.CSIDL_COMMON_PICTURES);
                    _KnownFolderMap.Add("CommonVideo", CSIDL.CSIDL_COMMON_VIDEO);
                    _KnownFolderMap.Add("ConnectionsFolder", CSIDL.CSIDL_CONNECTIONS);
                    _KnownFolderMap.Add("ControlPanelFolder", CSIDL.CSIDL_CONTROLS);
                    _KnownFolderMap.Add("Cookies", CSIDL.CSIDL_COOKIES);
                    _KnownFolderMap.Add("Desktop", CSIDL.CSIDL_DESKTOPDIRECTORY);
                    _KnownFolderMap.Add("Favorites", CSIDL.CSIDL_FAVORITES);
                    _KnownFolderMap.Add("Fonts", CSIDL.CSIDL_FONTS);
                    _KnownFolderMap.Add("History", CSIDL.CSIDL_HISTORY);
                    _KnownFolderMap.Add("InternetFolder", CSIDL.CSIDL_INTERNET);
                    _KnownFolderMap.Add("Local AppData", CSIDL.CSIDL_LOCAL_APPDATA);
                    _KnownFolderMap.Add("LocalizedResourcesDir", CSIDL.CSIDL_RESOURCES_LOCALIZED);
                    _KnownFolderMap.Add("My Music", CSIDL.CSIDL_MYMUSIC);
                    _KnownFolderMap.Add("My Pictures", CSIDL.CSIDL_MYPICTURES);
                    _KnownFolderMap.Add("My Video", CSIDL.CSIDL_MYVIDEO);
                    _KnownFolderMap.Add("MyComputerFolder", CSIDL.CSIDL_DRIVES);
                    _KnownFolderMap.Add("NetHood", CSIDL.CSIDL_NETHOOD);
                    _KnownFolderMap.Add("NetworkPlacesFolder", CSIDL.CSIDL_NETWORK);
                    _KnownFolderMap.Add("OEM Links", CSIDL.CSIDL_COMMON_OEM_LINKS);
                    _KnownFolderMap.Add("Personal", CSIDL.CSIDL_PERSONAL);
                    _KnownFolderMap.Add("PrintersFolder", CSIDL.CSIDL_PRINTERS);
                    _KnownFolderMap.Add("PrintHood", CSIDL.CSIDL_PRINTHOOD);
                    _KnownFolderMap.Add("Profile", CSIDL.CSIDL_PROFILE);
                    _KnownFolderMap.Add("ProgramFiles", CSIDL.CSIDL_PROGRAM_FILES);
                    _KnownFolderMap.Add("ProgramFilesCommon", CSIDL.CSIDL_PROGRAM_FILES_COMMON);
                    _KnownFolderMap.Add("Programs", CSIDL.CSIDL_PROGRAMS);
                    _KnownFolderMap.Add("Recent", CSIDL.CSIDL_RECENT);
                    _KnownFolderMap.Add("RecycleBinFolder", CSIDL.CSIDL_BITBUCKET);
                    _KnownFolderMap.Add("ResourceDir", CSIDL.CSIDL_RESOURCES);
                    _KnownFolderMap.Add("SendTo", CSIDL.CSIDL_SENDTO);
                    _KnownFolderMap.Add("Start Menu", CSIDL.CSIDL_STARTMENU);
                    _KnownFolderMap.Add("Startup", CSIDL.CSIDL_STARTUP);
                    _KnownFolderMap.Add("System", CSIDL.CSIDL_SYSTEM);
                    _KnownFolderMap.Add("Templates", CSIDL.CSIDL_TEMPLATES);
                    _KnownFolderMap.Add("UserProfiles", CSIDL.CSIDL_PROFILES);
                    _KnownFolderMap.Add("Windows", CSIDL.CSIDL_WINDOWS);
                }
                return _KnownFolderMap;
            }
        }

        [CompilerGenerated]
        private sealed class <GetNames>d__0 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private string <>2__current;
            public IEnumerator<string> <>7__wrapa;
            private int <>l__initialThreadId;
            public IKnownFolder <Folder>5__6;
            public KNOWNFOLDER_DEFINITION <FolderDefinion>5__7;
            public uint <FolderIdCount>5__3;
            public IntPtr <FolderIdPtr>5__2;
            public IKnownFolderManager <FolderManager>5__1;
            public byte[] <GuidArray>5__4;
            public int <I>5__5;
            public string <NextKnownName>5__8;

            [DebuggerHidden]
            public <GetNames>d__0(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally9()
            {
                this.<>1__state = -1;
                Marshal.FinalReleaseComObject(this.<FolderManager>5__1);
            }

            private void <>m__Finallyb()
            {
                this.<>1__state = -1;
                if (this.<>7__wrapa != null)
                {
                    this.<>7__wrapa.Dispose();
                }
            }

            private bool MoveNext()
            {
                bool flag;
                try
                {
                    switch (this.<>1__state)
                    {
                        case 3:
                            goto Label_0131;

                        case 5:
                            goto Label_01B0;

                        case 0:
                            this.<>1__state = -1;
                            if (!OS.IsWinVista)
                            {
                                break;
                            }
                            this.<FolderManager>5__1 = (IKnownFolderManager) new CoKnownFolderManager();
                            this.<>1__state = 1;
                            this.<FolderIdCount>5__3 = 0;
                            this.<FolderManager>5__1.GetFolderIds(out this.<FolderIdPtr>5__2, ref this.<FolderIdCount>5__3);
                            this.<GuidArray>5__4 = new byte[0x10];
                            this.<I>5__5 = 0;
                            while (this.<I>5__5 < this.<FolderIdCount>5__3)
                            {
                                Marshal.Copy(this.<FolderIdPtr>5__2, this.<GuidArray>5__4, 0, this.<GuidArray>5__4.Length);
                                this.<FolderIdPtr>5__2 = this.<FolderIdPtr>5__2.Offset(this.<GuidArray>5__4.Length);
                                this.<FolderManager>5__1.GetFolder(new Guid(this.<GuidArray>5__4), out this.<Folder>5__6);
                                try
                                {
                                    this.<Folder>5__6.GetFolderDefinition(out this.<FolderDefinion>5__7);
                                }
                                finally
                                {
                                    Marshal.FinalReleaseComObject(this.<Folder>5__6);
                                }
                                this.<>2__current = this.<FolderDefinion>5__7.pszName;
                                this.<>1__state = 3;
                                return true;
                            Label_0131:
                                this.<>1__state = 1;
                                this.<I>5__5++;
                            }
                            this.<>m__Finally9();
                            goto Label_01CE;

                        default:
                            goto Label_01CE;
                    }
                    this.<>7__wrapa = KnownFolder.KnownFolderMap.Keys.GetEnumerator();
                    this.<>1__state = 4;
                    while (this.<>7__wrapa.MoveNext())
                    {
                        this.<NextKnownName>5__8 = this.<>7__wrapa.Current;
                        this.<>2__current = this.<NextKnownName>5__8;
                        this.<>1__state = 5;
                        return true;
                    Label_01B0:
                        this.<>1__state = 4;
                    }
                    this.<>m__Finallyb();
                Label_01CE:
                    flag = false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
                return flag;
            }

            [DebuggerHidden]
            IEnumerator<string> IEnumerable<string>.GetEnumerator()
            {
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    return this;
                }
                return new KnownFolder.<GetNames>d__0(0);
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<System.String>.GetEnumerator();
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
                    case 3:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finally9();
                        }
                        break;

                    case 4:
                    case 5:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finallyb();
                        }
                        break;
                }
            }

            string IEnumerator<string>.Current
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

