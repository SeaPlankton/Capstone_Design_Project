using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� Ȯ�强�� ����Ͽ� ��������� �Ⱦ���
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
    /// �ʱ� ����
    /// hp�� ������� ��, 
    /// </summary>
    public void Init()
    {
        HP = MaxHP;
        //EnemyEXP = 1;
    }
    /// <summary>
    /// ���񿡰� �������� ����.
    /// </summary>
    /// <param name="Damage">���� ������</param>
    public void Hit(float Damage)
    {
        if(HP > MaxHP)
            HP = MaxHP;

        HP -= Damage;
        if (HP < 1)
        {
            Zombie.ZombieChaseAI.ChangeState(ZombieChaseAI.ZombieState.Dead);
            //óġ �� ����ġ ȹ��
            Managers.Instance.Game.Player.PlayerCombat.KillEnemy(Zombie.ZombieCombat.EnemyEXP);
        }
    }
    /// <summary>
    /// �÷��̾�� �������� ����.
    /// </summary>
    public void AttackPlayer()
    {
        Managers.Instance.Game.Player.PlayerCombat.Hit(Attack);
    }
}
