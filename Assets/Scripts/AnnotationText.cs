using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class AnnotationText : MonoBehaviour
{
    [Tooltip("A text area for the recognizer to display the recognized strings.")]
    public Text DictationDisplay;
    public Text Saved;
    public GameObject Start;
    private DictationRecognizer dictationRecognizer;

    // Use this string to cache the text currently displayed in the text box.
    private StringBuilder textSoFar;

    // Using an empty string specifies the default microphone. 
    private static string deviceName = string.Empty;
    private int samplingRate;
    private const int messageLength = 10;
    private AudioClip audio;

    // Use this to reset the UI once the Microphone is done recording after it was started.
    private bool hasRecordingStarted;

    void Awake()
    {
        Debug.Log("anntext woke up");
        /* TODO: DEVELOPER CODING EXERCISE 3.a */

        // 3.a: Create a new DictationRecognizer and assign it to dictationRecognizer variable.
        dictationRecognizer = new DictationRecognizer();

        // 3.a: Register for dictationRecognizer.DictationHypothesis and implement DictationHypothesis below
        // This event is fired while the user is talking. As the recognizer listens, it provides text of what it's heard so far.
        dictationRecognizer.DictationHypothesis += DictationRecognizer_DictationHypothesis;

        // 3.a: Register for dictationRecognizer.DictationResult and implement DictationResult below
        // This event is fired after the user pauses, typically at the end of a sentence. The full recognized string is returned here.
        dictationRecognizer.DictationResult += DictationRecognizer_DictationResult;

        // 3.a: Register for dictationRecognizer.DictationComplete and implement DictationComplete below
        // This event is fired when the recognizer stops, whether from Stop() being called, a timeout occurring, or some other error.
        dictationRecognizer.DictationComplete += DictationRecognizer_DictationComplete;

        // 3.a: Register for dictationRecognizer.DictationError and implement DictationError below
        // This event is fired when an error occurs.
        dictationRecognizer.DictationError += DictationRecognizer_DictationError;

        // Query the maximum frequency of the default microphone. Use 'unused' to ignore the minimum frequency.
        int unused;
        Microphone.GetDeviceCaps(deviceName, out unused, out samplingRate);

        // Use this string to cache the text currently displayed in the text box.
        textSoFar = new StringBuilder();

        // Use this to reset the UI once the Microphone is done recording after it was started.
        hasRecordingStarted = false;
    }

    void Update()
    {
        // 3.a: Add condition to check if dictationRecognizer.Status is Running
        if (hasRecordingStarted && !Microphone.IsRecording(deviceName) && dictationRecognizer.Status == SpeechSystemStatus.Running)
        {
            // Reset the flag now that we're cleaning up the UI.
            hasRecordingStarted = false;

            // This acts like pressing the Stop button and sends the message to the Communicator.
            // If the microphone stops as a result of timing out, make sure to manually stop the dictation recognizer.
            // Look at the StopRecording function.
            SendMessage("RecordStop");
        }
    }

    /// <summary>
    /// Turns on the dictation recognizer and begins recording audio from the default microphone.
    /// </summary>
    /// <returns>The audio clip recorded from the microphone.</returns>
    public void StartRecording()
    {
        Debug.Log("started recording");
        // 3.a Shutdown the PhraseRecognitionSystem. This controls the KeywordRecognizers
        PhraseRecognitionSystem.Shutdown();

        //Change color of start button
        ColorBlock startColor = Start.GetComponent<Button>().colors;
        startColor.normalColor = Color.cyan;
        Start.GetComponent<Button>().colors = startColor;
        Start.GetComponent<Button>().image.color = Color.cyan;

        // 3.a: Start dictationRecognizer
        dictationRecognizer.Start();
        Saved.text = "Not Saved";
        // 3.a Uncomment this line
        //DictationDisplay.text = "Dictation is starting. It may take time to display your text the first time, but begin speaking now...";

        // Set the flag that we've started recording.
        hasRecordingStarted = true;

        // Start recording from the microphone for 10 seconds.
        audio = Microphone.Start(deviceName, false, messageLength, samplingRate);
    }
    public void Back()
    {
        if (dictationRecognizer.Status == SpeechSystemStatus.Running)
        {
            dictationRecognizer.Stop();
        }
        Microphone.End(deviceName);


        //Change color of start button
        ColorBlock startColor = Start.GetComponent<Button>().colors;
        startColor.normalColor = Color.white;
        Start.GetComponent<Button>().colors = startColor;
        Start.GetComponent<Button>().image.color = Color.white;


        if (PhraseRecognitionSystem.Status != SpeechSystemStatus.Running)
        {
            PhraseRecognitionSystem.Restart();
            PhraseRecognitionSystem.Restart();
        }

        int i;
        if(textSoFar.Length > 0)
        {
            bool foundSpace = false;
            for (i = textSoFar.Length - 2; i > -1; i--)
            {
                if (textSoFar.ToString()[i] == ' ')
                {
                    foundSpace = true;
                    break;
                }
            }
            if (foundSpace)
            {
                int wordSize = textSoFar.Length - i;
                textSoFar.Remove(i, wordSize);
                textSoFar.Append(' ');
            }
            else
            {
                textSoFar = new StringBuilder();
            }
            DictationDisplay.text = textSoFar.ToString();
        }


    }
    public void clear()
    {
        if(dictationRecognizer.Status == SpeechSystemStatus.Running)
        {
            dictationRecognizer.Stop();
        }
        Microphone.End(deviceName);

        //Change color of start button
        ColorBlock startColor = Start.GetComponent<Button>().colors;
        startColor.normalColor = Color.white;
        Start.GetComponent<Button>().colors = startColor;
        Start.GetComponent<Button>().image.color = Color.white;


        if (PhraseRecognitionSystem.Status != SpeechSystemStatus.Running)
        {
            PhraseRecognitionSystem.Restart();
            PhraseRecognitionSystem.Restart();
        }

        print("cleared textSoFar");
        DictationDisplay.text = "";
        textSoFar = new StringBuilder();
    }

    /// <summary>
    /// Ends the recording session.
    /// </summary>
    public void StopRecording()
    {
        Debug.Log("stopped recording");
        // 3.a: Check if dictationRecognizer.Status is Running and stop it if so
        print("dictiation status " + dictationRecognizer.Status.ToString());
        if(dictationRecognizer.Status == SpeechSystemStatus.Running)
        {
            dictationRecognizer.Stop();
        }
        Microphone.End(deviceName);

        //Change color of start button
        ColorBlock startColor = Start.GetComponent<Button>().colors;
        startColor.normalColor = Color.white;
        Start.GetComponent<Button>().colors = startColor;
        Start.GetComponent<Button>().image.color = Color.white;

        print(Microphone.IsRecording(deviceName).ToString());
        if(PhraseRecognitionSystem.Status != SpeechSystemStatus.Running)
        {
            PhraseRecognitionSystem.Restart();
            PhraseRecognitionSystem.Restart();
        }
        

    }

    /// <summary>
    /// This event is fired while the user is talking. As the recognizer listens, it provides text of what it's heard so far.
    /// </summary>
    /// <param name="text">The currently hypothesized recognition.</param>
    private void DictationRecognizer_DictationHypothesis(string text)
    {
        Debug.Log("hypotheses heard you and text so far is : " + textSoFar.ToString());
        // 3.a: Set DictationDisplay text to be textSoFar and new hypothesized text
        // We don't want to append to textSoFar yet, because the hypothesis may have changed on the next event
        DictationDisplay.text = textSoFar.ToString() + " " + text + "...";
    }

    /// <summary>
    /// This event is fired after the user pauses, typically at the end of a sentence. The full recognized string is returned here.
    /// </summary>
    /// <param name="text">The text that was heard by the recognizer.</param>
    /// <param name="confidence">A representation of how confident (rejected, low, medium, high) the recognizer is of this recognition.</param>
    private void DictationRecognizer_DictationResult(string text, ConfidenceLevel confidence)
    {
        // 3.a: Append textSoFar with latest text
        textSoFar.Append(text + ". ");

        // 3.a: Set DictationDisplay text to be textSoFar
        DictationDisplay.text = textSoFar.ToString();
    }

    /// <summary>
    /// This event is fired when the recognizer stops, whether from Stop() being called, a timeout occurring, or some other error.
    /// Typically, this will simply return "Complete". In this case, we check to see if the recognizer timed out.
    /// </summary>
    /// <param name="cause">An enumerated reason for the session completing.</param>
    private void DictationRecognizer_DictationComplete(DictationCompletionCause cause)
    {
        // If Timeout occurs, the user has been silent for too long.
        // With dictation, the default timeout after a recognition is 20 seconds.
        // The default timeout with initial silence is 5 seconds.
        if (cause == DictationCompletionCause.TimeoutExceeded)
        {
            Microphone.End(deviceName);

            DictationDisplay.text = "Dictation has timed out. Please press the record button again.";
            SendMessage("ResetAfterTimeout");
            //Change color of start button
            ColorBlock startColor = Start.GetComponent<Button>().colors;
            startColor.normalColor = Color.white;
            Start.GetComponent<Button>().colors = startColor;
            Start.GetComponent<Button>().image.color = Color.white;
            PhraseRecognitionSystem.Restart();
            PhraseRecognitionSystem.Restart();

        }
    }

    /// <summary>
    /// This event is fired when an error occurs.
    /// </summary>
    /// <param name="error">The string representation of the error reason.</param>
    /// <param name="hresult">The int representation of the hresult.</param>
    private void DictationRecognizer_DictationError(string error, int hresult)
    {
        // 3.a: Set DictationDisplay text to be the error string
        DictationDisplay.text = error + "\nHRESULT: " + hresult;
    }

   /* private IEnumerator RestartSpeechSystem(KeywordManager keywordToStart)
    {
        while (dictationRecognizer != null && dictationRecognizer.Status == SpeechSystemStatus.Running)
        {
            yield return null;
        }

        keywordToStart.StartKeywordRecognizer();
    } */
}