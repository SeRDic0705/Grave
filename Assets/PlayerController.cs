using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public Camera camera;
    public float moveSpeed = 3f;
    public LayerMask groundLayerMask;

    private Rigidbody rigidbody;
    private Vector3 inputDirection;

    private int comboStep = 0; // 현재 공격 단계 (0 = Idle, 1 = Attack1, 2 = Attack2, 3 = Attack3)
    private bool canContinueCombo = false; // 다음 공격으로 넘어갈 수 있는지 여부
    private bool isAttacking = false; // 공격 중인지 여부
    private float comboTimer = 0f; // 콤보 입력 타이머
    public float comboTimeout = 1f; // 콤보 입력 유효 시간

    public static PlayerController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        rigidbody = GetComponent<Rigidbody>();
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
        if (isAttacking) 
        {
            //Debug.Log("Attack in progress, movement stopped.");
            return;
        }

        if (inputDirection == Vector3.zero)
        {
            //Debug.Log("No input direction, character remains idle.");
        }

        // 이동 속도 계산
        rigidbody.velocity = inputDirection * moveSpeed + Vector3.up * rigidbody.velocity.y;
        LookAt(inputDirection);
        float speed = rigidbody.velocity.magnitude;



        //Debug.Log($"Velocity: {rigidbody.velocity}, Speed: {speed}");
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
                Debug.Log("NextComboStep Called");
                NextComboStep();
            }
        }
    }

    private void StartAttack()
    {
        if (isAttacking)
        {
            Debug.Log("StartAttack returned");
            return; // 이미 공격 중이면 무시
        }
        Debug.Log("StartAttack Called");
        LookAt(GetWorldPos());
        comboStep = 1; // 첫 번째 공격
        isAttacking = true;
        canContinueCombo = false;

        PlayAttackAnimation("Attack1");
    }

    private void NextComboStep()
    {
        if (comboStep < 3) // 최대 3단 공격
        {
            comboStep++;
            canContinueCombo = false; // 입력 대기 상태 종료
            string nextAttack = $"Attack{comboStep}";
            PlayAttackAnimation(nextAttack);
        }
    }

    private void PlayAttackAnimation(string attackName)
    {
        Debug.Log("PlayAttackAnimation Called");
        // 모든 공격 트리거 초기화
        animator.ResetTrigger("Attack1");
        animator.ResetTrigger("Attack2");
        animator.ResetTrigger("Attack3");

        // 현재 공격 트리거 활성화
        animator.SetTrigger(attackName);

        comboTimer = comboTimeout; // 콤보 입력 시간 리셋
    }

    private void ResetCombo()
    {
        comboStep = 0;
        isAttacking = false;
        canContinueCombo = false;

        animator.ResetTrigger("Attack1");
        animator.ResetTrigger("Attack2");
        animator.ResetTrigger("Attack3");

        animator.SetTrigger("Idle"); // Idle 상태로 복귀
    }

    // 애니메이션 이벤트에서 호출
    public void AllowCombo()
    {
        Debug.Log("Allowcombo called");
        canContinueCombo = true; // 다음 공격을 받을 수 있는 상태로 전환
    }

    // 애니메이션 이벤트에서 호출
    public void EndAttack()
    {
        if (!canContinueCombo) // 다음 입력이 없으면 공격 종료
        {
            Debug.Log("EndAttack called");
            ResetCombo();
        }
    }




    public void OnMoveInput(InputAction.CallbackContext context)
    {
        Vector2 curMoveInput = context.ReadValue<Vector2>();
        inputDirection = new Vector3(curMoveInput.x, 0f, curMoveInput.y);

        //Debug.Log($"Move Input: {curMoveInput}, Input Direction: {inputDirection}");
        animator.SetTrigger("Move");

        if (context.canceled)
        {
            animator.ResetTrigger("Move");
            animator.SetTrigger("Idle");
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
