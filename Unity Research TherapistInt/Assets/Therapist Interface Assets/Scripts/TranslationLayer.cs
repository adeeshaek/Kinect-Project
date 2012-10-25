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
    
#region Global Variables

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
    /// Name of the GameObject to which GUIText is attached
    /// </summary>
    public static string InfoTextGameObjectName = "InfoText";

    /// <summary>
    /// Name of GameObject to which GUI is attached 
    /// </summary>
    public static string GUIGameObjectName = "GUI";


    #region Lists and playback related variables

        /// <summary>
        /// List containing recording of avatar's movements to be played back
        /// </summary>
        public List<SerializeScript.SnapshotClass> playbackList;

        /// <summary>
        /// List containing a copy of the playbackList just before the most recent change
        /// </summary>
        public List<SerializeScript.SnapshotClass> backupList;

        /// <summary>
        /// List containing index of keypoints
        /// </summary>
        public List<SerializeScript.KeyPoint> keypointsList;

        /// <summary>
        /// Flags if the recording in memory is being played
        /// </summary>
        public bool isPlaying = false;

        /// <summary>
        /// Flags if the player's movements are being recorded
        /// </summary>
        public bool isRecording = false;

        /// <summary>
        /// Tracks the current frame of the recording
        /// </summary>
        public int currentFrame = 0;

        /// <summary>
        /// Flags true if listening for a pose
        /// </summary>
        public bool listeningForGesture = false;

        #region seated mode

        /// <summary>
        /// Flags true if seated mode is on
        /// </summary>
        public bool seatedModeOn = false;
        #endregion

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

#endregion

    #region List modifier methods

    /// <summary>
    /// Used to reduce the size of the XML file saved by the editor.
    /// This is done by removing all frames which are not key frames. 
    /// Makes resulting file small and easy to email / fast to load
    /// 
    /// This method automatically compresses the PlaybackList
    /// </summary>
    /// <returns></returns>
    public List<SerializeScript.SnapshotClass> CompressPlaybackList()
    {
        List<SerializeScript.SnapshotClass> outputList = new List<SerializeScript.SnapshotClass>();

        //make this list a copy of playback list

        //remove superfluous frames
        for (int i = 0; i < playbackList.Count; i++)
        {
            if (playbackList[i].isKeyFrame)
            {
                outputList.Add(playbackList[i]);
            }
        }

        return outputList;
    }

    /// <summary>
    /// Used to export the indicated recording from disk. Triggered from the Ok button
    ///     in the save dialog box. Exported files are compressed.
    /// </summary>
    /// <param name="fileNameIn"></param>
    public void ExportList(string fileNameIn)
    {
        SerializeScript.SaveToDisk(fileNameIn, CompressPlaybackList());
        Debug.Log("File exported succesfully");
    }

    /// <summary>
    /// Loads the indicated recording from disk
    /// Recordings are stored as XML files
    /// </summary>
    /// <param name="fileNameIn">The name of the XML file to be loaded</param>
    public void LoadList(string fileNameIn)
    {
        playbackList = SerializeScript.deserializeFromDisk(fileNameIn);
        Debug.Log("List " + fileNameIn + " succesfully loaded");
        StopPlaying(); //reset playback
        SyncLists(); //refresh keypointslist
        UpdateGUIKPList();
    }

    /// <summary>
    /// Saves the playback list in memory to disk
    /// </summary>
    /// <param name="fileNameIn">target filename to save to</param>
    public void SaveList(string fileNameIn)
    {
        SerializeScript.SaveToDisk(fileNameIn, playbackList);
        Debug.Log("Playback list saved succesfully");
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

        UpdateGUIKPList();
    }

    /// <summary>
    /// Changes the start point of the playback list
    /// </summary>
    public void NewStart()
    {
        //make a backup
        BackupLists();

        //delete all the frames before currentFrame
        playbackList.RemoveRange(0, currentFrame);
        resetPlayback(); //reset to frame 0

    }

    /// <summary>
    /// Changes the end point of the playbackList
    /// </summary>
    public void NewEnd()
    {
        BackupLists(); //make a backup

        //delete all the frames after currentFrame
        playbackList.RemoveRange(currentFrame, playbackList.Count - currentFrame);
        resetPlayback(); //reset to frame 0
    }

    /// <summary>
    /// Makes the current frame a Key Point
    /// </summary>
    public void AddKeyPoint()
    {
        BackupLists(); //make a backup

        playbackList[currentFrame].isKeyFrame = true;
        SyncLists(); //sync KP list
    }

    /// <summary>
    /// Deletes the selected key point
    /// </summary>
    /// <param name="selectedKPIndex">Index of the selected key point</param>
    public void DeleteKeyPoint(int selectedKPIndex)
    {
        BackupLists(); //make a backup

        playbackList[keypointsList[selectedKPIndex].frameID].isKeyFrame = false;
        SyncLists(); //sync KP list
    }

    /// <summary>
    /// Undo a change to the playbackList or keyPointsList
    /// </summary>
    public void UndoChange()
    {
        //clear playbacklist
        playbackList.Clear();
        playbackList.AddRange(backupList);

        SyncLists();
    }

    /// <summary>
    /// Copies contents of playbackList to backupList, making a backup
    /// </summary>
    public void BackupLists()
    {
        if (backupList != null)
            backupList.Clear(); //clear list

        else
            backupList = new List<SerializeScript.SnapshotClass>(); //init list

        if (playbackList != null)
            backupList.AddRange(playbackList); //copy playbackList to backupList
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
    /// plays the current frame from the recording in memory
    /// Note: Only invoked from TickEvent();
    /// </summary>
    void PlayFrame()
    {
        if (currentFrame < playbackList.Count)//check if playback is within bounds
        {
            //load current frame
            SerializeScript.SnapshotClass currFrame = playbackList[currentFrame];

            //play current frame
            GameObject.Find(avatarGameObjectName).GetComponent<ZigSkeleton>().PlayFrame(currFrame);

            //increment frame
            currentFrame++;
        }

        else //playback has exceeded bounds
        {
            resetPlayback();
            ResumeTracking();
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
        GameObject.Find(GUIGameObjectName).GetComponent<ButtonScript>().UpdateSlider(currentFrame, playbackList.Count); //reset slider
        ResumeTracking();
    }

    /// <summary>
    /// Makes the given avatar jump to a key frame
    /// </summary>
    /// <param name="keyPointIn">Id of the key point</param>
    /// <param name="avatarName">Name of the avatar gameObject</param>
    public void JumpAvatarToKeyFrame(int keyPointIn, string avatarName)
    {

    }

    /// <summary>
    /// For testing use
    /// Makes avatar jump to a given frame
    /// </summary>
    /// <param name="frameID">Index of frame to jump to </param>
    /// <param name="avatarName">name of avatar</param>
    public void JumpAvatarToFrame(int frameID, string avatarName)
    {
        //GameObject.Find(avatarName).GetComponent<ZigSkeleton>().JumpToFrame(playbackList[frameID]);
    }

    /// <summary>
    /// Starts playback of loaded recording
    /// </summary>
    public void StartPlaying()
    {
        if (playbackList != null)
        {
            isPlaying = true;
            StopTracking();
        }

        else
            Debug.Log("Error: No file loaded");
    }

    /// <summary>
    /// Pauses playback of loaded recording
    /// </summary>
    public void PausePlaying()
    {
        isPlaying = false;
        ResumeTracking();
    }

    /// <summary>
    /// Stops playback of loaded recording (sets currentFrame to 0)
    /// </summary>
    public void StopPlaying()
    {
        isPlaying = false;
        currentFrame = 0;
    }

    /// <summary>
    /// When the slider is moved, changes the frame played accordingly
    /// </summary>
    /// <param name="sliderIndexIn">Index of slider</param>
    public void HandleSliderChange(float sliderIndexIn)
    {
        float targetFrame = ((sliderIndexIn / 100) * playbackList.Count);
        currentFrame = (int)targetFrame;
        PlayFrame();
    }

    /// <summary>
    /// Jumps to the key point that is passed in
    /// </summary>
    /// <param name="keyPointIndexIn">Key Point to jump to</param>
    public void JumpToKeyPoint(int keyPointIndexIn)
    {
        JumpToFrame(keypointsList[keyPointIndexIn].frameID);
    }

    /// <summary>
    /// returns the current frame number
    /// </summary>
    /// <returns>current frame number</returns>
    public int getCurrentFrame()
    {
        return currentFrame;
    }

    /// <summary>
    /// Returns number of frames in paylist
    /// </summary>
    /// <returns>number of frames in playlist</returns>
    public int getFrameCount()
    {
        return playbackList.Count;
    }

    /// <summary>
    /// Sets current frame to the frame index passed in
    /// </summary>
    /// <param name="frameIndex">number of the frame to jump to</param>
    public void JumpToFrame(int frameIndex)
    {
        currentFrame = frameIndex;
        PlayFrame();
        GameObject.Find(GUIGameObjectName).GetComponent<ButtonScript>().UpdateSlider(currentFrame, playbackList.Count);
    }
#endregion

    #region Recording related methods
    /// <summary>
    /// Start recording user's movements
    /// </summary>
    public void StartRecording()
    {
        /*
        //initialize recordingList
        recordList = new List<SerializeScript.SnapshotClass>();
         */

        isRecording = true;
        //clear the playList
        
        BackupLists();
        if (playbackList != null)
            playbackList.Clear();

        //clear the recording list
        GameObject.Find(avatarGameObjectName).GetComponent<ZigSkeleton>().ClearRecordingList();
    }

    /// <summary>
    /// Stop recording user's movements
    /// </summary>
    public void StopRecording()
    {
        isRecording = false;

        if (playbackList != null)
            BackupLists();

        //get recording from ZigSkeleton

        //init playbackList
        playbackList = new List<SerializeScript.SnapshotClass>();

        playbackList =
            GameObject.Find(avatarGameObjectName).GetComponent<ZigSkeleton>()
            .ReturnRecordedList();
    }

    /// <summary>
    /// If isRecording is true, this method is called by the tickEvent,
    ///     and will fire every time the tickEvent is called. 
    /// This method creates a snapshot of the current pose and adds it 
    ///     to the current playlist
    /// </summary>
    public void RecordFrame()
    {
        /*
        //variables
        SerializeScript.SnapshotClass thisFrame = new SerializeScript.SnapshotClass();

        //initialize thisFrame/PoseIn
        thisFrame =
            GameObject.Find(avatarGameObjectName).GetComponent<ZigSkeleton>().ReturnFrame();

        /*
        poseIn =
            GameObject.Find(avatarGameObjectName).GetComponent<ZigSkeleton>().ReturnFullBodyPose();

        //copy the pose to thisFrame
        for (int i = 0; i < poseIn.Count; i++)
        {
            thisFrame.jointRotations[i] = poseIn[i];
        }
        */
        /*
        recordList.Add(thisFrame);
        Debug.Log(recordList.Count);
         */
        GameObject.Find(avatarGameObjectName).GetComponent<ZigSkeleton>().SetTick(); 
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
    /// Initializes the gesture tracking process
    /// </summary>
    public void StartListeningForGesture()
    {
        //make carl assume the keypoint pose
        //GameObject.Find(indicatorGameObjectName).GetComponent<ZigSkeleton>()
          //  .JumpToFrame(playbackList[keypointsList[0].frameID]);

        //make listening=true
        listeningForGesture = true;
    }

    /// <summary>
    /// Checks the tracking stream if the current posture matches the
    /// one of the keypoint passed in
    /// 
    /// Invoked by timerTick if isListening is true
    /// </summary>
    public void ListenForGesture()
    {
        //check the first keypoint
        if (keypointsList != null)
            if (keypointsList[0] != null)
            {

                //send snapshot to be checked
                SerializeScript.SnapshotClass checkSnap =
                    new SerializeScript.SnapshotClass();
                checkSnap = playbackList[keypointsList[0].frameID]; //assign snapshot

                //gather Eulerinfo on movement of character's left arm
                //Quaternion poseIn = GameObject.Find(indicatorGameObjectName).GetComponent<ZigSkeleton>()
                  //  .ReturnLeftArmPose();

                //GameObject.Find(avatarGameObjectName).GetComponent<ZigSkeleton>()
                  //  .CheckForPose(checkSnap, poseIn); //pass snap to thingy

            }

            else
                listeningForGesture = false;

        else
            listeningForGesture = false;
    }

    /// <summary>
    /// Updates text on the GUIText control to match that in the avatar
    /// </summary>
    public void UpdateGuiText()
    {
       // string textIn =
         //   GameObject.Find(avatarGameObjectName).GetComponent<ZigSkeleton>()
           // .GUIOutputText1;

      //  feedbackText1.text = textIn;

       // textIn =
         //   GameObject.Find(avatarGameObjectName).GetComponent<ZigSkeleton>()
           // .GUIOutputText2;

       // feedbackText2.text = textIn;

    }
    #endregion

    #region Key Points List related methods

    /// <summary>
    /// Updates the KeyPoints list in the GUI
    /// </summary>
    public void UpdateGUIKPList()
    {
        GameObject.Find(GUIGameObjectName).GetComponent<ButtonScript>().UpdateKPList();
    }

    #endregion

    #region timing related methods
    /// <summary>
    /// This event is fired regularly, and keeps time when playing
    /// </summary>
    public void TickEvent()
    {
        if (isPlaying)
        {
            GameObject.Find(GUIGameObjectName).GetComponent<ButtonScript>().UpdateSlider(currentFrame, playbackList.Count);
            PlayFrame();
        }

        if (listeningForGesture)
            ListenForGesture();

        if (isRecording)
            RecordFrame();

        //for seated mode
        if (seatedModeOn)
            GameObject.Find(avatarGameObjectName).GetComponent<ZigSkeleton>()
                .SeatedMode();
        else
            GameObject.Find(avatarGameObjectName).GetComponent<ZigSkeleton>()
                .DisableSeatedMode();

        UpdateGuiText();
    }

    #endregion


    // Use this for initialization
	void Start () {
	    
        //enable seated mode
        //REMOVE SOON?
       // GameObject.Find(avatarGameObjectName).GetComponent<ZigSkeleton>().SeatedMode();
	}
	
	// Update is called once per frame
	void Update () {
	    
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
