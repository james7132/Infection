using UnityEngine;
using System.Collections;

//Purely for testing
//GITHUB TEST
public class WhiteBloodCellSpawner : MonoBehaviour {
	public GameObject whiteBloodCell;
	public float spawnTimer;
	public float spawnTimerMax = 3.0f;
	public const int upperLimit = 425;
	public const int lowerLimit = -425;
	public const int leftLimit = 525;
	public const int rightLimit = -525;
	
	// Update is called once per frame
	void Update () {
		if (spawnTimer<=0) {
			// create the new white blood cell at randomly selected location.
			spawnCell(getCellType(), getSpawnSide());
			spawnTimer=spawnTimerMax;
		} else {
			//print(spawnTimer);
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
		
		return new Vector3(x,y,-22);
	}
	
	private string getCellType () {
		//double score = Global.getScore();
		string enemyType;
		int rand = Random.Range(0,3);
		switch (rand) {
		case 0:
			enemyType = "Dumb";
			break;
		case 1:
			enemyType = "Smart";
			break;
		case 2:
			enemyType = "Smarter";
			break;
		default:
			goto case 0;
		}
		print (enemyType);
		return enemyType;
	}
	
	private void spawnCell(string enemyType, Vector3 spawnLocation) {
		GameObject newCell = Instantiate(whiteBloodCell, spawnLocation, whiteBloodCell.transform.rotation) as GameObject;
		WhiteBloodCellUnit newUnit = newCell.GetComponent<WhiteBloodCellUnit>();
		//print (newUnit);
		newUnit.SendMessage("setType", enemyType);
	}
}