using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    public Light _light;
    public AudioSource _audio;
    public float minTimeBetween = 4;
    public float maxTimeBetween = 10;
    public float minThunderDelay = 0.5f;
    public float maxThunderDelay = 1.5f;
    public float lightLength = 0.1f;
    public List<AudioClip> audioClips;
 
    void Start()
    {
        StartCoroutine("Flicker");
    }

    IEnumerator Flicker()
    {
        while (this.isActiveAndEnabled)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(this.minTimeBetween, this.maxTimeBetween));
            this._light.enabled = true;
            yield return new WaitForSeconds(this.lightLength);
            this._light.enabled = false;
            yield return new WaitForSeconds(UnityEngine.Random.Range(this.minThunderDelay, this.maxThunderDelay));
            AudioClip clipToPlay = this.audioClips[UnityEngine.Random.Range(0, this.audioClips.Count)];
            this._audio.clip = clipToPlay;
            this._audio.Play();
            yield return new WaitForSeconds(clipToPlay.length);
            this._audio.Stop();
        }

    }
}
