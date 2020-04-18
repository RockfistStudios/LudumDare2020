using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameloopController : MonoBehaviour
{
    public delegate void ComicCallback();
    ComicCallback onComicComplete;

    public delegate void LevelCallback(bool win);
    LevelCallback onLevelComplete;

    public Animator gameloopStateAnim;

    public ActorController actorSpawner;


    private void Awake()
    {
        gameloopStateAnim = gameObject.GetComponent<Animator>();
        onComicComplete = ComicOver;
        currentLevel = 0;
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


    public void StartLevel(int id)
    {
        actorSpawner.StartLevel(id,LevelOver);
        //this will also send controls to toasty.
    }
    public void PlayBGAnimations()
    {

    }
    public void LevelOver(bool victory)
    {
        //set gamestate to win/lose resets etc. retry current level if lost?
        gameloopStateAnim.SetBool("Victory",victory);
        AdvanceGameState();
        //called when the level end animation finishes in the actormanager.       
    }

    int currentLevel = 0;
    int maxLevel = 0;
    public bool lastLevelLoops = true;
    public void StartNextLevel()
    {
        if (currentLevel >= maxLevel)
        {
            //end game instead, play winscreen?
            if (lastLevelLoops)
            {
                StartLevel(maxLevel);
            }
        }
        else
        {
            currentLevel++;
            StartLevel(currentLevel);
        }
    }
    public void LevelAlert()
    {
        //empty method because we need a damn anim event
    }

    public void ComicOver()
    {
        //when the comic ends, let this controller know so the game can move forward.
        AdvanceGameState();
        ShowBlackdrop(true);
    }


    bool m_toastyEnabled;
    bool toastyEnabled
    {
        get { return m_toastyEnabled; }
        set
        {
            m_toastyEnabled = value;
            onToastyEnableChange.Invoke(value);
            //maybe add something to this so toasty can play special animations on different levels.
        }
    }
    public BoolEvent onToastyEnableChange;
    public void EnableToasty(bool enable)
    {
        if (m_toastyEnabled != enable)
        {
            toastyEnabled = enable;
        }
    }
}
