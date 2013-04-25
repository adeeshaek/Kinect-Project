using UnityEngine;
using System.Collections;

/// <summary>
/// Extends UIButton in such a way that it works
/// similarly to NGUI's popup menu
/// Specific  to UIPanel in that it represents an 
/// item in the UIPanel. Here it sends a message to 
/// the parent's script
/// </summary>
public class UIPanelItemButton: UIButton {

    /// <summary>
    /// index of the frame that this panel button represents
    /// </summary>
    public int index;

	public void OnClick()
	{
        gameObject.transform.parent.GetComponent<UIListPanel>().OnPanelButtonClick(index);
	}

    public void MakeInvisible()
    {
        gameObject.GetComponentInChildren<UILabel>().enabled = false;
        gameObject.GetComponentInChildren<UISprite>().enabled = false;
        gameObject.GetComponent<BoxCollider>().enabled = false;
    }

    public void MakeVisible()
    {
        gameObject.GetComponentInChildren<UILabel>().enabled = true;
        gameObject.GetComponentInChildren<UISprite>().enabled = true;
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }
}
