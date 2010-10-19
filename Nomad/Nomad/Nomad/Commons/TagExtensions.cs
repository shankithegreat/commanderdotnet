namespace Nomad.Commons
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public static class TagExtensions
    {
        public static object GetTag(this ToolStripItem item, int key)
        {
            if (item.Tag != null)
            {
                TagContainter tag = item.Tag as TagContainter;
                if (tag != null)
                {
                    return ((tag.Key == key) ? tag.Value : null);
                }
                List<TagContainter> list = item.Tag as List<TagContainter>;
                if (list != null)
                {
                    foreach (TagContainter containter2 in list)
                    {
                        if (containter2.Key == key)
                        {
                            return containter2.Value;
                        }
                    }
                }
            }
            return null;
        }

        public static void SetTag(this ToolStripItem item, int key, object value)
        {
            List<TagContainter> tag = item.Tag as List<TagContainter>;
            if (tag != null)
            {
                foreach (TagContainter containter in tag)
                {
                    if (containter.Key == key)
                    {
                        containter.Value = value;
                        return;
                    }
                }
                tag.Add(new TagContainter(key, value));
            }
            else
            {
                TagContainter containter2 = item.Tag as TagContainter;
                if (containter2 != null)
                {
                    if (containter2.Key == key)
                    {
                        containter2.Value = value;
                    }
                    else
                    {
                        tag = new List<TagContainter>(2) {
                            containter2,
                            new TagContainter(key, value)
                        };
                        item.Tag = tag;
                    }
                }
                else
                {
                    Debug.Assert(item.Tag == null);
                    item.Tag = new TagContainter(key, value);
                }
            }
        }

        private class TagContainter
        {
            public int Key;
            public object Value;

            public TagContainter(int key, object value)
            {
                this.Key = key;
                this.Value = value;
            }
        }
    }
}

