using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFB;
using System;
using System.IO;

public class OnTap : MonoBehaviour {

//	public GameObject prefab;

	// Use this for initialization
	void Start() {

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
					#if UNITY_STANDALONE_OSX //TODO this only works on Steven's laptop
						String path = paths[0].Substring (87); 
						StartCoroutine(ImportObject (path)); 
					#elif UNITY_STANDALONE_WIN
						String path = paths[0];
						StartCoroutine(ImportObject (path)); 
					#endif
				}
			});
		}
	}

	IEnumerator ImportObject(String path) {
		ObjImporter importer = new ObjImporter ();
		Mesh mesh = importer.ImportFile (path);

		GameObject model = new GameObject();
		model.transform.position = new Vector3(0, -10, 20);
		model.transform.rotation = Quaternion.identity;
		model.transform.Rotate (-90,90,0);
		MeshFilter meshFilter = model.AddComponent<MeshFilter>();
		meshFilter.sharedMesh = mesh;
		MeshRenderer meshRenderer = model.AddComponent<MeshRenderer>();
//TODO figure out how to add proper material
//		Material material = Resources.Load("Group001Mat", typeof(Material)) as Material;
//		meshRenderer.material = material;

		yield return model;
	}
}