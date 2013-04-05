using UnityEngine;
using System.Collections;

//Purely for testing
//GITHUB TEST
public class WhiteBloodCellSpawner : MonoBehaviour {
	public GameObject whiteBloodCell;
	public float spawnTimer;
	public float spawnTimerMax = 10.0f;
	private float score = 0;
	public float enemyMax = 0;
	public float dumbMax = 0;
	public float smartMax = 0;
	public float smarterMax = 0;
	public float totalNum = 5;
	public static float totalNumMax = 10;
	public float scorePeriod = 40000.0f;
	public static float scorePeriodMin = 20000.0f;
	public float difficultyPeriod;
	public float enemySpeed = 10.0f;
	public static float enemySpeedMax = 30.0f;
	// Update is called once per frame
	void Update () {
		score = ScoreScript.getScore();
		
		// change frequency of the enemy count oscillation
		scorePeriod = Mathf.Lerp(scorePeriod, scorePeriodMin, Time.deltaTime/score*3);
		// calculate period of the difficulty
		difficultyPeriod = score/scorePeriod*2*Mathf.PI;
		
		// scale number of maximum number of enemies allowed on the screen
		totalNum = Mathf.Lerp(totalNum, totalNumMax, Time.deltaTime/score*2);
		//print (totalNum);
		
		// scale enemy speed with player score
		enemySpeed = Mathf.Lerp(enemySpeed, enemySpeedMax, Time.deltaTime/score);
		
		// spawn dumb white blood cells with increasing probability
		dumbMax = totalNum/2.0f - Mathf.Cos(difficultyPeriod)*totalNum/2.0f + 1;
		// spawn smart white blood cells with increasing probability after dumb white blood cell spawn probability peaks
		if (difficultyPeriod > 2.0f/3.0f*Mathf.PI) 	smartMax = totalNum/2.0f - Mathf.Cos(difficultyPeriod - 2.0f/3.0f*Mathf.PI)*totalNum/2.0f;
		// spawn smarter white blood cells with increasing probability after rising smart white blood cell spawn probability has passed inflection point
		if (difficultyPeriod > 4.0f/3.0f*Mathf.PI)	smarterMax = totalNum/2.0f + Mathf.Cos(difficultyPeriod + 2.0f/3.0f*Mathf.PI)*totalNum/2.0f;
		// max total white blood cells is sum of max of each type of white blood cell
		enemyMax = dumbMax + smartMax + smarterMax;
		
		if (spawnTimer <= 0) {
			// create the new white blood cell at randomly selected location
			//print (getWhiteCount ());
			// only spawn anything if the white blood cell count is less than the max allowed
			if (getWhiteCount() < enemyMax) {
				//print ("Dumb Max: " + dumbMax);
				//print ("Smart Max: " + smartMax);
				//print ("Smarter Max: " + smarterMax);
				spawnCell(getCellType(), getSpawnSide());
				spawnTimerMax *= 0.98f;
			}
			spawnTimer = spawnTimerMax;
			// reduce the spawn wait by 0.2% each time an enemy is spawned;
			print("spawnTimerMax = " + spawnTimerMax);
		} else {
			// count down to spawn
			spawnTimer -= Time.deltaTime;
		}
	
	}
	
	private Vector3 getSpawnSide() {
		int x,y;
			
		// pick random side to spawn on
		int side = Random.Range(1,5);
		switch (side) {
			case 1:
				// spawn somewhere on left side of screen
				x = Global.LeftLimit;
				y = Random.Range(Global.DeathLimit, Global.UpperLimit+1);
				break;
			case 2:
				// spawn somewhere on right side of screen
				x = Global.RightLimit;
				y = Random.Range(Global.DeathLimit, Global.UpperLimit+1);
				break;
			case 3:
				// spawn somewhere on bottom side of screen
				x = Random.Range(Global.LeftLimit, Global.RightLimit);
				y = Global.DeathLimit;
				break;
			case 4:
				// spawn somewhere on top side of screen
				x = Random.Range(Global.LeftLimit, Global.RightLimit);
				y = Global.UpperLimit;
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
		newUnit.SendMessage("setSpeed", enemySpeed);
	}
			
	private int getWhiteCount() {
		GameObject[] dumbs = GameObject.FindGameObjectsWithTag("Enemy Dumb");
		GameObject[] smarts = GameObject.FindGameObjectsWithTag("Enemy Smart");
		GameObject[] smarters = GameObject.FindGameObjectsWithTag("Enemy Smarter");
		return dumbs.Length + smarts.Length + smarters.Length;
	}
}