namespace System.Windows.Forms
{
    using Microsoft.COM;
    using Microsoft.Win32;
    using System;
    using System.Drawing;
    using System.Drawing.Text;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;
    using System.Security;

    [SuppressUnmanagedCodeSecurity]
    public sealed class DragImage : IDisposable
    {
        private const string ComCtl32Dll = "comctl32.dll";
        private bool Disposed;
        private static IDropTargetHelper Helper;
        private ImageList Image;
        private IntPtr ImagePtr;

        static DragImage()
        {
            try
            {
                Helper = new DragDropHelper() as IDropTargetHelper;
            }
            catch (COMException)
            {
            }
        }

        public DragImage(ListViewItem listItem)
        {
            Point point;
            this.ImagePtr = CommCtrl.ListView_CreateDragImage(listItem.ListView.Handle, listItem.Index, out point);
            Point point2 = listItem.ListView.PointToClient(Cursor.Position);
            ImageList_BeginDrag(this.ImagePtr, 0, point2.X - point.X, point2.Y - point.Y);
        }

        public DragImage(TreeNode node)
        {
            Point point = node.TreeView.PointToClient(Cursor.Position);
            this.ImagePtr = CommCtrl.TreeView_CreateDragImage(node.TreeView.Handle, node.Handle);
            ImageList_BeginDrag(this.ImagePtr, 0, point.X - node.Bounds.Left, point.Y - node.Bounds.Y);
        }

        public DragImage(System.Drawing.Image dragImage, Point hotSpot)
        {
            this.Image = new ImageList();
            this.Image.ColorDepth = ColorDepth.Depth32Bit;
            this.Image.ImageSize = dragImage.Size;
            this.Image.Images.Add(dragImage);
            ImageList_BeginDrag(this.Image.Handle, 0, hotSpot.X, hotSpot.Y);
        }

        public DragImage(System.Drawing.Image dragImage, string dragText, Font textFont, System.Drawing.Color textColor, Point hotSpot)
        {
            Size size2 = TextRenderer.MeasureText(dragText, textFont);
            if (dragImage != null)
            {
                size2.Width += dragImage.Width;
                size2.Height = dragImage.Height;
            }
            size2.Width = Math.Min(size2.Width, 0x100);
            Bitmap image = new Bitmap(size2.Width, size2.Height);
            using (Graphics graphics = Graphics.FromImage(image))
            {
                int x = 0;
                if (dragImage != null)
                {
                    graphics.DrawImage(dragImage, x, 0);
                    x += dragImage.Width;
                }
                graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                TextRenderer.DrawText(graphics, dragText, textFont, new Rectangle(x, 0, image.Width - x, image.Height), textColor, TextFormatFlags.EndEllipsis | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter);
            }
            this.Image = new ImageList();
            this.Image.ColorDepth = ColorDepth.Depth32Bit;
            this.Image.ImageSize = image.Size;
            this.Image.Images.Add(image);
            ImageList_BeginDrag(this.Image.Handle, 0, hotSpot.X, hotSpot.Y);
        }

        public void Dispose()
        {
            if (!this.Disposed)
            {
                this.Dispose(true);
                this.Disposed = true;
                GC.SuppressFinalize(this);
            }
        }

        private void Dispose(bool disposing)
        {
            ImageList_EndDrag();
            if (this.Image != null)
            {
                this.Image.Dispose();
            }
            this.Image = null;
            if (this.ImagePtr != IntPtr.Zero)
            {
                ImageList_Destroy(this.ImagePtr);
            }
            this.ImagePtr = IntPtr.Zero;
        }

        public static void DragDrop(IWin32Window owner, DragEventArgs e)
        {
            ImageList_DragLeave(owner.Handle);
            if (Helper != null)
            {
                Point ppt = new Point(e.X, e.Y);
                Helper.Drop((System.Runtime.InteropServices.ComTypes.IDataObject) e.Data, ref ppt, (uint) e.Effect);
            }
        }

        public static void DragEnter(IWin32Window owner, DragEventArgs e)
        {
            ImageList_DragEnter(owner.Handle, e.X, e.Y);
            if ((Helper != null) && e.Data.GetDataPresent("DragContext"))
            {
                Point pt = new Point(e.X, e.Y);
                Helper.DragEnter(owner.Handle, (System.Runtime.InteropServices.ComTypes.IDataObject) e.Data, ref pt, (uint) e.Effect);
            }
        }

        public static void DragLeave(IWin32Window owner)
        {
            ImageList_DragLeave(owner.Handle);
            if (Helper != null)
            {
                Helper.DragLeave();
            }
        }

        public static void DragOver(Control owner, DragEventArgs e)
        {
            Point p = new Point(e.X, e.Y);
            Point point2 = owner.PointToClient(p);
            ImageList_DragMove(point2.X, point2.Y);
            if (Helper != null)
            {
                Helper.DragOver(ref p, (uint) e.Effect);
            }
        }

        ~DragImage()
        {
            this.Dispose(false);
        }

        public static void Hide()
        {
            ImageList_DragShowNolock(false);
            if (Helper != null)
            {
                Helper.Show(false);
            }
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("comctl32.dll")]
        private static extern bool ImageList_BeginDrag(IntPtr himlTrack, int iTrack, int dxHotspot, int dyHotspot);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("comctl32.dll")]
        private static extern bool ImageList_Destroy(IntPtr himl);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("comctl32.dll")]
        private static extern bool ImageList_DragEnter(IntPtr hwndLock, int x, int y);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("comctl32.dll")]
        private static extern bool ImageList_DragLeave(IntPtr hwndLock);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("comctl32.dll")]
        private static extern bool ImageList_DragMove(int x, int y);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("comctl32.dll")]
        private static extern bool ImageList_DragShowNolock([MarshalAs(UnmanagedType.Bool)] bool fShow);
        [DllImport("comctl32.dll")]
        private static extern void ImageList_EndDrag();
        public static void Show()
        {
            ImageList_DragShowNolock(true);
            if (Helper != null)
            {
                Helper.Show(true);
            }
        }
    }
}

