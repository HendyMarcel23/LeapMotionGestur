using UnityEngine;
using System.Runtime.InteropServices;


namespace Leap.Unity {


    /// <summary>
    /// Use this component on a Game Object to allow it to be manipulated by a pinch gesture.  The component
    /// allows rotation, translation, and scale of the object (RTS).
    /// </summary>
    public class PalmHandler : MonoBehaviour {

        [SerializeField]
        private Transform _palmL;
        
        [SerializeField]
        private Transform _palmR;

        [SerializeField]
        private GameObject _leftHand;

        [SerializeField]
        private GameObject _rightHand;

        private float batasRotasiUp = 0.94f;


        void Start()
        {
            Debug.Log("Hello Palm Handler..");
        }

        void FixedUpdate()
        {
            if (_leftHand.activeSelf)
            {
                //Debug.Log("Position = " + _palmL.position + " | Rotation LEFT = " + _palmL.rotation.z);
                if (_palmL.rotation.z > batasRotasiUp)
                {
                    Debug.Log("Telapak Kiri Ke Atas");
                }
            }

            if (_rightHand.activeSelf)
            {
                if (_palmR.rotation.z > batasRotasiUp)
                {
                    Debug.Log("Telapak Kanan Ke Atas");
                }
            }

        }
        
    }
}
