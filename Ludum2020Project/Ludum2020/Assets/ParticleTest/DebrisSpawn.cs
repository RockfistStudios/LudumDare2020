using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisSpawn : MonoBehaviour
{

    public Transform leafprefab;
    public float leafSpeed = 5f;
   
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 3; i++)
            Instantiate(leafprefab, new Vector3(Random.Range(-6, 6), 8, 2), Quaternion.identity);

    }

    // Update is called once per frame
    void Update()
        
    {

       
    }
}
