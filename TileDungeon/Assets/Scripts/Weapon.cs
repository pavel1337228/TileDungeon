using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon", order = 51)]
public class Weapon : ScriptableObject
{
    [Header("Weapon Info")]
    public int weapon_id;
    public string weapon_name;
    public string weapon_discr;
    public int cost;
    public int weapon_rare;
    public RareList rare_list;
    [Header("Weapon Settings")]
    public int damage;
    public float attack_range;
    public float start_time_attack;
    [Header("Animation")]
    public GameObject animator;
    public bool FlipX;
    public bool FlipY;
    public Vector3 Transform;
    public Vector3 Scale;
}
