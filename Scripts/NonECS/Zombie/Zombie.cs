using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// 역할x, 묶어주는 역할
/// </summary>
[RequireComponent(typeof(ZombieAnimation), typeof(ZombieChaseAI), typeof(ZombieCombat))]
public class Zombie : MonoBehaviour
{
    public IObjectPool<GameObject> ZombiePool;
    public BoxCollider BoxCollider;
    public Rigidbody Rigidbody;
    [HideInInspector]
    public ZombieChaseAI ZombieChaseAI;
    [HideInInspector]
    public ZombieAnimation ZombieAnimation;
    [HideInInspector]
    public ZombieCombat ZombieCombat;

    // Start is called before the first frame update
    void Awake()
    {
        ZombieAnimation = GetComponent<ZombieAnimation>();
        ZombieChaseAI = GetComponent<ZombieChaseAI>();
        ZombieCombat = GetComponent<ZombieCombat>();
        BoxCollider = GetComponent<BoxCollider>();
        Rigidbody = GetComponent<Rigidbody>();
    }
    /// <summary>
    /// 초반에 실행되어야 하는 작업들 모음
    /// </summary>
    public void Init()
    {
        ZombieChaseAI.Init();
        ZombieCombat.Init();
    }
    /// <summary>
    /// 초반에 실행되어야 하는 작업들 모음 + 스탯 초기화
    /// </summary>
    /// <param name="zombieStats">초기화 값</param>
    public void Init(ZombieStats zombieStats)
    {
        ZombieChaseAI.UnFreeze();
        SetZombieStats(zombieStats);
        ZombieChaseAI.Init();
        ZombieCombat.Init();
    }

    public void SetZombieStats(ZombieStats zombieStats)
    {
        ZombieChaseAI.Speed = zombieStats.speed;
        ZombieChaseAI.RotationSpeed = zombieStats.rotationSpeed;
        ZombieChaseAI.ZombieID = zombieStats.ID;
        ZombieCombat.MaxHP = (int)zombieStats.hp;
        ZombieCombat.Attack = (int)zombieStats.attackPower;
        ZombieCombat.EnemyEXP = zombieStats.exp;
    }
}