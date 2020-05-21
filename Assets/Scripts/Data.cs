using System;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour {
	public static Data instance;
	public Graph graph;
	public string graphName;
	public GameObject player;

	void Awake() {
		if(instance==null) instance = this;
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
		if (name!="") {
			// LOADS GRAPH FROM JSON DATA
			FileUpdater fileUpdater = new FileUpdater();
			String json = fileUpdater.loadJSONFile(name);
			GraphData graphData = JsonUtility.FromJson<GraphData>(json);
			if(graphData!=null && graphData.vertices!=null&&graph!=null){
				graph.vertices = graphData.vertices;
			}

			return graph;
		} else return new Graph();
	}
	
	public void saveGraph(Graph graph, string name) {
		GraphData graphData = new GraphData();
		graphData.vertices = graph.vertices;
        
		string data = JsonUtility.ToJson(graphData, true);
		FileUpdater fileUpdater = new FileUpdater();
		bool status = fileUpdater.saveJSONFile(name, data);
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
	public string loadJSONFile (string fileName) {
		#if UNITY_EDITOR
			if (System.IO.File.Exists("Assets/Resources/" + fileName + ".json")) {
				string JSON = System.IO.File.ReadAllText("Assets/Resources/" + fileName + ".json"); //write string to file
				return JSON;
			} else return "";
		#endif
		#if !UNITY_EDITOR
			TextAsset json = Resources.Load<TextAsset>(fileName);
			if (json == null)
				return null;
			else return json.text;
		#endif
	}
    
	public bool saveJSONFile(string fileName, string json) {
		try {
			System.IO.File.WriteAllText("Assets/Resources/" + fileName + ".json", json); //write string to file
			return true;
		} catch (Exception e) {
			return false;
		}
	}
}