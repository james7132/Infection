using UnityEngine;
using System.Collections;

public class InstructionsScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI()
	{
		int x = Screen.width;
		int y = Screen.height;
		if(GUI.Button(new Rect(0.35f * x, 0.90f * y, 0.3f * x, 0.05f * y), "Return to Main Menu"))
		{
			Application.LoadLevel("MainMenu");
		}
	}
}
