using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Encapsulates behaviours for list panel
/// </summary>
public class UIListPanel : MonoBehaviour
{

    #region global variables
    /// <summary>
    /// list of items in the panel
    /// each string corresponds to item title
    /// </summary>
    List<string> itemsList;

    /// <summary>
    /// List of panel buttons in the list
    /// </summary>
    List<GameObject> panelButtonList;

    /// <summary>
    /// Ref to the prefab for the panel button 
    /// </summary>
    public GameObject panelButtonPrefab;

    /// <summary>
    /// Ref to the slider.
    /// </summary>
    public GameObject sliderRef;

    /// <summary>
    /// Ref to the selectButton.
    /// </summary>
    public GameObject selectItemButtonRef;

    /// <summary>
    /// Defines the threshold window in which the panel
    /// buttons display (in terms of y-axis). threshold
    /// is relative to the 
    /// </summary>
    public float buttonHeight = 0.2f;

    /// <summary>
    /// Max number of buttons used
    /// </summary>
    public float maxbuttons = 5;

    /// <summary>
    /// keeps track of the currently selected key point
    /// set to an unreasonably high number to begin with
    /// which allows us to check if it has been modified
    /// </summary>
    int selectedItem = 9999;

    #endregion

    #region callback methods
    /// <summary>
    /// handles a change in the slider value
    /// </summary>
    /// <param name="value"></param>
    public void OnSliderChange(float value)
    {
        DrawPanel();
        Debug.Log("Panel Slider changed to " + value);
    }

    public void OnSelectionChange(string item)
    {
        Debug.Log(item + " clicked!");
    }

    public void OnPanelButtonClick(int frameIndex)
    {
        Debug.Log("Element " + frameIndex + " clicked!");
    }

    #endregion

    public void Start()
    {
        //init variables
        itemsList = new List<string>();
        panelButtonList = new List<GameObject>();

    }

    /// <summary>
    /// Adds a new key point
    /// </summary>
    public void AddKeyPoint(int frameRef)
    {
        int numberOfButtonsInclusive = panelButtonList.Count + 1;

        Vector3 initPostition = 
            new Vector3(selectItemButtonRef.transform.position.x, 
                selectItemButtonRef.transform.position.y - (numberOfButtonsInclusive * buttonHeight),
                0);

        GameObject newPanelButton =
            Instantiate(panelButtonPrefab, initPostition, Quaternion.identity) as GameObject;
        newPanelButton.transform.parent = gameObject.transform;
        newPanelButton.transform.localScale = new Vector3(1, 1, 1);

        panelButtonList.Add(newPanelButton);

        DrawPanel();
    }

    /// <summary>
    /// removes the given key point
    /// </summary>
    /// <param name="keyPointToRemove"></param>
    public void RemoveKeyPoint(int keyPointToRemove)
    {

    }

    /// <summary>
    /// makes buttons visible or invisible depending on whether
    /// or not they are within the bounds of the panel
    /// called every time a change takes place
    /// </summary>
    public void DrawPanel()
    {

    }

}
