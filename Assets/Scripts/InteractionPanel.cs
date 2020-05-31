using System;
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


    private void Awake()
    {
        SharedInstance = this;
        gameObject.SetActive(false);
    }

    public void ShowInteraction(Sprite sprite, string text)
    {
        SpiderCharacterController.SharedInstance.SetHasControl(false);
        image.sprite = sprite;
        textbox.text = text;
        gameObject.SetActive(true);
        visible = true;
    }

    public void HideInteraction()
    {
        visible = false;
        gameObject.SetActive(false);
        SpiderCharacterController.SharedInstance.SetHasControl(true);
    }

    public void Update()
    {
        if (!visible) { return; }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            HideInteraction();
        }
    }
}
