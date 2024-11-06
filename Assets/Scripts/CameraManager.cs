using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // ī�޶� ���� ���
    public GameObject target;

    // ī�޶��� �̵� �ӵ�
    public float moveSpeed;

    // ����� ���� ��ġ ��
    private Vector3 targetPosition;

    void Start()
    {
        
    }

    void Update()
    {
        // ī�޶� �Ѿư� ����� null�� �ƴ� ��� ����
        if (target.gameObject != null)
        {
            // Ÿ���� position�� ������ targetPosition�� ����
            // ���⼭ this�� ī�޶��� z�� <- ĳ���ͺ��� ī�޶� �ڿ� �־�� ���� �� �ֱ� ������ �������ش�.
            targetPosition.Set(target.transform.position.x, target.transform.position.y, this.transform.position.z);

            // 1�ʿ� moveSpeed ��ŭ �̵�
            this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }
}
