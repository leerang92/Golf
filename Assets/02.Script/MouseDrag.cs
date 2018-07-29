using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDrag : MonoBehaviour {

    /*
     * mainCam : 메인 카메라 컴포넌트
     * lineRender : 마우스 위치에 따라 라인을 그릴 Line Renderer 컴포넌트
     * playerComp : 플레이어 컴포넌트
     */
    private Camera mainCam;
    private LineRenderer lineRender;
    private Player playerComp;

    /*
     * bDrag : 드래그 여부
     * bShooting : 공 날릴 수 있는지 여부
     */
    private bool bDrag = false;
    private bool bShooting = false;

    private float lineDist = 0.0f;

	void Start () {
        mainCam = Camera.main;
        playerComp = GetComponent<Player>();
        lineRender = GetComponent<LineRenderer>();
    }

    public IEnumerator OnMouseDrag()
    {
        bDrag = true;
        RaycastHit hit;
        // 라인 렌더러 시작 위치
        Vector3 StartPoint = GetMousePoint();

        while (bDrag)
        {
            // 마우스 위치로 레이캐스트 발사
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            // Ball 오브젝트에 충돌 시 Ball 발사 가능으로 변경
            if (Physics.Raycast(ray.origin, ray.direction, out hit) && hit.collider.CompareTag("Ball"))
            {
                bShooting = true;
            }

            // 현재 드래그 위치에 라인 그리기
            if(bShooting)
            {
                Vector3 EndPoint = GetMousePoint();
                lineDist = Vector3.Distance(StartPoint, EndPoint);
                yield return DrawingLine(StartPoint, EndPoint);
            }
            yield return null;
        }
    }

    // 현재 마우스 위치로 Line Renderer 그리기
    IEnumerator DrawingLine(Vector3 StartPos, Vector3 EndPos)
    {
        
        lineRender.SetPositions(new Vector3[] { StartPos, EndPos });

        yield return null;
    }

    // 현재 마우스 위치 반환
    private Vector3 GetMousePoint()
    {
        // 현재 마우스 위치를 Ray 위치로 변경하여 반환
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        return ray.origin + ray.direction * 500.0f;
    }

    // 볼 날릴 수 있는지 여부 반환
    public bool IsShooting()
    {
        return bShooting;
    }

    // 드래그 정보 초기화
    public void Reset()
    {
        playerComp.Power = lineDist;
        lineRender.enabled = false;
    }
}
