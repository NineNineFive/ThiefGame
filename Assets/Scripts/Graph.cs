using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class Graph : MonoBehaviour {
	public List<Vertex> vertices;
	
	
	public void Awake() {
		if(Data.instance!=null) Data.instance.loadGraph(this);
	}

	public void Start() {
		//fromTos = new List<FromTo>();
		if(Data.instance!=null) Data.instance.loadGraph(this);
		calculateEdges();
	}


	

	public void remove(string name) {
		// REMOVE VERTEX
		foreach (Vertex vertex in vertices.ToList()) {
			if (vertex.name == name) {
				vertices.Remove(vertex);
			}
		}
		
	}
    
	public void add(string name, float x, float y) {
		Vertex vertex = new Vertex(name,x,y);
		vertices.Add(vertex);
	}
    
	public void clear() {
		vertices.Clear();
	}
	
	public void calculateEdges() {
		foreach (Vertex vertex1 in vertices) {
			vertex1.edges = new List<Edge>();
			foreach (Vertex vertex2 in vertices) {
				// We should not add edges to the node itself
				if(!vertex1.Equals(vertex2)){
                    
					// Does the ray intersect any objects excluding the player layer
					Vector2 position1 = new Vector2(vertex1.x, vertex1.y);
					Vector2 position2 = new Vector2(vertex2.x, vertex2.y);
					Vector2 direction = position1 - position2;
                    
					// Cast a ray straight down.
					RaycastHit2D[] hit = Physics2D.LinecastAll(position1, position2);
					//Debug.Log(hit.Length);
					// If it hits something...
					bool isHit = false;
					foreach(RaycastHit2D raycastHit2D in hit){
						if (raycastHit2D.collider.CompareTag("Obstacle")) {
							isHit = true;
						}
					}

					if (!isHit) {
						Edge edge = new Edge(vertex1,vertex2);
						vertex1.edges.Add(edge);
					}
				}
			}
		}
	}
}

public class Edge {
	public Vertex from;
	public Vertex to;

	public Edge(Vertex from, Vertex to){
		this.from = from;
		this.to = to;
	}

	public Vertex getToVertex() {
		return to;
	}
}

[Serializable]
public class Vertex {
	public string name;
	public List<Edge> edges;
	[NonSerialized] public float distance;
	[NonSerialized] public float f;
	[NonSerialized] public float g;
	[NonSerialized] public float h;
	[NonSerialized] public Vertex predecessor;
	public float x;
	public float y;

	public Vertex(string name, float x, float y) {
		this.name = name;
		this.x = x;
		this.y = y;
		edges = new List<Edge>();
	}

	public float calculateF() {
		return g + h;
	}

	public List<Vertex> getNeighbours() {
		List<Vertex> neighbours = new List<Vertex>();
		foreach (Edge edge in edges)
		{
			neighbours.Add(edge.getToVertex());
		}

		return neighbours;
	}
}
