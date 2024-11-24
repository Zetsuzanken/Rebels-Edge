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
        if (collision.CompareTag("Player"))
        {
            anim.SetTrigger("Explosion");
            Destroy(player);
        }
    }
}
