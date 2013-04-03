using UnityEngine;
using System.Collections;

public class GlobalSoundHandler : MonoBehaviour {
	
	private AudioSource clickSound;
	private bool _clickHoldLockCheck = false;
	
	// Use this for initialization
	void Start () {
		clickSound = (AudioSource)gameObject.AddComponent("AudioSource");
        clickSound.clip = (AudioClip)Resources.Load("selection");
		clickSound.volume = 0.35f;
		clickSound.rolloffMode = AudioRolloffMode.Custom;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Input.GetMouseButton(1)){
			if(_clickHoldLockCheck == false) {
				_clickHoldLockCheck = true;
				clickSound.Play ();
			}
		}
		else
		{
			_clickHoldLockCheck = false;
		}
	}
}
