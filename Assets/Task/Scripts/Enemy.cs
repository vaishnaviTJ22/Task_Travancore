using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animation m_Animation;

    [Header("Animation Clips")]
    [SerializeField] private AnimationClip idleClip;
    [SerializeField] private AnimationClip walkClip;
    [SerializeField] private AnimationClip attackClip;
    [SerializeField] private AnimationClip dieClip;

    [Header("Enemy Stats")]
    [SerializeField] protected int health = 3;
    [SerializeField] protected float speed = 2f;
    [SerializeField] protected int attackDamage = 10;
    [SerializeField] protected int scoreValue = 10;

    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 5f;

    private Treasure target;
    private bool hasReachedTarget = false;
    private bool isAttacking = false;
    private bool isDead = false;

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
        else if (hasReachedTarget)
        {
            RotateTowardsTarget();
            PlayIdleAnimation();
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
        if (!isDead)
        {
            TakeDamage(1);
        }
    }

    private void TakeDamage(int damage)
    {
        if (isDead) return;

        health -= damage;
        if (health <= 0)
        {
            Die();
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

        float destroyDelay = dieClip != null ? dieClip.length : 1f;
        Destroy(gameObject, destroyDelay);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Treasure") && !hasReachedTarget && !isDead)
        {
            hasReachedTarget = true;
            PerformAttack();
        }
    }

    private void PerformAttack()
    {
        if (isAttacking || isDead) return;

        isAttacking = true;

        if (m_Animation != null && attackClip != null)
        {
            m_Animation.Play(attackClip.name);
        }

        if (target != null)
        {
            target.TakeDamage(attackDamage);
        }

        float attackDuration = attackClip != null ? attackClip.length : 1f;
        Invoke(nameof(ResetAttack), attackDuration);
    }

    private void ResetAttack()
    {
        isAttacking = false;
    }
}
