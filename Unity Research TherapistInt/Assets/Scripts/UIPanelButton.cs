using UnityEngine;
using System.Collections;

/// <summary>
/// Extends UIButton in such a way that it works
/// similarly to NGUI's popup menu
/// Specific  to UIPanel in that it connects
/// straight to the UIPanel script
/// </summary>
public class UIPanelButton: UIButton {
	
	public string actionName;
    public GameObject listPanel;
	
	public void OnClick()
	{
        listPanel.GetComponent<UIListPanel>().OnSelectionChange(actionName);
	}
}
