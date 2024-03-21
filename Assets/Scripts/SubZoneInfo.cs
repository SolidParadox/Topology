using UnityEngine;

public class SubZoneInfo : MonoBehaviour {
  public GameObject prefab1, prefab2;
  public int paramW, paramH;
  public float paramXs, paramYs;
  public Vector3 offset;

  public void Insert () {
    for ( int i = 0; i < transform.childCount; i++ ) {
      DestroyImmediate ( transform.GetChild( i ).gameObject );  
    }
    GameObject delta;
    for ( int i = 0; i < paramW; i++ ) {
      for ( int j = 0; j < paramH; j++ ) {
        delta = Instantiate ( ( i + j ) % 2 == 0 ? prefab1 : prefab2, transform );
        delta.transform.localPosition = new Vector3 ( i * paramXs, j * paramYs, delta.transform.localPosition.z ) + offset;
      }
    }
  }
}
