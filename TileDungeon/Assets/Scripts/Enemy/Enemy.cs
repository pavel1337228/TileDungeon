using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy : MonoBehaviour
{
    public int health;
    public bool AI;

    public List<GameObject> Drop = new List<GameObject>();
    public List<int> Count = new List<int>();
    public List<float> Chance = new List<float>();

    private GameObject Player;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    public SoundList soundList;
    [Header("AI")]
    public int damage;
    public float speed;
    public float time_attack;
    private float distance;
    [Header("AI Settings")]
    public float distance_to_damage_player;
    public Vector3 transform_damage_zone;
    public float distance_to_spoted_player;
    private bool spoted;
    private Animator _anim;
    private void Update() {
        if (AI) {
            EnemyAI();
            if (timeBtwAttack > 0)
            {
                timeBtwAttack -= Time.deltaTime;
            }
        }
    }
    private void EnemyAI()
    {
        if (Player == null)
        {
            return;
        }

        distance = Vector3.Distance(transform.position, Player.transform.position);

        if (distance > distance_to_spoted_player)
        {
            spoted = false;
        }

        if (distance <= distance_to_spoted_player)
        {
            spoted = true;
        }

        if (spoted)
        {
            if (Player.transform.position.x < transform.position.x)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            if (Player.transform.position.x > transform.position.x)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }

        if (spoted)
        {
            if (distance_to_damage_player > distance)
            {
                HitPlayer();
                _anim.SetTrigger("StopRun");
            }
        }

        if (spoted) {
            if (distance_to_damage_player < distance)
            {
                _anim.SetTrigger("Run");
                transform.position = new Vector3(Mathf.Lerp(transform.position.x, Player.transform.position.x, speed * Time.deltaTime), Mathf.Lerp(transform.position.y, Player.transform.position.y, speed * Time.deltaTime));
            }
        }

        if (!spoted) {
            if (distance_to_spoted_player < distance)
            {
                _anim.SetTrigger("StopRun");
            }
        }

    }

    private Collider2D[] AllColliders;
    private Collider2D enemies;

    private float timeBtwAttack;
    public LayerMask player;
    public void HitPlayer()
    {
        if (timeBtwAttack <= 0)
        {
            //myanim.GetComponent<Animator>().SetTrigger("Hit");
            enemies = null;
            AllColliders = Physics2D.OverlapCircleAll(transform.position + transform_damage_zone, distance_to_damage_player, player);
            for (int i = 0; i < AllColliders.Length; i++)
            {
                if (AllColliders[i].isTrigger)
                {
                    enemies = AllColliders[i];
                }
            }
            enemies.GetComponent<Player>().TakeDamage(damage);
            timeBtwAttack = time_attack;
        }
    }
    private void OnDrawGizmosSelected() {
        try
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, Player.transform.position);
        }
        catch { }

        try
        {
            if (spoted) {
                Gizmos.color = Color.red;
            }
            if (!spoted)
            {
                Gizmos.color = Color.cyan;
            }
            Gizmos.DrawWireSphere(transform.position, distance_to_spoted_player);
        }
        catch { }

        try
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position + transform_damage_zone, distance_to_damage_player);
        }
        catch { }
    }
    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        if (AI) {
            _anim = GetComponent<Animator>();
            try
            {
                Player = GameObject.FindGameObjectWithTag("Player");
            }
            catch { print("Player is null"); }
        }

        try
        {
            for (int i = 0; i <= Drop.Count; i++) {
                new ItemLoot(Drop[i], Chance[i], Random.Range(0, Count[i]));
                print("Add " + Drop[i] + " " + Chance[i]);
            }
        }
        catch { }
    }
    public void TakeDamage(int damage) {
        health -= damage;
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
        Transform getT = transform;
        Destroy(gameObject);

        //List<GameObject> spawningTrue = new List<GameObject>();
        ItemLoot ItemLootSelected = ItemLoot.AllItems.ReturnRandom<ItemLoot>();
        for (int i = 0; i <= ItemLootSelected.itemDropCount; i++) {
            GameObject objects = Instantiate(ItemLootSelected.nameID, getT.transform.position + new Vector3(Random.Range(0,0.2f), Random.Range(0, 0.2f)), Quaternion.Euler(0f,0f,0f), null);
        }
    }
}

public interface IRandom
{
    float itemDropChance
    {
        get;
        set;
    }
}
public class ItemLoot : IRandom
{

    public static List<ItemLoot> AllItems = new List<ItemLoot>();

    GameObject _nameID;
    float _itemDropChance;
    int _itemDropCount;

    public GameObject nameID
    {
        get { return _nameID; }
        set { _nameID = value; }
    }

    public float itemDropChance
    {
        get { return _itemDropChance; }
        set { _itemDropChance = value; }
    }
    public int itemDropCount
    {
        get { return _itemDropCount; }
        set { itemDropCount = value; }
    }

    public ItemLoot(GameObject nameID, float itemDropChance, int itemDropCount)
    {
        this.nameID = nameID;
        this.itemDropChance = itemDropChance;
        this._itemDropCount = itemDropCount;
        AllItems.Add(this);
    }

}