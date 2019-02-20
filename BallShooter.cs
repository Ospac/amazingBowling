using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallShooter : MonoBehaviour
{
    public CamFollower cam;
    public Rigidbody ball;
    public Transform firePos;
    public Slider powerSlider;
    public AudioSource shootingAudio;
    public AudioClip fireClip;
    public AudioClip chargingClip;
    public float minForce = 15f;
    public float maxForce = 30f;
    public float chargingTime = 0.75f;
    private float currentForce;
    private float chargeSpeed;
    private bool fired;
    private void OnEnable(){ //Component가 켜질 때 마다 실행되는 함수
        currentForce = minForce;
        powerSlider.value = minForce;
        fired = false;
    }
    private void Start()
    {
        chargeSpeed = (maxForce - minForce) / chargingTime; //거리 / 시간 = 속도
    }
    private void Update()
    {
        if(fired == true)
            return;
            
        powerSlider.value = minForce;

        if(currentForce >= maxForce && fired == false)
        {
            currentForce = maxForce;
            Fire();
        }
        else if(Input.GetButtonDown("Fire1"))
        {   
            currentForce = minForce;
            shootingAudio.clip = chargingClip;
            shootingAudio.Play();

        }
        else if(Input.GetButton("Fire1") && fired == false)
        {
            currentForce += (chargeSpeed * Time.deltaTime); //1초에 chargeSpeed만큼 더 currentForce값이 커짐
            powerSlider.value = currentForce;
        }
        else if(Input.GetButtonUp("Fire1") && fired == false)
        {
            Fire();
        }
    }
    private void Fire()
    {
        fired = true;
        Rigidbody ballInstance = Instantiate(ball,firePos.position,firePos.rotation);
        ballInstance.velocity = currentForce * firePos.forward; //forward는 z축앞 방향의 vector3
        
        shootingAudio.clip = fireClip;
        shootingAudio.Play();
        currentForce = minForce;

        cam.SetTarget(ballInstance.transform, CamFollower.State.Tracking); //State는 해당 class의 임의의 자료형임!!
        //cam을 레퍼런싱하여 함수를 쓰는것. 해당 cam객체는 Rig에 있음

    }
}
