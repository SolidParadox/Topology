using UnityEngine;

public class UnitInfo : MonoBehaviour {
  public static int [ ][ ] special = { new int [] {}, new int [] { 0 } };
  private static float strengthLerp = 3;
  public bool lerping;
  public Vector3 offset;

  public int type;
  public UnitInfo stack;
  public UnitInfo sub;

  public bool moving = false;
  private Collider col;

  private void Start () {
    col = GetComponent<Collider>();
  }

  public void RemoveStack () {
    if ( stack != null ) {
      stack.sub = null;
      stack = null;
    }
  }

  public void RemoveSub () {
    if ( sub != null ) {
      sub.stack = null;
      sub = null;
    }
  }

  public void ChangeTransparency( bool alpha ) {
    col.enabled = alpha;
    if ( stack != null ) {
      stack.ChangeTransparency( alpha );
    }
  }

  public void RemoveStackAndRetake () {
    UnitInfo delta = stack;
    RemoveStack ();
    stack = delta.stack;
    delta.stack = null;
    if ( stack != null ) {
      stack.sub = this;
    }
  }

  public void StackOn ( UnitInfo target ) {
    target.stack = this;
    sub = target;
  }

  public bool CanStackOn( int alpha ) {
    if ( stack != null ) return false;
    for ( int i = 0; i < special [ alpha ].Length; i++ ) {
      if ( special [ type ] [ i ] == alpha ) {
        return false;
      }
    }
    return true;
  }

  public bool CanStackOn ( UnitInfo alpha ) {
    return CanStackOn ( alpha.type );
  }

  private Vector3 RoundedVec ( Vector3 alpha ) {
    alpha.x = Mathf.Round ( alpha.x * 4 ) / 4;
    alpha.y = Mathf.Round ( alpha.y * 4 ) / 4;
    alpha.z = -0.1f;
    return alpha;
  }

  public void ChangeMoving ( bool alpha ) {
    moving = alpha;
    ChangeTransparency ( !alpha );
  }

  private void LateUpdate () {
    if ( lerping ) {
      if ( stack != null ) {
        stack.transform.position = Vector3.Lerp ( stack.transform.position, transform.position + offset, Time.deltaTime * strengthLerp );
      }
      if ( sub == null && !moving ) {
        transform.position = Vector3.Lerp ( transform.position, RoundedVec ( transform.position ), Time.deltaTime * strengthLerp );
      }
    }
  }
}
