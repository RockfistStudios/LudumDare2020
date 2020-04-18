using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameloopController : MonoBehaviour
{
    public delegate void ComicCallback();
    ComicCallback onComicComplete;

    public Animator gameloopStateAnim;


    private void Awake()
    {
        gameloopStateAnim = gameObject.GetComponent<Animator>();
        onComicComplete = ComicOver;
    }
    private void Start()
    {
        //fancy loads, etc.
        StartGame();
    }

    void StartGame()
    {
        AdvanceGameState();
    }

    public Animator blackDropAnim;
    public void ShowBlackdrop(bool show)
    {
        blackDropAnim.SetBool("Show",show);
    }

    public void StartComic()
    {
        //tell the comic to turn on. it will handle its' own input progression
        ComicController.instance.ShowComic(onComicComplete);
    }
    void AdvanceGameState()
    {
        gameloopStateAnim.SetTrigger("Advance");
    }

    public void ComicOver()
    {
        //when the comic ends, let this controller know so the game can move forward.
        AdvanceGameState();
        ShowBlackdrop(false);
    }


    bool m_toastyEnabled;
    bool toastyEnabled
    {
        get { return m_toastyEnabled; }
        set
        {
            m_toastyEnabled = value;
            onToastyEnableChange.Invoke(value);
        }
    }
    public BoolEvent onToastyEnableChange;
    public void EnableToasty(bool enable)
    {
        if (m_toastyEnabled != enable)
        {
            m_toastyEnabled = enable;
        }
    }
}
