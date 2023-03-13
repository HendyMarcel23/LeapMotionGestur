using UnityEngine;
using System.Runtime.InteropServices;


namespace Leap.Unity {


    /// <summary>
    /// Use this component on a Game Object to allow it to be manipulated by a pinch gesture.  The component
    /// allows rotation, translation, and scale of the object (RTS).
    /// </summary>
    public class LeapAction : MonoBehaviour {
        //Variable untuk Pinch
        /*
        [SerializeField]
        private PinchDetector _pinchDetectorL;
       
        [SerializeField]
        private PinchDetector _pinchDetectorR;
        */
        private GestureDetection gestureDetect;

        //Variable untuk AttachmentDetection
        [SerializeField]
        private Transform _palmL;

        [SerializeField]
        private Transform _palmR;

        [SerializeField]
        private Transform _thumbTipL;

        [SerializeField]
        private Transform _thumbTipR;

        [SerializeField]
        private Transform _indexTipL;

        [SerializeField]
        private Transform _indexTipR;

        [SerializeField]
        private Transform _middleTipL;

        [SerializeField]
        private Transform _middleTipR;

        [SerializeField]
        private Transform _pinkyTipL;

        [SerializeField]
        private Transform _pinkyTipR;

        [SerializeField]
        private GameObject _leftHand;

        [SerializeField]
        private GameObject _rightHand;

        private float batasRotasiUp = 0.94f;
        private float batasRotasiDown = 0.1f;
        private float batasRotasiFront = -0.6f;
        private float batasDistanceThumbTipAndPinkyTip = 0.05f;
        private float batasDistanceIndexTipAndMiddleTip = 0.05f;
        private float batasDistanceIndexTipAndThumbTip = 2.00f;
        private float batasDistanceMiddleTipAndThumbTip = 2.00f;
        private float batasDistanceMiddleTipAndIndexTip = 2.00f;
        private float batasDistanceForPinch = 0.03f;

        public int X { get; set; } // Fingertip X position in pixels
        public int Y { get; set; } // Fingertip Y position in pixels

        //Variabel Untuk Interpretasi Aksi
        private ActionInterpreter action;

        void Start()
        {
            Debug.Log("Hello From Leap Action.. ^_^");
            gestureDetect = new GestureDetection();
            action = new ActionInterpreter();
        }

        void FixedUpdate()
        {

            //Vector2 cursorPos = new Vector2();
            //GetCursorPos(out cursorPos);

            //Debug.Log(_indexTipR.leapvector.ToVector3(X,Y));
            if (isPinchKiri())
            {
                //Debug.Log("Pinch Kiri Kanan" );
                //action.performLeftClick();
                //action.performLeftClick();
            }
            else
            {
                //action.releaseLeftPinch();
            }
            
            if (isPinchKanan())
            {
                //action.performRightPinch(_palmR.transform.position);
                //action.performMoveCursor(_palmR.transform.position);
            }
            else
            {
                //action.releaseRightPinch();
            }

            /*
             isindexRight & middleRight meet

             isThumb Right & index Right active
            
             */

            if (_leftHand.activeSelf)
            {
                //Debug.Log("Rotation LEFT.x=" + _palmL.rotation.x + " | LEFT.y=" + _palmL.rotation.y + " | LEFT.z=" + _palmL.rotation.z);
                if (isTelapakKiriKeDepan())
                {
                    action.performZooming(_palmR.transform.position);
                }
                if (isTelapakKiriKeAtas())
                {
                    //action.performScrolling(_thumbTipR.transform.position);
                }
                if (gestureDetect.isLeftThumbFingerExtended())
                {
                    
                }
                

                //float dist = Vector3.Distance(_palmL.position, _thumbTipL.position);
                //Debug.Log("Jarak Jempol-Telapak = " + dist);
                /*if (isThumbTipLeftAndPinkyTipLeftMeet())
                {
                    action.performUndo();
                }
                
                 isTelapakKiriAtas p
                 isTelapakKiriAtas & indexKiri active p
                 isTelapakKiriBawah p
                 
                 lefthand not active bawah
                 isTelapakKiriAtas & thumb active
                 
                isTelapakKiriDepan p
                isTelapakKiriDepan not active
                isTelapakKiriAtas not active

                isTelapakKiriBawah & index active
                isTelapakKiriDepan & thumb index middle active
                isTelapakKiriAtas & thumb index active
                isTelapakKiriBawah & thumb active

                isTelapakKiriBawah not active

                
                isTelapakKiriBawah & thumb not active

                 
                 */

            }

            if (_rightHand.activeSelf)
            {
                if (isTelapakKananKeAtas())
                {
                    //action.performAltTab();
                }
                if (isTelapakKananKeDepan())
                {
                    //action.performRightClick();
                }

                if (isIndexTipRightAndMiddleTipRightMeet())
                {
                    //action.performMoveCursor(_palmR.transform.position);
                }

                /*if (isThumbTipRightAndPinkyTipRightMeet())
                {
                    action.performRedo();
                }
                
                 
                 
                 
                 
                 
                 
                 */

            }

            else if (isHandIdle())
            {
                action.performIdle();
            }
        }


        #region Definisi Aksi Tangan

        private bool isPinchKiri()
        {
            /*
            if (_pinchDetectorL != null && _pinchDetectorL.IsActive)
            {
                return true;
            }
            return false;
            */
            if (_leftHand.activeSelf == false)
            {
                return false;
            }
            return (Vector3.Distance(_indexTipL.position, _thumbTipL.position) < batasDistanceForPinch);
        }

        private bool isPinchKanan()
        {
            /*
            if (_pinchDetectorR != null && _pinchDetectorR.IsActive)
            {
                return true;
            }
            return false;
            */
            return (Vector3.Distance(_indexTipR.position, _thumbTipR.position) < batasDistanceForPinch);
        }

        private bool isTelapakKananKeAtas()
        {
            return (_palmR.rotation.z > batasRotasiUp);
        }

        private bool isTelapakKananKeBawah()
        {
            return ((_palmR.rotation.z < batasRotasiDown)&& (_palmR.rotation.z > -batasRotasiDown) && (_palmR.rotation.x < batasRotasiDown) && (_palmR.rotation.x > -3f * batasRotasiDown));
            //return true;
        }

        private bool isTelapakKiriKeAtas()
        {
            return (_palmL.rotation.z > batasRotasiUp);
        }

        private bool isTelapakKiriKeBawah()
        {
            return ((_palmL.rotation.z < batasRotasiDown) && (_palmL.rotation.z > -batasRotasiDown)&& (_palmL.rotation.x < batasRotasiDown) && (_palmL.rotation.x > -3f * batasRotasiDown));
            //return true;
        }

        private bool isTelapakKananKeDepan()
        {
            if (isPinchKanan())
            {
                return false;
            }
            return (_palmR.rotation.x < batasRotasiFront);
        }

        private bool isTelapakKiriKeDepan()
        {
            if (isPinchKiri())
            {
                return false;
            }
            return (_palmL.rotation.x < batasRotasiFront);
        }

        private bool isPalmLeftUpAndThumbLeftActive()
        {
            if (isPinchKiri())
            {
                return false;
            }
            if (isTelapakKiriKeAtas())
            {
                return true;
            }
            if (isTelapakKiriKeDepan())
            {
                return false;
            }
            if (isPinchKanan())
            {
                return false;
            }


            return (_palmL.rotation.z > batasRotasiUp && Vector3.Distance(_thumbTipL.position, _middleTipL.position) > 5f * batasDistanceMiddleTipAndThumbTip);
        }

        private bool isPalmLeftGripDown()
        {
            if (isPinchKiri())
            {
                return false;
            }
            if (isTelapakKiriKeAtas())
            {
                return false;
            }
            if (isTelapakKiriKeDepan())
            {
                return false;
            }

            return ((_palmL.rotation.z < batasRotasiDown) && (_palmL.rotation.z > -batasRotasiDown) && (_palmL.rotation.x < batasRotasiDown) && (_palmL.rotation.x > -3f * batasRotasiDown));
        }

        private bool isPalmLeftGripDownAndIndexLeftActive()
        {
            if (isPinchKiri())
            {
                return false;
            }
            if (isTelapakKiriKeAtas())
            {
                return false;
            }
            if (isTelapakKiriKeDepan())
            {
                return false;
            }

            return ((_palmL.rotation.z < batasRotasiDown) && (_palmL.rotation.z > -batasRotasiDown) && (_palmL.rotation.x < batasRotasiDown) && (_palmL.rotation.x > -3f * batasRotasiDown) && Vector3.Distance(_middleTipR.position, _indexTipR.position) > batasDistanceMiddleTipAndIndexTip);
        }

        private bool isPalmLeftGripDownAndTHumbLeftActive()
        {
            if (isPinchKiri())
            {
                return false;
            }
            if (isTelapakKiriKeAtas())
            {
                return false;
            }
            if (isTelapakKiriKeDepan())
            {
                return false;
            }

            return ((_palmL.rotation.z < batasRotasiDown) && (_palmL.rotation.z > -batasRotasiDown) && (_palmL.rotation.x < batasRotasiDown) && (_palmL.rotation.x > -3f * batasRotasiDown) && gestureDetect.isLeftThumbFingerExtended());
        }

        private bool isIndexTipLeftAndPalmLeftUp()
        {
            if (isPinchKiri())
            {
                return false;
            }
            if (isTelapakKiriKeAtas())
            {
                return true;
            }
            if (isTelapakKiriKeDepan())
            {
                return false;
            }

            return (gestureDetect.isLeftIndexFingerExtended());
        }

        private bool isThumbTipLeftIndexTipLeftAndPalmLeftUp()
        {
            if (isPinchKiri())
            {
                return false;
            }
            if (isTelapakKiriKeAtas())
            {
                return true;
            }
            if (isTelapakKiriKeDepan())
            {
                return false;
            }

            return (gestureDetect.isLeftThumbFingerExtended() && gestureDetect.isLeftIndexFingerExtended());
        }

        private bool isThumbTipLeftIndexTipLeftMiddleTipLeftAndPalmLeftFront()
        {
            if (isPinchKiri())
            {
                return false;
            }
            if (isTelapakKiriKeAtas())
            {
                return false;
            }
            if (isTelapakKiriKeDepan())
            {
                return true;
            }

            return (gestureDetect.isLeftThumbFingerExtended() && gestureDetect.isLeftIndexFingerExtended() && gestureDetect.isLeftMiddleFingerExtended());
        }

        private bool isIndexTipLeftMiddleTipLeftRingTipLeftPinkyTipLeftAndPalmLeftDown()
        {
            if (isPinchKiri())
            {
                return false;
            }
            if (isTelapakKiriKeAtas())
            {
                return false;
            }
            if (isTelapakKiriKeDepan())
            {
                return false;
            }

            return (gestureDetect.isLeftIndexFingerExtended() && gestureDetect.isLeftMiddleFingerExtended() && gestureDetect.isLeftRingFingerExtended() && gestureDetect.isLeftPinkyFingerExtended());
        }

        private bool isIndexTipLeftAndMiddleTipLeftMeet()
        {
            if (isPinchKiri())
            {
                return false;
            }
            if (isTelapakKiriKeAtas())
            {
                return false;
            }
            if (isTelapakKiriKeDepan())
            {
                return false;
            }
            if (isPinchKanan())
            {
                return false;
            }

            return (Vector3.Distance(_middleTipL.position, _indexTipL.position) < batasDistanceIndexTipAndMiddleTip);
        }

        private bool isThumbTipRightAndIndexTipRightActive()
        {
            if (isPinchKiri())
            {
                return false;
            }
            if (isTelapakKananKeAtas())
            {
                return false;
            }
            if (isTelapakKananKeDepan())
            {
                return false;
            }
            if (isPinchKanan())
            {
                return false;
            }

            return (Vector3.Distance(_thumbTipR.position, _indexTipR.position) > batasDistanceIndexTipAndThumbTip);
        }

        private bool isIndexTipRightAndMiddleTipRightMeet()
        {
            if (isPinchKiri())
            {
                return false;
            }
            if (isTelapakKananKeAtas())
            {
                return false;
            }
            if (isTelapakKananKeDepan())
            {
                return false;
            }
            if (isPinchKanan())
            {
                return false;
            }

            return (Vector3.Distance(_middleTipR.position, _indexTipR.position) < batasDistanceIndexTipAndMiddleTip);
        }

        private bool isThumbTipLeftAndPinkyTipLeftMeet()
        {
            if (isPinchKiri())
            {
                return false;
            }
            if (isTelapakKiriKeAtas())
            {
                return false;
            }
            if (isTelapakKiriKeDepan())
            {
                return false;
            }
            if (isPinchKanan())
            {
                return false;
            }

            return (Vector3.Distance(_pinkyTipL.position, _thumbTipL.position) < batasDistanceThumbTipAndPinkyTip);
        }

        private bool isThumbTipRightAndPinkyTipRightMeet()
        {
            if (isPinchKanan())
            {
                return false;
            }
            if (isTelapakKananKeAtas())
            {
                return false;
            }
            if (isTelapakKananKeDepan())
            {
                return false;
            }
            if (isPinchKiri())
            {
                return false;
            }
            return (Vector3.Distance(_pinkyTipR.position, _thumbTipR.position) < batasDistanceThumbTipAndPinkyTip);
        }

        private bool isHandIdle()
        {
            //if ((_rightHand.activeSelf == false) && (_leftHand.activeSelf == false))
            if (_leftHand.activeSelf == false)
            {
                return true;
            }
            if (isPinchKiri() || isPinchKanan() || isTelapakKiriKeDepan() || isTelapakKiriKeAtas() || isTelapakKananKeDepan() || isTelapakKananKeAtas() || isThumbTipLeftAndPinkyTipLeftMeet() || isThumbTipRightAndPinkyTipRightMeet())
            {
                return false;
            }
            return true;
        }

        #endregion

    }
}
