using UnityEngine;
using System.Collections;

//Purely for testing
public class WhiteBloodCellSpawner : MonoBehaviour {
	public GameObject whiteBloodCell;
	public float spawnTimer;
	public float spawnTimerMax = 1.0f;
	
	// Update is called once per frame
	void Update () {
		if(spawnTimer<=0){
			
			Instantiate(whiteBloodCell,new Vector3(Random.Range(-440,440), Random.Range(-350,350), -22), whiteBloodCell.transform.rotation);
			spawnTimer=spawnTimerMax;
		}
		else{
			spawnTimer-=Time.deltaTime*Random.Range(1,2);
		}
	
	}
}
