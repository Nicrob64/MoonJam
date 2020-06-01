using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class InteractionTrigger : MonoBehaviour
{

    public enum Tag
    {
        PLAYER,
        PROJECTILE
    }


    [Serializable]
    public struct CameraTransition
    {
        public Transform trans;
        public float transitionDuration;
    }

    [Serializable]
    public struct Interaction
    {
        public string message;
        public Sprite image;
        public UnityEvent addedEvent;
        public CameraTransition cameraTransition;
    }
    public List<Interaction> interactions;
    int interactionCount = 0;
    public bool randomize = false;
    public bool repeat = false;
    public bool useCollisionInsteadOfTrigger = false;
    bool isInteracting = false;
    bool queueNextInteraction = false;
    public Tag tagToCheck = Tag.PLAYER;
    string checkTag;


    public void Awake()
    {
        switch (tagToCheck)
        {
            case Tag.PLAYER:
                {
                    checkTag = "Player";
                    break;
                }
            case Tag.PROJECTILE:
                {
                    checkTag = "Projectile";
                    break;
                }
        }
    }

    public void QueueNextInteraction()
    {
        if (!isInteracting) { return; }
        else queueNextInteraction = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (useCollisionInsteadOfTrigger) { return; }
        if(other.gameObject.tag.CompareTo(checkTag) == 0)
        {
            StartCoroutine(DoInteract());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!useCollisionInsteadOfTrigger) { return; }
        if (collision.collider.gameObject.tag.CompareTo(checkTag) == 0)
        {
            StartCoroutine(DoInteract());
        }
    }

    IEnumerator DoInteract()
    {
        if (interactionCount >= interactions.Count) { yield break; }
        this.isInteracting = true;
        SpiderCharacterController.SharedInstance.SetHasControl(false);
        Vector3 originalPositionOfCamera = Vector3.zero;
        Quaternion originalRotationOfCamera = Quaternion.identity;
        if (Camera.main != null) { originalPositionOfCamera = Camera.main.transform.position; originalRotationOfCamera =Camera.main.transform.rotation; };
        
        do
        {
            queueNextInteraction = false;

            Interaction i = interactions[interactionCount];


            Transform t = null;
            if(Camera.main != null)
            {
                t = Camera.main.transform;
            }
            Vector3 initialPosition = Vector3.zero;
            Vector3 targetPosition = Vector3.zero;
            Quaternion initialRotation = Quaternion.identity;
            Quaternion targetRotation = Quaternion.identity;

            bool hasTransition = i.cameraTransition.trans != null && t != null && Camera.main != null;
            hasTransition = hasTransition && i.cameraTransition.trans.gameObject.name.CompareTo("this_is_a_stupid_hack") != 0;

            if (hasTransition)
            {
                initialPosition = t.position;
                targetPosition = i.cameraTransition.trans.position;
                initialRotation = t.rotation;
                targetRotation = i.cameraTransition.trans.rotation;
            }


            if (hasTransition)
            {
                yield return StartCoroutine(Transition(Camera.main.gameObject, initialPosition, targetPosition, initialRotation, targetRotation, i.cameraTransition.transitionDuration));
            }

            if (i.addedEvent != null)
            {
                i.addedEvent.Invoke();
            }
            if (i.image != null)
            {
                yield return StartCoroutine(InteractionPanel.SharedInstance.ShowInteraction(i.image, i.message));
            }

            //chain camera pans if possible
            if (hasTransition && !(queueNextInteraction && interactions[interactionCount + 1].cameraTransition.trans != null))
            {
                yield return StartCoroutine(Transition(Camera.main.gameObject, targetPosition, originalPositionOfCamera, targetRotation, originalRotationOfCamera, i.cameraTransition.transitionDuration));
            }

            interactionCount++;
            if (repeat)
            {
                interactionCount %= interactions.Count;
            }
        } while (queueNextInteraction);
        this.isInteracting = false;
        SpiderCharacterController.SharedInstance.SetHasControl(true);


    }


    IEnumerator Transition(GameObject thingToMove, Vector3 from, Vector3 to, Quaternion fromRotation, Quaternion toRotation, float duration)
    {

        float t = 0.0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime * (Time.timeScale / duration);


            thingToMove.transform.position = Vector3.Slerp(from, to, t);
            thingToMove.transform.rotation = Quaternion.Slerp(fromRotation, toRotation, t);
            
            yield return null;
        }

    }

}
