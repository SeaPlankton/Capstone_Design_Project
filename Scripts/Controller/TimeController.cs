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

    [Header("�ð� ���� UI")]
    public bool OnGameTimeControlPanel = false;
    public GameObject GameTimeControlPanel;
    public GameObject BossHpBarView;

    //�ð� ������ �� ĳ���� ������ ���߱�����
    [HideInInspector]
    public bool TimeStop = false;

    public float PreviousGameSpeed;

    //�ð� ���� Text ����� ��ü 
    public Text TimerText;
    private const int OneTickToInGameSec = 1; //1ƽ�� �ΰ��ӿ��� ������� ������ ���� ���ϴ� ��� [����/ 1ƽ�� �ΰ��ӿ��� 1������ ����]

    //2��Ģ ��ȯ 21�� ī��Ʈ����
    private int _spawnMin = 0;

    //Player�� ���� ��ź ����� �����̴�?
    private bool _goingDie = false;
    //Player �׾��� �� 5�ʾȿ� �� ���̸� ��Ȱ
    private int _rebornTime = 0;
    private bool spawnBoss = false;
    //ó�� 10���� 7���� ��ȯ�뵵
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
        //���� ����/��� �Լ� ü��
        GameStop += SetGameSpeedZero;
        GameResume += SetGameSpeedBack;
    }

    //���� ���� �� ����� ��� �Լ� ����
    public void StopGame()
    {
        TimeStop = true;
        GameStop();
    }

    //���� ��� �� ����� ��� �Լ� ����
    public void ResumeGame()
    {
        TimeStop = false;
        GameResume();
    }

    //Player �׾��� �� �ش��ϴ� UI
    public void DiePlayer()
    {
        if (_goingDie)
        {
            //��ź ����� ���� UI�� ���� - ��Ȱ ������ �ƴ�
            SuccessReborn();
            _goingDie = false;
            Managers.Instance.Game.UIController.IsGoingDie = false;
            Managers.Instance.Game.BannerPresenter.IsGoingDie = false;
            _rebornTime = 0;
            //��Ȱ �׼����� ����
            Managers.Instance.Game.AccessoryController.RemoveRebornAccessory();
        }
        PlayerDie();
        StopGame();
    }

    //��Ȱ UI�� ��Ȱ������
    public void RetryPlayer()
    {
        PlayerRetry();
        ResumeGame();
    }

    //��Ȱ - ��ź����� ���� ����
    public void CheckRebornTime()
    {
        _rebornTime = 0;
        _goingDie = true;
        Managers.Instance.Game.UIController.IsGoingDie = true;
        Managers.Instance.Game.BannerPresenter.IsGoingDie = true;
        //��ź ����� ���� UI�� ����
        RebornTimeCheck();
    }

    //��ź����� �����Ϸ�
    public void RebornSuccess()
    {
        _goingDie = false;
        Managers.Instance.Game.UIController.IsGoingDie = false;
        Managers.Instance.Game.BannerPresenter.IsGoingDie = false;
        _rebornTime = 0;
        //��ź ����� ���� UI�� ����
        SuccessReborn();
        //��Ȱ �׼����� ����
        Managers.Instance.Game.AccessoryController.RemoveRebornAccessory();
        Managers.Instance.Game.Player.PlayerCombat.SetDieFalse();
    }

    public void SpawnSpecialZombie()
    {
        SpawnZombie4();
    }

    //�ΰ��ӿ��� �ð����� ������ �������ִ� �Լ� 
    public void CheckGameTimer() //1ƽ ���� ���� 
    {
        if (!spawnBoss)
        {
            // ������ȯ
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

        //���� ���� 10�� �� 7���� ��ȯ
        if(Managers.Instance.TimeManager.InGameSec == 10 && _firstSpawn)
        {
            _firstSpawn = false;
            SpawnFirst();
        }

        //60���� ������� InGameHour�� ����
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

        //���� Player�� ��ź ����� �����̸�
        if (_goingDie)
        {
            _rebornTime++;

            //6�ʰ� �Ǹ� ���
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

    #region ������ �帧�� ���ϴ� �Լ���==========================================================

    //���� �ߴ�
    public void SetGameSpeedZero()
    {
        PreviousGameSpeed = Managers.Instance.TimeManager.GameSpeed;
        Managers.Instance.TimeManager.GameSpeed = 0f;
    }

    //���� ���� ���� �ӵ���
    public void SetGameSpeedBack()
    {
        Managers.Instance.TimeManager.GameSpeed = PreviousGameSpeed;
    }

    //���� 1��� ���� ���� �ӵ�
    public void ClickSetGame1xSpeed()
    {
        Managers.Instance.TimeManager.GameSpeed = 1f;
        //Managers.Instance.GameManager.NpcController.PlayNpcsAnimation(Managers.Instance.TimeManager.GameSpeed);

    }

    //���� 2��� ����
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
