using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.XR.WSA.Input;
using UnityEngine.Events;

public class InteractionTrigger : MonoBehaviour
{

    [Serializable]
    public struct Interaction
    {
        public string message;
        public Sprite image;
        public UnityEvent addedEvent;
    }
    public List<Interaction> interactions;
    int interactionCount = 0;
    public bool randomize = false;
    public bool repeat = false;
    public bool useCollisionInsteadOfTrigger = false;

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (useCollisionInsteadOfTrigger) { return; }
        Debug.Log(other.gameObject.tag);
        if(other.gameObject.tag.CompareTo("Player") == 0)
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
        Interaction i = interactions[interactionCount];
        if(i.addedEvent != null)
        {
            i.addedEvent.Invoke();
        }
        InteractionPanel.SharedInstance.ShowInteraction(i.image, i.message);
        interactionCount++;
        if (repeat)
        {
            interactionCount %= interactions.Count;
        }
    }
}
