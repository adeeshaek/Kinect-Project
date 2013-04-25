using UnityEngine;
using System.Collections;

/// <summary>
/// Recieves actions created by NGUI
/// </summary>

public class UIMenuActionReciever : MonoBehaviour {

    /// <summary>
    /// reference to key points panel
    /// </summary>
    public GameObject keyPointsPanel;

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
            Debug.Log(item + " selected!");


            //big swich statement to decide what action to take
            switch (item)
            {
                case "AddKPButton":
                    //adding frame 0 for now
                    keyPointsPanel.GetComponent<UIListPanel>().AddKeyPoint(0);
                    break;

                case "RemoveKPButton":
                    keyPointsPanel.GetComponent<UIListPanel>().RemoveKeyPoint(0);
                    break;

                default:
                    //fill in later
                    break;

            }

        }

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
