using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    float hPos = 0f;
    public float speed = 3f;
    public float damping = 5;
    // Update is called once per frame
    public Animator anim;
    void Update()
    {
        float val = Actor.ActorAveragePos();

        val /= damping;
        val=Mathf.Clamp(val,-1,1);
        float distance = Mathf.Abs(Actor.avgPos.x);
        hPos = Mathf.MoveTowards(hPos, val, Time.deltaTime * speed * distance);
        anim.SetFloat("HPos",hPos);
    }

    public void OnEatAnimStart(bool start)
    {
        anim.SetBool("EatPose", start);
    }
    //void OnEatAnimEnd()
    //{
    //    anim.SetBool("EatPose", false);
    //}
}
