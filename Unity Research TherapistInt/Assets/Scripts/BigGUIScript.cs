using UnityEngine;
using System.Collections;

/// <summary>
/// Used to control the big gui script
/// </summary>
public class BigGUIScript : MonoBehaviour {
    /// <summary>
    /// time for each message to appear
    /// </summary>
    public int cooldownTime = 5;

    /// <summary>
    /// current value of counter
    /// </summary>
    int countdownvalue;

    /// <summary>
    /// whether or not the text is visible
    /// </summary>
    bool textIsVisible = true;

    /// <summary>
    /// displays the text for a given time
    /// </summary>
    public void displayText(string message)
    {
        gameObject.GetComponent<UILabel>().enabled = true;
        gameObject.GetComponent<UILabel>().text = message;
        countdownvalue = cooldownTime;
    }

    /// <summary>
    /// decrements the counter, and makes the text invisible
    /// when needed
    /// </summary>
    public void decrementCounter()
    {
        if (countdownvalue > 0)
        {
            countdownvalue--;
        }

        else //hide the text
        {
            Debug.Log("Stopped");
            textIsVisible = false;
            gameObject.GetComponent<UILabel>().enabled = false;
        }
    }

    void FixedUpdate()
    {
        if (textIsVisible)
            decrementCounter();
    }
}
