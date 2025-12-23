using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animation m_Animation;

    [Header("Animation Clips")]
    [SerializeField] private AnimationClip idleClip;
    [SerializeField] private AnimationClip walkClip;
    [SerializeField] private AnimationClip attackClip;
    [SerializeField] private AnimationClip hitClip;
    [SerializeField] private AnimationClip dieClip;

    [Header("Enemy Stats")]
    [SerializeField] protected int maxHealth = 3;
    [SerializeField] protected float speed = 2f;
    [SerializeField] protected int attackDamage = 10;
    [SerializeField] protected int scoreValue = 10;

    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 5f;

    [Header("Attack Settings")]
    private float attackCooldown = 2f;

    [Header("Enemy Type")]
    [SerializeField] private bool isArmored = false;

    [Header("Hit Feedback")]
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private AudioClip hitSoundEffect;
    [SerializeField] private float knockbackForce = 0.5f;

    protected int health;
    private Treasure target;
    private bool hasReachedTarget = false;
    private bool isAttacking = false;
    private bool isDead = false;
    private Color originalColor;
    private Material enemyMaterial;

    private void OnEnable()
    {
        ResetEnemy();
    }

    private void Start()
    {
        target = FindObjectOfType<Treasure>();

       

        if (m_Animation != null && idleClip != null)
        {
            m_Animation.Play(idleClip.name);
        }
    }

    private void Update()
    {
        if (GameManager.Instance.isGameOver || isDead) return;

        if (!hasReachedTarget && !isAttacking)
        {
            MoveTowardsTarget();
            RotateTowardsTarget();
            PlayWalkAnimation();
        }
    }

    private void MoveTowardsTarget()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target.transform.position,
                speed * Time.deltaTime
            );
        }
    }

    private void RotateTowardsTarget()
    {
        if (target != null)
        {
            Vector3 direction = target.transform.position - transform.position;
            direction.y = 0;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );
            }
        }
    }

    private void PlayWalkAnimation()
    {
        if (m_Animation != null && walkClip != null && !m_Animation.IsPlaying(walkClip.name))
        {
            m_Animation.CrossFade(walkClip.name);
        }
    }

    private void PlayIdleAnimation()
    {
        if (m_Animation != null && idleClip != null && !m_Animation.IsPlaying(idleClip.name))
        {
            m_Animation.CrossFade(idleClip.name);
        }
    }

    private void OnMouseDown()
    {
        if (!isDead && !GameManager.Instance.isGameOver)
        {
            TakeDamage(1);
        }
    }

    private void TakeDamage(int damage)
    {
        if (isDead) return;

        health -= damage;

        ShowHitFeedback();

        if (health <= 0)
        {
            Die();
        }
    }

    private void ShowHitFeedback()
    {
        PlayHitAnimation();
        SpawnHitEffect();
        ApplyKnockback();
        SpawnSoundEffect();
    }

    private void PlayHitAnimation()
    {
        if (m_Animation != null && hitClip != null)
        {
            m_Animation.Play(hitClip.name);
        }
    }

    private void SpawnHitEffect()
    {
        if (hitEffectPrefab != null)
        {
            Vector3 hitPosition = transform.position + Vector3.up * 1f;
            GameObject effect = Instantiate(hitEffectPrefab, hitPosition, Quaternion.identity);
            Destroy(effect, 2f);
        }
    }

    private void SpawnSoundEffect()
    {
        AudioSource.PlayClipAtPoint(hitSoundEffect,transform.position,1f);
    }
   

    private void ApplyKnockback()
    {
        Vector3 knockbackDirection = (transform.position - Camera.main.transform.position).normalized;
        knockbackDirection.y = 0;

        Vector3 knockbackTarget = transform.position + knockbackDirection * knockbackForce;
        StartCoroutine(KnockbackCoroutine(knockbackTarget));
    }

    private IEnumerator KnockbackCoroutine(Vector3 targetPosition)
    {
        float elapsed = 0f;
        float duration = 0.15f;
        Vector3 startPosition = transform.position;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;

        GameManager.Instance.AddScore(scoreValue);

        if (m_Animation != null && dieClip != null)
        {
            m_Animation.Play(dieClip.name);
        }

        CancelInvoke();

        float returnDelay = dieClip != null ? dieClip.length : 1f;
        Invoke(nameof(ReturnToPool), returnDelay);
    }

    private void ReturnToPool()
    {
        EnemyPoolManager.Instance.ReturnEnemy(gameObject, isArmored);
    }

    private void ResetEnemy()
    {
        health = maxHealth;
        isDead = false;
        hasReachedTarget = false;
        isAttacking = false;

        if (target == null)
        {
            target = FindObjectOfType<Treasure>();
        }

        CancelInvoke();
        StopAllCoroutines();

        if (m_Animation != null && idleClip != null)
        {
            m_Animation.Play(idleClip.name);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Treasure") && !hasReachedTarget && !isDead)
        {
            hasReachedTarget = true;
            InvokeRepeating(nameof(PerformAttack), 0f, attackCooldown);
        }
    }

    private void PerformAttack()
    {
        if (isAttacking || isDead) return;

        isAttacking = true;

        if (m_Animation != null && attackClip != null)
        {
            transform.LookAt(target.transform);
            Debug.Log("Attack animation play");
            m_Animation.Play(attackClip.name);
        }

        float attackDuration = attackClip != null ? attackClip.length : 1f;
        Invoke(nameof(ResetAttack), attackDuration);
    }

    public void TreasureDamage()
    {
        if (target != null)
        {
            target.TakeDamage(attackDamage);
        }
    }

    private void ResetAttack()
    {
        isAttacking = false;
    }

    public void SetEnemyType(bool armored)
    {
        isArmored = armored;
    }
}
