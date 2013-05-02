using UnityEngine;
using System.Collections;

/// <summary>
/// Recieves actions created by NGUI
/// </summary>

public class UIMenuActionReciever : MonoBehaviour {

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

    int currentIndex = 0;

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
                    openFile(currentlySelectedFile);
                    break;

                case "Save":
                    saveFile(currentlySelectedFile);
                    break;

                case "Export":
                    exportFile(currentlySelectedFile);
                    break;

                default:
                    //fill in later
                    break;

            }

        }

	}

    public void savedFile(string fileName)
    {

    }

    public void exportedFile(string fileName)
    {

    }

    public void openedFile(string fileName)
    {

    }

    /// <summary>
    /// opens the file of the given fileName
    /// </summary>
    /// <param name="fileName"></param>
    public void openFile(string fileName)
    {
        setStatus("Opening File");
        LoadGroup.SetActiveRecursively(true);
    }

    /// <summary>
    /// saves the file to the given filename
    /// </summary>
    /// <param name="fileName"></param>
    public void saveFile(string fileName)
    {
        setStatus("Saving to File");
        SaveGroup.SetActiveRecursively(true);
    }

    /// <summary>
    /// exports the file to given filename
    /// </summary>
    /// <param name="fileName"></param>
    public void exportFile(string fileName)
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
