using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum typeItem
    {
        coin,
        any
    };

    public float distance;
    private GameObject Player;
    public typeItem type_item;
    public int count;
    public AudioClip clip;
    public float pick_speed;
    public float distanse_to_commit_item;
    public float distanse_to_pick_item;
    private ParticleSystem particleSystem;
    private void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
        try
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }
        catch { print("Player is null"); }

        if (type_item == typeItem.coin) {
            particleSystem.Pause();
            distanse_to_pick_item *= 2;
            pick_speed *= 1.5f;
        }

        if (type_item == typeItem.any)
        {
            particleSystem.Stop();
        }
    }
    private void OnDrawGizmosSelected()
    {
        try
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawLine(transform.position, Player.transform.position);
        }
        catch { }

        try
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawWireSphere(transform.position, distanse_to_pick_item);
        }
        catch { }

        try
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, distanse_to_commit_item);
        }
        catch { }
    }
    void Update()
    {
        if (Player != null)
            distance = Vector3.Distance(transform.position, Player.transform.position);

        if (distance <= distanse_to_pick_item) {
            particleSystem.Play();
            transform.LookAt2D(Player.transform.position);
            transform.position = new Vector3(Mathf.Lerp(transform.position.x, Player.transform.position.x, pick_speed * Time.deltaTime), Mathf.Lerp(transform.position.y, Player.transform.position.y, pick_speed * Time.deltaTime));
        }

        if (distance <= distanse_to_commit_item) {
            if (type_item == typeItem.coin)
            {
                Player.GetComponent<Player>().coin += count;
                Destroy(gameObject);
                Player.GetComponent<AudioSource>().clip = clip;
                Player.GetComponent<AudioSource>().Play();
            }
        }
    }
}
