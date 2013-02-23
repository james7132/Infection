using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour 
{
	float speed = 0.05f;
	float yOffset = 0;
	Vector2 uvOffset = Vector2.zero;
 
	void LateUpdate()
	{
		if(yOffset >= 0x0FFFFF)
		{
			yOffset = 0;
		}
		yOffset += speed * Time.deltaTime;
		uvOffset.y = yOffset;
		renderer.material.SetTextureOffset("_MainTex", uvOffset);
	}
}
