using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVSceneController : MonoBehaviour
{

    public static TVSceneController SharedInstance;

    [Serializable]
    public struct Fruit
    {
        public string name;
        public GameObject normal;
        public GameObject enemy;
        bool isEnemy;
    }

    bool isAvacadoDead = false;
    public GameObject avacado;
    public GameObject avacadoDead;
    public List<Fruit> fruit;
    public AudioSource staticAudio;
    public List<AudioClip> audioClips;
    public Material staticMaterial;
    public Material tvSceneMaterial;
    public GameObject tvScreen;
    public Light tvLighting;
    bool inProgress = false;
    public Light roomLight;
    public Light localLight;
    public float evilTime = 1.0f;
    bool hasBeenEvil = false;

    public float timeMin = 4.0f;
    public float timeMax = 15.0f;
    float currentT;
    float targetT;

    private void Awake()
    {
        SharedInstance = this;
    }

    public void Update()
    {
        if (isAvacadoDead)
        {
            currentT += Time.deltaTime * Time.timeScale;
            if (currentT > targetT)
            {
                NextScene();
            }
        }


        /*if (Input.GetKeyDown(KeyCode.P))
        {
            NextScene();
        }*/
    }

    public void NextScene()
    {

        currentT = 0;
        targetT = UnityEngine.Random.Range(timeMin, timeMax);

        if (inProgress) { return; }
        
        if (!isAvacadoDead)
        {
            StartCoroutine(PlayStatic());
            KillAvacado();
        }
        else if(!hasBeenEvil)
        {
            StartCoroutine(ShowEvil());
        }
        else if(UnityEngine.Random.Range(0,4) == 1)
        {
            StartCoroutine(ShowEvil());
        }
        else
        {
            StartCoroutine(PlayStatic());
            SpawnEnemy();
        }


    }

    IEnumerator PlayStatic()
    {
        inProgress = true;
        Color oldColour = tvLighting.color;
        tvLighting.color = Color.white;
        tvScreen.GetComponent<Renderer>().material = staticMaterial;
        staticAudio.clip = audioClips[UnityEngine.Random.Range(0, audioClips.Count)];
        staticAudio.Play();
        yield return new WaitForSeconds(staticAudio.clip.length);
        tvScreen.GetComponent<Renderer>().material = tvSceneMaterial;
        tvLighting.color = oldColour;
        inProgress = false;
    }


    IEnumerator ShowEvil()
    {
        inProgress = true;
        
        Color oldColour = tvLighting.color;
        tvLighting.color = Color.white;
        tvScreen.GetComponent<Renderer>().material = staticMaterial;
        staticAudio.clip = audioClips[0];
        staticAudio.Play();
        yield return new WaitForSeconds(staticAudio.clip.length);

        SetAllEvil();
        tvScreen.GetComponent<Renderer>().material = tvSceneMaterial;
        tvLighting.color = Color.red;
        localLight.color = Color.red;
        roomLight.gameObject.SetActive(false);

        yield return new WaitForSeconds(evilTime);

        SetAllNice();
        tvLighting.color = Color.white;
        tvScreen.GetComponent<Renderer>().material = staticMaterial;
        staticAudio.clip = audioClips[0];
        staticAudio.Play();

        SpawnEnemy();


        yield return new WaitForSeconds(staticAudio.clip.length);

        localLight.color = Color.white;
        tvScreen.GetComponent<Renderer>().material = tvSceneMaterial;
        tvLighting.color = oldColour;
        roomLight.gameObject.SetActive(true);

        hasBeenEvil = true;

        inProgress = false;
    }

    void SetAllEvil()
    {
        foreach(Fruit f in fruit)
        {
            EvilThing e = f.normal.GetComponent<EvilThing>();
            if(e != null)
            {
                e.SetEvil(true);
            }
        }
    }

    void SetAllNice()
    {
        foreach (Fruit f in fruit)
        {
            EvilThing e = f.normal.GetComponent<EvilThing>();
            if (e != null)
            {
                e.SetEvil(false);
            }
        }
    }

    void SpawnEnemy()
    {
        Fruit f = fruit[UnityEngine.Random.Range(0, fruit.Count)];

        if (f.normal.activeSelf) { 
            f.normal.SetActive(false);
            //spawn

            f.enemy.SetActive(true);
            f.enemy.GetComponent<EnemyBehaviour>().Spawn();
        }
        
    }

    public void KilledEnemy(EnemyBehaviour enemy)
    {
        if (!inProgress)
        {
            StartCoroutine(PlayStatic());
        }
        foreach (Fruit f in fruit)
        {
            if (enemy.gameObject == f.enemy)
            {
                f.normal.SetActive(true);
                f.enemy.SetActive(false);
            }
        }
    }

    void KillAvacado()
    {
        avacado.SetActive(false);
        avacadoDead.SetActive(true);
        isAvacadoDead = true;
    }

}
