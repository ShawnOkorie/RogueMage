using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;
using Object = System.Object;

public class BuffUI : MonoBehaviour
{
    public delegate void ShowBuffPanel(BuffUI sender, bool enter);
    public static event ShowBuffPanel OnPointerInteraction;

    public Spell.BuffType buffType;
    private Image buffUIImage;
    [HideInInspector] public string buffInfo;

    private TextMeshProUGUI buffUICounter;
    [HideInInspector] public int buffStackCount;
    
    private static GameObject buffInfoPanel;
    private static TextMeshProUGUI buffInfoText;
    private static Camera mainCamera;
    
    private void OnEnable()
    {
        buffUICounter = GetComponentInChildren<TextMeshProUGUI>();
        
        buffStackCount = 1;
        buffUICounter.text = buffStackCount.ToString();
    }
    public void Initialize(Spell spell)
    {
        buffType = spell.buffType;
        
        buffUIImage = GetComponentInChildren<Image>();
        buffUIImage.sprite = spell.buffSprite;

        buffInfo = spell.buffInfo;
    }
    public Spell.BuffType GetBuffUIBuff()
    {
        return buffType;
    }
    public void IncreaseBuffUICounter()
    {
        ++buffStackCount;
        buffUICounter.text = buffStackCount.ToString();
    }
    public void DecreaseBuffUICounter()
    {
        --buffStackCount;
        buffUICounter.text = buffStackCount.ToString();
    }

    public void Delete()
    {
        Destroy(gameObject);
    }

    public void ShowBuffInfo()
    {
        OnPointerInteraction?.Invoke(this,true);
    }

    public void HideBuffInfo()
    {
        OnPointerInteraction?.Invoke(this,false);
    }
}
