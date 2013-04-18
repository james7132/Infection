using UnityEngine;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class VirusHandler : MonoBehaviour 
	{	
		public GUIStyle selectionStyle;
		
		private static GameObject virus;
		private Rect selectionRect;
		private Vector2 startLoc;
		private bool selecting;
		
		//Timer for determining if you just clicked or if you're holding that sucker in a tender mouse embrace
		private float clickedTimer;
		
		public Camera cam;
		// Use this for initialization
		
		//Size of box that shows up just on click
		public float sizeOfBox = 100.0f;
		
		
		void Start () 
		{
			selectionRect = new Rect(-5, -5, 0, 0);
			
		}
		
		// Update is called once per frame
		void Update () 
		{	
			if (Global.GameState == GameState.IN_GAME) {
				if(Input.GetMouseButtonDown(0)) //Left Button Pressed Down, start selection
				{
					print("Button down");
					startLoc = Input.mousePosition;
					startLoc.y = Screen.height - startLoc.y;
					selectionRect.xMin = startLoc.x-(sizeOfBox/2);
					selectionRect.yMin = startLoc.y-(sizeOfBox/2);
					selectionRect.xMax = startLoc.x+(sizeOfBox/2); 
					selectionRect.yMax = startLoc.y+(sizeOfBox/2);
					selecting = true;
					
					GameObject[] viruses = GameObject.FindGameObjectsWithTag("Player");
					Vector3 screenPos;
					for(int i = 0; i < viruses.Length; i++)
					{
						screenPos = cam.WorldToScreenPoint(viruses[i].transform.localPosition );
						
						
						screenPos.y = Screen.height - screenPos.y;
						
						print("Screen pos: "+screenPos);
						print("Rect: "+selectionRect);
						
						if(selectionRect.Contains(screenPos))
						{
							
							print("Contained");
							viruses[i].GetComponent<LegitVirusScript>().selected=true;
						}
						else
						{
							viruses[i].GetComponent<LegitVirusScript>().selected=false;
						}
					}
				}
				
				else if(Input.GetMouseButton(0)) //Left Button held down, update GUI
				{
					
					if(clickedTimer>0.2f){
					
						print("Button");
						Vector2 mousePos = Input.mousePosition;
						mousePos.y = Screen.height - mousePos.y;
						if(mousePos.x > startLoc.x)
						{
							selectionRect.xMin = startLoc.x;
							selectionRect.xMax = mousePos.x;
						}
						else
						{
							selectionRect.xMin = mousePos.x;
							selectionRect.xMax = startLoc.x;
						}
						if(mousePos.y > startLoc.y)
						{
							selectionRect.yMin = startLoc.y;
							selectionRect.yMax = mousePos.y;
						}
						else
						{
							selectionRect.yMin = mousePos.y;
							selectionRect.yMax = startLoc.y;
						}
					}
					else{
						clickedTimer+=Time.deltaTime;
					}
				}
				else if(Input.GetMouseButtonUp(0)) // Left Button Released, update selection list
				{
					print("Button up");
					selecting = false;
					GameObject[] viruses = GameObject.FindGameObjectsWithTag("Player");
					Vector3 screenPos;
					for(int i = 0; i < viruses.Length; i++)
					{
						screenPos = cam.WorldToScreenPoint(viruses[i].transform.localPosition);
						screenPos.y = Screen.height - screenPos.y;
						if(selectionRect.Contains(screenPos))
						{
							viruses[i].GetComponent<LegitVirusScript>().selected=true;
						}
						else
						{
							viruses[i].GetComponent<LegitVirusScript>().selected=false;
						}
					}
					
					
					selectionRect.xMin = -5;
					selectionRect.yMin = -5;
					selectionRect.xMax = -5; 
					selectionRect.yMax = -5;
					
					clickedTimer=0.0f;
				}
				
			}
			/**
			if(Input.GetMouseButtonDown(1))
			{
				//Contexually decide what to do with the selected viruses
				if(selectedViruses.Count != 0)
				{
					Vector3 mousePos = camera.ViewportToWorldPoint(camera.ScreenToViewportPoint(Input.mousePosition));
					mousePos.z = 5f;
					foreach(GameObject virus in selectedViruses)
					{
						virus.GetComponent<VirusScript>().CurrentCommand = new MovementCommand(mousePos, virus.GetComponent<VirusScript>().MovementSpeed);
					}
				}
			}
			*/
		}
		
		
		
		
		void OnGUI()
		{
			if(selecting && Mathf.Abs(selectionRect.xMin - selectionRect.xMax) > 5 && Mathf.Abs(selectionRect.yMin - selectionRect.yMax) > 5)
			{
				GUI.Box (selectionRect, "");
			}
		}
	}
}