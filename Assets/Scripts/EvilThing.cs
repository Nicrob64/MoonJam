using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilThing : MonoBehaviour
{

    public GameObject mask;
    public Material evilMaterial;
    public Material goodMaterial;
    public Renderer theRenderer;

    bool isEvil = false;

    public void SetEvil(bool evil)
    {
        if(isEvil == evil) { return; }
        if (evil)
        {
            mask.SetActive(true);
            gameObject.GetComponentInChildren<Renderer>().material = evilMaterial;
        }
        else
        {
            mask.SetActive(false);
            gameObject.GetComponentInChildren<Renderer>().material = goodMaterial;
        }
        isEvil = evil;
    }
}
