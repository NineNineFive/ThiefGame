using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Data : MonoBehaviour {
	public static Data instance;
	public Graph graph;
	
	void Awake() {
		if(instance==null) instance = this;
		if(graph==null) graph = loadGraph(graph);
	}

	public Graph loadGraph(Graph graph) {
		// LOADS GRAPH FROM JSON DATA
		if(graph!=null){
			FileUpdater fileUpdater = new FileUpdater();
			String json = fileUpdater.loadJSONFile("graph.json");
			GraphData graphData = JsonUtility.FromJson<GraphData>(json);
			graph.vertices = graphData.vertices;
			
		}

		return graph;
	}
	
	public void saveGraph(Graph graph) {
		GraphData graphData = new GraphData();
		graphData.vertices = graph.vertices;
        
		string data = JsonUtility.ToJson(graphData, true);
		FileUpdater fileUpdater = new FileUpdater();
		bool status = fileUpdater.saveJSONFile("graph.json", data);
	}
}


[Serializable]
public class GraphData {
	public List<Vertex> vertices;
}


class FileUpdater {
	public String loadJSONFile (String fileName) {
		String JSON = System.IO.File.ReadAllText("Assets/Scripts/Data/"+fileName); //write string to file
		return JSON;
	}
    
	public bool saveJSONFile(String fileName, String JSONString) {
		try {
			System.IO.File.WriteAllText("Assets/Scripts/Data/"+fileName, JSONString); //write string to file
			return true;
		} catch (Exception e) {
			return false;
		}
	}
}