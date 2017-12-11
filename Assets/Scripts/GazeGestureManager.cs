using UnityEngine;
using UnityEngine.XR.WSA.Input;
using System.Collections.Generic;

public class GazeGestureManager : MonoBehaviour
{

    public GameObject inkBlot;
    public List<SingleLine> lines;
    public GameObject prefabLine;
    public Material currentMat;

    public enum MatColor { BLUE, RED, GREEN, BLACK, WHITE };
    //	#if WINDOWS_UWP
    public static GazeGestureManager Instance { get; private set; }

    // Tap and Navigation gesture recognizer.
    public GestureRecognizer NavigationRecognizer { get; private set; }

    // Manipulation gesture recognizer.
    public GestureRecognizer ManipulationRecognizer { get; private set; }

    // Annotate gesture recognizer
    public GestureRecognizer AnnotationRecognizer { get; private set; }

    // Currently active gesture recognizer.
    public GestureRecognizer ActiveRecognizer { get; private set; }

    public bool isAnnotating { get; private set; }

    public Vector3 AnnotationPosition { get; private set; }

    public Ray AnnotationRay { get; private set; }

    public bool IsNavigating { get; private set; }

    public Vector3 NavigationPosition { get; private set; }

    public bool IsManipulating { get; private set; }

    public Vector3 ManipulationPosition { get; private set; }

    // Represents the hologram that is currently being gazed at.
    public GameObject FocusedObject { get; private set; }

    void Awake()
    {
        lines = new List<SingleLine>();
        inkBlot = Resources.Load<GameObject>("Ink");
        prefabLine = Resources.Load<GameObject>("Line");
        currentMat = Resources.Load<Material>("black");

        Instance = this;
        NavigationRecognizer = new GestureRecognizer();

        NavigationRecognizer.SetRecognizableGestures(
            GestureSettings.Tap |
            GestureSettings.NavigationX  |
            GestureSettings.NavigationY);

        NavigationRecognizer.TappedEvent += NavigationRecognizer_TappedEvent;
        NavigationRecognizer.NavigationStartedEvent += NavigationRecognizer_NavigationStartedEvent;
        NavigationRecognizer.NavigationUpdatedEvent += NavigationRecognizer_NavigationUpdatedEvent;
        NavigationRecognizer.NavigationCompletedEvent += NavigationRecognizer_NavigationCompletedEvent;
        NavigationRecognizer.NavigationCanceledEvent += NavigationRecognizer_NavigationCanceledEvent;

        ManipulationRecognizer = new GestureRecognizer();

        ManipulationRecognizer.SetRecognizableGestures( GestureSettings.Tap |
            GestureSettings.ManipulationTranslate);

        ManipulationRecognizer.ManipulationStartedEvent += ManipulationRecognizer_ManipulationStartedEvent;
        ManipulationRecognizer.ManipulationUpdatedEvent += ManipulationRecognizer_ManipulationUpdatedEvent;
        ManipulationRecognizer.ManipulationCompletedEvent += ManipulationRecognizer_ManipulationCompletedEvent;
        ManipulationRecognizer.ManipulationCanceledEvent += ManipulationRecognizer_ManipulationCanceledEvent;

        AnnotationRecognizer = new GestureRecognizer();

        AnnotationRecognizer.SetRecognizableGestures(GestureSettings.Tap |
            GestureSettings.ManipulationTranslate);

        AnnotationRecognizer.ManipulationStartedEvent += Annotation_startEvent;
        AnnotationRecognizer.ManipulationUpdatedEvent += Annotation_upDateEvent;
        AnnotationRecognizer.ManipulationCompletedEvent += Annotation_EndEvent;
        AnnotationRecognizer.ManipulationCanceledEvent += Annotation_EndEvent;



        ResetGestureRecognizers();
    }

    void OnDestroy()
    {
        // 2.b: Unregister the Tapped and Navigation events on the NavigationRecognizer.
        NavigationRecognizer.TappedEvent -= NavigationRecognizer_TappedEvent;
        ManipulationRecognizer.TappedEvent -= ManipulationRecognizer_TappedEvent;

        NavigationRecognizer.NavigationStartedEvent -= NavigationRecognizer_NavigationStartedEvent;
        NavigationRecognizer.NavigationUpdatedEvent -= NavigationRecognizer_NavigationUpdatedEvent;
        NavigationRecognizer.NavigationCompletedEvent -= NavigationRecognizer_NavigationCompletedEvent;
        NavigationRecognizer.NavigationCanceledEvent -= NavigationRecognizer_NavigationCanceledEvent;

        // Unregister the Manipulation events on the ManipulationRecognizer.
        ManipulationRecognizer.ManipulationStartedEvent -= ManipulationRecognizer_ManipulationStartedEvent;
        ManipulationRecognizer.ManipulationUpdatedEvent -= ManipulationRecognizer_ManipulationUpdatedEvent;
        ManipulationRecognizer.ManipulationCompletedEvent -= ManipulationRecognizer_ManipulationCompletedEvent;
        ManipulationRecognizer.ManipulationCanceledEvent -= ManipulationRecognizer_ManipulationCanceledEvent;
    }

    /// <summary>
    /// Revert back to the default GestureRecognizer.
    /// </summary>
    public void ResetGestureRecognizers()
    {
        // Default to the navigation gestures.
        Transition("NavigationRecognizer");
    }

    /// <summary>
    /// Transition to a new GestureRecognizer.
    /// </summary>
    /// <param name="newRecognizer">The GestureRecognizer to transition to.</param>
    public void Transition(string newGesture)
    {
        GestureRecognizer newRecognizer = null;
        Debug.Log("recognized a new Gesture");
        if(newGesture == "NavigationRecognizer")
        {
            newRecognizer = NavigationRecognizer;
        }
        else if(newGesture == "ManipulationRecognizer")
        {
            newRecognizer = ManipulationRecognizer;
        }
        else if(newGesture == "AnnotationRecognizer")
        {
            newRecognizer = AnnotationRecognizer;
        }

        if (newRecognizer == null)
        {
            return;
        }

        if (ActiveRecognizer != null)
        {
            if (ActiveRecognizer == newRecognizer)
            {
                return;
            }
            Debug.Log("Changing gesture");
            ActiveRecognizer.CancelGestures();
            ActiveRecognizer.StopCapturingGestures();
        }

        newRecognizer.StartCapturingGestures();
        ActiveRecognizer = newRecognizer;
    }

    private void Annotation_startEvent(InteractionSourceKind source, Vector3 position, Ray ray)
    {
        Debug.Log("annotation focused object :" + FocusedObject.name);
        if (FocusedObject != null && FocusedObject.name == "New Game Object")
        {
            isAnnotating = true;
        }  
    }

    private void Annotation_upDateEvent(InteractionSourceKind source, Vector3 position, Ray ray)
    {
        Debug.Log("annotation focused object :" + FocusedObject.name);
        if (FocusedObject != null && FocusedObject.name == "New Game Object")
        {
            isAnnotating = true;
        }
    }

    private void Annotation_EndEvent(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
    {
        isAnnotating = false;
    }

    private void NavigationRecognizer_NavigationStartedEvent(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
    {
        print("navigation focused object name " + FocusedObject.name);
        if( FocusedObject.name == "Outer-Area")
        {
            // 2.b: Set IsNavigating to be true.
            IsNavigating = true;

            // 2.b: Set NavigationPosition to be relativePosition.
            NavigationPosition = relativePosition;
        }
    }


    private void NavigationRecognizer_NavigationUpdatedEvent(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
    {
        print("navigation focused object name " + FocusedObject.name);
        if (FocusedObject.name == "Outer-Area")
        {
            // 2.b: Set IsNavigating to be true.
            IsNavigating = true;

            // 2.b: Set NavigationPosition to be relativePosition.
            NavigationPosition = relativePosition;
        }

    }

    private void NavigationRecognizer_NavigationCompletedEvent(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
    {
        // 2.b: Set IsNavigating to be false.
        IsNavigating = false;
    }

    private void NavigationRecognizer_NavigationCanceledEvent(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
    {
        // 2.b: Set IsNavigating to be false.
        IsNavigating = false;
    }

    private void ManipulationRecognizer_ManipulationStartedEvent(InteractionSourceKind source, Vector3 position, Ray ray)
    {
        if (FocusedObject != null)
        {
            IsManipulating = true;

            ManipulationPosition = position;

           FocusedObject.SendMessageUpwards("PerformManipulationStart", position);
        }
    }

    private void ManipulationRecognizer_ManipulationUpdatedEvent(InteractionSourceKind source, Vector3 position, Ray ray)
    {
        if (FocusedObject != null)
        {
            IsManipulating = true;

            ManipulationPosition = position;

           FocusedObject.SendMessageUpwards("PerformManipulationUpdate", position);
        }
    }

    private void ManipulationRecognizer_ManipulationCompletedEvent(InteractionSourceKind source, Vector3 position, Ray ray)
    {
        IsManipulating = false;
    }

    private void ManipulationRecognizer_ManipulationCanceledEvent(InteractionSourceKind source, Vector3 position, Ray ray)
    {
        IsManipulating = false;
    }

    private void NavigationRecognizer_TappedEvent(InteractionSourceKind source, int tapCount, Ray ray)
    {

        if (FocusedObject != null)
        {
            FocusedObject.SendMessageUpwards("OnSelect");
        }
    }
    private void ManipulationRecognizer_TappedEvent(InteractionSourceKind source, int tapCount, Ray ray)
    {

        if (FocusedObject != null)
        {
            FocusedObject.SendMessageUpwards("OnSelect");
        }
    }

    void Update()
    {

        // Do a raycast into the world based on the user's
        // head position and orientation.
        var headPosition = Camera.main.transform.position;
        var gazeDirection = Camera.main.transform.forward;

        RaycastHit hitInfo;
        if (Physics.Raycast(headPosition, gazeDirection, out hitInfo))
        {
            // If the raycast hit a hologram, use that as the focused object.
            FocusedObject = hitInfo.collider.gameObject;
        }
        else
        {
            // If the raycast did not hit a hologram, clear the focused object.
            FocusedObject = null;
        }
    }
    /*
        // Use this for initialization
        void Start()
        {
            Instance = this;

            // Set up a GestureRecognizer to detect Select gestures.
            recognizer = new GestureRecognizer();
            recognizer.TappedEvent += (source, tapCount, ray) =>
            {
                // Send an OnSelect message to the focused object and its ancestors.
                if (FocusedObject != null)
                {
                    FocusedObject.SendMessage("OnSelect");
                }
                Debug.Log("In Tapped");
            };

            recognizer.ManipulationStartedEvent += (headPose, source, sourcePose) =>
            {
                // Send an OnSelect message to the focused object and its ancestors.
                if (FocusedObject != null)
                {
                    FocusedObject.SendMessage("OnMouseButtonDown");
                }
                Debug.Log("IN Hold Start");
            };

            recognizer.ManipulationCompletedEvent += (headPose, source, sourcePose) =>
            {
                // Send an OnSelect message to the focused object and its ancestors.
                if (FocusedObject != null)
                {
                    FocusedObject.SendMessage("OnMouseButtonUp");
                }
                Debug.Log("in Hold Complete");
            };

            recognizer.ManipulationCanceledEvent += (headPose, source, sourcePose) =>
            {
                // Send an OnSelect message to the focused object and its ancestors.
                if (FocusedObject != null)
                {
                    FocusedObject.SendMessage("OnMouseButtonUp");
                }
                Debug.Log("in hold cancled");
            };

            recognizer.StartCapturingGestures();
        }

        // Update is called once per frame
        void Update()
        {
            if (isHolding)
            {
                Debug.Log("is holding");
            }
            // Figure out which hologram is focused this frame.
            GameObject oldFocusObject = FocusedObject;

            // Do a raycast into the world based on the user's
            // head position and orientation.
            var headPosition = Camera.main.transform.position;
            var gazeDirection = Camera.main.transform.forward;

            RaycastHit hitInfo;
            if (Physics.Raycast(headPosition, gazeDirection, out hitInfo))
            {
                // If the raycast hit a hologram, use that as the focused object.
                FocusedObject = hitInfo.collider.gameObject;
            }
            else
            {
                // If the raycast did not hit a hologram, clear the focused object.
                FocusedObject = null;
            }

            // If the focused object changed this frame,
            // start detecting fresh gestures again.
            if ( (FocusedObject != oldFocusObject) && !isHolding) {
                recognizer.CancelGestures();
                recognizer.StartCapturingGestures();
            }
        }
        */
    //#endif 
}
