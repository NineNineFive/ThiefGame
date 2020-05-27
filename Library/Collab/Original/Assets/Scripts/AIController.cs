using System;
using System.Collections;
using UnityEngine;

public class AIController : MonoBehaviour {
    private HumanBehaviour humanBehaviour;
    public Graph graph;
    public Path path;
    private Vector3 startingPos;
    private Quaternion startingRot;
    public FieldOfView fov;
    private float timer = 0;
    public Target heardTarget = null;
    private Target target = null;
    public float hearingRadius = 5f;
    private bool didHey = false;
    private bool move = false;

    // Start is called before the first frame update
    void Awake() {
        startingPos = transform.position;
        startingRot = transform.rotation;
        Data data = Data.instance;
        graph = data.graph;

        try {
            humanBehaviour = GetComponent<HumanBehaviour>();
            if (humanBehaviour == null) throw new MissingComponentException();
        } catch (MissingComponentException componentException) {
            gameObject.AddComponent(typeof(HumanBehaviour));
            Debug.LogWarning("Couldn't find a guard behavior on object: "+gameObject.name+"... Therefore script added the guardbehavior component");
        }

        try {
            if (data.graph == null) throw new NullReferenceException();
        } catch (NullReferenceException nullException) {
            Debug.LogWarning("Graph or target might not be set in inspector");
        }

        path = new Path(data.graph, transform.position, new Vector3(0,0,0));

        heardTarget = null;
    }

    void Start() {
        StartCoroutine(AILogic());
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (move) {
            moveTowardsTarget();
        }
    }

    private IEnumerator AILogic() {
        while(true){
            // IF SEE THIEF
            if (fov.target != null) {
                // TARGET IS THIEF
                target = new Target(fov.target.transform.position);
                if(!didHey&&!AudioManager.instance.isPlaying("Hey")){
                    AudioManager.instance.Play("Hey");
                    didHey = true;
                }
                if (Vector2.Distance(transform.position,target.pos)<0.2f) {
                    caught();
                }
                timer = 0;
            } else {
                didHey = false;
                // IF HEARD SOUND
                // TARGET IS SOUND
                if(heardTarget!=null) {
                    path.from = transform.position;
                    path.to = heardTarget.pos;
                    findPath(path, path.from, path.to);
                    if (path.aStar.endDistance < hearingRadius) {
                        // guard can hear
                        if (waitSecs(7f/5)) {
                            heardTarget = null;
                            target = null;
                        } else {
                            target = heardTarget;
                        }
                    } else {
                        // guard cant hear
                        heardTarget = null;
                        target = null;
                    }
                } else {
                    // IF NO TARGET
                    if (waitSecs(3f/5)) {
                        //target = null;
                        target = new Target(startingPos,startingRot);
                    }
                }
            }
            
            // IF TARGET EXISTS
            if (target != null) {
                path.aStar.graph = graph;
                path.from = transform.position;
                path.to = target.pos;
                findPath(path, transform.position, path.to);
                if (Vector2.Distance(transform.position, target.pos) > 0.1f) {
                    move = true;
                } else {
                    move = false;
                    transform.rotation = startingRot;
                }
            }
            yield return new WaitForSeconds(0.1f); // AI RUNS PATHFINDING 10 times a second
        }
        yield return 0;
    }


    private bool waitSecs(float seconds) {
        if (timer >= seconds) {
            timer = 0;
            return true;
        }
        
        timer += Time.deltaTime;
        return false;
    }

    private void caught() {
        AudioManager.instance.Play("Caught");
        GameManager.getInstance().caught = true;
        GameManager.getInstance().endingPrevented = false;
        GameManager.getInstance().endGameForce();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //Application.LoadLevel("Menu");
    }

    private void moveTowardsTarget() {
        if(path.aStar.theResult!=null&&target!=null){
            int count = path.aStar.theResult.Count;
            if(count>=1){
                Vector2 waypointPos = new Vector2(0,0);

                if (path.aStar.theResult != null) {
                    if(count>=2){
                        Vertex next = path.aStar.theResult[count - 2];
                        waypointPos = new Vector2(next.x,next.y);
                        // TURN SMOOTH
                        float strength = 5;
                        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, target.pos - transform.position);
                        float str = Mathf.Min(strength * Time.deltaTime, 1);
                        Quaternion rotation = Quaternion.Lerp(transform.rotation, targetRotation, str);
                        if(path.aStar.endDistance>0.01f){
                            humanBehaviour.AITurn(rotation);
                        }
                        //humanBehaviour.turn(target.pos);
                    }
                }
                humanBehaviour.AIMove(transform.position,waypointPos);
            }
        }

    }
    
    private void findPath(Path path, Vector3 startPos, Vector3 endPos) {
        Data data = Data.instance;
        if(data.graph !=null){
            path.aStar.graph = data.graph;
            
            Vertex start = new Vertex("Start", startPos.x, startPos.y);
            Vertex end = new Vertex("End", endPos.x, endPos.y);
            
            
            // TODO: move start, remove and calculate elsewhere! like in the start of the AI controller and on doors when they open
            path.aStar.graph.vertices.Add(start);
            path.aStar.graph.vertices.Add(end);
            
            data.graph.calculateEdges();

            path.aStar.run(start, end);

            path.aStar.graph.vertices.Remove(start);
            path.aStar.graph.vertices.Remove(end);
            
        }
    }

    public void listenSounds(Vector2 soundPos) {
        heardTarget = new Target(soundPos,Quaternion.identity);
    }
    
}

public class Target {
    public GameObject obj;
    public Vector3 pos;
    public Quaternion rot;

    public Target(Vector3 pos) {
        this.pos = pos;
    }
    
    public Target(Vector3 pos, Quaternion rot) {
        this.pos = pos;
        this.rot = rot;
    }

}