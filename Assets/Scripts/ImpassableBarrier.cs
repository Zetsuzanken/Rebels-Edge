using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpassableBarrier : MonoBehaviour
{
    private bool playerInBarrierZone;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInBarrierZone = true;
            other.GetComponent<Movement>().SetBarrierState(true); // Inform the Movement script
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInBarrierZone = false;
            other.GetComponent<Movement>().SetBarrierState(false); // Inform the Movement script
        }
    }

    public bool IsPlayerBlocked()
    {
        return playerInBarrierZone;
    }
}
