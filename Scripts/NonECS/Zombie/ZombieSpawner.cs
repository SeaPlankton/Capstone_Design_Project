using System;
using UnityEngine;

/// <summary>
/// ���̻� ������ ���� (���� ����)
/// </summary>
[Obsolete]
public class ZombieSpawner : MonoBehaviour
{
    [Header("Spawn Options")]

    // �ִ� ��ȯ��
    public int SpawnAmount;
    // ���� ������
    public float SpawnDelay;

    private float waitTime;
    // ���ݱ����� ��ȯ��
    private int totalSpawnAmount;
    // Start is called before the first frame update
    void Start()
    {
        totalSpawnAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        waitTime += Time.deltaTime;
        if (waitTime >= SpawnDelay && totalSpawnAmount < SpawnAmount)
        {
            totalSpawnAmount++;
            waitTime = 0f;
            GameObject gameObject = Managers.Instance.ObjectPool.ZombiePool.Get();
            // ������ Ʈ�������� ���� ����
            gameObject.transform.position = transform.position;
            Zombie zombie = gameObject.GetComponent<Zombie>();
            // ���� �ɼ� �����ϰ� �ʱ� �۾�
            zombie.ZombieChaseAI.RotationSpeed = (UnityEngine.Random.Range(1, 3) == 1) ? 7f : 14f;
            zombie.ZombieChaseAI.RotationSpeed = 7f;
            zombie.ZombieChaseAI.Speed = UnityEngine.Random.Range(1f, 3f);
            zombie.ZombieChaseAI.Speed = 30f;
            zombie.Init();
        }
    }
}
