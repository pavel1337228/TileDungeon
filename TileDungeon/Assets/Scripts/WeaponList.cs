using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New WeaponList", menuName = "Weapon List", order = 51)]
public class WeaponList : ScriptableObject
{
    public List<Weapon> weapons = new List<Weapon>();
}
