using UnityEngine;
using System.Collections;

/// <summary>
/// Extends UIButton in such a way that it works
/// similarly to NGUI's popup menu
/// </summary>
public class ExtendedUIPlaybackButton : UIButton {
	
	public GameObject reciever;
	
	public string actionName;
	
	public void OnClick()
	{
		reciever.GetComponent<UIMenuActionReciever>().OnSelectionChange(actionName);
	}
}
