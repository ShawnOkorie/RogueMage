using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Mathf = UnityEngine.Mathf;
using Object = System.Object;

public class Buff : Object
{
    public Spell spell;
    public Spell.BuffType buffType;
    public Spell.BuffRemoveTrigger buffRemoveTrigger;
    
    public Buff(Spell spell)
    {
        this.spell = spell;
        buffType = spell.buffType;
        buffRemoveTrigger = spell.buffRemoveTrigger;
    }
    public float HandleBuffDefender(float damage)
    {
        switch (buffType)
            {
                case Spell.BuffType.Melt:
                    damage *= 1.5f;
                    break;
               
                case Spell.BuffType.Shock:
                    damage += 1;
                    break;
               
                case Spell.BuffType.Harden:
                    damage -= 1;
                    break;
                
                case Spell.BuffType.Aegis:
                    if (damage <= 0)
                        break;
                    
                    if (damage > 0)
                        damage *= 0;
                    break;
               
                /*case Spell.BuffType.Flow:
                    target.negateNextBuff = true;
                    break;*/
            }
        

        return  Mathf.Clamp(damage, 0, damage);
    } 
    public float HandleBuffAttacker(float damage)
    {
        switch (buffType)
        {
            case Spell.BuffType.Root:
                damage *= 0.75f;
                break;
           
            case Spell.BuffType.Chill:
                damage -= 1;
                break;
        }
        return damage;
    }

    public int HandleBuffMana(int currentmana)
    {
        switch (buffType)
        {
            case Spell.BuffType.Void:
                currentmana = 0;
                break;
            
            case Spell.BuffType.Winded:
                if (currentmana > 0)
                    currentmana -= 1;
                break;
        }
        return currentmana;
    }
}
