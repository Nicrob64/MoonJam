using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    public float transitionDuration = 6.0f;
    public Transform destination;
    public GameObject posedFruit;

    public List<GameObject> turnOff;
    public Light tvLight;

    public EvilThing b;
    public EvilThing w;
    public EvilThing p;
    public EvilThing pine;

    public Renderer tvScreen;
    public Material newCamTexture;
    public Camera cam;
    public AudioSource source;
    public AudioSource othersource;
    public Sprite endSprite;

    public void StartIt()
    {
        StartCoroutine(StartCutscene());
    }

    public void PrepareCutscene()
    {
        tvScreen.material = newCamTexture;
        posedFruit.SetActive(true);
    }

    public IEnumerator StartCutscene()
    {
        
        othersource.volume = 0.1f;
        othersource.Play();
        

        tvLight.color = Color.red;
        yield return StartCoroutine(Transition(cam.gameObject, cam.transform.position, destination.position, cam.transform.rotation, destination.rotation, transitionDuration));

        foreach (GameObject g in this.turnOff)
        {
            yield return new WaitForSeconds(1f);
            g.SetActive(false);
        }
        othersource.Stop();
        source.volume = 0.1f;
        source.Play();
        yield return new WaitForSeconds(1.2f);
        b.SetEvil(true);
        source.volume = 0.2f;
        yield return new WaitForSeconds(1f);
        w.SetEvil(true);
        source.volume = 0.3f;
        yield return new WaitForSeconds(1.5f);
        p.SetEvil(true);
        source.volume = 0.4f;
        yield return new WaitForSeconds(1.3f);
        pine.SetEvil(true);
        source.volume = 0.5f;
        yield return new WaitForSeconds(0.5f);
        tvScreen.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        tvLight.enabled = false;

        //w.transform.LookAt(cam.transform);

        float t = 0.0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime * (Time.timeScale / 3f);


            Vector3 relativePos = cam.transform.position - p.transform.position;
            Quaternion toRotation = Quaternion.LookRotation(relativePos);
            p.transform.rotation = Quaternion.Lerp(p.transform.rotation, toRotation, t);
            
            Vector3 relativePos2 = cam.transform.position - b.transform.position;
            Quaternion toRotation2 = Quaternion.LookRotation(relativePos2);
            b.transform.rotation = Quaternion.Lerp(b.transform.rotation, toRotation2, t);

            Vector3 relativePos3 = cam.transform.position - pine.transform.position;
            Quaternion toRotation3 = Quaternion.LookRotation(relativePos3);
            pine.transform.rotation = Quaternion.Lerp(pine.transform.rotation, toRotation3, t);

            yield return null;
        }

        source.Stop();
        w.gameObject.SetActive(false);
        p.gameObject.SetActive(false);
        pine.gameObject.SetActive(false);
        b.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(InteractionPanel.SharedInstance.ShowInteraction(endSprite, "The end. \n\nMade by nicrob64 in chat, i made this :)"));

        Application.Quit();

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
