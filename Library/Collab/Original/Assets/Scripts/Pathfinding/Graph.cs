using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Graph : MonoBehaviour {
	public List<Vertex> vertices = new List<Vertex>();

	public void Awake() {
		String json = GameManager.instance.loadJSONFile("graph.json");
		JSONList list = JsonUtility.FromJson<JSONList>(json);
		vertices = list.vertices;

		foreach (Vertex vertex in vertices) {
			foreach (int id in vertex.adjacency) {
				vertex.edges.Add(new Edge(vertex,vertices[id]));
			}
		}
	}


	public List<Vertex> getVertices() {
		return vertices;
	}
	/*
	public Vertex addVertex(string name, float x, float y) {
		Vertex newVertex = new Vertex(name,x,y);
		//vertices.Add(newVertex);
		return newVertex;
	}
	*/

	public void MyTest(String name, float x, float y) {
		//Vertex vertex = addVertex(name,x,y);
		
		//JsonUtility.FromJson<Vertex>(JSON);
		//GameManager.instance.saveJSONFile("graph.json","{\"name\":\""+name+"\",\"x\":\""+x+"\",\"y\":\""+y+"\"}");
		
	}

	public void OnDrawGizmos() {
		int z = -20;
		
		foreach (Vertex vertex in vertices) {
			Gizmos.color = Color.green;
			Vector3 vector3 = new Vector3(vertex.getX(), vertex.getY(), z);
			Gizmos.DrawSphere(vector3,0.05f);

			for (int i=0; i<vertex.edges.Count;i++) {
				Gizmos.DrawLine(new Vector3(vertex.x,vertex.y,z), new Vector3(vertex.edges[i].to.x,vertex.edges[i].to.y,z));
			}
				
			
		}
	}
}

public class Edge
{
	public Vertex to;
	public float distance = 0;


	public Vertex getToVertex() {
		//throw new NotImplementedException();
		return to;
	}
}

[Serializable]
public class JSONList {
	public List<Vertex> vertices = new List<Vertex>();
}

[Serializable]
public class Vertex {
	public int id;
	public string name;
	public List<Edge> edges;
	public float distance;
	public float f;
	public Vertex predecessor;
	public float x;
	public float y;
	public int[] adjacency;

	public Vertex(int id, string name, float x, float y) {
		this.id = id;
		this.name = name;
		this.x = x;
		this.y = y;
	}


	public void setDistance(float distance) {
		this.distance = distance;
	}

	public void setPredecessor(Vertex predecessor)
	{
		this.predecessor = predecessor;
	}

	public void setF(float f)
	{
		this.f = f;
	}

	public float getF()
	{
		return f;
	}
	
	public void setX(float x) {
		this.x = x;
	}
	
	public void setY(float y) {
		this.y = y;
	}

	public float getX() {
		return x;
	}
	
	public float getY() {
		return y;
	}
}
