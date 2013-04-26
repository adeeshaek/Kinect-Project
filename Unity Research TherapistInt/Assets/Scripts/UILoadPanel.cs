using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Encapsulates behaviours for list panel
/// </summary>
public class UILoadPanel : UIListPanel
{

    /// <summary>
    /// Recieves even when button click happens
    /// </summary>
    /// <param name="item"></param>
    public override void OnSelectionChange(string item)
    {
        Debug.Log(item + " clicked!");

        switch (item)
        {
            case "OpenButton":

                break;

            case "CancelButton":
                MakeInvisible();
                break;

            default:
                break;
        }

    }

    public void MakeInvisible()
    {
        gameObject.SetActiveRecursively(false);
    }
}
