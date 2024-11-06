using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    // 박스콜라이더 객체 생성 - 충돌범위 지정 위함
    private BoxCollider2D boxCollider;

    // 레이어 마스크를 사용하여 어떤 레이어와 충돌할지 결정
    // 즉, 통과가 불가능한 레이어 설정
    public LayerMask layerMask;

    // 캐릭터의 이동 속도
    public float speed;

    // 캐릭터의 이동 방향
    private Vector3 vector;

    // 캐릭터의 달리기 속도
    public float runSpeed;

    // shift 키를 누를 때에만 적용할 달리기 속도
    private float applyRunSpeed;

    // shift 키를 눌렀을 때와 누르지 않았을 때 구분을 위한 플래그
    private bool applyRunFlag = false;

    // 한번 방향키가 눌릴때마다 이동하는 거리
    public int walkCount;
    private int currentWalkCount;

    // 코루틴 동시 입력을 방지하기 위해 사용할 플래그
    private bool canMove = true;

    // 애니메이션 객체 생성
    private Animator animator;

    void Start()
    {
        // BoxCollider2D 컴포넌트를 가져와 boxCollider 변수에 할당
        boxCollider = GetComponent<BoxCollider2D>();

        // Animator 컴포넌트를 가져와 animator 변수에 할당
        animator = GetComponent<Animator>();
    }

    // 원하는 만큼 움직일 수 있게 대기 시간을 주는 코루틴
    IEnumerator MoveCoroutine()
    {
        // 코루틴은 한번만 실행되고 내부에서 실행된다.
        while (Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0)
        {
            // shift 키를 누르면 달리기 속도 적용
            if (Input.GetKey(KeyCode.LeftShift))
            {
                applyRunSpeed = runSpeed;
                applyRunFlag = true;
            }
            else
            {
                applyRunSpeed = 0;
                applyRunFlag = false;
            }

            // z값은 바뀌지 않기 때문에 z로만 해도 문제 없다
            vector.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), transform.position.z);

            // vector x의 값이 0이 아니면 y값을 0으로 설정
            if (vector.x != 0)
                vector.y = 0;

            // DirX의 파라미터 값을 vector.x, vector.y로 설정
            animator.SetFloat("DirX", vector.x);
            animator.SetFloat("DirY", vector.y);

            // Raycast : 레이저를 쏘아 맞은 물체의 정보를 가져옴
            // Raycast(시작점, 방향, 거리, 레이어마스크)
            RaycastHit2D hit;
            
            // A지점, 캐릭터의 현재 위치
            Vector2 start = transform.position;  
            // B지점, 캐릭터가 이동할 위치
            Vector2 end = start + new Vector2(vector.x * speed * walkCount, vector.y * speed * walkCount);

            // A지점, B지점
            // hit = Null <- A지점에 도달하였을 때
            // hit = 방해물 <- 방해물에 부딪혔을 때 방해물의 위치 리턴
            boxCollider.enabled = false; // 이동할 때 플레이어의 boxColider를 꺼주어야 자기 자신한테 충돌 발생 x
            hit = Physics2D.Linecast(start, end, layerMask);
            boxCollider.enabled = true; // 다시 켜준다

            // 방해물에 부딪혔을 때 걷는 모션을 제거
            if (hit.transform != null)
                break;

            // Stading Tree -> Walking Tree로 상태 변경
            animator.SetBool("Walking", true);

            while (currentWalkCount < walkCount)
            {
                // 좌우 방향키 입력 시 상하 이동을 막기 위해 상하 이동값을 0으로 설정
                if (vector.x != 0)
                {
                    // Translate : 현재 위치에서 인자로 받은 값만큼 이동
                    transform.Translate(vector.x * (speed + applyRunSpeed), 0, 0);
                }
                // 상하 방향키 입력 시 좌우 이동을 막기 위해 좌우 이동값을 0으로 설정
                else if (vector.y != 0)
                {
                    transform.Translate(0, vector.y * (speed + applyRunSpeed), 0);
                }

                // Shift 눌림 버튼이 활성화 되면 달리기 적용
                if (applyRunFlag)
                    currentWalkCount++;
          
                currentWalkCount++;
                yield return new WaitForSeconds(0.01f);
            }
            currentWalkCount = 0;
        }

        // Walking Tree -> Stading Tree로 상태 변경
        animator.SetBool("Walking", false);
        canMove = true;
    }

    void Update()
    {
        if (canMove)
        {
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                canMove = false;
                StartCoroutine(MoveCoroutine());
            }
        }

    }
}