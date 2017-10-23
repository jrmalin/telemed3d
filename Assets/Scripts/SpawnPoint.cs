using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {

	public GameObject model, modelProxy;
	public Manipulator man;
	public float extentConstant;
    public Vector3 origPos;
    bool print_once = false;

	// Use this for initialization
	void Start () {
		man = GetComponentInChildren<Manipulator>(true);
		man.gameObject.SetActive(false);
        origPos = transform.position;
        print(origPos);
	}
	
	// Update is called once per frame
	void Update () {


        transform.position = origPos;
		//If the user has imported a model...
		if (model && !print_once) {
			man.gameObject.SetActive(true);
			man.model = model;
            //send the shrink constant to the manipulator.
            man.extentConstant = extentConstant;
            man.transform.position = origPos;
            if (!print_once)
            {
                print(origPos);
                print_once = true;
            }
        }


	}
}
