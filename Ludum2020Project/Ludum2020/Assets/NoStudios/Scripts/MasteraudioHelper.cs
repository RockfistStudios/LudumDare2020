using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;

public class MasteraudioHelper : MonoBehaviour
{
    public string soundName = "name";
    
    public void PlaySoundRequest()
    {
        MasterAudio.PlaySound3DAtTransform(soundName,gameObject.transform);
    }
}
