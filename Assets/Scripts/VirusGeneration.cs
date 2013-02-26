using UnityEngine;
using System.Collections;

namespace AssemblyCSharp
{
	public class VirusGeneration : MonoBehaviour 
	{	
		public GameObject prefab;
		public static GameObject virus;
		
		private AudioSource startSound;

		// Use this for initialization
		void Start () 
		{
			virus = prefab;
			spawnVirus(0f, 0f);

			startSound = (AudioSource)gameObject.AddComponent("AudioSource");
	        startSound.clip = (AudioClip)Resources.Load("gameover-abstract");
			startSound.volume = 0.5f;
			startSound.rolloffMode = AudioRolloffMode.Custom;
			startSound.Play();
		}
		
		public static void spawnVirus(float x, float y)
		{
			Instantiate(virus, new Vector3(x, y, 5f), Quaternion.Euler(90f, 0f, 0f));
		}
		
		// Update is called once per frame
		void Update () 
		{
			
		}
	}
}