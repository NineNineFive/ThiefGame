using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour {
    private Mesh mesh;
    [Range(2,200)] public int rayCount = 100; //number of rays - can be modified
    public float FOV = 90f;
    private float oldFOV;
    public float viewDistance = 1f;
    private float currentAngleStart; //current angle - increased during a cycle
    private float currentAngle;
    private Vector3 origin;
    
    void Awake() {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        origin = Vector3.zero;
        currentAngleStart = 90 + FOV / 2; // get the angle the player is looking at
        oldFOV = FOV;
        currentAngle = currentAngleStart;
    }

    void LateUpdate() {
        if (FOV != oldFOV) {
            currentAngleStart = 90 + FOV / 2; 
        }

        float angleIncrease = FOV / rayCount; // angle increase between raycasts. Increases during a cycle

        //transform.position = origin;

        // initiate arrays containing vertices, uv and triangle:

        Vector3[] vertices = new Vector3[rayCount + 1 + 1]; //size is number of rays +  zero ray + origin
        Vector2[] uv = new Vector2[vertices.Length]; // base texture coordinates of the Mesh
        int[] triangles = new int[rayCount * 3]; // 1 ray makes up 1 triangle 

        vertices[0] = origin;

        int vertexIndex = 1; //starting at second index since first is defined at origin
        int triangleIndex = 0;

        for (int i = 0; i < rayCount; i++) { //loop to find positions of vertices 
            Vector3 vertex = origin + getVectorFromAngle(currentAngle) * viewDistance; //vertex positioned at vector terminal point, from angle between raycast * radius;
            //Vector3 rVertex = transform.parent.position + getVectorFromAngle(currentAngle) * viewDistance;
            
            RaycastHit2D[] raycastHit2D = Physics2D.RaycastAll(transform.parent.position, transform.parent.rotation * getVectorFromAngle(currentAngle), viewDistance);
            Vector2 point = Vector2.zero;
            foreach (RaycastHit2D hit in raycastHit2D) {
                if(hit.collider.CompareTag("Obstacle")||hit.collider.CompareTag("Hidden")){
                    if(hit.transform.gameObject.GetComponent<SpriteRenderer>().sharedMaterial.shader.name!="Sprites/Default") hit.transform.gameObject.GetComponent<SpriteRenderer>().material.shader = Shader.Find("Sprites/Default");
                }
                if (hit.collider.CompareTag("Obstacle")) {
                    point = hit.point;
                    vertex = transform.InverseTransformPoint(new Vector3(point.x, point.y, 0));
                    
                    break;
                }
            }

            vertices[vertexIndex] = vertex;

            if (i > 0) {
                //avoiding error when we're on the first index
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3; //3 vertices corresponds to one triangle
            }

            vertexIndex++;
            currentAngle -= angleIncrease;
        }

        currentAngle = currentAngleStart;
        
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.name = "our Mesh";
        //GetComponent<MeshFilter>().mesh = mesh;
    }
    
    public Vector3 getVectorFromAngle(float angleInDegrees) {
        float angleRad = angleInDegrees * (Mathf.Deg2Rad);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }
}