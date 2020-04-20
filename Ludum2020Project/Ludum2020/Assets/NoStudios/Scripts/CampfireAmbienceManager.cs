using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;

public class CampfireAmbienceManager : MonoBehaviour
{
    public string groupName = "CampsiteAmbience";
    public Transform target;

    PlaySoundResult psr;
    public void StartCampfire()
    {
        psr=MasterAudio.PlaySound3DAtTransform(groupName, target);
    }
    
    public void StopCampfire()
    {
        MasterAudio.StopAllOfSound(groupName);
    }

}
