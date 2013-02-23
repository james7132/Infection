using UnityEngine;
using System.Collections;

public class VirusCollisionDetection : MonoBehaviour 
{
	void OnCollisionEnter(Collision collision)
	{
		//if(collision.gameObject.layer == 9) //Collision with Red Blood Cells
		//{
			Debug.Log("Infected!");
		Debug.DrawRay(gameObject.transform.localPosition, collision.gameObject.transform.localPosition);
		//}
	}
}
