using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyType : ScriptableObject
{
   public float maxhealth;
   public Spell[] attacks;
   public bool randomAttackSelect;
}
