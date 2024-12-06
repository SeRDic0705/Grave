using UnityEngine;
using System.Collections;

public class ZombieController : MonoBehaviour
{
    public Transform player;
    public LayerMask groundLayer; 
    public LayerMask playerLayer; 

    public float patrolSpeed = 2f; // patrol �ӵ�
    public float chaseSpeed = 4f;  // chasing �ӵ�

    public float attackRange = 1.5f; // attack range
    public float sightRange = 10f; // �þ� range

    public float patrolPointRange = 5f; // patrol ����
    public float attackCooldown = 1f; // ���� cooltime

    private Vector3 patrolPoint; // patrol point
    private bool patrolPointSet = false; 

    // for animation
    private bool isAttacking = false;

    private Rigidbody rb;
    private Animator animator;

    private void Awake()
    {
        player = GameObject.Find("Player Character").transform;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

    }

    private void Start()
    {
        // patrol pouint set
        SetRandomPatrolPoint();
    }

    private void Update()
    {
        // Player find range / attack range
        bool playerInSight = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        bool playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if (!playerInSight && !playerInAttackRange)
            Patrol();
        else if (playerInSight && !playerInAttackRange)
            ChasePlayer();
        else if (playerInAttackRange)
            AttackPlayer();
    }


    // patroling
    private void Patrol()
    {
        if (!patrolPointSet)
        {
            // set patrol ponit
            SetRandomPatrolPoint(); 
        }

        // move to patrol point
        Vector3 direction = (patrolPoint - transform.position).normalized;
        Move(direction, patrolSpeed);

        // patrol point check
        if (Vector3.Distance(transform.position, patrolPoint) < 1f)
        {
            patrolPointSet = false; // ���� ���� ���� �� �ٽ� ���� ���� ����
        }

        // Transition
        animator.SetFloat("Speed", patrolSpeed); 
        animator.SetBool("IsAttacking", false); 
    }

    
    private void SetRandomPatrolPoint()
    {
        // Random�ϰ�
        float randomX = Random.Range(-patrolPointRange, patrolPointRange);
        float randomZ = Random.Range(-patrolPointRange, patrolPointRange);
        Vector3 randomPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        // ���鿡 ��� ���� patrol point set
        if (Physics.Raycast(randomPoint, Vector3.down, out RaycastHit hit, 2f, groundLayer))
        {
            patrolPoint = hit.point; 
            patrolPointSet = true;    
        }
    }

    private void Move(Vector3 direction, float speed)
    {
        // ���� �浹 Ȯ��
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 2f, groundLayer))
        {
            // ��� �� �̵�
            Vector3 targetPosition = transform.position + direction * speed * Time.deltaTime;
            rb.MovePosition(targetPosition); 

            // �̵� ���� ȸ��
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }
        }
    }

    // chasing
    private void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Move(direction, chaseSpeed);

        // Transition
        animator.SetFloat("Speed", chaseSpeed); 
        animator.SetBool("IsAttacking", false); 
    }


    // attacking
    private void AttackPlayer()
    {
        if (isAttacking) return;

        isAttacking = true;
        StartCoroutine(PerformAttack());
    }

    private IEnumerator PerformAttack()
    {
        // Transition
        animator.SetBool("IsAttacking", true); 
        animator.SetFloat("Speed", 0f); 
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
        animator.SetBool("IsAttacking", false);
    }


    private void OnDrawGizmosSelected()
    {
        // for Testing
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}