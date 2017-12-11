using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.EventSystems;

public class SpeechManager : MonoBehaviour
{
    public Manipulator man;
	KeywordRecognizer keywordRecognizer = null;
	Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

	// Use this for initialization
	void Start()
	{
        keywords.Add("Stop Model", () =>
        {
            if (man.currentChangeType != Manipulator.ChangeType.Annotate)
            {
                this.BroadcastMessage("Stop");
            }
        });
        keywords.Add("Stop Moving", () =>
        {
            if (man.currentChangeType != Manipulator.ChangeType.Annotate)
            {
                this.BroadcastMessage("Stop");
            }
        });
        keywords.Add("Stop Rotating", () =>
        {
            if (man.currentChangeType != Manipulator.ChangeType.Annotate)
            {
                this.BroadcastMessage("Stop");
            }
        });
        keywords.Add("Stop Scaling", () =>
        {
            if (man.currentChangeType != Manipulator.ChangeType.Annotate)
            {
                this.BroadcastMessage("Stop");
            }
        });
        keywords.Add("Reset Model", () =>
			{
                this.BroadcastMessage("Reset");
                AnnotateLayer annotate = (AnnotateLayer)FindObjectOfType(typeof(AnnotateLayer));
                annotate.Clear();
                Selection select = (Selection)FindObjectOfType(typeof(Selection));
                select.select(1);
            });
		keywords.Add("Move Left", () =>
			{
                if (man.currentChangeType != Manipulator.ChangeType.Annotate)
                {
                    this.BroadcastMessage("Move", Manipulator.MoveDirection.Left);
                }
			});
		keywords.Add("Move Right", () =>
			{
                if (man.currentChangeType != Manipulator.ChangeType.Annotate)
                {
                    this.BroadcastMessage("Move", Manipulator.MoveDirection.Right);
                }
			});
		keywords.Add("Move Up", () =>
			{
                if (man.currentChangeType != Manipulator.ChangeType.Annotate)
                {
                    this.BroadcastMessage("Move", Manipulator.MoveDirection.Up);
                }
			});
		keywords.Add("Move Down", () =>
			{
                if (man.currentChangeType != Manipulator.ChangeType.Annotate)
                {
                    this.BroadcastMessage("Move", Manipulator.MoveDirection.Down);
                }
			});
		keywords.Add("Move Forward", () =>
			{
                if (man.currentChangeType != Manipulator.ChangeType.Annotate)
                {
                    this.BroadcastMessage("Move", Manipulator.MoveDirection.Forward);
                }
			});
		keywords.Add("Move Backward", () =>
			{
                if (man.currentChangeType != Manipulator.ChangeType.Annotate)
                {
                    this.BroadcastMessage("Move", Manipulator.MoveDirection.Backward);
                }
			});
		keywords.Add("Rotate Left", () =>
			{
                if (man.currentChangeType != Manipulator.ChangeType.Annotate)
                {
                    this.BroadcastMessage("Rotate", Manipulator.RotationDirection.Left);
                }
			});
		keywords.Add("Rotate Right", () =>
        {
            if (man.currentChangeType != Manipulator.ChangeType.Annotate)
            {
                this.BroadcastMessage("Rotate", Manipulator.RotationDirection.Right);
            }
			});
		keywords.Add("Rotate Up", () =>
        {
            if (man.currentChangeType != Manipulator.ChangeType.Annotate)
            {
                this.BroadcastMessage("Rotate", Manipulator.RotationDirection.Up);
            }
			});
		keywords.Add("Rotate Down", () =>
			{
                if (man.currentChangeType != Manipulator.ChangeType.Annotate)
                {
                    this.BroadcastMessage("Rotate", Manipulator.RotationDirection.Down);
                }
			});
		keywords.Add("Scale Larger", () =>
        {
            if (man.currentChangeType != Manipulator.ChangeType.Annotate)
            {
                this.BroadcastMessage("Scale", Manipulator.ScaleSize.Larger);
            }
			});
		keywords.Add("Scale Smaller", () =>
			{
                if (man.currentChangeType != Manipulator.ChangeType.Annotate)
                {
                    this.BroadcastMessage("Scale", Manipulator.ScaleSize.Smaller);
                }
			});
		keywords.Add("Update Faster", () =>
			{
                if (man.currentChangeType != Manipulator.ChangeType.Annotate)
                {
                    this.BroadcastMessage("UpdateSpeed", true);
                }
			});
		keywords.Add("Update Slower", () =>
			{
                if (man.currentChangeType != Manipulator.ChangeType.Annotate)
                {
                    this.BroadcastMessage("UpdateSpeed", false);
                }
			});
        keywords.Add("Clear Annotations", () =>
        {
            if (man.currentChangeType == Manipulator.ChangeType.Annotate)
            {
                AnnotateLayer annotate = (AnnotateLayer)FindObjectOfType(typeof(AnnotateLayer));
                annotate.Clear();
            }
        });
        keywords.Add("Undo Annotation", () =>
        {
            if (man.currentChangeType == Manipulator.ChangeType.Annotate)
            {
                AnnotateLayer annotate = (AnnotateLayer)FindObjectOfType(typeof(AnnotateLayer));
                annotate.Undo();
            }
        });
        keywords.Add("Change Color To Black", () =>
        {
            if (man.currentChangeType == Manipulator.ChangeType.Annotate)
            {
                AnnotateLayer annotate = (AnnotateLayer)FindObjectOfType(typeof(AnnotateLayer));
                annotate.changePenColor(AnnotateLayer.MatColor.BLACK);
            }
        });
        keywords.Add("Change Color To Blue", () =>
        {
            if (man.currentChangeType == Manipulator.ChangeType.Annotate)
            {
                AnnotateLayer annotate = (AnnotateLayer)FindObjectOfType(typeof(AnnotateLayer));
                annotate.changePenColor(AnnotateLayer.MatColor.BLUE);
            }
        });
        keywords.Add("Change Color To Green", () =>
        {
            if (man.currentChangeType == Manipulator.ChangeType.Annotate)
            {
                AnnotateLayer annotate = (AnnotateLayer)FindObjectOfType(typeof(AnnotateLayer));
                annotate.changePenColor(AnnotateLayer.MatColor.GREEN);
            }
        });
        keywords.Add("Change Color To Red", () =>
        {
            print("in voice chaning color");
            if (man.currentChangeType == Manipulator.ChangeType.Annotate)
            {
                print("changing color to red");
                AnnotateLayer annotate = (AnnotateLayer)FindObjectOfType(typeof(AnnotateLayer));
                annotate.changePenColor(AnnotateLayer.MatColor.RED);
            }
        });
        keywords.Add("Change Color To White", () =>
        {
            if (man.currentChangeType == Manipulator.ChangeType.Annotate)
            {
                AnnotateLayer annotate = (AnnotateLayer)FindObjectOfType(typeof(AnnotateLayer));
                annotate.changePenColor( AnnotateLayer.MatColor.WHITE);
            }
        });
        keywords.Add("Start Annotating", () =>
        {
            Selection select = (Selection)FindObjectOfType(typeof(Selection));
            select.select(0);
            //GazeGestureManager.Instance.Transition("AnnotationRecognizer");
        });
        keywords.Add("Start Manipulating", () =>
        {
            Selection select = (Selection)FindObjectOfType(typeof(Selection));
            select.select(1);
            //GazeGestureManager.Instance.Transition("NavigationRecognizer");
        });


        // Tell the KeywordRecognizer about our keywords.
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

		// Register a callback for the KeywordRecognizer and start recognizing!
		keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
		keywordRecognizer.Start();
	}
    public void restartKeyword()
    {
        keywordRecognizer.Start();
    }
	private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
	{
		System.Action keywordAction;
		if (keywords.TryGetValue(args.text, out keywordAction))
		{
			keywordAction.Invoke();
		}
	}
}