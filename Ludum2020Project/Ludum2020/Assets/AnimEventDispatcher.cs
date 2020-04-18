using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventDispatcher : MonoBehaviour
{
    public UnityEngine.Events.UnityEvent onAnim;

    void AnimationEventInvoke()
    {
        onAnim.Invoke();
    }
}
