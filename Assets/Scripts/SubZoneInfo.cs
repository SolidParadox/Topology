using UnityEngine;
using System.Collections.Generic;
using System;

public class SubZoneInfo : MonoBehaviour {
  public GameObject prefab1, prefab2;
  public int paramW, paramH;
  public float paramXs, paramYs;
  public Vector3 offset;

  [SerializeField]
  public Dictionary<Vector2Int,UnitInfo> contents;

  public bool TryGetContext ( Vector2 pos, out List<UnitInfo> ui ) {
    Vector2Int poss = new Vector2Int ( Mathf.RoundToInt ( pos.x / paramXs ), Mathf.RoundToInt ( pos.y / paramYs ) );

    Debug.Log ( pos + " --- > " + poss );
    Debug.Log ( contents.ContainsKey ( poss ) );

    UnitInfo deltaUI;
    int debugMaxIterations = 0;
    ui = new List<UnitInfo>();

    if ( contents.TryGetValue ( poss, out deltaUI ) ) {

      while ( deltaUI != null && debugMaxIterations < 10 ) {
        Debug.Log ( "Found in stack : " + deltaUI.name );
        ui.Add ( deltaUI );
        deltaUI = deltaUI.stack;
        debugMaxIterations++;
      }

      return true;
    }
    return false;
  }

  public void Insert () {
    for ( int i = 0; i < transform.childCount; i++ ) {
      DestroyImmediate ( transform.GetChild( i ).gameObject );  
    }
    GameObject delta;
    if ( contents != null ) {
      contents.Clear ();
    } else {
      contents = new Dictionary<Vector2Int, UnitInfo> ();
    }
    for ( int i = 0; i < paramW; i++ ) {
      for ( int j = 0; j < paramH; j++ ) {
        delta = Instantiate ( ( i + j ) % 2 == 0 ? prefab1 : prefab2, transform );
        delta.transform.localPosition = new Vector3 ( i * paramXs, j * paramYs, delta.transform.localPosition.z ) + offset;
        contents.Add ( new Vector2Int ( i, j ), delta.GetComponent<UnitInfo>() );
      }
    }
  }

  private void Start () {
    Insert ();
  }
}
