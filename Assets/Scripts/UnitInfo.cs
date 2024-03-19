using UnityEngine;

public class UnitInfo : MonoBehaviour {
  public static int [ ][ ] special = { new int [] {}, new int [] { 0 } };
  private static float strengthLerp = 2;
  public bool lerping;
  public Vector3 offset;

  public void RemoveStackAndRetake () {
    if ( stack != null ) {
      if ( stack.stack != null ) {
        UnitInfo delta = stack;
        stack = stack.stack;
        delta.stack = null;
      }
    }
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

  private void LateUpdate () {
    if ( stack != null && lerping ) {
      stack.transform.position = Vector3.Lerp ( stack.transform.position, transform.position + offset, Time.deltaTime * strengthLerp );
    }
  }

  public int type;
  public UnitInfo stack;
}
