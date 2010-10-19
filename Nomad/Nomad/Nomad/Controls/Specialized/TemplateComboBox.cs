namespace Nomad.Controls.Specialized
{
    using Nomad.Controls.Actions;
    using Nomad.Dialogs;
    using Nomad.Properties;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;

    internal class TemplateComboBox : ComboBox
    {
        private string CurrentTemplateName;
        private Button FDeleteButton;
        private bool FModified;
        private Button FSaveButton;
        private ToolStripItem ManageMenuItem;

        public TemplateComboBox()
        {
            base.DisplayMember = "Key";
            base.ValueMember = "Value";
            if (!base.DesignMode)
            {
                this.ContextMenuStrip = new ContextMenuStrip();
                this.ManageMenuItem = this.ContextMenuStrip.Items.Add(Resources.sManageTemplateList, null, new EventHandler(this.ManageMenuItem_Click));
                this.ContextMenuStrip.Items.Add(new ToolStripSeparator());
                this.ContextMenuStrip.Items.Add(StandardActions.Cut, this, BindActionProperty.CanUpdate | BindActionProperty.CanClick | BindActionProperty.Image | BindActionProperty.Checked | BindActionProperty.Visible | BindActionProperty.Text | BindActionProperty.Enabled);
                this.ContextMenuStrip.Items.Add(StandardActions.Copy, this, BindActionProperty.CanUpdate | BindActionProperty.CanClick | BindActionProperty.Image | BindActionProperty.Checked | BindActionProperty.Visible | BindActionProperty.Text | BindActionProperty.Enabled);
                this.ContextMenuStrip.Items.Add(StandardActions.Paste, this, BindActionProperty.CanUpdate | BindActionProperty.CanClick | BindActionProperty.Image | BindActionProperty.Checked | BindActionProperty.Visible | BindActionProperty.Text | BindActionProperty.Enabled);
                this.ContextMenuStrip.Items.Add(new ToolStripSeparator());
                this.ContextMenuStrip.Items.Add(StandardActions.SelectAll, this, BindActionProperty.CanUpdate | BindActionProperty.CanClick | BindActionProperty.Image | BindActionProperty.Checked | BindActionProperty.Visible | BindActionProperty.Text | BindActionProperty.Enabled);
                this.ContextMenuStrip.Opening += new CancelEventHandler(this.ContextMenuStrip_Opening);
            }
        }

        public void ClearModified()
        {
            this.CurrentTemplateName = null;
            this.UpdateButtons();
        }

        private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            this.ManageMenuItem.Enabled = base.Items.Count > 0;
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (this.SelectedIndex >= 0)
            {
                base.Items.RemoveAt(this.SelectedIndex);
                this.Text = string.Empty;
                this.FModified = true;
                this.UpdateButtons();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (this.ContextMenuStrip != null)
            {
                this.ContextMenuStrip.Dispose();
            }
            this.ContextMenuStrip = null;
            base.Dispose(disposing);
        }

        public IEnumerable<KeyValuePair<string, T>> GetItems<T>()
        {
            return new <GetItems>d__0<T>(-2) { <>4__this = this };
        }

        public T GetValue<T>(int index)
        {
            KeyValueItem item = base.Items[index] as KeyValueItem;
            return ((item != null) ? ((T) item.Value) : default(T));
        }

        private void ManageMenuItem_Click(object sender, EventArgs e)
        {
            object selectedItem = base.SelectedItem;
            using (ManageListDialog dialog = new ManageListDialog())
            {
                dialog.Items = base.Items.Cast<object>().ToArray<object>();
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                base.BeginUpdate();
                base.Items.Clear();
                base.Items.AddRange(dialog.Items);
            }
            if (selectedItem != null)
            {
                base.SelectedItem = selectedItem;
                if (base.SelectedItem != selectedItem)
                {
                    this.Text = string.Empty;
                }
                else if (this.Focused)
                {
                    base.SelectAll();
                }
            }
            base.EndUpdate();
            this.UpdateButtons();
        }

        protected override void OnFormat(ListControlConvertEventArgs e)
        {
            KeyValueItem listItem = e.ListItem as KeyValueItem;
            if ((listItem != null) && (e.DesiredType == typeof(string)))
            {
                e.Value = listItem.Key;
            }
            base.OnFormat(e);
        }

        protected override void OnSelectionChangeCommitted(EventArgs e)
        {
            if (base.SelectedItem != null)
            {
                KeyValueItem selectedItem = base.SelectedItem as KeyValueItem;
                this.CurrentTemplateName = selectedItem.Key;
            }
            else
            {
                this.CurrentTemplateName = null;
            }
            base.BeginInvoke(new MethodInvoker(this.UpdateButtons));
            base.OnSelectionChangeCommitted(e);
        }

        protected override void OnTextUpdate(EventArgs e)
        {
            this.UpdateButtons();
            base.OnTextUpdate(e);
        }

        public void Save<T>(T value)
        {
            string str = this.Text.Trim();
            if (!string.IsNullOrEmpty(str))
            {
                int num = -1;
                for (int i = 0; i < base.Items.Count; i++)
                {
                    if (str.Equals(((KeyValueItem) base.Items[i]).Key, StringComparison.CurrentCultureIgnoreCase))
                    {
                        num = i;
                        break;
                    }
                }
                KeyValueItem item = new KeyValueItem {
                    Key = str,
                    Value = value
                };
                if (num < 0)
                {
                    num = base.Items.Add(item);
                }
                else
                {
                    base.Items[num] = item;
                }
                this.FModified = true;
                this.SelectedIndex = num;
                this.CurrentTemplateName = str;
                if (this.Focused)
                {
                    base.SelectAll();
                }
                this.UpdateButtons();
            }
        }

        public void SetItems<T>(IEnumerable<KeyValuePair<string, T>> items)
        {
            base.BeginUpdate();
            base.Items.Clear();
            if (items != null)
            {
                foreach (KeyValuePair<string, T> pair in items)
                {
                    base.Items.Add(new KeyValueItem { Key = pair.Key, Value = pair.Value });
                }
            }
            base.EndUpdate();
            this.FModified = false;
        }

        public void SetItems<T>(IEnumerable<T> items, Func<T, string> getName)
        {
            base.BeginUpdate();
            base.Items.Clear();
            if (items != null)
            {
                foreach (T local in items)
                {
                    base.Items.Add(new KeyValueItem { Key = getName(local), Value = local });
                }
            }
            base.EndUpdate();
            this.FModified = false;
        }

        public void UpdateButtons()
        {
            string str = this.Text.Trim();
            if (this.FDeleteButton != null)
            {
                this.FDeleteButton.Enabled = ((this.SelectedIndex >= 0) && str.Equals(this.CurrentTemplateName)) && str.Equals(((KeyValueItem) base.SelectedItem).Key);
            }
            if (!(base.DroppedDown || (this.FSaveButton == null)))
            {
                this.FSaveButton.Enabled = !string.IsNullOrEmpty(str) && !str.Equals(this.CurrentTemplateName);
            }
        }

        [DefaultValue((string) null)]
        public Button DeleteButton
        {
            get
            {
                return this.FDeleteButton;
            }
            set
            {
                if (this.FDeleteButton != value)
                {
                    if (this.FDeleteButton != null)
                    {
                        this.FDeleteButton.Click -= new EventHandler(this.DeleteButton_Click);
                    }
                    this.FDeleteButton = value;
                    if (this.FDeleteButton != null)
                    {
                        this.FDeleteButton.Click += new EventHandler(this.DeleteButton_Click);
                    }
                }
            }
        }

        [DefaultValue("Key")]
        public string DisplayMember
        {
            get
            {
                return base.DisplayMember;
            }
            set
            {
                base.DisplayMember = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public bool Modified
        {
            get
            {
                return this.FModified;
            }
            set
            {
                this.FModified = value;
            }
        }

        [DefaultValue((string) null)]
        public Button SaveButton
        {
            get
            {
                return this.FSaveButton;
            }
            set
            {
                this.FSaveButton = value;
            }
        }

        [DefaultValue("Value")]
        public string ValueMember
        {
            get
            {
                return base.ValueMember;
            }
            set
            {
                base.ValueMember = value;
            }
        }

        [CompilerGenerated]
        private sealed class <GetItems>d__0<T> : IEnumerable<KeyValuePair<string, T>>, IEnumerable, IEnumerator<KeyValuePair<string, T>>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private KeyValuePair<string, T> <>2__current;
            public TemplateComboBox <>4__this;
            public IEnumerator <>7__wrap3;
            public IDisposable <>7__wrap4;
            private int <>l__initialThreadId;
            public object <NextItem>5__1;
            public TemplateComboBox.KeyValueItem <NextValueItem>5__2;

            [DebuggerHidden]
            public <GetItems>d__0(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally5()
            {
                this.<>1__state = -1;
                this.<>7__wrap4 = this.<>7__wrap3 as IDisposable;
                if (this.<>7__wrap4 != null)
                {
                    this.<>7__wrap4.Dispose();
                }
            }

            private bool MoveNext()
            {
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            this.<>1__state = -1;
                            this.<>7__wrap3 = this.<>4__this.Items.GetEnumerator();
                            this.<>1__state = 1;
                            while (this.<>7__wrap3.MoveNext())
                            {
                                this.<NextItem>5__1 = this.<>7__wrap3.Current;
                                this.<NextValueItem>5__2 = this.<NextItem>5__1 as TemplateComboBox.KeyValueItem;
                                if (this.<NextValueItem>5__2 == null)
                                {
                                    goto Label_00B7;
                                }
                                this.<>2__current = new KeyValuePair<string, T>(this.<NextValueItem>5__2.Key, (T) this.<NextValueItem>5__2.Value);
                                this.<>1__state = 2;
                                return true;
                            Label_00B0:
                                this.<>1__state = 1;
                            Label_00B7:;
                            }
                            this.<>m__Finally5();
                            break;

                        case 2:
                            goto Label_00B0;
                    }
                    return false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
            }

            [DebuggerHidden]
            IEnumerator<KeyValuePair<string, T>> IEnumerable<KeyValuePair<string, T>>.GetEnumerator()
            {
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    return (TemplateComboBox.<GetItems>d__0<T>) this;
                }
                return new TemplateComboBox.<GetItems>d__0<T>(0) { <>4__this = this.<>4__this };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.String,T>>.GetEnumerator();
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
                switch (this.<>1__state)
                {
                    case 1:
                    case 2:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finally5();
                        }
                        break;
                }
            }

            KeyValuePair<string, T> IEnumerator<KeyValuePair<string, T>>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }
        }

        private class KeyValueItem
        {
            public string Key;
            public object Value;

            public override string ToString()
            {
                return this.Key;
            }
        }
    }
}

