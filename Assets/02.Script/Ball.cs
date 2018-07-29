using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    public Rigidbody rb;
    // 시간 : 값이 클 수록 느리고 높게 이동하게 됨
    public float time = 3f;
    // 중력값
    private float gravity;
    // 현재 이동중인지 여부
    private bool bMove = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gravity = Mathf.Abs(Physics.gravity.magnitude);

        rb.AddForce(Vector3.forward * 5.0f, ForceMode.VelocityChange);
    }

    public void Shooting(Vector3 TargetPos, Vector3 WindDir, float power)
    {
        bMove = true;
        Vector3 Velocity = GetVelocity(TargetPos, WindDir, power);
        rb.AddForce(Velocity, ForceMode.VelocityChange);
    }

    /*
     * TargetPos : 날아갈 타겟 위치
     * WindDir : 현재 바람이 부는 방향
     * power : 공이 날아갈 힘의 크기
     */
    public Vector3 GetVelocity(Vector3 TargetPos, Vector3 WindDir, float power = 1.0f)
    {
        float X, Y, Z; // 축
        float X0 = 0f, Y0 = 0f, Z0 = 0f; // 초기 축
        float V0x, V0y, V0z; // 축 속도

        // 타겟까지의 거리
        Vector3 Distance = TargetPos - transform.position;

        // 각 축을 따라 이동할 거리
        X = Distance.x + WindDir.x;
        Y = Distance.y;
        Z = Distance.z * power + WindDir.z;

        // 각 축의 속도 계산 (속도 방정식)
        V0x = (X - X0) / time;
        V0z = (Z - Z0) / time;
        V0y = (Y - Y0 + (0.5f * gravity * Mathf.Pow(time, 2))) / time;

        //V0x -= rb.velocity.x;
        //V0y -= rb.velocity.y;
        //V0z -= rb.velocity.z; 

        Vector3 Velocity = new Vector3(V0x, V0y, V0z);
        return Velocity;
    }

    void StopBall()
    {
        rb.isKinematic = true;
        bMove = false;
    }
}
