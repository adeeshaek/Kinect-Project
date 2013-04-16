using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// Extends Zigskeleton class and adds custom methods to it.
/// Should work with any version of Zigfu, however the transforms, initialRotations 
/// and rootPosition variables must be changed from private to protected for access
/// restrictions to work
/// </summary>
public class ExtendedZigSkeleton : ZigSkeleton {

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

}
