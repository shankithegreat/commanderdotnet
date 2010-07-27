using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace TestForm
{
    public static class BZip2ArchiveHelper
    {
        private const string dllName = "bzip2.dll";

        /// <summary>
        /// Perform all necessary operations when an archive is to be opened.
        /// </summary>
        /// <param name="archiveData">The information about the archive being open.</param>
        /// <returns>The unique handle representing the archive.</returns>
        [DllImport(dllName)]
        public static extern int OpenArchive(ref OpenArchiveData archiveData);
        /// <summary>
        /// Perform all necessary operations when an archive is about to be closed.
        /// </summary>
        /// <param name="handle">The value returned by a programmer within a previous call to OpenArchive.</param>
        /// <returns>Return zero on success, or one of the error values otherwise.</returns>
        [DllImport(dllName)]
        public static extern int CloseArchive(int handle);
        /// <summary>
        /// Gets the information about the next file contained in the archive.
        /// </summary>
        /// <param name="handle">The handle returned by OpenArchive.</param>
        /// <param name="headerData">The information about the next file contained in the archive.</param>
        /// <returns>When all files in the archive have been returned, ReadHeader return E_END_ARCHIVE which will prevent ReaderHeader from being called again. If an error occurs, ReadHeader return one of the error values or 0 for no error.</returns>
        [DllImport(dllName)]
        public static extern int ReadHeader(int handle, ref HeaderData headerData);
        /// <summary>
        /// Unpack the specified file or test the integrity of the archive.
        /// </summary>
        /// <param name="handle">The handle returned by OpenArchive.</param>
        /// <param name="operation"></param>
        /// <param name="destPath"></param>
        /// <param name="destName">Either DestName contains the full path and file name and DestPath is NULL, or DestName contains only the file name and DestPath the file path. This is done for compatibility with unrar.dll.</param>
        /// <returns></returns>
        [DllImport(dllName)]
        public static extern int ProcessFile(int handle, OperationMode operation, string destPath, string destName);
        /// <summary>
        /// This function allows you to notify user about changing a volume when packing files.
        /// </summary>
        /// <param name="handle">The handle returned by OpenArchive.</param>
        /// <param name="changeVolProc">The pointer to a function that you may want to call when notifying user to change volume (e.g. insterting another diskette).</param>
        [DllImport(dllName)]
        public static extern void SetChangeVolProc(int handle, ChangeVolumeHandle changeVolProc);
        /// <summary>
        /// This function allows you to notify user about the progress when you un/pack files.
        /// </summary>
        /// <param name="handle">The handle returned by OpenArchive.</param>
        /// <param name="processDataProc">The pointer to a function that you may want to call when notifying user about the progress being made when you pack or extract files from an archive.</param>
        [DllImport(dllName)]
        public static extern void SetProcessDataProc(int handle, ProcessDataHandle processDataProc);

        /// <summary>
        /// Specifies what should happen when a user creates, or adds files to the archive.
        /// </summary>
        /// <param name="packedFile">Refers to the archive that is to be created or modified. The string contains the full path.</param>
        /// <param name="subPath">Is either NULL, when the files should be packed with the paths given with the file names, or not NULL when they should be placed below the given subdirectory within the archive. 
        ///    Example:
        ///       SubPath="subdirectory"
        ///       Name in AddList="subdir2\filename.ext"
        ///       -> File should be packed as "subdirectory\subdir2\filename.ext"</param>
        /// <param name="srcPath">The path to the files in AddList</param>
        /// <param name="addList">Each string in AddList is zero-delimited (ends in zero), and the AddList string ends with an extra zero byte, i.e. there are two zero bytes at the end of AddList.</param>
        /// <param name="flags">The flags.</param>
        /// <returns>Return zero on success, or one of the error values otherwise.</returns>
        [DllImport(dllName)]
        public static extern int PackFiles(string packedFile, string subPath, string srcPath, string addList, PackMode flags);
        /// <summary>
        /// Delete the specified files from the archive.
        /// </summary>
        /// <param name="packedFile">The full path and name of the the archive.</param>
        /// <param name="deleteList">The list of files that should be deleted from the archive. The format of this string is the same as AddList within PackFiles.</param>
        /// <returns>Return zero on success, or one of the error values otherwise.</returns>
        [DllImport(dllName)]
        public static extern int DeleteFiles(string packedFile, string deleteList);
        /// <summary>
        /// Tells what features your packer plugin supports.
        /// </summary>
        /// <returns>Omitting PK_CAPS_NEW and PK_CAPS_MODIFY means PackFiles will never be called and so you don’t have to implement PackFiles. Omitting PK_CAPS_MULTIPLE means PackFiles will be supplied with just one file. Leaving out PK_CAPS_DELETE means DeleteFiles will never be called; leaving out PK_CAPS_OPTIONS means ConfigurePacker will not be called. PK_CAPS_MEMPACK enables the functions StartMemPack, PackToMem and DoneMemPack. If PK_CAPS_BY_CONTENT is returned, Totalcmd calls the function CanYouHandleThisFile when the user presses Ctrl+PageDown on an unknown archive type. Finally, if PK_CAPS_SEARCHTEXT is returned, Total Commander will search for text inside files packed with this plugin. This may not be a good idea for certain plugins like the diskdir plugin, where file contents may not be available. If PK_CAPS_HIDE is set, the plugin will not show the file type as a packer. This is useful for plugins which are mainly used for creating files, e.g. to create batch files, avi files etc. The file needs to be opened with Ctrl+PgDn in this case, because Enter will launch the associated application.</returns>
        [DllImport(dllName)]
        public static extern CapsType GetPackerCaps();
        /// <summary>
        /// Gets called when the user clicks the Configure button from within "Pack files..." dialog box.
        /// </summary>
        /// <param name="parent">The window handle of main process.</param>
        /// <param name="dllInstance">The handle of the DLL (your DLL) that creates your dialog box.</param>
        [DllImport(dllName)]
        public static extern void ConfigurePacker(int parent, int dllInstance);
        /// <summary>
        /// Starts packing into memory. This function is only needed if you want to create archives in combination with TAR, e.g. TAR.BZ2. It allows Totalcmd to create a TAR.Plugin file in a single step.
        /// </summary>
        /// <param name="options">1 - The output stream should include the complete headers (beginning+end)</param>
        /// <param name="fileName">The name of the file being packed - some packers store the name in the local header.</param>
        /// <returns>Return a user-defined handle (e.g. pointer to a structure) on success, zero otherwise.</returns>
        [DllImport(dllName)]
        public static extern int StartMemPack(int options, string fileName);
        /// <summary>
        /// Packs the next chunk of data passed to it and/or returns the compressed data to the calling program. It is implemented together with StartMemPack and DoneMemPack
        /// Description of the function:
        ///    PackToMem is the most complex function of the packer plugin. It is called by Total Commander in a loop as long as there is data to be packed, and as there is data to retrieve. The plugin should do the following:
        ///    1.	As long as there is data sent through BufIn, take it and add it to your internal buffers (if there is enough space).
        ///    2.	As soon as there is enough data in the internal input buffers, start packing to the output buffers.
        ///    3.	As soon as there is enough data in the internal output buffers, start sending data to BufOut.
        ///    4.	When InLen is 0, there is no more data to be compressed, so finish sending data to BufOut until no more data is in the output buffer.
        ///    5.	When there is no more data available, return 1.
        ///    6.	There is no obligation to take any data through BufIn or send any through BufOut. Total Commander will call this function until it either returns 1, or an error.
        /// </summary>
        /// <param name="hMemPack">The handle returned by StartMemPack().</param>
        /// <param name="bufIn">The pointer to the data which needs to be packed.</param>
        /// <param name="inLen">The number of bytes pointed to by BufIn.</param>
        /// <param name="taken">The number of bytes taken from the buffer. If not the whole buffer is taken, the calling program will pass the remaining bytes to the plugin in a later call.</param>
        /// <param name="bufOut">The pointer to a buffer which can receive packed data.</param>
        /// <param name="outLen">The size of the buffer pointed to by BufOut.</param>
        /// <param name="written">The number of bytes placed in the buffer pointed to by BufOut.</param>
        /// <param name="seekBy">The offset from the current output posisition by which the file pointer has to be moved BEFORE accepting the data in BufOut. This allows the plugin to modify a file header also AFTER packing, e.g. to write a CRC to the header.</param>
        /// <returns>Return MEMPACK_OK (=0) on success, MEMPACK_DONE (=1) when done, or one of the error values otherwise.</returns>
        [DllImport(dllName)]
        public static extern int PackToMem(int hMemPack, string bufIn, int inLen, string taken, string bufOut, int outLen, string written, int seekBy);
        /// <summary>
        /// Ends packing into memory. This function is used together with StartMemPack and PackToMem.
        /// </summary>
        /// <param name="hMemPack">The handle returned by StartMemPack.</param>
        /// <returns>Return zero if successful, or one of the error codes otherwise.</returns>
        [DllImport(dllName)]
        public static extern int DoneMemPack(int hMemPack);
        /// <summary>
        /// Allows the plugin to handle files with different extensions than the one defined in main process. 
        /// It is called when the plugin defines PK_CAPS_BY_CONTENT, and the user tries to open an archive with Ctrl+PageDown.
        /// </summary>
        /// <param name="fileName">The fully qualified name (path+name) of the file to be checked.</param>
        /// <returns>Return true (nonzero) if the plugin recognizes the file as an archive which it can handle. The detection must be by contents, NOT by extension. If this function is not implemented, Totalcmd assumes that only files with a given extension can be handled by the plugin.</returns>
        [DllImport(dllName)]
        public static extern bool CanYouHandleThisFile(string fileName);
        /// <summary>
        /// Called immediately after loading the DLL, before any other function. This function is new in version 2.1. It requires Total Commander >=5.51, but is ignored by older versions.
        /// </summary>
        /// <param name="dps">This structure of type PackDefaultParamStruct currently contains the version number of the plugin interface, and the suggested location for the settings file (ini file). It is recommended to store any plugin-specific information either directly in that file, or in that directory under a different name. Make sure to use a unique header when storing data in this file, because it is shared by other file system plugins! If your plugin needs more than 1kbyte of data, you should use your own ini file because ini files are limited to 64k.</param>
        [DllImport(dllName)]
        public static extern void PackSetDefaultParams(ref PackDefaultParam dps);
        /// <summary>
        /// Called when loading the plugin. The passed values should be stored in the plugin for later use. This function is only needed if you want to use the secure password store in Total Commander.
        /// </summary>
        /// <param name="pPkCryptProc">The pointer to the crypto callback function. See PkCryptProc for a description of this function.</param>
        /// <param name="cryptoNr">The parameter which needs to be passed to the callback function.</param>
        /// <param name="flags">Regarding the crypto connection. Currently only PK_CRYPTOPT_MASTERPASS_SET is defined. It is set when the user has defined a master password.</param>
        [DllImport(dllName)]
        public static extern void PkSetCryptCallback(PkCryptHandle pPkCryptProc, int cryptoNr, int flags);
    }

    public static class Zip7ArchiveHelper
    {
        private const string dllName = "7zip.dll";

        /// <summary>
        /// Perform all necessary operations when an archive is to be opened.
        /// </summary>
        /// <param name="archiveData">The information about the archive being open.</param>
        /// <returns>The unique handle representing the archive.</returns>
        [DllImport(dllName)]
        public static extern int OpenArchive(ref OpenArchiveData archiveData);
        /// <summary>
        /// Perform all necessary operations when an archive is about to be closed.
        /// </summary>
        /// <param name="handle">The value returned by a programmer within a previous call to OpenArchive.</param>
        /// <returns>Return zero on success, or one of the error values otherwise.</returns>
        [DllImport(dllName)]
        public static extern int CloseArchive(int handle);
        /// <summary>
        /// Gets the information about the next file contained in the archive.
        /// </summary>
        /// <param name="handle">The handle returned by OpenArchive.</param>
        /// <param name="headerData">The information about the next file contained in the archive.</param>
        /// <returns>When all files in the archive have been returned, ReadHeader return E_END_ARCHIVE which will prevent ReaderHeader from being called again. If an error occurs, ReadHeader return one of the error values or 0 for no error.</returns>
        [DllImport(dllName)]
        public static extern int ReadHeader(int handle, ref HeaderData headerData);
        /// <summary>
        /// Unpack the specified file or test the integrity of the archive.
        /// </summary>
        /// <param name="handle">The handle returned by OpenArchive.</param>
        /// <param name="operation"></param>
        /// <param name="destPath"></param>
        /// <param name="destName">Either DestName contains the full path and file name and DestPath is NULL, or DestName contains only the file name and DestPath the file path. This is done for compatibility with unrar.dll.</param>
        /// <returns></returns>
        [DllImport(dllName)]
        public static extern int ProcessFile(int handle, OperationMode operation, string destPath, string destName);
        /// <summary>
        /// This function allows you to notify user about changing a volume when packing files.
        /// </summary>
        /// <param name="handle">The handle returned by OpenArchive.</param>
        /// <param name="changeVolProc">The pointer to a function that you may want to call when notifying user to change volume (e.g. insterting another diskette).</param>
        [DllImport(dllName)]
        public static extern void SetChangeVolProc(int handle, ChangeVolumeHandle changeVolProc);
        /// <summary>
        /// This function allows you to notify user about the progress when you un/pack files.
        /// </summary>
        /// <param name="handle">The handle returned by OpenArchive.</param>
        /// <param name="processDataProc">The pointer to a function that you may want to call when notifying user about the progress being made when you pack or extract files from an archive.</param>
        [DllImport(dllName)]
        public static extern void SetProcessDataProc(int handle, ProcessDataHandle processDataProc);

        /// <summary>
        /// Specifies what should happen when a user creates, or adds files to the archive.
        /// </summary>
        /// <param name="packedFile">Refers to the archive that is to be created or modified. The string contains the full path.</param>
        /// <param name="subPath">Is either NULL, when the files should be packed with the paths given with the file names, or not NULL when they should be placed below the given subdirectory within the archive. 
        ///    Example:
        ///       SubPath="subdirectory"
        ///       Name in AddList="subdir2\filename.ext"
        ///       -> File should be packed as "subdirectory\subdir2\filename.ext"</param>
        /// <param name="srcPath">The path to the files in AddList</param>
        /// <param name="addList">Each string in AddList is zero-delimited (ends in zero), and the AddList string ends with an extra zero byte, i.e. there are two zero bytes at the end of AddList.</param>
        /// <param name="flags">The flags.</param>
        /// <returns>Return zero on success, or one of the error values otherwise.</returns>
        [DllImport(dllName)]
        public static extern int PackFiles(string packedFile, string subPath, string srcPath, string addList, PackMode flags);
        /// <summary>
        /// Delete the specified files from the archive.
        /// </summary>
        /// <param name="packedFile">The full path and name of the the archive.</param>
        /// <param name="deleteList">The list of files that should be deleted from the archive. The format of this string is the same as AddList within PackFiles.</param>
        /// <returns>Return zero on success, or one of the error values otherwise.</returns>
        [DllImport(dllName)]
        public static extern int DeleteFiles(string packedFile, string deleteList);
        /// <summary>
        /// Tells what features your packer plugin supports.
        /// </summary>
        /// <returns>Omitting PK_CAPS_NEW and PK_CAPS_MODIFY means PackFiles will never be called and so you don’t have to implement PackFiles. Omitting PK_CAPS_MULTIPLE means PackFiles will be supplied with just one file. Leaving out PK_CAPS_DELETE means DeleteFiles will never be called; leaving out PK_CAPS_OPTIONS means ConfigurePacker will not be called. PK_CAPS_MEMPACK enables the functions StartMemPack, PackToMem and DoneMemPack. If PK_CAPS_BY_CONTENT is returned, Totalcmd calls the function CanYouHandleThisFile when the user presses Ctrl+PageDown on an unknown archive type. Finally, if PK_CAPS_SEARCHTEXT is returned, Total Commander will search for text inside files packed with this plugin. This may not be a good idea for certain plugins like the diskdir plugin, where file contents may not be available. If PK_CAPS_HIDE is set, the plugin will not show the file type as a packer. This is useful for plugins which are mainly used for creating files, e.g. to create batch files, avi files etc. The file needs to be opened with Ctrl+PgDn in this case, because Enter will launch the associated application.</returns>
        [DllImport(dllName)]
        public static extern CapsType GetPackerCaps();
        /// <summary>
        /// Gets called when the user clicks the Configure button from within "Pack files..." dialog box.
        /// </summary>
        /// <param name="parent">The window handle of main process.</param>
        /// <param name="dllInstance">The handle of the DLL (your DLL) that creates your dialog box.</param>
        [DllImport(dllName)]
        public static extern void ConfigurePacker(int parent, int dllInstance);
        /// <summary>
        /// Starts packing into memory. This function is only needed if you want to create archives in combination with TAR, e.g. TAR.BZ2. It allows Totalcmd to create a TAR.Plugin file in a single step.
        /// </summary>
        /// <param name="options">1 - The output stream should include the complete headers (beginning+end)</param>
        /// <param name="fileName">The name of the file being packed - some packers store the name in the local header.</param>
        /// <returns>Return a user-defined handle (e.g. pointer to a structure) on success, zero otherwise.</returns>
        [DllImport(dllName)]
        public static extern int StartMemPack(int options, string fileName);
        /// <summary>
        /// Packs the next chunk of data passed to it and/or returns the compressed data to the calling program. It is implemented together with StartMemPack and DoneMemPack
        /// Description of the function:
        ///    PackToMem is the most complex function of the packer plugin. It is called by Total Commander in a loop as long as there is data to be packed, and as there is data to retrieve. The plugin should do the following:
        ///    1.	As long as there is data sent through BufIn, take it and add it to your internal buffers (if there is enough space).
        ///    2.	As soon as there is enough data in the internal input buffers, start packing to the output buffers.
        ///    3.	As soon as there is enough data in the internal output buffers, start sending data to BufOut.
        ///    4.	When InLen is 0, there is no more data to be compressed, so finish sending data to BufOut until no more data is in the output buffer.
        ///    5.	When there is no more data available, return 1.
        ///    6.	There is no obligation to take any data through BufIn or send any through BufOut. Total Commander will call this function until it either returns 1, or an error.
        /// </summary>
        /// <param name="hMemPack">The handle returned by StartMemPack().</param>
        /// <param name="bufIn">The pointer to the data which needs to be packed.</param>
        /// <param name="inLen">The number of bytes pointed to by BufIn.</param>
        /// <param name="taken">The number of bytes taken from the buffer. If not the whole buffer is taken, the calling program will pass the remaining bytes to the plugin in a later call.</param>
        /// <param name="bufOut">The pointer to a buffer which can receive packed data.</param>
        /// <param name="outLen">The size of the buffer pointed to by BufOut.</param>
        /// <param name="written">The number of bytes placed in the buffer pointed to by BufOut.</param>
        /// <param name="seekBy">The offset from the current output posisition by which the file pointer has to be moved BEFORE accepting the data in BufOut. This allows the plugin to modify a file header also AFTER packing, e.g. to write a CRC to the header.</param>
        /// <returns>Return MEMPACK_OK (=0) on success, MEMPACK_DONE (=1) when done, or one of the error values otherwise.</returns>
        [DllImport(dllName)]
        public static extern int PackToMem(int hMemPack, string bufIn, int inLen, string taken, string bufOut, int outLen, string written, int seekBy);
        /// <summary>
        /// Ends packing into memory. This function is used together with StartMemPack and PackToMem.
        /// </summary>
        /// <param name="hMemPack">The handle returned by StartMemPack.</param>
        /// <returns>Return zero if successful, or one of the error codes otherwise.</returns>
        [DllImport(dllName)]
        public static extern int DoneMemPack(int hMemPack);
        /// <summary>
        /// Allows the plugin to handle files with different extensions than the one defined in main process. 
        /// It is called when the plugin defines PK_CAPS_BY_CONTENT, and the user tries to open an archive with Ctrl+PageDown.
        /// </summary>
        /// <param name="fileName">The fully qualified name (path+name) of the file to be checked.</param>
        /// <returns>Return true (nonzero) if the plugin recognizes the file as an archive which it can handle. The detection must be by contents, NOT by extension. If this function is not implemented, Totalcmd assumes that only files with a given extension can be handled by the plugin.</returns>
        [DllImport(dllName)]
        public static extern bool CanYouHandleThisFile(string fileName);
        /// <summary>
        /// Called immediately after loading the DLL, before any other function. This function is new in version 2.1. It requires Total Commander >=5.51, but is ignored by older versions.
        /// </summary>
        /// <param name="dps">This structure of type PackDefaultParamStruct currently contains the version number of the plugin interface, and the suggested location for the settings file (ini file). It is recommended to store any plugin-specific information either directly in that file, or in that directory under a different name. Make sure to use a unique header when storing data in this file, because it is shared by other file system plugins! If your plugin needs more than 1kbyte of data, you should use your own ini file because ini files are limited to 64k.</param>
        [DllImport(dllName)]
        public static extern void PackSetDefaultParams(ref PackDefaultParam dps);
        /// <summary>
        /// Called when loading the plugin. The passed values should be stored in the plugin for later use. This function is only needed if you want to use the secure password store in Total Commander.
        /// </summary>
        /// <param name="pPkCryptProc">The pointer to the crypto callback function. See PkCryptProc for a description of this function.</param>
        /// <param name="cryptoNr">The parameter which needs to be passed to the callback function.</param>
        /// <param name="flags">Regarding the crypto connection. Currently only PK_CRYPTOPT_MASTERPASS_SET is defined. It is set when the user has defined a master password.</param>
        [DllImport(dllName)]
        public static extern void PkSetCryptCallback(PkCryptHandle pPkCryptProc, int cryptoNr, int flags);
    }

    public static class IsoArchiveHelper
    {
        private const string dllName = "iso.dll";

        /// <summary>
        /// Perform all necessary operations when an archive is to be opened.
        /// </summary>
        /// <param name="archiveData">The information about the archive being open.</param>
        /// <returns>The unique handle representing the archive.</returns>
        [DllImport(dllName)]
        public static extern int OpenArchive(ref OpenArchiveData archiveData);
        /// <summary>
        /// Perform all necessary operations when an archive is about to be closed.
        /// </summary>
        /// <param name="handle">The value returned by a programmer within a previous call to OpenArchive.</param>
        /// <returns>Return zero on success, or one of the error values otherwise.</returns>
        [DllImport(dllName)]
        public static extern int CloseArchive(int handle);
        /// <summary>
        /// Gets the information about the next file contained in the archive.
        /// </summary>
        /// <param name="handle">The handle returned by OpenArchive.</param>
        /// <param name="headerData">The information about the next file contained in the archive.</param>
        /// <returns>When all files in the archive have been returned, ReadHeader return E_END_ARCHIVE which will prevent ReaderHeader from being called again. If an error occurs, ReadHeader return one of the error values or 0 for no error.</returns>
        [DllImport(dllName)]
        public static extern int ReadHeader(int handle, ref HeaderData headerData);
        /// <summary>
        /// Unpack the specified file or test the integrity of the archive.
        /// </summary>
        /// <param name="handle">The handle returned by OpenArchive.</param>
        /// <param name="operation"></param>
        /// <param name="destPath"></param>
        /// <param name="destName">Either DestName contains the full path and file name and DestPath is NULL, or DestName contains only the file name and DestPath the file path. This is done for compatibility with unrar.dll.</param>
        /// <returns></returns>
        [DllImport(dllName)]
        public static extern int ProcessFile(int handle, OperationMode operation, string destPath, string destName);
        /// <summary>
        /// This function allows you to notify user about changing a volume when packing files.
        /// </summary>
        /// <param name="handle">The handle returned by OpenArchive.</param>
        /// <param name="changeVolProc">The pointer to a function that you may want to call when notifying user to change volume (e.g. insterting another diskette).</param>
        [DllImport(dllName)]
        public static extern void SetChangeVolProc(int handle, ChangeVolumeHandle changeVolProc);
        /// <summary>
        /// This function allows you to notify user about the progress when you un/pack files.
        /// </summary>
        /// <param name="handle">The handle returned by OpenArchive.</param>
        /// <param name="processDataProc">The pointer to a function that you may want to call when notifying user about the progress being made when you pack or extract files from an archive.</param>
        [DllImport(dllName)]
        public static extern void SetProcessDataProc(int handle, ProcessDataHandle processDataProc);

        /// <summary>
        /// Specifies what should happen when a user creates, or adds files to the archive.
        /// </summary>
        /// <param name="packedFile">Refers to the archive that is to be created or modified. The string contains the full path.</param>
        /// <param name="subPath">Is either NULL, when the files should be packed with the paths given with the file names, or not NULL when they should be placed below the given subdirectory within the archive. 
        ///    Example:
        ///       SubPath="subdirectory"
        ///       Name in AddList="subdir2\filename.ext"
        ///       -> File should be packed as "subdirectory\subdir2\filename.ext"</param>
        /// <param name="srcPath">The path to the files in AddList</param>
        /// <param name="addList">Each string in AddList is zero-delimited (ends in zero), and the AddList string ends with an extra zero byte, i.e. there are two zero bytes at the end of AddList.</param>
        /// <param name="flags">The flags.</param>
        /// <returns>Return zero on success, or one of the error values otherwise.</returns>
        [DllImport(dllName)]
        public static extern int PackFiles(string packedFile, string subPath, string srcPath, string addList, PackMode flags);
        /// <summary>
        /// Delete the specified files from the archive.
        /// </summary>
        /// <param name="packedFile">The full path and name of the the archive.</param>
        /// <param name="deleteList">The list of files that should be deleted from the archive. The format of this string is the same as AddList within PackFiles.</param>
        /// <returns>Return zero on success, or one of the error values otherwise.</returns>
        [DllImport(dllName)]
        public static extern int DeleteFiles(string packedFile, string deleteList);
        /// <summary>
        /// Tells what features your packer plugin supports.
        /// </summary>
        /// <returns>Omitting PK_CAPS_NEW and PK_CAPS_MODIFY means PackFiles will never be called and so you don’t have to implement PackFiles. Omitting PK_CAPS_MULTIPLE means PackFiles will be supplied with just one file. Leaving out PK_CAPS_DELETE means DeleteFiles will never be called; leaving out PK_CAPS_OPTIONS means ConfigurePacker will not be called. PK_CAPS_MEMPACK enables the functions StartMemPack, PackToMem and DoneMemPack. If PK_CAPS_BY_CONTENT is returned, Totalcmd calls the function CanYouHandleThisFile when the user presses Ctrl+PageDown on an unknown archive type. Finally, if PK_CAPS_SEARCHTEXT is returned, Total Commander will search for text inside files packed with this plugin. This may not be a good idea for certain plugins like the diskdir plugin, where file contents may not be available. If PK_CAPS_HIDE is set, the plugin will not show the file type as a packer. This is useful for plugins which are mainly used for creating files, e.g. to create batch files, avi files etc. The file needs to be opened with Ctrl+PgDn in this case, because Enter will launch the associated application.</returns>
        [DllImport(dllName)]
        public static extern CapsType GetPackerCaps();
        /// <summary>
        /// Gets called when the user clicks the Configure button from within "Pack files..." dialog box.
        /// </summary>
        /// <param name="parent">The window handle of main process.</param>
        /// <param name="dllInstance">The handle of the DLL (your DLL) that creates your dialog box.</param>
        [DllImport(dllName)]
        public static extern void ConfigurePacker(int parent, int dllInstance);
        /// <summary>
        /// Starts packing into memory. This function is only needed if you want to create archives in combination with TAR, e.g. TAR.BZ2. It allows Totalcmd to create a TAR.Plugin file in a single step.
        /// </summary>
        /// <param name="options">1 - The output stream should include the complete headers (beginning+end)</param>
        /// <param name="fileName">The name of the file being packed - some packers store the name in the local header.</param>
        /// <returns>Return a user-defined handle (e.g. pointer to a structure) on success, zero otherwise.</returns>
        [DllImport(dllName)]
        public static extern int StartMemPack(int options, string fileName);
        /// <summary>
        /// Packs the next chunk of data passed to it and/or returns the compressed data to the calling program. It is implemented together with StartMemPack and DoneMemPack
        /// Description of the function:
        ///    PackToMem is the most complex function of the packer plugin. It is called by Total Commander in a loop as long as there is data to be packed, and as there is data to retrieve. The plugin should do the following:
        ///    1.	As long as there is data sent through BufIn, take it and add it to your internal buffers (if there is enough space).
        ///    2.	As soon as there is enough data in the internal input buffers, start packing to the output buffers.
        ///    3.	As soon as there is enough data in the internal output buffers, start sending data to BufOut.
        ///    4.	When InLen is 0, there is no more data to be compressed, so finish sending data to BufOut until no more data is in the output buffer.
        ///    5.	When there is no more data available, return 1.
        ///    6.	There is no obligation to take any data through BufIn or send any through BufOut. Total Commander will call this function until it either returns 1, or an error.
        /// </summary>
        /// <param name="hMemPack">The handle returned by StartMemPack().</param>
        /// <param name="bufIn">The pointer to the data which needs to be packed.</param>
        /// <param name="inLen">The number of bytes pointed to by BufIn.</param>
        /// <param name="taken">The number of bytes taken from the buffer. If not the whole buffer is taken, the calling program will pass the remaining bytes to the plugin in a later call.</param>
        /// <param name="bufOut">The pointer to a buffer which can receive packed data.</param>
        /// <param name="outLen">The size of the buffer pointed to by BufOut.</param>
        /// <param name="written">The number of bytes placed in the buffer pointed to by BufOut.</param>
        /// <param name="seekBy">The offset from the current output posisition by which the file pointer has to be moved BEFORE accepting the data in BufOut. This allows the plugin to modify a file header also AFTER packing, e.g. to write a CRC to the header.</param>
        /// <returns>Return MEMPACK_OK (=0) on success, MEMPACK_DONE (=1) when done, or one of the error values otherwise.</returns>
        [DllImport(dllName)]
        public static extern int PackToMem(int hMemPack, string bufIn, int inLen, string taken, string bufOut, int outLen, string written, int seekBy);
        /// <summary>
        /// Ends packing into memory. This function is used together with StartMemPack and PackToMem.
        /// </summary>
        /// <param name="hMemPack">The handle returned by StartMemPack.</param>
        /// <returns>Return zero if successful, or one of the error codes otherwise.</returns>
        [DllImport(dllName)]
        public static extern int DoneMemPack(int hMemPack);
        /// <summary>
        /// Allows the plugin to handle files with different extensions than the one defined in main process. 
        /// It is called when the plugin defines PK_CAPS_BY_CONTENT, and the user tries to open an archive with Ctrl+PageDown.
        /// </summary>
        /// <param name="fileName">The fully qualified name (path+name) of the file to be checked.</param>
        /// <returns>Return true (nonzero) if the plugin recognizes the file as an archive which it can handle. The detection must be by contents, NOT by extension. If this function is not implemented, Totalcmd assumes that only files with a given extension can be handled by the plugin.</returns>
        [DllImport(dllName)]
        public static extern bool CanYouHandleThisFile(string fileName);
        /// <summary>
        /// Called immediately after loading the DLL, before any other function. This function is new in version 2.1. It requires Total Commander >=5.51, but is ignored by older versions.
        /// </summary>
        /// <param name="dps">This structure of type PackDefaultParamStruct currently contains the version number of the plugin interface, and the suggested location for the settings file (ini file). It is recommended to store any plugin-specific information either directly in that file, or in that directory under a different name. Make sure to use a unique header when storing data in this file, because it is shared by other file system plugins! If your plugin needs more than 1kbyte of data, you should use your own ini file because ini files are limited to 64k.</param>
        [DllImport(dllName)]
        public static extern void PackSetDefaultParams(ref PackDefaultParam dps);
        /// <summary>
        /// Called when loading the plugin. The passed values should be stored in the plugin for later use. This function is only needed if you want to use the secure password store in Total Commander.
        /// </summary>
        /// <param name="pPkCryptProc">The pointer to the crypto callback function. See PkCryptProc for a description of this function.</param>
        /// <param name="cryptoNr">The parameter which needs to be passed to the callback function.</param>
        /// <param name="flags">Regarding the crypto connection. Currently only PK_CRYPTOPT_MASTERPASS_SET is defined. It is set when the user has defined a master password.</param>
        [DllImport(dllName)]
        public static extern void PkSetCryptCallback(PkCryptHandle pPkCryptProc, int cryptoNr, int flags);
    }


    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct HeaderData
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string ArcName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string FileName;
        public int Flags;
        public int PackSize;
        public int UnpSize;
        public int HostOS;
        public int FileCRC;
        public int FileTime;
        public int UnpVer;
        public int Method;
        public FileAttributes FileAttr;
        public string CmtBuf;
        public int CmtBufSize;
        public int CmtSize;
        public int CmtState;

        public DateTime Time
        {
            get
            {
                int second = (FileTime & 0x1F) * 2;
                int minute = (FileTime >> 5) & 0x3F;
                int hour = (FileTime >> 11) & 0x1F;
                int day = (FileTime >> 16) & 0x1F;
                int month = (FileTime >> 21) & 0x0F;
                int year = ((FileTime >> 25) & 0x7F) + 1980;

                return new DateTime(year, month, day, hour, minute, second);
                //return DateTime.FromFileTimeUtc(this.FileTime.dwLowDateTime);
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    [ComVisible(true)]
    public struct FileTime
    {
        public FileTime(int year, int month, int day, int hour, int minute, int second)
        {
            this.Year = year;
            this.Hour = hour;
            this.Minute = minute;
            this.Month = month;
            this.Second = second;
            this.Day = day;
        }

        public int Day;
        public int Hour;
        public int Minute;
        public int Month;
        public int Second;
        public int Year;

    }

    public class FileTimeMarshaler : ICustomMarshaler
    {
        public object MarshalNativeToManaged(IntPtr pNativeData)
        {
            int value = pNativeData.ToInt32();

            int second = (value & 0x1F) * 2;
            int minute = (value >> 5) & 0x3F;
            int hour = (value >> 11) & 0x1F;
            int day = (value >> 16) & 0x1F;
            int month = (value >> 21) & 0x0F;
            int year = ((value >> 25) & 0x7F) + 1980;

            return new FileTime(year, month, day, hour, minute, second);
        }

        public IntPtr MarshalManagedToNative(object ManagedObj)
        {
            if (ManagedObj is FileTime)
            {
                FileTime d = (FileTime)ManagedObj;
                int value = (d.Year - 1980) << 25 | d.Month << 21 | d.Day << 16 | d.Hour << 11 | d.Minute << 5 | d.Second / 2;
                return new IntPtr(value);
            }

            return IntPtr.Zero;
        }

        public void CleanUpNativeData(IntPtr pNativeData)
        {
        }

        public void CleanUpManagedData(object ManagedObj)
        {
        }

        public int GetNativeDataSize()
        {
            return 4;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct tHeaderDataEx
    {
        public char[] ArcName;
        public char[] FileName;
        public int Flags;
        public uint PackSize;
        public uint PackSizeHigh;
        public uint UnpSize;
        public uint UnpSizeHigh;
        public int HostOS;
        public int FileCRC;
        public int FileTime;
        public int UnpVer;
        public int Method;
        public int FileAttr;
        public string CmtBuf;
        public int CmtBufSize;
        public int CmtSize;
        public int CmtState;
        public char[] Reserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct tHeaderDataExW
    {
        public int Flags;
        public uint PackSize;
        public uint PackSizeHigh;
        public uint UnpSize;
        public uint UnpSizeHigh;
        public int HostOS;
        public int FileCRC;
        public int FileTime;
        public int UnpVer;
        public int Method;
        public int FileAttr;
        public string CmtBuf;
        public int CmtBufSize;
        public int CmtSize;
        public int CmtState;
        public char[] Reserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct OpenArchiveData
    {
        public string ArcName;
        public OpenMode OpenMode;
        public int OpenResult;
        public string CmtBuf;
        public int CmtBufSize;
        public int CmtSize;
        public int CmtState;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct tOpenArchiveDataW
    {
        public int OpenMode;
        public int OpenResult;
        public int CmtBufSize;
        public int CmtSize;
        public int CmtState;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PackDefaultParam
    {
        public int size;
        public char[] DefaultIniName;
    }

    public delegate int ChangeVolumeHandle(ref char arcName, int mode);
    public delegate int ProcessDataHandle(ref char fileName, int size);
    public delegate int PkCryptHandle(int cryptoNr, int mode, ref char archiveName, ref char password, int maxlen);

    public enum OperationMode
    {
        /// <summary>
        /// Skip this file.
        /// </summary>
        Skip = 0,
        /// <summary>
        /// Test file integrity
        /// </summary>
        Test = 1,
        /// <summary>
        /// Extract to disk
        /// </summary>
        Extract = 2
    }

    public enum PackMode
    {
        /// <summary>
        /// Delete original after packing
        /// </summary>
        MoveFiles = 1,
        /// <summary>
        /// Save path names of files
        /// </summary>
        SavePaths = 2,
        /// <summary>
        /// Ask user for password, then encrypt file with that password
        /// </summary>
        Encrypt = 4
    }

    public enum CapsType
    {
        /// <summary>
        /// Can create new archives
        /// </summary>
        New = 1,
        /// <summary>
        /// Can modify existing archives
        /// </summary>
        Modify = 2,
        /// <summary>
        /// Archive can contain multiple files
        /// </summary>
        Multiple = 4,
        /// <summary>
        /// Can delete files
        /// </summary>
        Delete = 8,
        /// <summary>
        /// Has options dialog
        /// </summary>
        Options = 16,
        /// <summary>
        /// Supports packing in memory
        /// </summary>
        Mempack = 32,
        /// <summary>
        /// Detect archive type by content
        /// </summary>
        ByContent = 64,
        /// <summary>
        /// Allow searching for text in archives created with this plugin
        /// </summary>
        Searchtext = 128,
        /// <summary>
        /// Don't show packer icon, don't open with Enter but with Ctrl+PgDn
        /// </summary>
        Hide = 256,
        /// <summary>
        /// Plugin supports encryption.
        /// </summary>
        Encrypt = 512
    }

    public enum OpenMode
    {
        List = 0,
        Extract = 1
    }
}
