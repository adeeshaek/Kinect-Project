using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ZigSkeleton : MonoBehaviour 
{
	public Transform Head;
	public Transform Neck;
	public Transform Torso;
	public Transform Waist;

	public Transform LeftCollar;
	public Transform LeftShoulder;
	public Transform LeftElbow;
	public Transform LeftWrist;
	public Transform LeftHand;
	public Transform LeftFingertip;

	public Transform RightCollar;
	public Transform RightShoulder;
	public Transform RightElbow;
	public Transform RightWrist;
	public Transform RightHand;
	public Transform RightFingertip;

	public Transform LeftHip;
	public Transform LeftKnee;
	public Transform LeftAnkle;
	public Transform LeftFoot;

	public Transform RightHip;
	public Transform RightKnee;
	public Transform RightAnkle;
	public Transform RightFoot;
	public bool mirror = false;
	public bool UpdateJointPositions = false;
	public bool UpdateRootPosition = false;
	public bool UpdateOrientation = true;
	public bool RotateToPsiPose = false;
    public float RotationDamping = 30.0f;
    public float Damping = 30.0f;
	public Vector3 Scale = new Vector3(0.001f,0.001f,0.001f); 
	
	public Vector3 PositionBias = Vector3.zero;
	
	private Transform[] transforms;
	private Quaternion[] initialRotations;
	private Vector3 rootPosition;
	
	ZigJointId mirrorJoint(ZigJointId joint)
	{
		switch (joint) {
			case ZigJointId.LeftCollar:
				return ZigJointId.RightCollar;
			case ZigJointId.LeftShoulder:
				return ZigJointId.RightShoulder;
			case ZigJointId.LeftElbow:
				return ZigJointId.RightElbow;
			case ZigJointId.LeftWrist:
				return ZigJointId.RightWrist;
			case ZigJointId.LeftHand:
				return ZigJointId.RightHand;
			case ZigJointId.LeftFingertip:
				return ZigJointId.RightFingertip;
			case ZigJointId.LeftHip:
				return ZigJointId.RightHip;
			case ZigJointId.LeftKnee:
				return ZigJointId.RightKnee;
			case ZigJointId.LeftAnkle:
				return ZigJointId.RightAnkle;
			case ZigJointId.LeftFoot:
				return ZigJointId.RightFoot;
			
			case ZigJointId.RightCollar:
				return ZigJointId.LeftCollar;
			case ZigJointId.RightShoulder:
				return ZigJointId.LeftShoulder;
			case ZigJointId.RightElbow:
				return ZigJointId.LeftElbow;
			case ZigJointId.RightWrist:
				return ZigJointId.LeftWrist;
			case ZigJointId.RightHand:
				return ZigJointId.LeftHand;
			case ZigJointId.RightFingertip:
				return ZigJointId.LeftFingertip;
			case ZigJointId.RightHip:
				return ZigJointId.LeftHip;
			case ZigJointId.RightKnee:
				return ZigJointId.LeftKnee;
			case ZigJointId.RightAnkle:
				return ZigJointId.LeftAnkle;
			case ZigJointId.RightFoot:
				return ZigJointId.LeftFoot;
			
			
			default:
				return joint;
		}
	}
		
	/// <summary>
	/// Initializes Skeleton Tracking.
    /// The SEATED MODE SWITCH is here.
	/// </summary>
	
	public void Awake()
	{
		int jointCount = Enum.GetNames(typeof(ZigJointId)).Length;
		
		transforms = new Transform[jointCount];
		initialRotations = new Quaternion[jointCount];

        if (seatedModeOn)
            SeatedMode();

        transforms[(int)ZigJointId.Head] = Head;
        transforms[(int)ZigJointId.Neck] = Neck;
        transforms[(int)ZigJointId.Torso] = Torso;
        transforms[(int)ZigJointId.Waist] = Waist;
        transforms[(int)ZigJointId.LeftCollar] = LeftCollar;
        transforms[(int)ZigJointId.LeftShoulder] = LeftShoulder;
        transforms[(int)ZigJointId.LeftElbow] = LeftElbow;
        transforms[(int)ZigJointId.LeftWrist] = LeftWrist;
        transforms[(int)ZigJointId.LeftHand] = LeftHand;
        transforms[(int)ZigJointId.LeftFingertip] = LeftFingertip;
        transforms[(int)ZigJointId.RightCollar] = RightCollar;
        transforms[(int)ZigJointId.RightShoulder] = RightShoulder;
        transforms[(int)ZigJointId.RightElbow] = RightElbow;
        transforms[(int)ZigJointId.RightWrist] = RightWrist;
        transforms[(int)ZigJointId.RightHand] = RightHand;
        transforms[(int)ZigJointId.RightFingertip] = RightFingertip;
        transforms[(int)ZigJointId.LeftHip] = LeftHip;
        transforms[(int)ZigJointId.LeftKnee] = LeftKnee;
        transforms[(int)ZigJointId.LeftAnkle] = LeftAnkle;
        transforms[(int)ZigJointId.LeftFoot] = LeftFoot;
        transforms[(int)ZigJointId.RightHip] = RightHip;
        transforms[(int)ZigJointId.RightKnee] = RightKnee;
        transforms[(int)ZigJointId.RightAnkle] = RightAnkle;
        transforms[(int)ZigJointId.RightFoot] = RightFoot;
		
		
		
		// save all initial rotations
		// NOTE: Assumes skeleton model is in "T" pose since all rotations are relative to that pose
        foreach (ZigJointId j in Enum.GetValues(typeof(ZigJointId))) {
			if (transforms[(int)j])	{
				// we will store the relative rotation of each joint from the gameobject rotation
				// we need this since we will be setting the joint's rotation (not localRotation) but we 
				// still want the rotations to be relative to our game object
				initialRotations[(int)j] = Quaternion.Inverse(transform.rotation) * transforms[(int)j].rotation;
			}
		}
    }

    #region customGlobalVariables

    /// <summary>
    /// used to call the function ReturnFrameFunction 
    /// </summary>
    bool isReturningFrame = false;

    /// <summary>
    /// flags true if seated mode is on
    /// </summary>
    public bool seatedModeOn = false;

    /// <summary>
    /// List used to record user movement
    /// </summary>
    List<SerializeScript.SnapshotClass> recordList;

    /// <summary>
    /// A dict with backups of all the transfoms which 
    /// can be used to disable seated mode
    /// This has TransformName mapped to GameTransform
    /// e.g: LeftHip, Dana's LeftHip
    /// </summary>
    Dictionary<String, Transform> backupTransformList;

    #endregion

    #region customMethods

    /// <summary>
    /// Enables seated mode by setting the gameobjects for all the lower body
    /// poses to null. 
    /// Creates a backuplist of all joints which can be used to disable seated 
    /// mode
    /// </summary>
    public void SeatedMode()
    {

        //now set all of the lowerbody points to null

        transforms[(int)ZigJointId.Waist] = null;
        transforms[(int)ZigJointId.LeftHip] = null;
        transforms[(int)ZigJointId.LeftKnee] = null;
        transforms[(int)ZigJointId.LeftAnkle] = null;
        transforms[(int)ZigJointId.LeftFoot] = null;
        transforms[(int)ZigJointId.RightHip] = null;
        transforms[(int)ZigJointId.RightKnee] = null;
        transforms[(int)ZigJointId.RightAnkle] = null;
        transforms[(int)ZigJointId.RightFoot] = null;


    }

    /// <summary>
    /// Disables seated mode by reading the backupTransformList and restoring
    /// all of the transforms to the correct gameObjects
    /// 
    /// This also disables the bool
    /// </summary>
    public void DisableSeatedMode()
    {

        //restore the backupTransformList
        transforms[(int)ZigJointId.Waist] = Waist;
        transforms[(int)ZigJointId.LeftHip] = LeftHip;
        transforms[(int)ZigJointId.LeftKnee] = LeftKnee;
        transforms[(int)ZigJointId.LeftAnkle] = LeftAnkle;
        transforms[(int)ZigJointId.LeftFoot] = LeftFoot;
        transforms[(int)ZigJointId.RightHip] = RightHip;
        transforms[(int)ZigJointId.RightKnee] = RightKnee;
        transforms[(int)ZigJointId.RightAnkle] = RightAnkle;
        transforms[(int)ZigJointId.RightFoot] = RightFoot;


    }

    /// <summary>
    /// Makes the avatar assume the pose recorded in the frame passed in
    /// </summary>
    /// <param name="frameIn">The frame to be played</param>
    public void PlayFrame(SerializeScript.SnapshotClass frameIn)
    {
        //play frame
        foreach (ZigJointId j in Enum.GetValues(typeof(ZigJointId)))
        {
            if (null != transforms[(int)j])
            {
                transforms[(int)j].rotation = transform.rotation * frameIn.jointRotations[(int)j];
            }
        }

    }


    /// <summary>
    /// For testing use
    /// Makes the avatar jump to a frame 
    /// </summary>
    /// <param name="frameIn"></param>
    public void JumpToFrame(SerializeScript.SnapshotClass frameIn)
    {
        //store init rotation of avatar
        Quaternion initRotation = gameObject.GetComponent<Transform>().rotation;

        //play frame
        foreach (ZigJointId j in Enum.GetValues(typeof(ZigJointId)))
        {
            if (transforms.Length > (int)j) //check for out of bounds
            {
                if (null != transforms[(int)j])
                {
                    transforms[(int)j].rotation = transform.rotation * frameIn.jointRotations[(int)j];
                }
            }
        }

        //TESTING CODE
        //make character not rotate

        Transform temp = gameObject.GetComponent<Transform>();
        temp.rotation = initRotation;

    }

    /// <summary>
    /// Captures and returns quaternions describing rotations of each
    ///     joint of the avatar
    /// </summary>
    /// <returns>A List of Quaternions describing rotations of each joint</returns>
    public List<Quaternion> ReturnFullBodyPose()
    {
        //Instantiate new list
        List<Quaternion> Pose = new List<Quaternion>();

        //populate list with whole body's worth of poses
        foreach (ZigJointId j in Enum.GetValues(typeof(ZigJointId)))
        {
            if (transforms[(int)j])
            {
                Pose.Add(transforms[(int)j].rotation);
            }
        }
        // Debug.Log(Pose.Count);
        return Pose;
    }

    

    /// <summary>
    /// Callback function; called by Zig_UpdateUser
    /// 
    /// Returns a snapshot frame of the model's current position. Used by
    ///     TranslationLayer to record frames
    /// </summary>
    /// <returns>A frame to be recorded</returns>
    public SerializeScript.SnapshotClass ReturnFrameFunction()
    {
        //variables
        SerializeScript.SnapshotClass outputFrame = new SerializeScript.SnapshotClass(); 

        //init variable
        //populate with whole body's worth of poses
        foreach (ZigJointId j in Enum.GetValues(typeof(ZigJointId)))
        {
            if (transforms[(int)j])
            {
                outputFrame.jointRotations[(int)j] = transform.rotation;
            }
        }

        return outputFrame;
    }

    #region recording related methods

    /// <summary>
    /// Used to return the recorded list to the TranslationLayer
    /// </summary>
    /// <returns></returns>
    public List<SerializeScript.SnapshotClass> ReturnRecordedList()
    {
        return recordList;
        Debug.Log(recordList.Count + "Frames");
    }

    /// <summary>
    /// Called by TranslationLayer's tick every so often to record a frame
    ///     Tick is flagged false after every frame is captured
    /// </summary>
    public void SetTick()
    {
        isReturningFrame = true;
    }
    
    public void CaptureFrameFunction(ZigTrackedUser user)
    {
        //create frame
        SerializeScript.SnapshotClass frame =
            new SerializeScript.SnapshotClass();

        foreach (ZigInputJoint joint in user.Skeleton)
        {
            if (joint.GoodRotation) UpdateRotation(joint.Id, joint.Rotation);
            if (joint.GoodRotation)
            {
                frame.jointRotations[(int)joint.Id] = joint.Rotation; //store joint?
            }

        }

        recordList.Add(frame);

        isReturningFrame = false; //reset flag
    }

    /// <summary>
    /// Initializes and clears the record list
    /// </summary>
    public void ClearRecordingList()
    {
        recordList = new List<SerializeScript.SnapshotClass>();
        recordList.Clear();
    }
    #endregion





    #endregion
    void Start() 
    {
		// start out in calibration pose
		if (RotateToPsiPose) {
			RotateToCalibrationPose();
		}
	}
	
	void UpdateRoot(Vector3 skelRoot)
	{
        // +Z is backwards in OpenNI coordinates, so reverse it
        rootPosition = Vector3.Scale(new Vector3(skelRoot.x, skelRoot.y, skelRoot.z), doMirror(Scale)) + PositionBias;
		if (UpdateRootPosition) {
			transform.localPosition = (transform.rotation * rootPosition);
		}
	}
	
	void UpdateRotation(ZigJointId joint, Quaternion orientation)
	{
		joint = mirror ? mirrorJoint(joint) : joint;
        // make sure something is hooked up to this joint
        if (!transforms[(int)joint]) {
            return;
        }

        if (UpdateOrientation) {
			Quaternion newRotation = transform.rotation * orientation * initialRotations[(int)joint];
			if (mirror)
			{
				newRotation.y = -newRotation.y;
				newRotation.z = -newRotation.z;	
			}
			transforms[(int)joint].rotation = Quaternion.Slerp(transforms[(int)joint].rotation, newRotation, Time.deltaTime * RotationDamping);
        }
	}

	Vector3 doMirror(Vector3 vec)   
    {
        return new Vector3(mirror ? -vec.x : vec.x, vec.y, vec.z);
    }
	void UpdatePosition(ZigJointId joint, Vector3 position)
	{
		joint = mirror ? mirrorJoint(joint) : joint;
		// make sure something is hooked up to this joint
		if (!transforms[(int)joint]) {
			return;
		}
		
		if (UpdateJointPositions) {
            Vector3 dest = Vector3.Scale(position, doMirror(Scale)) - rootPosition;
			transforms[(int)joint].localPosition = Vector3.Lerp(transforms[(int)joint].localPosition, dest, Time.deltaTime * Damping);
		}
	}

	public void RotateToCalibrationPose()
	{
        foreach (ZigJointId j in Enum.GetValues(typeof(ZigJointId))) {
			if (null != transforms[(int)j])	{
				transforms[(int)j].rotation = transform.rotation * initialRotations[(int)j];
			}
		}
		
		// calibration pose is skeleton base pose ("T") with both elbows bent in 90 degrees
		if (null != RightElbow) {
            RightElbow.rotation = transform.rotation * Quaternion.Euler(0, -90, 90) * initialRotations[(int)ZigJointId.RightElbow];
		}
		if (null != LeftElbow) {
            LeftElbow.rotation = transform.rotation * Quaternion.Euler(0, 90, -90) * initialRotations[(int)ZigJointId.LeftElbow];
		}
	}
	
	public void SetRootPositionBias()
	{
		this.PositionBias = -rootPosition;
	}
	
	public void SetRootPositionBias(Vector3 bias)
	{
		this.PositionBias = bias;	
	}
	
	void Zig_UpdateUser(ZigTrackedUser user)
	{
		UpdateRoot(user.Position);
		if (user.SkeletonTracked) {
			foreach (ZigInputJoint joint in user.Skeleton) {
				if (joint.GoodPosition) UpdatePosition(joint.Id, joint.Position);
				if (joint.GoodRotation) UpdateRotation(joint.Id, joint.Rotation);
			}
		}

        if (isReturningFrame)
            CaptureFrameFunction(user);
	}

}
