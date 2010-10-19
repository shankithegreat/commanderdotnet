namespace Nomad.Controls.Specialized
{
    using Microsoft.IO;
    using Nomad;
    using Nomad.Commons.Controls;
    using Nomad.Configuration;
    using Nomad.FileSystem.Virtual.Filter;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Forms;

    public static class AutoCompleteEvents
    {
        private static IEnumerable ConcatSources(IEnumerable source, IEnumerable collection)
        {
            if (source == null)
            {
                return collection;
            }
            return source.Cast<object>().Concat<object>(collection.Cast<object>());
        }

        public static void GetComboBoxSource(object sender, GetCustomSourceEventArgs e)
        {
            Func<object, bool> predicate = null;
            Func<object, bool> func2 = null;
            ComboBox target = e.Target as ComboBox;
            if (((target != null) && (target.Items.Count > 0)) && (e.Text.Length > 0))
            {
                IEnumerable<object> source = target.Items.Cast<object>();
                if (target.AutoCompleteMode == AutoCompleteMode.Suggest)
                {
                    if (predicate == null)
                    {
                        predicate = delegate (object x) {
                            return x.ToString().IndexOf(e.Text, StringComparison.OrdinalIgnoreCase) >= 0;
                        };
                    }
                    source = source.Where<object>(predicate);
                }
                else
                {
                    if (func2 == null)
                    {
                        func2 = delegate (object x) {
                            return x.ToString().StartsWith(e.Text, StringComparison.OrdinalIgnoreCase);
                        };
                    }
                    source = source.Where<object>(func2);
                }
                e.CustomSource = ConcatSources(e.CustomSource, source);
                e.Handled = true;
            }
        }

        public static void GetKnownFoldersSource(object sender, GetCustomSourceEventArgs e)
        {
            Func<string, bool> predicate = null;
            if (e.Text.StartsWith("shell:", StringComparison.OrdinalIgnoreCase))
            {
                List<string> tag = e.Target.Tag as List<string>;
                if (tag == null)
                {
                    tag = new List<string>();
                    foreach (string str in KnownFolder.GetNames())
                    {
                        tag.Add("shell:" + str);
                    }
                    tag.TrimExcess();
                    e.Target.Tag = tag;
                }
                if (predicate == null)
                {
                    predicate = delegate (string x) {
                        return x.StartsWith(e.Text, StringComparison.OrdinalIgnoreCase);
                    };
                }
                IEnumerable<string> collection = tag.Where<string>(predicate);
                e.CustomSource = ConcatSources(e.CustomSource, collection);
                e.Handled = true;
            }
        }

        public static void GetRecentFoldersSource(object sender, GetCustomSourceEventArgs e)
        {
            AutoCompleteMode autoCompleteMode;
            Func<string, bool> predicate = null;
            Func<string, bool> func2 = null;
            TextBox target = e.Target as TextBox;
            if (target != null)
            {
                autoCompleteMode = target.AutoCompleteMode;
            }
            else
            {
                ComboBox box2 = e.Target as ComboBox;
                if (box2 == null)
                {
                    return;
                }
                autoCompleteMode = box2.AutoCompleteMode;
            }
            if (e.Text.Length > 0)
            {
                IEnumerable<string> changeFolder = HistorySettings.Default.ChangeFolder;
                if (changeFolder != null)
                {
                    if (autoCompleteMode == AutoCompleteMode.Suggest)
                    {
                        if (predicate == null)
                        {
                            predicate = delegate (string x) {
                                return x.IndexOf(e.Text, StringComparison.OrdinalIgnoreCase) >= 0;
                            };
                        }
                        changeFolder = changeFolder.Where<string>(predicate);
                    }
                    else
                    {
                        if (func2 == null)
                        {
                            func2 = delegate (string x) {
                                return x.StartsWith(e.Text, StringComparison.OrdinalIgnoreCase);
                            };
                        }
                        changeFolder = changeFolder.Where<string>(func2);
                    }
                    e.CustomSource = ConcatSources(e.CustomSource, changeFolder);
                    e.Handled = true;
                }
            }
        }

        public static void PreviewEnvironmentVariable(object sender, PreviewEnvironmentVariableEventArgs e)
        {
            switch (e.Value)
            {
                case "curdir":
                case "curitempath":
                case "curitemname":
                case "curselname":
                case "curselpath":
                case "fardir":
                case "faritempath":
                case "faritemname":
                case "farselname":
                case "farselpath":
                case "user":
                    e.Cancel = true;
                    break;
            }
        }

        public static void PreviewFileSystemInfo(object sender, PreviewFileSystemInfoEventArgs e)
        {
            IFileSystemInfoFilter hiddenItemsFilter = VirtualFilePanelSettings.Default.HiddenItemsFilter as IFileSystemInfoFilter;
            e.Cancel = !(e.Value is DirectoryInfo) || ((hiddenItemsFilter != null) && hiddenItemsFilter.IsMatch(e.Value));
        }
    }
}

