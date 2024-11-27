using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{

    public AudioClipGroup Group1;
    public AudioClipGroup Group2;
    public AudioClipGroup Group3;


    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            Group1.Play();
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            Group2.Play();
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            Group3.Play();
        }
    }
}
