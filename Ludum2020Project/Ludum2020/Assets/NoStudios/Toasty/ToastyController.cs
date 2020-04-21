using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToastyController : MonoBehaviour
{
    public static ToastyController instance;
    [SerializeField]
    public UnityEngine.Events.UnityAction<bool> onToastyAliveChanged;

    public Animator anim;
    public bool consumeFuel = false;
    public int toastyEatFuelAmount=0;
    public int m_fuel=50;
    public int burnRate = 2;
    public int fuel
    {
        get { return m_fuel; }
        set { m_fuel = Mathf.Clamp(value,-1,100); anim.SetInteger("FuelLevel", Mathf.Clamp(value, -1, 100)); toastyHPChange.Invoke(Mathf.Clamp(value, -1, 100)); }
    }
    public IntEvent toastyHPChange;

    public static Transform killPoint;
    public Transform killPointRef;

    private void Awake()
    {
        killPoint = killPointRef;
        instance = this;
        eatRegion.enabled = false;
    }

    public static bool holdingThings = false;
    public delegate void OnEatCompleteCallback();
    List<OnEatCompleteCallback> actorEatenCompleteCallback = new List<OnEatCompleteCallback>();
    public void CaughtByToasty(int fuelWorth,OnEatCompleteCallback callback)
    {
        actorEatenCompleteCallback.Add(callback);
        toastyEatFuelAmount += fuelWorth;
        holdingThings = true;
    }
    public void toastyEatFinish()
    {
        holdingThings = false;
        //called by anim at the end of toasty's eat animation
        if (actorEatenCompleteCallback.Count > 0)
        {
            foreach (OnEatCompleteCallback call in actorEatenCompleteCallback)
            {
                call(); //let the held things know they are dead now.
            }
        }
        actorEatenCompleteCallback.Clear();
        fuel += toastyEatFuelAmount;
        canEat = true;
        anim.ResetTrigger("Eat");
    }
    public static void BurnedPassiveByToasty(int fuelWorth)
    {
        //passive things healing toasty, like leaves.
        ToastyController.instance.fuel += fuelWorth;
    }

    bool canEat = true;
    bool alive = true;
    bool inputDisabled = true;
    public void EnableInput() { inputDisabled = false; }
    public void DisableInput() { inputDisabled = true; }
    //make a custom getter setter here, and check other states, etc
    public void ToastyEatInput()
    {
        if(canEat && alive && !inputDisabled)
        {
            anim.SetTrigger("Eat");
            if (onToastyEat != null)
            {
                onToastyEat();
            }
            canEat = false;
        }

    }
    public Collider eatRegion;
    public void toastyColliderEnable()
    {
        eatRegion.enabled = true;
    }
    public void toastyColliderDisable()
    {
        eatRegion.enabled = false;
    }


    public UnityEngine.Events.UnityEvent onEatAnimStart;
    public UnityEngine.Events.UnityEvent onEatAnimDone;
    void ToastyEatStartAnim()
    {
        onEatAnimStart.Invoke();
    }
    void ToastyEatEndAnim()
    {
        onEatAnimDone.Invoke();
    }

    public UnityEngine.Events.UnityAction onToastyEat;

    float timeAccumulator=0f;
    bool HPEatPause = false;
    private void Update()
    {
        if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            ToastyEatInput();
        }


        if(consumeFuel && !holdingThings)
        {
            timeAccumulator += Time.deltaTime;
            if(timeAccumulator>1f)
            {
                if(fuel>70)
                {
                    fuel -= burnRate*4;
                }
                if(gameLoopManager.currentLevel >0)
                {
                    fuel -= burnRate+2*gameLoopManager.currentLevel;
                }
                else
                {
                    fuel -= burnRate;
                }
                timeAccumulator = 0f;
            }
        }

        if(fuel<=0 && alive)
        {
            KillToasty();
        }
    }

    public GameloopController gameLoopManager;
    public void FirstEatComplete()
    {
        gameLoopManager.AdvanceGameState();
    }

    public void StopFuelBurn()
    {
        consumeFuel = false;
    }
    public void StartFuelBurn()
    {
        consumeFuel = true;
    }
    public void KillToasty()
    {
        Debug.LogWarning("toasty killed");
        alive = false;
        ToastyLowHP.instance.ToastyDead();
        //start playing death anim, music, etc.
        onToastyAliveChanged(false);
    }

    public void ResetToasty(bool animReset = true)
    {
        fuel = 50;
        alive = true;
        consumeFuel = true;
        Debug.LogWarning("toasty reset");
        if (animReset)
        {
            anim.SetBool("Reset", true);
        }
        onToastyAliveChanged(true);
    }

}
