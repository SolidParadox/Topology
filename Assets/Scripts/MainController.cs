using UnityEngine;

public class MainController : MonoBehaviour {
  public Camera cam;
  public TilingPlacementManager placementManager;
  public Transform target;
  private bool sear = false;

  public int mode;
  private Vector3 deltaPos, cPos;

  private RaycastHit hit;

  public float keyPosSens;
  public float releaseSnapStrength;

  public float strengthZoom;

  private int unlockedVertical = 1;

  private void Start () { 
    mode = 0;
    deltaPos = Vector3.zero; cPos = Vector3.zero;
  }

  public void UnlockVertical () {
    unlockedVertical = 1;
  }

  public void LockVertical () {
    unlockedVertical = 0;
  }

  void LateUpdate () {
    cPos = Input.mousePosition;

    if ( Input.GetAxis ( "Fire1" ) > 0 ) {
      if ( !sear ) {
        if ( Physics.Raycast ( cam.ScreenToWorldPoint ( cPos ) + new Vector3 ( 0, 0, -10 ), Vector3.forward, out hit ) ) {
          target = hit.collider.transform;
          placementManager.TrySetAlpha ( target.GetComponent<UnitInfo>() );
          mode = 2;
        } else {
          mode = 1;
        }
        sear = true;
      }
    } else {
      if ( mode != 0 ) {
        placementManager.StartSnap ();
      }
      mode = 0;
      sear = false;
    }

    Vector3 deltaWPos = cam.ScreenToWorldPoint ( cPos ) - cam.ScreenToWorldPoint ( deltaPos );
    deltaWPos.y *= unlockedVertical;

    cam.transform.localPosition += new Vector3 ( Input.GetAxis ( "Horizontal" ), Input.GetAxis ( "Vertical" ) * unlockedVertical ) * keyPosSens;
    if ( mode == 1 ) {
      cam.transform.localPosition -= deltaWPos;
    }
    if ( mode == 2 && unlockedVertical == 1 ) {
      target.localPosition += deltaWPos;
    }

    cam.orthographicSize -= Input.GetAxis ( "Mouse ScrollWheel" ) * strengthZoom;
    if ( cam.orthographicSize < 5 ) { cam.orthographicSize = 5; }

    if ( Input.GetKeyDown ( KeyCode.R ) ) {
      cam.orthographicSize = 5;
      cam.transform.localPosition = Vector3.zero;
      mode = 0;
    }

    deltaPos = cPos;
  }
}
