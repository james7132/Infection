using UnityEngine;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class VirusScriptConflict : MonoBehaviour
	{
		private float rotation;
		private const float rotationSpeed = 0.5f;
		private const float movementSpeed = 5f;
		
		void Start()
		{
		}
		
		void Update()
		{
			if(tag == "Player")
			{				
				Transform location = gameObject.transform;
				if(Input.GetAxis("Horizontal") > 0)
				{
					if(location.localPosition.x - movementSpeed > Global.rightLimit)
					{
						location.localPosition -= new Vector3(movementSpeed, 0f, 0f);
					}
					else
					{
						location.localPosition = new Vector3(Global.rightLimit, location.localPosition.y, location.localPosition.z);
					}
				}
				if(Input.GetAxis("Horizontal") < 0)
				{
					if(location.localPosition.x + movementSpeed < Global.leftLimit)
					{
						location.localPosition += new Vector3(movementSpeed, 0f, 0f);
					}
					else
					{
						location.localPosition = new Vector3(Global.leftLimit, location.localPosition.y, location.localPosition.z);
					}
				}
				if(Input.GetAxis("Vertical") > 0)
				{
					if(location.localPosition.y + movementSpeed < Global.upperLimit)
					{
						location.localPosition += new Vector3(0f, movementSpeed, 0f);
					}
					else
					{
						location.localPosition = new Vector3(location.localPosition.x, Global.upperLimit, location.localPosition.z);
					}
				}
				if(Input.GetAxis("Vertical") < 0)
				{
					location.localPosition -= new Vector3(0f, movementSpeed, 0f);
				}
			}
			else
			{
				const float dampening = 0.5f;
				gameObject.transform.localPosition += dampening * 
					new Vector3(Random.value * 2 * movementSpeed - movementSpeed, Random.value * 2 * movementSpeed - movementSpeed, 0f);
			}
			
			rotation += rotationSpeed * Time.deltaTime;
			if(rotation > 360)
			{
				rotation = 0;
			}
			Vector3 lookDir = new Vector3(Mathf.Cos(rotation), Mathf.Sin(rotation), 5f) - gameObject.transform.position; 
			lookDir.z = 0f;
			//gameObject.transform.localRotation = Quaternion.LookRotation(lookDir);
			
			if(gameObject.transform.localPosition.y < Global.deathLimit)
			{
				Destroy (gameObject);
			}
		}
	}
}
