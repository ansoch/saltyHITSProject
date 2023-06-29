using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerInfo
{
    //public delegate void PerformAnimAction(string conditionName);

    public static float FacingRight { get; private set; } = -1f;

    static public bool IsNotBusy { get; private set; }
    static public int Health { get; private set; } = 10;
    static public int MaxHealth { get; private set; } = 10;

    static public float Stamina { get; private set; } = 100f;
    static public float MaxStamina { get; private set; } = 100f;
    static public float StaminaRestoration { get; private set; } = 0.3f;
    static public float StaminaRestorationWaitUp { get; private set; } = 600;
    static private float staminaRestorationWaitUpMax = 600;
    static private float staminaRestorationWaitUpTick = 10;
    static private bool waitUp = false;
    static public void OnDamage(int damage) => Health -= damage;
    static public void OnHealing(int healing) => Health += healing + Health > MaxHealth ? MaxHealth - Health : healing;

    static public void OnAction(int actionCost)
    {
        Stamina = Stamina - actionCost < 0 ? 0 : Stamina - actionCost;
        waitUp = true;
        StaminaRestorationWaitUp = Stamina == 0f ? staminaRestorationWaitUpMax * 1.3f : staminaRestorationWaitUpMax;
    }
    static public void RestoreStaminaOnTick()
    {
        if (!waitUp)
        {
            Stamina = Stamina + StaminaRestoration > MaxStamina ? MaxStamina : Stamina + StaminaRestoration;
            StaminaRestorationWaitUp = staminaRestorationWaitUpMax;
        }
        else
        {
            Debug.Log("waitup");
            StaminaRestorationWaitUp -= staminaRestorationWaitUpTick;
            waitUp = StaminaRestorationWaitUp > 0;
        }
    }

    static public void SetFacingRight(this Player player, float value) => FacingRight = value;
    static public void SetState(this Player player, bool value) => IsNotBusy = value;
}
