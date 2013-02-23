using UnityEngine;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class VirusHandler : MonoBehaviour 
	{	
		public GameObject prefab;
		public GUIStyle selectionStyle;
		
		private static GameObject virus;
		private static List<GameObject> selectedViruses;
		private Rect selectionRect;
		private Vector2 startLoc;
		private bool selecting;
		
		// Use this for initialization
		void Start () 
		{
			virus = prefab;
			selectionRect = new Rect(-5, -5, 0, 0);
			selectedViruses =  new List<GameObject>();
			selecting = false;
			spawnVirus(0f, 0f);
		}
		
		public static GameObject spawnVirus(Vector3 spawn)
		{
			return spawnVirus(spawn.x, spawn.y);
		}
		
		public static GameObject spawnVirus(float x, float y)
		{
			return (GameObject) Instantiate(virus, new Vector3(x, y, 5f), Quaternion.Euler(90f, 0f, 0f));
		}
		
		public static void selectSingleVirus(GameObject virus)
		{
			if(virus.tag != "Virus")
			{
				return;
			}
			else
			{
				selectedViruses.Clear();
				selectedViruses.Add(virus);
				GameObject[] viruses = GameObject.FindGameObjectsWithTag("Virus");
				foreach(GameObject virusCheck in viruses)
				{
					virusCheck.GetComponent<VirusScript>().Selected = ((virusCheck == virus) ? true : false);
				}
			}
		}
		
		// Update is called once per frame
		void Update () 
		{
			if(Input.GetMouseButtonDown(0)) //Left Button Pressed Down, start selection
			{
				startLoc = Input.mousePosition;
				startLoc.y = Screen.height - startLoc.y;
				selectionRect.xMin = startLoc.x;
				selectionRect.yMin = startLoc.y;
				selectionRect.xMax = startLoc.x; 
				selectionRect.yMax = startLoc.y;
				selectedViruses.Clear();
				GameObject[] viruses = GameObject.FindGameObjectsWithTag("Virus");
				foreach(GameObject v in viruses)
				{
					v.GetComponent<VirusScript>().Selected = false;
				}
				selecting = true;
			}
			else if(Input.GetMouseButton(0)) //Left Button held down, update GUI
			{
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
			else if(Input.GetMouseButtonUp(0)) // Left Button Released, update selection list
			{
				selecting = false;
				GameObject[] viruses = GameObject.FindGameObjectsWithTag("Virus");
				Vector3 screenPos;
				for(int i = 0; i < viruses.Length; i++)
				{
					screenPos = camera.WorldToScreenPoint(viruses[i].transform.localPosition);
					screenPos.y = Screen.height - screenPos.y;
					if(selectionRect.Contains(screenPos))
					{
						selectedViruses.Add(viruses[i]);
						viruses[i].GetComponent<VirusScript>().Selected = true;
					}
				}
				selectionRect.xMin = -5;
				selectionRect.yMin = -5;
				selectionRect.xMax = -5; 
				selectionRect.yMax = -5;
			}
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