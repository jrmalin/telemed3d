using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleLine : MonoBehaviour {

	LineRenderer myLR;
	// Use this for initialization
	void Start () {

		myLR = this.GetComponent<LineRenderer> ();
		myLR.positionCount = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
