using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class GraphEditor : EditorWindow {
    
    public Graph graph;
    public Gizmos gizmos;
    public Vector3 cursorPos;
    private GameObject startObj;
    private GameObject endObj;
    private string name;
    private Vector2 vector2;
    private Data data;

    [MenuItem("Our Custom Editors/Graph Editor")]
    private static void Init() {
        EditorWindow window = GetWindow(typeof(GraphEditor));
        window.Show();
    }

    private void Update() {
        updateCursor();
    }

    private void updateCursor() {
        Gizmos gizmos = Gizmos.instance;
        if(gizmos!=null){
            cursorPos = gizmos.cursorPos;
        }
    }

    
    
    private void OnGUI() {
        // SELECT GRAPH
        graph = (Graph) EditorGUILayout.ObjectField("Graph", graph, typeof(Graph), true);
        gizmos = (Gizmos) EditorGUILayout.ObjectField("Gizmos", gizmos, typeof(Gizmos), true);

        // IF GRAPH SELECTED
        if(graph!=null&&gizmos!=null) {
            if(gizmos.graph == null) gizmos.graph = graph;
            /*
            if(gizmos.aStar == null) gizmos.aStar = new AStar(graph);
            */
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            
            name = EditorGUILayout.TextField("name", name);   
            //vector2 = EditorGUILayout.Vector2Field("Vector2", vector2); 
            // CREATE NODE AT THE COORDINATE
            if (GUILayout.Button("Create Node")) {
                vector2.x = cursorPos.x;
                vector2.y = cursorPos.y;
                graph.add(name, vector2.x, vector2.y);
                name = "";
                GUI.FocusControl(null);
            }
            
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            
            name = EditorGUILayout.TextField("name", name);   
            if (GUILayout.Button("Remove Node")) {
                graph.remove(name);
            }
            
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            
            if (GUILayout.Button("Remove All Nodes")) {
                graph.clear();
            }

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            
            
            if (GUILayout.Button("Load Graph")) {
                data = Data.instance;
                if (data == null) {
                    data = Data.instance;
                }
                //Debug.Log("loading graph from "+data);
                graph = data.loadGraph(graph); 

                EditorWindow view = GetWindow<SceneView>();
                view.Repaint(); // REFRESHES THE SCENE VIEW
            }
            
            if (GUILayout.Button("Calculate Edges")) {
                graph.calculateEdges();
            }
            
            if (GUILayout.Button("Save Graph")) {
                if (data == null) {
                    data = Data.instance;
                }
                data.saveGraph(graph); 
                data.loadGraph(graph);
                
                
                EditorWindow view = GetWindow<SceneView>();
                view.Repaint(); // REFRESHES THE SCENE VIEW
            }
            
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            
            startObj = (GameObject) EditorGUILayout.ObjectField("Start", startObj, typeof(GameObject), true);
            endObj = (GameObject) EditorGUILayout.ObjectField("End", endObj, typeof(GameObject), true);
            if (GUILayout.Button("Run AStar")) {

                // RUNASTAR TODO: do it
            }
            
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }
    }

   


}

