using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class BuffInfoPanel : MonoBehaviour
{
    [SerializeField] private float panelOffsetX;
    
    private TextMeshProUGUI buffInfoText;
    private Camera mainCamera;
    private string buffInfo;

    private float cameraHeight;
    private float cameraWidth;
    
    private void Awake()
    {
        buffInfoText = GetComponentInChildren<TextMeshProUGUI>();
        mainCamera = FindObjectOfType<Camera>();
        
        BuffUI.OnPointerInteraction += OnPointerInteraction;
        
        cameraHeight = mainCamera.orthographicSize * 2;
        cameraWidth = mainCamera.aspect * cameraHeight;
    }
    
    private void OnPointerInteraction(BuffUI sender, bool activate)
    {
        if (activate == false)
            gameObject.SetActive(false);

        else
            gameObject.SetActive(true);
        
        buffInfoText.text = sender.buffType + ": " + sender.buffInfo;
        
        Vector3 panelPosition = new Vector3(
            Mathf.Clamp(sender.transform.position.x,panelOffsetX - cameraWidth, cameraWidth - panelOffsetX),
            sender.transform.position.y - cameraHeight * 0.2f);

        transform.position = panelPosition;
    }
    

    /*switch (buffType)
    {
        case Spells.BuffType.Melt:
            BuffInfoText.text = "Next Hit against this Unit Deals <color=green>50%</color> increased Damage";
            break;
        
        case Spells.BuffType.Shock:
            BuffInfoText.text = "Hits against this Unit Deal <color=green>+1</color> increased Damage per Stack";
            break;
        
        case Spells.BuffType.Winded:
            BuffInfoText.text = "At the Start of next Turn, regenerate 1 <color=C37A13>Mana</color> less per Stack";
            break;
        
        case Spells.BuffType.Chill:
            BuffInfoText.text = "Hits from this Unit Deal <color=red>-1</color> Damage per Stack";
            break;
        
        case Spells.BuffType.Entangle:
            BuffInfoText.text = "Next Hit From this Unit Deals <color=red>50%</color> reduced Damage";
            break;
        
        
        case Spells.BuffType.Metallicize:
            BuffInfoText.text = "Hits against this Unit Deal <color=red>-1</color> Damage per Stack";
            break;
       
        case Spells.BuffType.Aegis:
            BuffInfoText.text = "Next Hit against this Unit has its Damage reduced to <color=green>0</color>";
            break;
        
        case Spells.BuffType.Void:
            BuffInfoText.text = "At the Start of next Turn, do <color=red>not</color> regenerate <color=C37A13>Mana</color>";
            break;
        
        case Spells.BuffType.Stun:
            
            break;
        
        case Spells.BuffType.Flow:
           
            break;
        
        case Spells.BuffType.None:
            BuffInfoText.text = "How did we get Here";
            break;*/
    
}
