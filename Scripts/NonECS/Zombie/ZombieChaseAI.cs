using System;
using UnityEngine;

/*
 * ���� ���� ������ ����
 * 1. ��� ��Ÿ�� ���� ����
 * 2. ���
 * 
 * ���� ������ ����ȭ�� ���� ���� �ּ��� ����� ���� ����.
 */

/// <summary>
/// ���� StateMachine ����
/// ������ ������ �����̾�����, Ȯ��
/// </summary>
[RequireComponent(typeof(Zombie))]
public class ZombieChaseAI : MonoBehaviour
{
    [HideInInspector]
    public Zombie Zombie;
    public int ZombieID;
    public enum ZombieState
    {
        Idle = 0,
        Chase = 1,
        Attack = 2,
        Dead = 3
    }
    public ZombieState PresentState;
    public Vector3 TargetTransform;

    // �÷��̾���� �Ÿ��� ���� (��Ʈ ��� ���ϱ� ����)
    public float SqrDistance;
    
    [Header("Chase Option")]
    // ���� ����ȭ�� ���� ������
    public float DelayChaseTime = 0.2f;
    // �ӵ�
    public float Speed;

    // ���� ����ٰ� �ٽ� ������ �� �ӵ� ����
    public float PreviousSpeed;

    // ȸ�� �ӵ�
    public float RotationSpeed = 5f;
    [Header("Attack Option")]
    // sqrrange = ���ݹ����� ����
    public float AttackSqrRange = 1f;
    public float AttackIntervalTime = 0.5f;

    [Header("Debug")]
    // ���� ����� ����� ������.
    public bool IsClosest = false;
    // ����ִ��� üũ
    public bool IsDead = false;

    // �߰� Ÿ�� ���
    private float waitChaseTime = 0f;
    // �ױ� Ÿ�� ���
    private float waitDeadTime = 0f;

    // ���� ������ ���� ���
    [HideInInspector]
    public bool IsGameStop = false;
    
    private float playerTouchTime = 0f;

    // Start is called before the first frame update
    void Awake()
    {
        Zombie = GetComponent<Zombie>();
        DelayChaseTime = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsGameStop) return;
        // �߰ݻ���
        if (PresentState == ZombieState.Chase)
        {
            waitChaseTime += Time.deltaTime;
            if (waitChaseTime >= DelayChaseTime)
            {
                waitChaseTime = 0f;

                LookPlayer();
            }
            // �Ÿ� ���
            TargetTransform = new Vector3(Managers.Instance.Game.Player.transform.position.x, transform.position.y, Managers.Instance.Game.Player.transform.position.z);
            Vector3 LookDirection = TargetTransform - transform.position;

            // ������ �̵�
            transform.position += Speed * Time.deltaTime * new Vector3(transform.forward.x, 0f, transform.forward.z);
            Debug.DrawRay(transform.position, transform.forward, Color.blue);
            SqrDistance = LookDirection.sqrMagnitude;
            // �Ÿ��� ����� ��������� �������� ��ȯ
            if (SqrDistance < AttackSqrRange) ChangeState(ZombieState.Attack);
        } else if (PresentState == ZombieState.Attack)
        {
            // ���� ��Ÿ�� ī��Ʈ
            playerTouchTime += Time.deltaTime;

            // �ϴ� ����
            Zombie.Rigidbody.velocity = Vector3.zero;

            // �Ÿ� ���
            TargetTransform = new Vector3(Managers.Instance.Game.Player.transform.position.x, transform.position.y, Managers.Instance.Game.Player.transform.position.z);
            Vector3 LookDirection = TargetTransform - transform.position;
            SqrDistance = LookDirection.sqrMagnitude;

            // ���� �ֱⰡ �Ǹ� ����
            if (playerTouchTime > AttackIntervalTime) 
            {
                playerTouchTime = 0f;
                Zombie.ZombieCombat.AttackPlayer();
            }
            // �Ÿ� �־����� �ٽ� ���� ���� ����
            if (SqrDistance > AttackSqrRange) ChangeState(ZombieState.Chase);
        } else if (PresentState == ZombieState.Dead)
        {
            // �׾��� ��� 3�� �� ����
            waitDeadTime += Time.deltaTime;
            if (waitDeadTime > 3f)
            {
                waitDeadTime = 0f;
                
                Zombie.ZombiePool.Release(gameObject);
            }

        } else if (PresentState == ZombieState.Idle)
        {

        }
    }
    /// <summary>
    /// ���� ��ȯ�� �ʹ� �۾�
    /// </summary>
    public void Init()
    {
        ChangeState(ZombieChaseAI.ZombieState.Chase);
        LookPlayer();
        IsDead = false;
    }
    
    /// <summary>
    /// �� �״�� �÷��̾ �ٶ󺸰� ����� ��.
    /// </summary>
    public void LookPlayer()
    {
        TargetTransform = new Vector3(Managers.Instance.Game.Player.transform.position.x, transform.position.y, Managers.Instance.Game.Player.transform.position.z);
        Vector3 LookDirection = TargetTransform - transform.position;
        LookDirection.y = 0;

        if (LookDirection != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(LookDirection);
            transform.rotation = rotation;
        }
    }
    /// <summary>
    /// ������ ���� ��ȯ
    /// </summary>
    /// <param name="zombieState">��ȯ�� ����</param>
    public void ChangeState(ZombieState zombieState)
    {
        switch (zombieState)
        {
            case ZombieState.Chase:
                playerTouchTime = 0f;
                PresentState = ZombieState.Chase;
                Zombie.ZombieAnimation.ChangeState(ZombieAnimation.EnemyState.WalkForward);
                break;
            case ZombieState.Attack:
                PresentState = ZombieState.Attack;
                Zombie.ZombieAnimation.ChangeState(ZombieAnimation.EnemyState.Attack);
                break;
            case ZombieState.Dead:
                playerTouchTime = 0f;
                PresentState = ZombieState.Dead;
                Freeze();
                // ���� ���� ����� ���񿴴ٸ�
                if (IsClosest)
                {
                    Managers.Instance.Game.ZombieController.ClosestZombie = null;
                    IsClosest = false;
                }
                IsDead = true;
                Zombie.ZombieAnimation.ChangeState(ZombieAnimation.EnemyState.Death);
                break;
            default:
                break;
        }

    }
    public void Freeze()
    {
        Zombie.BoxCollider.enabled = false;
        Zombie.Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }
    public void UnFreeze()
    {
        Zombie.BoxCollider.enabled = true;
        Zombie.Rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
    }

    //�������� ����Ʈ ź ���� �� ���� ���ο� �ɾ��� �Լ�, ��ġ�� �Ⱦ˷��༭ ����
    public void SetZomBieSlow(float figure)
    {
        Speed -= figure;
    }
}
