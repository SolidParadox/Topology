using System.Collections.Generic;
using TMPro;
using UnityEngine;

// UI Crosshair Stacks Info
public class UICSInfo : MonoBehaviour {
  public MainController mc;
  public TMP_Text label;

  void LateUpdate () {
    if ( mc.CanGetCSInfo() ) {
      List<int> delta = mc.GetCSInfo();
      if ( delta != null ) {
        label.text = "";
        foreach ( var x in delta ) {
          label.text += x + " ";
        }
      }
    } else {
      label.text = "DEBUG, DEAD";
    }
  }
}
