using System;
using System.Collections.Generic;
public class AStar {
    public Graph graph;
    private float infinity = Int32.MaxValue;
    private bool pathable;
    public List<Vertex> theResult;
    public float endDistance;
    public AStar(Graph graph) {
        this.graph = graph;
    }

    public void run(Vertex start, Vertex end) {
        String pathString = "";
        pathable = calculate(start, end);
        
        if (pathable) {
            Vertex resultCurrent = end;
            List<Vertex> Path = new List<Vertex>();
            Path.Add(end);
            while (resultCurrent != start && resultCurrent.predecessor != null) {
                resultCurrent = resultCurrent.predecessor;
                Path.Add(resultCurrent);
            }
            
            theResult = Path;

            foreach (Vertex vertex in theResult) {
                pathString += vertex.name;
            }
        }
    }

    public bool calculate(Vertex start, Vertex end) {
        if (start == null || end == null) return false;
        SortedSet<Vertex> openList = new SortedSet<Vertex>(new FCompare()); // 1
        List<Vertex> closedList = new List<Vertex>(); // 1
        openList.Add(start); // log(n)
        Vertex current; // 1
        List<Vertex> currentNeighbors; // 1
        // we use: f+g=h
        // f: is the distance to a node
        // g: is the weight of a node
        // h: is the total distance from start to the node

        foreach (Vertex vertex in graph.vertices) {
            vertex.g = infinity;
            vertex.h = Euclidean(vertex, end); // 1
        }
        
        start.g = 0; // 1
        start.calculateF(); // 1

        int looplimit = 0;
        while (openList.Count > 0 && looplimit<graph.vertices.Count) {
            looplimit += 1;
            // v
            current = openList.Min; // log(v)
            currentNeighbors = current.getNeighbours();
            openList.Remove(current);
            //Debug.Log("Remove from open list: " + current.name);
            if (current.name.Equals(end.name)) {
                endDistance = end.f;
                // 1
                return true;
            }
            closedList.Add(current);

            foreach (Vertex nextVertex in currentNeighbors) {
                // E
                //Debug.Log("We go to: " + nextVertex.name);
                float newG = 0; // newG
                // CALCULATE DISTANCE
                    newG = current.g + Euclidean(current, nextVertex);
                // IF NEW CALCULATED DISTANCE is less than CALCULATED DISTANCE ON VERTEX
                if (newG < nextVertex.g) {
                    //Debug.Log("We set previous to be: " + current.name);
                    nextVertex.predecessor = current;
                    nextVertex.g = newG;
                    nextVertex.f = nextVertex.calculateF();
                    //Debug.Log("We set the value: " + newG + ", to the vertex: " + nextVertex.name);
                    // IF closedlist and openlist does not contain vertex
                    if (!closedList.Contains(nextVertex) && !openList.Contains(nextVertex)) {
                        // n
                        openList.Add(nextVertex);
                    }
                    
                    // BINARY HEAP (HEAP SORTING)
                    if (openList.Contains(nextVertex)) {
                        openList.Remove(nextVertex);
                        openList.Add(nextVertex);
                    }
                }
            }
        }
        return false;
    }

    public float Euclidean(Vertex from, Vertex to) {
        float x = Math.Abs(to.x - from.x);
        float y = Math.Abs(to.y - from.y);
        float distance = (float)Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
        return distance;
    }
}

public class FCompare : IComparer<Vertex> {
    public int Compare(Vertex vertex1, Vertex vertex2) {
        try {
            return vertex1.f.CompareTo(vertex2.f);
        }
        catch (Exception e) {
            Console.WriteLine(e); // Write the error
            return 0; // If Error, return they are equal.
        }
    }
    
}
