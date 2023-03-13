using UnityEngine;
using System.Runtime.InteropServices;


namespace Leap.Unity {


    /// <summary>
    /// Use this component on a Game Object to allow it to be manipulated by a pinch gesture.  The component
    /// allows rotation, translation, and scale of the object (RTS).
    /// </summary>
    public class ActionInterpreter : MonoBehaviour {

        public enum HandStatus
        {
            Idle,
            MoveCursor, //Pinch Right Only
            LeftClick, //Pinch Left && Pinch Right
            RightClick, //Telapak Kanan Ke Depan
            Zooming, //Telapak Kiri ke Atas + Pinch Right Geser ke atas/bawah
            Scrolling, //Telapak Kiri ke Depan + Pinch Right Geser ke atas/bawah
            Undo, //Thumb Tip Left bertemu Palm Left
            Redo, //Thumb Tip Right bertemu Palm Right
            AltTab //Telapak Kanan ke Atas
        }

        private HandStatus state;
        private HandStatus lastState;

        private Vector3 lastHandPosition;
        private float xMovement;
        private float yMovement;
        private float zMovement;
        private float movementScale;

        private bool isRightPinchReleased;
        private bool isLeftPinchReleased;

        private MouseControl mc;

        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out Vector2 pos);

        
        public ActionInterpreter()
        {
            state = HandStatus.Idle;
            lastState = HandStatus.Idle;
            lastHandPosition = Vector3.zero;
            xMovement = lastHandPosition.x;
            yMovement = lastHandPosition.y;
            zMovement = lastHandPosition.z;
            movementScale = 5000;
            isRightPinchReleased = true;
            isLeftPinchReleased = true;
            mc = new MouseControl();
        }

        public void performIdle()
        {
            lastState = state;
            state = HandStatus.Idle;
            showStatus();
        }

        public void performMoveCursor(Vector3 pinchLocation)
        {
            calculateMovement(pinchLocation);
            if (state == HandStatus.Scrolling)
            {
                int scrollValue = (int)yMovement/10;
                mc.cmdScroll(-scrollValue);

                int horizontalScrollValue = (int)xMovement / 10;
                mc.cmdHorizontalScroll(-horizontalScrollValue);
            }

            else
            {
                lastState = state;
                state = HandStatus.MoveCursor;
                moveCursor();
                showStatus();
            }
        }

        private void calculateMovement(Vector3 newPosition)
        {
            Vector3 movement = newPosition - lastHandPosition;
            lastHandPosition = newPosition;
            
            xMovement = movement.x * movementScale;
            yMovement = movement.y * movementScale;
            zMovement = movement.z * movementScale;
        }

        private void moveCursor()
        {
            mc.changeMousePosition((int)xMovement, (int)yMovement*-1);
        }

        public void performLeftClick()
        {
            if (isLeftPinchReleased) // baru klik
            {
                mc.leftMouseDownClick();
               
            }
            isLeftPinchReleased = false;
            lastState = state;
            state = HandStatus.LeftClick;
            showStatus();
        }

        public void performRightClick()
        {
            if (lastState == HandStatus.RightClick)
            {
                mc.rightMouseUpClick();
            }
            else
            {
                
            }
            lastState = state;
            state = HandStatus.RightClick;
            showStatus();
        }

        public void performZooming(Vector3 palmRLocation)
        {
            //Debug.Log("action Zooming = " + palmRLocation);

            calculateMovement(palmRLocation);
            int zoomValue = (int)xMovement / 10;
            mc.cmdScroll(zoomValue);

            lastState = state;
            mc.pressControlKey();
            state = HandStatus.Zooming;
            showStatus();
        }

        public void performScrollingVer(Vector3 palmRLocation)
        {
            //Debug.Log("action Scrolling = " + pinchLocation);
            mc.releaseControlKey();
            calculateMovement(palmRLocation);
            int scrollValue = (int)yMovement / 20;
            mc.cmdScroll(scrollValue);

            lastState = state;
            state = HandStatus.Scrolling;
            showStatus();
        }

        public void performScrollingHor(Vector3 palmRLocation)
        {
            //Debug.Log("action Scrolling = " + pinchLocation);
            mc.releaseControlKey();
            calculateMovement(palmRLocation);
            int horizontalScrollValue = (int)xMovement / 10;
            mc.cmdHorizontalScroll(horizontalScrollValue);

            lastState = state;
            state = HandStatus.Scrolling;
            showStatus();
        }

        public void performUndo()
        {
            lastState = state;
            if (lastState != HandStatus.Undo)
            {
                mc.cmdUndo();
            }
            state = HandStatus.Undo;
            showStatus();
        }

        public void performRedo()
        {
            lastState = state;
            if (lastState != HandStatus.Redo)
            {
                mc.cmdRedo();
            }
            state = HandStatus.Redo;
            showStatus();
        }

        public void performAltTab()
        {
            lastState = state;
            if (lastState == HandStatus.AltTab)
            {
                mc.cmdAltTab();
            }
            state = HandStatus.AltTab;
            showStatus();
        }

        public void releaseRightPinch()
        {
            isRightPinchReleased = true;
        }

        public void performRightPinch(Vector3 pinchLocation)
        {
            if (isRightPinchReleased)
            {
                lastHandPosition = pinchLocation;
            }
            isRightPinchReleased = false;
        }

        public void releaseLeftPinch()
        {
            if (isLeftPinchReleased == false)
            {
                mc.leftMouseUpClick();
                mc.releaseAltKeyPress();
                mc.releaseControlKey();
            }
            isLeftPinchReleased = true;
        }

        

        private void showStatus()
        {
            if (state == HandStatus.MoveCursor)
            {
                Vector2 cursorPos = new Vector2();
                Vector3 mouse = Input.mousePosition;
                
                //Debug.Log("Movement x=" + (float)xMovement + " |y=" + (float)yMovement + " |z=" + (float)zMovement);
                //Debug.Log("Cursor x=" + mouse.x + " |y=" + mouse.y);
                //SetCursorPos((int)mouse.x, (int)mouse.y);
            }
            if(lastState != state)
            {
                switch (state)
                {
                    case HandStatus.Idle:
                        Debug.Log("Status Idle");
                        break;
                    case HandStatus.MoveCursor:
                        Debug.Log("Status Move Cursor");
                        break;
                    case HandStatus.LeftClick:
                        Debug.Log("Status Left Click");
                        break;
                    case HandStatus.RightClick:
                        Debug.Log("Status Right Click");
                        break;
                    case HandStatus.Zooming:
                        Debug.Log("Status Zooming");
                        break;
                    case HandStatus.Scrolling:
                        Debug.Log("Status Scrolling");
                        break;
                    case HandStatus.Undo:
                        Debug.Log("Status Undo");
                        break;
                    case HandStatus.Redo:
                        Debug.Log("Status Redo");
                        break;
                    case HandStatus.AltTab:
                        Debug.Log("Status AltTab");
                        break;
                    default:
                        Debug.Log("Status Default - Unknown");
                        break;

                }
            }
        }
    }
}
