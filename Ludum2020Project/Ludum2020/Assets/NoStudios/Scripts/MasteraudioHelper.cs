using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;

public class MasteraudioHelper : MonoBehaviour
{
    public Transform targetPos;

    public const string RabbitPanic = "RabbitPanic";
    public const string CampfireEnd = "CampfireEnd";
    public const string campSiteAmbience = "CampsiteAmbience";
    public const string MothBurn = "MothBurn";
    public const string RabbitMove = "RabbitMove";
    public const string RabbitMove100 = "RabbitMove_100";
    public const string Tcon1 = "ToastyConsume1";
    public const string Tcon2 = "ToastyConsume2";
    public const string ToastyIdle = "ToastyIdle";
    public const string WagonMoveFire = "WagonMovingAblaze";
    public const string WagonMove = "WagonMovingNormal";

    public void PlayRabbitPanic()
    {
        MasterAudio.PlaySound3DAtTransform(RabbitPanic,gameObject.transform);
    }

    public void PlayRabbitWalk()
    {
        MasterAudio.PlaySound3DAtTransform(RabbitMove, gameObject.transform);
    }

    public void PlayRabbitWalk100()
    {
        MasterAudio.PlaySound3DAtTransform(RabbitMove100, gameObject.transform);
    }


    public void PlayToastyConsume()
    {
        if (ToastyController.holdingThings)
        {
            MasterAudio.PlaySound3DAtTransform(Tcon1, gameObject.transform);
        }
    }

    public void StartToastyIdle()
    {
        MasterAudio.PlaySound3DAtTransform(ToastyIdle,targetPos);
    }
    public void StopToastyIdle()
    {
        MasterAudio.FadeOutAllOfSound(ToastyIdle, 1f);
    }
}
