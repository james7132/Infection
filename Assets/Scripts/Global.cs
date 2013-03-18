using System;
using UnityEngine;

//namespace AssemblyCSharp
//{
	public class Global	 : MonoBehaviour 
	{
		public static GameState GameState = GameState.IN_GAME;
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
		
		public Global()
		{
			UpperLimit = Upper_Limit;
			DeathLimit = Death_Limit;
			LeftLimit = Left_Limit;
			RightLimit = Right_Limit;
		}
		
		void Start()
		{
			playerScore = 0;
		}
		
		void Update()
		{
			GameObject[] viruses = GameObject.FindGameObjectsWithTag("Virus");
			GameObject[] infectedBloodCells = GameObject.FindGameObjectsWithTag("Infected Blood Cell");
			if(viruses.Length <= 0 && infectedBloodCells.Length <= 0)
			{
				GameState = GameState.GAME_OVER;
			}
		}
	}
	
	public enum GameState
	{
		IN_GAME, PAUSE, GAME_OVER
	}
//}

