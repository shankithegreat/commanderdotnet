namespace System
{
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct Tuple<T1, T2, T3>
    {
        public readonly T1 Item1;
        public readonly T2 Item2;
        public readonly T3 Item3;
        public Tuple(T1 item1, T2 item2, T3 item3)
        {
            this.Item1 = item1;
            this.Item2 = item2;
            this.Item3 = item3;
        }

        public override bool Equals(object obj)
        {
            if (obj is Tuple<T1, T2, T3>)
            {
                Tuple<T1, T2, T3> tuple = (Tuple<T1, T2, T3>) obj;
                return ((tuple.Item1.Equals(this.Item1) && tuple.Item2.Equals(this.Item2)) && tuple.Item3.Equals(this.Item3));
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (((base.GetHashCode() ^ this.Item1.GetHashCode()) ^ this.Item2.GetHashCode()) ^ this.Item3.GetHashCode());
        }
    }
}

