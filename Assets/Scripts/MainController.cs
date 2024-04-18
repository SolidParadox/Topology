using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour {
  public TilingPlacementManager manTP;
  public BuildingViewManager    manBV;

  public Camera cam;
  public Transform target;

  private bool sear1;

  public LayerMask lmRUnit;

  public int mode;
  private Vector2 deltaPos, deltaSPPos, cPos;
  public float keyPosSens;
  public float strengthZoom;

  public GameObject crosshair;
  public LayerMask lmGPR; // Ground penetrating radar

  private void Start () { 
    mode = 0;
    deltaPos = Vector3.zero; cPos = Vector3.zero;
    crosshair.SetActive ( false );
  }

  public void ReleaseTarget () {
    if ( mode == 2 ) {
      manTP.ReleaseAlpha ();
    }
    if ( mode == 4 ) {
      // Do something
      crosshair.SetActive ( false );
    }
    mode = -1;
    target = null;
  }

  void LateUpdate () {
    cPos = cam.ScreenToWorldPoint ( Input.mousePosition );

    // TARGET STUFF
    if ( Input.GetAxis ( "Fire1" ) > 0 ) {
      if ( mode == 0 ) {
        RaycastHit hit;
        if ( Physics.Raycast ( (Vector3)cPos + Vector3.forward * -25, Vector3.forward, out hit, 75, lmRUnit ) ) {
          // Grabbed Unit, move the unit
          target = hit.collider.transform;
          if ( manTP.TrySetAlpha ( target ) ) {
            mode = 2;
          } else {
            mode = 1;
          }
        } else {
          // Grabbed terrain, move the camera
          mode = 1;
        }
      }
    } else {
      ReleaseTarget ();
      mode = 0;
    }

    Vector3 deltaWPos = cPos - deltaPos;

    // DELTA STUFF
    Vector2 rhoCM = new Vector2 ( Input.GetAxis ( "Horizontal" ), Input.GetAxis ( "Vertical" ) );
    if ( rhoCM.magnitude != 0 ) {
      if ( mode != 4 ) {
        ReleaseTarget ();
        // Activate Shit
        crosshair.SetActive ( true );
      }
      mode = 4;
    }

    if ( mode == 1 ) {
      cam.transform.localPosition -= cam.ScreenToWorldPoint ( (Vector2)Input.mousePosition ) - cam.ScreenToWorldPoint ( deltaSPPos );
    }
    if ( mode == 2 ) {
      float test = Input.mousePosition.x / Screen.width;
      if ( test <= 0.05f ) { rhoCM.x = -1; }
      if ( test >= 0.95f ) { rhoCM.x = 1; }
      test = Input.mousePosition.y / Screen.height;
      if ( test <= 0.05f ) { rhoCM.y = -1; }
      if ( test >= 0.95f ) { rhoCM.y = 1; }
      target.localPosition += deltaWPos;
    }
    if ( mode == 4 ) {
      RaycastHit hit;
      if ( Physics.Raycast ( crosshair.transform.position, Vector3.forward, out hit, 50, lmGPR ) ) {
        target = hit.collider.transform;        
      }
    }
      // CAMERA STUFF
      if ( Input.GetAxis ( "Mouse ScrollWheel" ) != 0 ) {
      float deltaOS = cam.orthographicSize;
      cam.orthographicSize -= Input.GetAxis ( "Mouse ScrollWheel" ) * strengthZoom;
      if ( cam.orthographicSize < 2 ) { cam.orthographicSize = 2; }
      if ( deltaOS != cam.orthographicSize ) {
        rhoCM += ( cPos - (Vector2)cam.transform.position )
          * Mathf.Sign ( Input.GetAxis ( "Mouse ScrollWheel" ) ) / cam.orthographicSize;
      }
    }

    cam.transform.localPosition += ( Vector3 )rhoCM * keyPosSens;

    // SUB CAMERA STUFF ( RESET )
    if ( Input.GetKeyDown ( KeyCode.R ) ) {
      cam.orthographicSize = 5;
      cam.transform.localPosition = Vector3.zero;
      mode = -1;
      target = null;
    }

    // FLOOR MANAGER STUFF
    if ( Input.GetKeyDown ( KeyCode.M ) ) {
      ReleaseTarget ();
      manBV.ChangeMode ();
    }

    // MOUSE POSITION 
    deltaSPPos = Input.mousePosition;
    deltaPos = cPos;
  }

  public bool CanGetCSInfo () {
    return target != null && mode == 4;
  }

  public List<int> GetCSInfo () {
    if ( target != null && mode == 4 ) {
      return target.GetComponent<UnitInfo> ().GetStackInfo ();
    }
    return null;
  }
}
