using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnotateLayer : MonoBehaviour {


	public GameObject inkBlot;
	public bool writing;
	public bool onLayer;
	public Vector3 prevPos;
	public List<SingleLine> lines;
	public GameObject prefabLine;
	public Material currentMat;

	public enum MatColor {BLUE, RED, GREEN, BLACK, WHITE};

	// Use this for initialization
	void Start () {
        lines = new List<SingleLine>();
		inkBlot = Resources.Load<GameObject> ("Ink");
		writing = false;
		onLayer = false;
		prefabLine = Resources.Load<GameObject> ("Line");
		currentMat = Resources.Load<Material> ("green");
	}

	void changePenColor(MatColor clr){

		if (clr == MatColor.BLUE) {

			currentMat = Resources.Load<Material> ("blue");
		} else if (clr == MatColor.GREEN) {

			currentMat = Resources.Load<Material> ("green");
		} else if (clr == MatColor.RED) {

			currentMat = Resources.Load<Material> ("red");
		} else if (clr == MatColor.BLACK) {

			currentMat = Resources.Load<Material> ("black");
		} else if (clr == MatColor.WHITE) {

			currentMat = Resources.Load<Material> ("white");
		}
	}

	void Undo(){

		GameObject temp = lines [lines.Count - 1].gameObject;
		lines.RemoveAt (lines.Count - 1);
		Destroy (temp);

	}

	void Clear(){

		for (int i = 0; i < lines.Count; ++i) {

			GameObject temp = lines [i].gameObject;
			Destroy (temp);


		}
		lines.Clear ();
	}

	void OnMouseEnter(){

		onLayer = true;

	}

	void OnMouseExit(){

		onLayer = false;
	}
    public void AnnotateStart(Vector3 position)
    {
        GameObject newLine = Instantiate(prefabLine, this.gameObject.transform);
        //GameObject newLine = Instantiate(prefabLine, GameObject.Find("Model").transform);
        newLine.GetComponent<LineRenderer>().material = currentMat;
        SingleLine line = newLine.GetComponent<SingleLine>();
        if (line == null)
        {
            print("line doesnt exist");
        }
        if (lines == null)
        {
            print("lines doesnt exist");
        }
        lines.Add(line);
    }

    public void AnnotateUpadate(Ray ray)
    {
        //draw circle at current spot.
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        //Instantiate (inkBlot, hit.point, new Quaternion (0, 0, 0, 0), this.gameObject.transform);

        LineRenderer currentLR = lines[lines.Count - 1].GetComponent<LineRenderer>();

        int index = currentLR.positionCount;
        currentLR.positionCount = index + 1;
        currentLR.SetPosition(index, this.transform.InverseTransformPoint(hit.point));
    }

	// Update is called once per frame
	void Update () {
#if WINDOWS_UWP
        if (GazeGestureManager.Instance.isAnnotating && !writing)
        {
            print("in update");
            writing = true;
            prevPos = GazeGestureManager.Instance.AnnotationPosition;
            GameObject newLine = Instantiate(prefabLine, this.gameObject.transform);
            //GameObject newLine = Instantiate(prefabLine, GameObject.Find("Model").transform);
            newLine.GetComponent<LineRenderer>().material = currentMat;
            SingleLine line = newLine.GetComponent<SingleLine>();
            if (line == null)
            {
                print("line doesnt exist");
            }
            if (lines == null)
            {
                print("lines doesnt exist");
            }
            lines.Add(line);
            print("line list size");
            print(lines.Count);
         
        }
        else if(!GazeGestureManager.Instance.isAnnotating)
        {
            writing = false;
        }
    #else
        if (Input.GetMouseButtonDown (0) && onLayer) {

			writing = true;
			prevPos = Input.mousePosition;
            GameObject newLine = Instantiate (prefabLine, this.gameObject.transform);
            //GameObject newLine = Instantiate(prefabLine, GameObject.Find("Model").transform);
			newLine.GetComponent<LineRenderer> ().material = currentMat;
            SingleLine line = newLine.GetComponent<SingleLine>();
            if( line == null)
            {
                print("line doesnt exist");
            }
            if( lines == null)
            {
                print("lines doesnt exist");
            }
            lines.Add(line);
		}


		if (Input.GetMouseButtonUp (0)) {

			writing = false;
		}
#endif

    }

	void LateUpdate(){
#if WINDOWS_UWP
        //draw circle at current spot.
        if (writing)
        {
            print("in lateUpdate");
   
            var headPosition = Camera.main.transform.position;
            var gazeDirection = Camera.main.transform.forward;

            RaycastHit hitInfo;
            if (Physics.Raycast(headPosition, gazeDirection, out hitInfo))
            {
                print("model name an location");
                print(hitInfo.collider.gameObject.name);
                print(hitInfo.collider.gameObject.transform.position);

                //Instantiate (inkBlot, hit.point, new Quaternion (0, 0, 0, 0), this.gameObject.transform);

                LineRenderer currentLR = lines[lines.Count - 1].GetComponent<LineRenderer>();

                int index = currentLR.positionCount;
                currentLR.positionCount = index + 1;
                print("string current index and position");
                print(currentLR.gameObject.transform.position);
                print(index);
                string point_str = "hit point vector: " + hitInfo.point.ToString("F4");
                print(point_str);
                currentLR.SetPosition(index, this.transform.InverseTransformPoint(hitInfo.point));
                //currentLR.SetPosition(index, (hitInfo.point));
            }
            else
            {
                writing = false;
            }

        }
#else
        if (writing) {
			//draw circle at current spot.
			RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		    if(Physics.Raycast(ray, out hit)){
                //Instantiate (inkBlot, hit.point, new Quaternion (0, 0, 0, 0), this.gameObject.transform);

                LineRenderer currentLR = lines[lines.Count - 1].GetComponent<LineRenderer>();

                int index = currentLR.positionCount;
                currentLR.positionCount = index + 1;
                currentLR.SetPosition(index, this.transform.InverseTransformPoint(hit.point));
            }
            else
            {
                writing = false;
            }

		}
#endif
    }




}
