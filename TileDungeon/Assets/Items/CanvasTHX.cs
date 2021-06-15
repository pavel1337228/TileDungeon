using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasTHX : MonoBehaviour
{
    public GameObject PickInfo;
    public TMP_Text PickInfo_Name;
    public TMP_Text PickInfo_Descr;
    public TMP_Text PickInfo_Damage;

    public GameObject PickButton;
    public GameObject NextButton;
    public void thx1() {
        StartCoroutine(THX());
    }
    public IEnumerator THX() {
        PickInfo.SetActive(true);
        yield return new WaitForSeconds(5f);
        PickInfo.SetActive(false);
    }
}
