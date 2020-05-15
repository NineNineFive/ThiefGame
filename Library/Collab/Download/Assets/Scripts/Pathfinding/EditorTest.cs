using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Graph))]
public class EditorTest : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Create Node"))
        {
            Graph graph = (Graph) target;
            //graph
            //GameObject.CreatePrimitive(PrimitiveType.Sphere);
        }
    }
}
/*
public void On
}
*/