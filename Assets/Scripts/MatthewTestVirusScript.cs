using UnityEngine;
using System.Collections;

public class MatthewTestVirusScript : MonoBehaviour {
	Camera cam;
	public Vector3 goalPosition;
	public float speed = 60.0f;
	private ArrayList viruses;
	public bool makeBabies=true;
	
	
	void Start(){
		goalPosition=transform.position;
		viruses = new ArrayList();
	}
	
	// Update is called once per frame
	void Update () {
		GetOtherPlayers();
		MoveAway();
		
		if(cam!=null){
			if(Input.GetMouseButton(0)){
				goalPosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,-82));
			}
			
			
			Vector3 difference = goalPosition-transform.position;
			
			if(difference.magnitude>0.1f){
				difference.Normalize();
				
				transform.position+= difference*Time.deltaTime*speed;
			}
		
		}
		else{
			GameObject camObj = GameObject.FindGameObjectWithTag("MainCamera") as GameObject;
			cam = camObj.GetComponent<Camera>();
		}
	}
	
	void MoveAway(){
		foreach(GameObject otherVirus in viruses){
			if(otherVirus!=null){
				Vector3 difference = otherVirus.transform.position-transform.position;
				if(difference.magnitude<20){
					difference.Normalize();
					difference*=21;
					transform.position-= difference;
				}
			}
		}
	}
	
	void OnTriggerEnter(Collider other){
		if(makeBabies){
			if(other.tag=="Red Blood Cell"){
				//Debug.Log("I hit a: "+other.name);
				Object newVirus = Instantiate(gameObject,other.transform.position,transform.rotation);
				newVirus.name ="Bob";
				Destroy(other.gameObject);
			}
			if(other.tag=="Enemy"){
				Destroy(other.gameObject);
			}
		}
	}
	
	void GetOtherPlayers(){
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		
		foreach(GameObject virus in players){
			if(!viruses.Contains(virus) && virus!=gameObject){
				viruses.Add(virus);
			}
		}
	}
	
}
