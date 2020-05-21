using System.Collections.Generic;
using System.Linq;
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
    public bool visible = true;
    public bool renderUpdater = true;
    public bool colliderOn = true;
    public GameObject target = null;
    
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
                if(renderUpdater){
                    // UPDATE Render of what is seen
                    if(hit.collider.CompareTag("Obstacle")||hit.collider.CompareTag("Hidden")){
                        if(hit.transform.gameObject.GetComponent<SpriteRenderer>().sharedMaterial.shader.name!="Sprites/Default") hit.transform.gameObject.GetComponent<SpriteRenderer>().material.shader = Shader.Find("Sprites/Default");
                    }
                }
                // Change field of view Ray ending points
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
        mesh.name = "Field of View";
        //GetComponent<MeshFilter>().mesh = mesh;
        
        if (visible) {
            GetComponent<MeshRenderer>().enabled = true;
        } else {
            GetComponent<MeshRenderer>().enabled = false;
        }

        if (colliderOn) {
            makePolygonCollider2D(mesh);
        }
    }
    
    public Vector3 getVectorFromAngle(float angleInDegrees) {
        float angleRad = angleInDegrees * (Mathf.Deg2Rad);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    private void makePolygonCollider2D(Mesh mesh) {
        if(mesh == null) {
            return;
        }
 
        PolygonCollider2D polygonCollider2D = gameObject.GetComponent<PolygonCollider2D>();
        if (polygonCollider2D != null) {

            polygonCollider2D.pathCount = 1;

            List<Vector3> vertices = new List<Vector3>();
            mesh.GetVertices(vertices);

            var boundaryPath = EdgeHelpers.GetEdges(mesh.triangles).FindBoundary().SortEdges();

            Vector3[] yourVectors = new Vector3[boundaryPath.Count];
            for (int i = 0; i < boundaryPath.Count; i++) {
                yourVectors[i] = vertices[boundaryPath[i].v1];
            }

            List<Vector2> newColliderVertices = new List<Vector2>();

            for (int i = 0; i < yourVectors.Length; i++) {
                newColliderVertices.Add(new Vector2(yourVectors[i].x, yourVectors[i].y));
            }

            Vector2[] newPoints = newColliderVertices.Distinct().ToArray();


            polygonCollider2D.SetPath(0, newPoints);
        }
    }
    
    private void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            // DONT PREVENT PLAYER FROM ENDING THE GAME 
            GameManager.getInstance().setEndingPrevented(true);
            target = other.gameObject;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            // DONT PREVENT PLAYER FROM ENDING THE GAME
            GameManager.getInstance().setEndingPrevented(false);
            target = null;
        }
    }
}


public static class EdgeHelpers {
     public struct Edge {
         public int v1;
         public int v2;
         public int triangleIndex;
         public Edge(int aV1, int aV2, int aIndex)
         {
             v1 = aV1;
             v2 = aV2;
             triangleIndex = aIndex;
         }
     }
 
     public static List<Edge> GetEdges(int[] aIndices)
     {
         List<Edge> result = new List<Edge>();
         for (int i = 0; i < aIndices.Length; i += 3)
         {
             int v1 = aIndices[i];
             int v2 = aIndices[i + 1];
             int v3 = aIndices[i + 2];
             result.Add(new Edge(v1, v2, i));
             result.Add(new Edge(v2, v3, i));
             result.Add(new Edge(v3, v1, i));
         }
         return result;
     }
 
     public static List<Edge> FindBoundary(this List<Edge> aEdges)
     {
         List<Edge> result = new List<Edge>(aEdges);
         for (int i = result.Count-1; i > 0; i--)
         {
             for (int n = i - 1; n >= 0; n--)
             {
                 if (result[i].v1 == result[n].v2 && result[i].v2 == result[n].v1)
                 {
                     // shared edge so remove both
                     result.RemoveAt(i);
                     result.RemoveAt(n);
                     i--;
                     break;
                 }
             }
         }
         return result;
     }
     public static List<Edge> SortEdges(this List<Edge> aEdges)
     {
         List<Edge> result = new List<Edge>(aEdges);
         for (int i = 0; i < result.Count-2; i++)
         {
             Edge E = result[i];
             for(int n = i+1; n < result.Count; n++)
             {
                 Edge a = result[n];
                 if (E.v2 == a.v1)
                 {
                     // in this case they are already in order so just continoue with the next one
                     if (n == i+1)
                         break;
                     // if we found a match, swap them with the next one after "i"
                     result[n] = result[i + 1];
                     result[i + 1] = a;
                     break;
                 }
             }
         }
         return result;
     }
 }
 