namespace Nomad.Commons.IO
{
    using System;

    public enum IniElementType
    {
        None,
        Section,
        KeyValuePair,
        StringLine,
        EmptyStringLine,
        Comment
    }
}

