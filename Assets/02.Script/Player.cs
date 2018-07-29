using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public enum EPlayerState : int
    {
        Prepare,
        Shooting,
    }

    private EPlayerState playerState = EPlayerState.Prepare;

    [SerializeField]
    private GameObject BallObj; // 골프공 오브젝트

    private Camera mainCam; // 메인 카메라
    private Ball BallComp; // BallComp : 볼 오브젝트의 컴포넌트
    private MouseDrag DragComp;  // DragComp : 마우스 드래그 관리하는 컴포넌트

    /* 공이 날아갈 궤적을 그릴 변수들 */
    [SerializeField]
    private GameObject TargetDecalObj;
    private LineRenderer lineRender; // Line Renderer 컴포넌트
    public int linePathPointCount = 20; // Line Renderer로 라인을 그릴 때 사용할 포인트 갯수

    private float power = 0.0f; // 마우스 드래그에 따른 공이 날아갈 힘을 줄 변수
    public float Power
    {
        get
        {
            return power;
        }
        set
        {
            power = value * 10.0f;
            power = Mathf.Clamp(power, 1.0f, 1.2f);
        }
    }

    public static Player instance;

    void Start()
    {
        instance = this;
        mainCam = Camera.main;
        BallComp = BallObj.GetComponent<Ball>();
        DragComp = GetComponent<MouseDrag>();
        lineRender = GetComponent<LineRenderer>();

        TargetDecalObj.SetActive(false);

        SetCamera(EPlayerState.Prepare);
    }
	
	void Update ()
    {
       switch(playerState)
        {
            case EPlayerState.Prepare:
                if (Input.GetMouseButton(0))
                {
                    SetBallPosition();
                }
                break;
            case EPlayerState.Shooting:
                BallShootingControl();
                break;
        }
    }
    
    public void ChangeState(EPlayerState state)
    {
        playerState = state;
        SetCamera(playerState);
    }

    void SetCamera(EPlayerState state)
    {
        Vector3 BallPos = BallComp.transform.position;
        if (state == EPlayerState.Prepare)
        {
            TargetDecalObj.SetActive(true);
            mainCam.transform.position = new Vector3(BallPos.x, BallPos.y + 100.0f, BallPos.z - 50.0f);
            mainCam.transform.Rotate(new Vector3(40.0f, 0, 0));
        }
        else if(state == EPlayerState.Shooting)
        {
            TargetDecalObj.SetActive(false);
            mainCam.transform.position = new Vector3(BallPos.x, BallPos.y + 1, BallPos.z - 10.0f);
            mainCam.transform.LookAt(BallPos);
        }
    }

    // 볼을 선택한 위치로 날리도록 설정
    void SetBallPosition()
    { 
        // 볼이 날아갈 위치 설정
        Vector3 TargetPos = GetBallDestination();
        TargetDecalObj.transform.position = new Vector3(TargetPos.x, TargetPos.y + 0.5f, TargetPos.z);

        if (!lineRender.enabled)
            lineRender.enabled = true;
        // 볼이 날아갈 궤도 그리기
        Vector3 Direction = BallComp.GetVelocity(TargetPos, Vector3.zero);
        UpdateTrajectory(BallComp.transform.position, Direction, Physics.gravity);
    }

    // 마우스 위치의 볼이 날아갈 위치 반환
    Vector3 GetBallDestination()
    {
        Vector3 targetPos = new Vector3(0, 0, 0);
        RaycastHit hit;
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        // Ball 오브젝트에 충돌 시 Ball 발사 가능으로 변경
        if (Physics.Raycast(ray.origin, ray.direction, out hit) && hit.collider.CompareTag("Ground"))
        {
            targetPos = hit.point;
        }
        return targetPos;
    }

    // 볼을 날릴 때 컨트롤할 함수
    void BallShootingControl()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 슈팅 강도 라인을 마우스 위치에 따라 그리기
            StartCoroutine(DragComp.OnMouseDrag());
        }
        else if (Input.GetMouseButtonUp(0))
        {
            // 드래그 정보 초기화
            DragComp.Reset();
            // 공 날리기
            if (DragComp.IsShooting())
            {
                // 현재 바람 방향
                Vector3 WindDir = GameManager.instance.GetWindDirection();
                // 공 날리기
                BallComp.Shooting(TargetDecalObj.transform.position, WindDir, power);
            }
        }
    }

    /* 공이 날아갈 궤도를 그리는 함수
     * initPosition : 시작 위치
     * initVelocity : 도착 위치
     * gravity : 중력값
     */
    void UpdateTrajectory(Vector3 initPosition, Vector3 direction, Vector3 gravity)
    {
        float timeDelta = 10f / direction.magnitude;

        lineRender.SetVertexCount(linePathPointCount);

        for(int i = 0; i < linePathPointCount; ++i)
        {
            lineRender.SetPosition(i, initPosition);

            initPosition += (direction * timeDelta) + (0.5f * gravity * timeDelta * timeDelta);
            direction += gravity * timeDelta;
        }
    }

    public EPlayerState GetPlayerState()
    {
        return playerState;
    }
}
