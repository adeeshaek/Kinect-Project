using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Draws all the GUI elements in the scene
/// </summary>
public class ButtonScript : MonoBehaviour {

    //global vars
    #region Global variables
    /// <summary>
    /// The name of the gameObject to which the TranslationLayer script is attached
    /// </summary>
    public string intermediateObjectName = "Intermediate";

    #region GUI controls
    /// <summary>
    /// GUI button panel for user interaction
    /// </summary>
    public myGUIControls.ButtonPanel myPanel;

    /// <summary>
    /// Load/Save dialog box
    /// Used to allow user to select file to load
    /// </summary>
    public myGUIControls.SaveDialog loadSaveDialog;

    #endregion

    #endregion

    /// <summary>
    /// Use for GUI controls
    /// </summary>
    void OnGUI()
    {
        myPanel.Draw();
        loadSaveDialog.Draw();
    }

    #region Event Handlers
    /// <summary>
    /// Handles event raised when a button in the buttonPanel is clicked
    /// Checks which button was pressed and calls the appropriate handler
    /// </summary>
    /// <param name="sender">Ref to object which raised the event</param>
    /// <param name="e">Event args</param>
    void EventHandler(object sender, EventArgs e)
    {
        //Debug.Log("Button " + myPanel.getOutputIndex() + " Pressed");
        switch (myPanel.getOutputIndex())
        {
            case 0:
                LoadButtonPressed();
                break;

            case 1:
                PlayButtonPressed();
                break;

            case 2:
                WholeBodyListenButtonPressed();
                break;

            case 3:
                SeatedModeButtonPressed();
                break;
        }
    }

    /// <summary>
    /// Handles the event raised when the seated mode button is pressed
    /// Also manages the states of the seated mode button
    /// </summary>
    void SeatedModeButtonPressed()
    {
        //check if the intermediate object has seated mode on
        bool seatedModeState = GameObject.Find(intermediateObjectName).GetComponent<TranslationLayer>()
            .seatedModeOn;

        //if so, set seated mode to off 
        if (seatedModeState)
        {
            GameObject.Find(intermediateObjectName).GetComponent<TranslationLayer>()
                .seatedModeOn = false;

            //set button to read seated mode off
            myPanel.buttonArray[3].text = "Seated Mode Off";
        }

        else
        {
            //else set seated mode to on
            GameObject.Find(intermediateObjectName).GetComponent<TranslationLayer>()
                .seatedModeOn = true;

            //set button to read Seated Mode on
            myPanel.buttonArray[3].text = "Seated Mode On";
        }


    }

    /// <summary>
    /// Handles event raised when user clicks Load in the Load dialog box
    /// Loads the selected file into memory
    /// </summary>
    /// <param name="sender">Reference to sender</param>
    /// <param name="e">Event Args</param>
    void LoadSaveDialogEvent(object sender, EventArgs e)
    {
        //Debug.Log("File " + loadSaveDialog.outputText + " selected!");
        GameObject.Find(intermediateObjectName).GetComponent<TranslationLayer>()
            .LoadList(loadSaveDialog.outputText);
    }

    /// <summary>
    /// Handles event raised when the load button in the button panel is clicked
    /// It makes the load dialog box visible
    /// </summary>
    private void LoadButtonPressed()
    {
        loadSaveDialog.Prompt("Load","Cancel",myGUIControls.SaveDialog.eventType.Load);
    }


    /// <summary>
    /// Handles event raised when play button in the button panel is pressed
    /// </summary>
    private void PlayButtonPressed()
    {
        GameObject.Find(intermediateObjectName).GetComponent<TranslationLayer>().DemoRecording();
    }

    /// <summary>
    /// Handles event raised when fake success is pressed
    /// 
    /// It simulates succesful gesture held
    /// </summary>
    private void FakeSuccessButtonPressed()
    {
        /*
        //make Carl move to the first key point.
        GameObject.Find(intermediateObjectName).GetComponent<TranslationLayer>().JumpToFrame(30, "Carl");
        
         */
        //GameObject.Find(intermediateObjectName).GetComponent<TranslationLayer>().StartListeningForGesture();
        GameObject.Find(intermediateObjectName).GetComponent<TranslationLayer>().GestureHeldEvent();
    
    }

    /// <summary>
    /// Handles event raised when Whole body listen button in the button panel is pressed
    /// </summary>
    public void WholeBodyListenButtonPressed()
    {
        GameObject.Find(intermediateObjectName).GetComponent<TranslationLayer>().StartListeningForWholeBodyGesture();
    }

    #endregion

    // Use this for initialization
	void Start () {
        //Register Event handler for button panel
        myPanel.buttonPanelClickHandler += new myGUIControls.ButtonPanelClickevent(EventHandler);

        //Register Event handler for loadSaveDialog
        loadSaveDialog.saveButtonEventHandler += new myGUIControls.SaveButtonEventHandler(LoadSaveDialogEvent);

        //intialize loadSaveDialog
        loadSaveDialog.InitMethod();
        
	}
	
	// Update is called once per frame
	void Update () {
        //update loadSaveDialog
        loadSaveDialog.UpdateMethod();
	}
}
