namespace Nomad.FileSystem.Property.Providers.Wdx
{
    using System;
    using System.Diagnostics;

    [DebuggerDisplay("WdxField: {FieldName}, {FieldType}")]
    public class WdxFieldInfo
    {
        public static readonly object DelayedValue = new object();
        public string FieldName;
        public int FieldType;
        public ContentFlag Flags;
        public static readonly object OnDemandValue = new object();
        public string[] Units;
    }
}

