using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[CreateAssetMenu]
public class Spell : ScriptableObject
{
    [Header("Spell")]
    
    public float mindamage;
    public float maxdamage;
    public int hitCount;
    public int manaCost;
    public Targeting damageTargeting;
    public Sprite spellSprite;
    public enum Targeting
    {
        Single,
        All,
        Self,
        None
    }
    public AudioClip spellSFX;
   
    [Header("Buff")]
    
    public BuffType buffType;
    public BuffTargeting buffTargeting;
    public BuffRemoveTrigger buffRemoveTrigger;
    public float buffChance; //true = Buffs that are triggered and dont expire at turn end
    public int buffCount;
    public Sprite buffSprite;
    public string buffInfo;
    public enum BuffType
    {
        Melt,
        Shock,
        Stun,
        Winded,
        Chill,
        Root,
        Flow,
        Harden,
        Aegis,
        Void,
        None
    }
    public enum BuffTargeting
    {
        Single,
        All,
        Self,
        None
    }
    public enum BuffRemoveTrigger
    {
        OnHit,
        OnCast,
        TurnEnd,
        TurnStart,
        None
    }
}
