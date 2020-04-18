using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    public bool runDebug = true;
    public int debugLevelID = 0;

    public ActorLevelSettings levelSpawns;
    public List<Transform> spawnPoints = new List<Transform>();
    public List<Transform> navTargets = new List<Transform>();

    public Animator actorManagerAnim;
    // Start is called before the first frame update
    void Start()
    {
        if(runDebug)
        {
            StartLevel(debugLevelID);
        }
    }

    // Update is called once per frame

    void Update()
    {

    }

    public void SpawnActor(int id)
    {
        ActorLevelSettings.ActorSpawnData spawnData = levelSpawns.actorPrefabs[id];
        spawnData.targetFromID = navTargets[spawnData.targetID];
        GameObject newActor = GameObject.Instantiate(spawnData.prefab, spawnPoints[spawnData.spawnPointID].position,Quaternion.identity);
        newActor.GetComponent<Actor>().OnSpawn(spawnData);
    }

    float timeSinceLevelStart = 0f;
    bool levelRunning = false;
    public void StartLevel(int id)
    {
        actorManagerAnim.SetInteger("PlayLevel",id);
        levelRunning = true;
    }

    public BoolEvent OnLevelEnded;
    public void LevelComplete(bool alive)
    {
        //level over, toasty is alive or dead.
        //this would play after the cart animation completes, or when toasty dies to an early exit.
        levelRunning = false;
        OnLevelEnded.Invoke(alive);
    }

    public void OnEarlyStop()
    {
        //toasty has died, stop spawns. 
    }
    public void CleanSpawns()
    {
        //removes all active spawns.
        //used for when the cart animation finishes, or when toasty has died.
    }

}
