using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;


namespace Leap.Unity {


    /// <summary>
    /// Use this component on a Game Object to allow it to be manipulated by a pinch gesture.  The component
    /// allows rotation, translation, and scale of the object (RTS).
    /// </summary>
    public class GestureInterpreter : MonoBehaviour {

        [SerializeField]
        public GestureDetection gestureDetector;
        private ActionInterpreter action;

        [SerializeField]
        private GameObject _leftHand;

        [SerializeField]
        private GameObject _rightHand;

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
        private Transform _ringTipL;

        [SerializeField]
        private Transform _ringTipR;

        [SerializeField]
        private Transform _pinkyTipL;

        [SerializeField]
        private Transform _pinkyTipR;
        // private posisi jari telunjuk kanan.
        // posisiTerakhir
        // posisiTerkini

        public static System.Drawing.Point Position { get; set; }

        public Button leftThumbButton;
        public Button leftIndexButton;
        public Button leftMiddleButton;
        public Button leftRingButton;
        public Button leftPinkyButton;

        public Button rightThumbButton;
        public Button rightIndexButton;
        public Button rightMiddleButton;
        public Button rightRingButton;
        public Button rightPinkyButton;

        public Button leftPalmButton;
        public Text leftPalmText;
        public Button rightPalmButton;
        public Text rightPalmText;

        public Text actionCommandText;

        private float batasRotasiUp = 0.94f;
        private float batasRotasiDown = 0.1f;
        private float batasRotasiFront = -0.51f;
        private float batasDistanceForPinch = 0.03f;

        private bool isMoveUp;
        private bool isMoveDown;



        public GestureInterpreter()
        {
            //posisiTerakhir = posisiTerkini
        }
        
        void Start()
        {
            Debug.Log("Hello From Leap Action.. ^_^");
            action = new ActionInterpreter();
        }

        void FixedUpdate()
        {
            //reset setiap update --> isMoveUp = isMoveDown = false;
            //posisiTerkini = ambil dari data leapmotion
            // if (posisiTerkini.z > posisiTerakhir.z then pergerakan ke atas
            // isMoveUp = true;
            // else isMoveDown = true;
            //posisiTerakhir = posisiTerkini

            cekFingerStatus();
            cekPalmDirection();
            interpreteCommand();

        }

        private void interpreteCommand()
        {
            bool anyAction = false;

            if (isPinchKiri() && isPinchKanan())
            {
                actionCommandText.text = "Both Hands Pinch";
                anyAction = true;
            }
            else if (isPinchKiri())
            {
                actionCommandText.text = "Left Group Retreat";
                anyAction = true;
            }
            //simulasi geser kanan
            else if (gestureDetector.XRightIndexNow - 0.05 > gestureDetector.XRightIndexThen)
            {
                //simulasi geser kanan atas
                if (gestureDetector.YRightIndexNow - 0.03 > gestureDetector.YRightIndexThen)
                {
                    actionCommandText.text = "Right Up";
                    anyAction = true;
                }
                //simulasi geser kanan bawah
                else if (gestureDetector.YRightIndexNow + 0.03 < gestureDetector.YRightIndexThen)
                {
                    actionCommandText.text = "Right Down";
                    anyAction = true;
                }
                else
                {
                    actionCommandText.text = "Right";
                    anyAction = true;
                }
            }
            //simulasi geser kiri
            else if (gestureDetector.XRightIndexNow + 0.05 < gestureDetector.XRightIndexThen)
            {
                //simulasi geser kiri atas
                if (gestureDetector.YRightIndexNow - 0.03 > gestureDetector.YRightIndexThen)
                {
                    actionCommandText.text = "Left Up";
                    anyAction = true;
                }
                //simulasi geser kiri bawah
                else if (gestureDetector.YRightIndexNow + 0.03 < gestureDetector.YRightIndexThen)
                {
                    actionCommandText.text = "Left Down";
                    anyAction = true;
                }
                else
                {
                    actionCommandText.text = "Left";
                    anyAction = true;
                }
            }
            //simulasi geser atas
            else if (gestureDetector.YRightIndexNow - 0.03 > gestureDetector.YRightIndexThen)
            {
                actionCommandText.text = "Up";
                anyAction = true;
            }
            //simulasi geser bawah
            else if (gestureDetector.YRightIndexNow + 0.03 < gestureDetector.YRightIndexThen)
            {
                actionCommandText.text = "Down";
                anyAction = true;
            }

            // simulasi zoom.
            else if (isTelapakKiriKeAtas() && isPinchKanan())
            {
                actionCommandText.text = "Zooming";
                if (isMoveDown)
                {
                    //lakukan aksi zoom out
                }
                else if (isMoveUp)
                {
                    //lakukan aksi zoom in
                }
                anyAction = true;
            }
            else if (isPinchKanan())
            {
                actionCommandText.text = "Right Hand Pinch";
                anyAction = true;
            }
           
            if (isTelapakKananKeAtas() && isTelapakKiriKeAtas())
            {
                actionCommandText.text = "Spread Drone";
                anyAction = true;
            }


            bool spidermanLeftGesture = false;
            bool spidermanRightGesture = false;
            if (gestureDetector.isRightThumbFingerExtended() && gestureDetector.isRightIndexFingerExtended() && gestureDetector.isRightPinkyFingerExtended()
                && !gestureDetector.isRightMiddleFingerExtended() && !gestureDetector.isRightRingFingerExtended()
                )
            {
                spidermanRightGesture = true;
                anyAction = true;
            }

            if (gestureDetector.isLeftThumbFingerExtended() && gestureDetector.isLeftIndexFingerExtended() && gestureDetector.isLeftPinkyFingerExtended()
                && !gestureDetector.isLeftMiddleFingerExtended() && !gestureDetector.isLeftRingFingerExtended()
                )
            {
                spidermanLeftGesture = true;
                anyAction = true;
            }

            if (spidermanLeftGesture && spidermanRightGesture)
            {
                actionCommandText.text = "Hold Position";
            }
            else if (spidermanLeftGesture)
            {
                actionCommandText.text = "Left Group Retreat";
            }
            else if (spidermanRightGesture)
            {
                actionCommandText.text = "Right Group Retreat";
            }

            if (!anyAction)
            {
                actionCommandText.text = "-";
            }
        }

        private void cekPalmDirection()
        {
            if (_leftHand.activeSelf)
            {
                if (isTelapakKiriKeAtas())
                {
                    leftPalmButton.GetComponent<UnityEngine.UI.Image>().color = Color.cyan;
                    leftPalmText.text = "Up";
                }
                else if (isTelapakKiriKeDepan())
                {
                    leftPalmButton.GetComponent<UnityEngine.UI.Image>().color = Color.yellow;
                    leftPalmText.text = "Front";
                }
                else
                {
                    leftPalmButton.GetComponent<UnityEngine.UI.Image>().color = Color.gray;
                    leftPalmText.text = "Idle";
                }
            }
            else
            {
                leftPalmButton.GetComponent<UnityEngine.UI.Image>().color = Color.red;
                leftPalmText.text = "-";
            }

            if (_rightHand.activeSelf)
            {
                if (isTelapakKananKeAtas())
                {
                    rightPalmButton.GetComponent<UnityEngine.UI.Image>().color = Color.cyan;
                    rightPalmText.text = "Up";
                }
                else if (isTelapakKananKeDepan())
                {
                    rightPalmButton.GetComponent<UnityEngine.UI.Image>().color = Color.yellow;
                    rightPalmText.text = "Front";
                }
                else
                {
                    rightPalmButton.GetComponent<UnityEngine.UI.Image>().color = Color.gray;
                    rightPalmText.text = "Idle";
                }
            }
            else
            {
                rightPalmButton.GetComponent<UnityEngine.UI.Image>().color = Color.red;
                rightPalmText.text = "-";
            }
        }

        #region deteksi gerakan tangan
        private bool isPinchKiri()
        {
            if (_leftHand.activeSelf == false)
            {
                return false;
            }
            return (Vector3.Distance(_indexTipL.position, _thumbTipL.position) < batasDistanceForPinch);
        }

        private bool isPinchKanan()
        {
            if (_rightHand.activeSelf == false)
            {
                return false;
            }
            return (Vector3.Distance(_indexTipR.position, _thumbTipR.position) < batasDistanceForPinch);
        }

        private bool isTelapakKananKeAtas()
        {
            return (_palmR.rotation.z > batasRotasiUp);
        }

        private bool isTelapakKananKeBawah()
        {
            return ((_palmR.rotation.z < batasRotasiDown) && (_palmR.rotation.z > -batasRotasiDown) && (_palmR.rotation.x < batasRotasiDown) && (_palmR.rotation.x > -3f * batasRotasiDown));
            //return true;
        }

        private bool isTelapakKiriKeAtas()
        {
            return (_palmL.rotation.z > batasRotasiUp);
        }

        private bool isTelapakKiriKeBawah()
        {
            return ((_palmL.rotation.z < batasRotasiDown) && (_palmL.rotation.z > -batasRotasiDown) && (_palmL.rotation.x < batasRotasiDown) && (_palmL.rotation.x > -3f * batasRotasiDown));
            //return true;
        }

        private bool isTelapakKananKeDepan()
        {
            return (_palmR.rotation.x < batasRotasiFront);
        }

        private bool isTelapakKiriKeDepan()
        {
            return (_palmL.rotation.x < batasRotasiFront);
        }
        #endregion

        private void cekFingerStatus()
        {
            //Cek Tangan Kiri
            if (gestureDetector.isLeftThumbFingerExtended())
            {
                leftThumbButton.GetComponent<UnityEngine.UI.Image>().color = Color.green;
                
            }
            else
            {
                leftThumbButton.GetComponent<UnityEngine.UI.Image>().color = Color.red;
            }
            if (gestureDetector.isLeftIndexFingerExtended())
            {
                leftIndexButton.GetComponent<UnityEngine.UI.Image>().color = Color.green;
            }
            else
            {
                leftIndexButton.GetComponent<UnityEngine.UI.Image>().color = Color.red;
            }
            if (gestureDetector.isLeftMiddleFingerExtended())
            {
                leftMiddleButton.GetComponent<UnityEngine.UI.Image>().color = Color.green;
            }
            else
            {
                leftMiddleButton.GetComponent<UnityEngine.UI.Image>().color = Color.red;
            }
            if (gestureDetector.isLeftRingFingerExtended())
            {
                leftRingButton.GetComponent<UnityEngine.UI.Image>().color = Color.green;
            }
            else
            {
                leftRingButton.GetComponent<UnityEngine.UI.Image>().color = Color.red;
            }
            if (gestureDetector.isLeftPinkyFingerExtended())
            {
                leftPinkyButton.GetComponent<UnityEngine.UI.Image>().color = Color.green;
            }
            else
            {
                leftPinkyButton.GetComponent<UnityEngine.UI.Image>().color = Color.red;
            }

            if (gestureDetector.isLeftIndexFingerExtended() && isTelapakKiriKeAtas())
            {
                leftIndexButton.GetComponent<UnityEngine.UI.Image>().color = Color.green;
                //action.performAltTab();
            }
            else
            {
                leftIndexButton.GetComponent<UnityEngine.UI.Image>().color = Color.red;
            }
            if (gestureDetector.isLeftIndexFingerExtended() && isTelapakKiriKeAtas())
            {
                //action.performRightClick();
            }
            else
            {
                
            }
            if (gestureDetector.isLeftThumbFingerExtended() && gestureDetector.isLeftIndexFingerExtended() && gestureDetector.isLeftMiddleFingerExtended() && gestureDetector.isLeftRingFingerExtended() && gestureDetector.isLeftPinkyFingerExtended() && isTelapakKiriKeAtas())
            {
                action.performRightClick();
            }
            else
            {

            }
            if (!gestureDetector.isLeftThumbFingerExtended() && !gestureDetector.isLeftIndexFingerExtended() && !gestureDetector.isLeftMiddleFingerExtended() && !gestureDetector.isLeftRingFingerExtended() && !gestureDetector.isLeftPinkyFingerExtended() && isTelapakKiriKeBawah() && !isTelapakKananKeBawah())
            {
                //action.performLeftClick();
            }
            else
            { 
            
            }
            if (gestureDetector.isLeftThumbFingerExtended() && isTelapakKiriKeAtas())
            {
                //action.performLeftClick();
            }
            else
            {

            }
            if (isTelapakKiriKeDepan() && isTelapakKananKeDepan())
            {
                //action.performZooming(_palmR.transform.position);
            }
            else
            {

            }
            if (!gestureDetector.isLeftThumbFingerExtended() && !gestureDetector.isLeftIndexFingerExtended() && !gestureDetector.isLeftMiddleFingerExtended() && !gestureDetector.isLeftRingFingerExtended() && !gestureDetector.isLeftPinkyFingerExtended() && isTelapakKiriKeDepan() && !gestureDetector.isRightThumbFingerExtended() && !gestureDetector.isRightIndexFingerExtended() && !gestureDetector.isRightMiddleFingerExtended() && !gestureDetector.isRightRingFingerExtended() && !gestureDetector.isRightPinkyFingerExtended() && isTelapakKananKeDepan())
            {
                //action.performZooming(_palmR.transform.position);
            }
            else
            {

            }
            if (!gestureDetector.isLeftThumbFingerExtended() && !gestureDetector.isLeftIndexFingerExtended() && !gestureDetector.isLeftMiddleFingerExtended() && !gestureDetector.isLeftRingFingerExtended() && !gestureDetector.isLeftPinkyFingerExtended() && isTelapakKiriKeAtas())
            {
                //action.performZooming(_palmR.transform.position);
            }
            else
            {

            }
            if (gestureDetector.isLeftIndexFingerExtended() && isTelapakKiriKeBawah() && isTelapakKananKeAtas())
            {
                //rotate
            }
            else
            {

            }
            if (gestureDetector.isLeftThumbFingerExtended() && gestureDetector.isLeftIndexFingerExtended() && gestureDetector.isLeftMiddleFingerExtended() && isTelapakKiriKeDepan())
            {
                //rotate
            }
            else
            {

            }
            if (!gestureDetector.isLeftThumbFingerExtended() && !gestureDetector.isLeftIndexFingerExtended() && !gestureDetector.isLeftMiddleFingerExtended() && !gestureDetector.isLeftRingFingerExtended() && !gestureDetector.isLeftPinkyFingerExtended() && isTelapakKiriKeBawah() && isTelapakKananKeBawah())
            {
                action.performScrollingVer(_palmR.transform.position);
            }
            else
            {

            }
            if (gestureDetector.isLeftThumbFingerExtended() && !gestureDetector.isLeftIndexFingerExtended() && !gestureDetector.isLeftMiddleFingerExtended() && !gestureDetector.isLeftRingFingerExtended() && !gestureDetector.isLeftPinkyFingerExtended() && isTelapakKiriKeBawah() && isPinchKanan())
            {
                //action.performScrollingVer(_palmR.transform.position);
            }
            else
            {

            }
            if (gestureDetector.isLeftThumbFingerExtended() && gestureDetector.isLeftIndexFingerExtended() && !gestureDetector.isLeftMiddleFingerExtended() && !gestureDetector.isLeftRingFingerExtended() && !gestureDetector.isLeftPinkyFingerExtended() && isTelapakKiriKeAtas() && isTelapakKananKeBawah())
            {
                //action.performScrollingVer(_palmR.transform.position);
            }
            else
            {

            }
            if (!gestureDetector.isLeftThumbFingerExtended() && !gestureDetector.isLeftIndexFingerExtended() && !gestureDetector.isLeftMiddleFingerExtended() && !gestureDetector.isLeftRingFingerExtended() && !gestureDetector.isLeftPinkyFingerExtended() && isTelapakKiriKeBawah() && !gestureDetector.isRightThumbFingerExtended() && !gestureDetector.isRightIndexFingerExtended() && !gestureDetector.isRightMiddleFingerExtended() && !gestureDetector.isRightRingFingerExtended() && !gestureDetector.isRightPinkyFingerExtended() && isTelapakKananKeBawah())
            {
                action.performScrollingHor(_palmR.transform.position);
            }
            else
            {

            }
            if (gestureDetector.isLeftIndexFingerExtended() && gestureDetector.isLeftMiddleFingerExtended() && gestureDetector.isLeftRingFingerExtended() && gestureDetector.isLeftPinkyFingerExtended() && isTelapakKiriKeBawah())
            {
                //action.performScrollingHor(_palmR.transform.position);
            }
            else
            {

            }
            if (gestureDetector.isLeftThumbFingerExtended() && gestureDetector.isLeftIndexFingerExtended() && !gestureDetector.isLeftMiddleFingerExtended() && !gestureDetector.isLeftRingFingerExtended() && !gestureDetector.isLeftPinkyFingerExtended() && isTelapakKiriKeAtas() && !gestureDetector.isRightThumbFingerExtended() && !gestureDetector.isRightIndexFingerExtended() && !gestureDetector.isRightMiddleFingerExtended() && !gestureDetector.isRightRingFingerExtended() && !gestureDetector.isRightPinkyFingerExtended() && isTelapakKananKeBawah())
            {
                //action.performScrollingHor(_palmR.transform.position);
            }
            else
            {

            }

            // Cek Tangan Kanan
            if (gestureDetector.isRightThumbFingerExtended())
            {
                rightThumbButton.GetComponent<UnityEngine.UI.Image>().color = Color.green;
            }
            else
            {
                rightThumbButton.GetComponent<UnityEngine.UI.Image>().color = Color.red;
            }
            if (gestureDetector.isRightIndexFingerExtended())
            {
                rightIndexButton.GetComponent<UnityEngine.UI.Image>().color = Color.green;
            }
            else
            {
                rightIndexButton.GetComponent<UnityEngine.UI.Image>().color = Color.red;
            }
            if (gestureDetector.isRightMiddleFingerExtended())
            {
                rightMiddleButton.GetComponent<UnityEngine.UI.Image>().color = Color.green;
            }
            else
            {
                rightMiddleButton.GetComponent<UnityEngine.UI.Image>().color = Color.red;
            }
            if (gestureDetector.isRightRingFingerExtended())
            {
                rightRingButton.GetComponent<UnityEngine.UI.Image>().color = Color.green;
            }
            else
            {
                rightRingButton.GetComponent<UnityEngine.UI.Image>().color = Color.red;
            }
            if (gestureDetector.isRightPinkyFingerExtended())
            {
                rightPinkyButton.GetComponent<UnityEngine.UI.Image>().color = Color.green;
            }
            else
            {
                rightPinkyButton.GetComponent<UnityEngine.UI.Image>().color = Color.red;
            }

            if (gestureDetector.isRightThumbFingerExtended() && gestureDetector.isRightIndexFingerExtended())
            {
                //action.performMoveCursor(_palmR.transform.position);
            }
            else
            {
                
            }
            if (gestureDetector.isRightThumbFingerExtended() && gestureDetector.isRightIndexFingerExtended() && gestureDetector.isRightMiddleFingerExtended() && gestureDetector.isRightRingFingerExtended() && gestureDetector.isRightPinkyFingerExtended() && isTelapakKananKeAtas())
            {
                action.performAltTab();
            }
            else
            {

            }


            if (isPinchKiri())
            {
                //Debug.Log("Pinch Kiri Kanan" );
                //action.performLeftClick();
                action.performLeftClick();
            }
            else
            {
                action.releaseLeftPinch();
            }

            if (isPinchKanan())
            {
                action.performRightPinch(_palmR.transform.position);
                action.performMoveCursor(_palmR.transform.position);
            }
            else
            {
                action.releaseRightPinch();
            }

        }
    }
}
