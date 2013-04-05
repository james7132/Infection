using UnityEngine;
using System.Collections.Generic;

	public class RedBloodCellScript : MonoBehaviour 
	{	
		private const float fallSpeed = 50f;
		private bool infected = false;
		//It takes half a second to infect a red blood cell
		public float infectionTimer = 0.5f;
		private float infectionTimerMax;
		//Number is the p
		private int nextIndex = 0;
		private float infectionSpeed = 0f;
		
		//Collection of viruses (max you can have is 10)
		public LegitVirusScript[] viruses;
		
		//The body of this RedBloodCellScript
		public GameObject body;
		
		public float maxScaleFlucutation = 0.5f;
	
		//Random variable amounds
		private float x, y, z;
		//Speed to set
		public float speed;
		
		private Object[] bursts;
		private float[] burstTimes;
		private int totalBursts;
		public GameObject explosionFab;
	
		
	
		public bool Infected
		{
			get { return infected; }
		}
		
		// Use this for initialization
		void Start () 
		{
			//Set random bits
			x = Random.Range(-1f,1f);
			y = Random.Range(-1f,1f);
			z = Random.Range(-1f,1f);
			
			bursts = new Object[1000];
			burstTimes = new float[1000];
			totalBursts = 0;
			viruses = new LegitVirusScript[10];
			
			
			transform.localScale = new Vector3(
				transform.localScale.x * (1 - maxScaleFlucutation + 2 * Random.value * maxScaleFlucutation),
				transform.localScale.y * (1 - maxScaleFlucutation + 2 * Random.value * maxScaleFlucutation),
				transform.localScale.z * (1 - maxScaleFlucutation + 2 * Random.value * maxScaleFlucutation)
			);
			
		
			infectionTimerMax=infectionTimer;
		}
		
		public void Infect(LegitVirusScript virus, float _infectionSpeed)
		{
			nextIndex = fixVirusArray();
			
			
			if(nextIndex<viruses.Length-1 && !weHaveThatVirus(virus.gameObject)){
				infected = true;
				infectionSpeed +=_infectionSpeed;
				
				
				//Next Index is the next index to place a virus at
				viruses[nextIndex] = virus;
					
				
			}
		}
		
		// Update is called once per frame
		void Update () 
		{
			
			
			
			nextIndex = fixVirusArray();
			
			//Cheap way to avoid giant red blood cell
			if (transform.localScale.magnitude > 20){
				if(viruses[0]!=null){
					Instantiate(viruses[0].gameObject,viruses[0].transform.position,viruses[0].transform.rotation);
					if(explosionFab) {
						bursts[totalBursts] = Instantiate(explosionFab, transform.position, transform.rotation);
						burstTimes[totalBursts] = 0.0f;
						++totalBursts;
						if (totalBursts == 1000) totalBursts = 0;
					}
					
				}
				Destroy(gameObject);
			}
		
		
			//If there's no viruses, we're no longer infected
			if(nextIndex==0)
			{
				infected=false;
				
				//Allows for healing
				if(infectionTimer<infectionTimerMax){
					infectionTimer+=Time.deltaTime;
				}
				
			}
			
			if(infected)
			{
				
				if(infectionTimer <= 0)
				{
					
					foreach(LegitVirusScript virus in viruses)
					{
						if(virus!=null){
							virus.YouCanStopInfectingNow();
						}
						
					}
				
					//Make the new virus
					if(viruses[0]!=null){
						Instantiate(viruses[0].gameObject,viruses[0].transform.position,viruses[0].transform.rotation);
						if(explosionFab) {
							bursts[totalBursts] = Instantiate(explosionFab, transform.position, transform.rotation);
							burstTimes[totalBursts] = 0.0f;
							++totalBursts;
							if (totalBursts == 1000) totalBursts = 0;
						}
						Destroy(gameObject);
					}	
					
					
				
					
					
					
				}
				else
				{
					//Decrement Timer
					infectionTimer-=infectionSpeed*Time.deltaTime;
					
				}
				
				
				//Color, should be yellow if it's infected
				Color yellow = new Color(1.0f,1.0f,0.0f,1.0f);
				
				if(body.renderer.material.color.g!=1.0f){
					body.renderer.material.color = Color.Lerp(body.renderer.material.color,yellow,Time.deltaTime*5);
					gameObject.transform.localScale += Vector3.one * 5 * Time.deltaTime;
				}
				
			}
			else
			{
				//Rotate randomly
				transform.Rotate(x*speed*Time.deltaTime,y*speed*Time.deltaTime,z*speed*Time.deltaTime);
				
				gameObject.transform.localPosition -= new Vector3(0f, fallSpeed*Time.deltaTime, 0f);
				
				
				//Color, should be red if it's not infected
				Color red = new Color(1.0f,0.0f,0.0f,1.0f);
				
				if(body.renderer.material.color.g!=0.0f){
					body.renderer.material.color = Color.Lerp(body.renderer.material.color,red,Time.deltaTime*5);
				}
			}
			
			
			
			
			if(gameObject.transform.localPosition.y < Global.DeathLimit)
			{
				Destroy (gameObject);
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
		
		//Fixes up the array, returns number of viruses currently attached to red blood cell
		public int fixVirusArray(){
			int numOfViruses = 0;
			int lastNonNullIndex=0;
			
			
			for(int i =0; i<viruses.Length; i++)
			{
				if(viruses[i]!=null)
				{
					numOfViruses++;
					lastNonNullIndex=i;
				}
			}
			
			if(numOfViruses==nextIndex)
			{
				//We're done! We have as many viruses as expected
				return numOfViruses;
			}
			else if(numOfViruses!=0)
			{
				//We have less viruses than we're expecting, we should shift everyone down
				for(int i =0; i<viruses.Length; i++)
				{
					if(i<=lastNonNullIndex)
					{
						if(viruses[i]==null)
						{
							//Go from the back to the front
							for(int j=viruses.Length-1; j>i; j--)
							{
								if(viruses[j]!=null)
								{
									viruses[i] = viruses[j];
									viruses[j]=null;
									
								}
								
							}
							
							//If viruses i is still null, we've done all we can
							if(viruses[i]==null)
							{
								return numOfViruses;
							}
							
						}
						
						
						
					}
					
					
				}
				
				return numOfViruses;
				
				
			}
			
			return 0;
			
		}
		
	
		//Check to see if we already have that virus
		bool weHaveThatVirus(GameObject virus){
			bool match=false;
		
			foreach(LegitVirusScript vira in viruses){
				if(vira!=null && virus==vira.gameObject){
					match=true;
				}
			}
		
			return match;
		
		}
		
		
	}

