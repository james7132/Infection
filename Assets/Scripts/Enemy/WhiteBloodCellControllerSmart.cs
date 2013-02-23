using UnityEngine;
using System.Collections;

public class WhiteBloodCellControllerSmart : MonoBehaviour {
	//WhiteBloodCells
	GameObject[] whiteBloodCells;
	//Viruses
	GameObject[] viruses;
	
	
	// Update is called once per frame
	void Update () {
		
		getWhiteBloodCells();
		getViruses();
		
		foreach(GameObject cell in whiteBloodCells){
			
			if(cell!=null){
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
		
	}
	
	
	//Determind goal position for white blood cell
	Vector3 determineGoalPosition(GameObject whiteBloodCell){
		Vector3 goalPos = new Vector3();
		
		
		
		foreach(GameObject virus in viruses){
			
		Vector3 distanceToVirus = virus.transform.position-whiteBloodCell.transform.position;
			
			Debug.Log("Even getting here: "+distanceToVirus);
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
	
	
	//We'll need to change this when we have other enemies
	void getWhiteBloodCells(){
		whiteBloodCells=GameObject.FindGameObjectsWithTag("Enemy");
	}
	
	void getViruses(){
		viruses = GameObject.FindGameObjectsWithTag("Player");
	}
}
