using UnityEngine;

public class TilingPlacementManager : MonoBehaviour {
  public UnitInfo target;
  public LayerMask lmRFloor;
  public int mode = 0;

  public bool TrySetAlpha ( UnitInfo _alpha ) {
    if ( _alpha.movable == false ) return false;
    if ( target == null ) {
      target = _alpha;
      target.ChangeMoving ( true );
      target.RemoveSub ();
      mode = 0;
      return true;
    }
    return false;
  }

  public bool TrySetAlpha ( Transform _alpha ) {
    return TrySetAlpha ( _alpha.GetComponent<UnitInfo> () );
  }

  public void LateUpdate () {
    if ( target != null ) {
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
    if ( Physics.Raycast ( target.transform.position - Vector3.forward * 20, Vector3.forward, out hit, 50, lmRFloor ) ) {
      UnitInfo uif = hit.transform.GetComponent<UnitInfo>();
      if ( uif != null ) { // And you presumably can stack them on each other
        target.StackOn ( uif );
      }
    }
    target.ChangeMoving ( false );
    target = null;
  }
}
