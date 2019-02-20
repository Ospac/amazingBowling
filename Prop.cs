using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{

    public int score = 5;
    public ParticleSystem explosionParticle;
    public float hp = 10f;
    public void TakeDamage(float damage){
        hp -= damage;
        if(hp <= 0){
            ParticleSystem instance = Instantiate(explosionParticle,transform.position, transform.rotation);
            
            AudioSource explosionAudio = instance.GetComponent<AudioSource>();
            explosionAudio.Play();

            GameManager.instance.AddScore(score); 

            Destroy(instance.gameObject, instance.duration);
            gameObject.SetActive(false); //Prop을 라운드마다 파괴하고 생성x, 파괴된 prop을 off하는 방식으로

        }
    }
}
