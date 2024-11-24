using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofCollapse : MonoBehaviour
{
    public GameObject player;

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
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
        Destroy(player);
    }

    public IEnumerator RestartAfterDelay(PlayerHealth playerHealth, float delay)
    {
        yield return new WaitForSeconds(delay);
        playerHealth.OnDeathAnimationComplete();
    }
}
