using UnityEngine;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class RedBloodCellScript : MonoBehaviour 
	{	
		private const float fallSpeed = 0.75f;
		private bool infected = false;
		private float infectionTimer = 1000f;
		private float number = 0;
		private float infectionSpeed = 0f;
		
		//Random variable amounds
		private float x, y, z;
		//Speed to set
		public float speed;
		
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
		}
		
		public void Infect(GameObject virus)
		{
			infected = true;
			infectionSpeed += virus.GetComponent<VirusScript>().InfectionSpeed;
			number += virus.GetComponent<VirusScript>().ReproductionLevel;
		}
		
		// Update is called once per frame
		void Update () 
		{
			//Rotate randomly
			transform.Rotate(x*speed*Time.deltaTime,y*speed*Time.deltaTime,z*speed*Time.deltaTime);
			
			/**
			if(infected)
			{
				gameObject.renderer.material.color = Color.green;
				gameObject.GetComponentsInChildren<Renderer>().
				infectionTimer -= infectionSpeed;
				if(infectionTimer <= 0)
				{
					int distance;
					GameObject currentNewVirus;
					float direction = 0f;
					float expellingForce = Random.value * number * 10;
					Vector3 targetLocation = new Vector3();
					for(int i = 0; i < number; i++)
					{
						currentNewVirus = VirusHandler.spawnVirus(gameObject.transform.localPosition);
						direction = Random.value * 2 * Mathf.PI;
						targetLocation.x = Mathf.Cos(direction) * expellingForce + transform.localPosition.x;
						targetLocation.y = Mathf.Sin(direction) * expellingForce + transform.localPosition.y;
						targetLocation.z = gameObject.transform.localPosition.z;
						currentNewVirus.GetComponent<VirusScript>().CurrentCommand = new MovementCommand(targetLocation, expellingForce);
					}
					Destroy(gameObject);
				}
			}
			else
			{
				gameObject.renderer.material.color = Color.white;
			}
			*/
			gameObject.transform.localPosition -= new Vector3(0f, fallSpeed, 0f);
			if(gameObject.transform.localPosition.y < Global.DeathLimit)
			{
				Destroy (gameObject);
			}
		}
	}
}
