namespace Nomad.Commons
{
    using System;
    using System.Diagnostics;

    [DebuggerDisplay("SimpleRename, {FDestName}")]
    public class SimpleRenameFilter : IRenameFilter
    {
        private string FDestName;

        public SimpleRenameFilter(string destName)
        {
            this.FDestName = destName;
        }

        public string CreateNewName(string sourceName)
        {
            return this.FDestName;
        }
    }
}

