using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System;
using System.IO;

#if  WINDOWS_UWP
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.UI.ViewManagement;
#endif

public class Selection : MonoBehaviour {
    public Manipulator man;
    public GameObject model;
    public GameObject tv;
    public GameObject annotationUI;
    public GameObject annAssistant;
    public Text annotationText;
    public Text didSave;
    public GameObject mainUI;

#if WINDOWS_UWP
    StorageFile file = null;
    bool writing = false;
#endif
    int option;
    //options
    int annotate = 0;
    int manipulate = 1;
    int tutorial = 3;
    int save = 4;
    int video = 5;
    int readme = 6;
    int play_video = 7;
    int replay_video = 8;
    int closeAnnUI = 9;
    int saveAnnText = 10;
    int moveAnnText = 11;
    int moveTv = 12;
    int moveMainUI = 13;

	// Use this for initialization
	void Start () {
        Debug.Log("init of selection script");
    }
	
	// Update is called once per frame
	void Update () {
        if (GazeGestureManager.Instance.isAnnotating)
        {
            Text text = mainUI.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.Find("SavedResponse").gameObject.GetComponent<Text>();
            text.text = "Not Saved";
            text.color = Color.black;
        }
        #if WINDOWS_UWP
        if( file != null && !writing)
        {
            writing = true;
            writeFile(annotationText);
        }
        #endif

    }
#if WINDOWS_UWP
    async void writeFile(Text text)
    {
        didSave.text = "Saving ...";
        print("in write file");
        CachedFileManager.DeferUpdates(file);
        print("after cashe");
        await FileIO.WriteTextAsync(file, text.text.ToString());
        print("after await1?");
        FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
        print("after await2?");
        file = null;
        writing = false;
        if (status == FileUpdateStatus.Complete)
        {
            didSave.text = "Saved";
        }
        else
        {
            didSave.text = "Save Failed";
        }
    }
#endif
    void reposition(bool toMove)
    {
        GameObject annOptionsBar = annotationUI.transform.Find("right").gameObject;
        GameObject startButton = annOptionsBar.transform.GetChild(0).gameObject.transform.Find("Start").gameObject;
        GameObject stopButton = annOptionsBar.transform.GetChild(0).gameObject.transform.Find("Stop").gameObject;
        GameObject saveButton = annOptionsBar.transform.GetChild(0).gameObject.transform.Find("Save").gameObject;
        GameObject backButton = annOptionsBar.transform.GetChild(0).gameObject.transform.Find("Back").gameObject;
        GameObject moveButton = annOptionsBar.transform.GetChild(0).gameObject.transform.Find("Move").gameObject;
        Text moveText = moveButton.GetComponentInChildren<Text>();

        if (toMove)
        {
            moveText.text = "Place";
            annotationUI.GetComponent<AnnotationMoveAction>().isMoving = true;
            startButton.SetActive(false);
            stopButton.SetActive(false);
            saveButton.SetActive(false);
            backButton.SetActive(false);

        }
        else
        {
            moveText.text = "Reposition";
            annotationUI.GetComponent<AnnotationMoveAction>().isMoving = false;
            startButton.SetActive(true);
            stopButton.SetActive(true);
            saveButton.SetActive(true);
            backButton.SetActive(true);
        }
    }
    public void select(int command)
    {
        option = command;
        if(command == annotate)
        {
            man.currentChangeType = Manipulator.ChangeType.Annotate;
            man.transform.gameObject.SetActive(false);
            annAssistant.SetActive(true);
            annAssistant.transform.localPosition = new Vector3(-2, -3 / 4, -2);
            GazeGestureManager.Instance.Transition("AnnotationRecognizer");
        }
        else if( command == manipulate)
        {
            if(man.currentChangeType == Manipulator.ChangeType.Annotate)
            {
                annAssistant.SetActive(false);
                annotationUI.SetActive(true);
                annotationUI.transform.localPosition = new Vector3(-1, -3/4, -2);
                reposition(false);
            }
            man.currentChangeType = Manipulator.ChangeType.None;
            man.transform.gameObject.SetActive(true);
            GazeGestureManager.Instance.Transition("NavigationRecognizer");
        }
        else if( command == tutorial)
        {
            tv.SetActive(!tv.active);
            print("tv active :" + tv.active);

            if (tv.active)
            {
                
                GameObject screen = tv.transform.Find("TV Screen").gameObject;
                screen.transform.Find("Video Tutorial").gameObject.SetActive(false);
                screen.transform.Find("Text Tutorial").gameObject.SetActive(false);
                screen.GetComponent<BoxCollider>().enabled = true;

                GameObject bottomBar = tv.transform.Find("bottom bar").gameObject;
                GameObject tvPannel = bottomBar.transform.Find("TV Pannel").gameObject;

                tvPannel.transform.Find("Play and Pause").gameObject.SetActive(false);
                tvPannel.transform.Find("Replay").gameObject.SetActive(false);
                
                Text text = GetComponentInChildren<Text>();
                text.text = "Exit Tutorial";
            }
            else
            {
                Text text = GetComponentInChildren<Text>();
                text.text = "Tutorial";
            }
        }
        else if( command == readme)
        {
            GameObject screen = tv.transform.Find("TV Screen").gameObject;
            GameObject show_readme = screen.transform.Find("Text Tutorial").gameObject;
            screen.GetComponent<BoxCollider>().enabled = true;

            show_readme.SetActive(true);
            screen.transform.Find("Video Tutorial").gameObject.SetActive(false);

            GameObject bottomBar = tv.transform.Find("bottom bar").gameObject;
            GameObject tvPannel = bottomBar.transform.Find("TV Pannel").gameObject;

            tvPannel.transform.Find("Play and Pause").gameObject.SetActive(false);
            tvPannel.transform.Find("Replay").gameObject.SetActive(false);

        }
        else if (command == video)
        {
            // show video

            GameObject screen = tv.transform.Find("TV Screen").gameObject;
            GameObject videoObject = screen.transform.Find("Video Tutorial").gameObject;

            if (!videoObject.active)
            {
                screen.GetComponent<BoxCollider>().enabled = false;
                screen.transform.Find("Text Tutorial").gameObject.SetActive(false);

                // show video
                screen.transform.Find("Video Tutorial").gameObject.SetActive(true);

                GameObject bottomBar = tv.transform.Find("bottom bar").gameObject;
                GameObject tvPannel = bottomBar.transform.Find("TV Pannel").gameObject;

                // show play and replay buttons 
                tvPannel.transform.Find("Play and Pause").gameObject.SetActive(true);
                tvPannel.transform.Find("Replay").gameObject.SetActive(true);

            }


        }
        else if( command == play_video)
        {
            GameObject screen = tv.transform.Find("TV Screen").gameObject;
            VideoPlayer video = screen.transform.Find("Video Tutorial").gameObject.GetComponent<VideoPlayer>();
            string buttonName = this.gameObject.name;
            Text text;
            if (buttonName == "Play and Pause")
            {
                text = GetComponentInChildren<Text>();
            }
            else
            {
                GameObject bottomBar = tv.transform.Find("bottom bar").gameObject;
                GameObject tvPannel = bottomBar.transform.Find("TV Pannel").gameObject;
                GameObject playButton = tvPannel.transform.Find("Play and Pause").gameObject;
                text = playButton.GetComponentInChildren<Text>();

            }

                if (video.isPlaying)
            {
                video.Pause();
                text.text = "Play";

            }
            else
            {
                video.Play();
                text.text = "Pause";
            }

        }
        else if( command == replay_video)
        {
            GameObject screen = tv.transform.Find("TV Screen").gameObject;
            VideoPlayer video = screen.transform.Find("Video Tutorial").gameObject.GetComponent<VideoPlayer>();
            video.Stop();
            select(play_video);
        }
        else if( command == closeAnnUI)
        {
            annotationUI.SetActive(false);
        }
        else if(command == saveAnnText)
        {
            #if WINDOWS_UWP

            UnityEngine.WSA.Application.InvokeOnUIThread(async () =>
            {
                FileSavePicker savePicker = new FileSavePicker();
                savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                // Dropdown of file types the user can save the file as
                savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });
                // Default file name if the user does not type one in or select a file to replace
                savePicker.SuggestedFileName = "Patient Annotation Notes";
                file = await savePicker.PickSaveFileAsync();
            }, true);

            #endif
        }
        else if(command == moveAnnText){
            GameObject annOptionsBar = annotationUI.transform.Find("right").gameObject;
            GameObject moveButton = annOptionsBar.transform.GetChild(0).gameObject.transform.Find("Move").gameObject;
            Text moveText = moveButton.GetComponentInChildren<Text>();

            if(moveText.text == "Reposition")
            {
                moveButton.GetComponent<Button>().image.color = Color.cyan;
                reposition(true);
            }
            else
            {
                moveButton.GetComponent<Button>().image.color = Color.white;
                reposition(false);
            }
        }
        else if(command == moveTv)
        {
            GameObject moveButton = tv.transform.Find("bottom bar").GetChild(0).gameObject.transform.Find("Move").gameObject;
            Text moveText = moveButton.GetComponentInChildren<Text>();
            if(moveText.text == "Move")
            {
                moveText.text = "Place";
                moveButton.GetComponent<Button>().image.color = Color.cyan;
                tv.GetComponent<AnnotationMoveAction>().isMoving = true;
            }
            else
            {
                moveText.text = "Move";
                moveButton.GetComponent<Button>().image.color = Color.white;
                tv.GetComponent<AnnotationMoveAction>().isMoving = false;
            }
        }
        else if(command == moveMainUI)
        {
            GameObject moveButton = mainUI.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.Find("Move").gameObject;
            Text moveText = moveButton.GetComponentInChildren<Text>();
            if (moveText.text == "Move UI")
            {
                moveText.text = "Place";
                moveButton.GetComponent<Button>().image.color = Color.cyan;
                mainUI.GetComponent<AnnotationMoveAction>().isMoving = true;
            }
            else
            {
                moveText.text = "Move UI";
                moveButton.GetComponent<Button>().image.color = Color.white;
                mainUI.GetComponent<AnnotationMoveAction>().isMoving = false;
            }
        }
    }
}
