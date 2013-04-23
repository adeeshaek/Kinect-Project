using UnityEngine;
using System.Collections;

/// <summary>
/// Recieves actions created by NGUI
/// </summary>

public class UIMenuActionReciever : MonoBehaviour {
	
	void OnSelectionChange(string item)
	{
		
		//NGUI sometimes fires randomly at the start of the 
		//level. This hack can get around that.
		if (Time.timeSinceLevelLoad > 1)
		{
			Debug.Log (item + " selected!" );
		}

	}
	
}
