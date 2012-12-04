using UnityEngine;
using System.Collections;
using System.Threading; //for timer class
using System.Collections.Generic; //for linked list
using System; //for EventArgs



public class ButtonScript : MonoBehaviour
{
    #region Global Vars

    //declare the GUI controls
    /// <summary>
    /// Two button panels
    /// </summary>
    public myGUIControls.HorizontalButtonPanel topPanel, bottomPanel;

    /// <summary>
    /// Lists out the Key points
    /// </summary>
    public myGUIControls.GUIList keyPointsList;

    /// <summary>
    /// Play/Pause Button
    /// </summary>
    public myGUIControls.ComplexButton playPauseButton;

    /// <summary>
    /// Slider control
    /// </summary>
    public myGUIControls.IBoxHSlider slider;

    /// <summary>
    /// Dialog box used to load and save
    /// </summary>
    public myGUIControls.SaveDialog loadSaveDialog;

    //global booleans
    /// <summary>
    /// Flags true if a recording is being played
    /// </summary>
    public bool isPlaying;

    /// <summary>
    /// Flags true if user movements are being recorded
    /// </summary>
    public bool isRecording;

    /// <summary>
    /// Flags true if user is being tracked
    /// </summary>
    public bool isTracking;

    //names of gameObjects
    /// <summary>
    /// The name of the gameobject to which the translationLayer script is attached to 
    /// </summary>
    public static string translationLayerObjectName = "TranslationLayer";

    /// <summary>
    /// Name of the gameObject the ZigSkeleton script is attached to
    /// (in other words, the avatar)
    /// </summary>
    public static string avatarObjectName = "Dana";

    #endregion

    #region event handlers
    
    /// <summary>
    /// Handles event raised when a button on the top panel is clicked
    /// </summary>
    /// <param name="Sender"></param>
    /// <param name="e"></param>
    public void TopPanelButtonClicked(object Sender, EventArgs e)
    {
        //find out which button was clicked, and call the appropriate handler
        switch (topPanel.getOutputIndex())
        {
            case 0:
                RecordingButtonClicked();
                break;

            case 1:
                TrackingButtonClicked();
                break;

            case 2:
                ResetButtonClicked();
                break;

            case 3:
                LoadButtonClicked();
                break;

            case 4:
                SaveButtonClicked();
                break;

            case 5:
                ExportButtonClicked();
                break;


        }
    }



    /// <summary>
    /// Handles event raised when a button on the bottom panel is clicked
    /// </summary>
    /// <param name="Sender"></param>
    /// <param name="e"></param>
    public void BottomPanelButtonClicked(object Sender, EventArgs e)
    {
        //find out which button was clicked and call the appropriate handler
        switch (bottomPanel.getOutputIndex())
        {
            case 0:
                SetStartButtonClicked();
                break;

            case 1:
                SetEndButtonClicked();
                break;

            case 2:
                NewKPButtonClicked();
                break;

            case 3:
                DeleteKPButtonClicked();
                break;

            case 4:
                UndoButtonClicked();
                break;

            case 5:
                SeatedModeButtonClicked();
                break;
        }
    }

    /// <summary>
    /// Handles event raised when a button on the keypoints list is clicked
    /// </summary>
    /// <param name="Sender"></param>
    /// <param name="e"></param>
    public void KeyPointsListClicked(object Sender, EventArgs e)
    {
        //jump to the keypoint
        GameObject.Find(translationLayerObjectName).GetComponent<TranslationLayer>().JumpToKeyPoint(keyPointsList.outputIndex);
    }

    /// <summary>
    /// Handles event raised when the play/pause button is clicked
    /// </summary>
    /// <param name="Sender"></param>
    /// <param name="e"></param>
    public void PlayPauseButtonClicked(object Sender, EventArgs e)
    {
        if (isPlaying)
        {
            //call pause function
            GameObject.Find(translationLayerObjectName).GetComponent<TranslationLayer>()
                .PausePlaying();

            //change button text
            playPauseButton.text = "Play";
            isPlaying = false;
        }

        else
        {
            //call play function
            GameObject.Find(translationLayerObjectName).GetComponent<TranslationLayer>()
                .StartPlaying();
            //change button text
            playPauseButton.text = "Pause";
            //set flag to true
            isPlaying = true;
        }
    }



    /// <summary>
    /// Handles event raised when an item is selected using the load/save dialog box
    /// </summary>
    /// <param name="Sender"></param>
    /// <param name="e"></param>
    public void LoadSaveDialogItemSelected(object Sender, EventArgs e)
    {
        if (loadSaveDialog.getCurrentEvent() == myGUIControls.SaveDialog.eventType.Load)
        {
            Debug.Log("Loaded file " + loadSaveDialog.outputText);
            GameObject.Find(translationLayerObjectName).GetComponent<TranslationLayer>()
                .LoadList(loadSaveDialog.outputText); //load file
        }

        else if (loadSaveDialog.getCurrentEvent() == myGUIControls.SaveDialog.eventType.Save)
        {
            Debug.Log("Selected to save to file " + loadSaveDialog.outputText);
            GameObject.Find(translationLayerObjectName).GetComponent<TranslationLayer>()
                .SaveList(loadSaveDialog.outputText);
        }

        else
        {
            Debug.Log("Selected to export to file" + loadSaveDialog.outputIndex);
            GameObject.Find(translationLayerObjectName).GetComponent<TranslationLayer>()
                .ExportList(loadSaveDialog.outputText);
        }
    }

    #region top button panel event handlers

    /// <summary>
    /// Event handler for recording button
    /// </summary>
    public void RecordingButtonClicked()
    {
        if (isRecording)
        {
            //stop recording
            GameObject.Find(translationLayerObjectName).GetComponent<TranslationLayer>()
                .StopRecording();
            topPanel.buttonArray[0].text = "Not Recording";
            isRecording = false;
        }

        else
        {
            //start recording
            GameObject.Find(translationLayerObjectName).GetComponent<TranslationLayer>()
                .StartRecording();

            topPanel.buttonArray[0].text = "Recording";
            isRecording = true;
        }
    }

    /// <summary>
    /// Event handler for tracking button
    /// </summary>
    public void TrackingButtonClicked()
    {
        if (isTracking)
        {
            //call function to pause tracking
            GameObject.Find(translationLayerObjectName).GetComponent<TranslationLayer>()
                .StopTracking();
            topPanel.buttonArray[1].text = "Not tracking";
            isTracking = false;
        }

        else
        {
            //call function to resume tracking
            GameObject.Find(translationLayerObjectName).GetComponent<TranslationLayer>()
                .ResumeTracking();
            topPanel.buttonArray[1].text = "Tracking";
            isTracking = true;
        }
    }
    
    /// <summary>
    /// Event handler for reset button
    /// </summary>
    public void ResetButtonClicked()
    {

    }
    
    /// <summary>
    /// Event handler for load button
    /// </summary>
    public void LoadButtonClicked()
    {

        //show the LoadSavePanel 
        loadSaveDialog.Prompt("Load", "Cancel", myGUIControls.SaveDialog.eventType.Load);

    }
    
    /// <summary>
    /// Event handler for save button
    /// </summary>
    public void SaveButtonClicked()
    {
        Debug.Log("Save clicked");
        //prompt
        loadSaveDialog.Prompt("Save", "Cancel", myGUIControls.SaveDialog.eventType.Save);
    }

    /// <summary>
    /// Event handler for export button
    /// </summary>
    public void ExportButtonClicked()
    {
        loadSaveDialog.Prompt("Export", "Cancel", myGUIControls.SaveDialog.eventType.Export);
    }
    #endregion

    #region bottom button panel event handlers

    /// <summary>
    /// Event handler for set start button
    /// </summary>
    public void SetStartButtonClicked()
    {
        GameObject.Find(translationLayerObjectName).GetComponent<TranslationLayer>()
            .NewStart();
    }

    /// <summary>
    /// Event handler for set end button
    /// </summary>
    public void SetEndButtonClicked()
    {
        Debug.Log("Set End btn clicked"); 
        GameObject.Find(translationLayerObjectName).GetComponent<TranslationLayer>()
            .NewEnd();
    }

    /// <summary>
    /// Event handler for New KP button
    /// </summary>
    public void NewKPButtonClicked()
    {
        GameObject.Find(translationLayerObjectName).GetComponent<TranslationLayer>()
            .AddKeyPoint();
    }

    /// <summary>
    /// Event handler for Delete KP button
    /// </summary>
    public void DeleteKPButtonClicked()
    {
        GameObject.Find(translationLayerObjectName).GetComponent<TranslationLayer>()
            .DeleteKeyPoint(keyPointsList.outputIndex);
    }

    /// <summary>
    /// Event handler for Undo button
    /// </summary>
    public void UndoButtonClicked()
    {
        GameObject.Find(translationLayerObjectName).GetComponent<TranslationLayer>()
            .UndoChange();
    }

    /// <summary>
    /// Event handler for seated mode button
    /// </summary>
    public void SeatedModeButtonClicked()
    {
        bool seatedmodebool = GameObject.Find(translationLayerObjectName).GetComponent<TranslationLayer>()
            .seatedModeOn;

        //seated mode is on
        if (seatedmodebool)
        {
            bottomPanel.buttonArray[5].text = "Seated Mode Off";
            GameObject.Find(translationLayerObjectName).GetComponent<TranslationLayer>()
            .DisableSeatedMode();
        }

        else
        {
            bottomPanel.buttonArray[5].text = "Seated Mode On";
            GameObject.Find(translationLayerObjectName).GetComponent<TranslationLayer>()
            .EnableSeatedMode();
        }
    }
    #endregion

    #region slider methods

    /// <summary>
    /// Handles event raised when the slider is moved
    /// </summary>
    /// <param name="Sender"></param>
    /// <param name="e"></param>
    public void SliderValueChanged(object Sender, EventArgs e)
    {
        GameObject.Find(translationLayerObjectName).GetComponent<TranslationLayer>()
            .HandleSliderChange(slider.value);
    }

    /// <summary>
    /// Updates the slider position to reflect playback progress
    /// Note: This method is called from TranslationLayer
    /// </summary>
    /// <param name="currentFrameIndexIn">index of current frame</param>
    /// <param name="maxFramesIn">max number of frames in playlist</param>
    public void UpdateSlider(int currentFrameIndexIn, int maxFramesIn)
    {
        float sliderValue = (currentFrameIndexIn * 100 / maxFramesIn);
        slider.value = sliderValue;
        //Debug.Log(currentFrameIndexIn + " " + maxFramesIn + " " + sliderValue);
    }

    #endregion

    #region Key Points List related methods
    /// <summary>
    /// Updates the Key Points List
    /// </summary>
    public void UpdateKPList()
    {
        //get the KP list from the TranslationLayer
        List<SerializeScript.KeyPoint> keyPoints 
            = GameObject.Find(translationLayerObjectName).GetComponent<TranslationLayer>().keypointsList;

        //clear KP list in GUI
        keyPointsList.clear();

        //traverse keyPoints and add KPs to GUI list
        for (int i = 0; i < keyPoints.Count; i++)
        {
            keyPointsList.add("Key Point " + i);
        }
    }

    #endregion

    #endregion

    /// <summary>
    /// GUI Elements are drawn in this method
    /// </summary>
    public void OnGUI()
    {
        //draw the GUI components
        topPanel.Draw();
        bottomPanel.Draw();
        keyPointsList.Draw();
        playPauseButton.Draw();
        slider.Draw();
        loadSaveDialog.Draw();
    }

    /// <summary>
    /// Method is called when script starts
    /// </summary>
    public void Start()
    {
        //initializing methods
        loadSaveDialog.InitMethod();

        //add event handlers
        topPanel.buttonPanelClickHandler += new myGUIControls.ButtonPanelClickevent(TopPanelButtonClicked);
        bottomPanel.buttonPanelClickHandler += new myGUIControls.ButtonPanelClickevent(BottomPanelButtonClicked);
        keyPointsList.listItemSelectedEventHandler += new myGUIControls.GUIListItemSelectedEventHandler(KeyPointsListClicked);
        playPauseButton.clickEventHandler += new myGUIControls.ComplexButtonEventHandler(PlayPauseButtonClicked);
        slider.SliderEventHandler += new myGUIControls.IBoxSliderEventHandler(SliderValueChanged);
        loadSaveDialog.saveButtonEventHandler +=new myGUIControls.SaveButtonEventHandler(LoadSaveDialogItemSelected);

    }

    /// <summary>
    /// Method is called every frame
    /// </summary>
    public void Update()
    {
        //call update method for GUI
        slider.Update();
        keyPointsList.UpdateMethod();
        loadSaveDialog.UpdateMethod();
    }

}
