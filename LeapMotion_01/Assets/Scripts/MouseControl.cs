using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using WindowsInput;

namespace Leap.Unity {


    /// <summary>
    /// Use this component on a Game Object to allow it to be manipulated by a pinch gesture.  The component
    /// allows rotation, translation, and scale of the object (RTS).
    /// </summary>
    public class MouseControl : MonoBehaviour {

        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(out MousePosition lpMousePosition);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(long dwFlags, long dx, long dy, long cButtons, long dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        private InputSimulator s;


        public void leftMouseDownClick()
        {
            //mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            s.Mouse.LeftButtonDown();
        }

        public void leftMouseUpClick()
        {
            //mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            s.Mouse.LeftButtonUp();
        }

        public void rightMouseClick()
        {
            //mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
            s.Mouse.RightButtonDown();
        }

        public void rightMouseUpClick()
        {
            //mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
            s.Mouse.RightButtonUp();
        }

        public void cmdAltTab()
        {
             //s.Keyboard.TextEntry("Hello sim !");
            //s.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_A);
            //s.Keyboard.ModifiedKeyStroke(WindowsInput.Native.VirtualKeyCode.MENU, WindowsInput.Native.VirtualKeyCode.TAB);
            s.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.MENU);
            s.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.TAB);
        }
        public void AltKeyPress()
        {
            s.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.MENU);
        }
        public void releaseAltKeyPress()
        {
            s.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.MENU);
        }

        public void cmdUndo()
        {
            s.Keyboard.ModifiedKeyStroke(WindowsInput.Native.VirtualKeyCode.CONTROL, WindowsInput.Native.VirtualKeyCode.VK_Z);
        }
        public void cmdRedo()
        {
            s.Keyboard.ModifiedKeyStroke(WindowsInput.Native.VirtualKeyCode.CONTROL, WindowsInput.Native.VirtualKeyCode.VK_Y);
        }

        public void cmdScroll(int v)
        {
            s.Mouse.VerticalScroll(v);
        }

        public void cmdHorizontalScroll(int h)
        {
            s.Mouse.HorizontalScroll(h);
        }

        public void pressControlKey()
        {
            s.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.CONTROL);
        }
        public void releaseControlKey()
        {
            s.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.CONTROL);
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct MousePosition
        {
            public int x;
            public int y;
        }

        private MousePosition mp;

        public MouseControl()
        {
            s = new InputSimulator();
        }
        
        public void changeMousePosition(int xdmp, int ydmp)
        {
            GetCursorPos(out mp);

            SetCursorPos(mp.x + xdmp, mp.y + ydmp);
        }
    }
}
