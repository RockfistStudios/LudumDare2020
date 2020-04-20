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

    public List<Actor> activeActors;

    public Animator actorManagerAnim;
    // Start is called before the first frame update
    void Start()
    {
        if(runDebug)
        {
            StartLevel(debugLevelID,null);
        }
        ToastyController.instance.onToastyAliveChanged += OnToastyStateChange;
    }

    // Update is called once per frame

    void Update()
    {

    }

    public void OnToastyStateChange(bool alive)
    {
        toastyAlive = true;
        if(!alive)
        {
            //toasty died. stop spawns, play lose.
            toastyAlive = false;
        }
    }

    public void DoNothing()
    {

    }

    public void reEnableToastyInputs()
    {
        ToastyController.instance.EnableInput();
    }

    public void SpawnActor(int id)
    {
        ActorLevelSettings.ActorSpawnData spawnData = levelSpawns.actorPrefabs[id];
        spawnData.targetFromID = navTargets[spawnData.targetID];
        GameObject newActor = GameObject.Instantiate(spawnData.prefab, spawnPoints[spawnData.spawnPointID].position,Quaternion.identity);
        Actor actorRef = newActor.GetComponent<Actor>();
        actorRef.OnSpawn(spawnData);
        activeActors.Add(actorRef);
    }

    float timeSinceLevelStart = 0f;
    bool levelRunning = false;
    GameloopController.LevelCallback LevelCompleteCallback;
    public void StartLevel(int id,GameloopController.LevelCallback onLevelComplete)
    {
       
        actorManagerAnim.SetInteger("PlayLevel",id);
        levelRunning = true;
        LevelCompleteCallback = onLevelComplete;
        ToastyController.instance.StartFuelBurn();
    }

    public BoolEvent OnLevelEnded;
    bool m_toastyAlive = true;
    bool toastyAlive
    {
        get { return m_toastyAlive; }
        set { m_toastyAlive = value; actorManagerAnim.SetBool("ToastyAlive",value); }
    }

    public UnityEngine.Events.UnityEvent<bool> onLevelWinOutcome; //toasty will now survive the level if the cart is lit.
    //at this point, toasty cannot burn out normally.
    public void LevelSpawnEndSuccess()
    {
        //the outcome of the main level has been decided. toasty should live if he gets on the cart.
        ToastyController.instance.StopFuelBurn();
        //onLevelWinOutcome.Invoke(true);
    }
    public void LevelEndLoss()
    {
        //something has caused a lose condition in the level. such as toasty dying or missing the cart.
    }

    public Animator blackDropAnim;
    public void BlackScreen()
    {
        blackDropAnim.SetBool("Show", false);
        //clean spawns during this anim.
    }
    public void RemoveBlackScreen()
    {
        blackDropAnim.SetBool("Show", true);
        //clean spawns during this anim.
    }

    public void LevelComplete()
    {
        //level over, toasty is alive or dead.
        //this would play after the cart animation completes, or when toasty dies to an early exit.
        //this can be extended by adding a state after each level animation (level 0 ending) etc.
        levelRunning = false;
        OnLevelEnded.Invoke(toastyAlive);
        if (LevelCompleteCallback == null)
        {
            Debug.LogWarning("level ended. did not have a callback because it is running in debug mode");
        }
        else
        {
            // we can split the level ends here. toasty might be alive and miss the cart
            LevelCompleteCallback(toastyAlive);
        }
    }

    public void BeginLevelCloseout()
    {
        //for some useful NPC animations later
        Debug.Log("Level ending soon");
    }
    public void EndLevelCloseout()
    {
        Debug.Log("Level ending now");
        CleanSpawns();
    }

    public void CleanSpawns()
    {
        foreach(Actor a in activeActors)
        {
            a.RequestClear();
        }
        //removes all active spawns.
        //used for when the cart animation finishes, or when toasty has died.
    }

}
