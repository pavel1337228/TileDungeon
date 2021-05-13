using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SoundList", menuName = "Sound List", order = 51)]
public class SoundList : ScriptableObject
{
    public List<AudioClip> clips = new List<AudioClip>();
}
