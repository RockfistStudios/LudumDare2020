using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    // Start is called before the first frame update
    public bool runDebug = false;
    public ActorLevelSettings.ActorSpawnData debugSpawnInfo;
    public UnityEngine.AI.NavMeshAgent navAgent;

    public Animator actorAnimator;

    void Start()
    {
        if (runDebug)
        {
            OnSpawn(debugSpawnInfo);
        }
    }

    // Update is called once per frame
    void Update()
    {
        SetAnimatorSpeed();
        if(exitImmunity>0)
        {
            exitImmunity -= Time.deltaTime;
        }
    }


    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;
    Vector3 lastPos = new Vector3();
    public void SetAnimatorSpeed()
    {
        Vector3 worldDeltaPosition = lastPos-gameObject.transform.position;
        lastPos = gameObject.transform.position;
        // Map 'worldDeltaPosition' to local space
        float dy = Vector3.Dot(transform.right, worldDeltaPosition);
        float dx = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        // Low-pass filter the deltaMove
        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

        // Update velocity if time advances
        if (Time.deltaTime > 1e-5f)
            velocity = smoothDeltaPosition / Time.deltaTime;

        bool shouldMove = velocity.magnitude > 0.1f && navAgent.remainingDistance > navAgent.radius;

        // Update animation parameters
        actorAnimator.SetBool("move", shouldMove);
        actorAnimator.SetFloat("velx", velocity.x);
        actorAnimator.SetFloat("vely", velocity.y);
    }

    public void OnSpawn(ActorLevelSettings.ActorSpawnData spawnInfo)
    {
        lastPos = gameObject.transform.position;
        if(runDebug)
        {
            SetTarget(spawnInfo.debugTarget);
        }
        else
        {
            SetTarget(spawnInfo.targetFromID);
        }
        //start anims etc
    }

    Transform navTarget;
    public void SetTarget(Transform target)
    {
        navTarget = target;
        navAgent.SetDestination(target.position);
    }

    bool nearToasty = false;
    float exitImmunity = 3f;
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ExitPoint" && exitImmunity<=0)
        {
            //play exit animation
            //gtfo
            GameObject.Destroy(this.gameObject, 1f);
        }
    }

}
