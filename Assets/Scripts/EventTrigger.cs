using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventTrigger : MonoBehaviour
{
    public List<UnityEvent> interactions;
    int interactionCount = 0;
    public bool randomize = false;
    public bool repeat = false;
    public bool useCollisionInsteadOfTrigger = false;

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (useCollisionInsteadOfTrigger) { return; }
        if (other.gameObject.tag.CompareTo("Player") == 0)
        {
            DoInteract();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!useCollisionInsteadOfTrigger) { return; }
        Debug.Log(collision.collider.gameObject.tag);
        if (collision.collider.gameObject.tag.CompareTo("Player") == 0)
        {
            DoInteract();
        }
    }

    void DoInteract()
    {
        if (interactionCount >= interactions.Count) { return; }
        UnityEvent i = interactions[interactionCount];
        i.Invoke();
        interactionCount++;
        if (repeat)
        {
            interactionCount %= interactions.Count;
        }
    }
}
