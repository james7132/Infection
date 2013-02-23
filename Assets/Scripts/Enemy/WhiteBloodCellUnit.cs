using UnityEngine;
using System.Collections;

//To be used in a WhiteBloodCellController
public class WhiteBloodCellUnit : MonoBehaviour {
	//This can be modified to increase difficulty
	public float speed = 25.0f;
	//For Shooting, should be a prefab
	public GameObject projectile;
	public float fireTimer;
	public float fireTimerMax=0.5f;
	//For info storing
	public Vector3 goalPosition;
	public float xMax = 440;
	public float xMin = -440;
	public float yMax = 350;
	public float yMin = -350;
	
	GameObject[] whiteBloodCells;
				
	private AudioSource boomSound;
	
	void Start() {
		boomSound = (AudioSource)gameObject.AddComponent("AudioSource");
        boomSound.clip = (AudioClip)Resources.Load("damage");
		boomSound.volume = 0.5f;
		boomSound.rolloffMode = AudioRolloffMode.Custom;
	}
	
	void OnTriggerStay(Collider other) {
		if(other.gameObject.tag == "Enemy")
		{
			Vector3 direction = other.transform.position - transform.position;
			transform.position -= direction.normalized * Time.deltaTime * 50000 / (1+Mathf.Pow(direction.magnitude,7/3));
		}
	}
	
	private Vector3 getTangentialMovement() {
		Vector3 difference = goalPosition-transform.position;
		Vector3 tangent = Vector3.Cross (difference.normalized, transform.up);
		tangent = Vector3.Cross (tangent, difference.normalized);
		tangent = new Vector3(tangent.x, tangent.y, 0);
		Vector3 old = transform.position;
		//Debug.DrawLine (old, old + 30 * tangent * Time.deltaTime * difference.magnitude/3);
		return tangent * Time.deltaTime * difference.magnitude/100;
	}
	
	private Vector3 getGoalMovement() {
		Vector3 difference = goalPosition-transform.position;
		
		difference.Normalize();
		difference*=Time.deltaTime*Mathf.Pow(difference.magnitude,5/3);
		return difference;
	}
	
	/// <summary>
	/// Moves the white blood towards the point at their given speed
	/// </summary>
	/// <returns>
	/// Whether it's at the spot or not
	/// </returns>
	/// <param name='point'>
	/// The point the white blood cell is moving towards
	/// </param>
	public bool MoveTowards(Vector3 point) {
		goalPosition = point;
		getWhiteBloodCells();
		
		// tangential movement prevents clumped viruses from looking too stiff
		transform.position += speed*(0.9f*getGoalMovement() + 0.1f*getTangentialMovement());
		// prevent movement on the z axis
		transform.position = new Vector3(transform.position.x, transform.position.y, 0);
		
		return (transform.position - goalPosition).magnitude < 0.1f;
	}
	
	void Update(){
		
		if(fireTimer>0){
			fireTimer-=Time.deltaTime;
		}
	}
	
	void getWhiteBloodCells(){
		whiteBloodCells=GameObject.FindGameObjectsWithTag("Enemy");
	}
	
	
	//See if we're in the bounds
	private bool inBounds(Vector3 pos){
		if(pos.x>xMin && pos.x<xMax && pos.y>yMin && pos.y<yMax){
			return true;
		}
		else{
			return false;
		}
	}
	
	/// <summary>
	/// Fire a projectile at a given speed at a point
	/// </summary>
	/// <param name='point'>
	/// The point we're firing at
	/// </param>
	/// <param name='speedOfProjectile'>
	/// Speed of projectile.
	/// </param>
	public void Fire(Vector3 point, float speedOfProjectile){
		
		if(fireTimer<=0){
			boomSound.Play();
			//Instantiate the projectile
			GameObject projectileObj = Instantiate(projectile,transform.position,transform.rotation) as GameObject;
			WhiteBloodCellProjectile cellProj = projectileObj.GetComponent<WhiteBloodCellProjectile>();
			
			//Set projectile's direction and speed
			cellProj.speed=speedOfProjectile;
			Vector3 difference = point-transform.position;
			difference.Normalize();
			
			cellProj.direction=difference;
			
			fireTimer=fireTimerMax;
			
		}
		
	}
	
}
