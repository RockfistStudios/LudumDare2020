using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    public List<ActorLevelSettings> levels = new List<ActorLevelSettings>();
    public ActorLevelSettings levelSpawns;
    public List<Transform> spawnPoints = new List<Transform>();

    public Animator actorManagerAnim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnActor(int id)
    {
        ActorLevelSettings.ActorSpawnData spawnData = levelSpawns.actorPrefabs[id];
        GameObject newActor = GameObject.Instantiate(spawnData.prefab, spawnPoints[spawnData.spawnPointID].position,Quaternion.identity);
    }

    float timeSinceLevelStart = 0f;
    bool runLevel = false;
    public void StartLevel(int id)
    {
        runLevel = true;
    }

    public BoolEvent OnLevelEnded;
    public void LevelComplete(bool alive)
    {
        //level over, toasty is alive or dead.
        runLevel = false;
        OnLevelEnded.Invoke(alive);
    }

}
