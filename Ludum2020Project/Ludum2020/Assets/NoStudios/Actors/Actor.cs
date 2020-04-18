using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    // Start is called before the first frame update
    public bool runDebug = true;
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
        
    }


    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;
    public void SetAnimatorSpeed()
    {
        Vector3 worldDeltaPosition = navAgent.nextPosition - transform.position;

        // Map 'worldDeltaPosition' to local space
        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        // Low-pass filter the deltaMove
        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

        // Update velocity if time advances
        if (Time.deltaTime > 1e-5f)
            velocity = smoothDeltaPosition / Time.deltaTime;

        bool shouldMove = velocity.magnitude > 0.5f && navAgent.remainingDistance > navAgent.radius;

        // Update animation parameters
        actorAnimator.SetBool("move", shouldMove);
        actorAnimator.SetFloat("velx", velocity.x);
        actorAnimator.SetFloat("vely", velocity.y);
    }

    public Collider exitPoint;
    public void OnSpawn(ActorLevelSettings.ActorSpawnData spawnInfo)
    {
        if(runDebug)
        {
            SetTarget(spawnInfo.debugTarget);
            exitPoint = spawnInfo.debugExitCollider;
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
    public void OnTriggerEnter(Collider other)
    {
        if (other == exitPoint)
        {
            //play exit animation
            //gtfo
            GameObject.Destroy(this.gameObject, 1f);
        }
    }

}
