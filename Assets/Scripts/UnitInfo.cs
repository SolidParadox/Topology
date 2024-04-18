using UnityEngine;
using System.Collections.Generic;

public class UnitInfo : MonoBehaviour {
  public static int [ ][ ] special = { new int [] {}, new int [] { 0 } };
  private static float strengthLerp = 3;
  public bool lerping;
  private Vector3 offset;

  public float topRho;
  public float bottomRho;

  public int type;
  public UnitInfo stack;
  public UnitInfo sub;

  public bool moving;
  public bool frozen;
  private Collider col;

  private int originalLayer;
  public float layerRevertTimer = 0.25f;
  private float deltaRT;

  public bool movable = false;

  private void Start () {
    col = GetComponent<Collider> ();
    originalLayer = gameObject.layer;
  }

  public void RevertLayer () {
    gameObject.layer = originalLayer;
  }

  private bool hardToggled = false;
  public void SetLayer ( int alpha, bool hard = false ) {
    hardToggled = hard;
    if ( hardToggled ) return;
    gameObject.layer = alpha;
    deltaRT = layerRevertTimer;
    if ( stack != null ) { stack.SetLayer ( alpha ); }
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

  public void ChangeTransparency ( bool alpha ) {
    col.enabled = alpha;
    if ( stack != null ) {
      stack.ChangeTransparency ( alpha );
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
    offset = -new Vector3 ( 0, 0, bottomRho + sub.topRho );
  }

  public bool CanStackOn ( int alpha ) {
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

  public List<int> GetStackInfo () {
    List<int> delta = new List<int>{ type };
    if ( stack != null ) {
      delta.AddRange ( stack.GetStackInfo() );
    }
    return delta;
  }

  private void LateUpdate () {
    if ( lerping ) {
      if ( sub != null ) {
        transform.position = Vector3.Lerp ( transform.position, sub.transform.position + sub.transform.rotation * offset, Time.deltaTime * strengthLerp );
        transform.rotation = Quaternion.Lerp ( transform.rotation, sub.transform.rotation, Time.deltaTime * strengthLerp );
      }
      if ( !frozen && sub == null && !moving ) {
        transform.position = Vector3.Lerp ( transform.position, RoundedVec ( transform.position ), Time.deltaTime * strengthLerp );
      }
    }
    if ( deltaRT >= 0 ) {
      deltaRT -= Time.deltaTime;
      if ( deltaRT <= 0 ) { 
        RevertLayer ();
      }
    }
  }
}
