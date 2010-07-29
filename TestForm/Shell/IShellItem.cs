using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace TestForm.Shell
{
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe")]
    public interface IShellItem
    {
        /// <summary>
        /// Binds to a handler for an item as specified by the handler ID value (BHID).
        /// </summary>
        /// <param name="pbc">A pointer to an IBindCtx  interface on a bind context object. Used to pass optional parameters to the handler. The contents of the bind context are handler-specific. For example, when binding to BHID_Stream, the STGM flags in the bind context indicate the mode of access desired (read versus read/write).</param>
        /// <param name="bhid">Reference to a GUID that specifies which handler will be created.</param>
        /// <param name="riid">IID of the object type to retrieve.</param>
        /// <param name="ppv">When this method returns, contains a pointer of type riid that is returned by the handler specified by rbhid.</param>
        void BindToHandler(IntPtr pbc, [MarshalAs(UnmanagedType.LPStruct)] Guid bhid, [MarshalAs(UnmanagedType.LPStruct)] Guid riid, [Out, MarshalAs(UnmanagedType.Interface)] out IShellFolder ppv);
        /// <summary>
        /// Gets the parent of an IShellItem object.
        /// </summary>
        /// <param name="ppsi">The address of a pointer to the parent of an IShellItem interface.</param>
        void GetParent(out IShellItem ppsi);
        /// <summary>
        /// Gets the display name of the IShellItem object.
        /// </summary>
        /// <param name="sigdnName">One of the SIGDN values that indicates how the name should look.</param>
        /// <param name="ppszName">A value that, when this function returns successfully, receives the address of a pointer to the retrieved display name.</param>
        void GetDisplayName(SIGDN sigdnName, [MarshalAs(UnmanagedType.LPWStr)] out string ppszName);
        /// <summary>
        /// Gets a requested set of attributes of the IShellItem object.
        /// </summary>
        /// <param name="sfgaoMask">Specifies the attributes to retrieve. One or more of the SFGAO values. Use a bitwise OR operator to determine the attributes to retrieve.</param>
        /// <param name="psfgaoAttribs">A pointer to a value that, when this method returns successfully, contains the requested attributes. One or more of the SFGAO values. Only those attributes specified by sfgaoMask are returned; other attribute values are undefined.</param>
        void GetAttributes(SFGAO sfgaoMask, out SFGAO psfgaoAttribs);
        /// <summary>
        /// Compares two IShellItem objects.
        /// </summary>
        /// <param name="psi">A pointer to an IShellItem object to compare with the existing IShellItem object. </param>
        /// <param name="hint">One of the SICHINTF values that determines how to perform the comparison. See SICHINTF for the list of possible values for this parameter.</param>
        /// <param name="piOrder">This parameter receives the result of the comparison. If the two items are the same this parameter equals zero; if they are different the parameter is nonzero.</param>
        void Compare(IShellItem psi, SICHINTF hint, out int piOrder);
    }

    public enum SICHINTF : uint
    {
        SICHINT_DISPLAY = 0x00000000,
        SICHINT_CANONICAL = 0x10000000,
        SICHINT_TEST_FILESYSPATH_IF_NOT_EQUAL = 0x20000000,
        SICHINT_ALLFIELDS = 0x80000000
    }


}
