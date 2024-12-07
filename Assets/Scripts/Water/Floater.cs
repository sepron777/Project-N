using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class Floater : MonoBehaviour
{
    public Rigidbody rb;
    public float dehpSub;
    public float displacementAnt;
    public int floaters;
    public float waterDrag;
    public float waterAngularDrag;
    public WaterSurface waterSurface;
    WaterSearchParameters waterSpectrumParameters;
    WaterSearchResult waterSearchResult;
    private void FixedUpdate()
    {
        rb.AddForceAtPosition(Physics.gravity/floaters, transform.position,ForceMode.Acceleration);

        waterSpectrumParameters.startPositionWS = transform.position;

        waterSurface.ProjectPointOnWaterSurface(waterSpectrumParameters,out waterSearchResult);

        if (transform.position.y< waterSearchResult.projectedPositionWS.y)
        {
            float dispacement = Mathf.Clamp01((waterSearchResult.projectedPositionWS.y-transform.position.y)*dehpSub)* displacementAnt;
            rb.AddForceAtPosition(new Vector3(0,Mathf.Abs(Physics.gravity.y)*dispacement,0),transform.position,ForceMode.Acceleration);
            rb.AddForce(dispacement*-rb.linearVelocity*waterDrag*Time.fixedDeltaTime,ForceMode.VelocityChange);
            rb.AddTorque(dispacement * -rb.linearVelocity * waterAngularDrag*Time.fixedDeltaTime,ForceMode.VelocityChange);
        }
    }
}
