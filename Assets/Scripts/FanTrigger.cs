using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanTrigger : MonoBehaviour
{

    public float verticalSpeed = 9.81f;


    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.CompareTo("Player") == 0 || other.gameObject.tag.CompareTo("Projectile") == 0)
        {
            other.GetComponent<Rigidbody>().AddForce(Vector3.up * verticalSpeed, ForceMode.Acceleration);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        
    }

}
