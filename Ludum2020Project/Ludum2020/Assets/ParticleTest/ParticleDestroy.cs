using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroy : MonoBehaviour
{
    public float liveTime = 3f;
    private void Start()
    {
        GameObject.Destroy(this.gameObject, liveTime);
    }
}
