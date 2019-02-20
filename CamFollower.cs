using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollower : MonoBehaviour
{
    public enum State {
        Idle,Ready,Tracking
    }
    private State state { //property. 외부에서는 대입연산자로 쉽게 접근 가능하고, 내부에서는 다른 처리를 넣기위해서 쓰임
        set{ 
            switch(value)
            {
                case State.Idle:
                    targetZoomSize = roundReadyZoomSize;
                    break;
                case State.Ready:
                    targetZoomSize = readyShotZoomSize;
                    break;
                case State.Tracking:
                    targetZoomSize = trackingZoomSize;
                    break;
            }   
        }
    }

    private Transform target;
    public float smoothTime = 0.2f;
    private Vector3 lastMovingVelocity; //마지막 순간의 속도, SmoothDamp 호출시 필요
    private float lastZoomSpeed;

    private Vector3 targetPosition;
    private Camera cam;
    private float targetZoomSize = 5f;
    
    private const float roundReadyZoomSize = 14.5f;
    private const float readyShotZoomSize = 5f;
    private const float trackingZoomSize = 10f;

    void Awake()
    {
        cam = GetComponentInChildren<Camera>();

        state = State.Idle;
    }
    
    private void Move()
    {
        targetPosition = target.transform.position;
        // transform.position = targetPosition  //카메라가 딱 붙어서 이동하는 것처럼 보임
        Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref lastMovingVelocity,smoothTime);

        transform.position = smoothPosition;
    }
    private void Zoom()
    {
        float smoothZoomSize = Mathf.SmoothDamp(cam.orthographicSize,targetZoomSize,ref lastZoomSpeed, smoothTime);

        cam.orthographicSize = smoothZoomSize;
    }
    private void Reset()
    {
        state = State.Idle;
    }
    public void SetTarget(Transform newTarget, State newState)
    {
        target = newTarget;
        state = newState;   
    }
    private void FixedUpdate()
    {
        if(target != null)
        {
            Move();
            Zoom();
        }
    }
}
