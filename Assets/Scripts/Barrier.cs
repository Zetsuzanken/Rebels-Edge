using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//script

public class Barrier : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Movement playerMovement = other.GetComponent<Movement>();
            if (playerMovement != null)
            {
                playerMovement.SetBarrierStatus(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Movement playerMovement = other.GetComponent<Movement>();
            if (playerMovement != null)
            {
                playerMovement.SetBarrierStatus(false);
            }
        }
    }
}
