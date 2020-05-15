using UnityEngine;

public class Path {
    public AStar aStar;
    public GameObject from;
    public GameObject to;
	
    public Path(Graph graph, GameObject from, GameObject to) {
        this.from = from;
        this.to = to;
        aStar = new AStar(graph);
    }
}