using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    // 캐릭터의 이동 속도
    public float speed;

    // 캐릭터의 이동 방향
    private Vector3 vector;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            // z값은 바뀌지 않기 때문에 z로만 해도 문제 없다
            vector.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), transform.position.z);

            // 좌우 방향키 입력 시 상하 이동을 막기 위해 상하 이동값을 0으로 설정
            if (vector.x != 0)
            {
                // Translate : 현재 위치에서 인자로 받은 값만큼 이동
                transform.Translate(vector.x * speed * Time.deltaTime, 0, 0);
            }
            // 상하 방향키 입력 시 좌우 이동을 막기 위해 좌우 이동값을 0으로 설정
            else if (vector.y != 0)
            {
                transform.Translate(0, vector.y * speed * Time.deltaTime, 0);
            }
        }
    }
}
