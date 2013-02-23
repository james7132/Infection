using UnityEngine;
using System.Collections;

public class RotatingScript : MonoBehaviour {
	//Random variable amounds
	private float x, y, z;
	//Speed to set
	public float speed;
	
	// Use this for initialization
	void Start () {
		//Set random bits
		x = Random.Range(-1f,1f);
		y = Random.Range(-1f,1f);
		z = Random.Range(-1f,1f);
	}
	
	// Update is called once per frame
	void Update () {
		//Rotate randomly
		transform.Rotate(x*speed*Time.deltaTime,y*speed*Time.deltaTime,z*speed*Time.deltaTime);
	}
}
