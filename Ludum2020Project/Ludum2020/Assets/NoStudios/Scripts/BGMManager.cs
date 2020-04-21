using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;

public class BGMManager : MonoBehaviour
{
    public PlaylistController PC;
    float opv = 0f;
    public static BGMManager instance;

    public string highLoop;
    public string lowLoop;
    public string standardLoop;


    private void Start()
    {
        instance = this;
        ToastyController.instance.onToastyAliveChanged += aliveChanged;
    }
    bool isAlive = true;
    public void aliveChanged(bool alive)
    {
        isAlive = alive;
        if(!alive)
        {
            isPlaying = false;
            PC.FadeToVolume(0, 1f);
        }
    }

    public void RestartBGMForLevel()
    {
        PC.StartPlaylist("Toasted_Restart");
        isPlaying = true;
        PC.FadeToVolume(1, .25f);
    }

    bool isPlaying = false;

    public void StartIntro()
    {
        PC.StartPlaylist("Toasted");
        isPlaying = true;
    }
    public void FadeIntroDown()
    {
        isPlaying = false;
        PC.FadeToVolume(0, 2f);
    }
    public void ResumeLoop()
    {
        PC.PlayNextSong();
        PC.FadeToVolume(1, .25f);
        isPlaying = true;
        //PC.PausePlaylist();
        //PC.PlayNextSong();
        //PC.UnpausePlaylist();
        //PC.PlayNextSong();
    }

    public void SetLoopByHealth(int level)
    {
        if (level > 70)
        {
            //move to high
            if (PC.IsSongPlaying(standardLoop)||PC.IsSongPlaying(lowLoop))
            {
                //PC.start
            }
        }
        else if(level <=70 && level>30)
        {
            //stand
        }
        else
        {
            //low
        }
    }


}
