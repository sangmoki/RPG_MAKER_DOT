using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    // ĳ������ �̵� �ӵ�
    public float speed;

    // ĳ������ �̵� ����
    private Vector3 vector;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            // z���� �ٲ��� �ʱ� ������ z�θ� �ص� ���� ����
            vector.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), transform.position.z);

            // �¿� ����Ű �Է� �� ���� �̵��� ���� ���� ���� �̵����� 0���� ����
            if (vector.x != 0)
            {
                // Translate : ���� ��ġ���� ���ڷ� ���� ����ŭ �̵�
                transform.Translate(vector.x * speed * Time.deltaTime, 0, 0);
            }
            // ���� ����Ű �Է� �� �¿� �̵��� ���� ���� �¿� �̵����� 0���� ����
            else if (vector.y != 0)
            {
                transform.Translate(0, vector.y * speed * Time.deltaTime, 0);
            }
        }
    }
}
