using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target; // 따라갈 대상 (캐릭터)
    public Vector3 offset = new Vector3(0, 10, -10); // 캐릭터와의 고정 거리

    private Vector3 fixedRotation; // 고정된 카메라 방향

    void Start()
    {
        // 초기 카메라의 회전값을 저장
        fixedRotation = transform.eulerAngles;
    }

    void LateUpdate()
    {
        if (target != null)
        {
            // 캐릭터의 위치에 offset만큼 이동
            transform.position = target.position + offset;

            // 카메라의 방향은 초기값으로 고정
            transform.rotation = Quaternion.Euler(fixedRotation);
        }
    }
}
