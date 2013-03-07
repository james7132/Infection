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
	
	private Object[] bursts;
	private float[] burstTimes;
	private int totalBursts;
	
	private ArrayList viruses;
	public bool makeBabies=true;
	public float infectionSpeed=1.0f;
	
	
	private AudioSource deathSound;
	private AudioSource juicySound;
	private AudioSource clickSound;

	public GameObject explosionFab;
	
	//Infecting a thing, I cannot move 
	public bool infecting;
	private GameObject target;
	private Vector3 prevTargetPos;
	
	
	public bool selected=true;
	
	
	
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
		
		bursts = new Object[1000];
		burstTimes = new float[1000];
		totalBursts = 0;
		
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
		
		clickSound = (AudioSource)gameObject.AddComponent("AudioSource");
        clickSound.clip = (AudioClip)Resources.Load("selection");
		clickSound.volume = 0.35f;
		clickSound.rolloffMode = AudioRolloffMode.Custom;
	}
	private bool _clickHoldLockCheck = false;
	
	// Update is called once per frame
	void Update () {
		GetOtherPlayers();
		MoveAway();
		
		//COLOR DECISION
		if(!selected)
		{
			Color lightYellow = new Color(1.0f,(241.0f/255.0f),(135.0f/255.0f),1.0f);
			
			if(!renderer.material.color.Equals(lightYellow))
			{
				renderer.material.color = Color.Lerp(renderer.material.color, lightYellow,Time.deltaTime*20);
			}
			
		}
		else
		{
			Color yellow = new Color(1.0f,1.0f,0.0f,1.0f);
			
			if(!renderer.material.color.Equals(yellow))
			{
				renderer.material.color = Color.Lerp(renderer.material.color, yellow,Time.deltaTime*20);
			}
		}
		
		
		
		if(cam!=null ){
			
			if(selected){
				if(Input.GetMouseButton(1)){
					goalPosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y, 0));
					goalPosition.z = 0;
					if(_clickHoldLockCheck == false) {
						_clickHoldLockCheck = true;
						clickSound.Play ();
					}
				} else {
					_clickHoldLockCheck = false;
				}
			}
			
			//Moving bits
			if(!infecting)
			{
				
				//Move normally
				Vector3 difference = goalPosition-transform.position;
				
				if(difference.magnitude>0.1f){
					difference.Normalize();
					
					Vector3 newPos = transform.position;
					
					newPos+= difference*Time.deltaTime*speed;
					
					transform.position=newPos;
					
				}
			}
			
		
		}
		else{
			ScoreScript.addScore(1000, true, transform.localPosition);
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
		
		
		for (int i=0; i<totalBursts;++i) {
			if (burstTimes[i] != null && bursts[i] != null) {	
				burstTimes[i] += Time.deltaTime;
				if (burstTimes[i] >= 3.0f) {
					Destroy(bursts[i]);
					burstTimes[i] = 0.0f;
				}
			}
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
				if(explosionFab) {
					bursts[totalBursts] = Instantiate(explosionFab, transform.position, transform.rotation);
					burstTimes[totalBursts] = 0.0f;
					++totalBursts;
					if (totalBursts == 1000) totalBursts = 0;
				}
			}
			
			//James Added - added helpless condition
			if(other.tag=="Enemy" && !helpless){
				deathSound.Play();
				if(explosionFab) {
					bursts[totalBursts] = Instantiate(explosionFab, transform.position, transform.rotation);
					burstTimes[totalBursts] = 0.0f;
					++totalBursts;
					if (totalBursts == 1000) totalBursts = 0;
				}
				ScoreScript.addScore(1000, true, transform.localPosition);
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
