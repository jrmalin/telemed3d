using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class outerArea : MonoBehaviour {

	Vector3 origMousePos, finalMousePos;
	public Vector3 newForward, newUp;
	bool acceptingInput, rotated;
	public Manipulator man;
	public Vector3 rotateValues;
	public Quaternion lastRotate;
	public bool isRotating;

	Quaternion GetRotate(){

		Vector3 change = finalMousePos - origMousePos;
		if (change.magnitude < 0.01) {
			isRotating = false;
		}
		Vector3 tempRotation;
		tempRotation.x = change.y;
		tempRotation.y = -change.x;
		tempRotation.z = 0;

		return (Quaternion.Euler (tempRotation));


	}

	// Use this for initialization
	void Start () {
		newForward = transform.forward;
		acceptingInput = false;
		isRotating = false;
		man = GetComponentInParent<Manipulator> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (acceptingInput) {

			if (Input.GetMouseButtonUp (0)) {

				acceptingInput = false;
				finalMousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
				lastRotate = GetRotate ();
			}
		}
	}


	void OnMouseOver(){

		if (Input.GetMouseButtonDown (0)) {

			origMousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
			acceptingInput = true;
			isRotating = true;
		}
	}
		
}
