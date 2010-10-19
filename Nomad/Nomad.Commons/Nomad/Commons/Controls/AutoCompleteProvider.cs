namespace Nomad.Commons.Controls
{
    using Microsoft;
    using Microsoft.IO;
    using Microsoft.Win32;
    using Nomad.Commons.Collections;
    using Nomad.Commons.IO;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    [ProvideProperty("AutoComplete", typeof(Control)), DesignerCategory("Code")]
    public class AutoCompleteProvider : Component, IExtenderProvider
    {
        private HashSet<Control> AutoCompleteControlSet;
        private string CurrentDir;
        private ExpandMode CurrentExpand;
        private Form CurrentForm;
        private string CurrentText;
        private static readonly object EventBeforeSourcesLookup = new object();
        private static readonly object EventFormat = new object();
        private static readonly object EventGetCurrentDirectory = new object();
        private static readonly object EventGetCustomSource = new object();
        private static readonly object EventPreviewEnvironmentVariable = new object();
        private static readonly object EventPreviewFileSystemInfo = new object();
        private PopupForm ExpandForm;
        private string LastEnvironemntVariablesSource;
        private List<string> LastEnvironmentVariablesCollection;
        private List<string> LastFileSystemCollection;
        private string LastFileSystemSource;

        public event EventHandler<CancelEventArgs> BeforeSourcesLookup
        {
            add
            {
                base.Events.AddHandler(EventBeforeSourcesLookup, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventBeforeSourcesLookup, value);
            }
        }

        public event ListControlConvertEventHandler Format
        {
            add
            {
                base.Events.AddHandler(EventFormat, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventFormat, value);
            }
        }

        public event EventHandler<GetCurrentDirectoryEventArgs> GetCurrentDirectory
        {
            add
            {
                base.Events.AddHandler(EventGetCurrentDirectory, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventGetCurrentDirectory, value);
            }
        }

        public event EventHandler<GetCustomSourceEventArgs> GetCustomSource
        {
            add
            {
                base.Events.AddHandler(EventGetCustomSource, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventGetCustomSource, value);
            }
        }

        public event EventHandler<PreviewEnvironmentVariableEventArgs> PreviewEnvironmentVariable
        {
            add
            {
                base.Events.AddHandler(EventPreviewEnvironmentVariable, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventPreviewEnvironmentVariable, value);
            }
        }

        public event EventHandler<PreviewFileSystemInfoEventArgs> PreviewFileSystemInfo
        {
            add
            {
                base.Events.AddHandler(EventPreviewFileSystemInfo, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventPreviewFileSystemInfo, value);
            }
        }

        private void AddControl(Control control)
        {
            if (this.AutoCompleteControlSet == null)
            {
                this.AutoCompleteControlSet = new HashSet<Control>();
            }
            control.Disposed += new EventHandler(this.Control_Disposed);
            control.KeyDown += new KeyEventHandler(this.Control_KeyDown);
            control.KeyPress += new KeyPressEventHandler(this.Control_KeyPress);
            control.Leave += new EventHandler(this.Control_Leave);
            control.TextChanged += new EventHandler(this.Control_TextChanged);
            this.AutoCompleteControlSet.Add(control);
        }

        public bool CanExtend(object extendee)
        {
            return ((extendee is TextBox) || (extendee is ComboBox));
        }

        private void Control_Disposed(object sender, EventArgs e)
        {
            if ((this.ExpandForm != null) && (this.ExpandForm.Tag == sender))
            {
                this.ExpandForm.Hide();
                this.ExpandForm.Tag = null;
            }
            this.RemoveControl((Control) sender);
        }

        private void Control_KeyDown(object sender, KeyEventArgs e)
        {
            Control control;
            ListBox listBox;
            this.CurrentExpand = ExpandMode.None;
            if ((this.ExpandForm != null) && this.ExpandForm.Visible)
            {
                control = (Control) sender;
                switch (e.KeyData)
                {
                    case Keys.Up:
                    case Keys.Down:
                        listBox = this.ExpandForm.ListBox;
                        switch (e.KeyData)
                        {
                            case Keys.Up:
                                if (listBox.SelectedIndex >= 0)
                                {
                                    listBox.SelectedIndex--;
                                }
                                else
                                {
                                    listBox.SelectedIndex = listBox.Items.Count - 1;
                                }
                                goto Label_00F9;

                            case Keys.Right:
                                goto Label_00F9;

                            case Keys.Down:
                                if (listBox.SelectedIndex < (listBox.Items.Count - 1))
                                {
                                    listBox.SelectedIndex++;
                                }
                                else
                                {
                                    listBox.SelectedIndex = -1;
                                }
                                goto Label_00F9;
                        }
                        goto Label_00F9;

                    case Keys.Right:
                        return;

                    case Keys.Delete:
                        if (GetSelectionLength(control) > 0)
                        {
                            switch (GetAutoCompleteMode(control))
                            {
                                case AutoCompleteMode.Suggest:
                                case AutoCompleteMode.SuggestAppend:
                                    this.CurrentExpand = ExpandMode.Suggest;
                                    control.BeginInvoke(new EventHandler(this.Control_TextChanged), new object[] { sender, e });
                                    return;

                                case AutoCompleteMode.Append:
                                    return;
                            }
                        }
                        return;
                }
            }
            return;
        Label_00F9:
            if (listBox.SelectedIndex >= 0)
            {
                object listItem = listBox.Items[listBox.SelectedIndex];
                ListControlConvertEventArgs args = new ListControlConvertEventArgs(listItem.ToString(), typeof(string), listItem);
                this.OnFormat(args);
                control.Text = args.Value.ToString();
            }
            else
            {
                control.Text = this.CurrentText;
            }
            SetSelectionRange(control, control.Text.Length, 0);
            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        private void Control_KeyPress(object sender, KeyPressEventArgs e)
        {
            Control control = (Control) sender;
            switch (GetAutoCompleteMode(control))
            {
                case AutoCompleteMode.Suggest:
                    this.CurrentExpand = ExpandMode.Suggest;
                    break;

                case AutoCompleteMode.Append:
                    this.CurrentExpand = (e.KeyChar >= ' ') ? ExpandMode.Append : ExpandMode.None;
                    break;

                case AutoCompleteMode.SuggestAppend:
                    this.CurrentExpand = ExpandMode.Suggest | ((e.KeyChar >= ' ') ? ExpandMode.Append : ExpandMode.None);
                    break;

                default:
                    this.CurrentExpand = ExpandMode.None;
                    break;
            }
            if ((GetSelectionLength(control) > 0) && (e.KeyChar == '\b'))
            {
                control.BeginInvoke(new EventHandler(this.Control_TextChanged), new object[] { sender, e });
            }
        }

        private void Control_Leave(object sender, EventArgs e)
        {
            this.CurrentExpand = ExpandMode.None;
            if (this.ExpandForm != null)
            {
                this.ExpandForm.Hide();
            }
        }

        private void Control_TextChanged(object sender, EventArgs e)
        {
            ExpandMode currentExpand = this.CurrentExpand;
            this.CurrentExpand = ExpandMode.None;
            if (currentExpand != ExpandMode.None)
            {
                Control control = (Control) sender;
                bool flag = true;
                IEnumerable<object> source = this.ExpandSource(control);
                if (!(source is object[]))
                {
                    object obj3;
                    if ((currentExpand & ExpandMode.Suggest) > ExpandMode.None)
                    {
                        if (this.ExpandForm == null)
                        {
                            this.ExpandForm = new PopupForm();
                            this.ExpandForm.ListBox.FormattingEnabled = true;
                            this.ExpandForm.ListBox.Sorted = true;
                            this.ExpandForm.Click += new EventHandler(this.PopupForm_Click);
                            this.ExpandForm.KeyDown += new KeyEventHandler(this.PopupForm_KeyDown);
                            this.ExpandForm.VisibleChanged += new EventHandler(this.PopupForm_VisibleChanged);
                            this.ExpandForm.ListBox.MouseMove += new MouseEventHandler(this.ListBox_MouseMove);
                            this.ExpandForm.ListBox.Format += new ListControlConvertEventHandler(this.ListBox_Format);
                        }
                        this.ExpandForm.ListBox.BeginUpdate();
                        this.ExpandForm.ListBox.Items.Clear();
                        foreach (object obj2 in source)
                        {
                            this.ExpandForm.ListBox.Items.Add(obj2);
                        }
                        this.ExpandForm.ListBox.EndUpdate();
                        if (this.ExpandForm.ListBox.Items.Count > 0)
                        {
                            this.CurrentText = control.Text;
                            this.ExpandForm.Bounds = GetPopupBounds(control, (Math.Min(15, this.ExpandForm.ListBox.Items.Count) * this.ExpandForm.ListBox.ItemHeight) + 2);
                            this.ExpandForm.Tag = control;
                            this.ExpandForm.Show();
                            flag = false;
                        }
                    }
                    if (((currentExpand & ExpandMode.Append) > ExpandMode.None) && ((obj3 = source.FirstOrDefault<object>()) != null))
                    {
                        ListControlConvertEventArgs args = new ListControlConvertEventArgs(obj3.ToString(), typeof(string), obj3);
                        this.OnFormat(args);
                        int length = control.Text.Length;
                        control.Text = control.Text + args.Value.ToString().Substring(length);
                        SetSelectionRange(control, length, control.Text.Length - length);
                    }
                }
                if (flag && (this.ExpandForm != null))
                {
                    this.ExpandForm.Hide();
                }
            }
        }

        private void ControlForm_Deactivate(object sender, EventArgs e)
        {
            ((Form) sender).Deactivate -= new EventHandler(this.ControlForm_Deactivate);
            if (this.ExpandForm != null)
            {
                this.ExpandForm.Hide();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (this.AutoCompleteControlSet != null)
            {
                foreach (Control control in this.AutoCompleteControlSet)
                {
                    this.RemoveControlBinding(control);
                }
                this.AutoCompleteControlSet = null;
            }
            if (this.ExpandForm != null)
            {
                this.ExpandForm.Dispose();
                this.ExpandForm = null;
            }
            base.Dispose(disposing);
        }

        private IEnumerable<string> ExpandEnvironmentVariables(Control control, string text)
        {
            Func<string, bool> predicate = null;
            int num = 0;
            int length = -1;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '%')
                {
                    length = i;
                    num++;
                }
            }
            if ((num % 2) == 1)
            {
                string b = text.Substring(0, length);
                if (!string.Equals(this.LastEnvironemntVariablesSource, b, StringComparison.OrdinalIgnoreCase))
                {
                    this.LastEnvironemntVariablesSource = b;
                    try
                    {
                        PreviewEnvironmentVariableEventArgs args = null;
                        this.LastEnvironmentVariablesCollection = new List<string>();
                        foreach (string str2 in Environment.GetEnvironmentVariables().Keys)
                        {
                            PreviewEnvironmentVariableEventArgs e = args ?? (args = new PreviewEnvironmentVariableEventArgs(control));
                            e._Value = str2;
                            e.Cancel = false;
                            this.OnPreviewEnvironmentVariable(e);
                            if (!e.Cancel)
                            {
                                this.LastEnvironmentVariablesCollection.Add(string.Concat(new object[] { b, '%', str2, '%' }));
                            }
                        }
                        this.LastEnvironmentVariablesCollection.TrimExcess();
                    }
                    catch
                    {
                        this.LastEnvironmentVariablesCollection = null;
                    }
                }
                if (this.LastEnvironmentVariablesCollection != null)
                {
                    if (predicate == null)
                    {
                        predicate = delegate (string x) {
                            return x.StartsWith(text, StringComparison.OrdinalIgnoreCase);
                        };
                    }
                    return this.LastEnvironmentVariablesCollection.Where<string>(predicate);
                }
            }
            return new string[0];
        }

        private IEnumerable<string> ExpandFileSystem(Control control, string text)
        {
            int num = text.LastIndexOfAny(new char[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar });
            string b = (num >= 0) ? text.Substring(0, num + 1) : string.Empty;
            if (!string.Equals(this.LastFileSystemSource, b, StringComparison.OrdinalIgnoreCase))
            {
                this.LastFileSystemSource = b;
                try
                {
                    this.LastFileSystemCollection = null;
                    string str2 = this.UseEnvironmentVariablesSource ? Environment.ExpandEnvironmentVariables(b) : b;
                    if (!string.IsNullOrEmpty(str2))
                    {
                        GetCurrentDirectoryEventArgs e = new GetCurrentDirectoryEventArgs(control, this.CurrentDir);
                        this.OnGetCurrentDirectory(e);
                        if (!string.IsNullOrEmpty(e.CurrentDirectory))
                        {
                            System.IO.Directory.SetCurrentDirectory(e.CurrentDirectory);
                            this.LastFileSystemCollection = new List<string>();
                        }
                        else if (Path.IsPathRooted(str2))
                        {
                            this.LastFileSystemCollection = new List<string>();
                        }
                    }
                    if (this.LastFileSystemCollection != null)
                    {
                        PreviewFileSystemInfoEventArgs args2 = null;
                        foreach (FileSystemInfo info in Microsoft.IO.Directory.GetFileSystemInfos(str2))
                        {
                            PreviewFileSystemInfoEventArgs args3 = args2 ?? (args2 = new PreviewFileSystemInfoEventArgs(control));
                            args3._Value = info;
                            args3.Cancel = false;
                            this.OnPreviewFileSystemInfo(args3);
                            if (!args3.Cancel)
                            {
                                this.LastFileSystemCollection.Add(b + info.Name);
                            }
                        }
                        this.LastFileSystemCollection.TrimExcess();
                    }
                }
                catch
                {
                    this.LastFileSystemCollection = null;
                }
            }
            if (((text.Length > 0) && (this.LastFileSystemCollection != null)) && (this.LastFileSystemCollection.Count > 0))
            {
                return this.LastFileSystemCollection.Where<string>(delegate (string x) {
                    return x.StartsWith(text, StringComparison.OrdinalIgnoreCase);
                });
            }
            return new string[0];
        }

        protected virtual IEnumerable<object> ExpandSource(Control control)
        {
            IEnumerable<object> first = new object[0];
            CancelEventArgs e = new CancelEventArgs();
            this.OnBeforeSourcesLookup(e);
            if (!e.Cancel)
            {
                if (this.UseFileSystemSource)
                {
                    first = first.Concat<object>(this.ExpandFileSystem(control, control.Text).Cast<object>());
                }
                if (this.UseEnvironmentVariablesSource)
                {
                    first = first.Concat<object>(this.ExpandEnvironmentVariables(control, control.Text).Cast<object>());
                }
                if (!this.UseCustomSource)
                {
                    return first;
                }
                GetCustomSourceEventArgs args2 = new GetCustomSourceEventArgs(control, control.Text);
                this.OnGetCustomSource(args2);
                if (args2.Handled && (args2.CustomSource != null))
                {
                    first = first.Concat<object>(args2.CustomSource.Cast<object>());
                }
            }
            return first;
        }

        [DefaultValue(false)]
        public bool GetAutoComplete(Control control)
        {
            return ((this.AutoCompleteControlSet != null) && this.AutoCompleteControlSet.Contains(control));
        }

        private static AutoCompleteMode GetAutoCompleteMode(Control control)
        {
            TextBox box = control as TextBox;
            if (box != null)
            {
                return box.AutoCompleteMode;
            }
            ComboBox box2 = control as ComboBox;
            if (box2 != null)
            {
                return box2.AutoCompleteMode;
            }
            return AutoCompleteMode.None;
        }

        private static Rectangle GetPopupBounds(Control control, int height)
        {
            Rectangle rectangle = control.RectangleToScreen(control.ClientRectangle);
            Padding padding = new Padding(3);
            TextBox box = control as TextBox;
            if (box != null)
            {
                padding = new Padding((box.BorderStyle == BorderStyle.None) ? 1 : 3);
            }
            ComboBox box2 = control as ComboBox;
            if (box2 != null)
            {
                padding = new Padding(3, 3, 3 + SystemInformation.VerticalScrollBarWidth, 3);
            }
            return new Rectangle(rectangle.Left + padding.Left, rectangle.Bottom - padding.Top, rectangle.Width - padding.Horizontal, height);
        }

        private static int GetSelectionLength(Control control)
        {
            TextBox box = control as TextBox;
            if (box != null)
            {
                return box.SelectionLength;
            }
            ComboBox box2 = control as ComboBox;
            if (box2 != null)
            {
                return box2.SelectionLength;
            }
            return 0;
        }

        private void ListBox_Format(object sender, ListControlConvertEventArgs e)
        {
            this.OnFormat(e);
        }

        private void ListBox_MouseMove(object sender, MouseEventArgs e)
        {
            int num;
            ListBox box = (ListBox) sender;
            box.SelectedIndex = ((num = box.IndexFromPoint(e.Location)) >= 0) ? num : -1;
        }

        protected virtual void OnBeforeSourcesLookup(CancelEventArgs e)
        {
            EventHandler<CancelEventArgs> handler = base.Events[EventBeforeSourcesLookup] as EventHandler<CancelEventArgs>;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnFormat(ListControlConvertEventArgs e)
        {
            if ((e.ListItem is FileSystemInfo) && (e.DesiredType == typeof(string)))
            {
                e.Value = PathHelper.ExcludeTrailingDirectorySeparator(e.ListItem.ToString());
            }
            ListControlConvertEventHandler handler = base.Events[EventFormat] as ListControlConvertEventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnGetCurrentDirectory(GetCurrentDirectoryEventArgs e)
        {
            EventHandler<GetCurrentDirectoryEventArgs> handler = base.Events[EventGetCurrentDirectory] as EventHandler<GetCurrentDirectoryEventArgs>;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnGetCustomSource(GetCustomSourceEventArgs e)
        {
            EventHandler<GetCustomSourceEventArgs> handler = base.Events[EventGetCustomSource] as EventHandler<GetCustomSourceEventArgs>;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnPreviewEnvironmentVariable(PreviewEnvironmentVariableEventArgs e)
        {
            EventHandler<PreviewEnvironmentVariableEventArgs> handler = base.Events[EventPreviewEnvironmentVariable] as EventHandler<PreviewEnvironmentVariableEventArgs>;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnPreviewFileSystemInfo(PreviewFileSystemInfoEventArgs e)
        {
            EventHandler<PreviewFileSystemInfoEventArgs> handler = base.Events[EventPreviewFileSystemInfo] as EventHandler<PreviewFileSystemInfoEventArgs>;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void PopupForm_Click(object sender, EventArgs e)
        {
            ListBox listBox = this.ExpandForm.ListBox;
            if (listBox.SelectedIndex >= 0)
            {
                object listItem = listBox.Items[listBox.SelectedIndex];
                ListControlConvertEventArgs args = new ListControlConvertEventArgs(listItem.ToString(), typeof(string), listItem);
                this.OnFormat(args);
                Control tag = this.ExpandForm.Tag as Control;
                if (tag != null)
                {
                    tag.Text = args.Value.ToString();
                    SetSelectionRange(tag, 0, tag.Text.Length);
                }
                this.ExpandForm.Hide();
            }
        }

        private void PopupForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Return:
                case Keys.Escape:
                    if (e.KeyData == Keys.Return)
                    {
                        Control tag = this.ExpandForm.Tag as Control;
                        if (tag != null)
                        {
                            SetSelectionRange(tag, 0, tag.Text.Length);
                        }
                    }
                    this.ExpandForm.Hide();
                    break;
            }
        }

        private void PopupForm_VisibleChanged(object sender, EventArgs e)
        {
            if (this.CurrentForm != null)
            {
                this.CurrentForm.Deactivate -= new EventHandler(this.ControlForm_Deactivate);
            }
            this.CurrentForm = null;
            Control control = (Control) sender;
            if (control.Visible)
            {
                Control tag = control.Tag as Control;
                if (tag != null)
                {
                    this.CurrentForm = tag.FindForm();
                    if (this.CurrentForm != null)
                    {
                        this.CurrentForm.Deactivate += new EventHandler(this.ControlForm_Deactivate);
                    }
                }
            }
            else
            {
                control.Tag = null;
            }
        }

        private void RemoveControl(Control control)
        {
            this.RemoveControlBinding(control);
            this.AutoCompleteControlSet.Remove(control);
        }

        private void RemoveControlBinding(Control control)
        {
            control.Disposed -= new EventHandler(this.Control_Disposed);
            control.KeyDown -= new KeyEventHandler(this.Control_KeyDown);
            control.KeyPress -= new KeyPressEventHandler(this.Control_KeyPress);
            control.Leave -= new EventHandler(this.Control_Leave);
            control.TextChanged -= new EventHandler(this.Control_TextChanged);
        }

        public void SetAutoComplete(Control control, bool autoComplete)
        {
            if (!(!autoComplete || this.GetAutoComplete(control)))
            {
                this.AddControl(control);
            }
            else
            {
                this.RemoveControl(control);
            }
        }

        private static void SetSelectionRange(Control control, int selectionStart, int selectionLength)
        {
            TextBox box = control as TextBox;
            if (box != null)
            {
                box.SelectionStart = selectionStart;
                box.SelectionLength = selectionLength;
            }
            ComboBox box2 = control as ComboBox;
            if (box2 != null)
            {
                box2.SelectionStart = selectionStart;
                box2.SelectionLength = selectionLength;
            }
        }

        [DefaultValue("")]
        public string CurrentDirectory
        {
            get
            {
                return (this.CurrentDir ?? string.Empty);
            }
            set
            {
                this.CurrentDir = value;
            }
        }

        [DefaultValue(false)]
        public bool UseCustomSource { get; set; }

        [DefaultValue(false)]
        public bool UseEnvironmentVariablesSource { get; set; }

        [DefaultValue(false)]
        public bool UseFileSystemSource { get; set; }

        [Flags]
        private enum ExpandMode
        {
            None,
            Suggest,
            Append
        }

        private class PopupForm : Form, IMessageFilter
        {
            public readonly System.Windows.Forms.ListBox ListBox;

            public PopupForm()
            {
                base.SetStyle(ControlStyles.Selectable, false);
                base.FormBorderStyle = FormBorderStyle.None;
                base.StartPosition = FormStartPosition.Manual;
                base.ShowInTaskbar = false;
                base.Visible = false;
                this.ListBox = new System.Windows.Forms.ListBox();
                this.ListBox.BorderStyle = BorderStyle.None;
                this.ListBox.Dock = DockStyle.Fill;
                base.Controls.Add(this.ListBox);
                this.BackColor = this.ListBox.BackColor;
            }

            protected override void Dispose(bool disposing)
            {
                this.ListBox.Dispose();
                base.Dispose(disposing);
            }

            protected override void OnVisibleChanged(EventArgs e)
            {
                base.OnVisibleChanged(e);
                if (base.Visible)
                {
                    Application.AddMessageFilter(this);
                }
                else
                {
                    Application.RemoveMessageFilter(this);
                }
            }

            public void Show()
            {
                Windows.SetWindowPos(base.Handle, Windows.HWND_TOPMOST, 0, 0, 0, 0, SWP.SWP_NOMOVE | SWP.SWP_NOSIZE | SWP.SWP_NOACTIVATE);
                base.Show();
            }

            bool IMessageFilter.PreFilterMessage(ref Message m)
            {
                if (m.Msg == 0x100)
                {
                    switch (((Keys) ((int) m.WParam)))
                    {
                        case Keys.Return:
                        case Keys.Escape:
                            this.OnKeyDown(new KeyEventArgs((Keys) ((int) m.WParam)));
                            return true;
                    }
                }
                return false;
            }

            protected override void WndProc(ref Message m)
            {
                if (m.Msg == 0x21)
                {
                    this.OnClick(EventArgs.Empty);
                    m.Result = (IntPtr) 4L;
                }
                else
                {
                    base.WndProc(ref m);
                }
            }

            protected override System.Windows.Forms.CreateParams CreateParams
            {
                get
                {
                    System.Windows.Forms.CreateParams createParams = base.CreateParams;
                    createParams.Style |= -2139095040;
                    createParams.ExStyle |= 0x8000008;
                    if (OS.IsWinXP && SystemInformation.IsDropShadowEnabled)
                    {
                        createParams.ClassStyle |= 0x20000;
                    }
                    return createParams;
                }
            }

            protected override bool ShowWithoutActivation
            {
                get
                {
                    return true;
                }
            }
        }
    }
}

