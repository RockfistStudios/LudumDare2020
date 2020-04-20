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
    public const string Tcon1 = "ToastyConsume1";
    public const string Tcon2 = "ToastyConsume2";
    public const string ToastyIdle = "ToastyIdle";
    public const string WagonMoveFire = "WagonMovingAblaze";
    public const string WagonMove = "WagonMovingNormal";

    public void PlayRabbitPanic()
    {
        MasterAudio.PlaySound3DAtTransform(RabbitPanic,gameObject.transform);
    }

    public void PlayToastyConsume()
    {
        MasterAudio.PlaySound3DAtTransform(Tcon1, gameObject.transform);
    }


}
