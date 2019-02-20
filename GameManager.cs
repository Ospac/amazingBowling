using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public UnityEvent onReset;
    public static GameManager instance;
    public GameObject readyPannel;
    public Text scoreText;
    public Text bestScoreText;
    public Text messageText;
    public bool isRoundActive = false;

    private int score = 0;

    public SpawnGenerator spawnGenerator;
    public ShooterRotator shooterRotator;
    public CamFollower cam;

    void Awake()
    {
        instance = this;
        UpdateUI();
    }
    void Start()
    {
        StartCoroutine("roundRoutine");
    }
    public void AddScore(int newScore){
        score += newScore;
        UpdateBestScore();
        UpdateUI();
    }
    private void UpdateBestScore()
    {
        if(GetBestScore() < score)
        {
            PlayerPrefs.SetInt("BestScore",score);
        }
    }
    int GetBestScore()
    {
        return PlayerPrefs.GetInt("BestScore");
    }

    void UpdateUI()
    {
        scoreText.text = "Score: " + score;
        bestScoreText.text = "BestScore: " + GetBestScore(); 
    }

    public void OnballDestroy() //Ball.cs에서, 볼 폭파 시점에 호출(gameManager에서 처리를 하기엔 그 시점을 알 수가 없음)
    {
        UpdateUI();
        isRoundActive = false;
    }

    public void Reset()
    {
        score = 0;
        UpdateUI();

        //라운드를 다시 처음부터 시작
        StartCoroutine("roundRoutine");
    }

    IEnumerator roundRoutine()
    {
        //READY
        onReset.Invoke(); // spawnGenerator.Reset();
        readyPannel.SetActive(true);
        cam.SetTarget(shooterRotator.transform, CamFollower.State.Idle); //cam을 포신으로, Idle상태로(zoomSize조절)
        shooterRotator.enabled = false;

        isRoundActive = false;
        messageText.text = "Ready...";
        yield return new WaitForSeconds(3f);
    
        //PLAY
        isRoundActive = true;
        readyPannel.SetActive(false);
        shooterRotator.enabled = true;  

        cam.SetTarget(shooterRotator.transform, CamFollower.State.Ready);
        while(isRoundActive == true) //Ball.cs에서 OnBallDestroy() 호출시
        {
            yield return null;
        }

        //END
        readyPannel.SetActive(true);
        shooterRotator.enabled = false;
        messageText.text = "Wait For Next Round...";
        yield return new WaitForSeconds(3f);
        Reset();
    }  
}
