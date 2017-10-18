using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {

	public GameObject model, modelProxy;
	public Manipulator man;
	public float extentConstant;

	// Use this for initialization
	void Start () {
		man = GetComponentInChildren<Manipulator>(true);
		man.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

		//If the user has imported a model...
		if (model) {
			man.gameObject.SetActive(true);
			man.model = model;
			//send the shrink constant to the manipulator.
			man.extentConstant = extentConstant;
		}


	}
}
