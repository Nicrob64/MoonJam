using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlicker : MonoBehaviour
{

    public List<GameObject> enemies;

    public float minTimeBetween = 4;
    public float maxTimeBetween = 10;
    public float lightLength = 0.01f;

    void Start()
    {
        StartCoroutine("Flicker");
    }

    IEnumerator Flicker()
    {
        while (this.isActiveAndEnabled)
        {
            GameObject enemy = enemies[Random.Range(0, enemies.Count)];
            yield return new WaitForSeconds(UnityEngine.Random.Range(this.minTimeBetween, this.maxTimeBetween));
            enemy.SetActive(true);
            yield return new WaitForSeconds(this.lightLength);
            enemy.SetActive(false);
            yield return new WaitForSeconds(this.lightLength*2);
            enemy.SetActive(true);
            yield return new WaitForSeconds(this.lightLength);
            enemy.SetActive(false);
        }

    }
}
