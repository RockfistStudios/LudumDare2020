using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToastyController : MonoBehaviour
{
    public static ToastyController instance;
    public UnityEngine.Events.UnityAction<bool> onToastyAliveChanged;

    public Animator anim;
    public bool consumeFuel = false;
    public int m_fuel=50;
    public int fuel
    {
        get { return m_fuel; }
        set { m_fuel = value; anim.SetInteger("FuelLevel",value); toastyHPChange.Invoke(value); }
    }
    public IntEvent toastyHPChange;

    private void Awake()
    {
        instance = this;
    }

    float timeAccumulator=0f;
    private void Update()
    {
        if(consumeFuel)
        {
            timeAccumulator += Time.deltaTime;
            if(timeAccumulator>1f)
            {
                fuel -= 1;
                timeAccumulator = 0f;
            }
        }
    }

    public void StopFuelBurn()
    {
        consumeFuel = false;
    }
    public void StartFuelBurn()
    {
        consumeFuel = true;
    }

    public UnityEngine.Events.UnityEvent onToastyDieAnimStart;
    public void KillToasty()
    {
        Debug.LogWarning("toasty killed");
        //start playing death anim, music, etc.
        onToastyAliveChanged(false);
    }

    public void ResetToasty()
    {
        Debug.LogWarning("toasty reset");
        onToastyAliveChanged(true);
    }

}
