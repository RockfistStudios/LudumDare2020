using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToastyController : MonoBehaviour
{
    public static ToastyController instance;
    public UnityEngine.Events.UnityAction<bool> onToastyAliveChanged;

    public Animator anim;
    public bool consumeFuel = false;
    public int toastyEatFuelAmount=0;
    public int m_fuel=50;
    public int fuel
    {
        get { return m_fuel; }
        set { m_fuel = value; anim.SetInteger("FuelLevel",value); toastyHPChange.Invoke(value); }
    }
    public IntEvent toastyHPChange;

    public static Transform killPoint;
    public Transform killPointRef;

    private void Awake()
    {
        killPoint = killPointRef;
        instance = this;
    }


    public delegate void OnEatCompleteCallback();
    List<OnEatCompleteCallback> actorEatenCompleteCallback;
    public void CaughtByToasty(int fuelWorth,OnEatCompleteCallback callback)
    {
        actorEatenCompleteCallback.Add(callback);
        toastyEatFuelAmount += fuelWorth;
    }
    public void toastyEatFinish()
    {
        //called by anim at the end of toasty's eat animation
        foreach(OnEatCompleteCallback call in actorEatenCompleteCallback)
        {
            call(); //let the held things know they are dead now.
        }
        actorEatenCompleteCallback.Clear();
        fuel += toastyEatFuelAmount;
    }
    public static void BurnedPassiveByToasty(int fuelWorth)
    {
        //passive things healing toasty, like leaves.
        ToastyController.instance.fuel += fuelWorth;
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
