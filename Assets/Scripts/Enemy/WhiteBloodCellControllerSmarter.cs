using UnityEngine;
using System.Collections;

public class WhiteBloodCellControllerSmarter : MonoBehaviour {
	//WhiteBloodCells
	GameObject[] whiteBloodCells;
	//Viruses
	GameObject[] viruses;
	
	GameObject suicide;
	WhiteBloodCellUnit suicideUnit;
	
	// Update is called once per frame
	void Update () {
		
		getWhiteBloodCells();
		getViruses();
		
		foreach(GameObject cell in whiteBloodCells){
			
			if(cell!=null && cell!=suicide){
				Vector3 cellPos = determineGoalPosition(cell);
				WhiteBloodCellUnit unit = cell.GetComponent<WhiteBloodCellUnit>();
				
				if(unit!=null){
					//Move towards cell position
					unit.MoveTowards(cellPos);
					
					
					//Fire if you can
					GameObject virus = findClosestVirus(cell);
					if(virus!=null){
						unit.Fire(virus.transform.position,400.0f);
					}
				}
			}
		}
		
		//If we've got more than 3 pick one to be the suicice
		if(whiteBloodCells.Length>3 && suicide==null){
			suicide = whiteBloodCells[3];
			suicideUnit = suicide.GetComponent<WhiteBloodCellUnit>();
			
			
			//You're the suicide! Boost your fire rate
			suicideUnit.fireTimerMax=0.2f;
			suicideUnit.speed+=80;
		}
		
		if(suicide!=null){
			//If we've got a suicide send it running at the middle of the viruses
			suicideUnit.MoveTowards(getAverageVirusPosition());
			suicideUnit.Fire(findClosestVirus(suicide).transform.position, 400.0f);
		}
		
	}
	
	
	
	
	//Determind goal position for white blood cell
	Vector3 determineGoalPosition(GameObject whiteBloodCell){
		Vector3 goalPos = new Vector3();
		
		
		
		foreach(GameObject virus in viruses){
			
		Vector3 distanceToVirus = virus.transform.position-whiteBloodCell.transform.position;
			
			
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
		
		
		/**
		GameObject virus = findClosestVirus(whiteBloodCell);
	
		if(virus!=null){
			Vector3 distanceToVirus = virus.transform.position-transform.position;
			
			if(distanceToVirus.magnitude>20){
				//distanceToVirus.Normalize();
				
				goalPos+=distanceToVirus;
			}
		}
		*/
		//goalPos.z=0;
		//goalPos+=whiteBloodCell.transform.position;
		
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
	
	//For divebombing the middle of the viruses
	Vector3 getAverageVirusPosition(){
		Vector3 middleOfViruses = new Vector3();
		
		foreach(GameObject virus in viruses){
			middleOfViruses+=virus.transform.position;
		}
		middleOfViruses/=viruses.Length;
		
		return middleOfViruses;
	}
	
	
	//We'll need to change this when we have other enemies
	void getWhiteBloodCells(){
		whiteBloodCells=GameObject.FindGameObjectsWithTag("Enemy");
	}
	
	void getViruses(){
		viruses = GameObject.FindGameObjectsWithTag("Player");
	}
}
