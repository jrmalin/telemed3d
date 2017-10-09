using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SFB;
using System;
using System.IO;

public class OnTap : MonoBehaviour {

	public GameObject prefab;
	public GameObject model;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {

	}

	void OnMouseOver() {
		if(Input.GetMouseButtonDown(0)){
			var extensions = new [] {
				new ExtensionFilter("3D Model Files", "obj")
			};
			StandaloneFileBrowser.OpenFilePanelAsync("Open File", "", extensions, false, (string[] paths) => { 
				if (paths.Length != 0 && paths [0].Length != 0) {
					paths[0] = "Assets/Models/" + paths[0].Substring (37); //TODO get file without hardcoding
					print (paths[0]);
					StartCoroutine(importObject (paths[0])); 
				}
			});
		}
	}

	IEnumerator importObject(String path) {
		ObjImporter importer = new ObjImporter ();
		Mesh mesh = importer.ImportFile (path);
		GameObject model = (GameObject)Instantiate (prefab);
		model.GetComponent<MeshFilter> ().mesh = mesh;
		print ("made model");
		yield return model;
	}
		

	IEnumerator OpenFilePicker() {
		string path = EditorUtility.OpenFilePanel("Select a 3D model", "", "obj");
		if (path.Length != 0) {
			print (path);
			ObjImporter importer = new ObjImporter();
			Mesh mesh = importer.ImportFile(path);
			GameObject model = (GameObject)Instantiate(prefab);
			model.GetComponent<MeshFilter>().mesh = mesh;
			print ("made model");
			yield return model;
		}
	}
}