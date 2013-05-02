using UnityEngine;
using System.Collections;

/// <summary>
/// Recieves actions created by NGUI
/// </summary>

public class UIMenuActionReciever : MonoBehaviour
{

    #region global variables

    #region gui_related global variables
    /// <summary>
    /// enum which checks if play button is playing or is paused
    /// </summary>
    public enum ButtonState
    {
        Playing,
        Paused,
        Recording,
        Not_Recording
    };

    /// <summary>
    /// instance of enum which checks state of play button
    /// </summary>
    ButtonState playButtonStatus = ButtonState.Paused;

    /// <summary>
    /// Instance of enum which checks the state of the Record button
    /// </summary>
    ButtonState recordButtonStatus = ButtonState.Not_Recording;

    /// <summary>
    /// Currently selected file. Set by UILoadPanel
    /// or UISavePanel
    /// </summary>
    public string currentlySelectedFile;

    /// <summary>
    /// reference to key points panel
    /// </summary>
    public GameObject keyPointsPanel;

    /// <summary>
    /// reference to status label
    /// </summary>
    public GameObject StatusLabel;

    /// <summary>
    /// reference to load panel
    /// </summary>
    public GameObject LoadGroup;

    /// <summary>
    /// reference to save panel
    /// </summary>
    public GameObject SaveGroup;

    /// <summary>
    /// reference to export panel
    /// </summary>
    public GameObject ExportGroup;

    /// <summary>
    /// reference to play button
    /// </summary>
    public GameObject PlayButton;

    /// <summary>
    /// reference to record button
    /// </summary>
    public GameObject RecordButton;

    /// <summary>
    /// selected index
    /// </summary>
    int currentIndex = 0;

    #endregion

    #region tracking related global vars

    /// <summary>
    /// reference to translation layer object
    /// </summary>
    public GameObject translationLayerObject;

    #endregion

    #endregion

    #region main callback method

    /// <summary>
	/// Callback which is called every time a button in the UI
	/// is selected	
	/// </summary>
	/// <param name='item'>
	/// Name of the item which was selected
	/// </param>
	public void OnSelectionChange(string item)
	{
		
		//NGUI sometimes fires randomly at the start of the 
		//level. This hack can get around that.
        if (Time.timeSinceLevelLoad > 1)
        {

            //big swich statement to decide what action to take
            switch (item)
            {
                case "AddKPButton":
                    //adding frame 0 for now
                    keyPointsPanel.GetComponent<UIListPanel>().AddItem(currentIndex.ToString());
                    setStatus("Key Point " + currentIndex + " added");
                    currentIndex++;
                    break;

                case "RemoveKPButton":
                    bool success = keyPointsPanel.GetComponent<UIListPanel>().RemoveItem();
                    if (success)
                        setStatus("Key Point " + " removed");
                    else
                        setStatus("Key Point not removed");

                    break;

                case "PlayButton":
                    setStatus("Playing");
                    break;

                case "StopButton":
                    setStatus("Paused");
                    break;

                case "Open":
                    openFileButtonPressed();
                    break;

                case "Save":
                    saveFileButtonPressed();
                    break;

                case "Export":
                    exportFileButtonPressed();
                    break;

                case "New":
                    newButtonPressed();
                    break;

                case "Play":
                    playPauseButtonPressed();
                    break;

                case "Stop":
                    stopButtonPressed();
                    break;

                case "Record":
                    recordButtonPressed();
                    break;

                case "Pause":
                    playPauseButtonPressed();
                    break;

                default:
                    //fill in later
                    break;

            }

        }

	}

    /// <summary>
    /// triggered when a kp is selected in the kp list  
    /// </summary>
    /// <param name="keyPointIndex">Index of the KP selected</param>
    public void KeyPointsListClicked(int keyPointIndex)
    {
        setStatus("Jumping to Key Point " + keyPointIndex);
        translationLayerObject.GetComponent<TranslationLayer>().JumpToKeyPoint(keyPointIndex);
    }

    #endregion

    /// <summary>
    /// changes the status of the play/pause button
    /// </summary>
    /// <param name="newStatus"></param>
    public void setPlayPauseButtonStatus(ButtonState newStatus)
    {
        switch (newStatus)
        {

            case ButtonState.Paused:
                PlayButton.GetComponentInChildren<UISprite>().spriteName = "play";
                break;

            case ButtonState.Playing:
                PlayButton.GetComponentInChildren<UISprite>().spriteName = "pause";
                break;

        }
    }

    /// <summary>
    /// changes the status of the record button
    /// </summary>
    /// <param name="newStatus"></param>
    public void setRecordButtonStatus(ButtonState newStatus)
    {
        switch (newStatus)
        {

            case ButtonState.Recording:
                RecordButton.GetComponentInChildren<UISprite>().spriteName = "record_pressed";
                break;

            case ButtonState.Not_Recording:
                RecordButton.GetComponentInChildren<UISprite>().spriteName = "record_normal";
                break;

        }
    }

    /// <summary>
    /// called when the play button is pressed
    /// </summary>
    public void playPauseButtonPressed()
    {
        //check the status and switch accordingly
        switch (playButtonStatus)
        {
            case ButtonState.Paused:
                start_playing();
                break;

            case ButtonState.Playing:
                pause_playing();
                break;
        }
    }

    /// <summary>
    /// called when the play button is pressed
    /// </summary>
    public void recordButtonPressed()
    {
        //check the status and switch accordingly
        switch (recordButtonStatus)
        {
            case ButtonState.Recording:
                stop_recording();
                break;

            case ButtonState.Not_Recording:
                start_recording();
                break;
        }
    }

    /// <summary>
    /// called when the stop button is pressed
    /// </summary>
    public void stopButtonPressed()
    {
        if (recordButtonStatus == ButtonState.Recording)
        {
            stop_recording();
        }

        else if (playButtonStatus == ButtonState.Playing)
        {
            stop_playing();
        }

        else setStatus("Nothing to stop, sorry!");
    }

    /// <summary>
    /// stops playback
    /// </summary>
    public void stop_playing()
    {
        setStatus("Playback Stopped");
        playButtonStatus = ButtonState.Playing;
        setPlayPauseButtonStatus(ButtonState.Paused);
    }

    /// <summary>
    /// starts playing
    /// </summary>
    public void pause_playing()
    {
        setStatus("Playback Paused");
        playButtonStatus = ButtonState.Paused;
        setPlayPauseButtonStatus(ButtonState.Paused);
    }

    /// <summary>
    /// stops playing
    /// </summary>
    public void start_playing()
    {
        setStatus("Playing Recorded Movements");
        playButtonStatus = ButtonState.Playing;
        setPlayPauseButtonStatus(ButtonState.Playing);
    }

    /// <summary>
    /// starts recording
    /// </summary>
    public void start_recording()
    {
        setStatus("Recording");
        recordButtonStatus = ButtonState.Recording;
        setRecordButtonStatus(ButtonState.Recording);
    }

    /// <summary>
    /// stops recording
    /// </summary>
    public void stop_recording()
    {
        setStatus("Stopping Record");
        recordButtonStatus = ButtonState.Not_Recording;
        setRecordButtonStatus(ButtonState.Not_Recording);
    }

    /// <summary>
    /// triggered when "new file" button pressed
    /// </summary>
    public void newButtonPressed()
    {
        setStatus("Created New File");
    }

    /// <summary>
    /// triggered when save dialog selects a file to save to 
    /// </summary>
    /// <param name="fileName">file to save to</param>
    public void savedFile(string filePath, string fileName)
    {
        setStatus("Saved file to " + fileName);
    }

    /// <summary>
    /// triggered when export dialog selects a file
    /// </summary>
    /// <param name="fileName">name of file to export to</param>
    public void exportedFile(string filePath, string fileName)
    {
        setStatus("Exported file to " + fileName);
    }

    /// <summary>
    /// triggered when file open dialog selects a file
    /// </summary>
    /// <param name="fileName">name of file to open</param>
    public void openedFile(string filePath, string fileName)
    {
        setStatus("Opened file " + fileName);
    }

    /// <summary>
    /// opens the file open dialog
    /// </summary>
    public void openFileButtonPressed()
    {
        setStatus("Opening File");
        LoadGroup.SetActiveRecursively(true);
    }

    /// <summary>
    /// saves the file save dialog
    /// </summary>
    public void saveFileButtonPressed()
    {
        setStatus("Saving to File");
        SaveGroup.SetActiveRecursively(true);
    }

    /// <summary>
    /// exports the file export dialog
    /// </summary>
    public void exportFileButtonPressed()
    {
        setStatus("Exporting to File");
        ExportGroup.SetActiveRecursively(true);
    }

    public void setStatus(string status)
    {
        StatusLabel.GetComponent<UILabel>().text = status;
    }

	/// <summary>
	/// Callback called every time slider value is changed
	/// </summary>
	/// <param name='value'>
	/// float value of the slider (between 0 and 1)
	/// </param>
	public void OnSliderChange(float value)
	{
		Debug.Log ("Slider changed to " + value);
	}

    public void Start()
    {

    }

}
