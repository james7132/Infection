using UnityEngine;
using System.Collections;

public class WhiteBloodCellControllerDumb : MonoBehaviour {
	//WhiteBloodCells
	GameObject[] whiteBloodCells;
	//Viruses
	GameObject[] viruses;
	Vector3[] prevWhitePositions;
	Vector3 sumWhitePositions;
	Vector3 avgWhitePosition;
	
	void getSumWhitePositions() {
		sumWhitePositions = new Vector3(0,0,0);
		foreach (GameObject cell in whiteBloodCells) {
			sumWhitePositions += cell.transform.position;
		}
	}
	
	Vector3[] getWhitePositions() {
		int i = 0;
		Vector3[] positions = new Vector3[whiteBloodCells.Length];
		foreach (GameObject cell in whiteBloodCells) {
			positions[i] = cell.transform.position;
			i++;
		}
		return positions;
	}
	
	// Update is called once per frame
	void Update () {
		getWhiteBloodCells();
		getSumWhitePositions();
		avgWhitePosition = sumWhitePositions/whiteBloodCells.Length;
		getViruses();
		
		int i = 0;
		foreach(GameObject cell in whiteBloodCells){
			if(cell!=null){
				//Vector3 cellPos = determineGoalPosition(cell);
				WhiteBloodCellUnit unit = cell.GetComponent<WhiteBloodCellUnit>();
				
				Vector3 destination = avgWhitePosition;
				
				if (viruses.Length > 0) {
					destination = avgWhitePosition*0.9f + getAverageVirusPosition()*0.1f;
					//destination = new Vector3(avgWhitePosition
				}
				
				if(unit!=null){
					//Debug.Log (avgWhitePositions);
					//Move towards cell position
					unit.speed = 20.0f;
					unit.MoveTowards(destination);
					
					//Fire if you can
					GameObject virus = findClosestVirus(cell);
					if(virus!=null){
						unit.Fire(virus.transform.position,400.0f);
					}
				}
			}
			
			i++;
		}
		
	}
	
	
	//Determind goal position for white blood cell
	Vector3 determineGoalPosition(GameObject whiteBloodCell){
		Vector3 goalPos = new Vector3();
	
		foreach(GameObject virus in viruses){
			
			Vector3 distanceToVirus = virus.transform.position-whiteBloodCell.transform.position;
			//Debug.Log("Even getting here: "+distanceToVirus);
			if(distanceToVirus.magnitude<200){
				distanceToVirus.Normalize();
				goalPos-=distanceToVirus*220;
			}
			
			if(distanceToVirus.magnitude>220){
				//distanceToVirus.Normalize();	
				goalPos+=distanceToVirus;
			}
			
		}
		
		goalPos/=viruses.Length;
		
		Vector3 avoidPos = new Vector3();
		
		foreach(GameObject cell in whiteBloodCells){
			//If not you
			if(cell!=whiteBloodCell){
				Vector3 distanceToCells = cell.transform.position-whiteBloodCell.transform.position;
				
				if(distanceToCells.magnitude<300){
					distanceToCells.Normalize();
					
					avoidPos-=distanceToCells*320;
				}
			}
		}
		if(whiteBloodCells.Length>1){
			avoidPos/=(whiteBloodCells.Length-1);
			
			goalPos+=avoidPos;
		}
		
		goalPos.z=0;
		goalPos+=whiteBloodCell.transform.position;
		
		return goalPos;
	}
	
	
	//Find Closest Virus to the passed in cell
	GameObject findClosestVirus(GameObject whiteBloodCell){
		GameObject closest = null;
		float distance = 1000.0f;
		
		foreach(GameObject virus in viruses){
			float currDist = (virus.transform.position-whiteBloodCell.transform.position).magnitude;
			
			if(currDist<distance){
				distance=currDist;
				closest=virus;
			}
		}
		
		return closest;
	}
	
	
	
	//We'll need to change this when we have other enemies
	void getWhiteBloodCells(){
		whiteBloodCells=GameObject.FindGameObjectsWithTag("Enemy");
	}
	
	void getViruses(){
		viruses = GameObject.FindGameObjectsWithTag("Player");
	}
	
	Vector3 getAverageVirusPosition(){
		Vector3 middleOfViruses = new Vector3();
		
		foreach(GameObject virus in viruses){
			middleOfViruses+=virus.transform.position;
		}
		middleOfViruses/=viruses.Length;
		
		return middleOfViruses;
	}
}
