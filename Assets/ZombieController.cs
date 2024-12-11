using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask what_is_ground, what_is_player;

    private Animator animator;
    private ZombieGenerator zombieGenerator;

    // Sounds
    private AudioSource audioSource;
    public AudioClip[] voiceGrunts;
    public AudioClip slurpBlood;
    public AudioClip attackGrunt1;
    public AudioClip attackGrunt2;


    public Vector3 walk_point;
    bool walk_point_set;
    public float walk_point_range;

    // Attacking
    public float time_between_attacks;
    bool already_attacked;

    public float sight_range, attack_range;
    public bool player_in_sight_range, player_in_attack_range;

    // Setting
    int maxHP = 20;
    float levelUpTimer = 0f;
    int currentHP;
    int Damage = 1;
    float DamageCounter = 0;
    private bool isDead = false;

    private void Awake()
    {
        player = GameObject.Find("Player Character").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        currentHP = maxHP;
        time_between_attacks = 3.0f;
    }

    public void SetSpeed(float speed)
    {
        if (agent != null)
        {
            agent.speed = speed;
        }
    }

    public void SetZombieGenerator(ZombieGenerator generator)
    {
        zombieGenerator = generator;
    }

    private void Start()
    {
        animator.SetBool("isRunning", true);
        SetSpeed(2.5f);
    }

    private void SearchWalkPoint()
    {
        float random_z = Random.Range(-walk_point_range, walk_point_range);
        float random_x = Random.Range(-walk_point_range, walk_point_range);

        walk_point = new Vector3(transform.position.x + random_x, transform.position.y, transform.position.z + random_z);

        if (Physics.Raycast(walk_point, -transform.up, 2f, what_is_ground)) walk_point_set = true;
    }

    private void PlayRandomGruntLoop()
    {
        if (audioSource != null && voiceGrunts.Length >= 3)
        {
            if (!audioSource.isPlaying)
            {
                int randomIndex = Random.Range(0, 3);
                audioSource.clip = voiceGrunts[randomIndex];
                audioSource.loop = true;
                audioSource.Play();
            }
        }
    }

    private void StopSound()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
            audioSource.loop = false;
        }
    }

    private void PlayAttackSound()
    {
        if (audioSource != null && voiceGrunts.Length >= 5)
        {
            int randomIndex = Random.Range(3, 5);
            audioSource.clip = voiceGrunts[randomIndex];
            audioSource.loop = false;
            audioSource.Play();
        }
    }

    private void PlayDeathSound()
    {
        if (audioSource != null && slurpBlood != null)
        {
            audioSource.clip = slurpBlood;
            audioSource.loop = false;
            audioSource.Play();
        }
    }

    public void PlayRightHandAttackSound()
    {
        if (audioSource != null && attackGrunt1 != null)
        {
            audioSource.clip = attackGrunt1;
            audioSource.loop = false;
            audioSource.Play();
        }
    }

    public void PlayLeftHandAttackSound()
    {
        if (audioSource != null && attackGrunt2 != null)
        {
            audioSource.clip = attackGrunt2;
            audioSource.loop = false;
            audioSource.Play();
        }
    }

    private void Patroling()
    {
        if (!walk_point_set) SearchWalkPoint();
        if (walk_point_set) agent.SetDestination(walk_point);

        Vector3 distance_to_walk_point = transform.position - walk_point;
        if (distance_to_walk_point.magnitude < 1f) walk_point_set = false;

        animator.SetBool("isRunning", false);
        StopSound();
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);

        animator.SetBool("isRunning", true);
        animator.SetBool("isAttacking", false);

        PlayRandomGruntLoop();
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        animator.SetBool("isRunning", false);
        animator.SetBool("isAttacking", true);

        StopSound();
        PlayAttackSound();

        if (!already_attacked)
        {
            already_attacked = true;

            GameManager.Instance.Player.hp -= Damage;

            if (GameManager.Instance.Player.hp <= 0)
            {
                Debug.Log("game over");
            }

            Invoke(nameof(ResetAttack), time_between_attacks);
        }
    }

    private void ResetAttack()
    {
        already_attacked = false;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHP -= damage;
        Debug.Log($"Zombie HP: {currentHP}");

        animator.ResetTrigger("isRunning");
        animator.ResetTrigger("isAttacking");

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

        animator.ResetTrigger("Attacked");
        animator.ResetTrigger("isRunning");
        animator.ResetTrigger("isAttacking");

        StopSound();
        PlayDeathSound();

        if (Random.value > 0.5f)
        {
            animator.SetTrigger("FallingBack");
        }
        else
        {
            animator.SetTrigger("FallingFront");
        }

        StartCoroutine(WaitForDeathAnimation());
    }

    private IEnumerator WaitForDeathAnimation()
    {
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        float animationLength = clipInfo.Length > 0 ? clipInfo[0].clip.length : 1f;

        yield return new WaitForSeconds(animationLength);

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Attack"))
        {
            TakeDamage(GameManager.Instance.Player.atk);
        }
    }

    void Update()
    {
        if (isDead) return;

        // Distance calcuation
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Push zombies
        if (distanceToPlayer < 0.5f)
        {
            Vector3 directionAwayFromPlayer = (transform.position - player.position).normalized;
            transform.position += directionAwayFromPlayer * 0.3f; // push coefficient 
        }

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

        levelUpTimer += Time.deltaTime;
        if (levelUpTimer > 60.0f)
        {
            maxHP += 20;
        }

        DamageCounter = Time.deltaTime;
        if (DamageCounter > 60.0f)
        {
            Damage += 1;
        }
    }
}