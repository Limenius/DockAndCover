using UnityEngine;
using System.Collections;


public class SpacecraftController : MonoBehaviour {

	public float positionAcceleratorCoefficient;
	public float rotationAcceleratorCoefficient;
	public float dockingPositionThreshold;
	public float dockingAccelerationThreshold;
	public float dockingRotationThreshold;

	public GameObject station;

	private Vector3 acceleration;
	private Vector3 speed;
	private Vector3 rotationAcceleration;
	private Vector3 rotationSpeed;

	public Texture2D crosshairImage;

	void Start()
	{
		this.acceleration = new Vector3 (0.0f, 0.0f, 0.0f);
		this.rotationAcceleration = new Vector3 (0.0f, 0.0f, 0.0f);

	}

	void OnGUI()
	{
		float xMin = (Screen.width / 2) - (crosshairImage.width / 2);
		float yMin = (Screen.height / 2) - (crosshairImage.height / 2);
		GUI.DrawTexture(new Rect(xMin, yMin, crosshairImage.width, crosshairImage.height), crosshairImage);
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
			Debug.Log ("docking");
		}
	}

	private bool checkDocking()
	{
		if (Mathf.Abs(rigidbody.position.x - this.station.rigidbody.position.x) > this.dockingPositionThreshold) {
			return false;
		}

		if (Mathf.Abs(rigidbody.position.y - this.station.rigidbody.position.y) > this.dockingPositionThreshold) {
			return false;
		}

		if (Mathf.Abs(rigidbody.position.z - this.station.rigidbody.position.z) > this.dockingPositionThreshold) {
			return false;
		}

		if (Mathf.Abs(rigidbody.velocity.x) > this.dockingAccelerationThreshold) {
			return false;
		}

		if (Mathf.Abs(rigidbody.velocity.y) > this.dockingAccelerationThreshold) {
			return false;
		}

		if (Mathf.Abs(rigidbody.velocity.z) > this.dockingAccelerationThreshold) {
			return false;
		}

		if (Mathf.Abs((rigidbody.rotation.x - 0.7071068f) - this.station.rigidbody.rotation.x) > this.dockingRotationThreshold) {
			Debug.Log("camera " + (rigidbody.rotation.x ));
			Debug.Log(this.station.rigidbody.rotation.x);
			return false;
		}

		if (Mathf.Abs(rigidbody.rotation.y - this.station.rigidbody.rotation.y) > this.dockingRotationThreshold) {
			return false;
		}

		if (Mathf.Abs(rigidbody.rotation.z - this.station.rigidbody.rotation.z) > this.dockingRotationThreshold) {
			return false;
		}


		return true;
	}

}
