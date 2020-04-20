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
        ToastyController.instance.DisableInput();
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
   public void BlackScreen()
    {
        blackDropAnim.SetBool("Show", false);
        //clean spawns during this anim.
    }
    public void RemoveBlackScreen()
    {
        blackDropAnim.SetBool("Show", true);
        //clean spawns during this anim.
    }

    public void StartComic()
    {
        //tell the comic to turn on. it will handle its' own input progression
        ComicController.instance.ShowComic(onComicComplete);
    }
    public void AdvanceGameState()
    {
        gameloopStateAnim.SetTrigger("Advance");
    }


    public void StartLevel(int id)
    {
        if(id==0 || !levelOutcome)
        {
            ToastyController.instance.ResetToasty();
            //the level controller fades it early to remove spawns, it is brought back here.
            RemoveBlackScreen();
        }

        //if toasty died, restart.
        actorSpawner.StartLevel(id,LevelOver);
        //this will also send controls to toasty.
    }
    public void PlayBGAnimations()
    {

    }

    bool levelOutcome = true;
    public void LevelOver(bool victory)
    {
        gameloopStateAnim.SetBool("Victory", victory);
        //set gamestate to win/lose resets etc. retry current level if lost?
        if (victory)
        {
            levelOutcome = true;           
            AdvanceGameState();
        }
        else
        {
            levelOutcome = false;

            currentLevel -= 1;
            AdvanceGameState();
        }
        //called when the level end animation finishes in the actormanager.       
    }

    int currentLevel = 0;
    int maxLevel = 1;
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

    
    public void GameOver(bool isPacifist)
    {
        //fade to black, reset toasty, try again?
    }

    public Animator gameOverAnim;
    public void GameOverPerma(bool isPacifist)
    {
        gameOverAnim.SetBool("Restart", true);
        //special ending if not eating rabbit. restart last level (or first) if not.
    }


    //bool m_toastyEnabled;
    //bool toastyEnabled
    //{
    //    get { return m_toastyEnabled; }
    //    set
    //    {
    //        m_toastyEnabled = value;
    //        onToastyEnableChange.Invoke(value);
    //        //maybe add something to this so toasty can play special animations on different levels.
    //    }
    //}
    //public BoolEvent onToastyEnableChange;
    //public void EnableToasty(bool enable)
    //{
    //    if (m_toastyEnabled != enable)
    //    {
    //        toastyEnabled = enable;
    //    }
    //}
}
