using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class GraphEditor : EditorWindow {
    
    public Data data;
    public Gizmos gizmos;
    public Vector3 cursorPos;
    private GameObject startObj;
    private GameObject endObj;
    private string name;
    private string savedFileName;
    private Vector2 vector2;

    [MenuItem("Our Custom Editors/Graph Editor")]
    private static void Init() {
        EditorWindow window = GetWindow(typeof(GraphEditor));
        window.Show();
    }

    private void Awake() {
        savedFileName = "";
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
        data = (Data) EditorGUILayout.ObjectField("Data", data, typeof(Data), true);
        gizmos = (Gizmos) EditorGUILayout.ObjectField("Gizmos", gizmos, typeof(Gizmos), true);
        string oldName = savedFileName;
        savedFileName = EditorGUILayout.TextField("File Name", savedFileName);
        if(data!=null&&(data.graph==null||savedFileName!=oldName)){ 
            data.loadGraph(savedFileName);
        }
        // IF GRAPH SELECTED
        if(data!=null&&gizmos!=null&&savedFileName!=null&&savedFileName!="") {
            
            

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
                data.graph.add(name, vector2.x, vector2.y);
                name = "";
                GUI.FocusControl(null);
            }
            
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            
            name = EditorGUILayout.TextField("name", name);   
            if (GUILayout.Button("Remove Node")) {
                data.graph.remove(name);
            }
            
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            
            if (GUILayout.Button("Remove All Nodes")) {
                data.graph.clear();
            }

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            
            /*
            if (GUILayout.Button("Load Graph")) {
                if (data == null) {
                    data = Data.instance;
                }
                //Debug.Log("loading graph from "+data);
                data.graph = data.loadGraph(savedFileName); 

                EditorWindow view = GetWindow<SceneView>();
                view.Repaint(); // REFRESHES THE SCENE VIEW
            }
            */
            
            if (GUILayout.Button("Calculate Edges")) {
                data.graph.calculateEdges();
            }
            
            if (GUILayout.Button("Save Graph")) {
                if (data == null) {
                    data = Data.instance;
                }
                data.saveGraph(data.graph,savedFileName);

                EditorWindow view = GetWindow<SceneView>();
                view.Repaint(); // REFRESHES THE SCENE VIEW
            }
            
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            
            startObj = (GameObject) EditorGUILayout.ObjectField("Start", startObj, typeof(GameObject), true);
            endObj = (GameObject) EditorGUILayout.ObjectField("End", endObj, typeof(GameObject), true);
            if (GUILayout.Button("Run AStar")) {
                Vertex vertexStart = new Vertex("Start",startObj.transform.position.x,startObj.transform.position.y);
                Vertex vertexEnd = new Vertex("End",endObj.transform.position.x,endObj.transform.position.y);
                AStar aStar = new AStar(data.graph);
                aStar.run(vertexStart,vertexEnd);
            }
            
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }
    }

   


}

