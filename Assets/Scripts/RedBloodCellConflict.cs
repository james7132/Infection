using UnityEngine;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class RedBloodCellConflict : MonoBehaviour 
	{	
		private const float fallSpeed = 1f;
		public bool infected;
		
		// Use this for initialization
		void Start () 
		{

		}
		
		// Update is called once per frame
		void Update () 
		{
			gameObject.transform.localPosition -= new Vector3(0f, fallSpeed, 0f);
			if(gameObject.transform.localPosition.y < Global.deathLimit)
			{
				Destroy (gameObject);
			}
		}
	}
}
