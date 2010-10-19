namespace Nomad
{
    using System;

    internal enum InitTask
    {
        SetupAppearance,
        SetupEditor,
        SetupViewer,
        SetupExternalTools,
        MakeArchivesHighligter,
        ExcludeFromWer,
        RegisterJumpListTasks,
        CreateDesktopShortcut,
        CompressFiles,
        NGen
    }
}

