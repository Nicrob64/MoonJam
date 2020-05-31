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

    private void Awake()
    {
        SharedInstance = this;
    }

    public void NextScene()
    {
        if (inProgress) { return; }
        StartCoroutine(PlayStatic());
        if (!isAvacadoDead)
        {
            KillAvacado();
        }
        else {
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


    void SpawnEnemy()
    {
        Fruit f = fruit[UnityEngine.Random.Range(0, fruit.Count)];
        f.normal.SetActive(false);
    }

    void KillAvacado()
    {
        avacado.SetActive(false);
        avacadoDead.SetActive(true);
        isAvacadoDead = true;
    }

}
