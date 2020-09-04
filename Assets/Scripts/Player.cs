using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	const float CameraMouseRotationSpeed = 12f;
	const float CameraControllerRotationSpeed = 3.0f;
	const float CameraXRotMin = -40.0f;
	const float CameraXRotMax = 30.0f;

	const float DirectionInterpolateSpeed = 1.0f;
	const float MotionInterpolateSpeed = 10.0f;
	const float RotationInterpolateSpeed = 10.0f;

	const float Speed = 12.0f;

	Quaternion modelRotation = Quaternion.identity;
	float cameraXRot = 0.0f;

	float horizontal = 0.0f;
	float vertical = 0.0f;

	float mouseX = 0f;
	float mouseY = 0f;

	new Camera camera;
	new Rigidbody rigidbody;
	Animator animator;

	[SerializeField]
	GameObject model = null;

	[SerializeField]
	GameObject cameraPivot = null;

	[SerializeField]
	GameObject cameraBase = null;

	// ===================================================

	void Start()
	{
		camera = GetComponentInChildren<Camera>();
		rigidbody = GetComponent<Rigidbody>();
		animator = model.GetComponent<Animator>();

		Cursor.lockState = CursorLockMode.Locked;
	}


	void Update()
	{
		horizontal = Input.GetAxisRaw("Horizontal");
		vertical = Input.GetAxisRaw("Vertical");

		if (horizontal != 0f || vertical != 0f)
			animator.Play("Walk");
		else
			animator.Play("Idle");

		mouseX = Input.GetAxis("Mouse X");
		mouseY = Input.GetAxis("Mouse Y");

		if (Input.GetButtonDown("Action"))
			Controller.Singleton.Dialogue(new List<string>(){"Hello there", "How are you today", "This is a test"});
	}


	void FixedUpdate()
	{
		Vector3 target = new Vector3(horizontal, 0f, vertical);
		target = camera.transform.TransformDirection(target);
		target.y = 0f;
		Vector3 result = target * Speed;

		rigidbody.velocity = new Vector3(result.x, rigidbody.velocity.y, result.z);

		modelRotation = Quaternion.LookRotation(target, Vector3.up);
		Quaternion newrot = Quaternion.Slerp(model.transform.rotation, modelRotation, RotationInterpolateSpeed * Time.deltaTime);
		model.transform.rotation = newrot;

		RotateCamera(mouseX, mouseY);
	}

	void RotateCamera(float movex, float movey)
	{
		cameraBase.transform.RotateAround(cameraBase.transform.position, new Vector3(0, 1, 0), movex * CameraMouseRotationSpeed);
		cameraXRot += -movey * CameraMouseRotationSpeed;
		cameraXRot = Mathf.Clamp(cameraXRot, CameraXRotMin, CameraXRotMax);
		Vector3 rot = cameraPivot.transform.rotation.eulerAngles;
		rot.x = cameraXRot;
		cameraPivot.transform.rotation = Quaternion.Euler(rot);
	}
}
