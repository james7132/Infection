using UnityEngine;
using System.Collections;

public class WhiteBloodCellProjectile : MonoBehaviour {
	//The direction we're travelling in, should be a unit vector
	public Vector3 direction;
	//The speed to we should move at
	public float speed;
	//How long have you been alive anyway?
	private float lifeTimer;
	//Max time you're allowed to be alive, basically the range
	private float lifeTimerMax = 0.5f;
	
	//Just move till you die
	void Update(){
		transform.position+= direction*speed*Time.deltaTime;
		
		
		//Life timer business
		if(lifeTimer<lifeTimerMax){
			lifeTimer+=Time.deltaTime;
		}
		else{
			Destroy(gameObject);
		}
	}
	
	
	//If you hit a virus, destroy it and yourself
	void OnTriggerEnter(Collider other){
		if(other.tag=="Player"){
			Destroy(other.gameObject);
			Destroy(gameObject);
		}
	}
}
