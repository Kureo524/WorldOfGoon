using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelConstructor))]
public class LevelConstructorEditor : Editor {
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LevelConstructor script = (LevelConstructor)target;

        if (GUILayout.Button("Check for links")) {
            script.CheckForLinks();
        }
    }
}
