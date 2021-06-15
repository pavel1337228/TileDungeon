using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public enum typeItem
    {
        coin,
        sword,
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
    private CanvasTHX parsertext;

    [Header("For Weapon")]
    public Weapon wp;
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

        if (type_item == typeItem.sword)
        {
            particleSystem.Stop();
        }

        if (type_item == typeItem.any)
        {
            particleSystem.Stop();
        }

        parsertext = GameObject.Find("Canvas").GetComponent<CanvasTHX>();
        try
        {
            parsertext.PickButton.GetComponent<Button>().onClick.AddListener(PickButton);
        }
        catch { print("No button?"); }
        PickInfo = parsertext.PickInfo;
        PickInfo_Name = parsertext.PickInfo_Name;
        PickInfo_Descr = parsertext.PickInfo_Descr;
        PickInfo_Damage = parsertext.PickInfo_Damage;

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

        if (distance > distanse_to_pick_item)
        {
            particleSystem.Pause();
            particleSystem.Clear();
            try
            {
                parsertext.PickButton.SetActive(false);
            }
            catch { }
            transform.eulerAngles = Vector3.up * Time.deltaTime;
            //transform.LookAt2D(new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, transform.position.y+1,Time.deltaTime), transform.position.z));
        }

        if (distance <= distanse_to_commit_item) {
            if (type_item == typeItem.coin)
            {
                Player.GetComponent<Player>().coin += count;
                Destroy(gameObject);
                Player.GetComponent<AudioSource>().clip = clip;
                Player.GetComponent<AudioSource>().Play();
            }

            if (type_item == typeItem.sword) {
                parsertext.PickButton.SetActive(true);
                //StartCoroutine(Info());
                Pick();
            }
        }
    }
    //public GameObject pickButton;
    public void PickButton() {
        allowPick = true;
    }

    public void Pick() {
        if (!allowPick) {
            return;
        }

        Player.GetComponent<Player>().PickUpWeapon(wp.weapon_id);
        StartCoroutine(Info());
        parsertext.PickButton.SetActive(false);
        allowPick = false;
        Destroy(gameObject);
    }

    private bool allowPick;
    private GameObject PickInfo;
    private TMP_Text PickInfo_Name;
    private TMP_Text PickInfo_Descr;
    private TMP_Text PickInfo_Damage;
    public IEnumerator Info()
    {
        Color rareColor = wp.rare_list.rares[wp.weapon_rare].rare_color;
        PickInfo_Name.color = rareColor;
        PickInfo_Name.text = wp.weapon_name;
        PickInfo_Descr.text = wp.weapon_discr;
        PickInfo_Damage.text = "Damage ("+wp.damage+")";
        parsertext.thx1();
        yield break;
    }

}
