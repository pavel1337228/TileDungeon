using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New RareList", menuName = "Rare List", order = 51)]
public class RareList : ScriptableObject
{
    public List<Rare> rares = new List<Rare>();
}
