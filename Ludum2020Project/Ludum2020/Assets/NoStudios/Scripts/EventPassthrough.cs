using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventPassthrough : MonoBehaviour
{
    public UnityEngine.Events.UnityEvent onInvoke;

    public void PlayPuff()
    {
        onInvoke.Invoke();
    }

}
