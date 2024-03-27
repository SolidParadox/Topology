using UnityEngine;

public class TilingPlacementManager : MonoBehaviour {
  public UnitInfo alpha;
  public LayerMask lmRFloor;
  public int mode = 0;

  public bool TrySetAlpha ( UnitInfo _alpha ) {
    if ( alpha == null ) {
      alpha = _alpha;
      alpha.ChangeMoving ( true );
      alpha.RemoveSub ();
      mode = 0;
      return true;
    }
    return false;
  }

  public bool TrySetAlpha ( Transform _alpha ) {
    return TrySetAlpha ( _alpha.GetComponent<UnitInfo> () );
  }

  public void LateUpdate () {
    if ( alpha != null ) {
      // During mode 0, i could continually raycast and check if it's hitting something, and highlight it accordingly
      // Also, the highlighted ones could ever so slightly move the stack up so its easier to stack things in between
      if ( mode == 1 ) {
        StackAndReleaseAlpha();
        mode = 0;
      }
    }
  }

  public void ReleaseAlpha () {
    mode = 1;
  }

  private void StackAndReleaseAlpha () { 
    RaycastHit hit;
    if ( Physics.Raycast ( alpha.transform.position - Vector3.forward * 20, Vector3.forward, out hit, 50, lmRFloor ) ) {
      UnitInfo uif = hit.transform.GetComponent<UnitInfo>();
      if ( uif != null ) { // And you presumably can stack them on each other
        alpha.StackOn ( uif );
      }
    }
    alpha.ChangeMoving ( false );
    alpha = null;
  }
}
