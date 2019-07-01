using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SoundStarter : MonoBehaviour
{
    public AudioSource Debug;
    public float Delay = 0;

    void OnEnable()
    {
        if (Debug != null)
        {
            Debug.time = Delay;
            Debug.Play();
        }
    }

}
