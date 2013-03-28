using UnityEngine;
using System.Collections;

public class HUDHandler : MonoBehaviour
{
	public GUIText HUD_time;
	public GUIText HUD_score;
	public GUIText HUD_multi;
	public GUIText HUD_count;

	private float startTime;
	private string elapsedTime;
	
	public ScoreScript scoreScript;
	
	private Color drkRed = new Color(.8f, .1f, .1f);
	private const int VLIMIT = 6;	//number of viruses to signal a warning

	void Start()
	{
		startTime = Time.time;
		//HUD_time.material.color = drkRed;
		//HUD_score.material.color = drkRed;
		//HUD_multi.material.color = drkRed;
	}

	void Update()
	{
		GameObject[] viruses = GameObject.FindGameObjectsWithTag("Player");
		
		HUD_score.text = Mathf.Round(scoreScript.score).ToString();
		float guiTime = Time.time - startTime;
		if (viruses.Length > 0)
		{	 
			int minutes = (int)(guiTime / 60);
			int seconds = (int)(guiTime % 60);
			int fraction = (int)((guiTime * 100) % 100);

			elapsedTime = string.Format ("{0:00}:{1:00}.{2:00}", minutes, seconds, fraction); 
			HUD_time.text = elapsedTime;
			
		}
		if (viruses.Length <= VLIMIT && (int)((guiTime * 100) % 100) >= 50)
			HUD_count.material.color = Color.red;
		else
			HUD_count.material.color = Color.white;
		HUD_count.text = viruses.Length.ToString();
	}
}
