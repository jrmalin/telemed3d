using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.IO;

#if  WINDOWS_UWP
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.ViewManagement;
#endif

public class OnTap : MonoBehaviour {

	public bool fileLoaded;
	public SpawnPoint sp;
	#if  WINDOWS_UWP
	StorageFile file = null;
	#endif
	bool loaded = false;

	// Use this for initialization
	void Start() {
		sp = FindObjectOfType<SpawnPoint> ();
		fileLoaded = false;
	}

	// Update is called once per frame
	void Update () {
#if WINDOWS_UWP
        if (file != null && !loaded)
        {
            loaded = true;
            print("file picked");
            print(file.Path); //C:\Data\Users\telem\AppData\Local\Packages\microsoft.microsoftskydrive_8wekyb3d8bbwe\LocalState\OpenFile\model_mesh.obj

            //C:\Data\Users\telem\AppData\Local\Packages\microsoft.microsoftskydrive_8wekyb3d8bbwe\LocalState\OpenFile\model_mesh.obj
            //Texture2D pic = TextureLoader.LoadTexture(file.Path);
            //StartCoroutine(ImportObject(Path.GetFileNameWithoutExtension(file.Path)));
            /*GameObject model = OBJLoader.LoadOBJFile(file.Path);
            Debug.Log("made model");
            model.transform.position = new Vector3(0, -10, 20);
            model.transform.rotation = Quaternion.identity;
            model.transform.Rotate(-90, 90, 0); */

            print("done");
			//Instantiate(TestPrefab);
			//Renderer renderer = pic.GetComponent<Renderer>();
			//renderer.material.mainTexture = await file.OpenAsync(FileAccessMode.Read); ;
		}
		#endif
	}

	public void OnMouseOver() {
#if WINDOWS_UWP
        
        if(!this.fileLoaded){
			StartCoroutine(ImportObject (null));
            print("in if statement");
            Text text = GetComponentInChildren<Text>();
			text.text = "Reset Model";
		}
		else if(this.fileLoaded) {
			print ("Rotating object back to normal");
			Manipulator man = FindObjectOfType<Manipulator> ();
			man.transform.localRotation = new Quaternion (0, 180, 0, 0);
			man.transform.localScale = new Vector3 (1, 1, 1);
		}
        /*UnityEngine.WSA.Application.InvokeOnUIThread(async () =>
		{
			FileOpenPicker openPicker = new FileOpenPicker();
			// debug_statement = "created a FileOpenPicker";
			openPicker.ViewMode = PickerViewMode.Thumbnail;
			openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
			openPicker.FileTypeFilter.Add("*");
			print(openPicker.SuggestedStartLocation);
			file = await openPicker.PickSingleFileAsync();
		}, true); //was false before, trying it as true */
#else
        print("in onMouseOver");
		if( !this.fileLoaded){
			StartCoroutine(ImportObject (null));
            print("in if statement");
            Text text = GetComponentInChildren<Text>();
			text.text = "Reset Model";
		}
		else if(this.fileLoaded) {
			print ("Rotating object back to normal");
			Manipulator man = FindObjectOfType<Manipulator> ();
			man.transform.localRotation = new Quaternion (0, 180, 0, 0);
			man.transform.localScale = new Vector3 (1, 1, 1);
		}
#endif
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
		Renderer renderer = model.GetComponent <Renderer> ();

		//TODO figure out how to add proper material
		Texture2D texture = Resources.Load ("model_texture", typeof(Texture2D)) as Texture2D;
		renderer.material.mainTexture = texture;

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

		model.transform.position -= center;


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

		this.fileLoaded = true;
		yield return model;
	}
}