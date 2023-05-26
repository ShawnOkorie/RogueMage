using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueButton : MonoBehaviour
{
   [SerializeField] private SpellButtonManager spellButtonManager;
   
   public void LoadMainScene()
   {
      if (spellButtonManager.IsFull())
      {
         SceneManager.LoadScene("Main");
      }
   }
}
