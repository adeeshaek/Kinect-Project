using UnityEngine;
using System.Collections;

/// <summary>
/// Extends UIButton in such a way that it works
/// similarly to NGUI's popup menu
/// </summary>
public class ExtendedUIButton : UIButton {
	
	public GameObject reciever;
	
	public void OnClick()
	{
		string thisButtonType = this.GetComponentInChildren<UILabel>().text;
		reciever.GetComponent<UIMenuActionReciever>().OnSelectionChange(thisButtonType);
	}
	
}
