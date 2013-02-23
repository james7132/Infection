using UnityEngine;
using System.Collections;

public class WhiteBloodCellControllerTest : WhiteBloodCellController {
	public WhiteBloodCellUnit unit;
	public GameObject virus;
	
	// Use this for initialization
	void Start () {
		virus = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if(virus!=null)
		{
			
			unit.MoveTowards(virus.transform.position);
			
			unit.Fire(virus.transform.position,400.0f);
		}
		else
		{
			virus = GameObject.FindGameObjectWithTag("Player");
			
		}
	}
}
