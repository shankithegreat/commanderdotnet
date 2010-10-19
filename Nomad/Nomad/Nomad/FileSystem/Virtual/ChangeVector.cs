namespace Nomad.FileSystem.Virtual
{
    using System;
    using System.Collections.Specialized;

    public static class ChangeVector
    {
        public static readonly BitVector32.Section CuttedItems = BitVector32.CreateSection(2, Highlighters);
        private static BitVector32 FValue = new BitVector32();
        public static readonly BitVector32.Section Highlighters = BitVector32.CreateSection(3, Icon);
        public static readonly BitVector32.Section Icon = BitVector32.CreateSection(3, Localization);
        public static readonly BitVector32.Section Localization = BitVector32.CreateSection(3);

        public static void CopyTo(ref int value, BitVector32.Section section)
        {
            int num = section.Mask << section.Offset;
            value = (value & ~num) | (FValue.Data & num);
        }

        public static bool Equals(int value, BitVector32.Section section)
        {
            int num = section.Mask << section.Offset;
            return ((value & num) == (FValue.Data & num));
        }

        public static unsafe void Increment(BitVector32.Section section)
        {
            ref BitVector32 vectorRef;
            BitVector32.Section section2;
            (vectorRef = (BitVector32) &FValue)[section2 = section] = vectorRef[section2] + 1;
        }

        public static int Value
        {
            get
            {
                return FValue.Data;
            }
        }
    }
}

