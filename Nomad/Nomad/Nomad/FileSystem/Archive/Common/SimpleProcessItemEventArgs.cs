namespace Nomad.FileSystem.Archive.Common
{
    using System;

    public class SimpleProcessItemEventArgs : ProcessItemEventArgs
    {
        private ISequenceableItem FItem;
        private object FUserState;

        public SimpleProcessItemEventArgs(ISequenceableItem item, object userState)
        {
            this.FItem = item;
            this.FUserState = userState;
        }

        public override ISequenceableItem Item
        {
            get
            {
                return this.FItem;
            }
        }

        public override object UserState
        {
            get
            {
                return this.FUserState;
            }
        }
    }
}

