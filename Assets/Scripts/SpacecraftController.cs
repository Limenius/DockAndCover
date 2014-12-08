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


	public GUITexture crosshair;

	public GUIText distanceX;
	public GUIText distanceY;
	public GUIText distanceZ;
	//public GUIText accelerationY;
	public GUIText velocity;
	public GUIText rotationY;
	public GUIText rotationX;
	public GUIText rotationZ;

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

	public int lastLevel;

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
		this.lastLevel = 11;
		this.playing = false;
		this.paused = true;
		this.timerGUI.text = "";

	}

	public void Initial() {
		centerGUI.text = "DOCK AND COVER\nby Limenius";
		additionalCenterGUI.text = "Press space to begin";

		this.level = 1;
		setStationRendererStatus (false);
		ClearSecondaryGUI ();

		if (Input.GetKeyDown (KeyCode.Space)) {
			StartCoroutine(DisplayLevel());
		}
	}
	


	void Update()
	{
		if (this.playing) {
			if (!this.paused) {
				if (Input.GetKeyDown (KeyCode.R)) 
				{
					
					StartCoroutine(DisplayLevel());
					
				}
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


				//this.accelerationY.text = "Acceleration Y " + this.acceleration.y.ToString("n3");

				this.velocity.text = "Velocity " + this.rigidbody.velocity.magnitude.ToString("n3");
				if (!this.checkVelocity ()) {
						this.velocity.color = Color.red;
				} else {
						this.velocity.color = Color.green;
				}
	            
				this.rotationY.text = "Rotation Y " + this.rigidbody.rotation.eulerAngles.y.ToString("n3");
				if (!this.checkRotationY ()) {
						this.rotationY.color = Color.red;
				} else {
						this.rotationY.color = Color.green;
				}

				this.rotationX.text = "Rotation X " + (this.rigidbody.rotation.eulerAngles.x - 90).ToString("n3");
				if (!this.checkRotationX ()) {
						this.rotationX.color = Color.red;
				} else {
						this.rotationX.color = Color.green;
				}

				this.rotationZ.text = "Rotation Z " + this.rigidbody.rotation.eulerAngles.z.ToString("n3");
				if (!this.checkRotationZ ()) {
						this.rotationZ.color = Color.red;
				} else {
						this.rotationZ.color = Color.green;
				}
				this.crosshair.enabled = true;
			} else {
				ClearSecondaryGUI();

			}

		} else if (!this.gameOver) {
			Initial ();
		} else if (this.restart) {

			if (Input.GetKeyDown (KeyCode.R)) 
			{

				StartCoroutine(DisplayLevel());

			}

		}

	
	}

	public void ClearSecondaryGUI() {
		this.distanceX.text = "";
		this.distanceY.text = "";
		this.distanceZ.text = "";
		this.rotationX.text = "";
		this.rotationY.text = "";
		this.rotationZ.text = "";
		this.velocity.text = "";
		this.crosshair.enabled = false;
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

		this.timerGUI.text = "";
		switch (this.level) {
		case 1:
			this.levelTimed = false;
			this.additionalCenterGUI.text = "Press O and P to approach and retreat from the station\nWatch out your docking speed";
			this.transform.position = new Vector3(0.0f, 3.0f, 0.0f);
			this.rigidbody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
			this.transform.rotation = Quaternion.Euler (90.0f, 0.0f, 0.0f);
			this.rigidbody.angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
			break;
		case 2:
			this.levelTimed = false;
			this.additionalCenterGUI.text = "Let's try a bit further... and faster";
			this.transform.position = new Vector3(0.0f, 6.0f, 0.0f);
			this.rigidbody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
			this.transform.rotation = Quaternion.Euler (90.0f, 0.0f, 0.0f);
			this.rigidbody.angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
			this.levelTimed = false;
			break;
		case 3:
			this.levelTimed = false;
			this.additionalCenterGUI.text = "Use arrows to move\nIf you feel lost, press R to restart";			
			this.transform.position = new Vector3(0.0f, 6.0f, 0.0f);
			this.rigidbody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
			this.transform.rotation = Quaternion.Euler (90.0f, 0.0f, 0.0f);
			this.rigidbody.angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
			this.levelTimed = false;			
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
			this.additionalCenterGUI.text = "You can rotate in two more directions, right?\nUse A/D/W/S for that";
			this.transform.position = new Vector3(0.0f, 3.0f, 0.0f);
			this.rigidbody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
			this.transform.rotation = Quaternion.Euler (70.0f, 0.0f, 0.0f);
			this.rigidbody.angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
			break;
		case 6:
			this.levelTimed = true;
			this.additionalCenterGUI.text = "Ok... that was just a simulation\nLet's go for some real stuff!";
			this.transform.position = new Vector3(0.0f, 4.0f, 0.0f);
			this.rigidbody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
			this.transform.rotation = Quaternion.Euler (90.0f, 0.0f, 0.0f);
			this.rigidbody.angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
			this.levelTime = 10.0f;
			break;
		case 7:
			this.levelTimed = true;
			this.additionalCenterGUI.text = "Nice one...\nTry to control your spin quick now";
			this.transform.position = new Vector3(0.0f, 12.0f, 0.0f);
			this.rigidbody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
			this.transform.rotation = Quaternion.Euler (90.0f, 0.0f, 0.0f);
			this.rigidbody.angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
			this.levelTime = 15.0f;
			break;
		case 8:
			this.levelTimed = true;
			this.additionalCenterGUI.text = "You are good!\nLet's see how you deal with a crisis";
			this.transform.position = new Vector3(0.0f, 4.0f, 0.0f);
			this.rigidbody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
			this.transform.rotation = Quaternion.Euler (90.0f, 0.0f, 0.0f);
			this.rigidbody.angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
			this.levelTime = 25.0f;
			break;
		case 9:
			this.levelTimed = true;
			this.additionalCenterGUI.text = "Ok... What about a REAL crisis?";
			this.transform.position = new Vector3(0.0f, 2.0f, 0.0f);
			this.rigidbody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
			this.transform.rotation = Quaternion.Euler (90.0f, 0.0f, 0.0f);
			this.rigidbody.angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
			this.levelTime = 50.0f;
			break;
		case 10:
			this.levelTimed = true;
			this.additionalCenterGUI.text = "That was great, but you had a lot of time\nProve now that you are THE BEST\nand go happily to home";
			this.transform.position = new Vector3(-1.0f, 2.0f, -1.0f);
			this.rigidbody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
			this.transform.rotation = Quaternion.Euler (90.0f, -45.0f, -30.0f);
			this.rigidbody.angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
			this.levelTime = 30.0f;
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
		case 6:
			this.rigidbody.velocity = new Vector3(0.0f, 2.0f, 0.0f);
			this.rigidbody.angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
			break;
		case 7:
			this.rigidbody.velocity = new Vector3(0.0f, -3.0f, 0.0f);
			this.rigidbody.angularVelocity = new Vector3(0.0f, 0.6f, 0.0f);
			break;
		case 8:
			this.rigidbody.velocity = new Vector3(0.0f, 2.0f, 0.0f);
			this.rigidbody.angularVelocity = new Vector3(-2.0f, 0.0f, 0.0f);
			break;
		case 9:
			this.rigidbody.velocity = new Vector3(0.0f, 2.0f, 0.0f);
			this.rigidbody.angularVelocity = new Vector3(4.0f, 0.0f, 3.0f);
			break;
		case 10:
			this.rigidbody.velocity = new Vector3(0.0f, 2.0f, 0.0f);
			this.rigidbody.angularVelocity = new Vector3(4.0f, 0.0f, 3.0f);
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

				this.acceleration = this.acceleration + transform.TransformDirection(new Vector3 (0.0f, + this.positionAcceleratorCoefficient, 0.0f));

			}

			if (Input.GetKey ("down")) {
				this.acceleration = this.acceleration + transform.TransformDirection(new Vector3 (0.0f, - this.positionAcceleratorCoefficient, 0.0f));

			}

			if (Input.GetKey ("right")) {
				this.acceleration = this.acceleration + transform.TransformDirection(new Vector3 (+ this.positionAcceleratorCoefficient, 0.0f, 0.0f));
			}

			if (Input.GetKey ("left")) {
				this.acceleration = this.acceleration + transform.TransformDirection(new Vector3 (- this.positionAcceleratorCoefficient, 0.0f, 0.0f));
			}

			if (Input.GetKey ("o")) {
				this.acceleration = this.acceleration + transform.TransformDirection(new Vector3 (0.0f, 0.0f, + this.positionAcceleratorCoefficient));

			}

			if (Input.GetKey ("p")) {
				this.acceleration = this.acceleration + transform.TransformDirection(new Vector3 (0.0f, 0.0f, - this.positionAcceleratorCoefficient));


		
			}

			rigidbody.velocity += this.acceleration * (Time.deltaTime * Random.Range (0.9f, 1.1f));

			if (Input.GetKey ("z")) {
				this.rotationAcceleration = this.rotationAcceleration + transform.TransformDirection(new Vector3 (0.0f, 0.0f, + this.rotationAcceleratorCoefficient));
			}

			if (Input.GetKey ("x")) {
				this.rotationAcceleration = this.rotationAcceleration + transform.TransformDirection(new Vector3 (0.0f, 0.0f, - this.rotationAcceleratorCoefficient));
			}

			if (Input.GetKey ("a")) {
				this.rotationAcceleration = this.rotationAcceleration + transform.TransformDirection(new Vector3 (0.0f, - this.rotationAcceleratorCoefficient, 0.0f ));


			}

			if (Input.GetKey ("d")) {
				this.rotationAcceleration = this.rotationAcceleration + transform.TransformDirection(new Vector3 (0.0f, + this.rotationAcceleratorCoefficient, 0.0f ));
			}

			if (Input.GetKey ("w")) {
				this.rotationAcceleration = this.rotationAcceleration + transform.TransformDirection(new Vector3 (- this.rotationAcceleratorCoefficient, 0.0f, 0.0f ));

			}

			if (Input.GetKey ("s")) {
				this.rotationAcceleration = this.rotationAcceleration + transform.TransformDirection(new Vector3 (+ this.rotationAcceleratorCoefficient, 0.0f, 0.0f ));
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
			StartCoroutine (HookAnimation (hook1.transform, 90.0f, 0.0f, 0.0f));
			StartCoroutine (HookAnimation (hook2.transform, 90.0f, 0.0f, 0.0f));
			StartCoroutine (HookAnimation (hook3.transform, 90.0f, 0.0f, 0.0f));

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
		ResetHooks ();
		this.level ++;
		if (this.level == this.lastLevel) {
			YouWon();
		} else {
		
			StartCoroutine(DisplayLevel());
		}
	}

	public void YouWon() {
		centerGUI.text = "Congratulations! You won";
		additionalCenterGUI.text = "You have mastered this thing\nPress R to play again";
		playing = false;
		gameOver = true;
		restart = true;
		paused = true;
		this.level = 1;
		setStationRendererStatus (false);
	}

	IEnumerator HookAnimation(Transform transform, float targetAngleX, float targetAngleY, float targetAngleZ )
	{
		
		float totalTime = 1.0f;
		float timeElapsed = 0.0f;
		
		while (timeElapsed < totalTime)
		{
			timeElapsed += Time.deltaTime;
			transform.Rotate(targetAngleX * Time.deltaTime / totalTime, targetAngleY * Time.deltaTime / totalTime, targetAngleZ * Time.deltaTime / totalTime);
			
			yield return null;
			
		}
		Destroy (pipipiObj);
		GameObject dockingObj = (GameObject)Instantiate (docking, transform.position, transform.rotation);
		dockingObj.transform.parent = transform;

		
	}


	private void ResetHooks() 
	{
		hook1.transform.Rotate(-90.0f, 0.0f, 0.0f);
		hook2.transform.Rotate(-90.0f, 0.0f, 0.0f);

		hook3.transform.Rotate(-90.0f, 0.0f, 0.0f);


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

    private bool checkRotationX(){

		if (Mathf.Abs((rigidbody.rotation.eulerAngles.x - 90) - this.station.rigidbody.rotation.eulerAngles.x) > 5) {
			return false;
		}
        return true;
    }

    private bool checkRotationZ(){

		if (Mathf.Abs(rigidbody.rotation.eulerAngles.z - (this.station.rigidbody.rotation.eulerAngles.z)) > 5) {
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
			StartCoroutine (GameOver("Crashed\nThat ship was worth $3bn"));


		}
	}

	IEnumerator GameOver(string reason) {
		GameObject explosionObj = (GameObject) Instantiate (explosion, transform.position, transform.rotation);
		explosionObj.transform.parent = transform;
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
