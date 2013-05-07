using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
        Stopped,
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
    /// ref to slider
    /// </summary>
    public GameObject Slider;

    /// <summary>
    /// name of big Guitext gameobject
    /// </summary>
    public GameObject bigGuiTextObject;

    /// <summary>
    /// settings menu
    /// </summary>
    public GameObject settingsMenu;

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
                    AddKPButtonPressed();
                    break;

                case "RemoveKPButton":
                    RemoveKPButtonPressed();
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

                case "Settings":
                    showSettingsView();
                    break;

                default:
                    //fill in later
                    break;

            }

        }

	}

    /// <summary>
    /// Adds a new key point
    /// </summary>
    public void AddKPButtonPressed()
    {
        //adding frame 0 for now
        translationLayerObject.GetComponent<TranslationLayer>().AddKeyPoint();
        keyPointsPanel.GetComponent<KeyPointsListPanel>().AddItem(currentIndex.ToString());
        setStatus("Key Point " + currentIndex + " added");
        currentIndex++;
    }
    
    /// <summary>
    /// Removes a new key point
    /// </summary>
    public void RemoveKPButtonPressed()
    {
        int currentlySelectedKeyPoint 
            = keyPointsPanel.GetComponent<KeyPointsListPanel>().getCurrentlySelectedKeyPoint();

        translationLayerObject.GetComponent<TranslationLayer>().DeleteKeyPoint(currentlySelectedKeyPoint);

        bool success = keyPointsPanel.GetComponent<KeyPointsListPanel>().RemoveItem();
        if (success)
            setStatus("Key Point " + " removed");
        else
            setStatus("Key Point not removed");
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
            case ButtonState.Stopped:
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

        else
        {
            stop_playing();
        }

    }

    /// <summary>
    /// stops playback
    /// </summary>
    public void stop_playing()
    {
        setStatus("Playback Stopped");
        playButtonStatus = ButtonState.Stopped;
        setPlayPauseButtonStatus(ButtonState.Paused);
        translationLayerObject.GetComponent<TranslationLayer>().StopPlaying();
    }

    /// <summary>
    /// starts playing
    /// </summary>
    public void pause_playing()
    {
        setStatus("Playback Paused");
        playButtonStatus = ButtonState.Paused;
        setPlayPauseButtonStatus(ButtonState.Paused);
        translationLayerObject.GetComponent<TranslationLayer>().PausePlaying();
    }

    /// <summary>
    /// stops playing
    /// </summary>
    public void start_playing()
    {
        setStatus("Playing Recorded Movements");
        playButtonStatus = ButtonState.Playing;
        setPlayPauseButtonStatus(ButtonState.Playing);
        translationLayerObject.GetComponent<TranslationLayer>().StartPlaying();
    }

    /// <summary>
    /// starts recording
    /// </summary>
    public void start_recording()
    {
        translationLayerObject.GetComponent<TranslationLayer>().StartRecordingInstruction();
        setBigGuiStatus("Please assume pose to record");
        setStatus("Recording");
        recordButtonStatus = ButtonState.Recording;
        setRecordButtonStatus(ButtonState.Recording);
    }

    /// <summary>
    /// stops recording
    /// </summary>
    public void stop_recording()
    {
        translationLayerObject.GetComponent<TranslationLayer>().StopRecordingInstruction();
        setStatus("Stopping Record");
        recordButtonStatus = ButtonState.Not_Recording;
        setRecordButtonStatus(ButtonState.Not_Recording);
        makeBigGuiDisappear();
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
    /// triggered when "new file" button pressed
    /// </summary>
    public void newButtonPressed()
    {
        translationLayerObject.GetComponent<TranslationLayer>().NewFile();

        setStatus("Created New File");
    }

    #region key points list related callbacks

    /// <summary>
    /// removes the key point. called by KeyPointsListPanel
    /// </summary>
    /// <param name="index"></param>
    public void removeKeyPoint(int index)
    {
        Debug.Log("Delete called");
        translationLayerObject.GetComponent<TranslationLayer>().DeleteKeyPoint(index);
    }


    #region IO related callbacks

    /// <summary>
    /// triggered when save dialog selects a file to save to 
    /// </summary>
    /// <param name="fileName">file to save to</param>
    public void savedFile(string filePath, string fileName)
    {
        setStatus("Saved file to " + fileName);
        translationLayerObject.GetComponent<TranslationLayer>().SaveList(filePath);
    }

    /// <summary>
    /// triggered when export dialog selects a file
    /// </summary>
    /// <param name="fileName">name of file to export to</param>
    public void exportedFile(string filePath, string fileName)
    {
        setStatus("Exported file to " + fileName);
        translationLayerObject.GetComponent<TranslationLayer>().ExportList(filePath);
    }

    /// <summary>
    /// triggered when file open dialog selects a file
    /// </summary>
    /// <param name="fileName">name of file to open</param>
    public void openedFile(string filePath, string fileName)
    {
        setStatus("Opened file " + fileName);
        translationLayerObject.GetComponent<TranslationLayer>().LoadList(filePath);
    }

    /// <summary>
    /// updates the key points list with the given playback list. 
    /// called from translationlayer
    /// </summary>
    /// <param name="playbackList"></param>
    public void updateKeyPointsList(List<SerializeScript.KeyPoint> playbackList)
    {
        //clear the list
        keyPointsPanel.GetComponent<KeyPointsListPanel>().ClearList();

        Debug.Log("Playlist size: " + playbackList.Count);

        //add each of the kps to the list
        for (int i = 0; i < playbackList.Count; i++)
        {
            keyPointsPanel.GetComponent<KeyPointsListPanel>().AddItem("Key Point " + i);
        }

    }

    #endregion

    #endregion

    /// <summary>
    /// Callback called every time slider value is changed
    /// </summary>
    /// <param name='value'>
    /// float value of the slider (between 0 and 1)
    /// </param>
    public void OnSliderChange(float value)
	{
        translationLayerObject.GetComponent<TranslationLayer>().HandleSliderChange(value);
	}

    /// <summary>
    /// sets slider value
    /// </summary>
    /// <param name="currentFrameIndexIn"></param>
    /// <param name="maxFramesIn"></param>
    public void UpdateSlider(int currentFrameIndexIn, int maxFramesIn)
    {
        float sliderValue;
        if (maxFramesIn == 0)
            sliderValue = 0.0f;
        else
            sliderValue = ((float)currentFrameIndexIn / (float)maxFramesIn);
        
        Slider.GetComponent<UISlider>().sliderValue = sliderValue;
        Slider.GetComponent<UISlider>().ForceUpdate();
    }

    public void Start()
    {

    }

    /// <summary>
    /// makes system sleep for a given time
    /// </summary>
    /// <param name="seconds"></param>
    /// <returns></returns>
    IEnumerator waitForSeconds(int seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    #region big gui message related methods

    /// <summary>
    /// sets the text of the big gui message. makes it active,
    /// then makes it visible for several seconds, then makes 
    /// it inactive
    /// </summary>
    /// <param name="statusMessage"></param>
    void setBigGuiStatus(string statusMessage)
    {
        bigGuiTextObject.GetComponent<BigGUIScript>().displayText(statusMessage);        
    }

    /// <summary>
    /// makes the big gui disappear
    /// </summary>
    public void makeBigGuiDisappear()
    {
        bigGuiTextObject.GetComponent<BigGUIScript>().makeTextDisappear();
    }
    #endregion

    #region settings menu related methods

    /// <summary>
    /// shows the settings view
    /// </summary>
    public void showSettingsView()
    {
        setStatus("Opening Settings Menu");
        settingsMenu.SetActiveRecursively(true);
    }

    #endregion

}
