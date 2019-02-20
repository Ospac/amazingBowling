using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    //Layer는 Tag와 달리 여러 개를  변수 하나로 지정할 수 있음
    public LayerMask whatIsProp;
    public ParticleSystem explosionParticle;
    public AudioSource explosionAudio;
    public float maxDamage = 100f;
    public float explosionForce = 1000f;
    public float lifeTime = 10f;
    public float explosionRadius = 20f;

    void Start()    
    {
        Destroy(gameObject, lifeTime);
    }

    //Ball이 Plane가 접촉시 호출
    //explosionParticle은 Ball의 child라서 따라다니다가, 폭발 시점에서 분리됨(Ball obejct가 먼저 사라져야하기 떄문에)
    private void OnTriggerEnter(Collider other)
    {
        //특정 원안에, 조건에 맞는 collider들의 배열 반환
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, whatIsProp);

        for(int i = 0; i < colliders.Length; i++)
        {
            Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();
            targetRigidbody.AddExplosionForce(explosionForce,transform.position, explosionRadius); //폭발에 의해 튕기는 효과

            Prop targetProp = colliders[i].GetComponent<Prop>();
            targetProp.TakeDamage(CalculateDamage(targetProp.transform.position)); //각Prop에게 damage

        }
        explosionParticle.transform.parent = null;
        explosionParticle.Play();
        explosionAudio.Play();
        Destroy(explosionParticle.gameObject, explosionParticle.duration); //해당 particle의 재생시간이, Destroy의 지연시간
        Destroy(gameObject);
        // GameManager.instance.OnballDestroy(); // 파괴시점, GameManager의 roundRoutine()이 PLAY페이즈에서 END페이즈로 넘어감
    }
    
    private float CalculateDamage(Vector3 targetPosition){
        Vector3 explosionToTarget = targetPosition - transform.position;
        float distance = explosionToTarget.magnitude;
        float edgeToCenterDistance = explosionRadius - distance; 
        float percentage = edgeToCenterDistance / explosionRadius;
        float damage = percentage * maxDamage;
        
        //position이 원밖이지만 collider에 포함된 경우, edgeToCenterDistance값이 음이 되어, 
        //damage가 음수가 되는 경우가 생김(체력회복), 그 경우 0으로 바꾼다.
        damage = Mathf.Max(0,damage); 
        return damage;

    }

    //해당 게임오브젝트가 파괴되는 순간 자동으로 호출되는 내부함수, 여기에 onballDestroy()를 넣어서
    //게임에서 ball이 파괴되는 두 경우(OnTriggerEnter, Destroy(gameObject, lifeTime);) 즉, 닿아서 파괴될 때와 시간이 지나서 파괴 될 때에 
    //처리를 onDestroy()하나로 커버하자
    private void OnDestroy()
    {
        GameManager.instance.OnballDestroy();
    }
}
