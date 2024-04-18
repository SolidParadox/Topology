using UnityEngine;

public class Mani : MonoBehaviour {
  void Start () {
    for ( int i = 1; i < Display.displays.Length; i++ ) {
      Display.displays [ i ].Activate ();
    }
  } 
}
