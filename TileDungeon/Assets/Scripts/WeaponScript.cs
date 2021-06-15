using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public GameObject WeaponSelection;
    public GameObject Animator;

    public GameObject WeaponSelectionTemplate;
    public GameObject AnimatorTemplate;
    private GameObject WeaponParent;
    private PlayerController pc;
    private int ID;
    private Animator _anim;

   
    public void Awake() {
        pc = GetComponent<PlayerController>();
        WeaponParent = pc.Weapon;
    }
    public IEnumerator ChangeWeapon()
    {
        yield return new WaitForSeconds(0.2f);
        if (pc == null)
        {
            print("Null");
            yield break;
        }

        ID = pc.id_weapon;

        try
        {
            if (WeaponSelection.name != "MyWeapon")
            {
                Destroy(WeaponSelection);
                WeaponSelection = WeaponSelectionTemplate;
            }
        }
        catch { print("Object = null"); }

        try
        {
            if (Animator.name != "Animator")
            {
                Destroy(Animator);
                Animator = AnimatorTemplate;
            }
        }
        catch { print("Object = null"); }


        myweap = Instantiate(WeaponSelection, WeaponParent.transform.position, Quaternion.EulerAngles(0, 0, 0), WeaponParent.transform);
        myanim = Instantiate(Animator, WeaponParent.transform.position, Quaternion.EulerAngles(0,0,0), myweap.transform);

        WeaponSelection = myweap;
        Animator = myanim;

        GameObject animator = pc.wplist.weapons[ID].animator;

        myweap.name = "" + pc.wplist.weapons[ID].weapon_name;
        myanim.name = "" + pc.wplist.weapons[ID].weapon_name + ".Animator";

        print(animator.name);
        print(myanim.name);


        myweap.transform.localRotation = Quaternion.Euler(0f,0f,0f);

        myanim.transform.localPosition = pc.wplist.weapons[ID].Transform;
        myanim.transform.localScale = pc.wplist.weapons[ID].Scale;
        myanim.GetComponent<SpriteRenderer>().sortingOrder = 1;
        myanim.GetComponent<SpriteRenderer>().flipX = pc.wplist.weapons[ID].FlipX;
        myanim.GetComponent<SpriteRenderer>().flipY = pc.wplist.weapons[ID].FlipY;
        myanim.GetComponent<Animator>().runtimeAnimatorController = animator.GetComponent<Animator>().runtimeAnimatorController;
        //myanim.transform.position = pc.wplist.weapons[ID].Transform;



        attackRange = pc.wplist.weapons[ID].attack_range;
        startTimeBtwAttack = pc.wplist.weapons[ID].start_time_attack;
    }

    private GameObject myweap;
    private GameObject myanim;
    private Collider2D[] AllColliders;
    private List<Collider2D> enemies = new List<Collider2D>();
    private float attackRange;
    public LayerMask enemy;

    private float timeBtwAttack;
    private float startTimeBtwAttack;
    public void Hit() {
        if (timeBtwAttack <= 0)
        {
            myanim.GetComponent<Animator>().SetTrigger("Hit");
            enemies.Clear();
            AllColliders = Physics2D.OverlapCircleAll(myweap.transform.position, attackRange, enemy);
            for (int i = 0; i < AllColliders.Length; i++) {
                if (AllColliders[i].isTrigger) {
                    enemies.Add(AllColliders[i]);
                }
            }
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].GetComponent<Enemy>().TakeDamage(pc.wplist.weapons[ID].damage);
            }
            timeBtwAttack = startTimeBtwAttack;
            print("Hit");
        }
    }

    public void Update()
    {
        if (timeBtwAttack > 0)
        {
            timeBtwAttack -= Time.deltaTime;
        }
    }

    private void OnDrawGizmosSelected() {
        try
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(myweap.transform.position, attackRange);
        }
        catch { }
    }


}
