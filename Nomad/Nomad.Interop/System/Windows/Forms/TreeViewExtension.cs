namespace System.Windows.Forms
{
    using Microsoft.Win32;
    using System;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public static class TreeViewExtension
    {
        private static MethodInfo NodeFromHandle;

        public static bool GetDropHilited(this TreeNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException();
            }
            if (!((node.TreeView != null) && node.TreeView.IsHandleCreated))
            {
                return false;
            }
            TVITEM pitem = new TVITEM {
                hItem = node.Handle,
                mask = TVIF.TVIF_HANDLE | TVIF.TVIF_STATE,
                stateMask = TVIS.TVIS_DROPHILITED
            };
            CommCtrl.TreeView_GetItem(node.TreeView.Handle, ref pitem);
            return ((pitem.state & TVIS.TVIS_DROPHILITED) > ((TVIS) 0));
        }

        public static TreeNode GetDropHilited(this TreeView tv)
        {
            if (!tv.IsHandleCreated)
            {
                return null;
            }
            IntPtr ptr = Windows.SendMessage(tv.Handle, 0x110a, (IntPtr) 8L, IntPtr.Zero);
            if (ptr == IntPtr.Zero)
            {
                return null;
            }
            if (NodeFromHandle == null)
            {
                NodeFromHandle = typeof(TreeView).GetMethod("NodeFromHandle", BindingFlags.NonPublic | BindingFlags.Instance);
            }
            return (TreeNode) NodeFromHandle.Invoke(tv, new object[] { ptr });
        }

        public static void SetDropHilited(this TreeNode node, bool value)
        {
            if (node == null)
            {
                throw new ArgumentNullException();
            }
            if (!((node.TreeView != null) && node.TreeView.IsHandleCreated))
            {
                throw new InvalidOperationException();
            }
            if (value)
            {
                Windows.SendMessage(node.TreeView.Handle, 0x110b, (IntPtr) 8L, node.Handle);
            }
            else
            {
                TVITEM pitem = new TVITEM {
                    hItem = node.Handle,
                    mask = TVIF.TVIF_HANDLE | TVIF.TVIF_STATE,
                    state = value ? TVIS.TVIS_DROPHILITED : ((TVIS) 0),
                    stateMask = TVIS.TVIS_DROPHILITED
                };
                CommCtrl.TreeView_SetItem(node.TreeView.Handle, ref pitem);
            }
        }

        public static void SetDropHilited(this TreeView tv, TreeNode node)
        {
            if (tv == null)
            {
                throw new ArgumentNullException();
            }
            if (!tv.IsHandleCreated)
            {
                throw new InvalidOperationException();
            }
            Windows.SendMessage(tv.Handle, 0x110b, (IntPtr) 8L, (node != null) ? node.Handle : IntPtr.Zero);
        }

        public static void SetHasChildren(this TreeNode node, bool value)
        {
            if (node == null)
            {
                throw new ArgumentNullException();
            }
            if (!((node.TreeView != null) && node.TreeView.IsHandleCreated))
            {
                throw new InvalidOperationException();
            }
            TVITEM pitem = new TVITEM {
                hItem = node.Handle,
                mask = TVIF.TVIF_CHILDREN | TVIF.TVIF_HANDLE,
                cChildren = value ? 1 : 0
            };
            CommCtrl.TreeView_SetItem(node.TreeView.Handle, ref pitem);
        }
    }
}

