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
    /*
     * List of sounds we want
     * ----------------------
     * Ambient sounds
     * * Background noise
     * * Music
     * * 
     * Voices and Sound
     * ----------------
     * Gesture Success
     * Gesture Fail
     * 
     * Start Game
     * End Game 
     * 
     */
    #region Sound Clips

    /// <summary>
    /// Encapsulates clips for sounds played when an exercise
    /// is completed
    /// </summary>
    [System.Serializable]
    public class GestureSuccessFail
    {
        public AudioClip success;
        public AudioClip fail;
    }

    /// <summary>
    /// Encapsulates audio clips for background music and background
    /// noise
    /// </summary>
    [System.Serializable]
    public class AmbientSounds
    {
        public AudioClip backgroundNoise;
        public AudioClip music;
    }

    /// <summary>
    /// Encapsulates audio clips for sounds played at the start and
    /// end of the game
    /// </summary>
    [System.Serializable]
    public class StartEndGame
    {
        public AudioClip startGame;
        public AudioClip endGame;
    }
    

    /// <summary>
    /// Instance of class ExerciseSuccessFail
    /// </summary>
    public GestureSuccessFail gestureSuccessFail;

    /// <summary>
    /// Instance of AmbientSounds
    /// </summary>
    public AmbientSounds ambientSounds;

    /// <summary>
    /// Instance of StartEndGame
    /// </summary>
    public StartEndGame startEndGame;

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
        mainAudioSource.clip = gestureSuccessFail.success;

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
        mainAudioSource.clip = gestureSuccessFail.fail;

        //play the clip
        mainAudioSource.Play();

    }

    /// <summary>
    /// Plays the sound on game start
    /// called by translation layer
    /// </summary>
    public void PlayGameStartSound()
    {
        //set the clip
        mainAudioSource.clip = startEndGame.startGame;

        //play clip
        mainAudioSource.Play();
    }

    /// <summary>
    /// plays the sound on game end
    /// called by translation layer
    /// </summary>
    public void PlayGameEndSound()
    {
        //set the clip
        mainAudioSource.clip = startEndGame.endGame;

        //play clip
        mainAudioSource.Play();
    }

    /// <summary>
    /// plays the background music
    /// called by translation layer
    /// </summary>
    public void PlayBackgroundNoise()
    {
        //set the clip
        mainAudioSource.clip = ambientSounds.backgroundNoise;

        //play clip
        mainAudioSource.Play();
    }

    /// <summary>
    /// plays the music
    /// called by translation layer
    /// </summary>
    public void PlayMusic()
    {
        //set the clip
        mainAudioSource.clip = ambientSounds.music;

        //play clip
        mainAudioSource.Play();
    }
    #endregion

    // Use this for initialization
	void Start () {
        mainAudioSource = gameObject.GetComponent<AudioSource>(); //reference to the Audiosource attached to the GameObject

        //make backgroundnoise and music play
        PlayBackgroundNoise();
        PlayMusic();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
