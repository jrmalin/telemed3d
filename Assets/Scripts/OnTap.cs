﻿using System.Collections;
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

	public bool FileLoaded;
	public SpawnPoint sp;
	#if  WINDOWS_UWP
	IReadOnlyList<StorageFile> file = null;
	#endif
	bool loaded = false;
    public Manipulator man;

	// Use this for initialization
	void Start() {
        sp = FindObjectOfType<SpawnPoint> ();
		FileLoaded = false;
	}

	// Update is called once per frame
	void Update () {
#if WINDOWS_UWP
        if (file != null && !loaded)
        {
            loaded = true;
            print("file picked");
            string objFile = null;
            string annFile = null;
            string textureFile = null;
            int i;
            for( i = 0 ; i < file.Count; i++){
                if(Path.GetExtension(file[i].Path).ToLower() == ".jpg")
                {
                    textureFile = file[i].Path;
                }
                else if(Path.GetExtension(file[i].Path).ToLower() == ".txt")
                {
                    annFile = file[i].Path;
                }
                else
                {
                    objFile = file[i].Path;
                }
            }

            StartCoroutine(ImportObject(objFile, textureFile));

            if(file.Count == 3)
            {
                SaveAndLoadAnnotate saveFunction = new SaveAndLoadAnnotate();
                saveFunction.LoadAnnotate(annFile);
            }

            Text text = GetComponentInChildren<Text>();
            text.text = "Reset Model";

            GameObject parent = transform.parent.gameObject;
            parent.transform.Find("Manipulate Model").gameObject.SetActive(true);
            parent.transform.Find("Annotate Model").gameObject.SetActive(true);
            parent.transform.Find("Save").gameObject.SetActive(true);

            print("done");
			//Instantiate(TestPrefab);
			//Renderer renderer = pic.GetComponent<Renderer>();
			//renderer.material.mainTexture = await file.OpenAsync(FileAccessMode.Read); ;
		}
#endif
    }

    public void OnMouseOver() {
#if WINDOWS_UWP
        
        if(!this.FileLoaded){
            UnityEngine.WSA.Application.InvokeOnUIThread(async () =>
            {
                FileOpenPicker openPicker = new FileOpenPicker();
                // debug_statement = "created a FileOpenPicker";
                openPicker.ViewMode = PickerViewMode.Thumbnail;
                openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                openPicker.FileTypeFilter.Add("*");
                print(openPicker.SuggestedStartLocation);
                file = await openPicker.PickMultipleFilesAsync();
            }, true);
		}
		else if(this.FileLoaded) {
			print ("Rotating object back to normal");
            GazeGestureManager.Instance.ResetGestureRecognizers();
            man.Reset();
            AnnotateLayer annotate = (AnnotateLayer)FindObjectOfType(typeof(AnnotateLayer));
            annotate.Clear();
            man.transform.gameObject.SetActive(true);
		}

#else
        print("in onMouseOver");
		if( !FileLoaded){
			StartCoroutine(ImportObject (null,null));
            print("in if statement");
            Text text = GetComponentInChildren<Text>();
            text.text = "Reset Model";
            GameObject parent = transform.parent.gameObject;
            parent.transform.Find("Manipulate Model").gameObject.SetActive(true);
            parent.transform.Find("Annotate Model").gameObject.SetActive(true);
            parent.transform.Find("Save").gameObject.SetActive(true);
            SaveAndLoadAnnotate saveFunction = new SaveAndLoadAnnotate();
            saveFunction.LoadAnnotate("");
            //GetComponentInChildren<TextMesh>().text = "Reset Model";
		}
		else if(FileLoaded) {
			print ("Rotating object back to normal");
            //Manipulator man = FindObjectOfType<Manipulator>();
            if(man == null)
            {
                print("man is null");
            }
            man.transform.gameObject.SetActive(true);
            man.currentChangeType = Manipulator.ChangeType.None;
            man.transform.localRotation = new Quaternion (0, 180, 0, 0);
			man.transform.localScale = new Vector3 (1, 1, 1);

        }
#endif
    }

    IEnumerator ImportObject(String obj_path, String texture_path) {

        #if !WINDOWS_UWP
        ObjImporter imported = new ObjImporter();
        Mesh mesh = imported.ImportFile("Assets/Resources/model_mesh.obj") as Mesh;
        #else
        ObjImporter imported = new ObjImporter();
        Mesh mesh = imported.ImportFile(obj_path) as Mesh;
        #endif

        GameObject model = new GameObject();

		model.transform.position = sp.transform.position;
		model.transform.rotation = Quaternion.identity;
		model.transform.Rotate (0,0,0);

        MeshFilter meshFilter = model.AddComponent<MeshFilter>();               //ObjImporter
        meshFilter.sharedMesh = mesh;

        MeshCollider meshCollider = model.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;

        model.AddComponent<AnnotateLayer>();

        MeshRenderer meshRenderer = model.AddComponent<MeshRenderer>();
        Renderer renderer = model.GetComponent <Renderer> ();

#if WINDOWS_UWP
        Texture2D texture = TextureLoader.LoadTexture(texture_path) as Texture2D;
#else
        Texture2D texture = TextureLoader.LoadTexture("Assets/Resources/model_texture.jpg") as Texture2D;
#endif
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

		FileLoaded = true;
		yield return model;
	}
}