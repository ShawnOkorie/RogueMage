using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class BuffUIManager : MonoBehaviour
{
    private List<GameObject> currentBuffUIList = new List<GameObject>();
    private int currentBuffUIIndex;
    
    [Header("References")] 
    public Player player;
    private Enemy[] enemyCombatUnits;
    public GameObject BuffUIPrefab;
    private BuffUI buffUI;
    
    void Start()
    {
        GameObject[] enemyGameObjects = GameObject.FindGameObjectsWithTag("Enemy");
        enemyCombatUnits = new Enemy[enemyGameObjects.Length];

        for (int i = 0; i < enemyGameObjects.Length; i++)
            enemyCombatUnits[i] = enemyGameObjects[i].GetComponent<Enemy>();
        
        player.OnBuffStackChanged += ChangeBuffUI;
        foreach (Enemy t in enemyCombatUnits)
            t.OnBuffStackChanged += ChangeBuffUI;
        
        currentBuffUIIndex = -1;
    }
    
    private void OnDisable()
    {
        player.OnBuffStackChanged -= ChangeBuffUI;
        foreach (var t in enemyCombatUnits)
            t.OnBuffStackChanged -= ChangeBuffUI;
    }

    private void ChangeBuffUI(CombatUnit.buffStackChange buffStackChange, Transform parent, Buff buff)
    {
        switch (buffStackChange)
        {
            case CombatUnit.buffStackChange.add:

                currentBuffUIList.Add(Instantiate(BuffUIPrefab, parent));

                buffUI = currentBuffUIList[currentBuffUIList.Count - 1].GetComponent<BuffUI>();
                buffUI.Initialize(buff.spell);
                break;
            
            case CombatUnit.buffStackChange.delete:
                
                for (int i = currentBuffUIList.Count - 1; i >= 0; i--)
                {
                    if (currentBuffUIList[i] == null)
                        continue; 
                    
                    buffUI = currentBuffUIList[i].GetComponent<BuffUI>();

                    if (buffUI.GetBuffUIBuff() == buff.buffType) ;
                        buffUI.Delete();
                }
                break;
            
            case CombatUnit.buffStackChange.increase: 
                
                for (int i = currentBuffUIList.Count - 1; i >= 0; i--)
                {
                    if (currentBuffUIList[i] == null)
                        continue;
                    buffUI = currentBuffUIList[i].GetComponent<BuffUI>();
                    
                    if (buffUI.GetBuffUIBuff() == buff.buffType)
                        buffUI.IncreaseBuffUICounter();
                }
                break;
            
            case CombatUnit.buffStackChange.decrease:
               
                for (int i = currentBuffUIList.Count - 1; i >= 0; i--)
                {
                    if (currentBuffUIList[i] == null)
                        continue;

                    buffUI = currentBuffUIList[i].GetComponent<BuffUI>();

                    if (buffUI.GetBuffUIBuff() == buffUI.buffType)
                    {
                        buffUI.DecreaseBuffUICounter();
                        
                        if (buffUI.buffStackCount == 0)
                            buffUI.Delete();
                    }
                }
                break;
        }
    }
    
}
