namespace Nomad
{
    using Nomad.Dialogs;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.ComponentModel;

    public class ChangeItemErrorEventArgs : VirtualItemEventArgs
    {
        private ChangeItemAction _Action;
        private AvailableItemActions _Available;
        public readonly Exception Error;

        public ChangeItemErrorEventArgs(IVirtualItem item, AvailableItemActions available, Exception error) : base(item)
        {
            this._Available = available;
            this.Error = error;
        }

        public void FromMessageDialogResult(MessageDialogResult result)
        {
            switch (result)
            {
                case MessageDialogResult.Skip:
                    this.Action = ChangeItemAction.Skip;
                    return;

                case MessageDialogResult.Retry:
                    this.Action = ChangeItemAction.Retry;
                    return;

                case MessageDialogResult.Ignore:
                    this.Action = ChangeItemAction.Ignore;
                    return;

                case MessageDialogResult.Cancel:
                    this.Action = ChangeItemAction.Cancel;
                    return;
            }
            throw new InvalidEnumArgumentException();
        }

        public ChangeItemAction Action
        {
            get
            {
                return this._Action;
            }
            set
            {
                if (!((value != ChangeItemAction.Ignore) || this.CanIgnore))
                {
                    throw new ArgumentException();
                }
                this._Action = value;
            }
        }

        public AvailableItemActions Available
        {
            get
            {
                return this._Available;
            }
        }

        public bool CanElevate
        {
            get
            {
                return ((this._Available & AvailableItemActions.CanElevate) > AvailableItemActions.None);
            }
            set
            {
                if (value)
                {
                    this._Available |= AvailableItemActions.CanElevate;
                }
                else
                {
                    this._Available &= ~AvailableItemActions.CanElevate;
                }
            }
        }

        public bool CanIgnore
        {
            get
            {
                return ((this._Available & AvailableItemActions.CanIgnore) > AvailableItemActions.None);
            }
        }

        public bool CanRetry
        {
            get
            {
                return ((this._Available & AvailableItemActions.CanRetry) > AvailableItemActions.None);
            }
        }
    }
}

