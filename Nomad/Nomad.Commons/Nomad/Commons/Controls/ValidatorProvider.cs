namespace Nomad.Commons.Controls
{
    using Microsoft.Win32;
    using Nomad.Commons.Collections;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Reflection;
    using System.Windows.Forms;

    [DesignerCategory("Code"), ProvideProperty("IsValid", typeof(Control)), ProvideProperty("ValidateOn", typeof(Control)), ProvideProperty("ValidateError", typeof(Control))]
    public class ValidatorProvider : Component, IExtenderProvider
    {
        private Dictionary<Control, string> ControlErrorMap;
        private Dictionary<Control, ValidateOn> ControlValidateMap = new Dictionary<Control, ValidateOn>();
        private System.Drawing.Color ErrorBack = System.Drawing.Color.FromArgb(0xff, 0x66, 0x66);
        private System.Drawing.Color ErrorText = SystemColors.HighlightText;
        private static readonly object EventControlValidated = new object();
        private static readonly object EventValidating = new object();
        private static readonly object EventValidatingControl = new object();
        private FormValidate FFormValidate;
        private ContainerControl FOwner;
        private HashSet<Control> InvalidControlSet;
        private bool IsValidating;
        private static MethodInfo PerformControlValidationMethod;
        private Timer ValidateTimer;
        private int ValidateTimerInterval = 0x3e8;
        private ToolTip ValidateTooltip;
        private string ValidateTooltipTitle;

        public event EventHandler<ControlValidatedEventArgs> ControlValidated
        {
            add
            {
                base.Events.AddHandler(EventControlValidated, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventControlValidated, value);
            }
        }

        public event EventHandler<CancelEventArgs> Validating
        {
            add
            {
                base.Events.AddHandler(EventValidating, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventValidating, value);
            }
        }

        public event EventHandler<ValidatingControlEventArgs> ValidatingControl
        {
            add
            {
                base.Events.AddHandler(EventValidatingControl, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventValidatingControl, value);
            }
        }

        private void AddControlError(Control control, string error)
        {
            if (this.ControlErrorMap == null)
            {
                this.ControlErrorMap = new Dictionary<Control, string>();
            }
            this.ControlErrorMap.Add(control, error);
            control.Disposed += new EventHandler(this.Control_Error_Disposed);
            control.Enter += new EventHandler(this.Control_Error_Enter);
            control.Leave += new EventHandler(this.Control_Error_Leave);
            control.Validated += new EventHandler(this.Control_Error_Disposed);
            control.BackColor = this.ErrorBack;
            control.ForeColor = this.ErrorText;
            if (control.Focused)
            {
                this.ShowTooltip(control, error);
            }
        }

        private void AddControlValidate(Control control, ValidateOn validateOn)
        {
            this.ControlValidateMap.Add(control, validateOn);
            control.Disposed += new EventHandler(this.Control_Validate_Disposed);
            control.EnabledChanged += new EventHandler(this.Control_Validate_EnabledChanged);
            control.Validating += new CancelEventHandler(this.Control_Validate_Validating);
            control.Validated += new EventHandler(this.Control_Validate_Validated);
            switch (validateOn)
            {
                case ValidateOn.TextChanged:
                    control.TextChanged += new EventHandler(this.Control_Validate_TextChanged);
                    break;

                case ValidateOn.TextChangedTimer:
                    control.TextChanged += new EventHandler(this.Control_ValidateTimer_TextChanged);
                    break;

                case ValidateOn.FocusTimer:
                    control.Enter += new EventHandler(this.Control_ValidateTimer_Enter);
                    control.Leave += new EventHandler(this.Control_ValidateTimer_Leave);
                    break;

                case ValidateOn.ApplicationIdle:
                    control.Enter += new EventHandler(this.Control_ValidateIdle_Enter);
                    control.Leave += new EventHandler(this.Control_ValidateIdle_Leave);
                    break;
            }
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            ValidateOn on;
            Control key = Control.FromChildHandle(Windows.GetFocus());
            if (((key == null) || !this.ControlValidateMap.TryGetValue(key, out on)) || (on != ValidateOn.ApplicationIdle))
            {
                Application.Idle -= new EventHandler(this.Application_Idle);
            }
            else
            {
                this.ValidateControl(key, false);
            }
        }

        private void AssingOwner(ContainerControl newOwner)
        {
            if (this.FOwner != null)
            {
                this.Owner_Disposed(this.FOwner, EventArgs.Empty);
            }
            this.FOwner = newOwner;
            if (this.FOwner != null)
            {
                this.FOwner.Disposed += new EventHandler(this.Owner_Disposed);
                Form fOwner = this.FOwner as Form;
                if ((fOwner != null) && (this.FFormValidate == FormValidate.PreventClosing))
                {
                    fOwner.FormClosing += new FormClosingEventHandler(this.ValidateForm_FormClosing);
                }
            }
        }

        public bool CanExtend(object extendee)
        {
            return ((extendee is Control) && !(extendee is ContainerControl));
        }

        private void Control_Error_Disposed(object sender, EventArgs e)
        {
            this.RemoveControlError((Control) sender);
        }

        private void Control_Error_Enter(object sender, EventArgs e)
        {
            string str;
            Control key = (Control) sender;
            if (this.ControlErrorMap.TryGetValue(key, out str))
            {
                this.ShowTooltip(key, str);
            }
        }

        private void Control_Error_Leave(object sender, EventArgs e)
        {
            this.HideTooltip((Control) sender);
        }

        private void Control_Validate_Disposed(object sender, EventArgs e)
        {
            this.RemoveControlValidate((Control) sender);
        }

        private void Control_Validate_EnabledChanged(object sender, EventArgs e)
        {
            if (this.OwnerFormValidate == FormValidate.DisableAcceptButton)
            {
                this.EnableAcceptButton();
            }
        }

        private void Control_Validate_TextChanged(object sender, EventArgs e)
        {
            Control control = (Control) sender;
            if (control.Focused && !this.IsValidating)
            {
                this.IsValidating = true;
                try
                {
                    this.ValidateControl(control, false);
                }
                finally
                {
                    this.IsValidating = false;
                }
            }
        }

        private void Control_Validate_Validated(object sender, EventArgs e)
        {
            Control item = (Control) sender;
            if (this.InvalidControlSet != null)
            {
                this.InvalidControlSet.Remove(item);
            }
            if (!(((this.OwnerFormValidate != FormValidate.DisableAcceptButton) || (this.FOwner == null)) || this.FOwner.IsHandleCreated))
            {
                this.EnableAcceptButton();
            }
            this.OnControlValidated(new ControlValidatedEventArgs(item));
        }

        private void Control_Validate_Validating(object sender, CancelEventArgs e)
        {
            Control control = (Control) sender;
            ValidatingControlEventArgs args = new ValidatingControlEventArgs(control, e.Cancel);
            this.OnValidatingControl(args);
            e.Cancel = args.Cancel;
            if (this.InvalidControlSet == null)
            {
                this.InvalidControlSet = new HashSet<Control>();
            }
            this.InvalidControlSet.Add((Control) sender);
            if (((this.OwnerFormValidate == FormValidate.DisableAcceptButton) && (this.FOwner != null)) && this.FOwner.IsHandleCreated)
            {
                this.FOwner.BeginInvoke(new MethodInvoker(this.EnableAcceptButton));
            }
        }

        private void Control_ValidateIdle_Enter(object sender, EventArgs e)
        {
            Application.Idle += new EventHandler(this.Application_Idle);
        }

        private void Control_ValidateIdle_Leave(object sender, EventArgs e)
        {
            Application.Idle -= new EventHandler(this.Application_Idle);
        }

        private void Control_ValidateTimer_Enter(object sender, EventArgs e)
        {
            this.ValidateTimerNeeded();
            this.ValidateTimer.Tag = sender;
            this.ValidateTimer.Start();
        }

        private void Control_ValidateTimer_Leave(object sender, EventArgs e)
        {
            if (this.ValidateTimer != null)
            {
                this.ValidateTimer.Stop();
                this.ValidateTimer.Tag = null;
            }
        }

        private void Control_ValidateTimer_TextChanged(object sender, EventArgs e)
        {
            this.ValidateTimerNeeded();
            this.ValidateTimer.Stop();
            this.ValidateTimer.Tag = sender;
            this.ValidateTimer.Start();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.ValidateTimer != null)
                {
                    this.ValidateTimer.Dispose();
                }
                this.ValidateTimer = null;
                if (this.ValidateTooltip != null)
                {
                    this.ValidateTooltip.Dispose();
                }
                this.ValidateTooltip = null;
            }
            base.Dispose(disposing);
        }

        private void EnableAcceptButton()
        {
            Form fOwner = this.FOwner as Form;
            if (fOwner != null)
            {
                Control acceptButton = fOwner.AcceptButton as Control;
                if (acceptButton != null)
                {
                    acceptButton.Enabled = this.Validate(false);
                }
            }
        }

        protected IEnumerable<Control> GetInvalidControls(ValidationConstraints validationConstraints)
        {
            return this.InvalidControlSet.AsEnumerable<Control>().Where<Control>(delegate (Control x) {
                return ((((((validationConstraints & ValidationConstraints.Selectable) == ValidationConstraints.None) || x.CanSelect) && (((validationConstraints & ValidationConstraints.Enabled) == ValidationConstraints.None) || x.Enabled)) && (((validationConstraints & ValidationConstraints.Visible) == ValidationConstraints.None) || x.Visible)) && (((validationConstraints & ValidationConstraints.TabStop) == ValidationConstraints.None) || x.TabStop));
            });
        }

        [DefaultValue(true)]
        public bool GetIsValid(Control control)
        {
            return ((this.InvalidControlSet == null) || !this.InvalidControlSet.Contains(control));
        }

        [DefaultValue("")]
        public string GetValidateError(Control control)
        {
            string str;
            if (control == null)
            {
                throw new ArgumentNullException();
            }
            if ((this.ControlErrorMap != null) && this.ControlErrorMap.TryGetValue(control, out str))
            {
                return str;
            }
            return string.Empty;
        }

        [DefaultValue(0)]
        public ValidateOn GetValidateOn(Control control)
        {
            ValidateOn on;
            if (control == null)
            {
                throw new ArgumentNullException();
            }
            if (this.ControlValidateMap.TryGetValue(control, out on))
            {
                return on;
            }
            return ValidateOn.Default;
        }

        private void HideTooltip(Control control)
        {
            if (this.ValidateTooltip != null)
            {
                this.ValidateTooltip.Hide(control);
            }
        }

        protected virtual void OnControlValidated(ControlValidatedEventArgs e)
        {
            EventHandler<ControlValidatedEventArgs> handler = base.Events[EventValidating] as EventHandler<ControlValidatedEventArgs>;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnValidating(CancelEventArgs e)
        {
            EventHandler<CancelEventArgs> handler = base.Events[EventValidating] as EventHandler<CancelEventArgs>;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnValidatingControl(ValidatingControlEventArgs e)
        {
            EventHandler<ValidatingControlEventArgs> handler = base.Events[EventValidatingControl] as EventHandler<ValidatingControlEventArgs>;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void Owner_Disposed(object sender, EventArgs e)
        {
            ContainerControl control = (ContainerControl) sender;
            control.Disposed -= new EventHandler(this.Owner_Disposed);
            Form form = control as Form;
            if (form != null)
            {
                form.FormClosing -= new FormClosingEventHandler(this.ValidateForm_FormClosing);
            }
        }

        private void RemoveControlError(Control control)
        {
            control.Disposed -= new EventHandler(this.Control_Error_Disposed);
            control.Enter -= new EventHandler(this.Control_Error_Enter);
            control.Leave -= new EventHandler(this.Control_Error_Leave);
            control.Validated -= new EventHandler(this.Control_Error_Disposed);
            this.ControlErrorMap.Remove(control);
            control.ResetBackColor();
            control.ResetForeColor();
            this.HideTooltip(control);
        }

        private void RemoveControlValidate(Control control)
        {
            control.Disposed -= new EventHandler(this.Control_Validate_Disposed);
            control.EnabledChanged -= new EventHandler(this.Control_Validate_EnabledChanged);
            control.Validating -= new CancelEventHandler(this.Control_Validate_Validating);
            control.Validated -= new EventHandler(this.Control_Validate_Validated);
            control.TextChanged -= new EventHandler(this.Control_Validate_TextChanged);
            control.TextChanged -= new EventHandler(this.Control_ValidateTimer_TextChanged);
            control.Enter -= new EventHandler(this.Control_ValidateTimer_Enter);
            control.Leave -= new EventHandler(this.Control_ValidateTimer_Leave);
            control.Enter -= new EventHandler(this.Control_ValidateIdle_Enter);
            control.Leave -= new EventHandler(this.Control_ValidateIdle_Leave);
            if ((this.ValidateTimer != null) && (this.ValidateTimer.Tag == control))
            {
                this.ValidateTimer.Stop();
                this.ValidateTimer = null;
            }
            this.ControlValidateMap.Remove(control);
        }

        public void RemoveValidateError(Control control)
        {
            if (control == null)
            {
                throw new ArgumentNullException();
            }
            if (this.ControlErrorMap != null)
            {
                this.RemoveControlError(control);
            }
        }

        public void SetIsValid(Control control, bool isValid)
        {
            if (isValid)
            {
                if (this.InvalidControlSet != null)
                {
                    this.InvalidControlSet.Remove(control);
                }
                if (this.OwnerFormValidate == FormValidate.DisableAcceptButton)
                {
                    this.EnableAcceptButton();
                }
            }
            else
            {
                if (this.InvalidControlSet == null)
                {
                    this.InvalidControlSet = new HashSet<Control>();
                }
                this.InvalidControlSet.Add(control);
            }
        }

        public void SetValidateError(Control control, string error)
        {
            string str;
            if (control == null)
            {
                throw new ArgumentNullException();
            }
            if ((this.ControlErrorMap != null) && this.ControlErrorMap.TryGetValue(control, out str))
            {
                if (str != error)
                {
                    if (string.IsNullOrEmpty(error))
                    {
                        this.RemoveControlError(control);
                    }
                    else
                    {
                        this.ControlErrorMap[control] = error;
                        if (control.Focused)
                        {
                            this.ShowTooltip(control, error);
                        }
                    }
                }
            }
            else
            {
                this.AddControlError(control, error);
            }
        }

        public void SetValidateOn(Control control, ValidateOn validateOn)
        {
            ValidateOn on;
            if (control == null)
            {
                throw new ArgumentNullException();
            }
            if (!this.ControlValidateMap.TryGetValue(control, out on) || (on != validateOn))
            {
                this.RemoveControlValidate(control);
                switch (validateOn)
                {
                    case ValidateOn.Default:
                        return;

                    case ValidateOn.TextChanged:
                    case ValidateOn.TextChangedTimer:
                    case ValidateOn.FocusTimer:
                    case ValidateOn.ApplicationIdle:
                    case ValidateOn.Validate:
                        this.AddControlValidate(control, validateOn);
                        return;
                }
                throw new InvalidEnumArgumentException();
            }
        }

        private bool ShouldSerializeOwnerAutoValidate()
        {
            return false;
        }

        private bool ShouldSerializeOwnerFormValidate()
        {
            return (this.FOwner is Form);
        }

        private void ShowTooltip(Control control, string tooltip)
        {
            this.ValidateTooltipNeeded();
            this.ValidateTooltip.Show(tooltip, control, 0, control.Height + control.Margin.Bottom);
        }

        public bool Validate()
        {
            return this.Validate(ValidationConstraints.Enabled, true);
        }

        public bool Validate(bool validateOwner)
        {
            return this.Validate(ValidationConstraints.Enabled, validateOwner);
        }

        public bool Validate(ValidationConstraints validationConstraints, bool validateOwner)
        {
            CancelEventArgs e = new CancelEventArgs();
            if (validateOwner && (this.FOwner != null))
            {
                e.Cancel = !this.FOwner.ValidateChildren(validationConstraints);
            }
            else
            {
                foreach (Control control in this.GetInvalidControls(validationConstraints))
                {
                    e.Cancel = true;
                    break;
                }
            }
            this.OnValidating(e);
            return !e.Cancel;
        }

        private void ValidateControl(Control control, bool bulkValidation)
        {
            if (PerformControlValidationMethod == null)
            {
                PerformControlValidationMethod = typeof(Control).GetMethod("PerformControlValidation", BindingFlags.NonPublic | BindingFlags.Instance, null, new System.Type[] { typeof(bool) }, null);
            }
            if (PerformControlValidationMethod != null)
            {
                PerformControlValidationMethod.Invoke(control, new object[] { bulkValidation });
            }
            else if (!bulkValidation)
            {
                Control parent = control.Parent;
                ContainerControl control3 = parent as ContainerControl;
                while ((parent != null) && (control3 == null))
                {
                    parent = parent.Parent;
                    control3 = parent as ContainerControl;
                }
                if (control3 != null)
                {
                    if (control.Focused)
                    {
                        control3.Validate();
                    }
                    else
                    {
                        Control window = Control.FromChildHandle(Windows.GetFocus());
                        if (window != null)
                        {
                            using (new LockWindowRedraw(window, false))
                            {
                                using (new LockWindowRedraw(control, false))
                                {
                                    if (control.Focus())
                                    {
                                        control3.Validate();
                                    }
                                    window.Focus();
                                }
                            }
                        }
                    }
                }
            }
        }

        private void ValidateForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            switch (e.CloseReason)
            {
                case CloseReason.TaskManagerClosing:
                case CloseReason.ApplicationExitCall:
                case CloseReason.WindowsShutDown:
                    return;
            }
            Form form = (Form) sender;
            if ((form.Modal && (form.DialogResult != DialogResult.Cancel)) && !this.Validate(false))
            {
                foreach (Control control in this.GetInvalidControls(ValidationConstraints.Visible | ValidationConstraints.Enabled))
                {
                    control.Focus();
                    break;
                }
                if (this.FFormValidate == FormValidate.DisableAcceptButton)
                {
                    Control acceptButton = form.AcceptButton as Control;
                    if (acceptButton != null)
                    {
                        acceptButton.Enabled = false;
                    }
                }
                e.Cancel = true;
            }
        }

        private void ValidateTimer_Tick(object sender, EventArgs e)
        {
            Timer timer = (Timer) sender;
            Control tag = (Control) timer.Tag;
            if (tag == null)
            {
                timer.Stop();
            }
            else
            {
                ValidateOn on;
                if (this.ControlValidateMap.TryGetValue(tag, out on) && (on == ValidateOn.TextChangedTimer))
                {
                    timer.Stop();
                }
                this.ValidateControl(tag, false);
            }
        }

        private void ValidateTimerNeeded()
        {
            if (this.ValidateTimer == null)
            {
                this.ValidateTimer = new Timer();
                this.ValidateTimer.Interval = this.ValidateTimerInterval;
                this.ValidateTimer.Tick += new EventHandler(this.ValidateTimer_Tick);
            }
        }

        private void ValidateTooltipNeeded()
        {
            if (this.ValidateTooltip == null)
            {
                this.ValidateTooltip = new ToolTip();
                this.ValidateTooltip.ToolTipIcon = ToolTipIcon.Error;
                this.ValidateTooltip.ToolTipTitle = this.ValidateTooltipTitle;
            }
        }

        [DefaultValue(typeof(System.Drawing.Color), "#ff6666"), Category("Appearance")]
        public System.Drawing.Color ErrorBackColor
        {
            get
            {
                return this.ErrorBack;
            }
            set
            {
                this.ErrorBack = value;
            }
        }

        [DefaultValue(typeof(System.Drawing.Color), "HighlightText"), Category("Appearance")]
        public System.Drawing.Color ErrorForeColor
        {
            get
            {
                return this.ErrorText;
            }
            set
            {
                this.ErrorText = value;
            }
        }

        [DefaultValue((string) null), Category("Owner")]
        public ContainerControl Owner
        {
            get
            {
                return this.FOwner;
            }
            set
            {
                if (this.FOwner != value)
                {
                    this.AssingOwner(value);
                }
            }
        }

        [Category("Owner")]
        public AutoValidate OwnerAutoValidate
        {
            get
            {
                return ((this.FOwner != null) ? this.FOwner.AutoValidate : AutoValidate.Disable);
            }
            set
            {
                if (this.FOwner != null)
                {
                    this.FOwner.AutoValidate = value;
                }
            }
        }

        [Category("Owner"), DefaultValue(0)]
        public FormValidate OwnerFormValidate
        {
            get
            {
                return (this.ShouldSerializeOwnerFormValidate() ? this.FFormValidate : FormValidate.None);
            }
            set
            {
                bool flag = this.FFormValidate == FormValidate.PreventClosing;
                this.FFormValidate = value;
                if (this.ShouldSerializeOwnerFormValidate() && (flag ^ (this.FFormValidate == FormValidate.PreventClosing)))
                {
                    this.AssingOwner(this.FOwner);
                }
            }
        }

        [DefaultValue(0x3e8)]
        public int TimerInterval
        {
            get
            {
                return ((this.ValidateTimer != null) ? this.ValidateTimer.Interval : this.ValidateTimerInterval);
            }
            set
            {
                this.ValidateTimerInterval = (value > 0) ? value : 1;
                if (this.ValidateTimer != null)
                {
                    this.ValidateTimer.Interval = this.ValidateTimerInterval;
                }
            }
        }

        [DefaultValue("")]
        public string TooltipTitle
        {
            get
            {
                return ((this.ValidateTooltip != null) ? this.ValidateTooltip.ToolTipTitle : (this.ValidateTooltipTitle ?? string.Empty));
            }
            set
            {
                this.ValidateTooltipTitle = value;
                if (this.ValidateTooltip != null)
                {
                    this.ValidateTooltip.ToolTipTitle = this.ValidateTooltipTitle;
                }
            }
        }
    }
}

