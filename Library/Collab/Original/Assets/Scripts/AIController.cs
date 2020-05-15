using System;
using System.Collections;
using UnityEngine;

public class AIController : MonoBehaviour {
    private GuardBehaviour guardBehaviour;
    public GameObject target;
    public Graph graph;
    public Path path;
    private float targetRange = 1.5f;
    private float speed = 0.9f;
    private float halt = 2f;
    private State state; //statemachine
    private Vector3 startingPos;
    private Vector3 position;
    private IEnumerator corutineB;

    private enum State {
        Guarding,
        ChasingTarget,
        Caught
    }

    // Start is called before the first frame update
    void Awake() {
        startingPos = transform.position;
        target = GameObject.Find("Player");
        state = State.Guarding;
        
        try {
            guardBehaviour = GetComponent<GuardBehaviour>();
            if (guardBehaviour == null) throw new MissingComponentException();
        } catch (MissingComponentException componentException) {
            gameObject.AddComponent(typeof(GuardBehaviour));
            Debug.LogWarning("Couldn't find a guard behavior on object: "+gameObject.name+"... Therefore script added the guardbehavior component");
        }

        try {
            if (graph == null || target == null) throw new NullReferenceException();
        } catch (NullReferenceException nullException) {
            Debug.LogWarning("Graph or target might not be set in inspector");
        }
        
        path = new Path(graph, gameObject, target);
    }

    // Update is called once per frame
    void FixedUpdate() {
        path.aStar.graph = graph;
        FindPath(path, path.from, path.to);

        switch (state) {
            case State.Guarding:
                Guarding();
                break;
            case State.ChasingTarget:
                ChasingTarget();
                break;
            case State.Caught:
                Caught();
                break;
        }
    }

    private void Guarding() {
        if (FindTarget(targetRange)) {
            state = State.ChasingTarget;
        } else {
            corutineB = Return(halt);
            StartCoroutine(corutineB);
            state = State.Guarding;
        }
    }

    private void ChasingTarget() {
        Vertex pVertex = PlayerPath(path);
        
        if (!FindTarget(2f) || pVertex==null) {
            state = State.Guarding;
        }
        
        if (target != null) {
            guardBehaviour.Turn(target);
        }

        if (FindTarget(0.2f)) {
            state = State.Caught;
        }
        
        if (pVertex != null)
        {
            position = transform.position;
            Vector3 target = new Vector3(pVertex.x, pVertex.y, 0);
            guardBehaviour.Move2(position, target, speed);
        }
    }

    private void Caught() {
        Debug.Log("Caught");
    }

    private bool FindTarget(float range) {
        if (target != null && guardBehaviour != null) {
            if (Vector2.Distance(guardBehaviour.transform.position, target.transform.position) < range) {
                return true;
            }
        }
        return false;
    }

    private void FindPath(Path path, GameObject startObj, GameObject endObj) {
        Vertex start = new Vertex("Start", startObj.transform.position.x, startObj.transform.position.y);
        Vertex end = new Vertex("End", endObj.transform.position.x, endObj.transform.position.y);
        
        path.aStar.graph = graph;
        path.aStar.graph.vertices.Add(start);
        path.aStar.graph.vertices.Add(end);

        // TODO: implement calculateEdges only for start to all vertices and all vertices to end
        graph.calculateEdges();

        path.aStar.run(start, end);

        path.aStar.graph.vertices.Remove(start);
        path.aStar.graph.vertices.Remove(end);
    }

    private Vertex PlayerPath(Path path) {
        //bool isEmpty = path.aStar.theResult.Any();
        if (path.aStar.theResult != null) {
            int count = path.aStar.theResult.Count;
            if(count>=2){
                return path.aStar.theResult[count - 2];
            }
        }
        state = State.Guarding;
        return null;
    }

    private IEnumerator Return(float sec) {
        position = transform.position;
        guardBehaviour.Move2(position, startingPos, speed);
        yield return new WaitForSeconds(sec);
    }
}