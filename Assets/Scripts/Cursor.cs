using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class Cursor : MonoBehaviour
{
	private MeshRenderer meshRenderer;
    private Color meshColor;
	// Use this for initialization
	void Start()
	{
		// Grab the mesh renderer that's on the same object as this script.
		meshRenderer = this.gameObject.GetComponentInChildren<MeshRenderer>();
        meshColor = meshRenderer.materials[0].color;
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
			// If the raycast hit a hologram...
			// Display the cursor mesh.
			meshRenderer.enabled = true;

            if (foundHand())
            {
                meshRenderer.materials[0].color = Color.green;
            }
            else
            {
                meshRenderer.materials[0].color = meshColor;
            }


            // Move thecursor to the point where the raycast hit.
            this.transform.position = hitInfo.point;

			// Rotate the cursor to hug the surface of the hologram.
			this.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
		}
		else
		{
			// If the raycast did not hit a hologram, hide the cursor mesh.
			meshRenderer.enabled = false;
		}
	}

    bool foundHand()
    {
        var sourceStates = InteractionManager.GetCurrentReading();
        Vector3 handLocation = new Vector3();
        bool found = false;
        int i;
        for (i = 0; i < sourceStates.Length; i++)
        {
            if (sourceStates[i].source.kind == InteractionSourceKind.Hand)
            {
                print("getting hand location");
                found = sourceStates[i].sourcePose.TryGetPosition(out handLocation);

            }
        }
        return found;
    }
}