using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class CombatUnit : MonoBehaviour
{
    [Header("References")]
    public Transform buffUIGroup;
    public List<Buff> currentBuffs = new List<Buff>();
    private bool containsBuff;
    
    public delegate void BuffChange(buffStackChange buffStackChange, Transform parent, Buff buff);
    public event BuffChange OnBuffStackChanged; //Event for Buff UI
    public enum buffStackChange
    {
        add,
        delete,
        increase,
        decrease
    }
    
    [Header("Health")]
    protected float maxhealth;

    [HideInInspector]public float currenthealth;
    private TextMeshProUGUI healthtxt;
    private UnityEngine.UI.Slider healthbar;

    //public bool negateNextBuff;
    protected virtual void Awake()
    {
        healthtxt = GetComponentInChildren<TextMeshProUGUI>();
        healthbar = GetComponentInChildren<UnityEngine.UI.Slider>();
    }
    protected virtual void Start()
    {
        currenthealth = maxhealth;
        healthtxt.text = currenthealth + "/" + maxhealth;
        healthbar.value = currenthealth / maxhealth;
       
        //negateNextBuff = false;
    }
    public void TakeDamage(float damage,CombatUnit target,CombatUnit caster)
    {

        for (int i = currentBuffs.Count - 1; i >= 0; i--)
        {
            if (target == caster)
                continue;

            damage = currentBuffs[i].HandleBuffDefender(damage);
        }

        damage = Mathf.Floor(damage);
        
        if (damage != 0)
        {
            currenthealth -= damage;
            
            if (currenthealth > maxhealth)
            {
                float i = currenthealth - maxhealth;
                currenthealth -= i;
            }
            healthtxt.text = currenthealth + "/" + maxhealth;
            healthbar.value = currenthealth / maxhealth;
        }

        for (int i = currentBuffs.Count - 1; i >= 0; i--)
        {
            if (target == caster)
                continue;

            if (currentBuffs[i].buffRemoveTrigger == Spell.BuffRemoveTrigger.OnHit)
            {
                RemoveBuffByType(currentBuffs[i].buffType, i);
                break;  
            }
        }
    }
   
    public void ReceiveBuff(Buff buff, int count)
    {
        for (int i = 0; i <= count - 1; ++i)
        {
            /*if (negateNextBuff)
            {
                negateNextBuff = false;
                RemoveFlow();
                continue;
            }*/

            if (ContainsBuff(buff.buffType))
                OnBuffStackChanged?.Invoke(buffStackChange.increase, buffUIGroup, buff);
            
            else
                OnBuffStackChanged?.Invoke(buffStackChange.add, buffUIGroup, buff);
            
            currentBuffs.Add(buff);
        }
    }

    public void RemoveBuffByType(Spell.BuffType buffType, int i)
    {
        if (currentBuffs.Count == 0)
            return;

        if (currentBuffs[i].buffType == buffType)
        {
            OnBuffStackChanged?.Invoke(buffStackChange.decrease, buffUIGroup,currentBuffs[i]);
            currentBuffs.RemoveAt(i);
        }
    }
    public void RemoveBuffAtTurnEnd()
    {
        if (currentBuffs.Count == 0)
            return;

        for (int i = currentBuffs.Count - 1; i >= 0; i--)
        {
            if (currentBuffs[i].buffRemoveTrigger == Spell.BuffRemoveTrigger.TurnEnd)
            {
                OnBuffStackChanged?.Invoke(buffStackChange.delete, buffUIGroup,currentBuffs[i]);
                currentBuffs.RemoveAt(i);
            }
        }
    }
    public void RemoveBuffAtTurnStart()
    {
        if (currentBuffs.Count == 0)
            return;

        for (int i = currentBuffs.Count - 1; i >= 0; i--)
        {
            if (currentBuffs[i].buffRemoveTrigger == Spell.BuffRemoveTrigger.TurnStart)
            {
                OnBuffStackChanged?.Invoke(buffStackChange.delete, buffUIGroup,currentBuffs[i]);
                currentBuffs.RemoveAt(i);
            }
        }  
    }
    /*public void RemoveFlow()
    {
        if (currentBuffs.Count == 0)
        {
            return;
        } 
        
        for (int i = currentBuffs.Count - 1; i >= 0; i--)
        {
            if (currentBuffs[i].name == "Flow")
            {
                currentBuffs.RemoveAt(i);
            } 
        }
    }*/
    public void PrintCurrentBuffs()
    {
        for (int i = currentBuffs.Count - 1; i >= 0; i--)
            print(currentBuffs[i].buffType);
    }

    private bool ContainsBuff(Spell.BuffType buffType)
    {
        foreach (Buff buff in currentBuffs)
        {
            if (buff.buffType == buffType)
                return true;
        }
        return false;
    }
}
