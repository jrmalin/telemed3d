using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class AnnotateLayer : MonoBehaviour {


	public GameObject inkBlot;
	public bool writing;
	public bool onLayer;
	public Vector3 prevPos;
	public List<SingleLine> lines;
	public GameObject prefabLine;
	public Material currentMat;
    public Vector3 startHandLocation;
    public Vector3 startHeadLocation;
    public Vector3 gazeDirection;


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

	public void changePenColor(MatColor clr){

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

	public void Undo(){

		GameObject temp = lines [lines.Count - 1].gameObject;
		lines.RemoveAt (lines.Count - 1);
		Destroy (temp);

	}

	public void Clear(){

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
            startHeadLocation = Camera.main.transform.position;
            gazeDirection = Camera.main.transform.forward;
            startHandLocation = getHandLocation();
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

    Vector3 getHandLocation()
    {
        var sourceStates = InteractionManager.GetCurrentReading();
        Vector3 handLocation = new Vector3();
        int i;
        for (i = 0; i < sourceStates.Length; i++)
        {
            if (sourceStates[i].source.kind == InteractionSourceKind.Hand)
            {
                print("getting hand location");
                sourceStates[i].sourcePose.TryGetPosition(out handLocation);
                
            }
        }
        return handLocation;
    }
	void LateUpdate(){
#if WINDOWS_UWP
        //draw circle at current spot.
        if (writing)
        {
            print("in lateUpdate");

            // var headPosition = Camera.main.transform.position;
            
            Vector3 handLocation = startHeadLocation +  4*(getHandLocation() - startHandLocation);
            
            print("hand location : " + handLocation.ToString("F4"));
            print("head location : " + Camera.main.transform.position.ToString("F4"));
            RaycastHit hitInfo;
            if (Physics.Raycast(handLocation, gazeDirection, out hitInfo))
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
                print(gameObject.name);
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
