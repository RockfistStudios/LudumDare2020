using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;

public class BGMManager : MonoBehaviour
{
    public PlaylistController PC;
    float opv = 0f;
    public void StartIntro()
    {
        opv = PC.PlaylistVolume;
        PC.StartPlaylist("Toasted");
    }
    public void FadeIntroDown()
    {
        PC.FadeToVolume(0, 1f);
    }
    public void ResumeLoop()
    {
        PC.PlaylistVolume = opv;
        PC.PausePlaylist();
        PC.PlayNextSong();
        PC.UnpausePlaylist();
        //PC.PlayNextSong();
    }

    public void SetLoopByHealth(int level)
    {

    }


}
