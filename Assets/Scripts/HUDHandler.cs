using UnityEngine;
using System.Collections;

public class HUDHandler : MonoBehaviour
{
	public GUIText HUD_time;
	public GUIText HUD_score;
	public GUIText HUD_multi;
	public GUIText HUD_count;
	public GUITexture HUD_left;
	public GUITexture HUD_right;

	private const int W_CONSTRAINT_MIN = 800;
	private const int H_CONSTRAINT_MIN = 800;

	private const int W_CONSTRAINT_MAX = 1100;
	private const int H_CONSTRAINT_MAX = 900;

	private float startTime;
	private string elapsedTime;
	
	private Color drkRed = new Color(.8f, .1f, .1f);
	private const int VLIMIT = 6;	//number of viruses to signal a warning

	private int prevWidth;
	private int prevHeight;

	void Start()
	{
		startTime = Time.time;
		//HUD_time.material.color = drkRed;
		//HUD_score.material.color = drkRed;
		//HUD_multi.material.color = drkRed;

		prevWidth = Screen.width;
		prevHeight = Screen.height;
		resizeGUI();
	}

	private void resizeGUI()
	{
		int pi_w = 512;		//for main HUD Textures
		int pi_h = 512;

		int count_w = -85;	//virus count
		int count_h = 55;

		int multi_w = 140;	//multiplier
		int multi_h = 50;

		int score_w = 210;	//score
		int score_h = 85;

		//int time_w = 210;	//time
		//int time_h = 50;

		double w_ratio_min = Screen.width / (double)W_CONSTRAINT_MIN;
		double w_ratio_max = Screen.width / (double)W_CONSTRAINT_MAX;
		double h_ratio_min = Screen.height / (double)H_CONSTRAINT_MIN;
		double h_ratio_max = Screen.height / (double)H_CONSTRAINT_MAX;

		bool shrinkFont = false;

		if (Screen.width < W_CONSTRAINT_MIN)
		{
			pi_w = (int)(w_ratio_min * 512);
			count_w = (int)(w_ratio_min * -85);
			multi_w = (int)(w_ratio_min * 140);
			score_w = (int)(w_ratio_min * 210);
			shrinkFont = true;
		}
		else if (Screen.width > W_CONSTRAINT_MAX)
		{
			pi_w = (int)(w_ratio_max * 512);
			count_w = (int)(w_ratio_max * -85);
			multi_w = (int)(w_ratio_max * 140);
			score_w = (int)(w_ratio_max * 210);
		}
		if (Screen.height < H_CONSTRAINT_MIN)
		{
			pi_h = (int)(h_ratio_min * 512);
			count_h = (int)(h_ratio_min * 55);
			multi_h = (int)(h_ratio_min * 50);
			score_h = (int)(h_ratio_min * 85);
			shrinkFont = true;
		}
		else if (Screen.height < H_CONSTRAINT_MAX)
		{
			pi_h = (int)(h_ratio_max * 512);
			count_h = (int)(h_ratio_max * 55);
			multi_h = (int)(h_ratio_max * 50);
			score_h = (int)(h_ratio_max * 85);
		}
		
		double textRatio = 1;
		if (shrinkFont)
		{
			textRatio = h_ratio_min;
			if (w_ratio_min < h_ratio_min)
			{
				textRatio = w_ratio_min;	
			}
		}
		
		print(w_ratio_min + " " + w_ratio_max + " " + 
			  h_ratio_min + " " + h_ratio_max + " " +
			  textRatio);
		
		HUD_count.fontSize = (int)(44 * textRatio);
		HUD_multi.fontSize = (int)(64 * textRatio);
		HUD_score.fontSize = (int)(24 * textRatio);
		HUD_time.fontSize = (int)(25 * textRatio);

		Rect newInset = new Rect(0, 0, pi_w, pi_h);
		HUD_left.pixelInset = newInset;

		newInset = new Rect(-1*pi_w, 0, pi_w, pi_h);
		HUD_right.pixelInset = newInset;

		HUD_count.pixelOffset = new Vector2(count_w, count_h);
		HUD_multi.pixelOffset = new Vector2(multi_w, multi_h);
		HUD_score.pixelOffset = new Vector2(score_w, score_h);
		HUD_time.pixelOffset = new Vector2(score_w, multi_h);
	}

	void Update()
	{
		if (prevWidth != Screen.width || prevHeight != Screen.height)
		{
			resizeGUI();
			prevWidth = Screen.width;
			prevHeight = Screen.height;
		}			

		GameObject[] viruses = GameObject.FindGameObjectsWithTag("Player");
		
		HUD_score.text = Mathf.Round(ScoreScript.getScore()).ToString();
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
