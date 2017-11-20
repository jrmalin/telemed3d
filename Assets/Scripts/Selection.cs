using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Selection : MonoBehaviour {
    public Manipulator man;
    public GameObject tutorial_view;
    int option;
    //options
    int annotate = 0;
    int manipulate = 1;
    int tutorial = 3;

	// Use this for initialization
	void Start () {
        option = 1;
        tutorial_view.SetActive(false);
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
        else if( command == tutorial)
        {
            tutorial_view.SetActive(!tutorial_view.active);
            if (tutorial_view.active)
            {
                Text text = GetComponentInChildren<Text>();
                text.text = "Exit Tutorial";
            }
            else
            {
                Text text = GetComponentInChildren<Text>();
                text.text = "Tutorial";
            }

        }
    }
}
