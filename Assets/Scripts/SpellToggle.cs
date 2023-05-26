using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellToggle : SpellButton
{
    public delegate void SelectSpell(Spell spell, bool isOn);
    public event SelectSpell OnSpellChosen;
    public delegate void ShowBuffPanel(SpellToggle sender, bool enter);
    public static event ShowBuffPanel OnPointerInteraction;

    private Toggle toggle;
    private Image backgroundImage;

    public Vector3 TogglePosition => transform.position;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        backgroundImage = GetComponentInChildren<Image>();
        manaCostText = GetComponentInChildren<TextMeshProUGUI>();
    }

    protected override void Start()
    {
        backgroundImage.sprite = mySpell.spellSprite;
        manaCostText.text = mySpell.manaCost.ToString();
    }

    public void ChooseSpell()
    {
        OnSpellChosen?.Invoke(mySpell,toggle.isOn);
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
}
