using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour 
{
	//private string playerName;
	private int playerScore;
	public GUIStyle guiStyle;
	public GUITexture logoItem;
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
	
	void OnGUI()
	{
		if (Global.GameState == GameState.PAUSE) {
			playerScore = (int) ScoreScript.getScore();
			int xMax = Screen.width;
			int yMax = Screen.height;
			logoItem.pixelInset = new Rect(-0.25f * xMax, 0.15f * yMax, 0.5f * xMax, 0.20f * yMax);
			logoItem.enabled = true;
			GUI.Label(new Rect(0.5f * xMax-8, 0.4f * yMax - 5, 16, 10), "PAUSED", guiStyle);
		} else {
			logoItem.enabled = false;
		}
	}
}
