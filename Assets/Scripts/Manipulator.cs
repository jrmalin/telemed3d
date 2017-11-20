using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manipulator : MonoBehaviour {

	public enlarge[] spheres;
	public Vector3 newSizeVec, newRotVec;
	public bool changingSize;
	public GameObject model, modelProxy;
	public outerArea OA;
	public float extentConstant;

    public enum MoveDirection { Forward, Backward, Left, Right, Up, Down, None };
    MoveDirection currentMoveDir;
    public enum RotationDirection { Left, Right, Up, Down, None };
    RotationDirection currentRotationDir;
    public enum ScaleSize { Larger, Smaller, None };
    ScaleSize currentScaleSize;
    public enum ChangeType { Move, Rotate, Scale, Annotate, None };
    public ChangeType currentChangeType;
    float speed;

    Vector3 origPos;
	float scalarScale;
	enlarge currentSphere;

	// Use this for initialization
	void Start () {

		this.transform.localScale = new Vector3 (1, 1, 1);
		this.transform.localRotation = new Quaternion (0, 180, 0, 0);
		spheres = GetComponentsInChildren<enlarge> ();
		newSizeVec = this.transform.localScale;
		scalarScale = this.transform.localScale.x;
		changingSize = false;
		origPos = transform.position;
		OA = GetComponentInChildren<outerArea> ();

        speed = 0.5f;
        currentMoveDir = MoveDirection.None;
        currentRotationDir = RotationDirection.None;
        currentScaleSize = ScaleSize.None;
        currentChangeType = ChangeType.None;
    }


	//this function returns the enlarger that the user has just manipulated
	enlarge GetMovedEnlarger(){

		for (int i = 0; i < spheres.Length; ++i) {
			
			if (spheres [i].changed) {
				
				return spheres [i];
			}
		}

		return null;
	}



	void Update(){

        if (currentChangeType == ChangeType.Move)
        {
            Move(currentMoveDir);
        }
        else if (currentChangeType == ChangeType.Rotate)
        {
            Rotate(currentRotationDir);
        }
        else if (currentChangeType == ChangeType.Scale)
        {
            Scale(currentScaleSize);
        }
        else if( currentChangeType == ChangeType.Annotate)
        {
            //Do nothing
        }
        else if (currentChangeType == ChangeType.None)
        {
            Stop();
        }

        //update model size
        Vector3 temp;
		temp = this.transform.localScale * extentConstant;
		modelProxy.transform.localScale = temp;

		//update model rotation
		transform.Rotate (OA.lastRotate.eulerAngles, Space.World);
		modelProxy.transform.rotation = this.transform.rotation;

        //upadate model location
        modelProxy.transform.position = this.transform.position;


	}
		
	void FixedUpdate () {

        
		if (!changingSize) {

			enlarge index = GetMovedEnlarger ();

			if (index) {

				float newDist = (index.transform.position - this.transform.position).magnitude;
				float newSize = ((1) / Mathf.Sqrt (3)) * newDist;
				newSizeVec.Set (newSize, newSize, newSize);
				changingSize = true;
				currentSphere = index;

			}
				

		}

		if (changingSize){

			this.transform.localScale = Vector3.MoveTowards (this.transform.localScale, newSizeVec, Time.deltaTime);

			if (this.transform.localScale == newSizeVec) {
				changingSize = false;
				currentSphere.changed = false;
			}
		}


	}
    void Move(MoveDirection dir)
    {
        currentChangeType = ChangeType.Move;
        currentMoveDir = dir;
        float step = speed * Time.deltaTime;
        Vector3 target;

        if (dir == MoveDirection.Forward)
        {
            target = new Vector3(0, 0, -10);
        }
        else if (dir == MoveDirection.Backward)
        {
            target = new Vector3(0, 0, 10);
        }
        else if (dir == MoveDirection.Left)
        {
            target = new Vector3(-10, 0, 0);
        }
        else if (dir == MoveDirection.Right)
        {
            target = new Vector3(10, 0, 0);
        }
        else if (dir == MoveDirection.Up)
        {
            target = new Vector3(0, 10, 0);
        }
        else if (dir == MoveDirection.Down)
        {
            target = new Vector3(0, -10, 0);
        }
        else
        {
            target = new Vector3(0, 0, 0); ;
        }

        transform.position = Vector3.MoveTowards(transform.position, transform.position + target, step);
    }


    void Rotate(RotationDirection dir)
    {
        currentChangeType = ChangeType.Rotate;
        currentRotationDir = dir;
        float step = speed/5 * 360 * Time.deltaTime;

        if (dir == RotationDirection.Left)
        {
            transform.RotateAround(transform.position, Vector3.up, step);
        }
        else if (dir == RotationDirection.Right)
        {
            transform.RotateAround(transform.position, Vector3.up, -step);
        }
        else if (dir == RotationDirection.Up)
        {
            transform.RotateAround(transform.position, Vector3.right, step);
        }
        else if (dir == RotationDirection.Down)
        {
            transform.RotateAround(transform.position, Vector3.right, -step);
        }
    }

    void Scale(ScaleSize size)
    {
        currentChangeType = ChangeType.Scale;
        currentScaleSize = size;
        float step = speed * Time.deltaTime;
        Vector3 target;

        if (size == ScaleSize.Larger)
        {
            var growthFactor = transform.localScale[0] * 1.5f;
            target = new Vector3(growthFactor, growthFactor, growthFactor);
        }
        else if (size == ScaleSize.Smaller)
        {
            var shrinkFactor = -1 * transform.localScale[0] * 0.5f;
            target = new Vector3(shrinkFactor, shrinkFactor, shrinkFactor);
        }
        else
        {
            target = new Vector3(0, 0, 0);
        }


        transform.localScale = Vector3.MoveTowards(transform.localScale, transform.localScale + target, step);
    }

    void UpdateSpeed(bool faster)
    {
        if (faster)
        {
            speed *= 1.5f;
        }
        else
        {
            speed *= 0.66f;
        }
    }

    void Stop()
    {
        currentMoveDir = MoveDirection.None;
        currentRotationDir = RotationDirection.None;
        currentScaleSize = ScaleSize.None;
        currentChangeType = ChangeType.None;
        transform.localScale = Vector3.MoveTowards(transform.localScale, transform.localScale, 0);
        transform.position = Vector3.MoveTowards(transform.position, transform.position, 0);
        transform.RotateAround(transform.position, Vector3.up, 0);
        transform.RotateAround(transform.position, Vector3.right, 0);
    }

    void Reset()
    {
        Stop();
        speed = 0.5f;
        this.transform.localRotation = new Quaternion(0, 180, 0, 0);
        this.transform.localScale = new Vector3(1, 1, 1);
        this.transform.localPosition = new Vector3(0, 0, 0);
    }


}
