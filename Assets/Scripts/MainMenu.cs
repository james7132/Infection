using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour 
{
	private readonly Rect[] buttonRects;
	private readonly string[] buttonName = {"Start Game", "Instructions", "Credits" , "Highscores"};
	private const float buttonWidth = 0.15f;
	private const float buttonSpacing = 0.025f;
	public GUITexture logoItem;
	public GUIStyle guiStyle;
	
	public MainMenu()
	{
		float bottomMenuWidth = buttonWidth * buttonName.Length + buttonSpacing * (buttonName.Length - 1);
		float startX = 0.5f - (0.5f * bottomMenuWidth);
		buttonRects = new Rect[buttonName.Length];
		for(int i = 0; i < buttonName.Length; i++)
		{
			buttonRects[i] = new Rect(startX + (buttonWidth + buttonSpacing) * i, 0.85f, buttonWidth, 0.05f);
		}
	}
	
	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {}
	
	void OnGUI()
	{
		int x = Screen.width;
		int y = Screen.height;
		logoItem.pixelInset = new Rect(-0.35f * x, -0.15f * y, 0.75f * x, 0.35f * y);
		for(int i = 0; i < buttonRects.Length; i++)
		{
			bool button = GUI.Button(new Rect(buttonRects[i].x * x, buttonRects[i].y * y, buttonRects[i].width * x, buttonRects[i].height * y), buttonName[i]);
			if(button)
			{
				switch (buttonName[i]) 
				{
				case "Start Game":
					PlayerPrefs.SetFloat("new game",1.0f);
					Application.LoadLevel("MainGamePlay");
					break;
				case "Highscores":
					PlayerPrefs.SetFloat("player score",0.0f);
					Application.LoadLevel ("HighscoreMenu");
					break;
				case "Credits":
					Application.LoadLevel("Credits");
					break;
				case "Instructions":
					Application.LoadLevel("Instructions");
					break;
				default:
					goto case "Start Game";
				}
			}
		}
	}
}
