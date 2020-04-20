using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;

public class BGMManager : MonoBehaviour
{
    public PlaylistController PC;
    float opv = 0f;
    public static BGMManager instance;

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
            PC.FadeToVolume(0, 1f);
        }
    }

    public void RestartBGMForLevel()
    {
        PC.StartPlaylist("Toasted_Restart");
        PC.FadeToVolume(1, .25f);
    }

    public void StartIntro()
    {
        PC.StartPlaylist("Toasted");
    }
    public void FadeIntroDown()
    {
        PC.FadeToVolume(0, 2f);
    }
    public void ResumeLoop()
    {
        PC.PlayNextSong();
        PC.FadeToVolume(1, .25f);
        //PC.PausePlaylist();
        //PC.PlayNextSong();
        //PC.UnpausePlaylist();
        //PC.PlayNextSong();
    }

    public void SetLoopByHealth(int level)
    {

    }


}
