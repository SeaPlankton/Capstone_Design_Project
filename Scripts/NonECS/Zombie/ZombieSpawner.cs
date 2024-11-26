using System;
using UnityEngine;

/// <summary>
/// 더이상 사용되지 않음 (추후 삭제)
/// </summary>
[Obsolete]
public class ZombieSpawner : MonoBehaviour
{
    [Header("Spawn Options")]

    // 최대 소환량
    public int SpawnAmount;
    // 스폰 딜레이
    public float SpawnDelay;

    private float waitTime;
    // 지금까지의 소환량
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
            // 스포너 트랜스폼과 같게 변경
            gameObject.transform.position = transform.position;
            Zombie zombie = gameObject.GetComponent<Zombie>();
            // 좀비 옵션 설정하고 초기 작업
            zombie.ZombieChaseAI.RotationSpeed = (UnityEngine.Random.Range(1, 3) == 1) ? 7f : 14f;
            zombie.ZombieChaseAI.RotationSpeed = 7f;
            zombie.ZombieChaseAI.Speed = UnityEngine.Random.Range(1f, 3f);
            zombie.ZombieChaseAI.Speed = 30f;
            zombie.Init();
        }
    }
}
