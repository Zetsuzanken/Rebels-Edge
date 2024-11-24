using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesHealth : MonoBehaviour
{
    public float health;

    private bool isDead;
    private float currentHealth;
    private Animator anim;

    void Start()
    {
        isDead = false;
        currentHealth = health;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        HandleInput();
    }
    
    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, health);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;

        anim.SetBool("isDead", isDead);

        StartCoroutine(DestroyAfterDelay(1.5f));
    }

    public IEnumerator DestroyAfterDelay(float delay)
    {
        Debug.Log("DEAD");
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    public void InstantDeath()
    {
        if (isDead)
            return;

        currentHealth = 0f;
        Die();
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            TakeDamage(10f);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            InstantDeath();
        }
    }
}