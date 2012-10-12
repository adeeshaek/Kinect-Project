using UnityEngine;
using System.Collections;

/// <summary>
/// This class handles all of the sounds
/// in the game. For each sound event,
/// a method is defined in this class, and
/// called from the translationLayer. 
/// 
/// The audio clips and audiosources are defined 
/// in this class
/// </summary>
public class SoundScript : MonoBehaviour
{

    #region Sound Clips

    /// <summary>
    /// Encapsulates clips for sounds played when an exercise
    /// is completed
    /// </summary>
    [System.Serializable]
    public class ExerciseSuccessFail
    {
        public AudioClip success;
        public AudioClip fail;
    }

    /// <summary>
    /// Instance of class ExerciseSuccessFail
    /// </summary>
    public ExerciseSuccessFail exerciseSuccessFail;

    #endregion

    #region Audio Sources

    /// <summary>
    /// The default Audiosource to use
    /// </summary>
    AudioSource mainAudioSource;

    #endregion

    #region Exercise Success and Failure

    /// <summary>
    /// Plays the sound on Succesful Gesture
    /// Called by translation layer
    /// </summary>
    public void PlayGestureSuccessSound()
    {
        //set the clip
        mainAudioSource.clip = exerciseSuccessFail.success;

        //play the clip
        mainAudioSource.Play();
    }

    /// <summary>
    /// Plays the sound on failed Gesture
    /// Called by translation layer
    /// </summary>
    public void PlayGestureFailSound()
    {
        //set the clip
        mainAudioSource.clip = exerciseSuccessFail.fail;

        //play the clip
        mainAudioSource.Play();

    }

    #endregion

    // Use this for initialization
	void Start () {
        mainAudioSource = gameObject.GetComponent<AudioSource>(); //reference to the Audiosource attached to the GameObject

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
