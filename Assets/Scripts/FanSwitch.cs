using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanSwitch : MonoBehaviour
{

    public List<GameObject> turnOn;
    public Light lightToUpdate;
    public Material ledMat;
    public Renderer led;
    public InteractionTrigger collisionTrigger;
    public List<GameObject> turnOff;

    public void SetSwitchActivated()
    {
        foreach(GameObject o in turnOn)
        {
            o.SetActive(true);
        }
        foreach(GameObject d in turnOff)
        {
            d.SetActive(false);
        }
        lightToUpdate.color = Color.green;
        led.material = ledMat;
        Component.Destroy(collisionTrigger);
    }
}
