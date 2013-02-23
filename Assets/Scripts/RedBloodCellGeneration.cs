using UnityEngine;
using System.Collections;

namespace AssemblyCSharp
{
	public class RedBloodCellGeneration : MonoBehaviour 
	{	
		public GameObject prefab;
		private static GameObject bloodCell;
		private const int initialSpawnNumber = 30;
		private const float spawnThreshold = 0.95f;
		
		// Use this for initialization
		void Start () 
		{
			bloodCell = prefab;
			for(int i = 0; i < initialSpawnNumber; i++)
			{
				spawnNormalCell(Random.value * (Global.leftLimit - Global.rightLimit) - Global.leftLimit,
					Random.value * (Global.upperLimit - Global.deathLimit) - Global.upperLimit);
			}
		}
		
		// Update is called once per frame
		void Update () 
		{
			if(Random.value > spawnThreshold)
			{
				spawnNormalCell(Random.value * (Global.leftLimit - Global.rightLimit) - Global.leftLimit);
			}
		}
		
		public static void spawnNormalCell(float x)
		{
			spawnNormalCell(x, Global.upperLimit + 50);
		}
		
		private static void spawnNormalCell(float x, float y)
		{
			Instantiate(bloodCell, new Vector3(x , y, 5f), Quaternion.Euler(90f, 0f, 0f));
		}
	}
}
