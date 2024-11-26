using System;
using System.Collections.Generic;
using UnityEngine;


//�ð��� ����Ҷ� �ʿ��� ������
public class TimeCountInfo
{
    //TODO : Npc ���� ���ͷ�Ʈ �Ҷ� �ʿ� ���� �߰�
    public delegate void OnTick();
    public OnTick OnTickEvent;
    public OnTick OnEndEvent;
    public OnTick OnInterruptEvent;
    public float StartTime;
    public float DuringTime;
    public float EndTime;
    public float TimeSpeed;
    public int CurrentTick;
    public int DuringTick; //DuringTick�� �� tick�� �������� �Է°����� ���� [DuringTick�� 10�̶�°� �ش� �۾��� 10�� �Ѵٴ� ��]
}

public class TimeManager : MonoBehaviour
{
    //���ӿ��� ��ü���� �ð��帧�� TIMESTAMP�� �������� ��´�.  
    public float TIMESTAMP = 0;
    public List<TimeCountInfo> TimeCountInfoList = new List<TimeCountInfo>();


    [Header("���� �ߴ��� üũ�ϴ� ����")]
    public bool IsPause = false;

    //���� �ð� Ȯ���ϱ� ���� ����
    [Header("�ΰ����� �ð��� ����ϱ� ���� ����")]
    public TimeCountInfo TimeCountInfo_InGame = new TimeCountInfo();
    [Header("���� ���� �ð��� ���õ� ����")]
    public int InGameMin; //���� ���� Hour
    public int InGameSec; //���� ���� Minute

    private const float OneTickToSec = 1f; //���ǽð� ���ʸ� 1ƽ���� �������� ���ϴ� ��� [����/ 5�ʷ� ����] 
    //private const int OneTickToInGameMin = 5; //1ƽ�� �ΰ��ӿ��� ������� ������ ���� ���ϴ� ��� [����/ 1ƽ�� �ΰ��ӿ��� 1������ ����]

    //�������� Time.deltaTime�� ������� �ʰ� GameDeltaTime ��� 
    //�ӵ��� GameSpeed�� ���� 
    [Header("������ �ð��帧(���, ����) ���� ���� ")]
    public float GameDeltaTime = 0f;
    public float GameSpeed = 1f;



    private void Start()
    {
        TimeCountInfo timeCountInfo = new TimeCountInfo();

        //���� ���� ��� 
        CountTime(9999, 1, timeCountInfo.OnTickEvent);
    }
    private void Update()
    {
        GameDeltaTime = Time.deltaTime * GameSpeed;
        //TIMESTAMP�� �������� �ٸ� TimeCountInfo���� ���� �ð��� ���ϱ� ������
        //Time.deltaTime�� ��� ������ 
        TIMESTAMP += GameDeltaTime;

        UpdateTimeCountInfoList();
        UpdateTimeCountInfo_InGame();
    }

    //�� �ٽ� �ҷ����� ��������� �ð� ���������� �귯��
    public void RestartTimeManager()
    {
        TIMESTAMP = 0;
        GameSpeed = 1f;
        TimeCountInfoList.Clear();
        InGameMin = 0;
        InGameSec = 0;
        TimeCountInfoList = new List<TimeCountInfo>();
        TimeCountInfo_InGame = new TimeCountInfo();
    }

    //TimeCountInfoList�� ��ϵ� TimeCountInfo�� ������Ʈ
    public void UpdateTimeCountInfoList()
    {

        for (int i = 0; i < TimeCountInfoList.Count; i++)
        {
            //TIMESTAMP�� ������ �ð� ���� ũ��, ����Tick�� �ɸ���Tick�� ���� ���(tick�� ���� ������ ���)
            if (TIMESTAMP > TimeCountInfoList[i].EndTime && TimeCountInfoList[i].CurrentTick == TimeCountInfoList[i].DuringTick)
            {
                TimeCountInfoList[i].OnEndEvent();
                TimeCountInfoList.RemoveAt(i);
            }
            else if (TimeCountInfoList[i].TimeSpeed != 1f) //#1. �ð��� 1����� �ƴѰ��
            {
                //#2. (OneTickToSec / TimeCountInfoList[i].TimeSpeed) <-1����� �ƴҶ� 1ƽ�� �ɸ��� �ð�
                if (TIMESTAMP > TimeCountInfoList[i].StartTime + (OneTickToSec / TimeCountInfoList[i].TimeSpeed)) //#3. ���۽ð��� 1����� �ƴҶ� 1ƽ�� �ɸ��� �ð��� ���Ѱ��� TIMESTAMP�� �Ѵ´ٸ� (1ƽ�� ���ŵǾ�����)
                {
                    TimeCountInfoList[i].CurrentTick += 1;

                    if (TimeCountInfoList[i].OnTickEvent != null)
                    {
                        TimeCountInfoList[i].OnTickEvent();
                    }

                    TimeCountInfoList[i].StartTime += (OneTickToSec / TimeCountInfoList[i].TimeSpeed); //�ð��ӵ� ���� ����ؼ� ���۽ð��� ������ 
                }

            }
            else if (TIMESTAMP > TimeCountInfoList[i].StartTime + OneTickToSec) //#3. ���۽ð��� 1ƽ�� �ɸ��� �ð��� ���Ѱ��� TIMESTAMP�� �Ѵ´ٸ� (1ƽ�� ���ŵǾ�����)
            {
                TimeCountInfoList[i].CurrentTick += 1;

                if (TimeCountInfoList[i].OnTickEvent != null)
                {
                    TimeCountInfoList[i].OnTickEvent();
                }
                TimeCountInfoList[i].StartTime += OneTickToSec; //1ƽ�� �ɸ��� �ð��� ���۽ð��� ������
            }
        }
    }

    //UpdateeTimeCountInfoList�� ���� �����ϵ�, ������ ���Ǵ� OnEndEvent�� ���� ó�� ����
    public void UpdateTimeCountInfo_InGame()
    {
        //�� ���� �ð��� ���� 
        if (TimeCountInfo_InGame.TimeSpeed != 1f)
        {
            if (TIMESTAMP > TimeCountInfo_InGame.StartTime + (OneTickToSec / TimeCountInfo_InGame.TimeSpeed))
            {
                //�� ���� �ð����� ���� �ð��� ���ϴ°� �ǹ̰� ������ �𸣰���
                //TimeCountInfo_InGame.CurrentTick += 1;

                if (TimeCountInfo_InGame.OnTickEvent != null)
                {
                    TimeCountInfo_InGame.OnTickEvent();
                }
                TimeCountInfo_InGame.StartTime += (OneTickToSec / TimeCountInfo_InGame.TimeSpeed);
            }

        }
        else if (TIMESTAMP > TimeCountInfo_InGame.StartTime + OneTickToSec)
        {
            //�� ���� �ð����� ���� �ð��� ���ϴ°� �ǹ̰� ������ �𸣰���
            //TimeCountInfo_InGame.CurrentTick += 1;

            if (TimeCountInfo_InGame.OnTickEvent != null)
            {
                TimeCountInfo_InGame.OnTickEvent();
            }
            TimeCountInfo_InGame.StartTime += OneTickToSec;
        }
    }

    /// <summary>
    /// TimeCountInfo����Ʈ�� �� TimeCountInfo�� ����ϴ� �Լ� 
    /// </summary>
    /// <param name="tick"> �� ƽ�� �������� �Է�</param>
    /// <param name="speed">�� ������� ���� �Է�</param>
    /// <param name="onTick">ƽ�� ���ŵɶ����� ������ �Լ� �Է�</param>
    /// <param name="onEnd">�Է��� tick�� ���� �����ϰ� ���� ������ ������ �Լ� �Է�</param>
    public void CountTime(int tick, float speed, TimeCountInfo.OnTick onTick, TimeCountInfo.OnTick onEnd, TimeCountInfo.OnTick onInterrupt)
    {
        //TimeCountInfo����Ʈ�� �� timeCountInfo�� ���� �������� 
        TimeCountInfo timeCountInfo = new TimeCountInfo();
        timeCountInfo.StartTime = TIMESTAMP; //���� TIMESTAMP�� ���� �ð����� ���� 
        timeCountInfo.DuringTime = tick * OneTickToSec; //�ɸ��� �ð��� �Է��� tick�� �����Ȱ�(1ƽ�� ���ʷ� ����)�� ���ؼ� ������ 
        timeCountInfo.EndTime = timeCountInfo.StartTime + timeCountInfo.DuringTime;
        timeCountInfo.TimeSpeed = speed;
        timeCountInfo.CurrentTick = 0;
        timeCountInfo.DuringTick = tick;


        //1����� �ƴѰ�� EndTime�� ���� ��������
        //Ex) StartTime: 0, DuringTime: 20, tick = 10
        //1��� speed�� 1�϶� ������ �ð��� : 0 + (20 / 1) => 20   
        //2��� speed�� 2�϶� ������ �ð��� : 0 + (20 / 2) => 10
        if (timeCountInfo.TimeSpeed != 1f)
        {
            timeCountInfo.EndTime = timeCountInfo.StartTime + (timeCountInfo.DuringTime / timeCountInfo.TimeSpeed);
        }

        //ƽ�� ���ŵɶ� ���� ����� �Լ��� ���
        timeCountInfo.OnTickEvent += onTick;
        //ƽ�� ���� �����ϰ� ����(������) ����� �Լ��� ���
        timeCountInfo.OnEndEvent += onEnd;
        //ƽ ������ Ư�� ������ �Ǿ����� ������ �� ����� �Լ����
        timeCountInfo.OnInterruptEvent += onInterrupt;

        //�Ű������� null������ �޴� ���(�Լ� ���� ���Ϸ��� ���)
        if (onTick == null)
        {
            timeCountInfo.OnTickEvent -= onTick;
        }
        if (onEnd == null)
        {
            timeCountInfo.OnEndEvent -= onEnd;
        }
        if (onInterrupt == null)
        {
            timeCountInfo.OnInterruptEvent -= onInterrupt;
        }

        //TimeCountInfo����Ʈ�� timeCount �������� 
        TimeCountInfoList.Add(timeCountInfo);
        Debug.Log(DateTime.Now + "(" + TIMESTAMP + ")");
    }

    //CounTime�����ε� 
    //�ΰ��ӿ����� �ð��� ���� �����Ƿ� onEndTick ���ŵ�
    public void CountTime(int tick, float speed, TimeCountInfo.OnTick onTick)
    {
        TimeCountInfo_InGame.StartTime = TIMESTAMP;  //���� TIMESTAMP�� ���� �ð����� ���� 
        TimeCountInfo_InGame.DuringTime = tick * OneTickToSec; //�ɸ��� �ð��� �Է��� tick�� �����Ȱ�(1ƽ�� ���ʷ� ����)�� ���ؼ� ������ 
        TimeCountInfo_InGame.EndTime = TimeCountInfo_InGame.StartTime + TimeCountInfo_InGame.DuringTime;
        TimeCountInfo_InGame.TimeSpeed = speed;
        TimeCountInfo_InGame.CurrentTick = 0;
        TimeCountInfo_InGame.DuringTick = tick;

        //1����� �ƴѰ�� EndTime�� ���� ��������
        //Ex) StartTime: 0, DuringTime: 20, tick = 10
        //1��� speed�� 1�϶� ������ �ð��� : 0 + (20 / 1) => 20   
        //2��� speed�� 2�϶� ������ �ð��� : 0 + (20 / 2) => 10
        if (TimeCountInfo_InGame.TimeSpeed != 1f)
        {
            TimeCountInfo_InGame.EndTime = TimeCountInfo_InGame.StartTime + (TimeCountInfo_InGame.DuringTime / TimeCountInfo_InGame.TimeSpeed);
        }

        //ƽ�� ���ŵɶ� ���� ����� �Լ��� ���
        TimeCountInfo_InGame.OnTickEvent += onTick;
    }


    #region ������ �帧�� ���ϴ� �Լ���==========================================================

    //���� �ߴ�
    public void PauseGame()
    {
        GameSpeed = 0f;
        //Managers.Instance.GameManager.NpcController.PauseNpcsAnimation();
    }

    //���� ���
    public void PlayGame()
    {
        GameSpeed = 1f;
        //Managers.Instance.GameManager.NpcController.PlayNpcsAnimation(GameSpeed);
        //TODO : ���̺��� GridPlcement �ϼ��Ǹ� �߰��Ұ�
    }

    //���� 1��� ���� ���� �ӵ�
    public void SetGame1xSpeed()
    {
        GameSpeed = 1f;
        //Managers.Instance.GameManager.NpcController.PlayNpcsAnimation(GameSpeed);

    }

    //���� 2��� ����
    public void SetGame2xSpeed()
    {
        GameSpeed = 2f;
        //Managers.Instance.GameManager.NpcController.PlayNpcsAnimation(GameSpeed);

    }
    #endregion
}
