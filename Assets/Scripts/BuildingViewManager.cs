using UnityEngine;

public class BuildingViewManager : MonoBehaviour {
  public MainController mc;
  public Vector3 overviewAngles;
  private Quaternion target = Quaternion.identity;
  public bool overviewMode = false;
  public float strengthLerp = 3;

  public int currentFloor;

  public Transform zoneRoot;
  private Transform [ ] zones;
  private bool fsSear = false;

  private void Start () {
    currentFloor = 0;
    zones = new Transform [ zoneRoot.childCount ];
    for ( int i = 0; i < zoneRoot.childCount; i++ ) {
      zones [ i ] = zoneRoot.GetChild( i );
    }
  }

  void LateUpdate () {
    if ( Input.GetKeyDown ( KeyCode.M ) ) {
      overviewMode = !overviewMode;
      if ( overviewMode ) {
        mc.LockVertical ();
        target = Quaternion.Euler ( overviewAngles );
      } else {
        mc.UnlockVertical ();
        target = Quaternion.identity;
      }
    }

    zoneRoot.localRotation = Quaternion.Lerp ( zoneRoot.localRotation, target, strengthLerp * Time.deltaTime );
    
    if ( Input.GetAxis ( "MVFS" ) != 0 && fsSear && overviewMode ) {
      currentFloor += (int) Mathf.Sign ( Input.GetAxis ( "MVFS" ) );
      currentFloor = ( currentFloor + zones.Length ) % zones.Length;
      fsSear = false;
    }

    if ( Input.GetAxis ( "MVFS" ) == 0 ) {
      fsSear = true;
    }
  }
}
