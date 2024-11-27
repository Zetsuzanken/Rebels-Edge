using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofCollapse : MonoBehaviour
{
    public GameObject player;

    private Animator anim;
    private AudioSource audioSource; // AudioSource komponent
    public AudioClip collapseSound;  // Heli, mida mängitakse kokkuvarisemisel

    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>(); // Oletame, et AudioSource on sama objektiga
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Movement movement = collision.GetComponent<Movement>();
        PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

        if (collision.CompareTag("Player"))
        {
            if (movement.IsJumping() == true)
            {
                return;
            }
            Explosion();
            StartCoroutine(RestartAfterDelay(playerHealth, 1.5f));
        }
    }

    public void Explosion()
    {
        anim.SetTrigger("Explosion");
        PlayCollapseSound();  // Mängib kokkuvarisemise heli
        Destroy(player);
    }

    public void PlayCollapseSound()
    {
        if (audioSource != null && collapseSound != null)
        {
            audioSource.PlayOneShot(collapseSound);  // Mängib heli
        }
    }

    public IEnumerator RestartAfterDelay(PlayerHealth playerHealth, float delay)
    {
        yield return new WaitForSeconds(delay);
        playerHealth.OnDeathAnimationComplete();
    }
}
