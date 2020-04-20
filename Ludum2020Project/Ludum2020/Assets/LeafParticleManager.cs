using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafParticleManager : MonoBehaviour
{
    ParticleSystem leaves;
    public ParticleSystem flares;
    List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    // Start is called before the first frame update
    void Start()
    {
        leaves = gameObject.GetComponent<ParticleSystem>();
    }

    ParticleSystem.Particle[] particles = new ParticleSystem.Particle[10];
    public Transform toastyPassiveBurnPoint;
    private void Update()
    {
        int numParticles = leaves.GetParticles(particles);
        int i = 0;
        //
        while (i < particles.Length)
        {
            if (Vector3.Distance(particles[i].position, toastyPassiveBurnPoint.position) < 1f
                && !(Vector3.Distance(particles[i].position, Vector3.zero) < .1f)
                )
            {
                SpawnSmokePuff(particles[i].position, particles[i].velocity);
                particles[i].remainingLifetime = 0f;
            }
            i++;
        }
        leaves.SetParticles(particles);
    }


    //public void OnParticleCollision(GameObject other)
    //{
    //    int numCollisions = 0;
    //    if (other.gameObject.tag == "DebrisBurn")
    //    {
    //        numCollisions = leaves.GetCollisionEvents(other, collisionEvents);
    //        int i = 0;
    //        while (i < numCollisions)
    //        {
    //            Vector3 newVel = collisionEvents[i].velocity;
    //            Vector3 pos = collisionEvents[i].intersection;
    //            SpawnSmokePuff(pos, newVel);
    //            i++;
    //        }
    //    }
    //}
    public int fuelValue = 1;
    public void SpawnSmokePuff(Vector3 position,Vector3 velocity)
    {
        ToastyController.BurnedPassiveByToasty(fuelValue);
        Debug.LogWarning("smoke puff at =" + position.ToString());
        GameObject.Instantiate(flares, position,Quaternion.identity);
    }

}
