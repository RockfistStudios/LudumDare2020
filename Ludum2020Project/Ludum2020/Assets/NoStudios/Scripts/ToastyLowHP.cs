using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;

public class ToastyLowHP : MonoBehaviour
{
    public static ToastyLowHP instance;
    public string sweepGroupName= "Ending_Music_Failure_Stress";
    public int ultralowHP =8;
    

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ToastyController.instance.onToastyAliveChanged += ToastyAliveChange;
    }
    public void ToastyAliveChange(bool alive)
    {
        toastyAlive = alive;
    }
    bool toastyAlive = true;
    public void ToastyHPChange(int HP)
    {
        if(HP<ultralowHP && !playingSweep && toastyAlive)
        {
            ToastyUltralow();
        }
        else if(playingSweep && HP>ultralowHP)
        {
            ToastyHealed();
        }
    }

    bool playingSweep = false;
    public void ToastyUltralow()
    {
        if(playingSweep)
        {
            return;
        }
        //start sweep
        MasterAudio.PlaySound(sweepGroupName);
       
        playingSweep = true;
    }
    public void ToastyDead()
    {
        if (playingSweep)
        {
            playingSweep = false;
            //MasterAudio.fadeout
            MasterAudio.StopAllOfSound(sweepGroupName);
            MasterAudio.PlaySound3DAtTransform("MothBurn", ToastyController.instance.gameObject.transform);
            //stop audio
        }
        //play puff noise
    }
    public void ToastyHealed()
    {
        if (playingSweep)
        {
            MasterAudio.StopAllOfSound(sweepGroupName);
        }
        //stop sweep
        //play puff
    }

}
