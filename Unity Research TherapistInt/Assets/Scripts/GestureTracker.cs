using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Tracks gesutures to see if player has assumed the
/// pose necessary to keep recording
/// </summary>
public class GestureTracker : MonoBehaviour {

    /// <summary>
    /// flagged if is listening for gestures
    /// </summary>
    bool isListeningForGestures
    {
        get;
        set;
    }

    /// <summary>
    /// flags whether or not the tracking clone is initiated
    /// </summary>
    bool trackingCloneInitiated
    {
        get;
        set;
    }

    /// <summary>
    /// ref to translation layer
    /// </summary>
    public GameObject translationLayer;

    /// <summary>
    /// reference to the prefab which is cloned in order to check
    /// if the user is holding the pose. This prefab is cloned
    /// at the same location as the avatar, and it is made to assume
    /// the pose that is checked for. Then the vector distance between
    /// respective joints in the avatar and the clone are checked.
    /// If the distance is less than the threshold, the pose was held
    /// succesfully
    /// </summary>
    public GameObject trackingClonePrefab;

    /// <summary>
    /// ref to avatar
    /// </summary>
    public GameObject avatar;

    /// <summary>
    /// the tracking clone
    /// </summary>
    GameObject trackingClone;

    /// <summary>
    /// filename of the main pose
    /// </summary>
    static string gestureTrackingPoseFileName = "therapist_pose.xml";

    /// <summary>
    /// pose to start recording
    /// </summary>
    SerializeScript.SnapshotClass gestureTrackingPose;

    /// <summary>
    /// The threshold value for allowed drift between the 
    /// recorded pose and the held pose
    /// </summary>
    public float gestureTrackingThreshold = 0.8f;

    #region low-level methods

    /// <summary>
    /// instantiates the tracking clone
    /// </summary>
    private void initiateTrackingClone()
    {
        Vector3 avatarPosition =
            avatar.GetComponent<Transform>().position;

        Quaternion avatarRotation =
            avatar.GetComponent<Transform>().rotation;

        trackingClone 
            = Instantiate(trackingClonePrefab, avatarPosition, avatarRotation) as GameObject;

        trackingCloneInitiated = true;
    }

    /// <summary>
    /// makes the tracking clone assume recording pose
    /// </summary>
    private void trackingCloneJumpToPose()
    {
        trackingClone.GetComponent<ExtendedZigSkeleton>()
            .JumpToFrame(gestureTrackingPose);
    }

    /// <summary>
    /// destroys the tracking clone
    /// </summary>
    private void destroyTrackingClone()
    {
        Destroy(trackingClone);
    }

    #endregion

    /// <summary>
    /// checks the pose to see if it is being held by the user
    /// </summary>
    public void listenForGesture()
    {

        bool isHoldingPose = false;
        List<Transform> avatarPose, targetPose;

        avatarPose = avatar.GetComponent<ExtendedZigSkeleton>().ReturnTransformsList();
        targetPose = trackingClone.GetComponent<ExtendedZigSkeleton>().ReturnTransformsList();

        isHoldingPose = CheckForPose(avatarPose, targetPose);

        if (isHoldingPose)
            gestureSuccesfullyHeld();
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
        if (sum < gestureTrackingThreshold)
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
    /// makes the gesturechecker start listening for
    /// gestures
    /// </summary>
    public void startListeningForGestures()
    {
        isListeningForGestures = true;
        initiateTrackingClone();
        trackingCloneJumpToPose();
    }

    /// <summary>
    /// makes the gesturechecker stop listening for gestures
    /// </summary>
    public void stopListeningForGestures()
    {
        destroyTrackingClone();
    }

    void gestureSuccesfullyHeld()
    {
        destroyTrackingClone();
        isListeningForGestures = false;
        Debug.Log("Gesture Succesfully held");
        translationLayer.GetComponent<TranslationLayer>().GestureSuccesfullyHeld();
    }

    /// <summary>
    /// loads the gestureTrackingPose
    /// </summary>
    void loadTherapistPose()
    {
        List<SerializeScript.SnapshotClass> gestureList;   
        gestureList =
            SerializeScript.deserializeFromDisk(gestureTrackingPoseFileName);
        gestureTrackingPose = gestureList[0];
    }

    /// <summary>
    /// called with every fixedupdate. Called from the TranslationLayer
    /// </summary>
    public void tickEvent()
    {
        if (isListeningForGestures)
        {
            listenForGesture();
        }
    }

	// Use this for initialization
	void Start () {

        loadTherapistPose();
        isListeningForGestures = false;
        trackingCloneInitiated = false;

	}

       
}
