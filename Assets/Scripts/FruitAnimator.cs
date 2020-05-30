using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitAnimator : MonoBehaviour
{
    public Animator anim;
    public float minSecondsBetweenWave = 6;
    public float maxSecondsBetweenWave = 12;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Animate");
    }

    IEnumerator Animate()
    {
        while (this.isActiveAndEnabled)
        {
            yield return new WaitForSeconds(Random.Range(minSecondsBetweenWave, maxSecondsBetweenWave));
            anim.SetTrigger("Wave");
        }
    }
}
