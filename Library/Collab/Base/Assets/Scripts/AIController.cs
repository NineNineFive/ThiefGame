using System;
using System.Collections;
using UnityEngine;

public class AIController : MonoBehaviour {
    private HumanBehaviour humanBehaviour;
    public GameObject target;
    public Graph graph;
    public Path path;
    private float targetRange = 1.5f;
    private float speed = 0.9f;
    private float halt = 2f;
    private State state; //statemachine
    private Vector3 startingPos;
    private Vector3 position;
    public FieldOfView fov;
        

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
        Data data = Data.instance;

        try {
            humanBehaviour = GetComponent<HumanBehaviour>();
            if (humanBehaviour == null) throw new MissingComponentException();
        } catch (MissingComponentException componentException) {
            gameObject.AddComponent(typeof(HumanBehaviour));
            Debug.LogWarning("Couldn't find a guard behavior on object: "+gameObject.name+"... Therefore script added the guardbehavior component");
        }

        try {
            if (data.graph == null || target == null) throw new NullReferenceException();
        } catch (NullReferenceException nullException) {
            Debug.LogWarning("Graph or target might not be set in inspector");
        }

        path = new Path(data.graph, gameObject, target);
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
        if (FindTarget(targetRange)||fov.target!=null) {
            state = State.ChasingTarget;
        } else {
            state = State.Guarding;
        }
    }
    
    private void ChasingTarget() {
        if(fov.target!=null){
            path.to = fov.target.gameObject;
        }
        
        MoveTowardsTarget();
        if (!FindTarget(0.5f)) {
            state = State.Guarding;
        }

        if (FindTarget(0.2f)) {
            state = State.Caught;
        }
    }
    
    private void Caught() {
        Application.LoadLevel(0);
        //Debug.Log("Caught");
    }

    private void MoveTowardsTarget() {
        Vector2 waypointPos = getNextWaypointPos();
        Vector2 targetPos = getTarget();
        humanBehaviour.turn(targetPos);
        humanBehaviour.AIMove(transform.position,waypointPos);
    }
    

    private Vector2 getNextWaypointPos() {
        if (path.aStar.theResult != null) {
            int count = path.aStar.theResult.Count;
            if(count>=2){
                Vertex next = path.aStar.theResult[count - 2];
                return new Vector2(next.x,next.y);
            }
        } 
        return new Vector2(0,0);
    }

    private Vector2 getTarget() {
        int count = path.aStar.theResult.Count;
        if(count>=1){
            Vertex target = path.aStar.theResult[0];
            return new Vector2(target.x,target.y);
        }

        return new Vector2(0,0);
    }


    private bool FindTarget(float range) {
        if (target != null && humanBehaviour != null) {
            if (Vector2.Distance(humanBehaviour.transform.position, target.transform.position) < range) {
                return true;
            }
        }
        return false;
    }
    private void FindPath(Path path, GameObject startObj, GameObject endObj) {
        Data data;
        if(Data.instance !=null){
            data = Data.instance;
                
            Vertex start = new Vertex("Start", startObj.transform.position.x, startObj.transform.position.y);
            Vertex end = new Vertex("End", endObj.transform.position.x, endObj.transform.position.y);
            path.aStar.graph = data.graph;
            
            // TODO: move start, remove and calculate elsewhere! like in the start of the AI controller and on doors when they open
            path.aStar.graph.vertices.Add(start);
            path.aStar.graph.vertices.Add(end);
            
            data.graph.calculateEdges();

            path.aStar.run(start, end);

            path.aStar.graph.vertices.Remove(start);
            path.aStar.graph.vertices.Remove(end);
            
        }
    }
}