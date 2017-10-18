using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manipulator : MonoBehaviour {

	public enlarge[] spheres;
	public Vector3 newSizeVec, newRotVec, temp1, temp2, temp3;
	public bool changingSize, changingRotate;
	public GameObject model;
	public outerArea OA;

	Vector3 origPos;
	float scalarScale;
	enlarge currentSphere;
	Rotator currentRotator;

	// Use this for initialization
	void Start () {

		spheres = GetComponentsInChildren<enlarge> ();
		newSizeVec = this.transform.localScale;
		scalarScale = this.transform.localScale.x;
		changingSize = false;
		changingRotate = false;
		origPos = transform.position;
		OA = GetComponentInChildren<outerArea> ();
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

		model.transform.localScale = this.transform.localScale;


		transform.Rotate (OA.lastRotate.eulerAngles, Space.World);



		model.transform.rotation = this.transform.rotation;

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
