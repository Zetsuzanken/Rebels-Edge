using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    [Header("Scroll Settings")]
    [Tooltip("Speed at which the camera scrolls upwards.")]
    public float scrollSpeed = 1f;

    private bool isScrolling = true;

    public void StopScrolling()
    {
        isScrolling = false;
    }

    public void StartScrolling()
    {
        isScrolling = true;
    }

    void Update()
    {
        if (isScrolling)
        {
            transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
        }
    }
}
