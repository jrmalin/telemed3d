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
	IReadOnlyList<StorageFile> file = null;
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
            if(Path.GetExtension(file[0].Path).ToLower() == ".jpg")
                StartCoroutine(ImportObject(file[1].Path, file[0].Path));
            else
               StartCoroutine(ImportObject(file[0].Path, file[1].Path));

            Text text = GetComponentInChildren<Text>();
            text.text = "Reset Model";

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
			StartCoroutine(ImportObject (null,null));
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

    IEnumerator ImportObject(String obj_path, String texture_path) {

        //ObjImporter imported = new ObjImporter();                                //ObjImporter
        //Mesh mesh = imported.ImportFile(obj_path);      
        //ObjImporter2 imported = new ObjImporter2();                                //ObjImporter2
        //Mesh mesh = imported.ImportFile(obj_path);
        #if !WINDOWS_UWP
        ObjImporter imported = new ObjImporter();
        Mesh mesh = imported.ImportFile("Assets/Resources/model_mesh.obj") as Mesh;
        #else
        ObjImporter imported = new ObjImporter();
        Mesh mesh = imported.ImportFile(obj_path) as Mesh;
        #endif
        //Mesh mesh = Resources.Load ("model_mesh", typeof(Mesh)) as Mesh;         //Hard Code

        //read mesh structure 
        /*StreamWriter stream = new StreamWriter("app_vectors.txt");
        stream.WriteLine("vectors");
        for( int i = 0; i < mesh.vertices.Length; i++)
        {
            string temp = i + " : " + mesh.vertices[i].ToString("F5");
            stream.WriteLine(temp);
        }
        stream.Dispose();
        StreamWriter stream2 = new StreamWriter("app_uv.txt");
        stream2.WriteLine("uv");
        for (int i = 0; i < mesh.uv.Length; i++)
        {
            string temp = i + " : " + mesh.uv[i].ToString("F5");
            stream2.WriteLine(temp);
        }
        stream2.Dispose();
        StreamWriter stream3 = new StreamWriter("app_triangles.txt");
        stream3.WriteLine("triangles");
        for (int i = 0; i < mesh.triangles.Length; i++)
        {
            string temp = i + " : " + mesh.triangles[i].ToString("F5");
            stream3.WriteLine(temp);
        }
        stream3.Dispose();
        StreamWriter stream4 = new StreamWriter("app_normals.txt");
        stream4.WriteLine("normals");
        for (int i = 0; i < mesh.normals.Length; i++)
        {
            string temp = i + " : " + mesh.normals[i].ToString("F5");
            stream4.WriteLine(temp);
        }
        stream4.Dispose();*/

        GameObject model = new GameObject();

		model.transform.position = sp.transform.position;
		model.transform.rotation = Quaternion.identity;
		model.transform.Rotate (0,0,0);

        MeshFilter meshFilter = model.AddComponent<MeshFilter>();               //ObjImporter
        meshFilter.sharedMesh = mesh;

        MeshRenderer meshRenderer = model.AddComponent<MeshRenderer>();
        Renderer renderer = model.GetComponent <Renderer> ();


        //TODO figure out how to add proper material
#if WINDOWS_UWP
        Texture2D texture = TextureLoader.LoadTexture(texture_path) as Texture2D;
#else
        Texture2D texture = TextureLoader.LoadTexture("Assets/Resources/model_texture.jpg") as Texture2D;
#endif
        //Texture2D texture = Resources.Load ("model_texture", typeof(Texture2D)) as Texture2D;
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