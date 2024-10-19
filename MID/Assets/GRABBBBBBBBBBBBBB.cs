using UnityEngine;

public class ObjectGrabber : MonoBehaviour
{
    public float grabDistance = 2f; // Maximum distance to grab objects
    public Transform holdPosition;  // Position where grabbed object will be held
    private GameObject grabbedObject = null;  // Reference to the currently grabbed object
    private Rigidbody grabbedObjectRb = null; // Reference to the grabbed object's Rigidbody

    public float throwForce = 5f;  // Force applied when throwing the object

    void Update()
    {
        if (Input.GetMouseButtonDown(0))  // Left mouse button or VR controller trigger
        {
            if (grabbedObject == null)
            {
                TryGrabObject();  // Try grabbing an object if none is currently grabbed
            }
            else
            {
                ReleaseObject();  // Release the currently grabbed object
            }
        }

        if (grabbedObject != null)
        {
            MoveObject();  // Move the grabbed object to the holding position
        }
    }

    void TryGrabObject()
    {
        // Perform a raycast to detect objects in front of the player
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, grabDistance))
        {
            if (hit.collider.CompareTag("Grabbable"))  // Ensure the object is tagged as "Grabbable"
            {
                grabbedObject = hit.collider.gameObject;
                grabbedObjectRb = grabbedObject.GetComponent<Rigidbody>();

                if (grabbedObjectRb != null)
                {
                    // Disable gravity and kinematic to control the object manually
                    grabbedObjectRb.useGravity = false;
                    grabbedObjectRb.isKinematic = true;

                    // Optionally, parent the object to the player hand or hold position
                    grabbedObject.transform.SetParent(holdPosition);
                }
            }
        }
    }

    void ReleaseObject()
    {
        if (grabbedObjectRb != null)
        {
            // Release the object by enabling physics again
            grabbedObjectRb.useGravity = true;
            grabbedObjectRb.isKinematic = false;

            // Optionally, throw the object with force
            grabbedObjectRb.AddForce(Camera.main.transform.forward * throwForce, ForceMode.VelocityChange);
        }

        // Unparent the object and clear references
        grabbedObject.transform.SetParent(null);
        grabbedObject = null;
        grabbedObjectRb = null;
    }

    void MoveObject()
    {
        // Keep the grabbed object at the hold position
        grabbedObject.transform.position = holdPosition.position;
    }
}
