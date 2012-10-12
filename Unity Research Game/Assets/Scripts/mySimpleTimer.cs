using UnityEngine;
using System.Collections;
using System; //for EventArgs

/// <summary>
/// Defines simple timer for use with Unity3d
/// </summary>
public class mySimpleTimer : MonoBehaviour
{
    /// <summary>
    /// A simple countdown timer class for use with Unity3d. 
    /// 
    /// Make sure to place the StartMethod() in Start() and UpdateMethod in Update()
    /// </summary>
    [System.Serializable]
    public class SimpleCountdownTimer
    {
        #region Global Variables

        /// <summary>
        /// The time from which to start counting down
        /// (seconds)
        /// </summary>
        private double startTime;

        /// <summary>
        /// Time remaining before 0 is reached. 
        /// (seconds)
        /// </summary>
        private double remainingTime;

        /// <summary>
        /// Flags true if timer is active
        /// </summary>
        public bool isCounting;

        /// <summary>
        /// Event handler for when timer reaches 0
        /// </summary>
        private TimerEventHandler timerEventHandler;

        #region Type Definitions

        /// <summary>
        /// Type definition for delegate called when timer reaches 0
        /// </summary>
        /// <param name="sender">Reference to object calling method</param>
        /// <param name="e">reference to event Arguments</param>
        public delegate void TimerEventHandler(object sender, EventArgs e);

        #endregion

        #endregion

        #region methods
        /// <summary>
        /// Starts the countdown
        /// </summary>
        public void StartCountdown()
        {
            remainingTime = startTime * 1;
            isCounting = true;
        }

        /// <summary>
        /// Stops the countdown and resets the timer to 0
        /// </summary>
        public void StopCountdown()
        {
            isCounting = false;
            remainingTime = startTime;
        }

        /// <summary>
        /// Resets the timer to 0 but keeps counting
        /// </summary>
        public void ResetTimer()
        {
            remainingTime = startTime * 1;
        }

        /// <summary>
        /// Returns the time remaining
        /// </summary>
        /// <returns>Time remaining</returns>
        public int GetRemainingTime()
        {
            return (int)remainingTime;
        }

        /// <summary>
        /// Sets the time from which to count down
        /// </summary>
        /// <param name="startTimeIn">Time from which to count down</param>
        public void SetStartTime(double startTimeIn)
        {
            startTime = startTimeIn;
        }

        /// <summary>
        /// Makes the timer work :) Place in the Update() method of target class
        /// </summary>
        public void UpdateMethod()
        {
            if (isCounting)
            {
             
                if (remainingTime <= 0)
                {
                    remainingTime = startTime;
                    isCounting = false;
                    timerEventHandler(this, new EventArgs()); //call handler
                }
                
                remainingTime = remainingTime - Time.deltaTime;
            }
        }

        /// <summary>
        /// Initializes the timer. Place in Start() method of target class
        /// </summary>
        /// <param name="startTime">The time from which to count down to</param>
        public void StartMethod(double startTimeIn, TimerEventHandler timerEventHandlerIn)
        {
            remainingTime = startTime * 1;
            startTime = startTimeIn;
            timerEventHandler = timerEventHandlerIn;
        }

        #endregion
    };


}
