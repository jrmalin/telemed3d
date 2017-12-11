using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleLine : MonoBehaviour {

	public LineRenderer myLR;
	// Use this for initialization
	void Awake () {

		myLR = this.GetComponent<LineRenderer> ();
		//myLR.positionCount = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
