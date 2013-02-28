using UnityEngine;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class VirusScript : MonoBehaviour
	{
		private float rotation;
		private const float rotationSpeed = 0.5f;
		
		private float movementSpeed = 0.5f;
		public float MovementSpeed
		{
			get { return movementSpeed; }
			set { movementSpeed = value; }
		}
		
		private bool selected = false;
		public bool Selected
		{
			get { return selected; }
			set { selected = value; }
		}
		
		private float infectionSpeed = 2;
		public float InfectionSpeed
		{
			get { return infectionSpeed; }
		}
		
		private uint reproductionLevel = 2;
		public uint ReproductionLevel
		{
			get { return reproductionLevel; } 
		}
		
		private Command currentCommand = null;
		public Command CurrentCommand
		{
			get { return currentCommand; }
			set { currentCommand = value; }
		}
		
		void Start()
		{
		}
		
		void Update()
		{
			if(selected)
			{
				gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
			}
			else
			{
				gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
			}
			
			if(currentCommand != null)
			{
				currentCommand.executeCommand(gameObject);
			}
			
			rotation += rotationSpeed * Time.deltaTime;
			if(rotation > 360)
			{
				rotation = 0;
			}
			Vector3 lookDir = new Vector3(Mathf.Cos(rotation), Mathf.Sin(rotation), 5f) - gameObject.transform.position; 
			lookDir.z = 0f;
			//gameObject.transform.localRotation = Quaternion.LookRotation(lookDir);
			
			if(gameObject.transform.localPosition.y < Global.DeathLimit)
			{
				Destroy (gameObject);
			}
		}
		
		void OnMouseUp()
		{
			Debug.Log("Clicked");
			VirusHandler.selectSingleVirus(gameObject);
		}
		
		void OnTriggerEnter(Collider other)
		{
			Debug.Log("Hello");
			if(other.gameObject.tag == "Red Blood Cell")
			{
				GameObject redBloodCell = other.gameObject;
				//redBloodCell.GetComponent<RedBloodCellScript>().Infect(gameObject);
				Destroy(gameObject);
			}
		}
	}
}
