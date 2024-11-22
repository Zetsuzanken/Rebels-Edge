using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    public float speed;
    public GameObject player;

    private Rigidbody2D rb;
    private Animator anim;
    private Transform currentPoint;
    private bool detectedPlayer;
    private bool returningToPatrol;
    private Vector2 direction;

    void Start()
    {
        detectedPlayer = false;
        returningToPatrol = false;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentPoint = pointB.transform;
        anim.SetBool("isRunning", true);
    }

    void Update()
    {
        if (detectedPlayer)
        {
            FollowPlayer();
        }
        else if (returningToPatrol)
        {
            ReturnToPointA();
        }
        else
        {
            PatrolBetweenPoints();
        }
    }

    private void FollowPlayer()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance <= 0.5f)
        {
            rb.velocity = Vector2.zero;
            anim.SetBool("isRunning", false);
        }
        else
        {
            direction = (player.transform.position - transform.position).normalized;
            rb.velocity = direction * speed;
       
            anim.SetBool("isRunning", true);

            if (direction.x > 0 && transform.localScale.x < 0 || direction.x < 0 && transform.localScale.x > 0)
            {
                Flip();
            }
        }
    }

    private void PatrolBetweenPoints()
    {
        if ((currentPoint == pointB.transform && transform.localScale.x < 0) ||
        (currentPoint == pointA.transform && transform.localScale.x > 0))
        {
            Flip();
        }

        rb.velocity = new Vector2(currentPoint == pointB.transform ? speed : -speed, 0);
      
        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f)
        {
            currentPoint = currentPoint == pointB.transform ? pointA.transform : pointB.transform;
        }
    }

    private void ReturnToPointA()
    {
        direction = (pointA.transform.position - transform.position).normalized;
        rb.velocity = direction * speed;

        if (Vector2.Distance(transform.position, pointA.transform.position) < 0.5f)
        {
            rb.velocity = Vector2.zero;
            returningToPatrol = false;
            currentPoint = pointB.transform;
        }

        if (direction.x > 0 && transform.localScale.x < 0 || direction.x < 0 && transform.localScale.x > 0)
        {
            Flip();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            detectedPlayer = true;
            returningToPatrol = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Patrol"))
        {
            detectedPlayer = false;
            returningToPatrol = true;
        }
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
    }
}
