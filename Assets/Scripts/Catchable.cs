using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Catchable : MonoBehaviour
{

    public bool isCaught = false;
    public float minTimeBetweenStruggle = 6f;
    public float maxTimeBetweenStruggle = 12f;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (isCaught) { StartCoroutine("Struggle"); }
    }

    public void SetCaught(bool caught)
    {
        this.isCaught = caught;
        if (this.isCaught)
        {
            StartCoroutine("Struggle");
        }
    }


    IEnumerator Struggle()
    {
        while (isCaught)
        {
            anim.SetTrigger("Struggle");
            float timeToWait = Random.Range(minTimeBetweenStruggle, maxTimeBetweenStruggle);
            yield return new WaitForSeconds(timeToWait);
        }
        
        
    }

}
