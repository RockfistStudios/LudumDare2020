using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperTools 
{

}

[System.Serializable]
public class StringEvent : UnityEngine.Events.UnityEvent<string> { }
[System.Serializable]
public class IntEvent : UnityEngine.Events.UnityEvent<int> { }
[System.Serializable]
public class FloatEvent : UnityEngine.Events.UnityEvent<float> { }
[System.Serializable]
public class BoolEvent : UnityEngine.Events.UnityEvent<bool> { }


