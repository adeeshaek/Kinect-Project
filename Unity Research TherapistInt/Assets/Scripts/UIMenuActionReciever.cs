using UnityEngine;
using System.Collections;

/// <summary>
/// Recieves actions created by NGUI
/// </summary>

public class UIMenuActionReciever : MonoBehaviour {
	
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
			Debug.Log (item + " selected!" );
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
	
}
