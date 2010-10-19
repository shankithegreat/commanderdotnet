namespace Nomad.Workers
{
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.Collections;

    internal class OverwritePropertyValueRule : IOverwriteRule
    {
        private OverwriteDialogResult OverwriteResult;
        private int PropertyId;
        private object Value;

        public OverwritePropertyValueRule(int propertyId, object value, OverwriteDialogResult result)
        {
            if (((result == OverwriteDialogResult.None) || (result == OverwriteDialogResult.Abort)) || (result == OverwriteDialogResult.Rename))
            {
                throw new ArgumentException();
            }
            this.PropertyId = propertyId;
            this.Value = value;
            this.OverwriteResult = result;
        }

        public OverwriteDialogResult GetOverwrite(IVirtualItem source, IVirtualItem dest)
        {
            if ((source != null) && (dest != null))
            {
                if (!(source.IsPropertyAvailable(this.PropertyId) && dest.IsPropertyAvailable(this.PropertyId)))
                {
                    return OverwriteDialogResult.None;
                }
                object a = source[this.PropertyId];
                if ((Comparer.DefaultInvariant.Compare(a, this.Value) == 0) && (Comparer.DefaultInvariant.Compare(a, dest[this.PropertyId]) == 0))
                {
                    return this.OverwriteResult;
                }
            }
            return OverwriteDialogResult.None;
        }
    }
}

