using UnityEngine;
using System.Collections;

/// <summary>
/// This class handles all of the sounds
/// in the game. For each sound event,
/// a method is defined in this class, and
/// called from the translationLayer. 
/// 
/// The audio clips and methods to call each
/// sound event are called from this class.
/// 
/// There are 3 gameobjects with audiosource
/// components attached. They are:
/// * AmbientSound
/// * Effects
/// * Music
/// * Voice
/// 
/// This script uses the above gameObjects to 
/// play the relevant clips 
/// 
/// </summary>
public class SoundScript : MonoBehaviour
{
    /*
     * List of sounds we want
     * ----------------------
     * Ambient sounds
     * * Background noise
     * * Music
     * * Game Background noise
     * * Game Music
     * 
     * Voices and Sound
     * ----------------
     * Gesture Success
     * Gesture Fail
     * 
     * Start Game
     * 
     */
    #region Global Variables

    #region Gameobjects

    /*
     * The gameobjects for AmbientSounds, Effects and Music
     * are each assigned a name, and the names are recorded 
     * here
     */

    /// <summary>
    /// name of gameObject which plays ambient sounds
    /// </summary>
    public static string AmbientSoundGameObjectName = "AmbientSound";

    /// <summary>
    /// name of gameobject which plays effects
    /// </summary>
    public static string EffectsGameObjectName = "Effects";

    /// <summary>
    /// name of gameobject which plays music
    /// </summary>
    public static string MusicGameObjectName = "Music";

    #endregion
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
    /// noise. These sounds only play once a "Game" has started.
    /// </summary>
    [System.Serializable]
    public class GameAmbientSounds
    {
        public AudioClip backgroundNoise;
        public AudioClip music;
    }
    
    /// <summary>
    /// Encapsulates audio clips for background music and background
    /// noise. These are ambient sounds which play before a game starts
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
    
    /*
     * Each of the above classes where sound clips are attached
     * are instantiated here. This is to make the unity menu neat
     */
    /// <summary>
    /// Instance of class ExerciseSuccessFail
    /// </summary>
    public GestureSuccessFail gestureSuccessFail;

    /// <summary>
    /// Instance of AmbientSounds
    /// </summary>
    public AmbientSounds ambientSounds;

    /// <summary>
    /// Instance of GameAmbientSounds
    /// </summary>
    public GameAmbientSounds gameAmbientSounds;

    /// <summary>
    /// Instance of StartEndGame
    /// </summary>
    public StartEndGame startEndGame;

    #endregion
    #endregion

    #region Audio Sources

    /// <summary>
    /// Source for playing ambient sounds
    /// </summary>
    AudioSource ambientSoundSource;

    /// <summary>
    /// Source for playing effects
    /// </summary>
    AudioSource effectsSource;

    /// <summary>
    /// Source for playing music
    /// </summary>
    AudioSource musicSource;

    #endregion

    #region Exercise Success and Failure

    /// <summary>
    /// Plays the sound on Succesful Gesture
    /// Called by translation layer
    /// </summary>
    public void PlayGestureSuccessSound()
    {
        //set the clip
        effectsSource.clip = gestureSuccessFail.success;

        //play the clip
        effectsSource.Play();
    }

    /// <summary>
    /// Plays the sound on failed Gesture
    /// Called by translation layer
    /// </summary>
    public void PlayGestureFailSound()
    {
        //set the clip
        effectsSource.clip = gestureSuccessFail.fail;

        //play the clip
        effectsSource.Play();

    }

    /// <summary>
    /// Plays the sound on game start
    /// called by translation layer
    /// </summary>
    public void PlayGameStartSound()
    {
        //set the clip
        effectsSource.clip = startEndGame.startGame;

        //play clip
        effectsSource.Play();
    }

    /// <summary>
    /// plays the sound on game end
    /// called by translation layer
    /// </summary>
    public void PlayGameEndSound()
    {
        //set the clip
        effectsSource.clip = startEndGame.endGame;

        //play clip
        effectsSource.Play();
    }

     /// <summary>
    /// plays the In-game background music
    /// called by translation layer
    /// </summary>
    public void PlayGameBackgroundNoise()
    {
        //set the clip
        ambientSoundSource.clip = gameAmbientSounds.backgroundNoise;

        //play clip
        ambientSoundSource.Play();
    }

    /// <summary>
    /// plays the in-game music
    /// called by translation layer
    /// </summary>
    public void PlayGameMusic()
    {
        //set the clip
        musicSource.clip = gameAmbientSounds.music;

        //play clip
        musicSource.Play();
    }

    /// <summary>
    /// Stops playing all ambient music
    /// called by translation layer
    /// </summary>
    public void StopAmbientMusic()
    {
        musicSource.Stop();
        ambientSoundSource.Stop();
    }
    
    /// <summary>
    /// plays the ambient background music
    /// called by translation layer
    /// </summary>
    public void PlayBackgroundNoise()
    {
        //set the clip
        ambientSoundSource.clip = ambientSounds.backgroundNoise;

        //play clip
        ambientSoundSource.Play();
    }

    /// <summary>
    /// plays the ambient music
    /// called by translation layer
    /// </summary>
    public void PlayMusic()
    {
        //set the clip
        musicSource.clip = ambientSounds.music;

        //play clip
        musicSource.Play();
    }
    #endregion

    // Use this for initialization
	void Start () {
        /*
         * Initializing audio Sources
         * --------------------------
         */
        ambientSoundSource = GameObject.Find(AmbientSoundGameObjectName)
            .GetComponent<AudioSource>();
        effectsSource = GameObject.Find(EffectsGameObjectName)
            .GetComponent<AudioSource>();
        musicSource = GameObject.Find(MusicGameObjectName)
            .GetComponent<AudioSource>();

        //make backgroundnoise and music play from the start of the game
        PlayBackgroundNoise();
        PlayMusic();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
