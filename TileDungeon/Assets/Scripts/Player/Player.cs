using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Player")]
    public int health;
    public int coin;
    public int id_room;

    [Header("Invetory")]
    public List<GameObject> MyItems = new List<GameObject>();
    public GameObject WeaponSelected;
    public GameObject ArmourSelected;

    [Header("Setting")]
    private UnityEngine.SceneManagement.Scene room;

    public Slider slider_hp;
    public float min_fake = 0.125f;
    public float max_fake = 0.875f;

    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    public SoundList soundList;
    private void Awake()
    {
        room = SceneManager.GetActiveScene();
        id_room = room.buildIndex;
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        float h = health;
        slider_hp.value = h.Remap(0f, 100f, min_fake, max_fake);
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        float h = health;
        slider_hp.value = h.Remap(0f, 100f, min_fake, max_fake);
        StartCoroutine(Hit());
        if (health <= 0)
        {
            StartCoroutine(Die());
        }
    }
    public IEnumerator Hit()
    {
        int rnd = Random.Range(0, soundList.clips.Count);
        audioSource.clip = soundList.clips[rnd];
        audioSource.Play();

        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }
    public IEnumerator Die()
    {
        int rnd = Random.Range(0, soundList.clips.Count);
        audioSource.clip = soundList.clips[rnd];
        audioSource.Play();
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}

