using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttontoNextLvl : MonoBehaviour
{

    GameObject NewLevel;
    public void Click() {
        try
        {
            NewLevel = GameObject.FindGameObjectWithTag("NextDoor");
        }
        catch {
            gameObject.SetActive(false);
            return;
        }
        NewLevel.GetComponent<NewLevel>().NextScene();
    }
}
