namespace Nomad.Workers
{
    using Nomad.FileSystem.Property;
    using Nomad.FileSystem.Virtual;
    using System;
    using System.Collections;

    internal class OverwritePropertyRule : IOverwriteRule
    {
        private Compare CompareOperator;
        private OverwriteDialogResult OverwriteResult;
        private int PropertyId;

        public OverwritePropertyRule(int propertyId, Compare compareOperator, OverwriteDialogResult result)
        {
            if (((result == OverwriteDialogResult.None) || (result == OverwriteDialogResult.Abort)) || (result == OverwriteDialogResult.Rename))
            {
                throw new ArgumentException();
            }
            this.PropertyId = propertyId;
            this.CompareOperator = compareOperator;
            this.OverwriteResult = result;
        }

        public OverwriteDialogResult GetOverwrite(IVirtualItem source, IVirtualItem dest)
        {
            if (this.CompareOperator == Compare.Always)
            {
                return this.OverwriteResult;
            }
            if (((this.CompareOperator != Compare.Never) && (source != null)) && (dest != null))
            {
                int num;
                if (!(source.IsPropertyAvailable(this.PropertyId) && dest.IsPropertyAvailable(this.PropertyId)))
                {
                    return OverwriteDialogResult.None;
                }
                switch (this.PropertyId)
                {
                    case 7:
                    case 8:
                    case 9:
                    {
                        DateTime time = (DateTime) source[8];
                        DateTime time2 = (DateTime) dest[8];
                        TimeSpan span = (TimeSpan) (time2 - time);
                        num = Math.Sign((long) (span.Ticks / 0x989680L));
                        break;
                    }
                    default:
                        num = Comparer.DefaultInvariant.Compare(dest[this.PropertyId], source[this.PropertyId]);
                        break;
                }
                switch (this.CompareOperator)
                {
                    case Compare.Equal:
                        return ((num == 0) ? this.OverwriteResult : OverwriteDialogResult.None);

                    case Compare.Greater:
                        return ((num > 0) ? this.OverwriteResult : OverwriteDialogResult.None);

                    case Compare.GreaterEqual:
                        return ((num >= 0) ? this.OverwriteResult : OverwriteDialogResult.None);

                    case Compare.Less:
                        return ((num < 0) ? this.OverwriteResult : OverwriteDialogResult.None);

                    case Compare.LessEqual:
                        return ((num <= 0) ? this.OverwriteResult : OverwriteDialogResult.None);

                    case Compare.NotEqual:
                        return ((num != 0) ? this.OverwriteResult : OverwriteDialogResult.None);
                }
            }
            return OverwriteDialogResult.None;
        }
    }
}

