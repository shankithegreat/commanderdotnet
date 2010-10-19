namespace Nomad.Commons
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;

    [DebuggerDisplay("DictionaryRename, {RenameDictionary.Count} items")]
    public class DictionaryRenameFilter : IRenameFilter
    {
        private Dictionary<string, string> RenameDictionary;

        public DictionaryRenameFilter(IEnumerable<KeyValuePair<string, string>> values)
        {
            ICollection is2 = values as ICollection;
            if (is2 != null)
            {
                this.RenameDictionary = new Dictionary<string, string>(is2.Count);
            }
            else
            {
                this.RenameDictionary = new Dictionary<string, string>();
            }
            foreach (KeyValuePair<string, string> pair in values)
            {
                this.RenameDictionary.Add(pair.Key, pair.Value);
            }
        }

        public DictionaryRenameFilter(string sourceName, string destName)
        {
            this.RenameDictionary = new Dictionary<string, string>(1);
            this.RenameDictionary.Add(sourceName, destName);
        }

        public string CreateNewName(string sourceName)
        {
            string str;
            if (this.RenameDictionary.TryGetValue(sourceName, out str))
            {
                return str;
            }
            return sourceName;
        }
    }
}

