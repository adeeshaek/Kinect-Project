using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Encapsulates behaviours for list panel
/// </summary>
public class UILoadPanel : UIListPanel
{
    /// <summary>
    /// currently selected item path
    /// </summary>
    public string selectedItemPath;

    /// <summary>
    /// name of the selected item
    /// </summary>
    public string selectedItemName;

    /// <summary>
    /// reference to reciever
    /// </summary>
    public GameObject NGUIReciever;

    /// <summary>
    /// initializes everything
    /// </summary>
    public void Start()
    {
        MakeVisible();
    }

    /// <summary>
    /// Removes the word 'selected' from the selected button's text
    /// </summary>
    /// <param name="index"></param>
    public override void OnPanelButtonClick(int index)
    {

        string itemText =
            panelButtonList[index].GetComponentInChildren<UILabel>().text;

        selectItemButtonRef.GetComponentInChildren<UILabel>().text
    = (itemText);

        selectedItem = index;
    }

    /// <summary>
    /// Recieves even when button click happens
    /// </summary>
    /// <param name="item"></param>
    public override void OnSelectionChange(string item)
    {
        switch (item)
        {
            case "OpenButton":
                OpenFile();
                break;

            case "CancelButton":
                MakeInvisible();
                break;

            default:
                break;
        }

    }

    /// <summary>
    /// does the GUI part of opening the file, namely
    /// making the panel disappear
    /// </summary>
    public void OpenFile()
    {
        selectedItemName = selectItemButtonRef.GetComponentInChildren<UILabel>().text;
        selectedItemPath = Directory.GetCurrentDirectory() +
            "\\" + selectItemButtonRef.GetComponentInChildren<UILabel>().text;
        /*
        Debug.Log(selectedItemPath);
        NGUIReciever.GetComponent<UIMenuActionReciever>().currentlySelectedFile = selectedItemPath;
        NGUIReciever.GetComponent<UIMenuActionReciever>()
            .setStatus("Opened File " + selectItemButtonRef.GetComponentInChildren<UILabel>().text);
       */
        NGUIReciever.GetComponent<UIMenuActionReciever>()
            .openedFile(selectedItemPath, selectedItemName);
        
        MakeInvisible();
    }

    /// <summary>
    /// Makes the panel invisible
    /// </summary>
    public void MakeInvisible()
    {
        gameObject.SetActiveRecursively(false);
    }

    /// <summary>
    /// Re-creates the panel...
    /// </summary>
    public void MakeVisible()
    {
        panelButtonList = new List<GameObject>();
        string path = Directory.GetCurrentDirectory();
        DirSearch(path);
        ReDrawPanel();
    }

    /// <summary>
    /// Searches the current directory for files and adds them to the list as needed.
    /// Note: This method adapted from method suggested by John T,
    /// on May 30th, 2009,
    /// http://stackoverflow.com/questions/929276/how-to-recursively-list-all-the-files-in-a-directory-in-c
    /// </summary>
    /// <param name="sDir">The current directory</param>
    /// <param name="buttonListIn">The list of buttons to be amended</param>
    public void DirSearch(string sDir)
    {
        panelButtonList.Clear();

        try
        {

            foreach (string f in Directory.GetFiles(sDir))
            {
                //Debug.Log(f);
                //if the file is an xml, add to list
                if (f.EndsWith(".xml")) //XML HARDCODED
                {
                    //delete the path from f
                    string g = f.Remove(0, sDir.Length + 1);

                    AddItem(g);
                    
                }
            }

        }
        catch (System.Exception excpt)
        {
            Debug.Log(excpt.Message);
        }
    }
}
