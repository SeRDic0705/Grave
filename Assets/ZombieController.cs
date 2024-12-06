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

    // Patroling
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

    private void Awake()
    {
        player = GameObject.Find("Player Character").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        currentHP = maxHP;
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
                Debug.Log("Player got damaged");
            }
            Invoke(nameof(ResetAttack), time_between_attacks);
        }
    }

    private void Die()
    {

        agent.enabled = false;
        Destroy(gameObject, 0.5f);


        FindObjectOfType<ZombieGenerator>().ZombieDied();
    }

    void Update()
    {

        player_in_sight_range = Physics.CheckSphere(transform.position, sight_range, what_is_player);
        player_in_attack_range = Physics.CheckSphere(transform.position, attack_range, what_is_player);

        if (!player_in_sight_range && !player_in_attack_range)
        {
            Patroling();
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