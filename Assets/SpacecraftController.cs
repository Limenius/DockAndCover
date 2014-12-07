using UnityEngine;
using System.Collections;


public class SpacecraftController : MonoBehaviour {

	public float positionAcceleratorCoefficient;
	public float rotationAcceleratorCoefficient;
	public float dockingPositionThreshold;
	public float dockingVelocityThreshold;
	public float dockingRotationThreshold;

	public float hookSpeed;

	public GameObject hook1;
	public GameObject hook2;
	public GameObject hook3;

	public GameObject station;

	private Vector3 acceleration;
	private Vector3 speed;
	private Vector3 rotationAcceleration;
	private Vector3 rotationSpeed;

	public Texture2D crosshairImage;

	public GUIText distanceX;
	public GUIText distanceY;
	public GUIText accelerationY;
	public GUIText velocityY;

	public GameObject explosion;
	public GameObject docking;
	public GameObject pipipi;

	private bool isAlive;
	private bool isDocked;

	private GameObject pipipiObj;

	void Start()
	{
		this.acceleration = new Vector3 (0.0f, 0.0f, 0.0f);
		this.rotationAcceleration = new Vector3 (0.0f, 0.0f, 0.0f);
		this.isAlive = true;
		this.isDocked = false;

	}

	void OnGUI()
	{
		float xMin = (Screen.width / 2) - (crosshairImage.width / 2);
		float yMin = (Screen.height / 2) - (crosshairImage.height / 2);
		GUI.DrawTexture(new Rect(xMin, yMin, crosshairImage.width, crosshairImage.height), crosshairImage);
	}

	void Update()
	{
		this.distanceX.text = "Distance X " + Mathf.Abs (rigidbody.position.x - this.station.rigidbody.position.x);
		if (!this.checkPositionX ()) {
			this.distanceX.color = Color.red;
		} else {
			this.distanceX.color = Color.green;
		}
		this.distanceY.text = "Distance Y " + Mathf.Abs (rigidbody.position.y - this.station.rigidbody.position.y);
		if (!this.checkPositionY ()) {
			this.distanceY.color = Color.red;
		} else {
			this.distanceY.color = Color.green;
		}

		this.accelerationY.text = "Acceleration Y " + this.acceleration.y;

		this.velocityY.text = "Velocity Y " + this.rigidbody.velocity.y;
		if (!this.checkVelocityY ()) {
						this.velocityY.color = Color.red;
				} else {
			this.velocityY.color = Color.green;
				}

	}

	void FixedUpdate()
	{
		if (Input.GetKey ("up")) {
			this.acceleration = new Vector3(this.acceleration.x, this.acceleration.y, this.acceleration.z - this.positionAcceleratorCoefficient * Time.deltaTime);
		}
		
		if (Input.GetKey ("down")) {
			this.acceleration = new Vector3(this.acceleration.x, this.acceleration.y, this.acceleration.z + this.positionAcceleratorCoefficient * Time.deltaTime);
		}

		if (Input.GetKey ("right")) {
			this.acceleration = new Vector3(this.acceleration.x - this.positionAcceleratorCoefficient * Time.deltaTime, this.acceleration.y, this.acceleration.z);
		}
		
		if (Input.GetKey ("left")) {
			this.acceleration = new Vector3(this.acceleration.x + this.positionAcceleratorCoefficient * Time.deltaTime, this.acceleration.y, this.acceleration.z);
		}

		if (Input.GetKey ("o")) {
			this.acceleration = this.acceleration + new Vector3(0.0f, - this.positionAcceleratorCoefficient * Time.deltaTime, 0.0f);
		}
		
		if (Input.GetKey ("p")) {
			this.acceleration = this.acceleration + new Vector3(0.0f, + this.positionAcceleratorCoefficient * Time.deltaTime, 0.0f);
		}

		rigidbody.velocity += this.acceleration;

		if (Input.GetKey ("a")) {
			this.rotationAcceleration = this.rotationAcceleration + new Vector3(0.0f, 0.0f, - this.rotationAcceleratorCoefficient * Time.deltaTime);
		}
		
		if (Input.GetKey ("s")) {
			this.rotationAcceleration = this.rotationAcceleration + new Vector3(0.0f, 0.0f, + this.rotationAcceleratorCoefficient * Time.deltaTime);
		}

		if (Input.GetKey ("q")) {
			this.rotationAcceleration = this.rotationAcceleration + new Vector3(0.0f, - this.rotationAcceleratorCoefficient * Time.deltaTime, 0.0f);
		}
		
		if (Input.GetKey ("w")) {
			this.rotationAcceleration = this.rotationAcceleration + new Vector3(0.0f, + this.rotationAcceleratorCoefficient * Time.deltaTime, 0.0f);
		}

		if (Input.GetKey ("z")) {
			this.rotationAcceleration = this.rotationAcceleration + new Vector3(- this.rotationAcceleratorCoefficient * Time.deltaTime, 0.0f, 0.0f);
		}
		
		if (Input.GetKey ("x")) {
			this.rotationAcceleration = this.rotationAcceleration + new Vector3(+ this.rotationAcceleratorCoefficient * Time.deltaTime, 0.0f, 0.0f);
		}


		rigidbody.angularVelocity += this.rotationAcceleration;


		if (this.checkDocking ()) {
			this.dock();
		}
	}

	private void dock() {
		if (!isDocked) {
			this.acceleration = new Vector3(0.0f, 0.0f, 0.0f);
			this.rigidbody.velocity = new Vector3 (0.0f, 0.0f, 0.0f);
			this.rigidbody.angularVelocity = new Vector3 (0.0f, 0.0f, 0.0f);
			StartCoroutine (hookAnimation (hook1.transform, Quaternion.Euler (90.0f, 0.0f, 0.0f), this.hookSpeed, 0.1f));
			StartCoroutine (hookAnimation (hook2.transform, Quaternion.Euler (90.0f, 0.0f, 120.0f), this.hookSpeed, 0.1f));
			StartCoroutine (hookAnimation (hook3.transform, Quaternion.Euler (90.0f, 0.0f, 240.0f), this.hookSpeed, 0.1f));
			pipipiObj = (GameObject)Instantiate (pipipi, transform.position, transform.rotation);
			pipipiObj.transform.parent = transform;
			isDocked = true;
		}
		

	}

	IEnumerator hookAnimation(Transform trans, Quaternion destRot, float speed, float threshold )
	{
		float angleDist = Quaternion.Angle(trans.rotation, destRot);
		
		while (angleDist > threshold)
		{
			trans.rotation = Quaternion.RotateTowards(trans.rotation, destRot, Time.deltaTime * speed);
			yield return null;
			
			angleDist = Quaternion.Angle(trans.rotation, destRot);
		}
		Destroy (pipipiObj);
		GameObject dockingObj = (GameObject)Instantiate (docking, transform.position, transform.rotation);
		dockingObj.transform.parent = transform;
	}

	private bool checkPositionX()
	{
		if (Mathf.Abs(rigidbody.position.x - this.station.rigidbody.position.x) > this.dockingPositionThreshold) {
			return false;
		}
		return true;
	}

	private bool checkPositionY()
	{
		if (Mathf.Abs(rigidbody.position.y - this.station.rigidbody.position.y) > this.dockingPositionThreshold) {
			return false;
		}
		return true;
	}

	private bool checkVelocityY()
	{
		if (Mathf.Abs(rigidbody.velocity.y) > this.dockingVelocityThreshold) {
			return false;
		}
		return true;
	}

	private bool checkDocking()
	{
	
		if (!this.checkPositionX ()) {
			return false;
		}

		if (!this.checkPositionY ()) {
			return false;
		}
//
//		if (Mathf.Abs(rigidbody.position.z - this.station.rigidbody.position.z) > this.dockingPositionThreshold) {
//			return false;
//		}
//
//		if (Mathf.Abs(rigidbody.velocity.x) > this.dockingVelocityThreshold) {
//			return false;
//		}

		if (!this.checkVelocityY ()) {
			return false;
		}

//		if (Mathf.Abs(rigidbody.velocity.z) > this.dockingVelocityThreshold) {
//			return false;
//		}
//
//		if (Mathf.Abs((rigidbody.rotation.x - 0.7071068f) - this.station.rigidbody.rotation.x) > this.dockingRotationThreshold) {
//			Debug.Log("camera " + (rigidbody.rotation.x ));
//			Debug.Log(this.station.rigidbody.rotation.x);
//			return false;
//		}
//
//		if (Mathf.Abs(rigidbody.rotation.y - this.station.rigidbody.rotation.y) > this.dockingRotationThreshold) {
//			return false;
//		}
//
//		if (Mathf.Abs(rigidbody.rotation.z - this.station.rigidbody.rotation.z) > this.dockingRotationThreshold) {
//			return false;
//		}


		return true;


	}

	void OnTriggerEnter(Collider other) {

		Debug.Log ("MUERTE");
		if (isAlive) {
			GameObject explosionObj = (GameObject) Instantiate (explosion, transform.position, transform.rotation);
			explosionObj.transform.parent = transform;

			isAlive = false;
		}
	}

}
