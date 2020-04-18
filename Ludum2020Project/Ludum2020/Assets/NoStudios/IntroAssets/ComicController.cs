using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComicController : MonoBehaviour
{
    public static ComicController instance;
    public Animator comicAnim;
    // Update is called once per frame

    private void Awake()
    {
        instance = this;
    }
    
    void Update()
    {
        
        if(comicShown)
        {
            if(Input.GetMouseButton(0))
            {
                comicAnim.SetTrigger("AdvanceComic");
            }
        }
    }

    bool comicShown = false;
    GameloopController.ComicCallback onComicCompleteCallback;
    public void ShowComic(GameloopController.ComicCallback onComplete)
    {
        comicShown = true;
        onComicCompleteCallback = onComplete;
        comicAnim.SetBool("ShowComic",true);
    }

    public void FinishComic()
    {
        comicShown = false;
        comicAnim.ResetTrigger("AdvanceComic");
        comicAnim.SetBool("ShowComic", false);
        onComicCompleteCallback();
    }
}
