using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetGame : MonoBehaviour
{
 
    public void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

}
