using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Toggle = UnityEngine.UI.Toggle;
using TMPro;
public class SpellButtonManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI selectionText;
    
    private GameObject[] spellButtons;
    private GameObject[] spellToggleArray;
    private SpellToggle spellToggle;
    public Spell[] chosenSpells;
    private Spell nextSpell;
    private int chosenSpellsIndex;
    private int currentElementsCounter;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        chosenSpells = new Spell[4];
        spellButtons = new GameObject[4];
        spellToggleArray = new GameObject[9];


        spellToggleArray = GameObject.FindGameObjectsWithTag("Toggle");
    }

    private void Start()
    {
        foreach (GameObject t in spellToggleArray)
        {
            spellToggle = t.GetComponent<SpellToggle>();
            spellToggle.OnSpellChosen += ChangeChosenSpells;   
        }

        SceneManager.sceneLoaded += SetUpMainScene;

        currentElementsCounter = 0;

        selectionText.text = $"{currentElementsCounter}/{chosenSpells.Length}";
    }
    private void ChangeChosenSpells(Spell spell, bool isOn)
    {
        if (isOn)
        {
            currentElementsCounter++;
            selectionText.text = $"{currentElementsCounter}/{chosenSpells.Length}";
            AddSpell(spell);
        }
        else
        {
            currentElementsCounter--;
            selectionText.text = $"{currentElementsCounter}/{chosenSpells.Length}";
            RemoveSpell(spell);
        }
        if (currentElementsCounter == chosenSpells.Length)
        {
            foreach (GameObject t in spellToggleArray)
            {
                spellToggle = t.GetComponent<SpellToggle>();
                
                if (ContainsSpell(spellToggle.mySpell))
                {
                    Toggle toggle = spellToggle.GetComponent<Toggle>();
                    toggle.interactable = true;
                }
                else if (ContainsSpell(spellToggle.mySpell) == false)
                {
                    Toggle toggle = spellToggle.GetComponent<Toggle>();
                    toggle.interactable = false;
                }
            }
        }
        else
        {
            foreach (GameObject t in spellToggleArray)
            {
                spellToggle = t.GetComponent<SpellToggle>();
                
                Toggle toggle = spellToggle.GetComponent<Toggle>();
                toggle.interactable = true;
            }
        }
    }
    private void AddSpell(Spell spell)
    {
        for (int i = 0; i < chosenSpells.Length; i++)
        {
            if (chosenSpells[i] == null)
            {
                chosenSpells[i] = spell;
                return;   
            }
        }
    }
    private void RemoveSpell(Spell spell)
    {
        for (int i = 0; i < chosenSpells.Length; i++)
        {
            if (spell == chosenSpells[i])
                chosenSpellsIndex = i;
        }
        if (spell == chosenSpells[chosenSpellsIndex])
            chosenSpells[chosenSpellsIndex] = null;
    }
    public bool IsFull()
    {
        if (currentElementsCounter == chosenSpells.Length)
            return true;
        
        return false;
    }

    private bool ContainsSpell(Spell spell)
    {
        foreach (Spell t in chosenSpells)
        {
            if (spell == t)
                return true;
        }
        return false;
    }

    private void SetUpMainScene(Scene scene, LoadSceneMode mode)
    {
        spellButtons = GameObject.FindGameObjectsWithTag("SpellButton");

        for (int i = 0; i < chosenSpells.Length; i++)
        {
            SpellButton spellButton = spellButtons[i].GetComponent<SpellButton>();
            
            if (spellButton.mySpell == null)
            {
                spellButton.mySpell = chosenSpells[i];
            }
        }
    }
}
