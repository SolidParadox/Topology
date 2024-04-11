using System.Linq;
using UnityEngine;

public class BuildingViewManager : MonoBehaviour {
  public Vector3 idleAngles;
  public Vector3 overviewAngles;

  private Quaternion target = Quaternion.identity;

  public bool overviewMode = false;

  public float strengthLerp = 3;

  public int currentFloor;

  public Transform zoneRoot;
  private Transform [ ] zones;

  private void Start () {
    currentFloor = 0;
    zones = new Transform [ zoneRoot.childCount ];
    for ( int i = 0; i < zoneRoot.childCount; i++ ) {
      zones [ i ] = zoneRoot.GetChild( i );
    }
    ChangeMode ( true );
  }

  public void ChangeMode ( bool mode = false ) {
    if ( !mode ) {
      overviewMode = !overviewMode;
    }
    if ( overviewMode ) {
      target = Quaternion.Euler ( overviewAngles );
    } else {
      target = Quaternion.Euler ( idleAngles );
    }
    for ( int i = 0; i < zones.Length; i++ ) {
      zones [ i ].gameObject.SetActive ( overviewMode || i == currentFloor );
    }
  }

  public void ChangeFloor ( bool up ) {
    if ( overviewMode ) {
      currentFloor += up ? 1 : -1;
      currentFloor = ( currentFloor + zones.Length ) % zones.Length;
    }
  }

  void LateUpdate () {
    zoneRoot.localRotation = Quaternion.Lerp ( zoneRoot.localRotation, target, strengthLerp * Time.deltaTime );
  }
}
