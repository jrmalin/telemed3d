using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if WINDOWS_UWP
using UnityEngine.XR;
#endif

#if WINDOWS_UWP
public class outerArea : MonoBehaviour
{

    Vector3 origMousePos, finalMousePos;
    public Vector3 newForward, newUp;
    bool acceptingInput, rotated;
    public Manipulator man;
    public Vector3 rotateValues;
    public Quaternion lastRotate;

    Quaternion GetRotate()
    {

        Vector3 change = finalMousePos - origMousePos;
        Vector3 tempRotation;
        tempRotation.x = change.y;
        tempRotation.y = -change.x;
        tempRotation.z = 0;

        return (Quaternion.Euler(tempRotation));


    }

    // Use this for initialization
    void Start()
    {
        newForward = transform.forward;
        acceptingInput = false;
        man = GetComponentInParent<Manipulator>();
    }

    // Update is called once per frame
    void Update() {
    }

    void GetMouseButtonUp(){

        if (acceptingInput) {
            acceptingInput = false;
            finalMousePos = Camera.main.ScreenToViewportPoint(InputTracking.GetLocalPosition(XRNode.RightHand));
            Debug.Log("OuterArea -> GetMouseButtonUP -> AcceptingInput -> hand location " + InputTracking.GetLocalPosition(XRNode.RightHand));
            lastRotate = GetRotate();
        }
}



    void OnMouseButtonDown() { 

        origMousePos = Camera.main.ScreenToViewportPoint(InputTracking.GetLocalPosition(XRNode.RightHand));
        Debug.Log("OuterArea -> GetMouseButtonDown -> hand location " + InputTracking.GetLocalPosition(XRNode.RightHand));

        acceptingInput = true;


     }




}

#else
public class outerArea : MonoBehaviour {

	Vector3 origMousePos, finalMousePos;
	public Vector3 newForward, newUp;
	bool acceptingInput, rotated;
	public Manipulator man;
	public Vector3 rotateValues;
	public Quaternion lastRotate;

	Quaternion GetRotate(){

		Vector3 change = finalMousePos - origMousePos;
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


		}
			


	}
		
}
#endif