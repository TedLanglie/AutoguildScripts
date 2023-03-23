using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStats : MonoBehaviour
{
    [Header("Info")]
    public new string name;
    public string primaryClass;
    public string subClass;
    public string titleClass;
    public string race;
    public int level;
    [Header("Stats")]
    public float maxHealth;
    public float baseDamage;
    public float critChance;
    public float critDamage;
    public float dodgeChance;
    public float blockChance;
    public float parryChance;
    [Header("Traits")]
    public List<string> traits = new List<string>();
}
