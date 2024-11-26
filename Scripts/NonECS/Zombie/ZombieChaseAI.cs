using System;
using UnityEngine;

/*
 * 기존 로직 수정된 이유
 * 1. 계속 얼타는 현상 방지
 * 2. 비쌈
 * 
 * 공격 로직은 최적화로 인해 가장 최선의 방안을 택한 것임.
 */

/// <summary>
/// 간단 StateMachine 구현
/// 기존엔 추적만 역할이었으나, 확장
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

    // 플레이어와의 거리의 제곱 (루트 계산 피하기 위함)
    public float SqrDistance;
    
    [Header("Chase Option")]
    // 성능 최적화를 위한 딜레이
    public float DelayChaseTime = 0.2f;
    // 속도
    public float Speed;

    // 게임 멈췄다가 다시 진행할 때 속도 저장
    public float PreviousSpeed;

    // 회전 속도
    public float RotationSpeed = 5f;
    [Header("Attack Option")]
    // sqrrange = 공격범위의 제곱
    public float AttackSqrRange = 1f;
    public float AttackIntervalTime = 0.5f;

    [Header("Debug")]
    // 가장 가까운 좀비로 지정됨.
    public bool IsClosest = false;
    // 살아있는지 체크
    public bool IsDead = false;

    // 추격 타임 대기
    private float waitChaseTime = 0f;
    // 죽기 타임 대기
    private float waitDeadTime = 0f;

    // 게임 정지시 로직 대기
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
        // 추격상태
        if (PresentState == ZombieState.Chase)
        {
            waitChaseTime += Time.deltaTime;
            if (waitChaseTime >= DelayChaseTime)
            {
                waitChaseTime = 0f;

                LookPlayer();
            }
            // 거리 계산
            TargetTransform = new Vector3(Managers.Instance.Game.Player.transform.position.x, transform.position.y, Managers.Instance.Game.Player.transform.position.z);
            Vector3 LookDirection = TargetTransform - transform.position;

            // 앞으로 이동
            transform.position += Speed * Time.deltaTime * new Vector3(transform.forward.x, 0f, transform.forward.z);
            Debug.DrawRay(transform.position, transform.forward, Color.blue);
            SqrDistance = LookDirection.sqrMagnitude;
            // 거리가 충분히 가까워지면 공격으로 전환
            if (SqrDistance < AttackSqrRange) ChangeState(ZombieState.Attack);
        } else if (PresentState == ZombieState.Attack)
        {
            // 공격 쿨타임 카운트
            playerTouchTime += Time.deltaTime;

            // 일단 정지
            Zombie.Rigidbody.velocity = Vector3.zero;

            // 거리 계산
            TargetTransform = new Vector3(Managers.Instance.Game.Player.transform.position.x, transform.position.y, Managers.Instance.Game.Player.transform.position.z);
            Vector3 LookDirection = TargetTransform - transform.position;
            SqrDistance = LookDirection.sqrMagnitude;

            // 공격 주기가 되면 공격
            if (playerTouchTime > AttackIntervalTime) 
            {
                playerTouchTime = 0f;
                Zombie.ZombieCombat.AttackPlayer();
            }
            // 거리 멀어지면 다시 추적 상태 진입
            if (SqrDistance > AttackSqrRange) ChangeState(ZombieState.Chase);
        } else if (PresentState == ZombieState.Dead)
        {
            // 죽었을 경우 3초 뒤 삭제
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
    /// 좀비 소환시 초반 작업
    /// </summary>
    public void Init()
    {
        ChangeState(ZombieChaseAI.ZombieState.Chase);
        LookPlayer();
        IsDead = false;
    }
    
    /// <summary>
    /// 말 그대로 플레이어를 바라보게 만드는 것.
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
    /// 좀비의 상태 전환
    /// </summary>
    /// <param name="zombieState">전환할 상태</param>
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
                // 만약 가장 가까운 좀비였다면
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

    //스나이퍼 페인트 탄 있을 시 좀비 슬로우 걸어줄 함수, 수치는 안알려줘서 미정
    public void SetZomBieSlow(float figure)
    {
        Speed -= figure;
    }
}
