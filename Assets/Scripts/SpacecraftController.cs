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

	private Quaternion originalHook1Rot;
	private Quaternion originalHook2Rot;
	private Quaternion originalHook3Rot;

	public GameObject station;

	private Vector3 acceleration;
	private Vector3 speed;
	private Vector3 rotationAcceleration;
	private Vector3 rotationSpeed;

	public Texture2D crosshairImage;

	public GUIText distanceX;
	public GUIText distanceY;
	public GUIText distanceZ;
	public GUIText accelerationY;
	public GUIText velocity;
	public GUIText rotationY;

	public GameObject explosion;
	public GameObject docking;
	public GameObject pipipi;

	private bool isDocked;

	private GameObject pipipiObj;

	private bool restart;
	private bool gameOver;
	private int level;
	private bool playing;
	private bool paused;

	public float levelWait;
	public float gameOverWait;
	public float dockedWait;

	public GUIText centerGUI;
	public GUIText additionalCenterGUI;
	public GUIText timerGUI;

	private bool levelTimed;
	private float levelTime;
	private float startLevelTime;


	void Start()
	{
		this.acceleration = new Vector3 (0.0f, 0.0f, 0.0f);
		this.rotationAcceleration = new Vector3 (0.0f, 0.0f, 0.0f);
		this.isDocked = false;

		this.restart = false;
		this.gameOver = false;
		this.level = 5;
		this.playing = false;
		this.paused = true;

		this.originalHook1Rot = this.hook1.transform.rotation;
		this.originalHook2Rot = this.hook2.transform.rotation;
		this.originalHook3Rot = this.hook3.transform.rotation;

		this.timerGUI.text = "";

	}



	void OnGUI()
	{
		//float xMin = (Screen.width / 2) - (crosshairImage.width / 2);
		//float yMin = (Screen.height / 2) - (crosshairImage.height / 2);
		//GUI.DrawTexture(new Rect(xMin, yMin, crosshairImage.width, crosshairImage.height), crosshairImage);
	}

	void Update()
	{
		if (this.playing) {
			this.distanceX.text = "Distance X " + rigidbody.position.x.ToString("n3") ;
			if (!this.checkPositionX ()) {
					this.distanceX.color = Color.red;
			} else {
					this.distanceX.color = Color.green;
			}
			this.distanceY.text = "Distance Y " + rigidbody.position.y.ToString("n3") ;
			if (!this.checkPositionY ()) {
					this.distanceY.color = Color.red;
			} else {
					this.distanceY.color = Color.green;
			}
			this.distanceZ.text = "Distance Z " + rigidbody.position.z.ToString("n3") ;
			if (!this.checkPositionZ ()) {
					this.distanceZ.color = Color.red;
			} else {
					this.distanceZ.color = Color.green;
			}


			this.accelerationY.text = "Acceleration Y " + this.acceleration.y.ToString("n3");

			this.velocity.text = "Velocity " + this.rigidbody.velocity.magnitude.ToString("n3");
			if (!this.checkVelocity ()) {
					this.velocity.color = Color.red;
			} else {
					this.velocity.color = Color.green;
			}
            
			this.rotationY.text = "Rotation " + this.rigidbody.rotation.eulerAngles.y.ToString("n3");
			if (!this.checkRotationY ()) {
					this.rotationY.color = Color.red;
			} else {
					this.rotationY.color = Color.green;
			}
		} else if (!this.gameOver) {
			StartCoroutine(DisplayLevel());
		} else if (this.restart) {

			if (Input.GetKeyDown (KeyCode.R)) 
			{

				StartCoroutine(DisplayLevel());

			}

		}

	
	}
	
	IEnumerator DisplayLevel() {
		this.playing = true;
		this.paused = true;
		PreLevel ();
		centerGUI.text = "Level " + level;	
		yield return new WaitForSeconds(levelWait);
		centerGUI.text = "";

		StartLevel ();

	}

	private void setStationRendererStatus (bool enabled)
	{
		Renderer[] lChildRenderers=this.station.GetComponentsInChildren<Renderer>();
		foreach ( Renderer lRenderer in lChildRenderers)
		{
			lRenderer.enabled=enabled;
		}
	}

	private void PreLevel()
	{
		setStationRendererStatus (true);
        isDocked = false;
		ResetHooks ();
		this.timerGUI.text = "";
		switch (this.level) {
		case 1:
			this.levelTimed = false;
			this.additionalCenterGUI.text = "Press O and P to approach\nand retreat from the station";
			this.transform.position = new Vector3(0.0f, 3.0f, 0.0f);
			this.rigidbody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
			this.transform.rotation = Quaternion.Euler (90.0f, 0.0f, 0.0f);
			this.rigidbody.angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
			this.levelTime = 10.0f;
			break;
		case 2:
			this.levelTimed = false;
			this.additionalCenterGUI.text = "Watch out your docking speed";

			this.transform.position = new Vector3(0.0f, 6.0f, 0.0f);
			this.rigidbody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
			this.transform.rotation = Quaternion.Euler (90.0f, 0.0f, 0.0f);
			this.rigidbody.angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
			this.levelTimed = false;

			this.levelTime = 100.0f;
			break;
		case 3:
			this.levelTimed = false;
			this.additionalCenterGUI.text = "Use arrows to move";
			
			this.transform.position = new Vector3(0.0f, 6.0f, 0.0f);
			this.rigidbody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
			this.transform.rotation = Quaternion.Euler (90.0f, 0.0f, 0.0f);
			this.rigidbody.angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
			this.levelTimed = false;
			
			this.levelTime = 100.0f;
			break;
		case 4:
			this.levelTimed = false;
			this.additionalCenterGUI.text = "Press Z and X to rotate";
			this.transform.position = new Vector3(0.0f, 3.0f, 0.0f);
			this.rigidbody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
			this.transform.rotation = Quaternion.Euler (90.0f, 0.0f, 0.0f);
			this.rigidbody.angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
			break;
		case 5:
			this.levelTimed = false;
			this.additionalCenterGUI.text = "You know you can rotate in two more directions, right? A/D/W/S for those";
			this.transform.position = new Vector3(0.0f, 3.0f, 0.0f);
			this.rigidbody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
			this.transform.rotation = Quaternion.Euler (70.0f, 0.0f, 0.0f);
			this.rigidbody.angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
			break;
		}
		if (this.levelTimed) {
			this.timerGUI.text = this.levelTime.ToString ("n1");
		}
	}

	private void StartLevel()
	{
		paused = false;
		this.additionalCenterGUI.text = "";
		switch (this.level) {
		case 1:
			this.rigidbody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
			this.rigidbody.angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
			break;
		case 2:
			this.rigidbody.velocity = new Vector3(0.0f, -2.0f, 0.0f);
			this.rigidbody.angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
			break;
		case 3:
			this.rigidbody.velocity = new Vector3(1.0f, 0.0f, 0.0f);
			this.rigidbody.angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
			break;
		case 4:
			this.rigidbody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
			this.rigidbody.angularVelocity = new Vector3(0.0f, 0.3f, 0.0f);
			break;
		case 5:
			this.rigidbody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
			this.rigidbody.angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
			break;
		}
		this.startLevelTime = Time.time;
	}
	
	void FixedUpdate()
	{
		if (this.playing && !this.isDocked && !this.paused) {
			if (this.levelTimed) {
				float remaining = (this.levelTime - (Time.time - this.startLevelTime));
				if (remaining <= 0.0f) {
					remaining = 0.0f;
					StartCoroutine (GameOver("Time is out.\nYou die."));

					
				}
				this.timerGUI.text = remaining.ToString("n1");
			}

			this.acceleration = new Vector3 (0.0f, 0.0f, 0.0f);
			this.rotationAcceleration = new Vector3 (0.0f, 0.0f, 0.0f);

			if (Input.GetKey ("up")) {

					this.acceleration = new Vector3 (this.acceleration.x, this.acceleration.y, this.acceleration.z + this.positionAcceleratorCoefficient);
			}

			if (Input.GetKey ("down")) {
					this.acceleration = new Vector3 (this.acceleration.x, this.acceleration.y, this.acceleration.z - this.positionAcceleratorCoefficient);
			}

			if (Input.GetKey ("right")) {
					this.acceleration = new Vector3 (this.acceleration.x + this.positionAcceleratorCoefficient, this.acceleration.y, this.acceleration.z);
			}

			if (Input.GetKey ("left")) {
					this.acceleration = new Vector3 (this.acceleration.x - this.positionAcceleratorCoefficient, this.acceleration.y, this.acceleration.z);
			}

			if (Input.GetKey ("o")) {
					this.acceleration = this.acceleration + new Vector3 (0.0f, - this.positionAcceleratorCoefficient, 0.0f);
			}

			if (Input.GetKey ("p")) {
					this.acceleration = this.acceleration + new Vector3 (0.0f, + this.positionAcceleratorCoefficient, 0.0f);
			}

			rigidbody.velocity += this.acceleration * (Time.deltaTime * Random.Range (0.9f, 1.1f));

			if (Input.GetKey ("z")) {
					this.rotationAcceleration = this.rotationAcceleration + new Vector3 (0.0f, - this.rotationAcceleratorCoefficient, 0.0f);
			}

			if (Input.GetKey ("x")) {
					this.rotationAcceleration = this.rotationAcceleration + new Vector3 (0.0f, + this.rotationAcceleratorCoefficient, 0.0f);
			}

			if (Input.GetKey ("a")) {
					this.rotationAcceleration = this.rotationAcceleration + new Vector3 (0.0f, 0.0f, - this.rotationAcceleratorCoefficient);
			}

			if (Input.GetKey ("d")) {
					this.rotationAcceleration = this.rotationAcceleration + new Vector3 (0.0f, 0.0f, + this.rotationAcceleratorCoefficient);
			}

			if (Input.GetKey ("w")) {
					this.rotationAcceleration = this.rotationAcceleration + new Vector3 (- this.rotationAcceleratorCoefficient, 0.0f, 0.0f);
			}

			if (Input.GetKey ("s")) {
					this.rotationAcceleration = this.rotationAcceleration + new Vector3 (+ this.rotationAcceleratorCoefficient, 0.0f, 0.0f);
			}


			rigidbody.angularVelocity += this.rotationAcceleration * (Time.deltaTime * Random.Range (0.9f, 1.1f));


			if (this.checkDocking ()) {
					this.dock ();
			}
	}
	}

	private void dock() {
		if (!isDocked) {
			this.acceleration = new Vector3(0.0f, 0.0f, 0.0f);
			this.rigidbody.velocity = new Vector3 (0.0f, 0.0f, 0.0f);
			this.rigidbody.angularVelocity = new Vector3 (0.0f, 0.0f, 0.0f);
			StartCoroutine (HookAnimation (hook1.transform, Quaternion.Euler (90.0f, 0.0f, 0.0f), this.hookSpeed, 0.1f));
			StartCoroutine (HookAnimation (hook2.transform, Quaternion.Euler (90.0f, 0.0f, 120.0f), this.hookSpeed, 0.1f));
			StartCoroutine (HookAnimation (hook3.transform, Quaternion.Euler (90.0f, 0.0f, 240.0f), this.hookSpeed, 0.1f));
			StartCoroutine (DuringDock());
			pipipiObj = (GameObject)Instantiate (pipipi, transform.position, transform.rotation);
			pipipiObj.transform.parent = transform;
			isDocked = true;
			paused = true;
		}
		

	}

	IEnumerator DuringDock() {
		centerGUI.text = "You did it";

		yield return new WaitForSeconds(dockedWait);
		this.level ++;
		StartCoroutine(DisplayLevel());
	}

	IEnumerator HookAnimation(Transform trans, Quaternion destRot, float speed, float threshold )
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

	private void ResetHooks() 
	{
		hook1.transform.rotation = originalHook1Rot;
		hook2.transform.rotation = originalHook2Rot;
		hook3.transform.rotation = originalHook3Rot;
	}

	private bool checkPositionX()
	{
		if (Mathf.Abs(rigidbody.position.x) > this.dockingPositionThreshold) {
			return false;
		}
		return true;
	}

	private bool checkPositionY()
	{
		if (rigidbody.position.y > this.dockingPositionThreshold) {
			return false;
		}
		return true;
	}

	private bool checkPositionZ()
	{
		if (Mathf.Abs(rigidbody.position.z) > this.dockingPositionThreshold) {
			return false;
		}
		return true;
	}

	private bool checkVelocity()
	{
		if (Mathf.Abs(rigidbody.velocity.magnitude) > this.dockingVelocityThreshold) {
			return false;
		}
		return true;
	}

    private bool checkRotationY(){

        float angleDifference = Mathf.Abs(rigidbody.rotation.eulerAngles.y - (this.station.rigidbody.rotation.eulerAngles.y + 30)) % 60;

		if ( (angleDifference < this.dockingRotationThreshold) || ((60 - angleDifference) < this.dockingRotationThreshold) ) {
            //Debug.Log("Our rotation" + rigidbody.rotation.eulerAngles.y);
            //Debug.Log("Station rotation" + (this.station.rigidbody.rotation.eulerAngles.y + 30));
            //Debug.Log("Angle difference" + Mathf.Abs(rigidbody.rotation.eulerAngles.y - (this.station.rigidbody.rotation.eulerAngles.y + 30)) % 60);
			return true;
		}
        return false;
    }

	private bool checkDocking()
	{
	
		if (!this.checkPositionX ()) {
			return false;
		}

		if (!this.checkPositionY ()) {
			return false;
		}

		if (!this.checkPositionZ ()) {
			return false;
		}
//
//		if (Mathf.Abs(rigidbody.position.z - this.station.rigidbody.position.z) > this.dockingPositionThreshold) {
//			return false;
//		}
//

		if (!this.checkVelocity ()) {
			return false;
		}

//		if (Mathf.Abs((rigidbody.rotation.x - 0.7071068f) - this.station.rigidbody.rotation.x) > this.dockingRotationThreshold) {
//			Debug.Log("camera " + (rigidbody.rotation.x ));
//			Debug.Log(this.station.rigidbody.rotation.x);
//			return false;
//		}
//
		if (!this.checkRotationY ()) {
			return false;
		}

//
//		if (Mathf.Abs(rigidbody.rotation.z - this.station.rigidbody.rotation.z) > this.dockingRotationThreshold) {
//			return false;
//		}


		return true;


	}

	void OnTriggerEnter(Collider other) {

		if (playing) {
			GameObject explosionObj = (GameObject) Instantiate (explosion, transform.position, transform.rotation);
			explosionObj.transform.parent = transform;


			StartCoroutine (GameOver("Crashed\nThat ship was worth $3bn"));


		}
	}

	IEnumerator GameOver(string reason) {
		playing = false;
		gameOver = true;
		restart = true;
		paused = true;
		setStationRendererStatus (false);
		centerGUI.text = "Game Over";	
		additionalCenterGUI.text = reason;
		yield return new WaitForSeconds(gameOverWait);
		additionalCenterGUI.text = "";
		centerGUI.text = "Press R to restart";	
		
		
	}

}
