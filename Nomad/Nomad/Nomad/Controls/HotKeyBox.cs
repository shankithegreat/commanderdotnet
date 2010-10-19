namespace Nomad.Controls
{
    using System;
    using System.ComponentModel;
    using System.Text;
    using System.Windows.Forms;

    public class HotKeyBox : TextBox
    {
        private static object EventHotKeyChanged = new object();
        private static object EventPreviewHotKey = new object();
        private Keys FHotKey;
        private TypeConverter FKeysConverter;
        private Keys FWorkKey;

        [EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public event EventHandler HideSelectionChanged
        {
            add
            {
                base.HideSelectionChanged += value;
            }
            remove
            {
                base.HideSelectionChanged -= value;
            }
        }

        public event EventHandler HotKeyChanged
        {
            add
            {
                base.Events.AddHandler(EventHotKeyChanged, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventHotKeyChanged, value);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public event EventHandler MultilineChanged
        {
            add
            {
                base.MultilineChanged += value;
            }
            remove
            {
                base.MultilineChanged -= value;
            }
        }

        public event EventHandler<PreviewHotKeyEventArgs> PreviewHotKey
        {
            add
            {
                base.Events.AddHandler(EventPreviewHotKey, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventPreviewHotKey, value);
            }
        }

        public HotKeyBox()
        {
            this.KeysConverter = new System.Windows.Forms.KeysConverter();
        }

        private void AppendModifiers(StringBuilder keyBuilder, Keys modifiers)
        {
            if ((modifiers & Keys.Control) > Keys.None)
            {
                keyBuilder.Append(this.FKeysConverter.ConvertToString(Keys.Control));
                keyBuilder.Append(" + ");
            }
            if ((modifiers & Keys.Shift) > Keys.None)
            {
                keyBuilder.Append(this.FKeysConverter.ConvertToString(Keys.Shift));
                keyBuilder.Append(" + ");
            }
            if ((modifiers & Keys.Alt) > Keys.None)
            {
                keyBuilder.Append(this.FKeysConverter.ConvertToString(Keys.Alt));
                keyBuilder.Append(" + ");
            }
        }

        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Tab:
                case (Keys.Shift | Keys.Tab):
                    return false;
            }
            return true;
        }

        public static bool IsSimpleKeyData(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Escape:
                case Keys.Space:
                case Keys.Prior:
                case Keys.Next:
                case Keys.End:
                case Keys.Home:
                case Keys.Left:
                case Keys.Up:
                case Keys.Right:
                case Keys.Down:
                case Keys.Insert:
                case Keys.Delete:
                case Keys.Decimal:
                case Keys.Back:
                case Keys.Return:
                    return true;
            }
            Keys keys = keyData & Keys.KeyCode;
            return ((((keyData & ~Keys.KeyCode) & ~Keys.Shift) == Keys.None) && ((((keys >= Keys.D0) && (keys <= Keys.D9)) || ((keys >= Keys.A) && (keys <= Keys.Z))) || ((keys >= Keys.NumPad0) && (keys <= Keys.NumPad9))));
        }

        public static bool IsValidKeyCode(Keys keyCode)
        {
            switch (keyCode)
            {
                case Keys.ShiftKey:
                case Keys.ControlKey:
                case Keys.Menu:
                case Keys.Pause:
                case Keys.Capital:
                case Keys.None:
                case Keys.NumLock:
                case Keys.Scroll:
                    return false;
            }
            return true;
        }

        private string KeyDataToString(ref Keys keyData)
        {
            StringBuilder keyBuilder = new StringBuilder();
            this.AppendModifiers(keyBuilder, keyData & ~Keys.KeyCode);
            Keys keyCode = keyData & Keys.KeyCode;
            if (IsValidKeyCode(keyCode))
            {
                keyBuilder.Append(this.FKeysConverter.ConvertToString(keyCode));
                return keyBuilder.ToString();
            }
            keyData = Keys.None;
            return this.FKeysConverter.ConvertToString(Keys.None);
        }

        protected virtual void OnHotKeyChanged(EventArgs e)
        {
            EventHandler handler = base.Events[EventHotKeyChanged] as EventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            StringBuilder keyBuilder = new StringBuilder();
            this.AppendModifiers(keyBuilder, e.Modifiers);
            if (IsValidKeyCode(e.KeyCode))
            {
                keyBuilder.Append(this.FKeysConverter.ConvertToString(e.KeyCode));
            }
            this.Text = keyBuilder.ToString();
            base.SelectionStart = this.Text.Length;
            this.FWorkKey = e.KeyData;
            e.SuppressKeyPress = true;
            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            this.UpdateHotKey(this.FHotKey, this.FWorkKey);
            base.OnKeyUp(e);
        }

        protected override void OnLeave(EventArgs e)
        {
            this.UpdateHotKey(this.FHotKey, this.FWorkKey);
            base.OnLeave(e);
        }

        protected virtual void OnPreviewHotKey(PreviewHotKeyEventArgs e)
        {
            EventHandler<PreviewHotKeyEventArgs> handler = base.Events[EventPreviewHotKey] as EventHandler<PreviewHotKeyEventArgs>;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            return false;
        }

        private void UpdateHotKey(Keys priorHotKey, Keys newHotKey)
        {
            if (!IsValidKeyCode(newHotKey & Keys.KeyCode))
            {
                newHotKey = Keys.None;
            }
            PreviewHotKeyEventArgs e = new PreviewHotKeyEventArgs(newHotKey) {
                Cancel = (newHotKey == Keys.None) ? true : IsSimpleKeyData(newHotKey)
            };
            this.OnPreviewHotKey(e);
            this.FHotKey = (IsValidKeyCode(e.HotKey & Keys.KeyCode) && !e.Cancel) ? e.HotKey : Keys.None;
            this.Text = this.KeyDataToString(ref this.FHotKey);
            base.SelectionStart = this.Text.Length;
            if (this.FHotKey != priorHotKey)
            {
                this.OnHotKeyChanged(EventArgs.Empty);
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public AutoCompleteStringCollection AutoCompleteCustomSource
        {
            get
            {
                return base.AutoCompleteCustomSource;
            }
            set
            {
                base.AutoCompleteCustomSource = value;
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.Windows.Forms.AutoCompleteMode AutoCompleteMode
        {
            get
            {
                return base.AutoCompleteMode;
            }
            set
            {
                base.AutoCompleteMode = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public System.Windows.Forms.AutoCompleteSource AutoCompleteSource
        {
            get
            {
                return base.AutoCompleteSource;
            }
            set
            {
                base.AutoCompleteSource = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool HideSelection
        {
            get
            {
                return base.HideSelection;
            }
            set
            {
                base.HideSelection = true;
            }
        }

        [DefaultValue(typeof(Keys), "None")]
        public Keys HotKey
        {
            get
            {
                return this.FHotKey;
            }
            set
            {
                this.UpdateHotKey(this.FHotKey, value);
                this.FWorkKey = this.FHotKey;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TypeConverter KeysConverter
        {
            get
            {
                return this.FKeysConverter;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }
                this.FKeysConverter = value;
                this.UpdateHotKey(this.FHotKey, this.FHotKey);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public string[] Lines
        {
            get
            {
                return base.Lines;
            }
            set
            {
                base.Lines = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override int MaxLength
        {
            get
            {
                return base.MaxLength;
            }
            set
            {
                base.MaxLength = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Multiline
        {
            get
            {
                return base.Multiline;
            }
            set
            {
                base.Multiline = false;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public char PasswordChar
        {
            get
            {
                return base.PasswordChar;
            }
            set
            {
                base.PasswordChar = value;
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.Windows.Forms.ScrollBars ScrollBars
        {
            get
            {
                return base.ScrollBars;
            }
            set
            {
                base.ScrollBars = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public override bool ShortcutsEnabled
        {
            get
            {
                return base.ShortcutsEnabled;
            }
            set
            {
                base.ShortcutsEnabled = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public bool UseSystemPasswordChar
        {
            get
            {
                return base.UseSystemPasswordChar;
            }
            set
            {
                base.UseSystemPasswordChar = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public bool WordWrap
        {
            get
            {
                return base.WordWrap;
            }
            set
            {
                base.WordWrap = value;
            }
        }
    }
}

