using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropRandom : MonoBehaviour
{
    public static DropRandom SharedInstance;
    public List<Transform> dropPoints;
    int count = 0;
    public GameObject canvas;
    public AudioSource staticEffect;
    public float delay = 0.2f;
    public Sprite sprite;

    private void Awake()
    {
        SharedInstance = this;
    }

    public void EventableDrop()
    {
        StartCoroutine(Drop());
    }

    public IEnumerator Drop()
    {
        SpiderCharacterController.SharedInstance.SetHasControl(false);

        canvas.SetActive(true);
        staticEffect.Play();

        Transform point = dropPoints[Random.Range(0, dropPoints.Count)];
        SpiderCharacterController.SharedInstance.gameObject.transform.position = point.position;
        SpiderCharacterController.SharedInstance.gameObject.transform.rotation = point.rotation;

        yield return new WaitForSeconds(delay);

        staticEffect.Pause();
        canvas.SetActive(false);

        if(count == 0)
        {
            yield return StartCoroutine(InteractionPanel.SharedInstance.ShowInteraction(sprite, "That was scary, looks like the chaos got to me and teleported me somewhere else. I better stay away from them"));
        }

        if (count == 10)
        {
            yield return StartCoroutine(InteractionPanel.SharedInstance.ShowInteraction(sprite, "Wow I sure am getting caught a lot, sometimes it feels like they just teleport in on top of me and get me. I should keep an eye on them."));
        }

        if (count == 20)
        {
            yield return StartCoroutine(InteractionPanel.SharedInstance.ShowInteraction(sprite, "I am impressively bad at this"));
        }

        count++;
        SpiderCharacterController.SharedInstance.SetHasControl(true);
    }

}
