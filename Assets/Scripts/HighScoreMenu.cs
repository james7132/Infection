using UnityEngine;
using System.Collections;

public class HighScoreMenu : MonoBehaviour 
{
	private string playerName = "n00b";
	private string[] names = {"Player 1","Player 2","Player 3","Player 4","Player 5","Player 6","Player 7","Player 8","Player 9","Player 10"};
	private float playerScore;
	private float[] scores = {250000,175000,140000,100000,75000,50000,25000,12500,6250,100};
	private bool addButtonPressed = false;
	public GUIStyle guiStyle;
	
	public HighScoreMenu()
	{

	}
	
	// Use this for initialization
	void Start () 
	{
		playerName = PlayerPrefs.GetString("player name");
		playerScore = PlayerPrefs.GetFloat("player score");
		
		for (int i = 0; i < names.Length; i++) {
			names[i] = PlayerPrefs.GetString("name"+i);
			scores[i] = PlayerPrefs.GetFloat("score"+i);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		// update high score records
		for (int i = 0; i < names.Length; i++) {
			PlayerPrefs.SetFloat("score"+i,scores[i]);
			PlayerPrefs.SetString("name"+i,names[i]);
		}
	}
	
	void OnGUI()
	{
		int xMax = Screen.width;
		int yMax = Screen.height;
		GUI.Label(new Rect(0.1f * xMax, 0.1f * yMax, 0.1f * xMax, 0.1f * yMax), "INFECTION", guiStyle);
		GUI.Label(new Rect(0.1f * xMax, 0.2f * yMax, 0.1f * xMax, 0.1f * yMax), "HIGH SCORES", guiStyle);
		
		// display all the recorded high scores
		int yPos = (int) (0.25 * yMax);
		int count = 0;
		for (int i = 0; i < names.Length; i++) {
			GUI.Label(new Rect(0.1f * xMax, yPos, 0.1f * xMax, 0.1f * yMax), PlayerPrefs.GetString("name"+i), guiStyle);
			GUI.Label(new Rect(0.5f * xMax, yPos, 0.1f * xMax, 0.1f * yMax), PlayerPrefs.GetFloat("score"+i).ToString(), guiStyle);
			yPos += (int) (0.025*yMax);
		}
		
		// only allow player to edit name and add score to high score table when their score is at least as good as the lowest high score
		if (playerScore >= scores[scores.Length-1]) {
			yPos += (int) (0.05*yMax);
			
			if (!addButtonPressed) {
				playerName = GUI.TextField(new Rect(0.1f * xMax, yPos, 0.2f * xMax, 0.035f * yMax), playerName, 20);
				GUI.Label(new Rect(0.5f * xMax, yPos, 0.2f * xMax, 0.035f * yMax), playerScore.ToString());
				
				yPos += (int) (0.05*yMax);
				bool addButton = GUI.Button(new Rect(0.1f * xMax, yPos, 0.2f * xMax, 0.035f * yMax), "Add Score");
				
				if (addButton) {
					addButtonPressed = true;
					addScore();
				}
			} 
		} else {
			GUI.Label(new Rect(0.1f * xMax, yPos, 0.2f * xMax, 0.035f * yMax), "You Scored");
			GUI.Label(new Rect(0.5f * xMax, yPos, 0.2f * xMax, 0.035f * yMax), playerScore.ToString());
		}
		
		bool returnButton = GUI.Button(new Rect(0.1f * xMax, 0.8f * yMax, 0.2f * xMax, 0.035f * yMax), "Return to Menu");
		if (returnButton) {
			Application.LoadLevel("MainMenu");
		}
	}
	
	private void addScore() {
		// find where the player's score fits into the high score table
		int rank = 0;
		for (int i = scores.Length-1; i >= 0; i--) {
			if (playerScore < scores[i]) {
				rank = i+1;
				print (rank);
				break;
			}
		}
		
		// move all the scores below player's rank down
		for (int i = scores.Length-1; i > rank; i--) {
			scores[i] = scores[i-1];
			names[i] = names[i-1];
		}
		
		// insert player's score in appropriate location
		scores[rank] = playerScore;
		names[rank] = playerName; 
	}
}
