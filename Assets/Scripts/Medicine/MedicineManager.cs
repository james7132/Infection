using UnityEngine;
using System.Collections;

namespace AssemblyCSharp
{
	public class MedicineManager : MonoBehaviour 
	{	
		public float spawnTime = 45.0f;
		private float spawnTimer = 0.0f;
		public GameObject MedicineProtect;
		public GameObject MedicineSlow;
		public GameObject MedicineHurt;
		private static GameObject medProtect;
		private static GameObject medSlow;
		private static GameObject medHurt;
		private float difficultySpike = 0.0f;
		// increase with difficulty of 45-arctan((1/5)x)
		
		
		
		// Use this for initialization
		void Start () 
		{
			medHurt = MedicineHurt;
			medSlow = MedicineSlow;
			medProtect = MedicineProtect;
		}
		
		// Update is called once per frame
		void Update () 
		{
			difficultySpike += Time.deltaTime/2.0f;
			spawnTimer += Time.deltaTime;
			
			if (spawnTimer >= spawnTime) {
				spawnTimer = 0.0f;
				spawnTime = spawnTime - Mathf.Atan((1.0f/5.0f)*difficultySpike);
				spawnMed(Random.value * (Global.LeftLimit - Global.RightLimit) - Global.LeftLimit);
			}
		}
		
		public static void spawnMed(float x)
		{
			spawnMed(x, Global.UpperLimit + 50);
		}
		
		private static void spawnMed(float x, float y)
		{
			
			float r = Random.value;
			if (r <= .33f)
				Instantiate(medHurt, new Vector3(x , y, 7f), Quaternion.Euler(90f, 0f, 0f));
			if (r >= .33f && r <= .66f)
				Instantiate(medSlow, new Vector3(x , y, 7f), Quaternion.Euler(90f, 0f, 0f));
			if (r >= .66f)
				Instantiate(medProtect, new Vector3(x , y, 7f), Quaternion.Euler(90f, 0f, 0f));
		}
	}
}
