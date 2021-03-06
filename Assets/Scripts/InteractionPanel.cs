﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionPanel : MonoBehaviour
{

    public Image image;
    public Text textbox;

    public static InteractionPanel SharedInstance;
    private bool visible = false;
    private bool shouldHide = false;


    private void Awake()
    {
        SharedInstance = this;
        gameObject.SetActive(false);
    }

    public IEnumerator ShowInteraction(Sprite sprite, string text)
    {
        //SpiderCharacterController.SharedInstance.SetHasControl(false);
        image.sprite = sprite;
        textbox.text = text;
        gameObject.SetActive(true);
        visible = true;
        shouldHide = false;
        while (!shouldHide)
        {
            yield return null; //wait for input
        }
        HideInteraction();
    }

    public void HideInteraction()
    {
        visible = false;
        gameObject.SetActive(false);
        //SpiderCharacterController.SharedInstance.SetHasControl(true);
    }

    public void Update()
    {
        if (!visible) { return; }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            shouldHide = true;
            //HideInteraction();
        }
    }
}
