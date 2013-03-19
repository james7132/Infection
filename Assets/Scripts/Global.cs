using System;
using UnityEngine;
public class Global	 : MonoBehaviour 
{
	private static Global instance;
	public static GameState GameState = GameState.START_GAME;
	public double virusCountToScoreRatio = 100;
	public double playerScore;
	public int Upper_Limit = 360;
	public int Death_Limit = -400;
	public int Left_Limit = 475;
	public int Right_Limit = -475;
	public static int UpperLimit;
	public static int DeathLimit;
	public static int LeftLimit;
	public static int RightLimit;
	private int totalVirusCount = 1;
	
	public Global()
	{
		UpperLimit = Upper_Limit;
		DeathLimit = Death_Limit;
		LeftLimit = Left_Limit;
		RightLimit = Right_Limit;
	}
	
	void Start()
	{
		instance = this;
		playerScore = 0;
	}
	
	void Update()
	{
		GameObject[] viruses = GameObject.FindGameObjectsWithTag("Player");
		GameObject[] infectedBloodCells = GameObject.FindGameObjectsWithTag("Infected Blood Cell");
	
		// GAME STATE HANDLING
		switch (GameState) {
		case GameState.START_GAME:
			// INITIALIZE GAME
			PlayerPrefs.SetFloat("player score",0);
			GameState = GameState.IN_GAME;
			break;
		case GameState.IN_GAME:
			// PLAY GAME
			if (viruses.Length <= 0 && infectedBloodCells.Length <= 0)
			{
				GameState = GameState.GAME_OVER;
			}
			
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				GameState = GameState.PAUSE;
				GameObject bg = GameObject.Find("Background");
				bg.GetComponent<Background>().setSpeeds(0.001f);
				bg.GetComponent<Background>().setColors(Color.yellow,Color.yellow,Color.yellow,Color.yellow);
				Time.timeScale = 0.02f;
			}
			break;
		case GameState.PAUSE:
			// PAUSE GAME
			if (viruses.Length <= 0 && infectedBloodCells.Length <= 0)
			{
				GameState = GameState.GAME_OVER;
			}
			
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				GameState = GameState.IN_GAME;
				GameObject bg = GameObject.Find("Background");
				bg.GetComponent<Background>().resetSpeeds();
				Time.timeScale = 1;
			}
			break;
		case GameState.GAME_OVER:
			// GAME OVER
			PlayerPrefs.SetFloat("player score",ScoreScript.getScore());
			Application.LoadLevel("HighScoreMenu");
			break;
		default:
			goto case GameState.IN_GAME;
		}
	}
	
	public static void countVirus() {
		instance.totalVirusCount++;
	}
	
	public static int getTotalVirusCount() {
		return instance.totalVirusCount;
	}
}

public enum GameState
{
	START_GAME, IN_GAME, PAUSE, GAME_OVER
}