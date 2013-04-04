using UnityEngine;
using System.Collections;

public class LegitVirusScript : MonoBehaviour {
	Camera cam;
	public Vector3 goalPosition;
	public float speed = 160.0f;
	
	//James added
	private float baseSpeed = 160.0f;
	private bool slowed;
	private bool helpless;
	private bool poisoned;
	private float poisTimer = 0.0f;
	private float hurtTimer;
	private float slowTimer;
	private float protectTimer;
	
	private ArrayList viruses;
	public bool makeBabies=true;
	public float infectionSpeed=1.0f;
	
	
	private AudioSource deathSound;
	private AudioSource juicySound;

	public GameObject explosionFab;
	
	//Infecting a thing, I cannot move 
	public bool infecting;
	public GameObject target;
	private Vector3 prevTargetPos;
	
	
	public bool selected=true;
	
	//The current Rotation Vector
	Vector3 curVector;
	
	
	void Start(){
		//James added
		//baseSpeed = speed;
		slowed = false;
		helpless = false;
		poisoned = false;
		poisTimer = 0.0f;
		slowTimer = 0.0f;
		hurtTimer = 0.0f;
		protectTimer = 0.0f;
		
		goalPosition=transform.position;
		viruses = new ArrayList();
		
		deathSound = (AudioSource)gameObject.AddComponent("AudioSource");
        deathSound.clip = (AudioClip)Resources.Load("cells-splatter");
		deathSound.volume = 0.5f;
		deathSound.rolloffMode = AudioRolloffMode.Custom;
		
		juicySound = (AudioSource)gameObject.AddComponent("AudioSource");
        juicySound.clip = (AudioClip)Resources.Load("gameover-bodyfailure");
		juicySound.volume = 0.45f;
		juicySound.rolloffMode = AudioRolloffMode.Custom;
	}
	
	// Update is called once per frame
	void Update () {
		GetOtherPlayers();
		MoveAway();
		
		
		//If distance to target is too great, you aren't infected
		if(target!=null){
			Vector3 toTarget = target.transform.position-transform.position;
			if(toTarget.magnitude>100){
				infecting=false;
			}
		}
		
		//COLOR DECISION
		if(!selected)
		{
			Color lightYellow = new Color(1.0f,(241.0f/255.0f),(135.0f/255.0f),1.0f);
			
			if(renderer!=null && !renderer.material.color.Equals(lightYellow))
			{
				renderer.material.color = Color.Lerp(renderer.material.color, lightYellow,Time.deltaTime*20);
			}
			
		}
		else
		{
			Color yellow = new Color(1.0f,1.0f,0.0f,1.0f);
			
			if(renderer!=null && !renderer.material.color.Equals(yellow))
			{
				renderer.material.color = Color.Lerp(renderer.material.color, yellow,Time.deltaTime*20);
			}
		}
		
		
		
		if(cam!=null ){
			
			if(selected && Input.GetMouseButton(1)){
					goalPosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y, 0));
					
					curVector = transform.forward;
					goalPosition.z = 0;
			}
			
			//Moving bits
			if(!infecting)
			{
				//Turn to face goal position
				Vector3 lookVector = goalPosition-transform.position;
				
				//I have to do this because the virus model is rotated 90 degrees off
				
				Vector3 newLookVector = new Vector3();
				
				Quaternion targetRotation = new Quaternion();
				if(lookVector.y>0){
					
					newLookVector = new Vector3(lookVector.y, -1*lookVector.x, lookVector.z);
					//newLookVector = Vector3.forward;//new Vector3(lookVector.y, -1*lookVector.x, lookVector.z);
					//newLookVector = new Vector3(-1*lookVector.y, lookVector.x, lookVector.z);
					//newLookVector*=-1;
					targetRotation = (Quaternion.LookRotation((newLookVector), Vector3.down));
				}
				
				else{
					newLookVector = new Vector3(lookVector.y, -1*lookVector.x, lookVector.z);
					targetRotation = (Quaternion.LookRotation((newLookVector)));
				}
				
				
			
				transform.rotation = Quaternion.Slerp(transform.rotation,targetRotation,Time.deltaTime*5.0f);
				
				
				//Move normally
				Vector3 difference = goalPosition-transform.position;
				
				if(difference.magnitude>0.1f){
					difference.Normalize();
					
					Vector3 newPos = transform.position;
					
					newPos+= difference*Time.deltaTime*speed;
					
					transform.position=newPos;
					
				}
			}
			//If we don't have a target, we are no longer infecting
			else if(infecting && target==null){
				infecting=false;
			}
			
		
		}
		else{
			ScoreScript.addScore(10, true, transform.localPosition);
			GameObject camObj = GameObject.FindGameObjectWithTag("MainCamera") as GameObject;
			cam = camObj.GetComponent<Camera>();
		}
		
		//James Added
		if (slowed) {
			speed = baseSpeed/2.0f;
		}
		
		if (!slowed) {
			speed = baseSpeed;
		}
		
		if (!poisoned) {
			poisTimer = 0.0f;	
		}
		if (poisoned) {
			poisTimer += Time.deltaTime;
		}
		if (poisTimer > 5.0f && poisoned) {
			Destroy(gameObject);
		}
		
		hurtTimer += Time.deltaTime;
		protectTimer += Time.deltaTime;
		slowTimer += Time.deltaTime;
		
		if (hurtTimer > 0.5f) {
			poisoned = false;
			hurtTimer = 0.0f;
		}
		if (protectTimer > 0.5f) {
			helpless = false;
			protectTimer = 0.0f;
		}
		if (slowTimer > 0.5f) {
			slowed = false;
			slowTimer = 0.0f;
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
				
				RedBloodCellScript red = other.GetComponent<RedBloodCellScript>();
				red.Infect(this,infectionSpeed);
				infecting=true;
				target=other.gameObject;
				prevTargetPos=target.transform.position;
				
				if(juicySound!=null){
					juicySound.Play();
				}
				if(explosionFab!=null) {
					Instantiate(explosionFab, transform.position, transform.rotation);
				}
			}
			
			//James Added - added helpless condition
			if((other.tag=="Enemy Dumb" || other.tag=="Enemy Smart" || other.tag=="EnemySmarter") && !helpless){
				deathSound.Play();
				if(explosionFab != null) {
					Instantiate(explosionFab, transform.position, transform.rotation);
				}
				ScoreScript.addScore(100, true, transform.localPosition);
				Destroy(other.gameObject);
			}
		}
	}
	
	//James Added
	void OnTriggerStay(Collider other) {
		if(other.tag=="MedicineSlow"){
			slowed = true;
			slowTimer = 0.0f;
		}
		
		if(other.tag=="MedicineHurt"){
			poisoned = true;
			hurtTimer = 0.0f;
		}
		
		if(other.tag=="MedicineProtect"){
			helpless = true;
			protectTimer = 0.0f;
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
	
	//Message Sent to me to let me know I can stop Infectiong
	public void YouCanStopInfectingNow(){
		infecting=false;
	}
}
