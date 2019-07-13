using System;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace VisualDump.Helpers
{
    internal static class WindowsInterop
    {
        public static class User32
        {
            public delegate bool Win32Callback(IntPtr WindowHandle, IntPtr Param);

            [DllImport("user32.dll")]
            public static extern uint GetWindowThreadProcessId(IntPtr WindowHandle, out uint ProcessID);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool EnumChildWindows(IntPtr ParentHandle, Win32Callback Callback, IntPtr Param);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern int GetWindowText(IntPtr WindowHandle, StringBuilder String, int MaxCount);

            [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
            public static extern int GetWindowTextLength(IntPtr WindowHandle);
        }

        public static IEnumerable<IntPtr> EnumerateWindows(this Process Process)
        {
            foreach (IntPtr windowHandle in GetChildWindows(IntPtr.Zero))
            {
                User32.GetWindowThreadProcessId(windowHandle, out uint processID);
                if (processID == Process.Id)
                    yield return windowHandle;
            }
        }

        private static List<IntPtr> GetChildWindows(IntPtr Parent)
        {
            List<IntPtr> result = new List<IntPtr>();
            GCHandle listHandle = GCHandle.Alloc(result);
            try
            {
                User32.EnumChildWindows(Parent, EnumWindow, GCHandle.ToIntPtr(listHandle));
            } 
            finally
            {
                if (listHandle.IsAllocated)
                    listHandle.Free();
            }
            return result;
        }

        private static bool EnumWindow(IntPtr Handle, IntPtr ListPointer)
        {
            if (GCHandle.FromIntPtr(ListPointer).Target is List<IntPtr> list)
                list.Add(Handle);
            else
                throw new InvalidCastException();
            return true;
        }

        public static string GetWindowText(IntPtr WindowHandle)
        {
            StringBuilder sb = new StringBuilder(User32.GetWindowTextLength(WindowHandle) + 1);
            User32.GetWindowText(WindowHandle, sb, sb.Capacity);
            return sb.ToString();
        }
    }
}
