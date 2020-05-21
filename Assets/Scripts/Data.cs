using System;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour {
	public static Data instance;
	public Graph graph;
	public string graphName;
	public GameObject player;
	private TextAsset jsonFile;
	void Awake() {
		if(instance==null) instance = this;
		jsonFile = Resources.Load<TextAsset>(graphName);
		graph = new Graph();
		graph = loadGraph(graphName);
		if(graph!=null){
			graph.calculateEdges();
		}
	}

	public Inventory getPlayerInventory() {
		if (player!=null) {
			PlayerController playerController = player.GetComponent<PlayerController>();
			if (playerController != null) {
				return playerController.inventory;
			}
		}
		return null;
	}
	
	public Graph loadGraph(string name) {
		jsonFile = Resources.Load<TextAsset>(name);
		if (jsonFile!=null) {
			// LOADS GRAPH FROM JSON DATA
			FileUpdater fileUpdater = new FileUpdater(jsonFile);
			String json = fileUpdater.loadJSONFile(name+".json");
			GraphData graphData = JsonUtility.FromJson<GraphData>(json);
			//Debug.Log(graph);
			//Debug.Log(graphData.vertices);
			if(graphData!=null && graphData.vertices!=null&&graph!=null){
				graph.vertices = graphData.vertices;
			}
			//Debug.Log(graph.vertices);

			return graph;
		} else return new Graph();
	}
	
	public void saveGraph(Graph graph, string name) {
		GraphData graphData = new GraphData();
		graphData.vertices = graph.vertices;
        
		string data = JsonUtility.ToJson(graphData, true);
		FileUpdater fileUpdater = new FileUpdater(jsonFile);
		bool status = fileUpdater.saveJSONFile(name+".json", data);
		if (status == true) {
			Debug.Log("Success");
		}
	}
}


[Serializable]
public class GraphData {
	public List<Vertex> vertices;
}


class FileUpdater {
	public TextAsset jsonFile;

	public FileUpdater(TextAsset jsonFile) {
		this.jsonFile = jsonFile;
	}
	public String loadJSONFile (String fileName) {
		#if UNITY_EDITOR
		TextAsset json = Resources.Load<TextAsset>("Assets/Resources/" + fileName);
		String JSON = System.IO.File.ReadAllText("Assets/Resources/"+fileName); //write string to file
		return JSON;
		#endif
		if (jsonFile == null)
			return null;
		else return jsonFile.text;
	}
    
	public bool saveJSONFile(String fileName, String JSONString) {
		try {
			System.IO.File.WriteAllText("Assets/Resources/"+fileName, JSONString); //write string to file
			return true;
		} catch (Exception e) {
			return false;
		}
	}
}