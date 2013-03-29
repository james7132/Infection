using UnityEngine;
using System.Collections.Generic;

public class ScoreScript : MonoBehaviour 
{
	private static ScoreScript instance;
	public double virusCountToScoreRatio = 100;
	private List<ScorePopup> popups;
	private List<ScorePopup> toRemove;
	protected float popupMaxTime = 0.05f;
	public float popupUpFloatSpeed = 2;
	public Color basePopupColor = Color.white;
	public float fadeOutSpeed = 0.5f;
	public float score = 0;
	
	// Use this for initialization
	void Start () 
	{
		instance = this;
		//popupMaxTime = 0.25f / (float)(fadeOutSpeed);
		popups = new List<ScorePopup>();
		toRemove = new List<ScorePopup>();
		score = 0;
	}
	
	public static void addScore(long amount, bool show, Vector3 scoreLocation)
	{
		instance.score += amount;
		if(show)
		{
			instance.popups.Add(new ScorePopup(amount, scoreLocation));
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		//GameObject[] viruses = GameObject.FindGameObjectsWithTag("Player");
		int totalVirusCount = Global.getTotalVirusCount();
		score += Time.deltaTime * (float)virusCountToScoreRatio * (Mathf.Log10(Mathf.Pow(totalVirusCount, 3)) + 1);
		
		ScorePopup[] scores = popups.ToArray();
		
		
		//You can't update a List while iterating through it, have to use an array
		for(int i = 0; i < scores.Length; i++)
		{
			scores[i].Update();
			
			if(scores[i].Expired)
			{
				
				toRemove.Add(scores[i]);
			}
		}
		
		popups = new List<ScorePopup>(scores);
		
		for(int i = 0; i < toRemove.Count; i++)
		{
			//print("ever removing a popup?");
			popups.Remove(toRemove[i]);
		}
		toRemove.Clear();
	}
	
	void OnGUI ()
	{	
		//print("Count: "+popups.Count);
		for(int i = 0; i < popups.Count; i++)
		{
			GUI.color = popups[i].Color;
			//Debug.Log(GUI.color.a);
			Rect rect = GUILayoutUtility.GetRect(new GUIContent(popups.ToString()), "label");
			rect.x = popups[i].ScreenLocation.x;
			rect.y = Screen.height - popups[i].ScreenLocation.y;
			GUI.Label(rect, popups[i].ToString());
		}
	}
	
	public static float getScore() {
		return instance.score;
	}
	
	private struct ScorePopup
	{
		private long amount;
		long Amount
		{
			get { return amount; }
		}
		
		private Vector2 screenLoc;
		public Vector2 ScreenLocation
		{
			get { return screenLoc; }
		}
		
		public float timeElapsed;
		public bool Expired
		{
			get { return timeElapsed > ScoreScript.instance.popupMaxTime; }
		}
		
		private Color color;
		public Color Color
		{
			get { return color; }
		}
		
		public ScorePopup(long amt, Vector3 worldLoc)
		{
			GameObject camObj = GameObject.FindGameObjectWithTag("MainCamera") as GameObject;
			Camera cam = camObj.GetComponent<Camera>();
			screenLoc = cam.WorldToScreenPoint(worldLoc);
			amount = amt;
			timeElapsed = 0;
			color = ScoreScript.instance.basePopupColor;
			color.a = 1;
		}	
		
		public void Update()
		{
			float deltaTime = Time.deltaTime;
			
			
			//timeElapsed += deltaTime;
			//print("Time elapsed: "+timeElapsed);
			
			screenLoc.y += ((float)ScoreScript.instance.popupUpFloatSpeed) * Screen.height * (float)deltaTime;
			float oldA = color.a;
			color.a -= ((float)ScoreScript.instance.fadeOutSpeed) * (float)deltaTime;
			//Debug.Log(oldA + " -> " + color.a + ", " + ScoreScript.instance.popupMaxTime + ", " + timeElapsed);
		}
		
		public string ToString()
		{
			return "+" + amount;
		}
	}
}
