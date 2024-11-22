using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    [Header("Scroll Settings")]
    [Tooltip("Speed at which the camera scrolls upwards.")]
    public float scrollSpeed = 1f;

    private bool isScrolling = true;

    private GameObject player;
    private PlayerHealth playerHealth;
    private Collider2D playerCollider;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
            playerCollider = player.GetComponent<Collider2D>();
        }
    }

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
            CheckPlayerOutOfView();
        }
    }

    void CheckPlayerOutOfView()
    {
        if (player == null || playerCollider == null || playerHealth == null)
            return;

        if (playerHealth.IsDead())
            return;

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

        if (!GeometryUtility.TestPlanesAABB(planes, playerCollider.bounds))
        {
            playerHealth.InstantDeath();
        }
    }
}
