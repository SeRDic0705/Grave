using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask what_is_ground, what_is_player;

    private Animator animator;

    // Patroling (??)
    public Vector3 walk_point;
    bool walk_point_set;
    public float walk_point_range;

    // Attacking
    public float time_between_attacks;
    bool already_attacked;

    public float sight_range, attack_range;
    public bool player_in_sight_range, player_in_attack_range;

    int maxHP = 100;
    int currentHP;
    private bool isDead = false;

    private void Awake()
    {
        player = GameObject.Find("Player Character").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        currentHP = maxHP;
    }

    private void Start()
    {   // IDLE 제거 (추후 토의)
        animator.SetBool("isRunning", true);
    }

    private void SearchWalkPoint()
    {
        float random_z = Random.Range(-walk_point_range, walk_point_range);
        float random_x = Random.Range(-walk_point_range, walk_point_range);

        walk_point = new Vector3(transform.position.x + random_x, transform.position.y, transform.position.z + random_z);

        if (Physics.Raycast(walk_point, -transform.up, 2f, what_is_ground)) walk_point_set = true;
    }

    private void Patroling()
    {
        if (!walk_point_set) SearchWalkPoint();
        if (walk_point_set) agent.SetDestination(walk_point);

        Vector3 distance_to_walk_point = transform.position - walk_point;
        if (distance_to_walk_point.magnitude < 1f) walk_point_set = false;

        animator.SetBool("isRunning", false);
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);

        animator.SetBool("isRunning", true);
        animator.SetBool("isAttacking", false);
    }

    private void ResetAttack()
    {
        already_attacked = false;
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        animator.SetBool("isRunning", false);
        animator.SetBool("isAttacking", true);

        if (!already_attacked)
        {
            already_attacked = true;
            HealthManager playerHealth = player.GetComponent<HealthManager>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);
                Debug.Log("Player Got Damaged!"); // Testing용
            }
            Invoke(nameof(ResetAttack), time_between_attacks);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHP -= damage;
        Debug.Log($"Zombie HP: {currentHP}"); // Testing용

        animator.SetTrigger("Attacked");

        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;
        agent.enabled = false;

        // Random
        if (Random.value > 0.5f)
        {
            animator.SetTrigger("FallingBack");
        }
        else
        {
            animator.SetTrigger("FallingFront");
        }

        // 코루틴
        StartCoroutine(WaitForDeathAnimation());
    }

    private IEnumerator WaitForDeathAnimation()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Wait
        yield return new WaitForSeconds(stateInfo.length);
        Destroy(gameObject);
        FindObjectOfType<ZombieGenerator>()?.ZombieDied();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("ok");
        }
    }

    void Update()
    {
        if (isDead) return;

        player_in_sight_range = Physics.CheckSphere(transform.position, sight_range, what_is_player);
        player_in_attack_range = Physics.CheckSphere(transform.position, attack_range, what_is_player);

        if (!player_in_sight_range && !player_in_attack_range)
        {
            Patroling(); // ???
        }
        else if (player_in_sight_range && !player_in_attack_range)
        {
            ChasePlayer();
        }
        else if (player_in_sight_range && player_in_attack_range)
        {
            AttackPlayer();
        }
    }
}