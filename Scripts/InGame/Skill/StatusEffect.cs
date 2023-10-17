using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum STATUS_EFFECT
{
    NONE = 0,           //0000 0000
    SLOW = 1 << 0,      //0000 0001
    POISON = 1 << 1,    //0000 0010
    STUN = 1 << 2,      //0000 0100
    RESTRAINT = 1 << 3, //0000 1000
    FIRE = 1 << 4,      //0001 0000
    ALL = int.MaxValue, //1111 1111
}

public class StatusEffect
{
    private STATUS_EFFECT statusEffect;

    public StatusEffect()
    {
        this.statusEffect = STATUS_EFFECT.NONE;
    }

    public StatusEffect(STATUS_EFFECT statusEffect)
    {
        this.statusEffect = statusEffect;
    }

    public void SetStatusEffect(STATUS_EFFECT statusEffect)
    {
        this.statusEffect = statusEffect;
    }

    public void AddStatusEffect(STATUS_EFFECT statusEffect)
    {
        this.statusEffect |= statusEffect;
    }

    public void RemoveStatusEffect(STATUS_EFFECT statusEffect)
    {
        this.statusEffect &= ~statusEffect;
    }

    public bool IsStatusEffect(STATUS_EFFECT statusEffect)
    {
        return (this.statusEffect & statusEffect) == 0 ? false : true;
    }
}
