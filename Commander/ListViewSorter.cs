﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.ComponentModel;
using ShellDll;

namespace Commander
{
    /// <summary>
    /// This class is used to sort the ListViewItems in the BrowserListView
    /// </summary>
    public class BrowserListSorter : IComparer
    {
        /// <summary>
        /// This method will compare the ShellItems of the ListViewItems to determine the return value for
        /// comparing the ListViewItems.
        /// </summary>
        public int Compare(object x, object y)
        {
            ListViewItem itemX = x as ListViewItem;
            ListViewItem itemY = y as ListViewItem;

            if (itemX != null && itemX.Tag != null)
            {
                return 1;
            }
            else if (itemY != null && itemY.Tag != null)
            {
                return -1;
            }
            return 0;
        }
    }
}
