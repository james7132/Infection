using UnityEngine;
using System.Collections;

public class BackgroundMusic : MonoBehaviour {
	
	public static BackgroundMusic instance;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void Awake()
	{
		if(instance != null && instance != this)
		{
				Destroy (gameObject);
		}
		else
		{
				instance = this;
		}
		DontDestroyOnLoad(gameObject);
	}
}
