using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnableActors", menuName = "ScriptableObjects/ActorSettingsList", order = 1)]
public class ActorLevelSettings : ScriptableObject
{
    //actor ID and unique properties as used by all levels
    public Dictionary<int,ActorSpawnData> actorPrefabs;


    [System.Serializable]
    public struct ActorSpawnData
    {
        public GameObject prefab;
        public int spawnPointID;
        public Transform debugTarget;
        public Collider debugExitCollider;
        //behavior settings, like turnsearly, or runs faster, etc.
    }
}
