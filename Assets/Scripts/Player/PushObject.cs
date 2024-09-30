using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushObject : MonoBehaviour
{
    public float pushPower = 2.0f;  // The strength of the push

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.collider.attachedRigidbody;

        // No rigidbody or the object is kinematic, we can't push it
        if (rb == null || rb.isKinematic)
            return;

        // Calculate push direction from character controller movement direction
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // Apply the push force, multiplied by pushPower
        rb.AddForce(pushDir * pushPower, ForceMode.Impulse);
    }
}
