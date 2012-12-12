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

    #region custom Global vars

    /// <summary>
    /// GUI feedback text
    /// </summary>
    public string GUIOutputText1 = "1";

    /// <summary>
    /// More GUI feedback text
    /// </summary>
    public string GUIOutputText2 = "2";

    /// <summary>
    /// The speed at which the indicator object is animated
    /// </summary>
    public float animateSpeed = 1;

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

<<<<<<< HEAD
=======

>>>>>>> 5c72776d2527ac6ccba102ce87e900f53363801c
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

<<<<<<< HEAD
=======

>>>>>>> 5c72776d2527ac6ccba102ce87e900f53363801c
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
    /// Make the model smoothly animate to the supplied pose. Animation is a Slerp
    /// 
    /// This is a Slerp animation, so it should be called repeatedly through a tickEvent.
    /// </summary>
    /// <param name="frameIn">Frame to animate to</param>
    public void AnimateToFrame(SerializeScript.SnapshotClass frameIn)
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
                    //transforms[(int)j].rotation = transform.rotation * frameIn.jointRotations[(int)j];
                    transforms[(int)j].rotation =
                        Quaternion.Slerp(transforms[(int)j].rotation, transform.rotation * frameIn.jointRotations[(int)j], Time.deltaTime * animateSpeed);

                }
            }
        }

        //TESTING CODE
        //make character not rotate

        Transform temp = gameObject.GetComponent<Transform>();
        temp.rotation = initRotation;

    }

    /// <summary>
    /// Check if the avatar's current pose is the same as the pose
    /// passed in
    /// </summary>
    /// <param name="frameIn"></param>
    public void CheckForPose(SerializeScript.SnapshotClass frameIn,
        Quaternion leftShoulderPoseIn)
    {
        //declare vars
        float avatarX, avatarY, avatarZ,
            templateX, templateY, templateZ,
            xDiff, yDiff, zDiff;

        //instantiate vars
        /*
        templateX = frameIn.jointRotations[(int)ZigJointId.LeftShoulder].eulerAngles.x;
        templateY = frameIn.jointRotations[(int)ZigJointId.LeftShoulder].eulerAngles.y;
        templateZ = frameIn.jointRotations[(int)ZigJointId.LeftShoulder].eulerAngles.z;
         */

        //get quaternion of local left shoulder
        Quaternion localLeftShoulder =
            transforms[(int)ZigJointId.LeftShoulder].rotation;

        //define vectors to compare
        Vector2 local, template;

        local = localLeftShoulder * Vector3.forward;
        template = leftShoulderPoseIn * Vector3.forward;

        Vector3.Normalize(local);
        Vector3.Normalize(template);

        //Quaternion leftShoulderInverse = Quaternion.Inverse(leftShoulderPoseIn);

        string QuatDotProduct = Quaternion.Dot(leftShoulderPoseIn, localLeftShoulder).ToString();

        string dotProduct =
            Vector3.Dot(local, template).ToString();
        //GUIOutputText1 = outputString1;
        //GUIOutputText2 = outputString2;

        GUIOutputText1 = dotProduct;
        GUIOutputText2 = QuatDotProduct;

        //Now redo using dot product of euclidian vectors
        

    }

    public double CheckForFullBodyPose(List<Quaternion> PosesIn)
    {
        //declare variables
        double dotProduct, sum = 0, counter = 0, score; //variables used to calculate score

        Vector2 localJoint, templateJoint; 


            //iterate through all the joints
            foreach (ZigJointId j in Enum.GetValues(typeof(ZigJointId)))
            {

                if (transforms[(int)j] != null && PosesIn[(int)j] != null && (int)j < PosesIn.Count)
                {
                    /*
                    //instantiate vector joints
                    localJoint = transforms[(int)j].rotation * Vector3.forward;
                    templateJoint = PosesIn[(int)j] * Vector3.forward;

                    //normalize vectors
                    Vector3.Normalize(localJoint);
                    Vector3.Normalize(templateJoint);

                    //compare vectors
                    dotProduct = Vector3.Dot(localJoint, templateJoint);
                     */
                    //compare quaternions
                    dotProduct = Quaternion.Dot(transforms[(int)j].rotation, PosesIn[(int)j]);

                    //remove the sign from the dotProduct
                    if (dotProduct < 0)
                        dotProduct *= -1;

                    sum += dotProduct; //sum is the aggregate of dotProducts

                    counter++; //counts number of joints

                }
            }

        //calculate return value
        score = sum / counter;

        GUIOutputText1 = score.ToString();
        GUIOutputText2 = sum.ToString();

        return score;
    }

    /// <summary>
    /// FOR Testing purposes
    /// Captures and returns the quaternion describing the rotation
    /// of the left shoulder of the avatar
    /// </summary>
    /// <returns>Quaternion describing rotation of the left shoulder</returns>
    public Quaternion ReturnLeftArmPose()
    {
        return transforms[(int)ZigJointId.LeftShoulder].rotation;
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
    /// Used to get a list of all the joint transforms in a character
    /// </summary>
    /// <returns>List of all the transforms</returns>
    public List<Transform> ReturnTransformsList()
    {
        //instantiate new list
        List<Transform> transformsList = new List<Transform>();

        //populate list with whole body's worth of transforms
        foreach (ZigJointId j in Enum.GetValues(typeof(ZigJointId)))
        {
            if ((int)j < transforms.Length)
            {
                if (transforms[(int)j])
                {
                    transformsList.Add(transforms[(int)j]);
                }
            }
        }

        return transformsList;
    }



    #endregion

    public void Awake()
	{
		int jointCount = Enum.GetNames(typeof(ZigJointId)).Length;
		
		transforms = new Transform[jointCount];
		initialRotations = new Quaternion[jointCount];
		
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
	}

}
