using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 추후 확장성을 고려하여 만들었으나 안쓰임
/// </summary>
public class ZombieCombat : MonoBehaviour
{
    [HideInInspector]
    public Zombie Zombie;

    public float MaxHP;
    public float HP;
    public int EnemyEXP;
    public int Attack;

    private void Awake()
    {
        Zombie = GetComponent<Zombie>();
    }

    /// <summary>
    /// 초기 설정
    /// hp를 원래대로 셋, 
    /// </summary>
    public void Init()
    {
        HP = MaxHP;
        //EnemyEXP = 1;
    }
    /// <summary>
    /// 좀비에게 데미지를 가함.
    /// </summary>
    /// <param name="Damage">가할 데미지</param>
    public void Hit(float Damage)
    {
        if(HP > MaxHP)
            HP = MaxHP;

        HP -= Damage;
        if (HP < 1)
        {
            Zombie.ZombieChaseAI.ChangeState(ZombieChaseAI.ZombieState.Dead);
            //처치 시 경험치 획득
            Managers.Instance.Game.Player.PlayerCombat.KillEnemy(Zombie.ZombieCombat.EnemyEXP);
        }
    }
    /// <summary>
    /// 플레이어에게 데미지를 가함.
    /// </summary>
    public void AttackPlayer()
    {
        Managers.Instance.Game.Player.PlayerCombat.Hit(Attack);
    }
}
