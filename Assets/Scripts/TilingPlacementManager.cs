using UnityEngine;
using System.Collections.Generic;

public class TilingPlacementManager : MonoBehaviour {
  public UnitInfo alpha;
  private UnitInfo initialBase;
  public SubZoneInfo szi;

  private int step = 0;

  private void Start () {
    step = 0;
  }

  public bool TrySetAlpha ( UnitInfo _alpha ) {
    if ( alpha == null ) {
      alpha = _alpha;
      return true;
    }
    return false;
  }

  public void LateUpdate () {
    if ( alpha != null ) {
      List<UnitInfo> delta;
      int deltaLG;
      Vector3 cPos = alpha.transform.localPosition;

      if ( step == 2 ) {
        if ( szi.TryGetContext ( cPos, out delta ) ) {
          deltaLG = delta.Count;
          Debug.Log ( "FOUND A TILE TO STACK ON" );
          delta [ deltaLG - 1 ].stack = alpha;
        } else {
          Debug.Log ( "GOING BACK AAAAAAA" );
          if ( initialBase != null ) {
            initialBase.stack = alpha;
          }
        }
        step = 3;
      }
      
      if ( step == 0 ) {
        if ( szi.TryGetContext ( cPos, out delta ) ) {
          deltaLG = delta.Count;
          for ( int i = 0; i < deltaLG; i++ ) { 
            if ( delta [ i ].stack == alpha ) {
              delta [ i ].stack = null;
              break;
            }
          }
        }
        step = 1;
      }

      if ( step == 3 ) {
        step = 0;
        alpha = null;
        initialBase = null;
      }
    }
  }


  public void StartSnap () {
    step = 2;
  }
}
