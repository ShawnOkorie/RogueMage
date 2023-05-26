using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellButton : MonoBehaviour
{
    public delegate void ShowBuffPanel(SpellButton sender, bool enter);
    public static event ShowBuffPanel OnPointerInteraction;
    
    private static Player player;
    private Image myImage;
    protected TextMeshProUGUI manaCostText;
    
    public Spell mySpell;
    [HideInInspector] public float spellMinDmg;
    [HideInInspector] public float spellMaxDmg;
    public int SpellHitCount => mySpell.hitCount;
    public Spell.Targeting SpellDamageTarget => mySpell.damageTargeting;
    public Spell.BuffType SpellBuffType => mySpell.buffType;
    public int BuffCount => mySpell.buffCount;
    public Spell.BuffTargeting spellBuffTargeting => mySpell.buffTargeting;
    public Vector3 ButtonPosition => transform.position;
    
    private void Awake()
    {
        if (player == null)
            player = FindObjectOfType<Player>();

        myImage = GetComponent<Image>();
        manaCostText = GetComponentInChildren<TextMeshProUGUI>();
    }

    protected virtual void Start()
    {
        myImage.sprite = mySpell.spellSprite;
        manaCostText.text = mySpell.manaCost.ToString();
    }

    public void TriggerSpell()
    {
        player.CheckCast(mySpell);
    }

    public bool HasDamageRange()
    {
        if (mySpell.mindamage != mySpell.maxdamage)
            return true;
        
        return false;
    }

    public bool IsTargetingEnemy()
    {
        if (mySpell.damageTargeting == Spell.Targeting.Self)
            return false;
        
        return true;
    }
    public void ShowSpellInfo()
    {
        if (HasDamageRange())
        {
            spellMinDmg = Mathf.Abs(mySpell.mindamage);
            spellMaxDmg = Mathf.Abs(mySpell.maxdamage);
        }
        else
            spellMaxDmg = Mathf.Abs(mySpell.maxdamage);
        
        OnPointerInteraction?.Invoke(this,true);
    }

    public void HideSpellInfo()
    {
        OnPointerInteraction?.Invoke(this,false);
    }
}
