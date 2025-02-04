using UnityEngine;

public class SimpleCameraCollision : MonoBehaviour
{
    public float collisionOffset = 0.5f; //
    public LayerMask collisionMask; // 

    private void LateUpdate()
    {
        // 
        RaycastHit hit;
        Vector3 direction = -transform.localPosition.normalized; // 

        if (Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity, collisionMask))
        {
            // 
            Vector3 newPosition = hit.point + hit.normal * collisionOffset;
            transform.position = newPosition;
        }
    }
}
