using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Encapsulates behaviours for list panel
/// </summary>
public class KeyPointsListPanel : UIListPanel
{
    /// <summary>
    /// reference to reciever
    /// </summary>
    public GameObject NGUIReciever;

    /// <summary>
    /// triggered when a key point is selected
    /// </summary>
    /// <param name="item"></param>
    public override void OnPanelButtonClick(int index)
    {
        string itemText =
    panelButtonList[index].GetComponentInChildren<UILabel>().text;

        selectItemButtonRef.GetComponentInChildren<UILabel>().text
    = (itemText + " selected");

        selectedItem = index;
        NGUIReciever.GetComponent<UIMenuActionReciever>().KeyPointsListClicked(index);
    }

}
