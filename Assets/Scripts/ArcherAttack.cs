using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ArcherAttack : MonoBehaviour
{
    public GameObject player;
    public float shootInterval = 2f;
    public float detectionRadius = 5f;

    private Animator animator;
    private bool isPlayerInRange = false;
    private bool isShooting = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        CheckPlayerDistance();

        if (isPlayerInRange && !isShooting)
        {
            StartCoroutine(ShootContinuously());
        }
    }

    private void CheckPlayerDistance()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);
        isPlayerInRange = distance <= detectionRadius;
    }

    private IEnumerator ShootContinuously()
    {
        isShooting = true;

        while (isPlayerInRange)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;

            Debug.Log(direction.y + " y");
            Debug.Log(direction.x + " x");

            if (direction.x > 0) // Shooting right
            {
                if (transform.localScale.x < 0)
                    Flip();
                if (direction.x > -0.3f && direction.x < 0.3f && direction.y > 0)
                    animator.SetTrigger("shootUp");
                else if (direction.x > -0.3f && direction.x < 0.3f && direction.y < 0)
                    animator.SetTrigger("shootDown");
                else if (direction.y > 0.5f)
                    animator.SetTrigger("shootDiagUp");
                else if (direction.y < -0.5f)
                    animator.SetTrigger("shootDiagDown");
               
                else
                    animator.SetTrigger("shootRight");
            }
            else // Shooting left
            {
                if (transform.localScale.x > 0)
                    Flip();
                if (direction.x > -0.3f && direction.x < 0.3f && direction.y > 0)
                    animator.SetTrigger("shootUp");
                else if (direction.x > -0.3f && direction.x < 0.3f && direction.y < 0)
                    animator.SetTrigger("shootDown");
                else if (direction.y > 0.5f)
                    animator.SetTrigger("shootDiagUp");
                else if (direction.y < -0.5f)
                    animator.SetTrigger("shootDiagDown");
                else
                    animator.SetTrigger("shootRight");
            }
            animator.SetTrigger("Idle");

            yield return new WaitForSeconds(shootInterval);
        }

        isShooting = false;
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
