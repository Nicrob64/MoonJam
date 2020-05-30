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

    void Start()
    {
        m_Rigidbody.AddForce(m_Rigidbody.transform.forward * speed);
        Destroy(gameObject, lifespan);
    }
}
