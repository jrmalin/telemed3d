using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manipulator : MonoBehaviour {

	public enlarge[] spheres;
	public Vector3 newSizeVec, newRotVec, temp1, temp2, temp3, newPosVec;
	public bool changingSize, changingRotate, changingPosition;
	public GameObject model;
	public outerArea OA;

	Vector3 origPos;
	float scalarScale;
	enlarge currentSphere;
	Rotator currentRotator;

	public bool rotateOn, moveOn;

	// Use this for initialization
	void Start () {

		spheres = GetComponentsInChildren<enlarge> ();
		newSizeVec = this.transform.localScale;
		scalarScale = this.transform.localScale.x;
		changingSize = false;
		changingRotate = false;
		changingPosition = false;
		origPos = transform.position;
		OA = GetComponentInChildren<outerArea> ();

		rotateOn = true;
		moveOn = false;
	}


	//this function returns the enlarger that the user has just manipulated
	enlarge GetMovingEnlarger(){

		for (int i = 0; i < spheres.Length; ++i) {

			if (spheres [i].changed) {

				return spheres [i];
			}
		}
			




		return null;


	}



	void Update(){

		if (moveOn) {
			rotateOn = false;
		}

		if (rotateOn) {
			moveOn = false;
		}

		model.transform.localScale = this.transform.localScale;

		if (rotateOn) {
			transform.Rotate (OA.lastRotate.eulerAngles, Space.World);
			model.transform.rotation = this.transform.rotation;
		}

		if (moveOn && !changingPosition) {

			Vector3 posChange = OA.finalMousePos - OA.origMousePos;
			newPosVec = this.transform.position + posChange;

			changingPosition = true;


		}

		if (changingPosition) {

			Vector3.MoveTowards (this.transform.position, newPosVec, Time.deltaTime);
			if (this.transform.position == newPosVec) {

				changingPosition = false;
			}
		}

	}
		
	void FixedUpdate () {


		if (!changingSize) {
			enlarge index = GetMovingEnlarger ();

			if (index) {

				float newDist = (index.transform.position - this.transform.position).magnitude;
				float newSize = ((1) / Mathf.Sqrt (3)) * newDist;

				newSizeVec.Set (newSize, newSize, newSize);
				changingSize = true;
				currentSphere = index;

			}
				

		}

		if (changingSize){

			this.transform.localScale = Vector3.MoveTowards (this.transform.localScale, newSizeVec, Time.deltaTime);

			if (this.transform.localScale == newSizeVec) {
				changingSize = false;
				currentSphere.changed = false;
			}
		}


	}


}
