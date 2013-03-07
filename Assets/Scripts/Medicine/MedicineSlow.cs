using UnityEngine;
using System.Collections;

namespace AssemblyCSharp
{
	public class MedicineSlow : MonoBehaviour 
	{	
		public ParticleSystem cloud;
		private ParticleSystem newCloud;
		public float speed = 0.85f;

		// Use this for initialization
		void Start () 
		{
			Vector3 pos = gameObject.transform.position;
			newCloud = Instantiate(cloud, pos, Quaternion.Euler(0.0f, 0.0f, 0.0f)) as ParticleSystem;
		}
		
		// Update is called once per frame
		void Update () 
		{
			gameObject.transform.localPosition -= new Vector3(0f, speed, 0f);
			newCloud.gameObject.transform.localPosition = gameObject.transform.localPosition;
			if(gameObject.transform.localPosition.y+250.0f < Global.DeathLimit)
			{
				Destroy(newCloud.gameObject);
				Destroy(gameObject);
			}
		}
	}
}
