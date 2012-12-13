using UnityEngine;
using System.Collections;

/// <summary>
/// Anchors the GUI Text to the right place
/// </summary>
public class anchorScript : MonoBehaviour {
	
	#region Public Variables
	
	/// <summary>
	/// 1 text object
	/// </summary>
	public GUIText text1; 
	
	/// <summary>
	/// 2nd text object
	/// </summary>
	public GUIText text2;
	
	/// <summary>
	/// The distance between GUIText1 and GUIText 2
	/// </summary>
	public int offset;
	
	#endregion
	
	// Use this for initialization
	void Start () {
		text1.pixelOffset = new Vector2 (text1.pixelOffset.x, text2.pixelOffset.y + offset);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
