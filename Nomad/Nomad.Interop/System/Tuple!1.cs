namespace System
{
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct Tuple<T1>
    {
        public readonly T1 Item1;
        public Tuple(T1 item1)
        {
            this.Item1 = item1;
        }

        public override bool Equals(object obj)
        {
            return ((obj is Tuple<T1>) && ((Tuple<T1>) obj).Item1.Equals(this.Item1));
        }

        public override int GetHashCode()
        {
            return (base.GetHashCode() ^ this.Item1.GetHashCode());
        }
    }
}

