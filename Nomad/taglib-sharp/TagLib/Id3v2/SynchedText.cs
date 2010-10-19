namespace TagLib.Id3v2
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct SynchedText
    {
        private long time;
        private string text;
        public SynchedText(long time, string text)
        {
            this.time = time;
            this.text = text;
        }

        public long Time
        {
            get
            {
                return this.time;
            }
            set
            {
                this.time = value;
            }
        }
        public string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                this.text = value;
            }
        }
    }
}

