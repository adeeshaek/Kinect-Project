using UnityEngine;
using System.Collections;

/// <summary>
/// Encapsulates behaviours for Keypoints panel
/// </summary>
public class KeyPointsPanel : MonoBehaviour {

    /// <summary>
    /// keeps track of the number of Key Points
    /// </summary>
    int keyPointsCount = 0;

    /// <summary>
    /// keeps track of the currently selected key point
    /// set to an unreasonably high number to begin with
    /// which allows us to check if it has been modified
    /// </summary>
    int selectedKeyPoint = 10000;

    /// <summary>
    /// Adds a new key point
    /// </summary>
    public void AddKeyPoint()
    {
        keyPointsCount++;

        CheckPanelButtonBounds();
    }

    /// <summary>
    /// removes the given key point
    /// </summary>
    /// <param name="keyPointToRemove"></param>
    public void RemoveKeyPoint(int keyPointToRemove)
    {
        keyPointsCount--;


    }

    /// <summary>
    /// handles a change in the slider value
    /// </summary>
    /// <param name="value"></param>
    public void SliderValueChanged(float value)
    {
        CheckPanelButtonBounds();
    }

    /// <summary>
    /// makes buttons visible or invisible depending on whether
    /// or not they are within the bounds of the panel
    /// called every time a change takes place
    /// </summary>
    public void CheckPanelButtonBounds()
    {

    }

}
