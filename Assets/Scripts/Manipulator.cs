using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manipulator : MonoBehaviour {

	public enlarge[] spheres;
	public Vector3 newSizeVec, newRotVec, temp1, temp2, temp3;
	public bool changingSize;
	public GameObject model, modelProxy;
	public outerArea OA;
	public float extentConstant;

	Vector3 origPos;
	float scalarScale;
	enlarge currentSphere;

	// Use this for initialization
	void Start () {

		this.transform.localScale = new Vector3 (2, 2, 2);
		spheres = GetComponentsInChildren<enlarge> ();
		newSizeVec = this.transform.localScale;
		scalarScale = this.transform.localScale.x;
		changingSize = false;
		origPos = transform.position;
		OA = GetComponentInChildren<outerArea> ();
	}


	//this function returns the enlarger that the user has just manipulated
	enlarge GetMovedEnlarger(){

		for (int i = 0; i < spheres.Length; ++i) {
			
			if (spheres [i].changed) {
				
				return spheres [i];
			}
		}

		return null;
	}



	void Update(){

		//update model size
		Vector3 temp;
		temp = this.transform.localScale * extentConstant;
		modelProxy.transform.localScale = temp;

		//update model rotation
		transform.Rotate (OA.lastRotate.eulerAngles, Space.World);
		modelProxy.transform.rotation = this.transform.rotation;

	}
		
	void FixedUpdate () {


		if (!changingSize) {
			enlarge index = GetMovedEnlarger ();

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
