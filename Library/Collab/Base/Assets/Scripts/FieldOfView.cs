using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FieldOfView : MonoBehaviour
{
    public Mesh mesh;
    public float FOV;
    public float viewDistance;
    public Vector3 origin;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        float viewDistance = 1f; //radius / can be modified
        float FOV = 90f; //90 angle of FOV - can be modified
        origin = Vector3.zero;
        //Vector3 origin = Vector3.zero;
    }

    void LateUpdate()
        {
            int rayCount = 50; //number of rays - can be modified
            float currentAngle = 0f; //current angle - increased during a cycle
            float angleIncrease = FOV / rayCount; // angle increase between raycasts. Increases during a cycle

            //transform.position = origin;

            // initiate arrays containing vertices, uv and triangle:

            Vector3[] vertices = new Vector3[rayCount + 1 + 1]; //size is number of vertices + nr. rays + origin
            Vector2[] uv = new Vector2[vertices.Length]; // base texture coordinates of the Mesh
            int[] triangles = new int[rayCount * 3]; // 1 ray makes up 1 triangle 

            vertices[0] = origin;

            int vertexIndex = 1; //starting at second index since first is defined at origin
            int triangleIndex = 0;

            for (int i = 0; i <= rayCount; i++) //loop to find positions of vertices 
            {
                Vector3 vertex;

                RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, getVectorFromAngle(currentAngle) * viewDistance,
                    viewDistance);
                if (raycastHit2D.collider == null || raycastHit2D.collider.CompareTag("Player") ||
                    raycastHit2D.collider.CompareTag("Enemy"))
                {
                    vertex = origin + getVectorFromAngle(currentAngle) *
                        viewDistance; //vertex positioned at vector terminal point, from angle between raycast * radius
                    Debug.DrawLine(origin, vertex, Color.yellow, 10f);
                    Debug.Log("No collision" + vertex + raycastHit2D.collider);
                }
                else
                {
                    //Hit object
                    vertex = raycastHit2D.point;
                    Debug.DrawLine(origin, vertex, Color.red, 10f);
                    Debug.Log("Collision:" + vertex + raycastHit2D.collider);
                }

                vertices[vertexIndex] = vertex;

                if (i > 0)
                {
                    //avoiding error when we're on the first index
                    triangles[triangleIndex + 0] = 0;
                    triangles[triangleIndex + 1] = vertexIndex - 1;
                    triangles[triangleIndex + 2] = vertexIndex;

                    triangleIndex += 3; //3 vertices corresponds to one triangle
                }

                vertexIndex++;
                currentAngle -= angleIncrease;
            }

            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;
            mesh.name = "our Mesh";
        }
    public Vector3 getVectorFromAngle(float angleInDegrees)
    {
        float angleRad = angleInDegrees * (Mathf.Deg2Rad);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }
    }
