using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFB;
using System;
using System.IO;

public class OnTap : MonoBehaviour {


	public SpawnPoint sp;

	// Use this for initialization
	void Start() {
		sp = FindObjectOfType<SpawnPoint> ();
	}

	// Update is called once per frame
	void Update () {

	}

	void OnMouseOver() {
		if(Input.GetMouseButtonDown(0)){
			var extensions = new [] {
				new ExtensionFilter("3D Model Files", "obj")
			};
			StartCoroutine(ImportObject (null)); 
			/*StandaloneFileBrowser.OpenFilePanelAsync("Open File", "", extensions, false, (string[] paths) => { 
				if (paths.Length != 0 && paths [0].Length != 0) {
					#if UNITY_STANDALONE_OSX //TODO this only works on Steven's laptop
						String path = paths[0].Substring (57); 
						StartCoroutine(ImportObject (path)); 
					#elif UNITY_STANDALONE_WIN
						String path = paths[0];
						StartCoroutine(ImportObject (path)); 
					#endif
				}
			});*/
		}
	}

	IEnumerator ImportObject(String path) {
		/*ObjImporter importer = new ObjImporter ();
		Mesh mesh = importer.ImportFile (path);*/
		Mesh mesh = Resources.Load ("model_mesh", typeof(Mesh)) as Mesh;
		GameObject model = new GameObject();
		model.transform.position = sp.transform.position;
		model.transform.rotation = Quaternion.identity;
		model.transform.Rotate (0,0,0);
		MeshFilter meshFilter = model.AddComponent<MeshFilter>();
		meshFilter.sharedMesh = mesh;
		MeshRenderer meshRenderer = model.AddComponent<MeshRenderer>();
//TODO figure out how to add proper material

		//fix the model's size and pivot
		sp.model = model;
		model.transform.parent = sp.modelProxy.transform;
		Mesh m = model.GetComponent<MeshFilter> ().mesh;
		Vector3[] vertices = m.vertices;
		Vector3 center = Vector3.zero;

		//Recenter about origin
		for (int i = 0; i < vertices.Length; ++i) {

			center.x += vertices [i].x;
			center.y += vertices [i].y;
			center.z += vertices [i].z;

		}

		center.x = center.x / vertices.Length;
		center.y = center.y / vertices.Length;
		center.z = center.z / vertices.Length;

		model.transform.localPosition -= center;


		//Resize to fit into box.
		Vector3 modelExtents = m.bounds.extents;
		float extentConstant = 1;


		if (modelExtents.x >= modelExtents.y && modelExtents.x >= modelExtents.z) {
			extentConstant = Mathf.Pow(modelExtents.x, -1);
		} else if (modelExtents.y >= modelExtents.x && modelExtents.y >= modelExtents.z) {
			extentConstant = Mathf.Pow(modelExtents.y, -1);
		} else if (modelExtents.z >= modelExtents.x && modelExtents.z >= modelExtents.y) {
			extentConstant = Mathf.Pow(modelExtents.z, -1);
		}

		//send the shrinking constant to the spawnPoint Obj.
		sp.extentConstant = extentConstant;


		yield return model;
	}
}