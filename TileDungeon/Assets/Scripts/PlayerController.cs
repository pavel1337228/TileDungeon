using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInput _input;
    private Animator _anim;

    public void Awake() {
        _anim = this.GetComponent<Animator>();
        _vinetka.SetActive(true);

        thisWeapon = gameObject.GetComponent<WeaponScript>();
        StartCoroutine(thisWeapon.ChangeWeapon());
    }
    private void Update() {
        float scaledCameraMoveSpeed = _cameraMoveSpeed * Time.deltaTime;
        Camera.main.transform.position = new Vector3(Mathf.Lerp(Camera.main.transform.position.x, transform.position.x, scaledCameraMoveSpeed), Mathf.Lerp(Camera.main.transform.position.y, transform.position.y, scaledCameraMoveSpeed), -10);
        _vinetka.transform.position = new Vector3(Mathf.Lerp(_vinetka.transform.position.x, transform.position.x, scaledCameraMoveSpeed * 3), Mathf.Lerp(_vinetka.transform.position.y, transform.position.y, scaledCameraMoveSpeed * 3));

        Move(_moveDerection);
    }

    public void OnMoveMouse(InputAction.CallbackContext context)
    {
        MouseAim(context.ReadValue<Vector2>());
    }
    private void MouseAim(Vector2 mousePosition)
    {
        Vector3 _mousePosition = new Vector3(mousePosition.x, mousePosition.y, 0);


        Vector3 orbVector = Camera.main.WorldToScreenPoint(pivot.position);
        orbVector = _mousePosition - orbVector;
        float angle = Mathf.Atan2(orbVector.y, orbVector.x) * Mathf.Rad2Deg;

        //pivot.position = orb.position;
        pivot.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }

    private Transform orb;

    private Transform pivot;
    void Start()
    {
        //pivot = orb.transform;
        orb = Weapon.transform;
        pivot = WeaponOrbit.transform;

        Weapon.transform.parent = pivot;
        Weapon.transform.position += Vector3.up * radius;
    }

    [Header("Speed And Vinetka")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _cameraMoveSpeed;
    [SerializeField] private GameObject _vinetka;
    [Header("Weapons")]
    [SerializeField] public int id_weapon = 0;
    [SerializeField] public WeaponList wplist;

    [Header("Weapons Settings")]
    private WeaponScript thisWeapon;
    [SerializeField] private GameObject WeaponOrbit;
    [SerializeField] public GameObject Weapon;
    public float radius;
    //public float offset = 0.1f;
    private Vector2 mousePosition;
    private Vector3 mousePos;

    private Vector2 _moveDerection;
    public void OnMove(InputAction.CallbackContext context) {
        _moveDerection = context.ReadValue<Vector2>();
    }

    public void OnHit(InputAction.CallbackContext context)
    {
        Hit();
    }

    private void Hit()
    {
        thisWeapon.Hit();
    }
    private void Move(Vector2 direction)
    {
        float scaledMoveSpeed = _moveSpeed * Time.deltaTime;
        if ((direction.x != 0) | (direction.y != 0)) { _anim.SetTrigger("Run"); }
        else { _anim.SetTrigger("StopRun"); }


        Vector3 moveDirection = new Vector3(direction.x, direction.y, 0);
        if (direction.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        if (direction.x > 0)
        { 
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        transform.position += moveDirection * scaledMoveSpeed;
    }

}

public static class ExtensionMethods
{
    public static T ReturnRandom<T>(this List<T> items) where T : IRandom
    {
        System.Random _r = new System.Random();
        List<float> probs = new List<float>();
        float total = 0f;
        for (int i = 0; i < items.Count; i++)
        {
            probs.Add(items[i].itemDropChance);
            total += items[i].itemDropChance;
        }

        int index = probs.Count - 1;

        float RandomPoint = (float)_r.NextDouble() * total;

        for (int i = 0; i < probs.Count; i++)
        {
            if (RandomPoint < probs[i])
            {
                index = i;
                break;

            }
            else
                RandomPoint -= probs[i];
        }

        return items[index];

    }

    #region LookAt2D
    public static void LookAt2D(this Transform me, Vector3 target, Vector3? eye = null)
    {
        float signedAngle = Vector2.SignedAngle(eye ?? me.up, target - me.position);

        if (Mathf.Abs(signedAngle) >= 1e-3f)
        {
            var angles = me.eulerAngles;
            angles.z += signedAngle;
            me.eulerAngles = angles;
        }
    }
    public static void LookAt2D(this Transform me, Transform target, Vector3? eye = null)
    {
        me.LookAt2D(target.position, eye);
    }
    public static void LookAt2D(this Transform me, GameObject target, Vector3? eye = null)
    {
        me.LookAt2D(target.transform.position, eye);
    }
    #endregion
    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

}