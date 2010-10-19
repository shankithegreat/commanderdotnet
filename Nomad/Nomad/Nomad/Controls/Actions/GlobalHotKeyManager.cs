namespace Nomad.Controls.Actions
{
    using Microsoft.Win32;
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    public static class GlobalHotKeyManager
    {
        private static Dictionary<Action, int[]> ActionIdMap;
        private static HotKeyFilter HotKeyListener;
        private static int NextId;
        private static int SuspendedCount;

        private static void Action_ShortcutsChanged(object sender, EventArgs e)
        {
            HotKeyListener.ResetHotKeyMap();
            if (SuspendedCount <= 0)
            {
                Action action = (Action) sender;
                UnregisterHotKeys(ActionIdMap[action]);
                ActionIdMap[action] = RegisterHotKeys(action);
            }
        }

        public static void RegisterAction(Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException();
            }
            UnregisterAction(action);
            if (ActionIdMap == null)
            {
                ActionIdMap = new Dictionary<Action, int[]>();
            }
            if (HotKeyListener == null)
            {
                HotKeyListener = new HotKeyFilter(ActionIdMap.Keys);
            }
            ActionIdMap.Add(action, RegisterHotKeys(action));
            action.ShortcutsChanged += new EventHandler(GlobalHotKeyManager.Action_ShortcutsChanged);
            HotKeyListener.ResetHotKeyMap();
        }

        private static int[] RegisterHotKeys(Action action)
        {
            if ((action.Shortcuts == null) || (action.Shortcuts.Length == 0))
            {
                return new int[0];
            }
            List<int> list = new List<int>();
            foreach (Keys keys in action.Shortcuts)
            {
                MOD fsModifiers = WindowsWrapper.ShortcutKeysToModifiers(keys);
                if (Windows.RegisterHotKey(IntPtr.Zero, ++NextId, fsModifiers, (uint) (keys & Keys.KeyCode)))
                {
                    list.Add(NextId);
                    HotKeyListener.Register();
                }
            }
            return list.ToArray();
        }

        public static void ResumeHotKeys()
        {
            SuspendedCount--;
            if (SuspendedCount <= 0)
            {
                List<Action> list = new List<Action>(ActionIdMap.Keys);
                foreach (Action action in list)
                {
                    ActionIdMap[action] = RegisterHotKeys(action);
                }
            }
        }

        public static void SuspendHotKeys()
        {
            SuspendedCount++;
            if (SuspendedCount <= 1)
            {
                foreach (int[] numArray in ActionIdMap.Values)
                {
                    UnregisterHotKeys(numArray);
                }
                HotKeyListener.Unregister();
            }
        }

        public static bool UnregisterAction(Action action)
        {
            int[] numArray;
            if (action == null)
            {
                throw new ArgumentNullException();
            }
            if (!((ActionIdMap != null) && ActionIdMap.TryGetValue(action, out numArray)))
            {
                return false;
            }
            action.ShortcutsChanged -= new EventHandler(GlobalHotKeyManager.Action_ShortcutsChanged);
            UnregisterHotKeys(numArray);
            ActionIdMap.Remove(action);
            if (ActionIdMap.Count == 0)
            {
                HotKeyListener.Unregister();
            }
            return true;
        }

        private static void UnregisterHotKeys(int[] hotKeyList)
        {
            foreach (int num in hotKeyList)
            {
                Windows.UnregisterHotKey(IntPtr.Zero, num);
            }
        }

        private class HotKeyFilter : IMessageFilter
        {
            private IEnumerable<Action> FActions;
            private Dictionary<int, Action> HotKeyActionMap;
            private bool Registered;

            public HotKeyFilter(IEnumerable<Action> actions)
            {
                this.FActions = actions;
            }

            public bool PreFilterMessage(ref Message m)
            {
                if ((m.Msg == 0x312) && (m.HWnd == IntPtr.Zero))
                {
                    Action action;
                    if (this.HotKeyActionMap == null)
                    {
                        this.RecreateHotKeyMap();
                    }
                    if (this.HotKeyActionMap.TryGetValue((int) m.LParam, out action))
                    {
                        action.Execute();
                        return true;
                    }
                }
                return false;
            }

            private void RecreateHotKeyMap()
            {
                this.HotKeyActionMap = new Dictionary<int, Action>();
                foreach (Action action in this.FActions)
                {
                    if (action.Shortcuts != null)
                    {
                        foreach (Keys keys in action.Shortcuts)
                        {
                            MOD mod = WindowsWrapper.ShortcutKeysToModifiers(keys);
                            this.HotKeyActionMap.Add((int) (mod | ((MOD) (((int) (keys & 0xffff)) << 0x10))), action);
                        }
                    }
                }
            }

            public void Register()
            {
                if (!this.Registered)
                {
                    Application.AddMessageFilter(this);
                    this.Registered = true;
                }
            }

            public void ResetHotKeyMap()
            {
                this.HotKeyActionMap = null;
            }

            public void Unregister()
            {
                if (this.Registered)
                {
                    Application.RemoveMessageFilter(this);
                    this.Registered = false;
                }
            }
        }
    }
}

