using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// ����x, �����ִ� ����
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
    /// �ʹݿ� ����Ǿ�� �ϴ� �۾��� ����
    /// </summary>
    public void Init()
    {
        ZombieChaseAI.Init();
        ZombieCombat.Init();
    }
    /// <summary>
    /// �ʹݿ� ����Ǿ�� �ϴ� �۾��� ���� + ���� �ʱ�ȭ
    /// </summary>
    /// <param name="zombieStats">�ʱ�ȭ ��</param>
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