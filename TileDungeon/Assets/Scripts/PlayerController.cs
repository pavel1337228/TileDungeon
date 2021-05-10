using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    private PlayerInput _input;
    private Animator _anim;

    private void Awake() {
        _input = new PlayerInput();
        _anim = this.GetComponent<Animator>();
    }

    private void OnEnable() {
        _input.Enable();
    }

    public float f = 8.9f;
    private void Update() {
        Vector2 direction = _input.Player.Move.ReadValue<Vector2>();
        Move(direction);
    }

    private void OnDisable() {
        _input.Disable();
    }

    [SerializeField]private float _moveSpeed;
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

    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

}