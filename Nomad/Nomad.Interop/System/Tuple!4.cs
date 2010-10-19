namespace System
{
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct Tuple<T1, T2, T3, T4>
    {
        public readonly T1 Item1;
        public readonly T2 Item2;
        public readonly T3 Item3;
        public readonly T4 Item4;
        public Tuple(T1 item1, T2 item2, T3 item3, T4 item4)
        {
            this.Item1 = item1;
            this.Item2 = item2;
            this.Item3 = item3;
            this.Item4 = item4;
        }

        public override bool Equals(object obj)
        {
            if (obj is Tuple<T1, T2, T3, T4>)
            {
                Tuple<T1, T2, T3, T4> tuple = (Tuple<T1, T2, T3, T4>) obj;
                return (((tuple.Item1.Equals(this.Item1) && tuple.Item2.Equals(this.Item2)) && tuple.Item3.Equals(this.Item3)) && tuple.Item4.Equals(this.Item4));
            }
            return false;
        }

        public override int GetHashCode()
        {
            return ((((base.GetHashCode() ^ this.Item1.GetHashCode()) ^ this.Item2.GetHashCode()) ^ this.Item3.GetHashCode()) ^ this.Item4.GetHashCode());
        }
    }
}

