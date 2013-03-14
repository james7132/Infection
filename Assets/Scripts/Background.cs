using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour 
{
	public Camera cam;
	
	public GameObject layer1;
	public GameObject layer2;
	public GameObject layer3;
	public GameObject layer4;
	
	private GameObject[] layers;
	
	public Color bgColor1;
	public Color bgColor2;
	public Color bgColor3;
	public Color bgColor4;
	
	private Color[] colors;
	
	public float speed1;
	public float speed2;
	public float speed3;
	public float speed4;
	
	private float[] speeds;
	
	float[] yOffset = {0, 0, 0, 0};
	Vector2[] uvOffset = {Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero};
 
	void Start()
	{
		layers = new GameObject[4];
		colors = new Color[4];
		speeds = new float[4];
		layers[0] = layer1;
		layers[1] = layer2;
		layers[2] = layer3;
		layers[3] = layer4;
		colors[0] = bgColor1;
		colors[1] = bgColor2;
		colors[2] = bgColor3;
		colors[3] = bgColor4;
		speeds[0] = speed1;
		speeds[1] = speed2;
		speeds[2] = speed3;
		speeds[3] = speed4;
		
	}
	
	void LateUpdate()
	{
		cam.backgroundColor = colors[0];
		for(int i = 0; i < 4; i++)
		{
			if(yOffset[i] >= 0x0FFFFF)
			{
				yOffset[i] = 0;
			}
			yOffset[i] += speeds[i] * Time.deltaTime;
			uvOffset[i].y = yOffset[i];
			layers[i].renderer.material.SetTextureOffset("_MainTex", uvOffset[i]);
			//layers[i].renderer.material.color = colors[i];
		}
	}
}
