using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selection : MonoBehaviour {

    int option;
    //options
    int move = 0;
    int rotate = 1;
    int enlarge = 2;
    int annotate = 3;

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
        if(command == move)
        {
            GazeGestureManager.Instance.Transition(GazeGestureManager.Instance.ManipulationRecognizer);
        }
        else if( command == rotate)
        {
            GazeGestureManager.Instance.ResetGestureRecognizers();
        }
        else if( command == enlarge)
        {
            GazeGestureManager.Instance.Transition(GazeGestureManager.Instance.ManipulationRecognizer);
            //reveal spheres for expansion
        }
    }
}
