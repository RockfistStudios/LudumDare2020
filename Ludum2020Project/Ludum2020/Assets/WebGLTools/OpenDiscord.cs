using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDiscord : MonoBehaviour
{
    public string discordURL;
    public void OpenDiscordURL()
    {
        try
        {
            Application.OpenURL(discordURL);
        }
        catch
        {
            Debug.Log("couldn't open url!");
        }

    }

}
