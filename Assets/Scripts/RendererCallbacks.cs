using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RendererCallbacks : MonoBehaviour
{
    private void OnBecameVisible()
    {
        Debug.Log("I have been seen renderer");
    }

    private void OnBecameInvisible()
    {
        Debug.Log("No longer here");
    }
}
