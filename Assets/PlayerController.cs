using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{


    public Animator animator;
    public Camera camera;
    public float moveSpeed = 3f;
    public LayerMask groundLayerMask;
    public AudioSource audioSource; // 오디오 소스 컴포넌트
    public AudioClip[] attackClips; // 공격 사운드 클립 배열 (Attack1, Attack2, Attack3 순서)
    public AudioClip[] roarClips; // 공격 사운드 클립 배열 (Attack1, Attack2, Attack3 순서)
    public AudioClip footstepClip; // 발걸음 소리 클립
    public AudioClip skill1Clip; // skill1 효과음 클립

    public Collider attackCollider;

    public Collider skill1Collider;

    public ParticleSystem[] attackParticles;
    public ParticleSystem skill1Particle; // skill1 파티클 시스템

    private Rigidbody rigidbody;
    private Vector3 inputDirection;

    private int comboStep = 0; // 현재 공격 단계 (0 = Idle, 1 = Attack1, 2 = Attack2, 3 = Attack3)
    private bool canContinueCombo = false; // 다음 공격으로 넘어갈 수 있는지 여부
    private bool isAttacking = false; // 공격 중인지 여부
    private float comboTimer = 0f; // 콤보 입력 타이머
    public float comboTimeout = 1f; // 콤보 입력 유효 시간
    private bool isMoving = false; // 이동 중인지 여부

    private GameManager gameManager;
    public static PlayerController Instance { get; private set; }

    private void Awake()
    {
        gameManager = GameManager.Instance;

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        rigidbody = GetComponent<Rigidbody>();

        // AudioSource가 설정되지 않은 경우 자동으로 추가
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void FixedUpdate()
    {
        if (!isAttacking)
        {
            Move();
        }
        else
        {
            rigidbody.velocity = Vector3.zero; // 공격 중 이동 정지
        }

        if (comboStep > 0 && comboTimer > 0)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer <= 0)
            {
                ResetCombo();
            }
        }
    }

    private void Move()
    {
        if (isAttacking) return;

        rigidbody.velocity = inputDirection * moveSpeed + Vector3.up * rigidbody.velocity.y;
        LookAt(inputDirection);

        if (inputDirection != Vector3.zero && !isMoving)
        {
            isMoving = true;
            PlayFootstepSound();
        }
        else if (inputDirection == Vector3.zero)
        {
            isMoving = false;
            audioSource.Stop(); // 이동 멈춤 시 발소리 정지
        }
    }

    private void LookAt(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Quaternion targetAngle = Quaternion.LookRotation(direction);
            transform.rotation = targetAngle;
        }
    }

    public void OnClickMouseLeft(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            if (!isAttacking)
            {
                StartAttack();
            }
            else if (canContinueCombo)
            {
                NextComboStep();
            }
        }
    }

    private void StartAttack()
    {
        if (isAttacking) return;

        LookAt(GetWorldPos());
        comboStep = 1;
        isAttacking = true;
        canContinueCombo = false;

        PlayAttackAnimation("Attack1");
    }

    private void NextComboStep()
    {
        if (comboStep < 3)
        {
            comboStep++;
            canContinueCombo = false;
            string nextAttack = $"Attack{comboStep}";
            PlayAttackAnimation(nextAttack);
        }
    }

    private void PlayAttackAnimation(string attackName)
    {
        animator.ResetTrigger("Attack1");
        animator.ResetTrigger("Attack2");
        animator.ResetTrigger("Attack3");

        animator.SetTrigger(attackName);
        comboTimer = comboTimeout;

    }


    private void EnableAttackCollider()
    {
        attackCollider.enabled = true;
        attackParticles[comboStep-1].Play();
    }

    private void ResetCombo()
    {
        comboStep = 0;
        isAttacking = false;
        canContinueCombo = false;

        animator.ResetTrigger("Attack1");
        animator.ResetTrigger("Attack2");
        animator.ResetTrigger("Attack3");
        animator.SetTrigger("Idle");
    }

    // 애니메이션 이벤트에서 호출
    public void AllowCombo()
    {
        canContinueCombo = true;
    }

    // 애니메이션 이벤트에서 호출
    public void EndAttack()
    {
        attackCollider.enabled = false;
        if (!canContinueCombo)
        {
            ResetCombo();
        }
    }

    // 애니메이션 이벤트에서 호출
    public void EndSkill()
    {
        isAttacking = false;
        animator.ResetTrigger("Skill1");
        animator.ResetTrigger("Skill2");

        animator.SetTrigger("Idle");
        animator.ResetTrigger("Idle");

        skill1Collider.enabled = false;

    }

    /*
    // 애니메이션 이벤트에서 호출
    public void PlayAttackSound()
    {
        if (comboStep > 0 && comboStep <= attackClips.Length)
        {
            audioSource.PlayOneShot(attackClips[comboStep - 1]);
        }
    }
    */

    public void PlayAttackSound()
    {
        // 현재 재생 중인 애니메이션 상태를 확인
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Attack1, Attack2, Attack3 상태에 따라 적절한 효과음 재생
        if (stateInfo.IsName("Attack1") && attackClips.Length > 0)
        {
            audioSource.PlayOneShot(attackClips[0]);
        }
        else if (stateInfo.IsName("Attack2") && attackClips.Length > 1)
        {
            audioSource.PlayOneShot(attackClips[1]);
        }
        
        else if (stateInfo.IsName("Attack3") && attackClips.Length > 2)
        {
            audioSource.PlayOneShot(attackClips[2]);
        }
    }

    public void PlayRoarSound()
    {
        // 현재 재생 중인 애니메이션 상태를 확인
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Attack1, Attack2, Attack3 상태에 따라 적절한 효과음 재생
        if (comboStep == 1)
        {
            audioSource.PlayOneShot(roarClips[0]);
        }
        else if (comboStep == 2)
        {
            audioSource.PlayOneShot(roarClips[1]);
            Debug.Log("Roar2");
        }
        else if (comboStep == 3)
        {
            audioSource.PlayOneShot(roarClips[2]);
            Debug.Log("Roar3");
        }
    }

    // 특수 스킬 1 실행
    public void OnSkill1Input(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            ActivateSkill1();
        }
    }

    private void ActivateSkill1()
    {
        if (isAttacking) return;

        isAttacking = true;
        skill1Collider.enabled = true;

        animator.SetTrigger("Skill1"); // skill1 애니메이션 실행
    }

    // skill1 효과음 재생
    public void PlaySkill1Sound()
    {
        Debug.Log("PlaySkill1Sound called");
        if (skill1Clip != null)
        {
            audioSource.PlayOneShot(skill1Clip);
        }
        else
        {
            Debug.LogWarning("Skill1 sound clip is not assigned.");
        }
    }

    public void PlaySkill1Particle()
    {

        if (skill1Particle != null && !skill1Particle.isPlaying)
        {
            skill1Particle.Play();
            Debug.Log("PlaySkill1Particle called");
        }
        else
        {
            Debug.LogWarning("Skill1 particle system is not assigned or already playing.");
        }
    }

    // 특수 스킬 1 실행
    public void OnSkill2Input(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            ActivateSkill2();
        }
    }

    private void ActivateSkill2()
    {
        animator.SetTrigger("Skill2"); // skill1 애니메이션 실행
    }

    public void OnSkill3Input(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            ActivateSkill3();
        }
    }

    private void ActivateSkill3()
    {
        animator.SetTrigger("Skill3"); // skill1 애니메이션 실행
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        Vector2 curMoveInput = context.ReadValue<Vector2>();
        inputDirection = new Vector3(curMoveInput.x, 0f, curMoveInput.y);

        animator.SetTrigger("Move");

        if (context.canceled)
        {
            animator.ResetTrigger("Move");
            animator.SetTrigger("Idle");
        }
    }

    private void PlayFootstepSound()
    {
        if (footstepClip != null)
        {
            audioSource.clip = footstepClip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    private Vector3 GetWorldPos()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, groundLayerMask))
        {
            Vector3 target = hitInfo.point;
            target.y = 0f;
            return (target - transform.position).normalized;
        }

        return Vector3.zero;
    }
}
