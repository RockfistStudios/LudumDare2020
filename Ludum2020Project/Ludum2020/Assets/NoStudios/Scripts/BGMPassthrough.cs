using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPassthrough : MonoBehaviour
{
    public UnityEngine.Events.UnityEvent onInvoke;

    public void ResumeBGMPassthrough()
    {
        onInvoke.Invoke();
    }

}
