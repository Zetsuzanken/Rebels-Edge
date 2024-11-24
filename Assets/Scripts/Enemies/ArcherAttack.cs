using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ArcherAttack : MonoBehaviour
{
    public GameObject player;
    public float shootInterval = 2f;
    public float detectionRadius = 5f;
    public GameObject arrowPrefab;
    public Transform arrowSpawnPoint;

    private Animator animator;
    private bool isPlayerInRange = false;
    private bool isShooting = false;
    private EnemiesHealth enemiesHealth;

    void Start()
    {
        animator = GetComponent<Animator>();
        enemiesHealth = GetComponent<EnemiesHealth>();
    }

    void Update()
    {
        if (enemiesHealth.IsDead())
        {
            return;
        }

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

        while (isPlayerInRange && !enemiesHealth.IsDead())
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;

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

            yield return new WaitForSeconds(1f);

            SpawnArrow();

            animator.SetTrigger("Idle");

            yield return new WaitForSeconds(shootInterval);
        }

        isShooting = false;
    }

    private void SpawnArrow()
    {
        if (player == null) return;

        GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, Quaternion.identity);
        ArrowProjectile arrowScript = arrow.GetComponent<ArrowProjectile>();
        if (arrowScript != null)
        {
            arrowScript.SetTarget(player.transform.position);
        }
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
