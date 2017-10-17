using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SFB;
using System;
#if  WINDOWS_UWP
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.ViewManagement;
#endif
using System.IO;

public class OnTap : MonoBehaviour
{
    StorageFile file = null;
    bool loaded = false;
    //public GameObject TestPrefab;
    //Text debug;

    // Use this for initialization
    void Start()
        {
             print("All good");
        }

        // Update is called once per frame
        void Update()
        {
            if (file != null && !loaded)
            {
                loaded = true;
                print("file picked");
                print(file.Path); //C:\Data\Users\telem\AppData\Local\Packages\microsoft.microsoftskydrive_8wekyb3d8bbwe\LocalState\OpenFile\model_mesh.obj
                                  //C:\Data\Users\telem\AppData\Local\Packages\microsoft.microsoftskydrive_8wekyb3d8bbwe\LocalState\OpenFile\model_mesh.obj
                                  //Texture2D pic = TextureLoader.LoadTexture(file.Path);
                //StartCoroutine(ImportObject(Path.GetFileNameWithoutExtension(file.Path)));
                GameObject model = OBJLoader.LoadOBJFile(file.Path);
                Debug.Log("made model");
                model.transform.position = new Vector3(0, -10, 20);
                model.transform.rotation = Quaternion.identity;
                model.transform.Rotate(-90, 90, 0);

            print("done");
                //Instantiate(TestPrefab);
                //Renderer renderer = pic.GetComponent<Renderer>();
                //renderer.material.mainTexture = await file.OpenAsync(FileAccessMode.Read); ;
            }
        }
        #if WINDOWS_UWP

        void OnSelect()
        {

            UnityEngine.WSA.Application.InvokeOnUIThread(async () =>
            {
                FileOpenPicker openPicker = new FileOpenPicker();
                // debug_statement = "created a FileOpenPicker";
                openPicker.ViewMode = PickerViewMode.Thumbnail;
                openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                openPicker.FileTypeFilter.Add("*");
                print(openPicker.SuggestedStartLocation);
                file = await openPicker.PickSingleFileAsync();
            }, true); //was false before, trying it as true 
        }
        #else
        void OnMouseOver()
        {
                if (Input.GetMouseButtonDown(0))
                {
                    var extensions = new[] {
                    new ExtensionFilter("3D Model Files", "obj")
                    };
                    StandaloneFileBrowser.OpenFilePanelAsync("Open File", "", extensions, false, (string[] paths) =>
                    {
                        if (paths.Length != 0 && paths[0].Length != 0)
                        {
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
        #endif

        IEnumerator ImportObject(String path)
         {
                print("importing object");
                ObjImporter importer = new ObjImporter();
                Mesh mesh = importer.ImportFile(path);

                GameObject model = new GameObject();
                model.transform.position = new Vector3(0, -10, 20);
                model.transform.rotation = Quaternion.identity;
                model.transform.Rotate(-90, 90, 0);
                MeshFilter meshFilter = model.AddComponent<MeshFilter>();
                meshFilter.sharedMesh = mesh;
                MeshRenderer meshRenderer = model.AddComponent<MeshRenderer>();
                //TODO figure out how to add proper material
                //		Material material = Resources.Load("Group001Mat", typeof(Material)) as Material;
                //		meshRenderer.material = material;
                print("finished importing");
                yield return model;
     
        }
}
