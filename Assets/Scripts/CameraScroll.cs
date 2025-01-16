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

    public AudioClip levelStartClip;

    // Position to play the sound (default is the object's position)
    public Vector3 soundPosition;

    void Start()
    {
        if (soundPosition == Vector3.zero)
        {
            soundPosition = transform.position;
        }

        // Check if the AudioClip is assigned
        if (levelStartClip != null)
        {
            // Play the clip at the specified position
            AudioSource.PlayClipAtPoint(levelStartClip, soundPosition);
        }
        else
        {
            Debug.LogWarning("No AudioClip assigned for level start sound!");
        }

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
