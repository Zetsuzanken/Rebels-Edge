using System.Collections;
using UnityEngine;
using Unity;

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

    private AudioSource audioSource; // Üks AudioSource kõigile helidele
    private AudioSource deathAudioSource; // Eraldi AudioSource surma heli jaoks

    public AudioClip walkSound;
    public AudioClip jumpSound;
    public AudioClip deathSound;

    private bool isWalking = false;

    private bool hasPlayedDeathSound = false; // Lisatud lipp surma heli kordumise vältimiseks

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        playerHealth = GetComponent<PlayerHealth>();
        barrier = FindObjectOfType<Barrier>();
        audioSource = GetComponent<AudioSource>(); // Kõikide helide jaoks üks AudioSource
        deathAudioSource = gameObject.AddComponent<AudioSource>(); // Eraldi AudioSource surma heli jaoks
        deathAudioSource.volume = 1f; // Surma heli helitugevus
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

        if (input.x < 0 && !facingLeft || input.x > 0 && facingLeft && playerHealth.currentHealth > 0)
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

        if (playerHealth.currentHealth <= 0 && !hasPlayedDeathSound)
        {
            PlayDeathSound();
            hasPlayedDeathSound = true; // Surmaheli mängitakse ainult üks kord
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

        // Kõndimise heli mängimine, kui liikumine toimub
        if (input.magnitude > 0 && isOnGround && !isJumping && !audioSource.isPlaying && !isWalking)
        {
            isWalking = true;
            PlayWalkSound();
        }
        else if (input.magnitude == 0 && isWalking)
        {
            isWalking = false;
            audioSource.Stop();
        }
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
        /*
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        facingLeft = !facingLeft;
        */

        facingLeft = !facingLeft;
        input.x *= -1;
    }

    IEnumerator Jump()
    {
        isJumping = true;
        canJump = false;

        anim.SetTrigger("Jump");

        // Peatame kõik helid, kuid ei peata surma heli, kui see mängib
        StopAllSoundsExceptDeath();
        if (jumpSound != null)
        {

            audioSource.pitch = 1;
            audioSource.PlayOneShot(jumpSound); // Hüppe heli mängimine kohe
        }

        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = originalScale * jumpScale;
        float startY = transform.position.y;
        float targetY = startY + jumpHeight;

        float elapsedTime = 0f;
        float halfJumpDuration = jumpDuration / 2;

        // Esimene pool hüpist
        while (elapsedTime < halfJumpDuration)
        {
            transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / halfJumpDuration);
            float newY = Mathf.Lerp(startY, targetY, elapsedTime / halfJumpDuration);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            transform.position += new Vector3(0, input.y * speed * Time.deltaTime, 0);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Teine pool hüpist
        elapsedTime = 0f;
        while (elapsedTime < halfJumpDuration)
        {
            transform.localScale = Vector3.Lerp(targetScale, originalScale, elapsedTime / halfJumpDuration);
            float newY = Mathf.Lerp(targetY, startY + input.y, elapsedTime / halfJumpDuration);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            transform.position += new Vector3(0, input.y * speed * Time.deltaTime, 0);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = originalScale;
        isJumping = false;

        yield return new WaitForSeconds(jumpCooldown);
        canJump = true;
    }

    void PlayWalkSound()
    {
        if (walkSound != null)
        {
            StopAllSounds(); // Peatab kõik helid enne kõndimise heli mängimist
            audioSource.clip = walkSound;
            audioSource.loop = true;  // Mängib pidevalt
            audioSource.volume = 0.5f;
            audioSource.pitch = 2;
            audioSource.Play();
        }
    }

    void PlayDeathSound()
    {
        audioSource.pitch = 1;
        if (deathSound != null && !deathAudioSource.isPlaying)
        {
            deathAudioSource.PlayOneShot(deathSound);  // Mängib ainult üks kord surma heli
        }
    }

    void StopAllSounds()
    {
        audioSource.Stop();
    }

    void StopAllSoundsExceptDeath()
    {
        audioSource.Stop();
        deathAudioSource.Stop();  // Kui surma heli on juba mängimas, ei peata seda
    }
}
