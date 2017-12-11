using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnnotationAssistant : MonoBehaviour {
    public GameObject model;
    public GameObject AnnAssUI;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Move()
    {
        GameObject commands = AnnAssUI.transform.GetChild(0).gameObject.transform.Find("Commands").gameObject;
        Text text = commands.transform.Find("Move").gameObject.GetComponentInChildren<Text>();
        Debug.Log(text.text);
        if(text.text == "Move")
        {
            text.text = "Place";
            commands.transform.Find("Move").GetComponent<Button>().image.color = Color.cyan;
            AnnAssUI.GetComponent<AnnotationMoveAction>().isMoving = true;
        }
        else
        {
            text.text = "Move";
            commands.transform.Find("Move").GetComponent<Button>().image.color = Color.white;
            AnnAssUI.GetComponent<AnnotationMoveAction>().isMoving = false;
        }
    }
    public void changeColor(int color)
    {

        if(color == 0)
        {
            getAnnotateLayer().changePenColor(AnnotateLayer.MatColor.RED);
        }
        else if( color == 1)
        {
            getAnnotateLayer().changePenColor(AnnotateLayer.MatColor.BLUE);
        }
        else if (color == 2)
        {
            getAnnotateLayer().changePenColor(AnnotateLayer.MatColor.GREEN);
        }
        else if (color == 3)
        {
            getAnnotateLayer().changePenColor(AnnotateLayer.MatColor.BLACK);
        }
        else if (color == 4)
        {
            getAnnotateLayer().changePenColor(AnnotateLayer.MatColor.WHITE);
        }
    }
    public void clear()
    {
        getAnnotateLayer().Clear();
    }
    public void undo()
    {
        getAnnotateLayer().Undo();
    }
    AnnotateLayer getAnnotateLayer()
    {
        return model.transform.Find("New Game Object").gameObject.GetComponent<AnnotateLayer>();
    }
}
