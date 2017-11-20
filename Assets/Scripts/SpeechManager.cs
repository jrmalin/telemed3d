using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.EventSystems;

public class SpeechManager : MonoBehaviour
{
	KeywordRecognizer keywordRecognizer = null;
	Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

	// Use this for initialization
	void Start()
	{
        keywords.Add("Stop Model", () =>
        {
            this.BroadcastMessage("Stop");
        });
        keywords.Add("Reset Model", () =>
			{
				this.BroadcastMessage("Reset");
			});
		keywords.Add("Move Left", () =>
			{
				this.BroadcastMessage("Move", Manipulator.MoveDirection.Left);
			});
		keywords.Add("Move Right", () =>
			{
				this.BroadcastMessage("Move", Manipulator.MoveDirection.Right);
			});
		keywords.Add("Move Up", () =>
			{
				this.BroadcastMessage("Move", Manipulator.MoveDirection.Up);
			});
		keywords.Add("Move Down", () =>
			{
				this.BroadcastMessage("Move", Manipulator.MoveDirection.Down);
			});
		keywords.Add("Move Forward", () =>
			{
				this.BroadcastMessage("Move", Manipulator.MoveDirection.Forward);
			});
		keywords.Add("Move Backward", () =>
			{
				this.BroadcastMessage("Move", Manipulator.MoveDirection.Backward);
			});
		keywords.Add("Rotate Left", () =>
			{
				this.BroadcastMessage("Rotate", Manipulator.RotationDirection.Left);
			});
		keywords.Add("Rotate Right", () =>
			{
				this.BroadcastMessage("Rotate", Manipulator.RotationDirection.Right);
			});
		keywords.Add("Rotate Up", () =>
			{
				this.BroadcastMessage("Rotate", Manipulator.RotationDirection.Up);
			});
		keywords.Add("Rotate Down", () =>
			{
				this.BroadcastMessage("Rotate", Manipulator.RotationDirection.Down);
			});
		keywords.Add("Scale Larger", () =>
			{
				this.BroadcastMessage("Scale", Manipulator.ScaleSize.Larger);
			});
		keywords.Add("Scale Smaller", () =>
			{
				this.BroadcastMessage("Scale", Manipulator.ScaleSize.Smaller);
			});
		keywords.Add("Update Faster", () =>
			{
				this.BroadcastMessage("UpdateSpeed", true);
			});
		keywords.Add("Update Slower", () =>
			{
				this.BroadcastMessage("UpdateSpeed", false);
			});


		// Tell the KeywordRecognizer about our keywords.
		keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

		// Register a callback for the KeywordRecognizer and start recognizing!
		keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
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