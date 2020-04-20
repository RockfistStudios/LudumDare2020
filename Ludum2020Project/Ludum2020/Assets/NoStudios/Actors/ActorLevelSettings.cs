using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnableActors", menuName = "ScriptableObjects/ActorSettingsList", order = 1)]
public class ActorLevelSettings : ScriptableObject
{
    //actor ID and unique properties as used by all levels
    public List<ActorSpawnData> actorPrefabs;


    [System.Serializable]
    public struct ActorSpawnData
    {
        public GameObject prefab;
        public int spawnPointID;
        public int targetID; //the spawn manager uses this number to pick from the list of transforms
        public Transform targetFromID; //this is set by the spawn manager before being sent to the actor itself.
        public int fuelWorth;
        public float panicSpeed;
        public float speedMult;
        public float exitImmunity;
        public Transform debugTarget;
        public Collider debugExitCollider;
        //behavior settings, like turnsearly, or runs faster, etc.
    }
}
