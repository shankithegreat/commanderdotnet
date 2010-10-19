namespace Nomad.Controls.Actions
{
    using Nomad.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    [DesignTimeVisible(false), ToolboxItem(false), DefaultProperty("Text"), DesignerCategory("Code")]
    public abstract class CustomAction : NamedComponent, IAction, INotifyPropertyChanged
    {
        private static object EventImageChanged = new object();
        private static object EventOwnerChanged = new object();
        private static object EventPropertyChanged = new object();
        private static object EventShortcutsChanged = new object();
        private static object EventTextChanged = new object();
        private System.Drawing.Image FImage;
        private int FImageIndex = -1;
        private string FImageKey = string.Empty;
        private ActionManager FOwner;
        private Keys[] FShortcuts;
        private object FTag;
        private string FText = string.Empty;

        [Category("Property Changed")]
        public event EventHandler ImageChanged
        {
            add
            {
                base.Events.AddHandler(EventImageChanged, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventImageChanged, value);
            }
        }

        [Category("Property Changed")]
        public event EventHandler OwnerChanged
        {
            add
            {
                base.Events.AddHandler(EventOwnerChanged, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventOwnerChanged, value);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                base.Events.AddHandler(EventPropertyChanged, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventPropertyChanged, value);
            }
        }

        [Category("Property Changed")]
        public event EventHandler ShortcutsChanged
        {
            add
            {
                base.Events.AddHandler(EventShortcutsChanged, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventShortcutsChanged, value);
            }
        }

        [Category("Property Changed")]
        public event EventHandler TextChanged
        {
            add
            {
                base.Events.AddHandler(EventTextChanged, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventTextChanged, value);
            }
        }

        protected CustomAction()
        {
        }

        protected virtual void DoExecute(ActionEventArgs e)
        {
            if (this.Owner != null)
            {
                this.Owner.RaisePreviewExecuteAction(e);
            }
        }

        protected virtual void DoUpdate(UpdateActionEventArgs e)
        {
            if (this.Owner != null)
            {
                this.Owner.RaisePreviewUpdateAction(e);
            }
        }

        public bool Execute()
        {
            return this.Execute(null, null);
        }

        public bool Execute(object target)
        {
            return this.Execute(null, target);
        }

        public virtual bool Execute(object source, object target)
        {
            if ((this.Update(source, target) & ActionState.Enabled) > ActionState.None)
            {
                ActionEventArgs e = new ActionEventArgs(this, source, target);
                this.DoExecute(e);
                return e.Handled;
            }
            return false;
        }

        public bool IsShortcutDefined(Keys shortcut)
        {
            return (((this.FShortcuts != null) && (this.FShortcuts.Length > 0)) && (Array.IndexOf<Keys>(this.FShortcuts, shortcut) >= 0));
        }

        protected virtual void OnImageChanged(EventArgs e)
        {
            EventHandler handler = base.Events[EventImageChanged] as EventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
            this.OnPropertyChanged(new PropertyChangedEventArgs("Image"));
        }

        protected virtual void OnOwnerChanged(EventArgs e)
        {
            EventHandler handler = base.Events[EventOwnerChanged] as EventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
            this.OnPropertyChanged(new PropertyChangedEventArgs("Owner"));
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            EventHandler handler = base.Events[EventPropertyChanged] as EventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnShortcutsChanged()
        {
            EventHandler handler = base.Events[EventShortcutsChanged] as EventHandler;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(new PropertyChangedEventArgs("Shortcuts"));
        }

        protected virtual void OnTextChanged(EventArgs e)
        {
            EventHandler handler = base.Events[EventTextChanged] as EventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
            this.OnPropertyChanged(new PropertyChangedEventArgs("Text"));
        }

        public void ResetShortcutKeys()
        {
            this.FShortcuts = null;
        }

        public void ResetShortcuts()
        {
            this.FShortcuts = null;
        }

        private bool ShouldSerializeShortcutKeys()
        {
            return ((this.FShortcuts != null) && (this.FShortcuts.Length == 1));
        }

        private bool ShouldSerializeShortcuts()
        {
            return ((this.FShortcuts != null) && (this.FShortcuts.Length > 1));
        }

        public ActionState Update()
        {
            return this.Update(null, null);
        }

        public ActionState Update(object target)
        {
            return this.Update(null, target);
        }

        public virtual ActionState Update(object source, object target)
        {
            if (!base.DesignMode)
            {
                UpdateActionEventArgs e = new UpdateActionEventArgs(this, source, target);
                this.DoUpdate(e);
                return e.State;
            }
            return (ActionState.Visible | ActionState.Enabled);
        }

        [DefaultValue((string) null), Category("Appearance")]
        public System.Drawing.Image Image
        {
            get
            {
                return this.FImage;
            }
            set
            {
                if (this.FImage != value)
                {
                    this.FImage = value;
                    if (this.FImage != null)
                    {
                        this.FImageIndex = -1;
                        this.FImageKey = string.Empty;
                    }
                    this.OnImageChanged(EventArgs.Empty);
                }
            }
        }

        [Browsable(false), Category("Appearance"), DefaultValue(-1)]
        public int ImageIndex
        {
            get
            {
                return this.FImageIndex;
            }
            set
            {
                if (this.FImageIndex != value)
                {
                    if (value < -1)
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                    this.FImageIndex = value;
                    if (this.FImageIndex >= 0)
                    {
                        this.FImage = null;
                        this.FImageKey = string.Empty;
                    }
                    this.OnImageChanged(EventArgs.Empty);
                }
            }
        }

        [DefaultValue(""), Browsable(false), Category("Appearance")]
        public string ImageKey
        {
            get
            {
                return this.FImageKey;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }
                if (this.FImageKey != value)
                {
                    this.FImageKey = value;
                    if (this.FImageKey != string.Empty)
                    {
                        this.FImage = null;
                        this.FImageIndex = -1;
                    }
                    this.OnImageChanged(EventArgs.Empty);
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public ActionManager Owner
        {
            get
            {
                return this.FOwner;
            }
            set
            {
                if (this.FOwner != value)
                {
                    this.FOwner = value;
                    this.OnOwnerChanged(EventArgs.Empty);
                }
            }
        }

        public Keys ShortcutKeys
        {
            get
            {
                return (((this.FShortcuts == null) || (this.FShortcuts.Length == 0)) ? Keys.None : this.FShortcuts[0]);
            }
            set
            {
                if (this.ShortcutKeys != value)
                {
                    if (this.FShortcuts == null)
                    {
                        if (value != Keys.None)
                        {
                            this.Shortcuts = new Keys[] { value };
                        }
                    }
                    else if (value == Keys.None)
                    {
                        if (this.FShortcuts.Length <= 1)
                        {
                            this.Shortcuts = null;
                        }
                        else
                        {
                            Keys[] destinationArray = new Keys[this.FShortcuts.Length - 1];
                            Array.Copy(this.FShortcuts, 1, destinationArray, 0, destinationArray.Length);
                            this.Shortcuts = destinationArray;
                        }
                    }
                    else if (this.FShortcuts.Length == 0)
                    {
                        this.Shortcuts = new Keys[] { value };
                    }
                    else
                    {
                        this.FShortcuts[0] = value;
                        this.OnShortcutsChanged();
                    }
                }
            }
        }

        public Keys[] Shortcuts
        {
            get
            {
                return this.FShortcuts;
            }
            set
            {
                if (this.FShortcuts != value)
                {
                    this.FShortcuts = value;
                    this.OnShortcutsChanged();
                }
            }
        }

        [Bindable(true), DefaultValue((string) null), TypeConverter(typeof(StringConverter)), Category("Data")]
        public object Tag
        {
            get
            {
                return this.FTag;
            }
            set
            {
                this.FTag = value;
            }
        }

        [Localizable(true), Category("Appearance")]
        public virtual string Text
        {
            get
            {
                return this.FText;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }
                if (this.FText != value)
                {
                    this.FText = value;
                    this.OnTextChanged(EventArgs.Empty);
                }
            }
        }
    }
}

