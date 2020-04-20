using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    float hPos = 0f;
    float speed = 3f;
    // Update is called once per frame
    public Animator anim;
    void Update()
    {
        float val = Actor.ActorAveragePos();

       
        if (val >0)
        {
            hPos = Mathf.MoveTowards(hPos,1,Time.deltaTime*speed);
        }
        else if(val<0)
        {
            hPos = Mathf.MoveTowards(hPos, -1, Time.deltaTime * speed);
        }
        else
        {
            hPos = Mathf.MoveTowards(hPos, 0, Time.deltaTime * speed);
        }
        anim.SetFloat("HPos",hPos);
    }
}
