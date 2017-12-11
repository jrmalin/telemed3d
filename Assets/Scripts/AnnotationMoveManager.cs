using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class AnnotationMoveManager : MonoBehaviour {
    public GestureRecognizer ManipulationRecognizer { get; private set; }
    public bool IsManipulating { get; private set; }
    GameObject FocusedObject;
    public static AnnotationMoveManager Instance { get; private set; }

    public Vector3 ManipulationPosition { get; private set; }
    // Use this for initialization
    void Start () {

        Instance = this;
        ManipulationRecognizer = new GestureRecognizer();

        ManipulationRecognizer.SetRecognizableGestures(GestureSettings.Tap |
            GestureSettings.ManipulationTranslate);

        ManipulationRecognizer.ManipulationStartedEvent += ManipulationRecognizer_ManipulationStartedEvent;
        ManipulationRecognizer.ManipulationUpdatedEvent += ManipulationRecognizer_ManipulationUpdatedEvent;
        ManipulationRecognizer.ManipulationCompletedEvent += ManipulationRecognizer_ManipulationCompletedEvent;
        ManipulationRecognizer.ManipulationCanceledEvent += ManipulationRecognizer_ManipulationCanceledEvent;

        ManipulationRecognizer.StartCapturingGestures();
    }
    private void ManipulationRecognizer_ManipulationStartedEvent(InteractionSourceKind source, Vector3 position, Ray ray)
    {
        print(FocusedObject.name);
        if (FocusedObject != null)
        {
            IsManipulating = true;

            ManipulationPosition = position;

            FocusedObject.SendMessageUpwards("PerformManipulationStart", position);
        }
    }

    private void ManipulationRecognizer_ManipulationUpdatedEvent(InteractionSourceKind source, Vector3 position, Ray ray)
    {
        print(FocusedObject.name);
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

    // Update is called once per frame
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
}


