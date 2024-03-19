#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class TrackEditorEditor : MonoBehaviour {
}

#if UNITY_EDITOR
[CustomEditor ( typeof ( SubZoneInfo ) )]
public class SubZoneGenerator_Custom : Editor {
  public override void OnInspectorGUI () {
    base.OnInspectorGUI ();

    SubZoneInfo pathGenerator = (SubZoneInfo)target;
    if ( GUILayout.Button ( "INSERT" ) ) {
      pathGenerator.Insert ();
    }
    string currentMessage = "";
    if ( pathGenerator.contents == null ) {
      currentMessage = "NOT INITIALIZED";
    } else {
      currentMessage = "CONTENTS COUNT : " + pathGenerator.contents.Count.ToString ();
    }
    GUILayout.Label ( currentMessage );
  }
}
#endif