using UnityEngine;
using System.Collections;

namespace AssemblyCSharp
{
	public class VirusGeneration : MonoBehaviour 
	{	
		public GameObject prefab;
		public static GameObject virus;
		
		// Use this for initialization
		void Start () 
		{
			virus = prefab;
			spawnVirus(0f, 0f);
			selectNewVirus();
		}
		
		public static void spawnVirus(float x, float y)
		{
			Instantiate(virus, new Vector3(x, y, 5f), Quaternion.Euler(90f, 0f, 0f));
		}
		
		public static void selectNewVirus()
		{
			if(Global.Viruses.Length != 0)
			{
				Global.Viruses[0].tag = "Player";
			}
		}
		
		// Update is called once per frame
		void Update () 
		{
			
		}
	}
}