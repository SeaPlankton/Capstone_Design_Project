using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ZombieSpawnManager : MonoBehaviour
{
    public List<ZombieSpawnData> spawnDataList; // 레벨 데이터 리스트
    public bool loopLevelData; // 반복 옵션 추가

    private int currentSpawnDataIndex = 0; // 현재 위치한 스폰 데이터 추적
    private int currentSpawnCount = 0; // 지금까지 소환한 좀비 
    private float spawnTimer = 0f; // 딜레이 핸들
    private bool isGameStop;

    public List<ZombieBoxSpawner> BoxSpawners = new List<ZombieBoxSpawner>();

    //웨이브 파악 용도
    private int _wave = 1;

    //레벨업 시 소환될 특수좀비 용도
    private int _lv = 1;

    private void Start()
    {
        Managers.Instance.Game.TimeController.SpawnZombie1 += SpawnRule1;
        Managers.Instance.Game.TimeController.SpawnZombie2 += SpawnRule2;
        Managers.Instance.Game.TimeController.SpawnZombie3 += SpawnRule3;
        Managers.Instance.Game.TimeController.SpawnZombie4 += SpawnRule4;
        Managers.Instance.Game.TimeController.SpawnFirst += FirstSpawn;
        Managers.Instance.Game.TimeController.GameStop += OnGameStop;
        Managers.Instance.Game.TimeController.GameResume += OnGameResume;
    }
    public void OnGameStop()
    {
        isGameStop = true;
    }
    public void OnGameResume()
    {
        isGameStop = false;
    }

    //규칙 1,2,3 좀비 소환
    public void SpawnRule1()
    {
        int internalCount = 0, externalCount = 0;
        currentSpawnCount = 0;
        int zombieID = 1;
        int spawnCount = _wave * 2;

        // 좀비 소환 절반씩
        if (spawnCount % 2 != 0)
        {
            spawnCount--;
            externalCount++;
        }
        internalCount += spawnCount / 2;
        externalCount += spawnCount / 2;
        while (currentSpawnCount < internalCount)
        {
            SpawnZombie_Sc(zombieID);
            currentSpawnCount++;
        }
        currentSpawnCount = 0;
        while (currentSpawnCount < externalCount)
        {
            SpawnZombie_Ex(zombieID);
            currentSpawnCount++;
        }
    }
    public void SpawnRule2()
    {
        int internalCount = 0, externalCount = 0;
        currentSpawnCount = 0;
        int zombieID = 1;
        int spawnCount = _wave * 10;

        // 좀비 소환 절반씩
        if (spawnCount % 2 != 0)
        {
            spawnCount--;
            externalCount++;
        }
        internalCount += spawnCount / 2;
        externalCount += spawnCount / 2;
        while (currentSpawnCount < internalCount)
        {
            SpawnZombie_Sc(zombieID);
            currentSpawnCount++;
        }
        currentSpawnCount = 0;
        while (currentSpawnCount < externalCount)
        {
            SpawnZombie_Ex(zombieID);
            currentSpawnCount++;
        }
    }
    public void SpawnRule3()
    {
        int internalCount = 0, externalCount = 0;
        currentSpawnCount = 0;
        int zombieID = 1;
        int spawnCount = _wave * 100;

        //최대 마릿수는 700마리
        if (spawnCount > 700)
            spawnCount = 700;

        // 좀비 소환 절반씩
        if (spawnCount % 2 != 0)
        {
            spawnCount--;
            externalCount++;
        }
        internalCount += spawnCount / 2;
        externalCount += spawnCount / 2;
        while (currentSpawnCount < internalCount)
        {
            SpawnZombie_Sc(zombieID);
            currentSpawnCount++;
        }
        currentSpawnCount = 0;
        while (currentSpawnCount < externalCount)
        {
            SpawnZombie_Ex(zombieID);
            currentSpawnCount++;
        }

        _wave++;
    }

    public void SpawnRule4()
    {
        int internalCount = 0, externalCount = 0;
        currentSpawnCount = 0;
        int zombieID;
        int spawnCount = _lv * 2;

        // 좀비 소환 절반씩
        if (spawnCount % 2 != 0)
        {
            spawnCount--;
            externalCount++;
        }
        internalCount += spawnCount / 2;
        externalCount += spawnCount / 2;
        while (currentSpawnCount < internalCount)
        {
            zombieID = Random.Range(2, 4);
            Debug.Log(zombieID + "번 특수좀비 소환");
            SpawnZombie_Sc(zombieID);
            currentSpawnCount++;
        }
        currentSpawnCount = 0;
        while (currentSpawnCount < externalCount)
        {
            zombieID = Random.Range(2, 4);
            Debug.Log(zombieID + "번 특수좀비 소환");
            SpawnZombie_Ex(zombieID);
            currentSpawnCount++;
        }
        _lv++;
    }

    public void FirstSpawn()
    {
        int internalCount = 0, externalCount = 0;
        currentSpawnCount = 0;
        int zombieID = 1;
        int spawnCount = 7;

        // 좀비 소환 절반씩
        if (spawnCount % 2 != 0)
        {
            spawnCount--;
            externalCount++;
        }
        internalCount += spawnCount / 2;
        externalCount += spawnCount / 2;
        while (currentSpawnCount < internalCount)
        {
            SpawnZombie_Sc(zombieID);
            currentSpawnCount++;
        }
        currentSpawnCount = 0;
        while (currentSpawnCount < externalCount)
        {
            SpawnZombie_Ex(zombieID);
            currentSpawnCount++;
        }
    }


    private void SpawnZombie_Ex(int zombieIndex)
    {
        Managers.Instance.Game.ZombieBoxSpawner_Ex.SpawnZombie(zombieIndex);
    }
    private void SpawnZombie_Sc(int zombieIndex)
    {
        Managers.Instance.Game.ZombieBoxSpawner_Sc.SpawnZombie(zombieIndex);
    }

    //회중시계 악세서리 - 좀비속도 느리게
    public void ExecutionFunction()
    {
        for(int i = 0; i < BoxSpawners.Count; i++)
        {
            BoxSpawners[i].SetZombieSpeedForAccessory();
        }
    }
}

[System.Serializable]
public class ZombieSpawnData
{
    public int zombieID; // 좀비의 번호
    public int spawnCount; // 소환할 좀비의 수 (또는 딜레이 시간)
}
