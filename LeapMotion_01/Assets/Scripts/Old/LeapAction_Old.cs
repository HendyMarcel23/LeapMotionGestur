using UnityEngine;
using System.Runtime.InteropServices;


namespace Leap.Unity {


    /// <summary>
    /// Use this component on a Game Object to allow it to be manipulated by a pinch gesture.  The component
    /// allows rotation, translation, and scale of the object (RTS).
    /// </summary>
    public class LeapAction_Old : MonoBehaviour {

        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out Vector2 pos);

        [SerializeField]
        private PinchDetector _pinchDetectorL;
        public PinchDetector PinchDetectorL
        {
            get
            {
                return _pinchDetectorL;
            }
            set
            {
                _pinchDetectorL = value;
            }
        }

        [SerializeField]
        private PinchDetector _pinchDetectorR;
        public PinchDetector PinchDetectorR
        {
            get
            {
                return _pinchDetectorR;
            }
            set
            {
                _pinchDetectorR = value;
            }
        }

      

        void Start()
        {
            Debug.Log("Hello From Leap Action.. ^_^");
        }

        void FixedUpdate()
        {

            Vector2 cursorPos = new Vector2();
            GetCursorPos(out cursorPos);

            bool didUpdate = false;
            if (_pinchDetectorL != null)
                //didUpdate |= _pinchDetectorA.DidChangeFromLastFrame;
            if (_pinchDetectorR != null)
                //didUpdate |= _pinchDetectorB.DidChangeFromLastFrame;

            if (didUpdate)
            {
                //transform.SetParent(null, true);
            }

            if (_pinchDetectorL != null && _pinchDetectorL.IsActive &&
                _pinchDetectorR != null && _pinchDetectorR.IsActive)
            {
                //transformDoubleAnchor();
                //Debug.Log("Klik Kiri Kanan" );
            }
            else if (_pinchDetectorL != null && _pinchDetectorL.IsActive)
            {
                //transformSingleAnchor(_pinchDetectorA);
                //Debug.Log("Klik Kiri" + _pinchDetectorL.Position);
                
            }
            else if (_pinchDetectorR != null && _pinchDetectorR.IsActive)
            {
                //transformSingleAnchor(_pinchDetectorB);
                //Debug.Log("Klik Kanan");
                //SetCursorPos(Mathf.RoundToInt(cursorPos.x + 1), Mathf.RoundToInt(cursorPos.y + 1));
                //Debug.Log("Klik Kanan" + _pinchDetectorR.Position);

            }

          

            if (didUpdate)
            {
                //transform.SetParent(_anchor, true);
            }
        }

       

        
    }
}
