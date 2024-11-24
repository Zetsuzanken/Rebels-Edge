using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofCollapse : MonoBehaviour
{
    public GameObject player;

    private Animator anim;
    private Movement movement;

    void Start()
    {
        anim = GetComponent<Animator>();
        movement = GetComponent<Movement>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (movement.IsJumping() == true)
        {
            return;
        }
        if (collision.CompareTag("Player"))
        {
            anim.SetTrigger("Explosion");
            Destroy(player);
        }
    }
}
