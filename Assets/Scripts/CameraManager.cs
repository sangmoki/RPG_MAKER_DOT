using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // 카메라가 따라갈 대상
    public GameObject target;

    // 카메라의 이동 속도
    public float moveSpeed;

    // 대상의 현재 위치 값
    private Vector3 targetPosition;

    void Start()
    {
        
    }

    void Update()
    {
        // 카메라가 쫓아갈 대상이 null이 아닌 경우 실행
        if (target.gameObject != null)
        {
            // 타겟의 position을 가져와 targetPosition에 대입
            // 여기서 this는 카메라의 z값 <- 캐릭터보다 카메라가 뒤에 있어야 비출 수 있기 때문에 대입해준다.
            targetPosition.Set(target.transform.position.x, target.transform.position.y, this.transform.position.z);

            // 1초에 moveSpeed 만큼 이동
            this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }
}
