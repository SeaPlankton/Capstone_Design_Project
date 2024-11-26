using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ZombieSpawnManager : MonoBehaviour
{
    public List<ZombieSpawnData> spawnDataList; // ���� ������ ����Ʈ
    public bool loopLevelData; // �ݺ� �ɼ� �߰�

    private int currentSpawnDataIndex = 0; // ���� ��ġ�� ���� ������ ����
    private int currentSpawnCount = 0; // ���ݱ��� ��ȯ�� ���� 
    private float spawnTimer = 0f; // ������ �ڵ�
    private bool isGameStop;

    public List<ZombieBoxSpawner> BoxSpawners = new List<ZombieBoxSpawner>();

    //���̺� �ľ� �뵵
    private int _wave = 1;

    //������ �� ��ȯ�� Ư������ �뵵
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

    //��Ģ 1,2,3 ���� ��ȯ
    public void SpawnRule1()
    {
        int internalCount = 0, externalCount = 0;
        currentSpawnCount = 0;
        int zombieID = 1;
        int spawnCount = _wave * 2;

        // ���� ��ȯ ���ݾ�
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

        // ���� ��ȯ ���ݾ�
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

        //�ִ� �������� 700����
        if (spawnCount > 700)
            spawnCount = 700;

        // ���� ��ȯ ���ݾ�
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

        // ���� ��ȯ ���ݾ�
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
            Debug.Log(zombieID + "�� Ư������ ��ȯ");
            SpawnZombie_Sc(zombieID);
            currentSpawnCount++;
        }
        currentSpawnCount = 0;
        while (currentSpawnCount < externalCount)
        {
            zombieID = Random.Range(2, 4);
            Debug.Log(zombieID + "�� Ư������ ��ȯ");
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

        // ���� ��ȯ ���ݾ�
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

    //ȸ�߽ð� �Ǽ����� - ����ӵ� ������
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
    public int zombieID; // ������ ��ȣ
    public int spawnCount; // ��ȯ�� ������ �� (�Ǵ� ������ �ð�)
}
