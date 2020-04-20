using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopActor : Actor
{
    public float stopAtSeconds = 5f;
    float counter = 0f;
    //walks this long, stops indefinitely.
    bool sentStop = false;
    public override void Update()
    {
        if (!sentStop && !onFire)
        {
            counter += Time.deltaTime;
            if(counter>stopAtSeconds)
            {
                navAgent.speed = 0f;
            }
        }

        base.Update();

    }
}
