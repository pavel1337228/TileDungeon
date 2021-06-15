using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
public class NewLevel : MonoBehaviour
{
    private TilemapCollider2D tilemapCollider2D;
    public LayerMask gamer;
    public GameObject btn;
    void Awake()
    {
        try { GameObject Canvas = GameObject.Find("Canvas");
            btn = Canvas.GetComponent<CanvasTHX>().NextButton;
        }
        catch { print(":("); }
        btn.SetActive(false);
        tilemapCollider2D = GetComponent<TilemapCollider2D>();
        tilemapCollider2D.IsTouchingLayers(gamer);
    }

    void LateUpdate() {
        if (tilemapCollider2D.IsTouchingLayers(gamer))
        {
            btn.SetActive(true);
        }
        else {
            btn.SetActive(false);
        }
    }

    public void NextScene() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (SceneManager.sceneCountInBuildSettings -1  >= SceneManager.GetActiveScene().buildIndex + 1)
        {
            player.GetComponent<Player>().id_room = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadScene(player.GetComponent<Player>().id_room, LoadSceneMode.Single);
        }
        else
        {
            player.GetComponent<Player>().id_room = SceneManager.GetActiveScene().buildIndex;
            print(":( i dont have BOY NEXT DOOR");
        }
        btn.SetActive(false);
    }

}
