using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


/// <summary>
/// Acts as a translation layer between the GUI and the ZigSkeleton.cs script
/// Author: Adeesha Ekanayake
/// Date: June, August 2012
/// </summary>
public class TranslationLayer : MonoBehaviour {

    //lists to be kept in memory
    
    #region GameObject Variables

    /// <summary>
    /// Name of the GameObject to which ZigSkeleton script is attached
    /// </summary>
    public static string avatarGameObjectName = "Dana";

    /// <summary>
    /// Name of the GameObject which acts as an indicator
    /// This is used to show the user the pose he/she should
    /// assume
    /// </summary>
    public static string indicatorGameObjectName = "Carl";

    /// <summary>
    /// Name of the GameObject to which Zigfu script is attached
    /// </summary>
    public static string zigfuGameObjectName = "ZigFu";

    /// <summary>
    /// Name of the gameobject which handles all system sounds
    /// </summary>
    public static string soundGameObjectName = "SoundScript";

    /// <summary>
    /// Declares the sound gameObject, since there is only one for the whole script
    /// </summary>
    public SoundScript systemSounds;

    /// <summary>
    /// Name of the GameObject to which GUIText is attached
    /// </summary>
    public static string InfoTextGameObjectName = "InfoText";

    #endregion

    #region Generic Global Variables
    /// <summary>
    /// Flags true if the gestureRecognitionClone is created
    /// </summary>
    private bool cloneCreated = false;

    /// <summary>
    /// The key point pose the clone is holding
    /// </summary>
    private int cloneKPIndex = -1;

    /// <summary>
    /// This GameObject is a reference to an object created in runtime
    /// The object created is a clone of the game character,
    /// who is used to check if the user is holding the correct pose
    /// </summary>
    private GameObject gestureRecognitionClone;

    /// <summary>
    /// The prefab template according to which the gestureRecognitionClone is created.
    /// Assign to a prefab identical to the user avatar.
    /// </summary>
    public GameObject gestureRecognitionPrefab;

    /// <summary>
    /// The current Key point being checked for
    /// </summary>
    public int currentKeyPoint = 0;

    /// <summary>
    /// flags true if seated mode is on
    /// </summary>
    public bool seatedModeOn = false;

#endregion

    #region gesture tracking sensitivity variables

    /// <summary>
    /// This measures the sensitivity of the gesture recognition system. 
    ///     lower numbers = more sensitivity.
    /// </summary>
    public float gestureRecognitionThreshold = 1.2f;

    /// <summary>
    /// Measures sensitivity of gesture recognition, in seated mode
    /// </summary>
    public float seatedGestureRecognitionThreshold = 0.8f;

    /// <summary>
    /// Duration for which the user has to hold a gesture in order for it to register
    ///     as being succesfully held
    /// </summary>
    public int gestureHoldLength = 30;

    /// <summary>
    /// The level of accuracy to which player must hold a pose for the pose to
    ///     be succesful
    /// </summary>
    public double gestureAccuracy = .9;

    /// <summary>
    /// Flags true if a gesture has been initiated
    /// </summary>
    [HideInInspector]
    public bool isHoldingGesture = false;

    /// <summary>
    /// Temporary global variables used by CheckForGesture()
    /// </summary>
    public int gestureScore = 0, gestureCount = 0;

    /// <summary>
    /// Flags true if indicator is animating
    /// </summary>
    [HideInInspector]
    public bool isAnimatingIndicatorModel = false;

    /// <summary>
    /// the key point the indicator model is animating to
    /// </summary>
    public int indicatorModelKeyPoint = 0;

    /// <summary>
    /// Speed at which indicator GameObject animates
    /// </summary>
    public float indicatorAnimateSpeed = 1;


    /// <summary>
    /// flags true if checking for an exercise. Used to make the countdown timer work
    /// </summary>
    [HideInInspector]
    public bool isCheckingForExercise = false;


#endregion
    
    #region Exercise tracking related variables

    /// <summary>
    /// Keeps track of the number of succesful gestures the player makes in an execise
    /// </summary>
    public int exerciseScore = 0;

    #endregion

    #region Lists and playback related variables

    /// <summary>
    /// List containing recording of avatar's movements to be played back
    /// </summary>
    public List<SerializeScript.SnapshotClass> playbackList;

    /// <summary>
    /// List containing index of keypoints
    /// </summary>
    public List<SerializeScript.KeyPoint> keypointsList;

    /// <summary>
    /// Flags if the recording in memory is being played
    /// </summary>
    public bool isPlaying = false;

    /// <summary>
    /// The length of time a character holds a pose, when demoing.
    /// </summary>
    public int demoDelay = 0;

    /// <summary>
    /// The current key frame being played by the demo avatar
    /// </summary>
    public int demoCurrentKeyFrame = 0;

    /// <summary>
    /// Tracks the current frame of the recording
    /// </summary>
    public int currentFrame = 0;

    /// <summary>
    /// Flags true if listening for a pose
    /// </summary>
    [HideInInspector]
    public bool listeningForGesture = false;

    /// <summary>
    /// Flags true if listening for a whole body pose
    /// </summary>
    [HideInInspector]
    public bool listeningForWholeBodyGesture = false;

#endregion

    #region GUI Controls

    /// <summary>
    /// Text used for GUI feedback
    /// </summary>
    public GUIText feedbackText1;

    /// <summary>
    /// Text also used for GUI feedback
    /// </summary>
    public GUIText feedbackText2;
   
    #endregion

    #region timer control variables

    /// <summary>
    /// Countdown timer used for gesture tracking
    /// </summary>
    [HideInInspector]
    public mySimpleTimer.SimpleCountdownTimer countdownTimer;
    /// <summary>
    /// Countdown timer used for demonstrating exercises
    /// </summary>
    [HideInInspector]
    public mySimpleTimer.SimpleCountdownTimer demoCountdownTimer;

    #endregion

    #region List modifier methods

    /// <summary>
    /// Loads the indicated recording from disk
    /// Recordings are stored as XML files
    /// </summary>
    /// <param name="fileNameIn">The name of the XML file to be loaded</param>
    public void LoadList(string fileNameIn)
    {
        //stop checking if checking
        if (listeningForWholeBodyGesture || isPlaying || isAnimatingIndicatorModel || listeningForGesture)
        {
            listeningForWholeBodyGesture = false;
            indicatorModelKeyPoint = 0;
            currentKeyPoint = 0;

            listeningForGesture = false;
            isAnimatingIndicatorModel = false;
            isPlaying = false;

            //reset the gestures of the 2 models
            GameObject.Find(indicatorGameObjectName).GetComponent<ZigSkeleton>
                ().RotateToCalibrationPose();

            GameObject.Find(avatarGameObjectName).GetComponent<ZigSkeleton>
                ().RotateToCalibrationPose();

            //feedback
            feedbackText1.text = "File loaded succesfully!";
            feedbackText2.text = "Press play game to start!";
        }



        playbackList = SerializeScript.deserializeFromDisk(fileNameIn);
        Debug.Log("List " + fileNameIn + " succesfully loaded");

        SyncLists(); //refresh keypointslist
    }

    /// <summary>
    /// Synchronises playbackList and keyPointsList
    /// </summary>
    public void SyncLists()
    {
        if (playbackList != null)
        {

            keypointsList = new List<SerializeScript.KeyPoint>(); //clear keypointslist

            for (int i = 0; i < playbackList.Count; i++)//traverse playbackList
            {
                if (playbackList[i].isKeyFrame == true)//if this point is a key point
                {
                    //initialize new keyPoint
                    SerializeScript.KeyPoint newKeyPoint = new SerializeScript.KeyPoint();
                    newKeyPoint.frameID = i;

                    //add new keyPoint to list
                    keypointsList.Add(newKeyPoint);
                }
            }

            Debug.Log(" " + keypointsList.Count + " key points total");
        }
    }
#endregion

    #region playback related methods

        /// <summary>
        /// Plays the recording in memory, if a recording is in memory. 
        /// Otherwise, logs an error message.
        /// </summary>
        public void playRecording()
        {
            //log an error message if not
            if (playbackList != null)
            {

                //ensure ZigSkeleton is not tracking
                StopTracking();

                isPlaying = true;

            }

            else
            {
                Debug.Log("Error: There is no recording in memory");
            }
        }

        
        /// <summary>
        /// NOTE: THIS METHOD IS DEPRECATED:
        /// the system no longer plays back a recording; instead, in demo mode, the exercise is 
        /// demonstrated key point at a time.
        /// plays the current frame from the recording in memory
        /// Note: Only invoked from TickEvent
        /// </summary>
        void PlayFrame()
        {
            if (currentFrame < playbackList.Count)//check if playback is within bounds
            {
                //load current frame
                SerializeScript.SnapshotClass currFrame = playbackList[currentFrame];

                //play current frame
                GameObject.Find(avatarGameObjectName).GetComponent<ExtendedZigSkeleton>().PlayFrame(currFrame);

                //increment frame
                currentFrame++;
            }

            else //playback has exceeded bounds
            {
                resetPlayback();
            }
            
        }

        /// <summary>
        /// This is used to demonstrate each exercise, one pose at a time.
        /// </summary>
        public void DemoRecording()
        {
            //reset the current keyframe
            demoCurrentKeyFrame = 0;

            //check if the playlist is null
            if (playbackList != null)
            {

                //set flag to playing
                isPlaying = true;

                //StopTracking();

                //make model animate
                indicatorModelKeyPoint = 0;

                StartAnimatingIndicatorModel();

                //start the demotimer
                demoCountdownTimer.StartCountdown();

            }

            else
            {
                Debug.Log("No list loaded, cannot demo");
                
            }
        }

        /// <summary>
        /// This method is called by the demoCountdownTimer when it runs to 0.
        /// It is used to make the model assume the next pose, or the T-pose if it 
        /// has reached the end of the demo
        /// </summary>
        void NextGesture(object sender, EventArgs e)
        {
            //Make sure not to play keyframe after last keyfram
            if (demoCurrentKeyFrame < playbackList.Count - 1)
            {

                //advance keyframe
                demoCurrentKeyFrame++;

                //make indicator animate
                indicatorModelKeyPoint++;

                StartAnimatingIndicatorModel();

                //Start the counter again
                demoCountdownTimer.StartCountdown();
            }

            //means countdown has reached last keyframe
            else
            {
                //assume T-Pose
                Debug.Log("Playback complete");

                //reset keyframe
                demoCurrentKeyFrame = 0;

                //set flag to false
                isPlaying = false;

                //ResumeTracking();
            }
        }

        /// <summary>
        /// Stops playback and resets current frame to 0
        /// </summary>
        void resetPlayback()
        {
            isPlaying = false;
            currentFrame = 0;
            GameObject.Find(avatarGameObjectName).GetComponent<ZigSkeleton>().RotateToCalibrationPose();
            ResumeTracking();
        }

        /// <summary>
        /// Makes the given avatar jump to a key frame
        /// </summary>
        /// <param name="keyPointIn">Id of the key point</param>
        /// <param name="avatarName">Name of the avatar gameObject</param>
        public void JumpToKeyFrame(int keyPointIn, string avatarName)
        {

        }

        /// <summary>
        /// For testing use
        /// Makes avatar jump to a given frame
        /// </summary>
        /// <param name="frameID">Index of frame to jump to </param>
        /// <param name="avatarName">name of avatar</param>
        public void JumpToFrame(int frameID, string avatarName)
        {
            GameObject.Find(avatarName).GetComponent<ExtendedZigSkeleton>().JumpToFrame(playbackList[frameID]);
        }
    #endregion

    #region Tracking related methods
        /// <summary>
        /// Stops skeletal tracking, so that avatar is no longer moved by Zigfu
        /// </summary>
        public void StopTracking()
        {
            GameObject.Find(zigfuGameObjectName).GetComponent<ZigEngageSingleUser>().SkeletonTracked = false;
        }

        /// <summary>
        /// Resumes skeletal tracking, so avatar is once again tracked by Zigfu
        /// </summary>
        public void ResumeTracking()
        {
            GameObject.Find(zigfuGameObjectName).GetComponent<ZigEngageSingleUser>().SkeletonTracked = true;
        }

    #endregion

    #region Gesture tracking methods

        /// <summary>
        /// Checks if current posture matches the one of the keypoint passed in
        /// </summary>
        public void ListenForWholeBodyGesture()
        {
            //variables
            bool isHoldingPose = false;

            if (cloneCreated == false)//if the clone is not instantiated
            {
                //make a copy of the player avatar in the same place
                //get position and rotation of avatar to create clone
                Vector3 avatarPosition = 
                    GameObject.Find(avatarGameObjectName).GetComponent<Transform>().position;
                Quaternion avatarRotation = 
                    GameObject.Find(avatarGameObjectName).GetComponent<Transform>().rotation;

                //instantiate clone
                gestureRecognitionClone = Instantiate(gestureRecognitionPrefab, avatarPosition, avatarRotation) as GameObject;

                //make clone invisible
                gestureRecognitionClone.GetComponentInChildren<Renderer>().enabled = false;

                cloneCreated = true;//set flag to true
            }

            //check the current keypoint
            if (keypointsList != null)
                if (keypointsList[currentKeyPoint] != null)
                {

                    //send snapshot to be checked
                    SerializeScript.SnapshotClass checkSnap =
                        new SerializeScript.SnapshotClass();
                    checkSnap = playbackList[keypointsList[currentKeyPoint].frameID]; //assign snapshot

                    //make the clone assume the pose to check 
                    gestureRecognitionClone.GetComponent<ExtendedZigSkeleton>().JumpToFrame(checkSnap);

    /*                //make the template model assume pose to check
                    GameObject.Find(indicatorGameObjectName).GetComponent<ZigSkeleton>()
                        .JumpToFrame(checkSnap);
     */

                    List<Transform> avatarPose =
                        GameObject.Find(avatarGameObjectName).GetComponent<ExtendedZigSkeleton>().ReturnTransformsList();

                    List<Transform> templatePose =
                        gestureRecognitionClone.GetComponent<ExtendedZigSkeleton>().ReturnTransformsList();

                    if (!seatedModeOn)
                        isHoldingPose = CheckForPose(avatarPose, templatePose);
                    else
                    {
                        isHoldingPose = CheckForSeatedPose(avatarPose, templatePose);
                        //make gesture recognition clone assume seated mode
                        gestureRecognitionClone.GetComponent<ExtendedZigSkeleton>().SeatedMode();
                    }

                    if (isHoldingPose)
                    {
                        feedbackText2.text = "Holding Pose!";
                        isHoldingGesture = true; //initiate gesture
                        listeningForGesture = false; //disable listening for gesture
                    }

                    else
                        feedbackText2.text = "Not holding Pose";
                }

                else
                    listeningForWholeBodyGesture = false;

            else
                listeningForWholeBodyGesture = false;
        }

        /// <summary>
        /// Makes system start listening for a full body gesture
        /// </summary>
        public void StartListeningForWholeBodyGesture()
        {
            //Play sounds!
            systemSounds.PlayGameStartSound();
            systemSounds.PlayGameBackgroundNoise();
            systemSounds.PlayGameMusic();

            //set current keyPoint to 0
            currentKeyPoint = 0;

            //make carl assume the keypoint pose
            indicatorModelKeyPoint = 0;

            StartAnimatingIndicatorModel();

            //make listening=true
            listeningForWholeBodyGesture = true;

            isCheckingForExercise = true;

            exerciseScore = 0; //reset score to 0

            //make timer count down
            countdownTimer.StartCountdown();
        }

        /// <summary>
        /// Checks if avatar and template are holding the same pose, In seated mode
        /// </summary>
        /// <param name="avatarPose">pose of player avatar</param>
        /// <param name="templatePose">pose of template avatar to check against</param>
        /// <returns>Boolean, true if user is holding gesture and false if not</returns>
        public bool CheckForSeatedPose(List<Transform> avatarPose, List<Transform> templatePose)
        {

            float sum = 0; //sum of the distance between poses


            for (int i = 0; i < avatarPose.Count; i++)
            {

                if (i < templatePose.Count)
                {
                    sum += Vector3.Distance(avatarPose[i].position, templatePose[i].position);
                }
            }

            //GUI text it!
            //feedbackText1.text = sum.ToString();

            //check if it is below threshold
            if (sum < seatedGestureRecognitionThreshold)
            {
                //This means user is holding gesture
                return true;
            }

            else
            {
                return false;
            }
        }
        
        /// <summary>
        /// Checks if avatar and template are holding the same pose
        /// </summary>
        /// <param name="avatarPose">pose of player avatar</param>
        /// <param name="templatePose">pose of template avatar to check against</param>
        /// <returns>Boolean, true if user is holding gesture and false if not</returns>
        public bool CheckForPose(List<Transform> avatarPose, List<Transform> templatePose)
        {
            float sum = 0; //sum of the distance between poses

            for (int i = 0; i < avatarPose.Count; i++)
            {
                if (i < templatePose.Count)
                {
                    sum += Vector3.Distance(avatarPose[i].position, templatePose[i].position);
                }
            }

            //GUI text it!
            //feedbackText1.text = sum.ToString();
           
            //check if it is below threshold
            if (sum < gestureRecognitionThreshold)
            {
                //This means user is holding gesture
                return true;
            }

            else 
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if user holds a pose for a set duration
        /// Executed by tickEvent
        /// </summary>
        public void CheckForGesture()
        {
            if (gestureCount < gestureHoldLength)
            {
                //get data to check for pose
                List<Transform> avatarPose =
        GameObject.Find(avatarGameObjectName).GetComponent<ExtendedZigSkeleton>().ReturnTransformsList();

                List<Transform> templatePose =
                    gestureRecognitionClone.GetComponent<ExtendedZigSkeleton>().ReturnTransformsList();

                //check if seated mode
                if (!seatedModeOn)
                {
                    //if holding gesture add to the score
                    if (CheckForPose(avatarPose, templatePose))
                    {
                        gestureScore++;
                    }
                }

                else
                {
                    //if holding gesture add to the score
                    if (CheckForSeatedPose(avatarPose, templatePose))
                    {
                        gestureScore++;
                    }
                }

                //increment count
                gestureCount++;

            }

            else
            {
                //check if gesture is succesful
                double gestureCheck = gestureScore / gestureCount;

                //stop checking for gesture
                isHoldingGesture = false;
                listeningForWholeBodyGesture = true;

                //reset count
                gestureCount = 0;
                gestureScore = 0;

                //if gesture is succesful, do whatever
                if (gestureCheck >= gestureAccuracy)
                {
                    Debug.Log("Gesture success!");
                    GestureHeldEvent();
                }

                else
                {
                    Debug.Log("Gesture failed");
                }


            }
        }

        /// <summary>
        /// This event is called by CheckForGesture. 
        /// 
        /// If the user holds the gesture for a defined length of time (gestureHoldLength),
        ///     CheckForGesture will calculate a score of how accurately the pose was held.
        ///     If the accuracy is greater than gestureAccuracy this method is called.
        ///     
        /// This method also plays a sound, systemSounds.soundOnSuccesfulGesture
        ///     
        /// This method advances the current keyPoint to the next, and shows that the player
        ///     succesfully did a part of an exercise
        /// </summary>
        public void GestureHeldEvent()
        {
            feedbackText1.text = "Succesful Gesture!";

            exerciseScore++; //add to score

            systemSounds.PlayGestureSuccessSound(); //play the sound

            //advance to next keypoint
            Debug.Log("Gesture Succesful! " + currentKeyPoint + " " + indicatorModelKeyPoint);

            if (currentKeyPoint < keypointsList.Count - 1)
            {
                currentKeyPoint++; //increment current keypoint

                indicatorModelKeyPoint++;

                StartAnimatingIndicatorModel();

                countdownTimer.ResetTimer();//reset timer
            }

            else //finished holding gestures
            {
                ExerciseHeldEvent();//
                countdownTimer.StopCountdown();
            }
            
        }

        /// <summary>
        /// This event is called by TimerZeroEvent. 
        /// If the timer reaches 0 and the pose is not succesfully held, 
        ///     this method is called. 
        ///     
        /// The method also plays a sound - systemSounds.soundOnFailedGesture
        /// 
        /// This method has to do the same as GestureHeldEvent except that
        ///     it says gesture not held.
        /// </summary>
        public void GestureNotHeldEvent()
        {
            //advance to next keypoint
            Debug.Log("Not succesful " + currentKeyPoint + " " + indicatorModelKeyPoint);

            feedbackText1.text = "Gesture Not held succesfully, moving on";

            systemSounds.PlayGestureFailSound(); //play sound


            if (currentKeyPoint < keypointsList.Count - 1)
            {
                currentKeyPoint++; //increment current keypoint

                indicatorModelKeyPoint++;

                StartAnimatingIndicatorModel();

                countdownTimer.SetStartTime(gestureHoldLength);

                countdownTimer.StartCountdown();
            }

            else //finished holding gestures
            {
                ExerciseHeldEvent();//
                countdownTimer.StopCountdown();
            }
        }

        /// <summary>
        /// This is called if the user succesfully holds an entire exercise. This method is
        ///     called from GestureHeldEvent()
        /// </summary>
        public void ExerciseHeldEvent()
        {
            Debug.Log("Exercise Complete:  " + exerciseScore + " / " + keypointsList.Count);
            listeningForWholeBodyGesture = false; //stop listening
            isCheckingForExercise = false;
            feedbackText1.text = "Exercise Complete:  " + exerciseScore + " / " + keypointsList.Count;
        
            //stop playing some sounds
            systemSounds.StopAmbientMusic();

            //play sound
            systemSounds.PlayGameEndSound();
            systemSounds.PlayMusic();
            systemSounds.PlayBackgroundNoise();
        }

        /// <summary>
        /// Updates text on the GUIText control to match that in the avatar
        /// </summary>
        public void UpdateGuiText()
        {
            string textIn =
                GameObject.Find(avatarGameObjectName).GetComponent<ExtendedZigSkeleton>()
                .GUIOutputText1;

            feedbackText1.text = textIn;

            textIn =
                GameObject.Find(avatarGameObjectName).GetComponent<ExtendedZigSkeleton>()
                .GUIOutputText2;

            //Declare the SoundScript
            systemSounds = GameObject.Find(soundGameObjectName).GetComponent<SoundScript>();

            feedbackText2.text = textIn;

        }
        #endregion

    #region animation related methods

        /// <summary>
        /// This method smoothly animates the indicator model as per the animation recorded by the
        ///     therapist, until it reaches the required keypoint.
        /// </summary>
        /// <param name="KeypointIn"></param>
        public void StartAnimatingIndicatorModel()
        {

            isAnimatingIndicatorModel = true;

            GameObject.Find(indicatorGameObjectName).GetComponent<ExtendedZigSkeleton>().animateSpeed = 
                indicatorAnimateSpeed;

        }

        /// <summary>
        /// This method is called by tickEvent, and it smoothly animates the indicator model until the
        ///     frame in indicatorModelKeyPoint is reached.
        /// </summary>
        public void AnimateIndicatorModel()
        {
            
            //make the avatar jump to the frame passed in.
            GameObject.Find(indicatorGameObjectName).GetComponent<ExtendedZigSkeleton>()
                .AnimateToFrame(playbackList[keypointsList[indicatorModelKeyPoint].frameID]);

        }

        #endregion

    #region timing related methods

        /// <summary>
        /// This event handler is called if the countdown timer reaches 0
        /// 
        /// It checks if the system is checking for an exercise. If it is doing so,
        ///     it calls the GestureNotHeldEvent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TimerZeroEvent(object sender, EventArgs e)
        {
            Debug.Log("Timer is 0!");
            countdownTimer.StopCountdown();
            if (isCheckingForExercise)
            {
                GestureNotHeldEvent();
            }
        }

        /// <summary>
        /// This event is fired regularly, and keeps time when playing
        /// </summary>
        public void TickEvent()
        {
            if (listeningForWholeBodyGesture)
                ListenForWholeBodyGesture();

            if (isHoldingGesture)
                CheckForGesture();

            if (isAnimatingIndicatorModel)
                AnimateIndicatorModel();

            //seated mode
            if (seatedModeOn)
            {
                GameObject.Find(avatarGameObjectName).GetComponent<ExtendedZigSkeleton>()
                    .SeatedMode();
                GameObject.Find(indicatorGameObjectName).GetComponent<ExtendedZigSkeleton>()
        .SeatedMode();
            }

            else
            {
                GameObject.Find(avatarGameObjectName).GetComponent<ExtendedZigSkeleton>()
                    .DisableSeatedMode();
                GameObject.Find(indicatorGameObjectName).GetComponent<ExtendedZigSkeleton>()
                    .DisableSeatedMode();
            }
            //UpdateGuiText();
        }

        #endregion

    #region Sound related methods



    #endregion

    // Use this for initialization
void Start () {

        //init the countdown timers
        countdownTimer = new mySimpleTimer.SimpleCountdownTimer();
        countdownTimer.StartMethod(gestureHoldLength, 
            new mySimpleTimer.SimpleCountdownTimer.TimerEventHandler(TimerZeroEvent));

        demoCountdownTimer = new mySimpleTimer.SimpleCountdownTimer();
        demoCountdownTimer.StartMethod(demoDelay,
            new mySimpleTimer.SimpleCountdownTimer.TimerEventHandler(NextGesture));

        systemSounds =
            GameObject.Find(soundGameObjectName).GetComponent<SoundScript>();
        
        
	}
	
	// Update is called once per frame
void Update()
{
    countdownTimer.UpdateMethod();
    demoCountdownTimer.UpdateMethod();
    feedbackText2.text = countdownTimer.GetRemainingTime().ToString();
}

    /// <summary>
    /// Unity specific event, which is fired regularly.
    /// Used for physics calculations.
    /// In this case, it is used to fire the TickEvent() regularly.
    /// </summary>
    void FixedUpdate()
    {
        TickEvent(); //tick!
    }
}
