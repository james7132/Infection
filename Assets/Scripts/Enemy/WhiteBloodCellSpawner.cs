using UnityEngine;
using System.Collections;

//Purely for testing
//GITHUB TEST
public class WhiteBloodCellSpawner : MonoBehaviour {
	public GameObject whiteBloodCell;
	public float spawnTimer;
	public float spawnTimerMax = 10.0f;
	public const int upperLimit = 425;
	public const int lowerLimit = -425;
	public const int leftLimit = 525;
	public const int rightLimit = -525;
	private float score = 0;
	public float enemyMax = 0;
	public float dumbMax = 0;
	public float smartMax = 0;
	public float smarterMax = 0;
	public static float totalConst = 9;
	
	// Update is called once per frame
	void Update () {
		score = ScoreScript.getScore();
		float thing = score/400000.0f*2*Mathf.PI;
		// spawn dumb white blood cells with increasing probability
		dumbMax = totalConst/2.0f - Mathf.Cos(thing)*totalConst/2.0f;
		// spawn smart white blood cells with increasing probability after dumb white blood cell spawn probability peaks
		if (thing > 2.0f/3.0f*Mathf.PI) 	smartMax = totalConst/2.0f - Mathf.Cos(thing - 2.0f/3.0f*Mathf.PI)*totalConst/2.0f;
		// spawn smarter white blood cells with increasing probability after rising smart white blood cell spawn probability has passed inflection point
		if (thing > 4.0f/3.0f*Mathf.PI)		smarterMax = totalConst/2.0f + Mathf.Cos(thing + 2.0f/3.0f*Mathf.PI)*totalConst/2.0f;
		// max total white blood cells is sum of max of each type of white blood cell
		enemyMax = dumbMax + smartMax + smarterMax;
		if (spawnTimer <= 0) {
			// create the new white blood cell at randomly selected location
			print (getWhiteCount ());
			// only spawn anything if the white blood cell count is less than the max allowed
			if (getWhiteCount() < enemyMax) {
				print ("Dumb Max: " + dumbMax);
				print ("Smart Max: " + smartMax);
				print ("Smarter Max: " + smarterMax);
				spawnCell(getCellType(), getSpawnSide());
				spawnTimerMax *= 0.98f;
			}
			spawnTimer = spawnTimerMax;
			// reduce the spawn wait by 0.2% each time an enemy is spawned;
			print("spawnTimerMax = " + spawnTimerMax);
		} else {
			// count down to spawn
			spawnTimer-=Time.deltaTime;
		}
	
	}
	
	private Vector3 getSpawnSide() {
		int x,y;
			
		// pick random side to spawn on
		int side = Random.Range(1,5);
		switch (side) {
			case 1:
				// spawn somewhere on left side of screen
				x = leftLimit;
				y = Random.Range(lowerLimit,upperLimit+1);
				break;
			case 2:
				// spawn somewhere on right side of screen
				x = rightLimit;
				y = Random.Range(lowerLimit,upperLimit+1);
				break;
			case 3:
				// spawn somewhere on bottom side of screen
				x = Random.Range(leftLimit,rightLimit);
				y = lowerLimit;
				break;
			case 4:
				// spawn somewhere on top side of screen
				x = Random.Range(leftLimit,rightLimit);
				y = upperLimit;
				break;
			default:
				goto case 4;
		}
		
		return new Vector3(x,y,5);
	}
	
	private string getCellType () {
		string enemyType;
		// pick a number between 0 and the maximum number of enemies
		float rand = Random.Range(0,enemyMax);
		
		if (rand <= dumbMax) {
			print ("[ " + rand + " Dumb ]");
			// spawn dumb enemy
			enemyType = "Dumb";
		} else if (rand <= dumbMax + smartMax) {
			print("[ " + rand + " Smart ]");
			// spawn smart enemy
			enemyType = "Smart";
		} else {
			print("[ " + rand + " Smarter ]");
			// spawn smarter enemy
			enemyType = "Smarter";
		}
		
		//print (enemyType);
		return enemyType;
	}
	
	private void spawnCell(string enemyType, Vector3 spawnLocation) {
		GameObject newCell = Instantiate(whiteBloodCell, spawnLocation, whiteBloodCell.transform.rotation) as GameObject;
		WhiteBloodCellUnit newUnit = newCell.GetComponent<WhiteBloodCellUnit>();
		//print (newUnit);
		newUnit.SendMessage("setType", enemyType);
	}
			
	private int getWhiteCount() {
		GameObject[] dumbs = GameObject.FindGameObjectsWithTag("Enemy Dumb");
		GameObject[] smarts = GameObject.FindGameObjectsWithTag("Enemy Smart");
		GameObject[] smarters = GameObject.FindGameObjectsWithTag("Enemy Smarter");
		return dumbs.Length + smarts.Length + smarters.Length;
	}
}