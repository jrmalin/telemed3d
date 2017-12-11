using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

#if  WINDOWS_UWP
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.UI.ViewManagement;
#endif
public class SaveAndLoadAnnotate : MonoBehaviour {

#if WINDOWS_UWP
    StorageFile file = null;
    bool writing = false;
#endif
    public AnnotateLayer AL;
    string output;
    public Text didSave;
    public GameObject line;

	// Use this for initialization
	void Awake () {
        line = Resources.Load("Line") as GameObject;
 
	}
	
	// Update is called once per frame
	void Update () {
        #if WINDOWS_UWP
        if( file != null && !writing)
        {
            writing = true;
            writeFile(output);
        }
        #endif
    }
#if WINDOWS_UWP
    async void writeFile(string text)
    {
        didSave.color = Color.black;
        didSave.text = "Saving ...";
        print("in write file");
        CachedFileManager.DeferUpdates(file);
        print("after cashe");
        await FileIO.WriteTextAsync(file, text);
        print("after await1?");
        FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
        print("after await2?");
        file = null;
        writing = false;
        if (status == FileUpdateStatus.Complete)
        {
            didSave.text = "Saved";
            didSave.color = Color.green;
        }
        else
        {
            didSave.text = "Save Failed";
            didSave.color = Color.red;
        }
    }
#endif
    public void LoadAnnotate(string filename)
    {

        AL = FindObjectOfType<AnnotateLayer>();
        line = Resources.Load("Line") as GameObject;
#if !WINDOWS_UWP
        string input = System.IO.File.ReadAllText(Application.dataPath + "\\Resources\\modelstuff.txt");
#else
        StreamReader stream = File.OpenText(filename);
        string input = stream.ReadToEnd();
#endif
        AL.lines = new List<SingleLine>();
        while (input.Length >= 2)
        {

            string temp = input.Substring(0, input.IndexOf("\n"));
            input = input.Substring(input.IndexOf("\n") + 1);

            if (temp.Contains("LINE"))
            {

                GameObject L = Instantiate(line, AL.transform);
                AL.lines.Add(L.GetComponent<SingleLine>());
                AL.lines[AL.lines.Count - 1].myLR.positionCount = 0;


            }else if (temp.Contains("green"))
            {
                AL.lines[AL.lines.Count-1].myLR.material = Resources.Load("green", typeof(Material)) as Material;
            }
            else if (temp.Contains("blue"))
            {
                AL.lines[AL.lines.Count - 1].myLR.material = Resources.Load("blue", typeof(Material)) as Material;
            }
            else if (temp.Contains("red"))
            {
                AL.lines[AL.lines.Count - 1].myLR.material = Resources.Load("red", typeof(Material)) as Material;
            }
            else if (temp.Contains("white"))
            {
                AL.lines[AL.lines.Count - 1].myLR.material = Resources.Load("white", typeof(Material)) as Material;
            }
            else if (temp.Contains("black"))
            {
                AL.lines[AL.lines.Count - 1].myLR.material = Resources.Load("black", typeof(Material)) as Material;
            }
            else if (temp.Contains("NEXT"))
            {
                continue;
            }
            else
            {
                float x, y, z;
                AL.lines[AL.lines.Count - 1].myLR.positionCount++;

                x = (float)System.Convert.ToDecimal(temp.Substring(0, temp.IndexOf(" ")));

                temp = temp.Substring(temp.IndexOf(" ") + 1 );

                y = (float)System.Convert.ToDecimal(temp.Substring(0, temp.IndexOf(" ")));

                temp = temp.Substring(temp.IndexOf(" ") + 1);

                z = (float)System.Convert.ToDecimal(temp);

                int index = AL.lines[AL.lines.Count - 1].myLR.positionCount - 1;
                AL.lines[AL.lines.Count - 1].myLR.SetPosition(index, new Vector3(x, y, z));

            }


        }

    }

    public void SaveAnnotate()
    {
        AL = FindObjectOfType<AnnotateLayer>();
        output = "";
        didSave.text = "";
        for(int i = 0; i < AL.lines.Count; ++i)
        {
            output += ("LINE " + i + "\n");

            output += (AL.lines[i].myLR.materials[0].name + "\n");

            for(int j = 0; j < AL.lines[i].myLR.positionCount; ++j)
            {
                output += AL.lines[i].myLR.GetPosition(j).x;
                output += " ";
                output += AL.lines[i].myLR.GetPosition(j).y;
                output += " ";
                output += AL.lines[i].myLR.GetPosition(j).z;
                output += "\n";
            }
            output += "NEXT\n";
        }

        //System.IO.File.WriteAllText(Application.dataPath, output);
#if WINDOWS_UWP

            UnityEngine.WSA.Application.InvokeOnUIThread(async () =>
            {
                FileSavePicker savePicker = new FileSavePicker();
                savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                // Dropdown of file types the user can save the file as
                savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });
                // Default file name if the user does not type one in or select a file to replace
                savePicker.SuggestedFileName = "Patient Model Annotation";
                file = await savePicker.PickSaveFileAsync();
            }, true);
#else
                System.IO.File.WriteAllText(Application.dataPath + "/Resources" + "/modelstuff.txt", output);

#endif

    }

}
