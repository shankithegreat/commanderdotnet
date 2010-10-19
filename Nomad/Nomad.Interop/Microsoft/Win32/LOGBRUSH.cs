namespace Microsoft.Win32
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct LOGBRUSH
    {
        public BS lbStyle;
        public uint lbColor;
        public HS lbHatch;
        public LOGBRUSH(BS style)
        {
            this.lbStyle = style;
            this.lbColor = 0;
            this.lbHatch = HS.HS_HORIZONTAL;
        }

        public LOGBRUSH(System.Drawing.Color color)
        {
            this.lbStyle = BS.BS_SOLID;
            this.lbColor = WindowsWrapper.ColorToCOLORREF(color);
            this.lbHatch = HS.HS_HORIZONTAL;
        }
    }
}

