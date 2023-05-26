using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SpellInfoPanel : MonoBehaviour
{
    private Camera mainCamera;
    
    private float cameraHeight;
    private float cameraWidth;
    
    [SerializeField] private float panelOffsetX;
    [SerializeField] private TextMeshProUGUI damageInfo;
    [SerializeField] private TextMeshProUGUI buffInfo;

    private Color textColor;
    void Awake()
    {
        mainCamera = FindObjectOfType<Camera>();

        Scene scene = SceneManager.GetActiveScene();

        if (scene.name == "Spell Select")
            SpellToggle.OnPointerInteraction += OnPointerInteraction;

        if (scene.name == "Main")
            SpellButton.OnPointerInteraction += OnPointerInteraction;
        
        cameraHeight = mainCamera.orthographicSize * 2;
        cameraWidth = mainCamera.aspect * cameraHeight; 
    }

    private void OnPointerInteraction(SpellButton spellButton, bool activate)
    {
        OnPointerInteraction(spellButton,null, activate);
    }
    
    private void OnPointerInteraction(SpellToggle spellToggle, bool activate)
    {
        OnPointerInteraction(null,spellToggle, activate);
    }

    private void OnPointerInteraction(SpellButton spellButton, SpellToggle spellToggle, bool activate)
    {
        if (activate == false)
            gameObject.SetActive(false);

        else
            gameObject.SetActive(true);

        if (spellButton != null && spellToggle == null)
        {
           SpellButton sender = spellButton;
           
           //set Damage Color
           textColor = sender.IsTargetingEnemy() ? Color.red : Color.green;

           //set Damage Text
           if (sender.HasDamageRange())
               damageInfo.text = $"{sender.SpellHitCount}x<color=#{ColorUtility.ToHtmlStringRGB(textColor)}>{sender.spellMinDmg}-{sender.spellMaxDmg}</color> DMG\n{sender.SpellDamageTarget}";
       
           else
               damageInfo.text = $"{sender.SpellHitCount}x<color=#{ColorUtility.ToHtmlStringRGB(textColor)}>{sender.spellMaxDmg}</color> DMG\n \n{sender.SpellDamageTarget}";
       
           //set Buff Text
           buffInfo.text = $"{sender.BuffCount}x {sender.SpellBuffType}\n \n {sender.spellBuffTargeting}";
        
        
           Vector3 panelPosition = new Vector3(
               Mathf.Clamp( sender.ButtonPosition.x,panelOffsetX - cameraWidth, cameraWidth - panelOffsetX),
               sender.ButtonPosition.y + cameraHeight * 0.2f);

           transform.position = panelPosition;
        }

        if (spellButton == null && spellToggle != null)
        {
            SpellToggle sender = spellToggle;
           
            //set Damage Color
            textColor = sender.IsTargetingEnemy() ? Color.red : Color.green;

            //set Damage Text
            if (sender.HasDamageRange())
                damageInfo.text = $"{sender.SpellHitCount}x<color=#{ColorUtility.ToHtmlStringRGB(textColor)}>{sender.spellMinDmg}-{sender.spellMaxDmg}</color> DMG\n{sender.SpellDamageTarget}";
       
            else
                damageInfo.text = $"{sender.SpellHitCount}x<color=#{ColorUtility.ToHtmlStringRGB(textColor)}>{sender.spellMaxDmg}</color> DMG\n \n{sender.SpellDamageTarget}";
       
            //set Buff Text
            buffInfo.text = $"{sender.BuffCount}x {sender.SpellBuffType}\n \n {sender.spellBuffTargeting}";
        
        
            Vector3 panelPosition = new Vector3(
                Mathf.Clamp( sender.TogglePosition.x,panelOffsetX - cameraWidth, cameraWidth - panelOffsetX),
                sender.TogglePosition.y - 2.5f);

            transform.position = panelPosition;
        }

       
    }
}
