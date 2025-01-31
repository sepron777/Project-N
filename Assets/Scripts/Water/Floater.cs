using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class Floater : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    public float depthBeforeSubmerged = 1;
    public float displacementAmount = 3;
    public int floaterCount = 1;

    public WaterSurface waterSurface;
    WaterSearchParameters waterSpectrumParameters;
    WaterSearchResult waterSearchResult;

    public float waterDrag = 0.99f;
    public float waterAngularDrag = 0.5f;

    private void FixedUpdate()
    {
        rb.AddForceAtPosition(Physics.gravity / floaterCount, transform.position, ForceMode.Acceleration);
        waterSpectrumParameters.startPositionWS = transform.position;
        waterSurface.ProjectPointOnWaterSurface(waterSpectrumParameters, out waterSearchResult);
        float height = waterSearchResult.projectedPositionWS.y;
        if (transform.position.y < height)
        {
            float displacementMultiplier = Mathf.Clamp01((height - transform.position.y) / depthBeforeSubmerged) * displacementAmount;
            rb.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), transform.position, ForceMode.Acceleration);
            rb.AddForce(displacementMultiplier * -rb.linearVelocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            rb.AddTorque(displacementMultiplier * -rb.angularVelocity * waterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }
}