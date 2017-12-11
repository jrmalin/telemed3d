using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnotationMoveAction : MonoBehaviour {

    private Vector3 manipulationPreviousPosition;
    public bool isMoving;
    // Use this for initialization
    void Start () {
        isMoving = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void PerformManipulationStart(Vector3 position)
    {
        if (isMoving)
        {
            manipulationPreviousPosition = position;
        }
    }

    void PerformManipulationUpdate(Vector3 position)
    {
        if (AnnotationMoveManager.Instance.IsManipulating && isMoving)
        {
            /* TODO: DEVELOPER CODING EXERCISE 4.a */

            Vector3 moveVector = Vector3.zero;

            // 4.a: Calculate the moveVector as position - manipulationPreviousPosition.
            moveVector = 4*(position - manipulationPreviousPosition);

            // 4.a: Update the manipulationPreviousPosition with the current position.
            manipulationPreviousPosition = position;

            // 4.a: Increment this transform's position by the moveVector.
            transform.position += moveVector;
        }
    }
}
