using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileDamager : MonoBehaviour
{
    private TilemapCollider2D tilemapCollider2D;
    public LayerMask gamer;
    public int damege_sprite;
    private float timer;
    // Start is called before the first frame update
    void Awake()
    {
        tilemapCollider2D = GetComponent<TilemapCollider2D>();
        tilemapCollider2D.IsTouchingLayers(gamer);

        timer = 0;

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (damege_sprite <= (int)timer) {
            if (tilemapCollider2D.IsTouchingLayers(gamer))
            {
                Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
                player.TakeDamage(15);
            }
            timer = 0;
        }
    }
}
