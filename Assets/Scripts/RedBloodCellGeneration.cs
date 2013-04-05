using UnityEngine;
using System.Collections;

namespace AssemblyCSharp
{
	public class RedBloodCellGeneration : MonoBehaviour 
	{	
		public GameObject prefab;
		private static GameObject bloodCell;
		private const int initialSpawnNumber = 10;
		private const float spawnThreshold = 0.3f;
		public float spawnTimer = 0;
		public float spawnTimerResetMin = 0.2f;
		private float spawnTimerReset = 1.0f;
		private float score;
		
		// Use this for initialization
		void Start () 
		{
			bloodCell = prefab;
			for(int i = 0; i < initialSpawnNumber; i++)
			{
				spawnNormalCell(Random.value * (Global.LeftLimit - Global.RightLimit) - Global.LeftLimit,
					Random.value * (Global.UpperLimit - Global.DeathLimit) - Global.UpperLimit);
			}
		}
		
		// Update is called once per frame
		void Update () 
		{
			score = ScoreScript.getScore();
			spawnTimerReset = Mathf.Lerp(spawnTimerReset, spawnTimerResetMin, Time.deltaTime/score*20);
			print("spawntimerreset: " + spawnTimerReset);
			if (spawnTimer < spawnTimerReset) 
			{
				spawnTimer += Time.deltaTime;
			} 
			else 
			{
				if(Random.value > spawnThreshold)
				{
					spawnNormalCell(Random.value * (Global.LeftLimit - Global.RightLimit) - Global.LeftLimit);
				}
				spawnTimer = 0;
			}
		}
		
		public static void spawnNormalCell(float x)
		{
			spawnNormalCell(x, Global.UpperLimit + 50);
		}
		
		private static void spawnNormalCell(float x, float y)
		{
			Instantiate(bloodCell, new Vector3(x , y, 5f), Quaternion.Euler(90f, 0f, 0f));
		}
	}
}
