using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public float speed = 10f;   
    public float lifespan = 3f; 

    private Rigidbody m_Rigidbody;

 
    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    public void Fire(Vector3 dir)
    {
        m_Rigidbody.AddForce(dir * speed, ForceMode.VelocityChange);
        StartCoroutine("Fade");
    }

    IEnumerator Fade()
    {
        yield return new WaitForSeconds(lifespan);
        gameObject.SetActive(false);
    }
}
