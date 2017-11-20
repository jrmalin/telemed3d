using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selection : MonoBehaviour {
    public Manipulator man;
    int option;
    //options
    int annotate = 0;
    int manipulate = 1;

	// Use this for initialization
	void Start () {
        option = 1;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void select(int command)
    {
        option = command;
        if(command == annotate)
        {
            man.currentChangeType = Manipulator.ChangeType.Annotate;
            man.transform.gameObject.SetActive(false);
            GazeGestureManager.Instance.Transition("AnnotationRecognizer");
        }
        else if( command == manipulate)
        {
            man.currentChangeType = Manipulator.ChangeType.None;
            man.transform.gameObject.SetActive(true);
            GazeGestureManager.Instance.Transition("NavigationRecognizer");
        }
    }
}
