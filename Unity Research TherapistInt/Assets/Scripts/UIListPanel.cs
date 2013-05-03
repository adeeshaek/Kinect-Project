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
    /// List of panel buttons in the list
    /// </summary>
    protected List<GameObject> panelButtonList;

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
    public int maxbuttons = 5;

    /// <summary>
    /// Updated every time slider value changes
    /// </summary>
    public float currentSliderValue = 0;

    /// <summary>
    /// Max threshold for button - set in inspector
    /// </summary>
    public float maxThresh = 0.23f;

    /// <summary>
    /// Min threshold for button - set in inspector
    /// </summary>
    public float minThresh = -0.4f;

    /// <summary>
    /// keeps track of the currently selected key point
    /// set to an unreasonably high number to begin with
    /// which allows us to check if it has been modified
    /// </summary>
    protected int selectedItem = 9999;

    #endregion

    #region callback methods
    /// <summary>
    /// handles a change in the slider value
    /// </summary>
    /// <param name="value"></param>
    public void OnSliderChange(float value)
    {
        ReDrawPanel();
        currentSliderValue = (1-value);
    }

    public virtual void OnSelectionChange(string item)
    {
        Debug.Log(item + " clicked!");

    }

    public virtual void OnPanelButtonClick(int index)
    {

        string itemText =
            panelButtonList[index].GetComponentInChildren<UILabel>().text;

        selectItemButtonRef.GetComponentInChildren<UILabel>().text
    = (itemText + " selected");

        selectedItem = index;
    }

    #endregion

    public void Start()
    {
        //init variables
        panelButtonList = new List<GameObject>();

    }

    
    /// <summary>
    /// Adds a new key point
    /// </summary>
    public void AddItem(string myText)
    {
        Debug.Log("adding Item " + myText);
        int numberOfButtonsInclusive = panelButtonList.Count + 1;

        Vector3 initPostition = 
            new Vector3(selectItemButtonRef.transform.position.x, 
                selectItemButtonRef.transform.position.y - (numberOfButtonsInclusive * buttonHeight),
                0);

        GameObject newPanelButton =
            Instantiate(panelButtonPrefab, initPostition, Quaternion.identity) as GameObject;
        newPanelButton.transform.parent = gameObject.transform;
        newPanelButton.transform.localScale = new Vector3(1, 1, 1);
        newPanelButton.GetComponentInChildren<UILabel>().text = myText;
        newPanelButton.GetComponent<UIPanelItemButton>().index = numberOfButtonsInclusive - 1;

        panelButtonList.Add(newPanelButton);

        ReDrawPanel();
    }

    /// <summary>
    /// parameterized remove
    /// </summary>
    /// <param name="itemIndex"></param>
    /// <returns></returns>
    public bool RemoveItem(int itemIndex)
    {
        Destroy(panelButtonList[itemIndex]);
        panelButtonList.RemoveAt(itemIndex);
        resequence();
        ReDrawPanel();
        return true;
    }

    /// <summary>
    /// removes the given key point
    /// </summary>
    /// <param name="keyPointToRemove"></param>
    public bool RemoveItem()
    {
        return RemoveItem(selectedItem);
    }

    /// <summary>
    /// removes all items in the list
    /// </summary>
    public void ClearList()
    {
        
        for (int i = 0; i < panelButtonList.Count; i++)
        {
            RemoveItem(0);
        }

        Debug.Log("Number of items after clearing: " + panelButtonList.Count);
    }

    /// <summary>
    /// makes buttons visible or invisible depending on whether
    /// or not they are within the bounds of the panel
    /// called every time a change takes place
    /// </summary>
    public void ReDrawPanel()
    {
        //vars
        GameObject currentPanelButton;
        int space;

        //init position for a button
        Vector3 initPostition =
new Vector3(selectItemButtonRef.transform.position.x,
selectItemButtonRef.transform.position.y,
0);
        //calculate the first button space
        //using the number of buttons in the whole space
        //and the slider position
        
        if (panelButtonList.Count > maxbuttons)
            space = panelButtonList.Count - maxbuttons;
        else
            space = 0;

        float minimumThresh = (space * currentSliderValue);

        //go through the list and move each item to the right 
        //place

        for (int i = 0; i < panelButtonList.Count; i++)
        {
            currentPanelButton = panelButtonList[i];

            //change position of buttn
            currentPanelButton.transform.position =
                new Vector3(initPostition.x,
            initPostition.y - ((i + 1) * buttonHeight) + (minimumThresh * buttonHeight),
            0);

            if (currentPanelButton.transform.position.y < maxThresh
                && currentPanelButton.transform.position.y > minThresh)
            {
                currentPanelButton.GetComponent<UIPanelItemButton>().MakeVisible();

            }

            //make button disappear if too many buttons displayed
            else
            {
                currentPanelButton.GetComponent<UIPanelItemButton>().MakeInvisible();
            }

        }

    }

    /// <summary>
    /// resequences the panel list by copying all the items
    /// into a new list, then destroying and redrawing the list
    /// </summary>
    protected void resequence()
    {
        List<GameObject> newList = new List<GameObject>();
        GameObject current;

        for (int i = 0; i < panelButtonList.Count; i++)
        {
            current = panelButtonList[i];

            if (current != null)
            {
                newList.Add(current);
            }

        }

        panelButtonList = newList;

    }

}
