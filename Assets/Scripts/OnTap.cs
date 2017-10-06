using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class OnTap : MonoBehaviour {

	public GameObject prefab;
	public GameObject model;

	[MenuItem("Example/Overwrite Texture")]

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseOver() {
		if(Input.GetMouseButtonDown(0)){
			StartCoroutine(OpenFilePicker());
		}
	}

	IEnumerator OpenFilePicker() {
		string path = EditorUtility.OpenFilePanel("Select a 3D model", "", "obj");
		if (path.Length != 0) {
			ObjImporter importer = new ObjImporter();
			Mesh mesh = importer.ImportFile(path);
			GameObject model = (GameObject)Instantiate(prefab);
			model.GetComponent<MeshFilter>().mesh = mesh;
			print ("made model");
			yield return model;
		}
	}
}
