
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;
using Unity.Mathematics;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    private readonly int targetFrameRate = 30;
    public enum CurrentTurn
    {
        playerTurn,
        enemyTurn
    }

    public delegate void ChangeTarget(CombatUnit target);
    public event ChangeTarget OnTargetChanged;
    
    private CurrentTurn _currentTurn;
    
    [Header("References")] 
    public Player player;
    private Enemy[] enemyCombatUnits;

    private CombatUnit currentTarget;
    private CombatUnit currentAttacker;
    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFrameRate;
    }
    private void Start()
    {
        GameObject[] enemyGameObjects = GameObject.FindGameObjectsWithTag("Enemy");
        enemyCombatUnits = new Enemy[enemyGameObjects.Length];
       
        for (int i = 0; i < enemyGameObjects.Length; i++)
        {
            enemyCombatUnits[i] = enemyGameObjects[i].GetComponent<Enemy>();
            enemyCombatUnits[i].ChooseNextSpell();
        }
        
        ChangeTurn(CurrentTurn.playerTurn);
        AutoTargetEnemy();
    }
    private void ChangeTurn(CurrentTurn newState)
    {
        _currentTurn = newState;

        switch (_currentTurn)
        {
            case CurrentTurn.playerTurn:
                print("player turn");
                
                currentAttacker = player;
               
                AutoTargetEnemy();
                SetButtonInteractability(true);
                
                player.OnPlayerTurnStart();
                
                for (int i = enemyCombatUnits.Length - 1; i >= 0; i--)
                {
                    enemyCombatUnits[i].RemoveBuffAtTurnEnd();
                    enemyCombatUnits[i].ChooseNextSpell();  
                }
                break;
            
            case CurrentTurn.enemyTurn:
                print("enemy turn");

                SetButtonInteractability(false);
                
                player.RemoveBuffAtTurnEnd();
                
                StartCoroutine(EnemyTurn());
                break;
        }
    }
    public CurrentTurn GetCurrentTurn()
    {
        return _currentTurn;
    }
    public void EndTurn()
    {
        if (_currentTurn == CurrentTurn.playerTurn)
            ChangeTurn(CurrentTurn.enemyTurn); 
    }

    public void SetTarget(CombatUnit target)
    {
        currentTarget = target;
        OnTargetChanged?.Invoke(currentTarget);
    }
    
    private void AutoTargetEnemy()
    {
        for (int i = 0; i < enemyCombatUnits.Length; i++)
        {
            if (enemyCombatUnits[i] != null)
            {
                currentTarget = enemyCombatUnits[i];
                OnTargetChanged?.Invoke(currentTarget);
                break;
            }
        }
    }
    public void CastSpell(Spell spell)
    {
        if (spell.damageTargeting == Spell.Targeting.Self) 
            SetTarget(currentAttacker);
        
        else if (currentTarget == player || currentTarget == null)
             AutoTargetEnemy();
        
        CastSpell(spell,currentTarget,currentAttacker);
    }

    private void CastSpell(Spell spell, CombatUnit target, CombatUnit caster)
    {
        if (target == null)
        {
            Debug.Log("error: no target");
            return;
        }
        StartCoroutine(SpellCast(spell,target,caster));
    }

    public IEnumerator SpellCast(Spell spell, CombatUnit target,CombatUnit caster)
    {
        for (int u = 0; u < spell.hitCount; u++)
        {
            if (spell.maxdamage == 0)
                continue;

            float damage = Random.Range(spell.mindamage,spell.maxdamage);
            damage = Mathf.Floor(damage);
            print(damage);
           
            for (int i = caster.currentBuffs.Count - 1; i >= 0; i--)
                damage = caster.currentBuffs[i].HandleBuffAttacker(damage);

            for (int i = caster.currentBuffs.Count - 1; i >= 0; i--)
            {
                if (caster.currentBuffs[i].buffRemoveTrigger == Spell.BuffRemoveTrigger.OnCast)
                {
                    caster.RemoveBuffByType(caster.currentBuffs[i].buffType, i);
                    break;  
                }
            }
            
            if (spell.damageTargeting == Spell.Targeting.All)
            {
                for (int i = 0; i < enemyCombatUnits.Length; i++)
                    enemyCombatUnits[i].TakeDamage(damage, target, caster);
            }
            else
                target.TakeDamage(damage, target, caster);
            
            yield return new WaitForSeconds(0.5f);
        }
        
        float rand = Random.Range(0,1);
        if (rand < spell.buffChance)
        {
            switch (spell.buffTargeting)
            {
                case Spell.BuffTargeting.Single:
                    target.ReceiveBuff(new Buff(spell),spell.buffCount);
                    break;
                
                case Spell.BuffTargeting.All:
                    for (int i = 0; i < enemyCombatUnits.Length; i++)
                        enemyCombatUnits[i].ReceiveBuff(new Buff(spell),spell.buffCount);
                    break;
                
                case Spell.BuffTargeting.Self:
                    caster.ReceiveBuff(new Buff(spell),spell.buffCount);
                    break;
            }
        }
       
        if (_currentTurn == CurrentTurn.playerTurn)
            SetButtonInteractability(true);
    }
    private IEnumerator EnemyTurn()
    {
        SetTarget(player);

        for (int i = 0; i < enemyCombatUnits.Length; i++)
        {
            if (enemyCombatUnits[i] != null)
            {
                yield return new WaitForSeconds(0.5f);
                
                currentAttacker = enemyCombatUnits[i];
                enemyCombatUnits[i].RemoveBuffAtTurnStart();
                enemyCombatUnits[i].EnemyAttack(currentTarget,currentAttacker);
            }
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(2);
        ChangeTurn(CurrentTurn.playerTurn);
    }

    public void SetButtonInteractability(bool interactable)
    {
        Button[] buttons = FindObjectsOfType<Button>();

        if (player.currentmana > player.manaDebtMax)
        {
            foreach (var button in buttons)
                button.interactable = interactable;
        }
    }
}
