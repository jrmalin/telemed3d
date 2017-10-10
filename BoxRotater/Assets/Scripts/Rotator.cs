using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {


	float staticDist;
	Ray directionToCenter;
	Vector3 mousePosOnLastFrame;
	public bool changed, changing;
	public GameObject manipulator;
	public Vector3 staticPos;
	public Vector3 prevPos;
	public Vector3 rotateDirection;
	public Vector3 origPos;

	// Use this for initialization
	void Start () {
		
		staticDist = manipulator.transform.localScale.x;
		prevPos = this.transform.position;
		origPos = this.transform.localPosition;

		changed = false;
		changing = false;
	}
	
	// Update is called once per frame
	void Update () {
		staticDist = manipulator.transform.localScale.x*Mathf.Sqrt(3)/2;
		mousePosOnLastFrame = Input.mousePosition;
		rotateDirection = this.transform.localPosition - manipulator.transform.position;

		if (!changed && !changing) {

			prevPos = this.transform.position;

		}

		if (changed) {

			this.transform.position = staticPos;


		}

		if (!manipulator.GetComponent<Manipulator> ().changingRotate && !changing && !changed) {

			this.transform.localPosition = origPos;

		}

	}
		

	void OnMouseOver(){

		directionToCenter.direction = this.transform.position - manipulator.transform.position;
		directionToCenter.origin = manipulator.transform.position;




		if (Input.GetMouseButtonDown (0)) {


			changing = true;
		}


		if (Input.GetMouseButton (0)) {

			Vector3 vec = Input.mousePosition - mousePosOnLastFrame;




			//create the newPos vec
			float mag = vec.magnitude;
			//check to see if you are dragging in or out
			float toCenterDistNow, toCenterDistLast;
			Vector3 temp1 = Camera.main.WorldToViewportPoint (manipulator.transform.position);
			Vector3 temp2 = Camera.main.ScreenToViewportPoint (Input.mousePosition);
			Vector3 temp3 = Camera.main.ScreenToViewportPoint (mousePosOnLastFrame);

			toCenterDistNow = (temp2 - temp1).magnitude;
			toCenterDistLast = (temp3 - temp1).magnitude;


			if (toCenterDistNow - toCenterDistLast < 0) {

				mag *= -1;

			} else {

				mag *= 1;
			}

			Vector3 newPos;

			if (mag < 0) {

				newPos = Camera.main.ViewportToWorldPoint (Input.mousePosition);

				newPos = manipulator.transform.InverseTransformPoint (newPos);

				if (origPos.x == 0) {

					newPos.x = 0;
				}else if(origPos.y == 0){

					newPos.y = 0;
				}else if(origPos.z == 0){

					newPos.z = 0;
				}

				newPos.Normalize ();
				newPos = newPos * staticDist;

				transform.localPosition = Vector3.MoveTowards(transform.localPosition, newPos, Time.deltaTime);	
			}


			changing = true;



		}

		if (Input.GetMouseButtonUp (0)) {

			changing = false;
			changed = true;
			staticPos = this.transform.position;

		}
			



	}

	void OnMouseExit(){

		if (changing) {
			changed = true;
			changing = false;
			staticPos = this.transform.position;
		}


	}
}
