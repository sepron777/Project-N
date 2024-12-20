using UnityEngine;

public class Cone : MonoBehaviour
{
    public float coneAngle = 45f; // Half-angle of the cone in degrees
    public float coneRange = 10f; // Range of the cone
    public LayerMask targetLayer; // Layers to include in the detection

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Replace with your trigger condition
        {
            DetectInCone();
        }
    }

    void DetectInCone()
    {
        // Step 1: Find objects within the sphere
        Collider[] hits = Physics.OverlapSphere(transform.position, coneRange, targetLayer);

        foreach (Collider hit in hits)
        {
            Vector3 directionToTarget = (hit.transform.position - transform.position).normalized;

            // Step 2: Check if the object is within the cone's angle
            if (Vector3.Angle(transform.forward, directionToTarget) <= coneAngle)
            {
                // Object is within the cone
                Debug.Log("Object in cone: " + hit.name);
            }
        }
    }

    void OnDrawGizmos()
    {
        // Visualize the cone in the editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, coneRange);

        // Visualize the cone boundaries in 3D
        Gizmos.color = Color.red;
        DrawCone(transform.position, transform.forward, coneRange, coneAngle);
    }

    private void DrawCone(Vector3 position, Vector3 direction, float range, float angle)
    {
        // Step 1: Calculate the base radius of the cone
        float radius = Mathf.Tan(Mathf.Deg2Rad * angle) * range;

        // Step 2: Draw multiple lines to approximate the cone
        int segments = 20; // Increase for smoother cone visualization
        for (int i = 0; i <= segments; i++)
        {
            float theta = i * 360f / segments;
            Quaternion rotation = Quaternion.Euler(0, theta, 0);

            Vector3 baseDirection = rotation * (Vector3.forward * radius);
            Vector3 point = position + direction.normalized * range + baseDirection;

            // Draw the line from the origin to the cone boundary
            Gizmos.DrawLine(position, point);
        }
    }
}
