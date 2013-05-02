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
                    openFile();
                    break;

                case "Save":
                    saveFile();
                    break;

                case "Export":
                    exportFile();
                    break;

                default:
                    //fill in later
                    break;

            }

        }

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
    public void openFile()
    {
        setStatus("Opening File");
        LoadGroup.SetActiveRecursively(true);
    }

    /// <summary>
    /// saves the file save dialog
    /// </summary>
    public void saveFile()
    {
        setStatus("Saving to File");
        SaveGroup.SetActiveRecursively(true);
    }

    /// <summary>
    /// exports the file export dialog
    /// </summary>
    public void exportFile()
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
