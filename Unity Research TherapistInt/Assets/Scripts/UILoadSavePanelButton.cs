using UnityEngine;
using System.Collections;

public class UILoadSavePanelButton : UIButton {

    /// <summary>
    /// index of the frame that this panel button represents
    /// </summary>
    public int index;

    public int frameIndex;

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
