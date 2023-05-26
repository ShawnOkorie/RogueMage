using System;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Enemy : CombatUnit
{
    [Header("References")]
    public EnemyType enemyType;
    private CombatManager combatManager;
    public TextMeshProUGUI previewText;
    public Image previewIcon;
    [SerializeField] private Image targetImage;

    private Spell _nextSpell;
    private int counter;
    protected override void Awake()
    {
        combatManager = GameObject.FindGameObjectWithTag("CombatManager").GetComponent<CombatManager>();
        combatManager.OnTargetChanged += ChangeTargetImage;
        base.Awake();
    }

    protected override void Start()
    {
        maxhealth = enemyType.maxhealth;
        counter = 1;
        targetImage.gameObject.SetActive(false);
        base.Start();
    }

    public void EnemyAttack(CombatUnit target,CombatUnit caster)
    {
        StartCoroutine(combatManager.SpellCast(_nextSpell, target, caster));

        previewIcon.color = Color.clear;
        previewText.gameObject.SetActive(false);
    }

    public void ChooseNextSpell()
    {
        switch (enemyType.randomAttackSelect)
        {
            case true:
                int rand = Random.Range(0, enemyType.attacks.Length);
                _nextSpell = enemyType.attacks[rand];
                break;
            case false:

                if (counter < enemyType.attacks.Length)
                {
                    _nextSpell = enemyType.attacks[counter];
                        counter++;

                    if (counter >= enemyType.attacks.Length)
                        counter = 0;
                }
                break;
        }
        SetActionPreview();
    }

    private void SetActionPreview()
    {
        //Text
        if (_nextSpell.maxdamage <= 0)
        {
            previewText.gameObject.SetActive(false);
        }
        else if (_nextSpell.hitCount == 1)
        {
            previewText.gameObject.SetActive(true);
            previewText.text = _nextSpell.maxdamage.ToString();
        }
        else 
        {
            previewText.gameObject.SetActive(true);
            previewText.text = _nextSpell.maxdamage + "x" + _nextSpell.hitCount + " (" +  _nextSpell.maxdamage * _nextSpell.hitCount + ")";
        }

        //Image
        if (_nextSpell.damageTargeting == Spell.Targeting.Self)
        {
            previewIcon.color = Color.cyan;
        }
        else if (_nextSpell.buffType == Spell.BuffType.None)
        {
            previewIcon.color = Color.red;
        }
        else if (_nextSpell.maxdamage == 0)
        {
            previewIcon.color = Color.green;
        }
        else
        {
           previewIcon.color = Color.magenta; 
        }
    }

    private void ChangeTargetImage(CombatUnit target)
    {
        if (target == this)
            targetImage.gameObject.SetActive(true);
        else
            targetImage.gameObject.SetActive(false);
    }
    
}