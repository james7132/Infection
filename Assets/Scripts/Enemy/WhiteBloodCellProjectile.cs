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
		// I DON'T LIKE THIS BEING HERE RIGHT NOW BUT THIS NEEDS TO BE CALLED WHEN THE LAST VIRUS HAS DIED
		if(other.tag=="Player"){
			if (lastVirus()) {
				PlayerPrefs.SetFloat("player score",ScoreScript.getScore());
				Application.LoadLevel("HighScoreMenu");
			} else {
				Destroy(other.gameObject);
				Destroy(gameObject);
			}
		}
	}
	
	private bool lastVirus() {
		GameObject[] viruses = GameObject.FindGameObjectsWithTag("Player");
		return (viruses.Length == 1);
	}	
}
