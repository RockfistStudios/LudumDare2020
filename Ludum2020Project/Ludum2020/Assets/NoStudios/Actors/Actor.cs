﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    // Start is called before the first frame update
    public bool runDebug = false;
    public ActorLevelSettings.ActorSpawnData debugSpawnInfo;
    public UnityEngine.AI.NavMeshAgent navAgent;
    public int startingFuelWorth=0;

    public Animator actorAnimator;

    public bool inKillRange = false;
    void Start()
    {
        if (runDebug)
        {
            OnSpawn(debugSpawnInfo);
        }
    }

    public void RequestClear()
    {
        if(!exiting)
        {
            GameObject.Destroy(this.gameObject, 1f);
            exiting = true;
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
        startingFuelWorth = spawnInfo.fuelWorth;
        //start anims etc
    }

    Transform navTarget;
    public void SetTarget(Transform target)
    {
        navTarget = target;
        navAgent.SetDestination(target.position);
    }



    float exitImmunity = 3f;
    bool exiting = false;
    bool beingEaten;
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ExitPoint" && exitImmunity<=0)
        {
            //play exit animation
            //gtfo
            exiting = true;
            GameObject.Destroy(this.gameObject, 1f);
        }
        else if(other.tag == "ToastyKillRange")
        {
            navAgent.updatePosition = false;
            navAgent.updateRotation = false;
            //snap this objects hang point to toasty's kill transform
            //potentially use a physics joint for this
            ToastyController.instance.CaughtByToasty(startingFuelWorth,OnToastyKillComplete);

        //we turn toasty's controller on when he starts his eat animation. that frame, everything inside should
        //be caught, and alert toasty he is holding them
        //when toasty finishes his eat, they will receive a callback to remove themselves.
        }
    }

    public void OnToastyKillComplete()
    {
        Debug.LogWarning("Toasty ate the things");
        //crunch!@
        //this object should clean itself and make whatever sound/fx needed.
    }

}
