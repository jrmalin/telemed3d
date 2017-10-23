using System.Collections;
using System.Collections.Generic;
# if WINDOWS_UWP
using UnityEngine.XR;
# endif
using UnityEngine;

#if WINDOWS_UWP
public class enlarge : MonoBehaviour {

    public GameObject manipulator;


	Vector3 mousePosOnLastFrame, newPos;
	Ray directionOfMvmt;
	public bool changed, changing;
	Vector3 staticPos;

	// Use this for initialization
	void Start () {

		directionOfMvmt.origin = manipulator.transform.position;
		directionOfMvmt.direction = (this.transform.position - manipulator.transform.position).normalized;
		changed = false;
		changing = false;
	}

	void Update(){
        print("in update enlarge");
        if(changing){
            print("in update enlarge changing");
            //Vector3 vec = Input.mousePosition - mousePosOnLastFrame;
            Vector3 vec = InputTracking.GetLocalPosition(XRNode.RightHand) - mousePosOnLastFrame;
            //create the newPos vec
            float mag = vec.magnitude;
			//check to see if you are dragging in or out
			float toCenterDistNow, toCenterDistLast;
			Vector3 temp1 = Camera.main.WorldToViewportPoint (manipulator.transform.position);
            //Vector3 temp2 = Camera.main.ScreenToViewportPoint (Input.mousePosition);
            Vector3 temp2 = Camera.main.ScreenToViewportPoint(InputTracking.GetLocalPosition(XRNode.RightHand));
            Debug.Log("Enlarge -> Update -> changing -> hand location " + InputTracking.GetLocalPosition(XRNode.RightHand));
			Vector3 temp3 = Camera.main.ScreenToViewportPoint (mousePosOnLastFrame);

			toCenterDistNow = (temp2 - temp1).magnitude;
			toCenterDistLast = (temp3 - temp1).magnitude;

			if (toCenterDistNow - toCenterDistLast < 0) {

				mag *= -1;

			} else {

				mag *= 1;
			}


			newPos = this.transform.position + (directionOfMvmt.direction * mag);


			transform.position = Vector3.MoveTowards(transform.position, newPos, Time.deltaTime);			

			changing = true;
        }
		if (changed) {
            print("in update enlarge changed");
			this.GetComponentInParent<Rigidbody> ().constraints = RigidbodyConstraints.FreezePosition;
			this.transform.position = staticPos;


		} 
        else {
            print("in update enlarge else");
			this.GetComponentInParent<Rigidbody> ().constraints = RigidbodyConstraints.None;

		}


	}

	// Update is called once per frame
	void LateUpdate () {

		directionOfMvmt.origin = manipulator.transform.position;
		directionOfMvmt.direction = (this.transform.position - manipulator.transform.position).normalized;
        //mousePosOnLastFrame = Input.mousePosition;
        mousePosOnLastFrame = InputTracking.GetLocalPosition(XRNode.RightHand);


	}


	void OnMouseButtonDown(){

			changing = true;
	}
		
	void OnMouseButtonUp(){

		if (changing) {
			changed = true;
			changing = false;
			staticPos = this.transform.position;
		}


	}

}

# else
public class enlarge : MonoBehaviour {

    public GameObject manipulator;


	Vector3 mousePosOnLastFrame, newPos;
	Ray directionOfMvmt;
	public bool changed, changing;
	Vector3 staticPos;

	// Use this for initialization
	void Start () {

		mousePosOnLastFrame = Input.mousePosition;
		directionOfMvmt.origin = manipulator.transform.position;
		directionOfMvmt.direction = (this.transform.position - manipulator.transform.position).normalized;
		changed = false;
		changing = false;
	}

	void Update(){

		if (changed) {

			this.GetComponentInParent<Rigidbody> ().constraints = RigidbodyConstraints.FreezePosition;
			this.transform.position = staticPos;


		} else {

			this.GetComponentInParent<Rigidbody> ().constraints = RigidbodyConstraints.None;

		}


	}

	// Update is called once per frame
	void LateUpdate () {

		directionOfMvmt.origin = manipulator.transform.position;
		directionOfMvmt.direction = (this.transform.position - manipulator.transform.position).normalized;
		mousePosOnLastFrame = Input.mousePosition;


	}

	void OnMouseOver(){

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


			newPos = this.transform.position + (directionOfMvmt.direction * mag);


			transform.position = Vector3.MoveTowards(transform.position, newPos, Time.deltaTime);			

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
#endif