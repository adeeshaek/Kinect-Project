using UnityEngine;
using System.Collections;

public class settingsMenuScript : MonoBehaviour {

    /// <summary>
    /// called when a button is pressed
    /// </summary>
    /// <param name="item"></param>
    public void OnSelectionChange(string item)
    {
        if (Time.timeSinceLevelLoad > 1)
        {
            switch (item)
            {
                case "close":
                    Debug.Log("CLose");
                    break;
            }
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
