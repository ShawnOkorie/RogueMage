using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;
using UnityEngine.Serialization;
public class Player : CombatUnit
{
    [Header("References")]
    public CombatManager combatManager;
    [SerializeField] private TextMeshProUGUI manatxt;
    [SerializeField] private TextMeshProUGUI manadebttxt;
    
    [Header("Mana")] 
    private int maxmana = 3;
    [HideInInspector] public int currentmana;
    [HideInInspector] public int manaDebtMax = -3;
    private bool inManaDebt;
    private bool playerCanCast;
    

    protected override void Start()
    {
        maxhealth = 100;
        
        base.Start();
        
        inManaDebt = false;
        currentmana = maxmana;
        manatxt.text = currentmana.ToString();
    }
    private void Update()
    {
        if (combatManager.GetCurrentTurn() == CombatManager.CurrentTurn.playerTurn && currentmana > manaDebtMax)
            playerCanCast = true;
        
        else
            playerCanCast = false;
        
        if (currentmana < 0)
            inManaDebt = true;
        
        if (currentmana == manaDebtMax)
            playerCanCast = false;
    }

    public void CheckCast(Spell spell)
    {
        if (playerCanCast)
        {
            ReduceMana(spell.manaCost);
            combatManager.CastSpell(spell);   
        }
    }

    private void ReduceMana(int cost)
    {
        currentmana -= cost;
        manatxt.text = currentmana.ToString();
    }
    public void OnPlayerTurnStart()
    {
        currentmana = maxmana;

        for (int i = currentBuffs.Count - 1; i >= 0; i--)
        {
           currentmana = currentBuffs[i].HandleBuffMana(currentmana);
           manatxt.text = currentmana.ToString();
        }
        
        if (inManaDebt && manaDebtMax <= 0)
        {
            manaDebtMax++;
            inManaDebt = false;
        }
        
        manatxt.text = currentmana.ToString();
        manadebttxt.text = $"Debt Limit: {manaDebtMax}";
        
        RemoveBuffAtTurnStart();
    }
    
}