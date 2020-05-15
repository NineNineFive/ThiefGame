using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Graph))]
public class EditorTest : Editor {
    private Graph graph;
    private ArrayList x = new ArrayList();
    private ArrayList y = new ArrayList();
    private float Count = 0;
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        graph = (Graph) target;
        string name = EditorGUILayout.TextField("name");
        
        if (GUILayout.Button("Create Node")) {
            x.Add(0f);
            y.Add(0f);
            graph.MyTest(name,0,0);
        }
        int i = 0;
        foreach (Vertex vertex in graph.getVertices()) {
            vertex.name = EditorGUILayout.TextField("name");
            x[i] = EditorGUILayout.FloatField("x:",(float) x[i]);
            y[i] = EditorGUILayout.FloatField("y:",(float) y[i]);
            vertex.setX((float) x[i]);
            vertex.setY((float) y[i]);
            i++;
        }

    }


    //public void On
}
