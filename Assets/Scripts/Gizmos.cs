using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Gizmos : MonoBehaviour {
    public static Gizmos instance;
    [HideInInspector] public Vector3 cursorPos; // GraphEditor cursor in scene
    private Data data;
    public List<AIController> AIs;
    private Camera sceneEditorCamera;

    private void Awake() {
        if(instance==null) instance = this;
        if(Camera.main!=null) sceneEditorCamera = Camera.main;
        data = Data.instance;
    }

    #if UNITY_EDITOR
        private void Update() {
            if (sceneEditorCamera != null) {
                if (Input.GetButtonDown("Fire1")) {
                    cursorPos = sceneEditorCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
                }
            }
        }

        private void OnDrawGizmos() {
            // EDITOR CURSOR
            drawEditorCursor();

            // GRAPH
            if(data!=null&&data.graph!=null){
                drawGraph();
            }
        }

        private void drawEditorCursor() {
            UnityEngine.Gizmos.color = Color.magenta;
            UnityEngine.Gizmos.DrawSphere(new Vector3(cursorPos.x, cursorPos.y, 0), 0.05f);
        }
        
        private void drawGraph() {
            
            if(Application.isEditor){
                
                float z = 0;

                foreach (Vertex vertex in data.graph.vertices) {
                    UnityEngine.Gizmos.color = Color.green;
                    Vector3 vector3 = new Vector3(vertex.x, vertex.y, z);

                    GUIStyle guiStyle = new GUIStyle();
                    guiStyle.alignment = TextAnchor.MiddleLeft;
                    guiStyle.normal.textColor = Color.red;

                    Handles.Label(vector3+new Vector3(-0.035f,-0.12f,z-5), vertex.name, guiStyle);
                    UnityEngine.Gizmos.DrawSphere(vector3,0.05f);
                    if(vertex.edges!=null){
                        for (int i=0; i<vertex.edges.Count;i++) {
                            UnityEngine.Gizmos.DrawLine(new Vector3(vertex.x,vertex.y,z), new Vector3(vertex.edges[i].to.x,vertex.edges[i].to.y,z));
                        }
                    }
                }
                    
                // DRAW AI
                foreach(AIController AI in AIs){
                    if(AI!=null) {
                        // PATHFINDING
                        Path path = AI.path;
                        // ASTAR DEBUG GIZMOS
                        if(path!=null&&path.aStar!=null){
                            List<Vertex> waypoints = path.aStar.theResult; 
                            
                            if (waypoints != null && waypoints.Count>1) {
                                for (int i=0; i<waypoints.Count-1;i++) {
                                    UnityEngine.Gizmos.color = Color.red;
                                    Vector3 fromVector3 = new Vector3(waypoints[i].x, waypoints[i].y, z);
                                    Vector3 toVector3 = new Vector3(waypoints[i+1].x, waypoints[i+1].y,z);
                                    UnityEngine.Gizmos.DrawSphere(fromVector3,0.05f);
                                    Debug.DrawLine(fromVector3, toVector3,Color.red);
                                }
                            }
                        }
                        #if UNITY_EDITOR
                        // SOUND LISTENER
                        Handles.color = Color.red;
                        Handles.DrawWireDisc(AI.transform.position , AI.transform.forward, AI.hearingRadius);
                        
                        #endif
                    }
                }
            }
        }
    #endif
}
