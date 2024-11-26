using System;
using System.Collections.Generic;
using UnityEngine;


//시간을 기록할때 필요한 정보들
public class TimeCountInfo
{
    //TODO : Npc 변수 인터럽트 할때 필요 추후 추가
    public delegate void OnTick();
    public OnTick OnTickEvent;
    public OnTick OnEndEvent;
    public OnTick OnInterruptEvent;
    public float StartTime;
    public float DuringTime;
    public float EndTime;
    public float TimeSpeed;
    public int CurrentTick;
    public int DuringTick; //DuringTick은 몇 tick을 실행할지 입력값으로 받음 [DuringTick이 10이라는건 해당 작업을 10번 한다는 뜻]
}

public class TimeManager : MonoBehaviour
{
    //게임에서 전체적인 시간흐름을 TIMESTAMP를 기준으로 삼는다.  
    public float TIMESTAMP = 0;
    public List<TimeCountInfo> TimeCountInfoList = new List<TimeCountInfo>();


    [Header("게임 중단을 체크하는 변수")]
    public bool IsPause = false;

    //게임 시간 확인하기 위한 변수
    [Header("인게임의 시간을 기록하기 위한 변수")]
    public TimeCountInfo TimeCountInfo_InGame = new TimeCountInfo();
    [Header("게임 안의 시간과 관련된 정보")]
    public int InGameMin; //게임 안의 Hour
    public int InGameSec; //게임 안의 Minute

    private const float OneTickToSec = 1f; //현실시간 몇초를 1틱으로 설정할지 정하는 상수 [현재/ 5초로 설정] 
    //private const int OneTickToInGameMin = 5; //1틱을 인게임에서 몇분으로 설정할 건지 정하는 상수 [현재/ 1틱은 인게임에서 1분으로 설정]

    //이제부터 Time.deltaTime은 사용하지 않고 GameDeltaTime 사용 
    //속도는 GameSpeed로 조절 
    [Header("게임의 시간흐름(배속, 정지) 관련 변수 ")]
    public float GameDeltaTime = 0f;
    public float GameSpeed = 1f;



    private void Start()
    {
        TimeCountInfo timeCountInfo = new TimeCountInfo();

        //게임 시작 재기 
        CountTime(9999, 1, timeCountInfo.OnTickEvent);
    }
    private void Update()
    {
        GameDeltaTime = Time.deltaTime * GameSpeed;
        //TIMESTAMP를 기준으로 다른 TimeCountInfo들의 시작 시간을 정하기 때문에
        //Time.deltaTime을 계속 더해줌 
        TIMESTAMP += GameDeltaTime;

        UpdateTimeCountInfoList();
        UpdateTimeCountInfo_InGame();
    }

    //씬 다시 불러오면 설정해줘야 시간 정상적으로 흘러감
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

    //TimeCountInfoList에 등록된 TimeCountInfo들 업데이트
    public void UpdateTimeCountInfoList()
    {

        for (int i = 0; i < TimeCountInfoList.Count; i++)
        {
            //TIMESTAMP가 끝나는 시간 보다 크고, 현재Tick이 걸리는Tick과 같은 경우(tick을 전부 수행한 경우)
            if (TIMESTAMP > TimeCountInfoList[i].EndTime && TimeCountInfoList[i].CurrentTick == TimeCountInfoList[i].DuringTick)
            {
                TimeCountInfoList[i].OnEndEvent();
                TimeCountInfoList.RemoveAt(i);
            }
            else if (TimeCountInfoList[i].TimeSpeed != 1f) //#1. 시간이 1배속이 아닌경우
            {
                //#2. (OneTickToSec / TimeCountInfoList[i].TimeSpeed) <-1배속인 아닐때 1틱당 걸리는 시간
                if (TIMESTAMP > TimeCountInfoList[i].StartTime + (OneTickToSec / TimeCountInfoList[i].TimeSpeed)) //#3. 시작시간에 1배속이 아닐때 1틱당 걸리는 시간을 더한것이 TIMESTAMP를 넘는다면 (1틱이 갱신되었을때)
                {
                    TimeCountInfoList[i].CurrentTick += 1;

                    if (TimeCountInfoList[i].OnTickEvent != null)
                    {
                        TimeCountInfoList[i].OnTickEvent();
                    }

                    TimeCountInfoList[i].StartTime += (OneTickToSec / TimeCountInfoList[i].TimeSpeed); //시간속도 비율 계산해서 시작시간에 더해줌 
                }

            }
            else if (TIMESTAMP > TimeCountInfoList[i].StartTime + OneTickToSec) //#3. 시작시간에 1틱당 걸리는 시간을 더한것이 TIMESTAMP를 넘는다면 (1틱이 갱신되었을때)
            {
                TimeCountInfoList[i].CurrentTick += 1;

                if (TimeCountInfoList[i].OnTickEvent != null)
                {
                    TimeCountInfoList[i].OnTickEvent();
                }
                TimeCountInfoList[i].StartTime += OneTickToSec; //1틱당 걸리는 시간을 시작시간에 더해줌
            }
        }
    }

    //UpdateeTimeCountInfoList와 구조 동일하되, 끝날때 사용되는 OnEndEvent를 따로 처리 안함
    public void UpdateTimeCountInfo_InGame()
    {
        //인 게임 시간을 갱신 
        if (TimeCountInfo_InGame.TimeSpeed != 1f)
        {
            if (TIMESTAMP > TimeCountInfo_InGame.StartTime + (OneTickToSec / TimeCountInfo_InGame.TimeSpeed))
            {
                //인 게임 시간에서 현재 시간을 더하는게 의미가 있을지 모르겠음
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
            //인 게임 시간에서 현재 시간을 더하는게 의미가 있을지 모르겠음
            //TimeCountInfo_InGame.CurrentTick += 1;

            if (TimeCountInfo_InGame.OnTickEvent != null)
            {
                TimeCountInfo_InGame.OnTickEvent();
            }
            TimeCountInfo_InGame.StartTime += OneTickToSec;
        }
    }

    /// <summary>
    /// TimeCountInfo리스트에 들어갈 TimeCountInfo를 등록하는 함수 
    /// </summary>
    /// <param name="tick"> 몇 틱을 수행할지 입력</param>
    /// <param name="speed">몇 배속으로 할지 입력</param>
    /// <param name="onTick">틱이 갱신될때마다 실행할 함수 입력</param>
    /// <param name="onEnd">입력한 tick을 전부 수행하고 나서 끝날때 실행할 함수 입력</param>
    public void CountTime(int tick, float speed, TimeCountInfo.OnTick onTick, TimeCountInfo.OnTick onEnd, TimeCountInfo.OnTick onInterrupt)
    {
        //TimeCountInfo리스트에 들어갈 timeCountInfo를 새로 설정해줌 
        TimeCountInfo timeCountInfo = new TimeCountInfo();
        timeCountInfo.StartTime = TIMESTAMP; //현재 TIMESTAMP를 시작 시간으로 설정 
        timeCountInfo.DuringTime = tick * OneTickToSec; //걸리는 시간은 입력한 tick과 설정된값(1틱당 몇초로 둘지)를 곱해서 정해줌 
        timeCountInfo.EndTime = timeCountInfo.StartTime + timeCountInfo.DuringTime;
        timeCountInfo.TimeSpeed = speed;
        timeCountInfo.CurrentTick = 0;
        timeCountInfo.DuringTick = tick;


        //1배속이 아닌경우 EndTime을 따로 설정해줌
        //Ex) StartTime: 0, DuringTime: 20, tick = 10
        //1배속 speed가 1일때 끝나는 시간은 : 0 + (20 / 1) => 20   
        //2배속 speed가 2일때 끝나는 시간은 : 0 + (20 / 2) => 10
        if (timeCountInfo.TimeSpeed != 1f)
        {
            timeCountInfo.EndTime = timeCountInfo.StartTime + (timeCountInfo.DuringTime / timeCountInfo.TimeSpeed);
        }

        //틱이 갱신될때 마다 사용할 함수를 등록
        timeCountInfo.OnTickEvent += onTick;
        //틱을 전부 수행하고 나서(끝날때) 사용할 함수를 등록
        timeCountInfo.OnEndEvent += onEnd;
        //틱 수행후 특정 조건이 되었을때 중지한 후 사용할 함수등록
        timeCountInfo.OnInterruptEvent += onInterrupt;

        //매개변수를 null값으로 받는 경우(함수 실행 안하려는 경우)
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

        //TimeCountInfo리스트에 timeCount 설정해줌 
        TimeCountInfoList.Add(timeCountInfo);
        Debug.Log(DateTime.Now + "(" + TIMESTAMP + ")");
    }

    //CounTime오버로딩 
    //인게임에서의 시간은 끝이 없으므로 onEndTick 제거됨
    public void CountTime(int tick, float speed, TimeCountInfo.OnTick onTick)
    {
        TimeCountInfo_InGame.StartTime = TIMESTAMP;  //현재 TIMESTAMP를 시작 시간으로 설정 
        TimeCountInfo_InGame.DuringTime = tick * OneTickToSec; //걸리는 시간은 입력한 tick과 설정된값(1틱당 몇초로 둘지)를 곱해서 정해줌 
        TimeCountInfo_InGame.EndTime = TimeCountInfo_InGame.StartTime + TimeCountInfo_InGame.DuringTime;
        TimeCountInfo_InGame.TimeSpeed = speed;
        TimeCountInfo_InGame.CurrentTick = 0;
        TimeCountInfo_InGame.DuringTick = tick;

        //1배속이 아닌경우 EndTime을 따로 설정해줌
        //Ex) StartTime: 0, DuringTime: 20, tick = 10
        //1배속 speed가 1일때 끝나는 시간은 : 0 + (20 / 1) => 20   
        //2배속 speed가 2일때 끝나는 시간은 : 0 + (20 / 2) => 10
        if (TimeCountInfo_InGame.TimeSpeed != 1f)
        {
            TimeCountInfo_InGame.EndTime = TimeCountInfo_InGame.StartTime + (TimeCountInfo_InGame.DuringTime / TimeCountInfo_InGame.TimeSpeed);
        }

        //틱이 갱신될때 마다 사용할 함수를 등록
        TimeCountInfo_InGame.OnTickEvent += onTick;
    }


    #region 게임의 흐름을 정하는 함수들==========================================================

    //게임 중단
    public void PauseGame()
    {
        GameSpeed = 0f;
        //Managers.Instance.GameManager.NpcController.PauseNpcsAnimation();
    }

    //게임 재생
    public void PlayGame()
    {
        GameSpeed = 1f;
        //Managers.Instance.GameManager.NpcController.PlayNpcsAnimation(GameSpeed);
        //TODO : 테이블은 GridPlcement 완성되면 추가할것
    }

    //게임 1배속 설정 원래 속도
    public void SetGame1xSpeed()
    {
        GameSpeed = 1f;
        //Managers.Instance.GameManager.NpcController.PlayNpcsAnimation(GameSpeed);

    }

    //게임 2배속 설정
    public void SetGame2xSpeed()
    {
        GameSpeed = 2f;
        //Managers.Instance.GameManager.NpcController.PlayNpcsAnimation(GameSpeed);

    }
    #endregion
}
