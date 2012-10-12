/*
 * Class SerializeScript
 * Purpose: create an xml file describing the recorded movements
 * of the avatar
 */
///<summary>
///This class contains all the methods used to serialize and deserialize the list containing the game recording into an XML file
///It also contains the classes which are used to record the avatar's movements
///</summary>
using UnityEngine;
using System.Collections;
using System.IO; //for the textWriter class
using System.Xml.Serialization; //for the XML serializer class
using System.Collections.Generic; //for the list class

/// <summary>
/// This class contains all the methods used to serialize and deserialize recordings of avatar movements
/// The class which stores these recordings is a List of Snapshotclass objects. 
/// Snapshotclass objects store information for a single frame, and they are also defined in this class
/// </summary>
public class SerializeScript : MonoBehaviour
{
    
   
    #region Class definitions
    /// <summary>
    /// Obsolete class
    /// Used to take a still image of the avatar posing
    /// A list of Snapshot objects can be used to create a recording
    /// </summary>
    public class SnapshotClass //class to take a still of a pose
    {
        /// <summary>
        /// Stores rotation of each joint in an avatar in the form of a Quaternion
        /// </summary>
        public Quaternion[] jointRotations = new Quaternion[25]; //the joint data
        /// <summary>
        /// Marks if this frame is a key point
        /// </summary>
        public bool isKeyFrame = false; //flags if this still is a key point

    }

    /// <summary>
    /// This class keeps track of key point information. 
    /// Each exercise is composed of a checkpoint style set of key points
    /// </summary>
    public class KeyPoint //class keeping track of key points
    {
        /// <summary>
        /// The id of the frame corresponding to this key point
        /// </summary>
        public int frameID;
    }

    #endregion

    #region Serialization and Deserialization methods
    
 /*
 * function SaveTempList
 * purpose: serialize the list of snapshots into an XML file(hardcoded filename)
 * input/output: 
  *     input - List<SnapshotClass> list: a list of SnapshotClass objects
  *     output - an XML file
 * properties modified: none
 */
    /// <summary>
    /// Used to serialise a temporary recording into an XML file
    /// This method is called to temporarily store a recording before it is saved
    /// The filename is hardcoded
    /// </summary>
    /// <param name="list">The snapshot list to be saved</param>
    public static void SaveTempList(List<SnapshotClass> list)
    {
        //declare objects
        XmlSerializer serializer = new XmlSerializer(typeof(List<SnapshotClass>));
        TextWriter textWriter = new StreamWriter(@"tempList.tmp"); //filename is entered here

        //do things
        serializer.Serialize(textWriter, list); //serialize!
        textWriter.Close(); //close, housekeeping
    }

    /*
    * function LoadTempList
    * purpose: deserialize the data from an XML file (filename hardcoded)
     * into a list of SnapshotClass objects
    * input/output: 
     * output: List of SnapshotClass objects
    * properties modified: none
    */
    /// <summary>
    /// Deserialises a recording from a temporary file.
    /// Used for temp files, not for loading proper
    /// Because of this, the filename is hardcoded
    /// </summary>
    /// <returns>An recording of the Avatar's movements</returns>
    public static List<SnapshotClass> LoadTempList()
    {
        //declare variables / objects
        List<SnapshotClass> returnClass;
        XmlSerializer deserializer = new XmlSerializer(typeof(List<SnapshotClass>));
        TextReader textReader = new StreamReader("tempList.tmp");

        returnClass = (List<SnapshotClass>)deserializer.Deserialize(textReader); //deserialize!
        textReader.Close(); //housekeeping

        return returnClass;
    }

    #endregion

    /*
     * SaveToFile
     * purpose: Saves a given file to disk
     * input: 
     *  string fileName: name of destination file
     *  List<SnapshotClass> targetList: list to be saved
     * properties modified: none
     */
    /// <summary>
    /// Used to save recordings to disk
    /// </summary>
    /// <param name="fileName">Name of the target file</param>
    /// <param name="targetList">Pointer to the recording to be saved</param>
    public static void SaveToDisk(string fileName, List<SnapshotClass> targetList)
    {
        //declare objects
        XmlSerializer serializer = new XmlSerializer(typeof(List<SnapshotClass>));
        TextWriter textWriter = new StreamWriter(fileName); //filename is entered here

        //do things
        serializer.Serialize(textWriter, targetList); //serialize!
        textWriter.Close(); //close, housekeeping
    }
    
   /*
    * function deserializeFromDisk
    * purpose: deserialize the data from an XML file 
    * into a list of SnapshotClass objects
    * input/output: 
    *   input: filename - file name of the object
    *   output: List of SnapshotClass objects
    * properties modified: none
    */
    /// <summary>
    /// Deserialize a recording from an XML file
    /// Used to load a recording
    /// </summary>
    /// <param name="fileNameIn">filename of file to be loaded</param>
    /// <returns>Recording of avatar's movements</returns>
    public static List<SnapshotClass> deserializeFromDisk(string fileNameIn)
    {
        //declare variables / objects
        List<SnapshotClass> returnClass;
        XmlSerializer deserializer = new XmlSerializer(typeof(List<SnapshotClass>));
        TextReader textReader = new StreamReader(fileNameIn);

        returnClass = (List<SnapshotClass>)deserializer.Deserialize(textReader); //deserialize!
        textReader.Close(); //housekeeping

        return returnClass;
    }


}
