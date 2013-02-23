using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class Global	 : MonoBehaviour 
	{
		public static GameState GameState = GameState.IN_GAME;
		public static GameObject PlayerVirus;
		//Adding this here to avoid null pointer
		public static GameObject[] Viruses= new GameObject[0];
		public static GameObject[] NormalRedBloodCells;
		public static GameObject[] InfectedBloodCells;
		public const int upperLimit = 360;
		public const int deathLimit = -400;
		public const int leftLimit = 475;
		public const int rightLimit = -475;
		
		void Start()
		{
		}
		
		void Update()
		{
			PlayerVirus = GameObject.FindGameObjectWithTag("Player");
			Viruses = GameObject.FindGameObjectsWithTag("Virus");
			NormalRedBloodCells = GameObject.FindGameObjectsWithTag("Red Blood Cell");
			InfectedBloodCells = GameObject.FindGameObjectsWithTag("Infected Blood Cell");
			if(Viruses.Length <= 0 && InfectedBloodCells.Length <= 0)
			{
				GameState = GameState.GAME_OVER;
			}
		}
	}
	
	public enum GameState
	{
		IN_GAME, PAUSE, GAME_OVER
	}
}

