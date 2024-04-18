using UnityEngine;

public class VisualizerScript : MonoBehaviour {
  public UnitInfo target;

  public int visibleLayer;
  public int visibleTargetLayer;

  public LayerMask rayMask;

  public Camera cam;

  public float angularResolution = 5;
  public float downAngle = 30;
  public float sweepResolution = 5;
  private float sweepDelta;

  void Update () {
    Vector3 pointingVector;
    target.GetComponent<UnitInfo>().SetLayer ( visibleTargetLayer, true );
    sweepDelta += sweepResolution * Time.deltaTime;

    if ( sweepDelta >= downAngle ) {
      sweepDelta -= downAngle;
    }

    pointingVector = Quaternion.Euler ( sweepDelta, 0, 0 ) * Vector3.forward;

    for ( float i = 0; i < 360; i += angularResolution ) {
      RaycastHit hit;
      Vector3 origin = target.transform.position - target.transform.forward * target.topRho;
      Vector3 direction = target.transform.rotation * pointingVector * 50;
      if ( Physics.Raycast ( origin, direction, out hit, 50, rayMask ) ) {
        UnitInfo uif = hit.transform.GetComponent<UnitInfo>();
        if ( uif != null ) {
          uif.SetLayer ( visibleLayer );
          if ( uif == target ) {
            Debug.DrawLine ( origin, origin + direction, Color.red );
          } else {
            Debug.DrawLine ( origin, origin + direction, Color.green );
          }
        }
      }
      pointingVector = Quaternion.Euler ( 0, 0, angularResolution ) * pointingVector;
    }
  }
}
