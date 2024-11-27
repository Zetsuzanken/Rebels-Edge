using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    public float speed = 0.5f;
    public float sprintMultiplier = 2f;
    public Rigidbody2D rb;
    public Vector2 input;

    Animator anim;
    private Vector2 lastMoveDirection;
    private bool facingLeft = true;
    public Vector2 moveVelocity = new Vector2(0, 0);

    public float jumpScale = 1.5f;
    public float jumpHeight = 2f;
    public float jumpDuration = 0.5f;
    public float jumpCooldown = 1f; 
    private bool isJumping = false;
    private bool canJump = true;

    private LevelEnd LevelEnd;

    private PlayerHealth playerHealth;
    private RoofCollapse roofCollapse;

    private bool isOnGround = true;

    private Barrier barrier;
    private bool isAgainstImpassableBarrier;
    private bool isNearBarrier = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        playerHealth = GetComponent<PlayerHealth>();
        barrier = FindObjectOfType<Barrier>();
    }

    public void SetBarrierState(bool isBlocked)
    {
        isAgainstImpassableBarrier = isBlocked; 
    }

    public void SetBarrierStatus(bool status)
    {
        isNearBarrier = status;
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();
        Animate();
        if(input.x < 0 && !facingLeft || input.x > 0 && facingLeft && playerHealth.currentHealth > 0)
        {
            Flip();
        }

        if (Input.GetKeyDown(KeyCode.Space) && canJump) 
        {
            StartCoroutine(Jump());
        }


        if (!isOnGround && !isJumping)
        {
            playerHealth.InstantDeath();
        }

    }

    public void FixedUpdate()
    {
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? speed * sprintMultiplier : speed;

        moveVelocity = input * currentSpeed;

     
        if (isJumping)
        {
            moveVelocity.y = rb.velocity.y;
        }

        if (playerHealth.currentHealth <= 0) 
        {
            moveVelocity = Vector2.zero;
            canJump = false;
        }

        if (isNearBarrier)
        {
            Vector2 playerToBarrier = (barrier.transform.position - transform.position).normalized;
            float dotProduct = Vector2.Dot(moveVelocity.normalized, playerToBarrier);

            if (dotProduct > 0)
            {
                moveVelocity = Vector2.zero;
            }
        }

        if (isAgainstImpassableBarrier)
        {
            Vector2 playerToBarrier = (GetComponent<Collider2D>().bounds.center - barrier.transform.position).normalized;
            float dotProduct = Vector2.Dot(moveVelocity.normalized, playerToBarrier);

            if (dotProduct > 0)
            {
               
                float projection = Vector2.Dot(moveVelocity, playerToBarrier);
                moveVelocity -= projection * playerToBarrier;
            }
        }
        else
        {
            canJump = true;
        }

        rb.velocity = moveVelocity;
    }

    public bool IsJumping()
    {
        return isJumping;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = false;
        }
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if ((moveX == 0 && moveY == 0) && (input.x != 0 || input.y != 0))
        {
            lastMoveDirection = input;
        }

        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        input.Normalize();
    }

    void Animate()
    {
        anim.SetFloat("MoveX", input.x);
        anim.SetFloat("MoveY", input.y);
        anim.SetFloat("MoveMagnitude", input.magnitude);
        anim.SetFloat("LastMoveX", lastMoveDirection.x);
        anim.SetFloat("LastMoveY", lastMoveDirection.y);
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        facingLeft = !facingLeft;
    }

    IEnumerator Jump()
    {
        isJumping = true;
        canJump = false;

        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = originalScale * jumpScale;
        float startY = transform.position.y;
        float targetY = startY + jumpHeight;

        float elapsedTime = 0f;
        float halfJumpDuration = jumpDuration / 2;

        // First half of the jump: scale up and move upwards
        while (elapsedTime < halfJumpDuration)
        {
            // Update scale
            transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / halfJumpDuration);

            // Calculate the new y position
            float newY = Mathf.Lerp(startY, targetY, elapsedTime / halfJumpDuration);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            // Combine y movement with input
            transform.position += new Vector3(0, input.y * speed * Time.deltaTime, 0);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Second half of the jump: scale down and move downwards
        elapsedTime = 0f;
        while (elapsedTime < halfJumpDuration)
        {
            // Update scale
            transform.localScale = Vector3.Lerp(targetScale, originalScale, elapsedTime / halfJumpDuration);

            // Calculate the new y position
            float newY = Mathf.Lerp(targetY, startY + input.y, elapsedTime / halfJumpDuration);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            // Combine y movement with input
            transform.position += new Vector3(0, input.y * speed * Time.deltaTime, 0);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Restore original scale and reset jumping state
        transform.localScale = originalScale;
        isJumping = false;

        // Wait for the jump cooldown before allowing another jump
        yield return new WaitForSeconds(jumpCooldown);
        canJump = true;
    }
}
