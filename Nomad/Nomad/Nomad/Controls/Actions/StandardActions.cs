namespace Nomad.Controls.Actions
{
    using Microsoft.Win32;
    using Nomad.Properties;
    using System;
    using System.ComponentModel.Design;
    using System.Windows.Forms;

    public static class StandardActions
    {
        private static StandardAction _Copy;
        private static StandardAction _Cut;
        private static StandardAction _Delete;
        private static StandardAction _Paste;
        private static StandardAction _SelectAll;

        public static StandardAction Copy
        {
            get
            {
                if (_Copy == null)
                {
                    StandardAction action1 = _Copy;
                }
                return (_Copy = new CopyAction());
            }
        }

        public static StandardAction Cut
        {
            get
            {
                if (_Cut == null)
                {
                    StandardAction action1 = _Cut;
                }
                return (_Cut = new CutAction());
            }
        }

        public static StandardAction Delete
        {
            get
            {
                if (_Delete == null)
                {
                    StandardAction action1 = _Delete;
                }
                return (_Delete = new DeleteAction());
            }
        }

        public static StandardAction Paste
        {
            get
            {
                if (_Paste == null)
                {
                    StandardAction action1 = _Paste;
                }
                return (_Paste = new PasteAction());
            }
        }

        public static StandardAction SelectAll
        {
            get
            {
                if (_SelectAll == null)
                {
                    StandardAction action1 = _SelectAll;
                }
                return (_SelectAll = new SelectAllAction());
            }
        }

        private class CopyAction : StandardActions.StandardSelectionAction
        {
            public CopyAction() : base(StandardCommands.Copy)
            {
                base.Shortcuts = new Keys[] { Keys.Control | Keys.C, Keys.Control | Keys.Insert };
            }

            protected override void DoExecute(ActionEventArgs e)
            {
                base.DoExecute(e);
                if (!e.Handled)
                {
                    TextBox target = e.Target as TextBox;
                    if (target != null)
                    {
                        target.Copy();
                        e.Handled = true;
                    }
                    else
                    {
                        ComboBox box2 = e.Target as ComboBox;
                        if ((box2 != null) && (box2.DropDownStyle != ComboBoxStyle.DropDownList))
                        {
                            Windows.SendMessage(box2.Handle, 0x301, IntPtr.Zero, IntPtr.Zero);
                            e.Handled = true;
                        }
                    }
                }
            }

            public override string DefaultText
            {
                get
                {
                    return Resources.sStandardActionCopy;
                }
            }
        }

        private class CutAction : StandardActions.StandardSelectionAction
        {
            public CutAction() : base(StandardCommands.Cut)
            {
                base.ShortcutKeys = Keys.Control | Keys.X;
            }

            protected override void DoExecute(ActionEventArgs e)
            {
                base.DoExecute(e);
                if (!e.Handled)
                {
                    TextBox target = e.Target as TextBox;
                    if (target != null)
                    {
                        target.Cut();
                        e.Handled = true;
                    }
                    else
                    {
                        ComboBox box2 = e.Target as ComboBox;
                        if ((box2 != null) && (box2.DropDownStyle != ComboBoxStyle.DropDownList))
                        {
                            Windows.SendMessage(box2.Handle, 0x300, IntPtr.Zero, IntPtr.Zero);
                            e.Handled = true;
                        }
                    }
                }
            }

            public override string DefaultText
            {
                get
                {
                    return Resources.sStandardActionCut;
                }
            }
        }

        private class DeleteAction : StandardActions.StandardSelectionAction
        {
            public DeleteAction() : base(StandardCommands.Delete)
            {
            }

            protected override void DoExecute(ActionEventArgs e)
            {
                base.DoExecute(e);
                if (!e.Handled)
                {
                    TextBox target = e.Target as TextBox;
                    if (target != null)
                    {
                        target.SelectedText = string.Empty;
                        e.Handled = true;
                    }
                    else
                    {
                        ComboBox box2 = e.Target as ComboBox;
                        if ((box2 != null) && (box2.DropDownStyle != ComboBoxStyle.DropDownList))
                        {
                            box2.SelectedText = string.Empty;
                            e.Handled = true;
                        }
                    }
                }
            }

            public override string DefaultText
            {
                get
                {
                    return Resources.sStandardActionDelete;
                }
            }
        }

        private class PasteAction : StandardAction
        {
            public PasteAction() : base(StandardCommands.Paste)
            {
                base.Shortcuts = new Keys[] { Keys.Control | Keys.V, Keys.Shift | Keys.Insert };
            }

            protected override void DoExecute(ActionEventArgs e)
            {
                base.DoExecute(e);
                if (!e.Handled)
                {
                    TextBox target = e.Target as TextBox;
                    if (target != null)
                    {
                        target.Paste();
                        e.Handled = true;
                    }
                    else
                    {
                        ComboBox box2 = e.Target as ComboBox;
                        if ((box2 != null) && (box2.DropDownStyle != ComboBoxStyle.DropDownList))
                        {
                            Windows.SendMessage(box2.Handle, 770, IntPtr.Zero, IntPtr.Zero);
                            e.Handled = true;
                        }
                    }
                }
            }

            protected override void DoUpdate(UpdateActionEventArgs e)
            {
                base.DoUpdate(e);
                if (!e.Handled)
                {
                    e.State = ActionState.Visible;
                    ComboBox target = e.Target as ComboBox;
                    if ((e.Target is TextBox) || ((target != null) && (target.DropDownStyle != ComboBoxStyle.DropDownList)))
                    {
                        try
                        {
                            if (Clipboard.ContainsText())
                            {
                                e.State |= ActionState.Enabled;
                            }
                        }
                        catch
                        {
                        }
                        e.Handled = true;
                    }
                }
            }

            public override string DefaultText
            {
                get
                {
                    return Resources.sStandardActionPaste;
                }
            }
        }

        private class SelectAllAction : StandardAction
        {
            public SelectAllAction() : base(StandardCommands.SelectAll)
            {
                base.ShortcutKeys = Keys.Control | Keys.A;
            }

            protected override void DoExecute(ActionEventArgs e)
            {
                base.DoExecute(e);
                if (!e.Handled)
                {
                    TextBox target = e.Target as TextBox;
                    if (target != null)
                    {
                        target.SelectAll();
                        e.Handled = true;
                    }
                    else
                    {
                        ComboBox box2 = e.Target as ComboBox;
                        if ((box2 != null) && (box2.DropDownStyle != ComboBoxStyle.DropDownList))
                        {
                            box2.SelectAll();
                            e.Handled = true;
                        }
                    }
                }
            }

            protected override void DoUpdate(UpdateActionEventArgs e)
            {
                base.DoUpdate(e);
                if (!e.Handled)
                {
                    e.State = ActionState.Visible;
                    TextBox target = e.Target as TextBox;
                    if (target != null)
                    {
                        if (target.SelectionLength < target.Text.Length)
                        {
                            e.State |= ActionState.Enabled;
                        }
                        e.Handled = true;
                    }
                    else
                    {
                        ComboBox box2 = e.Target as ComboBox;
                        if ((box2 != null) && (box2.DropDownStyle != ComboBoxStyle.DropDownList))
                        {
                            if (box2.SelectionLength < box2.Text.Length)
                            {
                                e.State |= ActionState.Enabled;
                            }
                            e.Handled = true;
                        }
                    }
                }
            }

            public override string DefaultText
            {
                get
                {
                    return Resources.sStandardActionSelectAll;
                }
            }
        }

        private abstract class StandardSelectionAction : StandardAction
        {
            protected StandardSelectionAction(CommandID actionId) : base(actionId)
            {
            }

            protected override void DoUpdate(UpdateActionEventArgs e)
            {
                base.DoUpdate(e);
                if (!e.Handled)
                {
                    e.State = ActionState.Visible;
                    TextBox target = e.Target as TextBox;
                    if (target != null)
                    {
                        if (target.SelectionLength > 0)
                        {
                            e.State |= ActionState.Enabled;
                        }
                        e.Handled = true;
                    }
                    else
                    {
                        ComboBox box2 = e.Target as ComboBox;
                        if ((box2 != null) && (box2.DropDownStyle != ComboBoxStyle.DropDownList))
                        {
                            if (box2.SelectionLength > 0)
                            {
                                e.State |= ActionState.Enabled;
                            }
                            e.Handled = true;
                        }
                    }
                }
            }
        }
    }
}

