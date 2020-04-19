using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisDestroy : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("DebrisBurn"))
        {
            Destroy(this.gameObject);
           
        }
    }
    public float debrisLifeTime = 10f;
    private void Start()
    {
        Destroy(gameObject, debrisLifeTime);
    }


}