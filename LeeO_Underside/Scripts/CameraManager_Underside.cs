using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager_Underside : MonoBehaviour {

	public float followSpeed = 3; //Speed ​​at which the camera follows us
	public float mouseSpeed = 2; //Speed ​​at which we rotate the camera with the mouse
	//public float controllerSpeed = 5; //Speed ​​at which we rotate the camera with the joystick
	public float cameraDist = 3; //Distance to which the camera is located

	public Transform target; //Player the camera follows

	[HideInInspector]
	public Transform pivot; //Pivot on which the camera rotates(distance that we want between the camera and our character)
	[HideInInspector]
	public Transform camTrans; //Camera position

	float turnSmoothing = .1f; //Smooths all camera movements (Time it takes the camera to reach the rotation indicated with the joystick)
	public float minAngleX = -10;
	public float maxAngleX = 50; //Maximum angle that we allow the camera to reach
	public float minAngleY = -35;
	public float maxAngleY = 35;

	float smoothX;
	float smoothY;
	float smoothXvelocity;
	float smoothYvelocity;
	public float lookAngle; //Angle the camera has on the Y axis
	public float tiltAngle; //Angle the camera has up / down
	public bool AlwaysRotate = true;

	public bool lookingAround = false;

	public void Init()
	{
		camTrans = Camera.main.transform;
		pivot = camTrans.parent;
	}

	void FollowTarget(float d)
	{ //Function that makes the camera follow the player
		float speed = d * followSpeed; //Set speed regardless of fps
		Vector3 targetPosition = Vector3.Slerp(transform.position, target.position, speed); //Bring the camera closer to the player interpolating with the velocity(0.5 half, 1 everything)
		targetPosition.y += 0.1f;
		targetPosition.z = -4;
		transform.position = targetPosition; //Update the camera position
	}

	void HandleRotations(float d, float v, float h, float targetSpeed)
	{ //Function that rotates the camera correctly
		if (turnSmoothing > 0)
		{
			smoothX = Mathf.SmoothDamp(smoothX, h, ref smoothXvelocity, turnSmoothing);
			smoothY = Mathf.SmoothDamp(smoothY, v, ref smoothYvelocity, turnSmoothing);
		}
		else
		{
			smoothX = h;
			smoothY = v;
		}
		Quaternion targetRotation;
		lookAngle += smoothX * targetSpeed;
		tiltAngle += -smoothY * targetSpeed;
		lookAngle = Mathf.Clamp(lookAngle, minAngleY, maxAngleY);
		tiltAngle = Mathf.Clamp(tiltAngle, minAngleX, maxAngleX);
		targetRotation = Quaternion.Euler(tiltAngle, lookAngle, 0);
		if (!Input.GetMouseButton(1) && !AlwaysRotate)
		{
			targetRotation = Quaternion.Euler(0,0,0);
		}
		camTrans.transform.rotation = Quaternion.Slerp(camTrans.transform.rotation, targetRotation, d * 5);

	}

	private void FixedUpdate()
	{//Function that correctly rotates the camera based on the joystick / mouse and follows the player (the delta time is sent to be independent of the fps)
		float h = Input.GetAxis("Mouse X");
		float v = Input.GetAxis("Mouse Y");

		//float c_h = Input.GetAxis("RightAxis X");
		//float c_v = Input.GetAxis("RightAxis Y");

		float targetSpeed = mouseSpeed;

		/*if (c_h != 0 || c_v != 0)
		{ //Overwrites if i use joystick
			h = c_h;
			v = -c_v;
			targetSpeed = controllerSpeed; 
		}*/

		FollowTarget(Time.deltaTime); //Follow player
		HandleRotations(Time.deltaTime, v, h, targetSpeed); //Rotates camera
	}

	private void LateUpdate()
	{
		//Here begins the code that is responsible for bringing the camera closer by detecting wall
		float dist = cameraDist + 1.0f; // distance to the camera + 1.0 so the camera doesnt jump 1 unit in if it hits someting far out
		Ray ray = new Ray(camTrans.parent.position, camTrans.position - camTrans.parent.position);// get a ray in space from the target to the camera.
		RaycastHit hit;
		// read from the taret to the targetPosition;
		if (Physics.Raycast(ray, out hit, dist))
		{
			if (hit.transform.tag == "Wall")
			{
				// store the distance;
				dist = hit.distance - 0.25f;
			}
		}
		// check if the distance is greater than the max camera distance;
		if (dist > cameraDist) dist = cameraDist;
		camTrans.localPosition = new Vector3(0, 0, -dist);
	}

	public static CameraManager_Underside singleton; //You can call CameraManager.singleton from other script (There can be only one)
	void Awake()
	{
		singleton = this; //Self-assigns
		Init();
	}

}
