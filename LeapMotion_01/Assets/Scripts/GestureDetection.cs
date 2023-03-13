/******************************************************************************
 * Copyright (C) Ultraleap, Inc. 2011-2020.                                   *
 *                                                                            *
 * Use subject to the terms of the Apache License 2.0 available at            *
 * http://www.apache.org/licenses/LICENSE-2.0, or another agreement           *
 * between Ultraleap and you, your company or other organization.             *
 ******************************************************************************/

using UnityEngine;
using System.Collections;
using System;
using Leap.Unity.Attributes;

namespace Leap.Unity {

  /**
   * Detects when specified fingers are in an extended or non-extended state.
   * 
   * You can specify whether each finger is extended, not extended, or in either state.
   * This detector activates when every finger on the observed hand meets these conditions.
   * 
   * If added to a HandModelBase instance or one of its children, this detector checks the
   * finger state at the interval specified by the Period variable. You can also specify
   * which hand model to observe explicitly by setting handModel in the Unity editor or 
   * in code.
   * 
   * @since 4.1.2
   */
  public class GestureDetection : Detector {
    /**
     * The interval at which to check finger state.
     * @since 4.1.2
     */
    [Tooltip("The interval in seconds at which to check this detector's conditions.")]
    [Units("seconds")]
    [MinValue(0)]
    public float Period = .1f; //seconds

    /**
     * The HandModelBase instance to observe. 
     * Set automatically if not explicitly set in the editor.
     * @since 4.1.2
     */
    [Tooltip("The hand model to watch. Set automatically if detector is on a hand.")]
    public HandModelBase HandModelRight = null;
    public HandModelBase HandModelLeft = null;

    private PointingState leftPinkyPointingState = PointingState.NotExtended;
    private PointingState leftRingPointingState = PointingState.NotExtended;
    private PointingState leftMiddlePointingState = PointingState.NotExtended;
    private PointingState leftIndexPointingState = PointingState.NotExtended;
    private PointingState leftThumbPointingState = PointingState.NotExtended;
    private PointingState rightPinkyPointingState = PointingState.NotExtended;
    private PointingState rightRingPointingState = PointingState.NotExtended;
    private PointingState rightMiddlePointingState = PointingState.NotExtended;
    private PointingState rightIndexPointingState = PointingState.NotExtended;
    private PointingState rightThumbPointingState = PointingState.NotExtended;
  

    private IEnumerator watcherCoroutine;
    public float XRightIndexNow;
    public float XRightIndexThen;
    public float YRightIndexNow;
    public float YRightIndexThen;

    void Awake () {
      watcherCoroutine = extendedFingerWatcher();
    }
  
    void OnEnable () {
      StartCoroutine(watcherCoroutine);
    }
  
    void OnDisable () {
      StopCoroutine(watcherCoroutine);
      Deactivate();
    }
  
    IEnumerator extendedFingerWatcher() {
      Hand hand;
      
      while(true)
      {
        if (HandModelRight != null && HandModelRight.IsTracked)
        {
            
            hand = HandModelRight.GetLeapHand();
            //Debug.Log(hand.GetIndex().TipPosition.x);
            XRightIndexNow = hand.GetIndex().TipPosition.x;
            YRightIndexNow = hand.GetIndex().TipPosition.y;
            if (hand != null)
            {
                if (hand.Fingers[0].IsExtended) //jempol
                {
                    rightThumbPointingState = PointingState.Extended;
                }
                else
                {
                    rightThumbPointingState = PointingState.NotExtended;
                }
                if (hand.Fingers[1].IsExtended) //telunjuk
                {
                    
                    rightIndexPointingState = PointingState.Extended;
                }
                else
                {
                    rightIndexPointingState = PointingState.NotExtended;
                }
                if (hand.Fingers[2].IsExtended) //tengah
                {
                    rightMiddlePointingState = PointingState.Extended;
                }
                else
                {
                    rightMiddlePointingState = PointingState.NotExtended;
                }
                if (hand.Fingers[3].IsExtended) //manis
                {
                    rightRingPointingState = PointingState.Extended;
                }
                else
                {
                    rightRingPointingState = PointingState.NotExtended;
                }
                if (hand.Fingers[4].IsExtended) //kelingking
                {
                    rightPinkyPointingState = PointingState.Extended;
                }
                else
                {
                    rightPinkyPointingState = PointingState.NotExtended;
                }

            }
            
        }
        else
        {
            rightThumbPointingState = PointingState.NotExtended;
            rightIndexPointingState = PointingState.NotExtended;
            rightMiddlePointingState = PointingState.NotExtended;
            rightRingPointingState = PointingState.NotExtended;
            rightPinkyPointingState = PointingState.NotExtended;
        }
                
        if (HandModelLeft != null && HandModelLeft.IsTracked)
        {
            hand = HandModelLeft.GetLeapHand();
            if (hand != null)
            {
                if (hand.Fingers[0].IsExtended) //jempol
                {
                    leftThumbPointingState = PointingState.Extended;
                }
                else
                {
                    leftThumbPointingState = PointingState.NotExtended;
                }
                if (hand.Fingers[1].IsExtended) //telunjuk
                {
                    leftIndexPointingState = PointingState.Extended;
                }
                else
                {
                    leftIndexPointingState = PointingState.NotExtended;
                }
                if (hand.Fingers[2].IsExtended) //tengah
                {
                    leftMiddlePointingState = PointingState.Extended;
                }
                else
                {
                    leftMiddlePointingState = PointingState.NotExtended;
                }
                if (hand.Fingers[3].IsExtended) //manis
                {
                    leftRingPointingState = PointingState.Extended;
                }
                else
                {
                    leftRingPointingState = PointingState.NotExtended;
                }
                if (hand.Fingers[4].IsExtended) //kelingking
                {
                    leftPinkyPointingState = PointingState.Extended;
                }
                else
                {
                    leftPinkyPointingState = PointingState.NotExtended;
                }
            }
        }
        else
        {
            leftThumbPointingState = PointingState.NotExtended;
            leftIndexPointingState = PointingState.NotExtended;
            leftMiddlePointingState = PointingState.NotExtended;
            leftRingPointingState = PointingState.NotExtended;
            leftPinkyPointingState = PointingState.NotExtended;
        }
        yield return new WaitForSeconds(Period);
                XRightIndexThen = XRightIndexNow;
                YRightIndexThen = YRightIndexNow;
            }
    }

        #region cek status Extended Fingers

        public bool isLeftPinkyFingerExtended()
        {
            return (leftPinkyPointingState == PointingState.Extended);
        }
        public bool isLeftRingFingerExtended()
        {
            return (leftRingPointingState == PointingState.Extended);
        }
        public bool isLeftMiddleFingerExtended()
        {
            return (leftMiddlePointingState == PointingState.Extended);
        }
        public bool isLeftIndexFingerExtended()
        {
            return (leftIndexPointingState == PointingState.Extended);
        }
        public bool isLeftThumbFingerExtended()
        {
            return (leftThumbPointingState == PointingState.Extended);
        }

        public bool isRightPinkyFingerExtended()
        {
            return (rightPinkyPointingState == PointingState.Extended);
        }
        public bool isRightRingFingerExtended()
        {
            return (rightRingPointingState == PointingState.Extended);
        }
        public bool isRightMiddleFingerExtended()
        {
            return (rightMiddlePointingState == PointingState.Extended);
        }
        public bool isRightIndexFingerExtended()
        {
            return (rightIndexPointingState == PointingState.Extended);
        }
        public bool isRightThumbFingerExtended()
        {
            return (rightThumbPointingState == PointingState.Extended);
        }

        #endregion
    }

    /** Defines the settings for comparing extended finger states */
    public enum PointingState{Extended, NotExtended, Either}
}
