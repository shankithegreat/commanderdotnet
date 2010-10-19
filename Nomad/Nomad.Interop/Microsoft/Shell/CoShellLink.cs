namespace Microsoft.Shell
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, ClassInterface(ClassInterfaceType.None), Guid("00021401-0000-0000-C000-000000000046")]
    public class CoShellLink
    {
        public const uint EXP_DARWIN_ID_SIG = 0xa0000006;
        public const uint EXP_LOGO3_ID_SIG = 0xa0000007;
        public const uint EXP_SPECIAL_FOLDER_SIG = 0xa0000005;
        public const uint EXP_SZ_ICON_SIG = 0xa0000007;
        public const uint EXP_SZ_LINK_SIG = 0xa0000001;
        public const uint NT_CONSOLE_PROPS_SIG = 0xa0000002;
        public const uint NT_FE_CONSOLE_PROPS_SIG = 0xa0000004;
    }
}

