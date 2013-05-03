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

    public int frameIndex;

    /// <summary>
    /// sets the index of this button
    /// </summary>
    /// <param name="indexIn"></param>
    public void setIndex(int indexIn)
    {
        index = indexIn;
    }

    /// <summary>
    /// called on click
    /// </summary>
	public void OnClick()
	{
        gameObject.transform.parent.GetComponent<KeyPointsListPanel>().OnPanelButtonClick(index);
	}

    /// <summary>
    /// makes this button invisible
    /// </summary>
    public void MakeInvisible()
    {
        gameObject.GetComponentInChildren<UILabel>().enabled = false;
        gameObject.GetComponentInChildren<UISprite>().enabled = false;
        gameObject.GetComponent<BoxCollider>().enabled = false;
    }

    /// <summary>
    /// makes the button visible
    /// </summary>
    public void MakeVisible()
    {
        gameObject.GetComponentInChildren<UILabel>().enabled = true;
        gameObject.GetComponentInChildren<UISprite>().enabled = true;
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }
}
