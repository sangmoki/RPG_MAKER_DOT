using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    // �ڽ��ݶ��̴� ��ü ���� - �浹���� ���� ����
    private BoxCollider2D boxCollider;

    // ���̾� ����ũ�� ����Ͽ� � ���̾�� �浹���� ����
    // ��, ����� �Ұ����� ���̾� ����
    public LayerMask layerMask;

    // ĳ������ �̵� �ӵ�
    public float speed;

    // ĳ������ �̵� ����
    private Vector3 vector;

    // ĳ������ �޸��� �ӵ�
    public float runSpeed;

    // shift Ű�� ���� ������ ������ �޸��� �ӵ�
    private float applyRunSpeed;

    // shift Ű�� ������ ���� ������ �ʾ��� �� ������ ���� �÷���
    private bool applyRunFlag = false;

    // �ѹ� ����Ű�� ���������� �̵��ϴ� �Ÿ�
    public int walkCount;
    private int currentWalkCount;

    // �ڷ�ƾ ���� �Է��� �����ϱ� ���� ����� �÷���
    private bool canMove = true;

    // �ִϸ��̼� ��ü ����
    private Animator animator;

    void Start()
    {
        // BoxCollider2D ������Ʈ�� ������ boxCollider ������ �Ҵ�
        boxCollider = GetComponent<BoxCollider2D>();

        // Animator ������Ʈ�� ������ animator ������ �Ҵ�
        animator = GetComponent<Animator>();
    }

    // ���ϴ� ��ŭ ������ �� �ְ� ��� �ð��� �ִ� �ڷ�ƾ
    IEnumerator MoveCoroutine()
    {
        // �ڷ�ƾ�� �ѹ��� ����ǰ� ���ο��� ����ȴ�.
        while (Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0)
        {
            // shift Ű�� ������ �޸��� �ӵ� ����
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

            // z���� �ٲ��� �ʱ� ������ z�θ� �ص� ���� ����
            vector.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), transform.position.z);

            // vector x�� ���� 0�� �ƴϸ� y���� 0���� ����
            if (vector.x != 0)
                vector.y = 0;

            // DirX�� �Ķ���� ���� vector.x, vector.y�� ����
            animator.SetFloat("DirX", vector.x);
            animator.SetFloat("DirY", vector.y);

            // Raycast : �������� ��� ���� ��ü�� ������ ������
            // Raycast(������, ����, �Ÿ�, ���̾��ũ)
            RaycastHit2D hit;
            
            // A����, ĳ������ ���� ��ġ
            Vector2 start = transform.position;  
            // B����, ĳ���Ͱ� �̵��� ��ġ
            Vector2 end = start + new Vector2(vector.x * speed * walkCount, vector.y * speed * walkCount);

            // A����, B����
            // hit = Null <- A������ �����Ͽ��� ��
            // hit = ���ع� <- ���ع��� �ε����� �� ���ع��� ��ġ ����
            boxCollider.enabled = false; // �̵��� �� �÷��̾��� boxColider�� ���־�� �ڱ� �ڽ����� �浹 �߻� x
            hit = Physics2D.Linecast(start, end, layerMask);
            boxCollider.enabled = true; // �ٽ� ���ش�

            // ���ع��� �ε����� �� �ȴ� ����� ����
            if (hit.transform != null)
                break;

            // Stading Tree -> Walking Tree�� ���� ����
            animator.SetBool("Walking", true);

            while (currentWalkCount < walkCount)
            {
                // �¿� ����Ű �Է� �� ���� �̵��� ���� ���� ���� �̵����� 0���� ����
                if (vector.x != 0)
                {
                    // Translate : ���� ��ġ���� ���ڷ� ���� ����ŭ �̵�
                    transform.Translate(vector.x * (speed + applyRunSpeed), 0, 0);
                }
                // ���� ����Ű �Է� �� �¿� �̵��� ���� ���� �¿� �̵����� 0���� ����
                else if (vector.y != 0)
                {
                    transform.Translate(0, vector.y * (speed + applyRunSpeed), 0);
                }

                // Shift ���� ��ư�� Ȱ��ȭ �Ǹ� �޸��� ����
                if (applyRunFlag)
                    currentWalkCount++;
          
                currentWalkCount++;
                yield return new WaitForSeconds(0.01f);
            }
            currentWalkCount = 0;
        }

        // Walking Tree -> Stading Tree�� ���� ����
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