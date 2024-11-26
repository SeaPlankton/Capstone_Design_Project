using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miku.Utils;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    public delegate void WaveUp();
    public event WaveUp WaveUpgrade;

    public delegate void SetSpawnZombie();
    public event SetSpawnZombie SpawnZombie1;
    public event SetSpawnZombie SpawnZombie2;
    public event SetSpawnZombie SpawnZombie3;
    public event SetSpawnZombie SpawnZombie4;
    public event SetSpawnZombie SpawnFirst;

    public delegate void GameState();
    public event GameState GameStop;
    public event GameState GameResume;
    public event GameState GamePlay;
    public event GameState PlayerDie;
    public event GameState PlayerRetry;
    public event GameState RebornTimeCheck;
    public event GameState SuccessReborn;

    [Header("시간 관련 UI")]
    public bool OnGameTimeControlPanel = false;
    public GameObject GameTimeControlPanel;
    public GameObject BossHpBarView;

    //시간 멈췄을 때 캐릭터 움직임 멈추기위함
    [HideInInspector]
    public bool TimeStop = false;

    public float PreviousGameSpeed;

    //시간 관련 Text 디버그 대체 
    public Text TimerText;
    private const int OneTickToInGameSec = 1; //1틱을 인게임에서 몇분으로 설정할 건지 정하는 상수 [현재/ 1틱은 인게임에서 1분으로 설정]

    //2규칙 소환 21초 카운트위함
    private int _spawnMin = 0;

    //Player가 지금 폭탄 목걸이 상태이니?
    private bool _goingDie = false;
    //Player 죽었을 때 5초안에 적 죽이면 부활
    private int _rebornTime = 0;
    private bool spawnBoss = false;
    //처음 10초후 7마리 소환용도
    private bool _firstSpawn = true;
    private void Awake()
    {
        Managers.Instance.Game.OnGridSceneLoaded += Init;
    }

    private void Init()
    {
        Managers.Instance.Game.TimeController = this;
    }

    private void OnDestroy()
    {
        if (Managers.Instance.Game == null) return;
        Managers.Instance.Game.OnGridSceneLoaded -= Init;
    }
    private void Start()
    {
        Managers.Instance.TimeManager.CountTime(99999999, 1, CheckGameTimer);
        Managers.Instance.TimeManager.CountTime(99999999, 1, CheckTimerTextUI);
        Managers.Instance.Game.InputManager.SpawnBoss += SpawnBoss;
        //게임 정지/재생 함수 체인
        GameStop += SetGameSpeedZero;
        GameResume += SetGameSpeedBack;
    }

    //게임 정지 시 연결된 모든 함수 실행
    public void StopGame()
    {
        TimeStop = true;
        GameStop();
    }

    //게임 재생 시 연결된 모든 함수 실행
    public void ResumeGame()
    {
        TimeStop = false;
        GameResume();
    }

    //Player 죽었을 때 해당하는 UI
    public void DiePlayer()
    {
        if (_goingDie)
        {
            //폭탄 목걸이 상태 UI들 해제 - 부활 성공이 아님
            SuccessReborn();
            _goingDie = false;
            Managers.Instance.Game.UIController.IsGoingDie = false;
            Managers.Instance.Game.BannerPresenter.IsGoingDie = false;
            _rebornTime = 0;
            //부활 액세서리 삭제
            Managers.Instance.Game.AccessoryController.RemoveRebornAccessory();
        }
        PlayerDie();
        StopGame();
    }

    //부활 UI로 부활했을때
    public void RetryPlayer()
    {
        PlayerRetry();
        ResumeGame();
    }

    //부활 - 폭탄목걸이 상태 진입
    public void CheckRebornTime()
    {
        _rebornTime = 0;
        _goingDie = true;
        Managers.Instance.Game.UIController.IsGoingDie = true;
        Managers.Instance.Game.BannerPresenter.IsGoingDie = true;
        //폭탄 목걸이 상태 UI들 실행
        RebornTimeCheck();
    }

    //폭탄목걸이 해제완료
    public void RebornSuccess()
    {
        _goingDie = false;
        Managers.Instance.Game.UIController.IsGoingDie = false;
        Managers.Instance.Game.BannerPresenter.IsGoingDie = false;
        _rebornTime = 0;
        //폭탄 목걸이 상태 UI들 해제
        SuccessReborn();
        //부활 액세서리 삭제
        Managers.Instance.Game.AccessoryController.RemoveRebornAccessory();
        Managers.Instance.Game.Player.PlayerCombat.SetDieFalse();
    }

    public void SpawnSpecialZombie()
    {
        SpawnZombie4();
    }

    //인게임에서 시간대의 정보를 설정해주는 함수 
    public void CheckGameTimer() //1틱 마다 실행 
    {
        if (!spawnBoss)
        {
            // 보스소환
            if (Managers.Instance.TimeManager.InGameMin == 10 && Managers.Instance.TimeManager.InGameSec == 30)
            {
                SpawnBoss(true);
            }
        }

        if (Managers.Instance.TimeManager.InGameSec < 60)
        {
            Managers.Instance.TimeManager.InGameSec += OneTickToInGameSec;
            _spawnMin += 1;
        }

        //게임 시작 10초 후 7마리 소환
        if(Managers.Instance.TimeManager.InGameSec == 10 && _firstSpawn)
        {
            _firstSpawn = false;
            SpawnFirst();
        }

        //60분이 지난경우 InGameHour를 갱신
        if (Managers.Instance.TimeManager.InGameSec >= 60)
        {
            Managers.Instance.TimeManager.InGameMin += 1;
            Managers.Instance.TimeManager.InGameSec -= 60;
           
            if (!spawnBoss)
            {
                WaveUpgrade();
                SpawnZombie3();             
            }                      
        }

        if (Managers.Instance.TimeManager.InGameSec % 4 == 0 && !spawnBoss)
        {
            SpawnZombie1();
        }

        if (_spawnMin % 21 == 0 && !spawnBoss)
        {
            _spawnMin = 0;
            SpawnZombie2();
        }

        //만약 Player가 폭탄 목걸이 상태이면
        if (_goingDie)
        {
            _rebornTime++;

            //6초가 되면 사망
            if (_rebornTime == 6)
            {
                DiePlayer();
            }
        }
    }

    public void SpawnBoss(bool n)
    {
        if (spawnBoss) return;
        spawnBoss = true;
        BossHpBarView.SetActive(true);
        Managers.Instance.Game.Boss.gameObject.SetActive(true);
        Managers.Instance.Game.Boss.BossCombat.Init();
        Managers.Instance.Game.Boss.BossAI.bossPatterns[5].CoolTimeTimer = 90f;
        Managers.Instance.Game.Player.PlayerController.IsShootBoss = true;
    }

    public void CheckTimerTextUI()
    {
        if (Managers.Instance.TimeManager.InGameSec < 10)
        {
            TimerText.text = Managers.Instance.TimeManager.InGameMin + " : 0" + Managers.Instance.TimeManager.InGameSec;
        }
        else
        {
            TimerText.text = Managers.Instance.TimeManager.InGameMin + " : " + Managers.Instance.TimeManager.InGameSec;
        }
    }

    public void ClickTimeUI()
    {
        if (OnGameTimeControlPanel == false)
        {
            GameTimeControlPanel.SetActive(true);
            OnGameTimeControlPanel = true;
        }
        else
        {
            GameTimeControlPanel.SetActive(false);
            OnGameTimeControlPanel = false;
        }
    }

    #region 게임의 흐름을 정하는 함수들==========================================================

    //게임 중단
    public void SetGameSpeedZero()
    {
        PreviousGameSpeed = Managers.Instance.TimeManager.GameSpeed;
        Managers.Instance.TimeManager.GameSpeed = 0f;
    }

    //멈춘 게임 원래 속도로
    public void SetGameSpeedBack()
    {
        Managers.Instance.TimeManager.GameSpeed = PreviousGameSpeed;
    }

    //게임 1배속 설정 원래 속도
    public void ClickSetGame1xSpeed()
    {
        Managers.Instance.TimeManager.GameSpeed = 1f;
        //Managers.Instance.GameManager.NpcController.PlayNpcsAnimation(Managers.Instance.TimeManager.GameSpeed);

    }

    //게임 2배속 설정
    public void ClickSetGame2xSpeed()
    {
        Managers.Instance.TimeManager.GameSpeed = 2f;
        //Managers.Instance.GameManager.NpcController.PlayNpcsAnimation(Managers.Instance.TimeManager.GameSpeed);

    }

    public void ClickSetGame3xSpeed()
    {
        Managers.Instance.TimeManager.GameSpeed = 3f;
        //Managers.Instance.GameManager.NpcController.PlayNpcsAnimation(Managers.Instance.TimeManager.GameSpeed);
    }
    #endregion  
}
