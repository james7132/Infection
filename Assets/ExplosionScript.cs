using UnityEngine;
using System.Collections;

public class ExplosionScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		ParticleSystem[] particleSystems = gameObject.GetComponentsInChildren<ParticleSystem>();
		bool check = true;
		foreach(ParticleSystem p in particleSystems)
		{
			check &= p.isStopped;
		}
		if(check)
		{
			Destroy (gameObject);
		}
	}
}
